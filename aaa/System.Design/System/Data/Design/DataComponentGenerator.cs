using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel.Design;

namespace System.Data.Design
{
	// Token: 0x0200006F RID: 111
	internal sealed class DataComponentGenerator
	{
		// Token: 0x060004B8 RID: 1208 RVA: 0x00004046 File Offset: 0x00003046
		internal DataComponentGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.dataSourceGenerator = codeGenerator;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00004058 File Offset: 0x00003058
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

		// Token: 0x04000A99 RID: 2713
		private TypedDataSourceCodeGenerator dataSourceGenerator;

		// Token: 0x04000A9A RID: 2714
		private static string adapterDesigner = "Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner";
	}
}
