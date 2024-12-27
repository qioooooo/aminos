using System;

namespace System.Configuration
{
	// Token: 0x0200009C RID: 156
	internal class StreamUpdate
	{
		// Token: 0x06000622 RID: 1570 RVA: 0x0001C986 File Offset: 0x0001B986
		internal StreamUpdate(string newStreamname)
		{
			this._newStreamname = newStreamname;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0001C995 File Offset: 0x0001B995
		internal string NewStreamname
		{
			get
			{
				return this._newStreamname;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001C99D File Offset: 0x0001B99D
		// (set) Token: 0x06000625 RID: 1573 RVA: 0x0001C9A5 File Offset: 0x0001B9A5
		internal bool WriteCompleted
		{
			get
			{
				return this._writeCompleted;
			}
			set
			{
				this._writeCompleted = value;
			}
		}

		// Token: 0x040003E1 RID: 993
		private string _newStreamname;

		// Token: 0x040003E2 RID: 994
		private bool _writeCompleted;
	}
}
