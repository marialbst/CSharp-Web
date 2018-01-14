namespace SimpleMvc.Framework.Attributes.Property
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public abstract class PropertyAttribute : Attribute
    {
        public abstract bool IsValid(object value);
    }
}
