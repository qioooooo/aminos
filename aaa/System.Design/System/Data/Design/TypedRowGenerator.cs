using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace System.Data.Design
{
	// Token: 0x020000D2 RID: 210
	internal sealed class TypedRowGenerator
	{
		// Token: 0x060008D6 RID: 2262 RVA: 0x0001D090 File Offset: 0x0001C090
		internal TypedRowGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.codeGenerator = codeGenerator;
			this.convertXmlToObject = typeof(DataColumn).GetMethod("ConvertXmlToObject", BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[] { typeof(string) }, null);
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060008D7 RID: 2263 RVA: 0x0001D0DE File Offset: 0x0001C0DE
		internal MethodInfo ConvertXmlToObject
		{
			get
			{
				return this.convertXmlToObject;
			}
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0001D0E8 File Offset: 0x0001C0E8
		internal void GenerateRows(CodeTypeDeclaration dataSourceClass)
		{
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				dataSourceClass.Members.Add(this.GenerateRow(designTable));
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0001D158 File Offset: 0x0001C158
		internal void GenerateTypedRowEventHandlers(CodeTypeDeclaration dataSourceClass)
		{
			if (this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareEvents) && this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareDelegates))
			{
				foreach (object obj in this.codeGenerator.TableHandler.Tables)
				{
					DesignTable designTable = (DesignTable)obj;
					dataSourceClass.Members.Add(this.GenerateTypedRowEventHandler(designTable));
				}
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0001D1F8 File Offset: 0x0001C1F8
		internal void GenerateTypedRowEventArgs(CodeTypeDeclaration dataSourceClass)
		{
			if (this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareEvents) && this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareDelegates))
			{
				foreach (object obj in this.codeGenerator.TableHandler.Tables)
				{
					DesignTable designTable = (DesignTable)obj;
					dataSourceClass.Members.Add(this.CreateTypedRowEventArg(designTable));
				}
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0001D298 File Offset: 0x0001C298
		private CodeTypeDeclaration CreateTypedRowEventArg(DesignTable designTable)
		{
			if (designTable == null)
			{
				throw new InternalException("DesignTable should not be null.");
			}
			DataTable dataTable = designTable.DataTable;
			string generatorRowClassName = designTable.GeneratorRowClassName;
			string generatorTableClassName = designTable.GeneratorTableClassName;
			string generatorRowClassName2 = designTable.GeneratorRowClassName;
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class(designTable.GeneratorRowEvArgName, false, TypeAttributes.Public);
			codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(EventArgs)));
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Row event argument class", true));
			codeTypeDeclaration.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.Type(generatorRowClassName2), "eventRow"));
			codeTypeDeclaration.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(DataRowAction)), "eventAction"));
			codeTypeDeclaration.Members.Add(this.EventArgConstructor(generatorRowClassName2));
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.Type(generatorRowClassName2), "Row", (MemberAttributes)24578);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), "eventRow")));
			codeTypeDeclaration.Members.Add(codeMemberProperty);
			codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(DataRowAction)), "Action", (MemberAttributes)24578);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), "eventAction")));
			codeTypeDeclaration.Members.Add(codeMemberProperty);
			return codeTypeDeclaration;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0001D3F4 File Offset: 0x0001C3F4
		private CodeTypeDelegate GenerateTypedRowEventHandler(DesignTable table)
		{
			if (table == null)
			{
				throw new InternalException("DesignTable should not be null.");
			}
			string generatorRowClassName = table.GeneratorRowClassName;
			CodeTypeDelegate codeTypeDelegate = new CodeTypeDelegate(table.GeneratorRowEvHandlerName);
			codeTypeDelegate.TypeAttributes |= TypeAttributes.Public;
			codeTypeDelegate.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(object)), "sender"));
			codeTypeDelegate.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(table.GeneratorRowEvArgName), "e"));
			return codeTypeDelegate;
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0001D478 File Offset: 0x0001C478
		private CodeTypeDeclaration GenerateRow(DesignTable table)
		{
			if (table == null)
			{
				throw new InternalException("DesignTable should not be null.");
			}
			string generatorRowClassName = table.GeneratorRowClassName;
			string generatorTableClassName = table.GeneratorTableClassName;
			string generatorTableVarName = table.GeneratorTableVarName;
			TypedColumnHandler columnHandler = this.codeGenerator.TableHandler.GetColumnHandler(table.Name);
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class(generatorRowClassName, true, TypeAttributes.Public);
			codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(DataRow)));
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Represents strongly named DataRow class.", true));
			codeTypeDeclaration.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.Type(generatorTableClassName), generatorTableVarName));
			codeTypeDeclaration.Members.Add(this.RowConstructor(generatorTableClassName, generatorTableVarName));
			columnHandler.AddRowColumnProperties(codeTypeDeclaration);
			columnHandler.AddRowGetRelatedRowsMethods(codeTypeDeclaration);
			return codeTypeDeclaration;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001D540 File Offset: 0x0001C540
		private CodeConstructor RowConstructor(string tableClassName, string tableFieldName)
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)4098);
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRowBuilder)), "rb"));
			codeConstructor.BaseConstructorArgs.Add(CodeGenHelper.Argument("rb"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), tableFieldName), CodeGenHelper.Cast(CodeGenHelper.Type(tableClassName), CodeGenHelper.Property(CodeGenHelper.This(), "Table"))));
			return codeConstructor;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0001D5CC File Offset: 0x0001C5CC
		private CodeConstructor EventArgConstructor(string rowConcreteClassName)
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)24578);
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(rowConcreteClassName), "row"));
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRowAction)), "action"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), "eventRow"), CodeGenHelper.Argument("row")));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), "eventAction"), CodeGenHelper.Argument("action")));
			return codeConstructor;
		}

		// Token: 0x04000CAA RID: 3242
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000CAB RID: 3243
		private MethodInfo convertXmlToObject;
	}
}
