using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Iris.Importer
{
    public enum OrganizationType
    {
        [Description("ΥΠΟΥΡΓΕΙΟ")]
        Ministry = 0,
        [Description("ΟΤΑ")]
        OTA = 7,
        [Description("Δημ.Τομ.")]
        PublicSector = 8,
        [Description("Ιδιωτ.Τομ.")]
        PrivateSector = 9,
        [Description("Άλλο")]
        Other = 10
    }

    public enum EmployeeCategory
    {
        [Description("ΠΕ")]
        PE,
        [Description("ΤΕ")]
        TE,
        [Description("ΔΕ")]
        DE,
        [Description("ΥΕ")]
        YE,
        [Description("Άλλο")]
        Other
    }

    public static class EnumerationExtensions
    {
        public static IEnumerable<Lookup> ToLookup(Type enumType)
        {
            MethodInfo mi = typeof(EnumerationExtensions).GetMethod("EnumToDescriptionLookup")
                                                         .MakeGenericMethod(enumType);

            return (IEnumerable<Lookup>)mi.Invoke(null, null);
        }

        public static IEnumerable<Lookup> EnumToLookup<T>()
        {
            return Enum.GetValues(typeof(T))
                       .OfType<object>()
                       .Select(
                           x => new Lookup
                           {
                               Id = ((int)x).ToString(),
                               Description = Enum.GetName(typeof(T), x)
                           });
        }

        public static IEnumerable<Lookup> EnumToDescriptionLookup<T>()
        {
            return Enum.GetValues(typeof(T))
                       .OfType<object>()
                       .Select(
                           x => new Lookup
                           {
                               Id = ((int)x).ToString(),
                               Description = GetDescription<T>(Enum.GetName(typeof(T), x))
                           });
        }

        public static T GetEnumFromLookup<T>(int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static string GetDescription<T>(string value)
        {
            FieldInfo field = typeof(T).GetField(value.ToString());
            DescriptionAttribute descAtt = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return descAtt == null || string.IsNullOrEmpty(descAtt.Description) ? value : descAtt.Description;
        }
    }
}
