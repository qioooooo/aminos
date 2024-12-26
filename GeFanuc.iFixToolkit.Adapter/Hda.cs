using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x0200000E RID: 14
	public sealed class Hda
	{
		// Token: 0x06000043 RID: 67
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaAddNtf@12")]
		public static extern int AddNtf(int gHandle, out int tHandle, string sNodeTagField);

		// Token: 0x06000044 RID: 68
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaDefineGroup@4")]
		public static extern int DefineGroup(out int gHandle);

		// Token: 0x06000045 RID: 69
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaDeleteGroup@4")]
		public static extern int DeleteGroup(int gHandle);

		// Token: 0x06000046 RID: 70
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaDeleteNtf@8")]
		public static extern int DeleteNtf(int gHandle, int tHandle);

		// Token: 0x06000047 RID: 71
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumGetNode@16")]
		public static extern int EnumGetNode(int gHandle, int lIndex, [Out] StringBuilder sbNode, int lMaxLength);

		// Token: 0x06000048 RID: 72
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumGetNtf@16")]
		public static extern int EnumGetNtf(int gHandle, int lIndex, [Out] StringBuilder sbNodeTagField, int lMaxLength);

		// Token: 0x06000049 RID: 73
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumNodes@8")]
		public static extern int EnumNodes(int gHandle, out int lCount);

		// Token: 0x0600004A RID: 74
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumNtfs@12")]
		public static extern int EnumNtfs(int gHandle, string sNodeName, out int lCount);

		// Token: 0x0600004B RID: 75
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetCurrentHistorian@0")]
		public static extern int GetCurrentHistorian();

		// Token: 0x0600004C RID: 76
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetData@32")]
		public static extern int GetData(int gHandle, int tHandle, int iStartSample, int iNumSamples, [Out] float[] afValues, [Out] int[] aiTimes, [Out] int[] aiStatuses, [Out] int[] aiAlarms);

		// Token: 0x0600004D RID: 77
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetDuration@12")]
		public static extern int GetDuration(int gHandle, [Out] StringBuilder DurationStr, int lMaxLength);

		// Token: 0x0600004E RID: 78
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetInterval@12")]
		public static extern int GetInterval(int gHandle, [Out] StringBuilder sbInterval, int lMaxLength);

		// Token: 0x0600004F RID: 79
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetMode@12")]
		public static extern int GetMode(int gHandle, int tHandle, out int lMode);

		// Token: 0x06000050 RID: 80
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetNtf@16")]
		public static extern int GetNtf(int gHandle, int tHandle, [Out] StringBuilder sbNodeTagField, int lMaxLength);

		// Token: 0x06000051 RID: 81
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetNumSamples@12")]
		public static extern int GetNumSamples(int gHandle, int tHandle, out int lNumSamples);

		// Token: 0x06000052 RID: 82
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetPath@12")]
		public static extern int GetPath(int gHandle, [Out] StringBuilder sbPath, int lMaxLength);

		// Token: 0x06000053 RID: 83
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetStart@20")]
		public static extern int GetStart(int gHandle, [Out] StringBuilder sbDate, int lMaxDateLength, [Out] StringBuilder sbTime, int lMaxTimeLength);

		// Token: 0x06000054 RID: 84
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaNtfCount@8")]
		public static extern int NtfCount(int gHandle, out int lCount);

		// Token: 0x06000055 RID: 85
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaRead@8")]
		public static extern int Read(int gHandle, int lReserved);

		// Token: 0x06000056 RID: 86
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetDuration@8")]
		public static extern int SetDuration(int gHandle, string sDuration);

		// Token: 0x06000057 RID: 87
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetInterval@8")]
		public static extern int SetInterval(int gHandle, string sInterval);

		// Token: 0x06000058 RID: 88
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetMode@12")]
		public static extern int SetMode(int gHandle, int tHandle, int lMode);

		// Token: 0x06000059 RID: 89
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetPath@8")]
		public static extern int SetPath(int gHandle, string szPath);

		// Token: 0x0600005A RID: 90
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetStart@12")]
		public static extern int SetStart(int gHandle, string szDate, string szTime);

		// Token: 0x0600005B RID: 91 RVA: 0x000025BF File Offset: 0x000015BF
		private Hda()
		{
		}
	}
}
