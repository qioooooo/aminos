using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000248 RID: 584
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBPROP
	{
		// Token: 0x0600206F RID: 8303 RVA: 0x00262B34 File Offset: 0x00261F34
		internal tagDBPROP()
		{
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00262B48 File Offset: 0x00261F48
		internal tagDBPROP(int propertyID, bool required, object value)
		{
			this.dwPropertyID = propertyID;
			this.dwOptions = (required ? 0 : 1);
			this.vValue = value;
		}

		// Token: 0x040014CD RID: 5325
		internal int dwPropertyID;

		// Token: 0x040014CE RID: 5326
		internal int dwOptions;

		// Token: 0x040014CF RID: 5327
		internal OleDbPropertyStatus dwStatus;

		// Token: 0x040014D0 RID: 5328
		internal tagDBIDX columnid;

		// Token: 0x040014D1 RID: 5329
		[MarshalAs(UnmanagedType.Struct)]
		internal object vValue;
	}
}
