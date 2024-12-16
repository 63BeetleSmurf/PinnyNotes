using System.Linq;
using System.Reflection;

namespace PinnyNotes.WpfUi.Helpers;

public static class PropertiesHelper
{
    public static void CopyMatchingProperties(object source, object destination)
    {
        PropertyInfo[] sourceProperties = source.GetType().GetProperties();
        PropertyInfo[] destProperties = destination.GetType().GetProperties();

        foreach (PropertyInfo sourceProperty in sourceProperties)
        {
            PropertyInfo? destProperty = destProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
            if (destProperty != null && destProperty.CanWrite)
                destProperty.SetValue(destination, sourceProperty.GetValue(source));
        }
    }
}
