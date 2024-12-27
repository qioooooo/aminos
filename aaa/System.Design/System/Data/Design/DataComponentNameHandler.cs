using System;
using System.CodeDom.Compiler;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x02000071 RID: 113
	internal sealed class DataComponentNameHandler
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x000053F4 File Offset: 0x000043F4
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x000053FC File Offset: 0x000043FC
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

		// Token: 0x060004CB RID: 1227 RVA: 0x00005405 File Offset: 0x00004405
		internal void GenerateMemberNames(DesignTable designTable, CodeDomProvider codeProvider, bool languageCaseInsensitive, ArrayList problemList)
		{
			this.languageCaseInsensitive = languageCaseInsensitive;
			this.validator = new MemberNameValidator(null, codeProvider, this.languageCaseInsensitive);
			this.validator.UseSuffix = true;
			this.AddReservedNames();
			this.ProcessMemberNames(designTable);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000543C File Offset: 0x0000443C
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

		// Token: 0x060004CD RID: 1229 RVA: 0x0000558C File Offset: 0x0000458C
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

		// Token: 0x060004CE RID: 1230 RVA: 0x0000561C File Offset: 0x0000461C
		internal void ProcessClassName(DesignTable table)
		{
			if (!StringUtil.EqualValue(table.DataAccessorName, table.UserDataComponentName, this.languageCaseInsensitive) || StringUtil.Empty(table.GeneratorDataComponentClassName))
			{
				table.GeneratorDataComponentClassName = this.validator.GenerateIdName(table.DataAccessorName);
				return;
			}
			table.GeneratorDataComponentClassName = this.validator.GenerateIdName(table.GeneratorDataComponentClassName);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00005684 File Offset: 0x00004684
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00005840 File Offset: 0x00004840
		internal static string DeleteMethodName
		{
			get
			{
				return DataComponentNameHandler.deleteMethodName;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00005847 File Offset: 0x00004847
		internal static string UpdateMethodName
		{
			get
			{
				return DataComponentNameHandler.updateMethodName;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060004D2 RID: 1234 RVA: 0x0000584E File Offset: 0x0000484E
		internal static string InsertMethodName
		{
			get
			{
				return DataComponentNameHandler.insertMethodName;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00005855 File Offset: 0x00004855
		internal static string AdapterVariableName
		{
			get
			{
				return DataComponentNameHandler.adapterVariableName;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0000585C File Offset: 0x0000485C
		internal static string AdapterPropertyName
		{
			get
			{
				return DataComponentNameHandler.adapterPropertyName;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00005863 File Offset: 0x00004863
		internal static string InitAdapter
		{
			get
			{
				return DataComponentNameHandler.initAdapter;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0000586A File Offset: 0x0000486A
		internal static string SelectCmdCollectionVariableName
		{
			get
			{
				return DataComponentNameHandler.selectCmdCollectionVariableName;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00005871 File Offset: 0x00004871
		internal static string SelectCmdCollectionPropertyName
		{
			get
			{
				return DataComponentNameHandler.selectCmdCollectionPropertyName;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00005878 File Offset: 0x00004878
		internal static string InitCmdCollection
		{
			get
			{
				return DataComponentNameHandler.initCmdCollection;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0000587F File Offset: 0x0000487F
		internal static string DefaultConnectionVariableName
		{
			get
			{
				return DataComponentNameHandler.defaultConnectionVariableName;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00005886 File Offset: 0x00004886
		internal static string DefaultConnectionPropertyName
		{
			get
			{
				return DataComponentNameHandler.defaultConnectionPropertyName;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000588D File Offset: 0x0000488D
		internal static string TransactionPropertyName
		{
			get
			{
				return DataComponentNameHandler.transactionPropertyName;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x00005894 File Offset: 0x00004894
		internal static string TransactionVariableName
		{
			get
			{
				return DataComponentNameHandler.transactionVariableName;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x0000589B File Offset: 0x0000489B
		internal static string InitConnection
		{
			get
			{
				return DataComponentNameHandler.initConnection;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x000058A2 File Offset: 0x000048A2
		internal static string PagingMethodSuffix
		{
			get
			{
				return DataComponentNameHandler.pagingMethodSuffix;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000058A9 File Offset: 0x000048A9
		internal static string ClearBeforeFillVariableName
		{
			get
			{
				return DataComponentNameHandler.clearBeforeFillVariableName;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x000058B0 File Offset: 0x000048B0
		internal static string ClearBeforeFillPropertyName
		{
			get
			{
				return DataComponentNameHandler.clearBeforeFillPropertyName;
			}
		}

		// Token: 0x04000A9F RID: 2719
		private MemberNameValidator validator;

		// Token: 0x04000AA0 RID: 2720
		private bool languageCaseInsensitive;

		// Token: 0x04000AA1 RID: 2721
		private bool globalSources;

		// Token: 0x04000AA2 RID: 2722
		private static readonly string pagingMethodSuffix = "Page";

		// Token: 0x04000AA3 RID: 2723
		private static readonly string initMethodName = "InitClass";

		// Token: 0x04000AA4 RID: 2724
		private static readonly string deleteMethodName = "Delete";

		// Token: 0x04000AA5 RID: 2725
		private static readonly string insertMethodName = "Insert";

		// Token: 0x04000AA6 RID: 2726
		private static readonly string updateMethodName = "Update";

		// Token: 0x04000AA7 RID: 2727
		private static readonly string adapterVariableName = "_adapter";

		// Token: 0x04000AA8 RID: 2728
		private static readonly string adapterPropertyName = "Adapter";

		// Token: 0x04000AA9 RID: 2729
		private static readonly string initAdapter = "InitAdapter";

		// Token: 0x04000AAA RID: 2730
		private static readonly string selectCmdCollectionVariableName = "_commandCollection";

		// Token: 0x04000AAB RID: 2731
		private static readonly string selectCmdCollectionPropertyName = "CommandCollection";

		// Token: 0x04000AAC RID: 2732
		private static readonly string initCmdCollection = "InitCommandCollection";

		// Token: 0x04000AAD RID: 2733
		private static readonly string defaultConnectionVariableName = "_connection";

		// Token: 0x04000AAE RID: 2734
		private static readonly string defaultConnectionPropertyName = "Connection";

		// Token: 0x04000AAF RID: 2735
		private static readonly string transactionVariableName = "_transaction";

		// Token: 0x04000AB0 RID: 2736
		private static readonly string transactionPropertyName = "Transaction";

		// Token: 0x04000AB1 RID: 2737
		private static readonly string initConnection = "InitConnection";

		// Token: 0x04000AB2 RID: 2738
		private static readonly string clearBeforeFillVariableName = "_clearBeforeFill";

		// Token: 0x04000AB3 RID: 2739
		private static readonly string clearBeforeFillPropertyName = "ClearBeforeFill";
	}
}
