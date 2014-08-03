using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace obj.mapper
{
    public class ObjectMap
    {

        public static List<Type> MapTypes = new List<Type>() { 
        typeof(Decimal),
        typeof(Guid),
        typeof(Int32),
        typeof(String),
        typeof(Byte[]),
        typeof(DateTime), 
        typeof(Boolean)
        };

        public static object BindObject(Object Source, Object Destination)
        {
            var _sourceProperties = Source.GetType().GetProperties();
            foreach (var item in _sourceProperties)
            {
                var _sourceValue = item.GetValue(Source, null);
                var _destItem = Destination.GetType().GetProperty(item.Name);
                if (_destItem == null)
                    continue;
                var _destinationValue = _destItem.GetValue(Destination, null);
                dynamic _sourceType;
                if (_sourceValue != null)
                    _sourceType = _sourceValue.GetType();
                else if (_destinationValue != null) //source has been set to null, use destination to get value rather
                    _sourceType = _destinationValue.GetType();
                else
                    continue; //no values needs to be assigned, both null.. just continue;
                //is simple type property
                if (MapTypes.Contains(_sourceType))
                    if (Destination.GetType().GetProperty(item.Name) != null) //got property now assign
                    {
                        //   if (_sourceValue != null) //only assign non null values, so that we can leave out items that we don't need without accidentally assigning them
                        _destItem.SetValue(Destination, _sourceValue, null);
                    }
                    else
                    {
                        if (_sourceValue.GetType().BaseType == typeof(EntityObject)) //found complex entity go recursive baby!
                        {
                            if (_destinationValue != null)
                            {
                                _destItem.SetValue(Destination, BindObject(_sourceValue, _destinationValue), null);
                            }
                            else //find clever way to add to entity collection
                            {
                                _destinationValue = Activator.CreateInstance(_sourceType.GetType());
                                _destItem.SetValue(Destination, BindObject(_sourceValue, _destinationValue), null);
                            }
                        }
                    }
            }
            return Destination;
        }
    }
}
