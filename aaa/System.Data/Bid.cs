using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

// Token: 0x02000109 RID: 265
[ComVisible(false)]
internal static class Bid
{
	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06001095 RID: 4245 RVA: 0x00219668 File Offset: 0x00218A68
	internal static bool DefaultOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Default) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001096 RID: 4246 RVA: 0x00219684 File Offset: 0x00218A84
	internal static bool TraceOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001097 RID: 4247 RVA: 0x002196A0 File Offset: 0x00218AA0
	internal static bool ScopeOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06001098 RID: 4248 RVA: 0x002196BC File Offset: 0x00218ABC
	internal static bool PerfOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Perf) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06001099 RID: 4249 RVA: 0x002196D8 File Offset: 0x00218AD8
	internal static bool ResourceOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Resource) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x0600109A RID: 4250 RVA: 0x002196F4 File Offset: 0x00218AF4
	internal static bool MemoryOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Memory) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x0600109B RID: 4251 RVA: 0x00219710 File Offset: 0x00218B10
	internal static bool StatusOkOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.StatusOk) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x0600109C RID: 4252 RVA: 0x0021972C File Offset: 0x00218B2C
	internal static bool AdvancedOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.Advanced) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x0600109D RID: 4253 RVA: 0x0021974C File Offset: 0x00218B4C
	internal static bool StateDumpOn
	{
		get
		{
			return (Bid.modFlags & Bid.ApiGroup.StateDump) != Bid.ApiGroup.Off;
		}
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0021976C File Offset: 0x00218B6C
	internal static bool IsOn(Bid.ApiGroup flag)
	{
		return (Bid.modFlags & flag) != Bid.ApiGroup.Off;
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x00219788 File Offset: 0x00218B88
	internal static bool AreOn(Bid.ApiGroup flags)
	{
		return (Bid.modFlags & flags) == flags;
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x060010A0 RID: 4256 RVA: 0x002197A0 File Offset: 0x00218BA0
	internal static IntPtr NoData
	{
		get
		{
			return Bid.__noData;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x060010A1 RID: 4257 RVA: 0x002197B4 File Offset: 0x00218BB4
	internal static IntPtr ID
	{
		get
		{
			return Bid.modID;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x060010A2 RID: 4258 RVA: 0x002197C8 File Offset: 0x00218BC8
	internal static bool IsInitialized
	{
		get
		{
			return Bid.modID != Bid.NoData;
		}
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x002197E4 File Offset: 0x00218BE4
	internal static void PutStr(string str)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)0U, str);
		}
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x00219824 File Offset: 0x00218C24
	internal static void PutStrLine(string str)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, str);
		}
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x00219864 File Offset: 0x00218C64
	internal static void PutNewLine()
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)2U, string.Empty);
		}
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x002198A8 File Offset: 0x00218CA8
	internal static void PutStrEx(uint flags, string str)
	{
		if (Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)flags, str);
		}
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x002198DC File Offset: 0x00218CDC
	internal static void PutSmartNewLine()
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, string.Empty);
		}
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x00219920 File Offset: 0x00218D20
	internal static uint NewLineEx(bool addNewLine)
	{
		if (!addNewLine)
		{
			return 0U;
		}
		return 1U;
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x00219934 File Offset: 0x00218D34
	internal static void Trace(string strConst)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, strConst);
		}
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x00219970 File Offset: 0x00218D70
	internal static void TraceEx(uint flags, string strConst)
	{
		if (Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, (UIntPtr)flags, strConst);
		}
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x002199A4 File Offset: 0x00218DA4
	internal static void Trace(string fmtPrintfW, string a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x002199E4 File Offset: 0x00218DE4
	internal static void TraceEx(uint flags, string fmtPrintfW, string a1)
	{
		if (Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, (UIntPtr)flags, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x00219A1C File Offset: 0x00218E1C
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

	// Token: 0x060010AE RID: 4270 RVA: 0x00219A78 File Offset: 0x00218E78
	internal static void ScopeEnter(out IntPtr hScp, string strConst)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, strConst);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x00219AC4 File Offset: 0x00218EC4
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, string a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x00219B10 File Offset: 0x00218F10
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, IntPtr a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x00219B5C File Offset: 0x00218F5C
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x00219BA8 File Offset: 0x00218FA8
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x00219BF4 File Offset: 0x00218FF4
	internal static bool Enabled(string traceControlString)
	{
		return (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && !(Bid.modID == Bid.NoData) && Bid.NativeMethods.Enabled(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, traceControlString);
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x00219C34 File Offset: 0x00219034
	internal static void TraceBin(string constStrHeader, byte[] buff, ushort length)
	{
		if (Bid.modID != Bid.NoData)
		{
			if (constStrHeader != null && constStrHeader.Length > 0)
			{
				Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, constStrHeader);
			}
			if ((ushort)buff.Length < length)
			{
				length = (ushort)buff.Length;
			}
			Bid.NativeMethods.TraceBin(Bid.modID, UIntPtr.Zero, (UIntPtr)16U, "<Trace|BLOB> %p %u\n", buff, length);
		}
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x00219CA0 File Offset: 0x002190A0
	internal static void TraceBinEx(byte[] buff, ushort length)
	{
		if (Bid.modID != Bid.NoData)
		{
			if ((ushort)buff.Length < length)
			{
				length = (ushort)buff.Length;
			}
			Bid.NativeMethods.TraceBin(Bid.modID, UIntPtr.Zero, (UIntPtr)16U, "<Trace|BLOB> %p %u\n", buff, length);
		}
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x00219CE8 File Offset: 0x002190E8
	internal static Bid.ApiGroup GetApiGroupBits(Bid.ApiGroup mask)
	{
		return Bid.modFlags & mask;
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x00219CFC File Offset: 0x002190FC
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

	// Token: 0x060010B8 RID: 4280 RVA: 0x00219D58 File Offset: 0x00219158
	internal static bool AddMetaText(string metaStr)
	{
		if (Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.AddMetaText(Bid.modID, Bid.DefaultCmdSpace, Bid.CtlCmd.AddMetaText, IntPtr.Zero, metaStr, IntPtr.Zero);
		}
		return true;
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x00219D98 File Offset: 0x00219198
	[Conditional("DEBUG")]
	internal static void DTRACE(string strConst)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, strConst);
		}
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x00219DD8 File Offset: 0x002191D8
	[Conditional("DEBUG")]
	internal static void DTRACE(string clrFormatString, params object[] args)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.PutStr(Bid.modID, UIntPtr.Zero, (UIntPtr)1U, string.Format(CultureInfo.CurrentCulture, clrFormatString, args));
		}
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x00219E20 File Offset: 0x00219220
	[Conditional("DEBUG")]
	internal static void DASSERT(bool condition)
	{
		if (!condition)
		{
			global::System.Diagnostics.Trace.Assert(false);
		}
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x00219E38 File Offset: 0x00219238
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

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060010BD RID: 4285 RVA: 0x00219E98 File Offset: 0x00219298
	internal static IntPtr DefaultCmdSpace
	{
		get
		{
			return Bid.__defaultCmdSpace;
		}
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x00219EAC File Offset: 0x002192AC
	internal static IntPtr GetCmdSpaceID(string textID)
	{
		if (!(Bid.modID != Bid.NoData))
		{
			return IntPtr.Zero;
		}
		return Bid.NativeMethods.GetCmdSpaceID(Bid.modID, Bid.DefaultCmdSpace, Bid.CtlCmd.CmdSpaceQuery, 0U, textID, IntPtr.Zero);
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x00219EEC File Offset: 0x002192EC
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

	// Token: 0x060010C0 RID: 4288 RVA: 0x00219F28 File Offset: 0x00219328
	private static string getAppDomainFriendlyName()
	{
		string text = AppDomain.CurrentDomain.FriendlyName;
		if (text == null || text.Length <= 0)
		{
			text = "AppDomain.H" + AppDomain.CurrentDomain.GetHashCode();
		}
		return text;
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x00219F68 File Offset: 0x00219368
	[FileIOPermission(SecurityAction.Assert, Unrestricted = true)]
	private static string getModulePath(Module mod)
	{
		return mod.FullyQualifiedName;
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x00219F7C File Offset: 0x0021937C
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

	// Token: 0x060010C3 RID: 4291 RVA: 0x0021A050 File Offset: 0x00219450
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

	// Token: 0x060010C4 RID: 4292 RVA: 0x0021A108 File Offset: 0x00219508
	private static IntPtr internalInitialize()
	{
		Bid.deterministicStaticInit();
		Bid.ai = new Bid.AutoInit();
		return Bid.modID;
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0021A12C File Offset: 0x0021952C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void PoolerTrace(string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Pooling) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x0021A170 File Offset: 0x00219570
	internal static void PoolerTrace(string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Pooling) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x0021A1B4 File Offset: 0x002195B4
	internal static void PoolerTrace(string fmtPrintfW, int a1, int a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Pooling) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x0021A1F8 File Offset: 0x002195F8
	internal static void PoolerTrace(string fmtPrintfW, int a1, int a2, int a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Pooling) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x060010C9 RID: 4297 RVA: 0x0021A240 File Offset: 0x00219640
	internal static void PoolerScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Pooling) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0021A290 File Offset: 0x00219690
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x0021A2E0 File Offset: 0x002196E0
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, string fmtPrintfW2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, fmtPrintfW2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x0021A330 File Offset: 0x00219730
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x0021A380 File Offset: 0x00219780
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, string fmtPrintfW2, string fmtPrintfW3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, fmtPrintfW2, fmtPrintfW3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x0021A3D0 File Offset: 0x002197D0
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string fmtPrintfW2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, fmtPrintfW2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x0021A420 File Offset: 0x00219820
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x0021A470 File Offset: 0x00219870
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, string fmtPrintfW2, string fmtPrintfW3, string fmtPrintfW4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, fmtPrintfW2, fmtPrintfW3, fmtPrintfW4);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x0021A4C4 File Offset: 0x002198C4
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string fmtPrintfW2, string fmtPrintfW3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, fmtPrintfW2, fmtPrintfW3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x0021A518 File Offset: 0x00219918
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string fmtPrintfW2, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, fmtPrintfW2, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x0021A56C File Offset: 0x0021996C
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2, string fmtPrintfW2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, fmtPrintfW2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0021A5C0 File Offset: 0x002199C0
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string fmtPrintfW2, string fmtPrintfW3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, fmtPrintfW2, fmtPrintfW3, a4);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x0021A614 File Offset: 0x00219A14
	internal static void NotificationsScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string fmtPrintfW2, string fmtPrintfW3, string fmtPrintfW4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, fmtPrintfW2, fmtPrintfW3, fmtPrintfW4, a5);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x0021A66C File Offset: 0x00219A6C
	internal static void NotificationsTrace(string fmtPrintfW)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW);
		}
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x0021A6AC File Offset: 0x00219AAC
	internal static void NotificationsTrace(string fmtPrintfW, string fmtPrintfW2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, fmtPrintfW2);
		}
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x0021A6F0 File Offset: 0x00219AF0
	internal static void NotificationsTrace(string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x0021A734 File Offset: 0x00219B34
	internal static void NotificationsTrace(string fmtPrintfW, bool a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x0021A778 File Offset: 0x00219B78
	internal static void NotificationsTrace(string fmtPrintfW, string fmtPrintfW2, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, fmtPrintfW2, a1);
		}
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x0021A7BC File Offset: 0x00219BBC
	internal static void NotificationsTrace(string fmtPrintfW, int a1, string fmtPrintfW2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, fmtPrintfW2);
		}
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x0021A800 File Offset: 0x00219C00
	internal static void NotificationsTrace(string fmtPrintfW, bool a1, string fmtPrintfW2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, fmtPrintfW2);
		}
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x0021A844 File Offset: 0x00219C44
	internal static void NotificationsTrace(string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0021A888 File Offset: 0x00219C88
	internal static void NotificationsTrace(string fmtPrintfW, bool a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x0021A8CC File Offset: 0x00219CCC
	internal static void NotificationsTrace(string fmtPrintfW, int a1, bool a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x0021A910 File Offset: 0x00219D10
	internal static void NotificationsTrace(string fmtPrintfW, string fmtPrintfW2, string fmtPrintfW3, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, fmtPrintfW2, fmtPrintfW3, (long)a1);
		}
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x0021A954 File Offset: 0x00219D54
	internal static void NotificationsTrace(string fmtPrintfW, bool a1, string fmtPrintfW2, string fmtPrintfW3, string fmtPrintfW4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, fmtPrintfW2, fmtPrintfW3, fmtPrintfW4);
		}
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x0021A99C File Offset: 0x00219D9C
	internal static void NotificationsTrace(string fmtPrintfW, int a1, string fmtPrintfW2, string fmtPrintfW3, string fmtPrintfW4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Dependency) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, fmtPrintfW2, fmtPrintfW3, fmtPrintfW4);
		}
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x0021A9E4 File Offset: 0x00219DE4
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void TraceSqlReturn(string fmtPrintfW, ODBC32.RetCode a1)
	{
		if ((a1 != ODBC32.RetCode.SUCCESS || (Bid.modFlags & Bid.ApiGroup.StatusOk) != Bid.ApiGroup.Off) && (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1);
		}
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0021AA30 File Offset: 0x00219E30
	internal static void TraceSqlReturn(string fmtPrintfW, ODBC32.RetCode a1, string a2)
	{
		if ((a1 != ODBC32.RetCode.SUCCESS || (Bid.modFlags & Bid.ApiGroup.StatusOk) != Bid.ApiGroup.Off) && (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1, a2);
		}
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x0021AA7C File Offset: 0x00219E7C
	internal static void TraceSqlReturn(string fmtPrintfW, ODBC32.RetCode a1, string a2, string a3)
	{
		if ((a1 != ODBC32.RetCode.SUCCESS || (Bid.modFlags & Bid.ApiGroup.StatusOk) != Bid.ApiGroup.Off) && (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1, a2, a3);
		}
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x0021AAC8 File Offset: 0x00219EC8
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, OleDbHResult a1)
	{
		if ((a1 != OleDbHResult.S_OK || (Bid.modFlags & Bid.ApiGroup.StatusOk) != Bid.ApiGroup.Off) && (Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1);
		}
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x0021AB14 File Offset: 0x00219F14
	internal static void Trace(string fmtPrintfW, OleDbHResult a1, string a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1, a2);
		}
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x0021AB54 File Offset: 0x00219F54
	internal static void Trace(string fmtPrintfW, OleDbHResult a1, IntPtr a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1, a2);
		}
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x0021AB94 File Offset: 0x00219F94
	internal static void Trace(string fmtPrintfW, OleDbHResult a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, (int)a1, a2);
		}
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x0021ABD4 File Offset: 0x00219FD4
	internal static void Trace(string fmtPrintfW, string a1, string a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x0021AC14 File Offset: 0x0021A014
	internal static void Trace(string fmtPrintfW, int a1, string a2, bool a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x0021AC54 File Offset: 0x0021A054
	internal static void Trace(string fmtPrintfW, int a1, int a2, string a3, string a4, int a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5);
		}
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x0021AC98 File Offset: 0x0021A098
	internal static void Trace(string fmtPrintfW, int a1, int a2, long a3, uint a4, int a5, uint a6, uint a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7);
		}
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x0021ACE0 File Offset: 0x0021A0E0
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, Guid a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x0021AD2C File Offset: 0x0021A12C
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010F0 RID: 4336 RVA: 0x0021AD7C File Offset: 0x0021A17C
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, bool a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x0021ADCC File Offset: 0x0021A1CC
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, int a1, string a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x0021AE0C File Offset: 0x0021A20C
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static void Trace(string fmtPrintfW, IntPtr a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x0021AE4C File Offset: 0x0021A24C
	internal static void Trace(string fmtPrintfW, int a1)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1);
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x0021AE8C File Offset: 0x0021A28C
	internal static void Trace(string fmtPrintfW, int a1, int a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x0021AECC File Offset: 0x0021A2CC
	internal static void Trace(string fmtPrintfW, int a1, IntPtr a2, IntPtr a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x0021AF0C File Offset: 0x0021A30C
	internal static void Trace(string fmtPrintfW, int a1, IntPtr a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010F7 RID: 4343 RVA: 0x0021AF4C File Offset: 0x0021A34C
	internal static void Trace(string fmtPrintfW, int a1, string a2, string a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010F8 RID: 4344 RVA: 0x0021AF8C File Offset: 0x0021A38C
	internal static void Trace(string fmtPrintfW, int a1, string a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x0021AFCC File Offset: 0x0021A3CC
	internal static void Trace(string fmtPrintfW, int a1, string a2, string a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x060010FA RID: 4346 RVA: 0x0021B010 File Offset: 0x0021A410
	internal static void Trace(string fmtPrintfW, int a1, int a2, int a3, string a4, string a5, int a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6);
		}
	}

	// Token: 0x060010FB RID: 4347 RVA: 0x0021B058 File Offset: 0x0021A458
	internal static void Trace(string fmtPrintfW, int a1, int a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x0021B098 File Offset: 0x0021A498
	internal static void Trace(string fmtPrintfW, int a1, bool a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x060010FD RID: 4349 RVA: 0x0021B0D8 File Offset: 0x0021A4D8
	internal static void Trace(string fmtPrintfW, int a1, int a2, int a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x060010FE RID: 4350 RVA: 0x0021B11C File Offset: 0x0021A51C
	internal static void Trace(string fmtPrintfW, int a1, int a2, bool a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x060010FF RID: 4351 RVA: 0x0021B15C File Offset: 0x0021A55C
	internal static void Trace(string fmtPrintfW, int a1, int a2, int a3, int a4, int a5, int a6, int a7)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7);
		}
	}

	// Token: 0x06001100 RID: 4352 RVA: 0x0021B1A4 File Offset: 0x0021A5A4
	internal static void Trace(string fmtPrintfW, int a1, string a2, int a3, int a4, bool a5)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5);
		}
	}

	// Token: 0x06001101 RID: 4353 RVA: 0x0021B1E8 File Offset: 0x0021A5E8
	internal static void Trace(string fmtPrintfW, int a1, long a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2);
		}
	}

	// Token: 0x06001102 RID: 4354 RVA: 0x0021B228 File Offset: 0x0021A628
	internal static void Trace(string fmtPrintfW, int a1, int a2, long a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3);
		}
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x0021B268 File Offset: 0x0021A668
	internal static void Trace(string fmtPrintfW, int a1, string a2, string a3, string a4, int a5, long a6)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6);
		}
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x0021B2B0 File Offset: 0x0021A6B0
	internal static void Trace(string fmtPrintfW, int a1, long a2, int a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x0021B2F4 File Offset: 0x0021A6F4
	internal static void Trace(string fmtPrintfW, int a1, int a2, long a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x0021B338 File Offset: 0x0021A738
	internal static void Trace(string fmtPrintfW, int a1, int a2, int a3, int a4, string a5, string a6, string a7, int a8)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4, a5, a6, a7, a8);
		}
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x0021B384 File Offset: 0x0021A784
	internal static void Trace(string fmtPrintfW, int a1, int a2, string a3, string a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Trace) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.Trace(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, fmtPrintfW, a1, a2, a3, a4);
		}
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0021B3C8 File Offset: 0x0021A7C8
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x0021B414 File Offset: 0x0021A814
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, bool a2)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x0021B460 File Offset: 0x0021A860
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2, string a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x0021B4B0 File Offset: 0x0021A8B0
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, string a2, bool a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x0021B500 File Offset: 0x0021A900
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2, bool a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x0021B550 File Offset: 0x0021A950
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2, int a3, string a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3, a4);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x0021B5A0 File Offset: 0x0021A9A0
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2, int a3)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0021B5F0 File Offset: 0x0021A9F0
	internal static void ScopeEnter(out IntPtr hScp, string fmtPrintfW, int a1, int a2, bool a3, int a4)
	{
		if ((Bid.modFlags & Bid.ApiGroup.Scope) != Bid.ApiGroup.Off && Bid.modID != Bid.NoData)
		{
			Bid.NativeMethods.ScopeEnter(Bid.modID, UIntPtr.Zero, UIntPtr.Zero, out hScp, fmtPrintfW, a1, a2, a3, a4);
			return;
		}
		hScp = Bid.NoData;
	}

	// Token: 0x04000B21 RID: 2849
	private const int BidVer = 9210;

	// Token: 0x04000B22 RID: 2850
	private const uint configFlags = 3489660928U;

	// Token: 0x04000B23 RID: 2851
	private const string dllName = "System.Data.dll";

	// Token: 0x04000B24 RID: 2852
	private static IntPtr __noData;

	// Token: 0x04000B25 RID: 2853
	private static object _setBitsLock = new object();

	// Token: 0x04000B26 RID: 2854
	private static IntPtr modID = Bid.internalInitialize();

	// Token: 0x04000B27 RID: 2855
	private static Bid.ApiGroup modFlags;

	// Token: 0x04000B28 RID: 2856
	private static string modIdentity;

	// Token: 0x04000B29 RID: 2857
	private static Bid.CtrlCB ctrlCallback;

	// Token: 0x04000B2A RID: 2858
	private static Bid.BindingCookie cookieObject;

	// Token: 0x04000B2B RID: 2859
	private static GCHandle hCookie;

	// Token: 0x04000B2C RID: 2860
	private static IntPtr __defaultCmdSpace;

	// Token: 0x04000B2D RID: 2861
	private static Bid.AutoInit ai;

	// Token: 0x0200010A RID: 266
	internal enum ApiGroup : uint
	{
		// Token: 0x04000B2F RID: 2863
		Off,
		// Token: 0x04000B30 RID: 2864
		Default,
		// Token: 0x04000B31 RID: 2865
		Trace,
		// Token: 0x04000B32 RID: 2866
		Scope = 4U,
		// Token: 0x04000B33 RID: 2867
		Perf = 8U,
		// Token: 0x04000B34 RID: 2868
		Resource = 16U,
		// Token: 0x04000B35 RID: 2869
		Memory = 32U,
		// Token: 0x04000B36 RID: 2870
		StatusOk = 64U,
		// Token: 0x04000B37 RID: 2871
		Advanced = 128U,
		// Token: 0x04000B38 RID: 2872
		Pooling = 4096U,
		// Token: 0x04000B39 RID: 2873
		Dependency = 8192U,
		// Token: 0x04000B3A RID: 2874
		StateDump = 16384U,
		// Token: 0x04000B3B RID: 2875
		MaskBid = 4095U,
		// Token: 0x04000B3C RID: 2876
		MaskUser = 4294963200U,
		// Token: 0x04000B3D RID: 2877
		MaskAll = 4294967295U
	}

	// Token: 0x0200010B RID: 267
	// (Invoke) Token: 0x06001112 RID: 4370
	private delegate Bid.ApiGroup CtrlCB(Bid.ApiGroup mask, Bid.ApiGroup bits);

	// Token: 0x0200010C RID: 268
	[StructLayout(LayoutKind.Sequential)]
	private class BindingCookie
	{
		// Token: 0x06001115 RID: 4373 RVA: 0x0021B664 File Offset: 0x0021AA64
		internal BindingCookie()
		{
			this._data = (IntPtr)(-1);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x0021B684 File Offset: 0x0021AA84
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Invalidate()
		{
			this._data = (IntPtr)(-1);
		}

		// Token: 0x04000B3E RID: 2878
		internal IntPtr _data;
	}

	// Token: 0x0200010D RID: 269
	private enum CtlCmd : uint
	{
		// Token: 0x04000B40 RID: 2880
		Reverse = 1U,
		// Token: 0x04000B41 RID: 2881
		Unicode,
		// Token: 0x04000B42 RID: 2882
		DcsBase = 1073741824U,
		// Token: 0x04000B43 RID: 2883
		DcsMax = 1610612732U,
		// Token: 0x04000B44 RID: 2884
		CplBase = 1610612736U,
		// Token: 0x04000B45 RID: 2885
		CplMax = 2147483644U,
		// Token: 0x04000B46 RID: 2886
		CmdSpaceCount = 1073741824U,
		// Token: 0x04000B47 RID: 2887
		CmdSpaceEnum = 1073741828U,
		// Token: 0x04000B48 RID: 2888
		CmdSpaceQuery = 1073741832U,
		// Token: 0x04000B49 RID: 2889
		GetEventID = 1073741846U,
		// Token: 0x04000B4A RID: 2890
		ParseString = 1073741850U,
		// Token: 0x04000B4B RID: 2891
		AddExtension = 1073741854U,
		// Token: 0x04000B4C RID: 2892
		AddMetaText = 1073741858U,
		// Token: 0x04000B4D RID: 2893
		AddResHandle = 1073741862U,
		// Token: 0x04000B4E RID: 2894
		Shutdown = 1073741866U,
		// Token: 0x04000B4F RID: 2895
		LastItem
	}

	// Token: 0x0200010E RID: 270
	private struct BIDEXTINFO
	{
		// Token: 0x06001117 RID: 4375 RVA: 0x0021B6A0 File Offset: 0x0021AAA0
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

		// Token: 0x04000B50 RID: 2896
		private IntPtr hModule;

		// Token: 0x04000B51 RID: 2897
		[MarshalAs(UnmanagedType.LPWStr)]
		private string DomainName;

		// Token: 0x04000B52 RID: 2898
		private int Reserved2;

		// Token: 0x04000B53 RID: 2899
		private int Reserved;

		// Token: 0x04000B54 RID: 2900
		[MarshalAs(UnmanagedType.LPWStr)]
		private string ModulePath;

		// Token: 0x04000B55 RID: 2901
		private IntPtr ModulePathA;

		// Token: 0x04000B56 RID: 2902
		private IntPtr pBindCookie;
	}

	// Token: 0x0200010F RID: 271
	private sealed class AutoInit : SafeHandle
	{
		// Token: 0x06001118 RID: 4376 RVA: 0x0021B6E4 File Offset: 0x0021AAE4
		internal AutoInit()
			: base(IntPtr.Zero, true)
		{
			Bid.initEntryPoint();
			this._bInitialized = true;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0021B70C File Offset: 0x0021AB0C
		protected override bool ReleaseHandle()
		{
			this._bInitialized = false;
			Bid.doneEntryPoint();
			return true;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600111A RID: 4378 RVA: 0x0021B728 File Offset: 0x0021AB28
		public override bool IsInvalid
		{
			get
			{
				return !this._bInitialized;
			}
		}

		// Token: 0x04000B57 RID: 2903
		private bool _bInitialized;
	}

	// Token: 0x02000110 RID: 272
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	private static class NativeMethods
	{
		// Token: 0x0600111B RID: 4379
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "DllBidPutStrW")]
		internal static extern void PutStr(IntPtr hID, UIntPtr src, UIntPtr info, string str);

		// Token: 0x0600111C RID: 4380
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string strConst);

		// Token: 0x0600111D RID: 4381
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, string a1);

		// Token: 0x0600111E RID: 4382
		[DllImport("System.Data.dll", EntryPoint = "DllBidScopeLeave")]
		internal static extern void ScopeLeave(IntPtr hID, UIntPtr src, UIntPtr info, ref IntPtr hScp);

		// Token: 0x0600111F RID: 4383
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string strConst);

		// Token: 0x06001120 RID: 4384
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, string a1);

		// Token: 0x06001121 RID: 4385
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1);

		// Token: 0x06001122 RID: 4386
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, IntPtr a1);

		// Token: 0x06001123 RID: 4387
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2);

		// Token: 0x06001124 RID: 4388
		[DllImport("System.Data.dll", CharSet = CharSet.Unicode, EntryPoint = "DllBidEnabledW")]
		internal static extern bool Enabled(IntPtr hID, UIntPtr src, UIntPtr info, string tcs);

		// Token: 0x06001125 RID: 4389
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void TraceBin(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, byte[] buff, ushort len);

		// Token: 0x06001126 RID: 4390
		[DllImport("System.Data.dll", CharSet = CharSet.Unicode, EntryPoint = "DllBidCtlProc")]
		internal static extern void AddMetaText(IntPtr hID, IntPtr cmdSpace, Bid.CtlCmd cmd, IntPtr nop1, string txtID, IntPtr nop2);

		// Token: 0x06001127 RID: 4391
		[DllImport("System.Data.dll", BestFitMapping = false, CharSet = CharSet.Ansi, EntryPoint = "DllBidCtlProc")]
		internal static extern IntPtr GetCmdSpaceID(IntPtr hID, IntPtr cmdSpace, Bid.CtlCmd cmd, uint noOp, string txtID, IntPtr NoOp2);

		// Token: 0x06001128 RID: 4392
		[DllImport("System.Data.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
		internal static extern void DllBidEntryPoint(ref IntPtr hID, int bInitAndVer, string sIdentity, uint propBits, ref Bid.ApiGroup pGblFlags, Bid.CtrlCB fAddr, ref Bid.BIDEXTINFO pExtInfo, IntPtr pHooks, IntPtr pHdr);

		// Token: 0x06001129 RID: 4393
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.dll")]
		internal static extern void DllBidEntryPoint(ref IntPtr hID, int bInitAndVer, IntPtr unused1, uint propBits, ref Bid.ApiGroup pGblFlags, IntPtr unused2, IntPtr unused3, IntPtr unused4, IntPtr unused5);

		// Token: 0x0600112A RID: 4394
		[DllImport("System.Data.dll")]
		internal static extern void DllBidInitialize();

		// Token: 0x0600112B RID: 4395
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.dll")]
		internal static extern void DllBidFinalize();

		// Token: 0x0600112C RID: 4396
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, string a3, string a4, int a5);

		// Token: 0x0600112D RID: 4397
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, bool a3);

		// Token: 0x0600112E RID: 4398
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, long a3, uint a4, int a5, uint a6, uint a7);

		// Token: 0x0600112F RID: 4399
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, string a1, string a2);

		// Token: 0x06001130 RID: 4400
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid a2);

		// Token: 0x06001131 RID: 4401
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, string a1, string a2);

		// Token: 0x06001132 RID: 4402
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2, int a3);

		// Token: 0x06001133 RID: 4403
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, bool a2, int a3);

		// Token: 0x06001134 RID: 4404
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2, string a3);

		// Token: 0x06001135 RID: 4405
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, string a1, string a2, string a3);

		// Token: 0x06001136 RID: 4406
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2, string a3, int a4);

		// Token: 0x06001137 RID: 4407
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2, string a3, string a4, int a5);

		// Token: 0x06001138 RID: 4408
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, IntPtr a1);

		// Token: 0x06001139 RID: 4409
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1);

		// Token: 0x0600113A RID: 4410
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, bool a1);

		// Token: 0x0600113B RID: 4411
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, string fmtPrintfW2, int a1);

		// Token: 0x0600113C RID: 4412
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, bool a1, string fmtPrintfW2);

		// Token: 0x0600113D RID: 4413
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, bool a1, int a2);

		// Token: 0x0600113E RID: 4414
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2);

		// Token: 0x0600113F RID: 4415
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2);

		// Token: 0x06001140 RID: 4416
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, IntPtr a2, IntPtr a3);

		// Token: 0x06001141 RID: 4417
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, IntPtr a2);

		// Token: 0x06001142 RID: 4418
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, string a3);

		// Token: 0x06001143 RID: 4419
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, int a3);

		// Token: 0x06001144 RID: 4420
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, string a3, int a4);

		// Token: 0x06001145 RID: 4421
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, int a3, string a4, string a5, int a6);

		// Token: 0x06001146 RID: 4422
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, int a3);

		// Token: 0x06001147 RID: 4423
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, bool a2);

		// Token: 0x06001148 RID: 4424
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, string a3, string a4);

		// Token: 0x06001149 RID: 4425
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, bool a1, string a2, string a3, string a4);

		// Token: 0x0600114A RID: 4426
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, int a3, int a4);

		// Token: 0x0600114B RID: 4427
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, bool a3);

		// Token: 0x0600114C RID: 4428
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, int a3, int a4, int a5, int a6, int a7);

		// Token: 0x0600114D RID: 4429
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, int a3, int a4, bool a5);

		// Token: 0x0600114E RID: 4430
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, long a2);

		// Token: 0x0600114F RID: 4431
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, long a3);

		// Token: 0x06001150 RID: 4432
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW1, string fmtPrintfW2, string fmtPrintfW3, long a4);

		// Token: 0x06001151 RID: 4433
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, string a2, string a3, string a4, int a5, long a6);

		// Token: 0x06001152 RID: 4434
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, long a2, int a3, int a4);

		// Token: 0x06001153 RID: 4435
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, long a3, int a4);

		// Token: 0x06001154 RID: 4436
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, int a3, int a4, string a5, string a6, string a7, int a8);

		// Token: 0x06001155 RID: 4437
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidTraceCW")]
		internal static extern void Trace(IntPtr hID, UIntPtr src, UIntPtr info, string fmtPrintfW, int a1, int a2, string a3, string a4);

		// Token: 0x06001156 RID: 4438
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2);

		// Token: 0x06001157 RID: 4439
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, bool a2);

		// Token: 0x06001158 RID: 4440
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2, string a3);

		// Token: 0x06001159 RID: 4441
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, string a2, bool a3);

		// Token: 0x0600115A RID: 4442
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2, bool a3);

		// Token: 0x0600115B RID: 4443
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2, int a3, string a4);

		// Token: 0x0600115C RID: 4444
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2, int a3);

		// Token: 0x0600115D RID: 4445
		[DllImport("System.Data.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "DllBidScopeEnterCW")]
		internal static extern void ScopeEnter(IntPtr hID, UIntPtr src, UIntPtr info, out IntPtr hScp, string fmtPrintfW, int a1, int a2, bool a3, int a4);
	}
}
