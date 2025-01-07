using System.Linq;
using System.Reflection;

namespace PinnyNotes.WpfUi.Helpers;

public static class PropertiesHelper
{
    public static void CopyMatchingProperties(object source, object destination)
    {
        PropertyInfo[] sourceProperties = source.GetType().GetProperties();
        PropertyInfo[] destinationProperties = destination.GetType().GetProperties();

        foreach (PropertyInfo sourceProperty in sourceProperties)
        {
            PropertyInfo? destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name && p.PropertyType == sourceProperty.PropertyType);
            if (destinationProperty != null && destinationProperty.CanWrite)
                destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
        }
    }
}
