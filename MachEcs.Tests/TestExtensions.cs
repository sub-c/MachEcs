using System.Reflection;

namespace MachEcs.Tests
{
    internal static class TestExtensions
    {
        public static T GetPrivateField<T>(this object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)field.GetValue(instance);
        }
    }
}
