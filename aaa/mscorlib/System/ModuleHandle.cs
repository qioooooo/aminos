using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;

namespace System
{
	// Token: 0x0200010A RID: 266
	[ComVisible(true)]
	public struct ModuleHandle
	{
		// Token: 0x06000F94 RID: 3988 RVA: 0x0002CE61 File Offset: 0x0002BE61
		internal unsafe ModuleHandle(void* pModule)
		{
			this.m_ptr = new IntPtr(pModule);
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000F95 RID: 3989 RVA: 0x0002CE6F File Offset: 0x0002BE6F
		internal unsafe void* Value
		{
			get
			{
				return this.m_ptr.ToPointer();
			}
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x0002CE7C File Offset: 0x0002BE7C
		internal bool IsNullHandle()
		{
			return this.m_ptr.ToPointer() == null;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x0002CE8D File Offset: 0x0002BE8D
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x0002CE9C File Offset: 0x0002BE9C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj is ModuleHandle && ((ModuleHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x0002CECC File Offset: 0x0002BECC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(ModuleHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x0002CEE0 File Offset: 0x0002BEE0
		public static bool operator ==(ModuleHandle left, ModuleHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x0002CEEA File Offset: 0x0002BEEA
		public static bool operator !=(ModuleHandle left, ModuleHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000F9C RID: 3996
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern RuntimeTypeHandle GetCallerType(ref StackCrawlMark stackMark);

		// Token: 0x06000F9D RID: 3997
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void* GetDynamicMethod(void* module, string name, byte[] sig, Resolver resolver);

		// Token: 0x06000F9E RID: 3998
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x06000F9F RID: 3999 RVA: 0x0002CEF8 File Offset: 0x0002BEF8
		internal static RuntimeTypeHandle[] CopyRuntimeTypeHandles(RuntimeTypeHandle[] inHandles)
		{
			if (inHandles == null || inHandles.Length == 0)
			{
				return inHandles;
			}
			RuntimeTypeHandle[] array = new RuntimeTypeHandle[inHandles.Length];
			for (int i = 0; i < inHandles.Length; i++)
			{
				array[i] = inHandles[i];
			}
			return array;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x0002CF3F File Offset: 0x0002BF3F
		private void ValidateModulePointer()
		{
			if (this.IsNullHandle())
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NullModuleHandle"));
			}
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0002CF59 File Offset: 0x0002BF59
		public RuntimeTypeHandle GetRuntimeTypeHandleFromMetadataToken(int typeToken)
		{
			return this.ResolveTypeHandle(typeToken);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x0002CF62 File Offset: 0x0002BF62
		public RuntimeTypeHandle ResolveTypeHandle(int typeToken)
		{
			return this.ResolveTypeHandle(typeToken, null, null);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0002CF70 File Offset: 0x0002BF70
		public unsafe RuntimeTypeHandle ResolveTypeHandle(int typeToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			this.ValidateModulePointer();
			if (!this.GetMetadataImport().IsValidToken(typeToken))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { typeToken, this }), new object[0]));
			}
			typeInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(typeInstantiationContext);
			methodInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(methodInstantiationContext);
			RuntimeTypeHandle runtimeTypeHandle;
			if (typeInstantiationContext == null || typeInstantiationContext.Length == 0)
			{
				if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
				{
					return this.ResolveType(typeToken, null, 0, null, 0);
				}
				int num = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr = methodInstantiationContext)
				{
					runtimeTypeHandle = this.ResolveType(typeToken, null, 0, ptr, num);
				}
			}
			else if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
			{
				int num2 = typeInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr2 = typeInstantiationContext)
				{
					runtimeTypeHandle = this.ResolveType(typeToken, ptr2, num2, null, 0);
				}
			}
			else
			{
				int num3 = typeInstantiationContext.Length;
				int num4 = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr3 = typeInstantiationContext)
				{
					fixed (RuntimeTypeHandle* ptr4 = methodInstantiationContext)
					{
						runtimeTypeHandle = this.ResolveType(typeToken, ptr3, num3, ptr4, num4);
					}
				}
			}
			return runtimeTypeHandle;
		}

		// Token: 0x06000FA4 RID: 4004
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern RuntimeTypeHandle ResolveType(int typeToken, RuntimeTypeHandle* typeInstArgs, int typeInstCount, RuntimeTypeHandle* methodInstArgs, int methodInstCount);

		// Token: 0x06000FA5 RID: 4005 RVA: 0x0002D0CD File Offset: 0x0002C0CD
		public RuntimeMethodHandle GetRuntimeMethodHandleFromMetadataToken(int methodToken)
		{
			return this.ResolveMethodHandle(methodToken);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x0002D0D6 File Offset: 0x0002C0D6
		public RuntimeMethodHandle ResolveMethodHandle(int methodToken)
		{
			return this.ResolveMethodHandle(methodToken, null, null);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0002D0E4 File Offset: 0x0002C0E4
		public unsafe RuntimeMethodHandle ResolveMethodHandle(int methodToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			this.ValidateModulePointer();
			if (!this.GetMetadataImport().IsValidToken(methodToken))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { methodToken, this }), new object[0]));
			}
			typeInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(typeInstantiationContext);
			methodInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(methodInstantiationContext);
			RuntimeMethodHandle runtimeMethodHandle;
			if (typeInstantiationContext == null || typeInstantiationContext.Length == 0)
			{
				if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
				{
					return this.ResolveMethod(methodToken, null, 0, null, 0);
				}
				int num = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr = methodInstantiationContext)
				{
					runtimeMethodHandle = this.ResolveMethod(methodToken, null, 0, ptr, num);
				}
			}
			else if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
			{
				int num2 = typeInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr2 = typeInstantiationContext)
				{
					runtimeMethodHandle = this.ResolveMethod(methodToken, ptr2, num2, null, 0);
				}
			}
			else
			{
				int num3 = typeInstantiationContext.Length;
				int num4 = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr3 = typeInstantiationContext)
				{
					fixed (RuntimeTypeHandle* ptr4 = methodInstantiationContext)
					{
						runtimeMethodHandle = this.ResolveMethod(methodToken, ptr3, num3, ptr4, num4);
					}
				}
			}
			return runtimeMethodHandle;
		}

		// Token: 0x06000FA8 RID: 4008
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern RuntimeMethodHandle ResolveMethod(int methodToken, RuntimeTypeHandle* typeInstArgs, int typeInstCount, RuntimeTypeHandle* methodInstArgs, int methodInstCount);

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002D241 File Offset: 0x0002C241
		public RuntimeFieldHandle GetRuntimeFieldHandleFromMetadataToken(int fieldToken)
		{
			return this.ResolveFieldHandle(fieldToken);
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x0002D24A File Offset: 0x0002C24A
		public RuntimeFieldHandle ResolveFieldHandle(int fieldToken)
		{
			return this.ResolveFieldHandle(fieldToken, null, null);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0002D258 File Offset: 0x0002C258
		public unsafe RuntimeFieldHandle ResolveFieldHandle(int fieldToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
		{
			this.ValidateModulePointer();
			if (!this.GetMetadataImport().IsValidToken(fieldToken))
			{
				throw new ArgumentOutOfRangeException("metadataToken", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidToken", new object[] { fieldToken, this }), new object[0]));
			}
			typeInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(typeInstantiationContext);
			methodInstantiationContext = ModuleHandle.CopyRuntimeTypeHandles(methodInstantiationContext);
			RuntimeFieldHandle runtimeFieldHandle;
			if (typeInstantiationContext == null || typeInstantiationContext.Length == 0)
			{
				if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
				{
					return this.ResolveField(fieldToken, null, 0, null, 0);
				}
				int num = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr = methodInstantiationContext)
				{
					runtimeFieldHandle = this.ResolveField(fieldToken, null, 0, ptr, num);
				}
			}
			else if (methodInstantiationContext == null || methodInstantiationContext.Length == 0)
			{
				int num2 = typeInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr2 = typeInstantiationContext)
				{
					runtimeFieldHandle = this.ResolveField(fieldToken, ptr2, num2, null, 0);
				}
			}
			else
			{
				int num3 = typeInstantiationContext.Length;
				int num4 = methodInstantiationContext.Length;
				fixed (RuntimeTypeHandle* ptr3 = typeInstantiationContext)
				{
					fixed (RuntimeTypeHandle* ptr4 = methodInstantiationContext)
					{
						runtimeFieldHandle = this.ResolveField(fieldToken, ptr3, num3, ptr4, num4);
					}
				}
			}
			return runtimeFieldHandle;
		}

		// Token: 0x06000FAC RID: 4012
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern RuntimeFieldHandle ResolveField(int fieldToken, RuntimeTypeHandle* typeInstArgs, int typeInstCount, RuntimeTypeHandle* methodInstArgs, int methodInstCount);

		// Token: 0x06000FAD RID: 4013
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Module GetModule();

		// Token: 0x06000FAE RID: 4014
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void* _GetModuleTypeHandle();

		// Token: 0x06000FAF RID: 4015 RVA: 0x0002D3B5 File Offset: 0x0002C3B5
		internal RuntimeTypeHandle GetModuleTypeHandle()
		{
			return new RuntimeTypeHandle(this._GetModuleTypeHandle());
		}

		// Token: 0x06000FB0 RID: 4016
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void _GetPEKind(out int peKind, out int machine);

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002D3C4 File Offset: 0x0002C3C4
		internal void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
		{
			int num;
			int num2;
			this._GetPEKind(out num, out num2);
			peKind = (PortableExecutableKinds)num;
			machine = (ImageFileMachine)num2;
		}

		// Token: 0x06000FB2 RID: 4018
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int _GetMDStreamVersion();

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0002D3E1 File Offset: 0x0002C3E1
		public int MDStreamVersion
		{
			get
			{
				return this._GetMDStreamVersion();
			}
		}

		// Token: 0x06000FB4 RID: 4020
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe extern void* _GetMetadataImport();

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0002D3E9 File Offset: 0x0002C3E9
		internal MetadataImport GetMetadataImport()
		{
			return new MetadataImport((IntPtr)this._GetMetadataImport());
		}

		// Token: 0x040004FB RID: 1275
		public static readonly ModuleHandle EmptyHandle = new ModuleHandle(null);

		// Token: 0x040004FC RID: 1276
		private IntPtr m_ptr;
	}
}
