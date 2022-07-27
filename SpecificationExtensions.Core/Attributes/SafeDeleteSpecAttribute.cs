using System;

namespace SpecificationExtensions.Core.Attributes
{
    public class SafeDeleteSpecAttribute : Attribute
    {
        public SafeDeleteSpecAttribute(Type type, string propertyName)
        {
            EntityType = type;
            PropertyName = propertyName;
        }
        
        public Type EntityType { get; set; }
        public string PropertyName { get; set; }
    }
}