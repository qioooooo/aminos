using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000338 RID: 824
	[Serializable]
	internal sealed class RuntimeConstructorInfo : ConstructorInfo, ISerializable
	{
		// Token: 0x06002093 RID: 8339 RVA: 0x000513D1 File Offset: 0x000503D1
		internal RuntimeConstructorInfo()
		{
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000513DC File Offset: 0x000503DC
		internal RuntimeConstructorInfo(RuntimeMethodHandle handle, RuntimeTypeHandle declaringTypeHandle, RuntimeType.RuntimeTypeCache reflectedTypeCache, MethodAttributes methodAttributes, BindingFlags bindingFlags)
		{
			this.m_bindingFlags = bindingFlags;
			this.m_handle = handle;
			this.m_reflectedTypeCache = reflectedTypeCache;
			this.m_declaringType = declaringTypeHandle.GetRuntimeType();
			this.m_parameters = null;
			this.m_toString = null;
			this.m_methodAttributes = methodAttributes;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00051428 File Offset: 0x00050428
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal override bool CacheEquals(object o)
		{
			RuntimeConstructorInfo runtimeConstructorInfo = o as RuntimeConstructorInfo;
			return runtimeConstructorInfo != null && runtimeConstructorInfo.m_handle.Equals(this.m_handle);
		}

		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x00051452 File Offset: 0x00050452
		private Signature Signature
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

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x0005147E File Offset: 0x0005047E
		private RuntimeTypeHandle ReflectedTypeHandle
		{
			get
			{
				return this.m_reflectedTypeCache.RuntimeTypeHandle;
			}
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x0005148B File Offset: 0x0005048B
		private void CheckConsistency(object target)
		{
			if (target == null && base.IsStatic)
			{
				return;
			}
			if (this.m_declaringType.IsInstanceOfType(target))
			{
				return;
			}
			if (target == null)
			{
				throw new TargetException(Environment.GetResourceString("RFLCT.Targ_StatMethReqTarg"));
			}
			throw new TargetException(Environment.GetResourceString("RFLCT.Targ_ITargMismatch"));
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x06002099 RID: 8345 RVA: 0x000514CA File Offset: 0x000504CA
		internal BindingFlags BindingFlags
		{
			get
			{
				return this.m_bindingFlags;
			}
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000514D2 File Offset: 0x000504D2
		internal override RuntimeMethodHandle GetMethodHandle()
		{
			return this.m_handle;
		}

		// Token: 0x170005AA RID: 1450
		// (get) Token: 0x0600209B RID: 8347 RVA: 0x000514DA File Offset: 0x000504DA
		internal override bool IsOverloaded
		{
			get
			{
				return this.m_reflectedTypeCache.GetConstructorList(MemberListType.CaseSensitive, this.Name).Count > 1;
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000514F8 File Offset: 0x000504F8
		internal override uint GetOneTimeSpecificFlags()
		{
			uint num = 16U;
			if ((this.DeclaringType != null && this.DeclaringType.IsAbstract) || base.IsStatic)
			{
				num |= 8U;
			}
			else if (this.DeclaringType == typeof(void))
			{
				num |= 2U;
			}
			else if (typeof(Delegate).IsAssignableFrom(this.DeclaringType))
			{
				num |= 128U;
			}
			return num;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00051563 File Offset: 0x00050563
		public override string ToString()
		{
			if (this.m_toString == null)
			{
				this.m_toString = "Void " + RuntimeMethodInfo.ConstructName(this);
			}
			return this.m_toString;
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x00051589 File Offset: 0x00050589
		public override object[] GetCustomAttributes(bool inherit)
		{
			return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000515A0 File Offset: 0x000505A0
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

		// Token: 0x060020A0 RID: 8352 RVA: 0x000515E8 File Offset: 0x000505E8
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

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x0005162E File Offset: 0x0005062E
		public override string Name
		{
			get
			{
				return this.m_handle.GetName();
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x0005163B File Offset: 0x0005063B
		[ComVisible(true)]
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x060020A3 RID: 8355 RVA: 0x0005163E File Offset: 0x0005063E
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

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x060020A4 RID: 8356 RVA: 0x00051655 File Offset: 0x00050655
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

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x00051671 File Offset: 0x00050671
		public override int MetadataToken
		{
			get
			{
				return this.m_handle.GetMethodDef();
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x00051680 File Offset: 0x00050680
		public override Module Module
		{
			get
			{
				return this.m_declaringType.GetTypeHandleInternal().GetModuleHandle().GetModule();
			}
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000516A8 File Offset: 0x000506A8
		internal override Type GetReturnType()
		{
			return this.Signature.ReturnTypeHandle.GetRuntimeType();
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000516C8 File Offset: 0x000506C8
		internal override ParameterInfo[] GetParametersNoCopy()
		{
			if (this.m_parameters == null)
			{
				this.m_parameters = ParameterInfo.GetParameters(this, this, this.Signature);
			}
			return this.m_parameters;
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000516EC File Offset: 0x000506EC
		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] parametersNoCopy = this.GetParametersNoCopy();
			if (parametersNoCopy.Length == 0)
			{
				return parametersNoCopy;
			}
			ParameterInfo[] array = new ParameterInfo[parametersNoCopy.Length];
			Array.Copy(parametersNoCopy, array, parametersNoCopy.Length);
			return array;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x0005171B File Offset: 0x0005071B
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_handle.GetImplAttributes();
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x00051728 File Offset: 0x00050728
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

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x0005176F File Offset: 0x0005076F
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_methodAttributes;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x060020AD RID: 8365 RVA: 0x00051777 File Offset: 0x00050777
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.Signature.CallingConvention;
			}
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00051784 File Offset: 0x00050784
		internal static void CheckCanCreateInstance(Type declaringType, bool isVarArg)
		{
			if (declaringType == null)
			{
				throw new ArgumentNullException("declaringType");
			}
			if (declaringType is ReflectionOnlyType)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyInvoke"));
			}
			if (declaringType.IsInterface)
			{
				throw new MemberAccessException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Acc_CreateInterfaceEx"), new object[] { declaringType }));
			}
			if (declaringType.IsAbstract)
			{
				throw new MemberAccessException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Acc_CreateAbstEx"), new object[] { declaringType }));
			}
			if (declaringType.GetRootElementType() == typeof(ArgIterator))
			{
				throw new NotSupportedException();
			}
			if (isVarArg)
			{
				throw new NotSupportedException();
			}
			if (declaringType.ContainsGenericParameters)
			{
				throw new MemberAccessException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Acc_CreateGenericEx"), new object[] { declaringType }));
			}
			if (declaringType == typeof(void))
			{
				throw new MemberAccessException(Environment.GetResourceString("Access_Void"));
			}
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x0005187F File Offset: 0x0005087F
		internal void ThrowNoInvokeException()
		{
			RuntimeConstructorInfo.CheckCanCreateInstance(this.DeclaringType, (this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs);
			if ((this.Attributes & MethodAttributes.Static) == MethodAttributes.Static)
			{
				throw new MemberAccessException(Environment.GetResourceString("Acc_NotClassInit"));
			}
			throw new TargetException();
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000518BC File Offset: 0x000508BC
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			if (this.m_invocationFlags == 0U)
			{
				this.m_invocationFlags = this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 2U) != 0U)
			{
				this.ThrowNoInvokeException();
			}
			this.CheckConsistency(obj);
			if (obj != null)
			{
				new SecurityPermission(SecurityPermissionFlag.SkipVerification).Demand();
			}
			if ((this.m_invocationFlags & 36U) != 0U)
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
			int num = this.Signature.Arguments.Length;
			int num2 = ((parameters != null) ? parameters.Length : 0);
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			if (num2 > 0)
			{
				object[] array = base.CheckArguments(parameters, binder, invokeAttr, culture, this.Signature);
				object obj2 = this.m_handle.InvokeMethodFast(obj, array, this.Signature, this.m_methodAttributes, (this.ReflectedType != null) ? this.ReflectedType.TypeHandle : RuntimeTypeHandle.EmptyHandle);
				for (int i = 0; i < num2; i++)
				{
					parameters[i] = array[i];
				}
				return obj2;
			}
			return this.m_handle.InvokeMethodFast(obj, null, this.Signature, this.m_methodAttributes, (this.DeclaringType != null) ? this.DeclaringType.TypeHandle : RuntimeTypeHandle.EmptyHandle);
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00051A18 File Offset: 0x00050A18
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

		// Token: 0x060020B2 RID: 8370 RVA: 0x00051A44 File Offset: 0x00050A44
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			RuntimeTypeHandle typeHandle = this.m_declaringType.TypeHandle;
			if (this.m_invocationFlags == 0U)
			{
				this.m_invocationFlags = this.GetOneTimeFlags();
			}
			if ((this.m_invocationFlags & 266U) != 0U)
			{
				this.ThrowNoInvokeException();
			}
			if ((this.m_invocationFlags & 164U) != 0U)
			{
				if ((this.m_invocationFlags & 32U) != 0U)
				{
					CodeAccessPermission.DemandInternal(PermissionType.ReflectionMemberAccess);
				}
				if ((this.m_invocationFlags & 4U) != 0U)
				{
					MethodBase.PerformSecurityCheck(null, this.m_handle, this.m_declaringType.TypeHandle.Value, this.m_invocationFlags & 268435456U);
				}
				if ((this.m_invocationFlags & 128U) != 0U)
				{
					new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				}
			}
			int num = this.Signature.Arguments.Length;
			int num2 = ((parameters != null) ? parameters.Length : 0);
			if (num != num2)
			{
				throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
			}
			RuntimeHelpers.RunClassConstructor(typeHandle);
			if (num2 > 0)
			{
				object[] array = base.CheckArguments(parameters, binder, invokeAttr, culture, this.Signature);
				object obj = this.m_handle.InvokeConstructor(array, this.Signature, typeHandle);
				for (int i = 0; i < num2; i++)
				{
					parameters[i] = array[i];
				}
				return obj;
			}
			return this.m_handle.InvokeConstructor(null, this.Signature, typeHandle);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x00051B88 File Offset: 0x00050B88
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedTypeHandle.GetRuntimeType(), this.ToString(), MemberTypes.Constructor);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00051BC4 File Offset: 0x00050BC4
		internal void SerializationInvoke(object target, SerializationInfo info, StreamingContext context)
		{
			this.MethodHandle.SerializationInvoke(target, this.Signature, info, context);
		}

		// Token: 0x04000DA4 RID: 3492
		private RuntimeMethodHandle m_handle;

		// Token: 0x04000DA5 RID: 3493
		private RuntimeType.RuntimeTypeCache m_reflectedTypeCache;

		// Token: 0x04000DA6 RID: 3494
		private RuntimeType m_declaringType;

		// Token: 0x04000DA7 RID: 3495
		private string m_toString;

		// Token: 0x04000DA8 RID: 3496
		private MethodAttributes m_methodAttributes;

		// Token: 0x04000DA9 RID: 3497
		private BindingFlags m_bindingFlags;

		// Token: 0x04000DAA RID: 3498
		private ParameterInfo[] m_parameters;

		// Token: 0x04000DAB RID: 3499
		private uint m_invocationFlags;

		// Token: 0x04000DAC RID: 3500
		private Signature m_signature;
	}
}
