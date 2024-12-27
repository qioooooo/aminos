using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000BB RID: 187
	internal class MarshalWbemObject : ICustomMarshaler
	{
		// Token: 0x0600059D RID: 1437 RVA: 0x00027305 File Offset: 0x00026305
		public static ICustomMarshaler GetInstance(string cookie)
		{
			return new MarshalWbemObject(cookie);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0002730D File Offset: 0x0002630D
		private MarshalWbemObject(string cookie)
		{
			this.cookie = cookie;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0002731C File Offset: 0x0002631C
		public void CleanUpManagedData(object obj)
		{
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0002731E File Offset: 0x0002631E
		public void CleanUpNativeData(IntPtr pObj)
		{
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00027320 File Offset: 0x00026320
		public int GetNativeDataSize()
		{
			return -1;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00027323 File Offset: 0x00026323
		public IntPtr MarshalManagedToNative(object obj)
		{
			return (IntPtr)obj;
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x0002732B File Offset: 0x0002632B
		public object MarshalNativeToManaged(IntPtr pObj)
		{
			return new IWbemClassObjectFreeThreaded(pObj);
		}

		// Token: 0x040002E6 RID: 742
		private string cookie;
	}
}
