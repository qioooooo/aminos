using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000108 RID: 264
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeFieldHandle : ISerializable
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x0002CC56 File Offset: 0x0002BC56
		internal unsafe RuntimeFieldHandle(void* pFieldHandle)
		{
			this.m_ptr = new IntPtr(pFieldHandle);
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000F74 RID: 3956 RVA: 0x0002CC64 File Offset: 0x0002BC64
		public IntPtr Value
		{
			[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
			get
			{
				return this.m_ptr;
			}
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x0002CC6C File Offset: 0x0002BC6C
		internal bool IsNullHandle()
		{
			return this.m_ptr.ToPointer() == null;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x0002CC7D File Offset: 0x0002BC7D
		public override int GetHashCode()
		{
			return ValueType.GetHashCodeOfPtr(this.m_ptr);
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x0002CC8C File Offset: 0x0002BC8C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj is RuntimeFieldHandle && ((RuntimeFieldHandle)obj).m_ptr == this.m_ptr;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x0002CCBC File Offset: 0x0002BCBC
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeFieldHandle handle)
		{
			return handle.m_ptr == this.m_ptr;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0002CCD0 File Offset: 0x0002BCD0
		public static bool operator ==(RuntimeFieldHandle left, RuntimeFieldHandle right)
		{
			return left.Equals(right);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x0002CCDA File Offset: 0x0002BCDA
		public static bool operator !=(RuntimeFieldHandle left, RuntimeFieldHandle right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06000F7B RID: 3963
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string GetName();

		// Token: 0x06000F7C RID: 3964
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe extern void* _GetUtf8Name();

		// Token: 0x06000F7D RID: 3965 RVA: 0x0002CCE7 File Offset: 0x0002BCE7
		internal Utf8String GetUtf8Name()
		{
			return new Utf8String(this._GetUtf8Name());
		}

		// Token: 0x06000F7E RID: 3966
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern FieldAttributes GetAttributes();

		// Token: 0x06000F7F RID: 3967
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeTypeHandle GetApproxDeclaringType();

		// Token: 0x06000F80 RID: 3968
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetToken();

		// Token: 0x06000F81 RID: 3969
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValue(object instance, RuntimeTypeHandle fieldType, RuntimeTypeHandle declaringType, ref bool domainInitialized);

		// Token: 0x06000F82 RID: 3970
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetValueDirect(RuntimeTypeHandle fieldType, TypedReference obj, RuntimeTypeHandle contextType);

		// Token: 0x06000F83 RID: 3971
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValue(object obj, object value, RuntimeTypeHandle fieldType, FieldAttributes fieldAttr, RuntimeTypeHandle declaringType, ref bool domainInitialized);

		// Token: 0x06000F84 RID: 3972
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetValueDirect(RuntimeTypeHandle fieldType, TypedReference obj, object value, RuntimeTypeHandle contextType);

		// Token: 0x06000F85 RID: 3973
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern RuntimeFieldHandle GetStaticFieldForGenericType(RuntimeTypeHandle declaringType);

		// Token: 0x06000F86 RID: 3974
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool AcquiresContextFromThis();

		// Token: 0x06000F87 RID: 3975 RVA: 0x0002CCF4 File Offset: 0x0002BCF4
		private RuntimeFieldHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			FieldInfo fieldInfo = (RuntimeFieldInfo)info.GetValue("FieldObj", typeof(RuntimeFieldInfo));
			if (fieldInfo == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
			this.m_ptr = fieldInfo.FieldHandle.Value;
			if (this.m_ptr.ToPointer() == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InsufficientState"));
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x0002CD70 File Offset: 0x0002BD70
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.m_ptr.ToPointer() == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_InvalidFieldState"));
			}
			RuntimeFieldInfo runtimeFieldInfo = (RuntimeFieldInfo)RuntimeType.GetFieldInfo(this);
			info.AddValue("FieldObj", runtimeFieldInfo, typeof(RuntimeFieldInfo));
		}

		// Token: 0x040004F9 RID: 1273
		private IntPtr m_ptr;
	}
}
