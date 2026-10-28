using System;
using System.Collections.Generic;
using System.Reflection;

namespace Munshiganj.PortableTests
{
    /// <summary>
    /// Runs the [Test] methods of the EditMode test assembly outside of the Unity Editor.
    /// The Unity Test Runner is the primary runner; this one makes the same assertions
    /// executable in CI environments that do not have the Unity Editor installed.
    /// </summary>
    public static class PortableTestRunner
    {
        public static int Main(string[] args)
        {
            int passed = 0;
            List<string> failures = new List<string>();

            Assembly assembly = typeof(PortableTestRunner).Assembly;
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                MethodInfo[] methods = type.GetMethods(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                    BindingFlags.Static | BindingFlags.DeclaredOnly);

                foreach (MethodInfo method in methods)
                {
                    if (!IsTest(method))
                    {
                        continue;
                    }

                    object instance = method.IsStatic ? null : Activator.CreateInstance(type);

                    try
                    {
                        method.Invoke(instance, null);
                        passed++;
                        Console.WriteLine("PASS  " + type.Name + "." + method.Name);
                    }
                    catch (Exception error)
                    {
                        Exception inner = error is TargetInvocationException && error.InnerException != null
                            ? error.InnerException
                            : error;

                        failures.Add(type.Name + "." + method.Name + ": " + inner.Message);
                        Console.WriteLine("FAIL  " + type.Name + "." + method.Name + ": " + inner.Message);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine(
                "Tests: " + (passed + failures.Count) + "  Passed: " + passed + "  Failed: " + failures.Count);

            if (failures.Count > 0)
            {
                Console.WriteLine();
                foreach (string failure in failures)
                {
                    Console.WriteLine("  " + failure);
                }

                return 1;
            }

            return 0;
        }

        static bool IsTest(MethodInfo method)
        {
            foreach (object attribute in method.GetCustomAttributes(false))
            {
                if (attribute.GetType().Name == "TestAttribute")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
