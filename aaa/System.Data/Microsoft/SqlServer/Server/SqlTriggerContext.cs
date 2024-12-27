using System;
using System.Data.Common;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000055 RID: 85
	public sealed class SqlTriggerContext
	{
		// Token: 0x060003AA RID: 938 RVA: 0x001CFFC0 File Offset: 0x001CF3C0
		internal SqlTriggerContext(TriggerAction triggerAction, bool[] columnsUpdated, SqlXml eventInstanceData)
		{
			this._triggerAction = triggerAction;
			this._columnsUpdated = columnsUpdated;
			this._eventInstanceData = eventInstanceData;
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060003AB RID: 939 RVA: 0x001CFFE8 File Offset: 0x001CF3E8
		public int ColumnCount
		{
			get
			{
				int num = 0;
				if (this._columnsUpdated != null)
				{
					num = this._columnsUpdated.Length;
				}
				return num;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060003AC RID: 940 RVA: 0x001D000C File Offset: 0x001CF40C
		public SqlXml EventData
		{
			get
			{
				return this._eventInstanceData;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060003AD RID: 941 RVA: 0x001D0020 File Offset: 0x001CF420
		public TriggerAction TriggerAction
		{
			get
			{
				return this._triggerAction;
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x001D0034 File Offset: 0x001CF434
		public bool IsUpdatedColumn(int columnOrdinal)
		{
			if (this._columnsUpdated != null)
			{
				return this._columnsUpdated[columnOrdinal];
			}
			throw ADP.IndexOutOfRange(columnOrdinal);
		}

		// Token: 0x0400063C RID: 1596
		private TriggerAction _triggerAction;

		// Token: 0x0400063D RID: 1597
		private bool[] _columnsUpdated;

		// Token: 0x0400063E RID: 1598
		private SqlXml _eventInstanceData;
	}
}
