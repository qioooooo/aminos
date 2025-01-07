using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GeFanuc.iFixToolkit.Adapter
{
	public sealed class Helper
	{
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixCheckRights@4")]
		public static extern int FixCheckRights(string szSecArea);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetCurrentUser@24")]
		public static extern int FixGetCurrentUser([Out] StringBuilder sbLoginName, int lMaxLoginLen, [Out] StringBuilder sbFullName, int lMaxFullLen, [Out] StringBuilder sbGroupName, int lMaxGroupLen);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetMachineName@12")]
		public static extern int FixGetMachineName(string szNodeName, [Out] StringBuilder sbMachineName, short nMaxSize);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetMyname@8")]
		public static extern int FixGetMyName([Out] StringBuilder sbNodeName, short nMaxSize);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetPath@12")]
		public static extern int FixGetPath(string szPathId, [Out] StringBuilder sbPath, short nMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetRunningVersion@8")]
		public static extern int FixGetRunningVersion(out int lMajorVersion, out int lMinorVersion);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetUserInfo@20")]
		public static extern int FixGetUserInfo(string szLoginName, [Out] StringBuilder sbFullName, short nMaxFullName, [Out] StringBuilder sbGroupName, short nMaxGroupName);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixIsFixRunning@0")]
		public static extern int FixIsFixRunning();

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixLogin@8")]
		public static extern int FixLogin(string szUser, string szPassword);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixLogout@0")]
		public static extern int FixLogout();

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixTaskDeregister@8")]
		public static extern int FixTaskDeregister(IntPtr hWnd, string szFileName);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixTaskRegister@12")]
		public static extern int FixTaskRegister(IntPtr hWnd, int lOrder, string szFileName);

		[DllImport("FIXTOOLS.dll", EntryPoint = "PauseNam@0")]
		public static extern int PauseNam();

		[DllImport("FIXTOOLS.dll", EntryPoint = "RestartNam@0")]
		public static extern int RestartNam();

		[DllImport("FIXTOOLS.dll", EntryPoint = "TimAddSeconds@20")]
		public static extern int TimAddSeconds(StringBuilder sbDate, int lMaxDateLen, StringBuilder sbTime, int lMaxTimeLen, int lSeconds);

		[DllImport("FIXTOOLS.dll", EntryPoint = "TimIntegersToString@40")]
		public static extern int TimIntegersToString(short nYear, short nMonth, short nDay, short nHour, short nMinute, short nSeconds, [Out] StringBuilder sbDate, short nMaxDateLen, [Out] StringBuilder sbTime, short nMaxTimeLen);

		[DllImport("FIXTOOLS.dll", EntryPoint = "TimStringToIntegers@32")]
		public static extern int TimStringToIntegers(string szDate, string szTime, out short nYear, out short nMonth, out short nDay, out short nHour, out short nMinute, out short nSeconds);

		[DllImport("FIXTOOLS.dll", EntryPoint = "NlsGetText@12")]
		public static extern int NlsGetText(int nError, [Out] StringBuilder sbErrorMsg, short nMaxLength);

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixDeregisterConsole@0")]
		public static extern int FixDeregisterConsole();

		[DllImport("FIXTOOLS.dll", EntryPoint = "FixRegisterConsole@4")]
		public static extern int FixRegisterConsole(Helper.FixRegisterConsoleDelegate lpfcCallback);

		private Helper()
		{
		}

		public delegate void FixRegisterConsoleDelegate();
	}
}
