using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Data.OracleClient
{
	// Token: 0x02000037 RID: 55
	internal abstract class OciHandle : SafeHandle
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x0005A8CC File Offset: 0x00059CCC
		protected OciHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0005A8E8 File Offset: 0x00059CE8
		protected OciHandle(OCI.HTYPE handleType)
			: base(IntPtr.Zero, false)
		{
			this._handleType = handleType;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0005A908 File Offset: 0x00059D08
		protected OciHandle(OciHandle parentHandle, OCI.HTYPE handleType)
			: this(parentHandle, handleType, OCI.MODE.OCI_DEFAULT, OciHandle.HANDLEFLAG.DEFAULT)
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0005A920 File Offset: 0x00059D20
		protected OciHandle(OciHandle parentHandle, OCI.HTYPE handleType, OCI.MODE ocimode, OciHandle.HANDLEFLAG handleflags)
			: this()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this._handleType = handleType;
				this._parentHandle = parentHandle;
				this._refCount = 1;
				int num;
				if (handleType <= OCI.HTYPE.OCI_DTYPE_FILE)
				{
					switch (handleType)
					{
					case OCI.HTYPE.OCI_HTYPE_ENV:
						if ((handleflags & OciHandle.HANDLEFLAG.NLS) == OciHandle.HANDLEFLAG.NLS)
						{
							num = TracedNativeMethods.OCIEnvNlsCreate(out this.handle, ocimode, 0, 0);
							if (num != 0 || IntPtr.Zero == this.handle)
							{
								throw ADP.OperationFailed("OCIEnvNlsCreate", num);
							}
							goto IL_0177;
						}
						else
						{
							num = TracedNativeMethods.OCIEnvCreate(out this.handle, ocimode);
							if (num != 0 || IntPtr.Zero == this.handle)
							{
								throw ADP.OperationFailed("OCIEnvCreate", num);
							}
							goto IL_0177;
						}
						break;
					case OCI.HTYPE.OCI_HTYPE_ERROR:
					case OCI.HTYPE.OCI_HTYPE_SVCCTX:
					case OCI.HTYPE.OCI_HTYPE_STMT:
					case OCI.HTYPE.OCI_HTYPE_SERVER:
					case OCI.HTYPE.OCI_HTYPE_SESSION:
						num = TracedNativeMethods.OCIHandleAlloc(parentHandle.EnvironmentHandle, out this.handle, handleType);
						if (num != 0 || IntPtr.Zero == this.handle)
						{
							throw ADP.OperationFailed("OCIHandleAlloc", num);
						}
						goto IL_0177;
					case OCI.HTYPE.OCI_HTYPE_BIND:
					case OCI.HTYPE.OCI_HTYPE_DEFINE:
					case OCI.HTYPE.OCI_HTYPE_DESCRIBE:
						goto IL_0177;
					default:
						switch (handleType)
						{
						case OCI.HTYPE.OCI_DTYPE_FIRST:
						case OCI.HTYPE.OCI_DTYPE_ROWID:
						case OCI.HTYPE.OCI_DTYPE_FILE:
							break;
						case OCI.HTYPE.OCI_DTYPE_SNAP:
						case OCI.HTYPE.OCI_DTYPE_RSET:
						case OCI.HTYPE.OCI_DTYPE_PARAM:
						case OCI.HTYPE.OCI_DTYPE_COMPLEXOBJECTCOMP:
							goto IL_0177;
						default:
							goto IL_0177;
						}
						break;
					}
				}
				else if (handleType != OCI.HTYPE.OCI_DTYPE_INTERVAL_DS)
				{
					switch (handleType)
					{
					case OCI.HTYPE.OCI_DTYPE_TIMESTAMP:
					case OCI.HTYPE.OCI_DTYPE_TIMESTAMP_TZ:
					case OCI.HTYPE.OCI_DTYPE_TIMESTAMP_LTZ:
						break;
					default:
						goto IL_0177;
					}
				}
				num = TracedNativeMethods.OCIDescriptorAlloc(parentHandle.EnvironmentHandle, out this.handle, handleType);
				if (num != 0 || IntPtr.Zero == this.handle)
				{
					throw ADP.OperationFailed("OCIDescriptorAlloc", num);
				}
				IL_0177:
				if (parentHandle != null)
				{
					parentHandle.AddRef();
					this._isUnicode = parentHandle.IsUnicode;
				}
				else
				{
					this._isUnicode = (handleflags & OciHandle.HANDLEFLAG.UNICODE) == OciHandle.HANDLEFLAG.UNICODE;
				}
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0005AAE8 File Offset: 0x00059EE8
		internal OciHandle EnvironmentHandle
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				OciHandle ociHandle;
				if (this.HandleType == OCI.HTYPE.OCI_HTYPE_ENV)
				{
					ociHandle = this;
				}
				else
				{
					ociHandle = this.ParentHandle.EnvironmentHandle;
				}
				return ociHandle;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0005AB10 File Offset: 0x00059F10
		internal OCI.HTYPE HandleType
		{
			get
			{
				return this._handleType;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0005AB24 File Offset: 0x00059F24
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0005AB44 File Offset: 0x00059F44
		internal bool IsUnicode
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._isUnicode;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0005AB58 File Offset: 0x00059F58
		internal OciHandle ParentHandle
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this._parentHandle;
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0005AB6C File Offset: 0x00059F6C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal int AddRef()
		{
			return Interlocked.Increment(ref this._refCount);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0005AB88 File Offset: 0x00059F88
		internal void GetAttribute(OCI.ATTR attribute, out byte value, OciErrorHandle errorHandle)
		{
			uint num = 0U;
			int num2 = TracedNativeMethods.OCIAttrGet(this, out value, out num, attribute, errorHandle);
			if (num2 != 0)
			{
				OracleException.Check(errorHandle, num2);
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0005ABB0 File Offset: 0x00059FB0
		internal void GetAttribute(OCI.ATTR attribute, out short value, OciErrorHandle errorHandle)
		{
			uint num = 0U;
			int num2 = TracedNativeMethods.OCIAttrGet(this, out value, out num, attribute, errorHandle);
			if (num2 != 0)
			{
				OracleException.Check(errorHandle, num2);
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0005ABD8 File Offset: 0x00059FD8
		internal void GetAttribute(OCI.ATTR attribute, out int value, OciErrorHandle errorHandle)
		{
			uint num = 0U;
			int num2 = TracedNativeMethods.OCIAttrGet(this, out value, out num, attribute, errorHandle);
			if (num2 != 0)
			{
				OracleException.Check(errorHandle, num2);
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0005AC00 File Offset: 0x0005A000
		internal void GetAttribute(OCI.ATTR attribute, out string value, OciErrorHandle errorHandle, OracleConnection connection)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			int num2 = TracedNativeMethods.OCIAttrGet(this, ref zero, ref num, attribute, errorHandle);
			if (num2 != 0)
			{
				OracleException.Check(errorHandle, num2);
			}
			byte[] array = new byte[num];
			Marshal.Copy(zero, array, 0, checked((int)num));
			value = connection.GetString(array);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0005AC48 File Offset: 0x0005A048
		internal byte[] GetBytes(string value)
		{
			uint length = (uint)value.Length;
			byte[] array;
			if (this.IsUnicode)
			{
				array = new byte[(ulong)length * (ulong)((long)ADP.CharSize)];
				this.GetBytes(value.ToCharArray(), 0, length, array, 0);
			}
			else
			{
				byte[] array2 = new byte[length * 4U];
				uint bytes = this.GetBytes(value.ToCharArray(), 0, length, array2, 0);
				array = new byte[bytes];
				Buffer.BlockCopy(array2, 0, array, 0, checked((int)bytes));
			}
			return array;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0005ACB8 File Offset: 0x0005A0B8
		internal uint GetBytes(char[] chars, int charIndex, uint charCount, byte[] bytes, int byteIndex)
		{
			uint num;
			if (this.IsUnicode)
			{
				checked
				{
					num = (uint)((long)charCount * unchecked((long)ADP.CharSize));
					Buffer.BlockCopy(chars, unchecked(charIndex * ADP.CharSize), bytes, byteIndex, (int)num);
				}
			}
			else
			{
				OciHandle environmentHandle = this.EnvironmentHandle;
				GCHandle gchandle = default(GCHandle);
				GCHandle gchandle2 = default(GCHandle);
				int num2;
				try
				{
					gchandle = GCHandle.Alloc(chars, GCHandleType.Pinned);
					IntPtr intPtr = new IntPtr((long)gchandle.AddrOfPinnedObject() + (long)charIndex);
					IntPtr zero;
					if (bytes == null)
					{
						zero = IntPtr.Zero;
						num = 0U;
					}
					else
					{
						gchandle2 = GCHandle.Alloc(bytes, GCHandleType.Pinned);
						zero = new IntPtr((long)gchandle2.AddrOfPinnedObject() + (long)byteIndex);
						num = checked((uint)(bytes.Length - byteIndex));
					}
					num2 = UnsafeNativeMethods.OCIUnicodeToCharSet(environmentHandle, zero, num, intPtr, charCount, out num);
				}
				finally
				{
					gchandle.Free();
					if (gchandle2.IsAllocated)
					{
						gchandle2.Free();
					}
				}
				if (num2 != 0)
				{
					throw ADP.OperationFailed("OCIUnicodeToCharSet", num2);
				}
			}
			return num;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0005ADB4 File Offset: 0x0005A1B4
		internal uint GetChars(byte[] bytes, int byteIndex, uint byteCount, char[] chars, int charIndex)
		{
			uint num;
			if (this.IsUnicode)
			{
				checked
				{
					num = (uint)((long)byteCount / unchecked((long)ADP.CharSize));
					Buffer.BlockCopy(bytes, byteIndex, chars, unchecked(charIndex * ADP.CharSize), (int)byteCount);
				}
			}
			else
			{
				OciHandle environmentHandle = this.EnvironmentHandle;
				GCHandle gchandle = default(GCHandle);
				GCHandle gchandle2 = default(GCHandle);
				int num2;
				try
				{
					gchandle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
					IntPtr intPtr = new IntPtr((long)gchandle.AddrOfPinnedObject() + (long)byteIndex);
					IntPtr zero;
					if (chars == null)
					{
						zero = IntPtr.Zero;
						num = 0U;
					}
					else
					{
						gchandle2 = GCHandle.Alloc(chars, GCHandleType.Pinned);
						zero = new IntPtr((long)gchandle2.AddrOfPinnedObject() + (long)charIndex);
						num = checked((uint)(chars.Length - charIndex));
					}
					num2 = UnsafeNativeMethods.OCICharSetToUnicode(environmentHandle, zero, num, intPtr, byteCount, out num);
				}
				finally
				{
					gchandle.Free();
					if (gchandle2.IsAllocated)
					{
						gchandle2.Free();
					}
				}
				if (num2 != 0)
				{
					throw ADP.OperationFailed("OCICharSetToUnicode", num2);
				}
			}
			return num;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0005AEB0 File Offset: 0x0005A2B0
		internal static string GetAttributeName(OciHandle handle, OCI.ATTR atype)
		{
			if (OCI.HTYPE.OCI_DTYPE_PARAM == handle.HandleType)
			{
				return ((OCI.PATTR)atype).ToString();
			}
			return atype.ToString();
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0005AEE0 File Offset: 0x0005A2E0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal static IntPtr HandleValueToTrace(OciHandle handle)
		{
			return handle.DangerousGetHandle();
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0005AEF4 File Offset: 0x0005A2F4
		internal string PtrToString(NativeBuffer buf)
		{
			string text;
			if (this.IsUnicode)
			{
				text = buf.PtrToStringUni(0);
			}
			else
			{
				text = buf.PtrToStringAnsi(0);
			}
			return text;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0005AF20 File Offset: 0x0005A320
		internal string PtrToString(IntPtr buf, int len)
		{
			string text;
			if (this.IsUnicode)
			{
				text = Marshal.PtrToStringUni(buf, len);
			}
			else
			{
				text = Marshal.PtrToStringAnsi(buf, len);
			}
			return text;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0005AF48 File Offset: 0x0005A348
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal int Release()
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			int num;
			try
			{
			}
			finally
			{
				num = Interlocked.Decrement(ref this._refCount);
				if (num == 0)
				{
					IntPtr intPtr = Interlocked.CompareExchange(ref this.handle, IntPtr.Zero, this.handle);
					if (IntPtr.Zero != intPtr)
					{
						OCI.HTYPE handleType = this.HandleType;
						OciHandle parentHandle = this.ParentHandle;
						OCI.HTYPE htype = handleType;
						int num2;
						if (htype <= OCI.HTYPE.OCI_DTYPE_FIRST)
						{
							switch (htype)
							{
							case OCI.HTYPE.OCI_HTYPE_ENV:
								num2 = TracedNativeMethods.OCIHandleFree(intPtr, handleType);
								if (num2 != 0)
								{
									throw ADP.OperationFailed("OCIHandleFree", num2);
								}
								goto IL_015E;
							case OCI.HTYPE.OCI_HTYPE_ERROR:
							case OCI.HTYPE.OCI_HTYPE_STMT:
							case OCI.HTYPE.OCI_HTYPE_SESSION:
								break;
							case OCI.HTYPE.OCI_HTYPE_SVCCTX:
							{
								OciHandle ociHandle = parentHandle;
								if (ociHandle != null)
								{
									OciHandle parentHandle2 = ociHandle.ParentHandle;
									if (parentHandle2 != null)
									{
										OciHandle parentHandle3 = parentHandle2.ParentHandle;
										if (parentHandle3 != null)
										{
											num2 = TracedNativeMethods.OCISessionEnd(intPtr, parentHandle3.DangerousGetHandle(), ociHandle.DangerousGetHandle(), OCI.MODE.OCI_DEFAULT);
										}
									}
								}
								break;
							}
							case OCI.HTYPE.OCI_HTYPE_BIND:
							case OCI.HTYPE.OCI_HTYPE_DEFINE:
							case OCI.HTYPE.OCI_HTYPE_DESCRIBE:
								goto IL_015E;
							case OCI.HTYPE.OCI_HTYPE_SERVER:
								TracedNativeMethods.OCIServerDetach(intPtr, parentHandle.DangerousGetHandle(), OCI.MODE.OCI_DEFAULT);
								break;
							default:
								if (htype != OCI.HTYPE.OCI_DTYPE_FIRST)
								{
									goto IL_015E;
								}
								goto IL_0146;
							}
							num2 = TracedNativeMethods.OCIHandleFree(intPtr, handleType);
							if (num2 != 0)
							{
								throw ADP.OperationFailed("OCIHandleFree", num2);
							}
							goto IL_015E;
						}
						else
						{
							switch (htype)
							{
							case OCI.HTYPE.OCI_DTYPE_ROWID:
							case OCI.HTYPE.OCI_DTYPE_FILE:
								break;
							case OCI.HTYPE.OCI_DTYPE_COMPLEXOBJECTCOMP:
								goto IL_015E;
							default:
								if (htype != OCI.HTYPE.OCI_DTYPE_INTERVAL_DS)
								{
									switch (htype)
									{
									case OCI.HTYPE.OCI_DTYPE_TIMESTAMP:
									case OCI.HTYPE.OCI_DTYPE_TIMESTAMP_TZ:
									case OCI.HTYPE.OCI_DTYPE_TIMESTAMP_LTZ:
										break;
									default:
										goto IL_015E;
									}
								}
								break;
							}
						}
						IL_0146:
						num2 = TracedNativeMethods.OCIDescriptorFree(intPtr, handleType);
						if (num2 != 0)
						{
							throw ADP.OperationFailed("OCIDescriptorFree", num2);
						}
						IL_015E:
						if (parentHandle != null)
						{
							parentHandle.Release();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0005B0DC File Offset: 0x0005A4DC
		protected override bool ReleaseHandle()
		{
			this.Release();
			return true;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0005B0F4 File Offset: 0x0005A4F4
		internal static void SafeDispose(ref OciHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0005B110 File Offset: 0x0005A510
		internal static void SafeDispose(ref OciEnvironmentHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0005B12C File Offset: 0x0005A52C
		internal static void SafeDispose(ref OciErrorHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0005B148 File Offset: 0x0005A548
		internal static void SafeDispose(ref OciRowidDescriptor handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0005B164 File Offset: 0x0005A564
		internal static void SafeDispose(ref OciStatementHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0005B180 File Offset: 0x0005A580
		internal static void SafeDispose(ref OciSessionHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0005B19C File Offset: 0x0005A59C
		internal static void SafeDispose(ref OciServiceContextHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0005B1B8 File Offset: 0x0005A5B8
		internal static void SafeDispose(ref OciServerHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0005B1D4 File Offset: 0x0005A5D4
		internal static void SafeDispose(ref OciDefineHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0005B1F0 File Offset: 0x0005A5F0
		internal static void SafeDispose(ref OciBindHandle handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0005B20C File Offset: 0x0005A60C
		internal static void SafeDispose(ref OciParameterDescriptor handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0005B228 File Offset: 0x0005A628
		internal static void SafeDispose(ref OciDateTimeDescriptor handle)
		{
			if (handle != null)
			{
				handle.Dispose();
			}
			handle = null;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0005B244 File Offset: 0x0005A644
		internal void SetAttribute(OCI.ATTR attribute, int value, OciErrorHandle errorHandle)
		{
			int num = TracedNativeMethods.OCIAttrSet(this, ref value, 0U, attribute, errorHandle);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0005B268 File Offset: 0x0005A668
		internal void SetAttribute(OCI.ATTR attribute, OciHandle value, OciErrorHandle errorHandle)
		{
			int num = TracedNativeMethods.OCIAttrSet(this, value, 0U, attribute, errorHandle);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0005B28C File Offset: 0x0005A68C
		internal void SetAttribute(OCI.ATTR attribute, string value, OciErrorHandle errorHandle)
		{
			uint length = (uint)value.Length;
			byte[] array = new byte[length * 4U];
			uint bytes = this.GetBytes(value.ToCharArray(), 0, length, array, 0);
			int num = TracedNativeMethods.OCIAttrSet(this, array, bytes, attribute, errorHandle);
			if (num != 0)
			{
				OracleException.Check(errorHandle, num);
			}
		}

		// Token: 0x0400031C RID: 796
		private OCI.HTYPE _handleType;

		// Token: 0x0400031D RID: 797
		private int _refCount;

		// Token: 0x0400031E RID: 798
		private OciHandle _parentHandle;

		// Token: 0x0400031F RID: 799
		private bool _isUnicode;

		// Token: 0x02000038 RID: 56
		[Flags]
		protected enum HANDLEFLAG
		{
			// Token: 0x04000321 RID: 801
			DEFAULT = 0,
			// Token: 0x04000322 RID: 802
			UNICODE = 1,
			// Token: 0x04000323 RID: 803
			NLS = 2
		}
	}
}
