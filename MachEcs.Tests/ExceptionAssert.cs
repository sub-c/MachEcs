using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MachEcs.Tests
{
    public static class ExceptionAssert
    {
        public static void Throws<T>(Action task)
            where T : Exception
        {
            try
            {
                task();
            }
            catch (Exception exception)
            {
                Assert.IsInstanceOfType(exception, typeof(T), $"Expected exception type {typeof(T).Name} but instead received exception {exception.GetType().Name}.");
                return;
            }
            Assert.Fail($"Expected exception type {typeof(T).Name}, but no exception was thrown.");
        }

        public static void Throws<T1, T2>(Action task)
            where T1 : Exception
            where T2 : Exception
        {
            try
            {
                task();
            }
            catch (Exception exception)
            {
                Assert.IsTrue(typeof(T1).IsAssignableFrom(exception.GetType()) || typeof(T2).IsAssignableFrom(exception.GetType()),
                    $"Expected exception of type {typeof(T1).Name} or {typeof(T2).Name}, but instead received exception {exception.GetType().Name}.");
                return;
            }
            Assert.Fail();
        }
    }
}
