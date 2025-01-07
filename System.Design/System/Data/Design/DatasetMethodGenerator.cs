using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace System.Data.Design
{
	internal sealed class DatasetMethodGenerator
	{
		internal DatasetMethodGenerator(TypedDataSourceCodeGenerator codeGenerator, DesignDataSource dataSource)
		{
			this.codeGenerator = codeGenerator;
			this.dataSource = dataSource;
			this.dataSet = dataSource.DataSet;
		}

		internal void AddMethods(CodeTypeDeclaration dataSourceClass)
		{
			this.AddSchemaSerializationModeMembers(dataSourceClass);
			this.initExpressionsMethod = this.InitExpressionsMethod();
			dataSourceClass.Members.Add(this.PublicConstructor());
			dataSourceClass.Members.Add(this.DeserializingConstructor());
			dataSourceClass.Members.Add(this.InitializeDerivedDataSet());
			dataSourceClass.Members.Add(this.CloneMethod(this.initExpressionsMethod));
			dataSourceClass.Members.Add(this.ShouldSerializeTablesMethod());
			dataSourceClass.Members.Add(this.ShouldSerializeRelationsMethod());
			dataSourceClass.Members.Add(this.ReadXmlSerializableMethod());
			dataSourceClass.Members.Add(this.GetSchemaSerializableMethod());
			dataSourceClass.Members.Add(this.InitVarsParamLess());
			CodeMemberMethod codeMemberMethod = null;
			CodeMemberMethod codeMemberMethod2 = null;
			this.InitClassAndInitVarsMethods(out codeMemberMethod, out codeMemberMethod2);
			dataSourceClass.Members.Add(codeMemberMethod2);
			dataSourceClass.Members.Add(codeMemberMethod);
			this.AddShouldSerializeSingleTableMethods(dataSourceClass);
			dataSourceClass.Members.Add(this.SchemaChangedMethod());
			dataSourceClass.Members.Add(this.GetTypedDataSetSchema());
			dataSourceClass.Members.Add(this.TablesProperty());
			dataSourceClass.Members.Add(this.RelationsProperty());
			if (this.initExpressionsMethod != null)
			{
				dataSourceClass.Members.Add(this.initExpressionsMethod);
			}
		}

		private void AddSchemaSerializationModeMembers(CodeTypeDeclaration dataSourceClass)
		{
			CodeMemberField codeMemberField = CodeGenHelper.FieldDecl(CodeGenHelper.GlobalType(typeof(SchemaSerializationMode)), "_schemaSerializationMode", CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(SchemaSerializationMode)), this.dataSource.SchemaSerializationMode.ToString()));
			dataSourceClass.Members.Add(codeMemberField);
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(SchemaSerializationMode)), "SchemaSerializationMode", (MemberAttributes)24580);
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(BrowsableAttribute).FullName, CodeGenHelper.Primitive(true)));
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl(typeof(DesignerSerializationVisibilityAttribute).FullName, CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DesignerSerializationVisibility)), "Visible")));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Field(CodeGenHelper.This(), "_schemaSerializationMode")));
			codeMemberProperty.SetStatements.Add(CodeGenHelper.Assign(CodeGenHelper.Field(CodeGenHelper.This(), "_schemaSerializationMode"), CodeGenHelper.Argument("value")));
			dataSourceClass.Members.Add(codeMemberProperty);
		}

		private CodeConstructor PublicConstructor()
		{
			CodeConstructor codeConstructor = CodeGenHelper.Constructor(MemberAttributes.Public);
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "BeginInit"));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitClass"));
			codeConstructor.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(CollectionChangeEventHandler)), "schemaChangedHandler", new CodeDelegateCreateExpression(CodeGenHelper.GlobalType(typeof(CollectionChangeEventHandler)), CodeGenHelper.This(), "SchemaChanged")));
			codeConstructor.Statements.Add(new CodeAttachEventStatement(new CodeEventReferenceExpression(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables"), "CollectionChanged"), CodeGenHelper.Variable("schemaChangedHandler")));
			codeConstructor.Statements.Add(new CodeAttachEventStatement(new CodeEventReferenceExpression(CodeGenHelper.Property(CodeGenHelper.Base(), "Relations"), "CollectionChanged"), CodeGenHelper.Variable("schemaChangedHandler")));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "EndInit"));
			if (this.initExpressionsMethod != null)
			{
				codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitExpressions"));
			}
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
				CodeGenHelper.Argument("context"),
				CodeGenHelper.Primitive(false)
			});
			List<CodeStatement> list = new List<CodeStatement>();
			list.AddRange(new CodeStatement[]
			{
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars", CodeGenHelper.Primitive(false))),
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(CollectionChangeEventHandler)), "schemaChangedHandler1", new CodeDelegateCreateExpression(CodeGenHelper.GlobalType(typeof(CollectionChangeEventHandler)), CodeGenHelper.This(), "SchemaChanged")),
				new CodeAttachEventStatement(new CodeEventReferenceExpression(CodeGenHelper.Property(CodeGenHelper.This(), "Tables"), "CollectionChanged"), CodeGenHelper.Variable("schemaChangedHandler1")),
				new CodeAttachEventStatement(new CodeEventReferenceExpression(CodeGenHelper.Property(CodeGenHelper.This(), "Relations"), "CollectionChanged"), CodeGenHelper.Variable("schemaChangedHandler1"))
			});
			if (this.initExpressionsMethod != null)
			{
				list.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.This(), "DetermineSchemaSerializationMode", new CodeExpression[]
				{
					CodeGenHelper.Argument("info"),
					CodeGenHelper.Argument("context")
				}), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(SchemaSerializationMode)), "ExcludeSchema")), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitExpressions"))));
			}
			list.Add(CodeGenHelper.Return());
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.This(), "IsBinarySerialized", new CodeExpression[]
			{
				CodeGenHelper.Argument("info"),
				CodeGenHelper.Argument("context")
			}), CodeGenHelper.Primitive(true)), list.ToArray()));
			codeConstructor.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(string)), "strSchema", CodeGenHelper.Cast(CodeGenHelper.GlobalType(typeof(string)), CodeGenHelper.MethodCall(CodeGenHelper.Argument("info"), "GetValue", new CodeExpression[]
			{
				CodeGenHelper.Str("XmlSchema"),
				CodeGenHelper.TypeOf(CodeGenHelper.GlobalType(typeof(string)))
			}))));
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			arrayList.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataSet)), "ds", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(DataSet)), new CodeExpression[0])));
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("ds"), "ReadXmlSchema", new CodeExpression[] { CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlTextReader)), new CodeExpression[] { CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(StringReader)), new CodeExpression[] { CodeGenHelper.Variable("strSchema") }) }) })));
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				arrayList.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Tables"), CodeGenHelper.Str(designTable.Name)), CodeGenHelper.Primitive(null)), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables"), "Add", CodeGenHelper.New(CodeGenHelper.Type(designTable.GeneratorTableClassName), new CodeExpression[] { CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Tables"), CodeGenHelper.Str(designTable.Name)) })))));
			}
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "DataSetName"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "DataSetName")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Prefix"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Prefix")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Namespace"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Namespace")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Locale"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Locale")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "CaseSensitive"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "CaseSensitive")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "EnforceConstraints"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "EnforceConstraints")));
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "Merge", new CodeExpression[]
			{
				CodeGenHelper.Variable("ds"),
				CodeGenHelper.Primitive(false),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(MissingSchemaAction)), "Add")
			})));
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars")));
			arrayList2.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "ReadXmlSchema", new CodeExpression[] { CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlTextReader)), new CodeExpression[] { CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(StringReader)), new CodeExpression[] { CodeGenHelper.Variable("strSchema") }) }) })));
			if (this.initExpressionsMethod != null)
			{
				arrayList2.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitExpressions")));
			}
			codeConstructor.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.This(), "DetermineSchemaSerializationMode", new CodeExpression[]
			{
				CodeGenHelper.Argument("info"),
				CodeGenHelper.Argument("context")
			}), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(SchemaSerializationMode)), "IncludeSchema")), (CodeStatement[])arrayList.ToArray(typeof(CodeStatement)), (CodeStatement[])arrayList2.ToArray(typeof(CodeStatement))));
			codeConstructor.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "GetSerializationData", new CodeExpression[]
			{
				CodeGenHelper.Argument("info"),
				CodeGenHelper.Argument("context")
			}));
			codeConstructor.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(CollectionChangeEventHandler)), "schemaChangedHandler", new CodeDelegateCreateExpression(CodeGenHelper.GlobalType(typeof(CollectionChangeEventHandler)), CodeGenHelper.This(), "SchemaChanged")));
			codeConstructor.Statements.Add(new CodeAttachEventStatement(new CodeEventReferenceExpression(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables"), "CollectionChanged"), CodeGenHelper.Variable("schemaChangedHandler")));
			codeConstructor.Statements.Add(new CodeAttachEventStatement(new CodeEventReferenceExpression(CodeGenHelper.Property(CodeGenHelper.This(), "Relations"), "CollectionChanged"), CodeGenHelper.Variable("schemaChangedHandler")));
			return codeConstructor;
		}

		private CodeMemberMethod InitializeDerivedDataSet()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitializeDerivedDataSet", (MemberAttributes)12292);
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "BeginInit"));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitClass"));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "EndInit"));
			return codeMemberMethod;
		}

		private CodeMemberMethod CloneMethod(CodeMemberMethod initExpressionsMethod)
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(DataSet)), "Clone", (MemberAttributes)24580);
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(this.codeGenerator.DataSourceName), "cln", CodeGenHelper.Cast(CodeGenHelper.Type(this.codeGenerator.DataSourceName), CodeGenHelper.MethodCall(CodeGenHelper.Base(), "Clone", new CodeExpression[0]))));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Variable("cln"), "InitVars", new CodeExpression[0]));
			if (initExpressionsMethod != null)
			{
				codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Variable("cln"), "InitExpressions", new CodeExpression[0]));
			}
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("cln"), "SchemaSerializationMode"), CodeGenHelper.Property(CodeGenHelper.This(), "SchemaSerializationMode")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable("cln")));
			return codeMemberMethod;
		}

		private CodeMemberMethod ShouldSerializeTablesMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(bool)), "ShouldSerializeTables", (MemberAttributes)12292);
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(false)));
			return codeMemberMethod;
		}

		private CodeMemberMethod ShouldSerializeRelationsMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(bool)), "ShouldSerializeRelations", (MemberAttributes)12292);
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(false)));
			return codeMemberMethod;
		}

		private CodeMemberMethod ReadXmlSerializableMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "ReadXmlSerializable", (MemberAttributes)12292);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(XmlReader)), "reader"));
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "Reset", new CodeExpression[0])));
			arrayList.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(DataSet)), "ds", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(DataSet)), new CodeExpression[0])));
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("ds"), "ReadXml", new CodeExpression[] { CodeGenHelper.Argument("reader") })));
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				arrayList.Add(CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Tables"), CodeGenHelper.Str(designTable.Name)), CodeGenHelper.Primitive(null)), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables"), "Add", CodeGenHelper.New(CodeGenHelper.Type(designTable.GeneratorTableClassName), new CodeExpression[] { CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Tables"), CodeGenHelper.Str(designTable.Name)) })))));
			}
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "DataSetName"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "DataSetName")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Prefix"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Prefix")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Namespace"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Namespace")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Locale"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Locale")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "CaseSensitive"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "CaseSensitive")));
			arrayList.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "EnforceConstraints"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "EnforceConstraints")));
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "Merge", new CodeExpression[]
			{
				CodeGenHelper.Variable("ds"),
				CodeGenHelper.Primitive(false),
				CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(MissingSchemaAction)), "Add")
			})));
			arrayList.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars")));
			arrayList2.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "ReadXml", new CodeExpression[] { CodeGenHelper.Argument("reader") })));
			arrayList2.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars")));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.This(), "DetermineSchemaSerializationMode", new CodeExpression[] { CodeGenHelper.Argument("reader") }), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(SchemaSerializationMode)), "IncludeSchema")), (CodeStatement[])arrayList.ToArray(typeof(CodeStatement)), (CodeStatement[])arrayList2.ToArray(typeof(CodeStatement))));
			return codeMemberMethod;
		}

		private CodeMemberMethod GetSchemaSerializableMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(XmlSchema)), "GetSchemaSerializable", (MemberAttributes)12292);
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(MemoryStream)), "stream", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(MemoryStream)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "WriteXmlSchema", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlTextWriter)), new CodeExpression[]
			{
				CodeGenHelper.Argument("stream"),
				CodeGenHelper.Primitive(null)
			})));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Argument("stream"), "Position"), CodeGenHelper.Primitive(0)));
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.MethodCall(CodeGenHelper.GlobalTypeExpr(typeof(XmlSchema)), "Read", new CodeExpression[]
			{
				CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlTextReader)), new CodeExpression[] { CodeGenHelper.Argument("stream") }),
				CodeGenHelper.Primitive(null)
			})));
			return codeMemberMethod;
		}

		private CodeMemberMethod InitVarsParamLess()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitVars", (MemberAttributes)4098);
			codeMemberMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars", new CodeExpression[] { CodeGenHelper.Primitive(true) }));
			return codeMemberMethod;
		}

		private void InitClassAndInitVarsMethods(out CodeMemberMethod initClassMethod, out CodeMemberMethod initVarsMethod)
		{
			initClassMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitClass", MemberAttributes.Private);
			initVarsMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitVars", (MemberAttributes)4098);
			initVarsMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(bool)), "initTable"));
			initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "DataSetName"), CodeGenHelper.Str(this.dataSet.DataSetName)));
			initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Prefix"), CodeGenHelper.Str(this.dataSet.Prefix)));
			if (DatasetMethodGenerator.namespaceProperty.ShouldSerializeValue(this.dataSet))
			{
				initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Namespace"), CodeGenHelper.Str(this.dataSet.Namespace)));
			}
			if (DatasetMethodGenerator.localeProperty.ShouldSerializeValue(this.dataSet))
			{
				initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "Locale"), CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(CultureInfo)), new CodeExpression[] { CodeGenHelper.Str(this.dataSet.Locale.ToString()) })));
			}
			if (DatasetMethodGenerator.caseSensitiveProperty.ShouldSerializeValue(this.dataSet))
			{
				initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "CaseSensitive"), CodeGenHelper.Primitive(this.dataSet.CaseSensitive)));
			}
			initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "EnforceConstraints"), CodeGenHelper.Primitive(this.dataSet.EnforceConstraints)));
			initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.This(), "SchemaSerializationMode"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(SchemaSerializationMode)), this.dataSource.SchemaSerializationMode.ToString())));
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				CodeExpression codeExpression = CodeGenHelper.Field(CodeGenHelper.This(), designTable.GeneratorTableVarName);
				if (this.TableContainsExpressions(designTable))
				{
					initClassMethod.Statements.Add(CodeGenHelper.Assign(codeExpression, CodeGenHelper.New(CodeGenHelper.Type(designTable.GeneratorTableClassName), new CodeExpression[] { CodeGenHelper.Primitive(false) })));
				}
				else
				{
					initClassMethod.Statements.Add(CodeGenHelper.Assign(codeExpression, CodeGenHelper.New(CodeGenHelper.Type(designTable.GeneratorTableClassName), new CodeExpression[0])));
				}
				initClassMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables"), "Add", codeExpression));
				initVarsMethod.Statements.Add(CodeGenHelper.Assign(codeExpression, CodeGenHelper.Cast(CodeGenHelper.Type(designTable.GeneratorTableClassName), CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables"), CodeGenHelper.Str(designTable.Name)))));
				initVarsMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Variable("initTable"), CodeGenHelper.Primitive(true)), new CodeStatement[] { CodeGenHelper.If(CodeGenHelper.IdNotEQ(codeExpression, CodeGenHelper.Primitive(null)), CodeGenHelper.Stm(CodeGenHelper.MethodCall(codeExpression, "InitVars"))) }));
			}
			CodeExpression codeExpression2 = null;
			foreach (object obj2 in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable2 = (DesignTable)obj2;
				DataTable dataTable = designTable2.DataTable;
				foreach (object obj3 in dataTable.Constraints)
				{
					Constraint constraint = (Constraint)obj3;
					if (constraint is ForeignKeyConstraint)
					{
						ForeignKeyConstraint foreignKeyConstraint = (ForeignKeyConstraint)constraint;
						CodeArrayCreateExpression codeArrayCreateExpression = new CodeArrayCreateExpression(CodeGenHelper.GlobalType(typeof(DataColumn)), 0);
						foreach (DataColumn dataColumn in foreignKeyConstraint.Columns)
						{
							codeArrayCreateExpression.Initializers.Add(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), this.codeGenerator.TableHandler.Tables[dataColumn.Table.TableName].GeneratorTableVarName), this.codeGenerator.TableHandler.Tables[dataColumn.Table.TableName].DesignColumns[dataColumn.ColumnName].GeneratorColumnPropNameInTable));
						}
						CodeArrayCreateExpression codeArrayCreateExpression2 = new CodeArrayCreateExpression(CodeGenHelper.GlobalType(typeof(DataColumn)), 0);
						foreach (DataColumn dataColumn2 in foreignKeyConstraint.RelatedColumns)
						{
							codeArrayCreateExpression2.Initializers.Add(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), this.codeGenerator.TableHandler.Tables[dataColumn2.Table.TableName].GeneratorTableVarName), this.codeGenerator.TableHandler.Tables[dataColumn2.Table.TableName].DesignColumns[dataColumn2.ColumnName].GeneratorColumnPropNameInTable));
						}
						if (codeExpression2 == null)
						{
							initClassMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(ForeignKeyConstraint)), "fkc"));
							codeExpression2 = CodeGenHelper.Variable("fkc");
						}
						initClassMethod.Statements.Add(CodeGenHelper.Assign(codeExpression2, CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(ForeignKeyConstraint)), new CodeExpression[]
						{
							CodeGenHelper.Str(foreignKeyConstraint.ConstraintName),
							codeArrayCreateExpression2,
							codeArrayCreateExpression
						})));
						initClassMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), this.codeGenerator.TableHandler.Tables[dataTable.TableName].GeneratorTableVarName), "Constraints"), "Add", codeExpression2));
						string text = foreignKeyConstraint.AcceptRejectRule.ToString();
						string text2 = foreignKeyConstraint.DeleteRule.ToString();
						string text3 = foreignKeyConstraint.UpdateRule.ToString();
						initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "AcceptRejectRule"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(foreignKeyConstraint.AcceptRejectRule.GetType()), text)));
						initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "DeleteRule"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(foreignKeyConstraint.DeleteRule.GetType()), text2)));
						initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression2, "UpdateRule"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(foreignKeyConstraint.UpdateRule.GetType()), text3)));
					}
				}
			}
			foreach (object obj4 in this.codeGenerator.RelationHandler.Relations)
			{
				DesignRelation designRelation = (DesignRelation)obj4;
				DataRelation dataRelation = designRelation.DataRelation;
				if (dataRelation != null)
				{
					CodeArrayCreateExpression codeArrayCreateExpression3 = new CodeArrayCreateExpression(CodeGenHelper.GlobalType(typeof(DataColumn)), 0);
					string generatorTableVarName = designRelation.ParentDesignTable.GeneratorTableVarName;
					foreach (DataColumn dataColumn3 in dataRelation.ParentColumns)
					{
						codeArrayCreateExpression3.Initializers.Add(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName), this.codeGenerator.TableHandler.Tables[dataColumn3.Table.TableName].DesignColumns[dataColumn3.ColumnName].GeneratorColumnPropNameInTable));
					}
					CodeArrayCreateExpression codeArrayCreateExpression4 = new CodeArrayCreateExpression(CodeGenHelper.GlobalType(typeof(DataColumn)), 0);
					string generatorTableVarName2 = designRelation.ChildDesignTable.GeneratorTableVarName;
					foreach (DataColumn dataColumn4 in dataRelation.ChildColumns)
					{
						codeArrayCreateExpression4.Initializers.Add(CodeGenHelper.Property(CodeGenHelper.Field(CodeGenHelper.This(), generatorTableVarName2), this.codeGenerator.TableHandler.Tables[dataColumn4.Table.TableName].DesignColumns[dataColumn4.ColumnName].GeneratorColumnPropNameInTable));
					}
					CodeExpression codeExpression3 = CodeGenHelper.Field(CodeGenHelper.This(), this.codeGenerator.RelationHandler.Relations[dataRelation.RelationName].GeneratorRelationVarName);
					initClassMethod.Statements.Add(CodeGenHelper.Assign(codeExpression3, CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(DataRelation)), new CodeExpression[]
					{
						CodeGenHelper.Str(dataRelation.RelationName),
						codeArrayCreateExpression3,
						codeArrayCreateExpression4,
						CodeGenHelper.Primitive(false)
					})));
					if (dataRelation.Nested)
					{
						initClassMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression3, "Nested"), CodeGenHelper.Primitive(true)));
					}
					ExtendedPropertiesHandler.CodeGenerator = this.codeGenerator;
					ExtendedPropertiesHandler.AddExtendedProperties(designRelation, codeExpression3, initClassMethod.Statements, dataRelation.ExtendedProperties);
					initClassMethod.Statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.This(), "Relations"), "Add", codeExpression3));
					initVarsMethod.Statements.Add(CodeGenHelper.Assign(codeExpression3, CodeGenHelper.Indexer(CodeGenHelper.Property(CodeGenHelper.This(), "Relations"), CodeGenHelper.Str(dataRelation.RelationName))));
				}
			}
			ExtendedPropertiesHandler.CodeGenerator = this.codeGenerator;
			ExtendedPropertiesHandler.AddExtendedProperties(this.dataSource, CodeGenHelper.This(), initClassMethod.Statements, this.dataSet.ExtendedProperties);
		}

		private void AddShouldSerializeSingleTableMethods(CodeTypeDeclaration dataSourceClass)
		{
			foreach (object obj in this.codeGenerator.TableHandler.Tables)
			{
				DesignTable designTable = (DesignTable)obj;
				string generatorTablePropName = designTable.GeneratorTablePropName;
				string text = MemberNameValidator.GenerateIdName("ShouldSerialize" + generatorTablePropName, this.codeGenerator.CodeProvider, false);
				CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(bool)), text, MemberAttributes.Private);
				codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Primitive(false)));
				dataSourceClass.Members.Add(codeMemberMethod);
			}
		}

		private CodeMemberMethod SchemaChangedMethod()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "SchemaChanged", MemberAttributes.Private);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(object)), "sender"));
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(CollectionChangeEventArgs)), "e"));
			codeMemberMethod.Statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Property(CodeGenHelper.Argument("e"), "Action"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(CollectionChangeAction)), "Remove")), CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.This(), "InitVars"))));
			return codeMemberMethod;
		}

		internal static void GetSchemaIsInCollection(CodeStatementCollection statements, string dsName, string collectionName)
		{
			CodeStatement[] array = new CodeStatement[]
			{
				CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("s1"), "Position"), CodeGenHelper.Primitive(0)),
				CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("s2"), "Position"), CodeGenHelper.Primitive(0)),
				CodeGenHelper.ForLoop(CodeGenHelper.Stm(new CodeSnippetExpression("")), CodeGenHelper.And(CodeGenHelper.IdNotEQ(CodeGenHelper.Property(CodeGenHelper.Variable("s1"), "Position"), CodeGenHelper.Property(CodeGenHelper.Variable("s1"), "Length")), CodeGenHelper.EQ(CodeGenHelper.MethodCall(CodeGenHelper.Variable("s1"), "ReadByte", new CodeExpression[0]), CodeGenHelper.MethodCall(CodeGenHelper.Variable("s2"), "ReadByte", new CodeExpression[0]))), CodeGenHelper.Stm(new CodeSnippetExpression("")), new CodeStatement[] { CodeGenHelper.Stm(new CodeSnippetExpression("")) }),
				CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Property(CodeGenHelper.Variable("s1"), "Position"), CodeGenHelper.Property(CodeGenHelper.Variable("s1"), "Length")), new CodeStatement[] { CodeGenHelper.Return(CodeGenHelper.Variable("type")) })
			};
			CodeStatement[] array2 = new CodeStatement[]
			{
				CodeGenHelper.Assign(CodeGenHelper.Variable("schema"), CodeGenHelper.Cast(CodeGenHelper.GlobalType(typeof(XmlSchema)), CodeGenHelper.Property(CodeGenHelper.Variable("schemas"), "Current"))),
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("s2"), "SetLength", new CodeExpression[] { CodeGenHelper.Primitive(0) })),
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("schema"), "Write", new CodeExpression[] { CodeGenHelper.Variable("s2") })),
				CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Property(CodeGenHelper.Variable("s1"), "Length"), CodeGenHelper.Property(CodeGenHelper.Variable("s2"), "Length")), array)
			};
			CodeStatement[] array3 = new CodeStatement[]
			{
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchema)), "schema", CodeGenHelper.Primitive(null)),
				CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("dsSchema"), "Write", new CodeExpression[] { CodeGenHelper.Variable("s1") })),
				CodeGenHelper.ForLoop(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(IEnumerator)), "schemas", CodeGenHelper.MethodCall(CodeGenHelper.MethodCall(CodeGenHelper.Variable(collectionName), "Schemas", new CodeExpression[] { CodeGenHelper.Property(CodeGenHelper.Variable("dsSchema"), "TargetNamespace") }), "GetEnumerator", new CodeExpression[0])), CodeGenHelper.MethodCall(CodeGenHelper.Variable("schemas"), "MoveNext", new CodeExpression[0]), CodeGenHelper.Stm(new CodeSnippetExpression("")), array2)
			};
			CodeStatement[] array4 = new CodeStatement[]
			{
				CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Variable("s1"), CodeGenHelper.Primitive(null)), new CodeStatement[] { CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("s1"), "Close", new CodeExpression[0])) }),
				CodeGenHelper.If(CodeGenHelper.IdNotEQ(CodeGenHelper.Variable("s2"), CodeGenHelper.Primitive(null)), new CodeStatement[] { CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Variable("s2"), "Close", new CodeExpression[0])) })
			};
			CodeStatement[] array5 = new CodeStatement[]
			{
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(MemoryStream)), "s1", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(MemoryStream)), new CodeExpression[0])),
				CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(MemoryStream)), "s2", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(MemoryStream)), new CodeExpression[0])),
				CodeGenHelper.Try(array3, new CodeCatchClause[0], array4)
			};
			statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchema)), "dsSchema", CodeGenHelper.MethodCall(CodeGenHelper.Variable(dsName), "GetSchemaSerializable", new CodeExpression[0])));
			statements.Add(CodeGenHelper.If(CodeGenHelper.MethodCall(CodeGenHelper.Variable(collectionName), "Contains", new CodeExpression[] { CodeGenHelper.Property(CodeGenHelper.Variable("dsSchema"), "TargetNamespace") }), array5));
			statements.Add(CodeGenHelper.MethodCall(CodeGenHelper.Argument("xs"), "Add", new CodeExpression[] { CodeGenHelper.Variable("dsSchema") }));
		}

		private CodeMemberMethod GetTypedDataSetSchema()
		{
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaComplexType)), "GetTypedDataSetSchema", (MemberAttributes)24579);
			codeMemberMethod.Parameters.Add(CodeGenHelper.ParameterDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaSet)), "xs"));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.Type(this.dataSource.GeneratorDataSetName), "ds", CodeGenHelper.New(CodeGenHelper.Type(this.dataSource.GeneratorDataSetName), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaComplexType)), "type", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaComplexType)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaSequence)), "sequence", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaSequence)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(typeof(XmlSchemaAny)), "any", CodeGenHelper.New(CodeGenHelper.GlobalType(typeof(XmlSchemaAny)), new CodeExpression[0])));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("any"), "Namespace"), CodeGenHelper.Property(CodeGenHelper.Variable("ds"), "Namespace")));
			codeMemberMethod.Statements.Add(CodeGenHelper.Stm(CodeGenHelper.MethodCall(CodeGenHelper.Property(CodeGenHelper.Variable("sequence"), "Items"), "Add", new CodeExpression[] { CodeGenHelper.Variable("any") })));
			codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(CodeGenHelper.Variable("type"), "Particle"), CodeGenHelper.Variable("sequence")));
			DatasetMethodGenerator.GetSchemaIsInCollection(codeMemberMethod.Statements, "ds", "xs");
			codeMemberMethod.Statements.Add(CodeGenHelper.Return(CodeGenHelper.Variable("type")));
			return codeMemberMethod;
		}

		private CodeMemberProperty TablesProperty()
		{
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(DataTableCollection)), DataSourceNameHandler.TablesPropertyName, (MemberAttributes)24594);
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerSerializationVisibilityAttribute", CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DesignerSerializationVisibility)), "Hidden")));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Property(CodeGenHelper.Base(), "Tables")));
			return codeMemberProperty;
		}

		private CodeMemberProperty RelationsProperty()
		{
			CodeMemberProperty codeMemberProperty = CodeGenHelper.PropertyDecl(CodeGenHelper.GlobalType(typeof(DataRelationCollection)), DataSourceNameHandler.RelationsPropertyName, (MemberAttributes)24594);
			codeMemberProperty.CustomAttributes.Add(CodeGenHelper.AttributeDecl("System.ComponentModel.DesignerSerializationVisibilityAttribute", CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DesignerSerializationVisibility)), "Hidden")));
			codeMemberProperty.GetStatements.Add(CodeGenHelper.Return(CodeGenHelper.Property(CodeGenHelper.Base(), "Relations")));
			return codeMemberProperty;
		}

		private CodeMemberMethod InitExpressionsMethod()
		{
			bool flag = false;
			CodeMemberMethod codeMemberMethod = CodeGenHelper.MethodDecl(CodeGenHelper.GlobalType(typeof(void)), "InitExpressions", MemberAttributes.Private);
			foreach (object obj in this.dataSource.DesignTables)
			{
				DesignTable designTable = (DesignTable)obj;
				DataTable dataTable = designTable.DataTable;
				foreach (object obj2 in dataTable.Columns)
				{
					DataColumn dataColumn = (DataColumn)obj2;
					if (dataColumn.Expression.Length > 0)
					{
						CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Property(CodeGenHelper.This(), designTable.GeneratorTablePropName), this.codeGenerator.TableHandler.Tables[dataColumn.Table.TableName].DesignColumns[dataColumn.ColumnName].GeneratorColumnPropNameInTable);
						flag = true;
						codeMemberMethod.Statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(codeExpression, "Expression"), CodeGenHelper.Str(dataColumn.Expression)));
					}
				}
			}
			if (flag)
			{
				return codeMemberMethod;
			}
			return null;
		}

		private bool TableContainsExpressions(DesignTable designTable)
		{
			DataTable dataTable = designTable.DataTable;
			foreach (object obj in dataTable.Columns)
			{
				DataColumn dataColumn = (DataColumn)obj;
				if (dataColumn.Expression.Length > 0)
				{
					return true;
				}
			}
			return false;
		}

		private TypedDataSourceCodeGenerator codeGenerator;

		private DesignDataSource dataSource;

		private DataSet dataSet;

		private CodeMemberMethod initExpressionsMethod;

		private static PropertyDescriptor namespaceProperty = TypeDescriptor.GetProperties(typeof(DataSet))["Namespace"];

		private static PropertyDescriptor localeProperty = TypeDescriptor.GetProperties(typeof(DataSet))["Locale"];

		private static PropertyDescriptor caseSensitiveProperty = TypeDescriptor.GetProperties(typeof(DataSet))["CaseSensitive"];
	}
}
