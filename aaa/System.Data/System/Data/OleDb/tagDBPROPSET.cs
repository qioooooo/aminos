using System;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
	// Token: 0x02000247 RID: 583
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal sealed class tagDBPROPSET
	{
		// Token: 0x0600206D RID: 8301 RVA: 0x00262AFC File Offset: 0x00261EFC
		internal tagDBPROPSET()
		{
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x00262B10 File Offset: 0x00261F10
		internal tagDBPROPSET(int propertyCount, Guid propertySet)
		{
			this.cProperties = propertyCount;
			this.guidPropertySet = propertySet;
		}

		// Token: 0x040014CA RID: 5322
		internal IntPtr rgProperties;

		// Token: 0x040014CB RID: 5323
		internal int cProperties;

		// Token: 0x040014CC RID: 5324
		internal Guid guidPropertySet;
	}
}
