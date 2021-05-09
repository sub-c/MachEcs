using System.Reflection;

namespace MachEcs.Tests
{
    internal static class TestUtilities
    {
        public static T GetPrivateInstanceField<T>(object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T)field.GetValue(instance);
        }
    }
}
