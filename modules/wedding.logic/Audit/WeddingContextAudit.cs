//         .==.        .==.
//        //'^\\      //^'\\
//       // ^ ^\(\__/)/^ ^^\\
//      //^ ^^  /6  6\ ^ ^ ^\\
//     //^ ^^ ^/( .. )\ ^^  ^\\
//    //^ ^^^/\| v""v |/\^ ^ ^\\
//   // ^^  /    `~~`    \^ ^ ^\\
//  Here be dragons...
// --------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Reflection;
using System.Data;
using System.Diagnostics;
using System.Data.Objects;
using System.Configuration;
using System.Web;
using System.Data.Entity.Infrastructure;

namespace wedding.logic
{
    public partial class WeddingContext
    {
        private string AuditConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["AuditEntities"] != null)
                    return ConfigurationManager.ConnectionStrings["AuditEntities"].ConnectionString;
                else
                    return "";
            }
        }

        public bool HasChanges
        {
            get
            {
                return this.HasChanges;
            }
        }



        public void RollBackChanges()
        {


            this.RollBackChanges();
            //List<ObjectStateEntry> inserts = this.ObjectStateManager.GetObjectStateEntries(EntityState.Added).ToList();
            //List<ObjectStateEntry> updates = this.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).ToList();
            //List<ObjectStateEntry> deletes = this.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).ToList();
            ////reset inserts
            //foreach (ObjectStateEntry item in inserts)
            //    item.Delete();
            //this.Refresh(System.Data.Objects.RefreshMode.StoreWins, updates);
            //this.Refresh(System.Data.Objects.RefreshMode.StoreWins, deletes);
        }


        private string GetTableName(string TableNameSpace)
        {
            string result = "";
            result = TableNameSpace.Substring(TableNameSpace.LastIndexOf('.') + 1, TableNameSpace.Length - (TableNameSpace.LastIndexOf('.') + 1));
            return result;
        }

        private string GetIPAddress()
        {
            string ip;
            if (System.Web.HttpContext.Current != null)
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            else
            {
                ip = "localhost";
            }
            return ip;
        }

        private string GetRequestValue(string requestValue)
        {
            string agent = "";
            if (System.Web.HttpContext.Current != null)
            {
                agent = System.Web.HttpContext.Current.Request.ServerVariables[requestValue];
            }

            return agent;
        }

        public override int SaveChanges()
        {

            //var tableList = new AuditConfig.ConfigController(this.Connection.ConnectionString, true);
            //if (tableList.TableList.Count > 0) //check if there is something to audit in configuration
            //{
            //ChangeSet items = GetChangeSet();
            var tmpQry = this.ChangeTracker.Entries().ToList();
            List<ObjectStateEntry> updates = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified).ToList();
            foreach (var item in updates)
            {
                try
                {
                    string tablename = GetTableName(item.EntitySet.ElementType.ToString());
                    //var doWrite = tableList.TableList.FirstOrDefault(c => c.TableName.Contains(tablename) && c.Update.Equals(true));
                    //if (doWrite != null)
                    //{
                    WriteModified(item.Entity);
                    CreateAuditTable(tablename, item.Entity);
                    WriteAudit(item.Entity, "UPDATE");
                    // }
                }
                catch (Exception ex)
                {
                    Exception myex = new Exception("Error on auditing an update \n" + ex.Message, ex.InnerException);
                    //StratsysMailer.ErrorHandler.HandleAndMailError(myex);
                    break; //End audit attempt
                }
            }
            List<ObjectStateEntry> inserts = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added).ToList();
            foreach (var item in inserts)
            {
                try
                {
                    string tablename = GetTableName(item.EntitySet.ElementType.ToString());
                    //var doWrite = tableList.TableList.FirstOrDefault(c => c.TableName.Contains(item.ToString()) && c.Insert.Equals(true));
                    //if (doWrite != null)
                    //{
                    WriteCreated(item.Entity);
                    WriteModified(item.Entity);
                    CreateAuditTable(tablename, item.Entity);
                    WriteAudit(item.Entity, "INSERT");
                    //}
                }
                catch (Exception ex)
                {
                    Exception myex = new Exception("Error on auditing an insert \n" + ex.Message, ex.InnerException);
                    //StratsysMailer.ErrorHandler.HandleAndMailError(myex);
                    break; //End audit attempt
                }
            }
            List<ObjectStateEntry> deletes = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted).ToList();
            foreach (var item in deletes)
            {
                try
                {
                    string tablename = GetTableName(item.EntitySet.ElementType.ToString());
                    //var doWrite = tableList.TableList.FirstOrDefault(c => c.TableName.Contains(item.ToString()) && c.Delete.Equals(true));
                    //if (doWrite != null)
                    //{
                    CreateAuditTable(tablename, item.Entity);
                    WriteAudit(item.Entity, "DELETE");
                    //}
                }
                catch (Exception ex)
                {
                    Exception myex = new Exception("Error on auditing a delete \n" + ex.Message, ex.InnerException);
                    //StratsysMailer.ErrorHandler.HandleAndMailError(myex);
                    break; //End audit attempt
                }
            }
            // }

            return base.SaveChanges();
        }

        public void CreateAuditTable(string TableName, object MyObject)
        {
            string _tableName = string.Format("Audit{0}", TableName);
            string _cmdText = string.Format("IF EXISTS(Select 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='{0}') SELECT 1 ELSE SELECT 0", _tableName);


            SqlCommand cmd = new SqlCommand(_cmdText, new SqlConnection(AuditConnectionString));
            cmd.Connection.Open();
            int result = (int)cmd.ExecuteScalar();
            cmd.Connection.Close();
            if (result == 1) //TableExists
            {
                //Now Check Schema
                DataSet _dsSchema = new DataSet();
                cmd.CommandText = string.Format("EXEC sp_help AUDIT{0}", TableName);
                SqlDataAdapter _comData = new SqlDataAdapter(cmd);
                cmd.Connection.Open();
                _comData.Fill(_dsSchema);
                cmd.Connection.Close();
                Dictionary<string, string> MissingColumns = new Dictionary<string, string>();
                //Check If columns are created in db table
                if (_dsSchema.Tables.Count > 2)
                {
                    foreach (PropertyInfo info in MyObject.GetType().GetProperties())
                    {
                        if (!ContainsProperty(info, _dsSchema.Tables[1]))
                            MissingColumns.Add(info.Name, GetSQLType(info.PropertyType.Name));
                    }
                }
                cmd.Connection.Open();
                foreach (var item in MissingColumns)
                {
                    cmd.CommandText = string.Format("ALTER TABLE Audit{0} ADD {1} {2}", TableName, item.Key, item.Value);
                    cmd.ExecuteNonQuery();
                }
                cmd.Connection.Close();
            }
            else //Create Table
            {
                StringBuilder _cmdCreate = new StringBuilder();
                _cmdCreate.Append(string.Format("CREATE TABLE [dbo].[Audit{0}](", TableName));
                _cmdCreate.Append(string.Format("[Audit{0}Id] [int] IDENTITY(1,1),", TableName));
                _cmdCreate.Append(string.Format("[AuditUserId] [varchar](20) NULL,", TableName));
                _cmdCreate.Append(string.Format("[AuditDate] [datetime] NULL,", TableName));
                _cmdCreate.Append(string.Format("[AuditRemoteIPAddress] [varchar](200) NULL,", TableName));
                _cmdCreate.Append(string.Format("[AuditRemoteUserAgent] [varchar](MAX) NULL,", TableName));
                _cmdCreate.Append(string.Format("[AuditRemoteContentType] [varchar](MAX) NULL,", TableName));
                _cmdCreate.Append(string.Format("[AuditAction] [varchar](20) NULL,", TableName));
                //Now add objects columns
                foreach (PropertyInfo item in MyObject.GetType().GetProperties())
                {
                    _cmdCreate.Append(string.Format("[{0}] {1},", item.Name, GetSQLType(item.PropertyType.Name)));
                }
                _cmdCreate.Append(string.Format(" CONSTRAINT [PK_Audit{0}] PRIMARY KEY CLUSTERED ", TableName));
                _cmdCreate.Append("(");
                _cmdCreate.Append(string.Format("[Audit{0}Id] ASC", TableName));
                _cmdCreate.Append(")WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]");
                _cmdCreate.Append(") ON [PRIMARY]");
                cmd.CommandText = _cmdCreate.ToString();
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }

        public void WriteModified(object MyObject)
        {

            if (ContainsProperty("DateModified", MyObject))
                MyObject.GetType().GetProperty("DateModified").SetValue(MyObject, DateTime.Now, null);
        }

        public void WriteCreated(object MyObject)
        {
            if (ContainsProperty("DateCreated", MyObject))
                MyObject.GetType().GetProperty("DateCreated").SetValue(MyObject, DateTime.Now, null);
        }

        public string WriteAudit(object MyObject, string Action)
        {
            string TableName = "Audit" + MyObject.GetType().Name;
            List<string> Columns = new List<string>();
            List<object> Values = new List<object>();
            //write default audit details
            Columns.Add("AuditUserId");
            Columns.Add("AuditDate");
            Columns.Add("AuditRemoteIPAddress");
            Columns.Add("AuditRemoteUserAgent");
            Columns.Add("AuditRemoteContentType");
            Columns.Add("AuditAction");
            Values.Add(System.Environment.UserName);
            Values.Add(DateTime.Now.ToString(@"yyyy'/'MM'/'dd HH':'mm':'ss"));
            Values.Add(GetIPAddress());
            Values.Add(GetRequestValue("HTTP_USER_AGENT"));
            Values.Add(GetRequestValue("HTTP_CONTENT_TYPE"));
            Values.Add(Action);
            foreach (PropertyInfo item in MyObject.GetType().GetProperties())
            {
                Columns.Add(item.Name);
                Values.Add(item.GetValue(MyObject, null));
            }
            StringBuilder _cmdText = new StringBuilder();
            _cmdText.Append(string.Format("INSERT INTO {0} (", TableName));
            int counter = 0;
            foreach (var item in Columns)
            {
                counter++;
                if (counter < Columns.Count)
                    _cmdText.Append("[" + item + "],"); //Add line with comma
                else
                    _cmdText.Append("[" + item + "]"); //Add last line
            }
            _cmdText.Append(") VALUES (");
            counter = 0;
            foreach (var item in Values)
            {
                if (item != null)
                {
                    var type = item.GetType();
                    //check if we need to add brackets to query
                    if (type.Name == "Int16" || type.Name == "Int32" || type.Name == "Decimal")
                        _cmdText.Append(item + ",");
                    else if (type.Name == "Boolean")
                    {
                        if ((bool)item)
                            _cmdText.Append(1 + ",");
                        else
                            _cmdText.Append(0 + ",");
                    }
                    else
                        if (type.Name == "DateTime")
                        {
                            string date = item.ToString();
                            var sqlmindate = new DateTime(1900, 1, 1);
                            if ((DateTime)item < sqlmindate)
                                date = sqlmindate.ToString();
                            _cmdText.Append("'" + Convert.ToDateTime(date).ToString(@"yyyy'/'MM'/'dd HH':'mm':'ss") + "',");
                        }
                        else
                        {
                            string log = ""; //truncate string greater than 255 characters
                            if (item.ToString().Length > 255)
                                log = item.ToString().Substring(0, 255);
                            else
                                log = item.ToString();
                            _cmdText.Append("'" + log.Replace("'", "") + "',"); //prevent SQL injection here
                        }
                }
                else
                {
                    _cmdText.Append("null,");
                }
            }
            string _cmd = _cmdText.ToString().Substring(0, _cmdText.Length - 1);
            _cmd = string.Format("{0})", _cmd);
            SqlCommand cmd = new SqlCommand(_cmd, new SqlConnection(AuditConnectionString));
            cmd.Connection.Open();
            //bazinga... now we audit our anonomous record
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            return _cmd;
        }

        public string GetSQLType(string Type)
        {
            switch (Type)
            {
                case "Guid": { return "[uniqueidentifier] NULL"; }
                case "Int16": { return "[smallint] NULL"; }
                case "Int32": { return "[int] NULL"; }
                case "Boolean": { return "[bit] NULL"; }
                case "Decimal": { return "[decimal](24, 8) NULL"; }
                case "DateTime": { return "[datetime] NULL"; }
                case "String": { return "[varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL"; }
                default: return "[varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL";
            }
        }

        public bool ContainsProperty(PropertyInfo Property, DataTable MyTable)
        {
            bool result = false;
            foreach (DataRow item in MyTable.Rows)
            {
                if (item[0].ToString() == Property.Name)
                    result = true;
            }
            return result;
        }

        public bool ContainsProperty(String PropertyName, object MyObject)
        {
            foreach (PropertyInfo info in MyObject.GetType().GetProperties())
                if (info.Name == PropertyName)
                    return true;
            return false;
        }


    }
}
