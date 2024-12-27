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
	// Token: 0x020000A3 RID: 163
	internal abstract class QueryGeneratorBase
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x0000FAC8 File Offset: 0x0000EAC8
		internal QueryGeneratorBase(TypedDataSourceCodeGenerator codeGenerator)
		{
			this.codeGenerator = codeGenerator;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0000FB22 File Offset: 0x0000EB22
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x0000FB2A File Offset: 0x0000EB2A
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

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0000FB33 File Offset: 0x0000EB33
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x0000FB3B File Offset: 0x0000EB3B
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

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0000FB44 File Offset: 0x0000EB44
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0000FB4C File Offset: 0x0000EB4C
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0000FB55 File Offset: 0x0000EB55
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x0000FB5D File Offset: 0x0000EB5D
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

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0000FB66 File Offset: 0x0000EB66
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x0000FB6E File Offset: 0x0000EB6E
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

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0000FB77 File Offset: 0x0000EB77
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x0000FB7F File Offset: 0x0000EB7F
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

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x0000FB88 File Offset: 0x0000EB88
		// (set) Token: 0x0600077A RID: 1914 RVA: 0x0000FB90 File Offset: 0x0000EB90
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

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600077B RID: 1915 RVA: 0x0000FB99 File Offset: 0x0000EB99
		// (set) Token: 0x0600077C RID: 1916 RVA: 0x0000FBA1 File Offset: 0x0000EBA1
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

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0000FBAA File Offset: 0x0000EBAA
		// (set) Token: 0x0600077E RID: 1918 RVA: 0x0000FBB2 File Offset: 0x0000EBB2
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0000FBBB File Offset: 0x0000EBBB
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x0000FBC3 File Offset: 0x0000EBC3
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

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0000FBCC File Offset: 0x0000EBCC
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0000FBD4 File Offset: 0x0000EBD4
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

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0000FBDD File Offset: 0x0000EBDD
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0000FBE5 File Offset: 0x0000EBE5
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

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0000FBEE File Offset: 0x0000EBEE
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x0000FBF6 File Offset: 0x0000EBF6
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

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0000FBFF File Offset: 0x0000EBFF
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x0000FC07 File Offset: 0x0000EC07
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

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0000FC10 File Offset: 0x0000EC10
		// (set) Token: 0x0600078A RID: 1930 RVA: 0x0000FC18 File Offset: 0x0000EC18
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

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0000FC21 File Offset: 0x0000EC21
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0000FC29 File Offset: 0x0000EC29
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0000FC32 File Offset: 0x0000EC32
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0000FC3A File Offset: 0x0000EC3A
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0000FC43 File Offset: 0x0000EC43
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0000FC4B File Offset: 0x0000EC4B
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0000FC54 File Offset: 0x0000EC54
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0000FC5C File Offset: 0x0000EC5C
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

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0000FC65 File Offset: 0x0000EC65
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0000FC6D File Offset: 0x0000EC6D
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0000FC78 File Offset: 0x0000EC78
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0000FCB8 File Offset: 0x0000ECB8
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

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0000FCDC File Offset: 0x0000ECDC
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

		// Token: 0x06000798 RID: 1944 RVA: 0x0000FD0C File Offset: 0x0000ED0C
		internal static CodeStatement SetCommandTextStatement(CodeExpression commandExpression, string commandText)
		{
			return CodeGenHelper.Assign(CodeGenHelper.Property(commandExpression, "CommandText"), CodeGenHelper.Str(commandText));
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0000FD34 File Offset: 0x0000ED34
		internal static CodeStatement SetCommandTypeStatement(CodeExpression commandExpression, CommandType commandType)
		{
			return CodeGenHelper.Assign(CodeGenHelper.Property(commandExpression, "CommandType"), CodeGenHelper.Field(CodeGenHelper.GlobalTypeExpr(typeof(CommandType)), commandType.ToString()));
		}

		// Token: 0x0600079A RID: 1946
		internal abstract CodeMemberMethod Generate();

		// Token: 0x0600079B RID: 1947 RVA: 0x0000FD74 File Offset: 0x0000ED74
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

		// Token: 0x0600079C RID: 1948 RVA: 0x0000FDD8 File Offset: 0x0000EDD8
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

		// Token: 0x0600079D RID: 1949 RVA: 0x0000FE20 File Offset: 0x0000EE20
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

		// Token: 0x0600079E RID: 1950 RVA: 0x0000FEA4 File Offset: 0x0000EEA4
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

		// Token: 0x0600079F RID: 1951 RVA: 0x0000FF40 File Offset: 0x0000EF40
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

		// Token: 0x060007A0 RID: 1952 RVA: 0x0000FFF0 File Offset: 0x0000EFF0
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

		// Token: 0x060007A1 RID: 1953 RVA: 0x00010090 File Offset: 0x0000F090
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

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001012C File Offset: 0x0000F12C
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

		// Token: 0x060007A3 RID: 1955 RVA: 0x000101C8 File Offset: 0x0000F1C8
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

		// Token: 0x060007A4 RID: 1956 RVA: 0x000105D8 File Offset: 0x0000F5D8
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

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001064C File Offset: 0x0000F64C
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

		// Token: 0x060007A6 RID: 1958 RVA: 0x000109D4 File Offset: 0x0000F9D4
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

		// Token: 0x060007A7 RID: 1959 RVA: 0x00010ACC File Offset: 0x0000FACC
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

		// Token: 0x060007A8 RID: 1960 RVA: 0x00010BCC File Offset: 0x0000FBCC
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

		// Token: 0x060007A9 RID: 1961 RVA: 0x00010CEF File Offset: 0x0000FCEF
		protected void AddSetParameterStatements(DesignParameter parameter, string parameterName, CodeExpression cmdExpression, int parameterIndex, IList statements)
		{
			this.AddSetParameterStatements(parameter, parameterName, null, cmdExpression, parameterIndex, 0, statements);
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00010D00 File Offset: 0x0000FD00
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

		// Token: 0x060007AB RID: 1963 RVA: 0x00011070 File Offset: 0x00010070
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

		// Token: 0x04000B96 RID: 2966
		private const string SqlCeParameterTypeName = "System.Data.SqlServerCe.SqlCeParameter, System.Data.SqlServerCe, Version=3.5.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";

		// Token: 0x04000B97 RID: 2967
		protected TypedDataSourceCodeGenerator codeGenerator;

		// Token: 0x04000B98 RID: 2968
		protected GenericNameHandler nameHandler;

		// Token: 0x04000B99 RID: 2969
		protected static string returnVariableName = "returnValue";

		// Token: 0x04000B9A RID: 2970
		protected static string commandVariableName = "command";

		// Token: 0x04000B9B RID: 2971
		protected static string startRecordParameterName = "startRecord";

		// Token: 0x04000B9C RID: 2972
		protected static string maxRecordsParameterName = "maxRecords";

		// Token: 0x04000B9D RID: 2973
		protected DbProviderFactory providerFactory;

		// Token: 0x04000B9E RID: 2974
		protected DbSource methodSource;

		// Token: 0x04000B9F RID: 2975
		protected DbSourceCommand activeCommand;

		// Token: 0x04000BA0 RID: 2976
		protected string methodName;

		// Token: 0x04000BA1 RID: 2977
		protected MemberAttributes methodAttributes;

		// Token: 0x04000BA2 RID: 2978
		protected Type containerParamType = typeof(DataSet);

		// Token: 0x04000BA3 RID: 2979
		protected string containerParamTypeName;

		// Token: 0x04000BA4 RID: 2980
		protected string containerParamName = "dataSet";

		// Token: 0x04000BA5 RID: 2981
		protected ParameterGenerationOption parameterOption;

		// Token: 0x04000BA6 RID: 2982
		protected Type returnType = typeof(void);

		// Token: 0x04000BA7 RID: 2983
		protected int commandIndex;

		// Token: 0x04000BA8 RID: 2984
		protected DesignTable designTable;

		// Token: 0x04000BA9 RID: 2985
		protected bool getMethod;

		// Token: 0x04000BAA RID: 2986
		protected bool pagingMethod;

		// Token: 0x04000BAB RID: 2987
		protected bool declarationOnly;

		// Token: 0x04000BAC RID: 2988
		protected MethodTypeEnum methodType;

		// Token: 0x04000BAD RID: 2989
		protected string updateParameterName;

		// Token: 0x04000BAE RID: 2990
		protected CodeTypeReference updateParameterTypeReference = CodeGenHelper.GlobalType(typeof(DataSet));

		// Token: 0x04000BAF RID: 2991
		protected string updateParameterTypeName;

		// Token: 0x04000BB0 RID: 2992
		protected CodeDomProvider codeProvider;

		// Token: 0x04000BB1 RID: 2993
		protected string updateCommandName;

		// Token: 0x04000BB2 RID: 2994
		protected bool isFunctionsDataComponent;

		// Token: 0x04000BB3 RID: 2995
		private static Type sqlCeParameterType;

		// Token: 0x04000BB4 RID: 2996
		private static object sqlCeParameterInstance;

		// Token: 0x04000BB5 RID: 2997
		private static PropertyDescriptor sqlCeParaDbTypeDescriptor;

		// Token: 0x04000BB6 RID: 2998
		private static string persistScaleAndPrecisionRegistryKey = "SOFTWARE\\Microsoft\\MSDataSetGenerator\\PersistScaleAndPrecision";
	}
}
