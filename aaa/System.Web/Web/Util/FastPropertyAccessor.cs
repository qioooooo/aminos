using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;
using System.Threading;
using System.Web.UI;

namespace System.Web.Util
{
	// Token: 0x02000763 RID: 1891
	internal class FastPropertyAccessor
	{
		// Token: 0x06005BED RID: 23533 RVA: 0x00170BDC File Offset: 0x0016FBDC
		static FastPropertyAccessor()
		{
			FastPropertyAccessor._interfacesToImplement[0] = typeof(IWebPropertyAccessor);
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x00170C84 File Offset: 0x0016FC84
		private static string GetUniqueCompilationName()
		{
			return Guid.NewGuid().ToString().Replace('-', '_');
		}

		// Token: 0x06005BEF RID: 23535 RVA: 0x00170CB0 File Offset: 0x0016FCB0
		[ReflectionPermission(SecurityAction.Assert, Flags = ReflectionPermissionFlag.ReflectionEmit)]
		private Type GetPropertyAccessorTypeWithAssert(Type type, string propertyName, PropertyInfo propInfo, FieldInfo fieldInfo)
		{
			MethodInfo methodInfo = null;
			MethodInfo methodInfo2 = null;
			Type type2;
			if (propInfo != null)
			{
				methodInfo = propInfo.GetGetMethod();
				methodInfo2 = propInfo.GetSetMethod();
				type2 = propInfo.PropertyType;
			}
			else
			{
				type2 = fieldInfo.FieldType;
			}
			if (this._dynamicModule == null)
			{
				lock (this)
				{
					if (this._dynamicModule == null)
					{
						string uniqueCompilationName = FastPropertyAccessor.GetUniqueCompilationName();
						AssemblyName assemblyName = new AssemblyName();
						assemblyName.Name = "A_" + uniqueCompilationName;
						AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run, null, null, null, null, null, true);
						this._dynamicModule = assemblyBuilder.DefineDynamicModule("M_" + uniqueCompilationName);
					}
				}
			}
			string text = string.Concat(new object[]
			{
				Util.MakeValidTypeNameFromString(type.Name),
				"_",
				propertyName,
				"_",
				FastPropertyAccessor._uniqueId++
			});
			TypeBuilder typeBuilder = this._dynamicModule.DefineType("T_" + text, TypeAttributes.Public, typeof(object), FastPropertyAccessor._interfacesToImplement);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetProperty", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(object), FastPropertyAccessor._getPropertyParameterList);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			if (methodInfo != null)
			{
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type);
				if (propInfo != null)
				{
					ilgenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
				}
				ilgenerator.Emit(OpCodes.Box, type2);
				ilgenerator.Emit(OpCodes.Ret);
				typeBuilder.DefineMethodOverride(methodBuilder, FastPropertyAccessor._getPropertyMethod);
			}
			else
			{
				ConstructorInfo constructor = typeof(InvalidOperationException).GetConstructor(Type.EmptyTypes);
				ilgenerator.Emit(OpCodes.Newobj, constructor);
				ilgenerator.Emit(OpCodes.Throw);
			}
			methodBuilder = typeBuilder.DefineMethod("SetProperty", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, null, FastPropertyAccessor._setPropertyParameterList);
			ilgenerator = methodBuilder.GetILGenerator();
			if (fieldInfo != null || methodInfo2 != null)
			{
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type);
				ilgenerator.Emit(OpCodes.Ldarg_2);
				if (type2.IsPrimitive)
				{
					ilgenerator.Emit(OpCodes.Unbox, type2);
					if (type2 == typeof(sbyte))
					{
						ilgenerator.Emit(OpCodes.Ldind_I1);
					}
					else if (type2 == typeof(byte))
					{
						ilgenerator.Emit(OpCodes.Ldind_U1);
					}
					else if (type2 == typeof(short))
					{
						ilgenerator.Emit(OpCodes.Ldind_I2);
					}
					else if (type2 == typeof(ushort))
					{
						ilgenerator.Emit(OpCodes.Ldind_U2);
					}
					else if (type2 == typeof(uint))
					{
						ilgenerator.Emit(OpCodes.Ldind_U4);
					}
					else if (type2 == typeof(int))
					{
						ilgenerator.Emit(OpCodes.Ldind_I4);
					}
					else if (type2 == typeof(long))
					{
						ilgenerator.Emit(OpCodes.Ldind_I8);
					}
					else if (type2 == typeof(ulong))
					{
						ilgenerator.Emit(OpCodes.Ldind_I8);
					}
					else if (type2 == typeof(bool))
					{
						ilgenerator.Emit(OpCodes.Ldind_I1);
					}
					else if (type2 == typeof(char))
					{
						ilgenerator.Emit(OpCodes.Ldind_U2);
					}
					else if (type2 == typeof(decimal))
					{
						ilgenerator.Emit(OpCodes.Ldobj, type2);
					}
					else if (type2 == typeof(float))
					{
						ilgenerator.Emit(OpCodes.Ldind_R4);
					}
					else if (type2 == typeof(double))
					{
						ilgenerator.Emit(OpCodes.Ldind_R8);
					}
					else
					{
						ilgenerator.Emit(OpCodes.Ldobj, type2);
					}
				}
				else if (type2.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Unbox, type2);
					ilgenerator.Emit(OpCodes.Ldobj, type2);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Castclass, type2);
				}
				if (propInfo != null)
				{
					ilgenerator.EmitCall(OpCodes.Callvirt, methodInfo2, null);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, FastPropertyAccessor._setPropertyMethod);
			return typeBuilder.CreateType();
		}

		// Token: 0x06005BF0 RID: 23536 RVA: 0x00171104 File Offset: 0x00170104
		private static void GetPropertyInfo(Type type, string propertyName, out PropertyInfo propInfo, out FieldInfo fieldInfo, out Type declaringType)
		{
			propInfo = FastPropertyAccessor.GetPropertyMostSpecific(type, propertyName);
			fieldInfo = null;
			if (propInfo != null)
			{
				MethodInfo methodInfo = propInfo.GetGetMethod();
				if (methodInfo == null)
				{
					methodInfo = propInfo.GetSetMethod();
				}
				declaringType = methodInfo.GetBaseDefinition().DeclaringType;
				if (declaringType.IsGenericType)
				{
					declaringType = type;
				}
				if (declaringType != type)
				{
					propInfo = declaringType.GetProperty(propertyName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
					return;
				}
			}
			else
			{
				fieldInfo = type.GetField(propertyName);
				if (fieldInfo == null)
				{
					throw new ArgumentException();
				}
				declaringType = fieldInfo.DeclaringType;
			}
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x00171184 File Offset: 0x00170184
		private static IWebPropertyAccessor GetPropertyAccessor(Type type, string propertyName)
		{
			if (FastPropertyAccessor.s_accessorGenerator == null || FastPropertyAccessor.s_accessorCache == null)
			{
				lock (FastPropertyAccessor.s_lockObject)
				{
					if (FastPropertyAccessor.s_accessorGenerator == null || FastPropertyAccessor.s_accessorCache == null)
					{
						FastPropertyAccessor.s_accessorGenerator = new FastPropertyAccessor();
						FastPropertyAccessor.s_accessorCache = new Hashtable();
					}
				}
			}
			int num = HashCodeCombiner.CombineHashCodes(type.GetHashCode(), propertyName.GetHashCode());
			IWebPropertyAccessor webPropertyAccessor = (IWebPropertyAccessor)FastPropertyAccessor.s_accessorCache[num];
			if (webPropertyAccessor != null)
			{
				return webPropertyAccessor;
			}
			FieldInfo fieldInfo = null;
			PropertyInfo propertyInfo = null;
			Type type2;
			FastPropertyAccessor.GetPropertyInfo(type, propertyName, out propertyInfo, out fieldInfo, out type2);
			int num2 = 0;
			if (type2 != type)
			{
				num2 = HashCodeCombiner.CombineHashCodes(type2.GetHashCode(), propertyName.GetHashCode());
				webPropertyAccessor = (IWebPropertyAccessor)FastPropertyAccessor.s_accessorCache[num2];
				if (webPropertyAccessor != null)
				{
					lock (FastPropertyAccessor.s_accessorCache.SyncRoot)
					{
						FastPropertyAccessor.s_accessorCache[num] = webPropertyAccessor;
					}
					return webPropertyAccessor;
				}
			}
			if (webPropertyAccessor == null)
			{
				Type propertyAccessorTypeWithAssert;
				lock (FastPropertyAccessor.s_accessorGenerator)
				{
					propertyAccessorTypeWithAssert = FastPropertyAccessor.s_accessorGenerator.GetPropertyAccessorTypeWithAssert(type2, propertyName, propertyInfo, fieldInfo);
				}
				webPropertyAccessor = (IWebPropertyAccessor)HttpRuntime.CreateNonPublicInstance(propertyAccessorTypeWithAssert);
			}
			lock (FastPropertyAccessor.s_accessorCache.SyncRoot)
			{
				FastPropertyAccessor.s_accessorCache[num] = webPropertyAccessor;
				if (num2 != 0)
				{
					FastPropertyAccessor.s_accessorCache[num2] = webPropertyAccessor;
				}
			}
			return webPropertyAccessor;
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x0017132C File Offset: 0x0017032C
		internal static object GetProperty(object target, string propName, bool inDesigner)
		{
			if (!inDesigner)
			{
				IWebPropertyAccessor propertyAccessor = FastPropertyAccessor.GetPropertyAccessor(target.GetType(), propName);
				return propertyAccessor.GetProperty(target);
			}
			FieldInfo fieldInfo = null;
			PropertyInfo propertyInfo = null;
			Type type;
			FastPropertyAccessor.GetPropertyInfo(target.GetType(), propName, out propertyInfo, out fieldInfo, out type);
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(target, null);
			}
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(target);
			}
			throw new ArgumentException();
		}

		// Token: 0x06005BF3 RID: 23539 RVA: 0x00171384 File Offset: 0x00170384
		private static PropertyInfo GetPropertyMostSpecific(Type type, string name)
		{
			for (Type type2 = type; type2 != null; type2 = type2.BaseType)
			{
				PropertyInfo property = type2.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				if (property != null)
				{
					return property;
				}
			}
			return null;
		}

		// Token: 0x06005BF4 RID: 23540 RVA: 0x001713B0 File Offset: 0x001703B0
		internal static void SetProperty(object target, string propName, object val, bool inDesigner)
		{
			if (!inDesigner)
			{
				IWebPropertyAccessor propertyAccessor = FastPropertyAccessor.GetPropertyAccessor(target.GetType(), propName);
				propertyAccessor.SetProperty(target, val);
				return;
			}
			FieldInfo fieldInfo = null;
			PropertyInfo propertyInfo = null;
			Type type = null;
			FastPropertyAccessor.GetPropertyInfo(target.GetType(), propName, out propertyInfo, out fieldInfo, out type);
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(target, val, null);
				return;
			}
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(target, val);
				return;
			}
			throw new ArgumentException();
		}

		// Token: 0x0400312D RID: 12589
		private const BindingFlags _declaredFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

		// Token: 0x0400312E RID: 12590
		private static object s_lockObject = new object();

		// Token: 0x0400312F RID: 12591
		private static FastPropertyAccessor s_accessorGenerator;

		// Token: 0x04003130 RID: 12592
		private static Hashtable s_accessorCache;

		// Token: 0x04003131 RID: 12593
		private static MethodInfo _getPropertyMethod = typeof(IWebPropertyAccessor).GetMethod("GetProperty");

		// Token: 0x04003132 RID: 12594
		private static MethodInfo _setPropertyMethod = typeof(IWebPropertyAccessor).GetMethod("SetProperty");

		// Token: 0x04003133 RID: 12595
		private static Type[] _getPropertyParameterList = new Type[] { typeof(object) };

		// Token: 0x04003134 RID: 12596
		private static Type[] _setPropertyParameterList = new Type[]
		{
			typeof(object),
			typeof(object)
		};

		// Token: 0x04003135 RID: 12597
		private static Type[] _interfacesToImplement = new Type[1];

		// Token: 0x04003136 RID: 12598
		private static int _uniqueId;

		// Token: 0x04003137 RID: 12599
		private ModuleBuilder _dynamicModule;
	}
}
