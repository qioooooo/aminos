using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl
{
	// Token: 0x02000015 RID: 21
	[StandardModule]
	internal sealed class modpublic
	{
		// Token: 0x04000101 RID: 257
		public static object g_Excel;

		// Token: 0x04000102 RID: 258
		public static frmAlarmSndCfg frmConfig;

		// Token: 0x04000103 RID: 259
		public static frmAlmSndRun frmRun;

		// Token: 0x04000104 RID: 260
		public static bool g_StopSound = false;

		// Token: 0x04000105 RID: 261
		public static DateTime g_DemoEndTime;

		// Token: 0x04000106 RID: 262
		public static bool g_biFixStarted = false;

		// Token: 0x04000107 RID: 263
		public static bool g_Mute = false;

		// Token: 0x04000108 RID: 264
		public static bool g_Demo;

		// Token: 0x04000109 RID: 265
		public static int g_iDelay = 0;

		// Token: 0x0400010A RID: 266
		public static bool g_ShowEvt = false;

		// Token: 0x0400010B RID: 267
		public static bool g_ShowErr = false;

		// Token: 0x0400010C RID: 268
		public static string g_WMCOPYDATA = "";

		// Token: 0x0400010D RID: 269
		public static string g_sCfgName = Application.StartupPath + "\\DEFAULT.TAC";

		// Token: 0x0400010E RID: 270
		public static int g_iSoundTime = 5;

		// Token: 0x0400010F RID: 271
		public static int g_iScanTime = 500;

		// Token: 0x04000110 RID: 272
		public static bool g_bMenuBar;

		// Token: 0x04000111 RID: 273
		public static bool g_bBackground;

		// Token: 0x04000112 RID: 274
		public static bool g_bPlayOnce;

		// Token: 0x04000113 RID: 275
		public static bool g_bMultiInstance;

		// Token: 0x04000114 RID: 276
		public static bool g_bUsingPlayFinishedNowFunction;

		// Token: 0x04000115 RID: 277
		public static int g_nPlayCount = 0;

		// Token: 0x04000116 RID: 278
		internal static IntPtr g_Gnum = IntPtr.Zero;

		// Token: 0x04000117 RID: 279
		internal static int[] g_ReadTagHadles;

		// Token: 0x04000118 RID: 280
		public static DataTable g_dtSoundQueue = new DataTable();

		// Token: 0x04000119 RID: 281
		public static string[] g_aPriorityForArea = new string[] { "CRITICAL", "HIHI", "HIGH", "MEDIUM", "LOW", "LOLO", "INFO" };

		// Token: 0x0400011A RID: 282
		public static string[] g_aPriorityForAnalogTag = new string[] { "HIHI", "HI", "LO", "LOLO" };

		// Token: 0x0400011B RID: 283
		public static string[] g_aPriorityList = new string[] { "NOPRI", "CRITICAL", "HIHI", "HIGH", "MEDIUM", "LOW", "LOLO", "INFO", "HI", "LO" };

		// Token: 0x0400011C RID: 284
		internal const string g_ApTitle = "iFix警報輪迴播音系統";

		// Token: 0x0400011D RID: 285
		internal const string cTableCfg = "Config";

		// Token: 0x0400011E RID: 286
		internal const string cCacheName = "cache_txiFixAlmSndCly_";

		// Token: 0x0400011F RID: 287
		internal const string g_Ext = ".tac";

		// Token: 0x04000120 RID: 288
		internal const int g_nAvailableColumns = 9;

		// Token: 0x04000121 RID: 289
		internal const string encryptKey = "73627273";

		// Token: 0x04000122 RID: 290
		internal const string encryptIV = "TrendTek";

		// Token: 0x04000123 RID: 291
		public static string sTrendtek = "\r\n\r\nPlease contact Trendtek Automation\r\nwww.TrendTek.com.tw";
	}
}
