using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F4 RID: 756
	internal static class CustomAttribute
	{
		// Token: 0x06001E16 RID: 7702 RVA: 0x0004B990 File Offset: 0x0004A990
		internal static bool IsDefined(RuntimeType type, RuntimeType caType, bool inherit)
		{
			if (type.GetElementType() != null)
			{
				return false;
			}
			if (PseudoCustomAttribute.IsDefined(type, caType))
			{
				return true;
			}
			if (CustomAttribute.IsCustomAttributeDefined(type.Module, type.MetadataToken, caType))
			{
				return true;
			}
			if (!inherit)
			{
				return false;
			}
			for (type = type.BaseType as RuntimeType; type != null; type = type.BaseType as RuntimeType)
			{
				if (CustomAttribute.IsCustomAttributeDefined(type.Module, type.MetadataToken, caType, inherit))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x0004BA04 File Offset: 0x0004AA04
		internal static bool IsDefined(RuntimeMethodInfo method, RuntimeType caType, bool inherit)
		{
			if (PseudoCustomAttribute.IsDefined(method, caType))
			{
				return true;
			}
			if (CustomAttribute.IsCustomAttributeDefined(method.Module, method.MetadataToken, caType))
			{
				return true;
			}
			if (!inherit)
			{
				return false;
			}
			for (method = method.GetParentDefinition() as RuntimeMethodInfo; method != null; method = method.GetParentDefinition() as RuntimeMethodInfo)
			{
				if (CustomAttribute.IsCustomAttributeDefined(method.Module, method.MetadataToken, caType, inherit))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x0004BA6E File Offset: 0x0004AA6E
		internal static bool IsDefined(RuntimeConstructorInfo ctor, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(ctor, caType) || CustomAttribute.IsCustomAttributeDefined(ctor.Module, ctor.MetadataToken, caType);
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x0004BA8D File Offset: 0x0004AA8D
		internal static bool IsDefined(RuntimePropertyInfo property, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(property, caType) || CustomAttribute.IsCustomAttributeDefined(property.Module, property.MetadataToken, caType);
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0004BAAC File Offset: 0x0004AAAC
		internal static bool IsDefined(RuntimeEventInfo e, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(e, caType) || CustomAttribute.IsCustomAttributeDefined(e.Module, e.MetadataToken, caType);
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0004BACB File Offset: 0x0004AACB
		internal static bool IsDefined(RuntimeFieldInfo field, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(field, caType) || CustomAttribute.IsCustomAttributeDefined(field.Module, field.MetadataToken, caType);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0004BAEA File Offset: 0x0004AAEA
		internal static bool IsDefined(ParameterInfo parameter, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(parameter, caType) || CustomAttribute.IsCustomAttributeDefined(parameter.Member.Module, parameter.MetadataToken, caType);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0004BB10 File Offset: 0x0004AB10
		internal static bool IsDefined(Assembly assembly, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(assembly, caType) || CustomAttribute.IsCustomAttributeDefined(assembly.ManifestModule, assembly.AssemblyHandle.GetToken(), caType);
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0004BB42 File Offset: 0x0004AB42
		internal static bool IsDefined(Module module, RuntimeType caType)
		{
			return PseudoCustomAttribute.IsDefined(module, caType) || CustomAttribute.IsCustomAttributeDefined(module, module.MetadataToken, caType);
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0004BB5C File Offset: 0x0004AB5C
		internal static object[] GetCustomAttributes(RuntimeType type, RuntimeType caType, bool inherit)
		{
			if (type.GetElementType() != null)
			{
				if (!caType.IsValueType)
				{
					return (object[])Array.CreateInstance(caType, 0);
				}
				return new object[0];
			}
			else
			{
				if (type.IsGenericType && !type.IsGenericTypeDefinition)
				{
					type = type.GetGenericTypeDefinition() as RuntimeType;
				}
				int i = 0;
				Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(type, caType, true, out i);
				if (!inherit || (caType.IsSealed && !CustomAttribute.GetAttributeUsage(caType).Inherited))
				{
					object[] customAttributes2 = CustomAttribute.GetCustomAttributes(type.Module, type.MetadataToken, i, caType);
					if (i > 0)
					{
						Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - i, i);
					}
					return customAttributes2;
				}
				List<object> list = new List<object>();
				bool flag = false;
				Type type2 = ((caType == null || caType.IsValueType || caType.ContainsGenericParameters) ? typeof(object) : caType);
				while (i > 0)
				{
					list.Add(customAttributes[--i]);
				}
				while (type != typeof(object) && type != null)
				{
					object[] customAttributes3 = CustomAttribute.GetCustomAttributes(type.Module, type.MetadataToken, 0, caType, flag, list);
					flag = true;
					for (int j = 0; j < customAttributes3.Length; j++)
					{
						list.Add(customAttributes3[j]);
					}
					type = type.BaseType as RuntimeType;
				}
				object[] array = Array.CreateInstance(type2, list.Count) as object[];
				Array.Copy(list.ToArray(), 0, array, 0, list.Count);
				return array;
			}
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0004BCC0 File Offset: 0x0004ACC0
		internal static object[] GetCustomAttributes(RuntimeMethodInfo method, RuntimeType caType, bool inherit)
		{
			if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
			{
				method = method.GetGenericMethodDefinition() as RuntimeMethodInfo;
			}
			int i = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(method, caType, true, out i);
			if (!inherit || (caType.IsSealed && !CustomAttribute.GetAttributeUsage(caType).Inherited))
			{
				object[] customAttributes2 = CustomAttribute.GetCustomAttributes(method.Module, method.MetadataToken, i, caType);
				if (i > 0)
				{
					Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - i, i);
				}
				return customAttributes2;
			}
			List<object> list = new List<object>();
			bool flag = false;
			Type type = ((caType == null || caType.IsValueType || caType.ContainsGenericParameters) ? typeof(object) : caType);
			while (i > 0)
			{
				list.Add(customAttributes[--i]);
			}
			while (method != null)
			{
				object[] customAttributes3 = CustomAttribute.GetCustomAttributes(method.Module, method.MetadataToken, 0, caType, flag, list);
				flag = true;
				for (int j = 0; j < customAttributes3.Length; j++)
				{
					list.Add(customAttributes3[j]);
				}
				method = method.GetParentDefinition() as RuntimeMethodInfo;
			}
			object[] array = Array.CreateInstance(type, list.Count) as object[];
			Array.Copy(list.ToArray(), 0, array, 0, list.Count);
			return array;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x0004BDF4 File Offset: 0x0004ADF4
		internal static object[] GetCustomAttributes(RuntimeConstructorInfo ctor, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(ctor, caType, true, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(ctor.Module, ctor.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x0004BE38 File Offset: 0x0004AE38
		internal static object[] GetCustomAttributes(RuntimePropertyInfo property, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(property, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(property.Module, property.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x0004BE78 File Offset: 0x0004AE78
		internal static object[] GetCustomAttributes(RuntimeEventInfo e, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(e, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(e.Module, e.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x0004BEB8 File Offset: 0x0004AEB8
		internal static object[] GetCustomAttributes(RuntimeFieldInfo field, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(field, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(field.Module, field.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0004BEF8 File Offset: 0x0004AEF8
		internal static object[] GetCustomAttributes(ParameterInfo parameter, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(parameter, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(parameter.Member.Module, parameter.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x0004BF40 File Offset: 0x0004AF40
		internal static object[] GetCustomAttributes(Assembly assembly, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(assembly, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(assembly.ManifestModule, assembly.AssemblyHandle.GetToken(), num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x0004BF88 File Offset: 0x0004AF88
		internal static object[] GetCustomAttributes(Module module, RuntimeType caType)
		{
			int num = 0;
			Attribute[] customAttributes = PseudoCustomAttribute.GetCustomAttributes(module, caType, out num);
			object[] customAttributes2 = CustomAttribute.GetCustomAttributes(module, module.MetadataToken, num, caType);
			if (num > 0)
			{
				Array.Copy(customAttributes, 0, customAttributes2, customAttributes2.Length - num, num);
			}
			return customAttributes2;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0004BFC3 File Offset: 0x0004AFC3
		internal static bool IsCustomAttributeDefined(Module decoratedModule, int decoratedMetadataToken, RuntimeType attributeFilterType)
		{
			return CustomAttribute.IsCustomAttributeDefined(decoratedModule, decoratedMetadataToken, attributeFilterType, false);
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0004BFD0 File Offset: 0x0004AFD0
		internal static bool IsCustomAttributeDefined(Module decoratedModule, int decoratedMetadataToken, RuntimeType attributeFilterType, bool mustBeInheritable)
		{
			if (decoratedModule.Assembly.ReflectionOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyCA"));
			}
			MetadataImport metadataImport = decoratedModule.MetadataImport;
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(decoratedModule, decoratedMetadataToken);
			Assembly assembly = null;
			foreach (CustomAttributeRecord customAttributeRecord in customAttributeRecords)
			{
				RuntimeType runtimeType;
				RuntimeMethodHandle runtimeMethodHandle;
				bool flag;
				bool flag2;
				if (CustomAttribute.FilterCustomAttributeRecord(customAttributeRecord, metadataImport, ref assembly, decoratedModule, decoratedMetadataToken, attributeFilterType, mustBeInheritable, null, null, out runtimeType, out runtimeMethodHandle, out flag, out flag2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0004C04F File Offset: 0x0004B04F
		internal static object[] GetCustomAttributes(Module decoratedModule, int decoratedMetadataToken, int pcaCount, RuntimeType attributeFilterType)
		{
			return CustomAttribute.GetCustomAttributes(decoratedModule, decoratedMetadataToken, pcaCount, attributeFilterType, false, null);
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0004C05C File Offset: 0x0004B05C
		internal unsafe static object[] GetCustomAttributes(Module decoratedModule, int decoratedMetadataToken, int pcaCount, RuntimeType attributeFilterType, bool mustBeInheritable, IList derivedAttributes)
		{
			if (decoratedModule.Assembly.ReflectionOnly)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_ReflectionOnlyCA"));
			}
			MetadataImport metadataImport = decoratedModule.MetadataImport;
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(decoratedModule, decoratedMetadataToken);
			Type type = ((attributeFilterType == null || attributeFilterType.IsValueType || attributeFilterType.ContainsGenericParameters) ? typeof(object) : attributeFilterType);
			if (attributeFilterType == null && customAttributeRecords.Length == 0)
			{
				return Array.CreateInstance(type, 0) as object[];
			}
			object[] array = Array.CreateInstance(type, customAttributeRecords.Length) as object[];
			int num = 0;
			SecurityContextFrame securityContextFrame = default(SecurityContextFrame);
			securityContextFrame.Push(decoratedModule.Assembly.InternalAssembly);
			Assembly assembly = null;
			for (int i = 0; i < customAttributeRecords.Length; i++)
			{
				object obj = null;
				CustomAttributeRecord customAttributeRecord = customAttributeRecords[i];
				RuntimeMethodHandle runtimeMethodHandle = default(RuntimeMethodHandle);
				RuntimeType runtimeType = null;
				int num2 = 0;
				IntPtr intPtr = customAttributeRecord.blob.Signature;
				IntPtr intPtr2 = (IntPtr)((void*)((byte*)(void*)intPtr + customAttributeRecord.blob.Length));
				bool flag;
				bool flag2;
				if (CustomAttribute.FilterCustomAttributeRecord(customAttributeRecord, metadataImport, ref assembly, decoratedModule, decoratedMetadataToken, attributeFilterType, mustBeInheritable, array, derivedAttributes, out runtimeType, out runtimeMethodHandle, out flag, out flag2))
				{
					if (!runtimeMethodHandle.IsNullHandle())
					{
						runtimeMethodHandle.CheckLinktimeDemands(decoratedModule, decoratedMetadataToken);
					}
					RuntimeConstructorInfo.CheckCanCreateInstance(runtimeType, flag2);
					if (flag)
					{
						obj = CustomAttribute.CreateCaObject(decoratedModule, runtimeMethodHandle, ref intPtr, intPtr2, out num2);
					}
					else
					{
						obj = runtimeType.TypeHandle.CreateCaInstance(runtimeMethodHandle);
						if (Marshal.ReadInt16(intPtr) != 1)
						{
							throw new CustomAttributeFormatException();
						}
						intPtr = (IntPtr)((void*)((byte*)(void*)intPtr + 2));
						num2 = (int)Marshal.ReadInt16(intPtr);
						intPtr = (IntPtr)((void*)((byte*)(void*)intPtr + 2));
					}
					for (int j = 0; j < num2; j++)
					{
						IntPtr signature = customAttributeRecord.blob.Signature;
						string text;
						bool flag3;
						Type type2;
						object obj2;
						CustomAttribute.GetPropertyOrFieldData(decoratedModule, ref intPtr, intPtr2, out text, out flag3, out type2, out obj2);
						try
						{
							if (flag3)
							{
								if (type2 == null && obj2 != null)
								{
									type2 = ((obj2.GetType() == typeof(RuntimeType)) ? typeof(Type) : obj2.GetType());
								}
								RuntimePropertyInfo runtimePropertyInfo;
								if (type2 == null)
								{
									runtimePropertyInfo = runtimeType.GetProperty(text) as RuntimePropertyInfo;
								}
								else
								{
									runtimePropertyInfo = runtimeType.GetProperty(text, type2, Type.EmptyTypes) as RuntimePropertyInfo;
								}
								RuntimeMethodInfo runtimeMethodInfo = runtimePropertyInfo.GetSetMethod(true) as RuntimeMethodInfo;
								if (runtimeMethodInfo.IsPublic)
								{
									runtimeMethodInfo.MethodHandle.CheckLinktimeDemands(decoratedModule, decoratedMetadataToken);
									runtimeMethodInfo.Invoke(obj, BindingFlags.Default, null, new object[] { obj2 }, null, true);
								}
							}
							else
							{
								RtFieldInfo rtFieldInfo = runtimeType.GetField(text) as RtFieldInfo;
								rtFieldInfo.InternalSetValue(obj, obj2, BindingFlags.Default, Type.DefaultBinder, null, false);
							}
						}
						catch (Exception ex)
						{
							throw new CustomAttributeFormatException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString(flag3 ? "RFLCT.InvalidPropFail" : "RFLCT.InvalidFieldFail"), new object[] { text }), ex);
						}
					}
					if (!intPtr.Equals(intPtr2))
					{
						throw new CustomAttributeFormatException();
					}
					array[num++] = obj;
				}
			}
			securityContextFrame.Pop();
			if (num == customAttributeRecords.Length && pcaCount == 0)
			{
				return array;
			}
			if (num == 0)
			{
				Array.CreateInstance(type, 0);
			}
			object[] array2 = Array.CreateInstance(type, num + pcaCount) as object[];
			Array.Copy(array, 0, array2, 0, num);
			return array2;
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0004C3C8 File Offset: 0x0004B3C8
		internal unsafe static bool FilterCustomAttributeRecord(CustomAttributeRecord caRecord, MetadataImport scope, ref Assembly lastAptcaOkAssembly, Module decoratedModule, MetadataToken decoratedToken, RuntimeType attributeFilterType, bool mustBeInheritable, object[] attributes, IList derivedAttributes, out RuntimeType attributeType, out RuntimeMethodHandle ctor, out bool ctorHasParameters, out bool isVarArg)
		{
			ctor = default(RuntimeMethodHandle);
			attributeType = null;
			ctorHasParameters = false;
			isVarArg = false;
			IntPtr signature = caRecord.blob.Signature;
			(IntPtr)((void*)((byte*)(void*)signature + caRecord.blob.Length));
			attributeType = decoratedModule.ResolveType(scope.GetParentToken(caRecord.tkCtor), null, null) as RuntimeType;
			if (!attributeFilterType.IsAssignableFrom(attributeType))
			{
				return false;
			}
			if (!CustomAttribute.AttributeUsageCheck(attributeType, mustBeInheritable, attributes, derivedAttributes))
			{
				return false;
			}
			if (attributeType.Assembly != lastAptcaOkAssembly && !attributeType.Assembly.AptcaCheck(decoratedModule.Assembly))
			{
				return false;
			}
			lastAptcaOkAssembly = decoratedModule.Assembly;
			ConstArray methodSignature = scope.GetMethodSignature(caRecord.tkCtor);
			isVarArg = (methodSignature[0] & 5) != 0;
			ctorHasParameters = methodSignature[1] != 0;
			if (ctorHasParameters)
			{
				ctor = decoratedModule.ModuleHandle.ResolveMethodHandle(caRecord.tkCtor);
			}
			else
			{
				ctor = attributeType.GetTypeHandleInternal().GetDefaultConstructor();
				if (ctor.IsNullHandle() && !attributeType.IsValueType)
				{
					throw new MissingMethodException(".ctor");
				}
			}
			if (ctor.IsNullHandle())
			{
				return attributeType.IsVisible || attributeType.TypeHandle.IsVisibleFromModule(decoratedModule.ModuleHandle);
			}
			if (ctor.IsVisibleFromModule(decoratedModule))
			{
				return true;
			}
			MetadataToken metadataToken = default(MetadataToken);
			if (decoratedToken.IsParamDef)
			{
				metadataToken = new MetadataToken(scope.GetParentToken(decoratedToken));
				metadataToken = new MetadataToken(scope.GetParentToken(metadataToken));
			}
			else if (decoratedToken.IsMethodDef || decoratedToken.IsProperty || decoratedToken.IsEvent || decoratedToken.IsFieldDef)
			{
				metadataToken = new MetadataToken(scope.GetParentToken(decoratedToken));
			}
			else if (decoratedToken.IsTypeDef)
			{
				metadataToken = decoratedToken;
			}
			return metadataToken.IsTypeDef && ctor.IsVisibleFromType(decoratedModule.ModuleHandle.ResolveTypeHandle(metadataToken));
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0004C5F4 File Offset: 0x0004B5F4
		private static bool AttributeUsageCheck(RuntimeType attributeType, bool mustBeInheritable, object[] attributes, IList derivedAttributes)
		{
			AttributeUsageAttribute attributeUsageAttribute = null;
			if (mustBeInheritable)
			{
				attributeUsageAttribute = CustomAttribute.GetAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					return false;
				}
			}
			if (derivedAttributes == null)
			{
				return true;
			}
			for (int i = 0; i < derivedAttributes.Count; i++)
			{
				if (derivedAttributes[i].GetType() == attributeType)
				{
					if (attributeUsageAttribute == null)
					{
						attributeUsageAttribute = CustomAttribute.GetAttributeUsage(attributeType);
					}
					return attributeUsageAttribute.AllowMultiple;
				}
			}
			return true;
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0004C650 File Offset: 0x0004B650
		internal static AttributeUsageAttribute GetAttributeUsage(RuntimeType decoratedAttribute)
		{
			Module module = decoratedAttribute.Module;
			MetadataImport metadataImport = module.MetadataImport;
			CustomAttributeRecord[] customAttributeRecords = CustomAttributeData.GetCustomAttributeRecords(module, decoratedAttribute.MetadataToken);
			AttributeUsageAttribute attributeUsageAttribute = null;
			foreach (CustomAttributeRecord customAttributeRecord in customAttributeRecords)
			{
				RuntimeType runtimeType = module.ResolveType(metadataImport.GetParentToken(customAttributeRecord.tkCtor), null, null) as RuntimeType;
				if (runtimeType == typeof(AttributeUsageAttribute))
				{
					if (attributeUsageAttribute != null)
					{
						throw new FormatException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Format_AttributeUsage"), new object[] { runtimeType }));
					}
					AttributeTargets attributeTargets;
					bool flag;
					bool flag2;
					CustomAttribute.ParseAttributeUsageAttribute(customAttributeRecord.blob, out attributeTargets, out flag, out flag2);
					attributeUsageAttribute = new AttributeUsageAttribute(attributeTargets, flag2, flag);
				}
			}
			if (attributeUsageAttribute == null)
			{
				return AttributeUsageAttribute.Default;
			}
			return attributeUsageAttribute;
		}

		// Token: 0x06001E2F RID: 7727
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _ParseAttributeUsageAttribute(IntPtr pCa, int cCa, out int targets, out bool inherited, out bool allowMultiple);

		// Token: 0x06001E30 RID: 7728 RVA: 0x0004C728 File Offset: 0x0004B728
		private static void ParseAttributeUsageAttribute(ConstArray ca, out AttributeTargets targets, out bool inherited, out bool allowMultiple)
		{
			int num;
			CustomAttribute._ParseAttributeUsageAttribute(ca.Signature, ca.Length, out num, out inherited, out allowMultiple);
			targets = (AttributeTargets)num;
		}

		// Token: 0x06001E31 RID: 7729
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern object _CreateCaObject(void* pModule, void* pCtor, byte** ppBlob, byte* pEndBlob, int* pcNamedArgs);

		// Token: 0x06001E32 RID: 7730 RVA: 0x0004C750 File Offset: 0x0004B750
		private unsafe static object CreateCaObject(Module module, RuntimeMethodHandle ctor, ref IntPtr blob, IntPtr blobEnd, out int namedArgs)
		{
			byte* ptr = (byte*)(void*)blob;
			byte* ptr2 = (byte*)(void*)blobEnd;
			int num;
			object obj = CustomAttribute._CreateCaObject(module.ModuleHandle.Value, (void*)ctor.Value, &ptr, ptr2, &num);
			blob = (IntPtr)((void*)ptr);
			namedArgs = num;
			return obj;
		}

		// Token: 0x06001E33 RID: 7731
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetPropertyOrFieldData(IntPtr pModule, byte** ppBlobStart, byte* pBlobEnd, out string name, out bool bIsProperty, out Type type, out object value);

		// Token: 0x06001E34 RID: 7732 RVA: 0x0004C7AC File Offset: 0x0004B7AC
		private unsafe static void GetPropertyOrFieldData(Module module, ref IntPtr blobStart, IntPtr blobEnd, out string name, out bool isProperty, out Type type, out object value)
		{
			byte* ptr = (byte*)(void*)blobStart;
			CustomAttribute._GetPropertyOrFieldData((IntPtr)module.ModuleHandle.Value, &ptr, (byte*)(void*)blobEnd, out name, out isProperty, out type, out value);
			blobStart = (IntPtr)((void*)ptr);
		}
	}
}
