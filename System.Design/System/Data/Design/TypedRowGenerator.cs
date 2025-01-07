using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace System.Data.Design
{
	internal sealed class TypedRowGenerator
	{
		internal TypedRowGenerator(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.codeGenerator = codeGenerator;
			this.convertXmlToObject = typeof(DataColumn).GetMethod("ConvertXmlToObject", BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[] { typeof(string) }, null);
		}

		internal MethodInfo ConvertXmlToObject
		{
			get
			{
				return this.convertXmlToObject;
			}
		}

		internal void GenerateRows(CodeTypeDeclaration dataSourceClass)
		{
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				dataSourceClass.Members.Add(this.GenerateRow(designTable));
			}
		}

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

		private CodeConstructor RowConstructor(string tableClassName, string tableFieldName)
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)4098);
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRowBuilder)), "rb"));
			codeConstructor.BaseConstructorArgs.Add(CodeGenHelper.Argument("rb"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), tableFieldName), CodeGenHelper.Cast(CodeGenHelper.Type(tableClassName), CodeGenHelper.Property(CodeGenHelper.This(), "Table"))));
			return codeConstructor;
		}

		private CodeConstructor EventArgConstructor(string rowConcreteClassName)
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)24578);
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(rowConcreteClassName), "row"));
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRowAction)), "action"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), "eventRow"), CodeGenHelper.Argument("row")));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), "eventAction"), CodeGenHelper.Argument("action")));
			return codeConstructor;
		}

		private TypedDataSourceCodeGenerator codeGenerator;

		private MethodInfo convertXmlToObject;
	}
}
