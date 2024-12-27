using System;
using System.CodeDom;
using System.Design;
using System.Reflection;

namespace System.Data.Design
{
	// Token: 0x020000C7 RID: 199
	internal sealed class TypedColumnHandler
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x0001B526 File Offset: 0x0001A526
		internal TypedColumnHandler(DesignTable designTable, TypedDataSourceCodeGenerator codeGenerator)
		{
			this.codeGenerator = codeGenerator;
			this.table = designTable.DataTable;
			this.designTable = designTable;
			this.columns = designTable.DesignColumns;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001B554 File Offset: 0x0001A554
		internal void AddPrivateVariables(CodeTypeDeclaration dataTableClass)
		{
			if (dataTableClass == null)
			{
				throw new InternalException("Table CodeTypeDeclaration should not be null.");
			}
			if (this.columns == null)
			{
				return;
			}
			foreach (object obj in this.columns)
			{
				DesignColumn designColumn = (DesignColumn)obj;
				dataTableClass.Members.Add(CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(DataColumn)), designColumn.GeneratorColumnVarNameInTable));
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001B5E4 File Offset: 0x0001A5E4
		internal void AddTableColumnProperties(CodeTypeDeclaration dataTableClass)
		{
			if (this.columns == null)
			{
				return;
			}
			foreach (object obj in this.columns)
			{
				DesignColumn designColumn = (DesignColumn)obj;
				CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(DataColumn)), designColumn.GeneratorColumnPropNameInTable, (MemberAttributes)24578);
				codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), designColumn.GeneratorColumnVarNameInTable)));
				dataTableClass.Members.Add(codeMemberProperty);
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001B690 File Offset: 0x0001A690
		internal void AddRowColumnProperties(CodeTypeDeclaration rowClass)
		{
			bool flag = false;
			string generatorRowClassName = this.codeGenerator.TableHandler.Tables[this.table.TableName].GeneratorRowClassName;
			string generatorTableVarName = this.codeGenerator.TableHandler.Tables[this.table.TableName].GeneratorTableVarName;
			foreach (object obj in this.columns)
			{
				DesignColumn designColumn = (DesignColumn)obj;
				DataColumn dataColumn = designColumn.DataColumn;
				Type dataType = dataColumn.DataType;
				string generatorColumnPropNameInRow = designColumn.GeneratorColumnPropNameInRow;
				string generatorColumnPropNameInTable = designColumn.GeneratorColumnPropNameInTable;
				GenericNameHandler genericNameHandler = new GenericNameHandler(new string[] { generatorColumnPropNameInRow }, this.codeGenerator.CodeProvider);
				CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.Type(dataType), generatorColumnPropNameInRow, (MemberAttributes)24578);
				CodeStatement codeStatement = CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.GlobalType(dataType), CodeGenHelper.Indexer(CodeGenHelper.This(), CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName), generatorColumnPropNameInTable))));
				if (dataColumn.AllowDBNull)
				{
					string text = (string)dataColumn.ExtendedProperties["nullValue"];
					if (text == null || text == "_throw")
					{
						codeStatement = CodeGenHelper.Try(codeStatement, CodeGenHelper.Catch(CodeGenHelper.GlobalType(typeof(InvalidCastException)), genericNameHandler.AddNameToList("e"), CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(StrongTypingException)), SR.GetString("CG_ColumnIsDBNull", new object[]
						{
							dataColumn.ColumnName,
							this.table.TableName
						}), genericNameHandler.GetNameFromList("e"))));
					}
					else
					{
						CodeExpression codeExpression = null;
						CodeExpression codeExpression2;
						if (text == "_null")
						{
							if (dataColumn.DataType.IsSubclassOf(typeof(ValueType)))
							{
								this.codeGenerator.ProblemList.Add(new DSGeneratorProblem(SR.GetString("CG_TypeCantBeNull", new object[]
								{
									dataColumn.ColumnName,
									dataColumn.DataType.Name
								}), ProblemSeverity.NonFatalError, designColumn));
								continue;
							}
							codeExpression2 = CodeGenHelper.Primitive(null);
						}
						else if (text == "_empty")
						{
							if (dataColumn.DataType == typeof(string))
							{
								codeExpression2 = CodeGenHelper.Property(CodeGenHelper.TypeExpr(CodeGenHelper.GlobalType(dataColumn.DataType)), "Empty");
							}
							else
							{
								codeExpression2 = CodeGenHelper.Field(CodeGenHelper.TypeExpr(CodeGenHelper.Type(generatorRowClassName)), generatorColumnPropNameInRow + "_nullValue");
								ConstructorInfo constructor = dataColumn.DataType.GetConstructor(new Type[] { typeof(string) });
								if (constructor == null)
								{
									this.codeGenerator.ProblemList.Add(new DSGeneratorProblem(SR.GetString("CG_NoCtor0", new object[]
									{
										dataColumn.ColumnName,
										dataColumn.DataType.Name
									}), ProblemSeverity.NonFatalError, designColumn));
									continue;
								}
								constructor.Invoke(new object[0]);
								codeExpression = CodeGenHelper.New(CodeGenHelper.Type(dataColumn.DataType), new CodeExpression[0]);
							}
						}
						else
						{
							if (!flag)
							{
								this.table.NewRow();
								flag = true;
							}
							object obj2 = this.codeGenerator.RowHandler.RowGenerator.ConvertXmlToObject.Invoke(dataColumn, new object[] { text });
							DSGeneratorProblem dsgeneratorProblem = CodeGenHelper.GenerateValueExprAndFieldInit(designColumn, obj2, text, generatorRowClassName, generatorColumnPropNameInRow + "_nullValue", out codeExpression2, out codeExpression);
							if (dsgeneratorProblem != null)
							{
								this.codeGenerator.ProblemList.Add(dsgeneratorProblem);
								continue;
							}
						}
						codeStatement = CodeGenHelper.If(CodeGenHelper.MethodCall(CodeGenHelper.This(), "Is" + generatorColumnPropNameInRow + "Null"), new CodeStatement[] { CodeGenHelper.Return(codeExpression2) }, new CodeStatement[] { codeStatement });
						if (codeExpression != null)
						{
							CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.Type(dataColumn.DataType.FullName), generatorColumnPropNameInRow + "_nullValue");
							codeMemberField.Attributes = (MemberAttributes)20483;
							codeMemberField.InitExpression = codeExpression;
							rowClass.Members.Add(codeMemberField);
						}
					}
				}
				codeMemberProperty.GetStatements.Add(codeStatement);
				codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.Indexer(CodeGenHelper.This(), CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName), generatorColumnPropNameInTable)), CodeGenHelper.Value()));
				rowClass.Members.Add(codeMemberProperty);
				if (dataColumn.AllowDBNull)
				{
					string text2 = "Is" + generatorColumnPropNameInRow + "Null";
					string text3 = MemberNameValidator.GenerateIdName(text2, this.codeGenerator.CodeProvider, false);
					CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(bool)), text3, (MemberAttributes)24578);
					codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.This(), "IsNull", CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName), generatorColumnPropNameInTable))));
					rowClass.Members.Add(codeMemberMethod);
					text2 = "Set" + generatorColumnPropNameInRow + "Null";
					text3 = MemberNameValidator.GenerateIdName(text2, this.codeGenerator.CodeProvider, false);
					CodeMemberMethod codeMemberMethod2 = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), text3, (MemberAttributes)24578);
					codeMemberMethod2.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Indexer(CodeGenHelper.This(), CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName), generatorColumnPropNameInTable)), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(Convert)), "DBNull")));
					rowClass.Members.Add(codeMemberMethod2);
				}
			}
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001BC90 File Offset: 0x0001AC90
		internal void AddRowGetRelatedRowsMethods(CodeTypeDeclaration rowClass)
		{
			DataRelationCollection childRelations = this.table.ChildRelations;
			for (int i = 0; i < childRelations.Count; i++)
			{
				DataRelation dataRelation = childRelations[i];
				string generatorRowClassName = this.codeGenerator.TableHandler.Tables[dataRelation.ChildTable.TableName].GeneratorRowClassName;
				CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(generatorRowClassName, 1), this.codeGenerator.RelationHandler.Relations[dataRelation.RelationName].GeneratorChildPropName, (MemberAttributes)24578);
				codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdEQ(CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), "Table"), "ChildRelations"), CodeGenHelper.Str(dataRelation.RelationName)), CodeGenHelper.Primitive(null)), CodeGenHelper.Return(new CodeArrayCreateExpression(generatorRowClassName, 0)), CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.Type(generatorRowClassName, 1), CodeGenHelper.MethodCall(CodeGenHelper.Base(), "GetChildRows", CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), "Table"), "ChildRelations"), CodeGenHelper.Str(dataRelation.RelationName)))))));
				rowClass.Members.Add(codeMemberMethod);
			}
			DataRelationCollection parentRelations = this.table.ParentRelations;
			for (int j = 0; j < parentRelations.Count; j++)
			{
				DataRelation dataRelation2 = parentRelations[j];
				string generatorRowClassName2 = this.codeGenerator.TableHandler.Tables[dataRelation2.ParentTable.TableName].GeneratorRowClassName;
				CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.Type(generatorRowClassName2), this.codeGenerator.RelationHandler.Relations[dataRelation2.RelationName].GeneratorParentPropName, (MemberAttributes)24578);
				codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.Type(generatorRowClassName2), CodeGenHelper.MethodCall(CodeGenHelper.This(), "GetParentRow", CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), "Table"), "ParentRelations"), CodeGenHelper.Str(dataRelation2.RelationName))))));
				codeMemberProperty.SetStatements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "SetParentRow", new CodeExpression[]
				{
					CodeGenHelper.Value(),
					CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), "Table"), "ParentRelations"), CodeGenHelper.Str(dataRelation2.RelationName))
				}));
				rowClass.Members.Add(codeMemberProperty);
			}
		}

		// Token: 0x04000C78 RID: 3192
		private TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000C79 RID: 3193
		private DataTable table;

		// Token: 0x04000C7A RID: 3194
		private DesignTable designTable;

		// Token: 0x04000C7B RID: 3195
		private DesignColumnCollection columns;
	}
}
