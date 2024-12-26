using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x020000E8 RID: 232
	public abstract class MethodInvoker
	{
		// Token: 0x06000A5A RID: 2650
		[DebuggerHidden]
		[DebuggerStepThrough]
		public abstract object Invoke(object thisob, object[] parameters);

		// Token: 0x06000A5B RID: 2651 RVA: 0x0004ED44 File Offset: 0x0004DD44
		private static bool DoesCallerRequireFullTrust(MethodInfo method)
		{
			Assembly assembly = method.DeclaringType.Assembly;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			byte[] publicKey = assembly.GetName().GetPublicKey();
			return publicKey != null && publicKey.Length != 0 && CustomAttribute.GetCustomAttributes(assembly, typeof(AllowPartiallyTrustedCallersAttribute), true).Length == 0;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0004EDA0 File Offset: 0x0004DDA0
		internal static MethodInvoker GetInvokerFor(MethodInfo method)
		{
			if (method.DeclaringType == typeof(CodeAccessPermission) && (method.Name == "Deny" || method.Name == "Assert" || method.Name == "PermitOnly"))
			{
				throw new JScriptException(JSError.CannotCallSecurityMethodLateBound);
			}
			MethodInvoker methodInvoker = MethodInvoker.invokerFor[method] as MethodInvoker;
			if (methodInvoker != null)
			{
				return methodInvoker;
			}
			if (!MethodInvoker.SafeToCall(method))
			{
				return null;
			}
			bool flag = MethodInvoker.DoesCallerRequireFullTrust(method);
			lock (MethodInvoker.invokerFor)
			{
				methodInvoker = MethodInvoker.invokerFor[method] as MethodInvoker;
				if (methodInvoker != null)
				{
					return methodInvoker;
				}
				methodInvoker = MethodInvoker.SpitAndInstantiateClassFor(method, flag);
				MethodInvoker.invokerFor[method] = methodInvoker;
			}
			return methodInvoker;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x0004EE7C File Offset: 0x0004DE7C
		private static bool SafeToCall(MethodInfo meth)
		{
			Type declaringType = meth.DeclaringType;
			return declaringType != null && declaringType != typeof(Activator) && declaringType != typeof(AppDomain) && declaringType != typeof(IsolatedStorageFile) && declaringType != typeof(MethodRental) && declaringType != typeof(TypeLibConverter) && declaringType != typeof(SecurityManager) && !typeof(Assembly).IsAssignableFrom(declaringType) && !typeof(MemberInfo).IsAssignableFrom(declaringType) && !typeof(ResourceManager).IsAssignableFrom(declaringType) && !typeof(Delegate).IsAssignableFrom(declaringType) && (declaringType.Attributes & TypeAttributes.HasSecurity) == TypeAttributes.NotPublic && (meth.Attributes & MethodAttributes.HasSecurity) == MethodAttributes.PrivateScope && (meth.Attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PrivateScope;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x0004EF68 File Offset: 0x0004DF68
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private static MethodInvoker SpitAndInstantiateClassFor(MethodInfo method, bool requiresDemand)
		{
			TypeBuilder typeBuilder = Runtime.ThunkModuleBuilder.DefineType("invoker" + MethodInvoker.count++, TypeAttributes.Public, typeof(MethodInvoker));
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(object), new Type[]
			{
				typeof(object),
				typeof(object[])
			});
			if (requiresDemand)
			{
				methodBuilder.AddDeclarativeSecurity(SecurityAction.Demand, new NamedPermissionSet("FullTrust"));
			}
			methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerStepThroughAttributeCtor, new object[0]));
			methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(Runtime.TypeRefs.debuggerHiddenAttributeCtor, new object[0]));
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			if (!method.DeclaringType.IsPublic)
			{
				method = method.GetBaseDefinition();
			}
			Type declaringType = method.DeclaringType;
			if (!method.IsStatic)
			{
				ilgenerator.Emit(OpCodes.Ldarg_1);
				if (declaringType.IsValueType)
				{
					Convert.EmitUnbox(ilgenerator, declaringType, Type.GetTypeCode(declaringType));
					Convert.EmitLdloca(ilgenerator, declaringType);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Castclass, declaringType);
				}
			}
			ParameterInfo[] parameters = method.GetParameters();
			LocalBuilder[] array = null;
			int i = 0;
			int num = parameters.Length;
			while (i < num)
			{
				ilgenerator.Emit(OpCodes.Ldarg_2);
				ConstantWrapper.TranslateToILInt(ilgenerator, i);
				Type type = parameters[i].ParameterType;
				if (type.IsByRef)
				{
					type = type.GetElementType();
					if (array == null)
					{
						array = new LocalBuilder[num];
					}
					array[i] = ilgenerator.DeclareLocal(type);
					ilgenerator.Emit(OpCodes.Ldelem_Ref);
					if (type.IsValueType)
					{
						Convert.EmitUnbox(ilgenerator, type, Type.GetTypeCode(type));
					}
					ilgenerator.Emit(OpCodes.Stloc, array[i]);
					ilgenerator.Emit(OpCodes.Ldloca, array[i]);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldelem_Ref);
					if (type.IsValueType)
					{
						Convert.EmitUnbox(ilgenerator, type, Type.GetTypeCode(type));
					}
				}
				i++;
			}
			if (!method.IsStatic && method.IsVirtual && !method.IsFinal && (!declaringType.IsSealed || !declaringType.IsValueType))
			{
				ilgenerator.Emit(OpCodes.Callvirt, method);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Call, method);
			}
			Type returnType = method.ReturnType;
			if (returnType == typeof(void))
			{
				ilgenerator.Emit(OpCodes.Ldnull);
			}
			else if (returnType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, returnType);
			}
			if (array != null)
			{
				int j = 0;
				int num2 = parameters.Length;
				while (j < num2)
				{
					LocalBuilder localBuilder = array[j];
					if (localBuilder != null)
					{
						ilgenerator.Emit(OpCodes.Ldarg_2);
						ConstantWrapper.TranslateToILInt(ilgenerator, j);
						ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
						Type elementType = parameters[j].ParameterType.GetElementType();
						if (elementType.IsValueType)
						{
							ilgenerator.Emit(OpCodes.Box, elementType);
						}
						ilgenerator.Emit(OpCodes.Stelem_Ref);
					}
					j++;
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			Type type2 = typeBuilder.CreateType();
			return (MethodInvoker)Activator.CreateInstance(type2);
		}

		// Token: 0x0400066B RID: 1643
		private static SimpleHashtable invokerFor = new SimpleHashtable(64U);

		// Token: 0x0400066C RID: 1644
		private static int count = 0;
	}
}
