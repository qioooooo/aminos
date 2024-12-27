using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200051E RID: 1310
	internal abstract class SafeFreeCredentials : SafeHandle
	{
		// Token: 0x0600284C RID: 10316 RVA: 0x000A5E2B File Offset: 0x000A4E2B
		protected SafeFreeCredentials()
			: base(IntPtr.Zero, true)
		{
			this._handle = default(SSPIHandle);
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x0600284D RID: 10317 RVA: 0x000A5E45 File Offset: 0x000A4E45
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this._handle.IsZero;
			}
		}

		// Token: 0x0600284E RID: 10318 RVA: 0x000A5E5C File Offset: 0x000A4E5C
		public static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			switch (dll)
			{
			case SecurDll.SECURITY:
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_008D;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
				}
				break;
			case SecurDll.SECUR32:
				break;
			default:
				goto IL_0068;
			}
			outCredential = new SafeFreeCredential_SECUR32();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				goto IL_008D;
			}
			finally
			{
				long num2;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.AcquireCredentialsHandleA(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
			}
			IL_0068:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
			IL_008D:
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x000A5F20 File Offset: 0x000A4F20
		public static int AcquireDefaultCredential(SecurDll dll, string package, CredentialUse intent, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			switch (dll)
			{
			case SecurDll.SECURITY:
				outCredential = new SafeFreeCredential_SECURITY();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_0091;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, IntPtr.Zero, null, null, ref outCredential._handle, out num2);
				}
				break;
			case SecurDll.SECUR32:
				break;
			default:
				goto IL_006C;
			}
			outCredential = new SafeFreeCredential_SECUR32();
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				goto IL_0091;
			}
			finally
			{
				long num2;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.AcquireCredentialsHandleA(null, package, (int)intent, null, IntPtr.Zero, null, null, ref outCredential._handle, out num2);
			}
			IL_006C:
			throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
			IL_0091:
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000A5FE8 File Offset: 0x000A4FE8
		public unsafe static int AcquireCredentialsHandle(SecurDll dll, string package, CredentialUse intent, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			int num = -1;
			IntPtr certContextArray = authdata.certContextArray;
			try
			{
				IntPtr intPtr = new IntPtr((void*)(&certContextArray));
				if (certContextArray != IntPtr.Zero)
				{
					authdata.certContextArray = intPtr;
				}
				switch (dll)
				{
				case SecurDll.SECURITY:
					outCredential = new SafeFreeCredential_SECURITY();
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						goto IL_00BB;
					}
					finally
					{
						long num2;
						num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcquireCredentialsHandleW(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
					}
					break;
				case SecurDll.SECUR32:
					goto IL_0093;
				case SecurDll.SCHANNEL:
					break;
				default:
					goto IL_0093;
				}
				outCredential = new SafeFreeCredential_SCHANNEL();
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					goto IL_00BB;
				}
				finally
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.AcquireCredentialsHandleA(null, package, (int)intent, null, ref authdata, null, null, ref outCredential._handle, out num2);
				}
				IL_0093:
				throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
				IL_00BB:;
			}
			finally
			{
				authdata.certContextArray = certContextArray;
			}
			if (num != 0)
			{
				outCredential.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x04002778 RID: 10104
		internal SSPIHandle _handle;
	}
}
