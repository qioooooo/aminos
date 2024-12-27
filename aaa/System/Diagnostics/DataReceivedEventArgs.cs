using System;

namespace System.Diagnostics
{
	// Token: 0x02000747 RID: 1863
	public class DataReceivedEventArgs : EventArgs
	{
		// Token: 0x060038D0 RID: 14544 RVA: 0x000EFBD1 File Offset: 0x000EEBD1
		internal DataReceivedEventArgs(string data)
		{
			this._data = data;
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x000EFBE0 File Offset: 0x000EEBE0
		public string Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x04003267 RID: 12903
		internal string _data;
	}
}
