using System;
using System.Data.OracleClient;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

// Token: 0x02000005 RID: 5
[ComVisible(false)]
internal static class Bid
{
	// Token: 0x17000005 RID: 5
	// (get) Token: 0x0600000D RID: 13 RVA: 0x000543B8 File Offset: 0x000537B8
	internal static bool TraceOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x0600000E RID: 14 RVA: 0x000543D4 File Offset: 0x000537D4
	internal static bool AdvancedOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Advanced) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000543F4 File Offset: 0x000537F4
	internal static bool IsOn(Bid.ApiGroup flag)
	{
		return (Bid.modFlags & flag) != Bid.ApiGroup.Off;
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000010 RID: 16 RVA: 0x00054410 File Offset: 0x00053810
	internal static IntPtr NoData
	{
		get
		{
			return Bid.__noData;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00054424 File Offset: 0x00053824
	internal static void PutStr(string str)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)0U, str);
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00054464 File Offset: 0x00053864
	internal static void Trace(string strConst)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, strConst);
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000544A0 File Offset: 0x000538A0
	internal static void Trace(string fmtPrintfW, string a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000544E0 File Offset: 0x000538E0
	internal static void ScopeLeave(ref IntPtr hScp)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			if (hScp != Bid.NoData)
			{
				Bid.NativeMethods.ScopeLeave(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, ref hScp);
				return;
			}
		}
		else
		{
			hScp = Bid.NoData;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0005453C File Offset: 0x0005393C
	internal static void ScopeEnter(out IntPtr hScp, string strConst)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, strConst);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00054588 File Offset: 0x00053988
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000545D4 File Offset: 0x000539D4
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00054620 File Offset: 0x00053A20
	internal static Bid.ApiGroup SetApiGroupBits(Bid.ApiGroup mask, Bid.ApiGroup bits)
	{
		Bid.ApiGroup apiGroup2;
		lock (Bid._setBitsLock)
		{
			Bid.ApiGroup apiGroup = Bid.modFlags;
			if (mask != Bid.ApiGroup.Off)
			{
				Bid.modFlags ^= (bits ^ apiGroup) & mask;
			}
			apiGroup2 = apiGroup;
		}
		return apiGroup2;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x0005467C File Offset: 0x00053A7C
	internal static bool AddMetaText(string metaStr)
	{
		if (Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.AddMetaText(Bid.modID, Bid.DefaultCmdSpace, Bid.CtlCmd.AddMetaText, IntPtr.Zero, metaStr, IntPtr.Zero);
		}
		return true;
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000546BC File Offset: 0x00053ABC
	[Conditional("DEBUG")]
	internal static void DTRACE(string strConst)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, strConst);
		}
	}

	// Token: 0x0600001B RID: 27 RVA: 0x000546FC File Offset: 0x00053AFC
	[Conditional("DEBUG")]
	internal static void DTRACE(string clrFormatString, params object[] args)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, string.Format(CultureInfo.CurrentCulture, clrFormatString, args));
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00054744 File Offset: 0x00053B44
	[Conditional("DEBUG")]
	internal static void DASSERT(bool condition)
	{
		if (!condition)
		{
			global::System.Diagnostics.Trace.Assert(false);
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x0005475C File Offset: 0x00053B5C
	private static void deterministicStaticInit()
	{
		Bid.__noData = (IntPtr)(-1);
		Bid.__defaultCmdSpace = (IntPtr)(-1);
		Bid.modFlags = Bid.ApiGroup.Off;
		Bid.modIdentity = string.Empty;
		Bid.ctrlCallback = new Bid.CtrlCB(Bid.SetApiGroupBits);
		Bid.cookieObject = new Bid.BindingCookie();
		Bid.hCookie = GCHandle.Alloc(Bid.cookieObject, GCHandleType.Pinned);
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600001E RID: 30 RVA: 0x000547BC File Offset: 0x00053BBC
	internal static IntPtr DefaultCmdSpace
	{
		get
		{
			return Bid.__defaultCmdSpace;
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000547D0 File Offset: 0x00053BD0
	private static string getIdentity(Module mod)
	{
		object[] customAttributes = mod.GetCustomAttributes(typeof(BidIdentityAttribute), true);
		string text;
		if (customAttributes.Length == 0)
		{
			text = mod.Name;
		}
		else
		{
			text = ((BidIdentityAttribute)customAttributes[0]).IdentityString;
		}
		return text;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x0005480C File Offset: 0x00053C0C
	private static string getAppDomainFriendlyName()
	{
		string text = AppDomain.CurrentDomain.FriendlyName;
		if (text == null || text.Length <= 0)
		{
			text = "AppDomain.H" + AppDomain.CurrentDomain.GetHashCode();
		}
		return text;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0005484C File Offset: 0x00053C4C
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	private static string getModulePath(Module mod)
	{
		return mod.FullyQualifiedName;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00054860 File Offset: 0x00053C60
	private static void initEntryPoint()
	{
		Bid.NativeMethods.DllBidInitialize();
		Module manifestModule = Assembly.GetExecutingAssembly().ManifestModule;
		Bid.modIdentity = Bid.getIdentity(manifestModule);
		Bid.modID = Bid.NoData;
		Bid.BIDEXTINFO bidextinfo = new Bid.BIDEXTINFO(Marshal.GetHINSTANCE(manifestModule), Bid.getModulePath(manifestModule), Bid.getAppDomainFriendlyName(), Bid.hCookie.AddrOfPinnedObject());
		Bid.NativeMethods.DllBidEntryPoint(ref Bid.modID, 9210, Bid.modIdentity, 3489660928U, ref Bid.modFlags, Bid.ctrlCallback, ref bidextinfo, IntPtr.Zero, IntPtr.Zero);
		if (Bid.modID != Bid.NoData)
		{
			object[] customAttributes = manifestModule.GetCustomAttributes(typeof(BidMetaTextAttribute), true);
			foreach (object obj in customAttributes)
			{
				Bid.AddMetaText(((BidMetaTextAttribute)obj).MetaText);
			}
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00054934 File Offset: 0x00053D34
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	private static void doneEntryPoint()
	{
		if (Bid.modID == Bid.NoData)
		{
			Bid.modFlags = Bid.ApiGroup.Off;
			return;
		}
		try
		{
			Bid.NativeMethods.DllBidEntryPoint(ref Bid.modID, 0, IntPtr.Zero, 3489660928U, ref Bid.modFlags, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
			Bid.NativeMethods.DllBidFinalize();
		}
		catch
		{
			Bid.modFlags = Bid.ApiGroup.Off;
		}
		finally
		{
			Bid.cookieObject.Invalidate();
			Bid.modID = Bid.NoData;
			Bid.modFlags = Bid.ApiGroup.Off;
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x000549EC File Offset: 0x00053DEC
	private static IntPtr internalInitialize()
	{
		Bid.deterministicStaticInit();
		Bid.ai = new Bid.AutoInit();
		return Bid.modID;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00054A10 File Offset: 0x00053E10
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void PoolerTrace(string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & (Bid.ApiGroup)4096U) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00054A54 File Offset: 0x00053E54
	internal static void PoolerTrace(string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & (Bid.ApiGroup)4096U) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00054A98 File Offset: 0x00053E98
	internal static void PoolerTrace(string fmtPrintfW, int a1, int a2, int a3)
	{
		if ((Bid.modFlags & (Bid.ApiGroup)4096U) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00054ADC File Offset: 0x00053EDC
	internal static void PoolerScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & (Bid.ApiGroup)4096U) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00054B2C File Offset: 0x00053F2C
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00054B78 File Offset: 0x00053F78
	internal static void Trace(string fmtPrintfW, uint a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00054BB8 File Offset: 0x00053FB8
	internal static void Trace(string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00054BF8 File Offset: 0x00053FF8
	internal static void Trace(string fmtPrintfW, string a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00054C38 File Offset: 0x00054038
	internal static void Trace(string fmtPrintfW, int a1, string a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00054C78 File Offset: 0x00054078
	internal static void Trace(string fmtPrintfW, int a1, IntPtr a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00054CB8 File Offset: 0x000540B8
	internal static void Trace(string fmtPrintfW, int a1, string a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00054CF8 File Offset: 0x000540F8
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00054D38 File Offset: 0x00054138
	internal static void Trace(string fmtPrintfW, int a1, int a2, string a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00054D78 File Offset: 0x00054178
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, IntPtr a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00054DBC File Offset: 0x000541BC
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00054E00 File Offset: 0x00054200
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, uint a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00054E44 File Offset: 0x00054244
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, int a3, IntPtr a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00054E88 File Offset: 0x00054288
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5);
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00054ECC File Offset: 0x000542CC
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, uint a4, uint a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5);
		}
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00054F10 File Offset: 0x00054310
	internal static void Trace(string fmtPrintfW, int a1, string a2, int a3, string a4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5);
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00054F54 File Offset: 0x00054354
	internal static void Trace(string fmtPrintfW, IntPtr a1, int a2, IntPtr a3, IntPtr a4, int a5, int a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6);
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00054F9C File Offset: 0x0005439C
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, IntPtr a4, uint a5, uint a6, uint a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7);
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00054FE4 File Offset: 0x000543E4
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, string a4, int a5, string a6, int a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7);
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0005502C File Offset: 0x0005442C
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, int a5, int a6, int a7, int a8)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8);
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00055078 File Offset: 0x00054478
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, int a3, IntPtr a4, IntPtr a5, IntPtr a6, IntPtr a7, IntPtr a8)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8);
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000550C4 File Offset: 0x000544C4
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, uint a5, IntPtr a6, int a7, IntPtr a8, IntPtr a9, int a10, int a11)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00055114 File Offset: 0x00054514
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, uint a5, IntPtr a6, int a7, int a8, IntPtr a9, IntPtr a10, int a11, int a12)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12);
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00055168 File Offset: 0x00054568
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, string a3, int a4, IntPtr a5, int a6, int a7, IntPtr a8, int a9, IntPtr a10, int a11, IntPtr a12, uint a13, IntPtr a14, int a15)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15);
		}
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000551C0 File Offset: 0x000545C0
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00055200 File Offset: 0x00054600
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, IntPtr a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00055240 File Offset: 0x00054640
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, IntPtr a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00055280 File Offset: 0x00054680
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, IntPtr a1, IntPtr a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x06000045 RID: 69 RVA: 0x000552C0 File Offset: 0x000546C0
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, OciHandle a1, int a2, int a3, IntPtr a4, IntPtr a5, int a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2, a3, a4, a5, a6);
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0005530C File Offset: 0x0005470C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, int a1, IntPtr a2, IntPtr a3, IntPtr a4, IntPtr a5, int a6, IntPtr a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7);
		}
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00055354 File Offset: 0x00054754
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, int a1, IntPtr a2, IntPtr a3, IntPtr a4, IntPtr a5, int a6, IntPtr a7, int a8, int a9)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8, a9);
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000553A0 File Offset: 0x000547A0
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, OCI.ATTR a3, OciHandle a4, string a5, uint a6, int a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), OciHandle.GetAttributeName(a1, a3), OciHandle.HandleValueToTrace(a4), a5, a6, a7);
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x00055404 File Offset: 0x00054804
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, OCI.ATTR a3, OciHandle a4, IntPtr a5, uint a6, int a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), OciHandle.GetAttributeName(a1, a3), OciHandle.HandleValueToTrace(a4), a5, a6, a7);
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00055468 File Offset: 0x00054868
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, OCI.ATTR a3, OciHandle a4, int a5, uint a6, int a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), OciHandle.GetAttributeName(a1, a3), OciHandle.HandleValueToTrace(a4), a5, a6, a7);
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000554CC File Offset: 0x000548CC
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, OciHandle a3, int a4, IntPtr a5, int a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), OciHandle.HandleValueToTrace(a3), a4, a5, a6);
		}
	}

	// Token: 0x0600004C RID: 76 RVA: 0x00055528 File Offset: 0x00054928
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, int a3, uint a4, OCI.ATTR a5, OciHandle a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), a3, a4, OciHandle.GetAttributeName(a1, a5), OciHandle.HandleValueToTrace(a6));
		}
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00055588 File Offset: 0x00054988
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, OciHandle a3, uint a4, OCI.ATTR a5, OciHandle a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), OciHandle.HandleValueToTrace(a3), a4, OciHandle.GetAttributeName(a1, a5), OciHandle.HandleValueToTrace(a6));
		}
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000555EC File Offset: 0x000549EC
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, uint a3, IntPtr a4, int a5, int a6, OCI.DATATYPE a7, IntPtr a8, IntPtr a9, IntPtr a10, int a11, IntPtr a12, int a13)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), a3, a4, a5, a6, a7.ToString(), a8, a9, a10, a11, a12, a13);
		}
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00055654 File Offset: 0x00054A54
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, uint a3, uint a4, uint a5, uint a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), a3, a4, a5, a6);
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000556A4 File Offset: 0x00054AA4
	internal static void Trace(string fmtPrintfW, OciHandle a1, OCI.HTYPE a2, string a3, uint a4, OCI.ATTR a5, OciHandle a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), a2.ToString(), a3, a4, OciHandle.GetAttributeName(a1, a5), OciHandle.HandleValueToTrace(a6));
		}
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00055704 File Offset: 0x00054B04
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, string a3, int a4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), a3, a4, a5);
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00055754 File Offset: 0x00054B54
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, OciHandle a3, OCI.CRED a4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), OciHandle.HandleValueToTrace(a3), a4.ToString(), a5);
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000557B0 File Offset: 0x00054BB0
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, OciHandle a3, int a4, int a5, IntPtr a6, IntPtr a7, int a8)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), OciHandle.HandleValueToTrace(a3), a4, a5, a6, a7, a8);
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00055808 File Offset: 0x00054C08
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, int a3, int a4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), a3, a4, a5);
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00055858 File Offset: 0x00054C58
	internal static void Trace(string fmtPrintfW, OciHandle a1, OciHandle a2, int a3, int a4, int a5, string a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, OciHandle.HandleValueToTrace(a1), OciHandle.HandleValueToTrace(a2), a3, a4, a5, a6);
		}
	}

	// Token: 0x040000C8 RID: 200
	private const int BidVer = 9210;

	// Token: 0x040000C9 RID: 201
	private const uint configFlags = 3489660928U;

	// Token: 0x040000CA RID: 202
	private const string dllName = "System.Data.OracleClient.dll";

	// Token: 0x040000CB RID: 203
	private static IntPtr __noData;

	// Token: 0x040000CC RID: 204
	private static object _setBitsLock = new object();

	// Token: 0x040000CD RID: 205
	private static IntPtr modID = Bid.internalInitialize();

	// Token: 0x040000CE RID: 206
	private static Bid.ApiGroup modFlags;

	// Token: 0x040000CF RID: 207
	private static string modIdentity;

	// Token: 0x040000D0 RID: 208
	private static Bid.CtrlCB ctrlCallback;

	// Token: 0x040000D1 RID: 209
	private static Bid.BindingCookie cookieObject;

	// Token: 0x040000D2 RID: 210
	private static GCHandle hCookie;

	// Token: 0x040000D3 RID: 211
	private static IntPtr __defaultCmdSpace;

	// Token: 0x040000D4 RID: 212
	private static Bid.AutoInit ai;

	// Token: 0x02000006 RID: 6
	internal enum ApiGroup : uint
	{
		// Token: 0x040000D6 RID: 214
		Off,
		// Token: 0x040000D7 RID: 215
		Default,
		// Token: 0x040000D8 RID: 216
		Trace,
		// Token: 0x040000D9 RID: 217
		Scope = 4U,
		// Token: 0x040000DA RID: 218
		Perf = 8U,
		// Token: 0x040000DB RID: 219
		Resource = 16U,
		// Token: 0x040000DC RID: 220
		Memory = 32U,
		// Token: 0x040000DD RID: 221
		StatusOk = 64U,
		// Token: 0x040000DE RID: 222
		Advanced = 128U,
		// Token: 0x040000DF RID: 223
		MaskBid = 4095U,
		// Token: 0x040000E0 RID: 224
		MaskUser = 4294963200U,
		// Token: 0x040000E1 RID: 225
		MaskAll = 4294967295U
	}

	// Token: 0x02000007 RID: 7
	// (Invoke) Token: 0x06000058 RID: 88
	private delegate Bid.ApiGroup CtrlCB(Bid.ApiGroup mask, Bid.ApiGroup bits);

	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Sequential)]
	private class BindingCookie
	{
		// Token: 0x0600005B RID: 91 RVA: 0x000558CC File Offset: 0x00054CCC
		internal BindingCookie()
		{
			this._data = (IntPtr)(-1);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000558EC File Offset: 0x00054CEC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Invalidate()
		{
			this._data = (IntPtr)(-1);
		}

		// Token: 0x040000E2 RID: 226
		internal IntPtr _data;
	}

	// Token: 0x02000009 RID: 9
	private enum CtlCmd : uint
	{
		// Token: 0x040000E4 RID: 228
		Reverse = 1U,
		// Token: 0x040000E5 RID: 229
		Unicode,
		// Token: 0x040000E6 RID: 230
		DcsBase = 1073741824U,
		// Token: 0x040000E7 RID: 231
		DcsMax = 1610612732U,
		// Token: 0x040000E8 RID: 232
		CplBase = 1610612736U,
		// Token: 0x040000E9 RID: 233
		CplMax = 2147483644U,
		// Token: 0x040000EA RID: 234
		CmdSpaceCount = 1073741824U,
		// Token: 0x040000EB RID: 235
		CmdSpaceEnum = 1073741828U,
		// Token: 0x040000EC RID: 236
		CmdSpaceQuery = 1073741832U,
		// Token: 0x040000ED RID: 237
		GetEventID = 1073741846U,
		// Token: 0x040000EE RID: 238
		ParseString = 1073741850U,
		// Token: 0x040000EF RID: 239
		AddExtension = 1073741854U,
		// Token: 0x040000F0 RID: 240
		AddMetaText = 1073741858U,
		// Token: 0x040000F1 RID: 241
		AddResHandle = 1073741862U,
		// Token: 0x040000F2 RID: 242
		Shutdown = 1073741866U,
		// Token: 0x040000F3 RID: 243
		LastItem
	}

	// Token: 0x0200000A RID: 10
	private struct BIDEXTINFO
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00055908 File Offset: 0x00054D08
		internal BIDEXTINFO(IntPtr hMod, string modPath, string friendlyName, IntPtr cookiePtr)
		{
			this.hModule = hMod;
			this.DomainName = friendlyName;
			this.Reserved2 = 0;
			this.Reserved = 0;
			this.ModulePath = modPath;
			this.ModulePathA = IntPtr.Zero;
			this.pBindCookie = cookiePtr;
		}

		// Token: 0x040000F4 RID: 244
		private IntPtr hModule;

		// Token: 0x040000F5 RID: 245
		[MarshalAs(UnmanagedType.LPWStr)]
		private string DomainName;

		// Token: 0x040000F6 RID: 246
		private int Reserved2;

		// Token: 0x040000F7 RID: 247
		private int Reserved;

		// Token: 0x040000F8 RID: 248
		[MarshalAs(UnmanagedType.LPWStr)]
		private string ModulePath;

		// Token: 0x040000F9 RID: 249
		private IntPtr ModulePathA;

		// Token: 0x040000FA RID: 250
		private IntPtr pBindCookie;
	}

	// Token: 0x0200000B RID: 11
	private sealed class AutoInit : SafeHandle
	{
		// Token: 0x0600005E RID: 94 RVA: 0x0005594C File Offset: 0x00054D4C
		internal AutoInit()
			: base(IntPtr.Zero, true)
		{
			Bid.initEntryPoint();
			this._bInitialized = true;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00055974 File Offset: 0x00054D74
		protected override bool ReleaseHandle()
		{
			this._bInitialized = false;
			Bid.doneEntryPoint();
			return true;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00055990 File Offset: 0x00054D90
		public override bool IsInvalid
		{
			get
			{
				return !this._bInitialized;
			}
		}

		// Token: 0x040000FB RID: 251
		private bool _bInitialized;
	}

	// Token: 0x0200000C RID: 12
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	private static class NativeMethods
	{
		// Token: 0x06000061 RID: 97
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DllBidPutStrW")]
		internal static extern void PutStr(IntPtr hID, UIntPtr src, UIntPtr info, string str);

		// Token: 0x06000062 RID: 98
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string strConst);

		// Token: 0x06000063 RID: 99
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, string a1);

		// Token: 0x06000064 RID: 100
		[DllImport("System.Data.OracleClient.dll", EntryPoint = "DllBidScopeLeave")]
		internal static extern void ScopeLeave(IntPtr hID, UIntPtr src, UIntPtr info, ref IntPtr hScp);

		// Token: 0x06000065 RID: 101
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string strConst);

		// Token: 0x06000066 RID: 102
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1);

		// Token: 0x06000067 RID: 103
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2);

		// Token: 0x06000068 RID: 104
		[DllImport("System.Data.OracleClient.dll", CharSet = CharSet.Unicode, EntryPoint = "DllBidCtlProc")]
		internal static extern void AddMetaText(IntPtr hID, IntPtr cmdSpace, Bid.CtlCmd cmd, IntPtr nop1, string txtID, IntPtr nop2);

		// Token: 0x06000069 RID: 105
		[DllImport("System.Data.OracleClient.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern void DllBidEntryPoint(ref IntPtr hID, int bInitAndVer, string sIdentity, uint propBits, ref Bid.ApiGroup pGblFlags, Bid.CtrlCB fAddr, ref Bid.BIDEXTINFO pExtInfo, IntPtr pHooks, IntPtr pHdr);

		// Token: 0x0600006A RID: 106
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll")]
		internal static extern void DllBidEntryPoint(ref IntPtr hID, int bInitAndVer, IntPtr unused1, uint propBits, ref Bid.ApiGroup pGblFlags, IntPtr unused2, IntPtr unused3, IntPtr unused4, IntPtr unused5);

		// Token: 0x0600006B RID: 107
		[DllImport("System.Data.OracleClient.dll")]
		internal static extern void DllBidInitialize();

		// Token: 0x0600006C RID: 108
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll")]
		internal static extern void DllBidFinalize();

		// Token: 0x0600006D RID: 109
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2);

		// Token: 0x0600006E RID: 110
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, uint a1, int a2);

		// Token: 0x0600006F RID: 111
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2);

		// Token: 0x06000070 RID: 112
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, string a1, int a2);

		// Token: 0x06000071 RID: 113
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2);

		// Token: 0x06000072 RID: 114
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, IntPtr a2, int a3);

		// Token: 0x06000073 RID: 115
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, int a3);

		// Token: 0x06000074 RID: 116
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3);

		// Token: 0x06000075 RID: 117
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, int a3);

		// Token: 0x06000076 RID: 118
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, string a3);

		// Token: 0x06000077 RID: 119
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, IntPtr a4);

		// Token: 0x06000078 RID: 120
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4);

		// Token: 0x06000079 RID: 121
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, uint a4);

		// Token: 0x0600007A RID: 122
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, int a3, IntPtr a4);

		// Token: 0x0600007B RID: 123
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, int a5);

		// Token: 0x0600007C RID: 124
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, uint a4, uint a5);

		// Token: 0x0600007D RID: 125
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, int a3, string a4, int a5);

		// Token: 0x0600007E RID: 126
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, string a3, int a4, int a5);

		// Token: 0x0600007F RID: 127
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, int a3, int a4, int a5);

		// Token: 0x06000080 RID: 128
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, uint a3, uint a4, uint a5, uint a6);

		// Token: 0x06000081 RID: 129
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, int a2, IntPtr a3, IntPtr a4, int a5, int a6);

		// Token: 0x06000082 RID: 130
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, IntPtr a4, uint a5, uint a6, uint a7);

		// Token: 0x06000083 RID: 131
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, string a4, int a5, string a6, int a7);

		// Token: 0x06000084 RID: 132
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, int a5, int a6, int a7, int a8);

		// Token: 0x06000085 RID: 133
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, int a5, IntPtr a6, IntPtr a7, int a8);

		// Token: 0x06000086 RID: 134
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, int a3, IntPtr a4, IntPtr a5, IntPtr a6, IntPtr a7, IntPtr a8);

		// Token: 0x06000087 RID: 135
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, uint a5, IntPtr a6, int a7, IntPtr a8, IntPtr a9, int a10, int a11);

		// Token: 0x06000088 RID: 136
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, int a4, uint a5, IntPtr a6, int a7, int a8, IntPtr a9, IntPtr a10, int a11, int a12);

		// Token: 0x06000089 RID: 137
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, string a3, int a4, IntPtr a5, int a6, int a7, IntPtr a8, int a9, IntPtr a10, int a11, IntPtr a12, uint a13, IntPtr a14, int a15);

		// Token: 0x0600008A RID: 138
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1);

		// Token: 0x0600008B RID: 139
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1);

		// Token: 0x0600008C RID: 140
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, int a2);

		// Token: 0x0600008D RID: 141
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, int a3);

		// Token: 0x0600008E RID: 142
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, IntPtr a2, IntPtr a3, IntPtr a4, IntPtr a5, int a6, IntPtr a7);

		// Token: 0x0600008F RID: 143
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, int a2, int a3, IntPtr a4, IntPtr a5, int a6);

		// Token: 0x06000090 RID: 144
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, IntPtr a2, IntPtr a3, IntPtr a4, IntPtr a5, int a6, IntPtr a7, int a8, int a9);

		// Token: 0x06000091 RID: 145
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, string a3, IntPtr a4, string a5, uint a6, int a7);

		// Token: 0x06000092 RID: 146
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, string a3, IntPtr a4, IntPtr a5, uint a6, int a7);

		// Token: 0x06000093 RID: 147
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, string a3, IntPtr a4, int a5, uint a6, int a7);

		// Token: 0x06000094 RID: 148
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, IntPtr a3, int a4, IntPtr a5, int a6);

		// Token: 0x06000095 RID: 149
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, int a3, uint a4, string a5, IntPtr a6);

		// Token: 0x06000096 RID: 150
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, IntPtr a3, uint a4, string a5, IntPtr a6);

		// Token: 0x06000097 RID: 151
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, uint a3, IntPtr a4, int a5, int a6, string a7, IntPtr a8, IntPtr a9, IntPtr a10, int a11, IntPtr a12, int a13);

		// Token: 0x06000098 RID: 152
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, string a2, string a3, uint a4, string a5, IntPtr a6);

		// Token: 0x06000099 RID: 153
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, IntPtr a3, string a4, int a5);

		// Token: 0x0600009A RID: 154
		[DllImport("System.Data.OracleClient.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1, IntPtr a2, int a3, int a4, int a5, string a6);
	}
}
