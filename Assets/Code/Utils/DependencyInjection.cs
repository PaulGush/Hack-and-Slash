using System;

namespace DependencyInjection
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : Attribute
    {
        public ProvideAttribute() { }
    }

    public interface IDependencyProvider { }
}