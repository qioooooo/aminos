using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace System.Data.Design
{
	// Token: 0x020000BD RID: 189
	internal sealed class TableAdapterManagerGenerator
	{
		// Token: 0x0600085E RID: 2142 RVA: 0x000155F9 File Offset: 0x000145F9
		internal TableAdapterManagerGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.dataSourceGenerator = codeGenerator;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00015608 File Offset: 0x00014608
		internal CodeTypeDeclaration GenerateAdapterManager(DesignDataSource dataSource, CodeTypeDeclaration dataSourceClass)
		{
			TypeAttributes typeAttributes = TypeAttributes.Public;
			foreach (object obj in dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				if ((designTable.DataAccessorModifier & TypeAttributes.Public) != TypeAttributes.Public)
				{
					typeAttributes = designTable.DataAccessorModifier;
				}
			}
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class("TableAdapterManager", true, typeAttributes);
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("TableAdapterManager is used to coordinate TableAdapters in the dataset to enable Hierarchical Update scenarios", true));
			codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(Component)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerCategoryAttribute", CodeGenHelper.Str("code")));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.ToolboxItem", CodeGenHelper.Primitive(true)));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerAttribute", CodeGenHelper.Str("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerDesigner, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")));
			codeTypeDeclaration.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(HelpKeywordAttribute).FullName, CodeGenHelper.Str("vs.data.TableAdapterManager")));
			TableAdapterManagerMethodGenerator tableAdapterManagerMethodGenerator = new TableAdapterManagerMethodGenerator(this.dataSourceGenerator, dataSource, dataSourceClass);
			tableAdapterManagerMethodGenerator.AddEverything(codeTypeDeclaration);
			try
			{
				CodeGenerator.ValidateIdentifiers(codeTypeDeclaration);
			}
			catch (Exception)
			{
			}
			return codeTypeDeclaration;
		}

		// Token: 0x04000C14 RID: 3092
		private const string adapterDesigner = "Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerDesigner";

		// Token: 0x04000C15 RID: 3093
		private const string helpKeyword = "vs.data.TableAdapterManager";

		// Token: 0x04000C16 RID: 3094
		private TypedDataSourceCodeGenerator dataSourceGenerator;
	}
}
