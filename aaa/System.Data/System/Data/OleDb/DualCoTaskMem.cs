using System;
using System.Data.Common;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x0200025F RID: 607
	internal sealed class DualCoTaskMem : SafeHandle
	{
		// Token: 0x060020D0 RID: 8400 RVA: 0x00264B4C File Offset: 0x00263F4C
		private DualCoTaskMem()
			: base(IntPtr.Zero, true)
		{
			this.handle2 = IntPtr.Zero;
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00264B70 File Offset: 0x00263F70
		internal DualCoTaskMem(UnsafeNativeMethods.IDBInfo dbInfo, int[] literals, out int literalCount, out IntPtr literalInfo, out OleDbHResult hr)
			: this()
		{
			int num = ((literals != null) ? literals.Length : 0);
			Bid.Trace("<oledb.IDBInfo.GetLiteralInfo|API|OLEDB>\n");
			hr = dbInfo.GetLiteralInfo(num, literals, out literalCount, out this.handle, out this.handle2);
			literalInfo = this.handle;
			Bid.Trace("<oledb.IDBInfo.GetLiteralInfo|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x00264BCC File Offset: 0x00263FCC
		internal DualCoTaskMem(UnsafeNativeMethods.IColumnsInfo columnsInfo, out IntPtr columnCount, out IntPtr columnInfos, out OleDbHResult hr)
			: this()
		{
			Bid.Trace("<oledb.IColumnsInfo.GetColumnInfo|API|OLEDB>\n");
			hr = columnsInfo.GetColumnInfo(out columnCount, out this.handle, out this.handle2);
			columnInfos = this.handle;
			Bid.Trace("<oledb.IColumnsInfo.GetColumnInfo|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x00264C18 File Offset: 0x00264018
		internal DualCoTaskMem(UnsafeNativeMethods.IDBSchemaRowset dbSchemaRowset, out int schemaCount, out IntPtr schemaGuids, out IntPtr schemaRestrictions, out OleDbHResult hr)
			: this()
		{
			Bid.Trace("<oledb.IDBSchemaRowset.GetSchemas|API|OLEDB>\n");
			hr = dbSchemaRowset.GetSchemas(out schemaCount, out this.handle, out this.handle2);
			schemaGuids = this.handle;
			schemaRestrictions = this.handle2;
			Bid.Trace("<oledb.IDBSchemaRowset.GetSchemas|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00264C74 File Offset: 0x00264074
		internal DualCoTaskMem(UnsafeNativeMethods.IColumnsRowset icolumnsRowset, out IntPtr cOptColumns, out OleDbHResult hr)
			: base(IntPtr.Zero, true)
		{
			Bid.Trace("<oledb.IColumnsRowset.GetAvailableColumns|API|OLEDB>\n");
			hr = icolumnsRowset.GetAvailableColumns(out cOptColumns, out this.handle);
			Bid.Trace("<oledb.IColumnsRowset.GetAvailableColumns|API|OLEDB|RET> %08X{HRESULT}\n", hr);
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x00264CB4 File Offset: 0x002640B4
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle && IntPtr.Zero == this.handle2;
			}
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00264CE8 File Offset: 0x002640E8
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				SafeNativeMethods.CoTaskMemFree(handle);
			}
			handle = this.handle2;
			this.handle2 = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				SafeNativeMethods.CoTaskMemFree(handle);
			}
			return true;
		}

		// Token: 0x0400153F RID: 5439
		private IntPtr handle2;
	}
}
