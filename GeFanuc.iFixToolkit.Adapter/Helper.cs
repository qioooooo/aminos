using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x0200000F RID: 15
	public sealed class Helper
	{
		// Token: 0x0600005C RID: 92
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixCheckRights@4")]
		public static extern int FixCheckRights(string szSecArea);

		// Token: 0x0600005D RID: 93
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetCurrentUser@24")]
		public static extern int FixGetCurrentUser([Out] StringBuilder sbLoginName, int lMaxLoginLen, [Out] StringBuilder sbFullName, int lMaxFullLen, [Out] StringBuilder sbGroupName, int lMaxGroupLen);

		// Token: 0x0600005E RID: 94
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetMachineName@12")]
		public static extern int FixGetMachineName(string szNodeName, [Out] StringBuilder sbMachineName, short nMaxSize);

		// Token: 0x0600005F RID: 95
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetMyname@8")]
		public static extern int FixGetMyName([Out] StringBuilder sbNodeName, short nMaxSize);

		// Token: 0x06000060 RID: 96
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetPath@12")]
		public static extern int FixGetPath(string szPathId, [Out] StringBuilder sbPath, short nMaxLength);

		// Token: 0x06000061 RID: 97
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetRunningVersion@8")]
		public static extern int FixGetRunningVersion(out int lMajorVersion, out int lMinorVersion);

		// Token: 0x06000062 RID: 98
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixGetUserInfo@20")]
		public static extern int FixGetUserInfo(string szLoginName, [Out] StringBuilder sbFullName, short nMaxFullName, [Out] StringBuilder sbGroupName, short nMaxGroupName);

		// Token: 0x06000063 RID: 99
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixIsFixRunning@0")]
		public static extern int FixIsFixRunning();

		// Token: 0x06000064 RID: 100
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixLogin@8")]
		public static extern int FixLogin(string szUser, string szPassword);

		// Token: 0x06000065 RID: 101
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixLogout@0")]
		public static extern int FixLogout();

		// Token: 0x06000066 RID: 102
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixTaskDeregister@8")]
		public static extern int FixTaskDeregister(IntPtr hWnd, string szFileName);

		// Token: 0x06000067 RID: 103
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixTaskRegister@12")]
		public static extern int FixTaskRegister(IntPtr hWnd, int lOrder, string szFileName);

		// Token: 0x06000068 RID: 104
		[DllImport("FIXTOOLS.dll", EntryPoint = "PauseNam@0")]
		public static extern int PauseNam();

		// Token: 0x06000069 RID: 105
		[DllImport("FIXTOOLS.dll", EntryPoint = "RestartNam@0")]
		public static extern int RestartNam();

		// Token: 0x0600006A RID: 106
		[DllImport("FIXTOOLS.dll", EntryPoint = "TimAddSeconds@20")]
		public static extern int TimAddSeconds(StringBuilder sbDate, int lMaxDateLen, StringBuilder sbTime, int lMaxTimeLen, int lSeconds);

		// Token: 0x0600006B RID: 107
		[DllImport("FIXTOOLS.dll", EntryPoint = "TimIntegersToString@40")]
		public static extern int TimIntegersToString(short nYear, short nMonth, short nDay, short nHour, short nMinute, short nSeconds, [Out] StringBuilder sbDate, short nMaxDateLen, [Out] StringBuilder sbTime, short nMaxTimeLen);

		// Token: 0x0600006C RID: 108
		[DllImport("FIXTOOLS.dll", EntryPoint = "TimStringToIntegers@32")]
		public static extern int TimStringToIntegers(string szDate, string szTime, out short nYear, out short nMonth, out short nDay, out short nHour, out short nMinute, out short nSeconds);

		// Token: 0x0600006D RID: 109
		[DllImport("FIXTOOLS.dll", EntryPoint = "NlsGetText@12")]
		public static extern int NlsGetText(int nError, [Out] StringBuilder sbErrorMsg, short nMaxLength);

		// Token: 0x0600006E RID: 110
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixDeregisterConsole@0")]
		public static extern int FixDeregisterConsole();

		// Token: 0x0600006F RID: 111
		[DllImport("FIXTOOLS.dll", EntryPoint = "FixRegisterConsole@4")]
		public static extern int FixRegisterConsole(Helper.FixRegisterConsoleDelegate lpfcCallback);

		// Token: 0x06000070 RID: 112 RVA: 0x000025C7 File Offset: 0x000015C7
		private Helper()
		{
		}

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x06000072 RID: 114
		public delegate void FixRegisterConsoleDelegate();
	}
}
