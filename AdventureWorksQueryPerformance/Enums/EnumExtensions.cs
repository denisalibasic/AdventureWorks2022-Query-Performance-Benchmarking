using System.Reflection;

namespace AdventureWorksQueryPerformance.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            EnumDescription[] attributes = (EnumDescription[])field.GetCustomAttributes(typeof(EnumDescription), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

    }
}
