using System.Collections.Generic;
using app.Metadata;
using System.CodeDom;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Text;
using System.IO;

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
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Lookup, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Customer, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Owner, typeof(string)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Money, typeof(decimal?)),
                    new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Uniqueidentifier, typeof(Guid)),
                }); 

        }
        private CodeNamespace CreateNameSpace (string nameSpaceName)
        {
            var returnValue = new CodeNamespace(nameSpaceName);
            returnValue.Imports.Add(new CodeNamespaceImport ("System"));
            returnValue.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            return returnValue;
        }

        public CodeTypeDeclaration CreateType (string name)
            => new CodeTypeDeclaration(name)
            {
                IsClass = true,
                IsPartial = true,
            };

        public CodeMemberProperty CreateProperty(AttributeMetadata attributeMetadata, Type propertyType)       
            => new CodeMemberProperty()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = $"{attributeMetadata.SchemaName} {{ get; {(attributeMetadata.IsValidForUpdate ? "set; ": "")} }}",
                Type = new CodeTypeReference(propertyType),
                HasGet = false,
                HasSet = false,                
            };

        public void Execute(List<EntityMetadata> entitMetadatas)
        {

            var provider = new Microsoft.CSharp.CSharpCodeProvider();                             
            var codeGeneratorOptions = new CodeGeneratorOptions();
            var ns = CreateNameSpace("webapi.entities");

            foreach (var entitMetadata in entitMetadatas)
            {
                var genType = CreateType(entitMetadata.SchemaName);
                foreach (var attribute in entitMetadata.Attributes)
                {            
                    if (!attribute.IsValidForRead)       
                      continue;

                    if (!_refMap.ContainsKey(attribute.AttributeType))
                        continue;

                    var genProp = CreateProperty(attribute, _refMap[attribute.AttributeType]);
                    genType.Members.Add(genProp);
                }
                ns.Types.Add(genType);

            }

            var cu = new CodeCompileUnit();
            cu.Namespaces.Add(ns);
            

            codeGeneratorOptions.BlankLinesBetweenMembers = false;
            codeGeneratorOptions.BracingStyle = "C";
            codeGeneratorOptions.IndentString = "   "; 

            var code = new StringBuilder();
            var stringWriter = new StringWriter(code);
            provider.GenerateCodeFromCompileUnit(cu, stringWriter, codeGeneratorOptions);

            var result = code.Replace("      {\r\n      }\r\n", "").ToString();

        }
    }
}