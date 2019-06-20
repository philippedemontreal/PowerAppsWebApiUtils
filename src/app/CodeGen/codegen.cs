using System.Collections.Generic;
using app.Metadata;
using System.CodeDom;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Text;
using System.IO;
using Newtonsoft.Json.Serialization;
using System.Dynamic;
using app.entities;

namespace app.codegen
{
    public class CodeGen
    {
        private readonly Dictionary<AttributeTypeCode, Type> _refMap;

        public CodeGen()
        {
            _refMap = new Dictionary<AttributeTypeCode, Type>( 
                new []
                {
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Boolean, typeof(bool?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.DateTime, typeof(DateTime?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Memo, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.EntityName, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.String, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.BigInt, typeof(Int64?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Double, typeof(double?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Decimal, typeof(decimal?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Integer, typeof(int?)),
                    //new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Lookup, typeof(string)),
                    //new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Customer, typeof(string)),
                    //new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Owner, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Money, typeof(decimal?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Uniqueidentifier, typeof(Guid?)),
                }); 

        }
        private CodeNamespace CreateNameSpace (string nameSpaceName)
        {
            var returnValue = new CodeNamespace(nameSpaceName);
            returnValue.Imports.Add(new CodeNamespaceImport ("System"));
            returnValue.Imports.Add(new CodeNamespaceImport ("System.Runtime.Serialization"));
            return returnValue;
        }

        public CodeTypeDeclaration CreateType (string name)
        {
            var result =  new CodeTypeDeclaration(name)
            {
                IsClass = true,
                IsPartial = true,
            };
            result.BaseTypes.Add(new CodeTypeReference(typeof(ExtendedEntity)));

            return result;
        }


        public CodeMemberProperty CreateProperty(AttributeMetadata attributeMetadata, Type propertyType) 
        {
            var result = new CodeMemberProperty()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = attributeMetadata.SchemaName,
                Type = new CodeTypeReference(propertyType),
                CustomAttributes = 
                {
                    new CodeAttributeDeclaration("DataMember", new CodeAttributeArgument("Name", new CodePrimitiveExpression(attributeMetadata.LogicalName)))
                }      
            };

            result.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                        new CodeBaseReferenceExpression(),
                            "GetAttributeValue",
                            new CodeTypeReference[] { new CodeTypeReference(propertyType) }),
                            new CodePrimitiveExpression(attributeMetadata.LogicalName))                            
            ));

            if (attributeMetadata.IsValidForUpdate)
            {
                result.SetStatements.Add(
                        new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(
                            new CodeBaseReferenceExpression(),
                                "SetAttributeValue",
                                new CodeTypeReference[] { new CodeTypeReference(propertyType) }),
                                new CodePrimitiveExpression(attributeMetadata.LogicalName),
                                new CodeVariableReferenceExpression("value")
                                ) 
                );
            }


            return result;
        }      



        public void Execute(List<EntityMetadata> entitMetadatas)
        {

            var provider = new Microsoft.CSharp.CSharpCodeProvider();                             
            var codeGeneratorOptions = new CodeGeneratorOptions();
            var ns = CreateNameSpace("webapi.entities");

            foreach (var entitMetadata in entitMetadatas.OrderBy(p => p.SchemaName))
            {
                MakeEntity(entitMetadata, ns);
            }

            var cu = new CodeCompileUnit();
            cu.Namespaces.Add(ns);
        
            codeGeneratorOptions.BlankLinesBetweenMembers = false;
            codeGeneratorOptions.BracingStyle = "C";
            codeGeneratorOptions.IndentString = "   "; 
            

            var code = new StringBuilder();
            var stringWriter = new StringWriter(code);
            provider.GenerateCodeFromCompileUnit(cu, stringWriter, codeGeneratorOptions);

            var result = code.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Philippe\Documents\Projects\Ts\DynCEWebApiEarlyBound\src\tests\account.cs", result);

        }

        private void MakeEntity(EntityMetadata entitMetadata, CodeNamespace ns)
        {
            var genType = CreateType(entitMetadata.SchemaName);
            var primary = entitMetadata.Attributes.Where(p => p.IsPrimaryId).FirstOrDefault();
            var prop = CreateProperty(primary, typeof(Guid));
            genType.Members.Add(prop);


            foreach (var attribute in entitMetadata.Attributes.OrderBy(p => p.SchemaName.ToLowerInvariant()))
            {            
                if (!attribute.IsValidForRead || attribute.IsPrimaryId)       
                    continue;

                if (!_refMap.ContainsKey(attribute.AttributeType))
                    continue;

                var genProp = CreateProperty(attribute, _refMap[attribute.AttributeType]);
                genType.Members.Add(genProp);
            }
            ns.Types.Add(genType);
            }
        }
}