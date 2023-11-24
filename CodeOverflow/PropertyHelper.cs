using System;

namespace CodeOverflow
{
    public static class PropertyHelper
    {
        // https://stackoverflow.com/a/1954663/1865718
        /// <summary>Get the values from (nested classes) properties.</summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetPropValue(this object obj, string name)
        {
            foreach (var part in name.Split('.'))
            {
                if (obj == null)
                {
                    return null;
                }

                var type = obj.GetType();
                var info = type.GetProperty(part);
                if (info == null)
                {
                    return null;
                }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }

        public static T GetPropValue<T>(this object obj, string name)
        {
            var retval = GetPropValue(obj, name);
            if (retval == null)
            {
                return default;
            }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        // https://stackoverflow.com/a/32184652/1865718
        /// <summary>
        /// Set object property using reflection
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if(property != null && property.CanWrite)
            {
                var t = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var safeValue = value == null ? null : Convert.ChangeType(value, t);

                property.SetValue(obj, safeValue, null);
            }
        }
    }
}