using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Design;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace System.Data.Design
{
	internal abstract class QueryGeneratorBase
	{
		internal QueryGeneratorBase(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.codeGenerator = codeGenerator;
		}

		internal DbProviderFactory ProviderFactory
		{
			get
			{
				return this.providerFactory;
			}
			set
			{
				this.providerFactory = value;
			}
		}

		internal DbSource MethodSource
		{
			get
			{
				return this.methodSource;
			}
			set
			{
				this.methodSource = value;
			}
		}

		internal DbSourceCommand ActiveCommand
		{
			get
			{
				return this.activeCommand;
			}
			set
			{
				this.activeCommand = value;
			}
		}

		internal string MethodName
		{
			get
			{
				return this.methodName;
			}
			set
			{
				this.methodName = value;
			}
		}

		internal ParameterGenerationOption ParameterOption
		{
			get
			{
				return this.parameterOption;
			}
			set
			{
				this.parameterOption = value;
			}
		}

		internal Type ContainerParameterType
		{
			get
			{
				return this.containerParamType;
			}
			set
			{
				this.containerParamType = value;
			}
		}

		internal string ContainerParameterTypeName
		{
			get
			{
				return this.containerParamTypeName;
			}
			set
			{
				this.containerParamTypeName = value;
			}
		}

		internal string ContainerParameterName
		{
			get
			{
				return this.containerParamName;
			}
			set
			{
				this.containerParamName = value;
			}
		}

		internal int CommandIndex
		{
			get
			{
				return this.commandIndex;
			}
			set
			{
				this.commandIndex = value;
			}
		}

		internal DesignTable DesignTable
		{
			get
			{
				return this.designTable;
			}
			set
			{
				this.designTable = value;
			}
		}

		internal bool GenerateGetMethod
		{
			get
			{
				return this.getMethod;
			}
			set
			{
				this.getMethod = value;
			}
		}

		internal bool GeneratePagingMethod
		{
			get
			{
				return this.pagingMethod;
			}
			set
			{
				this.pagingMethod = value;
			}
		}

		internal bool DeclarationOnly
		{
			get
			{
				return this.declarationOnly;
			}
			set
			{
				this.declarationOnly = value;
			}
		}

		internal MethodTypeEnum MethodType
		{
			get
			{
				return this.methodType;
			}
			set
			{
				this.methodType = value;
			}
		}

		internal string UpdateParameterName
		{
			get
			{
				return this.updateParameterName;
			}
			set
			{
				this.updateParameterName = value;
			}
		}

		internal string UpdateParameterTypeName
		{
			get
			{
				return this.updateParameterTypeName;
			}
			set
			{
				this.updateParameterTypeName = value;
			}
		}

		internal CodeTypeReference UpdateParameterTypeReference
		{
			get
			{
				return this.updateParameterTypeReference;
			}
			set
			{
				this.updateParameterTypeReference = value;
			}
		}

		internal CodeDomProvider CodeProvider
		{
			get
			{
				return this.codeProvider;
			}
			set
			{
				this.codeProvider = value;
			}
		}

		internal string UpdateCommandName
		{
			get
			{
				return this.updateCommandName;
			}
			set
			{
				this.updateCommandName = value;
			}
		}

		internal bool IsFunctionsDataComponent
		{
			get
			{
				return this.isFunctionsDataComponent;
			}
			set
			{
				this.isFunctionsDataComponent = value;
			}
		}

		internal static Type SqlCeParameterType
		{
			get
			{
				if (QueryGeneratorBase.sqlCeParameterType == null)
				{
					try
					{
						QueryGeneratorBase.sqlCeParameterType = Type.GetType("System.Data.SqlServerCe.SqlCeParameter, System.Data.SqlServerCe, Version=3.5.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91");
					}
					catch (FileLoadException)
					{
					}
				}
				return QueryGeneratorBase.sqlCeParameterType;
			}
		}

		internal static object SqlCeParameterInstance
		{
			get
			{
				if (QueryGeneratorBase.sqlCeParameterInstance == null && QueryGeneratorBase.SqlCeParameterType != null)
				{
					QueryGeneratorBase.sqlCeParameterInstance = Activator.CreateInstance(QueryGeneratorBase.SqlCeParameterType);
				}
				return QueryGeneratorBase.sqlCeParameterInstance;
			}
		}

		internal static PropertyDescriptor SqlCeParaDbTypeDescriptor
		{
			get
			{
				if (QueryGeneratorBase.sqlCeParaDbTypeDescriptor == null && QueryGeneratorBase.SqlCeParameterType != null)
				{
					QueryGeneratorBase.sqlCeParaDbTypeDescriptor = TypeDescriptor.GetProperties(QueryGeneratorBase.SqlCeParameterType)["DbType"];
				}
				return QueryGeneratorBase.sqlCeParaDbTypeDescriptor;
			}
		}

		internal static CodeStatement SetCommandTextStatement(CodeExpression commandExpression, string commandText)
		{
			return CodeGenHelper.Assign(CodeGenHelper.Property(commandExpression, "CommandText"), CodeGenHelper.Str(commandText));
		}

		internal static CodeStatement SetCommandTypeStatement(CodeExpression commandExpression, CommandType commandType)
		{
			return CodeGenHelper.Assign(CodeGenHelper.Property(commandExpression, "CommandType"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(CommandType)), commandType.ToString()));
		}

		internal abstract CodeMemberMethod Generate();

		protected DesignParameter GetReturnParameter(DbSourceCommand command)
		{
			foreach (object obj in command.Parameters)
			{
				DesignParameter designParameter = (DesignParameter)obj;
				if (designParameter.Direction == ParameterDirection.ReturnValue)
				{
					return designParameter;
				}
			}
			return null;
		}

		protected int GetReturnParameterPosition(DbSourceCommand command)
		{
			if (command == null || command.Parameters == null)
			{
				return -1;
			}
			for (int i = 0; i < command.Parameters.Count; i++)
			{
				if (command.Parameters[i].Direction == ParameterDirection.ReturnValue)
				{
					return i;
				}
			}
			return -1;
		}

		internal static CodeExpression AddNewParameterStatements(DesignParameter parameter, Type parameterType, DbProviderFactory factory, IList statements, CodeExpression parameterVariable)
		{
			if (parameterType == typeof(SqlParameter))
			{
				return QueryGeneratorBase.BuildNewSqlParameterStatement(parameter);
			}
			if (parameterType == typeof(OleDbParameter))
			{
				return QueryGeneratorBase.BuildNewOleDbParameterStatement(parameter);
			}
			if (parameterType == typeof(OdbcParameter))
			{
				return QueryGeneratorBase.BuildNewOdbcParameterStatement(parameter);
			}
			if (parameterType == typeof(OracleParameter))
			{
				return QueryGeneratorBase.BuildNewOracleParameterStatement(parameter);
			}
			if (parameterType == QueryGeneratorBase.SqlCeParameterType && StringUtil.NotEmptyAfterTrim(parameter.ProviderType))
			{
				return QueryGeneratorBase.BuildNewSqlCeParameterStatement(parameter);
			}
			return QueryGeneratorBase.BuildNewUnknownParameterStatements(parameter, parameterType, factory, statements, parameterVariable);
		}

		private static CodeExpression BuildNewSqlParameterStatement(DesignParameter parameter)
		{
			SqlParameter sqlParameter = new SqlParameter();
			SqlDbType sqlDbType = SqlDbType.Char;
			bool flag = false;
			if (parameter.ProviderType != null && parameter.ProviderType.Length > 0)
			{
				try
				{
					sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameter.ProviderType);
					flag = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				sqlParameter.DbType = parameter.DbType;
				sqlDbType = sqlParameter.SqlDbType;
			}
			return QueryGeneratorBase.NewParameter(parameter, typeof(SqlParameter), typeof(SqlDbType), sqlDbType.ToString());
		}

		private static CodeExpression BuildNewSqlCeParameterStatement(DesignParameter parameter)
		{
			SqlDbType sqlDbType = SqlDbType.Char;
			bool flag = false;
			if (parameter.ProviderType != null && parameter.ProviderType.Length > 0)
			{
				try
				{
					sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameter.ProviderType);
					flag = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				object obj = QueryGeneratorBase.SqlCeParameterInstance;
				if (obj != null)
				{
					PropertyDescriptor propertyDescriptor = QueryGeneratorBase.SqlCeParaDbTypeDescriptor;
					if (propertyDescriptor != null)
					{
						propertyDescriptor.SetValue(obj, parameter.DbType);
						sqlDbType = (SqlDbType)propertyDescriptor.GetValue(obj);
					}
				}
			}
			return QueryGeneratorBase.NewParameter(parameter, QueryGeneratorBase.SqlCeParameterType, typeof(SqlDbType), sqlDbType.ToString());
		}

		private static CodeExpression BuildNewOleDbParameterStatement(DesignParameter parameter)
		{
			OleDbParameter oleDbParameter = new OleDbParameter();
			OleDbType oleDbType = OleDbType.Char;
			bool flag = false;
			if (parameter.ProviderType != null && parameter.ProviderType.Length > 0)
			{
				try
				{
					oleDbType = (OleDbType)Enum.Parse(typeof(OleDbType), parameter.ProviderType);
					flag = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				oleDbParameter.DbType = parameter.DbType;
				oleDbType = oleDbParameter.OleDbType;
			}
			return QueryGeneratorBase.NewParameter(parameter, typeof(OleDbParameter), typeof(OleDbType), oleDbType.ToString());
		}

		private static CodeExpression BuildNewOdbcParameterStatement(DesignParameter parameter)
		{
			OdbcParameter odbcParameter = new OdbcParameter();
			OdbcType odbcType = OdbcType.Char;
			bool flag = false;
			if (parameter.ProviderType != null && parameter.ProviderType.Length > 0)
			{
				try
				{
					odbcType = (OdbcType)Enum.Parse(typeof(OdbcType), parameter.ProviderType);
					flag = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				odbcParameter.DbType = parameter.DbType;
				odbcType = odbcParameter.OdbcType;
			}
			return QueryGeneratorBase.NewParameter(parameter, typeof(OdbcParameter), typeof(OdbcType), odbcType.ToString());
		}

		private static CodeExpression BuildNewOracleParameterStatement(DesignParameter parameter)
		{
			OracleParameter oracleParameter = new OracleParameter();
			OracleType oracleType = OracleType.Char;
			bool flag = false;
			if (parameter.ProviderType != null && parameter.ProviderType.Length > 0)
			{
				try
				{
					oracleType = (OracleType)Enum.Parse(typeof(OracleType), parameter.ProviderType);
					flag = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				oracleParameter.DbType = parameter.DbType;
				oracleType = oracleParameter.OracleType;
			}
			return QueryGeneratorBase.NewParameter(parameter, typeof(OracleParameter), typeof(OracleType), oracleType.ToString());
		}

		private static CodeExpression NewParameter(DesignParameter parameter, Type parameterType, Type typeEnumType, string typeEnumValue)
		{
			CodeExpression codeExpression;
			if (parameterType == typeof(SqlParameter))
			{
				codeExpression = CodeGenHelper.New(CodeGenHelper.GlobalType(parameterType), new CodeExpression[]
				{
					CodeGenHelper.Str(parameter.ParameterName),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeEnumType), typeEnumValue),
					CodeGenHelper.Primitive(parameter.Size),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ParameterDirection)), parameter.Direction.ToString()),
					CodeGenHelper.Primitive(parameter.Precision),
					CodeGenHelper.Primitive(parameter.Scale),
					CodeGenHelper.Str(parameter.SourceColumn),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), parameter.SourceVersion.ToString()),
					CodeGenHelper.Primitive(parameter.SourceColumnNullMapping),
					CodeGenHelper.Primitive(null),
					CodeGenHelper.Str(string.Empty),
					CodeGenHelper.Str(string.Empty),
					CodeGenHelper.Str(string.Empty)
				});
			}
			else if (parameterType == QueryGeneratorBase.SqlCeParameterType)
			{
				codeExpression = CodeGenHelper.New(CodeGenHelper.GlobalType(parameterType), new CodeExpression[]
				{
					CodeGenHelper.Str(parameter.ParameterName),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeEnumType), typeEnumValue),
					CodeGenHelper.Primitive(parameter.Size),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ParameterDirection)), parameter.Direction.ToString()),
					CodeGenHelper.Primitive(parameter.IsNullable),
					CodeGenHelper.Primitive(parameter.Precision),
					CodeGenHelper.Primitive(parameter.Scale),
					CodeGenHelper.Str(parameter.SourceColumn),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), parameter.SourceVersion.ToString()),
					CodeGenHelper.Primitive(null)
				});
			}
			else if (parameterType == typeof(OracleParameter))
			{
				codeExpression = CodeGenHelper.New(CodeGenHelper.GlobalType(parameterType), new CodeExpression[]
				{
					CodeGenHelper.Str(parameter.ParameterName),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeEnumType), typeEnumValue),
					CodeGenHelper.Primitive(parameter.Size),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ParameterDirection)), parameter.Direction.ToString()),
					CodeGenHelper.Str(parameter.SourceColumn),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), parameter.SourceVersion.ToString()),
					CodeGenHelper.Primitive(parameter.SourceColumnNullMapping),
					CodeGenHelper.Primitive(null)
				});
			}
			else
			{
				codeExpression = CodeGenHelper.New(CodeGenHelper.GlobalType(parameterType), new CodeExpression[]
				{
					CodeGenHelper.Str(parameter.ParameterName),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeEnumType), typeEnumValue),
					CodeGenHelper.Primitive(parameter.Size),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ParameterDirection)), parameter.Direction.ToString()),
					CodeGenHelper.Cast(CodeGenHelper.GlobalType(typeof(byte)), CodeGenHelper.Primitive(parameter.Precision)),
					CodeGenHelper.Cast(CodeGenHelper.GlobalType(typeof(byte)), CodeGenHelper.Primitive(parameter.Scale)),
					CodeGenHelper.Str(parameter.SourceColumn),
					CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), parameter.SourceVersion.ToString()),
					CodeGenHelper.Primitive(parameter.SourceColumnNullMapping),
					CodeGenHelper.Primitive(null)
				});
			}
			return codeExpression;
		}

		private static bool ParamVariableDeclared(IList statements)
		{
			foreach (object obj in statements)
			{
				if (obj is CodeVariableDeclarationStatement)
				{
					CodeVariableDeclarationStatement codeVariableDeclarationStatement = obj as CodeVariableDeclarationStatement;
					if (codeVariableDeclarationStatement.Name == "param")
					{
						return true;
					}
				}
			}
			return false;
		}

		private static CodeExpression BuildNewUnknownParameterStatements(DesignParameter parameter, Type parameterType, DbProviderFactory factory, IList statements, CodeExpression parameterVariable)
		{
			if (!QueryGeneratorBase.ParamVariableDeclared(statements))
			{
				statements.Add(CodeGenHelper.VariableDecl(CodeGenHelper.GlobalType(parameterType), "param", CodeGenHelper.New(CodeGenHelper.GlobalType(parameterType), new CodeExpression[0])));
				parameterVariable = CodeGenHelper.Variable("param");
			}
			else
			{
				if (parameterVariable == null)
				{
					parameterVariable = CodeGenHelper.Variable("param");
				}
				statements.Add(CodeGenHelper.Assign(parameterVariable, CodeGenHelper.New(CodeGenHelper.GlobalType(parameterType), new CodeExpression[0])));
			}
			IDbDataParameter dbDataParameter = (IDbDataParameter)Activator.CreateInstance(parameterType);
			statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "ParameterName"), CodeGenHelper.Str(parameter.ParameterName)));
			if (parameter.DbType != dbDataParameter.DbType)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "DbType"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DbType)), parameter.DbType.ToString())));
			}
			PropertyInfo providerTypeProperty = ProviderManager.GetProviderTypeProperty(factory);
			if (providerTypeProperty != null && parameter.ProviderType != null && parameter.ProviderType.Length > 0)
			{
				object obj = null;
				try
				{
					obj = Enum.Parse(providerTypeProperty.PropertyType, parameter.ProviderType);
				}
				catch
				{
				}
				if (obj != null)
				{
					statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, providerTypeProperty.Name), CodeGenHelper.Field(CodeGenHelper.TypeExpr(CodeGenHelper.GlobalType(providerTypeProperty.PropertyType)), obj.ToString())));
				}
			}
			if (parameter.Size != dbDataParameter.Size)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "Size"), CodeGenHelper.Primitive(parameter.Size)));
			}
			if (parameter.Direction != dbDataParameter.Direction)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "Direction"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(ParameterDirection)), parameter.Direction.ToString())));
			}
			if (parameter.IsNullable != dbDataParameter.IsNullable)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "IsNullable"), CodeGenHelper.Primitive(parameter.IsNullable)));
			}
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(QueryGeneratorBase.persistScaleAndPrecisionRegistryKey))
			{
				if (registryKey != null)
				{
					if (parameter.Precision != dbDataParameter.Precision)
					{
						statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "Precision"), CodeGenHelper.Primitive(parameter.Precision)));
					}
					if (parameter.Scale != dbDataParameter.Scale)
					{
						statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "Scale"), CodeGenHelper.Primitive(parameter.Scale)));
					}
				}
			}
			if (parameter.SourceColumn != dbDataParameter.SourceColumn)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "SourceColumn"), CodeGenHelper.Str(parameter.SourceColumn)));
			}
			if (parameter.SourceVersion != dbDataParameter.SourceVersion)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "SourceVersion"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DataRowVersion)), parameter.SourceVersion.ToString())));
			}
			if (dbDataParameter is DbParameter && parameter.SourceColumnNullMapping != ((DbParameter)dbDataParameter).SourceColumnNullMapping)
			{
				statements.Add(CodeGenHelper.Assign(CodeGenHelper.Property(parameterVariable, "SourceColumnNullMapping"), CodeGenHelper.Primitive(parameter.SourceColumnNullMapping)));
			}
			return parameterVariable;
		}

		protected Type GetParameterUrtType(DesignParameter parameter)
		{
			if (this.ParameterOption == ParameterGenerationOption.SqlTypes)
			{
				return this.GetParameterSqlType(parameter);
			}
			if (this.ParameterOption == ParameterGenerationOption.Objects)
			{
				return typeof(object);
			}
			if (this.ParameterOption == ParameterGenerationOption.ClrTypes)
			{
				Type type;
				if (parameter.DbType == DbType.Time && this.methodSource != null && this.methodSource.Connection != null && StringUtil.EqualValue(this.methodSource.Connection.Provider, ManagedProviderNames.SqlClient, true))
				{
					type = typeof(TimeSpan);
				}
				else
				{
					type = TypeConvertions.DbTypeToUrtType(parameter.DbType);
				}
				if (type == null)
				{
					if (this.codeGenerator != null)
					{
						this.codeGenerator.ProblemList.Add(new DSGeneratorProblem(SR.GetString("CG_UnableToConvertDbTypeToUrtType", new object[] { this.MethodName, parameter.Name }), ProblemSeverity.NonFatalError, this.methodSource));
					}
					type = typeof(object);
				}
				return type;
			}
			throw new InternalException("Unknown parameter generation option.");
		}

		private Type GetParameterSqlType(DesignParameter parameter)
		{
			IDesignConnection designConnection = null;
			if (StringUtil.EqualValue(designConnection.Provider, ManagedProviderNames.SqlClient))
			{
				SqlDbType sqlDbType = SqlDbType.Char;
				bool flag = false;
				if (parameter.ProviderType != null && parameter.ProviderType.Length > 0)
				{
					try
					{
						sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), parameter.ProviderType);
						flag = true;
					}
					catch
					{
					}
				}
				if (!flag)
				{
					sqlDbType = new SqlParameter
					{
						DbType = parameter.DbType
					}.SqlDbType;
				}
				Type type = TypeConvertions.SqlDbTypeToSqlType(sqlDbType);
				if (type == null)
				{
					if (this.codeGenerator != null)
					{
						this.codeGenerator.ProblemList.Add(new DSGeneratorProblem(SR.GetString("CG_UnableToConvertSqlDbTypeToSqlType", new object[] { this.MethodName, parameter.Name }), ProblemSeverity.NonFatalError, this.methodSource));
					}
					type = typeof(object);
				}
				return type;
			}
			throw new InternalException("We should never attempt to generate SqlType-parameters for non-Sql providers.");
		}

		protected void AddThrowsClauseIfNeeded(CodeMemberMethod dbMethod)
		{
			CodeTypeReference[] array = new CodeTypeReference[1];
			int num = 0;
			bool flag = false;
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
				if (designParameter.Direction == ParameterDirection.Output || designParameter.Direction == ParameterDirection.InputOutput)
				{
					Type parameterUrtType = this.GetParameterUrtType(designParameter);
					if (CodeGenHelper.GenerateNullExpression(parameterUrtType) == null)
					{
						array[0] = CodeGenHelper.GlobalType(typeof(StrongTypingException));
						flag = true;
					}
				}
			}
			if (!flag)
			{
				int returnParameterPosition = this.GetReturnParameterPosition(this.activeCommand);
				if (returnParameterPosition >= 0 && !this.getMethod && this.methodSource.QueryType != QueryType.Scalar)
				{
					Type parameterUrtType2 = this.GetParameterUrtType(this.activeCommand.Parameters[returnParameterPosition]);
					if (CodeGenHelper.GenerateNullExpression(parameterUrtType2) == null)
					{
						array[0] = CodeGenHelper.GlobalType(typeof(StrongTypingException));
						flag = true;
					}
				}
			}
			if (flag)
			{
				dbMethod.UserData.Add("throwsCollection", new CodeTypeReferenceCollection(array));
			}
		}

		protected void AddSetParameterStatements(DesignParameter parameter, string parameterName, CodeExpression cmdExpression, int parameterIndex, IList statements)
		{
			this.AddSetParameterStatements(parameter, parameterName, null, cmdExpression, parameterIndex, 0, statements);
		}

		protected void AddSetParameterStatements(DesignParameter parameter, string parameterName, DesignParameter isNullParameter, CodeExpression cmdExpression, int parameterIndex, int isNullParameterIndex, IList statements)
		{
			Type parameterUrtType = this.GetParameterUrtType(parameter);
			CodeCastExpression codeCastExpression = new CodeCastExpression(parameterUrtType, CodeGenHelper.Argument(parameterName));
			codeCastExpression.UserData.Add("CastIsBoxing", true);
			CodeCastExpression codeCastExpression2;
			CodeCastExpression codeCastExpression3;
			if (this.codeGenerator != null && CodeGenHelper.IsGeneratingJSharpCode(this.codeGenerator.CodeProvider))
			{
				codeCastExpression2 = new CodeCastExpression(typeof(int), CodeGenHelper.Primitive(0));
				codeCastExpression2.UserData.Add("CastIsBoxing", true);
				codeCastExpression3 = new CodeCastExpression(typeof(int), CodeGenHelper.Primitive(1));
				codeCastExpression3.UserData.Add("CastIsBoxing", true);
			}
			else
			{
				codeCastExpression2 = new CodeCastExpression(typeof(object), CodeGenHelper.Primitive(0));
				codeCastExpression3 = new CodeCastExpression(typeof(object), CodeGenHelper.Primitive(1));
			}
			CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Indexer(CodeGenHelper.Property(cmdExpression, "Parameters"), CodeGenHelper.Primitive(parameterIndex)), "Value");
			CodeExpression codeExpression2 = null;
			if (isNullParameter != null)
			{
				codeExpression2 = CodeGenHelper.Property(CodeGenHelper.Indexer(CodeGenHelper.Property(cmdExpression, "Parameters"), CodeGenHelper.Primitive(isNullParameterIndex)), "Value");
			}
			int num = ((isNullParameter == null) ? 1 : 2);
			CodeStatement[] array = new CodeStatement[num];
			CodeStatement[] array2 = new CodeStatement[num];
			if (parameter.AllowDbNull && parameterUrtType.IsValueType)
			{
				array[0] = CodeGenHelper.Assign(codeExpression, new CodeCastExpression(parameterUrtType, CodeGenHelper.Property(CodeGenHelper.Argument(parameterName), "Value"))
				{
					UserData = { { "CastIsBoxing", true } }
				});
				array2[0] = CodeGenHelper.Assign(codeExpression, CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DBNull)), "Value"));
				if (isNullParameter != null)
				{
					array[1] = array[0];
					array2[1] = array2[0];
					array[0] = CodeGenHelper.Assign(codeExpression2, codeCastExpression2);
					array2[0] = CodeGenHelper.Assign(codeExpression2, codeCastExpression3);
				}
				statements.Add(CodeGenHelper.If(CodeGenHelper.EQ(CodeGenHelper.Property(CodeGenHelper.Argument(parameterName), "HasValue"), CodeGenHelper.Primitive(true)), array, array2));
				return;
			}
			if (parameter.AllowDbNull && !parameterUrtType.IsValueType)
			{
				array[0] = CodeGenHelper.Assign(codeExpression, CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(DBNull)), "Value"));
				array2[0] = CodeGenHelper.Assign(codeExpression, codeCastExpression);
				if (isNullParameter != null)
				{
					array[1] = array[0];
					array2[1] = array2[0];
					array[0] = CodeGenHelper.Assign(codeExpression2, codeCastExpression3);
					array2[0] = CodeGenHelper.Assign(codeExpression2, codeCastExpression2);
				}
				statements.Add(CodeGenHelper.If(CodeGenHelper.IdEQ(CodeGenHelper.Argument(parameterName), CodeGenHelper.Primitive(null)), array, array2));
				return;
			}
			if (!parameter.AllowDbNull && !parameterUrtType.IsValueType)
			{
				CodeStatement[] array3 = new CodeStatement[] { CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(ArgumentNullException)), parameterName) };
				array2[0] = CodeGenHelper.Assign(codeExpression, codeCastExpression);
				if (isNullParameter != null)
				{
					array2[1] = array2[0];
					array2[0] = CodeGenHelper.Assign(codeExpression2, codeCastExpression2);
				}
				statements.Add(CodeGenHelper.If(CodeGenHelper.IdEQ(CodeGenHelper.Argument(parameterName), CodeGenHelper.Primitive(null)), array3, array2));
				return;
			}
			if (!parameter.AllowDbNull && parameterUrtType.IsValueType)
			{
				if (isNullParameter != null)
				{
					statements.Add(CodeGenHelper.Assign(codeExpression2, codeCastExpression2));
				}
				statements.Add(CodeGenHelper.Assign(codeExpression, codeCastExpression));
			}
		}

		protected bool AddSetReturnParamValuesStatements(IList statements, CodeExpression commandExpression)
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
				if (designParameter.Direction == ParameterDirection.Output || designParameter.Direction == ParameterDirection.InputOutput)
				{
					Type parameterUrtType = this.GetParameterUrtType(designParameter);
					string nameFromList = this.nameHandler.GetNameFromList(designParameter.ParameterName);
					CodeExpression codeExpression = CodeGenHelper.Property(CodeGenHelper.Indexer(CodeGenHelper.Property(commandExpression, "Parameters"), CodeGenHelper.Primitive(i)), "Value");
					CodeExpression codeExpression2 = CodeGenHelper.GenerateDbNullCheck(codeExpression);
					CodeExpression codeExpression3 = CodeGenHelper.GenerateNullExpression(parameterUrtType);
					CodeStatement codeStatement;
					if (codeExpression3 == null)
					{
						if (designParameter.AllowDbNull && parameterUrtType.IsValueType)
						{
							codeStatement = CodeGenHelper.Assign(CodeGenHelper.Argument(nameFromList), CodeGenHelper.New(CodeGenHelper.NullableType(parameterUrtType), new CodeExpression[0]));
						}
						else if (designParameter.AllowDbNull && !parameterUrtType.IsValueType)
						{
							codeStatement = CodeGenHelper.Assign(CodeGenHelper.Argument(nameFromList), CodeGenHelper.Primitive(null));
						}
						else
						{
							codeStatement = CodeGenHelper.Throw(CodeGenHelper.GlobalType(typeof(StrongTypingException)), SR.GetString("CG_ParameterIsDBNull", new object[] { nameFromList }), CodeGenHelper.Primitive(null));
						}
					}
					else
					{
						codeStatement = CodeGenHelper.Assign(CodeGenHelper.Argument(this.nameHandler.GetNameFromList(designParameter.ParameterName)), codeExpression3);
					}
					CodeStatement codeStatement2;
					if (designParameter.AllowDbNull && parameterUrtType.IsValueType)
					{
						codeStatement2 = CodeGenHelper.Assign(CodeGenHelper.Argument(nameFromList), CodeGenHelper.New(CodeGenHelper.NullableType(parameterUrtType), new CodeExpression[] { CodeGenHelper.Cast(CodeGenHelper.GlobalType(parameterUrtType), codeExpression) }));
					}
					else
					{
						codeStatement2 = CodeGenHelper.Assign(CodeGenHelper.Argument(nameFromList), CodeGenHelper.Cast(CodeGenHelper.GlobalType(parameterUrtType), codeExpression));
					}
					statements.Add(CodeGenHelper.If(codeExpression2, codeStatement, codeStatement2));
				}
			}
			return true;
		}

		private const string SqlCeParameterTypeName = "System.Data.SqlServerCe.SqlCeParameter, System.Data.SqlServerCe, Version=3.5.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

		protected TypedDataSourceCodeGenerator codeGenerator;

		protected GenericNameHandler nameHandler;

		protected static string returnVariableName = "returnValue";

		protected static string commandVariableName = "command";

		protected static string startRecordParameterName = "startRecord";

		protected static string maxRecordsParameterName = "maxRecords";

		protected DbProviderFactory providerFactory;

		protected DbSource methodSource;

		protected DbSourceCommand activeCommand;

		protected string methodName;

		protected MemberAttributes methodAttributes;

		protected Type containerParamType = typeof(DataSet);

		protected string containerParamTypeName;

		protected string containerParamName = "dataSet";

		protected ParameterGenerationOption parameterOption;

		protected Type returnType = typeof(void);

		protected int commandIndex;

		protected DesignTable designTable;

		protected bool getMethod;

		protected bool pagingMethod;

		protected bool declarationOnly;

		protected MethodTypeEnum methodType;

		protected string updateParameterName;

		protected CodeTypeReference updateParameterTypeReference = CodeGenHelper.GlobalType(typeof(DataSet));

		protected string updateParameterTypeName;

		protected CodeDomProvider codeProvider;

		protected string updateCommandName;

		protected bool isFunctionsDataComponent;

		private static Type sqlCeParameterType;

		private static object sqlCeParameterInstance;

		private static PropertyDescriptor sqlCeParaDbTypeDescriptor;

		private static string persistScaleAndPrecisionRegistryKey = "SOFTWARE\\Microsoft\\MSDataSetGenerator\\PersistScaleAndPrecision";
	}
}
