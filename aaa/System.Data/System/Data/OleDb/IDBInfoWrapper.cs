using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000255 RID: 597
	internal struct IDBInfoWrapper : IDisposable
	{
		// Token: 0x0600209E RID: 8350 RVA: 0x00263A14 File Offset: 0x00262E14
		internal IDBInfoWrapper(object unknown)
		{
			this._unknown = unknown;
			this._value = unknown as UnsafeNativeMethods.IDBInfo;
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x00263A34 File Offset: 0x00262E34
		internal UnsafeNativeMethods.IDBInfo Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x00263A48 File Offset: 0x00262E48
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

		// Token: 0x04001519 RID: 5401
		private object _unknown;

		// Token: 0x0400151A RID: 5402
		private UnsafeNativeMethods.IDBInfo _value;
	}
}
