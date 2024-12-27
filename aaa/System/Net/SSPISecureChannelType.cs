using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004EF RID: 1263
	internal class SSPISecureChannelType : SSPIInterface
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002765 RID: 10085 RVA: 0x000A248C File Offset: 0x000A148C
		// (set) Token: 0x06002766 RID: 10086 RVA: 0x000A2493 File Offset: 0x000A1493
		public SecurityPackageInfoClass[] SecurityPackages
		{
			get
			{
				return SSPISecureChannelType.m_SecurityPackages;
			}
			set
			{
				SSPISecureChannelType.m_SecurityPackages = value;
			}
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000A249B File Offset: 0x000A149B
		public int EnumerateSecurityPackages(out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			return SafeFreeContextBuffer.EnumeratePackages(SSPISecureChannelType.Library, out pkgnum, out pkgArray);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000A24A9 File Offset: 0x000A14A9
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPISecureChannelType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000A24BA File Offset: 0x000A14BA
		public int AcquireDefaultCredential(string moduleName, CredentialUse usage, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireDefaultCredential(SSPISecureChannelType.Library, moduleName, usage, out outCredential);
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000A24C9 File Offset: 0x000A14C9
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPISecureChannelType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000A24DC File Offset: 0x000A14DC
		public int AcceptSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer inputBuffer, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000A2500 File Offset: 0x000A1500
		public int AcceptSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000A2524 File Offset: 0x000A1524
		public int InitializeSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, targetName, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000A254C File Offset: 0x000A154C
		public int InitializeSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPISecureChannelType.Library, ref credential, ref context, targetName, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000A2574 File Offset: 0x000A1574
		private int EncryptMessageHelper9x(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.NativeSSLWin9xSSPI.SealMessage(ref context._handle, 0U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000A2604 File Offset: 0x000A1604
		private int EncryptMessageHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.NativeNTSSPI.EncryptMessage(ref context._handle, 0U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000A2694 File Offset: 0x000A1694
		public int EncryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				return this.EncryptMessageHelper9x(context, inputOutput, sequenceNumber);
			}
			return this.EncryptMessageHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x06002772 RID: 10098 RVA: 0x000A26B0 File Offset: 0x000A16B0
		private int DecryptMessageHelper9x(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.NativeSSLWin9xSSPI.UnsealMessage(ref context._handle, inputOutput, IntPtr.Zero, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x000A2744 File Offset: 0x000A1744
		private int DecryptMessageHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				context.DangerousAddRef(ref flag);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			catch
			{
				if (flag)
				{
					context.DangerousRelease();
					flag = false;
				}
				throw;
			}
			finally
			{
				if (flag)
				{
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, null);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x06002774 RID: 10100 RVA: 0x000A27D4 File Offset: 0x000A17D4
		public int DecryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				return this.DecryptMessageHelper9x(context, inputOutput, sequenceNumber);
			}
			return this.DecryptMessageHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000A27F0 File Offset: 0x000A17F0
		public int MakeSignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			throw ExceptionHelper.MethodNotSupportedException;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000A27F7 File Offset: 0x000A17F7
		public int VerifySignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			throw ExceptionHelper.MethodNotSupportedException;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000A2800 File Offset: 0x000A1800
		public unsafe int QueryContextChannelBinding(SafeDeleteContext phContext, ContextAttribute attribute, out SafeFreeContextBufferChannelBinding refHandle)
		{
			refHandle = SafeFreeContextBufferChannelBinding.CreateEmptyHandle(SSPISecureChannelType.Library);
			Bindings bindings = default(Bindings);
			return SafeFreeContextBufferChannelBinding.QueryContextChannelBinding(SSPISecureChannelType.Library, phContext, attribute, &bindings, refHandle);
		}

		// Token: 0x06002778 RID: 10104 RVA: 0x000A2834 File Offset: 0x000A1834
		public unsafe int QueryContextAttributes(SafeDeleteContext phContext, ContextAttribute attribute, byte[] buffer, Type handleType, out SafeHandle refHandle)
		{
			refHandle = null;
			if (handleType != null)
			{
				if (handleType == typeof(SafeFreeContextBuffer))
				{
					refHandle = SafeFreeContextBuffer.CreateEmptyHandle(SSPISecureChannelType.Library);
				}
				else
				{
					if (handleType != typeof(SafeFreeCertContext))
					{
						throw new ArgumentException(SR.GetString("SSPIInvalidHandleType", new object[] { handleType.FullName }), "handleType");
					}
					refHandle = new SafeFreeCertContext();
				}
			}
			fixed (byte* ptr = buffer)
			{
				return SafeFreeContextBuffer.QueryContextAttributes(SSPISecureChannelType.Library, phContext, attribute, ptr, refHandle);
			}
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000A28D1 File Offset: 0x000A18D1
		public int QuerySecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle phToken)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000A28D8 File Offset: 0x000A18D8
		public int CompleteAuthToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040026B7 RID: 9911
		private static readonly SecurDll Library = (ComNetOS.IsWin9x ? SecurDll.SCHANNEL : SecurDll.SECURITY);

		// Token: 0x040026B8 RID: 9912
		private static SecurityPackageInfoClass[] m_SecurityPackages;
	}
}
