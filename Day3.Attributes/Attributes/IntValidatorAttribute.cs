using System;

namespace Attributes
{
    // Should be applied to properties and fields.
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IntValidatorAttribute : Attribute
    {
        public int Lower { get; }
        public int Upper { get; }

        public IntValidatorAttribute(int lower, int upper)
        {
            this.Lower = lower;
            this.Upper = upper;
        }
    }
}
