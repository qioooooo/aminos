using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x0200076D RID: 1901
	[ComVisible(true)]
	[Guid("82840BE1-D273-11D2-B94A-00600893B17A")]
	[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class PerformanceCounterManager : ICollectData
	{
		// Token: 0x06003A82 RID: 14978 RVA: 0x000F923E File Offset: 0x000F823E
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public PerformanceCounterManager()
		{
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x000F9246 File Offset: 0x000F8246
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		void ICollectData.CollectData(int callIdx, IntPtr valueNamePtr, IntPtr dataPtr, int totalBytes, out IntPtr res)
		{
			res = (IntPtr)(-1);
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x000F9255 File Offset: 0x000F8255
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		void ICollectData.CloseData()
		{
		}
	}
}
