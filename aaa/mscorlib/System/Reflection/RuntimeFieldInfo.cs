using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x02000339 RID: 825
	[Serializable]
	internal abstract class RuntimeFieldInfo : FieldInfo
	{
		// Token: 0x060020B5 RID: 8373 RVA: 0x00051BED File Offset: 0x00050BED
		protected RuntimeFieldInfo()
		{
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00051BF5 File Offset: 0x00050BF5
		protected RuntimeFieldInfo(RuntimeType.RuntimeTypeCache reflectedTypeCache, RuntimeType declaringType, BindingFlags bindingFlags)
		{
			this.m_bindingFlags = bindingFlags;
			this.m_declaringType = declaringType;
			this.m_reflectedTypeCache = reflectedTypeCache;
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x060020B7 RID: 8375 RVA: 0x00051C12 File Offset: 0x00050C12
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x00051C1A File Offset: 0x00050C1A
		private RuntimeTypeHandle ReflectedTypeHandle
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeTypeHandle;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x00051C28 File Offset: 0x00050C28
		internal RuntimeTypeHandle DeclaringTypeHandle
		{
			get
			{
				Type declaringType = this.DeclaringType;
				if (declaringType == null)
				{
					return this.Module.GetModuleHandle().GetModuleTypeHandle();
				}
				return declaringType.GetTypeHandleInternal();
			}
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x00051C59 File Offset: 0x00050C59
		internal virtual RuntimeFieldHandle GetFieldHandle()
		{
			return this.FieldHandle;
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x00051C61 File Offset: 0x00050C61
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x060020BC RID: 8380 RVA: 0x00051C64 File Offset: 0x00050C64
		public override Type ReflectedType
		{
			get
			{
				if (!this.m_reflectedTypeCache.IsGlobal)
				{
					return this.m_reflectedTypeCache.RuntimeType;
				}
				return null;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x00051C80 File Offset: 0x00050C80
		public override Type DeclaringType
		{
			get
			{
				if (!this.m_reflectedTypeCache.IsGlobal)
				{
					return this.m_declaringType;
				}
				return null;
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00051C97 File Offset: 0x00050C97
		public override string ToString()
		{
			return this.FieldType.SigToString() + " " + this.Name;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00051CB4 File Offset: 0x00050CB4
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00051CCC File Offset: 0x00050CCC
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.GetCustomAttributes(this, runtimeType);
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x00051D14 File Offset: 0x00050D14
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			RuntimeType runtimeType = attributeType.UnderlyingSystemType as RuntimeType;
			if (runtimeType == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
			}
			return CustomAttribute.IsDefined(this, runtimeType);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x00051D5A File Offset: 0x00050D5A
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Field);
		}

		// Token: 0x04000DAD RID: 3501
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DAE RID: 3502
		protected RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000DAF RID: 3503
		protected RuntimeType m_declaringType;
	}
}
