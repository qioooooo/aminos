using System;
using System.CodeDom;
using System.Collections;
using System.Data.Common;

namespace System.Data.Design
{
	// Token: 0x02000070 RID: 112
	internal sealed class DataComponentMethodGenerator
	{
		// Token: 0x060004BB RID: 1211 RVA: 0x0000420D File Offset: 0x0000320D
		internal DataComponentMethodGenerator(TypedDataSourceCodeGenerator codeGenerator, DesignTable designTable, bool generateHierarchicalUpdate)
		{
			this.generateHierarchicalUpdate = generateHierarchicalUpdate;
			this.codeGenerator = codeGenerator;
			this.designTable = designTable;
			if (designTable.Connection != null)
			{
				this.providerFactory = ProviderManager.GetFactory(designTable.Connection.Provider);
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00004248 File Offset: 0x00003248
		internal void AddMethods(CodeTypeDeclaration dataComponentClass, bool isFunctionsDataComponent)
		{
			if (dataComponentClass == null)
			{
				throw new InternalException("dataComponent CodeTypeDeclaration should not be null.");
			}
			if (isFunctionsDataComponent)
			{
				this.AddCommandCollectionMembers(dataComponentClass, true);
				this.AddInitCommandCollection(dataComponentClass, true);
				return;
			}
			if (this.designTable.Connection == null || this.providerFactory == null)
			{
				return;
			}
			this.AddConstructor(dataComponentClass);
			this.AddAdapterMembers(dataComponentClass);
			this.AddInitAdapter(dataComponentClass);
			this.AddConnectionMembers(dataComponentClass);
			this.AddInitConnection(dataComponentClass);
			if (this.generateHierarchicalUpdate)
			{
				this.AddTransactionMembers(dataComponentClass);
			}
			this.AddCommandCollectionMembers(dataComponentClass, false);
			this.AddInitCommandCollection(dataComponentClass, false);
			this.AddClearBeforeFillMembers(dataComponentClass);
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x000042D8 File Offset: 0x000032D8
		private void AddConstructor(CodeTypeDeclaration dataComponentClass)
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor(MemberAttributes.Public);
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.ClearBeforeFillPropertyName), CodeGenHelper.Primitive(true)));
			dataComponentClass.Members.Add(codeConstructor);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00004328 File Offset: 0x00003328
		private void AddAdapterMembers(CodeTypeDeclaration dataComponentClass)
		{
			Type type = this.providerFactory.CreateDataAdapter().GetType();
			CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(type), DataComponentNameHandler.AdapterVariableName);
			codeMemberField.UserData.Add("WithEvents", true);
			dataComponentClass.Members.Add(codeMemberField);
			CodeMemberProperty codeMemberProperty;
			if (this.generateHierarchicalUpdate)
			{
				codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(type), DataComponentNameHandler.AdapterPropertyName, (MemberAttributes)16386);
			}
			else
			{
				codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(type), DataComponentNameHandler.AdapterPropertyName, (MemberAttributes)20482);
			}
			codeMemberProperty.GetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdEQ(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName), CodeGenHelper.Primitive(null)), new CodeStatement[] { CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), DataComponentNameHandler.InitAdapter, new CodeExpression[0])) }));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName)));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00004430 File Offset: 0x00003430
		private void AddInitAdapter(CodeTypeDeclaration dataComponentClass)
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), DataComponentNameHandler.InitAdapter, (MemberAttributes)20482);
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName), CodeGenHelper.New(CodeGenHelper.GlobalType(this.providerFactory.CreateDataAdapter().GetType()), new CodeExpression[0])));
			if (this.designTable.Mappings != null && this.designTable.Mappings.Count > 0)
			{
				codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataTableMapping)), "tableMapping", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(DataTableMapping)), new CodeExpression[0])));
				codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("tableMapping"), "SourceTable"), CodeGenHelper.Str("Table")));
				codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("tableMapping"), "DataSetTable"), CodeGenHelper.Str(this.designTable.Name)));
				foreach (object obj in this.designTable.Mappings)
				{
					DataColumnMapping dataColumnMapping = (DataColumnMapping)obj;
					codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable("tableMapping"), "ColumnMappings"), "Add", new CodeExpression[]
					{
						CodeGenHelper.Str(dataColumnMapping.SourceColumn),
						CodeGenHelper.Str(dataColumnMapping.DataSetColumn)
					})));
				}
				codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName), "TableMappings"), "Add", new CodeExpression[] { CodeGenHelper.Variable("tableMapping") })));
			}
			this.AddInitAdapterCommands(codeMemberMethod);
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00004668 File Offset: 0x00003668
		private void AddCommandCollectionMembers(CodeTypeDeclaration dataComponentClass, bool isFunctionsDataComponent)
		{
			Type type;
			if (isFunctionsDataComponent)
			{
				type = typeof(IDbCommand);
			}
			else
			{
				type = this.providerFactory.CreateCommand().GetType();
			}
			dataComponentClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(type, 1), DataComponentNameHandler.SelectCmdCollectionVariableName));
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(type, 1), DataComponentNameHandler.SelectCmdCollectionPropertyName, (MemberAttributes)12290);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdEQ(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionVariableName), CodeGenHelper.Primitive(null)), new CodeStatement[] { CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), DataComponentNameHandler.InitCmdCollection, new CodeExpression[0])) }));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionVariableName)));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000474C File Offset: 0x0000374C
		private void AddInitCommandCollection(CodeTypeDeclaration dataComponentClass, bool isFunctionsDataComponent)
		{
			int num = this.designTable.Sources.Count;
			if (!isFunctionsDataComponent)
			{
				num++;
			}
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), DataComponentNameHandler.InitCmdCollection, (MemberAttributes)20482);
			Type type;
			if (isFunctionsDataComponent)
			{
				type = typeof(IDbCommand);
			}
			else
			{
				type = this.providerFactory.CreateCommand().GetType();
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionVariableName), CodeGenHelper.NewArray(CodeGenHelper.GlobalType(type), num)));
			if (!isFunctionsDataComponent && this.designTable.MainSource != null && this.designTable.MainSource is DbSource)
			{
				DbSource dbSource = (DbSource)this.designTable.MainSource;
				DbSourceCommand activeCommand = dbSource.GetActiveCommand();
				if (activeCommand != null)
				{
					CodeExpression codeExpression = CodeGenHelper.ArrayIndexer(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionVariableName), CodeGenHelper.Primitive(0));
					this.AddCommandInitStatements(codeMemberMethod.Statements, codeExpression, activeCommand, this.providerFactory, isFunctionsDataComponent);
				}
			}
			if (this.designTable.Sources != null)
			{
				int num2 = 0;
				if (isFunctionsDataComponent)
				{
					num2--;
				}
				foreach (object obj in this.designTable.Sources)
				{
					Source source = (Source)obj;
					DbSource dbSource2 = source as DbSource;
					num2++;
					if (dbSource2 != null)
					{
						DbProviderFactory factory = this.providerFactory;
						if (dbSource2.Connection != null)
						{
							factory = ProviderManager.GetFactory(dbSource2.Connection.Provider);
						}
						if (factory != null)
						{
							DbSourceCommand activeCommand2 = dbSource2.GetActiveCommand();
							if (activeCommand2 != null)
							{
								CodeExpression codeExpression2 = CodeGenHelper.ArrayIndexer(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionVariableName), CodeGenHelper.Primitive(num2));
								this.AddCommandInitStatements(codeMemberMethod.Statements, codeExpression2, activeCommand2, factory, isFunctionsDataComponent);
							}
						}
					}
				}
			}
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00004954 File Offset: 0x00003954
		private void AddConnectionMembers(CodeTypeDeclaration dataComponentClass)
		{
			Type type = this.providerFactory.CreateConnection().GetType();
			MemberAttributes modifier = ((DesignConnection)this.designTable.Connection).Modifier;
			dataComponentClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(type), DataComponentNameHandler.DefaultConnectionVariableName));
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(type), DataComponentNameHandler.DefaultConnectionPropertyName, modifier | MemberAttributes.Final);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdEQ(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.DefaultConnectionVariableName), CodeGenHelper.Primitive(null)), new CodeStatement[] { CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), DataComponentNameHandler.InitConnection, new CodeExpression[0])) }));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.DefaultConnectionVariableName)));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.DefaultConnectionVariableName), CodeGenHelper.Argument("value")));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "InsertCommand"), CodeGenHelper.Primitive(null)), CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "InsertCommand"), "Connection"), CodeGenHelper.Argument("value"))));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "DeleteCommand"), CodeGenHelper.Primitive(null)), CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "DeleteCommand"), "Connection"), CodeGenHelper.Argument("value"))));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "UpdateCommand"), CodeGenHelper.Primitive(null)), CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "UpdateCommand"), "Connection"), CodeGenHelper.Argument("value"))));
			int count = this.designTable.Sources.Count;
			CodeStatement codeStatement = CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), "i", CodeGenHelper.Primitive(0));
			CodeExpression codeExpression = CodeGenHelper.Less(CodeGenHelper.Variable("i"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), "Length"));
			CodeStatement codeStatement2 = CodeGenHelper.Assign(CodeGenHelper.Variable("i"), CodeGenHelper.BinOperator(CodeGenHelper.Variable("i"), CodeBinaryOperatorType.Add, CodeGenHelper.Primitive(1)));
			CodeExpression codeExpression2 = CodeGenHelper.Property(CodeGenHelper.Cast(CodeGenHelper.GlobalType(this.providerFactory.CreateCommand().GetType()), CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), CodeGenHelper.Variable("i"))), "Connection");
			CodeExpression codeExpression3 = CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), CodeGenHelper.Variable("i"));
			CodeStatement codeStatement3 = CodeGenHelper.If(CodeGenHelper.IdNotEQ(codeExpression3, CodeGenHelper.Primitive(null)), CodeGenHelper.Assign(codeExpression2, CodeGenHelper.Argument("value")));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.ForLoop(codeStatement, codeExpression, codeStatement2, new CodeStatement[] { codeStatement3 }));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00004CD4 File Offset: 0x00003CD4
		private void AddInitConnection(CodeTypeDeclaration dataComponentClass)
		{
			IDesignConnection connection = this.designTable.Connection;
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), DataComponentNameHandler.InitConnection, (MemberAttributes)20482);
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.DefaultConnectionVariableName), CodeGenHelper.New(CodeGenHelper.GlobalType(this.providerFactory.CreateConnection().GetType()), new CodeExpression[0])));
			CodeExpression codeExpression;
			if (connection.PropertyReference == null)
			{
				codeExpression = CodeGenHelper.Str(connection.ConnectionStringObject.ToFullString());
			}
			else
			{
				codeExpression = connection.PropertyReference;
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.DefaultConnectionVariableName), "ConnectionString"), codeExpression));
			dataComponentClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00004DA8 File Offset: 0x00003DA8
		private void AddTransactionMembers(CodeTypeDeclaration dataComponentClass)
		{
			Type transactionType = this.designTable.PropertyCache.TransactionType;
			if (transactionType == null)
			{
				return;
			}
			CodeTypeReference codeTypeReference = CodeGenHelper.GlobalType(transactionType);
			dataComponentClass.Members.Add(CodeGenHelper.FieldDecl(codeTypeReference, DataComponentNameHandler.TransactionVariableName));
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(codeTypeReference, DataComponentNameHandler.TransactionPropertyName, (MemberAttributes)4098);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.TransactionVariableName)));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.TransactionVariableName), CodeGenHelper.Argument("value")));
			CodeStatement codeStatement = CodeGenHelper.VariableDecl(CodeGenHelper.Type(typeof(int)), "i", CodeGenHelper.Primitive(0));
			CodeExpression codeExpression = CodeGenHelper.Less(CodeGenHelper.Variable("i"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), "Length"));
			CodeStatement codeStatement2 = CodeGenHelper.Assign(CodeGenHelper.Variable("i"), CodeGenHelper.BinOperator(CodeGenHelper.Variable("i"), CodeBinaryOperatorType.Add, CodeGenHelper.Primitive(1)));
			CodeExpression codeExpression2 = CodeGenHelper.Property(CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), CodeGenHelper.Variable("i")), "Transaction");
			CodeExpression codeExpression3 = CodeGenHelper.Variable("oldTransaction");
			CodeExpression codeExpression4 = CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.TransactionVariableName);
			CodeStatement codeStatement3 = this.GenerateSetTransactionStmt(codeExpression2, codeExpression3, codeExpression4);
			codeMemberProperty.SetStatements.Add(CodeGenHelper.ForLoop(codeStatement, codeExpression, codeStatement2, new CodeStatement[] { codeStatement3 }));
			CodeExpression codeExpression5 = CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName);
			CodeExpression codeExpression6 = CodeGenHelper.Property(codeExpression5, "DeleteCommand");
			codeExpression2 = CodeGenHelper.Property(codeExpression6, "Transaction");
			codeMemberProperty.SetStatements.Add(CodeGenHelper.If(CodeGenHelper.And(CodeGenHelper.IdNotEQ(codeExpression5, CodeGenHelper.Primitive(null)), CodeGenHelper.IdNotEQ(codeExpression6, CodeGenHelper.Primitive(null))), this.GenerateSetTransactionStmt(codeExpression2, codeExpression3, codeExpression4)));
			codeExpression6 = CodeGenHelper.Property(codeExpression5, "InsertCommand");
			codeExpression2 = CodeGenHelper.Property(codeExpression6, "Transaction");
			codeMemberProperty.SetStatements.Add(CodeGenHelper.If(CodeGenHelper.And(CodeGenHelper.IdNotEQ(codeExpression5, CodeGenHelper.Primitive(null)), CodeGenHelper.IdNotEQ(codeExpression6, CodeGenHelper.Primitive(null))), this.GenerateSetTransactionStmt(codeExpression2, codeExpression3, codeExpression4)));
			codeExpression6 = CodeGenHelper.Property(codeExpression5, "UpdateCommand");
			codeExpression2 = CodeGenHelper.Property(codeExpression6, "Transaction");
			codeMemberProperty.SetStatements.Add(CodeGenHelper.If(CodeGenHelper.And(CodeGenHelper.IdNotEQ(codeExpression5, CodeGenHelper.Primitive(null)), CodeGenHelper.IdNotEQ(codeExpression6, CodeGenHelper.Primitive(null))), this.GenerateSetTransactionStmt(codeExpression2, codeExpression3, codeExpression4)));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000505F File Offset: 0x0000405F
		private CodeStatement GenerateSetTransactionStmt(CodeExpression transaction, CodeExpression oldTransaction, CodeExpression newTransaction)
		{
			return CodeGenHelper.Assign(transaction, newTransaction);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00005068 File Offset: 0x00004068
		private void AddClearBeforeFillMembers(CodeTypeDeclaration dataComponentClass)
		{
			dataComponentClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(bool)), DataComponentNameHandler.ClearBeforeFillVariableName));
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(bool)), DataComponentNameHandler.ClearBeforeFillPropertyName, (MemberAttributes)24578);
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.ClearBeforeFillVariableName)));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.ClearBeforeFillVariableName), CodeGenHelper.Argument("value")));
			dataComponentClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00005110 File Offset: 0x00004110
		private void AddInitAdapterCommands(CodeMemberMethod method)
		{
			if (this.designTable.DeleteCommand != null)
			{
				CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName), "DeleteCommand");
				this.AddCommandInitStatements(method.Statements, codeExpression, this.designTable.DeleteCommand, this.providerFactory, false);
			}
			if (this.designTable.InsertCommand != null)
			{
				CodeExpression codeExpression2 = CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName), "InsertCommand");
				this.AddCommandInitStatements(method.Statements, codeExpression2, this.designTable.InsertCommand, this.providerFactory, false);
			}
			if (this.designTable.UpdateCommand != null)
			{
				CodeExpression codeExpression3 = CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), DataComponentNameHandler.AdapterVariableName), "UpdateCommand");
				this.AddCommandInitStatements(method.Statements, codeExpression3, this.designTable.UpdateCommand, this.providerFactory, false);
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x000051F0 File Offset: 0x000041F0
		private void AddCommandInitStatements(IList statements, CodeExpression commandExpression, DbSourceCommand command, DbProviderFactory currentFactory, bool isFunctionsDataComponent)
		{
			if (statements == null || commandExpression == null || command == null)
			{
				throw new InternalException("Argument should not be null.");
			}
			Type type = currentFactory.CreateParameter().GetType();
			Type type2 = currentFactory.CreateCommand().GetType();
			CodeExpression codeExpression = null;
			statements.Add(CodeGenHelper.Assign(commandExpression, CodeGenHelper.New(CodeGenHelper.GlobalType(type2), new CodeExpression[0])));
			if (isFunctionsDataComponent)
			{
				commandExpression = CodeGenHelper.Cast(CodeGenHelper.GlobalType(type2), commandExpression);
			}
			if (((DbSource)command.Parent).Connection == null || (this.designTable.Connection != null && this.designTable.Connection == ((DbSource)command.Parent).Connection))
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(commandExpression, "Connection"), CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.DefaultConnectionPropertyName)));
			}
			else
			{
				Type type3 = currentFactory.CreateConnection().GetType();
				IDesignConnection connection = ((DbSource)command.Parent).Connection;
				CodeExpression codeExpression2;
				if (connection.PropertyReference == null)
				{
					codeExpression2 = CodeGenHelper.Str(connection.ConnectionStringObject.ToFullString());
				}
				else
				{
					codeExpression2 = connection.PropertyReference;
				}
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(commandExpression, "Connection"), CodeGenHelper.New(CodeGenHelper.GlobalType(type3), new CodeExpression[] { codeExpression2 })));
			}
			statements.Add(QueryGeneratorBase.SetCommandTextStatement(commandExpression, command.CommandText));
			statements.Add(QueryGeneratorBase.SetCommandTypeStatement(commandExpression, command.CommandType));
			if (command.Parameters != null)
			{
				foreach (object obj in command.Parameters)
				{
					DesignParameter designParameter = (DesignParameter)obj;
					codeExpression = QueryGeneratorBase.AddNewParameterStatements(designParameter, type, currentFactory, statements, codeExpression);
					statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(commandExpression, "Parameters"), "Add", new CodeExpression[] { codeExpression })));
				}
			}
		}

		// Token: 0x04000A9B RID: 2715
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000A9C RID: 2716
		private DesignTable designTable;

		// Token: 0x04000A9D RID: 2717
		private DbProviderFactory providerFactory;

		// Token: 0x04000A9E RID: 2718
		private bool generateHierarchicalUpdate;
	}
}
