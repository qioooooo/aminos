using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000BA RID: 186
	internal sealed class IWbemQualifierSetFreeThreaded : IDisposable
	{
		// Token: 0x06000591 RID: 1425 RVA: 0x00027067 File Offset: 0x00026067
		public IWbemQualifierSetFreeThreaded(IntPtr pWbemQualifierSet)
		{
			this.pWbemQualifierSet = pWbemQualifierSet;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00027081 File Offset: 0x00026081
		public void Dispose()
		{
			this.Dispose_(false);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0002708A File Offset: 0x0002608A
		private void Dispose_(bool finalization)
		{
			if (this.pWbemQualifierSet != IntPtr.Zero)
			{
				Marshal.Release(this.pWbemQualifierSet);
				this.pWbemQualifierSet = IntPtr.Zero;
			}
			if (!finalization)
			{
				GC.KeepAlive(this);
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x000270C4 File Offset: 0x000260C4
		~IWbemQualifierSetFreeThreaded()
		{
			this.Dispose_(true);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000270F4 File Offset: 0x000260F4
		public int Get_(string wszName, int lFlags, ref object pVal, ref int plFlavor)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierGet_f(3, this.pWbemQualifierSet, wszName, lFlags, ref pVal, ref plFlavor);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0002713C File Offset: 0x0002613C
		public int Put_(string wszName, ref object pVal, int lFlavor)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierPut_f(4, this.pWbemQualifierSet, wszName, ref pVal, lFlavor);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00027184 File Offset: 0x00026184
		public int Delete_(string wszName)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierDelete_f(5, this.pWbemQualifierSet, wszName);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000271C8 File Offset: 0x000261C8
		public int GetNames_(int lFlags, out string[] pNames)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierGetNames_f(6, this.pWbemQualifierSet, lFlags, out pNames);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00027210 File Offset: 0x00026210
		public int BeginEnumeration_(int lFlags)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierBeginEnumeration_f(7, this.pWbemQualifierSet, lFlags);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00027254 File Offset: 0x00026254
		public int Next_(int lFlags, out string pstrName, out object pVal, out int plFlavor)
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierNext_f(8, this.pWbemQualifierSet, lFlags, out pstrName, out pVal, out plFlavor);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0002729C File Offset: 0x0002629C
		public int EndEnumeration_()
		{
			if (this.pWbemQualifierSet == IntPtr.Zero)
			{
				throw new ObjectDisposedException(IWbemQualifierSetFreeThreaded.name);
			}
			int num = WmiNetUtilsHelper.QualifierEndEnumeration_f(9, this.pWbemQualifierSet);
			GC.KeepAlive(this);
			return num;
		}

		// Token: 0x040002E2 RID: 738
		private const string SerializationBlobName = "flatWbemClassObject";

		// Token: 0x040002E3 RID: 739
		private static readonly string name = typeof(IWbemQualifierSetFreeThreaded).FullName;

		// Token: 0x040002E4 RID: 740
		public static Guid IID_IWbemClassObject = new Guid("DC12A681-737F-11CF-884D-00AA004B2E24");

		// Token: 0x040002E5 RID: 741
		private IntPtr pWbemQualifierSet = IntPtr.Zero;
	}
}
