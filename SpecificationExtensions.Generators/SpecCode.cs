namespace SpecificationExtensions.Generators
{
    public class SpecCode
    {
        public const string CustomUsingDirectives = "UsingsToFill";
        public const string Namespace = "NamespaceToReplace";
        public const string ClassName = "ClassNameToReplace";
        public const string SafeDeleteTypeName = "SafeDeleteTypeToReplace";
        public const string SafeDeletePropName = "SafeDeletePropToReplace";
        public const string GenericTDefinitionName = "GenericTDefinitionToReplace";

        public static string SafeDeleteSpecCode => 
            $@"// Auto-generated
using Ardalis.Specification;
using SpecificationExtensions.Core.Specifications;
{CustomUsingDirectives}

{Namespace}
{{
    public partial class {ClassName}<{GenericTDefinitionName}> : BaseSpec<{GenericTDefinitionName}> where {GenericTDefinitionName} : {SafeDeleteTypeName}
    {{
        public {ClassName}()
        {{
            Query
                .Where(model => model.{SafeDeletePropName} == IsIncludedDeleted ||
                                model.{SafeDeletePropName} == IsExcludedNotDeleted);
        }}

        public bool IsIncludedDeleted {{ get; private set; }}

        public bool IsExcludedNotDeleted {{ get; private set; }}
        
        public {ClassName}<{GenericTDefinitionName}> LoadAll()
        {{
            IsIncludedDeleted = true;
            IsExcludedNotDeleted = false;
        
            return this;
        }}
            
        public {ClassName}<{GenericTDefinitionName}> LoadOnlyDeleted()
        {{
            IsIncludedDeleted = true;
            IsExcludedNotDeleted = true;
        
            return this;
        }}
            
        public {ClassName}<{GenericTDefinitionName}> LoadOnlyNotDeleted()
        {{
            IsIncludedDeleted = false;
            IsExcludedNotDeleted = false;
        
            return this;
        }}
    }}
}}";

    }
}