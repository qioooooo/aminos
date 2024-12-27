using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000A3 RID: 163
	[Serializable]
	internal class DefaultBinder : Binder
	{
		// Token: 0x06000A00 RID: 2560 RVA: 0x0001DC08 File Offset: 0x0001CC08
		public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] canidates, ref object[] args, ParameterModifier[] modifiers, CultureInfo cultureInfo, string[] names, out object state)
		{
			state = null;
			if (canidates == null || canidates.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EmptyArray"), "canidates");
			}
			int[][] array = new int[canidates.Length][];
			int i;
			for (i = 0; i < canidates.Length; i++)
			{
				ParameterInfo[] parametersNoCopy = canidates[i].GetParametersNoCopy();
				array[i] = new int[(parametersNoCopy.Length > args.Length) ? parametersNoCopy.Length : args.Length];
				if (names == null)
				{
					for (int j = 0; j < args.Length; j++)
					{
						array[i][j] = j;
					}
				}
				else if (!DefaultBinder.CreateParamOrder(array[i], parametersNoCopy, names))
				{
					canidates[i] = null;
				}
			}
			Type[] array2 = new Type[canidates.Length];
			Type[] array3 = new Type[args.Length];
			for (i = 0; i < args.Length; i++)
			{
				if (args[i] != null)
				{
					array3[i] = args[i].GetType();
				}
			}
			int num = 0;
			bool flag = (bindingAttr & BindingFlags.OptionalParamBinding) != BindingFlags.Default;
			for (i = 0; i < canidates.Length; i++)
			{
				Type type = null;
				if (canidates[i] != null)
				{
					ParameterInfo[] parametersNoCopy2 = canidates[i].GetParametersNoCopy();
					if (parametersNoCopy2.Length == 0)
					{
						if (args.Length == 0 || (canidates[i].CallingConvention & CallingConventions.VarArgs) != (CallingConventions)0)
						{
							array[num] = array[i];
							canidates[num++] = canidates[i];
						}
					}
					else
					{
						int j;
						if (parametersNoCopy2.Length > args.Length)
						{
							j = args.Length;
							while (j < parametersNoCopy2.Length - 1 && parametersNoCopy2[j].DefaultValue != DBNull.Value)
							{
								j++;
							}
							if (j != parametersNoCopy2.Length - 1)
							{
								goto IL_03E2;
							}
							if (parametersNoCopy2[j].DefaultValue == DBNull.Value)
							{
								if (!parametersNoCopy2[j].ParameterType.IsArray || !parametersNoCopy2[j].IsDefined(typeof(ParamArrayAttribute), true))
								{
									goto IL_03E2;
								}
								type = parametersNoCopy2[j].ParameterType.GetElementType();
							}
						}
						else if (parametersNoCopy2.Length < args.Length)
						{
							int num2 = parametersNoCopy2.Length - 1;
							if (!parametersNoCopy2[num2].ParameterType.IsArray || !parametersNoCopy2[num2].IsDefined(typeof(ParamArrayAttribute), true) || array[i][num2] != num2)
							{
								goto IL_03E2;
							}
							type = parametersNoCopy2[num2].ParameterType.GetElementType();
						}
						else
						{
							int num3 = parametersNoCopy2.Length - 1;
							if (parametersNoCopy2[num3].ParameterType.IsArray && parametersNoCopy2[num3].IsDefined(typeof(ParamArrayAttribute), true) && array[i][num3] == num3 && !parametersNoCopy2[num3].ParameterType.IsAssignableFrom(array3[num3]))
							{
								type = parametersNoCopy2[num3].ParameterType.GetElementType();
							}
						}
						int num4 = ((type != null) ? (parametersNoCopy2.Length - 1) : args.Length);
						for (j = 0; j < num4; j++)
						{
							Type type2 = parametersNoCopy2[j].ParameterType;
							if (type2.IsByRef)
							{
								type2 = type2.GetElementType();
							}
							if (type2 != array3[array[i][j]] && (!flag || args[array[i][j]] != Type.Missing) && args[array[i][j]] != null && type2 != typeof(object))
							{
								if (type2.IsPrimitive)
								{
									if (array3[array[i][j]] == null)
									{
										break;
									}
									if (!DefaultBinder.CanConvertPrimitiveObjectToType(args[array[i][j]], (RuntimeType)type2))
									{
										break;
									}
								}
								else if (array3[array[i][j]] != null && !type2.IsAssignableFrom(array3[array[i][j]]) && (!array3[array[i][j]].IsCOMObject || !type2.IsInstanceOfType(args[array[i][j]])))
								{
									break;
								}
							}
						}
						if (type != null && j == parametersNoCopy2.Length - 1)
						{
							while (j < args.Length)
							{
								if (type.IsPrimitive)
								{
									if (array3[j] == null)
									{
										break;
									}
									if (!DefaultBinder.CanConvertPrimitiveObjectToType(args[j], (RuntimeType)type))
									{
										break;
									}
								}
								else if (array3[j] != null && !type.IsAssignableFrom(array3[j]) && (!array3[j].IsCOMObject || !type.IsInstanceOfType(args[j])))
								{
									break;
								}
								j++;
							}
						}
						if (j == args.Length)
						{
							array[num] = array[i];
							array2[num] = type;
							canidates[num++] = canidates[i];
						}
					}
				}
				IL_03E2:;
			}
			if (num == 0)
			{
				throw new MissingMethodException(Environment.GetResourceString("MissingMember"));
			}
			if (num == 1)
			{
				if (names != null)
				{
					state = new DefaultBinder.BinderState((int[])array[0].Clone(), args.Length, array2[0] != null);
					DefaultBinder.ReorderParams(array[0], args);
				}
				ParameterInfo[] parametersNoCopy3 = canidates[0].GetParametersNoCopy();
				if (parametersNoCopy3.Length == args.Length)
				{
					if (array2[0] != null)
					{
						object[] array4 = new object[parametersNoCopy3.Length];
						int num5 = parametersNoCopy3.Length - 1;
						Array.Copy(args, 0, array4, 0, num5);
						array4[num5] = Array.CreateInstance(array2[0], 1);
						((Array)array4[num5]).SetValue(args[num5], 0);
						args = array4;
					}
				}
				else if (parametersNoCopy3.Length > args.Length)
				{
					object[] array5 = new object[parametersNoCopy3.Length];
					for (i = 0; i < args.Length; i++)
					{
						array5[i] = args[i];
					}
					while (i < parametersNoCopy3.Length - 1)
					{
						array5[i] = parametersNoCopy3[i].DefaultValue;
						i++;
					}
					if (array2[0] != null)
					{
						array5[i] = Array.CreateInstance(array2[0], 0);
					}
					else
					{
						array5[i] = parametersNoCopy3[i].DefaultValue;
					}
					args = array5;
				}
				else if ((canidates[0].CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
				{
					object[] array6 = new object[parametersNoCopy3.Length];
					int num6 = parametersNoCopy3.Length - 1;
					Array.Copy(args, 0, array6, 0, num6);
					array6[num6] = Array.CreateInstance(array2[0], args.Length - num6);
					Array.Copy(args, num6, (Array)array6[num6], 0, args.Length - num6);
					args = array6;
				}
				return canidates[0];
			}
			int num7 = 0;
			bool flag2 = false;
			for (i = 1; i < num; i++)
			{
				int num8 = DefaultBinder.FindMostSpecificMethod(canidates[num7], array[num7], array2[num7], canidates[i], array[i], array2[i], array3, args);
				if (num8 == 0)
				{
					flag2 = true;
				}
				else if (num8 == 2)
				{
					num7 = i;
					flag2 = false;
				}
			}
			if (flag2)
			{
				throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
			}
			if (names != null)
			{
				state = new DefaultBinder.BinderState((int[])array[num7].Clone(), args.Length, array2[num7] != null);
				DefaultBinder.ReorderParams(array[num7], args);
			}
			ParameterInfo[] parametersNoCopy4 = canidates[num7].GetParametersNoCopy();
			if (parametersNoCopy4.Length == args.Length)
			{
				if (array2[num7] != null)
				{
					object[] array7 = new object[parametersNoCopy4.Length];
					int num9 = parametersNoCopy4.Length - 1;
					Array.Copy(args, 0, array7, 0, num9);
					array7[num9] = Array.CreateInstance(array2[num7], 1);
					((Array)array7[num9]).SetValue(args[num9], 0);
					args = array7;
				}
			}
			else if (parametersNoCopy4.Length > args.Length)
			{
				object[] array8 = new object[parametersNoCopy4.Length];
				for (i = 0; i < args.Length; i++)
				{
					array8[i] = args[i];
				}
				while (i < parametersNoCopy4.Length - 1)
				{
					array8[i] = parametersNoCopy4[i].DefaultValue;
					i++;
				}
				if (array2[num7] != null)
				{
					array8[i] = Array.CreateInstance(array2[num7], 0);
				}
				else
				{
					array8[i] = parametersNoCopy4[i].DefaultValue;
				}
				args = array8;
			}
			else if ((canidates[num7].CallingConvention & CallingConventions.VarArgs) == (CallingConventions)0)
			{
				object[] array9 = new object[parametersNoCopy4.Length];
				int num10 = parametersNoCopy4.Length - 1;
				Array.Copy(args, 0, array9, 0, num10);
				array9[i] = Array.CreateInstance(array2[num7], args.Length - num10);
				Array.Copy(args, num10, (Array)array9[i], 0, args.Length - num10);
				args = array9;
			}
			return canidates[num7];
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001E368 File Offset: 0x0001D368
		public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo cultureInfo)
		{
			int num = 0;
			if ((bindingAttr & BindingFlags.SetField) != BindingFlags.Default)
			{
				Type type = value.GetType();
				for (int i = 0; i < match.Length; i++)
				{
					Type fieldType = match[i].FieldType;
					if (fieldType == type)
					{
						match[num++] = match[i];
					}
					else if (value == Empty.Value && fieldType.IsClass)
					{
						match[num++] = match[i];
					}
					else if (fieldType == typeof(object))
					{
						match[num++] = match[i];
					}
					else if (fieldType.IsPrimitive)
					{
						if (DefaultBinder.CanConvertPrimitiveObjectToType(value, (RuntimeType)fieldType))
						{
							match[num++] = match[i];
						}
					}
					else if (fieldType.IsAssignableFrom(type))
					{
						match[num++] = match[i];
					}
				}
				if (num == 0)
				{
					throw new MissingFieldException(Environment.GetResourceString("MissingField"));
				}
				if (num == 1)
				{
					return match[0];
				}
			}
			int num2 = 0;
			bool flag = false;
			for (int i = 1; i < num; i++)
			{
				int num3 = DefaultBinder.FindMostSpecificField(match[num2], match[i]);
				if (num3 == 0)
				{
					flag = true;
				}
				else if (num3 == 2)
				{
					num2 = i;
					flag = false;
				}
			}
			if (flag)
			{
				throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
			}
			return match[num2];
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001E48C File Offset: 0x0001D48C
		public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
		{
			Type[] array = new Type[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				array[i] = types[i].UnderlyingSystemType;
				if (!(array[i] is RuntimeType))
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "types");
				}
			}
			types = array;
			if (match == null || match.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EmptyArray"), "match");
			}
			int num = 0;
			for (int i = 0; i < match.Length; i++)
			{
				ParameterInfo[] parametersNoCopy = match[i].GetParametersNoCopy();
				if (parametersNoCopy.Length == types.Length)
				{
					int j;
					for (j = 0; j < types.Length; j++)
					{
						Type parameterType = parametersNoCopy[j].ParameterType;
						if (parameterType != types[j] && parameterType != typeof(object))
						{
							if (parameterType.IsPrimitive)
							{
								if (!(types[j].UnderlyingSystemType is RuntimeType))
								{
									break;
								}
								if (!DefaultBinder.CanConvertPrimitive((RuntimeType)types[j].UnderlyingSystemType, (RuntimeType)parameterType.UnderlyingSystemType))
								{
									break;
								}
							}
							else if (!parameterType.IsAssignableFrom(types[j]))
							{
								break;
							}
						}
					}
					if (j == types.Length)
					{
						match[num++] = match[i];
					}
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0];
			}
			int num2 = 0;
			bool flag = false;
			int[] array2 = new int[types.Length];
			for (int i = 0; i < types.Length; i++)
			{
				array2[i] = i;
			}
			for (int i = 1; i < num; i++)
			{
				int num3 = DefaultBinder.FindMostSpecificMethod(match[num2], array2, null, match[i], array2, null, types, null);
				if (num3 == 0)
				{
					flag = true;
				}
				else if (num3 == 2)
				{
					flag = false;
					num2 = i;
				}
			}
			if (flag)
			{
				throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
			}
			return match[num2];
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001E62C File Offset: 0x0001D62C
		public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
		{
			int i = 0;
			if (match == null || match.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EmptyArray"), "match");
			}
			int num = 0;
			int num2 = ((indexes != null) ? indexes.Length : 0);
			int j = 0;
			while (j < match.Length)
			{
				if (indexes == null)
				{
					goto IL_00C5;
				}
				ParameterInfo[] indexParameters = match[j].GetIndexParameters();
				if (indexParameters.Length == num2)
				{
					for (i = 0; i < num2; i++)
					{
						Type parameterType = indexParameters[i].ParameterType;
						if (parameterType != indexes[i] && parameterType != typeof(object))
						{
							if (parameterType.IsPrimitive)
							{
								if (!(indexes[i].UnderlyingSystemType is RuntimeType))
								{
									break;
								}
								if (!DefaultBinder.CanConvertPrimitive((RuntimeType)indexes[i].UnderlyingSystemType, (RuntimeType)parameterType.UnderlyingSystemType))
								{
									break;
								}
							}
							else if (!parameterType.IsAssignableFrom(indexes[i]))
							{
								break;
							}
						}
					}
					goto IL_00C5;
				}
				IL_0128:
				j++;
				continue;
				IL_00C5:
				if (i == num2)
				{
					if (returnType != null)
					{
						if (match[j].PropertyType.IsPrimitive)
						{
							if (!(returnType.UnderlyingSystemType is RuntimeType))
							{
								goto IL_0128;
							}
							if (!DefaultBinder.CanConvertPrimitive((RuntimeType)returnType.UnderlyingSystemType, (RuntimeType)match[j].PropertyType.UnderlyingSystemType))
							{
								goto IL_0128;
							}
						}
						else if (!match[j].PropertyType.IsAssignableFrom(returnType))
						{
							goto IL_0128;
						}
					}
					match[num++] = match[j];
					goto IL_0128;
				}
				goto IL_0128;
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return match[0];
			}
			int num3 = 0;
			bool flag = false;
			int[] array = new int[num2];
			for (j = 0; j < num2; j++)
			{
				array[j] = j;
			}
			for (j = 1; j < num; j++)
			{
				int num4 = DefaultBinder.FindMostSpecificType(match[num3].PropertyType, match[j].PropertyType, returnType);
				if (num4 == 0 && indexes != null)
				{
					num4 = DefaultBinder.FindMostSpecific(match[num3].GetIndexParameters(), array, null, match[j].GetIndexParameters(), array, null, indexes, null);
				}
				if (num4 == 0)
				{
					num4 = DefaultBinder.FindMostSpecificProperty(match[num3], match[j]);
					if (num4 == 0)
					{
						flag = true;
					}
				}
				if (num4 == 2)
				{
					flag = false;
					num3 = j;
				}
			}
			if (flag)
			{
				throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
			}
			return match[num3];
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0001E824 File Offset: 0x0001D824
		public override object ChangeType(object value, Type type, CultureInfo cultureInfo)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_ChangeType"));
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0001E838 File Offset: 0x0001D838
		public override void ReorderArgumentArray(ref object[] args, object state)
		{
			DefaultBinder.BinderState binderState = (DefaultBinder.BinderState)state;
			DefaultBinder.ReorderParams(binderState.m_argsMap, args);
			if (!binderState.m_isParamArray)
			{
				if (args.Length > binderState.m_originalSize)
				{
					object[] array = new object[binderState.m_originalSize];
					Array.Copy(args, 0, array, 0, binderState.m_originalSize);
					args = array;
				}
				return;
			}
			int num = args.Length - 1;
			if (args.Length == binderState.m_originalSize)
			{
				args[num] = ((object[])args[num])[0];
				return;
			}
			object[] array2 = new object[args.Length];
			Array.Copy(args, 0, array2, 0, num);
			int i = num;
			int num2 = 0;
			while (i < array2.Length)
			{
				array2[i] = ((object[])args[num])[num2];
				i++;
				num2++;
			}
			args = array2;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0001E8F4 File Offset: 0x0001D8F4
		public static MethodBase ExactBinding(MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			MethodBase[] array = new MethodBase[match.Length];
			int num = 0;
			for (int i = 0; i < match.Length; i++)
			{
				ParameterInfo[] parametersNoCopy = match[i].GetParametersNoCopy();
				if (parametersNoCopy.Length != 0)
				{
					int j;
					for (j = 0; j < types.Length; j++)
					{
						Type parameterType = parametersNoCopy[j].ParameterType;
						if (!parameterType.Equals(types[j]))
						{
							break;
						}
					}
					if (j >= types.Length)
					{
						array[num] = match[i];
						num++;
					}
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (num == 1)
			{
				return array[0];
			}
			return DefaultBinder.FindMostDerivedNewSlotMeth(array, num);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0001E988 File Offset: 0x0001D988
		public static PropertyInfo ExactPropertyBinding(PropertyInfo[] match, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match");
			}
			PropertyInfo propertyInfo = null;
			int num = ((types != null) ? types.Length : 0);
			for (int i = 0; i < match.Length; i++)
			{
				ParameterInfo[] indexParameters = match[i].GetIndexParameters();
				int j;
				for (j = 0; j < num; j++)
				{
					Type parameterType = indexParameters[j].ParameterType;
					if (parameterType != types[j])
					{
						break;
					}
				}
				if (j >= num && (returnType == null || returnType == match[i].PropertyType))
				{
					if (propertyInfo != null)
					{
						throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
					}
					propertyInfo = match[i];
				}
			}
			return propertyInfo;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0001EA14 File Offset: 0x0001DA14
		private static int FindMostSpecific(ParameterInfo[] p1, int[] paramOrder1, Type paramArrayType1, ParameterInfo[] p2, int[] paramOrder2, Type paramArrayType2, Type[] types, object[] args)
		{
			if (paramArrayType1 != null && paramArrayType2 == null)
			{
				return 2;
			}
			if (paramArrayType2 != null && paramArrayType1 == null)
			{
				return 1;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < types.Length; i++)
			{
				if (args == null || args[i] != Type.Missing)
				{
					Type type = p1[paramOrder1[i]].ParameterType;
					Type type2 = p2[paramOrder2[i]].ParameterType;
					if (i == p1.Length - 1 && paramOrder1[i] == p1.Length - 1 && paramArrayType1 != null)
					{
						type = paramArrayType1;
					}
					if (i == p2.Length - 1 && paramOrder2[i] == p2.Length - 1 && paramArrayType2 != null)
					{
						type2 = paramArrayType2;
					}
					if (type != type2)
					{
						switch (DefaultBinder.FindMostSpecificType(type, type2, types[i]))
						{
						case 0:
							return 0;
						case 1:
							flag = true;
							break;
						case 2:
							flag2 = true;
							break;
						}
					}
				}
			}
			if (flag == flag2)
			{
				if (!flag && p1.Length != p2.Length && args != null)
				{
					if (p1.Length == args.Length)
					{
						return 1;
					}
					if (p2.Length == args.Length)
					{
						return 2;
					}
				}
				return 0;
			}
			if (!flag)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001EB08 File Offset: 0x0001DB08
		private static int FindMostSpecificType(Type c1, Type c2, Type t)
		{
			if (c1 == c2)
			{
				return 0;
			}
			if (c1 == t)
			{
				return 1;
			}
			if (c2 == t)
			{
				return 2;
			}
			if (c1.IsByRef || c2.IsByRef)
			{
				if (c1.IsByRef && c2.IsByRef)
				{
					c1 = c1.GetElementType();
					c2 = c2.GetElementType();
				}
				else if (c1.IsByRef)
				{
					if (c1.GetElementType() == c2)
					{
						return 2;
					}
					c1 = c1.GetElementType();
				}
				else
				{
					if (c2.GetElementType() == c1)
					{
						return 1;
					}
					c2 = c2.GetElementType();
				}
			}
			bool flag;
			bool flag2;
			if (c1.IsPrimitive && c2.IsPrimitive)
			{
				flag = DefaultBinder.CanConvertPrimitive((RuntimeType)c2, (RuntimeType)c1);
				flag2 = DefaultBinder.CanConvertPrimitive((RuntimeType)c1, (RuntimeType)c2);
			}
			else
			{
				flag = c1.IsAssignableFrom(c2);
				flag2 = c2.IsAssignableFrom(c1);
			}
			if (flag == flag2)
			{
				return 0;
			}
			if (flag)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001EBDC File Offset: 0x0001DBDC
		private static int FindMostSpecificMethod(MethodBase m1, int[] paramOrder1, Type paramArrayType1, MethodBase m2, int[] paramOrder2, Type paramArrayType2, Type[] types, object[] args)
		{
			int num = DefaultBinder.FindMostSpecific(m1.GetParametersNoCopy(), paramOrder1, paramArrayType1, m2.GetParametersNoCopy(), paramOrder2, paramArrayType2, types, args);
			if (num != 0)
			{
				return num;
			}
			if (!DefaultBinder.CompareMethodSigAndName(m1, m2))
			{
				return 0;
			}
			int hierarchyDepth = DefaultBinder.GetHierarchyDepth(m1.DeclaringType);
			int hierarchyDepth2 = DefaultBinder.GetHierarchyDepth(m2.DeclaringType);
			if (hierarchyDepth == hierarchyDepth2)
			{
				return 0;
			}
			if (hierarchyDepth < hierarchyDepth2)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0001EC3C File Offset: 0x0001DC3C
		private static int FindMostSpecificField(FieldInfo cur1, FieldInfo cur2)
		{
			if (!(cur1.Name == cur2.Name))
			{
				return 0;
			}
			int hierarchyDepth = DefaultBinder.GetHierarchyDepth(cur1.DeclaringType);
			int hierarchyDepth2 = DefaultBinder.GetHierarchyDepth(cur2.DeclaringType);
			if (hierarchyDepth == hierarchyDepth2)
			{
				return 0;
			}
			if (hierarchyDepth < hierarchyDepth2)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0001EC84 File Offset: 0x0001DC84
		private static int FindMostSpecificProperty(PropertyInfo cur1, PropertyInfo cur2)
		{
			if (!(cur1.Name == cur2.Name))
			{
				return 0;
			}
			int hierarchyDepth = DefaultBinder.GetHierarchyDepth(cur1.DeclaringType);
			int hierarchyDepth2 = DefaultBinder.GetHierarchyDepth(cur2.DeclaringType);
			if (hierarchyDepth == hierarchyDepth2)
			{
				return 0;
			}
			if (hierarchyDepth < hierarchyDepth2)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0001ECCC File Offset: 0x0001DCCC
		internal static bool CompareMethodSigAndName(MethodBase m1, MethodBase m2)
		{
			ParameterInfo[] parametersNoCopy = m1.GetParametersNoCopy();
			ParameterInfo[] parametersNoCopy2 = m2.GetParametersNoCopy();
			if (parametersNoCopy.Length != parametersNoCopy2.Length)
			{
				return false;
			}
			int num = parametersNoCopy.Length;
			for (int i = 0; i < num; i++)
			{
				if (parametersNoCopy[i].ParameterType != parametersNoCopy2[i].ParameterType)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0001ED18 File Offset: 0x0001DD18
		internal static int GetHierarchyDepth(Type t)
		{
			int num = 0;
			Type type = t;
			do
			{
				num++;
				type = type.BaseType;
			}
			while (type != null);
			return num;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0001ED38 File Offset: 0x0001DD38
		internal static MethodBase FindMostDerivedNewSlotMeth(MethodBase[] match, int cMatches)
		{
			int num = 0;
			MethodBase methodBase = null;
			for (int i = 0; i < cMatches; i++)
			{
				int hierarchyDepth = DefaultBinder.GetHierarchyDepth(match[i].DeclaringType);
				if (hierarchyDepth == num)
				{
					throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.Ambiguous"));
				}
				if (hierarchyDepth > num)
				{
					num = hierarchyDepth;
					methodBase = match[i];
				}
			}
			return methodBase;
		}

		// Token: 0x06000A10 RID: 2576
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool CanConvertPrimitive(RuntimeType source, RuntimeType target);

		// Token: 0x06000A11 RID: 2577
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool CanConvertPrimitiveObjectToType(object source, RuntimeType type);

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001ED84 File Offset: 0x0001DD84
		private static void ReorderParams(int[] paramOrder, object[] vars)
		{
			for (int i = 0; i < vars.Length; i++)
			{
				while (paramOrder[i] != i)
				{
					int num = paramOrder[paramOrder[i]];
					object obj = vars[paramOrder[i]];
					paramOrder[paramOrder[i]] = paramOrder[i];
					vars[paramOrder[i]] = vars[i];
					paramOrder[i] = num;
					vars[i] = obj;
				}
			}
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0001EDCC File Offset: 0x0001DDCC
		private static bool CreateParamOrder(int[] paramOrder, ParameterInfo[] pars, string[] names)
		{
			bool[] array = new bool[pars.Length];
			for (int i = 0; i < pars.Length; i++)
			{
				paramOrder[i] = -1;
			}
			for (int j = 0; j < names.Length; j++)
			{
				int k;
				for (k = 0; k < pars.Length; k++)
				{
					if (names[j].Equals(pars[k].Name))
					{
						paramOrder[k] = j;
						array[j] = true;
						break;
					}
				}
				if (k == pars.Length)
				{
					return false;
				}
			}
			int l = 0;
			for (int m = 0; m < pars.Length; m++)
			{
				if (paramOrder[m] == -1)
				{
					while (l < pars.Length)
					{
						if (!array[l])
						{
							paramOrder[m] = l;
							l++;
							break;
						}
						l++;
					}
				}
			}
			return true;
		}

		// Token: 0x020000A4 RID: 164
		internal class BinderState
		{
			// Token: 0x06000A15 RID: 2581 RVA: 0x0001EE7C File Offset: 0x0001DE7C
			internal BinderState(int[] argsMap, int originalSize, bool isParamArray)
			{
				this.m_argsMap = argsMap;
				this.m_originalSize = originalSize;
				this.m_isParamArray = isParamArray;
			}

			// Token: 0x0400038C RID: 908
			internal int[] m_argsMap;

			// Token: 0x0400038D RID: 909
			internal int m_originalSize;

			// Token: 0x0400038E RID: 910
			internal bool m_isParamArray;
		}
	}
}
