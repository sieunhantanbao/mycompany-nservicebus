using System;
using System.Linq;

namespace MyCompany.NServiceBus.Common
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAtribute,TValue> (
            this Type type, Func<TAtribute, TValue> valueSelector) where TAtribute: Attribute
        {
            var att = type.GetCustomAttributes(typeof(TAtribute), true).FirstOrDefault() as TAtribute;
            if(att!= null)
            {
                valueSelector(att);
            }
            return default(TValue);
        }
    }
}
