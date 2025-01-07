using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace System.Data.Design
{
	internal sealed class TableAdapterManagerGenerator
	{
		internal TableAdapterManagerGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.dataSourceGenerator = codeGenerator;
		}

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

		private const string adapterDesigner = "Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerDesigner";

		private const string helpKeyword = "vs.data.TableAdapterManager";

		private TypedDataSourceCodeGenerator dataSourceGenerator;
	}
}
