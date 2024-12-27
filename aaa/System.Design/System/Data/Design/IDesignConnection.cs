using System;
using System.CodeDom;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x02000091 RID: 145
	internal interface IDesignConnection : IDataSourceNamedObject, INamedObject, ICloneable, IDataSourceInitAfterLoading, IDataSourceXmlSpecialOwner
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000603 RID: 1539
		// (set) Token: 0x06000604 RID: 1540
		ConnectionString ConnectionStringObject { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000605 RID: 1541
		// (set) Token: 0x06000606 RID: 1542
		string ConnectionString { get; set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000607 RID: 1543
		// (set) Token: 0x06000608 RID: 1544
		string Provider { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000609 RID: 1545
		// (set) Token: 0x0600060A RID: 1546
		bool IsAppSettingsProperty { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600060B RID: 1547
		// (set) Token: 0x0600060C RID: 1548
		string AppSettingsObjectName { get; set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600060D RID: 1549
		// (set) Token: 0x0600060E RID: 1550
		CodePropertyReferenceExpression PropertyReference { get; set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600060F RID: 1551
		IDictionary Properties { get; }

		// Token: 0x06000610 RID: 1552
		IDbConnection CreateEmptyDbConnection();
	}
}
