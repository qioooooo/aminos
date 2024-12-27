using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000256 RID: 598
	internal struct IDBPropertiesWrapper : IDisposable
	{
		// Token: 0x060020A1 RID: 8353 RVA: 0x00263A74 File Offset: 0x00262E74
		internal IDBPropertiesWrapper(object unknown)
		{
			this._unknown = unknown;
			this._value = unknown as UnsafeNativeMethods.IDBProperties;
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x00263A94 File Offset: 0x00262E94
		internal UnsafeNativeMethods.IDBProperties Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00263AA8 File Offset: 0x00262EA8
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

		// Token: 0x0400151B RID: 5403
		private object _unknown;

		// Token: 0x0400151C RID: 5404
		private UnsafeNativeMethods.IDBProperties _value;
	}
}
