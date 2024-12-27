using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000257 RID: 599
	internal struct IDBSchemaRowsetWrapper : IDisposable
	{
		// Token: 0x060020A4 RID: 8356 RVA: 0x00263AD4 File Offset: 0x00262ED4
		internal IDBSchemaRowsetWrapper(object unknown)
		{
			this._unknown = unknown;
			this._value = unknown as UnsafeNativeMethods.IDBSchemaRowset;
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x00263AF4 File Offset: 0x00262EF4
		internal UnsafeNativeMethods.IDBSchemaRowset Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00263B08 File Offset: 0x00262F08
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

		// Token: 0x0400151D RID: 5405
		private object _unknown;

		// Token: 0x0400151E RID: 5406
		private UnsafeNativeMethods.IDBSchemaRowset _value;
	}
}
