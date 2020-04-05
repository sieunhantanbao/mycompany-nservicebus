using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NServiceBus.Gateway.Helpers
{
    public static class ExpandoExtension
    {
        public static void To<T>(this ExpandoObject expandoObject, T obj)
        {
            Mapper.Map(expandoObject, obj);
        }
    }
}
