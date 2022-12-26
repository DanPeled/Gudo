using System;

namespace SmartConsole
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CommandAttribute : Attribute { }
}