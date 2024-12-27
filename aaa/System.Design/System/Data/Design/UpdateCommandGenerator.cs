using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;

namespace System.Data.Design
{
	// Token: 0x020000D7 RID: 215
	internal class UpdateCommandGenerator : QueryGeneratorBase
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x0001DD00 File Offset: 0x0001CD00
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x0001DD08 File Offset: 0x0001CD08
		internal bool GenerateOverloadWithoutCurrentPKParameters
		{
			get
			{
				return this.generateOverloadWithoutCurrentPKParameters;
			}
			set
			{
				this.generateOverloadWithoutCurrentPKParameters = value;
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001DD11 File Offset: 0x0001CD11
		internal UpdateCommandGenerator(TypedDataSourceCodeGenerator codeGenerator)
			: base(codeGenerator)
		{
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001DD1C File Offset: 0x0001CD1C
		internal override CodeMemberMethod Generate()
		{
			if (this.methodSource == null)
			{
				throw new InternalException("MethodSource should not be null.");
			}
			if (base.MethodType == MethodTypeEnum.ColumnParameters && this.activeCommand == null)
			{
				throw new InternalException("ActiveCommand should not be null.");
			}
			this.methodAttributes = base.MethodSource.Modifier | MemberAttributes.Overloaded;
			this.returnType = typeof(int);
			CodeDomProvider codeDomProvider = ((this.codeProvider != null) ? this.codeGenerator.CodeProvider : base.CodeProvider);
			this.nameHandler = new GenericNameHandler(new string[]
			{
				base.MethodName,
				QueryGeneratorBase.commandVariableName,
				QueryGeneratorBase.returnVariableName
			}, codeDomProvider);
			return this.GenerateInternal();
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001DDD0 File Offset: 0x0001CDD0
		private CodeMemberMethod GenerateInternal()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(this.returnType), base.MethodName, this.methodAttributes);
			codeMemberMethod.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(HelpKeywordAttribute).FullName, CodeGenHelper.Str("vs.data.TableAdapter")));
			this.AddParametersToMethod(codeMemberMethod);
			if (this.declarationOnly)
			{
				return codeMemberMethod;
			}
			this.AddCustomAttributesToMethod(codeMemberMethod);
			if (this.AddStatementsToMethod(codeMemberMethod))
			{
				return codeMemberMethod;
			}
			return null;
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001DE4C File Offset: 0x0001CE4C
		private void AddParametersToMethod(CodeMemberMethod dbMethod)
		{
			DesignConnection designConnection = (DesignConnection)this.methodSource.Connection;
			if (designConnection == null)
			{
				throw new InternalException(string.Format(CultureInfo.CurrentCulture, "Connection for query {0} is null.", new object[] { this.methodSource.Name }));
			}
			string parameterPrefix = designConnection.ParameterPrefix;
			if (base.MethodType == MethodTypeEnum.ColumnParameters)
			{
				if (this.activeCommand.Parameters == null)
				{
					return;
				}
				using (IEnumerator enumerator = this.activeCommand.Parameters.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DesignParameter designParameter = (DesignParameter)obj;
						if (designParameter.Direction != ParameterDirection.ReturnValue && !designParameter.SourceColumnNullMapping && (!this.GenerateOverloadWithoutCurrentPKParameters || designParameter.SourceVersion != DataRowVersion.Current || !this.IsPrimaryColumn(designParameter.SourceColumn)))
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
					return;
				}
			}
			string text2 = this.nameHandler.AddParameterNameToList(base.UpdateParameterName, parameterPrefix);
			CodeParameterDeclarationExpression codeParameterDeclarationExpression2;
			if (base.UpdateParameterTypeName != null)
			{
				codeParameterDeclarationExpression2 = CodeGenHelper.ParameterDecl(CodeGenHelper.Type(base.UpdateParameterTypeName), text2);
			}
			else
			{
				codeParameterDeclarationExpression2 = CodeGenHelper.ParameterDecl(base.UpdateParameterTypeReference, text2);
			}
			dbMethod.Parameters.Add(codeParameterDeclarationExpression2);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001E010 File Offset: 0x0001D010
		private bool AddStatementsToMethod(CodeMemberMethod dbMethod)
		{
			if (this.GenerateOverloadWithoutCurrentPKParameters)
			{
				return this.AddCallOverloadUpdateStm(dbMethod);
			}
			if (base.MethodType == MethodTypeEnum.ColumnParameters && !this.AddSetParametersStatements(dbMethod.Statements))
			{
				return false;
			}
			if (!this.AddExecuteCommandStatements(dbMethod.Statements))
			{
				return false;
			}
			if (base.MethodType == MethodTypeEnum.ColumnParameters)
			{
				if (!this.AddSetReturnParamValuesStatements(dbMethod.Statements))
				{
					return false;
				}
				if (!this.AddReturnStatements(dbMethod.Statements))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001E088 File Offset: 0x0001D088
		private bool AddCallOverloadUpdateStm(CodeMemberMethod dbMethod)
		{
			int num = 0;
			if (this.activeCommand.Parameters != null)
			{
				num = this.activeCommand.Parameters.Count;
			}
			if (num <= 0)
			{
				return false;
			}
			List<CodeExpression> list = new List<CodeExpression>();
			bool flag = false;
			for (int i = 0; i < num; i++)
			{
				DesignParameter designParameter = this.activeCommand.Parameters[i];
				if (designParameter == null)
				{
					throw new DataSourceGeneratorException("Parameter type is not DesignParameter.");
				}
				if ((designParameter.Direction == ParameterDirection.Input || designParameter.Direction == ParameterDirection.InputOutput) && !designParameter.SourceColumnNullMapping)
				{
					if (designParameter.SourceVersion == DataRowVersion.Current && this.IsPrimaryColumn(designParameter.SourceColumn))
					{
						designParameter = this.GetOriginalVersionParameter(designParameter);
						if (designParameter != null)
						{
							flag = true;
						}
					}
					if (designParameter != null)
					{
						string nameFromList = this.nameHandler.GetNameFromList(designParameter.ParameterName);
						list.Add(CodeGenHelper.Argument(nameFromList));
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			CodeStatement codeStatement = CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.This(), "Update", list.ToArray()));
			dbMethod.Statements.Add(codeStatement);
			return true;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001E198 File Offset: 0x0001D198
		private DesignParameter GetOriginalVersionParameter(DesignParameter currentVersionParameter)
		{
			if (currentVersionParameter == null || currentVersionParameter.SourceVersion != DataRowVersion.Current)
			{
				throw new InternalException("Invalid argutment currentVersionParameter");
			}
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
				if ((designParameter.Direction == ParameterDirection.Input || designParameter.Direction == ParameterDirection.InputOutput) && !designParameter.SourceColumnNullMapping && designParameter.SourceVersion == DataRowVersion.Original && StringUtil.EqualValue(designParameter.SourceColumn, currentVersionParameter.SourceColumn))
				{
					return designParameter;
				}
			}
			return null;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0001E24C File Offset: 0x0001D24C
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
				if ((designParameter.Direction == ParameterDirection.Input || designParameter.Direction == ParameterDirection.InputOutput) && !designParameter.SourceColumnNullMapping)
				{
					string nameFromList = this.nameHandler.GetNameFromList(designParameter.ParameterName);
					CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName);
					DesignParameter designParameter2 = null;
					int num2 = 0;
					if (designParameter.SourceVersion == DataRowVersion.Original)
					{
						designParameter2 = this.FindCorrespondingIsNullParameter(designParameter, out num2);
					}
					base.AddSetParameterStatements(designParameter, nameFromList, designParameter2, codeExpression, i, num2, statements);
				}
			}
			return true;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0001E324 File Offset: 0x0001D324
		private bool AddExecuteCommandStatements(IList statements)
		{
			if (base.MethodType == MethodTypeEnum.ColumnParameters)
			{
				CodeStatement[] array = new CodeStatement[1];
				CodeStatement[] array2 = new CodeStatement[1];
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(ConnectionState)), this.nameHandler.AddNameToList("previousConnectionState"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName), "Connection"), "State")));
				statements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.BitwiseAnd(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName), "Connection"), "State"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Open")), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Open")), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName), "Connection"), "Open"))));
				array[0] = CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(int)), QueryGeneratorBase.returnVariableName, CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName), "ExecuteNonQuery", new CodeExpression[0]));
				array2[0] = CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Variable(this.nameHandler.GetNameFromList("previousConnectionState")), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ConnectionState)), "Closed")), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName), "Connection"), "Close")));
				statements.Add(CodeGenHelper.Try(array, new CodeCatchClause[0], array2));
			}
			else if (StringUtil.EqualValue(base.UpdateParameterTypeReference.BaseType, typeof(DataRow).FullName) && base.UpdateParameterTypeReference.ArrayRank == 0)
			{
				statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Update", new CodeExpression[] { CodeGenHelper.NewArray(base.UpdateParameterTypeReference, new CodeExpression[] { CodeGenHelper.Argument(base.UpdateParameterName) }) })));
			}
			else if (StringUtil.EqualValue(base.UpdateParameterTypeReference.BaseType, typeof(DataSet).FullName))
			{
				statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Update", new CodeExpression[]
				{
					CodeGenHelper.Argument(base.UpdateParameterName),
					CodeGenHelper.Str(base.DesignTable.Name)
				})));
			}
			else
			{
				statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), "Update", new CodeExpression[] { CodeGenHelper.Argument(base.UpdateParameterName) })));
			}
			return true;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0001E654 File Offset: 0x0001D654
		protected bool AddSetReturnParamValuesStatements(IList statements)
		{
			CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), DataComponentNameHandler.AdapterPropertyName), base.UpdateCommandName);
			CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = (CodeTryCatchFinallyStatement)statements[statements.Count - 1];
			return base.AddSetReturnParamValuesStatements(codeTryCatchFinallyStatement.TryStatements, codeExpression);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0001E6A0 File Offset: 0x0001D6A0
		private bool AddReturnStatements(IList statements)
		{
			CodeTryCatchFinallyStatement codeTryCatchFinallyStatement = (CodeTryCatchFinallyStatement)statements[statements.Count - 1];
			codeTryCatchFinallyStatement.TryStatements.Add(CodeGenHelper.Return(CodeGenHelper.Variable(QueryGeneratorBase.returnVariableName)));
			return true;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0001E6E0 File Offset: 0x0001D6E0
		private void AddCustomAttributesToMethod(CodeMemberMethod dbMethod)
		{
			DataObjectMethodType dataObjectMethodType = DataObjectMethodType.Update;
			if (this.methodSource.EnableWebMethods)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Web.Services.WebMethod");
				codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument("Description", CodeGenHelper.Str(this.methodSource.WebMethodDescription)));
				dbMethod.CustomAttributes.Add(codeAttributeDeclaration);
			}
			if (base.MethodType == MethodTypeEnum.GenericUpdate)
			{
				return;
			}
			if (this.activeCommand == this.methodSource.DeleteCommand)
			{
				dataObjectMethodType = DataObjectMethodType.Delete;
			}
			else if (this.activeCommand == this.methodSource.InsertCommand)
			{
				dataObjectMethodType = DataObjectMethodType.Insert;
			}
			else if (this.activeCommand == this.methodSource.UpdateCommand)
			{
				dataObjectMethodType = DataObjectMethodType.Update;
			}
			dbMethod.CustomAttributes.Add(new CodeAttributeDeclaration(CodeGenHelper.GlobalType(typeof(DataObjectMethodAttribute)), new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataObjectMethodType)), dataObjectMethodType.ToString())),
				new CodeAttributeArgument(CodeGenHelper.Primitive(true))
			}));
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0001E7EC File Offset: 0x0001D7EC
		private DesignParameter FindCorrespondingIsNullParameter(DesignParameter originalParameter, out int isNullParameterIndex)
		{
			if (originalParameter == null || originalParameter.SourceVersion != DataRowVersion.Original || originalParameter.SourceColumnNullMapping)
			{
				throw new InternalException("'originalParameter' is not valid.");
			}
			isNullParameterIndex = 0;
			for (int i = 0; i < this.activeCommand.Parameters.Count; i++)
			{
				DesignParameter designParameter = this.activeCommand.Parameters[i];
				if (designParameter == null)
				{
					throw new DataSourceGeneratorException("Parameter type is not DesignParameter.");
				}
				if (((designParameter.Direction != ParameterDirection.Input && designParameter.Direction != ParameterDirection.InputOutput) || (designParameter.SourceColumnNullMapping && designParameter.SourceVersion == DataRowVersion.Original)) && StringUtil.EqualValue(originalParameter.SourceColumn, designParameter.SourceColumn))
				{
					isNullParameterIndex = i;
					return designParameter;
				}
			}
			return null;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0001E89C File Offset: 0x0001D89C
		private bool IsPrimaryColumn(string columnName)
		{
			DataColumn[] primaryKeyColumns = base.DesignTable.PrimaryKeyColumns;
			if (primaryKeyColumns == null || primaryKeyColumns.Length == 0)
			{
				return false;
			}
			foreach (DataColumn dataColumn in primaryKeyColumns)
			{
				if (StringUtil.EqualValue(dataColumn.ColumnName, columnName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000CB8 RID: 3256
		private bool generateOverloadWithoutCurrentPKParameters;
	}
}
