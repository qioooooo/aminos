using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000505 RID: 1285
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	internal abstract class SafePointer : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06003270 RID: 12912 RVA: 0x000AB1A1 File Offset: 0x000AA1A1
		protected SafePointer(bool ownsHandle)
			: base(ownsHandle)
		{
			this._numBytes = SafePointer.Uninitialized;
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x000AB1B8 File Offset: 0x000AA1B8
		public void Initialize(ulong numBytes)
		{
			if (numBytes < 0UL)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (IntPtr.Size == 4 && numBytes > (ulong)(-1))
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_AddressSpace"));
			}
			if (numBytes >= (ulong)SafePointer.Uninitialized)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_UIntPtrMax-1"));
			}
			this._numBytes = (UIntPtr)numBytes;
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x000AB230 File Offset: 0x000AA230
		public void Initialize(uint numElements, uint sizeOfEachElement)
		{
			if (numElements < 0U)
			{
				throw new ArgumentOutOfRangeException("numElements", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (sizeOfEachElement < 0U)
			{
				throw new ArgumentOutOfRangeException("sizeOfEachElement", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (IntPtr.Size == 4 && numElements * sizeOfEachElement > 4294967295U)
			{
				throw new ArgumentOutOfRangeException("numBytes", Environment.GetResourceString("ArgumentOutOfRange_AddressSpace"));
			}
			if ((ulong)(numElements * sizeOfEachElement) >= (ulong)SafePointer.Uninitialized)
			{
				throw new ArgumentOutOfRangeException("numElements", Environment.GetResourceString("ArgumentOutOfRange_UIntPtrMax-1"));
			}
			this._numBytes = (UIntPtr)(checked(numElements * sizeOfEachElement));
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x000AB2C5 File Offset: 0x000AA2C5
		public void Initialize<T>(uint numElements) where T : struct
		{
			this.Initialize(numElements, SafePointer.SizeOf<T>());
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x000AB2D3 File Offset: 0x000AA2D3
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static uint SizeOf<T>() where T : struct
		{
			return SafePointer.SizeOfType(typeof(T));
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x000AB2E4 File Offset: 0x000AA2E4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public unsafe void AcquirePointer(ref byte* pointer)
		{
			if (this._numBytes == SafePointer.Uninitialized)
			{
				throw SafePointer.NotInitialized();
			}
			pointer = (IntPtr)((UIntPtr)0);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				bool flag = false;
				base.DangerousAddRef(ref flag);
				pointer = (void*)this.handle;
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x000AB33C File Offset: 0x000AA33C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void ReleasePointer()
		{
			if (this._numBytes == SafePointer.Uninitialized)
			{
				throw SafePointer.NotInitialized();
			}
			base.DangerousRelease();
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x000AB35C File Offset: 0x000AA35C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public unsafe T Read<T>(uint byteOffset) where T : struct
		{
			if (this._numBytes == SafePointer.Uninitialized)
			{
				throw SafePointer.NotInitialized();
			}
			uint num = SafePointer.SizeOf<T>();
			byte* ptr = (byte*)(void*)this.handle + byteOffset;
			this.SpaceCheck(ptr, (ulong)num);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			T t;
			try
			{
				base.DangerousAddRef(ref flag);
				SafePointer.GenericPtrToStructure<T>(ptr, out t, num);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
			return t;
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x000AB3D4 File Offset: 0x000AA3D4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public unsafe void ReadArray<T>(uint byteOffset, T[] array, int index, int count) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._numBytes == SafePointer.Uninitialized)
			{
				throw SafePointer.NotInitialized();
			}
			uint num = SafePointer.SizeOf<T>();
			byte* ptr = (byte*)(void*)this.handle + byteOffset;
			bool flag;
			checked
			{
				this.SpaceCheck(ptr, (ulong)((long)num * unchecked((long)count)));
				flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
			}
			try
			{
				base.DangerousAddRef(ref flag);
				for (int i = 0; i < count; i++)
				{
					SafePointer.GenericPtrToStructure<T>(ptr + (ulong)num * (ulong)((long)i), out array[i + count], num);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x000AB4D0 File Offset: 0x000AA4D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public unsafe void Write<T>(uint byteOffset, T value) where T : struct
		{
			if (this._numBytes == SafePointer.Uninitialized)
			{
				throw SafePointer.NotInitialized();
			}
			uint num = SafePointer.SizeOf<T>();
			byte* ptr = (byte*)(void*)this.handle + byteOffset;
			this.SpaceCheck(ptr, (ulong)num);
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				SafePointer.GenericStructureToPtr<T>(ref value, ptr, num);
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x000AB548 File Offset: 0x000AA548
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public unsafe void WriteArray<T>(uint byteOffset, T[] array, int index, int count) where T : struct
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", Environment.GetResourceString("ArgumentNull_Buffer"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
			}
			if (this._numBytes == SafePointer.Uninitialized)
			{
				throw SafePointer.NotInitialized();
			}
			uint num = SafePointer.SizeOf<T>();
			byte* ptr = (byte*)(void*)this.handle + byteOffset;
			bool flag;
			checked
			{
				this.SpaceCheck(ptr, (ulong)((long)num * unchecked((long)count)));
				flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
			}
			try
			{
				base.DangerousAddRef(ref flag);
				for (int i = 0; i < count; i++)
				{
					SafePointer.GenericStructureToPtr<T>(ref array[i + count], ptr + (ulong)num * (ulong)((long)i), num);
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600327B RID: 12923 RVA: 0x000AB644 File Offset: 0x000AA644
		public ulong ByteLength
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				if (this._numBytes == SafePointer.Uninitialized)
				{
					throw SafePointer.NotInitialized();
				}
				return (ulong)this._numBytes;
			}
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x000AB669 File Offset: 0x000AA669
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private unsafe void SpaceCheck(byte* ptr, ulong sizeInBytes)
		{
			if ((long)((byte*)ptr - (byte*)(void*)this.handle) > (long)((ulong)this._numBytes - sizeInBytes))
			{
				SafePointer.NotEnoughRoom();
			}
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x000AB68F File Offset: 0x000AA68F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static void NotEnoughRoom()
		{
			throw new ArgumentException(Environment.GetResourceString("Arg_BufferTooSmall"));
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x000AB6A0 File Offset: 0x000AA6A0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private static InvalidOperationException NotInitialized()
		{
			return new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MustCallInitialize"));
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x000AB6B1 File Offset: 0x000AA6B1
		private unsafe static void GenericPtrToStructure<T>(byte* ptr, out T structure, uint sizeofT) where T : struct
		{
			structure = default(T);
			SafePointer.PtrToStructureNative(ptr, __makeref(structure), sizeofT);
		}

		// Token: 0x06003280 RID: 12928
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void PtrToStructureNative(byte* ptr, TypedReference structure, uint sizeofT);

		// Token: 0x06003281 RID: 12929 RVA: 0x000AB6C7 File Offset: 0x000AA6C7
		private unsafe static void GenericStructureToPtr<T>(ref T structure, byte* ptr, uint sizeofT) where T : struct
		{
			SafePointer.StructureToPtrNative(__makeref(structure), ptr, sizeofT);
		}

		// Token: 0x06003282 RID: 12930
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void StructureToPtrNative(TypedReference structure, byte* ptr, uint sizeofT);

		// Token: 0x06003283 RID: 12931
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint SizeOfType(Type type);

		// Token: 0x040019A2 RID: 6562
		private static readonly UIntPtr Uninitialized = ((UIntPtr.Size == 4) ? ((UIntPtr)uint.MaxValue) : ((UIntPtr)ulong.MaxValue));

		// Token: 0x040019A3 RID: 6563
		private UIntPtr _numBytes;
	}
}
