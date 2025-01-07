using System;
using System.CodeDom;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;

namespace System.Data.Design
{
	internal class QueryGenerator : QueryGeneratorBase
	{
		internal QueryGenerator(TypedDataSourceCodeGenerator codeGenerator)
			: base(codeGenerator)
		{
		}

		internal override CodeMemberMethod Generate()
		{
			if (this.methodSource == null)
			{
				throw new InternalException("MethodSource should not be null.");
			}
			if (StringUtil.Empty(base.ContainerParameterName))
			{
				throw new InternalException("ContainerParameterName should not be empty.");
			}
			if (this.methodSource.SelectCommand == null)
			{
				this.codeGenerator.ProblemList.Add(new DSGeneratorProblem(SR.GetString("CG_MainSelectCommandNotSet", new object[] { base.DesignTable.Name }), ProblemSeverity.NonFatalError, this.methodSource));
				return null;
			}
			this.activeCommand = this.methodSource.SelectCommand;
			this.methodAttributes = MemberAttributes.Overloaded;
			if (this.getMethod)
			{
				this.methodAttributes |= base.MethodSource.GetMethodModifier;
			}
			else
			{
				this.methodAttributes |= base.MethodSource.Modifier;
			}
			if (this.codeProvider == null)
			{
				this.codeProvider = this.codeGenerator.CodeProvider;
			}
			this.nameHandler = new GenericNameHandler(new string[]
			{
				base.MethodName,
				QueryGeneratorBase.returnVariableName
			}, this.codeProvider);
			return this.GenerateInternal();
		}

		private CodeMemberMethod GenerateInternal()
		{
			this.returnType = typeof(int);
			CodeMemberMethod codeMemberMethod;
			if (this.getMethod)
			{
				codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(base.ContainerParameterTypeName), base.MethodName, this.methodAttributes);
			}
			else
			{
				codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(this.returnType), base.MethodName, this.methodAttributes);
			}
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

		private void AddParametersToMethod(CodeMemberMethod dbMethod)
		{
			if (!this.getMethod)
			{
				string text = this.nameHandler.AddNameToList(base.ContainerParameterName);
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(CodeGenHelper.Type(base.ContainerParameterTypeName), text);
				dbMethod.Parameters.Add(codeParameterDeclarationExpression);
			}
			if (base.GeneratePagingMethod)
			{
				string text2 = this.nameHandler.AddNameToList(QueryGeneratorBase.startRecordParameterName);
				CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(int)), text2);
				dbMethod.Parameters.Add(codeParameterDeclarationExpression);
				string text3 = this.nameHandler.AddNameToList(QueryGeneratorBase.maxRecordsParameterName);
				codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(int)), text3);
				dbMethod.Parameters.Add(codeParameterDeclarationExpression);
			}
			if (this.activeCommand.Parameters == null)
			{
				return;
			}
			DesignConnection designConnection = (DesignConnection)this.methodSource.Connection;
			if (designConnection == null)
			{
				throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Connection for query {0} is null.", new object[] { this.methodSource.Name }));
			}
			string parameterPrefix = designConnection.ParameterPrefix;
			foreach (object obj in this.activeCommand.Parameters)
			{
				DesignParameter designParameter = (DesignParameter)obj;
				if (designParameter.Direction != ParameterDirection.ReturnValue)
				{
					Type parameterUrtType = base.GetParameterUrtType(designParameter);
					string text4 = this.nameHandler.AddParameterNameToList(designParameter.ParameterName, parameterPrefix);
					CodeTypeReference codeTypeReference;
					if (designParameter.AllowDbNull && parameterUrtType.IsValueType)
					{
						codeTypeReference = CodeGenHelper.NullableType(parameterUrtType);
					}
					else
					{
						codeTypeReference = CodeGenHelper.Type(parameterUrtType);
					}
					CodeParameterDeclarationExpression codeParameterDeclarationExpression = CodeGenHelper.ParameterDecl(codeTypeReference, text4);
					codeParameterDeclarationExpression.Direction = CodeGenHelper.ParameterDirectionToFieldDirection(designParameter.Direction);
					dbMethod.Parameters.Add(codeParameterDeclarationExpression);
				}
			}
		}

		private bool AddStatementsToMethod(CodeMemberMethod dbMethod)
		{
			if (!this.AddSetCommandStatements(dbMethod.Statements))
			{
				return false;
			}
			if (!this.AddSetParametersStatements(dbMethod.Statements))
			{
				return false;
			}
			if (!this.AddClearStatements(dbMethod.Statements))
			{
				return false;
			}
			bool flag;
			if (base.GeneratePagingMethod)
			{
				flag = this.AddExecuteCommandStatementsForPaging(dbMethod.Statements);
			}
			else
			{
				flag = this.AddExecuteCommandStatements(dbMethod.Statements);
			}
			return flag && this.AddSetReturnParamValuesStatements(dbMethod.Statements) && this.AddReturnStatements(dbMethod.Statements);
		}

		private bool AddSetCommandStatements(IList statements)
		{
			base.ProviderFactory.CreateCommand().GetType();
			statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "SelectCommand"), CodeGenHelper.ArrayIndexer(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.SelectCmdCollectionPropertyName), CodeGenHelper.Primitive(base.CommandIndex))));
			return true;
		}

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
					CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "SelectCommand");
					base.AddSetParameterStatements(designParameter, nameFromList, codeExpression, i, statements);
				}
			}
			return true;
		}

		private bool AddClearStatements(IList statements)
		{
			if (!this.getMethod)
			{
				CodeStatement codeStatement;
				if (this.containerParamType == typeof(DataTable))
				{
					codeStatement = CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Argument(base.ContainerParameterName), "Clear", new CodeExpression[0]));
				}
				else
				{
					if (this.containerParamType != typeof(DataSet))
					{
						throw new InternalException("Unknown containerParameterType.");
					}
					codeStatement = CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Argument(base.ContainerParameterName), base.DesignTable.GeneratorTablePropName), "Clear", new CodeExpression[0]));
				}
				statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.ClearBeforeFillPropertyName), CodeGenHelper.Primitive(true)), codeStatement));
			}
			return true;
		}

		private bool AddExecuteCommandStatements(IList statements)
		{
			if (this.getMethod)
			{
				CodeExpression[] array = new CodeExpression[0];
				bool flag = this.designTable != null && this.designTable.HasAnyExpressionColumn;
				if (flag)
				{
					array = new CodeExpression[] { CodeGenHelper.Primitive(true) };
				}
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(base.ContainerParameterTypeName), base.ContainerParameterName, CodeGenHelper.New(CodeGenHelper.Type(base.ContainerParameterTypeName), array)));
			}
			CodeExpression[] array2 = new CodeExpression[] { CodeGenHelper.Variable(base.ContainerParameterName) };
			if (!this.getMethod)
			{
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), QueryGeneratorBase.returnVariableName, CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Fill", array2)));
			}
			else
			{
				statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Fill", array2)));
			}
			return true;
		}

		private bool AddExecuteCommandStatementsForPaging(IList statements)
		{
			if (this.containerParamType == typeof(DataTable))
			{
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(this.codeGenerator.DataSourceName), this.nameHandler.AddNameToList("dataSet"), CodeGenHelper.New(CodeGenHelper.Type(this.codeGenerator.DataSourceName), new CodeExpression[0])));
			}
			CodeExpression[] array = new CodeExpression[4];
			if (this.containerParamType == typeof(DataTable))
			{
				array[0] = CodeGenHelper.Variable(this.nameHandler.GetNameFromList("dataSet"));
			}
			else
			{
				array[0] = CodeGenHelper.Argument(base.ContainerParameterName);
			}
			array[1] = CodeGenHelper.Argument(this.nameHandler.GetNameFromList(QueryGeneratorBase.startRecordParameterName));
			array[2] = CodeGenHelper.Argument(this.nameHandler.GetNameFromList(QueryGeneratorBase.maxRecordsParameterName));
			array[3] = CodeGenHelper.Str("Table");
			if (!this.getMethod)
			{
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), QueryGeneratorBase.returnVariableName, CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Fill", array)));
			}
			else
			{
				statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Fill", array)));
			}
			if (this.containerParamType == typeof(DataTable) && !this.getMethod)
			{
				CodeStatement codeStatement = CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), "i", CodeGenHelper.Primitive(0));
				CodeExpression codeExpression = CodeGenHelper.Less(CodeGenHelper.Variable("i"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Variable(this.nameHandler.GetNameFromList("dataSet")), base.DesignTable.GeneratorName), "Rows"), "Count"));
				CodeStatement codeStatement2 = CodeGenHelper.Assign(CodeGenHelper.Variable("i"), CodeGenHelper.BinOperator(CodeGenHelper.Variable("i"), CodeBinaryOperatorType.Add, CodeGenHelper.Primitive(1)));
				CodeStatement codeStatement3 = CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Argument(base.ContainerParameterName), "ImportRow", new CodeExpression[] { CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Variable(this.nameHandler.GetNameFromList("dataSet")), base.DesignTable.GeneratorName), "Rows"), CodeGenHelper.Variable("i")) }));
				statements.Add(CodeGenHelper.ForLoop(codeStatement, codeExpression, codeStatement2, new CodeStatement[] { codeStatement3 }));
			}
			return true;
		}

		protected bool AddSetReturnParamValuesStatements(IList statements)
		{
			CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "SelectCommand");
			return base.AddSetReturnParamValuesStatements(statements, codeExpression);
		}

		private bool AddReturnStatements(IList statements)
		{
			if (this.getMethod)
			{
				if (base.GeneratePagingMethod)
				{
					statements.Add(CodeGenHelper.Return(CodeGenHelper.Property(CodeGenHelper.Variable(this.nameHandler.GetNameFromList("dataSet")), base.DesignTable.GeneratorName)));
				}
				else
				{
					statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(base.ContainerParameterName)));
				}
			}
			else
			{
				statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName)));
			}
			return true;
		}

		private void AddCustomAttributesToMethod(CodeMemberMethod dbMethod)
		{
			bool flag = false;
			if (this.methodSource.EnableWebMethods && this.getMethod)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Web.Services.WebMethod");
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Description", CodeGenHelper.Str(this.methodSource.WebMethodDescription)));
				dbMethod.CustomAttributes.Add(codeAttributeDeclaration);
			}
			if (base.GeneratePagingMethod)
			{
				return;
			}
			if (!this.getMethod && base.ContainerParameterType != typeof(DataTable))
			{
				return;
			}
			if (base.MethodSource == base.DesignTable.MainSource)
			{
				flag = true;
			}
			DataObjectMethodType dataObjectMethodType;
			if (this.getMethod)
			{
				dataObjectMethodType = DataObjectMethodType.Select;
			}
			else
			{
				dataObjectMethodType = DataObjectMethodType.Fill;
			}
			dbMethod.CustomAttributes.Add(new CodeAttributeDeclaration(CodeGenHelper.GlobalType(typeof(DataObjectMethodAttribute)), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataObjectMethodType)), dataObjectMethodType.ToString())),
				new CodeAttributeArgument(CodeGenHelper.Primitive(flag))
			}));
		}
	}
}
