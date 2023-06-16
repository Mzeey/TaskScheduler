using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.SharedLib.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            string enumValueName = Enum.GetName(enumType, value);
            MemberInfo memberInfo = enumType.GetField(enumValueName);

            if (memberInfo != null)
            {
                DescriptionAttribute attribute = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return enumValueName; // Return the enum value name if no description is found
        }
    }
}
