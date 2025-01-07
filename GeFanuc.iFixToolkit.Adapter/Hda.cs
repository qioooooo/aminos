using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GeFanuc.iFixToolkit.Adapter
{
	public sealed class Hda
	{
		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaAddNtf@12")]
		public static extern int AddNtf(int gHandle, out int tHandle, string sNodeTagField);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaDefineGroup@4")]
		public static extern int DefineGroup(out int gHandle);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaDeleteGroup@4")]
		public static extern int DeleteGroup(int gHandle);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaDeleteNtf@8")]
		public static extern int DeleteNtf(int gHandle, int tHandle);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumGetNode@16")]
		public static extern int EnumGetNode(int gHandle, int lIndex, [Out] StringBuilder sbNode, int lMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumGetNtf@16")]
		public static extern int EnumGetNtf(int gHandle, int lIndex, [Out] StringBuilder sbNodeTagField, int lMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumNodes@8")]
		public static extern int EnumNodes(int gHandle, out int lCount);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaEnumNtfs@12")]
		public static extern int EnumNtfs(int gHandle, string sNodeName, out int lCount);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetCurrentHistorian@0")]
		public static extern int GetCurrentHistorian();

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetData@32")]
		public static extern int GetData(int gHandle, int tHandle, int iStartSample, int iNumSamples, [Out] float[] afValues, [Out] int[] aiTimes, [Out] int[] aiStatuses, [Out] int[] aiAlarms);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetDuration@12")]
		public static extern int GetDuration(int gHandle, [Out] StringBuilder DurationStr, int lMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetInterval@12")]
		public static extern int GetInterval(int gHandle, [Out] StringBuilder sbInterval, int lMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetMode@12")]
		public static extern int GetMode(int gHandle, int tHandle, out int lMode);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetNtf@16")]
		public static extern int GetNtf(int gHandle, int tHandle, [Out] StringBuilder sbNodeTagField, int lMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetNumSamples@12")]
		public static extern int GetNumSamples(int gHandle, int tHandle, out int lNumSamples);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetPath@12")]
		public static extern int GetPath(int gHandle, [Out] StringBuilder sbPath, int lMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaGetStart@20")]
		public static extern int GetStart(int gHandle, [Out] StringBuilder sbDate, int lMaxDateLength, [Out] StringBuilder sbTime, int lMaxTimeLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaNtfCount@8")]
		public static extern int NtfCount(int gHandle, out int lCount);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaRead@8")]
		public static extern int Read(int gHandle, int lReserved);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetDuration@8")]
		public static extern int SetDuration(int gHandle, string sDuration);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetInterval@8")]
		public static extern int SetInterval(int gHandle, string sInterval);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetMode@12")]
		public static extern int SetMode(int gHandle, int tHandle, int lMode);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetPath@8")]
		public static extern int SetPath(int gHandle, string szPath);

		[DllImport("FIXTOOLS.dll", EntryPoint = "HdaSetStart@12")]
		public static extern int SetStart(int gHandle, string szDate, string szTime);

		private Hda()
		{
		}
	}
}
