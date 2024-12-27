using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020004F0 RID: 1264
	internal class SSPIAuthType : SSPIInterface
	{
		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x0600277D RID: 10109 RVA: 0x000A28F9 File Offset: 0x000A18F9
		// (set) Token: 0x0600277E RID: 10110 RVA: 0x000A2900 File Offset: 0x000A1900
		public SecurityPackageInfoClass[] SecurityPackages
		{
			get
			{
				return SSPIAuthType.m_SecurityPackages;
			}
			set
			{
				SSPIAuthType.m_SecurityPackages = value;
			}
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000A2908 File Offset: 0x000A1908
		public int EnumerateSecurityPackages(out int pkgnum, out SafeFreeContextBuffer pkgArray)
		{
			return SafeFreeContextBuffer.EnumeratePackages(SSPIAuthType.Library, out pkgnum, out pkgArray);
		}

		// Token: 0x06002780 RID: 10112 RVA: 0x000A2916 File Offset: 0x000A1916
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref AuthIdentity authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000A2927 File Offset: 0x000A1927
		public int AcquireDefaultCredential(string moduleName, CredentialUse usage, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireDefaultCredential(SSPIAuthType.Library, moduleName, usage, out outCredential);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000A2936 File Offset: 0x000A1936
		public int AcquireCredentialsHandle(string moduleName, CredentialUse usage, ref SecureCredential authdata, out SafeFreeCredentials outCredential)
		{
			return SafeFreeCredentials.AcquireCredentialsHandle(SSPIAuthType.Library, moduleName, usage, ref authdata, out outCredential);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000A2948 File Offset: 0x000A1948
		public int AcceptSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer inputBuffer, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPIAuthType.Library, ref credential, ref context, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x000A296C File Offset: 0x000A196C
		public int AcceptSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, SecurityBuffer[] inputBuffers, ContextFlags inFlags, Endianness endianness, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.AcceptSecurityContext(SSPIAuthType.Library, ref credential, ref context, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000A2990 File Offset: 0x000A1990
		public int InitializeSecurityContext(ref SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inputBuffer, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPIAuthType.Library, ref credential, ref context, targetName, inFlags, endianness, inputBuffer, null, outputBuffer, ref outFlags);
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000A29B8 File Offset: 0x000A19B8
		public int InitializeSecurityContext(SafeFreeCredentials credential, ref SafeDeleteContext context, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer[] inputBuffers, SecurityBuffer outputBuffer, ref ContextFlags outFlags)
		{
			return SafeDeleteContext.InitializeSecurityContext(SSPIAuthType.Library, ref credential, ref context, targetName, inFlags, endianness, null, inputBuffers, outputBuffer, ref outFlags);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000A29E0 File Offset: 0x000A19E0
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

		// Token: 0x06002788 RID: 10120 RVA: 0x000A2A70 File Offset: 0x000A1A70
		public int EncryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.EncryptMessageHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000A2A88 File Offset: 0x000A1A88
		private unsafe int DecryptMessageHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			uint num2 = 0U;
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, &num2);
					context.DangerousRelease();
				}
			}
			if (num == 0 && num2 == 2147483649U)
			{
				throw new InvalidOperationException(SR.GetString("net_auth_message_not_encrypted"));
			}
			return num;
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000A2B38 File Offset: 0x000A1B38
		public int DecryptMessage(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.DecryptMessageHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x000A2B50 File Offset: 0x000A1B50
		private int MakeSignatureHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.EncryptMessage(ref context._handle, 2147483649U, inputOutput, sequenceNumber);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000A2BE4 File Offset: 0x000A1BE4
		public int MakeSignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.MakeSignatureHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000A2BFC File Offset: 0x000A1BFC
		private unsafe int VerifySignatureHelper(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			int num = -2146893055;
			bool flag = false;
			uint num2 = 0U;
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
					num = UnsafeNclNativeMethods.NativeNTSSPI.DecryptMessage(ref context._handle, inputOutput, sequenceNumber, &num2);
					context.DangerousRelease();
				}
			}
			return num;
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000A2C90 File Offset: 0x000A1C90
		public int VerifySignature(SafeDeleteContext context, SecurityBufferDescriptor inputOutput, uint sequenceNumber)
		{
			if (ComNetOS.IsWin9x)
			{
				throw ExceptionHelper.MethodNotImplementedException;
			}
			return this.VerifySignatureHelper(context, inputOutput, sequenceNumber);
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x000A2CA8 File Offset: 0x000A1CA8
		public int QueryContextChannelBinding(SafeDeleteContext context, ContextAttribute attribute, out SafeFreeContextBufferChannelBinding binding)
		{
			binding = null;
			throw new NotSupportedException();
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000A2CB4 File Offset: 0x000A1CB4
		public unsafe int QueryContextAttributes(SafeDeleteContext context, ContextAttribute attribute, byte[] buffer, Type handleType, out SafeHandle refHandle)
		{
			refHandle = null;
			if (handleType != null)
			{
				if (handleType == typeof(SafeFreeContextBuffer))
				{
					refHandle = SafeFreeContextBuffer.CreateEmptyHandle(SSPIAuthType.Library);
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
				return SafeFreeContextBuffer.QueryContextAttributes(SSPIAuthType.Library, context, attribute, ptr, refHandle);
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000A2D51 File Offset: 0x000A1D51
		public int QuerySecurityContextToken(SafeDeleteContext phContext, out SafeCloseHandle phToken)
		{
			if (ComNetOS.IsWin9x)
			{
				throw new NotSupportedException();
			}
			return SafeCloseHandle.GetSecurityContextToken(phContext, out phToken);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000A2D67 File Offset: 0x000A1D67
		public int CompleteAuthToken(ref SafeDeleteContext refContext, SecurityBuffer[] inputBuffers)
		{
			if (ComNetOS.IsWin9x)
			{
				throw new NotSupportedException();
			}
			return SafeDeleteContext.CompleteAuthToken(SSPIAuthType.Library, ref refContext, inputBuffers);
		}

		// Token: 0x040026B9 RID: 9913
		private static readonly SecurDll Library = (ComNetOS.IsWin9x ? SecurDll.SECUR32 : SecurDll.SECURITY);

		// Token: 0x040026BA RID: 9914
		private static SecurityPackageInfoClass[] m_SecurityPackages;
	}
}
