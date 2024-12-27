using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Common;
using System.Design;
using System.Diagnostics;
using System.Reflection;

namespace System.Data.Design
{
	// Token: 0x020000C0 RID: 192
	internal sealed class TableAdapterManagerMethodGenerator
	{
		// Token: 0x0600086A RID: 2154 RVA: 0x00015B94 File Offset: 0x00014B94
		internal TableAdapterManagerMethodGenerator(TypedDataSourceCodeGenerator codeGenerator, DesignDataSource dataSource, CodeTypeDeclaration dataSourceType)
		{
			this.codeGenerator = codeGenerator;
			this.dataSource = dataSource;
			this.dataSourceType = dataSourceType;
			this.nameHandler = new TableAdapterManagerNameHandler(codeGenerator.CodeProvider);
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00015BC4 File Offset: 0x00014BC4
		internal void AddEverything(CodeTypeDeclaration dataComponentClass)
		{
			if (dataComponentClass == null)
			{
				throw new InternalException("dataComponent CodeTypeDeclaration should not be null.");
			}
			this.AddUpdateOrderMembers(dataComponentClass);
			this.AddAdapterMembers(dataComponentClass);
			this.AddVariableAndProperty(dataComponentClass, (MemberAttributes)24578, CodeGenHelper.GlobalType(typeof(bool)), "BackupDataSetBeforeUpdate", "_backupDataSetBeforeUpdate", false);
			this.AddConnectionMembers(dataComponentClass);
			this.AddTableAdapterCountMembers(dataComponentClass);
			this.AddUpdateAll(dataComponentClass);
			this.AddSortSelfRefRows(dataComponentClass);
			this.AddSelfRefComparer(dataComponentClass);
			this.AddMatchTableAdapterConnection(dataComponentClass);
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00015C40 File Offset: 0x00014C40
		private void AddUpdateOrderMembers(CodeTypeDeclaration dataComponentClass)
		{
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class("UpdateOrderOption", false, TypeAttributes.NestedPublic);
			codeTypeDeclaration.IsEnum = true;
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Update Order Option", true));
			CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.Type(typeof(int)), "InsertUpdateDelete", CodeGenHelper.Primitive(0));
			codeTypeDeclaration.Members.Add(codeMemberField);
			CodeMemberField codeMemberField2 = CodeGenHelper.FieldDecl(CodeGenHelper.Type(typeof(int)), "UpdateInsertDelete", CodeGenHelper.Primitive(1));
			codeTypeDeclaration.Members.Add(codeMemberField2);
			dataComponentClass.Members.Add(codeTypeDeclaration);
			this.AddVariableAndProperty(dataComponentClass, (MemberAttributes)24578, CodeGenHelper.Type("UpdateOrderOption"), "UpdateOrder", "_updateOrder", false);
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00015D0C File Offset: 0x00014D0C
		private void AddAdapterMembers(CodeTypeDeclaration dataComponentClass)
		{
			foreach (object obj in this.dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				if (this.CanAddTableAdapter(designTable))
				{
					designTable.PropertyCache.TAMAdapterPropName = this.nameHandler.GetTableAdapterPropName(designTable.GeneratorDataComponentClassName);
					designTable.PropertyCache.TAMAdapterVarName = this.nameHandler.GetTableAdapterVarName(designTable.PropertyCache.TAMAdapterPropName);
					string tamadapterVarName = designTable.PropertyCache.TAMAdapterVarName;
					CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.Type(designTable.GeneratorDataComponentClassName), tamadapterVarName);
					dataComponentClass.Members.Add(codeMemberField);
					CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.Type(designTable.GeneratorDataComponentClassName), designTable.PropertyCache.TAMAdapterPropName, (MemberAttributes)24578);
					codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.EditorAttribute", CodeGenHelper.Str("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), CodeGenHelper.Str("System.Drawing.Design.UITypeEditor")));
					codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.ThisField(tamadapterVarName)));
					codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.ThisField(tamadapterVarName), CodeGenHelper.Argument("value")));
					dataComponentClass.Members.Add(codeMemberProperty);
				}
			}
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00015E80 File Offset: 0x00014E80
		private void AddConnectionMembers(CodeTypeDeclaration dataComponentClass)
		{
			string text = "_connection";
			CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(IDbConnection)), text);
			dataComponentClass.Members.Add(codeMemberField);
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(IDbConnection)), "Connection", (MemberAttributes)24578);
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.Browsable", CodeGenHelper.Primitive(false)));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField(text)), CodeGenHelper.Return(CodeGenHelper.ThisField(text))));
			foreach (object obj in this.dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				if (this.CanAddTableAdapter(designTable))
				{
					string tamadapterVarName = designTable.PropertyCache.TAMAdapterVarName;
					codeMemberProperty.GetStatements.Add(CodeGenHelper.If(CodeGenHelper.And(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField(tamadapterVarName)), CodeGenHelper.IdIsNotNull(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName), "Connection"))), CodeGenHelper.Return(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName), "Connection"))));
				}
			}
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(null)));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.ThisField(text), CodeGenHelper.Argument("value")));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00016018 File Offset: 0x00015018
		private void AddTableAdapterCountMembers(CodeTypeDeclaration dataComponentClass)
		{
			string text = "count";
			CodeExpression codeExpression = CodeGenHelper.Variable(text);
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(int)), "TableAdapterInstanceCount", (MemberAttributes)24578);
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.Browsable", CodeGenHelper.Primitive(false)));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), text, CodeGenHelper.Primitive(0)));
			foreach (object obj in this.dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				if (this.CanAddTableAdapter(designTable))
				{
					string tamadapterVarName = designTable.PropertyCache.TAMAdapterVarName;
					codeMemberProperty.GetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField(tamadapterVarName)), CodeGenHelper.Assign(codeExpression, CodeGenHelper.BinOperator(codeExpression, CodeBinaryOperatorType.Add, CodeGenHelper.Primitive(1)))));
				}
			}
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(codeExpression));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x0001615C File Offset: 0x0001515C
		private void AddSortSelfRefRows(CodeTypeDeclaration dataComponentClass)
		{
			string text = "rows";
			string text2 = "relation";
			string text3 = "childFirst";
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "SortSelfReferenceRows", MemberAttributes.Family);
			codeMemberMethod.Parameters.AddRange(new CodeParameterDeclarationExpression[]
			{
				CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRow), 1), text),
				CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRelation)), text2),
				CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(bool)), text3)
			});
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(CodeGenHelper.GlobalTypeExpr("System.Array"), "Sort", new CodeTypeReference[] { CodeGenHelper.GlobalType(typeof(DataRow)) });
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeMethodReferenceExpression, new CodeExpression[]
			{
				CodeGenHelper.Argument(text),
				CodeGenHelper.New(CodeGenHelper.Type("SelfReferenceComparer"), new CodeExpression[]
				{
					CodeGenHelper.Argument(text2),
					CodeGenHelper.Argument(text3)
				})
			});
			codeMemberMethod.Statements.Add(CodeGenHelper.Stm(codeMethodInvokeExpression));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0001629C File Offset: 0x0001529C
		private void AddSelfRefComparer(CodeTypeDeclaration dataComponentClass)
		{
			string text = "_relation";
			string text2 = "_childFirst";
			CodeTypeDeclaration codeTypeDeclaration = CodeGenHelper.Class("SelfReferenceComparer", false, TypeAttributes.NestedPrivate);
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalGenericType("System.Collections.Generic.IComparer", typeof(DataRow));
			codeTypeDeclaration.BaseTypes.Add(CodeGenHelper.GlobalType(typeof(object)));
			codeTypeDeclaration.BaseTypes.Add(codeTypeReference);
			codeTypeDeclaration.Comments.Add(CodeGenHelper.Comment("Used to sort self-referenced table's rows", true));
			dataComponentClass.Members.Add(codeTypeDeclaration);
			codeTypeDeclaration.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(DataRelation)), text));
			codeTypeDeclaration.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(int)), text2));
			CodeConstructor codeConstructor = CodeGenHelper.Constructor(MemberAttributes.Assembly);
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRelation)), "relation"));
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(bool)), "childFirst"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.ThisField(text), CodeGenHelper.Argument("relation")));
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.Argument("childFirst"), CodeGenHelper.Assign(CodeGenHelper.ThisField(text2), CodeGenHelper.Primitive(-1)), CodeGenHelper.Assign(CodeGenHelper.ThisField(text2), CodeGenHelper.Primitive(1))));
			codeTypeDeclaration.Members.Add(codeConstructor);
			string text3 = "child";
			string text4 = "parent";
			string text5 = "newParent";
			string text6 = "IsChildAndParent";
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(bool)), text6, MemberAttributes.Private);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRow)), text3));
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRow)), text4));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCallStm(CodeGenHelper.GlobalTypeExpr(typeof(Debug)), "Assert", CodeGenHelper.IdIsNotNull(CodeGenHelper.Argument(text3))));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCallStm(CodeGenHelper.GlobalTypeExpr(typeof(Debug)), "Assert", CodeGenHelper.IdIsNotNull(CodeGenHelper.Argument(text4))));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataRow)), text5, CodeGenHelper.MethodCall(CodeGenHelper.Argument(text3), "GetParentRow", new CodeExpression[]
			{
				CodeGenHelper.ThisField(text),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), "Default")
			})));
			CodeIterationStatement codeIterationStatement = new CodeIterationStatement();
			codeIterationStatement.TestExpression = CodeGenHelper.And(CodeGenHelper.IdIsNotNull(CodeGenHelper.Variable(text5)), CodeGenHelper.And(CodeGenHelper.ReferenceNotEquals(CodeGenHelper.Variable(text5), CodeGenHelper.Argument(text3)), CodeGenHelper.ReferenceNotEquals(CodeGenHelper.Variable(text5), CodeGenHelper.Argument(text4))));
			codeIterationStatement.InitStatement = new CodeSnippetStatement();
			codeIterationStatement.IncrementStatement = new CodeSnippetStatement();
			codeIterationStatement.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Variable(text5), CodeGenHelper.MethodCall(CodeGenHelper.Variable(text5), "GetParentRow", new CodeExpression[]
			{
				CodeGenHelper.ThisField(text),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), "Default")
			})));
			codeMemberMethod.Statements.Add(codeIterationStatement);
			codeIterationStatement = new CodeIterationStatement();
			codeIterationStatement.TestExpression = CodeGenHelper.And(CodeGenHelper.IdIsNotNull(CodeGenHelper.Variable(text5)), CodeGenHelper.And(CodeGenHelper.ReferenceNotEquals(CodeGenHelper.Variable(text5), CodeGenHelper.Argument(text3)), CodeGenHelper.ReferenceNotEquals(CodeGenHelper.Variable(text5), CodeGenHelper.Argument(text4))));
			codeIterationStatement.InitStatement = CodeGenHelper.Assign(CodeGenHelper.Variable(text5), CodeGenHelper.MethodCall(CodeGenHelper.Argument(text3), "GetParentRow", new CodeExpression[]
			{
				CodeGenHelper.ThisField(text),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), "Original")
			}));
			codeIterationStatement.IncrementStatement = new CodeSnippetStatement();
			codeIterationStatement.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Variable(text5), CodeGenHelper.MethodCall(CodeGenHelper.Argument(text5), "GetParentRow", new CodeExpression[]
			{
				CodeGenHelper.ThisField(text),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), "Original")
			})));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNull(CodeGenHelper.Variable(text5)), codeIterationStatement));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.ReferenceEquals(CodeGenHelper.Variable(text5), CodeGenHelper.Argument(text4)), CodeGenHelper.Return(CodeGenHelper.Primitive(true))));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(false)));
			codeTypeDeclaration.Members.Add(codeMemberMethod);
			string text7 = "row1";
			string text8 = "row2";
			CodeMemberMethod codeMemberMethod2 = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(int)), "Compare", (MemberAttributes)24578);
			codeMemberMethod2.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRow)), text7));
			codeMemberMethod2.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRow)), text8));
			codeMemberMethod2.ImplementationTypes.Add(codeTypeReference);
			codeTypeDeclaration.Members.Add(codeMemberMethod2);
			codeMemberMethod2.Statements.Add(CodeGenHelper.If(CodeGenHelper.ReferenceEquals(CodeGenHelper.Argument(text7), CodeGenHelper.Argument(text8)), CodeGenHelper.Return(CodeGenHelper.Primitive(0))));
			codeMemberMethod2.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNull(CodeGenHelper.Argument(text7)), CodeGenHelper.Return(CodeGenHelper.Primitive(-1))));
			codeMemberMethod2.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNull(CodeGenHelper.Argument(text8)), CodeGenHelper.Return(CodeGenHelper.Primitive(1))));
			codeMemberMethod2.Statements.Add(new CodeSnippetStatement());
			codeMemberMethod2.Statements.Add(new CodeCommentStatement("Is row1 the child or grandchild of row2"));
			codeMemberMethod2.Statements.Add(CodeGenHelper.If(CodeGenHelper.MethodCall(CodeGenHelper.This(), text6, new CodeExpression[]
			{
				CodeGenHelper.Argument(text7),
				CodeGenHelper.Argument(text8)
			}), CodeGenHelper.Return(CodeGenHelper.ThisField(text2))));
			codeMemberMethod2.Statements.Add(new CodeSnippetStatement());
			codeMemberMethod2.Statements.Add(new CodeCommentStatement("Is row2 the child or grandchild of row1"));
			codeMemberMethod2.Statements.Add(CodeGenHelper.If(CodeGenHelper.MethodCall(CodeGenHelper.This(), text6, new CodeExpression[]
			{
				CodeGenHelper.Argument(text8),
				CodeGenHelper.Argument(text7)
			}), CodeGenHelper.Return(CodeGenHelper.BinOperator(CodeGenHelper.Primitive(-1), CodeBinaryOperatorType.Multiply, CodeGenHelper.ThisField(text2)))));
			codeMemberMethod2.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(0)));
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00016A04 File Offset: 0x00015A04
		private void AddMatchTableAdapterConnection(CodeTypeDeclaration dataComponentClass)
		{
			string text = "inputConnection";
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(bool)), "MatchTableAdapterConnection", MemberAttributes.Family);
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalType(typeof(IDbConnection));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, text);
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField("_connection")), CodeGenHelper.Return(CodeGenHelper.Primitive(true))));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.Or(CodeGenHelper.IdIsNull(CodeGenHelper.ThisProperty("Connection")), CodeGenHelper.IdIsNull(CodeGenHelper.Argument(text))), CodeGenHelper.Return(CodeGenHelper.Primitive(true))));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(typeof(string)), "Equals", new CodeExpression[]
			{
				CodeGenHelper.Property(CodeGenHelper.ThisProperty("Connection"), "ConnectionString"),
				CodeGenHelper.Property(CodeGenHelper.Argument(text), "ConnectionString"),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(StringComparison)), "Ordinal")
			}), CodeGenHelper.Return(CodeGenHelper.Primitive(true))));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(false)));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00016B84 File Offset: 0x00015B84
		private void AddUpdateAll(CodeTypeDeclaration dataComponentClass)
		{
			string text = "dataSet";
			string text2 = "backupDataSet";
			string text3 = "deletedRows";
			string text4 = "addedRows";
			string text5 = "updatedRows";
			string text6 = "result";
			string text7 = "workConnection";
			string text8 = "workTransaction";
			string text9 = "workConnOpened";
			string text10 = "allChangedRows";
			string text11 = "allAddedRows";
			string text12 = "adaptersWithAcceptChangesDuringUpdate";
			string text13 = "revertConnections";
			CodeExpression codeExpression = CodeGenHelper.Variable(text6);
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(int)), "UpdateAll", MemberAttributes.Public);
			string text14 = this.dataSourceType.Name;
			if (this.codeGenerator.DataSetNamespace != null)
			{
				text14 = CodeGenHelper.GetTypeName(this.codeGenerator.CodeProvider, this.codeGenerator.DataSetNamespace, text14);
			}
			CodeTypeReference codeTypeReference = CodeGenHelper.Type(text14);
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, text);
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeMemberMethod.Comments.Add(CodeGenHelper.Comment("Update all changes to the dataset.", true));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNull(CodeGenHelper.Argument(text)), CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(ArgumentNullException)), text)));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.Argument(text), "HasChanges"), CodeGenHelper.Primitive(false)), CodeGenHelper.Return(CodeGenHelper.Primitive(0))));
			foreach (object obj in this.dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				if (this.CanAddTableAdapter(designTable))
				{
					string tamadapterVarName = designTable.PropertyCache.TAMAdapterVarName;
					codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.And(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField(tamadapterVarName)), CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.This(), "MatchTableAdapterConnection", CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName), "Connection")), CodeGenHelper.Primitive(false))), new CodeStatement[]
					{
						new CodeThrowExceptionStatement(CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(ArgumentException)), new CodeExpression[] { CodeGenHelper.Str(SR.GetString("CG_TableAdapterManagerNeedsSameConnString")) }))
					}));
				}
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(IDbConnection)), text7, CodeGenHelper.ThisProperty("Connection")));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNull(CodeGenHelper.Variable(text7)), CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(ApplicationException)), SR.GetString("CG_TableAdapterManagerHasNoConnection"))));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(bool)), text9, CodeGenHelper.Primitive(false)));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.BitwiseAnd(CodeGenHelper.Property(CodeGenHelper.Variable(text7), "State"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Broken")), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Broken")), CodeGenHelper.MethodCallStm(CodeGenHelper.Variable(text7), "Close")));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Property(CodeGenHelper.Variable(text7), "State"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Closed")), new CodeStatement[]
			{
				CodeGenHelper.MethodCallStm(CodeGenHelper.Variable(text7), "Open"),
				CodeGenHelper.Assign(CodeGenHelper.Variable(text9), CodeGenHelper.Primitive(true))
			}));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(IDbTransaction)), text8, CodeGenHelper.MethodCall(CodeGenHelper.Variable(text7), "BeginTransaction")));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdIsNull(CodeGenHelper.Variable(text8)), CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(ApplicationException)), SR.GetString("CG_TableAdapterManagerNotSupportTransaction"))));
			CodeTypeReference codeTypeReference2 = CodeGenHelper.GlobalGenericType("System.Collections.Generic.List", typeof(DataRow));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(codeTypeReference2, text10, CodeGenHelper.New(codeTypeReference2, new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(codeTypeReference2, text11, CodeGenHelper.New(codeTypeReference2, new CodeExpression[0])));
			codeTypeReference2 = CodeGenHelper.GlobalGenericType("System.Collections.Generic.List", typeof(DataAdapter));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(codeTypeReference2, text12, CodeGenHelper.New(codeTypeReference2, new CodeExpression[0])));
			CodeTypeReference codeTypeReference3 = new CodeTypeReference("System.Collections.Generic.Dictionary", new CodeTypeReference[]
			{
				CodeGenHelper.GlobalType(typeof(object)),
				CodeGenHelper.GlobalType(typeof(IDbConnection))
			});
			codeTypeReference3.Options = CodeTypeReferenceOptions.GlobalReference;
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(codeTypeReference3, text13, CodeGenHelper.New(codeTypeReference3, new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(typeof(int)), text6, CodeGenHelper.Primitive(0)));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataSet)), text2, CodeGenHelper.Primitive(null)));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.ThisProperty("BackupDataSetBeforeUpdate"), new CodeStatement[]
			{
				CodeGenHelper.Assign(CodeGenHelper.Variable(text2), CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(DataSet)), new CodeExpression[0])),
				CodeGenHelper.MethodCallStm(CodeGenHelper.Variable(text2), "Merge", CodeGenHelper.Argument(text))
			}));
			List<CodeStatement> list = new List<CodeStatement>();
			list.Add(new CodeCommentStatement("---- Prepare for update -----------\r\n"));
			foreach (object obj2 in this.dataSource.DesignTables)
			{
				DesignTable designTable2 = (DesignTable)obj2;
				if (this.CanAddTableAdapter(designTable2))
				{
					string tamadapterVarName2 = designTable2.PropertyCache.TAMAdapterVarName;
					CodeStatement codeStatement;
					if (designTable2.PropertyCache.TransactionType != null)
					{
						codeStatement = CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName2), "Transaction"), CodeGenHelper.Cast(CodeGenHelper.GlobalType(designTable2.PropertyCache.TransactionType), CodeGenHelper.Variable(text8)));
					}
					else
					{
						codeStatement = new CodeCommentStatement("Note: The TableAdapter does not have the Transaction property.");
					}
					CodeStatement codeStatement2;
					if (designTable2.PropertyCache.AdapterType != null && typeof(DataAdapter).IsAssignableFrom(designTable2.PropertyCache.AdapterType))
					{
						codeStatement2 = CodeGenHelper.If(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName2), "Adapter"), "AcceptChangesDuringUpdate"), new CodeStatement[]
						{
							CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName2), "Adapter"), "AcceptChangesDuringUpdate"), CodeGenHelper.Primitive(false)),
							CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(text12), "Add", CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName2), "Adapter")))
						});
					}
					else
					{
						codeStatement2 = new CodeCommentStatement("Note: Adapter is not a DataAdapter, so AcceptChangesDuringUpdate cannot be set to false.");
					}
					list.Add(CodeGenHelper.If(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField(tamadapterVarName2)), new CodeStatement[]
					{
						CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(text13), "Add", new CodeExpression[]
						{
							CodeGenHelper.ThisField(tamadapterVarName2),
							CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName2), "Connection")
						})),
						CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName2), "Connection"), CodeGenHelper.Cast(CodeGenHelper.GlobalType(designTable2.PropertyCache.ConnectionType), CodeGenHelper.Variable(text7))),
						codeStatement,
						codeStatement2
					}));
				}
			}
			DataTable[] updateOrder = TableAdapterManagerHelper.GetUpdateOrder(this.dataSource.DataSet);
			this.AddUpdateUpdatedMethod(dataComponentClass, updateOrder, codeParameterDeclarationExpression, text, text6, text5, text10, text11);
			this.AddUpdateInsertedMethod(dataComponentClass, updateOrder, codeParameterDeclarationExpression, text, text6, text4, text11);
			this.AddUpdateDeletedMethod(dataComponentClass, updateOrder, codeParameterDeclarationExpression, text, text6, text3, text10);
			this.AddRealUpdatedRowsMethod(dataComponentClass, text5, text11);
			list.Add(new CodeCommentStatement("\r\n---- Perform updates -----------\r\n"));
			CodeStatement codeStatement3 = CodeGenHelper.Assign(codeExpression, CodeGenHelper.BinOperator(codeExpression, CodeBinaryOperatorType.Add, CodeGenHelper.MethodCall(CodeGenHelper.This(), "UpdateInsertedRows", new CodeExpression[]
			{
				CodeGenHelper.Argument(text),
				CodeGenHelper.Variable(text11)
			})));
			CodeStatement codeStatement4 = CodeGenHelper.Assign(codeExpression, CodeGenHelper.BinOperator(codeExpression, CodeBinaryOperatorType.Add, CodeGenHelper.MethodCall(CodeGenHelper.This(), "UpdateUpdatedRows", new CodeExpression[]
			{
				CodeGenHelper.Argument(text),
				CodeGenHelper.Variable(text10),
				CodeGenHelper.Variable(text11)
			})));
			list.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.ThisProperty("UpdateOrder"), CodeGenHelper.Field(CodeGenHelper.TypeExpr(CodeGenHelper.Type("UpdateOrderOption")), "UpdateInsertDelete")), new CodeStatement[] { codeStatement4, codeStatement3 }, new CodeStatement[] { codeStatement3, codeStatement4 }));
			list.Add(CodeGenHelper.Assign(codeExpression, CodeGenHelper.BinOperator(codeExpression, CodeBinaryOperatorType.Add, CodeGenHelper.MethodCall(CodeGenHelper.This(), "UpdateDeletedRows", new CodeExpression[]
			{
				CodeGenHelper.Argument(text),
				CodeGenHelper.Variable(text10)
			}))));
			list.Add(new CodeCommentStatement("\r\n---- Commit updates -----------\r\n"));
			list.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(text8), "Commit")));
			list.Add(this.HandleForEachRowInList(text11, new string[] { "AcceptChanges" }));
			list.Add(this.HandleForEachRowInList(text10, new string[] { "AcceptChanges" }));
			CodeCatchClause codeCatchClause = new CodeCatchClause();
			codeCatchClause.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Variable(text8), "Rollback"));
			codeCatchClause.Statements.Add(new CodeCommentStatement("---- Restore the dataset -----------"));
			codeCatchClause.Statements.Add(CodeGenHelper.If(CodeGenHelper.ThisProperty("BackupDataSetBeforeUpdate"), new CodeStatement[]
			{
				CodeGenHelper.MethodCallStm(CodeGenHelper.GlobalTypeExpr(typeof(Debug)), "Assert", CodeGenHelper.IdIsNotNull(CodeGenHelper.Variable(text2))),
				CodeGenHelper.MethodCallStm(CodeGenHelper.Argument(text), "Clear"),
				CodeGenHelper.MethodCallStm(CodeGenHelper.Argument(text), "Merge", CodeGenHelper.Variable(text2))
			}, new CodeStatement[] { this.HandleForEachRowInList(text11, new string[] { "AcceptChanges", "SetAdded" }) }));
			codeCatchClause.CatchExceptionType = CodeGenHelper.GlobalType(typeof(Exception));
			codeCatchClause.LocalName = "ex";
			codeCatchClause.Statements.Add(new CodeThrowExceptionStatement(CodeGenHelper.Variable("ex")));
			List<CodeStatement> list2 = new List<CodeStatement>();
			list2.Add(CodeGenHelper.If(CodeGenHelper.Variable(text9), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(text7), "Close"))));
			foreach (object obj3 in this.dataSource.DesignTables)
			{
				DesignTable designTable3 = (DesignTable)obj3;
				if (this.CanAddTableAdapter(designTable3))
				{
					string tamadapterVarName3 = designTable3.PropertyCache.TAMAdapterVarName;
					CodeStatement codeStatement5;
					if (designTable3.PropertyCache.TransactionType != null)
					{
						codeStatement5 = CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName3), "Transaction"), CodeGenHelper.Primitive(null));
					}
					else
					{
						codeStatement5 = new CodeCommentStatement("Note: No Transaction property of the TableAdapter");
					}
					list2.Add(CodeGenHelper.If(CodeGenHelper.IdIsNotNull(CodeGenHelper.ThisField(tamadapterVarName3)), new CodeStatement[]
					{
						CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.ThisField(tamadapterVarName3), "Connection"), CodeGenHelper.Cast(CodeGenHelper.GlobalType(designTable3.PropertyCache.ConnectionType), CodeGenHelper.Indexer(CodeGenHelper.Variable(text13), CodeGenHelper.ThisField(tamadapterVarName3)))),
						codeStatement5
					}));
				}
			}
			list2.Add(this.RestoreAdaptersWithACDU(text12));
			codeMemberMethod.Statements.Add(CodeGenHelper.Try(list.ToArray(), new CodeCatchClause[] { codeCatchClause }, list2.ToArray()));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(text6)));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000178F8 File Offset: 0x000168F8
		private void AddUpdateInsertedMethod(CodeTypeDeclaration dataComponentClass, DataTable[] orderedTables, CodeParameterDeclarationExpression dataSetPara, string dataSetStr, string resultStr, string addedRowsStr, string allAddedRowsStr)
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(int)), "UpdateInsertedRows", MemberAttributes.Private);
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalGenericType("System.Collections.Generic.List", typeof(DataRow));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, allAddedRowsStr);
			codeMemberMethod.Parameters.AddRange(new CodeParameterDeclarationExpression[] { dataSetPara, codeParameterDeclarationExpression });
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(typeof(int)), resultStr, CodeGenHelper.Primitive(0)));
			codeMemberMethod.Comments.Add(CodeGenHelper.Comment("Insert rows in top-down order.", true));
			for (int i = 0; i < orderedTables.Length; i++)
			{
				DesignTable designTable = this.dataSource.DesignTables[orderedTables[i]];
				if (this.CanAddTableAdapter(designTable))
				{
					codeMemberMethod.Statements.Add(this.AddUpdateAllTAUpdate(designTable, dataSetStr, resultStr, addedRowsStr, allAddedRowsStr, "Added", null));
				}
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(resultStr)));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00017A1C File Offset: 0x00016A1C
		private void AddUpdateDeletedMethod(CodeTypeDeclaration dataComponentClass, DataTable[] orderedTables, CodeParameterDeclarationExpression dataSetPara, string dataSetStr, string resultStr, string deletedRowsStr, string allChangedRowsStr)
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(int)), "UpdateDeletedRows", MemberAttributes.Private);
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalGenericType("System.Collections.Generic.List", typeof(DataRow));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, allChangedRowsStr);
			codeMemberMethod.Parameters.AddRange(new CodeParameterDeclarationExpression[] { dataSetPara, codeParameterDeclarationExpression });
			codeMemberMethod.Comments.Add(CodeGenHelper.Comment("Delete rows in bottom-up order.", true));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(typeof(int)), resultStr, CodeGenHelper.Primitive(0)));
			for (int i = orderedTables.Length - 1; i >= 0; i--)
			{
				DesignTable designTable = this.dataSource.DesignTables[orderedTables[i]];
				if (this.CanAddTableAdapter(designTable))
				{
					codeMemberMethod.Statements.Add(this.AddUpdateAllTAUpdate(designTable, dataSetStr, resultStr, deletedRowsStr, allChangedRowsStr, "Deleted", null));
				}
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(resultStr)));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00017B40 File Offset: 0x00016B40
		private void AddUpdateUpdatedMethod(CodeTypeDeclaration dataComponentClass, DataTable[] orderedTables, CodeParameterDeclarationExpression dataSetPara, string dataSetStr, string resultStr, string updatedRowsStr, string allChangedRowsStr, string allAddedRowsStr)
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(int)), "UpdateUpdatedRows", MemberAttributes.Private);
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalGenericType("System.Collections.Generic.List", typeof(DataRow));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, allChangedRowsStr);
			CodeParameterDeclarationExpression codeParameterDeclarationExpression2 = CodeGenHelper.ParameterDecl(codeTypeReference, allAddedRowsStr);
			codeMemberMethod.Parameters.AddRange(new CodeParameterDeclarationExpression[] { dataSetPara, codeParameterDeclarationExpression, codeParameterDeclarationExpression2 });
			codeMemberMethod.Comments.Add(CodeGenHelper.Comment("Update rows in top-down order.", true));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(typeof(int)), resultStr, CodeGenHelper.Primitive(0)));
			for (int i = 0; i < orderedTables.Length; i++)
			{
				DesignTable designTable = this.dataSource.DesignTables[orderedTables[i]];
				if (this.CanAddTableAdapter(designTable))
				{
					codeMemberMethod.Statements.Add(this.AddUpdateAllTAUpdate(designTable, dataSetStr, resultStr, updatedRowsStr, allChangedRowsStr, "ModifiedCurrent", allAddedRowsStr));
				}
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(resultStr)));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00017C78 File Offset: 0x00016C78
		private void AddRealUpdatedRowsMethod(CodeTypeDeclaration dataComponentClass, string updatedRowsStr, string allAddedRowsStr)
		{
			string text = "realUpdatedRows";
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(DataRow), 1), "GetRealUpdatedRows", MemberAttributes.Private);
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalGenericType("System.Collections.Generic.List", typeof(DataRow));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, allAddedRowsStr);
			CodeTypeReference codeTypeReference2 = CodeGenHelper.GlobalType(typeof(DataRow), 1);
			CodeParameterDeclarationExpression codeParameterDeclarationExpression2 = CodeGenHelper.ParameterDecl(codeTypeReference2, updatedRowsStr);
			codeMemberMethod.Comments.Add(CodeGenHelper.Comment("Remove inserted rows that become updated rows after calling TableAdapter.Update(inserted rows) first", true));
			codeMemberMethod.Parameters.AddRange(new CodeParameterDeclarationExpression[] { codeParameterDeclarationExpression2, codeParameterDeclarationExpression });
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.Or(CodeGenHelper.IdIsNull(CodeGenHelper.Argument(updatedRowsStr)), CodeGenHelper.Less(CodeGenHelper.Property(CodeGenHelper.Argument(updatedRowsStr), "Length"), CodeGenHelper.Primitive(1))), CodeGenHelper.Return(CodeGenHelper.Variable(updatedRowsStr))));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.Or(CodeGenHelper.IdIsNull(CodeGenHelper.Argument(allAddedRowsStr)), CodeGenHelper.Less(CodeGenHelper.Property(CodeGenHelper.Argument(allAddedRowsStr), "Count"), CodeGenHelper.Primitive(1))), CodeGenHelper.Return(CodeGenHelper.Variable(updatedRowsStr))));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(codeTypeReference, text, CodeGenHelper.New(codeTypeReference, new CodeExpression[0])));
			string text2 = "row";
			CodeStatement[] array = new CodeStatement[]
			{
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataRow)), text2, CodeGenHelper.Indexer(CodeGenHelper.Variable(updatedRowsStr), CodeGenHelper.Variable("i"))),
				CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.Argument(allAddedRowsStr), "Contains", CodeGenHelper.Variable(text2)), CodeGenHelper.Primitive(false)), CodeGenHelper.MethodCallStm(CodeGenHelper.Variable(text), "Add", CodeGenHelper.Variable(text2)))
			};
			codeMemberMethod.Statements.Add(this.GetForLoopItoCount(CodeGenHelper.Property(CodeGenHelper.Argument(updatedRowsStr), "Length"), array));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.Variable(text), "ToArray")));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00017EAC File Offset: 0x00016EAC
		private CodeStatement AddUpdateAllTAUpdate(DesignTable table, string dataSetStr, string resultStr, string updateRowsStr, string allUpdateRowsStr, string rowState, string allAddedRowsStr)
		{
			string tamadapterVarName = table.PropertyCache.TAMAdapterVarName;
			CodeStatement[] array = new CodeStatement[]
			{
				CodeGenHelper.Assign(CodeGenHelper.Variable(resultStr), CodeGenHelper.BinOperator(CodeGenHelper.Variable(resultStr), CodeBinaryOperatorType.Add, CodeGenHelper.MethodCall(CodeGenHelper.ThisField(tamadapterVarName), "Update", CodeGenHelper.Variable(updateRowsStr)))),
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(allUpdateRowsStr), "AddRange", CodeGenHelper.Variable(updateRowsStr)))
			};
			DataRelation[] selfRefRelations = TableAdapterManagerHelper.GetSelfRefRelations(table.DataTable);
			if (selfRefRelations != null && selfRefRelations.Length > 0)
			{
				bool flag = StringUtil.EqualValue("Deleted", rowState, true);
				List<CodeStatement> list = new List<CodeStatement>(array.Length + selfRefRelations.Length);
				for (int i = 0; i < selfRefRelations.Length; i++)
				{
					if (i > 0)
					{
						list.Add(new CodeCommentStatement("Note: More than one self-referenced relation found.  The generated code may not work correctly."));
					}
					list.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "SortSelfReferenceRows", new CodeExpression[]
					{
						CodeGenHelper.Variable(updateRowsStr),
						CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Argument(dataSetStr), "Relations"), CodeGenHelper.Str(selfRefRelations[i].RelationName)),
						CodeGenHelper.Primitive(flag)
					})));
				}
				list.AddRange(array);
				array = list.ToArray();
			}
			List<CodeStatement> list2 = new List<CodeStatement>(3);
			list2.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataRow), 1), updateRowsStr, CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Argument(dataSetStr), table.GeneratorTablePropName), "Select", new CodeExpression[]
			{
				CodeGenHelper.Primitive(null),
				CodeGenHelper.Primitive(null),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataViewRowState)), rowState)
			})));
			if (StringUtil.NotEmptyAfterTrim(allAddedRowsStr))
			{
				list2.Add(CodeGenHelper.Assign(CodeGenHelper.Argument(updateRowsStr), CodeGenHelper.MethodCall(CodeGenHelper.This(), "GetRealUpdatedRows", new CodeExpression[]
				{
					CodeGenHelper.Argument(updateRowsStr),
					CodeGenHelper.Argument(allAddedRowsStr)
				})));
			}
			list2.Add(CodeGenHelper.If(CodeGenHelper.And(CodeGenHelper.IdNotEQ(CodeGenHelper.Variable(updateRowsStr), CodeGenHelper.Primitive(null)), CodeGenHelper.Less(CodeGenHelper.Primitive(0), CodeGenHelper.Property(CodeGenHelper.Variable(updateRowsStr), "Length"))), array));
			return CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.ThisField(tamadapterVarName), CodeGenHelper.Primitive(null)), list2.ToArray());
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00018128 File Offset: 0x00017128
		private void AddVariableAndProperty(CodeTypeDeclaration codeType, MemberAttributes memberAttributes, CodeTypeReference propertyType, string propertyName, string variableName, bool getOnly)
		{
			codeType.Members.Add(CodeGenHelper.FieldDecl(propertyType, variableName));
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(propertyType, propertyName, memberAttributes);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.ThisField(variableName)));
			if (!getOnly)
			{
				codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.ThisField(variableName), CodeGenHelper.Argument("value")));
			}
			codeType.Members.Add(codeMemberProperty);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000181A0 File Offset: 0x000171A0
		private bool CanAddTableAdapter(DesignTable table)
		{
			if (table != null && table.HasAnyUpdateCommand)
			{
				MemberAttributes memberAttributes = ((DesignConnection)table.Connection).Modifier & MemberAttributes.AccessMask;
				if (memberAttributes == MemberAttributes.FamilyOrAssembly || memberAttributes == MemberAttributes.Assembly || memberAttributes == MemberAttributes.Public || memberAttributes == MemberAttributes.FamilyAndAssembly)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000181F4 File Offset: 0x000171F4
		private CodeStatement RestoreAdaptersWithACDU(string listStr)
		{
			CodeStatement[] array = new CodeStatement[]
			{
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataAdapter)), "adapter", CodeGenHelper.Indexer(CodeGenHelper.Variable("adapters"), CodeGenHelper.Variable("i"))),
				CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("adapter"), "AcceptChangesDuringUpdate"), CodeGenHelper.Primitive(true))
			};
			return CodeGenHelper.If(CodeGenHelper.Less(CodeGenHelper.Primitive(0), CodeGenHelper.Property(CodeGenHelper.Variable(listStr), "Count")), new CodeStatement[]
			{
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataAdapter), 1), "adapters", this.NewArray(CodeGenHelper.GlobalType(typeof(DataAdapter), 1), CodeGenHelper.Property(CodeGenHelper.Variable(listStr), "Count"))),
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(listStr), "CopyTo", CodeGenHelper.Variable("adapters"))),
				this.GetForLoopItoCount(CodeGenHelper.Property(CodeGenHelper.Variable("adapters"), "Length"), array)
			});
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00018318 File Offset: 0x00017318
		private CodeStatement HandleForEachRowInList(string listStr, string[] methods)
		{
			CodeStatement[] array = new CodeStatement[methods.Length + 1];
			array[0] = CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataRow)), "row", CodeGenHelper.Indexer(CodeGenHelper.Variable("rows"), CodeGenHelper.Variable("i")));
			for (int i = 0; i < methods.Length; i++)
			{
				array[i + 1] = CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("row"), methods[i]));
			}
			return CodeGenHelper.If(CodeGenHelper.Less(CodeGenHelper.Primitive(0), CodeGenHelper.Property(CodeGenHelper.Variable(listStr), "Count")), new CodeStatement[]
			{
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataRow), 1), "rows", this.NewArray(CodeGenHelper.GlobalType(typeof(DataRow), 1), CodeGenHelper.Property(CodeGenHelper.Variable(listStr), "Count"))),
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(listStr), "CopyTo", CodeGenHelper.Variable("rows"))),
				this.GetForLoopItoCount(CodeGenHelper.Property(CodeGenHelper.Variable("rows"), "Length"), array)
			});
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00018441 File Offset: 0x00017441
		private CodeStatement GetForLoopItoCount(CodeExpression countExp, CodeStatement[] forStms)
		{
			return this.GetForLoopItoCount("i", countExp, forStms);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00018450 File Offset: 0x00017450
		private CodeStatement GetForLoopItoCount(string iStr, CodeExpression countExp, CodeStatement[] forStms)
		{
			CodeStatement codeStatement = CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), iStr, CodeGenHelper.Primitive(0));
			CodeStatement codeStatement2 = CodeGenHelper.Assign(CodeGenHelper.Variable(iStr), CodeGenHelper.BinOperator(CodeGenHelper.Variable(iStr), CodeBinaryOperatorType.Add, CodeGenHelper.Primitive(1)));
			CodeExpression codeExpression = CodeGenHelper.Less(CodeGenHelper.Variable(iStr), countExp);
			return CodeGenHelper.ForLoop(codeStatement, codeExpression, codeStatement2, forStms);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000184B7 File Offset: 0x000174B7
		private CodeExpression NewArray(CodeTypeReference type, CodeExpression size)
		{
			return new CodeArrayCreateExpression(type, size);
		}

		// Token: 0x04000C1A RID: 3098
		private const string adapterPropertyEditor = "Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor";

		// Token: 0x04000C1B RID: 3099
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000C1C RID: 3100
		private DesignDataSource dataSource;

		// Token: 0x04000C1D RID: 3101
		private CodeTypeDeclaration dataSourceType;

		// Token: 0x04000C1E RID: 3102
		private TableAdapterManagerNameHandler nameHandler;
	}
}
