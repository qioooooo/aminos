using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace System.Data.OracleClient
{
	// Token: 0x02000082 RID: 130
	internal static class TracedNativeMethods
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x00070448 File Offset: 0x0006F848
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static int OraMTSEnlCtxGet(byte[] userName, byte[] password, byte[] serverName, OciHandle pOCISvc, OciHandle pOCIErr, out IntPtr pCtxt)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
			}
			finally
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OraMTSEnlCtxGet|ADV|OCI> userName=..., password=..., serverName=..., pOCISvc=0x%-07Ix pOCIErr=0x%-07Ix dwFlags=0x%08X\n", OciHandle.HandleValueToTrace(pOCISvc), OciHandle.HandleValueToTrace(pOCIErr), 0);
				}
				num = UnsafeNativeMethods.OraMTSEnlCtxGet(userName, password, serverName, pOCISvc, pOCIErr, 0U, out pCtxt);
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OraMTSEnlCtxGet|ADV|OCI|RET> pCtxt=0x%-07Ix rc=%d\n", pCtxt, num);
				}
			}
			return num;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000704C4 File Offset: 0x0006F8C4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static int OraMTSEnlCtxRel(IntPtr pCtxt)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
			}
			finally
			{
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OraMTSEnlCtxRel|ADV|OCI> pCtxt=%Id\n", pCtxt);
				}
				num = UnsafeNativeMethods.OraMTSEnlCtxRel(pCtxt);
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OraMTSEnlCtxRel|ADV|OCI|RET> rc=%d\n", num);
				}
			}
			return num;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x00070524 File Offset: 0x0006F924
		internal static int OraMTSOCIErrGet(ref int dwErr, NativeBuffer lpcEMsg, ref int lpdLen)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OraMTSOCIErrGet|ADV|OCI> dwErr=%08X, lpcEMsg=0x%-07Ix lpdLen=%d\n", dwErr, NativeBuffer.HandleValueToTrace(lpcEMsg), lpdLen);
			}
			int num = UnsafeNativeMethods.OraMTSOCIErrGet(ref dwErr, lpcEMsg, ref lpdLen);
			if (Bid.AdvancedOn)
			{
				if (num == 0)
				{
					Bid.Trace("<oc.OraMTSOCIErrGet|ADV|OCI|RET> rc=%d\n", num);
				}
				else
				{
					string text = lpcEMsg.PtrToStringAnsi(0, lpdLen);
					Bid.Trace("<oc.OraMTSOCIErrGet|ADV|OCI|RET> rd=%d message='%ls', lpdLen=%d\n", num, text, lpdLen);
				}
			}
			return num;
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00070588 File Offset: 0x0006F988
		internal static int OraMTSJoinTxn(OciEnlistContext pCtxt, IDtcTransaction pTrans)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OraMTSJoinTxn|ADV|OCI> pCtxt=0x%-07Ix pTrans=...\n", OciEnlistContext.HandleValueToTrace(pCtxt));
			}
			int num = UnsafeNativeMethods.OraMTSJoinTxn(pCtxt, pTrans);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OraMTSJoinTxn|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000705C8 File Offset: 0x0006F9C8
		internal static int oermsg(short rcode, NativeBuffer buf)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.oermsg|ADV|OCI> rcode=%d\n", (int)rcode);
			}
			int num = UnsafeNativeMethods.oermsg(rcode, buf);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.oermsg|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00070604 File Offset: 0x0006FA04
		internal static int OCIAttrGet(OciHandle trgthndlp, ref IntPtr attributep, ref uint sizep, OCI.ATTR attrtype, OciHandle errhp)
		{
			int num = UnsafeNativeMethods.OCIAttrGet(trgthndlp, trgthndlp.HandleType, ref attributep, ref sizep, attrtype, errhp);
			if (Bid.AdvancedOn)
			{
				if (OCI.ATTR.OCI_ATTR_SQLCODE == attrtype)
				{
					Bid.Trace("<oc.OCIAttrGet|ADV|OCI|RET>          trgthndlp=0x%-07Ix trghndltyp=%-18ls attrtype=%-22ls errhp=0x%-07Ix attributep=%-20ls sizep=%2d rc=%d\n", trgthndlp, trgthndlp.HandleType, attrtype, errhp, trgthndlp.PtrToString(attributep, checked((int)sizep)), sizep, num);
				}
				else
				{
					Bid.Trace("<oc.OCIAttrGet|ADV|OCI|RET>          trgthndlp=0x%-07Ix trghndltyp=%-18ls attrtype=%-22ls errhp=0x%-07Ix attributep=0x%-18Ix sizep=%2d rc=%d\n", trgthndlp, trgthndlp.HandleType, attrtype, errhp, attributep, sizep, num);
				}
			}
			return num;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00070674 File Offset: 0x0006FA74
		internal static int OCIAttrGet(OciHandle trgthndlp, out byte attributep, out uint sizep, OCI.ATTR attrtype, OciHandle errhp)
		{
			int num = 0;
			int num2 = UnsafeNativeMethods.OCIAttrGet(trgthndlp, trgthndlp.HandleType, out num, out sizep, attrtype, errhp);
			attributep = (byte)num;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIAttrGet|ADV|OCI|RET>          trgthndlp=0x%-07Ix trghndltyp=%-18ls attrtype=%-22ls errhp=0x%-07Ix attributep=%-20d sizep=%2d rc=%d\n", trgthndlp, trgthndlp.HandleType, attrtype, errhp, (int)attributep, sizep, num2);
			}
			return num2;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000706BC File Offset: 0x0006FABC
		internal static int OCIAttrGet(OciHandle trgthndlp, out short attributep, out uint sizep, OCI.ATTR attrtype, OciHandle errhp)
		{
			int num = 0;
			int num2 = UnsafeNativeMethods.OCIAttrGet(trgthndlp, trgthndlp.HandleType, out num, out sizep, attrtype, errhp);
			attributep = (short)num;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIAttrGet|ADV|OCI|RET>          trgthndlp=0x%-07Ix trghndltyp=%-18ls attrtype=%-22ls errhp=0x%-07Ix attributep=%-20d sizep=%2d rc=%d\n", trgthndlp, trgthndlp.HandleType, attrtype, errhp, (int)attributep, sizep, num2);
			}
			return num2;
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00070704 File Offset: 0x0006FB04
		internal static int OCIAttrGet(OciHandle trgthndlp, out int attributep, out uint sizep, OCI.ATTR attrtype, OciHandle errhp)
		{
			int num = UnsafeNativeMethods.OCIAttrGet(trgthndlp, trgthndlp.HandleType, out attributep, out sizep, attrtype, errhp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIAttrGet|ADV|OCI|RET>          trgthndlp=0x%-07Ix trghndltyp=%-18ls attrtype=%-22ls errhp=0x%-07Ix attributep=%-20d sizep=%2d rc=%d\n", trgthndlp, trgthndlp.HandleType, attrtype, errhp, attributep, sizep, num);
			}
			return num;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00070744 File Offset: 0x0006FB44
		internal static int OCIAttrGet(OciHandle trgthndlp, OciHandle attributep, out uint sizep, OCI.ATTR attrtype, OciHandle errhp)
		{
			int num = UnsafeNativeMethods.OCIAttrGet(trgthndlp, trgthndlp.HandleType, attributep, out sizep, attrtype, errhp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIAttrGet|ADV|OCI|RET>          trgthndlp=0x%-07Ix trghndltyp=%-18ls attrtype=%-22ls errhp=0x%-07Ix attributep=0x%-18Ix sizep=%2d rc=%d\n", trgthndlp, trgthndlp.HandleType, attrtype, errhp, OciHandle.HandleValueToTrace(attributep), sizep, num);
			}
			return num;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00070788 File Offset: 0x0006FB88
		internal static int OCIAttrSet(OciHandle trgthndlp, ref int attributep, uint size, OCI.ATTR attrtype, OciHandle errhp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIAttrSet|ADV|OCI>              trgthndlp=0x%-07Ix trghndltyp=%-18ls attributep=%-9d size=%-2d attrtype=%-22ls errhp=0x%-07Ix\n", trgthndlp, trgthndlp.HandleType, attributep, size, attrtype, errhp);
			}
			return UnsafeNativeMethods.OCIAttrSet(trgthndlp, trgthndlp.HandleType, ref attributep, size, attrtype, errhp);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x000707C8 File Offset: 0x0006FBC8
		internal static int OCIAttrSet(OciHandle trgthndlp, OciHandle attributep, uint size, OCI.ATTR attrtype, OciHandle errhp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIAttrSet|ADV|OCI>              trgthndlp=0x%-07Ix trghndltyp=%-18ls attributep=0x%-07Ix size=%d attrtype=%-22ls errhp=0x%-07Ix\n", trgthndlp, trgthndlp.HandleType, attributep, size, attrtype, errhp);
			}
			return UnsafeNativeMethods.OCIAttrSet(trgthndlp, trgthndlp.HandleType, attributep, size, attrtype, errhp);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00070808 File Offset: 0x0006FC08
		internal static int OCIAttrSet(OciHandle trgthndlp, byte[] attributep, uint size, OCI.ATTR attrtype, OciHandle errhp)
		{
			if (Bid.AdvancedOn)
			{
				string text;
				if (OCI.ATTR.OCI_ATTR_EXTERNAL_NAME == attrtype || OCI.ATTR.OCI_ATTR_INTERNAL_NAME == attrtype)
				{
					char[] chars = Encoding.UTF8.GetChars(attributep, 0, checked((int)size));
					text = new string(chars);
				}
				else
				{
					text = attributep.ToString();
				}
				Bid.Trace("<oc.OCIAttrSet|ADV|OCI>              trgthndlp=0x%-07Ix trghndltyp=%-18ls attributep='%ls' size=%d attrtype=%-22ls errhp=0x%-07Ix\n", trgthndlp, trgthndlp.HandleType, text, size, attrtype, errhp);
			}
			return UnsafeNativeMethods.OCIAttrSet(trgthndlp, trgthndlp.HandleType, attributep, size, attrtype, errhp);
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00070870 File Offset: 0x0006FC70
		internal static int OCIBindByName(OciHandle stmtp, out IntPtr bindpp, OciHandle errhp, string placeholder, int placeh_len, IntPtr valuep, int value_sz, OCI.DATATYPE dty, IntPtr indp, IntPtr alenp, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIBindByName|ADV|OCI>           stmtp=0x%-07Ix errhp=0x%-07Ix placeholder=%-20ls placeh_len=%-2d valuep=0x%-07Ix value_sz=%-4d dty=%d{OCI.DATATYPE} indp=0x%-07Ix *indp=%-3d alenp=0x%-07Ix *alenp=%-4d rcodep=0x%-07Ix maxarr_len=%-4d curelap=0x%-07Ix mode=0x%x{OCI.MODE}\n", OciHandle.HandleValueToTrace(stmtp), OciHandle.HandleValueToTrace(errhp), placeholder, placeh_len, valuep, value_sz, (int)dty, indp, (int)((IntPtr.Zero == indp) ? 0 : Marshal.ReadInt16(indp)), alenp, (int)((IntPtr.Zero == alenp) ? 0 : Marshal.ReadInt16(alenp)), IntPtr.Zero, 0U, IntPtr.Zero, (int)mode);
			}
			byte[] bytes = stmtp.GetBytes(placeholder);
			int num = bytes.Length;
			int num2 = UnsafeNativeMethods.OCIBindByName(stmtp, out bindpp, errhp, bytes, num, valuep, value_sz, dty, indp, alenp, IntPtr.Zero, 0U, IntPtr.Zero, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIBindByName|ADV|OCI|RET>       bindpp=0x%-07Ix rc=%d\n", bindpp, num2);
			}
			return num2;
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x0007092C File Offset: 0x0006FD2C
		internal static int OCIDefineByPos(OciHandle stmtp, out IntPtr hndlpp, OciHandle errhp, uint position, IntPtr valuep, int value_sz, OCI.DATATYPE dty, IntPtr indp, IntPtr rlenp, IntPtr rcodep, OCI.MODE mode)
		{
			int num = UnsafeNativeMethods.OCIDefineByPos(stmtp, out hndlpp, errhp, position, valuep, value_sz, dty, indp, rlenp, rcodep, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDefineByPos|ADV|OCI|RET>      stmtp=0x%-07Ix errhp=0x%-07Ix position=%-2d valuep=0x%-07Ix value_sz=%-4d dty=%-3d %-14s indp=0x%-07Ix rlenp=0x%-07Ix rcodep=0x%-07Ix mode=0x%x{OCI.MODE} hndlpp=0x%-07Ix rc=%d\n", stmtp, errhp, position, valuep, value_sz, (int)dty, dty, indp, rlenp, rcodep, (int)mode, hndlpp, num);
			}
			return num;
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00070980 File Offset: 0x0006FD80
		internal static int OCIDefineArrayOfStruct(OciHandle defnp, OciHandle errhp, uint pvskip, uint indskip, uint rlskip, uint rcskip)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDefineArrayOfStruct|ADV|OCI>  defnp=0x%-07Ix errhp=0x%-07Ix pvskip=%-4d indskip=%-4d rlskip=%-4d rcskip=%-4d\n", defnp, errhp, pvskip, indskip, rlskip, rcskip);
			}
			int num = UnsafeNativeMethods.OCIDefineArrayOfStruct(defnp, errhp, pvskip, indskip, rlskip, rcskip);
			if (num != 0 && Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDefineArrayOfStruct|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x000709CC File Offset: 0x0006FDCC
		internal static int OCIDefineDynamic(OciHandle defnp, OciHandle errhp, IntPtr octxp, OCI.Callback.OCICallbackDefine ocbfp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDefineDynamic|ADV|OCI>        defnp=0x%-07Ix errhp=0x%-07Ix octxp=0x%-07Ix ocbfp=...\n", OciHandle.HandleValueToTrace(defnp), OciHandle.HandleValueToTrace(errhp), octxp);
			}
			int num = UnsafeNativeMethods.OCIDefineDynamic(defnp, errhp, octxp, ocbfp);
			if (num != 0 && Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDefineDynamic|ADV|OCI|RET>    rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00070A18 File Offset: 0x0006FE18
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static int OCIDescriptorAlloc(OciHandle parenth, out IntPtr hndlpp, OCI.HTYPE type)
		{
			int num = UnsafeNativeMethods.OCIDescriptorAlloc(parenth, out hndlpp, type, 0U, IntPtr.Zero);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDescriptorAlloc|ADV|OCI|RET>  parenth=0x%-07Ix type=%3d xtramemsz=%d usrmempp=0x%-07Ix hndlpp=0x%-07Ix rc=%d\n", parenth, (int)type, 0, IntPtr.Zero, hndlpp, num);
			}
			return num;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x00070A58 File Offset: 0x0006FE58
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static int OCIDescriptorFree(IntPtr hndlp, OCI.HTYPE type)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIDescriptorFree|ADV|OCI>       hndlp=0x%Id type=%3d\n", hndlp, (int)type);
			}
			return UnsafeNativeMethods.OCIDescriptorFree(hndlp, type);
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00070A84 File Offset: 0x0006FE84
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static int OCIEnvCreate(out IntPtr envhpp, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIEnvCreate|ADV|OCI>  mode=0x%x{OCI.MODE} ctxp=0x%-07Ix malocfp=0x%-07Ix ralocfp=0x%-07Ix mfreefp=0x%-07Ix xtramemsz=%d usrmempp=0x%-07Ix", (int)mode, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero);
			}
			int num = UnsafeNativeMethods.OCIEnvCreate(out envhpp, mode, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0U, IntPtr.Zero);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIEnvCreate|ADV|OCI|RET>       envhpp=0x%-07Ix, rc=%d\n", envhpp, num);
			}
			return num;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00070AF8 File Offset: 0x0006FEF8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static int OCIEnvNlsCreate(out IntPtr envhpp, OCI.MODE mode, ushort charset, ushort ncharset)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIEnvNlsCreate|ADV|OCI> mode=0x%x{OCI.MODE} ctxp=0x%-07Ix malocfp=0x%-07Ix ralocfp=0x%-07Ix mfreefp=0x%-07Ix xtramemsz=%d usrmempp=0x%-07Ix charset=%d ncharset=%d", (int)mode, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0, IntPtr.Zero, (int)charset, (int)ncharset);
			}
			int num = UnsafeNativeMethods.OCIEnvNlsCreate(out envhpp, mode, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, 0U, IntPtr.Zero, charset, ncharset);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIEnvNlsCreate|ADV|OCI|RET>    envhpp=0x%-07Ix rc=%d\n", envhpp, num);
			}
			return num;
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00070B70 File Offset: 0x0006FF70
		internal static int OCIErrorGet(OciHandle hndlp, int recordno, out int errcodep, NativeBuffer bufp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIErrorGet|ADV|OCI>             hndlp=0x%-07Ix recordno=%d sqlstate=0x%-07Ix bufp=0x%-07Ix bufsiz=%d type=%d{OCI.HTYPE}\n", OciHandle.HandleValueToTrace(hndlp), recordno, IntPtr.Zero, NativeBuffer.HandleValueToTrace(bufp), bufp.Length, (int)hndlp.HandleType);
			}
			int num = UnsafeNativeMethods.OCIErrorGet(hndlp, checked((uint)recordno), IntPtr.Zero, out errcodep, bufp, (uint)bufp.Length, hndlp.HandleType);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIErrorGet|ADV|OCI|RET>         errcodep=%d rc=%d\n\t%ls\n\n", errcodep, num, hndlp.PtrToString(bufp));
			}
			return num;
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x00070BE4 File Offset: 0x0006FFE4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static int OCIHandleAlloc(OciHandle parenth, out IntPtr hndlpp, OCI.HTYPE type)
		{
			int num = UnsafeNativeMethods.OCIHandleAlloc(parenth, out hndlpp, type, 0U, IntPtr.Zero);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIHandleAlloc|ADV|OCI|RET>      parenth=0x%-07Ix type=%3d xtramemsz=%d usrmempp=0x%-07Ix hndlpp=0x%-07Ix rc=%d\n", parenth, (int)type, 0, IntPtr.Zero, hndlpp, num);
			}
			return num;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00070C24 File Offset: 0x00070024
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static int OCIHandleFree(IntPtr hndlp, OCI.HTYPE type)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIHandleFree|ADV|OCI>           hndlp=0x%-07Ix type=%3d\n", hndlp, (int)type);
			}
			return UnsafeNativeMethods.OCIHandleFree(hndlp, type);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00070C50 File Offset: 0x00070050
		internal static int OCILobAppend(OciHandle svchp, OciHandle errhp, OciHandle dst_locp, OciHandle src_locp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobAppend|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix dst_locp=0x%-07Ix src_locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(dst_locp), OciHandle.HandleValueToTrace(src_locp));
			}
			int num = UnsafeNativeMethods.OCILobAppend(svchp, errhp, dst_locp, src_locp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobAppend|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00070CA4 File Offset: 0x000700A4
		internal static int OCILobClose(OciHandle svchp, OciHandle errhp, OciHandle locp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oci.OCILobClose|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobClose(svchp, errhp, locp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobClose|ADV|OCI|RET> %d\n", num);
			}
			return num;
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00070CF0 File Offset: 0x000700F0
		internal static int OCILobCopy(OciHandle svchp, OciHandle errhp, OciHandle dst_locp, OciHandle src_locp, uint amount, uint dst_offset, uint src_offset)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobCopy|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix dst_locp=0x%-07Ix src_locp=0x%-07Ix amount=%u dst_offset=%u src_offset=%u\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(dst_locp), OciHandle.HandleValueToTrace(src_locp), amount, dst_offset, src_offset);
			}
			int num = UnsafeNativeMethods.OCILobCopy(svchp, errhp, dst_locp, src_locp, amount, dst_offset, src_offset);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobCopy|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00070D50 File Offset: 0x00070150
		internal static int OCILobCreateTemporary(OciHandle svchp, OciHandle errhp, OciHandle locp, [MarshalAs(UnmanagedType.U2)] [In] ushort csid, [MarshalAs(UnmanagedType.U1)] [In] OCI.CHARSETFORM csfrm, [MarshalAs(UnmanagedType.U1)] [In] OCI.LOB_TYPE lobtype, int cache, [MarshalAs(UnmanagedType.U2)] [In] OCI.DURATION duration)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobCreateTemporary|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=0x%-07Ix csid=%d csfrm=%d{OCI.CHARSETFORM} lobtype=%d{OCI.LOB_TYPE} cache=%d duration=%d{OCI.DURATION}\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp), (int)csid, (int)csfrm, (int)lobtype, cache, (int)duration);
			}
			int num = UnsafeNativeMethods.OCILobCreateTemporary(svchp, errhp, locp, csid, csfrm, lobtype, cache, duration);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobCreateTemporary|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00070DB0 File Offset: 0x000701B0
		internal static int OCILobErase(OciHandle svchp, OciHandle errhp, OciHandle locp, ref uint amount, uint offset)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobErase|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=0x%-07Ix amount=%d, offset=%d\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp), amount, offset);
			}
			int num = UnsafeNativeMethods.OCILobErase(svchp, errhp, locp, ref amount, offset);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobErase|ADV|OCI|RET> amount=%u, rc=%d\n", amount, num);
			}
			return num;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00070E08 File Offset: 0x00070208
		internal static int OCILobFileExists(OciHandle svchp, OciHandle errhp, OciHandle locp, out int flag)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFileExists|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobFileExists(svchp, errhp, locp, out flag);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFileExists|ADV|OCI|RET> flag=%u, rc=%d\n", flag, num);
			}
			return num;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00070E58 File Offset: 0x00070258
		internal static int OCILobFileGetName(OciHandle envhp, OciHandle errhp, OciHandle filep, IntPtr dir_alias, ref ushort d_length, IntPtr filename, ref ushort f_length)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFileGetName|ADV|OCI> envhp=0x%-07Ix errhp=0x%-07Ix filep=%Id\n", OciHandle.HandleValueToTrace(envhp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(filep));
			}
			int num = UnsafeNativeMethods.OCILobFileGetName(envhp, errhp, filep, dir_alias, ref d_length, filename, ref f_length);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFileGetName|ADV|OCI|RET> rc=%d, dir_alias='%ls', d_lenght=%d, filename='%ls', f_length=%d\n", num, envhp.PtrToString(dir_alias, (int)d_length), (int)d_length, envhp.PtrToString(filename, (int)f_length), (int)f_length);
			}
			return num;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00070EC8 File Offset: 0x000702C8
		internal static int OCILobFileSetName(OciHandle envhp, OciHandle errhp, OciFileDescriptor filep, string dir_alias, string filename)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFileSetName|ADV|OCI> envhp=0x%-07Ix errhp=0x%-07Ix filep=0x%-07Ix dir_alias='%ls', d_length=%d, filename='%ls', f_length=%d\n", OciHandle.HandleValueToTrace(envhp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(filep), dir_alias, dir_alias.Length, filename, filename.Length);
			}
			byte[] bytes = envhp.GetBytes(dir_alias);
			checked
			{
				ushort num = (ushort)bytes.Length;
				byte[] bytes2 = envhp.GetBytes(filename);
				ushort num2 = (ushort)bytes2.Length;
				int num3 = filep.OCILobFileSetNameWrapper(envhp, errhp, bytes, num, bytes2, num2);
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OCILobFileSetName|ADV|OCI|RET> rc=%d\n", num3);
				}
				return num3;
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00070F48 File Offset: 0x00070348
		internal static int OCILobFreeTemporary(OciHandle svchp, OciHandle errhp, OciHandle locp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFreeTemporary|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobFreeTemporary(svchp, errhp, locp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobFreeTemporary|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00070F94 File Offset: 0x00070394
		internal static int OCILobGetChunkSize(OciHandle svchp, OciHandle errhp, OciHandle locp, out uint lenp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobGetChunkSize|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobGetChunkSize(svchp, errhp, locp, out lenp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobGetChunkSize|ADV|OCI|RET> len=%u, rc=%d\n", lenp, num);
			}
			return num;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00070FE4 File Offset: 0x000703E4
		internal static int OCILobGetLength(OciHandle svchp, OciHandle errhp, OciHandle locp, out uint lenp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobGetLength|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobGetLength(svchp, errhp, locp, out lenp);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobGetLength|ADV|OCI|RET> len=%u, rc=%d\n", lenp, num);
			}
			return num;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00071034 File Offset: 0x00070434
		internal static int OCILobIsOpen(OciHandle svchp, OciHandle errhp, OciHandle locp, out int flag)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobIsOpen|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobIsOpen(svchp, errhp, locp, out flag);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobIsOpen|ADV|OCI|RET> flag=%d, rc=%d\n", flag, num);
			}
			return num;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00071084 File Offset: 0x00070484
		internal static int OCILobIsTemporary(OciHandle envhp, OciHandle errhp, OciHandle locp, out int flag)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobIsTemporary|ADV|OCI> envhp=0x%-07Ix errhp=0x%-07Ix locp=%Id\n", OciHandle.HandleValueToTrace(envhp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp));
			}
			int num = UnsafeNativeMethods.OCILobIsTemporary(envhp, errhp, locp, out flag);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobIsTemporary|ADV|OCI|RET> flag=%d, rc=%d\n", flag, num);
			}
			return num;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x000710D4 File Offset: 0x000704D4
		internal static int OCILobLoadFromFile(OciHandle svchp, OciHandle errhp, OciHandle dst_locp, OciHandle src_locp, uint amount, uint dst_offset, uint src_offset)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobLoadFromFile|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix dst_locp=0x%-07Ix src_locp=0x%-07Ix amount=%u dst_offset=%u src_offset=%u\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(dst_locp), OciHandle.HandleValueToTrace(src_locp), amount, dst_offset, src_offset);
			}
			int num = UnsafeNativeMethods.OCILobLoadFromFile(svchp, errhp, dst_locp, src_locp, amount, dst_offset, src_offset);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobLoadFromFile|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00071134 File Offset: 0x00070534
		internal static int OCILobOpen(OciHandle svchp, OciHandle errhp, OciHandle locp, byte mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobOpen|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=0x%-07Ix mode=%d\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp), (int)mode);
			}
			int num = UnsafeNativeMethods.OCILobOpen(svchp, errhp, locp, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobOpen|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00071184 File Offset: 0x00070584
		internal static int OCILobRead(OciHandle svchp, OciHandle errhp, OciHandle locp, ref int amtp, uint offset, IntPtr bufp, uint bufl, ushort csid, OCI.CHARSETFORM csfrm)
		{
			checked
			{
				uint num = (uint)amtp;
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OCILobRead|ADV|OCI>              svchp=0x%-07Ix errhp=0x%-07Ix locp=0x%-07Ix amt=%-4d offset=%-6u bufp=0x%-07Ix bufl=%-4d ctxp=0x%-07Ix cbfp=0x%-07Ix csid=%-4d csfrm=%d{OCI.CHARSETFORM}\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp), amtp, offset, bufp, (int)bufl, IntPtr.Zero, IntPtr.Zero, (int)csid, (int)csfrm);
				}
				int num2 = UnsafeNativeMethods.OCILobRead(svchp, errhp, locp, ref num, offset, bufp, bufl, IntPtr.Zero, IntPtr.Zero, csid, csfrm);
				amtp = (int)num;
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OCILobRead|ADV|OCI|RET>          amt=%-4d rc=%d\n", amtp, num2);
				}
				return num2;
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00071208 File Offset: 0x00070608
		internal static int OCILobTrim(OciHandle svchp, OciHandle errhp, OciHandle locp, uint newlen)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobTrim|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=0x%-07Ix newlen=%d\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp), newlen);
			}
			int num = UnsafeNativeMethods.OCILobTrim(svchp, errhp, locp, newlen);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCILobTrim|ADV|OCI|RET> rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00071258 File Offset: 0x00070658
		internal static int OCILobWrite(OciHandle svchp, OciHandle errhp, OciHandle locp, ref int amtp, uint offset, IntPtr bufp, uint buflen, byte piece, ushort csid, OCI.CHARSETFORM csfrm)
		{
			checked
			{
				uint num = (uint)amtp;
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OCILobWrite|ADV|OCI> svchp=0x%-07Ix errhp=0x%-07Ix locp=0x%-07Ix amt=%d offset=%u bufp=0x%-07Ix buflen=%d piece=%d{Byte} ctxp=0x%-07Ix cbfp=0x%-07Ix csid=%d csfrm=%d{OCI.CHARSETFORM}\n", OciHandle.HandleValueToTrace(svchp), OciHandle.HandleValueToTrace(errhp), OciHandle.HandleValueToTrace(locp), amtp, offset, bufp, (int)buflen, (int)piece, IntPtr.Zero, IntPtr.Zero, (int)csid, (int)csfrm);
				}
				int num2 = UnsafeNativeMethods.OCILobWrite(svchp, errhp, locp, ref num, offset, bufp, buflen, piece, IntPtr.Zero, IntPtr.Zero, csid, csfrm);
				amtp = (int)num;
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<oc.OCILobWrite|ADV|OCI|RET> amt=%d, rc=%d\n", amtp, num2);
				}
				return num2;
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x000712E0 File Offset: 0x000706E0
		internal static int OCIParamGet(OciHandle hndlp, OCI.HTYPE hType, OciHandle errhp, out IntPtr paramdpp, int pos)
		{
			int num = UnsafeNativeMethods.OCIParamGet(hndlp, hType, errhp, out paramdpp, checked((uint)pos));
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIParamGet|ADV|OCI|RET>         hndlp=0x%-07Ix htype=%-18ls errhp=0x%-07Ix pos=%d paramdpp=0x%-07Ix rc=%d\n", hndlp, hType, errhp, pos, paramdpp, num);
			}
			return num;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x00071318 File Offset: 0x00070718
		internal static int OCIRowidToChar(OciHandle rowidDesc, NativeBuffer outbfp, ref int bufferLength, OciHandle errhp)
		{
			ushort num = checked((ushort)bufferLength);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIRowidToChar|ADV|OCI>          rowidDesc=0x%-07Ix outbfp=0x%-07Ix outbflp=%d, errhp=0x%-07Ix\n", OciHandle.HandleValueToTrace(rowidDesc), NativeBuffer.HandleValueToTrace(outbfp), outbfp.Length, OciHandle.HandleValueToTrace(errhp));
			}
			int num2 = UnsafeNativeMethods.OCIRowidToChar(rowidDesc, outbfp, ref num, errhp);
			bufferLength = (int)num;
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIRowidToChar|ADV|OCI|RET>      outbfp='%ls' rc=%d\n", outbfp.PtrToStringAnsi(0, (int)num), num2);
			}
			return num2;
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0007137C File Offset: 0x0007077C
		internal static int OCIServerAttach(OciHandle srvhp, OciHandle errhp, string dblink, int dblink_len, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIServerAttach|ADV|OCI>         srvhp=0x%-07Ix errhp=0x%-07Ix dblink='%ls' dblink_len=%d mode=0x%x{OCI.MODE}\n", srvhp, errhp, dblink, dblink_len, (int)mode);
			}
			byte[] bytes = srvhp.GetBytes(dblink);
			int num = bytes.Length;
			int num2 = UnsafeNativeMethods.OCIServerAttach(srvhp, errhp, bytes, num, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIServerAttach|ADV|OCI|RET>     rc=%d\n", num2);
			}
			return num2;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x000713CC File Offset: 0x000707CC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static int OCIServerDetach(IntPtr srvhp, IntPtr errhp, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIServerDetach|ADV|OCI>        srvhp=0x%-07Ix errhp=0x%-07Ix mode=0x%x{OCI.MODE}\n", srvhp, errhp, (int)mode);
			}
			int num = UnsafeNativeMethods.OCIServerDetach(srvhp, errhp, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIServerDetach|ADV|OCI|RET>    rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0007140C File Offset: 0x0007080C
		internal static int OCIServerVersion(OciHandle hndlp, OciHandle errhp, NativeBuffer bufp)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIServerVersion|ADV|OCI>        hndlp=0x%-07Ix errhp=0x%-07Ix bufp=0x%-07Ix bufsz=%d hndltype=%d{OCI.HTYPE}\n", OciHandle.HandleValueToTrace(hndlp), OciHandle.HandleValueToTrace(errhp), NativeBuffer.HandleValueToTrace(bufp), bufp.Length, (int)hndlp.HandleType);
			}
			int num = UnsafeNativeMethods.OCIServerVersion(hndlp, errhp, bufp, (uint)bufp.Length, (byte)hndlp.HandleType);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIServerVersion|ADV|OCI|RET>    rc=%d\n%ls\n\n", num, hndlp.PtrToString(bufp));
			}
			return num;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00071478 File Offset: 0x00070878
		internal static int OCISessionBegin(OciHandle svchp, OciHandle errhp, OciHandle usrhp, OCI.CRED credt, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCISessionBegin|ADV|OCI>         svchp=0x%-07Ix errhp=0x%-07Ix usrhp=0x%-07Ix credt=%s mode=0x%x{OCI.MODE}\n", svchp, errhp, usrhp, credt, (int)mode);
			}
			int num = UnsafeNativeMethods.OCISessionBegin(svchp, errhp, usrhp, credt, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCISessionBegin|ADV|OCI|RET>     rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x000714BC File Offset: 0x000708BC
		internal static int OCISessionEnd(IntPtr svchp, IntPtr errhp, IntPtr usrhp, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCISessionEnd|ADV|OCI>           svchp=0x%-07Ix errhp=0x%-07Ix usrhp=0x%-07Ix mode=0x%x{OCI.MODE}\n", svchp, errhp, usrhp, (int)mode);
			}
			int num = UnsafeNativeMethods.OCISessionEnd(svchp, errhp, usrhp, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCISessionEnd|ADV|OCI|RET>       rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x000714FC File Offset: 0x000708FC
		internal static int OCIStmtExecute(OciHandle svchp, OciHandle stmtp, OciHandle errhp, int iters, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIStmtExecute|ADV|OCI>          svchp=0x%-07Ix stmtp=0x%-07Ix errhp=0x%-07Ix iters=%d rowoff=%d snap_in=0x%-07Ix snap_out=0x%-07Ix mode=0x%x{OCI.MODE}\n", svchp, stmtp, errhp, iters, 0, IntPtr.Zero, IntPtr.Zero, (int)mode);
			}
			int num = UnsafeNativeMethods.OCIStmtExecute(svchp, stmtp, errhp, checked((uint)iters), 0U, IntPtr.Zero, IntPtr.Zero, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIStmtExecute|ADV|OCI|RET>      rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00071558 File Offset: 0x00070958
		internal static int OCIStmtFetch(OciHandle stmtp, OciHandle errhp, int nrows, OCI.FETCH orientation, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIStmtFetch|ADV|OCI>            stmtp=0x%-07Ix errhp=0x%-07Ix nrows=%d orientation=%d{OCI.FETCH}, mode=0x%x{OCI.MODE}\n", stmtp, errhp, nrows, (int)orientation, (int)mode);
			}
			int num = UnsafeNativeMethods.OCIStmtFetch(stmtp, errhp, checked((uint)nrows), orientation, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIStmtFetch|ADV|OCI|RET>        rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x0007159C File Offset: 0x0007099C
		internal static int OCIStmtPrepare(OciHandle stmtp, OciHandle errhp, string stmt, OCI.SYNTAX language, OCI.MODE mode, OracleConnection connection)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIStmtPrepare|ADV|OCI>          stmtp=0x%-07Ix errhp=0x%-07Ix stmt_len=%d language=%d{OCI.SYNTAX} mode=0x%x{OCI.MODE}\n\t\t%ls\n\n", stmtp, errhp, stmt.Length, (int)language, (int)mode, stmt);
			}
			byte[] bytes = connection.GetBytes(stmt, false);
			uint num = (uint)bytes.Length;
			int num2 = UnsafeNativeMethods.OCIStmtPrepare(stmtp, errhp, bytes, num, language, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCIStmtPrepare|ADV|OCI|RET>      rc=%d\n", num2);
			}
			return num2;
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x000715F4 File Offset: 0x000709F4
		internal static int OCITransCommit(OciHandle srvhp, OciHandle errhp, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCITransCommit|ADV|OCI>          srvhp=0x%-07Ix errhp=0x%-07Ix mode=0x%x{OCI.MODE}\n", OciHandle.HandleValueToTrace(srvhp), OciHandle.HandleValueToTrace(errhp), (int)mode);
			}
			int num = UnsafeNativeMethods.OCITransCommit(srvhp, errhp, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCITransCommit|ADV|OCI|RET>      rc=%d\n", num);
			}
			return num;
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0007163C File Offset: 0x00070A3C
		internal static int OCITransRollback(OciHandle srvhp, OciHandle errhp, OCI.MODE mode)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCITransRollback|ADV|OCI>         srvhp=0x%-07Ix errhp=0x%-07Ix mode=0x%x{OCI.MODE}\n", OciHandle.HandleValueToTrace(srvhp), OciHandle.HandleValueToTrace(errhp), (int)mode);
			}
			int num = UnsafeNativeMethods.OCITransRollback(srvhp, errhp, mode);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<oc.OCITransRollback|ADV|OCI|RET>      rc=%d\n", num);
			}
			return num;
		}
	}
}
