using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel.Design;

namespace System.Data.Design
{
	internal sealed class DataComponentGenerator
	{
		internal DataComponentGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.dataSourceGenerator = codeGenerator;
		}

		internal CodeTypeDeclaration GenerateDataComponent(DesignTable designTable, bool isFunctionsComponent, bool generateHierarchicalUpdate)
		{
			string generatorDataComponentClassName = designTable.GeneratorDataComponentClassName;
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class(generatorDataComponentClassName, true, designTable.DataAccessorModifier);
			codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(designTable.BaseClass));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerCategoryAttribute", CodeGenHelper.Str("code")));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.ToolboxItem", CodeGenHelper.Primitive(true)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DataObjectAttribute", CodeGenHelper.Primitive(true)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerAttribute", CodeGenHelper.Str(DataComponentGenerator.adapterDesigner + ", Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(HelpKeywordAttribute).FullName, CodeGenHelper.Str("vs.data.TableAdapter")));
			if (designTable.WebServiceAttribute)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Web.Services.WebService");
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Namespace", CodeGenHelper.Str(designTable.WebServiceNamespace)));
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Description", CodeGenHelper.Str(designTable.WebServiceDescription)));
				codeTypeDeclaration.CustomAttributes.Add(codeAttributeDeclaration);
			}
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Represents the connection and commands used to retrieve and save data.", true));
			DataComponentMethodGenerator dataComponentMethodGenerator = new DataComponentMethodGenerator(this.dataSourceGenerator, designTable, generateHierarchicalUpdate);
			dataComponentMethodGenerator.AddMethods(codeTypeDeclaration, isFunctionsComponent);
			CodeGenerator.ValidateIdentifiers(codeTypeDeclaration);
			QueryHandler queryHandler = new QueryHandler(this.dataSourceGenerator, designTable);
			if (isFunctionsComponent)
			{
				queryHandler.AddFunctionsToDataComponent(codeTypeDeclaration, true);
			}
			else
			{
				queryHandler.AddQueriesToDataComponent(codeTypeDeclaration);
			}
			return codeTypeDeclaration;
		}

		private TypedDataSourceCodeGenerator dataSourceGenerator;

		private static string adapterDesigner = "Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner";
	}
}
