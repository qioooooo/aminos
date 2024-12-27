using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Data.Design
{
	// Token: 0x020000A4 RID: 164
	internal class FunctionGenerator : QueryGeneratorBase
	{
		// Token: 0x060007AD RID: 1965 RVA: 0x0001129A File Offset: 0x0001029A
		internal FunctionGenerator(TypedDataSourceCodeGenerator codeGenerator)
			: base(codeGenerator)
		{
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x000112A4 File Offset: 0x000102A4
		internal override CodeMemberMethod Generate()
		{
			if (this.methodSource == null)
			{
				throw new InternalException("MethodSource should not be null.");
			}
			this.activeCommand = this.methodSource.GetActiveCommand();
			if (this.activeCommand == null)
			{
				return null;
			}
			this.methodAttributes = base.MethodSource.Modifier | MemberAttributes.Overloaded;
			if (this.codeProvider == null)
			{
				this.codeProvider = this.codeGenerator.CodeProvider;
			}
			this.nameHandler = new GenericNameHandler(new string[]
			{
				base.MethodName,
				QueryGeneratorBase.returnVariableName,
				QueryGeneratorBase.commandVariableName
			}, this.codeProvider);
			return this.GenerateInternal();
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00011348 File Offset: 0x00010348
		private CodeMemberMethod GenerateInternal()
		{
			DesignParameter returnParameter = base.GetReturnParameter(this.activeCommand);
			CodeTypeReference codeTypeReference;
			if (this.methodSource.QueryType == QueryType.Scalar)
			{
				this.returnType = this.methodSource.ScalarCallRetval;
				if (this.returnType.IsValueType)
				{
					codeTypeReference = CodeGenHelper.NullableType(this.returnType);
				}
				else
				{
					codeTypeReference = CodeGenHelper.Type(this.returnType);
				}
			}
			else if (this.methodSource.DbObjectType == DbObjectType.Function && returnParameter != null)
			{
				this.returnType = base.GetParameterUrtType(returnParameter);
				if (returnParameter.AllowDbNull && this.returnType.IsValueType)
				{
					codeTypeReference = CodeGenHelper.NullableType(this.returnType);
				}
				else
				{
					codeTypeReference = CodeGenHelper.Type(this.returnType);
				}
			}
			else
			{
				this.returnType = typeof(int);
				codeTypeReference = CodeGenHelper.Type(this.returnType);
			}
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(codeTypeReference, base.MethodName, this.methodAttributes);
			codeMemberMethod.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(HelpKeywordAttribute).FullName, CodeGenHelper.Str("vs.data.TableAdapter")));
			this.AddParametersToMethod(codeMemberMethod);
			if (this.declarationOnly)
			{
				base.AddThrowsClauseIfNeeded(codeMemberMethod);
				return codeMemberMethod;
			}
			this.AddCustomAttributesToMethod(codeMemberMethod);
			if (this.AddStatementsToMethod(codeMemberMethod))
			{
				return codeMemberMethod;
			}
			return null;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00011484 File Offset: 0x00010484
		private void AddParametersToMethod(CodeMemberMethod dbMethod)
		{
			if (this.activeCommand.Parameters == null)
			{
				return;
			}
			DesignConnection designConnection = (DesignConnection)this.methodSource.Connection;
			if (designConnection == null)
			{
				throw new InternalException("Connection for query '" + this.methodSource.Name + "' is null.");
			}
			string parameterPrefix = designConnection.ParameterPrefix;
			foreach (object obj in this.activeCommand.Parameters)
			{
				DesignParameter designParameter = (DesignParameter)obj;
				if (designParameter.Direction != ParameterDirection.ReturnValue)
				{
					Type parameterUrtType = base.GetParameterUrtType(designParameter);
					string text = this.nameHandler.AddParameterNameToList(designParameter.ParameterName, parameterPrefix);
					CodeTypeReference codeTypeReference;
					if (designParameter.AllowDbNull && parameterUrtType.IsValueType)
					{
						codeTypeReference = CodeGenHelper.NullableType(parameterUrtType);
					}
					else
					{
						codeTypeReference = CodeGenHelper.Type(parameterUrtType);
					}
					CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, text);
					codeParameterDeclarationExpression.Direction = CodeGenHelper.ParameterDirectionToFieldDirection(designParameter.Direction);
					dbMethod.Parameters.Add(codeParameterDeclarationExpression);
				}
			}
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x000115AC File Offset: 0x000105AC
		private bool AddStatementsToMethod(CodeMemberMethod dbMethod)
		{
			return this.AddSetCommandStatements(dbMethod.Statements) && this.AddSetParametersStatements(dbMethod.Statements) && this.AddExecuteCommandStatements(dbMethod.Statements) && this.AddSetReturnParamValuesStatements(dbMethod.Statements) && this.AddReturnStatements(dbMethod.Statements);
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x00011618 File Offset: 0x00010618
		private bool AddSetCommandStatements(IList statements)
		{
			Type type = base.ProviderFactory.CreateCommand().GetType();
			CodeExpression codeExpression = CodeGenHelper.ArrayIndexer(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), CodeGenHelper.Primitive(base.CommandIndex));
			if (base.IsFunctionsDataComponent)
			{
				codeExpression = CodeGenHelper.Cast(CodeGenHelper.GlobalType(type), codeExpression);
			}
			statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(type), QueryGeneratorBase.commandVariableName, codeExpression));
			return true;
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001168C File Offset: 0x0001068C
		private bool AddSetParametersStatements(IList statements)
		{
			int num = 0;
			if (this.activeCommand.Parameters != null)
			{
				num = this.activeCommand.Parameters.Count;
			}
			for (int i = 0; i < num; i++)
			{
				DesignParameter designParameter = this.activeCommand.Parameters[i];
				if (designParameter == null)
				{
					throw new DataSourceGeneratorException("Parameter type is not DesignParameter.");
				}
				if (designParameter.Direction == ParameterDirection.Input || designParameter.Direction == ParameterDirection.InputOutput)
				{
					string nameFromList = this.nameHandler.GetNameFromList(designParameter.ParameterName);
					base.AddSetParameterStatements(designParameter, nameFromList, CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), i, statements);
				}
			}
			return true;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00011720 File Offset: 0x00010720
		private bool AddExecuteCommandStatements(IList statements)
		{
			CodeStatement[] array = new CodeStatement[1];
			CodeStatement[] array2 = new CodeStatement[1];
			statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(ConnectionState)), this.nameHandler.AddNameToList("previousConnectionState"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "Connection"), "State")));
			statements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.BitwiseAnd(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "Connection"), "State"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Open")), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Open")), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "Connection"), "Open"))));
			if (this.methodSource.QueryType == QueryType.Scalar)
			{
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(object)), QueryGeneratorBase.returnVariableName));
				array[0] = CodeGenHelper.Assign(CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName), CodeGenHelper.MethodCall(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "ExecuteScalar", new CodeExpression[0]));
			}
			else if (this.methodSource.DbObjectType == DbObjectType.Function && base.GetReturnParameterPosition(this.activeCommand) >= 0)
			{
				array[0] = CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "ExecuteNonQuery", new CodeExpression[0]));
			}
			else
			{
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), QueryGeneratorBase.returnVariableName));
				array[0] = CodeGenHelper.Assign(CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName), CodeGenHelper.MethodCall(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "ExecuteNonQuery", new CodeExpression[0]));
			}
			array2[0] = CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Variable(this.nameHandler.GetNameFromList("previousConnectionState")), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Closed")), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "Connection"), "Close")));
			statements.Add(CodeGenHelper.Try(array, new CodeCatchClause[0], array2));
			return true;
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001196C File Offset: 0x0001096C
		protected bool AddSetReturnParamValuesStatements(IList statements)
		{
			return base.AddSetReturnParamValuesStatements(statements, CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName));
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00011980 File Offset: 0x00010980
		private bool AddReturnStatements(IList statements)
		{
			int returnParameterPosition = base.GetReturnParameterPosition(this.activeCommand);
			if (this.methodSource.DbObjectType == DbObjectType.Function && this.methodSource.QueryType != QueryType.Scalar && returnParameterPosition >= 0)
			{
				DesignParameter designParameter = this.activeCommand.Parameters[returnParameterPosition];
				Type parameterUrtType = base.GetParameterUrtType(designParameter);
				CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Variable(QueryGeneratorBase.commandVariableName), "Parameters"), CodeGenHelper.Primitive(returnParameterPosition)), "Value");
				CodeExpression codeExpression2 = CodeGenHelper.GenerateDbNullCheck(codeExpression);
				CodeExpression codeExpression3 = CodeGenHelper.GenerateNullExpression(parameterUrtType);
				CodeStatement codeStatement;
				if (codeExpression3 == null)
				{
					if (designParameter.AllowDbNull && parameterUrtType.IsValueType)
					{
						codeStatement = CodeGenHelper.Return(CodeGenHelper.New(CodeGenHelper.NullableType(parameterUrtType), new CodeExpression[0]));
					}
					else if (designParameter.AllowDbNull && !parameterUrtType.IsValueType)
					{
						codeStatement = CodeGenHelper.Return(CodeGenHelper.Primitive(null));
					}
					else
					{
						codeStatement = CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(StrongTypingException)), SR.GetString("CG_ParameterIsDBNull", new object[] { this.activeCommand.Parameters[returnParameterPosition].ParameterName }), CodeGenHelper.Primitive(null));
					}
				}
				else
				{
					codeStatement = CodeGenHelper.Return(codeExpression3);
				}
				CodeStatement codeStatement2;
				if (designParameter.AllowDbNull && parameterUrtType.IsValueType)
				{
					codeStatement2 = CodeGenHelper.Return(CodeGenHelper.New(CodeGenHelper.NullableType(parameterUrtType), new CodeExpression[] { CodeGenHelper.Cast(CodeGenHelper.GlobalType(parameterUrtType), codeExpression) }));
				}
				else
				{
					CodeExpression codeExpression4 = CodeGenHelper.GenerateConvertExpression(codeExpression, typeof(object), parameterUrtType);
					codeStatement2 = CodeGenHelper.Return(codeExpression4);
				}
				statements.Add(CodeGenHelper.If(codeExpression2, codeStatement, codeStatement2));
			}
			else if (this.methodSource.QueryType == QueryType.Scalar)
			{
				CodeExpression codeExpression5 = CodeGenHelper.GenerateDbNullCheck(CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName));
				CodeStatement codeStatement3;
				CodeStatement codeStatement4;
				if (this.returnType.IsValueType)
				{
					codeStatement3 = CodeGenHelper.Return(CodeGenHelper.New(CodeGenHelper.NullableType(this.returnType), new CodeExpression[0]));
					codeStatement4 = CodeGenHelper.Return(CodeGenHelper.New(CodeGenHelper.NullableType(this.returnType), new CodeExpression[] { CodeGenHelper.Cast(CodeGenHelper.GlobalType(this.returnType), CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName)) }));
				}
				else
				{
					codeStatement3 = CodeGenHelper.Return(CodeGenHelper.Primitive(null));
					codeStatement4 = CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.GlobalType(this.returnType), CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName)));
				}
				statements.Add(CodeGenHelper.If(codeExpression5, codeStatement3, codeStatement4));
			}
			else
			{
				statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName)));
			}
			return true;
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00011C24 File Offset: 0x00010C24
		private void AddCustomAttributesToMethod(CodeMemberMethod dbMethod)
		{
			if (this.methodSource.EnableWebMethods)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Web.Services.WebMethod");
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Description", CodeGenHelper.Str(this.methodSource.WebMethodDescription)));
				dbMethod.CustomAttributes.Add(codeAttributeDeclaration);
			}
			DataObjectMethodType dataObjectMethodType = DataObjectMethodType.Select;
			if (this.methodSource.CommandOperation == CommandOperation.Update)
			{
				dataObjectMethodType = DataObjectMethodType.Update;
			}
			else if (this.methodSource.CommandOperation == CommandOperation.Delete)
			{
				dataObjectMethodType = DataObjectMethodType.Delete;
			}
			else if (this.methodSource.CommandOperation == CommandOperation.Insert)
			{
				dataObjectMethodType = DataObjectMethodType.Insert;
			}
			if (dataObjectMethodType != DataObjectMethodType.Select)
			{
				dbMethod.CustomAttributes.Add(new CodeAttributeDeclaration(CodeGenHelper.GlobalType(typeof(DataObjectMethodAttribute)), new CodeAttributeArgument[]
				{
					new CodeAttributeArgument(CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataObjectMethodType)), dataObjectMethodType.ToString())),
					new CodeAttributeArgument(CodeGenHelper.Primitive(false))
				}));
			}
		}
	}
}
