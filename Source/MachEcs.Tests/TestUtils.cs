using System;
using System.Reflection;

namespace MachEcs.Tests
{
    internal static class TestUtils
    {
        public static T GetPrivateField<T>(this object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic) ?? throw new NullReferenceException(); ;
            return (T?)field?.GetValue(instance) ?? throw new NullReferenceException($"{fieldName} is null.");
        }

        public static T GetPrivateProperty<T>(this object instance, string propertyName)
        {
            var property = instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (T?)property?.GetValue(instance) ?? throw new NullReferenceException($"{propertyName} is null.");
        }

        public static T GetStaticPrivateField<T>(Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            return (T?)field?.GetValue(null) ?? throw new NullReferenceException($"{fieldName} is null.");
        }
    }
}
