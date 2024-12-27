using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Reflection
{
	// Token: 0x02000337 RID: 823
	[Serializable]
	internal sealed class RuntimeMethodInfo : MethodInfo, ISerializable
	{
		// Token: 0x06002061 RID: 8289 RVA: 0x000507D8 File Offset: 0x0004F7D8
		internal static string ConstructParameters(ParameterInfo[] parameters, CallingConventions callingConvention)
		{
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			return RuntimeMethodInfo.ConstructParameters(array, callingConvention);
		}

		// Token: 0x06002062 RID: 8290 RVA: 0x00050810 File Offset: 0x0004F810
		internal static string ConstructParameters(Type[] parameters, CallingConventions callingConvention)
		{
			string text = "";
			string text2 = "";
			foreach (Type type in parameters)
			{
				text += text2;
				text += type.SigToString();
				if (type.IsByRef)
				{
					text = text.TrimEnd(new char[] { '&' });
					text += " ByRef";
				}
				text2 = ", ";
			}
			if ((callingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				text += text2;
				text += "...";
			}
			return text;
		}

		// Token: 0x06002063 RID: 8291 RVA: 0x0005089C File Offset: 0x0004F89C
		internal static string ConstructName(MethodBase mi)
		{
			string text = null;
			text += mi.Name;
			RuntimeMethodInfo runtimeMethodInfo = mi as RuntimeMethodInfo;
			if (runtimeMethodInfo != null && runtimeMethodInfo.IsGenericMethod)
			{
				text += runtimeMethodInfo.m_handle.ConstructInstantiation();
			}
			return text + "(" + RuntimeMethodInfo.ConstructParameters(mi.GetParametersNoCopy(), mi.CallingConvention) + ")";
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000508FF File Offset: 0x0004F8FF
		internal RuntimeMethodInfo()
		{
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x00050908 File Offset: 0x0004F908
		internal RuntimeMethodInfo(RuntimeMethodHandle handle, RuntimeTypeHandle declaringTypeHandle, RuntimeType.RuntimeTypeCache reflectedTypeCache, MethodAttributes methodAttributes, BindingFlags bindingFlags)
		{
			this.m_toString = null;
			this.m_bindingFlags = bindingFlags;
			this.m_handle = handle;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_parameters = null;
			this.m_methodAttributes = methodAttributes;
			this.m_declaringType = declaringTypeHandle.GetRuntimeType();
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06002066 RID: 8294 RVA: 0x00050954 File Offset: 0x0004F954
		private RuntimeTypeHandle ReflectedTypeHandle
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeTypeHandle;
			}
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x00050961 File Offset: 0x0004F961
		internal ParameterInfo[] FetchNonReturnParameters()
		{
			if (this.m_parameters == null)
			{
				this.m_parameters = ParameterInfo.GetParameters(this, this, this.Signature);
			}
			return this.m_parameters;
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x00050984 File Offset: 0x0004F984
		internal ParameterInfo FetchReturnParameter()
		{
			if (this.m_returnParameter == null)
			{
				this.m_returnParameter = ParameterInfo.GetReturnParameter(this, this, this.Signature);
			}
			return this.m_returnParameter;
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000509A8 File Offset: 0x0004F9A8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeMethodInfo runtimeMethodInfo = o as RuntimeMethodInfo;
			return runtimeMethodInfo != null && runtimeMethodInfo.m_handle.Equals(this.m_handle);
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000509D2 File Offset: 0x0004F9D2
		internal Signature Signature
		{
			get
			{
				if (this.m_signature == null)
				{
					this.m_signature = new Signature(this.m_handle, this.m_declaringType.GetTypeHandleInternal());
				}
				return this.m_signature;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x0600206B RID: 8299 RVA: 0x000509FE File Offset: 0x0004F9FE
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x0600206C RID: 8300 RVA: 0x00050A06 File Offset: 0x0004FA06
		internal override RuntimeMethodHandle GetMethodHandle()
		{
			return this.m_handle;
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x00050A10 File Offset: 0x0004FA10
		internal override MethodInfo GetParentDefinition()
		{
			if (!base.IsVirtual || this.m_declaringType.IsInterface)
			{
				return null;
			}
			Type baseType = this.m_declaringType.BaseType;
			if (baseType == null)
			{
				return null;
			}
			int slot = this.m_handle.GetSlot();
			if (baseType.GetTypeHandleInternal().GetNumVirtuals() <= slot)
			{
				return null;
			}
			return (MethodInfo)RuntimeType.GetMethodBase(baseType.GetTypeHandleInternal(), baseType.GetTypeHandleInternal().GetMethodAt(slot));
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x00050A84 File Offset: 0x0004FA84
		internal override uint GetOneTimeFlags()
		{
			uint num = 0U;
			if (this.ReturnType.IsByRef)
			{
				num = 2U;
			}
			return num | base.GetOneTimeFlags();
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x00050AAC File Offset: 0x0004FAAC
		public override string ToString()
		{
			if (this.m_toString == null)
			{
				this.m_toString = this.ReturnType.SigToString() + " " + RuntimeMethodInfo.ConstructName(this);
			}
			return this.m_toString;
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x00050AE0 File Offset: 0x0004FAE0
		public override int GetHashCode()
		{
			return this.GetMethodHandle().GetHashCode();
		}

		// Token: 0x06002071 RID: 8305 RVA: 0x00050B04 File Offset: 0x0004FB04
		public override bool Equals(object obj)
		{
			if (!this.IsGenericMethod)
			{
				return obj == this;
			}
			RuntimeMethodInfo runtimeMethodInfo = obj as RuntimeMethodInfo;
			RuntimeMethodHandle runtimeMethodHandle = this.GetMethodHandle().StripMethodInstantiation();
			RuntimeMethodHandle runtimeMethodHandle2 = runtimeMethodInfo.GetMethodHandle().StripMethodInstantiation();
			if (runtimeMethodHandle != runtimeMethodHandle2)
			{
				return false;
			}
			if (runtimeMethodInfo == null || !runtimeMethodInfo.IsGenericMethod)
			{
				return false;
			}
			Type[] genericArguments = this.GetGenericArguments();
			Type[] genericArguments2 = runtimeMethodInfo.GetGenericArguments();
			if (genericArguments.Length != genericArguments2.Length)
			{
				return false;
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (genericArguments[i] != genericArguments2[i])
				{
					return false;
				}
			}
			if (runtimeMethodInfo.IsGenericMethod)
			{
				if (this.DeclaringType != runtimeMethodInfo.DeclaringType)
				{
					return false;
				}
				if (this.ReflectedType != runtimeMethodInfo.ReflectedType)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x00050BBF File Offset: 0x0004FBBF
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType, inherit);
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x00050BD8 File Offset: 0x0004FBD8
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
			return CustomAttribute.GetCustomAttributes(this, runtimeType, inherit);
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x00050C20 File Offset: 0x0004FC20
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
			return CustomAttribute.IsDefined(this, runtimeType, inherit);
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x00050C67 File Offset: 0x0004FC67
		public override string Name
		{
			get
			{
				if (this.m_name == null)
				{
					this.m_name = this.m_handle.GetName();
				}
				return this.m_name;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x00050C88 File Offset: 0x0004FC88
		public override Type DeclaringType
		{
			get
			{
				if (this.m_reflectedTypeCache.IsGlobal)
				{
					return null;
				}
				return this.m_declaringType;
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00050C9F File Offset: 0x0004FC9F
		public override Type ReflectedType
		{
			get
			{
				if (this.m_reflectedTypeCache.IsGlobal)
				{
					return null;
				}
				return this.m_reflectedTypeCache.RuntimeType;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x00050CBB File Offset: 0x0004FCBB
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00050CBE File Offset: 0x0004FCBE
		public override int MetadataToken
		{
			get
			{
				return this.m_handle.GetMethodDef();
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00050CCB File Offset: 0x0004FCCB
		public override Module Module
		{
			get
			{
				return this.m_declaringType.Module;
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00050CD8 File Offset: 0x0004FCD8
		internal override ParameterInfo[] GetParametersNoCopy()
		{
			this.FetchNonReturnParameters();
			return this.m_parameters;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00050CE8 File Offset: 0x0004FCE8
		public override ParameterInfo[] GetParameters()
		{
			this.FetchNonReturnParameters();
			if (this.m_parameters.Length == 0)
			{
				return this.m_parameters;
			}
			ParameterInfo[] array = new ParameterInfo[this.m_parameters.Length];
			Array.Copy(this.m_parameters, array, this.m_parameters.Length);
			return array;
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00050D30 File Offset: 0x0004FD30
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_handle.GetImplAttributes();
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x00050D3D File Offset: 0x0004FD3D
		internal override bool IsOverloaded
		{
			get
			{
				return this.m_reflectedTypeCache.GetMethodList(MemberListType.CaseSensitive, this.Name).Count > 1;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600207F RID: 8319 RVA: 0x00050D5C File Offset: 0x0004FD5C
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				Type declaringType = this.DeclaringType;
				if ((declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInReflectionOnly"));
				}
				return this.m_handle;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x00050DA3 File Offset: 0x0004FDA3
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_methodAttributes;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002081 RID: 8321 RVA: 0x00050DAB File Offset: 0x0004FDAB
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.Signature.CallingConvention;
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00050DB8 File Offset: 0x0004FDB8
		[ReflectionPermission(SecurityAction.Demand, Flags = ReflectionPermissionFlag.MemberAccess)]
		public override MethodBody GetMethodBody()
		{
			MethodBody methodBody = this.m_handle.GetMethodBody(this.ReflectedTypeHandle);
			if (methodBody != null)
			{
				methodBody.m_methodBase = this;
			}
			return methodBody;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00050DE2 File Offset: 0x0004FDE2
		private void CheckConsistency(object target)
		{
			if ((this.m_methodAttributes & MethodAttributes.Static) == MethodAttributes.Static || this.m_declaringType.IsInstanceOfType(target))
			{
				return;
			}
			if (target == null)
			{
				throw new TargetException(Environment.GetResourceString("RFLCT.Targ_StatMethReqTarg"));
			}
			throw new TargetException(Environment.GetResourceString("RFLCT.Targ_ITargMismatch"));
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00050E24 File Offset: 0x0004FE24
		private void ThrowNoInvokeException()
		{
			Type declaringType = this.DeclaringType;
			if ((declaringType == null && this.Module.Assembly.ReflectionOnly) || declaringType is ReflectionOnlyType)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyInvoke"));
			}
			if (this.DeclaringType.GetRootElementType() == typeof(ArgIterator))
			{
				throw new NotSupportedException();
			}
			if ((this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				throw new NotSupportedException();
			}
			if (this.DeclaringType.ContainsGenericParameters || this.ContainsGenericParameters)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_UnboundGenParam"));
			}
			if (base.IsAbstract)
			{
				throw new MemberAccessException();
			}
			if (this.ReturnType.IsByRef)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_ByRefReturn"));
			}
			throw new TargetException();
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00050EE8 File Offset: 0x0004FEE8
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(obj, invokeAttr, binder, parameters, culture, false);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00050EF8 File Offset: 0x0004FEF8
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture, bool skipVisibilityChecks)
		{
			int num = this.Signature.Arguments.Length;
			int num2 = ((parameters != null) ? parameters.Length : 0);
			if ((this.m_invocationFlags & 1U) == 0U)
			{
				this.m_invocationFlags = this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				this.ThrowNoInvokeException();
			}
			this.CheckConsistency(obj);
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			if (num2 > 65535)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("NotSupported_TooManyArgs"));
			}
			if (!skipVisibilityChecks && (this.m_invocationFlags & 36U) != 0U)
			{
				if ((this.m_invocationFlags & 32U) != 0U)
				{
					CodeAccessPermission.DemandInternal(PermissionType.ReflectionMemberAccess);
				}
				if ((this.m_invocationFlags & 4U) != 0U)
				{
					MethodBase.PerformSecurityCheck(obj, this.m_handle, this.m_declaringType.TypeHandle.Value, this.m_invocationFlags);
				}
			}
			RuntimeTypeHandle runtimeTypeHandle = RuntimeTypeHandle.EmptyHandle;
			if (!this.m_reflectedTypeCache.IsGlobal)
			{
				runtimeTypeHandle = this.m_declaringType.TypeHandle;
			}
			if (num2 == 0)
			{
				return this.m_handle.InvokeMethodFast(obj, null, this.Signature, this.m_methodAttributes, runtimeTypeHandle);
			}
			object[] array = base.CheckArguments(parameters, binder, invokeAttr, culture, this.Signature);
			object obj2 = this.m_handle.InvokeMethodFast(obj, array, this.Signature, this.m_methodAttributes, runtimeTypeHandle);
			for (int i = 0; i < num2; i++)
			{
				parameters[i] = array[i];
			}
			return obj2;
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x00051050 File Offset: 0x00050050
		public override Type ReturnType
		{
			get
			{
				return this.Signature.ReturnTypeHandle.GetRuntimeType();
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002088 RID: 8328 RVA: 0x00051070 File Offset: 0x00050070
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.ReturnParameter;
			}
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x00051078 File Offset: 0x00050078
		public override ParameterInfo ReturnParameter
		{
			get
			{
				this.FetchReturnParameter();
				return this.m_returnParameter;
			}
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x00051088 File Offset: 0x00050088
		public override MethodInfo GetBaseDefinition()
		{
			if (!base.IsVirtual || base.IsStatic || this.m_declaringType == null || this.m_declaringType.IsInterface)
			{
				return this;
			}
			int slot = this.m_handle.GetSlot();
			Type type = this.DeclaringType;
			Type type2 = this.DeclaringType;
			RuntimeMethodHandle runtimeMethodHandle = default(RuntimeMethodHandle);
			do
			{
				RuntimeTypeHandle typeHandleInternal = type.GetTypeHandleInternal();
				int numVirtuals = typeHandleInternal.GetNumVirtuals();
				if (numVirtuals <= slot)
				{
					break;
				}
				runtimeMethodHandle = typeHandleInternal.GetMethodAt(slot);
				type2 = type;
				type = type.BaseType;
			}
			while (type != null);
			return (MethodInfo)RuntimeType.GetMethodBase(type2.GetTypeHandleInternal(), runtimeMethodHandle);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0005111C File Offset: 0x0005011C
		public override MethodInfo MakeGenericMethod(params Type[] methodInstantiation)
		{
			if (methodInstantiation == null)
			{
				throw new ArgumentNullException("methodInstantiation");
			}
			Type[] array = new Type[methodInstantiation.Length];
			for (int i = 0; i < methodInstantiation.Length; i++)
			{
				array[i] = methodInstantiation[i];
			}
			methodInstantiation = array;
			if (!this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_NotGenericMethodDefinition"), new object[] { this }));
			}
			for (int j = 0; j < methodInstantiation.Length; j++)
			{
				if (methodInstantiation[j] == null)
				{
					throw new ArgumentNullException();
				}
				if (!(methodInstantiation[j] is RuntimeType))
				{
					return MethodBuilderInstantiation.MakeGenericMethod(this, methodInstantiation);
				}
			}
			Type[] genericArguments = this.GetGenericArguments();
			RuntimeType.SanityCheckGenericArguments(methodInstantiation, genericArguments);
			RuntimeTypeHandle[] array2 = new RuntimeTypeHandle[methodInstantiation.Length];
			for (int k = 0; k < methodInstantiation.Length; k++)
			{
				array2[k] = methodInstantiation[k].GetTypeHandleInternal();
			}
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = RuntimeType.GetMethodBase(this.m_reflectedTypeCache.RuntimeTypeHandle, this.m_handle.GetInstantiatingStub(this.m_declaringType.GetTypeHandleInternal(), array2)) as MethodInfo;
			}
			catch (VerificationException ex)
			{
				RuntimeType.ValidateGenericArguments(this, methodInstantiation, ex);
				throw ex;
			}
			return methodInfo;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x00051248 File Offset: 0x00050248
		public override Type[] GetGenericArguments()
		{
			RuntimeTypeHandle[] methodInstantiation = this.m_handle.GetMethodInstantiation();
			RuntimeType[] array;
			if (methodInstantiation != null)
			{
				array = new RuntimeType[methodInstantiation.Length];
				for (int i = 0; i < methodInstantiation.Length; i++)
				{
					array[i] = methodInstantiation[i].GetRuntimeType();
				}
			}
			else
			{
				array = new RuntimeType[0];
			}
			return array;
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00051296 File Offset: 0x00050296
		public override MethodInfo GetGenericMethodDefinition()
		{
			if (!this.IsGenericMethod)
			{
				throw new InvalidOperationException();
			}
			return RuntimeType.GetMethodBase(this.m_declaringType.GetTypeHandleInternal(), this.m_handle.StripMethodInstantiation()) as MethodInfo;
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x000512C6 File Offset: 0x000502C6
		public override bool IsGenericMethod
		{
			get
			{
				return this.m_handle.HasMethodInstantiation();
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x0600208F RID: 8335 RVA: 0x000512D3 File Offset: 0x000502D3
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.m_handle.IsGenericMethodDefinition();
			}
		}

		// Token: 0x170005A6 RID: 1446
		// (get) Token: 0x06002090 RID: 8336 RVA: 0x000512E0 File Offset: 0x000502E0
		public override bool ContainsGenericParameters
		{
			get
			{
				if (this.DeclaringType != null && this.DeclaringType.ContainsGenericParameters)
				{
					return true;
				}
				if (!this.IsGenericMethod)
				{
					return false;
				}
				Type[] genericArguments = this.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (genericArguments[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x00051330 File Offset: 0x00050330
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.m_reflectedTypeCache.IsGlobal)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GlobalMethodSerialization"));
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedTypeHandle.GetRuntimeType(), this.ToString(), MemberTypes.Method, (this.IsGenericMethod & !this.IsGenericMethodDefinition) ? this.GetGenericArguments() : null);
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x000513A4 File Offset: 0x000503A4
		internal static MethodBase InternalGetCurrentMethod(ref StackCrawlMark stackMark)
		{
			RuntimeMethodHandle runtimeMethodHandle = RuntimeMethodHandle.GetCurrentMethod(ref stackMark);
			if (runtimeMethodHandle.IsNullHandle())
			{
				return null;
			}
			runtimeMethodHandle = runtimeMethodHandle.GetTypicalMethodDefinition();
			return RuntimeType.GetMethodBase(runtimeMethodHandle);
		}

		// Token: 0x04000D99 RID: 3481
		private RuntimeMethodHandle m_handle;

		// Token: 0x04000D9A RID: 3482
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000D9B RID: 3483
		private string m_name;

		// Token: 0x04000D9C RID: 3484
		private string m_toString;

		// Token: 0x04000D9D RID: 3485
		private ParameterInfo[] m_parameters;

		// Token: 0x04000D9E RID: 3486
		private ParameterInfo m_returnParameter;

		// Token: 0x04000D9F RID: 3487
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DA0 RID: 3488
		private MethodAttributes m_methodAttributes;

		// Token: 0x04000DA1 RID: 3489
		private Signature m_signature;

		// Token: 0x04000DA2 RID: 3490
		private RuntimeType m_declaringType;

		// Token: 0x04000DA3 RID: 3491
		private uint m_invocationFlags;
	}
}
