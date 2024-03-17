using System.ComponentModel;
using System.Reflection;

namespace Order.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            return
                value == null ? "" :
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }

        public static string GetDisplayName(this Enum value)
            => value?
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DisplayNameAttribute>()
                    ?.DisplayName
                    ?? string.Empty;

        public static TEnum ToEnum<TEnum>(this string @enum)
            => (TEnum)Enum.Parse(typeof(TEnum), @enum);
    }
}
