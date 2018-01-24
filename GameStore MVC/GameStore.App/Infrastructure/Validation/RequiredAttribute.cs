namespace GameStore.App.Infrastructure.Validation
{
    using SimpleMvc.Framework.Attributes.Property;

    public class RequiredAttribute : PropertyAttribute
    {
        public override bool IsValid(object value)
        {
            return new System
                .ComponentModel
                .DataAnnotations
                .RequiredAttribute()
                .IsValid(value);
        }
    }
}
