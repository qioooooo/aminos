using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace System.Data.Design
{
	internal sealed class DataComponentNameHandler
	{
		internal bool GlobalSources
		{
			get
			{
				return this.globalSources;
			}
			set
			{
				this.globalSources = value;
			}
		}

		internal void GenerateMemberNames(DesignTable designTable, CodeDomProvider codeProvider, bool languageCaseInsensitive, ArrayList problemList)
		{
			this.languageCaseInsensitive = languageCaseInsensitive;
			this.validator = new MemberNameValidator(null, codeProvider, this.languageCaseInsensitive);
			this.validator.UseSuffix = true;
			this.AddReservedNames();
			this.ProcessMemberNames(designTable);
		}

		private void AddReservedNames()
		{
			this.validator.GetNewMemberName(DataComponentNameHandler.initMethodName);
			this.validator.GetNewMemberName(DataComponentNameHandler.deleteMethodName);
			this.validator.GetNewMemberName(DataComponentNameHandler.insertMethodName);
			this.validator.GetNewMemberName(DataComponentNameHandler.updateMethodName);
			this.validator.GetNewMemberName(DataComponentNameHandler.adapterVariableName);
			this.validator.GetNewMemberName(DataComponentNameHandler.adapterPropertyName);
			this.validator.GetNewMemberName(DataComponentNameHandler.initAdapter);
			this.validator.GetNewMemberName(DataComponentNameHandler.selectCmdCollectionVariableName);
			this.validator.GetNewMemberName(DataComponentNameHandler.selectCmdCollectionPropertyName);
			this.validator.GetNewMemberName(DataComponentNameHandler.initCmdCollection);
			this.validator.GetNewMemberName(DataComponentNameHandler.defaultConnectionVariableName);
			this.validator.GetNewMemberName(DataComponentNameHandler.defaultConnectionPropertyName);
			this.validator.GetNewMemberName(DataComponentNameHandler.transactionVariableName);
			this.validator.GetNewMemberName(DataComponentNameHandler.transactionPropertyName);
			this.validator.GetNewMemberName(DataComponentNameHandler.initConnection);
			this.validator.GetNewMemberName(DataComponentNameHandler.clearBeforeFillVariableName);
			this.validator.GetNewMemberName(DataComponentNameHandler.clearBeforeFillPropertyName);
			this.validator.GetNewMemberName("TableAdapterManager");
			this.validator.GetNewMemberName("UpdateAll");
		}

		private void ProcessMemberNames(DesignTable designTable)
		{
			this.ProcessClassName(designTable);
			if (!this.GlobalSources && designTable.MainSource != null)
			{
				this.ProcessSourceName((DbSource)designTable.MainSource);
			}
			if (designTable.Sources != null)
			{
				foreach (object obj in designTable.Sources)
				{
					Source source = (Source)obj;
					this.ProcessSourceName((DbSource)source);
				}
			}
		}

		internal void ProcessClassName(DesignTable table)
		{
			if (!StringUtil.EqualValue(table.DataAccessorName, table.UserDataComponentName, this.languageCaseInsensitive) || StringUtil.Empty(table.GeneratorDataComponentClassName))
			{
				table.GeneratorDataComponentClassName = this.validator.GenerateIdName(table.DataAccessorName);
				return;
			}
			table.GeneratorDataComponentClassName = this.validator.GenerateIdName(table.GeneratorDataComponentClassName);
		}

		internal void ProcessSourceName(DbSource source)
		{
			bool flag = !StringUtil.EqualValue(source.Name, source.UserSourceName, this.languageCaseInsensitive);
			bool flag2 = !StringUtil.EqualValue(source.GetMethodName, source.UserGetMethodName, this.languageCaseInsensitive);
			if (source.GenerateMethods == GenerateMethodTypes.Fill || source.GenerateMethods == GenerateMethodTypes.Both)
			{
				if (flag || StringUtil.Empty(source.GeneratorSourceName))
				{
					source.GeneratorSourceName = this.validator.GenerateIdName(source.Name);
				}
				else
				{
					source.GeneratorSourceName = this.validator.GenerateIdName(source.GeneratorSourceName);
				}
			}
			if (source.QueryType == QueryType.Rowset && (source.GenerateMethods == GenerateMethodTypes.Get || source.GenerateMethods == GenerateMethodTypes.Both))
			{
				if (flag2 || StringUtil.Empty(source.GeneratorGetMethodName))
				{
					source.GeneratorGetMethodName = this.validator.GenerateIdName(source.GetMethodName);
				}
				else
				{
					source.GeneratorGetMethodName = this.validator.GenerateIdName(source.GeneratorGetMethodName);
				}
			}
			if (source.QueryType == QueryType.Rowset && source.GeneratePagingMethods)
			{
				if (source.GenerateMethods == GenerateMethodTypes.Fill || source.GenerateMethods == GenerateMethodTypes.Both)
				{
					if (flag || StringUtil.Empty(source.GeneratorSourceNameForPaging))
					{
						source.GeneratorSourceNameForPaging = this.validator.GenerateIdName(source.Name + DataComponentNameHandler.pagingMethodSuffix);
					}
					else
					{
						source.GeneratorSourceNameForPaging = this.validator.GenerateIdName(source.GeneratorSourceNameForPaging);
					}
				}
				if (source.GenerateMethods == GenerateMethodTypes.Get || source.GenerateMethods == GenerateMethodTypes.Both)
				{
					if (flag2 || StringUtil.Empty(source.GeneratorGetMethodNameForPaging))
					{
						source.GeneratorGetMethodNameForPaging = this.validator.GenerateIdName(source.GetMethodName + DataComponentNameHandler.pagingMethodSuffix);
						return;
					}
					source.GeneratorGetMethodNameForPaging = this.validator.GenerateIdName(source.GeneratorGetMethodNameForPaging);
				}
			}
		}

		internal static string DeleteMethodName
		{
			get
			{
				return DataComponentNameHandler.deleteMethodName;
			}
		}

		internal static string UpdateMethodName
		{
			get
			{
				return DataComponentNameHandler.updateMethodName;
			}
		}

		internal static string InsertMethodName
		{
			get
			{
				return DataComponentNameHandler.insertMethodName;
			}
		}

		internal static string AdapterVariableName
		{
			get
			{
				return DataComponentNameHandler.adapterVariableName;
			}
		}

		internal static string AdapterPropertyName
		{
			get
			{
				return DataComponentNameHandler.adapterPropertyName;
			}
		}

		internal static string InitAdapter
		{
			get
			{
				return DataComponentNameHandler.initAdapter;
			}
		}

		internal static string SelectCmdCollectionVariableName
		{
			get
			{
				return DataComponentNameHandler.selectCmdCollectionVariableName;
			}
		}

		internal static string SelectCmdCollectionPropertyName
		{
			get
			{
				return DataComponentNameHandler.selectCmdCollectionPropertyName;
			}
		}

		internal static string InitCmdCollection
		{
			get
			{
				return DataComponentNameHandler.initCmdCollection;
			}
		}

		internal static string DefaultConnectionVariableName
		{
			get
			{
				return DataComponentNameHandler.defaultConnectionVariableName;
			}
		}

		internal static string DefaultConnectionPropertyName
		{
			get
			{
				return DataComponentNameHandler.defaultConnectionPropertyName;
			}
		}

		internal static string TransactionPropertyName
		{
			get
			{
				return DataComponentNameHandler.transactionPropertyName;
			}
		}

		internal static string TransactionVariableName
		{
			get
			{
				return DataComponentNameHandler.transactionVariableName;
			}
		}

		internal static string InitConnection
		{
			get
			{
				return DataComponentNameHandler.initConnection;
			}
		}

		internal static string PagingMethodSuffix
		{
			get
			{
				return DataComponentNameHandler.pagingMethodSuffix;
			}
		}

		internal static string ClearBeforeFillVariableName
		{
			get
			{
				return DataComponentNameHandler.clearBeforeFillVariableName;
			}
		}

		internal static string ClearBeforeFillPropertyName
		{
			get
			{
				return DataComponentNameHandler.clearBeforeFillPropertyName;
			}
		}

		private MemberNameValidator validator;

		private bool languageCaseInsensitive;

		private bool globalSources;

		private static readonly string pagingMethodSuffix = "Page";

		private static readonly string initMethodName = "InitClass";

		private static readonly string deleteMethodName = "Delete";

		private static readonly string insertMethodName = "Insert";

		private static readonly string updateMethodName = "Update";

		private static readonly string adapterVariableName = "_adapter";

		private static readonly string adapterPropertyName = "Adapter";

		private static readonly string initAdapter = "InitAdapter";

		private static readonly string selectCmdCollectionVariableName = "_commandCollection";

		private static readonly string selectCmdCollectionPropertyName = "CommandCollection";

		private static readonly string initCmdCollection = "InitCommandCollection";

		private static readonly string defaultConnectionVariableName = "_connection";

		private static readonly string defaultConnectionPropertyName = "Connection";

		private static readonly string transactionVariableName = "_transaction";

		private static readonly string transactionPropertyName = "Transaction";

		private static readonly string initConnection = "InitConnection";

		private static readonly string clearBeforeFillVariableName = "_clearBeforeFill";

		private static readonly string clearBeforeFillPropertyName = "ClearBeforeFill";
	}
}
