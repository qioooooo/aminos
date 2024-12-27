using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000258 RID: 600
	internal struct IOpenRowsetWrapper : IDisposable
	{
		// Token: 0x060020A7 RID: 8359 RVA: 0x00263B34 File Offset: 0x00262F34
		internal IOpenRowsetWrapper(object unknown)
		{
			this._unknown = unknown;
			this._value = unknown as UnsafeNativeMethods.IOpenRowset;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060020A8 RID: 8360 RVA: 0x00263B54 File Offset: 0x00262F54
		internal UnsafeNativeMethods.IOpenRowset Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x00263B68 File Offset: 0x00262F68
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

		// Token: 0x0400151F RID: 5407
		private object _unknown;

		// Token: 0x04001520 RID: 5408
		private UnsafeNativeMethods.IOpenRowset _value;
	}
}
