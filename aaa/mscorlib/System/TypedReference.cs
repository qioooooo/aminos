using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000119 RID: 281
	[ComVisible(true)]
	[CLSCompliant(false)]
	public struct TypedReference
	{
		// Token: 0x06001062 RID: 4194 RVA: 0x0002EA48 File Offset: 0x0002DA48
		[CLSCompliant(false)]
		[ReflectionPermission(SecurityAction.LinkDemand, MemberAccess = true)]
		public unsafe static TypedReference MakeTypedReference(object target, FieldInfo[] flds)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (flds == null)
			{
				throw new ArgumentNullException("flds");
			}
			if (flds.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ArrayZeroError"));
			}
			RuntimeFieldHandle[] array = new RuntimeFieldHandle[flds.Length];
			Type type = target.GetType();
			for (int i = 0; i < flds.Length; i++)
			{
				FieldInfo fieldInfo = flds[i];
				if (!(fieldInfo is RuntimeFieldInfo))
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeFieldInfo"));
				}
				if (fieldInfo.IsInitOnly || fieldInfo.IsStatic)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_TypedReferenceInvalidField"));
				}
				if (type != fieldInfo.DeclaringType && !type.IsSubclassOf(fieldInfo.DeclaringType))
				{
					throw new MissingMemberException(Environment.GetResourceString("MissingMemberTypeRef"));
				}
				Type fieldType = fieldInfo.FieldType;
				if (fieldType.IsPrimitive)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_TypeRefPrimitve"));
				}
				if (i < flds.Length - 1 && !fieldType.IsValueType)
				{
					throw new MissingMemberException(Environment.GetResourceString("MissingMemberNestErr"));
				}
				array[i] = fieldInfo.FieldHandle;
				type = fieldType;
			}
			TypedReference typedReference = default(TypedReference);
			TypedReference.InternalMakeTypedReference((void*)(&typedReference), target, array, type.TypeHandle);
			return typedReference;
		}

		// Token: 0x06001063 RID: 4195
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void InternalMakeTypedReference(void* result, object target, RuntimeFieldHandle[] flds, RuntimeTypeHandle lastFieldType);

		// Token: 0x06001064 RID: 4196 RVA: 0x0002EB7E File Offset: 0x0002DB7E
		public override int GetHashCode()
		{
			if (this.Type == IntPtr.Zero)
			{
				return 0;
			}
			return __reftype(this).GetHashCode();
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0002EBA6 File Offset: 0x0002DBA6
		public override bool Equals(object o)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_NYI"));
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0002EBB7 File Offset: 0x0002DBB7
		public unsafe static object ToObject(TypedReference value)
		{
			return TypedReference.InternalToObject((void*)(&value));
		}

		// Token: 0x06001067 RID: 4199
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern object InternalToObject(void* value);

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06001068 RID: 4200 RVA: 0x0002EBC1 File Offset: 0x0002DBC1
		internal bool IsNull
		{
			get
			{
				return this.Value.IsNull() && this.Type.IsNull();
			}
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0002EBDD File Offset: 0x0002DBDD
		public static Type GetTargetType(TypedReference value)
		{
			return __reftype(value);
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0002EBE7 File Offset: 0x0002DBE7
		public static RuntimeTypeHandle TargetTypeToken(TypedReference value)
		{
			return __reftype(value).TypeHandle;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x0002EBF6 File Offset: 0x0002DBF6
		[CLSCompliant(false)]
		public unsafe static void SetTypedReference(TypedReference target, object value)
		{
			TypedReference.InternalSetTypedReference((void*)(&target), value);
		}

		// Token: 0x0600106C RID: 4204
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void InternalSetTypedReference(void* target, object value);

		// Token: 0x04000566 RID: 1382
		private IntPtr Value;

		// Token: 0x04000567 RID: 1383
		private IntPtr Type;
	}
}
