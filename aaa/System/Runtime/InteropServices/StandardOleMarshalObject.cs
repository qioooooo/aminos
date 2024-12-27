using System;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200026A RID: 618
	[ComVisible(true)]
	public class StandardOleMarshalObject : MarshalByRefObject, UnsafeNativeMethods.IMarshal
	{
		// Token: 0x06001580 RID: 5504 RVA: 0x00045E4B File Offset: 0x00044E4B
		protected StandardOleMarshalObject()
		{
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00045E54 File Offset: 0x00044E54
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private IntPtr GetStdMarshaller(ref Guid riid, int dwDestContext, int mshlflags)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr iunknownForObject = Marshal.GetIUnknownForObject(this);
			if (iunknownForObject != IntPtr.Zero)
			{
				try
				{
					if (UnsafeNativeMethods.CoGetStandardMarshal(ref riid, iunknownForObject, dwDestContext, IntPtr.Zero, mshlflags, out zero) == 0)
					{
						return zero;
					}
				}
				finally
				{
					Marshal.Release(iunknownForObject);
				}
			}
			throw new InvalidOperationException(SR.GetString("StandardOleMarshalObjectGetMarshalerFailed", new object[] { riid.ToString() }));
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00045ED4 File Offset: 0x00044ED4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IMarshal.GetUnmarshalClass(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out Guid pCid)
		{
			pCid = typeof(UnsafeNativeMethods.IStdMarshal).GUID;
			return 0;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00045EF0 File Offset: 0x00044EF0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IMarshal.GetMarshalSizeMax(ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags, out int pSize)
		{
			Guid guid = riid;
			IntPtr stdMarshaller = this.GetStdMarshaller(ref guid, dwDestContext, mshlflags);
			int num;
			try
			{
				num = UnsafeNativeMethods.CoGetMarshalSizeMax(out pSize, ref guid, stdMarshaller, dwDestContext, pvDestContext, mshlflags);
			}
			finally
			{
				Marshal.Release(stdMarshaller);
			}
			return num;
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x00045F3C File Offset: 0x00044F3C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IMarshal.MarshalInterface(object pStm, ref Guid riid, IntPtr pv, int dwDestContext, IntPtr pvDestContext, int mshlflags)
		{
			Guid guid = riid;
			IntPtr stdMarshaller = this.GetStdMarshaller(ref guid, dwDestContext, mshlflags);
			int num;
			try
			{
				num = UnsafeNativeMethods.CoMarshalInterface(pStm, ref guid, stdMarshaller, dwDestContext, pvDestContext, mshlflags);
			}
			finally
			{
				Marshal.Release(stdMarshaller);
				if (pStm != null)
				{
					Marshal.ReleaseComObject(pStm);
				}
			}
			return num;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x00045F94 File Offset: 0x00044F94
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IMarshal.UnmarshalInterface(object pStm, ref Guid riid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			if (pStm != null)
			{
				Marshal.ReleaseComObject(pStm);
			}
			return -2147467263;
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x00045FB0 File Offset: 0x00044FB0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IMarshal.ReleaseMarshalData(object pStm)
		{
			if (pStm != null)
			{
				Marshal.ReleaseComObject(pStm);
			}
			return -2147467263;
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x00045FC1 File Offset: 0x00044FC1
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int UnsafeNativeMethods.IMarshal.DisconnectObject(int dwReserved)
		{
			return -2147467263;
		}
	}
}
