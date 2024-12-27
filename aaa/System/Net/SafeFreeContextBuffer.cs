using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000513 RID: 1299
	[SuppressUnmanagedCodeSecurity]
	internal abstract class SafeFreeContextBuffer : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002821 RID: 10273 RVA: 0x000A57CE File Offset: 0x000A47CE
		protected SafeFreeContextBuffer()
			: base(true)
		{
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000A57D7 File Offset: 0x000A47D7
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000A57E0 File Offset: 0x000A47E0
		internal static int EnumeratePackages(SecurDll Dll, out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			int num;
			switch (Dll)
			{
			case SecurDll.SECURITY:
			{
				SafeFreeContextBuffer_SECURITY safeFreeContextBuffer_SECURITY = null;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.EnumerateSecurityPackagesW(out pkgnum, out safeFreeContextBuffer_SECURITY);
				pkgArray = safeFreeContextBuffer_SECURITY;
				break;
			}
			case SecurDll.SECUR32:
			{
				SafeFreeContextBuffer_SECUR32 safeFreeContextBuffer_SECUR = null;
				num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.EnumerateSecurityPackagesA(out pkgnum, out safeFreeContextBuffer_SECUR);
				pkgArray = safeFreeContextBuffer_SECUR;
				break;
			}
			case SecurDll.SCHANNEL:
			{
				SafeFreeContextBuffer_SCHANNEL safeFreeContextBuffer_SCHANNEL = null;
				num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.EnumerateSecurityPackagesA(out pkgnum, out safeFreeContextBuffer_SCHANNEL);
				pkgArray = safeFreeContextBuffer_SCHANNEL;
				break;
			}
			default:
				throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
			}
			if (num != 0 && pkgArray != null)
			{
				pkgArray.SetHandleAsInvalid();
			}
			return num;
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000A5870 File Offset: 0x000A4870
		internal static SafeFreeContextBuffer CreateEmptyHandle(SecurDll dll)
		{
			switch (dll)
			{
			case SecurDll.SECURITY:
				return new SafeFreeContextBuffer_SECURITY();
			case SecurDll.SECUR32:
				return new SafeFreeContextBuffer_SECUR32();
			case SecurDll.SCHANNEL:
				return new SafeFreeContextBuffer_SCHANNEL();
			default:
				throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "dll");
			}
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000A58CC File Offset: 0x000A48CC
		public unsafe static int QueryContextAttributes(SecurDll dll, SafeDeleteContext phContext, ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			switch (dll)
			{
			case SecurDll.SECURITY:
				return SafeFreeContextBuffer.QueryContextAttributes_SECURITY(phContext, contextAttribute, buffer, refHandle);
			case SecurDll.SECUR32:
				return SafeFreeContextBuffer.QueryContextAttributes_SECUR32(phContext, contextAttribute, buffer, refHandle);
			case SecurDll.SCHANNEL:
				return SafeFreeContextBuffer.QueryContextAttributes_SCHANNEL(phContext, contextAttribute, buffer, refHandle);
			default:
				return -1;
			}
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000A5914 File Offset: 0x000A4914
		private unsafe static int QueryContextAttributes_SECURITY(SafeDeleteContext phContext, ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.QueryContextAttributesW(ref phContext._handle, contextAttribute, (void*)buffer);
					phContext.DangerousRelease();
				}
				if (num == 0 && refHandle != null)
				{
					if (refHandle is SafeFreeContextBuffer)
					{
						((SafeFreeContextBuffer)refHandle).Set(*(IntPtr*)buffer);
					}
					else
					{
						((SafeFreeCertContext)refHandle).Set(*(IntPtr*)buffer);
					}
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000A59C8 File Offset: 0x000A49C8
		private unsafe static int QueryContextAttributes_SECUR32(SafeDeleteContext phContext, ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.QueryContextAttributesA(ref phContext._handle, contextAttribute, (void*)buffer);
					phContext.DangerousRelease();
				}
				if (num == 0 && refHandle != null)
				{
					if (refHandle is SafeFreeContextBuffer)
					{
						((SafeFreeContextBuffer)refHandle).Set(*(IntPtr*)buffer);
					}
					else
					{
						((SafeFreeCertContext)refHandle).Set(*(IntPtr*)buffer);
					}
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000A5A7C File Offset: 0x000A4A7C
		private unsafe static int QueryContextAttributes_SCHANNEL(SafeDeleteContext phContext, ContextAttribute contextAttribute, byte* buffer, SafeHandle refHandle)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				phContext.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					phContext.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.QueryContextAttributesA(ref phContext._handle, contextAttribute, (void*)buffer);
					phContext.DangerousRelease();
				}
				if (num == 0 && refHandle != null)
				{
					if (refHandle is SafeFreeContextBuffer)
					{
						((SafeFreeContextBuffer)refHandle).Set(*(IntPtr*)buffer);
					}
					else
					{
						((SafeFreeCertContext)refHandle).Set(*(IntPtr*)buffer);
					}
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}
	}
}
