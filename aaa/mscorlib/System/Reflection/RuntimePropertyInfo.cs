using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;

namespace System.Reflection
{
	// Token: 0x0200033F RID: 831
	[Serializable]
	internal sealed class RuntimePropertyInfo : PropertyInfo, ISerializable
	{
		// Token: 0x060020EF RID: 8431 RVA: 0x00052A70 File Offset: 0x00051A70
		internal unsafe RuntimePropertyInfo(int tkProperty, RuntimeType declaredType, RuntimeType.RuntimeTypeCache reflectedTypeCache, out bool isPrivate)
		{
			MetadataImport metadataImport = declaredType.Module.MetadataImport;
			this.m_token = tkProperty;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_declaringType = declaredType;
			RuntimeTypeHandle typeHandleInternal = declaredType.GetTypeHandleInternal();
			RuntimeTypeHandle runtimeTypeHandle = reflectedTypeCache.RuntimeTypeHandle;
			metadataImport.GetPropertyProps(tkProperty, out this.m_utf8name, out this.m_flags, out MetadataArgs.Skip.ConstArray);
			int associatesCount = metadataImport.GetAssociatesCount(tkProperty);
			AssociateRecord* ptr = stackalloc AssociateRecord[sizeof(AssociateRecord) * associatesCount];
			metadataImport.GetAssociates(tkProperty, ptr, associatesCount);
			RuntimeMethodInfo runtimeMethodInfo;
			Associates.AssignAssociates(ptr, associatesCount, typeHandleInternal, runtimeTypeHandle, out runtimeMethodInfo, out runtimeMethodInfo, out runtimeMethodInfo, out this.m_getterMethod, out this.m_setterMethod, out this.m_otherMethod, out isPrivate, out this.m_bindingFlags);
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00052B20 File Offset: 0x00051B20
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimePropertyInfo runtimePropertyInfo = o as RuntimePropertyInfo;
			return runtimePropertyInfo != null && runtimePropertyInfo.m_token == this.m_token && this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().Equals(runtimePropertyInfo.m_declaringType.GetTypeHandleInternal().GetModuleHandle());
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x00052B78 File Offset: 0x00051B78
		internal unsafe Signature Signature
		{
			get
			{
				if (this.m_signature == null)
				{
					void* ptr;
					ConstArray constArray;
					this.Module.MetadataImport.GetPropertyProps(this.m_token, out ptr, out MetadataArgs.Skip.PropertyAttributes, out constArray);
					this.m_signature = new Signature(constArray.Signature.ToPointer(), constArray.Length, this.m_declaringType.GetTypeHandleInternal());
				}
				return this.m_signature;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x00052BE6 File Offset: 0x00051BE6
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x00052BEE File Offset: 0x00051BEE
		internal bool EqualsSig(RuntimePropertyInfo target)
		{
			return Signature.DiffSigs(this.Signature, this.DeclaringType.GetTypeHandleInternal(), target.Signature, target.DeclaringType.GetTypeHandleInternal());
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x00052C18 File Offset: 0x00051C18
		public override string ToString()
		{
			string text = this.PropertyType.SigToString() + " " + this.Name;
			RuntimeTypeHandle[] arguments = this.Signature.Arguments;
			if (arguments.Length > 0)
			{
				Type[] array = new Type[arguments.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = arguments[i].GetRuntimeType();
				}
				text = text + " [" + RuntimeMethodInfo.ConstructParameters(array, this.Signature.CallingConvention) + "]";
			}
			return text;
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x00052C9C File Offset: 0x00051C9C
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00052CB4 File Offset: 0x00051CB4
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

		// Token: 0x060020F7 RID: 8439 RVA: 0x00052CFC File Offset: 0x00051CFC
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

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x00052D42 File Offset: 0x00051D42
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x00052D48 File Offset: 0x00051D48
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = new Utf8String(this.m_utf8name).ToString();
				}
				return this.m_name;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x00052D82 File Offset: 0x00051D82
		public override Type DeclaringType
		{
			get
			{
				return this.m_declaringType;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x060020FB RID: 8443 RVA: 0x00052D8A File Offset: 0x00051D8A
		public override Type ReflectedType
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeType;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x00052D97 File Offset: 0x00051D97
		public override int MetadataToken
		{
			get
			{
				return this.m_token;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x060020FD RID: 8445 RVA: 0x00052D9F File Offset: 0x00051D9F
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x00052DAC File Offset: 0x00051DAC
		public override Type[] GetRequiredCustomModifiers()
		{
			return this.Signature.GetCustomModifiers(0, true);
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x00052DBB File Offset: 0x00051DBB
		public override Type[] GetOptionalCustomModifiers()
		{
			return this.Signature.GetCustomModifiers(0, false);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x00052DCC File Offset: 0x00051DCC
		internal object GetConstantValue(bool raw)
		{
			object value = MdConstant.GetValue(this.Module.MetadataImport, this.m_token, this.PropertyType.GetTypeHandleInternal(), raw);
			if (value == DBNull.Value)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_EnumLitValueNotFound"));
			}
			return value;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x00052E15 File Offset: 0x00051E15
		public override object GetConstantValue()
		{
			return this.GetConstantValue(false);
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x00052E1E File Offset: 0x00051E1E
		public override object GetRawConstantValue()
		{
			return this.GetConstantValue(true);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x00052E28 File Offset: 0x00051E28
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			ArrayList arrayList = new ArrayList();
			if (Associates.IncludeAccessor(this.m_getterMethod, nonPublic))
			{
				arrayList.Add(this.m_getterMethod);
			}
			if (Associates.IncludeAccessor(this.m_setterMethod, nonPublic))
			{
				arrayList.Add(this.m_setterMethod);
			}
			if (this.m_otherMethod != null)
			{
				for (int i = 0; i < this.m_otherMethod.Length; i++)
				{
					if (Associates.IncludeAccessor(this.m_otherMethod[i], nonPublic))
					{
						arrayList.Add(this.m_otherMethod[i]);
					}
				}
			}
			return arrayList.ToArray(typeof(MethodInfo)) as MethodInfo[];
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002104 RID: 8452 RVA: 0x00052EC0 File Offset: 0x00051EC0
		public override Type PropertyType
		{
			get
			{
				return this.Signature.ReturnTypeHandle.GetRuntimeType();
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x00052EE0 File Offset: 0x00051EE0
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_getterMethod, nonPublic))
			{
				return null;
			}
			return this.m_getterMethod;
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x00052EF8 File Offset: 0x00051EF8
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_setterMethod, nonPublic))
			{
				return null;
			}
			return this.m_setterMethod;
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x00052F10 File Offset: 0x00051F10
		public override ParameterInfo[] GetIndexParameters()
		{
			int num = 0;
			ParameterInfo[] array = null;
			MethodInfo methodInfo = this.GetGetMethod(true);
			if (methodInfo != null)
			{
				array = methodInfo.GetParametersNoCopy();
				num = array.Length;
			}
			else
			{
				methodInfo = this.GetSetMethod(true);
				if (methodInfo != null)
				{
					array = methodInfo.GetParametersNoCopy();
					num = array.Length - 1;
				}
			}
			if (array != null && array.Length == 0)
			{
				return array;
			}
			ParameterInfo[] array2 = new ParameterInfo[num];
			for (int i = 0; i < num; i++)
			{
				array2[i] = new ParameterInfo(array[i], this);
			}
			return array2;
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002108 RID: 8456 RVA: 0x00052F81 File Offset: 0x00051F81
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x00052F89 File Offset: 0x00051F89
		public override bool CanRead
		{
			get
			{
				return this.m_getterMethod != null;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x0600210A RID: 8458 RVA: 0x00052F97 File Offset: 0x00051F97
		public override bool CanWrite
		{
			get
			{
				return this.m_setterMethod != null;
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x00052FA5 File Offset: 0x00051FA5
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, index, null);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x00052FB4 File Offset: 0x00051FB4
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo getMethod = this.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_GetMethNotFnd"));
			}
			return getMethod.Invoke(obj, invokeAttr, binder, index, null);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x00052FE8 File Offset: 0x00051FE8
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, index, null);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x00052FF8 File Offset: 0x00051FF8
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo setMethod = this.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_SetMethNotFnd"));
			}
			object[] array;
			if (index != null)
			{
				array = new object[index.Length + 1];
				for (int i = 0; i < index.Length; i++)
				{
					array[i] = index[i];
				}
				array[index.Length] = value;
			}
			else
			{
				array = new object[] { value };
			}
			setMethod.Invoke(obj, invokeAttr, binder, array, culture);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x0005306A File Offset: 0x0005206A
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedType, this.ToString(), MemberTypes.Property);
		}

		// Token: 0x04000DBE RID: 3518
		private int m_token;

		// Token: 0x04000DBF RID: 3519
		private string m_name;

		// Token: 0x04000DC0 RID: 3520
		private unsafe void* m_utf8name;

		// Token: 0x04000DC1 RID: 3521
		private PropertyAttributes m_flags;

		// Token: 0x04000DC2 RID: 3522
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000DC3 RID: 3523
		private RuntimeMethodInfo m_getterMethod;

		// Token: 0x04000DC4 RID: 3524
		private RuntimeMethodInfo m_setterMethod;

		// Token: 0x04000DC5 RID: 3525
		private MethodInfo[] m_otherMethod;

		// Token: 0x04000DC6 RID: 3526
		private RuntimeType m_declaringType;

		// Token: 0x04000DC7 RID: 3527
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DC8 RID: 3528
		private Signature m_signature;
	}
}
