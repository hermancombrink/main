using log4net.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using wedding.logic;

namespace ginger.aalwyn.co.za.Controllers
{
    [Authorize]
    public class FileUploadController : ApiController
    {
        string _domain = "";
        IWeddingLogic _context;
        ILogger _logger;

        public string TAG
        {
            get
            {
                return "FileUploadController";
            }
        }

        public FileUploadController(IWeddingLogic context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        private void Init()
        {
            _domain = this.Request.RequestUri.DnsSafeHost;
            if (this.Request.RequestUri.Port != 80)
                _domain = string.Format("{0}:{1}", this.Request.RequestUri.DnsSafeHost, this.Request.RequestUri.Port.ToString());
            _domain = _domain.Replace("www.", "");
            _domain = _domain.Replace("www", "");
        }

        public WeddingSetting Upload()
        {
            Init();
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count == 1)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    WeddingSetting dbSetting = new WeddingSetting();
                    byte[] contentBody = new byte[postedFile.ContentLength];
                    postedFile.InputStream.Read(contentBody, 0, postedFile.ContentLength);
                    dbSetting.InviteConentBody = contentBody;
                    dbSetting.InviteContentType = postedFile.ContentType;
                    dbSetting.InviteContentLength = postedFile.ContentLength;
                    return _context.UpdateInvitePhoto(dbSetting, _domain);
                }
            }
            throw _logger.GetRaiseException("Unable to save file. 0 or more than 1 file has been submitted", TAG);
        }

      
    }
}
