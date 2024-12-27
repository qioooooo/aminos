using System;
using System.Data.OracleClient;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Transactions;

namespace System.Data.Common
{
	// Token: 0x02000083 RID: 131
	[SuppressUnmanagedCodeSecurity]
	internal sealed class UnsafeNativeMethods
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00071684 File Offset: 0x00070A84
		private UnsafeNativeMethods()
		{
		}

		// Token: 0x0600072B RID: 1835
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CheckTokenMembership(IntPtr tokenHandle, byte[] sidToCheck, out bool isMember);

		// Token: 0x0600072C RID: 1836
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ConvertSidToStringSidW(IntPtr sid, out IntPtr stringSid);

		// Token: 0x0600072D RID: 1837
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int CreateWellKnownSid(int sidType, byte[] domainSid, [Out] byte[] resultSid, ref uint resultSidLength);

		// Token: 0x0600072E RID: 1838
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetTokenInformation(IntPtr tokenHandle, uint token_class, IntPtr tokenStruct, uint tokenInformationLength, ref uint tokenString);

		// Token: 0x0600072F RID: 1839
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IsTokenRestricted(IntPtr tokenHandle);

		// Token: 0x06000730 RID: 1840
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		internal static extern int lstrlenA(IntPtr ptr);

		// Token: 0x06000731 RID: 1841
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int lstrlenW(IntPtr ptr);

		// Token: 0x06000732 RID: 1842
		[DllImport("kernel32.dll")]
		internal static extern void SetLastError(int dwErrCode);

		// Token: 0x06000733 RID: 1843
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oramts.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OraMTSEnlCtxGet([MarshalAs(UnmanagedType.LPArray)] [In] byte[] lpUname, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] lpPsswd, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] lpDbnam, OciHandle pOCISvc, OciHandle pOCIErr, uint dwFlags, out IntPtr pCtxt);

		// Token: 0x06000734 RID: 1844
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oramts.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OraMTSEnlCtxRel(IntPtr pCtxt);

		// Token: 0x06000735 RID: 1845
		[DllImport("oramts.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OraMTSOCIErrGet(ref int dwErr, NativeBuffer lpcEMsg, ref int lpdLen);

		// Token: 0x06000736 RID: 1846
		[DllImport("oramts.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OraMTSJoinTxn(OciEnlistContext pCtxt, IDtcTransaction pTrans);

		// Token: 0x06000737 RID: 1847
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int oermsg(short rcode, NativeBuffer buf);

		// Token: 0x06000738 RID: 1848
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIAttrGet(OciHandle trgthndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE trghndltyp, OciHandle attributep, out uint sizep, [MarshalAs(UnmanagedType.U4)] [In] OCI.ATTR attrtype, OciHandle errhp);

		// Token: 0x06000739 RID: 1849
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIAttrGet(OciHandle trgthndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE trghndltyp, out int attributep, out uint sizep, [MarshalAs(UnmanagedType.U4)] [In] OCI.ATTR attrtype, OciHandle errhp);

		// Token: 0x0600073A RID: 1850
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIAttrGet(OciHandle trgthndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE trghndltyp, ref IntPtr attributep, ref uint sizep, [MarshalAs(UnmanagedType.U4)] [In] OCI.ATTR attrtype, OciHandle errhp);

		// Token: 0x0600073B RID: 1851
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIAttrSet(OciHandle trgthndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE trghndltyp, OciHandle attributep, uint size, [MarshalAs(UnmanagedType.U4)] [In] OCI.ATTR attrtype, OciHandle errhp);

		// Token: 0x0600073C RID: 1852
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIAttrSet(OciHandle trgthndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE trghndltyp, ref int attributep, uint size, [MarshalAs(UnmanagedType.U4)] [In] OCI.ATTR attrtype, OciHandle errhp);

		// Token: 0x0600073D RID: 1853
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIAttrSet(OciHandle trgthndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE trghndltyp, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] attributep, uint size, [MarshalAs(UnmanagedType.U4)] [In] OCI.ATTR attrtype, OciHandle errhp);

		// Token: 0x0600073E RID: 1854
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIBindByName(OciHandle stmtp, out IntPtr bindpp, OciHandle errhp, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] placeholder, int placeh_len, IntPtr valuep, int value_sz, [MarshalAs(UnmanagedType.U2)] [In] OCI.DATATYPE dty, IntPtr indp, IntPtr alenp, IntPtr rcodep, uint maxarr_len, IntPtr curelap, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x0600073F RID: 1855
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCICharSetToUnicode(OciHandle hndl, IntPtr dst, uint dstsz, IntPtr src, uint srcsz, out uint size);

		// Token: 0x06000740 RID: 1856
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDateTimeFromArray(OciHandle hndl, OciHandle err, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] inarray, uint len, byte type, OciHandle datetime, OciHandle reftz, byte fsprec);

		// Token: 0x06000741 RID: 1857
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDateTimeToArray(OciHandle hndl, OciHandle err, OciHandle datetime, OciHandle reftz, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] outarray, ref uint len, byte fsprec);

		// Token: 0x06000742 RID: 1858
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDateTimeGetTimeZoneOffset(OciHandle hndl, OciHandle err, OciHandle datetime, out sbyte hour, out sbyte min);

		// Token: 0x06000743 RID: 1859
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDefineArrayOfStruct(OciHandle defnp, OciHandle errhp, uint pvskip, uint indskip, uint rlskip, uint rcskip);

		// Token: 0x06000744 RID: 1860
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDefineByPos(OciHandle stmtp, out IntPtr hndlpp, OciHandle errhp, uint position, IntPtr valuep, int value_sz, [MarshalAs(UnmanagedType.U2)] [In] OCI.DATATYPE dty, IntPtr indp, IntPtr alenp, IntPtr rcodep, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x06000745 RID: 1861
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDefineDynamic(OciHandle defnp, OciHandle errhp, IntPtr octxp, OCI.Callback.OCICallbackDefine ocbfp);

		// Token: 0x06000746 RID: 1862
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDescriptorAlloc(OciHandle parenth, out IntPtr descp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE type, uint xtramem_sz, IntPtr usrmempp);

		// Token: 0x06000747 RID: 1863
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIDescriptorFree(IntPtr hndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE type);

		// Token: 0x06000748 RID: 1864
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIEnvCreate(out IntPtr envhpp, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode, IntPtr ctxp, IntPtr malocfp, IntPtr ralocfp, IntPtr mfreefp, uint xtramemsz, IntPtr usrmempp);

		// Token: 0x06000749 RID: 1865
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIEnvNlsCreate(out IntPtr envhpp, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode, IntPtr ctxp, IntPtr malocfp, IntPtr ralocfp, IntPtr mfreefp, uint xtramemsz, IntPtr usrmempp, ushort charset, ushort ncharset);

		// Token: 0x0600074A RID: 1866
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIErrorGet(OciHandle hndlp, uint recordno, IntPtr sqlstate, out int errcodep, NativeBuffer bufp, uint bufsiz, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE type);

		// Token: 0x0600074B RID: 1867
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIHandleAlloc(OciHandle parenth, out IntPtr hndlpp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE type, uint xtramemsz, IntPtr usrmempp);

		// Token: 0x0600074C RID: 1868
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIHandleFree(IntPtr hndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE type);

		// Token: 0x0600074D RID: 1869
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobAppend(OciHandle svchp, OciHandle errhp, OciHandle dst_locp, OciHandle src_locp);

		// Token: 0x0600074E RID: 1870
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobClose(OciHandle svchp, OciHandle errhp, OciHandle locp);

		// Token: 0x0600074F RID: 1871
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobCopy(OciHandle svchp, OciHandle errhp, OciHandle dst_locp, OciHandle src_locp, uint amount, uint dst_offset, uint src_offset);

		// Token: 0x06000750 RID: 1872
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobCopy2(IntPtr svchp, IntPtr errhp, IntPtr dst_locp, IntPtr src_locp, ulong amount, ulong dst_offset, ulong src_offset);

		// Token: 0x06000751 RID: 1873
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobCreateTemporary(OciHandle svchp, OciHandle errhp, OciHandle locp, ushort csid, [MarshalAs(UnmanagedType.U1)] [In] OCI.CHARSETFORM csfrm, [MarshalAs(UnmanagedType.U1)] [In] OCI.LOB_TYPE lobtype, int cache, [MarshalAs(UnmanagedType.U2)] [In] OCI.DURATION duration);

		// Token: 0x06000752 RID: 1874
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobErase(OciHandle svchp, OciHandle errhp, OciHandle locp, ref uint amount, uint offset);

		// Token: 0x06000753 RID: 1875
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobFileExists(OciHandle svchp, OciHandle errhp, OciHandle locp, out int flag);

		// Token: 0x06000754 RID: 1876
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobFileGetName(OciHandle envhp, OciHandle errhp, OciHandle filep, IntPtr dir_alias, ref ushort d_length, IntPtr filename, ref ushort f_length);

		// Token: 0x06000755 RID: 1877
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobFileSetName(OciHandle envhp, OciHandle errhp, ref IntPtr filep, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] dir_alias, ushort d_length, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] filename, ushort f_length);

		// Token: 0x06000756 RID: 1878
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobFreeTemporary(OciHandle svchp, OciHandle errhp, OciHandle locp);

		// Token: 0x06000757 RID: 1879
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobGetChunkSize(OciHandle svchp, OciHandle errhp, OciHandle locp, out uint lenp);

		// Token: 0x06000758 RID: 1880
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobGetLength(OciHandle svchp, OciHandle errhp, OciHandle locp, out uint lenp);

		// Token: 0x06000759 RID: 1881
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobIsOpen(OciHandle svchp, OciHandle errhp, OciHandle locp, out int flag);

		// Token: 0x0600075A RID: 1882
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobIsTemporary(OciHandle envhp, OciHandle errhp, OciHandle locp, out int flag);

		// Token: 0x0600075B RID: 1883
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobLoadFromFile(OciHandle svchp, OciHandle errhp, OciHandle dst_locp, OciHandle src_locp, uint amount, uint dst_offset, uint src_offset);

		// Token: 0x0600075C RID: 1884
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobOpen(OciHandle svchp, OciHandle errhp, OciHandle locp, byte mode);

		// Token: 0x0600075D RID: 1885
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobRead(OciHandle svchp, OciHandle errhp, OciHandle locp, ref uint amtp, uint offset, IntPtr bufp, uint bufl, IntPtr ctxp, IntPtr cbfp, ushort csid, [MarshalAs(UnmanagedType.U1)] [In] OCI.CHARSETFORM csfrm);

		// Token: 0x0600075E RID: 1886
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobTrim(OciHandle svchp, OciHandle errhp, OciHandle locp, uint newlen);

		// Token: 0x0600075F RID: 1887
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCILobWrite(OciHandle svchp, OciHandle errhp, OciHandle locp, ref uint amtp, uint offset, IntPtr bufp, uint buflen, byte piece, IntPtr ctxp, IntPtr cbfp, ushort csid, [MarshalAs(UnmanagedType.U1)] [In] OCI.CHARSETFORM csfrm);

		// Token: 0x06000760 RID: 1888
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberAbs(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000761 RID: 1889
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberAdd(OciHandle err, byte[] number1, byte[] number2, byte[] result);

		// Token: 0x06000762 RID: 1890
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberArcCos(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000763 RID: 1891
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberArcSin(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000764 RID: 1892
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberArcTan(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000765 RID: 1893
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberArcTan2(OciHandle err, byte[] number1, byte[] number2, byte[] result);

		// Token: 0x06000766 RID: 1894
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberCeil(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000767 RID: 1895
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberCmp(OciHandle err, byte[] number1, byte[] number2, out int result);

		// Token: 0x06000768 RID: 1896
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberCos(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000769 RID: 1897
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberDiv(OciHandle err, byte[] number1, byte[] number2, byte[] result);

		// Token: 0x0600076A RID: 1898
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberExp(OciHandle err, byte[] p, byte[] result);

		// Token: 0x0600076B RID: 1899
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberFloor(OciHandle err, byte[] number, byte[] result);

		// Token: 0x0600076C RID: 1900
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberFromInt(OciHandle err, ref int inum, uint inum_length, OCI.SIGN inum_s_flag, byte[] number);

		// Token: 0x0600076D RID: 1901
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberFromInt(OciHandle err, ref uint inum, uint inum_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN inum_s_flag, byte[] number);

		// Token: 0x0600076E RID: 1902
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberFromInt(OciHandle err, ref long inum, uint inum_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN inum_s_flag, byte[] number);

		// Token: 0x0600076F RID: 1903
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberFromInt(OciHandle err, ref ulong inum, uint inum_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN inum_s_flag, byte[] number);

		// Token: 0x06000770 RID: 1904
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberFromReal(OciHandle err, ref double rnum, uint rnum_length, byte[] number);

		// Token: 0x06000771 RID: 1905
		[DllImport("oci.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern int OCINumberFromText(OciHandle err, [MarshalAs(UnmanagedType.LPStr)] [In] string str, uint str_length, [MarshalAs(UnmanagedType.LPStr)] [In] string fmt, uint fmt_length, IntPtr nls_params, uint nls_p_length, byte[] number);

		// Token: 0x06000772 RID: 1906
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberHypCos(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000773 RID: 1907
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberHypSin(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000774 RID: 1908
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberHypTan(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000775 RID: 1909
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberIntPower(OciHandle err, byte[] baseNumber, int exponent, byte[] result);

		// Token: 0x06000776 RID: 1910
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberIsInt(OciHandle err, byte[] number, out int result);

		// Token: 0x06000777 RID: 1911
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberLn(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000778 RID: 1912
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberLog(OciHandle err, byte[] b, byte[] number, byte[] result);

		// Token: 0x06000779 RID: 1913
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberMod(OciHandle err, byte[] number1, byte[] number2, byte[] result);

		// Token: 0x0600077A RID: 1914
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberMul(OciHandle err, byte[] number1, byte[] number2, byte[] result);

		// Token: 0x0600077B RID: 1915
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberNeg(OciHandle err, byte[] number, byte[] result);

		// Token: 0x0600077C RID: 1916
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberPower(OciHandle err, byte[] baseNumber, byte[] exponent, byte[] result);

		// Token: 0x0600077D RID: 1917
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberRound(OciHandle err, byte[] number, int decplace, byte[] result);

		// Token: 0x0600077E RID: 1918
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberShift(OciHandle err, byte[] baseNumber, int nDig, byte[] result);

		// Token: 0x0600077F RID: 1919
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberSign(OciHandle err, byte[] number, out int result);

		// Token: 0x06000780 RID: 1920
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberSin(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000781 RID: 1921
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberSqrt(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000782 RID: 1922
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberSub(OciHandle err, byte[] number1, byte[] number2, byte[] result);

		// Token: 0x06000783 RID: 1923
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberTan(OciHandle err, byte[] number, byte[] result);

		// Token: 0x06000784 RID: 1924
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberToInt(OciHandle err, byte[] number, uint rsl_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN rsl_flag, out int rsl);

		// Token: 0x06000785 RID: 1925
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberToInt(OciHandle err, byte[] number, uint rsl_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN rsl_flag, out uint rsl);

		// Token: 0x06000786 RID: 1926
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberToInt(OciHandle err, byte[] number, uint rsl_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN rsl_flag, out long rsl);

		// Token: 0x06000787 RID: 1927
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberToInt(OciHandle err, byte[] number, uint rsl_length, [MarshalAs(UnmanagedType.U4)] [In] OCI.SIGN rsl_flag, out ulong rsl);

		// Token: 0x06000788 RID: 1928
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberToReal(OciHandle err, byte[] number, uint rsl_length, out double rsl);

		// Token: 0x06000789 RID: 1929
		[DllImport("oci.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern int OCINumberToText(OciHandle err, byte[] number, [MarshalAs(UnmanagedType.LPStr)] [In] string fmt, int fmt_length, IntPtr nls_params, uint nls_p_length, ref uint buf_size, [MarshalAs(UnmanagedType.LPArray)] [In] [Out] byte[] buffer);

		// Token: 0x0600078A RID: 1930
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCINumberTrunc(OciHandle err, byte[] number, int decplace, byte[] result);

		// Token: 0x0600078B RID: 1931
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIParamGet(OciHandle hndlp, [MarshalAs(UnmanagedType.U4)] [In] OCI.HTYPE htype, OciHandle errhp, out IntPtr paramdpp, uint pos);

		// Token: 0x0600078C RID: 1932
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIRowidToChar(OciHandle rowidDesc, NativeBuffer outbfp, ref ushort outbflp, OciHandle errhp);

		// Token: 0x0600078D RID: 1933
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIServerAttach(OciHandle srvhp, OciHandle errhp, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] dblink, int dblink_len, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x0600078E RID: 1934
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIServerDetach(IntPtr srvhp, IntPtr errhp, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x0600078F RID: 1935
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIServerVersion(OciHandle hndlp, OciHandle errhp, NativeBuffer bufp, uint bufsz, [MarshalAs(UnmanagedType.U1)] [In] byte hndltype);

		// Token: 0x06000790 RID: 1936
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCISessionBegin(OciHandle svchp, OciHandle errhp, OciHandle usrhp, [MarshalAs(UnmanagedType.U4)] [In] OCI.CRED credt, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x06000791 RID: 1937
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCISessionEnd(IntPtr svchp, IntPtr errhp, IntPtr usrhp, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x06000792 RID: 1938
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIStmtExecute(OciHandle svchp, OciHandle stmtp, OciHandle errhp, uint iters, uint rowoff, IntPtr snap_in, IntPtr snap_out, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x06000793 RID: 1939
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIStmtFetch(OciHandle stmtp, OciHandle errhp, uint nrows, [MarshalAs(UnmanagedType.U2)] [In] OCI.FETCH orientation, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x06000794 RID: 1940
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIStmtPrepare(OciHandle stmtp, OciHandle errhp, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] stmt, uint stmt_len, [MarshalAs(UnmanagedType.U4)] [In] OCI.SYNTAX language, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE mode);

		// Token: 0x06000795 RID: 1941
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCITransCommit(OciHandle svchp, OciHandle errhp, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE flags);

		// Token: 0x06000796 RID: 1942
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCITransRollback(OciHandle svchp, OciHandle errhp, [MarshalAs(UnmanagedType.U4)] [In] OCI.MODE flags);

		// Token: 0x06000797 RID: 1943
		[DllImport("oci.dll", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int OCIUnicodeToCharSet(OciHandle hndl, IntPtr dst, uint dstsz, IntPtr src, uint srcsz, out uint size);
	}
}
