using System;

namespace System.Data.Design
{
	// Token: 0x0200006E RID: 110
	internal class DataAccessor : DataSourceComponent
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x00004021 File Offset: 0x00003021
		public DataAccessor(DesignTable designTable)
		{
			if (designTable == null)
			{
				throw new ArgumentNullException("DesignTable");
			}
			this.designTable = designTable;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000403E File Offset: 0x0000303E
		internal DesignTable DesignTable
		{
			get
			{
				return this.designTable;
			}
		}

		// Token: 0x04000A96 RID: 2710
		internal const string DEFAULT_BASE_CLASS = "System.ComponentModel.Component";

		// Token: 0x04000A97 RID: 2711
		internal const string DEFAULT_NAME_POSTFIX = "TableAdapter";

		// Token: 0x04000A98 RID: 2712
		private DesignTable designTable;
	}
}
