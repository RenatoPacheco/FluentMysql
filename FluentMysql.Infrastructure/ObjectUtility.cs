using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluentMysql.Infrastructure
{
    public static class ObjectUtility
    {
        private struct PropertyData
        {
            public string Description { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public string Property { get; set; }
            public bool List { get; set; }

        }

        private static PropertyData GetPropertyData(PropertyInfo property, object value)
        {
            PropertyData result = new PropertyData(); 
            object[] displayAttr;
            object[] descriptionAttr;
            Type type = value.GetType();
            PropertyInfo[] properties = type.GetProperties();

            result.Value = string.Empty;

            if (property.PropertyType == typeof(string) || !(typeof(IEnumerable).IsAssignableFrom(property.PropertyType)))
                result.Value = string.Format("{0}", property.GetValue(value, null));
            else
                result.List = true;

            if (result.Value.Equals(string.Format("{0}", property.PropertyType)))
                result.Value = string.Empty;

            result.Name = property.Name;
            result.Property = property.Name;
            result.Description = string.Empty;

            displayAttr = property.GetCustomAttributes(typeof(DisplayAttribute), false);
            descriptionAttr = property.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (displayAttr.Count() > 0)
                result.Name = ((DisplayAttribute)displayAttr[0]).Name;

            if (descriptionAttr.Count() > 0)
                result.Description = ((DescriptionAttribute)descriptionAttr[0]).Description;

            return result;
        }

        public static XElement PropertyToXElement(object value)
        {
            return PropertyToXElement(value, value.GetType().Name);
        }

        public static XElement PropertyToXElement(object value, string name)
        {
            return PropertyToXElement(value, new XElement(name));
        }

        public static XElement PropertyToXElement(object value, XElement element)
        {
            XElement item;
            PropertyData data;
            Type type = value.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                data = GetPropertyData(property, value);

                if (Regex.IsMatch(data.Value, "<|>|\""))
                {
                    item = new XElement(data.Property, new XCData(data.Value));
                }
                else
                {
                    item = new XElement(data.Property, data.Value);
                }
                item.Add(new XAttribute("Name", data.Name));
                item.Add(new XAttribute("Description", data.Description));
                element.Add(item);                
            }
            return element;
        }

        public static IDictionary<string, string> PropertyToDictionary(object value)
        {
            IDictionary<string, string> result = new Dictionary<string,string>();
            PropertyData data;
            Type type = value.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                data = GetPropertyData(property, value);
                result.Add(data.Property, data.Value);
            }
            return result;
        }


    }
}
