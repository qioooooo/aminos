using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Microsoft.Internal.Performance
{
	// Token: 0x020001E9 RID: 489
	internal sealed class CodeMarkers
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x00022B99 File Offset: 0x00021B99
		private CodeMarkers()
		{
			this.fUseCodeMarkers = CodeMarkers.NativeMethods.FindAtom("VSCodeMarkersEnabled") != 0;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00022BB8 File Offset: 0x00021BB8
		public void CodeMarker(CodeMarkerEvent nTimerID)
		{
			if (!this.fUseCodeMarkers)
			{
				return;
			}
			try
			{
				CodeMarkers.NativeMethods.DllPerfCodeMarker((int)nTimerID, null, 0);
			}
			catch (DllNotFoundException)
			{
				this.fUseCodeMarkers = false;
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00022BF4 File Offset: 0x00021BF4
		public void CodeMarkerEx(CodeMarkerEvent nTimerID, byte[] aBuff)
		{
			if (aBuff == null)
			{
				throw new ArgumentNullException("aBuff");
			}
			if (!this.fUseCodeMarkers)
			{
				return;
			}
			try
			{
				CodeMarkers.NativeMethods.DllPerfCodeMarker((int)nTimerID, aBuff, aBuff.Length);
			}
			catch (DllNotFoundException)
			{
				this.fUseCodeMarkers = false;
			}
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00022C40 File Offset: 0x00021C40
		[Obsolete("Please use InitPerformanceDll(CodeMarkerApp, string) instead to specify a registry root")]
		public void InitPerformanceDll(CodeMarkerApp iApp)
		{
			this.InitPerformanceDll(iApp, "Software\\Microsoft\\VisualStudio\\8.0");
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00022C50 File Offset: 0x00021C50
		public void InitPerformanceDll(CodeMarkerApp iApp, string strRegRoot)
		{
			this.fUseCodeMarkers = false;
			if (!this.UseCodeMarkers(strRegRoot))
			{
				return;
			}
			try
			{
				CodeMarkers.NativeMethods.AddAtom("VSCodeMarkersEnabled");
				CodeMarkers.NativeMethods.DllInitPerf((int)iApp);
				this.fUseCodeMarkers = true;
			}
			catch (DllNotFoundException)
			{
			}
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00022C9C File Offset: 0x00021C9C
		[Obsolete("Second parameter is ignored. Please use InitPerformanceDll(CodeMarkerApp, string) instead to specify a registry root")]
		public void InitPerformanceDll(CodeMarkerApp iApp, bool bEndBoot)
		{
			this.InitPerformanceDll(iApp);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00022CA5 File Offset: 0x00021CA5
		private bool UseCodeMarkers(string strRegRoot)
		{
			return !string.IsNullOrEmpty(this.GetPerformanceSubKey(Registry.LocalMachine, strRegRoot));
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00022CBC File Offset: 0x00021CBC
		private string GetPerformanceSubKey(RegistryKey hKey, string strRegRoot)
		{
			if (hKey == null)
			{
				return null;
			}
			string text = null;
			using (RegistryKey registryKey = hKey.OpenSubKey(strRegRoot + "\\Performance"))
			{
				if (registryKey != null)
				{
					text = registryKey.GetValue("").ToString();
				}
			}
			return text;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00022D14 File Offset: 0x00021D14
		public void UninitializePerformanceDLL(CodeMarkerApp iApp)
		{
			if (!this.fUseCodeMarkers)
			{
				return;
			}
			this.fUseCodeMarkers = false;
			ushort num = CodeMarkers.NativeMethods.FindAtom("VSCodeMarkersEnabled");
			if (num != 0)
			{
				CodeMarkers.NativeMethods.DeleteAtom(num);
			}
			try
			{
				CodeMarkers.NativeMethods.DllUnInitPerf((int)iApp);
			}
			catch (DllNotFoundException)
			{
			}
		}

		// Token: 0x0400083D RID: 2109
		private const string AtomName = "VSCodeMarkersEnabled";

		// Token: 0x0400083E RID: 2110
		private const string DllName = "Microsoft.Internal.Performance.CodeMarkers.dll";

		// Token: 0x0400083F RID: 2111
		public static readonly CodeMarkers Instance = new CodeMarkers();

		// Token: 0x04000840 RID: 2112
		private bool fUseCodeMarkers;

		// Token: 0x020001EA RID: 490
		internal class NativeMethods
		{
			// Token: 0x060008CC RID: 2252
			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", EntryPoint = "InitPerf")]
			public static extern void DllInitPerf(int iApp);

			// Token: 0x060008CD RID: 2253
			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", EntryPoint = "UnInitPerf")]
			public static extern void DllUnInitPerf(int iApp);

			// Token: 0x060008CE RID: 2254
			[DllImport("Microsoft.Internal.Performance.CodeMarkers.dll", EntryPoint = "PerfCodeMarker")]
			public static extern void DllPerfCodeMarker(int nTimerID, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] aUserParams, int cbParams);

			// Token: 0x060008CF RID: 2255
			[DllImport("kernel32.dll")]
			public static extern ushort FindAtom(string lpString);

			// Token: 0x060008D0 RID: 2256
			[DllImport("kernel32.dll")]
			public static extern ushort AddAtom(string lpString);

			// Token: 0x060008D1 RID: 2257
			[DllImport("kernel32.dll")]
			public static extern ushort DeleteAtom(ushort atom);
		}
	}
}
