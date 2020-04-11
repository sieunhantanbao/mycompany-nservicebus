using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.Helpers
{
    // By using a generic class we can take advantage
    // of the fact that .NET will create a new generic type
    // for each type T. This allows us to avoid creating
    // a dictionary of Dictionary<string, PropertyInfo>
    // for each type T. We also avoid the need for the 
    // lock statement with every call to Map.
    public static class ExpandoMapper<T> where T : class
    {
        private static readonly Dictionary<string, PropertyInfo> _propertyMap;

        static ExpandoMapper()
        {
            // Convert to lower case to avoid creating a new string than one
            _propertyMap =
                typeof(T).GetProperties().ToDictionary(p => p.Name.ToLower(), p => p);
        }

        public static void Map(ExpandoObject source, T dest)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (dest == null)
            {
                throw new ArgumentNullException("destination");
            }

            foreach (var kv in source)
            {
                PropertyInfo p;
                if (_propertyMap.TryGetValue(kv.Key.ToLower(), out p))
                {
                    var propertyType = p.PropertyType;
                    if (kv.Value == null)
                    {
                        if (!propertyType.IsByRef && propertyType.Name != "Nullable`1")
                        {
                            // Throw if type is a value type 
                            // but not Nullable<>
                            throw new ArgumentException("not nullable");
                        }
                    }
                    else if (kv.Value.GetType() != propertyType)
                    {
                        throw new ArgumentException("type mismatch");
                    }
                    p.SetValue(dest, kv.Value, null);
                }
            }
        }
    }

    public static class Mapper
    {
        public static void Map<T>(ExpandoObject source, T dest)
        {
            IDictionary<string, object> dict = source;
            var type = dest.GetType();

            foreach (var prop in type.GetProperties())
            {
                var lower = prop.Name.ToLower();
                var key = dict.Keys.FirstOrDefault(k => k.ToLower() == lower);
                if (key != null)
                {
                    JsonElement objectValue = (JsonElement)dict[key];
                    var objectValueKind = objectValue.ValueKind;
                    var propType = prop.PropertyType;
                    if((propType == typeof(List<string>) || propType == typeof(string[]))
                        && objectValueKind == JsonValueKind.Array)
                    {
                        var rawText = objectValue.GetRawText(); // the value look like ["value1","value2"]
                        // Remove the [ and ]
                        var valueString = rawText?.Substring(rawText.IndexOf('[') + 1, rawText.Length - 2);
                        var valueStringToArray = valueString.Split(",");
                        var testArray = valueString.Split(",");
                        prop.SetValue(dest, valueStringToArray, null);
                    }
                    else
                    {
                        prop.SetValue(dest, dict[key], null);
                    }
                }
            }
        }
    }
}
