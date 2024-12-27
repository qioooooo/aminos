using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000099 RID: 153
	[Serializable]
	internal sealed class JSBinder : Binder
	{
		// Token: 0x060006BD RID: 1725 RVA: 0x0002F0C4 File Offset: 0x0002E0C4
		internal static object[] ArrangeNamedArguments(MethodBase method, object[] args, string[] namedParameters)
		{
			ParameterInfo[] parameters = method.GetParameters();
			int num = parameters.Length;
			if (num == 0)
			{
				throw new JScriptException(JSError.MissingNameParameter);
			}
			object[] array = new object[num];
			int num2 = args.Length;
			int num3 = namedParameters.Length;
			int num4 = num2 - num3;
			ArrayObject.Copy(args, num3, array, 0, num4);
			for (int i = 0; i < num3; i++)
			{
				string text = namedParameters[i];
				if (text == null || text.Equals(""))
				{
					throw new JScriptException(JSError.MustProvideNameForNamedParameter);
				}
				int j = num4;
				while (j < num)
				{
					if (text.Equals(parameters[j].Name))
					{
						if (array[j] is Empty)
						{
							throw new JScriptException(JSError.DuplicateNamedParameter);
						}
						array[j] = args[i];
						break;
					}
					else
					{
						j++;
					}
				}
				if (j == num)
				{
					throw new JScriptException(JSError.MissingNameParameter);
				}
			}
			if (method is JSMethod)
			{
				return array;
			}
			for (int k = 0; k < num; k++)
			{
				if (array[k] == null || array[k] == Missing.Value)
				{
					object defaultParameterValue = TypeReferences.GetDefaultParameterValue(parameters[k]);
					if (defaultParameterValue == Convert.DBNull)
					{
						throw new ArgumentException(parameters[k].Name);
					}
					array[k] = defaultParameterValue;
				}
			}
			return array;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0002F1EC File Offset: 0x0002E1EC
		public override FieldInfo BindToField(BindingFlags bindAttr, FieldInfo[] match, object value, CultureInfo locale)
		{
			if (value == null)
			{
				value = DBNull.Value;
			}
			int num = int.MaxValue;
			int num2 = 0;
			FieldInfo fieldInfo = null;
			Type type = value.GetType();
			int i = 0;
			int num3 = match.Length;
			while (i < num3)
			{
				FieldInfo fieldInfo2 = match[i];
				int num4 = JSBinder.TypeDistance(Runtime.TypeRefs, fieldInfo2.FieldType, type);
				if (num4 < num)
				{
					num = num4;
					fieldInfo = fieldInfo2;
					num2 = 0;
				}
				else if (num4 == num)
				{
					num2++;
				}
				i++;
			}
			if (num2 > 0)
			{
				throw new AmbiguousMatchException();
			}
			return fieldInfo;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0002F267 File Offset: 0x0002E267
		public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo locale, string[] namedParameters, out object state)
		{
			state = null;
			return JSBinder.SelectMethodBase(Runtime.TypeRefs, match, ref args, modifiers, namedParameters);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002F27D File Offset: 0x0002E27D
		public override object ChangeType(object value, Type target_type, CultureInfo locale)
		{
			return Convert.CoerceT(value, target_type);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002F286 File Offset: 0x0002E286
		internal static MemberInfo[] GetDefaultMembers(IReflect ir)
		{
			return JSBinder.GetDefaultMembers(Globals.TypeRefs, ir);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002F294 File Offset: 0x0002E294
		internal static MemberInfo[] GetDefaultMembers(TypeReferences typeRefs, IReflect ir)
		{
			while (ir is ClassScope)
			{
				ClassScope classScope = (ClassScope)ir;
				classScope.owner.IsExpando();
				if (classScope.itemProp != null)
				{
					return new MemberInfo[] { classScope.itemProp };
				}
				ir = classScope.GetParent();
				if (ir is WithObject)
				{
					ir = (IReflect)((WithObject)ir).contained_object;
				}
			}
			if (ir is Type)
			{
				return JSBinder.GetDefaultMembers((Type)ir);
			}
			if (ir is JSObject)
			{
				return typeRefs.ScriptObject.GetDefaultMembers();
			}
			return null;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002F324 File Offset: 0x0002E324
		internal static MemberInfo[] GetDefaultMembers(Type t)
		{
			while (t != typeof(object) && t != null)
			{
				MemberInfo[] defaultMembers = t.GetDefaultMembers();
				if (defaultMembers != null && defaultMembers.Length > 0)
				{
					return defaultMembers;
				}
				t = t.BaseType;
			}
			return null;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002F360 File Offset: 0x0002E360
		internal static MethodInfo GetDefaultPropertyForArrayIndex(Type t, int index, Type elementType, bool getSetter)
		{
			try
			{
				MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(Runtime.TypeRefs, t);
				int num = 0;
				if (defaultMembers == null || (num = defaultMembers.Length) == 0)
				{
					return null;
				}
				int i = 0;
				while (i < num)
				{
					MemberInfo memberInfo = defaultMembers[i];
					MemberTypes memberType = memberInfo.MemberType;
					MemberTypes memberTypes = memberType;
					MethodInfo methodInfo;
					if (memberTypes == MemberTypes.Method)
					{
						methodInfo = (MethodInfo)memberInfo;
						goto IL_005F;
					}
					if (memberTypes == MemberTypes.Property)
					{
						methodInfo = ((PropertyInfo)memberInfo).GetGetMethod();
						goto IL_005F;
					}
					IL_0101:
					i++;
					continue;
					IL_005F:
					if (methodInfo == null)
					{
						goto IL_0101;
					}
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters == null || parameters.Length == 0)
					{
						Type returnType = methodInfo.ReturnType;
						if (typeof(Array).IsAssignableFrom(returnType) || typeof(IList).IsAssignableFrom(returnType))
						{
							return methodInfo;
						}
						goto IL_0101;
					}
					else
					{
						if (parameters.Length == 1 && memberType == MemberTypes.Property)
						{
							PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
							if (elementType != null)
							{
								if (!propertyInfo.PropertyType.IsAssignableFrom(elementType))
								{
									goto IL_0101;
								}
							}
							try
							{
								Convert.CoerceT(index, parameters[0].ParameterType);
								if (getSetter)
								{
									return propertyInfo.GetSetMethod();
								}
								return methodInfo;
							}
							catch (JScriptException)
							{
							}
							goto IL_0101;
						}
						goto IL_0101;
					}
				}
			}
			catch (InvalidOperationException)
			{
			}
			return null;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002F4B8 File Offset: 0x0002E4B8
		internal static MemberInfo[] GetInterfaceMembers(string name, Type t)
		{
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
			MemberInfo[] array = t.GetMember(name, bindingFlags);
			Type[] interfaces = t.GetInterfaces();
			if (interfaces == null || interfaces.Length == 0)
			{
				return array;
			}
			ArrayList arrayList = new ArrayList(interfaces);
			MemberInfoList memberInfoList = new MemberInfoList();
			memberInfoList.AddRange(array);
			for (int i = 0; i < arrayList.Count; i++)
			{
				Type type = (Type)arrayList[i];
				array = type.GetMember(name, bindingFlags);
				if (array != null)
				{
					memberInfoList.AddRange(array);
				}
				foreach (Type type2 in type.GetInterfaces())
				{
					if (arrayList.IndexOf(type2) == -1)
					{
						arrayList.Add(type2);
					}
				}
			}
			return memberInfoList.ToArray();
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002F570 File Offset: 0x0002E570
		private static bool FormalParamTypeIsObject(ParameterInfo par)
		{
			ParameterDeclaration parameterDeclaration = par as ParameterDeclaration;
			if (parameterDeclaration != null)
			{
				return parameterDeclaration.ParameterIReflect == Typeob.Object;
			}
			return par.ParameterType == Typeob.Object;
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002F5A2 File Offset: 0x0002E5A2
		public override void ReorderArgumentArray(ref object[] args, object state)
		{
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x0002F5A4 File Offset: 0x0002E5A4
		internal static MemberInfo Select(TypeReferences typeRefs, MemberInfo[] match, int matches, IReflect[] argIRs, MemberTypes memberType)
		{
			int num = 0;
			ParameterInfo[][] array = new ParameterInfo[matches][];
			bool flag = memberType == MemberTypes.Method;
			for (int i = 0; i < matches; i++)
			{
				MemberInfo memberInfo = match[i];
				if (memberInfo is PropertyInfo && flag)
				{
					memberInfo = ((PropertyInfo)memberInfo).GetGetMethod(true);
				}
				if (memberInfo != null && memberInfo.MemberType == memberType)
				{
					if (memberInfo is PropertyInfo)
					{
						array[i] = ((PropertyInfo)memberInfo).GetIndexParameters();
					}
					else
					{
						array[i] = ((MethodBase)memberInfo).GetParameters();
					}
					num++;
				}
			}
			int num2 = JSBinder.SelectBest(typeRefs, match, matches, argIRs, array, null, num, argIRs.Length);
			if (num2 < 0)
			{
				return null;
			}
			return match[num2];
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002F644 File Offset: 0x0002E644
		internal static MemberInfo Select(TypeReferences typeRefs, MemberInfo[] match, int matches, ref object[] args, string[] namedParameters, MemberTypes memberType)
		{
			bool flag = false;
			if (namedParameters != null && namedParameters.Length > 0)
			{
				if (args.Length < namedParameters.Length)
				{
					throw new JScriptException(JSError.MoreNamedParametersThanArguments);
				}
				flag = true;
			}
			int num = 0;
			ParameterInfo[][] array = new ParameterInfo[matches][];
			object[][] array2 = new object[matches][];
			bool flag2 = memberType == MemberTypes.Method;
			for (int i = 0; i < matches; i++)
			{
				MemberInfo memberInfo = match[i];
				if (flag2 && memberInfo.MemberType == MemberTypes.Property)
				{
					memberInfo = ((PropertyInfo)memberInfo).GetGetMethod(true);
				}
				if (memberInfo.MemberType == memberType)
				{
					if (memberType == MemberTypes.Property)
					{
						array[i] = ((PropertyInfo)memberInfo).GetIndexParameters();
					}
					else
					{
						array[i] = ((MethodBase)memberInfo).GetParameters();
					}
					if (flag)
					{
						array2[i] = JSBinder.ArrangeNamedArguments((MethodBase)memberInfo, args, namedParameters);
					}
					else
					{
						array2[i] = args;
					}
					num++;
				}
			}
			int num2 = JSBinder.SelectBest(typeRefs, match, matches, null, array, array2, num, args.Length);
			if (num2 < 0)
			{
				return null;
			}
			args = array2[num2];
			MemberInfo memberInfo2 = match[num2];
			if (flag2 && memberInfo2.MemberType == MemberTypes.Property)
			{
				memberInfo2 = ((PropertyInfo)memberInfo2).GetGetMethod(true);
			}
			return memberInfo2;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002F764 File Offset: 0x0002E764
		private static int SelectBest(TypeReferences typeRefs, MemberInfo[] match, int matches, IReflect[] argIRs, ParameterInfo[][] fparams, object[][] aparams, int candidates, int parameters)
		{
			if (candidates == 0)
			{
				return -1;
			}
			if (candidates == 1)
			{
				for (int i = 0; i < matches; i++)
				{
					if (fparams[i] != null)
					{
						return i;
					}
				}
			}
			bool[] array = new bool[matches];
			int[] array2 = new int[matches];
			for (int j = 0; j < matches; j++)
			{
				ParameterInfo[] array3 = fparams[j];
				if (array3 != null)
				{
					int num = array3.Length;
					int num2 = ((argIRs == null) ? aparams[j].Length : argIRs.Length);
					if (num2 > num && (num == 0 || !CustomAttribute.IsDefined(array3[num - 1], typeof(ParamArrayAttribute), false)))
					{
						fparams[j] = null;
						candidates--;
					}
					else
					{
						for (int k = parameters; k < num; k++)
						{
							ParameterInfo parameterInfo = array3[k];
							if (k == num - 1 && CustomAttribute.IsDefined(parameterInfo, typeof(ParamArrayAttribute), false))
							{
								break;
							}
							object defaultParameterValue = TypeReferences.GetDefaultParameterValue(parameterInfo);
							if (defaultParameterValue is DBNull)
							{
								array2[j] = 50;
							}
						}
					}
				}
			}
			int num3 = 0;
			while (candidates > 1)
			{
				int num4 = 0;
				int num5 = int.MaxValue;
				bool flag = false;
				for (int l = 0; l < matches; l++)
				{
					int num6 = 0;
					ParameterInfo[] array4 = fparams[l];
					if (array4 != null)
					{
						IReflect reflect = typeRefs.Missing;
						if (argIRs == null)
						{
							if (aparams[l].Length > num3)
							{
								object obj = aparams[l][num3];
								if (obj == null)
								{
									obj = DBNull.Value;
								}
								reflect = typeRefs.ToReferenceContext(obj.GetType());
							}
						}
						else if (num3 < parameters)
						{
							reflect = argIRs[num3];
						}
						int num7 = array4.Length;
						if (num7 - 1 > num3)
						{
							num4++;
						}
						IReflect reflect2 = typeRefs.Missing;
						if (num7 > 0 && num3 >= num7 - 1 && CustomAttribute.IsDefined(array4[num7 - 1], typeof(ParamArrayAttribute), false) && !(reflect is TypedArray) && reflect != typeRefs.ArrayObject && (!(reflect is Type) || !((Type)reflect).IsArray))
						{
							ParameterInfo parameterInfo2 = array4[num7 - 1];
							if (parameterInfo2 is ParameterDeclaration)
							{
								reflect2 = ((ParameterDeclaration)parameterInfo2).ParameterIReflect;
								reflect2 = ((TypedArray)reflect2).elementType;
							}
							else
							{
								reflect2 = parameterInfo2.ParameterType.GetElementType();
							}
							if (num3 == num7 - 1)
							{
								array2[l]++;
							}
						}
						else if (num3 < num7)
						{
							ParameterInfo parameterInfo3 = array4[num3];
							reflect2 = ((parameterInfo3 is ParameterDeclaration) ? ((ParameterDeclaration)parameterInfo3).ParameterIReflect : parameterInfo3.ParameterType);
							if (reflect == typeRefs.Missing)
							{
								object defaultParameterValue2 = TypeReferences.GetDefaultParameterValue(parameterInfo3);
								if (!(defaultParameterValue2 is DBNull))
								{
									reflect = reflect2;
									num6 = 1;
								}
							}
						}
						int num8 = JSBinder.TypeDistance(typeRefs, reflect2, reflect) + array2[l] + num6;
						if (num8 == num5)
						{
							if (num3 == num7 - 1 && array[l])
							{
								candidates--;
								fparams[l] = null;
							}
							flag = flag && array[l];
						}
						else if (num8 > num5)
						{
							if (flag && num3 < num7 && JSBinder.FormalParamTypeIsObject(fparams[l][num3]))
							{
								num5 = num8;
							}
							else if (num3 <= num7 - 1 || reflect != typeRefs.Missing || !CustomAttribute.IsDefined(array4[num7 - 1], typeof(ParamArrayAttribute), false))
							{
								array[l] = true;
							}
						}
						else
						{
							if (candidates == 1 && !array[l])
							{
								return l;
							}
							flag = array[l];
							for (int m = 0; m < l; m++)
							{
								if (fparams[m] != null && !array[m])
								{
									bool flag2 = fparams[m].Length <= num3;
									if ((!flag2 || parameters > num3) && (flag2 || !flag || !JSBinder.FormalParamTypeIsObject(fparams[m][num3])))
									{
										array[m] = true;
									}
								}
							}
							num5 = num8;
						}
					}
				}
				if (num3 >= parameters - 1 && num4 < 1)
				{
					break;
				}
				num3++;
			}
			int num9 = -1;
			int num10 = 0;
			while (num10 < matches && candidates > 0)
			{
				ParameterInfo[] array5 = fparams[num10];
				if (array5 != null)
				{
					if (array[num10])
					{
						candidates--;
						fparams[num10] = null;
					}
					else
					{
						int num11 = array5.Length;
						if (num9 == -1)
						{
							num9 = num10;
						}
						else if (Class.ParametersMatch(array5, fparams[num9]))
						{
							MemberInfo memberInfo = match[num9];
							JSWrappedMethod jswrappedMethod = match[num9] as JSWrappedMethod;
							if (jswrappedMethod != null)
							{
								memberInfo = jswrappedMethod.method;
							}
							if (memberInfo is JSFieldMethod || memberInfo is JSConstructor || memberInfo is JSProperty)
							{
								candidates--;
								fparams[num10] = null;
							}
							else
							{
								Type declaringType = match[num9].DeclaringType;
								Type declaringType2 = match[num10].DeclaringType;
								if (declaringType != declaringType2)
								{
									if (declaringType2.IsAssignableFrom(declaringType))
									{
										candidates--;
										fparams[num10] = null;
									}
									else if (declaringType.IsAssignableFrom(declaringType2))
									{
										fparams[num9] = null;
										num9 = num10;
										candidates--;
									}
								}
							}
						}
					}
				}
				num10++;
			}
			if (candidates != 1)
			{
				throw new AmbiguousMatchException();
			}
			return num9;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002FC28 File Offset: 0x0002EC28
		internal static Type HandleCoClassAttribute(Type t)
		{
			object[] customAttributes = CustomAttribute.GetCustomAttributes(t, typeof(CoClassAttribute), false);
			if (customAttributes != null && customAttributes.Length == 1)
			{
				t = ((CoClassAttribute)customAttributes[0]).CoClass;
				if (!t.IsPublic)
				{
					throw new JScriptException(JSError.NotAccessible, new Context(new DocumentContext("", null), t.ToString()));
				}
			}
			return t;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0002FC89 File Offset: 0x0002EC89
		internal static ConstructorInfo SelectConstructor(MemberInfo[] match, ref object[] args, string[] namedParameters)
		{
			return JSBinder.SelectConstructor(Globals.TypeRefs, match, ref args, namedParameters);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0002FC98 File Offset: 0x0002EC98
		internal static ConstructorInfo SelectConstructor(TypeReferences typeRefs, MemberInfo[] match, ref object[] args, string[] namedParameters)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				Type type = match[0] as Type;
				if (type != null)
				{
					if (type.IsInterface && type.IsImport)
					{
						type = JSBinder.HandleCoClassAttribute(type);
					}
					match = type.GetConstructors();
					num = match.Length;
				}
			}
			if (num == 1)
			{
				return match[0] as ConstructorInfo;
			}
			return (ConstructorInfo)JSBinder.Select(typeRefs, match, num, ref args, namedParameters, MemberTypes.Constructor);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0002FD03 File Offset: 0x0002ED03
		internal static ConstructorInfo SelectConstructor(MemberInfo[] match, IReflect[] argIRs)
		{
			return JSBinder.SelectConstructor(Globals.TypeRefs, match, argIRs);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x0002FD14 File Offset: 0x0002ED14
		internal static ConstructorInfo SelectConstructor(TypeReferences typeRefs, MemberInfo[] match, IReflect[] argIRs)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 1)
			{
				object obj = match[0];
				if (obj is JSGlobalField)
				{
					obj = ((JSGlobalField)obj).GetValue(null);
				}
				Type type = obj as Type;
				if (type != null)
				{
					if (type.IsInterface && type.IsImport)
					{
						type = JSBinder.HandleCoClassAttribute(type);
					}
					match = type.GetConstructors();
				}
				num = match.Length;
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0] as ConstructorInfo;
			}
			return (ConstructorInfo)JSBinder.Select(typeRefs, match, num, argIRs, MemberTypes.Constructor);
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0002FD98 File Offset: 0x0002ED98
		internal static MemberInfo SelectCallableMember(MemberInfo[] match, IReflect[] argIRs)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			return (num == 1) ? match[0] : JSBinder.Select(Globals.TypeRefs, match, num, argIRs, MemberTypes.Method);
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0002FDCC File Offset: 0x0002EDCC
		internal static MethodInfo SelectMethod(MemberInfo[] match, ref object[] args, string[] namedParameters)
		{
			return JSBinder.SelectMethod(Globals.TypeRefs, match, ref args, namedParameters);
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0002FDDC File Offset: 0x0002EDDC
		internal static MethodInfo SelectMethod(TypeReferences typeRefs, MemberInfo[] match, ref object[] args, string[] namedParameters)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			MemberInfo memberInfo = ((num == 1) ? match[0] : JSBinder.Select(typeRefs, match, num, ref args, namedParameters, MemberTypes.Method));
			if (memberInfo != null && memberInfo.MemberType == MemberTypes.Property)
			{
				memberInfo = ((PropertyInfo)memberInfo).GetGetMethod(true);
			}
			return memberInfo as MethodInfo;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002FE2C File Offset: 0x0002EE2C
		internal static MethodInfo SelectMethod(MemberInfo[] match, IReflect[] argIRs)
		{
			return JSBinder.SelectMethod(Globals.TypeRefs, match, argIRs);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0002FE3C File Offset: 0x0002EE3C
		internal static MethodInfo SelectMethod(TypeReferences typeRefs, MemberInfo[] match, IReflect[] argIRs)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			MemberInfo memberInfo = ((num == 1) ? match[0] : JSBinder.Select(typeRefs, match, num, argIRs, MemberTypes.Method));
			if (memberInfo != null && memberInfo.MemberType == MemberTypes.Property)
			{
				return ((PropertyInfo)memberInfo).GetGetMethod(true);
			}
			return memberInfo as MethodInfo;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002FE8C File Offset: 0x0002EE8C
		public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0];
			}
			if (match[0].MemberType == MemberTypes.Constructor)
			{
				return (ConstructorInfo)JSBinder.Select(Runtime.TypeRefs, match, num, types, MemberTypes.Constructor);
			}
			return (MethodInfo)JSBinder.Select(Runtime.TypeRefs, match, num, types, MemberTypes.Method);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002FEE4 File Offset: 0x0002EEE4
		private static MethodBase SelectMethodBase(TypeReferences typeRefs, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, string[] namedParameters)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0];
			}
			MethodBase methodBase = (MethodBase)JSBinder.Select(typeRefs, match, num, ref args, namedParameters, MemberTypes.Method);
			if (methodBase == null)
			{
				methodBase = (MethodBase)JSBinder.Select(typeRefs, match, num, ref args, namedParameters, MemberTypes.Constructor);
			}
			return methodBase;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002FF30 File Offset: 0x0002EF30
		internal static MethodInfo SelectOperator(MethodInfo op1, MethodInfo op2, Type t1, Type t2)
		{
			ParameterInfo[] array = null;
			if (op1 == null || (op1.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || (array = op1.GetParameters()).Length != 2)
			{
				op1 = null;
			}
			ParameterInfo[] array2 = null;
			if (op2 == null || (op2.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || (array2 = op2.GetParameters()).Length != 2)
			{
				op2 = null;
			}
			if (op1 == null)
			{
				return op2;
			}
			if (op2 == null)
			{
				return op1;
			}
			int num = JSBinder.TypeDistance(Globals.TypeRefs, array[0].ParameterType, t1) + JSBinder.TypeDistance(Globals.TypeRefs, array[1].ParameterType, t2);
			int num2 = JSBinder.TypeDistance(Globals.TypeRefs, array2[0].ParameterType, t1) + JSBinder.TypeDistance(Globals.TypeRefs, array2[1].ParameterType, t2);
			if (num <= num2)
			{
				return op1;
			}
			return op2;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002FFE4 File Offset: 0x0002EFE4
		internal static PropertyInfo SelectProperty(MemberInfo[] match, object[] args)
		{
			return JSBinder.SelectProperty(Globals.TypeRefs, match, args);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002FFF4 File Offset: 0x0002EFF4
		internal static PropertyInfo SelectProperty(TypeReferences typeRefs, MemberInfo[] match, object[] args)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0] as PropertyInfo;
			}
			int num2 = 0;
			PropertyInfo propertyInfo = null;
			ParameterInfo[][] array = new ParameterInfo[num][];
			object[][] array2 = new object[num][];
			for (int i = 0; i < num; i++)
			{
				MemberInfo memberInfo = match[i];
				if (memberInfo.MemberType == MemberTypes.Property)
				{
					MethodInfo getMethod = (propertyInfo = (PropertyInfo)memberInfo).GetGetMethod(true);
					if (getMethod == null)
					{
						array[i] = propertyInfo.GetIndexParameters();
					}
					else
					{
						array[i] = getMethod.GetParameters();
					}
					array2[i] = args;
					num2++;
				}
			}
			if (num2 <= 1)
			{
				return propertyInfo;
			}
			int num3 = JSBinder.SelectBest(typeRefs, match, num, null, array, array2, num2, args.Length);
			if (num3 < 0)
			{
				return null;
			}
			return (PropertyInfo)match[num3];
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x000300B0 File Offset: 0x0002F0B0
		public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type rtype, Type[] types, ParameterModifier[] modifiers)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0];
			}
			int num2 = 0;
			PropertyInfo propertyInfo = null;
			int maxValue = int.MaxValue;
			ParameterInfo[][] array = new ParameterInfo[num][];
			int i = 0;
			while (i < num)
			{
				propertyInfo = match[i];
				if (rtype == null)
				{
					goto IL_0074;
				}
				int num3 = JSBinder.TypeDistance(Globals.TypeRefs, propertyInfo.PropertyType, rtype);
				if (num3 <= maxValue)
				{
					if (num3 < maxValue)
					{
						for (int j = 0; j < i; j++)
						{
							if (array[j] != null)
							{
								array[j] = null;
								num2--;
							}
						}
						goto IL_0074;
					}
					goto IL_0074;
				}
				IL_0083:
				i++;
				continue;
				IL_0074:
				array[i] = propertyInfo.GetIndexParameters();
				num2++;
				goto IL_0083;
			}
			if (num2 <= 1)
			{
				return propertyInfo;
			}
			int num4 = JSBinder.SelectBest(Globals.TypeRefs, match, num, types, array, null, num2, types.Length);
			if (num4 < 0)
			{
				return null;
			}
			return match[num4];
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x00030174 File Offset: 0x0002F174
		internal static PropertyInfo SelectProperty(MemberInfo[] match, IReflect[] argIRs)
		{
			return JSBinder.SelectProperty(Globals.TypeRefs, match, argIRs);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00030184 File Offset: 0x0002F184
		internal static PropertyInfo SelectProperty(TypeReferences typeRefs, MemberInfo[] match, IReflect[] argIRs)
		{
			if (match == null)
			{
				return null;
			}
			int num = match.Length;
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0] as PropertyInfo;
			}
			return (PropertyInfo)JSBinder.Select(typeRefs, match, num, argIRs, MemberTypes.Property);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x000301BC File Offset: 0x0002F1BC
		private static int TypeDistance(TypeReferences typeRefs, IReflect formal, IReflect actual)
		{
			if (formal is TypedArray)
			{
				if (actual is TypedArray)
				{
					TypedArray typedArray = (TypedArray)formal;
					TypedArray typedArray2 = (TypedArray)actual;
					if (typedArray.rank == typedArray2.rank)
					{
						if (JSBinder.TypeDistance(typeRefs, typedArray.elementType, typedArray2.elementType) != 0)
						{
							return 100;
						}
						return 0;
					}
				}
				else if (actual is Type)
				{
					TypedArray typedArray3 = (TypedArray)formal;
					Type type = (Type)actual;
					if (type.IsArray && typedArray3.rank == type.GetArrayRank())
					{
						if (JSBinder.TypeDistance(typeRefs, typedArray3.elementType, type.GetElementType()) != 0)
						{
							return 100;
						}
						return 0;
					}
					else if (type == typeRefs.Array || type == typeRefs.ArrayObject)
					{
						return 30;
					}
				}
				return 100;
			}
			if (actual is TypedArray)
			{
				if (formal is Type)
				{
					Type type2 = (Type)formal;
					TypedArray typedArray4 = (TypedArray)actual;
					if (type2.IsArray && type2.GetArrayRank() == typedArray4.rank)
					{
						if (JSBinder.TypeDistance(typeRefs, type2.GetElementType(), typedArray4.elementType) != 0)
						{
							return 100;
						}
						return 0;
					}
					else
					{
						if (type2 == typeRefs.Array)
						{
							return 30;
						}
						if (type2 == typeRefs.Object)
						{
							return 50;
						}
					}
				}
				return 100;
			}
			if (formal is ClassScope)
			{
				if (!(actual is ClassScope))
				{
					return 100;
				}
				if (!((ClassScope)actual).IsSameOrDerivedFrom((ClassScope)formal))
				{
					return 100;
				}
				return 0;
			}
			else
			{
				if (!(actual is ClassScope))
				{
					return JSBinder.TypeDistance(typeRefs, Convert.ToType(typeRefs, formal), Convert.ToType(typeRefs, actual));
				}
				if (!(formal is Type))
				{
					return 100;
				}
				if (!((ClassScope)actual).IsPromotableTo((Type)formal))
				{
					return 100;
				}
				return 0;
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0003034C File Offset: 0x0002F34C
		private static int TypeDistance(TypeReferences typeRefs, Type formal, Type actual)
		{
			TypeCode typeCode = Type.GetTypeCode(actual);
			TypeCode typeCode2 = Type.GetTypeCode(formal);
			if (actual.IsEnum)
			{
				typeCode = TypeCode.Object;
			}
			if (formal.IsEnum)
			{
				typeCode2 = TypeCode.Object;
			}
			switch (typeCode)
			{
			case TypeCode.Object:
				if (formal == actual)
				{
					return 0;
				}
				if (formal == typeRefs.Missing)
				{
					return 200;
				}
				if (formal.IsAssignableFrom(actual))
				{
					Type[] interfaces = actual.GetInterfaces();
					int num = interfaces.Length;
					int i;
					for (i = 0; i < num; i++)
					{
						if (formal == interfaces[i])
						{
							return i + 1;
						}
					}
					i = 0;
					while (actual != typeRefs.Object && actual != null)
					{
						if (formal == actual)
						{
							return i + num + 1;
						}
						actual = actual.BaseType;
						i++;
					}
					return i + num + 1;
				}
				if (typeRefs.Array.IsAssignableFrom(formal) && (actual == typeRefs.Array || typeRefs.ArrayObject.IsAssignableFrom(actual)))
				{
					return 10;
				}
				if (typeCode2 == TypeCode.String)
				{
					return 20;
				}
				if (actual == typeRefs.ScriptFunction && typeRefs.Delegate.IsAssignableFrom(formal))
				{
					return 19;
				}
				return 100;
			case TypeCode.DBNull:
				if (formal != typeRefs.Object)
				{
					return 1;
				}
				return 0;
			case TypeCode.Boolean:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 12);
				case TypeCode.Boolean:
					return 0;
				case TypeCode.SByte:
					return 5;
				case TypeCode.Byte:
					return 1;
				case TypeCode.Int16:
					return 6;
				case TypeCode.UInt16:
					return 2;
				case TypeCode.Int32:
					return 7;
				case TypeCode.UInt32:
					return 3;
				case TypeCode.Int64:
					return 8;
				case TypeCode.UInt64:
					return 4;
				case TypeCode.Single:
					return 9;
				case TypeCode.Double:
					return 10;
				case TypeCode.Decimal:
					return 11;
				case TypeCode.String:
					return 13;
				}
				return 100;
			case TypeCode.Char:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 9);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.Char:
					return 0;
				case TypeCode.SByte:
					return 13;
				case TypeCode.Byte:
					return 12;
				case TypeCode.Int16:
					return 11;
				case TypeCode.UInt16:
					return 1;
				case TypeCode.Int32:
					return 3;
				case TypeCode.UInt32:
					return 2;
				case TypeCode.Int64:
					return 5;
				case TypeCode.UInt64:
					return 4;
				case TypeCode.Single:
					return 6;
				case TypeCode.Double:
					return 7;
				case TypeCode.Decimal:
					return 8;
				case TypeCode.String:
					return 10;
				}
				return 100;
			case TypeCode.SByte:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 7);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 0;
				case TypeCode.Byte:
					return 9;
				case TypeCode.Int16:
					return 1;
				case TypeCode.UInt16:
					return 10;
				case TypeCode.Int32:
					return 2;
				case TypeCode.UInt32:
					return 12;
				case TypeCode.Int64:
					return 3;
				case TypeCode.UInt64:
					return 13;
				case TypeCode.Single:
					return 4;
				case TypeCode.Double:
					return 5;
				case TypeCode.Decimal:
					return 6;
				case TypeCode.String:
					return 8;
				}
				return 100;
			case TypeCode.Byte:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 11);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 13;
				case TypeCode.Byte:
					return 0;
				case TypeCode.Int16:
					return 3;
				case TypeCode.UInt16:
					return 1;
				case TypeCode.Int32:
					return 5;
				case TypeCode.UInt32:
					return 4;
				case TypeCode.Int64:
					return 7;
				case TypeCode.UInt64:
					return 6;
				case TypeCode.Single:
					return 8;
				case TypeCode.Double:
					return 9;
				case TypeCode.Decimal:
					return 10;
				case TypeCode.String:
					return 12;
				}
				return 100;
			case TypeCode.Int16:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 6);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 12;
				case TypeCode.Byte:
					return 13;
				case TypeCode.Int16:
					return 0;
				case TypeCode.UInt16:
					return 8;
				case TypeCode.Int32:
					return 1;
				case TypeCode.UInt32:
					return 10;
				case TypeCode.Int64:
					return 2;
				case TypeCode.UInt64:
					return 11;
				case TypeCode.Single:
					return 3;
				case TypeCode.Double:
					return 4;
				case TypeCode.Decimal:
					return 5;
				case TypeCode.String:
					return 7;
				}
				return 100;
			case TypeCode.UInt16:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 9);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 13;
				case TypeCode.Byte:
					return 12;
				case TypeCode.Int16:
					return 11;
				case TypeCode.UInt16:
					return 0;
				case TypeCode.Int32:
					return 4;
				case TypeCode.UInt32:
					return 1;
				case TypeCode.Int64:
					return 5;
				case TypeCode.UInt64:
					return 2;
				case TypeCode.Single:
					return 6;
				case TypeCode.Double:
					return 7;
				case TypeCode.Decimal:
					return 8;
				case TypeCode.String:
					return 10;
				}
				return 100;
			case TypeCode.Int32:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 4);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 12;
				case TypeCode.Byte:
					return 13;
				case TypeCode.Int16:
					return 9;
				case TypeCode.UInt16:
					return 10;
				case TypeCode.Int32:
					return 0;
				case TypeCode.UInt32:
					return 7;
				case TypeCode.Int64:
					return 1;
				case TypeCode.UInt64:
					return 6;
				case TypeCode.Single:
					return 8;
				case TypeCode.Double:
					return 2;
				case TypeCode.Decimal:
					return 3;
				case TypeCode.String:
					return 5;
				}
				return 100;
			case TypeCode.UInt32:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 5);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 13;
				case TypeCode.Byte:
					return 12;
				case TypeCode.Int16:
					return 11;
				case TypeCode.UInt16:
					return 9;
				case TypeCode.Int32:
					return 7;
				case TypeCode.UInt32:
					return 0;
				case TypeCode.Int64:
					return 2;
				case TypeCode.UInt64:
					return 1;
				case TypeCode.Single:
					return 8;
				case TypeCode.Double:
					return 3;
				case TypeCode.Decimal:
					return 4;
				case TypeCode.String:
					return 6;
				}
				return 100;
			case TypeCode.Int64:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 2);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 8;
				case TypeCode.Byte:
					return 13;
				case TypeCode.Int16:
					return 7;
				case TypeCode.UInt16:
					return 11;
				case TypeCode.Int32:
					return 6;
				case TypeCode.UInt32:
					return 10;
				case TypeCode.Int64:
					return 0;
				case TypeCode.UInt64:
					return 9;
				case TypeCode.Single:
					return 5;
				case TypeCode.Double:
					return 4;
				case TypeCode.Decimal:
					return 1;
				case TypeCode.String:
					return 3;
				}
				return 100;
			case TypeCode.UInt64:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 2);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 13;
				case TypeCode.Byte:
					return 10;
				case TypeCode.Int16:
					return 12;
				case TypeCode.UInt16:
					return 8;
				case TypeCode.Int32:
					return 11;
				case TypeCode.UInt32:
					return 7;
				case TypeCode.Int64:
					return 4;
				case TypeCode.UInt64:
					return 0;
				case TypeCode.Single:
					return 6;
				case TypeCode.Double:
					return 5;
				case TypeCode.Decimal:
					return 1;
				case TypeCode.String:
					return 3;
				}
				return 100;
			case TypeCode.Single:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 12);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 10;
				case TypeCode.Byte:
					return 11;
				case TypeCode.Int16:
					return 7;
				case TypeCode.UInt16:
					return 8;
				case TypeCode.Int32:
					return 5;
				case TypeCode.UInt32:
					return 6;
				case TypeCode.Int64:
					return 3;
				case TypeCode.UInt64:
					return 4;
				case TypeCode.Single:
					return 0;
				case TypeCode.Double:
					return 1;
				case TypeCode.Decimal:
					return 2;
				case TypeCode.String:
					return 13;
				}
				return 100;
			case TypeCode.Double:
				switch (typeCode2)
				{
				case TypeCode.Object:
					return JSBinder.TypeDistance(typeRefs, formal, actual, 12);
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 10;
				case TypeCode.Byte:
					return 11;
				case TypeCode.Int16:
					return 7;
				case TypeCode.UInt16:
					return 8;
				case TypeCode.Int32:
					return 5;
				case TypeCode.UInt32:
					return 6;
				case TypeCode.Int64:
					return 3;
				case TypeCode.UInt64:
					return 4;
				case TypeCode.Single:
					return 2;
				case TypeCode.Double:
					return 0;
				case TypeCode.Decimal:
					return 1;
				case TypeCode.String:
					return 13;
				}
				return 100;
			case TypeCode.Decimal:
				switch (typeCode2)
				{
				case TypeCode.Object:
					if (formal != typeRefs.Object)
					{
						return 100;
					}
					return 12;
				case TypeCode.Boolean:
					return 14;
				case TypeCode.SByte:
					return 10;
				case TypeCode.Byte:
					return 11;
				case TypeCode.Int16:
					return 7;
				case TypeCode.UInt16:
					return 8;
				case TypeCode.Int32:
					return 5;
				case TypeCode.UInt32:
					return 6;
				case TypeCode.Int64:
					return 3;
				case TypeCode.UInt64:
					return 4;
				case TypeCode.Single:
					return 2;
				case TypeCode.Double:
					return 1;
				case TypeCode.Decimal:
					return 0;
				case TypeCode.String:
					return 13;
				}
				return 100;
			case TypeCode.DateTime:
			{
				TypeCode typeCode3 = typeCode2;
				if (typeCode3 != TypeCode.Object)
				{
					switch (typeCode3)
					{
					case TypeCode.Int32:
						return 9;
					case TypeCode.UInt32:
						return 8;
					case TypeCode.Int64:
						return 7;
					case TypeCode.UInt64:
						return 6;
					case TypeCode.Double:
						return 4;
					case TypeCode.Decimal:
						return 5;
					case TypeCode.DateTime:
						return 0;
					case TypeCode.String:
						return 3;
					}
					return 100;
				}
				if (formal != typeRefs.Object)
				{
					return 100;
				}
				return 1;
			}
			case TypeCode.String:
			{
				TypeCode typeCode4 = typeCode2;
				if (typeCode4 != TypeCode.Object)
				{
					if (typeCode4 == TypeCode.Char)
					{
						return 2;
					}
					if (typeCode4 == TypeCode.String)
					{
						return 0;
					}
					return 100;
				}
				else
				{
					if (formal != typeRefs.Object)
					{
						return 100;
					}
					return 1;
				}
				break;
			}
			}
			return 0;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00030BC7 File Offset: 0x0002FBC7
		private static int TypeDistance(TypeReferences typeRefs, Type formal, Type actual, int distFromObject)
		{
			if (formal == typeRefs.Object)
			{
				return distFromObject;
			}
			if (formal.IsEnum)
			{
				return JSBinder.TypeDistance(typeRefs, Enum.GetUnderlyingType(formal), actual) + 10;
			}
			return 100;
		}

		// Token: 0x04000310 RID: 784
		internal static readonly JSBinder ob = new JSBinder();
	}
}
