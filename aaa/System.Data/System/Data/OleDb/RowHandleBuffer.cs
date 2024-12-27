using System;
using System.Data.Common;
using System.Data.ProviderBase;

namespace System.Data.OleDb
{
	// Token: 0x02000260 RID: 608
	internal sealed class RowHandleBuffer : DbBuffer
	{
		// Token: 0x060020D7 RID: 8407 RVA: 0x00264D40 File Offset: 0x00264140
		internal RowHandleBuffer(IntPtr rowHandleFetchCount)
			: base((int)rowHandleFetchCount * ADP.PtrSize)
		{
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00264D60 File Offset: 0x00264160
		internal IntPtr GetRowHandle(int index)
		{
			return base.ReadIntPtr(index * ADP.PtrSize);
		}
	}
}
