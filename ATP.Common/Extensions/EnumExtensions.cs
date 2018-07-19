using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ATP.Common.Entities;

namespace ATP.Common.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<SelectItem> ToSelectList(this Enum self, string valueSelected = "")
        {
            var values = Enum.GetValues(self.GetType()).Cast<byte>()
                .Where(e => GetEnumListBindable(Enum.Parse(self.GetType(), e.ToString())))
                .Select(e => new SelectItem
                {
                    Value = e.ToString(),
                    Text = GetEnumDescription(Enum.Parse(self.GetType(), e.ToString())),
                    Selected = !string.IsNullOrEmpty(valueSelected) ? e.ToString() == valueSelected : false
                });

            return values;
        }

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = new DescriptionAttribute[] { };

            if (fi != null)
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static bool GetEnumListBindable<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = new ListBindableAttribute[] { };

            if (fi != null)
                attributes = (ListBindableAttribute[])fi.GetCustomAttributes(typeof(ListBindableAttribute), false);

            //return true by default
            return (attributes.Length > 0) ? attributes[0].ListBindable : true;
        }

        public static string GetEnumDescription(this Enum value)
        {
            if (value == null)
                return "";

            var fi = value.GetType().GetField(value.ToString());

            var attributes = new DescriptionAttribute[] { };

            if (fi != null)
                attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }


        public static IEnumerable<KeyValueInfo> ToKeyValueInfo(this Enum self)
        {
            var values = Enum.GetValues(self.GetType()).Cast<byte>()
                .Select(e => new KeyValueInfo(e, GetEnumDescription(Enum.Parse(self.GetType(), e.ToString()))));

            return values;
        }
    }
}
