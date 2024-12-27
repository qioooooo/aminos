using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Authentication.ExtendedProtection;

namespace System.Net
{
	// Token: 0x0200052C RID: 1324
	[SuppressUnmanagedCodeSecurity]
	internal abstract class SafeFreeContextBufferChannelBinding : ChannelBinding
	{
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x000A7BCE File Offset: 0x000A6BCE
		public override int Size
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000A7BD6 File Offset: 0x000A6BD6
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void Set(IntPtr value)
		{
			this.handle = value;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000A7BE0 File Offset: 0x000A6BE0
		internal static SafeFreeContextBufferChannelBinding CreateEmptyHandle(SecurDll dll)
		{
			switch (dll)
			{
			case SecurDll.SECURITY:
				return new SafeFreeContextBufferChannelBinding_SECURITY();
			case SecurDll.SECUR32:
				return new SafeFreeContextBufferChannelBinding_SECUR32();
			case SecurDll.SCHANNEL:
				return new SafeFreeContextBufferChannelBinding_SCHANNEL();
			default:
				throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "dll");
			}
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000A7C3C File Offset: 0x000A6C3C
		public unsafe static int QueryContextChannelBinding(SecurDll dll, SafeDeleteContext phContext, ContextAttribute contextAttribute, Bindings* buffer, SafeFreeContextBufferChannelBinding refHandle)
		{
			switch (dll)
			{
			case SecurDll.SECURITY:
				return SafeFreeContextBufferChannelBinding.QueryContextChannelBinding_SECURITY(phContext, contextAttribute, buffer, refHandle);
			case SecurDll.SECUR32:
				return SafeFreeContextBufferChannelBinding.QueryContextChannelBinding_SECUR32(phContext, contextAttribute, buffer, refHandle);
			case SecurDll.SCHANNEL:
				return SafeFreeContextBufferChannelBinding.QueryContextChannelBinding_SCHANNEL(phContext, contextAttribute, buffer, refHandle);
			default:
				return -1;
			}
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000A7C84 File Offset: 0x000A6C84
		private unsafe static int QueryContextChannelBinding_SECURITY(SafeDeleteContext phContext, ContextAttribute contextAttribute, Bindings* buffer, SafeFreeContextBufferChannelBinding refHandle)
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
					refHandle.Set(buffer->pBindings);
					refHandle.size = buffer->BindingsLength;
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000A7D24 File Offset: 0x000A6D24
		private unsafe static int QueryContextChannelBinding_SECUR32(SafeDeleteContext phContext, ContextAttribute contextAttribute, Bindings* buffer, SafeFreeContextBufferChannelBinding refHandle)
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
					refHandle.Set(buffer->pBindings);
					refHandle.size = buffer->BindingsLength;
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000A7DC4 File Offset: 0x000A6DC4
		private unsafe static int QueryContextChannelBinding_SCHANNEL(SafeDeleteContext phContext, ContextAttribute contextAttribute, Bindings* buffer, SafeFreeContextBufferChannelBinding refHandle)
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
					refHandle.Set(buffer->pBindings);
					refHandle.size = buffer->BindingsLength;
				}
				if (num != 0 && refHandle != null)
				{
					refHandle.SetHandleAsInvalid();
				}
			}
			return num;
		}

		// Token: 0x0400278C RID: 10124
		private int size;
	}
}
