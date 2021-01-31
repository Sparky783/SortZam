using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Tools.Utils
{
    public static class EnumUtils
    {
        /// <summary>
        /// Determine if object is Enum type
        /// </summary>
        /// <param name="model">Object model</param>
        /// <returns></returns>
        public static bool IsEnum(this object model)
        {
            return model.GetType().IsEnum();
        }

        /// <summary>
        /// Determine if object is Enum type
        /// </summary>
        /// <param name="type">Object type</param>
        /// <returns></returns>
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        /// Get the display name value attribute of the Enum value
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayName(this object enumValue)
        {
            if (enumValue == null || !enumValue.IsEnum())
                return null;
            return enumValue?.GetType().GetMember(enumValue.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>()?.Name ?? enumValue.ToString();
        }

        /// <summary>
        /// Get All Values for enum parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<T> GetValues<T>(this Type enumType)
        {
            return enumType.GetEnumValues().OfType<T>().ToList();
        }
    }
}
