using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000340 RID: 832
	[Serializable]
	internal sealed class RuntimeEventInfo : EventInfo, ISerializable
	{
		// Token: 0x06002110 RID: 8464 RVA: 0x00053094 File Offset: 0x00052094
		internal RuntimeEventInfo()
		{
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x0005309C File Offset: 0x0005209C
		internal unsafe RuntimeEventInfo(int tkEvent, RuntimeType declaredType, RuntimeType.RuntimeTypeCache reflectedTypeCache, out bool isPrivate)
		{
			MetadataImport metadataImport = declaredType.Module.MetadataImport;
			this.m_token = tkEvent;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_declaringType = declaredType;
			RuntimeTypeHandle typeHandleInternal = declaredType.GetTypeHandleInternal();
			RuntimeTypeHandle runtimeTypeHandle = reflectedTypeCache.RuntimeTypeHandle;
			metadataImport.GetEventProps(tkEvent, out this.m_utf8name, out this.m_flags);
			int associatesCount = metadataImport.GetAssociatesCount(tkEvent);
			AssociateRecord* ptr = stackalloc AssociateRecord[sizeof(AssociateRecord) * associatesCount];
			metadataImport.GetAssociates(tkEvent, ptr, associatesCount);
			RuntimeMethodInfo runtimeMethodInfo;
			Associates.AssignAssociates(ptr, associatesCount, typeHandleInternal, runtimeTypeHandle, out this.m_addMethod, out this.m_removeMethod, out this.m_raiseMethod, out runtimeMethodInfo, out runtimeMethodInfo, out this.m_otherMethod, out isPrivate, out this.m_bindingFlags);
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x00053144 File Offset: 0x00052144
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeEventInfo runtimeEventInfo = o as RuntimeEventInfo;
			return runtimeEventInfo != null && runtimeEventInfo.m_token == this.m_token && this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().Equals(runtimeEventInfo.m_declaringType.GetTypeHandleInternal().GetModuleHandle());
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002113 RID: 8467 RVA: 0x0005319B File Offset: 0x0005219B
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000531A4 File Offset: 0x000521A4
		public override string ToString()
		{
			if (this.m_addMethod == null || this.m_addMethod.GetParametersNoCopy().Length == 0)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoPublicAddMethod"));
			}
			return this.m_addMethod.GetParametersNoCopy()[0].ParameterType.SigToString() + " " + this.Name;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000531FF File Offset: 0x000521FF
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x00053218 File Offset: 0x00052218
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

		// Token: 0x06002117 RID: 8471 RVA: 0x00053260 File Offset: 0x00052260
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

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002118 RID: 8472 RVA: 0x000532A6 File Offset: 0x000522A6
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Event;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002119 RID: 8473 RVA: 0x000532AC File Offset: 0x000522AC
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

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x000532E6 File Offset: 0x000522E6
		public override Type DeclaringType
		{
			get
			{
				return this.m_declaringType;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x000532EE File Offset: 0x000522EE
		public override Type ReflectedType
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeType;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x0600211C RID: 8476 RVA: 0x000532FB File Offset: 0x000522FB
		public override int MetadataToken
		{
			get
			{
				return this.m_token;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x00053303 File Offset: 0x00052303
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x00053310 File Offset: 0x00052310
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedType, null, MemberTypes.Event);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00053334 File Offset: 0x00052334
		public override MethodInfo[] GetOtherMethods(bool nonPublic)
		{
			ArrayList arrayList = new ArrayList();
			if (this.m_otherMethod == null)
			{
				return new MethodInfo[0];
			}
			for (int i = 0; i < this.m_otherMethod.Length; i++)
			{
				if (Associates.IncludeAccessor(this.m_otherMethod[i], nonPublic))
				{
					arrayList.Add(this.m_otherMethod[i]);
				}
			}
			return arrayList.ToArray(typeof(MethodInfo)) as MethodInfo[];
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0005339D File Offset: 0x0005239D
		public override MethodInfo GetAddMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_addMethod, nonPublic))
			{
				return null;
			}
			return this.m_addMethod;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000533B5 File Offset: 0x000523B5
		public override MethodInfo GetRemoveMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_removeMethod, nonPublic))
			{
				return null;
			}
			return this.m_removeMethod;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000533CD File Offset: 0x000523CD
		public override MethodInfo GetRaiseMethod(bool nonPublic)
		{
			if (!Associates.IncludeAccessor(this.m_raiseMethod, nonPublic))
			{
				return null;
			}
			return this.m_raiseMethod;
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002123 RID: 8483 RVA: 0x000533E5 File Offset: 0x000523E5
		public override EventAttributes Attributes
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x04000DC9 RID: 3529
		private int m_token;

		// Token: 0x04000DCA RID: 3530
		private EventAttributes m_flags;

		// Token: 0x04000DCB RID: 3531
		private string m_name;

		// Token: 0x04000DCC RID: 3532
		private unsafe void* m_utf8name;

		// Token: 0x04000DCD RID: 3533
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000DCE RID: 3534
		private RuntimeMethodInfo m_addMethod;

		// Token: 0x04000DCF RID: 3535
		private RuntimeMethodInfo m_removeMethod;

		// Token: 0x04000DD0 RID: 3536
		private RuntimeMethodInfo m_raiseMethod;

		// Token: 0x04000DD1 RID: 3537
		private MethodInfo[] m_otherMethod;

		// Token: 0x04000DD2 RID: 3538
		private RuntimeType m_declaringType;

		// Token: 0x04000DD3 RID: 3539
		private BindingFlags m_bindingFlags;
	}
}
