using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000259 RID: 601
	internal struct ITransactionJoinWrapper : IDisposable
	{
		// Token: 0x060020AA RID: 8362 RVA: 0x00263B94 File Offset: 0x00262F94
		internal ITransactionJoinWrapper(object unknown)
		{
			this._unknown = unknown;
			this._value = unknown as NativeMethods.ITransactionJoin;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x00263BB4 File Offset: 0x00262FB4
		internal NativeMethods.ITransactionJoin Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x00263BC8 File Offset: 0x00262FC8
		public void Dispose()
		{
			object unknown = this._unknown;
			this._unknown = null;
			this._value = null;
			if (unknown != null)
			{
				Marshal.ReleaseComObject(unknown);
			}
		}

		// Token: 0x04001521 RID: 5409
		private object _unknown;

		// Token: 0x04001522 RID: 5410
		private NativeMethods.ITransactionJoin _value;
	}
}
