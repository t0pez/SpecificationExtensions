using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SpecificationExtensions.Generators
{
    [Generator]
    public class SafeDeleteSpecGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;

            foreach (var syntaxTree in syntaxTrees)
            {
                var safeDeleteSpecDeclarationSyntaxes = syntaxTree.GetRoot().DescendantNodes()
                                                                  .OfType<ClassDeclarationSyntax>()
                                                                  .Where(syntax => syntax.AttributeLists.Any(
                                                                             attribute =>
                                                                                 attribute.ToString()
                                                                                     .StartsWith(
                                                                                         "[SafeDeleteSpec(")))
                                                                  .ToList();

                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                foreach (var safeDeleteSpecDeclarationSyntax in safeDeleteSpecDeclarationSyntaxes)
                {
                    var usingDirectives = GetUsingDirectives(syntaxTree);
                    var usingDirectivesAsString = string.Join("\n", usingDirectives);

                    var genericTypeIdentifier = GetGenericTypeIdentifier(safeDeleteSpecDeclarationSyntax);
                    var genericTypeIdentifierAsString = genericTypeIdentifier?.Text;

                    var namespaceDirective = GetNamespaceDirective(semanticModel, safeDeleteSpecDeclarationSyntax);
                    var namespaceAsString = "namespace " + namespaceDirective;

                    var className = safeDeleteSpecDeclarationSyntax.Identifier.ToString();

                    var attributeArgumentList = safeDeleteSpecDeclarationSyntax.AttributeLists
                                                                               .Single(syntax => syntax.ToString()
                                                                                   .StartsWith(
                                                                                       "[SafeDeleteSpec("))
                                                                               .Attributes.Single().ArgumentList;

                    var safeDeleteTypeArgExpressionSyntax = GetTypeOfExpression(attributeArgumentList);
                    var safeDeleteTypeAsString = safeDeleteTypeArgExpressionSyntax?.Type.ToString();

                    var safeDeletePropArgExpressionSyntax = GetNameOfExpression(attributeArgumentList);
                    var safeDeletePropString = GetNameOfExpressionArgument(safeDeletePropArgExpressionSyntax);
                    var safeDeletePropAsString = safeDeletePropString?.Split('.').Last();

                    var code = SpecCode.SafeDeleteSpecCode;
                    var generatedCode = code.Replace(SpecCode.CustomUsingDirectives, usingDirectivesAsString)
                                            .Replace(SpecCode.Namespace, namespaceAsString)
                                            .Replace(SpecCode.ClassName, className)
                                            .Replace(SpecCode.GenericTDefinitionName, genericTypeIdentifierAsString)
                                            .Replace(SpecCode.SafeDeleteTypeName, safeDeleteTypeAsString)
                                            .Replace(SpecCode.SafeDeletePropName, safeDeletePropAsString);

                    context.AddSource($"{className}.Spec.cs", SourceText.From(generatedCode, Encoding.UTF8));
                }
            }
        }

        private static IEnumerable<UsingDirectiveSyntax> GetUsingDirectives(SyntaxTree syntaxTree)
        {
            var usingDirectives = syntaxTree.GetRoot().DescendantNodesAndSelf().OfType<UsingDirectiveSyntax>();
            return usingDirectives;
        }

        private static string GetNamespaceDirective(SemanticModel semanticModel,
                                                    ClassDeclarationSyntax safeDeleteSpecDeclarationSyntax)
        {
            var typeSymbol = semanticModel.GetDeclaredSymbol(safeDeleteSpecDeclarationSyntax);
            var namespaceDirective = typeSymbol?.ContainingNamespace.ToString();
            return namespaceDirective;
        }

        private static SyntaxToken? GetGenericTypeIdentifier(ClassDeclarationSyntax safeDeleteSpecDeclarationSyntax)
        {
            var genericTypeIdentifier =
                safeDeleteSpecDeclarationSyntax.TypeParameterList?.Parameters.First().Identifier;
            return genericTypeIdentifier;
        }

        private static TypeOfExpressionSyntax GetTypeOfExpression(AttributeArgumentListSyntax attributeArgumentList)
        {
            var safeDeleteTypeArgExpressionSyntax =
                attributeArgumentList?.Arguments.First().Expression as TypeOfExpressionSyntax;
            return safeDeleteTypeArgExpressionSyntax;
        }

        private static InvocationExpressionSyntax GetNameOfExpression(
            AttributeArgumentListSyntax attributeArgumentList)
        {
            var safeDeletePropArgExpressionSyntax =
                attributeArgumentList?.Arguments.Last().Expression as InvocationExpressionSyntax;
            return safeDeletePropArgExpressionSyntax;
        }

        private static string GetNameOfExpressionArgument(InvocationExpressionSyntax safeDeletePropArgExpressionSyntax)
        {
            return safeDeletePropArgExpressionSyntax?.ArgumentList.Arguments.First().ToString();
        }
    }
}