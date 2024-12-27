using System;

namespace System.Data
{
	// Token: 0x020000BA RID: 186
	public interface IDataParameter
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000C72 RID: 3186
		// (set) Token: 0x06000C73 RID: 3187
		DbType DbType { get; set; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000C74 RID: 3188
		// (set) Token: 0x06000C75 RID: 3189
		ParameterDirection Direction { get; set; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000C76 RID: 3190
		bool IsNullable { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000C77 RID: 3191
		// (set) Token: 0x06000C78 RID: 3192
		string ParameterName { get; set; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000C79 RID: 3193
		// (set) Token: 0x06000C7A RID: 3194
		string SourceColumn { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000C7B RID: 3195
		// (set) Token: 0x06000C7C RID: 3196
		DataRowVersion SourceVersion { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000C7D RID: 3197
		// (set) Token: 0x06000C7E RID: 3198
		object Value { get; set; }
	}
}
