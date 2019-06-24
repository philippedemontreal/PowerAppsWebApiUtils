using System.Collections.Generic;
using System.CodeDom;
using System;
using System.Linq;
using System.CodeDom.Compiler;
using System.Text;
using System.IO;
using app.entities;
using System.Text.RegularExpressions;
using Microsoft.Dynamics.CRM;

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
                    
                    //new KeyValuePair<AttributeTypeCode, Type>(AttributeTypeCode.Picklist, typeof(string)),
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


        public CodeTypeDeclaration CreateType (EntityMetadata entitMetadata)
        {
            var result =  new CodeTypeDeclaration(entitMetadata.SchemaName)
            {
                IsClass = true,
                IsPartial = true,
                CustomAttributes = 
                {
                    new CodeAttributeDeclaration("DataContract", new CodeAttributeArgument("Name", new CodePrimitiveExpression(entitMetadata.LogicalName)))
                }                  
            };
            result.BaseTypes.Add(new CodeTypeReference(typeof(ExtendedEntity)));


            result.Members.Add(new CodeMemberField 
                { 
                    Type = new CodeTypeReference(typeof(string)),  
                    Name = "LogicalName", 
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    InitExpression = new CodePrimitiveExpression(entitMetadata.LogicalName)
                });
            result.Members.Add(new CodeMemberField 
                { 
                    Type = new CodeTypeReference(typeof(string)),  
                    Name = "LogicalCollectionName", 
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    InitExpression = new CodePrimitiveExpression(entitMetadata.LogicalCollectionName)
                });

            result.Comments.Add(new CodeCommentStatement("<summary>", true));
            result.Comments.Add(new CodeCommentStatement($"<para>Description: {entitMetadata.Description.UserLocalizedLabel.Label}</para>", true));
            result.Comments.Add(new CodeCommentStatement($"<para>Display Name: {entitMetadata.DisplayName.UserLocalizedLabel.Label}</para>", true));
            result.Comments.Add(new CodeCommentStatement("</summary>", true));


            return result;
        }


       public CodeMemberProperty CreateProperty(AttributeMetadata attributeMetadata, Type propertyType)
        => CreateProperty(attributeMetadata, propertyType.FullName);
        public CodeMemberProperty CreateProperty(AttributeMetadata attributeMetadata, string propertyType) 
        {
            var schemaName = attributeMetadata.SchemaName;
           
            if (!schemaName.StartsWith("Yomi") && schemaName.Contains("Yomi"))
                return null;

            var result = new CodeMemberProperty()
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Final,
                Name = schemaName,
                Type = new CodeTypeReference(propertyType),
                CustomAttributes = 
                {
                    new CodeAttributeDeclaration("DataMember", new CodeAttributeArgument("Name", new CodePrimitiveExpression(attributeMetadata.LogicalName)))
                }      
            };

            if (attributeMetadata.IsValidForRead || attributeMetadata.IsValidForCreate || attributeMetadata.IsValidForUpdate)
            {
                result.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(
                            new CodeBaseReferenceExpression(),
                                "GetAttributeValue",
                                new CodeTypeReference[] { new CodeTypeReference(propertyType) }),
                                new CodePrimitiveExpression(attributeMetadata.LogicalName))                            
                ));
            }

            if (attributeMetadata.IsValidForCreate || attributeMetadata.IsValidForUpdate)
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

            result.Comments.Add(new CodeCommentStatement("<summary>", true));
            result.Comments.Add(new CodeCommentStatement($"<para>Description: {attributeMetadata.Description?.UserLocalizedLabel?.Label}</para>", true));
            result.Comments.Add(new CodeCommentStatement($"<para>Display Name: {attributeMetadata.DisplayName?.UserLocalizedLabel?.Label}</para>", true));
            result.Comments.Add(new CodeCommentStatement("</summary>", true));


            return result;
        }      



        public void Execute(string namespacename, List<EntityMetadata> entitMetadatas, Dictionary<string, PicklistAttributeMetadata> picklists)
        {

            var provider = new Microsoft.CSharp.CSharpCodeProvider();                             
            var codeGeneratorOptions = new CodeGeneratorOptions();

            var ns = CreateNameSpace(namespacename);
            foreach (var entitMetadata in entitMetadatas.OrderBy(p => p.SchemaName))
            {
                MakeEntity(entitMetadata, ns, picklists);
            }

            var generatedOptionsets = new List<string>();
            foreach (var picklist in picklists.Values.OrderBy(p => p.LogicalName))
            {
                if (generatedOptionsets.Contains(picklist.OptionSet.Name))
                    continue;
                generatedOptionsets.Add(picklist.OptionSet.Name);
                MakOptionSet(picklist, ns);
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

        private string Sanitize(string enumName)
        {
            if (string.IsNullOrEmpty(enumName))
                return "_EmptyString";
            
            var result = Regex.Replace(enumName, @"[^\w]", "");
            result = Regex.Replace(result, @"^(\d)", @"_$1");

            return result;
        }

        private void MakOptionSet(PicklistAttributeMetadata picklist, CodeNamespace ns)
        {
            var result =  new CodeTypeDeclaration(picklist.OptionSet.Name) { IsEnum = true };

            foreach (var optionset in picklist.OptionSet.Options)
            {
                var option = new CodeMemberField(picklist.OptionSet.Name, Sanitize(optionset.Label.UserLocalizedLabel.Label)) 
                { 
                    InitExpression = new CodePrimitiveExpression(optionset.Value)
                };

                result.Members.Add(option);
            }

            result.Comments.Add(new CodeCommentStatement("<summary>", true));
            result.Comments.Add(new CodeCommentStatement($"<para>Description: {picklist.Description.UserLocalizedLabel.Label}</para>", true));
            result.Comments.Add(new CodeCommentStatement($"<para>Display Name: {picklist.DisplayName.UserLocalizedLabel.Label}</para>", true));
            result.Comments.Add(new CodeCommentStatement("</summary>", true));

           ns.Types.Add(result);
         }

        private void MakeEntity(EntityMetadata entitMetadata, CodeNamespace ns,  Dictionary<string, PicklistAttributeMetadata> picklists)
        {
            var genType = CreateType(entitMetadata);
            var primary = entitMetadata.Attributes.Where(p => p.IsPrimaryId).FirstOrDefault();
            var prop = CreateProperty(primary, typeof(Guid));
            genType.Members.Add(prop);


            foreach (var attribute in entitMetadata.Attributes.OrderBy(p => p.SchemaName.ToLowerInvariant()))
            {            
                if (!attribute.IsValidForRead || attribute.IsPrimaryId)       
                    continue;


                if (attribute.AttributeType == AttributeTypeCode.Picklist || attribute.AttributeType == AttributeTypeCode.State || attribute.AttributeType == AttributeTypeCode.Status)
                {
                    var picklist = picklists.Select(p => p.Value).Where(p => p.LogicalName == attribute.LogicalName&& p.EntityLogicalName == entitMetadata.LogicalName).FirstOrDefault();
                    if (picklist != null)
                    {
                        genType.Members.Add(CreateProperty(attribute, $"{picklist.OptionSet.Name}?"));
                    }
                    continue;
                }
                else {
                                    
                    if (!_refMap.ContainsKey(attribute.AttributeType))
                        continue;
                    var genProp = CreateProperty(attribute, _refMap[attribute.AttributeType]);
                    if (genProp != null)
                        genType.Members.Add(genProp);

                }

            }
            ns.Types.Add(genType);
        }
    }
}
