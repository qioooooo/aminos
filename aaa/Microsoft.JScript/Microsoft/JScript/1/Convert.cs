using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000054 RID: 84
	public sealed class Convert
	{
		// Token: 0x060003DC RID: 988 RVA: 0x00018460 File Offset: 0x00017460
		public static bool IsBadIndex(AST ast)
		{
			if (!(ast is ConstantWrapper))
			{
				return false;
			}
			int num;
			try
			{
				num = (int)Convert.CoerceT(((ConstantWrapper)ast).value, typeof(int));
			}
			catch
			{
				return true;
			}
			return num < 0;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000184B4 File Offset: 0x000174B4
		public static double CheckIfDoubleIsInteger(double d)
		{
			if (d == Math.Round(d))
			{
				return d;
			}
			throw new JScriptException(JSError.TypeMismatch);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x000184C8 File Offset: 0x000174C8
		public static float CheckIfSingleIsInteger(float s)
		{
			if ((double)s == Math.Round((double)s))
			{
				return s;
			}
			throw new JScriptException(JSError.TypeMismatch);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000184DE File Offset: 0x000174DE
		public static object Coerce(object value, object type)
		{
			return Convert.Coerce(value, type, false);
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000184E8 File Offset: 0x000174E8
		internal static object Coerce(object value, object type, bool explicitOK)
		{
			TypeExpression typeExpression = type as TypeExpression;
			if (typeExpression != null)
			{
				type = typeExpression.ToIReflect();
			}
			TypedArray typedArray = type as TypedArray;
			if (typedArray != null)
			{
				IReflect elementType = typedArray.elementType;
				int rank = typedArray.rank;
				Type type2 = ((elementType is Type) ? ((Type)elementType) : ((elementType is ClassScope) ? ((ClassScope)elementType).GetBakedSuperType() : typeof(object)));
				ArrayObject arrayObject = value as ArrayObject;
				if (arrayObject != null)
				{
					return arrayObject.ToNativeArray(type2);
				}
				Array array = value as Array;
				if (array != null && array.Rank == rank)
				{
					type = Convert.ToType(TypedArray.ToRankString(rank), type2);
				}
				if (value == null || value is DBNull)
				{
					return null;
				}
			}
			ClassScope classScope = type as ClassScope;
			if (classScope == null)
			{
				if (!(type is Type))
				{
					type = Convert.ToType(Runtime.TypeRefs, (IReflect)type);
				}
				else
				{
					if (type == typeof(Type) && value is ClassScope)
					{
						return value;
					}
					if (((Type)type).IsEnum)
					{
						EnumWrapper enumWrapper = value as EnumWrapper;
						if (enumWrapper == null)
						{
							Type type3 = type as Type;
							return MetadataEnumValue.GetEnumValue(type3, Convert.CoerceT(value, Convert.GetUnderlyingType(type3), explicitOK));
						}
						if (enumWrapper.classScopeOrType == type)
						{
							return value;
						}
						throw new JScriptException(JSError.TypeMismatch);
					}
				}
				return Convert.CoerceT(value, (Type)type, explicitOK);
			}
			if (classScope.HasInstance(value))
			{
				return value;
			}
			EnumDeclaration enumDeclaration = classScope.owner as EnumDeclaration;
			if (enumDeclaration != null)
			{
				EnumWrapper enumWrapper2 = value as EnumWrapper;
				if (enumWrapper2 == null)
				{
					return new DeclaredEnumValue(Convert.Coerce(value, enumDeclaration.baseType), null, classScope);
				}
				if (enumWrapper2.classScopeOrType == classScope)
				{
					return value;
				}
				throw new JScriptException(JSError.TypeMismatch);
			}
			else
			{
				if (value == null || value is DBNull)
				{
					return null;
				}
				throw new JScriptException(JSError.TypeMismatch);
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001869C File Offset: 0x0001769C
		internal static object CoerceT(object value, Type type)
		{
			return Convert.CoerceT(value, type, false);
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000186A8 File Offset: 0x000176A8
		public static object CoerceT(object value, Type t, bool explicitOK)
		{
			if (t == typeof(object))
			{
				return value;
			}
			if (t == typeof(string) && value is string)
			{
				return value;
			}
			if (t.IsEnum && !(t is EnumBuilder) && !(t is TypeBuilder))
			{
				IConvertible iconvertible = Convert.GetIConvertible(value);
				TypeCode typeCode = Convert.GetTypeCode(value, iconvertible);
				if (typeCode == TypeCode.String)
				{
					return Enum.Parse(t, iconvertible.ToString(CultureInfo.InvariantCulture));
				}
				if (!explicitOK && typeCode != TypeCode.Empty)
				{
					Type type = value.GetType();
					if (type.IsEnum)
					{
						if (type != t)
						{
							throw new JScriptException(JSError.TypeMismatch);
						}
						return value;
					}
				}
				return Enum.ToObject(t, Convert.CoerceT(value, Convert.GetUnderlyingType(t), explicitOK));
			}
			else
			{
				TypeCode typeCode2 = Type.GetTypeCode(t);
				if (typeCode2 != TypeCode.Object)
				{
					return Convert.Coerce2(value, typeCode2, explicitOK);
				}
				if (value is ConcatString)
				{
					value = value.ToString();
				}
				if (value == null || (value == DBNull.Value && t != typeof(object)) || value is Missing || value is Missing)
				{
					if (!t.IsValueType)
					{
						return null;
					}
					if (!t.IsPublic && t.Assembly == typeof(ActiveXObjectConstructor).Assembly)
					{
						throw new JScriptException(JSError.CantCreateObject);
					}
					return Activator.CreateInstance(t);
				}
				else
				{
					if (t.IsAssignableFrom(value.GetType()))
					{
						return value;
					}
					if (typeof(Delegate).IsAssignableFrom(t))
					{
						if (value is Closure)
						{
							return ((Closure)value).ConvertToDelegate(t);
						}
						if (value is FunctionWrapper)
						{
							return ((FunctionWrapper)value).ConvertToDelegate(t);
						}
						if (value is FunctionObject)
						{
							return value;
						}
					}
					else
					{
						if (value is ArrayObject && typeof(Array).IsAssignableFrom(t))
						{
							return ((ArrayObject)value).ToNativeArray(t.GetElementType());
						}
						if (value is Array && t == typeof(ArrayObject) && ((Array)value).Rank == 1)
						{
							if (Globals.contextEngine == null)
							{
								Globals.contextEngine = new VsaEngine(true);
								Globals.contextEngine.InitVsaEngine("JS7://Microsoft.JScript.Vsa.VsaEngine", new DefaultVsaSite());
							}
							return Globals.contextEngine.GetOriginalArrayConstructor().ConstructWrapper((Array)value);
						}
						if (value is ClassScope && t == typeof(Type))
						{
							return ((ClassScope)value).GetTypeBuilderOrEnumBuilder();
						}
						if (value is TypedArray && t == typeof(Type))
						{
							return ((TypedArray)value).ToType();
						}
					}
					Type type2 = value.GetType();
					MethodInfo methodInfo;
					if (explicitOK)
					{
						methodInfo = t.GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type2 }, null);
						if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
						{
							methodInfo = new JSMethodInfo(methodInfo);
							return methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { value }, null);
						}
						methodInfo = Convert.GetToXXXXMethod(type2, t, explicitOK);
						if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
						{
							methodInfo = new JSMethodInfo(methodInfo);
							if (methodInfo.IsStatic)
							{
								return methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { value }, null);
							}
							return methodInfo.Invoke(value, BindingFlags.SuppressChangeType, null, new object[0], null);
						}
					}
					methodInfo = t.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type2 }, null);
					if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
					{
						methodInfo = new JSMethodInfo(methodInfo);
						return methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { value }, null);
					}
					methodInfo = Convert.GetToXXXXMethod(type2, t, false);
					if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
					{
						methodInfo = new JSMethodInfo(methodInfo);
						if (methodInfo.IsStatic)
						{
							return methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { value }, null);
						}
						return methodInfo.Invoke(value, BindingFlags.SuppressChangeType, null, new object[0], null);
					}
					else
					{
						if (t.IsByRef)
						{
							return Convert.CoerceT(value, t.GetElementType());
						}
						Type type3 = value.GetType();
						if (type3.IsCOMObject)
						{
							return value;
						}
						throw new JScriptException(JSError.TypeMismatch);
					}
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00018AC5 File Offset: 0x00017AC5
		public static object Coerce2(object value, TypeCode target, bool truncationPermitted)
		{
			if (truncationPermitted)
			{
				return Convert.Coerce2WithTruncationPermitted(value, target);
			}
			return Convert.Coerce2WithNoTrunctation(value, target);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00018ADC File Offset: 0x00017ADC
		private static object Coerce2WithNoTrunctation(object value, TypeCode target)
		{
			if (value is EnumWrapper)
			{
				value = ((EnumWrapper)value).value;
			}
			if (value is ConstantWrapper)
			{
				value = ((ConstantWrapper)value).value;
			}
			checked
			{
				try
				{
					IConvertible iconvertible = Convert.GetIConvertible(value);
					switch (Convert.GetTypeCode(value, iconvertible))
					{
					case TypeCode.Empty:
						break;
					case TypeCode.Object:
						if (!(value is Missing) && (!(value is Missing) || target == TypeCode.Object))
						{
							switch (target)
							{
							case TypeCode.Boolean:
								return Convert.ToBoolean(value, false);
							case TypeCode.Char:
							case TypeCode.SByte:
							case TypeCode.Byte:
							case TypeCode.Int16:
							case TypeCode.UInt16:
							case TypeCode.Int32:
							case TypeCode.UInt32:
							case TypeCode.Int64:
							case TypeCode.UInt64:
							case TypeCode.Single:
							case TypeCode.Double:
							case TypeCode.Decimal:
								return Convert.Coerce2WithNoTrunctation(Convert.ToNumber(value, iconvertible), target);
							case TypeCode.DateTime:
								if (value is DateObject)
								{
									return DatePrototype.getVarDate((DateObject)value);
								}
								return Convert.Coerce2WithNoTrunctation(Convert.ToNumber(value, iconvertible), target);
							case (TypeCode)17:
								goto IL_172C;
							case TypeCode.String:
								return Convert.ToString(value, iconvertible);
							default:
								goto IL_172C;
							}
						}
						break;
					case TypeCode.DBNull:
						switch (target)
						{
						case TypeCode.DBNull:
							return DBNull.Value;
						case TypeCode.Boolean:
							return false;
						case TypeCode.Char:
							return '\0';
						case TypeCode.SByte:
							return 0;
						case TypeCode.Byte:
							return 0;
						case TypeCode.Int16:
							return 0;
						case TypeCode.UInt16:
							return 0;
						case TypeCode.Int32:
							return 0;
						case TypeCode.UInt32:
							return 0U;
						case TypeCode.Int64:
							return 0L;
						case TypeCode.UInt64:
							return 0UL;
						case TypeCode.Single:
							return 0f;
						case TypeCode.Double:
							return 0.0;
						case TypeCode.Decimal:
							return 0m;
						case TypeCode.DateTime:
							return new DateTime(0L);
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return null;
						default:
							goto IL_172C;
						}
						break;
					case TypeCode.Boolean:
					{
						bool flag = iconvertible.ToBoolean(null);
						int num = (flag ? 1 : 0);
						switch (target)
						{
						case TypeCode.Boolean:
							return flag;
						case TypeCode.Char:
							return (char)num;
						case TypeCode.SByte:
							return (sbyte)num;
						case TypeCode.Byte:
							return (byte)num;
						case TypeCode.Int16:
							return (short)num;
						case TypeCode.UInt16:
							return (ushort)num;
						case TypeCode.Int32:
							return num;
						case TypeCode.UInt32:
							return (uint)num;
						case TypeCode.Int64:
							return unchecked((long)num);
						case TypeCode.UInt64:
							return (ulong)num;
						case TypeCode.Single:
							return (float)num;
						case TypeCode.Double:
							return (double)num;
						case TypeCode.Decimal:
							return num;
						case TypeCode.DateTime:
							return new DateTime(unchecked((long)num));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return flag ? "true" : "false";
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.Char:
					{
						char c = iconvertible.ToChar(null);
						ushort num2 = (ushort)c;
						switch (target)
						{
						case TypeCode.Boolean:
							return num2 != 0;
						case TypeCode.Char:
							return c;
						case TypeCode.SByte:
							return (sbyte)num2;
						case TypeCode.Byte:
							return (byte)num2;
						case TypeCode.Int16:
							return (short)num2;
						case TypeCode.UInt16:
							return num2;
						case TypeCode.Int32:
							return (int)num2;
						case TypeCode.UInt32:
							return (uint)num2;
						case TypeCode.Int64:
							return unchecked((long)num2);
						case TypeCode.UInt64:
							return unchecked((ulong)num2);
						case TypeCode.Single:
							return (float)num2;
						case TypeCode.Double:
							return (double)num2;
						case TypeCode.Decimal:
							return num2;
						case TypeCode.DateTime:
							return new DateTime(unchecked((long)num2));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return char.ToString(c);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.SByte:
					{
						sbyte b = iconvertible.ToSByte(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return b != 0;
						case TypeCode.Char:
							return (char)b;
						case TypeCode.SByte:
							return b;
						case TypeCode.Byte:
							return (byte)b;
						case TypeCode.Int16:
							return (short)b;
						case TypeCode.UInt16:
							return (ushort)b;
						case TypeCode.Int32:
							return (int)b;
						case TypeCode.UInt32:
							return (uint)b;
						case TypeCode.Int64:
							return unchecked((long)b);
						case TypeCode.UInt64:
							return (ulong)b;
						case TypeCode.Single:
							return (float)b;
						case TypeCode.Double:
							return (double)b;
						case TypeCode.Decimal:
							return b;
						case TypeCode.DateTime:
							return new DateTime(unchecked((long)b));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return b.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.Byte:
					{
						byte b2 = iconvertible.ToByte(null);
						unchecked
						{
							switch (target)
							{
							case TypeCode.Boolean:
								return b2 != 0;
							case TypeCode.Char:
								return (char)b2;
							case TypeCode.SByte:
								return checked((sbyte)b2);
							case TypeCode.Byte:
								return b2;
							case TypeCode.Int16:
								return (short)b2;
							case TypeCode.UInt16:
								return (ushort)b2;
							case TypeCode.Int32:
								return (int)b2;
							case TypeCode.UInt32:
								return (uint)b2;
							case TypeCode.Int64:
								return (long)((ulong)b2);
							case TypeCode.UInt64:
								return (ulong)b2;
							case TypeCode.Single:
								return (float)b2;
							case TypeCode.Double:
								return (double)b2;
							case TypeCode.Decimal:
								return b2;
							case TypeCode.DateTime:
								return new DateTime((long)((ulong)b2));
							case (TypeCode)17:
								goto IL_172C;
							case TypeCode.String:
								return b2.ToString(CultureInfo.InvariantCulture);
							default:
								goto IL_172C;
							}
							break;
						}
					}
					case TypeCode.Int16:
					{
						short num3 = iconvertible.ToInt16(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num3 != 0;
						case TypeCode.Char:
							return (char)num3;
						case TypeCode.SByte:
							return (sbyte)num3;
						case TypeCode.Byte:
							return (byte)num3;
						case TypeCode.Int16:
							return num3;
						case TypeCode.UInt16:
							return (ushort)num3;
						case TypeCode.Int32:
							return (int)num3;
						case TypeCode.UInt32:
							return (uint)num3;
						case TypeCode.Int64:
							return unchecked((long)num3);
						case TypeCode.UInt64:
							return (ulong)num3;
						case TypeCode.Single:
							return (float)num3;
						case TypeCode.Double:
							return (double)num3;
						case TypeCode.Decimal:
							return num3;
						case TypeCode.DateTime:
							return new DateTime(unchecked((long)num3));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num3.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.UInt16:
					{
						ushort num2 = iconvertible.ToUInt16(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num2 != 0;
						case TypeCode.Char:
							return (char)num2;
						case TypeCode.SByte:
							return (sbyte)num2;
						case TypeCode.Byte:
							return (byte)num2;
						case TypeCode.Int16:
							return (short)num2;
						case TypeCode.UInt16:
							return num2;
						case TypeCode.Int32:
							return (int)num2;
						case TypeCode.UInt32:
							return (uint)num2;
						case TypeCode.Int64:
							return unchecked((long)num2);
						case TypeCode.UInt64:
							return unchecked((ulong)num2);
						case TypeCode.Single:
							return (float)num2;
						case TypeCode.Double:
							return (double)num2;
						case TypeCode.Decimal:
							return num2;
						case TypeCode.DateTime:
							return new DateTime(unchecked((long)num2));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num2.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.Int32:
					{
						int num4 = iconvertible.ToInt32(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num4 != 0;
						case TypeCode.Char:
							return (char)num4;
						case TypeCode.SByte:
							return (sbyte)num4;
						case TypeCode.Byte:
							return (byte)num4;
						case TypeCode.Int16:
							return (short)num4;
						case TypeCode.UInt16:
							return (ushort)num4;
						case TypeCode.Int32:
							return num4;
						case TypeCode.UInt32:
							return (uint)num4;
						case TypeCode.Int64:
							return unchecked((long)num4);
						case TypeCode.UInt64:
							return (ulong)num4;
						case TypeCode.Single:
							return (float)num4;
						case TypeCode.Double:
							return (double)num4;
						case TypeCode.Decimal:
							return num4;
						case TypeCode.DateTime:
							return new DateTime(unchecked((long)num4));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num4.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.UInt32:
					{
						uint num5 = iconvertible.ToUInt32(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num5 != 0U;
						case TypeCode.Char:
							return (char)num5;
						case TypeCode.SByte:
							return (sbyte)num5;
						case TypeCode.Byte:
							return (byte)num5;
						case TypeCode.Int16:
							return (short)num5;
						case TypeCode.UInt16:
							return (ushort)num5;
						case TypeCode.Int32:
							return (int)num5;
						case TypeCode.UInt32:
							return num5;
						case TypeCode.Int64:
							return (long)num5;
						case TypeCode.UInt64:
							return unchecked((ulong)num5);
						case TypeCode.Single:
							return num5;
						case TypeCode.Double:
							return num5;
						case TypeCode.Decimal:
							return num5;
						case TypeCode.DateTime:
							return new DateTime((long)num5);
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num5.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.Int64:
					{
						long num6 = iconvertible.ToInt64(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num6 != 0L;
						case TypeCode.Char:
							return (char)num6;
						case TypeCode.SByte:
							return (sbyte)num6;
						case TypeCode.Byte:
							return (byte)num6;
						case TypeCode.Int16:
							return (short)num6;
						case TypeCode.UInt16:
							return (ushort)num6;
						case TypeCode.Int32:
							return (int)num6;
						case TypeCode.UInt32:
							return (uint)num6;
						case TypeCode.Int64:
							return num6;
						case TypeCode.UInt64:
							return (ulong)num6;
						case TypeCode.Single:
							return (float)num6;
						case TypeCode.Double:
							return (double)num6;
						case TypeCode.Decimal:
							return num6;
						case TypeCode.DateTime:
							return new DateTime(num6);
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num6.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.UInt64:
					{
						ulong num7 = iconvertible.ToUInt64(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num7 != 0UL;
						case TypeCode.Char:
							return (char)num7;
						case TypeCode.SByte:
							return (sbyte)num7;
						case TypeCode.Byte:
							return (byte)num7;
						case TypeCode.Int16:
							return (short)num7;
						case TypeCode.UInt16:
							return (ushort)num7;
						case TypeCode.Int32:
							return (int)num7;
						case TypeCode.UInt32:
							return (uint)num7;
						case TypeCode.Int64:
							return (long)num7;
						case TypeCode.UInt64:
							return num7;
						case TypeCode.Single:
							return num7;
						case TypeCode.Double:
							return num7;
						case TypeCode.Decimal:
							return num7;
						case TypeCode.DateTime:
							return new DateTime((long)num7);
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num7.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.Single:
					{
						float num8 = iconvertible.ToSingle(null);
						if (target != TypeCode.Boolean)
						{
							switch (target)
							{
							case TypeCode.Single:
								return num8;
							case TypeCode.Double:
								return (double)num8;
							case TypeCode.Decimal:
								return (decimal)num8;
							case TypeCode.String:
								return Convert.ToString((double)num8);
							}
							if (Math.Round((double)num8) != (double)num8)
							{
								goto IL_172C;
							}
							switch (target)
							{
							case TypeCode.Char:
								return (char)num8;
							case TypeCode.SByte:
								return (sbyte)num8;
							case TypeCode.Byte:
								return (byte)num8;
							case TypeCode.Int16:
								return (short)num8;
							case TypeCode.UInt16:
								return (ushort)num8;
							case TypeCode.Int32:
								return (int)num8;
							case TypeCode.UInt32:
								return (uint)num8;
							case TypeCode.Int64:
								return (long)num8;
							case TypeCode.UInt64:
								return (ulong)num8;
							case TypeCode.Single:
							case TypeCode.Double:
							case TypeCode.Decimal:
								goto IL_172C;
							case TypeCode.DateTime:
								return new DateTime((long)num8);
							default:
								goto IL_172C;
							}
						}
						else
						{
							if (num8 != num8)
							{
								return false;
							}
							return num8 != 0f;
						}
						break;
					}
					case TypeCode.Double:
					{
						double num9 = iconvertible.ToDouble(null);
						if (target == TypeCode.Boolean)
						{
							return Convert.ToBoolean(num9);
						}
						switch (target)
						{
						case TypeCode.Single:
							return (float)num9;
						case TypeCode.Double:
							return num9;
						case TypeCode.Decimal:
							return (decimal)num9;
						case TypeCode.String:
							return Convert.ToString(num9);
						}
						if (Math.Round(num9) != num9)
						{
							goto IL_172C;
						}
						switch (target)
						{
						case TypeCode.Char:
							return (char)num9;
						case TypeCode.SByte:
							return (sbyte)num9;
						case TypeCode.Byte:
							return (byte)num9;
						case TypeCode.Int16:
							return (short)num9;
						case TypeCode.UInt16:
							return (ushort)num9;
						case TypeCode.Int32:
							return (int)num9;
						case TypeCode.UInt32:
							return (uint)num9;
						case TypeCode.Int64:
							return (long)num9;
						case TypeCode.UInt64:
							return (ulong)num9;
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							goto IL_172C;
						case TypeCode.DateTime:
							return new DateTime((long)num9);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.Decimal:
					{
						decimal num10 = iconvertible.ToDecimal(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return num10 != 0m;
						case TypeCode.Char:
							return (char)decimal.ToUInt16(num10);
						case TypeCode.SByte:
							return decimal.ToSByte(num10);
						case TypeCode.Byte:
							return decimal.ToByte(num10);
						case TypeCode.Int16:
							return decimal.ToInt16(num10);
						case TypeCode.UInt16:
							return decimal.ToUInt16(num10);
						case TypeCode.Int32:
							return decimal.ToInt32(num10);
						case TypeCode.UInt32:
							return decimal.ToUInt32(num10);
						case TypeCode.Int64:
							return decimal.ToInt64(num10);
						case TypeCode.UInt64:
							return decimal.ToUInt64(num10);
						case TypeCode.Single:
							return decimal.ToSingle(num10);
						case TypeCode.Double:
							return decimal.ToDouble(num10);
						case TypeCode.Decimal:
							return num10;
						case TypeCode.DateTime:
							return new DateTime(decimal.ToInt64(num10));
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return num10.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case TypeCode.DateTime:
					{
						DateTime dateTime = iconvertible.ToDateTime(null);
						switch (target)
						{
						case TypeCode.Boolean:
						case TypeCode.Char:
						case TypeCode.SByte:
						case TypeCode.Byte:
						case TypeCode.Int16:
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Int64:
						case TypeCode.UInt64:
						case TypeCode.Single:
						case TypeCode.Double:
						case TypeCode.Decimal:
							return Convert.Coerce2WithNoTrunctation(dateTime.Ticks, target);
						case TypeCode.DateTime:
							return dateTime;
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							return dateTime.ToString(CultureInfo.InvariantCulture);
						default:
							goto IL_172C;
						}
						break;
					}
					case (TypeCode)17:
						goto IL_172C;
					case TypeCode.String:
					{
						string text = iconvertible.ToString(null);
						switch (target)
						{
						case TypeCode.Boolean:
							return Convert.ToBoolean(text, false);
						case TypeCode.Char:
							if (text.Length == 1)
							{
								return text[0];
							}
							throw new JScriptException(JSError.TypeMismatch);
						case TypeCode.SByte:
						case TypeCode.Byte:
						case TypeCode.Int16:
						case TypeCode.UInt16:
						case TypeCode.Int32:
						case TypeCode.UInt32:
						case TypeCode.Double:
							break;
						case TypeCode.Int64:
							goto IL_1698;
						case TypeCode.UInt64:
							goto IL_16B3;
						case TypeCode.Single:
							try
							{
								return float.Parse(text, CultureInfo.InvariantCulture);
							}
							catch
							{
								break;
							}
							goto Block_37;
						case TypeCode.Decimal:
							goto IL_16CB;
						case TypeCode.DateTime:
							goto IL_16E3;
						case (TypeCode)17:
							goto IL_172C;
						case TypeCode.String:
							goto IL_1726;
						default:
							goto IL_172C;
						}
						IL_1664:
						return Convert.Coerce2WithNoTrunctation(Convert.ToNumber(text), target);
						Block_37:
						try
						{
							IL_1698:
							return long.Parse(text, CultureInfo.InvariantCulture);
						}
						catch
						{
							goto IL_1664;
						}
						try
						{
							IL_16B3:
							return ulong.Parse(text, CultureInfo.InvariantCulture);
						}
						catch
						{
							goto IL_1664;
						}
						try
						{
							IL_16CB:
							return decimal.Parse(text, CultureInfo.InvariantCulture);
						}
						catch
						{
							goto IL_1664;
						}
						try
						{
							IL_16E3:
							return DateTime.Parse(text, CultureInfo.InvariantCulture);
						}
						catch
						{
							return DatePrototype.getVarDate(DateConstructor.ob.CreateInstance(new object[] { DatePrototype.ParseDate(text) }));
						}
						IL_1726:
						return text;
					}
					default:
						goto IL_172C;
					}
					switch (target)
					{
					case TypeCode.DBNull:
						return DBNull.Value;
					case TypeCode.Boolean:
						return false;
					case TypeCode.Char:
						return '\0';
					case TypeCode.SByte:
						return 0;
					case TypeCode.Byte:
						return 0;
					case TypeCode.Int16:
						return 0;
					case TypeCode.UInt16:
						return 0;
					case TypeCode.Int32:
						return 0;
					case TypeCode.UInt32:
						return 0U;
					case TypeCode.Int64:
						return 0L;
					case TypeCode.UInt64:
						return 0UL;
					case TypeCode.Single:
						return float.NaN;
					case TypeCode.Double:
						return double.NaN;
					case TypeCode.Decimal:
						return 0m;
					case TypeCode.DateTime:
						return new DateTime(0L);
					case TypeCode.String:
						return null;
					}
					IL_172C:;
				}
				catch (OverflowException)
				{
				}
				throw new JScriptException(JSError.TypeMismatch);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001A2B8 File Offset: 0x000192B8
		private static object Coerce2WithTruncationPermitted(object value, TypeCode target)
		{
			if (value is EnumWrapper)
			{
				value = ((EnumWrapper)value).value;
			}
			if (value is ConstantWrapper)
			{
				value = ((ConstantWrapper)value).value;
			}
			IConvertible iconvertible = Convert.GetIConvertible(value);
			switch (Convert.GetTypeCode(value, iconvertible))
			{
			case TypeCode.Empty:
				break;
			case TypeCode.Object:
				if (!(value is Missing) && (!(value is Missing) || target == TypeCode.Object))
				{
					switch (target)
					{
					case TypeCode.Boolean:
						return Convert.ToBoolean(value, iconvertible);
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						return Convert.Coerce2WithTruncationPermitted(Convert.ToNumber(value, iconvertible), target);
					case TypeCode.DateTime:
						if (value is DateObject)
						{
							return DatePrototype.getVarDate((DateObject)value);
						}
						return Convert.Coerce2WithTruncationPermitted(Convert.ToNumber(value, iconvertible), target);
					case (TypeCode)17:
						goto IL_1100;
					case TypeCode.String:
						return Convert.ToString(value, iconvertible);
					default:
						goto IL_1100;
					}
				}
				break;
			case TypeCode.DBNull:
				switch (target)
				{
				case TypeCode.DBNull:
					return DBNull.Value;
				case TypeCode.Boolean:
					return false;
				case TypeCode.Char:
					return '\0';
				case TypeCode.SByte:
					return 0;
				case TypeCode.Byte:
					return 0;
				case TypeCode.Int16:
					return 0;
				case TypeCode.UInt16:
					return 0;
				case TypeCode.Int32:
					return 0;
				case TypeCode.UInt32:
					return 0U;
				case TypeCode.Int64:
					return 0L;
				case TypeCode.UInt64:
					return 0UL;
				case TypeCode.Single:
					return 0f;
				case TypeCode.Double:
					return 0.0;
				case TypeCode.Decimal:
					return 0m;
				case TypeCode.DateTime:
					return new DateTime(0L);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return "null";
				default:
					goto IL_1100;
				}
				break;
			case TypeCode.Boolean:
			{
				bool flag = iconvertible.ToBoolean(null);
				int num = (flag ? 1 : 0);
				switch (target)
				{
				case TypeCode.Boolean:
					return flag;
				case TypeCode.Char:
					return (char)num;
				case TypeCode.SByte:
					return (sbyte)num;
				case TypeCode.Byte:
					return (byte)num;
				case TypeCode.Int16:
					return (short)num;
				case TypeCode.UInt16:
					return (ushort)num;
				case TypeCode.Int32:
					return num;
				case TypeCode.UInt32:
					return (uint)num;
				case TypeCode.Int64:
					return (long)num;
				case TypeCode.UInt64:
					return (ulong)((long)num);
				case TypeCode.Single:
					return (float)num;
				case TypeCode.Double:
					return (double)num;
				case TypeCode.Decimal:
					return num;
				case TypeCode.DateTime:
					return new DateTime((long)num);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					if (!flag)
					{
						return "false";
					}
					return "true";
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Char:
			{
				char c = iconvertible.ToChar(null);
				ushort num2 = (ushort)c;
				switch (target)
				{
				case TypeCode.Boolean:
					return num2 != 0;
				case TypeCode.Char:
					return c;
				case TypeCode.SByte:
					return (sbyte)num2;
				case TypeCode.Byte:
					return (byte)num2;
				case TypeCode.Int16:
					return (short)num2;
				case TypeCode.UInt16:
					return num2;
				case TypeCode.Int32:
					return (int)num2;
				case TypeCode.UInt32:
					return (uint)num2;
				case TypeCode.Int64:
					return (long)((ulong)num2);
				case TypeCode.UInt64:
					return (ulong)num2;
				case TypeCode.Single:
					return (float)num2;
				case TypeCode.Double:
					return (double)num2;
				case TypeCode.Decimal:
					return num2;
				case TypeCode.DateTime:
					return new DateTime((long)((ulong)num2));
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return char.ToString(c);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.SByte:
			{
				sbyte b = iconvertible.ToSByte(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return b != 0;
				case TypeCode.Char:
					return (char)b;
				case TypeCode.SByte:
					return b;
				case TypeCode.Byte:
					return (byte)b;
				case TypeCode.Int16:
					return (short)b;
				case TypeCode.UInt16:
					return (ushort)b;
				case TypeCode.Int32:
					return (int)b;
				case TypeCode.UInt32:
					return (uint)b;
				case TypeCode.Int64:
					return (long)b;
				case TypeCode.UInt64:
					return (ulong)((long)b);
				case TypeCode.Single:
					return (float)b;
				case TypeCode.Double:
					return (double)b;
				case TypeCode.Decimal:
					return b;
				case TypeCode.DateTime:
					return new DateTime((long)b);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return b.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Byte:
			{
				byte b2 = iconvertible.ToByte(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return b2 != 0;
				case TypeCode.Char:
					return (char)b2;
				case TypeCode.SByte:
					return (sbyte)b2;
				case TypeCode.Byte:
					return b2;
				case TypeCode.Int16:
					return (short)b2;
				case TypeCode.UInt16:
					return (ushort)b2;
				case TypeCode.Int32:
					return (int)b2;
				case TypeCode.UInt32:
					return (uint)b2;
				case TypeCode.Int64:
					return (long)((ulong)b2);
				case TypeCode.UInt64:
					return (ulong)b2;
				case TypeCode.Single:
					return (float)b2;
				case TypeCode.Double:
					return (double)b2;
				case TypeCode.Decimal:
					return b2;
				case TypeCode.DateTime:
					return new DateTime((long)((ulong)b2));
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return b2.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Int16:
			{
				short num3 = iconvertible.ToInt16(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num3 != 0;
				case TypeCode.Char:
					return (char)num3;
				case TypeCode.SByte:
					return (sbyte)num3;
				case TypeCode.Byte:
					return (byte)num3;
				case TypeCode.Int16:
					return num3;
				case TypeCode.UInt16:
					return (ushort)num3;
				case TypeCode.Int32:
					return (int)num3;
				case TypeCode.UInt32:
					return (uint)num3;
				case TypeCode.Int64:
					return (long)num3;
				case TypeCode.UInt64:
					return (ulong)((long)num3);
				case TypeCode.Single:
					return (float)num3;
				case TypeCode.Double:
					return (double)num3;
				case TypeCode.Decimal:
					return num3;
				case TypeCode.DateTime:
					return new DateTime((long)num3);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num3.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.UInt16:
			{
				ushort num2 = iconvertible.ToUInt16(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num2 != 0;
				case TypeCode.Char:
					return (char)num2;
				case TypeCode.SByte:
					return (sbyte)num2;
				case TypeCode.Byte:
					return (byte)num2;
				case TypeCode.Int16:
					return (short)num2;
				case TypeCode.UInt16:
					return num2;
				case TypeCode.Int32:
					return (int)num2;
				case TypeCode.UInt32:
					return (uint)num2;
				case TypeCode.Int64:
					return (long)((ulong)num2);
				case TypeCode.UInt64:
					return (ulong)num2;
				case TypeCode.Single:
					return (float)num2;
				case TypeCode.Double:
					return (double)num2;
				case TypeCode.Decimal:
					return num2;
				case TypeCode.DateTime:
					return new DateTime((long)((ulong)num2));
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num2.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Int32:
			{
				int num4 = iconvertible.ToInt32(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num4 != 0;
				case TypeCode.Char:
					return (char)num4;
				case TypeCode.SByte:
					return (sbyte)num4;
				case TypeCode.Byte:
					return (byte)num4;
				case TypeCode.Int16:
					return (short)num4;
				case TypeCode.UInt16:
					return (ushort)num4;
				case TypeCode.Int32:
					return num4;
				case TypeCode.UInt32:
					return (uint)num4;
				case TypeCode.Int64:
					return (long)num4;
				case TypeCode.UInt64:
					return (ulong)((long)num4);
				case TypeCode.Single:
					return (float)num4;
				case TypeCode.Double:
					return (double)num4;
				case TypeCode.Decimal:
					return num4;
				case TypeCode.DateTime:
					return new DateTime((long)num4);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num4.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.UInt32:
			{
				uint num5 = iconvertible.ToUInt32(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num5 != 0U;
				case TypeCode.Char:
					return (char)num5;
				case TypeCode.SByte:
					return (sbyte)num5;
				case TypeCode.Byte:
					return (byte)num5;
				case TypeCode.Int16:
					return (short)num5;
				case TypeCode.UInt16:
					return (ushort)num5;
				case TypeCode.Int32:
					return (int)num5;
				case TypeCode.UInt32:
					return num5;
				case TypeCode.Int64:
					return (long)((ulong)num5);
				case TypeCode.UInt64:
					return (ulong)num5;
				case TypeCode.Single:
					return num5;
				case TypeCode.Double:
					return num5;
				case TypeCode.Decimal:
					return num5;
				case TypeCode.DateTime:
					return new DateTime((long)((ulong)num5));
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num5.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Int64:
			{
				long num6 = iconvertible.ToInt64(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num6 != 0L;
				case TypeCode.Char:
					return (char)num6;
				case TypeCode.SByte:
					return (sbyte)num6;
				case TypeCode.Byte:
					return (byte)num6;
				case TypeCode.Int16:
					return (short)num6;
				case TypeCode.UInt16:
					return (ushort)num6;
				case TypeCode.Int32:
					return (int)num6;
				case TypeCode.UInt32:
					return (uint)num6;
				case TypeCode.Int64:
					return num6;
				case TypeCode.UInt64:
					return (ulong)num6;
				case TypeCode.Single:
					return (float)num6;
				case TypeCode.Double:
					return (double)num6;
				case TypeCode.Decimal:
					return num6;
				case TypeCode.DateTime:
					return new DateTime(num6);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num6.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.UInt64:
			{
				ulong num7 = iconvertible.ToUInt64(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num7 != 0UL;
				case TypeCode.Char:
					return (char)num7;
				case TypeCode.SByte:
					return (sbyte)num7;
				case TypeCode.Byte:
					return (byte)num7;
				case TypeCode.Int16:
					return (short)num7;
				case TypeCode.UInt16:
					return (ushort)num7;
				case TypeCode.Int32:
					return (int)num7;
				case TypeCode.UInt32:
					return (uint)num7;
				case TypeCode.Int64:
					return (long)num7;
				case TypeCode.UInt64:
					return num7;
				case TypeCode.Single:
					return num7;
				case TypeCode.Double:
					return num7;
				case TypeCode.Decimal:
					return num7;
				case TypeCode.DateTime:
					return new DateTime((long)num7);
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num7.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Single:
			{
				float num8 = iconvertible.ToSingle(null);
				if (target != TypeCode.Boolean)
				{
					switch (target)
					{
					case TypeCode.Single:
						return num8;
					case TypeCode.Double:
						return (double)num8;
					case TypeCode.Decimal:
						return (decimal)num8;
					case TypeCode.String:
						return Convert.ToString((double)num8);
					}
					long num6 = Runtime.DoubleToInt64((double)num8);
					switch (target)
					{
					case TypeCode.Char:
						return (char)num6;
					case TypeCode.SByte:
						return (sbyte)num6;
					case TypeCode.Byte:
						return (byte)num6;
					case TypeCode.Int16:
						return (short)num6;
					case TypeCode.UInt16:
						return (ushort)num6;
					case TypeCode.Int32:
						return (int)num6;
					case TypeCode.UInt32:
						return (uint)num6;
					case TypeCode.Int64:
						return num6;
					case TypeCode.UInt64:
						return (ulong)num6;
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						goto IL_1100;
					case TypeCode.DateTime:
						return new DateTime(num6);
					default:
						goto IL_1100;
					}
				}
				else
				{
					if (num8 != num8)
					{
						return false;
					}
					return num8 != 0f;
				}
				break;
			}
			case TypeCode.Double:
			{
				double num9 = iconvertible.ToDouble(null);
				if (target == TypeCode.Boolean)
				{
					return Convert.ToBoolean(num9);
				}
				switch (target)
				{
				case TypeCode.Single:
					return (float)num9;
				case TypeCode.Double:
					return num9;
				case TypeCode.Decimal:
					return (decimal)num9;
				case TypeCode.String:
					return Convert.ToString(num9);
				}
				long num6 = Runtime.DoubleToInt64(num9);
				switch (target)
				{
				case TypeCode.Char:
					return (char)num6;
				case TypeCode.SByte:
					return (sbyte)num6;
				case TypeCode.Byte:
					return (byte)num6;
				case TypeCode.Int16:
					return (short)num6;
				case TypeCode.UInt16:
					return (ushort)num6;
				case TypeCode.Int32:
					return (int)num6;
				case TypeCode.UInt32:
					return (uint)num6;
				case TypeCode.Int64:
					return num6;
				case TypeCode.UInt64:
					return (ulong)num6;
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					goto IL_1100;
				case TypeCode.DateTime:
					return new DateTime(num6);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.Decimal:
			{
				decimal num10 = iconvertible.ToDecimal(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return num10 != 0m;
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return Convert.Coerce2WithTruncationPermitted(Runtime.UncheckedDecimalToInt64(num10), target);
				case TypeCode.Single:
					return decimal.ToSingle(num10);
				case TypeCode.Double:
					return decimal.ToDouble(num10);
				case TypeCode.Decimal:
					return num10;
				case TypeCode.DateTime:
					return new DateTime(Runtime.UncheckedDecimalToInt64(num10));
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return num10.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case TypeCode.DateTime:
			{
				DateTime dateTime = iconvertible.ToDateTime(null);
				switch (target)
				{
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return Convert.Coerce2WithTruncationPermitted(dateTime.Ticks, target);
				case TypeCode.DateTime:
					return dateTime;
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return dateTime.ToString(CultureInfo.InvariantCulture);
				default:
					goto IL_1100;
				}
				break;
			}
			case (TypeCode)17:
				goto IL_1100;
			case TypeCode.String:
			{
				string text = iconvertible.ToString(null);
				switch (target)
				{
				case TypeCode.Boolean:
					return Convert.ToBoolean(text, false);
				case TypeCode.Char:
					if (text.Length == 1)
					{
						return text[0];
					}
					throw new JScriptException(JSError.TypeMismatch);
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Double:
					break;
				case TypeCode.Int64:
					goto IL_108A;
				case TypeCode.UInt64:
					goto IL_10B8;
				case TypeCode.Single:
					try
					{
						return float.Parse(text, CultureInfo.InvariantCulture);
					}
					catch
					{
						break;
					}
					goto Block_34;
				case TypeCode.Decimal:
					goto IL_10D0;
				case TypeCode.DateTime:
					goto IL_10EB;
				case (TypeCode)17:
					goto IL_1100;
				case TypeCode.String:
					return text;
				default:
					goto IL_1100;
				}
				IL_105C:
				return Convert.Coerce2WithTruncationPermitted(Convert.ToNumber(text), target);
				Block_34:
				try
				{
					IL_108A:
					return long.Parse(text, CultureInfo.InvariantCulture);
				}
				catch
				{
					try
					{
						return (long)ulong.Parse(text, CultureInfo.InvariantCulture);
					}
					catch
					{
						goto IL_105C;
					}
				}
				try
				{
					IL_10B8:
					return ulong.Parse(text, CultureInfo.InvariantCulture);
				}
				catch
				{
					goto IL_105C;
				}
				try
				{
					IL_10D0:
					return decimal.Parse(text, CultureInfo.InvariantCulture);
				}
				catch
				{
					goto IL_105C;
				}
				IL_10EB:
				return DateTime.Parse(text, CultureInfo.InvariantCulture);
			}
			default:
				goto IL_1100;
			}
			switch (target)
			{
			case TypeCode.DBNull:
				return DBNull.Value;
			case TypeCode.Boolean:
				return false;
			case TypeCode.Char:
				return '\0';
			case TypeCode.SByte:
				return 0;
			case TypeCode.Byte:
				return 0;
			case TypeCode.Int16:
				return 0;
			case TypeCode.UInt16:
				return 0;
			case TypeCode.Int32:
				return 0;
			case TypeCode.UInt32:
				return 0U;
			case TypeCode.Int64:
				return 0L;
			case TypeCode.UInt64:
				return 0UL;
			case TypeCode.Single:
				return float.NaN;
			case TypeCode.Double:
				return double.NaN;
			case TypeCode.Decimal:
				return 0m;
			case TypeCode.DateTime:
				return new DateTime(0L);
			case TypeCode.String:
				return "undefined";
			}
			IL_1100:
			throw new JScriptException(JSError.TypeMismatch);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0001B410 File Offset: 0x0001A410
		internal static void Emit(AST ast, ILGenerator il, Type source_type, Type target_type)
		{
			Convert.Emit(ast, il, source_type, target_type, false);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0001B41C File Offset: 0x0001A41C
		internal static void Emit(AST ast, ILGenerator il, Type source_type, Type target_type, bool truncationPermitted)
		{
			if (source_type == target_type)
			{
				return;
			}
			if (target_type == Typeob.Void)
			{
				il.Emit(OpCodes.Pop);
				return;
			}
			if (target_type.IsEnum)
			{
				if (source_type == Typeob.String || source_type == Typeob.Object)
				{
					il.Emit(OpCodes.Ldtoken, target_type);
					il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
					ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
					il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
					Convert.EmitUnbox(il, target_type, Type.GetTypeCode(Convert.GetUnderlyingType(target_type)));
					return;
				}
				Convert.Emit(ast, il, source_type, Convert.GetUnderlyingType(target_type));
				return;
			}
			else
			{
				if (source_type.IsEnum)
				{
					if (target_type.IsPrimitive)
					{
						Convert.Emit(ast, il, Convert.GetUnderlyingType(source_type), target_type);
						return;
					}
					if (target_type == Typeob.Object || target_type == Typeob.Enum)
					{
						il.Emit(OpCodes.Box, source_type);
						return;
					}
					if (target_type == Typeob.String)
					{
						il.Emit(OpCodes.Box, source_type);
						ConstantWrapper.TranslateToILInt(il, 0);
						il.Emit(OpCodes.Call, CompilerGlobals.toStringMethod);
						return;
					}
				}
				while (source_type is TypeBuilder)
				{
					source_type = source_type.BaseType;
					if (source_type == null)
					{
						source_type = Typeob.Object;
					}
					if (source_type == target_type)
					{
						return;
					}
				}
				if (source_type.IsArray && target_type.IsArray)
				{
					return;
				}
				TypeCode typeCode = Type.GetTypeCode(source_type);
				TypeCode typeCode2 = ((target_type is TypeBuilder) ? TypeCode.Object : Type.GetTypeCode(target_type));
				switch (typeCode)
				{
				case TypeCode.Empty:
					return;
				case TypeCode.Object:
					if (source_type == Typeob.Void)
					{
						il.Emit(OpCodes.Ldnull);
						source_type = Typeob.Object;
					}
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type.IsArray || target_type == Typeob.Array)
						{
							if (source_type == Typeob.ArrayObject || source_type == Typeob.Object)
							{
								if (target_type.IsArray)
								{
									il.Emit(OpCodes.Ldtoken, target_type.GetElementType());
								}
								else
								{
									il.Emit(OpCodes.Ldtoken, Typeob.Object);
								}
								il.Emit(OpCodes.Call, CompilerGlobals.toNativeArrayMethod);
							}
							il.Emit(OpCodes.Castclass, target_type);
							return;
						}
						if (target_type is TypeBuilder)
						{
							il.Emit(OpCodes.Castclass, target_type);
							return;
						}
						if (target_type == Typeob.Enum && source_type.BaseType == Typeob.Enum)
						{
							il.Emit(OpCodes.Box, source_type);
							return;
						}
						if (target_type == Typeob.Object || target_type.IsAssignableFrom(source_type))
						{
							if (source_type.IsValueType)
							{
								il.Emit(OpCodes.Box, source_type);
							}
							return;
						}
						if (Typeob.JSObject.IsAssignableFrom(target_type))
						{
							if (source_type.IsValueType)
							{
								il.Emit(OpCodes.Box, source_type);
							}
							ast.EmitILToLoadEngine(il);
							il.Emit(OpCodes.Call, CompilerGlobals.toObject2Method);
							il.Emit(OpCodes.Castclass, target_type);
							return;
						}
						if (Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						if (target_type.IsValueType || target_type.IsArray)
						{
							il.Emit(OpCodes.Ldtoken, target_type);
							il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
							ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
							il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
						}
						if (target_type.IsValueType)
						{
							Convert.EmitUnbox(il, target_type, typeCode2);
							return;
						}
						il.Emit(OpCodes.Castclass, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						break;
					case TypeCode.Boolean:
						if (source_type.IsValueType)
						{
							il.Emit(OpCodes.Box, source_type);
						}
						ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
						il.Emit(OpCodes.Call, CompilerGlobals.toBooleanMethod);
						return;
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Decimal:
					case TypeCode.DateTime:
						if (source_type.IsValueType)
						{
							il.Emit(OpCodes.Box, source_type);
						}
						if (truncationPermitted && typeCode2 == TypeCode.Int32)
						{
							il.Emit(OpCodes.Call, CompilerGlobals.toInt32Method);
							return;
						}
						ConstantWrapper.TranslateToILInt(il, (int)typeCode2);
						ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
						il.Emit(OpCodes.Call, CompilerGlobals.coerce2Method);
						if (target_type.IsValueType)
						{
							Convert.EmitUnbox(il, target_type, typeCode2);
						}
						return;
					case TypeCode.Single:
						if (source_type.IsValueType)
						{
							il.Emit(OpCodes.Box, source_type);
						}
						il.Emit(OpCodes.Call, CompilerGlobals.toNumberMethod);
						il.Emit(OpCodes.Conv_R4);
						return;
					case TypeCode.Double:
						if (source_type.IsValueType)
						{
							il.Emit(OpCodes.Box, source_type);
						}
						il.Emit(OpCodes.Call, CompilerGlobals.toNumberMethod);
						return;
					case TypeCode.String:
						if (source_type.IsValueType)
						{
							il.Emit(OpCodes.Box, source_type);
						}
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Castclass, Typeob.String);
							return;
						}
						ConstantWrapper.TranslateToILInt(il, 1);
						il.Emit(OpCodes.Call, CompilerGlobals.toStringMethod);
						break;
					default:
						return;
					}
					return;
				case TypeCode.DBNull:
					if (source_type.IsValueType)
					{
						il.Emit(OpCodes.Box, source_type);
					}
					if (typeCode2 == TypeCode.Object || (typeCode2 == TypeCode.String && !truncationPermitted))
					{
						if (target_type == Typeob.Object)
						{
							return;
						}
						if (!target_type.IsValueType)
						{
							il.Emit(OpCodes.Pop);
							il.Emit(OpCodes.Ldnull);
							return;
						}
					}
					if (target_type.IsValueType)
					{
						il.Emit(OpCodes.Ldtoken, target_type);
						il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
						ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
						il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
						Convert.EmitUnbox(il, target_type, typeCode2);
						return;
					}
					ConstantWrapper.TranslateToILInt(il, (int)typeCode2);
					ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
					il.Emit(OpCodes.Call, CompilerGlobals.coerce2Method);
					return;
				case TypeCode.Boolean:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						break;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						il.Emit(OpCodes.Conv_U8);
						return;
					case TypeCode.Single:
						il.Emit(OpCodes.Conv_R4);
						return;
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R8);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
					{
						Label label = il.DefineLabel();
						Label label2 = il.DefineLabel();
						il.Emit(OpCodes.Brfalse, label);
						il.Emit(OpCodes.Ldstr, "true");
						il.Emit(OpCodes.Br, label2);
						il.MarkLabel(label);
						il.Emit(OpCodes.Ldstr, "false");
						il.MarkLabel(label2);
						return;
					}
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Char:
				case TypeCode.UInt16:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
						break;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I2);
						break;
					case TypeCode.Int64:
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						il.Emit(OpCodes.Conv_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R_Un);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.uint32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						if (typeCode == TypeCode.Char)
						{
							il.Emit(OpCodes.Call, CompilerGlobals.convertCharToStringMethod);
							return;
						}
						Convert.EmitLdloca(il, Typeob.UInt32);
						il.Emit(OpCodes.Call, CompilerGlobals.uint32ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.SByte:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						break;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
					case TypeCode.Int16:
					case TypeCode.Int32:
						break;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U4);
						return;
					case TypeCode.Int64:
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I8);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R8);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.Int32);
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Byte:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
						break;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1_Un);
						break;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						il.Emit(OpCodes.Conv_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R_Un);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.uint32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.UInt32);
						il.Emit(OpCodes.Call, CompilerGlobals.uint32ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Int16:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1);
						break;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
					case TypeCode.Int32:
						break;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U4);
						return;
					case TypeCode.Int64:
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I8);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R8);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.Int32);
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Int32:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I2);
						break;
					case TypeCode.Int32:
						break;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U4);
						return;
					case TypeCode.Int64:
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U8);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R8);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.Int32);
						il.Emit(OpCodes.Call, CompilerGlobals.int32ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.UInt32:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I2);
						return;
					case TypeCode.Int32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I4_Un);
						break;
					case TypeCode.UInt32:
						break;
					case TypeCode.Int64:
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						il.Emit(OpCodes.Conv_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R_Un);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.uint32ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.UInt32);
						il.Emit(OpCodes.Call, CompilerGlobals.uint32ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Int64:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I2);
						return;
					case TypeCode.Int32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I4);
						return;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U4);
						break;
					case TypeCode.Int64:
						break;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U8);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U8);
						return;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R8);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.int64ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.Int64);
						il.Emit(OpCodes.Call, CompilerGlobals.int64ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.UInt64:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Conv_I8);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I2);
						return;
					case TypeCode.Int32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I4);
						return;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_U4);
						return;
					case TypeCode.Int64:
						if (truncationPermitted)
						{
							il.Emit(OpCodes.Conv_I8);
							return;
						}
						il.Emit(OpCodes.Conv_Ovf_I8_Un);
						break;
					case TypeCode.UInt64:
						break;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Conv_R_Un);
						return;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.uint64ToDecimalMethod);
						return;
					case TypeCode.DateTime:
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, Typeob.UInt64);
						il.Emit(OpCodes.Call, CompilerGlobals.uint64ToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Single:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
					case TypeCode.Decimal:
					case TypeCode.String:
						il.Emit(OpCodes.Conv_R8);
						Convert.Emit(ast, il, Typeob.Double, target_type);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_I2);
						return;
					case TypeCode.Int32:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I4);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_I4);
						return;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_Ovf_U4);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U4);
						return;
					case TypeCode.Int64:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_U8);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U8);
						break;
					case TypeCode.Single:
					case TypeCode.Double:
						break;
					case TypeCode.DateTime:
						if (truncationPermitted)
						{
							Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
						}
						else
						{
							il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
							il.Emit(OpCodes.Conv_Ovf_I8);
						}
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Double:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Call, CompilerGlobals.doubleToBooleanMethod);
						return;
					case TypeCode.Char:
					case TypeCode.UInt16:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U2);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U2);
						return;
					case TypeCode.SByte:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I1);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_I1);
						return;
					case TypeCode.Byte:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U1);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U1);
						return;
					case TypeCode.Int16:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I2);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_I2);
						return;
					case TypeCode.Int32:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I4);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_I4);
						return;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U4);
						return;
					case TypeCode.Int64:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_I8);
						return;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U8);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
						il.Emit(OpCodes.Conv_Ovf_U8);
						break;
					case TypeCode.Single:
					case TypeCode.Double:
						break;
					case TypeCode.Decimal:
						il.Emit(OpCodes.Call, CompilerGlobals.doubleToDecimalMethod);
						return;
					case TypeCode.DateTime:
						if (truncationPermitted)
						{
							Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
						}
						else
						{
							il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
							il.Emit(OpCodes.Conv_Ovf_I8);
						}
						il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
						return;
					case TypeCode.String:
						il.Emit(OpCodes.Call, CompilerGlobals.doubleToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.Decimal:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
						il.Emit(OpCodes.Ldsfld, CompilerGlobals.decimalZeroField);
						il.Emit(OpCodes.Call, CompilerGlobals.decimalCompare);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						il.Emit(OpCodes.Ldc_I4_0);
						il.Emit(OpCodes.Ceq);
						return;
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
						if (truncationPermitted)
						{
							Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_I4);
						}
						else
						{
							il.Emit(OpCodes.Call, CompilerGlobals.decimalToInt32Method);
						}
						Convert.Emit(ast, il, Typeob.Int32, target_type, truncationPermitted);
						return;
					case TypeCode.UInt32:
						if (truncationPermitted)
						{
							Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_U4);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.decimalToUInt32Method);
						return;
					case TypeCode.Int64:
						if (truncationPermitted)
						{
							Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.decimalToInt64Method);
						return;
					case TypeCode.UInt64:
						if (truncationPermitted)
						{
							Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_U8);
							return;
						}
						il.Emit(OpCodes.Call, CompilerGlobals.decimalToUInt64Method);
						break;
					case TypeCode.Single:
					case TypeCode.Double:
						il.Emit(OpCodes.Call, CompilerGlobals.decimalToDoubleMethod);
						Convert.Emit(ast, il, Typeob.Double, target_type, truncationPermitted);
						return;
					case TypeCode.Decimal:
						break;
					case TypeCode.DateTime:
						if (truncationPermitted)
						{
							Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
						}
						else
						{
							il.Emit(OpCodes.Call, CompilerGlobals.decimalToInt64Method);
						}
						Convert.Emit(ast, il, Typeob.Int64, target_type);
						return;
					case TypeCode.String:
						Convert.EmitLdloca(il, source_type);
						il.Emit(OpCodes.Call, CompilerGlobals.decimalToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.DateTime:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						il.Emit(OpCodes.Box, source_type);
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
						Convert.EmitLdloca(il, source_type);
						il.Emit(OpCodes.Call, CompilerGlobals.dateTimeToInt64Method);
						Convert.Emit(ast, il, Typeob.Int64, target_type, truncationPermitted);
						break;
					case TypeCode.DateTime:
						break;
					case TypeCode.String:
						Convert.EmitLdloca(il, source_type);
						il.Emit(OpCodes.Call, CompilerGlobals.dateTimeToStringMethod);
						return;
					default:
						goto IL_1B79;
					}
					return;
				case TypeCode.String:
					switch (typeCode2)
					{
					case TypeCode.Object:
						if (target_type != Typeob.Object && !(target_type is TypeBuilder) && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
						{
							return;
						}
						Convert.Emit(ast, il, Typeob.Object, target_type);
						return;
					case TypeCode.DBNull:
					case (TypeCode)17:
						goto IL_1B79;
					case TypeCode.Boolean:
					case TypeCode.Char:
					case TypeCode.SByte:
					case TypeCode.Byte:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
					case TypeCode.Decimal:
					case TypeCode.DateTime:
						if (truncationPermitted && typeCode2 == TypeCode.Int32)
						{
							il.Emit(OpCodes.Call, CompilerGlobals.toInt32Method);
							return;
						}
						ConstantWrapper.TranslateToILInt(il, (int)typeCode2);
						ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
						il.Emit(OpCodes.Call, CompilerGlobals.coerce2Method);
						if (target_type.IsValueType)
						{
							Convert.EmitUnbox(il, target_type, typeCode2);
						}
						break;
					case TypeCode.String:
						break;
					default:
						goto IL_1B79;
					}
					return;
				}
				IL_1B79:
				Convert.Emit(ast, il, source_type, Typeob.Object);
				il.Emit(OpCodes.Call, CompilerGlobals.throwTypeMismatch);
				LocalBuilder localBuilder = il.DeclareLocal(target_type);
				il.Emit(OpCodes.Ldloc, localBuilder);
				return;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001CFD5 File Offset: 0x0001BFD5
		internal static void EmitSingleToIntegerTruncatedConversion(ILGenerator il, OpCode opConversion)
		{
			il.Emit(OpCodes.Conv_R8);
			Convert.EmitDoubleToIntegerTruncatedConversion(il, opConversion);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001CFE9 File Offset: 0x0001BFE9
		internal static void EmitDoubleToIntegerTruncatedConversion(ILGenerator il, OpCode opConversion)
		{
			il.Emit(OpCodes.Call, CompilerGlobals.doubleToInt64);
			if (!opConversion.Equals(OpCodes.Conv_I8))
			{
				il.Emit(opConversion);
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0001D010 File Offset: 0x0001C010
		internal static void EmitDecimalToIntegerTruncatedConversion(ILGenerator il, OpCode opConversion)
		{
			il.Emit(OpCodes.Call, CompilerGlobals.uncheckedDecimalToInt64Method);
			if (!opConversion.Equals(OpCodes.Conv_I8))
			{
				il.Emit(opConversion);
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001D038 File Offset: 0x0001C038
		internal static void EmitUnbox(ILGenerator il, Type target_type, TypeCode target)
		{
			il.Emit(OpCodes.Unbox, target_type);
			switch (target)
			{
			case TypeCode.Boolean:
			case TypeCode.Byte:
				il.Emit(OpCodes.Ldind_U1);
				return;
			case TypeCode.Char:
			case TypeCode.UInt16:
				il.Emit(OpCodes.Ldind_U2);
				return;
			case TypeCode.SByte:
				il.Emit(OpCodes.Ldind_I1);
				return;
			case TypeCode.Int16:
				il.Emit(OpCodes.Ldind_I2);
				return;
			case TypeCode.Int32:
				il.Emit(OpCodes.Ldind_I4);
				return;
			case TypeCode.UInt32:
				il.Emit(OpCodes.Ldind_U4);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				il.Emit(OpCodes.Ldind_I8);
				return;
			case TypeCode.Single:
				il.Emit(OpCodes.Ldind_R4);
				return;
			case TypeCode.Double:
				il.Emit(OpCodes.Ldind_R8);
				return;
			default:
				il.Emit(OpCodes.Ldobj, target_type);
				return;
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001D108 File Offset: 0x0001C108
		private static bool EmittedCallToConversionMethod(AST ast, ILGenerator il, Type source_type, Type target_type)
		{
			MethodInfo methodInfo = target_type.GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { source_type }, null);
			if (methodInfo != null)
			{
				il.Emit(OpCodes.Call, methodInfo);
				Convert.Emit(ast, il, methodInfo.ReturnType, target_type);
				return true;
			}
			methodInfo = Convert.GetToXXXXMethod(source_type, target_type, true);
			if (methodInfo != null)
			{
				il.Emit(OpCodes.Call, methodInfo);
				return true;
			}
			methodInfo = target_type.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { source_type }, null);
			if (methodInfo != null)
			{
				il.Emit(OpCodes.Call, methodInfo);
				Convert.Emit(ast, il, methodInfo.ReturnType, target_type);
				return true;
			}
			methodInfo = Convert.GetToXXXXMethod(source_type, target_type, false);
			if (methodInfo != null)
			{
				il.Emit(OpCodes.Call, methodInfo);
				return true;
			}
			return false;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001D1C8 File Offset: 0x0001C1C8
		internal static void EmitLdarg(ILGenerator il, short argNum)
		{
			switch (argNum)
			{
			case 0:
				il.Emit(OpCodes.Ldarg_0);
				return;
			case 1:
				il.Emit(OpCodes.Ldarg_1);
				return;
			case 2:
				il.Emit(OpCodes.Ldarg_2);
				return;
			case 3:
				il.Emit(OpCodes.Ldarg_3);
				return;
			default:
				if (argNum < 256)
				{
					il.Emit(OpCodes.Ldarg_S, (byte)argNum);
					return;
				}
				il.Emit(OpCodes.Ldarg, argNum);
				return;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001D244 File Offset: 0x0001C244
		internal static void EmitLdloca(ILGenerator il, Type source_type)
		{
			LocalBuilder localBuilder = il.DeclareLocal(source_type);
			il.Emit(OpCodes.Stloc, localBuilder);
			il.Emit(OpCodes.Ldloca, localBuilder);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001D274 File Offset: 0x0001C274
		private static IReflect GetArrayElementType(IReflect ir)
		{
			if (ir is TypedArray)
			{
				return ((TypedArray)ir).elementType;
			}
			if (ir is Type && ((Type)ir).IsArray)
			{
				return ((Type)ir).GetElementType();
			}
			if (ir is ArrayObject || ir == Typeob.ArrayObject)
			{
				return Typeob.Object;
			}
			return null;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001D2D0 File Offset: 0x0001C2D0
		internal static int GetArrayRank(IReflect ir)
		{
			if (ir == Typeob.ArrayObject || ir is ArrayObject)
			{
				return 1;
			}
			if (ir is TypedArray)
			{
				return ((TypedArray)ir).rank;
			}
			if (ir is Type && ((Type)ir).IsArray)
			{
				return ((Type)ir).GetArrayRank();
			}
			return -1;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001D325 File Offset: 0x0001C325
		internal static IConvertible GetIConvertible(object ob)
		{
			return ob as IConvertible;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001D330 File Offset: 0x0001C330
		private static MethodInfo GetToXXXXMethod(IReflect ir, Type desiredType, bool explicitOK)
		{
			if (ir is TypeBuilder || ir is EnumBuilder)
			{
				return null;
			}
			MemberInfo[] member = ir.GetMember(explicitOK ? "op_Explicit" : "op_Implicit", BindingFlags.Static | BindingFlags.Public);
			if (member != null)
			{
				foreach (MemberInfo memberInfo in member)
				{
					if (memberInfo is MethodInfo && ((MethodInfo)memberInfo).ReturnType == desiredType)
					{
						return (MethodInfo)memberInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001D3A5 File Offset: 0x0001C3A5
		internal static TypeCode GetTypeCode(object ob, IConvertible ic)
		{
			if (ob == null)
			{
				return TypeCode.Empty;
			}
			if (ic == null)
			{
				return TypeCode.Object;
			}
			return ic.GetTypeCode();
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001D3B7 File Offset: 0x0001C3B7
		internal static TypeCode GetTypeCode(object ob)
		{
			return Convert.GetTypeCode(ob, Convert.GetIConvertible(ob));
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001D3C5 File Offset: 0x0001C3C5
		internal static Type GetUnderlyingType(Type type)
		{
			if (type is TypeBuilder)
			{
				return type.UnderlyingSystemType;
			}
			return Enum.GetUnderlyingType(type);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001D3DC File Offset: 0x0001C3DC
		internal static bool IsArray(IReflect ir)
		{
			return ir == Typeob.Array || ir == Typeob.ArrayObject || ir is TypedArray || ir is ArrayObject || (ir is Type && ((Type)ir).IsArray);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001D415 File Offset: 0x0001C415
		private static bool IsArrayElementTypeKnown(IReflect ir)
		{
			return ir == Typeob.ArrayObject || ir is TypedArray || ir is ArrayObject || (ir is Type && ((Type)ir).IsArray);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001D446 File Offset: 0x0001C446
		internal static bool IsArrayRankKnown(IReflect ir)
		{
			return ir == Typeob.ArrayObject || ir is TypedArray || ir is ArrayObject || (ir is Type && ((Type)ir).IsArray);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001D477 File Offset: 0x0001C477
		internal static bool IsArrayType(IReflect ir)
		{
			return ir is TypedArray || ir == Typeob.Array || ir == Typeob.ArrayObject || (ir is Type && ((Type)ir).IsArray);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001D4A8 File Offset: 0x0001C4A8
		internal static bool IsJScriptArray(IReflect ir)
		{
			return ir is ArrayObject || ir == Typeob.ArrayObject;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001D4BC File Offset: 0x0001C4BC
		internal static bool IsPrimitiveSignedNumericType(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
			case TypeCode.SByte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
				return true;
			}
			return false;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001D508 File Offset: 0x0001C508
		internal static bool IsPrimitiveSignedIntegerType(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
			case TypeCode.SByte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
				return true;
			}
			return false;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0001D548 File Offset: 0x0001C548
		internal static bool IsPrimitiveUnsignedIntegerType(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
			case TypeCode.Byte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
				return true;
			}
			return false;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0001D588 File Offset: 0x0001C588
		internal static bool IsPrimitiveIntegerType(Type t)
		{
			switch (Type.GetTypeCode(t))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001D5CC File Offset: 0x0001C5CC
		internal static bool IsPrimitiveNumericTypeCode(TypeCode tc)
		{
			switch (tc)
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001D610 File Offset: 0x0001C610
		internal static bool IsPrimitiveNumericType(IReflect ir)
		{
			Type type = ir as Type;
			return type != null && Convert.IsPrimitiveNumericTypeCode(Type.GetTypeCode(type));
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001D634 File Offset: 0x0001C634
		internal static bool IsPrimitiveNumericTypeFitForDouble(IReflect ir)
		{
			Type type = ir as Type;
			if (type == null)
			{
				return false;
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Single:
			case TypeCode.Double:
				return true;
			}
			return false;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001D68C File Offset: 0x0001C68C
		private static bool IsPromotableTo(Type source_type, Type target_type)
		{
			TypeCode typeCode = Type.GetTypeCode(source_type);
			TypeCode typeCode2 = Type.GetTypeCode(target_type);
			if (Convert.promotable[(int)typeCode, (int)typeCode2])
			{
				return true;
			}
			if ((typeCode == TypeCode.Object || typeCode == TypeCode.String) && typeCode2 == TypeCode.Object)
			{
				if (target_type.IsAssignableFrom(source_type))
				{
					return true;
				}
				if (target_type == Typeob.BooleanObject && source_type == Typeob.Boolean)
				{
					return true;
				}
				if (target_type == Typeob.StringObject && source_type == Typeob.String)
				{
					return true;
				}
				if (target_type == Typeob.NumberObject && Convert.IsPromotableTo(source_type, Typeob.Double))
				{
					return true;
				}
				if (target_type == Typeob.Array || source_type == Typeob.Array || target_type.IsArray || source_type.IsArray)
				{
					return Convert.IsPromotableToArray(source_type, target_type);
				}
			}
			if (source_type == Typeob.BooleanObject && target_type == Typeob.Boolean)
			{
				return true;
			}
			if (source_type == Typeob.StringObject && target_type == Typeob.String)
			{
				return true;
			}
			if (source_type == Typeob.DateObject && target_type == Typeob.DateTime)
			{
				return true;
			}
			if (source_type == Typeob.NumberObject)
			{
				return Convert.IsPrimitiveNumericType(target_type);
			}
			if (source_type.IsEnum)
			{
				return !target_type.IsEnum && Convert.IsPromotableTo(Convert.GetUnderlyingType(source_type), target_type);
			}
			if (target_type.IsEnum)
			{
				return !source_type.IsEnum && Convert.IsPromotableTo(source_type, Convert.GetUnderlyingType(target_type));
			}
			MethodInfo methodInfo = target_type.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { source_type }, null);
			if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
			{
				return true;
			}
			methodInfo = Convert.GetToXXXXMethod(source_type, target_type, false);
			return methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001D804 File Offset: 0x0001C804
		internal static bool IsPromotableTo(IReflect source_ir, IReflect target_ir)
		{
			if (source_ir is TypedArray || target_ir is TypedArray || source_ir is ArrayObject || target_ir is ArrayObject || source_ir == Typeob.ArrayObject || target_ir == Typeob.ArrayObject)
			{
				return Convert.IsPromotableToArray(source_ir, target_ir);
			}
			if (target_ir is ClassScope)
			{
				if (!(((ClassScope)target_ir).owner is EnumDeclaration))
				{
					return source_ir is ClassScope && ((ClassScope)source_ir).IsSameOrDerivedFrom((ClassScope)target_ir);
				}
				if (Convert.IsPrimitiveNumericType(source_ir))
				{
					return Convert.IsPromotableTo(source_ir, ((EnumDeclaration)((ClassScope)target_ir).owner).baseType.ToType());
				}
				return source_ir == Typeob.String || source_ir == target_ir;
			}
			else
			{
				Type type;
				if (target_ir is Type)
				{
					if (target_ir == Typeob.Object)
					{
						return !(source_ir is Type) || !((Type)source_ir).IsByRef;
					}
					type = (Type)target_ir;
				}
				else if (target_ir is ScriptFunction)
				{
					type = Typeob.ScriptFunction;
				}
				else
				{
					type = Globals.TypeRefs.ToReferenceContext(target_ir.GetType());
				}
				if (source_ir is ClassScope)
				{
					return ((ClassScope)source_ir).IsPromotableTo(type);
				}
				return Convert.IsPromotableTo((source_ir is Type) ? ((Type)source_ir) : Globals.TypeRefs.ToReferenceContext(source_ir.GetType()), type);
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001D948 File Offset: 0x0001C948
		private static bool IsPromotableToArray(IReflect source_ir, IReflect target_ir)
		{
			if (!Convert.IsArray(source_ir))
			{
				return false;
			}
			if (target_ir == Typeob.Object)
			{
				return true;
			}
			if (!Convert.IsArray(target_ir))
			{
				if (target_ir is Type)
				{
					Type type = (Type)target_ir;
					if (type.IsInterface && type.IsAssignableFrom(Typeob.Array))
					{
						return source_ir is TypedArray || (source_ir is Type && ((Type)source_ir).IsArray);
					}
				}
				return false;
			}
			if (Convert.IsJScriptArray(source_ir) && !Convert.IsJScriptArray(target_ir))
			{
				return false;
			}
			if (target_ir == Typeob.Array)
			{
				return !Convert.IsJScriptArray(source_ir);
			}
			if (source_ir == Typeob.Array)
			{
				return false;
			}
			if (Convert.GetArrayRank(source_ir) == 1 && Convert.IsJScriptArray(target_ir))
			{
				return true;
			}
			if (Convert.GetArrayRank(source_ir) != Convert.GetArrayRank(target_ir))
			{
				return false;
			}
			IReflect arrayElementType = Convert.GetArrayElementType(source_ir);
			IReflect arrayElementType2 = Convert.GetArrayElementType(target_ir);
			if (arrayElementType == null || arrayElementType2 == null)
			{
				return false;
			}
			if ((arrayElementType is Type && ((Type)arrayElementType).IsValueType) || (arrayElementType2 is Type && ((Type)arrayElementType2).IsValueType))
			{
				return arrayElementType == arrayElementType2;
			}
			return Convert.IsPromotableTo(arrayElementType, arrayElementType2);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001DA54 File Offset: 0x0001CA54
		private static bool IsWhiteSpace(char c)
		{
			switch (c)
			{
			case '\t':
			case '\n':
			case '\v':
			case '\f':
			case '\r':
				break;
			default:
				if (c != ' ' && c != '\u00a0')
				{
					return c >= '\u0080' && char.IsWhiteSpace(c);
				}
				break;
			}
			return true;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001DA9F File Offset: 0x0001CA9F
		private static bool IsWhiteSpaceTrailer(char[] s, int i, int max)
		{
			while (i < max)
			{
				if (!Convert.IsWhiteSpace(s[i]))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001DAB9 File Offset: 0x0001CAB9
		internal static object LiteralToNumber(string str)
		{
			return Convert.LiteralToNumber(str, null);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001DAC4 File Offset: 0x0001CAC4
		internal static object LiteralToNumber(string str, Context context)
		{
			uint num = 10U;
			if (str[0] == '0' && str.Length > 1)
			{
				if (str[1] == 'x' || str[1] == 'X')
				{
					num = 16U;
				}
				else
				{
					num = 8U;
				}
			}
			object obj = Convert.parseRadix(str.ToCharArray(), num, (num == 16U) ? 2 : 0, 1, false);
			if (obj != null)
			{
				if (num == 8U && context != null && obj is int && (int)obj > 7)
				{
					context.HandleError(JSError.OctalLiteralsAreDeprecated);
				}
				return obj;
			}
			context.HandleError(JSError.BadOctalLiteral);
			return Convert.parseRadix(str.ToCharArray(), 10U, 0, 1, false);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001DB60 File Offset: 0x0001CB60
		internal static bool NeedsWrapper(TypeCode code)
		{
			switch (code)
			{
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
				return true;
			}
			return false;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001DBBC File Offset: 0x0001CBBC
		private static double DoubleParse(string str)
		{
			double num;
			try
			{
				num = double.Parse(str, NumberStyles.Float, CultureInfo.InvariantCulture);
			}
			catch (OverflowException)
			{
				int num2 = 0;
				int length = str.Length;
				while (num2 < length && Convert.IsWhiteSpace(str[num2]))
				{
					num2++;
				}
				if (num2 < length && str[num2] == '-')
				{
					num = double.NegativeInfinity;
				}
				else
				{
					num = double.PositiveInfinity;
				}
			}
			return num;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001DC38 File Offset: 0x0001CC38
		private static object parseRadix(char[] s, uint rdx, int i, int sign, bool ignoreTrailers)
		{
			int num = s.Length;
			if (i >= num)
			{
				return null;
			}
			ulong num2 = ulong.MaxValue / (ulong)rdx;
			int num3 = Convert.RadixDigit(s[i], rdx);
			if (num3 < 0)
			{
				return null;
			}
			ulong num4 = (ulong)((long)num3);
			int num5 = i;
			while (++i != num)
			{
				num3 = Convert.RadixDigit(s[i], rdx);
				if (num3 >= 0)
				{
					if (num4 <= num2)
					{
						ulong num6 = num4 * (ulong)rdx;
						ulong num7 = num6 + (ulong)((long)num3);
						if (num6 <= num7)
						{
							num4 = num7;
							continue;
						}
					}
					if (rdx == 10U)
					{
						try
						{
							double num8 = Convert.DoubleParse(new string(s, num5, num - num5));
							if (num8 == num8)
							{
								return (double)sign * num8;
							}
							if (!ignoreTrailers)
							{
								return null;
							}
						}
						catch
						{
						}
					}
					double num9 = num4 * rdx + (double)num3;
					while (++i != num)
					{
						num3 = Convert.RadixDigit(s[i], rdx);
						if (num3 < 0)
						{
							if (ignoreTrailers || Convert.IsWhiteSpaceTrailer(s, i, num))
							{
								return (double)sign * num9;
							}
							return null;
						}
						else
						{
							num9 = num9 * rdx + (double)num3;
						}
					}
					return (double)sign * num9;
				}
				if (!ignoreTrailers && !Convert.IsWhiteSpaceTrailer(s, i, num))
				{
					return null;
				}
				break;
			}
			if (sign < 0)
			{
				if (num4 <= (ulong)(-2147483648))
				{
					return (int)(-(int)num4);
				}
				if (num4 < 9223372036854775808UL)
				{
					return (long)(-(long)num4);
				}
				if (num4 == 9223372036854775808UL)
				{
					return long.MinValue;
				}
				return -num4;
			}
			else
			{
				if (num4 <= 2147483647UL)
				{
					return (int)num4;
				}
				if (num4 <= 9223372036854775807UL)
				{
					return (long)num4;
				}
				return num4;
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001DDD0 File Offset: 0x0001CDD0
		private static int RadixDigit(char c, uint r)
		{
			int num;
			if (c >= '0' && c <= '9')
			{
				num = (int)(c - '0');
			}
			else if (c >= 'A' && c <= 'Z')
			{
				num = (int)('\n' + c - 'A');
			}
			else
			{
				if (c < 'a' || c > 'z')
				{
					return -1;
				}
				num = (int)('\n' + c - 'a');
			}
			if ((long)num >= (long)((ulong)r))
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001DE21 File Offset: 0x0001CE21
		[DebuggerHidden]
		[DebuggerStepThrough]
		public static void ThrowTypeMismatch(object val)
		{
			throw new JScriptException(JSError.TypeMismatch, new Context(new DocumentContext("", null), val.ToString()));
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001DE40 File Offset: 0x0001CE40
		public static bool ToBoolean(double d)
		{
			return d == d && d != 0.0;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001DE57 File Offset: 0x0001CE57
		[DebuggerStepThrough]
		[DebuggerHidden]
		public static bool ToBoolean(object value)
		{
			if (value is bool)
			{
				return (bool)value;
			}
			return Convert.ToBoolean(value, Convert.GetIConvertible(value));
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001DE74 File Offset: 0x0001CE74
		[DebuggerHidden]
		[DebuggerStepThrough]
		public static bool ToBoolean(object value, bool explicitConversion)
		{
			if (value is bool)
			{
				return (bool)value;
			}
			if (!explicitConversion && value is BooleanObject)
			{
				return ((BooleanObject)value).value;
			}
			return Convert.ToBoolean(value, Convert.GetIConvertible(value));
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001DEA8 File Offset: 0x0001CEA8
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static bool ToBoolean(object value, IConvertible ic)
		{
			switch (Convert.GetTypeCode(value, ic))
			{
			case TypeCode.Empty:
				return false;
			case TypeCode.Object:
			{
				if (value is Missing || value is Missing)
				{
					return false;
				}
				Type type = value.GetType();
				MethodInfo methodInfo = type.GetMethod("op_True", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[] { type }, null);
				if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && methodInfo.ReturnType == typeof(bool))
				{
					methodInfo = new JSMethodInfo(methodInfo);
					return (bool)methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { value }, null);
				}
				return true;
			}
			case TypeCode.DBNull:
				return false;
			case TypeCode.Boolean:
				return ic.ToBoolean(null);
			case TypeCode.Char:
				return ic.ToChar(null) != '\0';
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
				return ic.ToInt32(null) != 0;
			case TypeCode.UInt32:
			case TypeCode.Int64:
				return ic.ToInt64(null) != 0L;
			case TypeCode.UInt64:
				return ic.ToUInt64(null) != 0UL;
			case TypeCode.Single:
			case TypeCode.Double:
			{
				double num = ic.ToDouble(null);
				return num == num && num != 0.0;
			}
			case TypeCode.Decimal:
				return ic.ToDecimal(null) != 0m;
			case TypeCode.DateTime:
				return true;
			case TypeCode.String:
				return ic.ToString(null).Length != 0;
			}
			return false;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0001E02B File Offset: 0x0001D02B
		internal static char ToChar(object value)
		{
			return (char)Convert.ToUint32(value);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001E034 File Offset: 0x0001D034
		private static char ToDigit(int digit)
		{
			if (digit >= 10)
			{
				return (char)(97 + digit - 10);
			}
			return (char)(48 + digit);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001E04C File Offset: 0x0001D04C
		public static object ToForInObject(object value, VsaEngine engine)
		{
			if (value is ScriptObject)
			{
				return value;
			}
			IConvertible iconvertible = Convert.GetIConvertible(value);
			switch (Convert.GetTypeCode(value, iconvertible))
			{
			case TypeCode.Object:
				return value;
			case TypeCode.Boolean:
				return engine.Globals.globalObject.originalBoolean.ConstructImplicitWrapper(iconvertible.ToBoolean(null));
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return engine.Globals.globalObject.originalNumber.ConstructImplicitWrapper(value);
			case TypeCode.DateTime:
				return value;
			case TypeCode.String:
				return engine.Globals.globalObject.originalString.ConstructImplicitWrapper(iconvertible.ToString(null));
			}
			return engine.Globals.globalObject.originalObject.ConstructObject();
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001E12E File Offset: 0x0001D12E
		internal static double ToInteger(double number)
		{
			if (number != number)
			{
				return 0.0;
			}
			return (double)Math.Sign(number) * Math.Floor(Math.Abs(number));
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0001E151 File Offset: 0x0001D151
		internal static double ToInteger(object value)
		{
			if (value is double)
			{
				return Convert.ToInteger((double)value);
			}
			if (value is int)
			{
				return (double)((int)value);
			}
			return Convert.ToInteger(value, Convert.GetIConvertible(value));
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001E184 File Offset: 0x0001D184
		internal static double ToInteger(object value, IConvertible ic)
		{
			switch (Convert.GetTypeCode(value, ic))
			{
			case TypeCode.Empty:
				return 0.0;
			case TypeCode.Object:
			case TypeCode.DateTime:
			{
				object obj = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
				if (obj != value)
				{
					return Convert.ToInteger(Convert.ToNumber(obj, ic));
				}
				return double.NaN;
			}
			case TypeCode.DBNull:
				return 0.0;
			case TypeCode.Boolean:
				return (double)(ic.ToBoolean(null) ? 1 : 0);
			case TypeCode.Char:
				return (double)ic.ToChar(null);
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return ic.ToDouble(null);
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Convert.ToInteger(ic.ToDouble(null));
			case TypeCode.String:
				return Convert.ToInteger(Convert.ToNumber(ic.ToString(null)));
			}
			return 0.0;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001E26E File Offset: 0x0001D26E
		public static int ToInt32(object value)
		{
			if (value is double)
			{
				return (int)Runtime.DoubleToInt64((double)value);
			}
			if (value is int)
			{
				return (int)value;
			}
			return Convert.ToInt32(value, Convert.GetIConvertible(value));
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0001E2A0 File Offset: 0x0001D2A0
		internal static int ToInt32(object value, IConvertible ic)
		{
			switch (Convert.GetTypeCode(value, ic))
			{
			case TypeCode.Empty:
				return 0;
			case TypeCode.Object:
			case TypeCode.DateTime:
			{
				object obj = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
				if (obj != value)
				{
					return Convert.ToInt32(obj, ic);
				}
				return 0;
			}
			case TypeCode.DBNull:
				return 0;
			case TypeCode.Boolean:
				if (!ic.ToBoolean(null))
				{
					return 0;
				}
				return 1;
			case TypeCode.Char:
				return (int)ic.ToChar(null);
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
				return ic.ToInt32(null);
			case TypeCode.UInt32:
			case TypeCode.Int64:
				return (int)ic.ToInt64(null);
			case TypeCode.UInt64:
				return (int)ic.ToUInt64(null);
			case TypeCode.Single:
			case TypeCode.Double:
				return (int)Runtime.DoubleToInt64(ic.ToDouble(null));
			case TypeCode.Decimal:
				return (int)Runtime.UncheckedDecimalToInt64(ic.ToDecimal(null));
			case TypeCode.String:
				return (int)Runtime.DoubleToInt64(Convert.ToNumber(ic.ToString(null)));
			}
			return 0;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001E384 File Offset: 0x0001D384
		internal static IReflect ToIReflect(Type t, VsaEngine engine)
		{
			GlobalObject globalObject = engine.Globals.globalObject;
			object obj = t;
			if (t == Typeob.ArrayObject)
			{
				obj = globalObject.originalArray.Construct();
			}
			else if (t == Typeob.BooleanObject)
			{
				obj = globalObject.originalBoolean.Construct();
			}
			else if (t == Typeob.DateObject)
			{
				obj = globalObject.originalDate.Construct(new object[0]);
			}
			else if (t == Typeob.EnumeratorObject)
			{
				obj = globalObject.originalEnumerator.Construct(new object[0]);
			}
			else if (t == Typeob.ErrorObject)
			{
				obj = globalObject.originalError.Construct(new object[0]);
			}
			else if (t == Typeob.EvalErrorObject)
			{
				obj = globalObject.originalEvalError.Construct(new object[0]);
			}
			else if (t == Typeob.JSObject)
			{
				obj = globalObject.originalObject.Construct(new object[0]);
			}
			else if (t == Typeob.NumberObject)
			{
				obj = globalObject.originalNumber.Construct();
			}
			else if (t == Typeob.RangeErrorObject)
			{
				obj = globalObject.originalRangeError.Construct(new object[0]);
			}
			else if (t == Typeob.ReferenceErrorObject)
			{
				obj = globalObject.originalReferenceError.Construct(new object[0]);
			}
			else if (t == Typeob.RegExpObject)
			{
				obj = globalObject.originalRegExp.Construct(new object[0]);
			}
			else if (t == Typeob.ScriptFunction)
			{
				obj = FunctionPrototype.ob;
			}
			else if (t == Typeob.StringObject)
			{
				obj = globalObject.originalString.Construct();
			}
			else if (t == Typeob.SyntaxErrorObject)
			{
				obj = globalObject.originalSyntaxError.Construct(new object[0]);
			}
			else if (t == Typeob.TypeErrorObject)
			{
				obj = globalObject.originalTypeError.Construct(new object[0]);
			}
			else if (t == Typeob.URIErrorObject)
			{
				obj = globalObject.originalURIError.Construct(new object[0]);
			}
			else if (t == Typeob.VBArrayObject)
			{
				obj = globalObject.originalVBArray.Construct();
			}
			else if (t == Typeob.ArgumentsObject)
			{
				obj = globalObject.originalObject.Construct(new object[0]);
			}
			return (IReflect)obj;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001E598 File Offset: 0x0001D598
		public static double ToNumber(object value)
		{
			if (value is int)
			{
				return (double)((int)value);
			}
			if (value is double)
			{
				return (double)value;
			}
			return Convert.ToNumber(value, Convert.GetIConvertible(value));
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001E5C8 File Offset: 0x0001D5C8
		internal static double ToNumber(object value, IConvertible ic)
		{
			switch (Convert.GetTypeCode(value, ic))
			{
			case TypeCode.Empty:
				return double.NaN;
			case TypeCode.Object:
			case TypeCode.DateTime:
			{
				object obj = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
				if (obj != value)
				{
					return Convert.ToNumber(obj, ic);
				}
				return double.NaN;
			}
			case TypeCode.DBNull:
				return 0.0;
			case TypeCode.Boolean:
				return (double)(ic.ToBoolean(null) ? 1 : 0);
			case TypeCode.Char:
				return (double)ic.ToChar(null);
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
				return (double)ic.ToInt32(null);
			case TypeCode.UInt32:
			case TypeCode.Int64:
				return (double)ic.ToInt64(null);
			case TypeCode.UInt64:
				return ic.ToUInt64(null);
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return ic.ToDouble(null);
			case TypeCode.String:
				return Convert.ToNumber(ic.ToString(null));
			}
			return 0.0;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0001E6B7 File Offset: 0x0001D6B7
		public static double ToNumber(string str)
		{
			return Convert.ToNumber(str, true, false, Missing.Value);
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001E6C8 File Offset: 0x0001D6C8
		internal static double ToNumber(string str, bool hexOK, bool octalOK, object radix)
		{
			if (!octalOK)
			{
				try
				{
					double num = Convert.DoubleParse(str);
					if (num != 0.0)
					{
						return num;
					}
					int num2 = 0;
					int length = str.Length;
					while (num2 < length && Convert.IsWhiteSpace(str[num2]))
					{
						num2++;
					}
					if (num2 < length && str[num2] == '-')
					{
						return -0.0;
					}
					return 0.0;
				}
				catch
				{
					int length2 = str.Length;
					int i = length2 - 1;
					int num3 = 0;
					while (num3 < length2 && Convert.IsWhiteSpace(str[num3]))
					{
						num3++;
					}
					if (hexOK)
					{
						while (i >= num3 && Convert.IsWhiteSpace(str[i]))
						{
							i--;
						}
						if (num3 > i)
						{
							return 0.0;
						}
						if (i < length2 - 1)
						{
							return Convert.ToNumber(str.Substring(num3, i - num3 + 1), hexOK, octalOK, radix);
						}
					}
					else
					{
						if (length2 - num3 >= 8 && string.CompareOrdinal(str, num3, "Infinity", 0, 8) == 0)
						{
							return double.PositiveInfinity;
						}
						if (length2 - num3 >= 9 && string.CompareOrdinal(str, num3, "-Infinity", 0, 8) == 0)
						{
							return double.NegativeInfinity;
						}
						if (length2 - num3 >= 9 && string.CompareOrdinal(str, num3, "+Infinity", 0, 8) == 0)
						{
							return double.PositiveInfinity;
						}
						while (i >= num3)
						{
							char c = str[i];
							if (JSScanner.IsDigit(c))
							{
								i--;
							}
							else
							{
								IL_01AF:
								while (i >= num3)
								{
									c = str[i];
									if (JSScanner.IsDigit(c))
									{
										break;
									}
									i--;
								}
								if (i < length2 - 1)
								{
									return Convert.ToNumber(str.Substring(num3, i - num3 + 1), hexOK, octalOK, radix);
								}
								return double.NaN;
							}
						}
						goto IL_01AF;
					}
				}
			}
			int length3 = str.Length;
			int num4 = 0;
			while (num4 < length3 && Convert.IsWhiteSpace(str[num4]))
			{
				num4++;
			}
			if (num4 >= length3)
			{
				if (hexOK && octalOK)
				{
					return double.NaN;
				}
				return 0.0;
			}
			else
			{
				int num5 = 1;
				bool flag = false;
				if (str[num4] == '-')
				{
					num5 = -1;
					num4++;
					flag = true;
				}
				else if (str[num4] == '+')
				{
					num4++;
					flag = true;
				}
				while (num4 < length3 && Convert.IsWhiteSpace(str[num4]))
				{
					num4++;
				}
				bool flag2 = radix == null || radix is Missing;
				if (num4 + 8 <= length3 && flag2 && !octalOK && str.Substring(num4, 8).Equals("Infinity"))
				{
					if (num5 <= 0)
					{
						return double.NegativeInfinity;
					}
					return double.PositiveInfinity;
				}
				else
				{
					int num6 = 10;
					if (!flag2)
					{
						num6 = Convert.ToInt32(radix);
					}
					if (num6 == 0)
					{
						flag2 = true;
						num6 = 10;
					}
					else if (num6 < 2 || num6 > 36)
					{
						return double.NaN;
					}
					if (num4 < length3 - 2 && str[num4] == '0')
					{
						if (str[num4 + 1] == 'x' || str[num4 + 1] == 'X')
						{
							if (!hexOK)
							{
								return 0.0;
							}
							if (flag && !octalOK)
							{
								return double.NaN;
							}
							if (flag2)
							{
								num6 = 16;
								num4 += 2;
							}
							else if (num6 == 16)
							{
								num4 += 2;
							}
						}
						else if (octalOK && flag2)
						{
							num6 = 8;
						}
					}
					if (num4 < length3)
					{
						return Convert.ToNumber(Convert.parseRadix(str.ToCharArray(), (uint)num6, num4, num5, hexOK && octalOK));
					}
					return double.NaN;
				}
			}
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001EAA0 File Offset: 0x0001DAA0
		internal static string ToLocaleString(object value)
		{
			return Convert.ToString(value, PreferredType.LocaleString, true);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0001EAAC File Offset: 0x0001DAAC
		public static object ToNativeArray(object value, RuntimeTypeHandle handle)
		{
			if (value is ArrayObject)
			{
				Type typeFromHandle = Type.GetTypeFromHandle(handle);
				return ((ArrayObject)value).ToNativeArray(typeFromHandle);
			}
			return value;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0001EAD8 File Offset: 0x0001DAD8
		public static object ToObject(object value, VsaEngine engine)
		{
			if (value is ScriptObject)
			{
				return value;
			}
			string text = value as string;
			if (text != null)
			{
				return engine.Globals.globalObject.originalString.ConstructImplicitWrapper(text);
			}
			IConvertible iconvertible = Convert.GetIConvertible(value);
			switch (Convert.GetTypeCode(value, iconvertible))
			{
			case TypeCode.Object:
				if (value is Array)
				{
					return engine.Globals.globalObject.originalArray.ConstructImplicitWrapper((Array)value);
				}
				return value;
			case TypeCode.Boolean:
				return engine.Globals.globalObject.originalBoolean.ConstructImplicitWrapper(iconvertible.ToBoolean(null));
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return engine.Globals.globalObject.originalNumber.ConstructImplicitWrapper(value);
			case TypeCode.DateTime:
				return iconvertible.ToDateTime(null);
			case TypeCode.String:
				return engine.Globals.globalObject.originalString.ConstructImplicitWrapper(iconvertible.ToString(null));
			}
			throw new JScriptException(JSError.NeedObject);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001EC04 File Offset: 0x0001DC04
		public static object ToObject2(object value, VsaEngine engine)
		{
			if (value is ScriptObject)
			{
				return value;
			}
			IConvertible iconvertible = Convert.GetIConvertible(value);
			switch (Convert.GetTypeCode(value, iconvertible))
			{
			case TypeCode.Object:
				if (value is Array)
				{
					return engine.Globals.globalObject.originalArray.ConstructImplicitWrapper((Array)value);
				}
				return value;
			case TypeCode.Boolean:
				return engine.Globals.globalObject.originalBoolean.ConstructImplicitWrapper(iconvertible.ToBoolean(null));
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return engine.Globals.globalObject.originalNumber.ConstructImplicitWrapper(value);
			case TypeCode.DateTime:
				return iconvertible.ToDateTime(null);
			case TypeCode.String:
				return engine.Globals.globalObject.originalString.ConstructImplicitWrapper(iconvertible.ToString(null));
			}
			return null;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001ED04 File Offset: 0x0001DD04
		internal static object ToObject3(object value, VsaEngine engine)
		{
			if (value is ScriptObject)
			{
				return value;
			}
			IConvertible iconvertible = Convert.GetIConvertible(value);
			switch (Convert.GetTypeCode(value, iconvertible))
			{
			case TypeCode.Object:
				if (value is Array)
				{
					return engine.Globals.globalObject.originalArray.ConstructWrapper((Array)value);
				}
				return value;
			case TypeCode.Boolean:
				return engine.Globals.globalObject.originalBoolean.ConstructWrapper(iconvertible.ToBoolean(null));
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return engine.Globals.globalObject.originalNumber.ConstructWrapper(value);
			case TypeCode.DateTime:
				return iconvertible.ToDateTime(null);
			case TypeCode.String:
				return engine.Globals.globalObject.originalString.ConstructWrapper(iconvertible.ToString(null));
			}
			return null;
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001EE04 File Offset: 0x0001DE04
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static object ToPrimitive(object value, PreferredType preferredType)
		{
			IConvertible iconvertible = Convert.GetIConvertible(value);
			TypeCode typeCode = Convert.GetTypeCode(value, iconvertible);
			return Convert.ToPrimitive(value, preferredType, iconvertible, typeCode);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001EE2C File Offset: 0x0001DE2C
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static object ToPrimitive(object value, PreferredType preferredType, ref IConvertible ic)
		{
			TypeCode typeCode = Convert.GetTypeCode(value, ic);
			TypeCode typeCode2 = typeCode;
			if (typeCode2 == TypeCode.Object || typeCode2 == TypeCode.DateTime)
			{
				object obj = Convert.ToPrimitive(value, preferredType, ic, typeCode);
				if (obj != value)
				{
					value = obj;
					ic = Convert.GetIConvertible(value);
				}
			}
			return value;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001EE68 File Offset: 0x0001DE68
		[DebuggerStepThrough]
		[DebuggerHidden]
		private static object ToPrimitive(object value, PreferredType preferredType, IConvertible ic, TypeCode tcode)
		{
			if (tcode != TypeCode.Object)
			{
				if (tcode != TypeCode.DateTime)
				{
					return value;
				}
				return DateConstructor.ob.Construct(ic.ToDateTime(null)).GetDefaultValue(preferredType);
			}
			else
			{
				Array array = value as Array;
				if (array != null && array.Rank == 1)
				{
					value = new ArrayWrapper(ArrayPrototype.ob, array, true);
				}
				if (value is ScriptObject)
				{
					object defaultValue = ((ScriptObject)value).GetDefaultValue(preferredType);
					if (Convert.GetTypeCode(defaultValue) != TypeCode.Object)
					{
						return defaultValue;
					}
					if ((value != defaultValue || preferredType != PreferredType.String) && preferredType != PreferredType.LocaleString)
					{
						throw new JScriptException(JSError.TypeMismatch);
					}
					if (!(value is JSObject))
					{
						return value.ToString();
					}
					ScriptObject parent = ((JSObject)value).GetParent();
					if (parent is ClassScope)
					{
						return ((ClassScope)parent).GetFullName();
					}
					return "[object Object]";
				}
				else
				{
					if (value is Missing || value is Missing)
					{
						return null;
					}
					IReflect reflect;
					if (value is IReflect && !(value is Type))
					{
						reflect = (IReflect)value;
					}
					else
					{
						reflect = value.GetType();
					}
					MethodInfo methodInfo;
					if (preferredType == PreferredType.String || preferredType == PreferredType.LocaleString)
					{
						methodInfo = Convert.GetToXXXXMethod(reflect, typeof(string), true);
					}
					else
					{
						methodInfo = Convert.GetToXXXXMethod(reflect, typeof(double), true);
						if (methodInfo == null)
						{
							methodInfo = Convert.GetToXXXXMethod(reflect, typeof(long), true);
						}
						if (methodInfo == null)
						{
							methodInfo = Convert.GetToXXXXMethod(reflect, typeof(ulong), true);
						}
					}
					if (methodInfo != null)
					{
						methodInfo = new JSMethodInfo(methodInfo);
						return methodInfo.Invoke(null, BindingFlags.SuppressChangeType, null, new object[] { value }, null);
					}
					try
					{
						try
						{
							MemberInfo memberInfo = LateBinding.SelectMember(JSBinder.GetDefaultMembers(Runtime.TypeRefs, reflect));
							if (memberInfo != null)
							{
								MemberTypes memberType = memberInfo.MemberType;
								if (memberType <= MemberTypes.Method)
								{
									switch (memberType)
									{
									case MemberTypes.Event:
										return null;
									case MemberTypes.Constructor | MemberTypes.Event:
										break;
									case MemberTypes.Field:
										return ((FieldInfo)memberInfo).GetValue(value);
									default:
										if (memberType == MemberTypes.Method)
										{
											return ((MethodInfo)memberInfo).Invoke(value, new object[0]);
										}
										break;
									}
								}
								else
								{
									if (memberType == MemberTypes.Property)
									{
										return JSProperty.GetValue((PropertyInfo)memberInfo, value, null);
									}
									if (memberType == MemberTypes.NestedType)
									{
										return memberInfo;
									}
								}
							}
							if (value == reflect)
							{
								Type type = value.GetType();
								TypeReflector typeReflectorFor = TypeReflector.GetTypeReflectorFor(type);
								if (typeReflectorFor.Is__ComObject() && (!VsaEngine.executeForJSEE || !(value is IDebuggerObject)))
								{
									reflect = type;
								}
							}
							if (VsaEngine.executeForJSEE)
							{
								IDebuggerObject debuggerObject = reflect as IDebuggerObject;
								if (debuggerObject != null)
								{
									if (debuggerObject.IsScriptObject())
									{
										return reflect.InvokeMember("< JScript-" + preferredType.ToString() + " >", BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType, null, value, new object[0], null, null, new string[0]);
									}
									throw new JScriptException(JSError.NonSupportedInDebugger);
								}
							}
							return reflect.InvokeMember(string.Empty, BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType, null, value, new object[0], null, null, new string[0]);
						}
						catch (TargetInvocationException ex)
						{
							throw ex.InnerException;
						}
					}
					catch (ArgumentException)
					{
					}
					catch (IndexOutOfRangeException)
					{
					}
					catch (MissingMemberException)
					{
					}
					catch (SecurityException)
					{
					}
					catch (TargetParameterCountException)
					{
					}
					catch (COMException ex2)
					{
						if (ex2.ErrorCode != -2147352573)
						{
							throw ex2;
						}
					}
					if (preferredType == PreferredType.Number)
					{
						return value;
					}
					if (value.GetType().IsCOMObject)
					{
						return "ActiveXObject";
					}
					if (value is char[])
					{
						return new string((char[])value);
					}
					return value.ToString();
				}
			}
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0001F260 File Offset: 0x0001E260
		internal static string ToString(object value)
		{
			return Convert.ToString(value, PreferredType.String, true);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001F26A File Offset: 0x0001E26A
		[DebuggerHidden]
		[DebuggerStepThrough]
		public static string ToString(object value, bool explicitOK)
		{
			return Convert.ToString(value, PreferredType.String, explicitOK);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001F274 File Offset: 0x0001E274
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static string ToString(object value, IConvertible ic)
		{
			return Convert.ToString(value, PreferredType.String, ic, true);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001F280 File Offset: 0x0001E280
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal static string ToString(object value, PreferredType pref, bool explicitOK)
		{
			string text = value as string;
			if (text != null)
			{
				return text;
			}
			StringObject stringObject = value as StringObject;
			if (stringObject != null && stringObject.noExpando)
			{
				return stringObject.value;
			}
			return Convert.ToString(value, pref, Convert.GetIConvertible(value), explicitOK);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0001F2C0 File Offset: 0x0001E2C0
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal static string ToString(object value, PreferredType pref, IConvertible ic, bool explicitOK)
		{
			Enum @enum = value as Enum;
			if (@enum != null)
			{
				return @enum.ToString("G");
			}
			EnumWrapper enumWrapper = value as EnumWrapper;
			if (enumWrapper != null)
			{
				return enumWrapper.ToString();
			}
			TypeCode typeCode = Convert.GetTypeCode(value, ic);
			if (pref == PreferredType.LocaleString)
			{
				switch (typeCode)
				{
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Single:
				case TypeCode.Double:
				{
					double num = ic.ToDouble(null);
					return num.ToString((num <= -1000000000000000.0 || num >= 1000000000000000.0) ? "g" : "n", NumberFormatInfo.CurrentInfo);
				}
				case TypeCode.Int64:
					return ic.ToInt64(null).ToString("n", NumberFormatInfo.CurrentInfo);
				case TypeCode.UInt64:
					return ic.ToUInt64(null).ToString("n", NumberFormatInfo.CurrentInfo);
				case TypeCode.Decimal:
					return ic.ToDecimal(null).ToString("n", NumberFormatInfo.CurrentInfo);
				}
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				if (!explicitOK)
				{
					return null;
				}
				return "undefined";
			case TypeCode.Object:
				return Convert.ToString(Convert.ToPrimitive(value, pref, ref ic), ic);
			case TypeCode.DBNull:
				if (!explicitOK)
				{
					return null;
				}
				return "null";
			case TypeCode.Boolean:
				if (!ic.ToBoolean(null))
				{
					return "false";
				}
				return "true";
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Decimal:
			case TypeCode.String:
				return ic.ToString(null);
			case TypeCode.Single:
			case TypeCode.Double:
				return Convert.ToString(ic.ToDouble(null));
			case TypeCode.DateTime:
				return Convert.ToString(DateConstructor.ob.Construct(ic.ToDateTime(null)));
			}
			return null;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001F48A File Offset: 0x0001E48A
		public static string ToString(bool b)
		{
			if (!b)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001F49C File Offset: 0x0001E49C
		public static string ToString(double d)
		{
			long num = (long)d;
			if ((double)num == d)
			{
				return num.ToString(CultureInfo.InvariantCulture);
			}
			if (d != d)
			{
				return "NaN";
			}
			if (double.IsPositiveInfinity(d))
			{
				return "Infinity";
			}
			if (double.IsNegativeInfinity(d))
			{
				return "-Infinity";
			}
			double num2 = ((d < 0.0) ? (-d) : d);
			int num3 = 15;
			string text = num2.ToString("e14", CultureInfo.InvariantCulture);
			if (Convert.DoubleParse(text) != num2)
			{
				text = num2.ToString("e15", CultureInfo.InvariantCulture);
				num3 = 16;
				if (Convert.DoubleParse(text) != num2)
				{
					text = num2.ToString("e16", CultureInfo.InvariantCulture);
					num3 = 17;
					if (Convert.DoubleParse(text) != num2)
					{
						text = num2.ToString("e17", CultureInfo.InvariantCulture);
						num3 = 18;
					}
				}
			}
			int num4 = int.Parse(text.Substring(num3 + 2, text.Length - (num3 + 2)), CultureInfo.InvariantCulture);
			while (text[num3] == '0')
			{
				num3--;
			}
			int num5 = num4 + 1;
			if (num3 <= num5 && num5 <= 21)
			{
				StringBuilder stringBuilder = new StringBuilder(num5 + 1);
				if (d < 0.0)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.Append(text[0]);
				if (num3 > 1)
				{
					stringBuilder.Append(text, 2, num3 - 1);
				}
				if (num4 - num3 >= 0)
				{
					stringBuilder.Append('0', num5 - num3);
				}
				return stringBuilder.ToString();
			}
			if (0 < num5 && num5 <= 21)
			{
				StringBuilder stringBuilder2 = new StringBuilder(num3 + 2);
				if (d < 0.0)
				{
					stringBuilder2.Append('-');
				}
				stringBuilder2.Append(text[0]);
				if (num5 > 1)
				{
					stringBuilder2.Append(text, 2, num5 - 1);
				}
				stringBuilder2.Append('.');
				stringBuilder2.Append(text, num5 + 1, num3 - num5);
				return stringBuilder2.ToString();
			}
			if (-6 < num5 && num5 <= 0)
			{
				StringBuilder stringBuilder3 = new StringBuilder(2 - num5);
				if (d < 0.0)
				{
					stringBuilder3.Append("-0.");
				}
				else
				{
					stringBuilder3.Append("0.");
				}
				if (num5 < 0)
				{
					stringBuilder3.Append('0', -num5);
				}
				stringBuilder3.Append(text[0]);
				stringBuilder3.Append(text, 2, num3 - 1);
				return stringBuilder3.ToString();
			}
			StringBuilder stringBuilder4 = new StringBuilder(28);
			if (d < 0.0)
			{
				stringBuilder4.Append('-');
			}
			stringBuilder4.Append(text.Substring(0, (num3 == 1) ? 1 : (num3 + 1)));
			stringBuilder4.Append('e');
			if (num4 >= 0)
			{
				stringBuilder4.Append('+');
			}
			stringBuilder4.Append(num4);
			return stringBuilder4.ToString();
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001F74C File Offset: 0x0001E74C
		internal static string ToString(object value, int radix)
		{
			if (radix == 10 || radix < 2 || radix > 36)
			{
				return Convert.ToString(value);
			}
			double num = Convert.ToNumber(value);
			if (num == 0.0)
			{
				return "0";
			}
			if (double.IsNaN(num))
			{
				return "NaN";
			}
			if (double.IsPositiveInfinity(num))
			{
				return "Infinity";
			}
			if (double.IsNegativeInfinity(num))
			{
				return "-Infinity";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (num < 0.0)
			{
				stringBuilder.Append('-');
				num = -num;
			}
			int num2 = Convert.rgcchSig[radix - 2];
			if (num < 8.673617379884035E-19 || num >= 2.305843009213694E+18)
			{
				int num3 = (int)Math.Log(num, (double)radix) + 1;
				double num4 = Math.Pow((double)radix, (double)num3);
				if (double.IsPositiveInfinity(num4))
				{
					num4 = Math.Pow((double)radix, (double)(--num3));
				}
				else if (num4 == 0.0)
				{
					num4 = Math.Pow((double)radix, (double)(++num3));
				}
				num /= num4;
				while (num < 1.0)
				{
					num *= (double)radix;
					num3--;
				}
				int num5 = (int)num;
				stringBuilder.Append(Convert.ToDigit(num5));
				num2--;
				num -= (double)num5;
				if (num != 0.0)
				{
					stringBuilder.Append('.');
					while (num != 0.0 && num2-- > 0)
					{
						num *= (double)radix;
						num5 = (int)num;
						if (num5 >= radix)
						{
							num5 = radix - 1;
						}
						stringBuilder.Append(Convert.ToDigit(num5));
						num -= (double)num5;
					}
				}
				stringBuilder.Append((num3 >= 0) ? "(e+" : "(e");
				stringBuilder.Append(num3.ToString(CultureInfo.InvariantCulture));
				stringBuilder.Append(')');
			}
			else
			{
				int num6;
				if (num >= 1.0)
				{
					num6 = 1;
					double num7 = 1.0;
					double num8;
					while ((num8 = num7 * (double)radix) <= num)
					{
						num6++;
						num7 = num8;
					}
					for (int i = 0; i < num6; i++)
					{
						int num9 = (int)(num / num7);
						if (num9 >= radix)
						{
							num9 = radix - 1;
						}
						stringBuilder.Append(Convert.ToDigit(num9));
						num -= (double)num9 * num7;
						num7 /= (double)radix;
					}
				}
				else
				{
					stringBuilder.Append('0');
					num6 = 0;
				}
				if (num != 0.0 && num6 < num2)
				{
					stringBuilder.Append('.');
					while (num != 0.0 && num6 < num2)
					{
						num *= (double)radix;
						int num9 = (int)num;
						if (num9 >= radix)
						{
							num9 = radix - 1;
						}
						stringBuilder.Append(Convert.ToDigit(num9));
						num -= (double)num9;
						if (num9 != 0 || num6 != 0)
						{
							num6++;
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001F9EA File Offset: 0x0001E9EA
		internal static Type ToType(IReflect ir)
		{
			return Convert.ToType(Globals.TypeRefs, ir);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0001F9F8 File Offset: 0x0001E9F8
		internal static Type ToType(TypeReferences typeRefs, IReflect ir)
		{
			if (ir is Type)
			{
				return (Type)ir;
			}
			if (ir is ClassScope)
			{
				return ((ClassScope)ir).GetTypeBuilderOrEnumBuilder();
			}
			if (ir is TypedArray)
			{
				return typeRefs.ToReferenceContext(((TypedArray)ir).ToType());
			}
			if (ir is ScriptFunction)
			{
				return typeRefs.ScriptFunction;
			}
			return typeRefs.ToReferenceContext(ir.GetType());
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001FA60 File Offset: 0x0001EA60
		internal static Type ToType(string descriptor, Type elementType)
		{
			Module module = elementType.Module;
			if (module is ModuleBuilder)
			{
				return module.GetType(elementType.FullName + descriptor);
			}
			return module.Assembly.GetType(elementType.FullName + descriptor);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001FAA6 File Offset: 0x0001EAA6
		internal static string ToTypeName(IReflect ir)
		{
			if (ir is ClassScope)
			{
				return ((ClassScope)ir).GetName();
			}
			if (ir is JSObject)
			{
				return ((JSObject)ir).GetClassName();
			}
			if (ir is GlobalScope)
			{
				return "Global Object";
			}
			return ir.ToString();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001FAE4 File Offset: 0x0001EAE4
		internal static uint ToUint32(object value)
		{
			if (value is uint)
			{
				return (uint)value;
			}
			return Convert.ToUint32(value, Convert.GetIConvertible(value));
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001FB04 File Offset: 0x0001EB04
		internal static uint ToUint32(object value, IConvertible ic)
		{
			switch (Convert.GetTypeCode(value, ic))
			{
			case TypeCode.Empty:
				return 0U;
			case TypeCode.Object:
			case TypeCode.DateTime:
			{
				object obj = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
				if (obj != value)
				{
					return Convert.ToUint32(obj, ic);
				}
				return 0U;
			}
			case TypeCode.DBNull:
				return 0U;
			case TypeCode.Boolean:
				if (!ic.ToBoolean(null))
				{
					return 0U;
				}
				return 1U;
			case TypeCode.Char:
				return (uint)ic.ToChar(null);
			case TypeCode.SByte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
				return (uint)ic.ToInt64(null);
			case TypeCode.Byte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
				return ic.ToUInt32(null);
			case TypeCode.UInt64:
				return (uint)ic.ToUInt64(null);
			case TypeCode.Single:
			case TypeCode.Double:
				return (uint)Runtime.DoubleToInt64(ic.ToDouble(null));
			case TypeCode.Decimal:
				return (uint)Runtime.UncheckedDecimalToInt64(ic.ToDecimal(null));
			case TypeCode.String:
				return (uint)Runtime.DoubleToInt64(Convert.ToNumber(ic.ToString(null)));
			}
			return 0U;
		}

		// Token: 0x040001F4 RID: 500
		private static bool[,] promotable = new bool[,]
		{
			{
				true, true, true, true, true, true, true, true, true, true,
				true, true, true, true, true, true, true, true, true
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false, false, false
			},
			{
				true, true, true, true, true, true, true, true, true, true,
				true, true, true, true, true, true, true, true, true
			},
			{
				false, false, false, true, true, true, true, true, true, true,
				true, true, true, true, true, true, true, true, false
			},
			{
				false, false, false, false, true, false, false, false, true, true,
				true, true, true, true, true, true, true, true, false
			},
			{
				false, false, false, false, false, true, false, true, false, true,
				false, true, false, true, true, true, true, true, false
			},
			{
				false, false, false, false, true, false, true, true, true, true,
				true, true, true, true, true, true, true, true, false
			},
			{
				false, false, false, false, false, true, false, true, false, true,
				false, true, false, true, true, true, true, true, false
			},
			{
				false, false, false, false, true, false, false, false, true, true,
				true, true, true, true, true, true, true, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, true,
				false, true, false, false, true, true, true, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				true, true, true, false, true, true, true, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, true, false, false, false, true, true, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, true, false, false, true, true, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, true, true, true, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, true, true, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, true, false, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, true, false, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false, true, false
			},
			{
				false, false, false, false, false, false, false, false, false, false,
				false, false, false, false, false, false, false, false, true
			}
		};

		// Token: 0x040001F5 RID: 501
		private static int[] rgcchSig = new int[]
		{
			53, 34, 27, 24, 22, 20, 19, 18, 17, 17,
			16, 16, 15, 15, 14, 14, 14, 14, 14, 13,
			13, 13, 13, 13, 13, 12, 12, 12, 12, 12,
			12, 12, 12, 12, 12
		};
	}
}
