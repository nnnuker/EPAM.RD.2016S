using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class StringValidatorAttribute : Attribute
    {
        public int MaxLength { get; }

        public StringValidatorAttribute(int maxLength)
        {
            this.MaxLength = maxLength;
        }
    }
}
