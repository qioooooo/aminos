using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Schema;

namespace System.Data.Design
{
	internal sealed class TableMethodGenerator
	{
		internal TableMethodGenerator(TypedDataSourceCodeGenerator codeGenerator, DesignTable designTable)
		{
			this.codeGenerator = codeGenerator;
			this.designTable = designTable;
		}

		internal void AddMethods(CodeTypeDeclaration dataTableClass)
		{
			if (dataTableClass == null)
			{
				throw new InternalException("Table CodeTypeDeclaration should not be null.");
			}
			this.rowClassName = this.designTable.GeneratorRowClassName;
			this.rowConcreteClassName = this.designTable.GeneratorRowClassName;
			this.tableClassName = this.designTable.GeneratorTableClassName;
			this.initExpressionsMethod = this.InitExpressionsMethod();
			if (this.initExpressionsMethod != null)
			{
				dataTableClass.Members.Add(this.ArgumentLessConstructorInitExpressions());
				dataTableClass.Members.Add(this.ConstructorWithBoolArgument());
			}
			else
			{
				dataTableClass.Members.Add(this.ArgumentLessConstructorNoInitExpressions());
			}
			dataTableClass.Members.Add(this.ConstructorWithArguments());
			dataTableClass.Members.Add(this.DeserializingConstructor());
			dataTableClass.Members.Add(this.AddTypedRowMethod());
			this.AddTypedRowByColumnsMethods(dataTableClass);
			this.AddFindByMethods(dataTableClass);
			if ((this.codeGenerator.GenerateOptions & TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets) != TypedDataSetGenerator.GenerateOption.LinqOverTypedDatasets)
			{
				dataTableClass.Members.Add(this.GetEnumeratorMethod());
			}
			dataTableClass.Members.Add(this.CloneMethod());
			dataTableClass.Members.Add(this.CreateInstanceMethod());
			CodeMemberMethod codeMemberMethod = null;
			CodeMemberMethod codeMemberMethod2 = null;
			this.InitClassAndInitVarsMethods(dataTableClass, out codeMemberMethod, out codeMemberMethod2);
			dataTableClass.Members.Add(codeMemberMethod2);
			dataTableClass.Members.Add(codeMemberMethod);
			dataTableClass.Members.Add(this.NewTypedRowMethod());
			dataTableClass.Members.Add(this.NewRowFromBuilderMethod());
			dataTableClass.Members.Add(this.GetRowTypeMethod());
			if (this.initExpressionsMethod != null)
			{
				dataTableClass.Members.Add(this.initExpressionsMethod);
			}
			if (this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareEvents) && this.codeGenerator.CodeProvider.Supports(GeneratorSupport.DeclareDelegates))
			{
				this.AddOnRowEventMethods(dataTableClass);
			}
			dataTableClass.Members.Add(this.RemoveRowMethod());
			dataTableClass.Members.Add(this.GetTypedTableSchema());
		}

		private CodeConstructor ArgumentLessConstructorInitExpressions()
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)24578);
			codeConstructor.ChainedConstructorArgs.Add(CodeGenHelper.Primitive(false));
			return codeConstructor;
		}

		private CodeConstructor ConstructorWithBoolArgument()
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)4098);
			codeConstructor.Attributes = (MemberAttributes)24578;
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(bool)), "initExpressions"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "TableName"), CodeGenHelper.Str(this.designTable.Name)));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "BeginInit"));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitClass"));
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Argument("initExpressions"), CodeGenHelper.Primitive(true)), new CodeStatement[] { CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitExpressions")) }));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "EndInit"));
			return codeConstructor;
		}

		private CodeConstructor ArgumentLessConstructorNoInitExpressions()
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)24578);
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "TableName"), CodeGenHelper.Str(this.designTable.Name)));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "BeginInit"));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitClass"));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "EndInit"));
			return codeConstructor;
		}

		private CodeConstructor ConstructorWithArguments()
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor((MemberAttributes)4098);
			codeConstructor.Attributes = (MemberAttributes)4098;
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataTable)), "table"));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "TableName"), CodeGenHelper.Property(CodeGenHelper.Argument("table"), "TableName")));
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Property(CodeGenHelper.Argument("table"), "CaseSensitive"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Argument("table"), "DataSet"), "CaseSensitive")), CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "CaseSensitive"), CodeGenHelper.Property(CodeGenHelper.Argument("table"), "CaseSensitive"))));
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Argument("table"), "Locale"), "ToString"), CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Argument("table"), "DataSet"), "Locale"), "ToString")), CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Locale"), CodeGenHelper.Property(CodeGenHelper.Argument("table"), "Locale"))));
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Property(CodeGenHelper.Argument("table"), "Namespace"), CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.Argument("table"), "DataSet"), "Namespace")), CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Namespace"), CodeGenHelper.Property(CodeGenHelper.Argument("table"), "Namespace"))));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Prefix"), CodeGenHelper.Property(CodeGenHelper.Argument("table"), "Prefix")));
			codeConstructor.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "MinimumCapacity"), CodeGenHelper.Property(CodeGenHelper.Argument("table"), "MinimumCapacity")));
			return codeConstructor;
		}

		private CodeConstructor DeserializingConstructor()
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor(MemberAttributes.Family);
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(SerializationInfo)), "info"));
			codeConstructor.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(StreamingContext)), "context"));
			codeConstructor.BaseConstructorArgs.AddRange(new CodeExpression[]
			{
				CodeGenHelper.Argument("info"),
				CodeGenHelper.Argument("context")
			});
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars"));
			return codeConstructor;
		}

		private CodeMemberMethod InitExpressionsMethod()
		{
			bool flag = false;
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitExpressions", MemberAttributes.Private);
			DataTable dataTable = this.designTable.DataTable;
			foreach (object obj in dataTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (dataColumn.Expression.Length > 0)
				{
					CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.This(), this.codeGenerator.TableHandler.Tables[dataColumn.Table.TableName].DesignColumns[dataColumn.ColumnName].GeneratorColumnPropNameInTable);
					flag = true;
					codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression, "Expression"), CodeGenHelper.Str(dataColumn.Expression)));
				}
			}
			if (flag)
			{
				return codeMemberMethod;
			}
			return null;
		}

		private CodeMemberMethod AddTypedRowMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), NameHandler.FixIdName("Add" + this.rowClassName), (MemberAttributes)24578);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(this.rowConcreteClassName), "row"));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), "Add", CodeGenHelper.Argument("row")));
			return codeMemberMethod;
		}

		private void AddTypedRowByColumnsMethods(CodeTypeDeclaration dataTableClass)
		{
			DataTable dataTable = this.designTable.DataTable;
			ArrayList arrayList = new ArrayList();
			bool flag = false;
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				if (!dataTable.Columns[i].AutoIncrement)
				{
					arrayList.Add(dataTable.Columns[i]);
				}
			}
			string text = NameHandler.FixIdName("Add" + this.rowClassName);
			GenericNameHandler genericNameHandler = new GenericNameHandler(new string[]
			{
				text,
				TableMethodGenerator.columnValuesArrayName
			}, this.codeGenerator.CodeProvider);
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(this.rowConcreteClassName), text, (MemberAttributes)24578);
			CodeMemberMethod codeMemberMethod2 = CodeGenHelper.MethodDecl(CodeGenHelper.Type(this.rowConcreteClassName), text, (MemberAttributes)24578);
			DataColumn[] array = new DataColumn[arrayList.Count];
			arrayList.CopyTo(array, 0);
			for (int j = 0; j < array.Length; j++)
			{
				Type dataType = array[j].DataType;
				DataRelation dataRelation = this.FindParentRelation(array[j]);
				if (this.ChildRelationFollowable(dataRelation))
				{
					string generatorRowClassName = this.codeGenerator.TableHandler.Tables[dataRelation.ParentTable.TableName].GeneratorRowClassName;
					string text2 = NameHandler.FixIdName("parent" + generatorRowClassName + "By" + dataRelation.RelationName);
					codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(generatorRowClassName), genericNameHandler.AddNameToList(text2)));
					codeMemberMethod2.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(generatorRowClassName), genericNameHandler.GetNameFromList(text2)));
				}
				else
				{
					codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(dataType), genericNameHandler.AddNameToList(this.codeGenerator.TableHandler.Tables[array[j].Table.TableName].DesignColumns[array[j].ColumnName].GeneratorColumnPropNameInRow)));
					if (StringUtil.Empty(array[j].Expression))
					{
						codeMemberMethod2.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(dataType), genericNameHandler.GetNameFromList(this.codeGenerator.TableHandler.Tables[array[j].Table.TableName].DesignColumns[array[j].ColumnName].GeneratorColumnPropNameInRow)));
					}
					else
					{
						flag = true;
					}
				}
			}
			CodeStatement codeStatement = CodeGenHelper.VariableDecl(CodeGenHelper.Type(this.rowConcreteClassName), NameHandler.FixIdName("row" + this.rowClassName), CodeGenHelper.Cast(CodeGenHelper.Type(this.rowConcreteClassName), CodeGenHelper.MethodCall(CodeGenHelper.This(), "NewRow")));
			codeMemberMethod.Statements.Add(codeStatement);
			codeMemberMethod2.Statements.Add(codeStatement);
			CodeExpression codeExpression = CodeGenHelper.Variable(NameHandler.FixIdName("row" + this.rowClassName));
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
			codeAssignStatement.Left = CodeGenHelper.Property(codeExpression, "ItemArray");
			CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression();
			codeArrayCreateExpression.CreateType = CodeGenHelper.GlobalType(typeof(object));
			CodeArrayCreateExpression codeArrayCreateExpression2 = new CodeArrayCreateExpression();
			codeArrayCreateExpression2.CreateType = CodeGenHelper.GlobalType(typeof(object));
			array = new DataColumn[dataTable.Columns.Count];
			dataTable.Columns.CopyTo(array, 0);
			for (int k = 0; k < array.Length; k++)
			{
				if (array[k].AutoIncrement)
				{
					codeArrayCreateExpression.Initializers.Add(CodeGenHelper.Primitive(null));
					codeArrayCreateExpression2.Initializers.Add(CodeGenHelper.Primitive(null));
				}
				else
				{
					DataRelation dataRelation2 = this.FindParentRelation(array[k]);
					if (this.ChildRelationFollowable(dataRelation2))
					{
						codeArrayCreateExpression.Initializers.Add(CodeGenHelper.Primitive(null));
						codeArrayCreateExpression2.Initializers.Add(CodeGenHelper.Primitive(null));
					}
					else
					{
						codeArrayCreateExpression.Initializers.Add(CodeGenHelper.Argument(genericNameHandler.GetNameFromList(this.codeGenerator.TableHandler.Tables[array[k].Table.TableName].DesignColumns[array[k].ColumnName].GeneratorColumnPropNameInRow)));
						if (StringUtil.Empty(array[k].Expression))
						{
							codeArrayCreateExpression2.Initializers.Add(CodeGenHelper.Argument(genericNameHandler.GetNameFromList(this.codeGenerator.TableHandler.Tables[array[k].Table.TableName].DesignColumns[array[k].ColumnName].GeneratorColumnPropNameInRow)));
						}
						else
						{
							codeArrayCreateExpression2.Initializers.Add(CodeGenHelper.Primitive(null));
						}
					}
				}
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(object), 1), TableMethodGenerator.columnValuesArrayName, codeArrayCreateExpression));
			codeMemberMethod2.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(object), 1), TableMethodGenerator.columnValuesArrayName, codeArrayCreateExpression2));
			for (int l = 0; l < array.Length; l++)
			{
				if (!array[l].AutoIncrement)
				{
					DataRelation dataRelation3 = this.FindParentRelation(array[l]);
					if (this.ChildRelationFollowable(dataRelation3))
					{
						string generatorRowClassName2 = this.codeGenerator.TableHandler.Tables[dataRelation3.ParentTable.TableName].GeneratorRowClassName;
						string text3 = NameHandler.FixIdName("parent" + generatorRowClassName2 + "By" + dataRelation3.RelationName);
						CodeStatement codeStatement2 = CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Argument(genericNameHandler.GetNameFromList(text3)), CodeGenHelper.Primitive(null)), CodeGenHelper.Assign(CodeGenHelper.Indexer(CodeGenHelper.Variable(TableMethodGenerator.columnValuesArrayName), CodeGenHelper.Primitive(l)), CodeGenHelper.Indexer(CodeGenHelper.Argument(genericNameHandler.GetNameFromList(text3)), CodeGenHelper.Primitive(dataRelation3.ParentColumns[0].Ordinal))));
						codeMemberMethod.Statements.Add(codeStatement2);
						codeMemberMethod2.Statements.Add(codeStatement2);
					}
				}
			}
			codeAssignStatement.Right = CodeGenHelper.Variable(TableMethodGenerator.columnValuesArrayName);
			codeMemberMethod.Statements.Add(codeAssignStatement);
			codeMemberMethod2.Statements.Add(codeAssignStatement);
			CodeExpression codeExpression2 = CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), "Add", codeExpression);
			codeMemberMethod.Statements.Add(codeExpression2);
			codeMemberMethod2.Statements.Add(codeExpression2);
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(codeExpression));
			codeMemberMethod2.Statements.Add(CodeGenHelper.Return(codeExpression));
			dataTableClass.Members.Add(codeMemberMethod);
			if (flag)
			{
				dataTableClass.Members.Add(codeMemberMethod2);
			}
		}

		private void AddFindByMethods(CodeTypeDeclaration dataTableClass)
		{
			DataTable dataTable = this.designTable.DataTable;
			for (int i = 0; i < dataTable.Constraints.Count; i++)
			{
				if (dataTable.Constraints[i] is UniqueConstraint && ((UniqueConstraint)dataTable.Constraints[i]).IsPrimaryKey)
				{
					DataColumn[] columns = ((UniqueConstraint)dataTable.Constraints[i]).Columns;
					string text = "FindBy";
					bool flag = true;
					for (int j = 0; j < columns.Length; j++)
					{
						text += this.codeGenerator.TableHandler.Tables[columns[j].Table.TableName].DesignColumns[columns[j].ColumnName].GeneratorColumnPropNameInRow;
						if (columns[j].ColumnMapping != MappingType.Hidden)
						{
							flag = false;
						}
					}
					if (!flag)
					{
						CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(this.rowClassName), NameHandler.FixIdName(text), (MemberAttributes)24578);
						for (int k = 0; k < columns.Length; k++)
						{
							codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(columns[k].DataType), this.codeGenerator.TableHandler.Tables[columns[k].Table.TableName].DesignColumns[columns[k].ColumnName].GeneratorColumnPropNameInRow));
						}
						CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression(typeof(object), columns.Length);
						for (int l = 0; l < columns.Length; l++)
						{
							codeArrayCreateExpression.Initializers.Add(CodeGenHelper.Argument(this.codeGenerator.TableHandler.Tables[columns[l].Table.TableName].DesignColumns[columns[l].ColumnName].GeneratorColumnPropNameInRow));
						}
						codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.Type(this.rowClassName), CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), "Find", codeArrayCreateExpression))));
						dataTableClass.Members.Add(codeMemberMethod);
					}
				}
			}
		}

		private CodeMemberMethod GetEnumeratorMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(IEnumerator)), "GetEnumerator", MemberAttributes.Public);
			codeMemberMethod.ImplementationTypes.Add(CodeGenHelper.GlobalType(typeof(IEnumerable)));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), "GetEnumerator")));
			return codeMemberMethod;
		}

		private CodeMemberMethod CloneMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(DataTable)), "Clone", (MemberAttributes)24580);
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(this.tableClassName), "cln", CodeGenHelper.Cast(CodeGenHelper.Type(this.tableClassName), CodeGenHelper.MethodCall(CodeGenHelper.Base(), "Clone", new CodeExpression[0]))));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Variable("cln"), "InitVars", new CodeExpression[0]));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable("cln")));
			return codeMemberMethod;
		}

		private CodeMemberMethod CreateInstanceMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(DataTable)), "CreateInstance", (MemberAttributes)12292);
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.New(CodeGenHelper.Type(this.tableClassName), new CodeExpression[0])));
			return codeMemberMethod;
		}

		private void InitClassAndInitVarsMethods(CodeTypeDeclaration tableClass, out CodeMemberMethod tableInitClass, out CodeMemberMethod tableInitVars)
		{
			DataTable dataTable = this.designTable.DataTable;
			tableInitClass = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitClass", MemberAttributes.Private);
			tableInitVars = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitVars", (MemberAttributes)4098);
			for (int i = 0; i < dataTable.Columns.Count; i++)
			{
				DataColumn dataColumn = dataTable.Columns[i];
				string generatorColumnVarNameInTable = this.codeGenerator.TableHandler.Tables[dataTable.TableName].DesignColumns[dataColumn.ColumnName].GeneratorColumnVarNameInTable;
				CodeExpression codeExpression = CodeGenHelper.Field(CodeGenHelper.This(), generatorColumnVarNameInTable);
				string text = "Element";
				if (dataColumn.ColumnMapping == MappingType.SimpleContent)
				{
					text = "SimpleContent";
				}
				else if (dataColumn.ColumnMapping == MappingType.Attribute)
				{
					text = "Attribute";
				}
				else if (dataColumn.ColumnMapping == MappingType.Hidden)
				{
					text = "Hidden";
				}
				tableInitClass.Statements.Add(CodeGenHelper.Assign(codeExpression, CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(DataColumn)), new CodeExpression[]
				{
					CodeGenHelper.Str(dataColumn.ColumnName),
					CodeGenHelper.TypeOf(CodeGenHelper.GlobalType(dataColumn.DataType)),
					CodeGenHelper.Primitive(null),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(MappingType)), text)
				})));
				ExtendedPropertiesHandler.CodeGenerator = this.codeGenerator;
				ExtendedPropertiesHandler.AddExtendedProperties(this.designTable.DesignColumns[dataColumn.ColumnName], codeExpression, tableInitClass.Statements, dataColumn.ExtendedProperties);
				tableInitClass.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Base(), "Columns"), "Add", CodeGenHelper.Field(CodeGenHelper.This(), generatorColumnVarNameInTable)));
			}
			for (int j = 0; j < dataTable.Constraints.Count; j++)
			{
				if (dataTable.Constraints[j] is UniqueConstraint)
				{
					UniqueConstraint uniqueConstraint = (UniqueConstraint)dataTable.Constraints[j];
					DataColumn[] columns = uniqueConstraint.Columns;
					CodeExpression[] array = new CodeExpression[columns.Length];
					for (int k = 0; k < columns.Length; k++)
					{
						array[k] = CodeGenHelper.Field(CodeGenHelper.This(), this.codeGenerator.TableHandler.Tables[columns[k].Table.TableName].DesignColumns[columns[k].ColumnName].GeneratorColumnVarNameInTable);
					}
					tableInitClass.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Constraints"), "Add", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(UniqueConstraint)), new CodeExpression[]
					{
						CodeGenHelper.Str(uniqueConstraint.ConstraintName),
						new CodeArrayCreateExpression(CodeGenHelper.GlobalType(typeof(DataColumn)), array),
						CodeGenHelper.Primitive(uniqueConstraint.IsPrimaryKey)
					})));
				}
			}
			for (int l = 0; l < dataTable.Columns.Count; l++)
			{
				DataColumn dataColumn2 = dataTable.Columns[l];
				string generatorColumnVarNameInTable2 = this.codeGenerator.TableHandler.Tables[dataTable.TableName].DesignColumns[dataColumn2.ColumnName].GeneratorColumnVarNameInTable;
				CodeExpression codeExpression2 = CodeGenHelper.Field(CodeGenHelper.This(), generatorColumnVarNameInTable2);
				tableInitVars.Statements.Add(CodeGenHelper.Assign(codeExpression2, CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Base(), "Columns"), CodeGenHelper.Str(dataColumn2.ColumnName))));
				if (dataColumn2.AutoIncrement)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "AutoIncrement"), CodeGenHelper.Primitive(true)));
				}
				if (dataColumn2.AutoIncrementSeed != 0L)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "AutoIncrementSeed"), CodeGenHelper.Primitive(dataColumn2.AutoIncrementSeed)));
				}
				if (dataColumn2.AutoIncrementStep != 1L)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "AutoIncrementStep"), CodeGenHelper.Primitive(dataColumn2.AutoIncrementStep)));
				}
				if (!dataColumn2.AllowDBNull)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "AllowDBNull"), CodeGenHelper.Primitive(false)));
				}
				if (dataColumn2.ReadOnly)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "ReadOnly"), CodeGenHelper.Primitive(true)));
				}
				if (dataColumn2.Unique)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "Unique"), CodeGenHelper.Primitive(true)));
				}
				if (!StringUtil.Empty(dataColumn2.Prefix))
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "Prefix"), CodeGenHelper.Str(dataColumn2.Prefix)));
				}
				if (TableMethodGenerator.columnNamespaceProperty.ShouldSerializeValue(dataColumn2))
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "Namespace"), CodeGenHelper.Str(dataColumn2.Namespace)));
				}
				if (dataColumn2.Caption != dataColumn2.ColumnName)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "Caption"), CodeGenHelper.Str(dataColumn2.Caption)));
				}
				if (dataColumn2.DefaultValue != DBNull.Value)
				{
					CodeExpression codeExpression3 = null;
					CodeExpression codeExpression4 = null;
					DesignColumn designColumn = this.codeGenerator.TableHandler.Tables[dataTable.TableName].DesignColumns[dataColumn2.ColumnName];
					DSGeneratorProblem dsgeneratorProblem = CodeGenHelper.GenerateValueExprAndFieldInit(designColumn, dataColumn2.DefaultValue, dataColumn2.DefaultValue, this.designTable.GeneratorTableClassName, generatorColumnVarNameInTable2 + "_defaultValue", out codeExpression3, out codeExpression4);
					if (dsgeneratorProblem != null)
					{
						this.codeGenerator.ProblemList.Add(dsgeneratorProblem);
					}
					else
					{
						if (codeExpression4 != null)
						{
							CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.Type(dataColumn2.DataType.FullName), generatorColumnVarNameInTable2 + "_defaultValue");
							codeMemberField.Attributes = (MemberAttributes)20483;
							codeMemberField.InitExpression = codeExpression4;
							tableClass.Members.Add(codeMemberField);
						}
						CodeCastExpression codeCastExpression = new CodeCastExpression(dataColumn2.DataType, codeExpression3);
						codeCastExpression.UserData.Add("CastIsBoxing", true);
						tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "DefaultValue"), codeCastExpression));
					}
				}
				if (dataColumn2.MaxLength != -1)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "MaxLength"), CodeGenHelper.Primitive(dataColumn2.MaxLength)));
				}
				if (dataColumn2.DateTimeMode != DataSetDateTime.UnspecifiedLocal)
				{
					tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "DateTimeMode"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataSetDateTime)), dataColumn2.DateTimeMode.ToString())));
				}
			}
			if (TableMethodGenerator.caseSensitiveProperty.ShouldSerializeValue(dataTable))
			{
				tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "CaseSensitive"), CodeGenHelper.Primitive(dataTable.CaseSensitive)));
			}
			CultureInfo locale = dataTable.Locale;
			if (locale != null && TableMethodGenerator.localeProperty.ShouldSerializeValue(dataTable))
			{
				tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Locale"), CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(CultureInfo)), new CodeExpression[] { CodeGenHelper.Str(dataTable.Locale.ToString()) })));
			}
			if (!StringUtil.Empty(dataTable.Prefix))
			{
				tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Prefix"), CodeGenHelper.Str(dataTable.Prefix)));
			}
			if (TableMethodGenerator.namespaceProperty.ShouldSerializeValue(dataTable))
			{
				tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Namespace"), CodeGenHelper.Str(dataTable.Namespace)));
			}
			if (dataTable.MinimumCapacity != 50)
			{
				tableInitClass.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "MinimumCapacity"), CodeGenHelper.Primitive(dataTable.MinimumCapacity)));
			}
			ExtendedPropertiesHandler.CodeGenerator = this.codeGenerator;
			ExtendedPropertiesHandler.AddExtendedProperties(this.designTable, CodeGenHelper.This(), tableInitClass.Statements, dataTable.ExtendedProperties);
		}

		private CodeMemberMethod NewTypedRowMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.Type(this.rowConcreteClassName), NameHandler.FixIdName("New" + this.rowClassName), (MemberAttributes)24578);
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Cast(CodeGenHelper.Type(this.rowConcreteClassName), CodeGenHelper.MethodCall(CodeGenHelper.This(), "NewRow"))));
			return codeMemberMethod;
		}

		private CodeMemberMethod NewRowFromBuilderMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(DataRow)), "NewRowFromBuilder", (MemberAttributes)12292);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRowBuilder)), "builder"));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.New(CodeGenHelper.Type(this.rowConcreteClassName), new CodeExpression[] { CodeGenHelper.Argument("builder") })));
			return codeMemberMethod;
		}

		private CodeMemberMethod GetRowTypeMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(Type)), "GetRowType", (MemberAttributes)12292);
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.TypeOf(CodeGenHelper.Type(this.rowConcreteClassName))));
			return codeMemberMethod;
		}

		private CodeMemberMethod CreateOnRowEventMethod(string eventName, string typedEventName)
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "OnRow" + eventName, (MemberAttributes)12292);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(DataRowChangeEventArgs)), "e"));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Base(), "OnRow" + eventName, CodeGenHelper.Argument("e")));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Event(typedEventName), CodeGenHelper.Primitive(null)), CodeGenHelper.Stm(CodeGenHelper.DelegateCall(CodeGenHelper.Event(typedEventName), CodeGenHelper.New(CodeGenHelper.Type(this.designTable.GeneratorRowEvArgName), new CodeExpression[]
			{
				CodeGenHelper.Cast(CodeGenHelper.Type(this.rowClassName), CodeGenHelper.Property(CodeGenHelper.Argument("e"), "Row")),
				CodeGenHelper.Property(CodeGenHelper.Argument("e"), "Action")
			})))));
			return codeMemberMethod;
		}

		private void AddOnRowEventMethods(CodeTypeDeclaration dataTableClass)
		{
			dataTableClass.Members.Add(this.CreateOnRowEventMethod("Changed", this.designTable.GeneratorRowChangedName));
			dataTableClass.Members.Add(this.CreateOnRowEventMethod("Changing", this.designTable.GeneratorRowChangingName));
			dataTableClass.Members.Add(this.CreateOnRowEventMethod("Deleted", this.designTable.GeneratorRowDeletedName));
			dataTableClass.Members.Add(this.CreateOnRowEventMethod("Deleting", this.designTable.GeneratorRowDeletingName));
		}

		private CodeMemberMethod RemoveRowMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), NameHandler.FixIdName("Remove" + this.rowClassName), (MemberAttributes)24578);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.Type(this.rowConcreteClassName), "row"));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Rows"), "Remove", CodeGenHelper.Argument("row")));
			return codeMemberMethod;
		}

		private bool ChildRelationFollowable(DataRelation relation)
		{
			return relation != null && (relation.ChildTable != relation.ParentTable || relation.ChildTable.Columns.Count != 1);
		}

		private DataRelation FindParentRelation(DataColumn column)
		{
			DataRelation[] array = new DataRelation[column.Table.ParentRelations.Count];
			column.Table.ParentRelations.CopyTo(array, 0);
			foreach (DataRelation dataRelation in array)
			{
				if (dataRelation.ChildColumns.Length == 1 && dataRelation.ChildColumns[0] == column)
				{
					return dataRelation;
				}
			}
			return null;
		}

		private CodeMemberMethod GetTypedTableSchema()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaComplexType)), "GetTypedTableSchema", (MemberAttributes)24579);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaSet)), "xs"));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaComplexType)), "type", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaComplexType)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaSequence)), "sequence", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaSequence)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(this.codeGenerator.DataSourceName), "ds", CodeGenHelper.New(CodeGenHelper.Type(this.codeGenerator.DataSourceName), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaAny)), "any1", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaAny)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any1"), "Namespace"), CodeGenHelper.Str("http://www.w3.org/2001/XMLSchema")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any1"), "MinOccurs"), CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(decimal)), new CodeExpression[] { CodeGenHelper.Primitive(0) })));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any1"), "MaxOccurs"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(decimal)), "MaxValue")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any1"), "ProcessContents"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(XmlSchemaContentProcessing)), "Lax")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable("sequence"), "Items"), "Add", new CodeExpression[] { CodeGenHelper.Variable("any1") })));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaAny)), "any2", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaAny)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any2"), "Namespace"), CodeGenHelper.Str("urn:schemas-microsoft-com:xml-diffgram-v1")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any2"), "MinOccurs"), CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(decimal)), new CodeExpression[] { CodeGenHelper.Primitive(1) })));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any2"), "ProcessContents"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(XmlSchemaContentProcessing)), "Lax")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable("sequence"), "Items"), "Add", new CodeExpression[] { CodeGenHelper.Variable("any2") })));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaAttribute)), "attribute1", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaAttribute)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("attribute1"), "Name"), CodeGenHelper.Primitive("namespace")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("attribute1"), "FixedValue"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Namespace")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable("type"), "Attributes"), "Add", new CodeExpression[] { CodeGenHelper.Variable("attribute1") })));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaAttribute)), "attribute2", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaAttribute)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("attribute2"), "Name"), CodeGenHelper.Primitive("tableTypeName")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("attribute2"), "FixedValue"), CodeGenHelper.Str(this.designTable.GeneratorTableClassName)));
			codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable("type"), "Attributes"), "Add", new CodeExpression[] { CodeGenHelper.Variable("attribute2") })));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("type"), "Particle"), CodeGenHelper.Variable("sequence")));
			DatasetMethodGenerator.GetSchemaIsInCollection(codeMemberMethod.Statements, "ds", "xs");
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable("type")));
			return codeMemberMethod;
		}

		private TypedDataSourceCodeGenerator codeGenerator;

		private DesignTable designTable;

		private string rowClassName;

		private string rowConcreteClassName;

		private string tableClassName;

		private CodeMemberMethod initExpressionsMethod;

		private static PropertyDescriptor namespaceProperty = TypeDescriptor.GetProperties(typeof(DataTable))["Namespace"];

		private static PropertyDescriptor localeProperty = TypeDescriptor.GetProperties(typeof(DataTable))["Locale"];

		private static PropertyDescriptor caseSensitiveProperty = TypeDescriptor.GetProperties(typeof(DataTable))["CaseSensitive"];

		private static PropertyDescriptor columnNamespaceProperty = TypeDescriptor.GetProperties(typeof(DataColumn))["Namespace"];

		private static PropertyDescriptor dateTimeModeProperty = TypeDescriptor.GetProperties(typeof(DataColumn))["DateTimeMode"];

		private static string columnValuesArrayName = "columnValuesArray";
	}
}
