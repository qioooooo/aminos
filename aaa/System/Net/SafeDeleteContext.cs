using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Net
{
	// Token: 0x02000523 RID: 1315
	internal abstract class SafeDeleteContext : SafeHandle
	{
		// Token: 0x0600285A RID: 10330 RVA: 0x000A61E5 File Offset: 0x000A51E5
		protected SafeDeleteContext()
			: base(IntPtr.Zero, true)
		{
			this._handle = default(SSPIHandle);
		}

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x000A61FF File Offset: 0x000A51FF
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this._handle.IsZero;
			}
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000A6216 File Offset: 0x000A5216
		public override string ToString()
		{
			return this._handle.ToString();
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000A622C File Offset: 0x000A522C
		internal unsafe static int InitializeSecurityContext(SecurDll dll, ref SafeFreeCredentials inCredentials, ref SafeDeleteContext refContext, string targetName, ContextFlags inFlags, Endianness endianness, SecurityBuffer inSecBuffer, SecurityBuffer[] inSecBuffers, SecurityBuffer outSecBuffer, ref ContextFlags outFlags)
		{
			if (inCredentials == null)
			{
				throw new ArgumentNullException("inCredentials");
			}
			SecurityBufferDescriptor securityBufferDescriptor = null;
			if (inSecBuffer != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(1);
			}
			else if (inSecBuffers != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(inSecBuffers.Length);
			}
			SecurityBufferDescriptor securityBufferDescriptor2 = new SecurityBufferDescriptor(1);
			bool flag = (inFlags & ContextFlags.AllocateMemory) != ContextFlags.Zero;
			int num = -1;
			SSPIHandle sspihandle = default(SSPIHandle);
			if (refContext != null)
			{
				sspihandle = refContext._handle;
			}
			GCHandle[] array = null;
			GCHandle gchandle = default(GCHandle);
			SafeFreeContextBuffer safeFreeContextBuffer = null;
			try
			{
				gchandle = GCHandle.Alloc(outSecBuffer.token, GCHandleType.Pinned);
				SecurityBufferStruct[] array2 = new SecurityBufferStruct[(securityBufferDescriptor == null) ? 1 : securityBufferDescriptor.Count];
				try
				{
					fixed (void* ptr = array2)
					{
						if (securityBufferDescriptor != null)
						{
							securityBufferDescriptor.UnmanagedPointer = ptr;
							array = new GCHandle[securityBufferDescriptor.Count];
							for (int i = 0; i < securityBufferDescriptor.Count; i++)
							{
								SecurityBuffer securityBuffer = ((inSecBuffer != null) ? inSecBuffer : inSecBuffers[i]);
								if (securityBuffer != null)
								{
									array2[i].count = securityBuffer.size;
									array2[i].type = securityBuffer.type;
									if (securityBuffer.unmanagedToken != null)
									{
										array2[i].token = securityBuffer.unmanagedToken.DangerousGetHandle();
									}
									else if (securityBuffer.token == null || securityBuffer.token.Length == 0)
									{
										array2[i].token = IntPtr.Zero;
									}
									else
									{
										array[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
										array2[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
									}
								}
							}
						}
						SecurityBufferStruct[] array3 = new SecurityBufferStruct[1];
						try
						{
							fixed (void* ptr2 = array3)
							{
								securityBufferDescriptor2.UnmanagedPointer = ptr2;
								array3[0].count = outSecBuffer.size;
								array3[0].type = outSecBuffer.type;
								if (outSecBuffer.token == null || outSecBuffer.token.Length == 0)
								{
									array3[0].token = IntPtr.Zero;
								}
								else
								{
									array3[0].token = Marshal.UnsafeAddrOfPinnedArrayElement(outSecBuffer.token, outSecBuffer.offset);
								}
								if (flag)
								{
									safeFreeContextBuffer = SafeFreeContextBuffer.CreateEmptyHandle(dll);
								}
								switch (dll)
								{
								case SecurDll.SECURITY:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SECURITY();
									}
									if (targetName == null || targetName.Length == 0)
									{
										targetName = " ";
									}
									try
									{
										fixed (char* ptr3 = targetName)
										{
											num = SafeDeleteContext.MustRunInitializeSecurityContext_SECURITY(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), (byte*)((targetName == " ") ? null : ptr3), inFlags, endianness, securityBufferDescriptor, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
											goto IL_044B;
										}
									}
									finally
									{
										string text = null;
									}
									break;
								case SecurDll.SECUR32:
									break;
								case SecurDll.SCHANNEL:
									goto IL_0381;
								default:
									goto IL_0423;
								}
								if (refContext == null || refContext.IsInvalid)
								{
									refContext = new SafeDeleteContext_SECUR32();
								}
								byte[] array4 = SafeDeleteContext.dummyBytes;
								if (targetName != null && targetName.Length != 0)
								{
									array4 = new byte[targetName.Length + 2];
									Encoding.Default.GetBytes(targetName, 0, targetName.Length, array4, 0);
								}
								try
								{
									fixed (byte* ptr4 = array4)
									{
										num = SafeDeleteContext.MustRunInitializeSecurityContext_SECUR32(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), (array4 == SafeDeleteContext.dummyBytes) ? null : ptr4, inFlags, endianness, securityBufferDescriptor, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
										goto IL_044B;
									}
								}
								finally
								{
									byte* ptr4 = null;
								}
								IL_0381:
								if (refContext == null || refContext.IsInvalid)
								{
									refContext = new SafeDeleteContext_SCHANNEL();
								}
								byte[] array5 = SafeDeleteContext.dummyBytes;
								if (targetName != null && targetName.Length != 0)
								{
									array5 = new byte[targetName.Length + 2];
									Encoding.Default.GetBytes(targetName, 0, targetName.Length, array5, 0);
								}
								try
								{
									fixed (byte* ptr5 = array5)
									{
										num = SafeDeleteContext.MustRunInitializeSecurityContext_SCHANNEL(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), (array5 == SafeDeleteContext.dummyBytes) ? null : ptr5, inFlags, endianness, securityBufferDescriptor, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
										goto IL_044B;
									}
								}
								finally
								{
									byte* ptr5 = null;
								}
								IL_0423:
								throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
								IL_044B:
								outSecBuffer.size = array3[0].count;
								outSecBuffer.type = array3[0].type;
								if (outSecBuffer.size > 0)
								{
									outSecBuffer.token = new byte[outSecBuffer.size];
									Marshal.Copy(array3[0].token, outSecBuffer.token, 0, outSecBuffer.size);
								}
								else
								{
									outSecBuffer.token = null;
								}
							}
						}
						finally
						{
							void* ptr2 = null;
						}
					}
				}
				finally
				{
					void* ptr = null;
				}
			}
			finally
			{
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].IsAllocated)
						{
							array[j].Free();
						}
					}
				}
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (safeFreeContextBuffer != null)
				{
					safeFreeContextBuffer.Close();
				}
			}
			return num;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000A67EC File Offset: 0x000A57EC
		private unsafe static int MustRunInitializeSecurityContext_SECURITY(ref SafeFreeCredentials inCredentials, void* inContextPtr, byte* targetName, ContextFlags inFlags, Endianness endianness, SecurityBufferDescriptor inputBuffer, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags attributes, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (!flag)
				{
					inCredentials = null;
				}
				else if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.InitializeSecurityContextW(ref handle, inContextPtr, targetName, inFlags, 0, endianness, inputBuffer, 0, ref outContext._handle, outputBuffer, ref attributes, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000A6930 File Offset: 0x000A5930
		private unsafe static int MustRunInitializeSecurityContext_SECUR32(ref SafeFreeCredentials inCredentials, void* inContextPtr, byte* targetName, ContextFlags inFlags, Endianness endianness, SecurityBufferDescriptor inputBuffer, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags attributes, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.InitializeSecurityContextA(ref handle, inContextPtr, targetName, inFlags, 0, endianness, inputBuffer, 0, ref outContext._handle, outputBuffer, ref attributes, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000A6A68 File Offset: 0x000A5A68
		private unsafe static int MustRunInitializeSecurityContext_SCHANNEL(ref SafeFreeCredentials inCredentials, void* inContextPtr, byte* targetName, ContextFlags inFlags, Endianness endianness, SecurityBufferDescriptor inputBuffer, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags attributes, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.InitializeSecurityContextA(ref handle, inContextPtr, targetName, inFlags, 0, endianness, inputBuffer, 0, ref outContext._handle, outputBuffer, ref attributes, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000A6BA0 File Offset: 0x000A5BA0
		internal unsafe static int AcceptSecurityContext(SecurDll dll, ref SafeFreeCredentials inCredentials, ref SafeDeleteContext refContext, ContextFlags inFlags, Endianness endianness, SecurityBuffer inSecBuffer, SecurityBuffer[] inSecBuffers, SecurityBuffer outSecBuffer, ref ContextFlags outFlags)
		{
			if (inCredentials == null)
			{
				throw new ArgumentNullException("inCredentials");
			}
			SecurityBufferDescriptor securityBufferDescriptor = null;
			if (inSecBuffer != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(1);
			}
			else if (inSecBuffers != null)
			{
				securityBufferDescriptor = new SecurityBufferDescriptor(inSecBuffers.Length);
			}
			SecurityBufferDescriptor securityBufferDescriptor2 = new SecurityBufferDescriptor(1);
			bool flag = (inFlags & ContextFlags.AllocateMemory) != ContextFlags.Zero;
			int num = -1;
			SSPIHandle sspihandle = default(SSPIHandle);
			if (refContext != null)
			{
				sspihandle = refContext._handle;
			}
			GCHandle[] array = null;
			GCHandle gchandle = default(GCHandle);
			SafeFreeContextBuffer safeFreeContextBuffer = null;
			try
			{
				gchandle = GCHandle.Alloc(outSecBuffer.token, GCHandleType.Pinned);
				SecurityBufferStruct[] array2 = new SecurityBufferStruct[(securityBufferDescriptor == null) ? 1 : securityBufferDescriptor.Count];
				try
				{
					fixed (void* ptr = array2)
					{
						if (securityBufferDescriptor != null)
						{
							securityBufferDescriptor.UnmanagedPointer = ptr;
							array = new GCHandle[securityBufferDescriptor.Count];
							for (int i = 0; i < securityBufferDescriptor.Count; i++)
							{
								SecurityBuffer securityBuffer = ((inSecBuffer != null) ? inSecBuffer : inSecBuffers[i]);
								if (securityBuffer != null)
								{
									array2[i].count = securityBuffer.size;
									array2[i].type = securityBuffer.type;
									if (securityBuffer.unmanagedToken != null)
									{
										array2[i].token = securityBuffer.unmanagedToken.DangerousGetHandle();
									}
									else if (securityBuffer.token == null || securityBuffer.token.Length == 0)
									{
										array2[i].token = IntPtr.Zero;
									}
									else
									{
										array[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
										array2[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
									}
								}
							}
						}
						SecurityBufferStruct[] array3 = new SecurityBufferStruct[1];
						try
						{
							fixed (void* ptr2 = array3)
							{
								securityBufferDescriptor2.UnmanagedPointer = ptr2;
								array3[0].count = outSecBuffer.size;
								array3[0].type = outSecBuffer.type;
								if (outSecBuffer.token == null || outSecBuffer.token.Length == 0)
								{
									array3[0].token = IntPtr.Zero;
								}
								else
								{
									array3[0].token = Marshal.UnsafeAddrOfPinnedArrayElement(outSecBuffer.token, outSecBuffer.offset);
								}
								if (flag)
								{
									safeFreeContextBuffer = SafeFreeContextBuffer.CreateEmptyHandle(dll);
								}
								switch (dll)
								{
								case SecurDll.SECURITY:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SECURITY();
									}
									num = SafeDeleteContext.MustRunAcceptSecurityContext_SECURITY(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor, inFlags, endianness, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
									break;
								case SecurDll.SECUR32:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SECUR32();
									}
									num = SafeDeleteContext.MustRunAcceptSecurityContext_SECUR32(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor, inFlags, endianness, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
									break;
								case SecurDll.SCHANNEL:
									if (refContext == null || refContext.IsInvalid)
									{
										refContext = new SafeDeleteContext_SCHANNEL();
									}
									num = SafeDeleteContext.MustRunAcceptSecurityContext_SCHANNEL(ref inCredentials, sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor, inFlags, endianness, refContext, securityBufferDescriptor2, ref outFlags, safeFreeContextBuffer);
									break;
								default:
									throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
								}
								outSecBuffer.size = array3[0].count;
								outSecBuffer.type = array3[0].type;
								if (outSecBuffer.size > 0)
								{
									outSecBuffer.token = new byte[outSecBuffer.size];
									Marshal.Copy(array3[0].token, outSecBuffer.token, 0, outSecBuffer.size);
								}
								else
								{
									outSecBuffer.token = null;
								}
							}
						}
						finally
						{
							void* ptr2 = null;
						}
					}
				}
				finally
				{
					void* ptr = null;
				}
			}
			finally
			{
				if (array != null)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].IsAllocated)
						{
							array[j].Free();
						}
					}
				}
				if (gchandle.IsAllocated)
				{
					gchandle.Free();
				}
				if (safeFreeContextBuffer != null)
				{
					safeFreeContextBuffer.Close();
				}
			}
			return num;
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000A7008 File Offset: 0x000A6008
		private unsafe static int MustRunAcceptSecurityContext_SECURITY(ref SafeFreeCredentials inCredentials, void* inContextPtr, SecurityBufferDescriptor inputBuffer, ContextFlags inFlags, Endianness endianness, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags outFlags, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (!flag)
				{
					inCredentials = null;
				}
				else if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.AcceptSecurityContext(ref handle, inContextPtr, inputBuffer, inFlags, endianness, ref outContext._handle, outputBuffer, ref outFlags, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x000A7148 File Offset: 0x000A6148
		private unsafe static int MustRunAcceptSecurityContext_SECUR32(ref SafeFreeCredentials inCredentials, void* inContextPtr, SecurityBufferDescriptor inputBuffer, ContextFlags inFlags, Endianness endianness, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags outFlags, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SECUR32.AcceptSecurityContext(ref handle, inContextPtr, inputBuffer, inFlags, endianness, ref outContext._handle, outputBuffer, ref outFlags, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000A727C File Offset: 0x000A627C
		private unsafe static int MustRunAcceptSecurityContext_SCHANNEL(ref SafeFreeCredentials inCredentials, void* inContextPtr, SecurityBufferDescriptor inputBuffer, ContextFlags inFlags, Endianness endianness, SafeDeleteContext outContext, SecurityBufferDescriptor outputBuffer, ref ContextFlags outFlags, SafeFreeContextBuffer handleTemplate)
		{
			int num = -2146893055;
			bool flag = false;
			bool flag2 = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				inCredentials.DangerousAddRef(ref flag);
				outContext.DangerousAddRef(ref flag2);
			}
			catch (Exception ex)
			{
				if (flag)
				{
					inCredentials.DangerousRelease();
					flag = false;
				}
				if (flag2)
				{
					outContext.DangerousRelease();
					flag2 = false;
				}
				if (!(ex is ObjectDisposedException))
				{
					throw;
				}
			}
			finally
			{
				SSPIHandle handle = inCredentials._handle;
				if (flag && flag2)
				{
					long num2;
					num = UnsafeNclNativeMethods.SafeNetHandles_SCHANNEL.AcceptSecurityContext(ref handle, inContextPtr, inputBuffer, inFlags, endianness, ref outContext._handle, outputBuffer, ref outFlags, out num2);
					if (outContext._EffectiveCredential != inCredentials && ((long)num & (long)((ulong)(-2147483648))) == 0L)
					{
						if (outContext._EffectiveCredential != null)
						{
							outContext._EffectiveCredential.DangerousRelease();
						}
						outContext._EffectiveCredential = inCredentials;
					}
					else
					{
						inCredentials.DangerousRelease();
					}
					outContext.DangerousRelease();
					if (handleTemplate != null)
					{
						handleTemplate.Set(((SecurityBufferStruct*)outputBuffer.UnmanagedPointer)->token);
						if (handleTemplate.IsInvalid)
						{
							handleTemplate.SetHandleAsInvalid();
						}
					}
				}
				if (inContextPtr == null && ((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					outContext._handle.SetToInvalid();
				}
			}
			return num;
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x000A73B0 File Offset: 0x000A63B0
		internal unsafe static int CompleteAuthToken(SecurDll dll, ref SafeDeleteContext refContext, SecurityBuffer[] inSecBuffers)
		{
			SecurityBufferDescriptor securityBufferDescriptor = new SecurityBufferDescriptor(inSecBuffers.Length);
			int num = -2146893055;
			GCHandle[] array = null;
			SecurityBufferStruct[] array2 = new SecurityBufferStruct[securityBufferDescriptor.Count];
			fixed (void* ptr = array2)
			{
				securityBufferDescriptor.UnmanagedPointer = ptr;
				array = new GCHandle[securityBufferDescriptor.Count];
				for (int i = 0; i < securityBufferDescriptor.Count; i++)
				{
					SecurityBuffer securityBuffer = inSecBuffers[i];
					if (securityBuffer != null)
					{
						array2[i].count = securityBuffer.size;
						array2[i].type = securityBuffer.type;
						if (securityBuffer.unmanagedToken != null)
						{
							array2[i].token = securityBuffer.unmanagedToken.DangerousGetHandle();
						}
						else if (securityBuffer.token == null || securityBuffer.token.Length == 0)
						{
							array2[i].token = IntPtr.Zero;
						}
						else
						{
							array[i] = GCHandle.Alloc(securityBuffer.token, GCHandleType.Pinned);
							array2[i].token = Marshal.UnsafeAddrOfPinnedArrayElement(securityBuffer.token, securityBuffer.offset);
						}
					}
				}
				SSPIHandle sspihandle = default(SSPIHandle);
				if (refContext != null)
				{
					sspihandle = refContext._handle;
				}
				try
				{
					if (dll == SecurDll.SECURITY)
					{
						if (refContext == null || refContext.IsInvalid)
						{
							refContext = new SafeDeleteContext_SECURITY();
						}
						bool flag = false;
						RuntimeHelpers.PrepareConstrainedRegions();
						try
						{
							try
							{
								refContext.DangerousAddRef(ref flag);
							}
							catch (Exception ex)
							{
								if (flag)
								{
									refContext.DangerousRelease();
									flag = false;
								}
								if (!(ex is ObjectDisposedException))
								{
									throw;
								}
							}
							goto IL_01CD;
						}
						finally
						{
							if (flag)
							{
								num = UnsafeNclNativeMethods.SafeNetHandles_SECURITY.CompleteAuthToken(sspihandle.IsZero ? null : ((void*)(&sspihandle)), securityBufferDescriptor);
								refContext.DangerousRelease();
							}
						}
						goto IL_01A5;
						IL_01CD:
						goto IL_0201;
					}
					IL_01A5:
					throw new ArgumentException(SR.GetString("net_invalid_enum", new object[] { "SecurDll" }), "Dll");
				}
				finally
				{
					if (array != null)
					{
						for (int j = 0; j < array.Length; j++)
						{
							if (array[j].IsAllocated)
							{
								array[j].Free();
							}
						}
					}
				}
				IL_0201:;
			}
			return num;
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x000A75EC File Offset: 0x000A65EC
		// Note: this type is marked as 'beforefieldinit'.
		static SafeDeleteContext()
		{
			byte[] array = new byte[1];
			SafeDeleteContext.dummyBytes = array;
		}

		// Token: 0x0400277D RID: 10109
		private const string dummyStr = " ";

		// Token: 0x0400277E RID: 10110
		private static readonly byte[] dummyBytes;

		// Token: 0x0400277F RID: 10111
		internal SSPIHandle _handle;

		// Token: 0x04002780 RID: 10112
		protected SafeFreeCredentials _EffectiveCredential;
	}
}
