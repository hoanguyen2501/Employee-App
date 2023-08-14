using System.Reflection;

namespace EmployeeApp.Utils.Validation
{
    public static class Validation
    {
        public static void TrimStringProperies(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(string))
                {
                    string currentValue = (string)property.GetValue(obj);
                    if (currentValue != null)
                    {
                        string trimmedValue = currentValue.Trim();
                        property.SetValue(obj, trimmedValue);
                    }
                }
            }
        }
    }
}
