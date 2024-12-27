using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace System.Security
{
	// Token: 0x02000673 RID: 1651
	public sealed class SecureString : IDisposable
	{
		// Token: 0x06003C0F RID: 15375 RVA: 0x000CE128 File Offset: 0x000CD128
		private static bool EncryptionSupported()
		{
			bool flag = true;
			try
			{
				Win32Native.SystemFunction041(SafeBSTRHandle.Allocate(null, 16U), 16U, 0U);
			}
			catch (EntryPointNotFoundException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x000CE160 File Offset: 0x000CD160
		internal SecureString(SecureString str)
		{
			this.AllocateBuffer(str.BufferLength);
			SafeBSTRHandle.Copy(str.m_buffer, this.m_buffer);
			this.m_length = str.m_length;
			this.m_enrypted = str.m_enrypted;
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x000CE19D File Offset: 0x000CD19D
		public SecureString()
		{
			this.CheckSupportedOnCurrentPlatform();
			this.AllocateBuffer(8);
			this.m_length = 0;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x000CE1BC File Offset: 0x000CD1BC
		[CLSCompliant(false)]
		public unsafe SecureString(char* value, int length)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (length > 65536)
			{
				throw new ArgumentOutOfRangeException("length", Environment.GetResourceString("ArgumentOutOfRange_Length"));
			}
			this.CheckSupportedOnCurrentPlatform();
			this.AllocateBuffer(length);
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.m_buffer.AcquirePointer(ref ptr);
				Buffer.memcpyimpl((byte*)value, ptr, length * 2);
			}
			finally
			{
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
			this.m_length = length;
			this.ProtectMemory();
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06003C13 RID: 15379 RVA: 0x000CE274 File Offset: 0x000CD274
		public int Length
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get
			{
				this.EnsureNotDisposed();
				return this.m_length;
			}
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x000CE284 File Offset: 0x000CD284
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void AppendChar(char c)
		{
			this.EnsureNotDisposed();
			this.EnsureNotReadOnly();
			this.EnsureCapacity(this.m_length + 1);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.UnProtectMemory();
				this.m_buffer.Write<char>((uint)(this.m_length * 2), c);
				this.m_length++;
			}
			finally
			{
				this.ProtectMemory();
			}
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x000CE2F4 File Offset: 0x000CD2F4
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Clear()
		{
			this.EnsureNotDisposed();
			this.EnsureNotReadOnly();
			this.m_length = 0;
			this.m_buffer.ClearBuffer();
			this.m_enrypted = false;
		}

		// Token: 0x06003C16 RID: 15382 RVA: 0x000CE31B File Offset: 0x000CD31B
		[MethodImpl(MethodImplOptions.Synchronized)]
		public SecureString Copy()
		{
			this.EnsureNotDisposed();
			return new SecureString(this);
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x000CE329 File Offset: 0x000CD329
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Dispose()
		{
			if (this.m_buffer != null && !this.m_buffer.IsInvalid)
			{
				this.m_buffer.Close();
				this.m_buffer = null;
			}
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x000CE354 File Offset: 0x000CD354
		[MethodImpl(MethodImplOptions.Synchronized)]
		public unsafe void InsertAt(int index, char c)
		{
			this.EnsureNotDisposed();
			this.EnsureNotReadOnly();
			if (index < 0 || index > this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_IndexString"));
			}
			this.EnsureCapacity(this.m_length + 1);
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.UnProtectMemory();
				this.m_buffer.AcquirePointer(ref ptr);
				char* ptr2 = (char*)ptr;
				for (int i = this.m_length; i > index; i--)
				{
					ptr2[i] = ptr2[i - 1];
				}
				ptr2[index] = c;
				this.m_length++;
			}
			finally
			{
				this.ProtectMemory();
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x000CE41C File Offset: 0x000CD41C
		[MethodImpl(MethodImplOptions.Synchronized)]
		public bool IsReadOnly()
		{
			this.EnsureNotDisposed();
			return this.m_readOnly;
		}

		// Token: 0x06003C1A RID: 15386 RVA: 0x000CE42A File Offset: 0x000CD42A
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void MakeReadOnly()
		{
			this.EnsureNotDisposed();
			this.m_readOnly = true;
		}

		// Token: 0x06003C1B RID: 15387 RVA: 0x000CE43C File Offset: 0x000CD43C
		[MethodImpl(MethodImplOptions.Synchronized)]
		public unsafe void RemoveAt(int index)
		{
			this.EnsureNotDisposed();
			this.EnsureNotReadOnly();
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_IndexString"));
			}
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.UnProtectMemory();
				this.m_buffer.AcquirePointer(ref ptr);
				char* ptr2 = (char*)ptr;
				for (int i = index; i < this.m_length - 1; i++)
				{
					ptr2[i] = ptr2[i + 1];
				}
				ptr2[(IntPtr)(--this.m_length) * 2] = '\0';
			}
			finally
			{
				this.ProtectMemory();
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
		}

		// Token: 0x06003C1C RID: 15388 RVA: 0x000CE4F8 File Offset: 0x000CD4F8
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void SetAt(int index, char c)
		{
			this.EnsureNotDisposed();
			this.EnsureNotReadOnly();
			if (index < 0 || index >= this.m_length)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_IndexString"));
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.UnProtectMemory();
				this.m_buffer.Write<char>((uint)(index * 2), c);
			}
			finally
			{
				this.ProtectMemory();
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06003C1D RID: 15389 RVA: 0x000CE568 File Offset: 0x000CD568
		private int BufferLength
		{
			get
			{
				return this.m_buffer.Length;
			}
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x000CE578 File Offset: 0x000CD578
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		private void AllocateBuffer(int size)
		{
			uint alignedSize = SecureString.GetAlignedSize(size);
			this.m_buffer = SafeBSTRHandle.Allocate(null, alignedSize);
			if (this.m_buffer.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
		}

		// Token: 0x06003C1F RID: 15391 RVA: 0x000CE5AC File Offset: 0x000CD5AC
		private void CheckSupportedOnCurrentPlatform()
		{
			if (!SecureString.supportedOnCurrentPlatform)
			{
				throw new NotSupportedException(Environment.GetResourceString("Arg_PlatformSecureString"));
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x000CE5C8 File Offset: 0x000CD5C8
		private void EnsureCapacity(int capacity)
		{
			if (capacity <= this.m_buffer.Length)
			{
				return;
			}
			if (capacity > 65536)
			{
				throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_Capacity"));
			}
			SafeBSTRHandle safeBSTRHandle = SafeBSTRHandle.Allocate(null, SecureString.GetAlignedSize(capacity));
			if (safeBSTRHandle.IsInvalid)
			{
				throw new OutOfMemoryException();
			}
			SafeBSTRHandle.Copy(this.m_buffer, safeBSTRHandle);
			this.m_buffer.Close();
			this.m_buffer = safeBSTRHandle;
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x000CE63A File Offset: 0x000CD63A
		private void EnsureNotDisposed()
		{
			if (this.m_buffer == null)
			{
				throw new ObjectDisposedException(null);
			}
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x000CE64B File Offset: 0x000CD64B
		private void EnsureNotReadOnly()
		{
			if (this.m_readOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ReadOnly"));
			}
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x000CE668 File Offset: 0x000CD668
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static uint GetAlignedSize(int size)
		{
			uint num = (uint)(size / 8 * 8);
			if (size % 8 != 0 || size == 0)
			{
				num += 8U;
			}
			return num;
		}

		// Token: 0x06003C24 RID: 15396 RVA: 0x000CE688 File Offset: 0x000CD688
		private unsafe int GetAnsiByteCount()
		{
			uint num = 1024U;
			uint num2 = 63U;
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			int num3;
			try
			{
				this.m_buffer.AcquirePointer(ref ptr);
				num3 = Win32Native.WideCharToMultiByte(0U, num, (char*)ptr, this.m_length, null, 0, IntPtr.Zero, new IntPtr((void*)(&num2)));
			}
			finally
			{
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
			return num3;
		}

		// Token: 0x06003C25 RID: 15397 RVA: 0x000CE6F8 File Offset: 0x000CD6F8
		private unsafe void GetAnsiBytes(byte* ansiStrPtr, int byteCount)
		{
			uint num = 1024U;
			uint num2 = 63U;
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.m_buffer.AcquirePointer(ref ptr);
				Win32Native.WideCharToMultiByte(0U, num, (char*)ptr, this.m_length, ansiStrPtr, byteCount - 1, IntPtr.Zero, new IntPtr((void*)(&num2)));
				*(ansiStrPtr + byteCount - 1) = 0;
			}
			finally
			{
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
		}

		// Token: 0x06003C26 RID: 15398 RVA: 0x000CE770 File Offset: 0x000CD770
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		private void ProtectMemory()
		{
			if (this.m_length == 0 || this.m_enrypted)
			{
				return;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				int num = Win32Native.SystemFunction040(this.m_buffer, (uint)(this.m_buffer.Length * 2), 0U);
				if (num < 0)
				{
					throw new CryptographicException(Win32Native.LsaNtStatusToWinError(num));
				}
				this.m_enrypted = true;
			}
		}

		// Token: 0x06003C27 RID: 15399 RVA: 0x000CE7D8 File Offset: 0x000CD7D8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.Synchronized)]
		internal unsafe IntPtr ToBSTR()
		{
			this.EnsureNotDisposed();
			int length = this.m_length;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					intPtr = Win32Native.SysAllocStringLen(null, length);
				}
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				this.UnProtectMemory();
				this.m_buffer.AcquirePointer(ref ptr);
				Buffer.memcpyimpl(ptr, (byte*)intPtr.ToPointer(), length * 2);
				intPtr2 = intPtr;
			}
			finally
			{
				this.ProtectMemory();
				if (intPtr2 == IntPtr.Zero && intPtr != IntPtr.Zero)
				{
					Win32Native.ZeroMemory(intPtr, (uint)(length * 2));
					Win32Native.SysFreeString(intPtr);
				}
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
			return intPtr2;
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x000CE8B0 File Offset: 0x000CD8B0
		[MethodImpl(MethodImplOptions.Synchronized)]
		internal unsafe IntPtr ToUniStr(bool allocateFromHeap)
		{
			this.EnsureNotDisposed();
			int length = this.m_length;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			byte* ptr = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					if (allocateFromHeap)
					{
						intPtr = Marshal.AllocHGlobal((length + 1) * 2);
					}
					else
					{
						intPtr = Marshal.AllocCoTaskMem((length + 1) * 2);
					}
				}
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				this.UnProtectMemory();
				this.m_buffer.AcquirePointer(ref ptr);
				Buffer.memcpyimpl(ptr, (byte*)intPtr.ToPointer(), length * 2);
				char* ptr2 = (char*)intPtr.ToPointer();
				ptr2[length] = '\0';
				intPtr2 = intPtr;
			}
			finally
			{
				this.ProtectMemory();
				if (intPtr2 == IntPtr.Zero && intPtr != IntPtr.Zero)
				{
					Win32Native.ZeroMemory(intPtr, (uint)(length * 2));
					if (allocateFromHeap)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					else
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
				}
				if (ptr != null)
				{
					this.m_buffer.ReleasePointer();
				}
			}
			return intPtr2;
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x000CE9B8 File Offset: 0x000CD9B8
		[MethodImpl(MethodImplOptions.Synchronized)]
		internal unsafe IntPtr ToAnsiStr(bool allocateFromHeap)
		{
			this.EnsureNotDisposed();
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			int num = 0;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				this.UnProtectMemory();
				num = this.GetAnsiByteCount() + 1;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					if (allocateFromHeap)
					{
						intPtr = Marshal.AllocHGlobal(num);
					}
					else
					{
						intPtr = Marshal.AllocCoTaskMem(num);
					}
				}
				if (intPtr == IntPtr.Zero)
				{
					throw new OutOfMemoryException();
				}
				this.GetAnsiBytes((byte*)intPtr.ToPointer(), num);
				intPtr2 = intPtr;
			}
			finally
			{
				this.ProtectMemory();
				if (intPtr2 == IntPtr.Zero && intPtr != IntPtr.Zero)
				{
					Win32Native.ZeroMemory(intPtr, (uint)num);
					if (allocateFromHeap)
					{
						Marshal.FreeHGlobal(intPtr);
					}
					else
					{
						Marshal.FreeCoTaskMem(intPtr);
					}
				}
			}
			return intPtr2;
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x000CEA84 File Offset: 0x000CDA84
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private void UnProtectMemory()
		{
			if (this.m_length == 0)
			{
				return;
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				if (this.m_enrypted)
				{
					int num = Win32Native.SystemFunction041(this.m_buffer, (uint)(this.m_buffer.Length * 2), 0U);
					if (num < 0)
					{
						throw new CryptographicException(Win32Native.LsaNtStatusToWinError(num));
					}
					this.m_enrypted = false;
				}
			}
		}

		// Token: 0x04001EB5 RID: 7861
		private const int BlockSize = 8;

		// Token: 0x04001EB6 RID: 7862
		private const int MaxLength = 65536;

		// Token: 0x04001EB7 RID: 7863
		private const uint ProtectionScope = 0U;

		// Token: 0x04001EB8 RID: 7864
		private SafeBSTRHandle m_buffer;

		// Token: 0x04001EB9 RID: 7865
		private int m_length;

		// Token: 0x04001EBA RID: 7866
		private bool m_readOnly;

		// Token: 0x04001EBB RID: 7867
		private bool m_enrypted;

		// Token: 0x04001EBC RID: 7868
		private static bool supportedOnCurrentPlatform = SecureString.EncryptionSupported();
	}
}
