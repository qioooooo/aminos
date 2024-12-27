using System;
using System.CodeDom.Compiler;
using System.Globalization;

namespace System.Data.Design
{
	// Token: 0x020000C1 RID: 193
	internal sealed class TableAdapterManagerNameHandler
	{
		// Token: 0x06000880 RID: 2176 RVA: 0x000184C0 File Offset: 0x000174C0
		public TableAdapterManagerNameHandler(CodeDomProvider provider)
		{
			this.codePrivider = provider;
			this.languageCaseInsensitive = (this.codePrivider.LanguageOptions & LanguageOptions.CaseInsensitive) == LanguageOptions.CaseInsensitive;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000881 RID: 2177 RVA: 0x000184E8 File Offset: 0x000174E8
		private MemberNameValidator TableAdapterManagerValidator
		{
			get
			{
				if (this.tableAdapterManagerValidator == null)
				{
					this.tableAdapterManagerValidator = new MemberNameValidator(new string[]
					{
						"SelfReferenceComparer", "UpdateAll", "SortSelfReferenceRows", "MatchTableAdapterConnection", "_connection", "Connection", "_backupDataSetBeforeUpdate", "BackupDataSetBeforeUpdate", "TableAdapterInstanceCount", "UpdateOrder",
						"_updateOrder", "UpdateOrderOption", "UpdateUpdatedRows", "UpdateInsertedRows", "UpdateDeletedRows", "GetRealUpdatedRows"
					}, this.codePrivider, this.languageCaseInsensitive);
				}
				return this.tableAdapterManagerValidator;
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000185AD File Offset: 0x000175AD
		internal string GetNewMemberName(string memberName)
		{
			return this.TableAdapterManagerValidator.GetNewMemberName(memberName);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x000185BB File Offset: 0x000175BB
		internal string GetTableAdapterPropName(string className)
		{
			return this.GetNewMemberName(className);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000185C4 File Offset: 0x000175C4
		internal string GetTableAdapterVarName(string propName)
		{
			propName = "_" + char.ToLower(propName[0], CultureInfo.InvariantCulture) + propName.Remove(0, 1);
			return this.GetNewMemberName(propName);
		}

		// Token: 0x04000C1F RID: 3103
		internal const string TableAdapterManagerClassName = "TableAdapterManager";

		// Token: 0x04000C20 RID: 3104
		internal const string SelfRefComparerClass = "SelfReferenceComparer";

		// Token: 0x04000C21 RID: 3105
		internal const string UpdateAllMethod = "UpdateAll";

		// Token: 0x04000C22 RID: 3106
		internal const string SortSelfRefRowsMethod = "SortSelfReferenceRows";

		// Token: 0x04000C23 RID: 3107
		internal const string MatchTAConnectionMethod = "MatchTableAdapterConnection";

		// Token: 0x04000C24 RID: 3108
		internal const string UpdateAllRevertConnectionsVar = "revertConnections";

		// Token: 0x04000C25 RID: 3109
		internal const string ConnectionVar = "_connection";

		// Token: 0x04000C26 RID: 3110
		internal const string ConnectionProperty = "Connection";

		// Token: 0x04000C27 RID: 3111
		internal const string BackupDataSetBeforeUpdateVar = "_backupDataSetBeforeUpdate";

		// Token: 0x04000C28 RID: 3112
		internal const string BackupDataSetBeforeUpdateProperty = "BackupDataSetBeforeUpdate";

		// Token: 0x04000C29 RID: 3113
		internal const string TableAdapterInstanceCountProperty = "TableAdapterInstanceCount";

		// Token: 0x04000C2A RID: 3114
		internal const string UpdateOrderOptionProperty = "UpdateOrder";

		// Token: 0x04000C2B RID: 3115
		internal const string UpdateOrderOptionVar = "_updateOrder";

		// Token: 0x04000C2C RID: 3116
		internal const string UpdateOrderOptionEnum = "UpdateOrderOption";

		// Token: 0x04000C2D RID: 3117
		internal const string UpdateOrderOptionEnumIUD = "InsertUpdateDelete";

		// Token: 0x04000C2E RID: 3118
		internal const string UpdateOrderOptionEnumUID = "UpdateInsertDelete";

		// Token: 0x04000C2F RID: 3119
		internal const string UpdateUpdatedRowsMethod = "UpdateUpdatedRows";

		// Token: 0x04000C30 RID: 3120
		internal const string UpdateInsertedRowsMethod = "UpdateInsertedRows";

		// Token: 0x04000C31 RID: 3121
		internal const string UpdateDeletedRowsMethod = "UpdateDeletedRows";

		// Token: 0x04000C32 RID: 3122
		internal const string GetRealUpdatedRowsMethod = "GetRealUpdatedRows";

		// Token: 0x04000C33 RID: 3123
		private MemberNameValidator tableAdapterManagerValidator;

		// Token: 0x04000C34 RID: 3124
		private bool languageCaseInsensitive;

		// Token: 0x04000C35 RID: 3125
		private CodeDomProvider codePrivider;
	}
}
