using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class Operators
	{
		internal static List<Symbols.Method> CollectOperators(Symbols.UserDefinedOperator Op, Type Type1, Type Type2, ref bool FoundType1Operators, ref bool FoundType2Operators)
		{
			bool flag = Type2 != null;
			List<Symbols.Method> list;
			if (!Symbols.IsRootObjectType(Type1) && Symbols.IsClassOrValueType(Type1))
			{
				Symbols.Container container = new Symbols.Container(Type1);
				MemberInfo[] array = container.LookupNamedMembers(Symbols.OperatorCLSNames[(int)Op]);
				MemberInfo[] array2 = array;
				object[] array3 = null;
				int num = Interaction.IIf<int>(Symbols.IsUnaryOperator(Op), 1, 2);
				string[] array4 = null;
				Type[] array5 = null;
				bool flag2 = true;
				Type type = null;
				int num2 = 0;
				int num3 = 0;
				list = OverloadResolution.CollectOverloadCandidates(array2, array3, num, array4, array5, flag2, type, ref num2, ref num3);
				if (list.Count > 0)
				{
					FoundType1Operators = true;
				}
			}
			else
			{
				list = new List<Symbols.Method>();
			}
			if (flag && !Symbols.IsRootObjectType(Type2) && Symbols.IsClassOrValueType(Type2))
			{
				Type type2;
				for (type2 = Type1; type2 != null; type2 = type2.BaseType)
				{
					if (Symbols.IsOrInheritsFrom(Type2, type2))
					{
						break;
					}
				}
				Symbols.Container container2 = new Symbols.Container(Type2);
				MemberInfo[] array6 = container2.LookupNamedMembers(Symbols.OperatorCLSNames[(int)Op]);
				MemberInfo[] array7 = array6;
				object[] array8 = null;
				int num4 = Interaction.IIf<int>(Symbols.IsUnaryOperator(Op), 1, 2);
				string[] array9 = null;
				Type[] array10 = null;
				bool flag3 = true;
				Type type3 = type2;
				int num3 = 0;
				int num2 = 0;
				List<Symbols.Method> list2 = OverloadResolution.CollectOverloadCandidates(array7, array8, num4, array9, array10, flag3, type3, ref num3, ref num2);
				if (list2.Count > 0)
				{
					FoundType2Operators = true;
				}
				list.AddRange(list2);
			}
			return list;
		}

		internal static Symbols.Method ResolveUserDefinedOperator(Symbols.UserDefinedOperator Op, object[] Arguments, bool ReportErrors)
		{
			Arguments = (object[])Arguments.Clone();
			Type type = null;
			Type type2;
			if (Arguments[0] == null)
			{
				type = Arguments[1].GetType();
				type2 = type;
				Arguments[0] = new Symbols.TypedNothing(type2);
			}
			else
			{
				type2 = Arguments[0].GetType();
				if (Arguments.Length > 1)
				{
					if (Arguments[1] != null)
					{
						type = Arguments[1].GetType();
					}
					else
					{
						type = type2;
						Arguments[1] = new Symbols.TypedNothing(type);
					}
				}
			}
			bool flag;
			bool flag2;
			List<Symbols.Method> list = Operators.CollectOperators(Op, type2, type, ref flag, ref flag2);
			if (list.Count > 0)
			{
				OverloadResolution.ResolutionFailure resolutionFailure;
				return OverloadResolution.ResolveOverloadedCall(Symbols.OperatorNames[(int)Op], list, Arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, BindingFlags.InvokeMethod, ReportErrors, ref resolutionFailure);
			}
			return null;
		}

		internal static object InvokeUserDefinedOperator(Symbols.Method OperatorMethod, bool ForceArgumentValidation, params object[] Arguments)
		{
			if ((!OperatorMethod.ArgumentsValidated || ForceArgumentValidation) && !OverloadResolution.CanMatchArguments(OperatorMethod, Arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, false, null))
			{
				string text = "";
				List<string> list = new List<string>();
				bool flag = OverloadResolution.CanMatchArguments(OperatorMethod, Arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, false, list);
				try
				{
					foreach (string text2 in list)
					{
						text = text + "\r\n    " + text2;
					}
				}
				finally
				{
					List<string>.Enumerator enumerator;
					((IDisposable)enumerator).Dispose();
				}
				text = Utils.GetResourceString("MatchArgumentFailure2", new string[]
				{
					OperatorMethod.ToString(),
					text
				});
				throw new InvalidCastException(text);
			}
			Symbols.Container container = new Symbols.Container(OperatorMethod.DeclaringType);
			return container.InvokeMethod(OperatorMethod, Arguments, null, BindingFlags.InvokeMethod);
		}

		internal static object InvokeUserDefinedOperator(Symbols.UserDefinedOperator Op, params object[] Arguments)
		{
			Symbols.Method method = Operators.ResolveUserDefinedOperator(Op, Arguments, true);
			if (method != null)
			{
				return Operators.InvokeUserDefinedOperator(method, false, Arguments);
			}
			if (Arguments.Length > 1)
			{
				throw Operators.GetNoValidOperatorException(Op, Arguments[0], Arguments[1]);
			}
			throw Operators.GetNoValidOperatorException(Op, Arguments[0]);
		}

		internal static Symbols.Method GetCallableUserDefinedOperator(Symbols.UserDefinedOperator Op, params object[] Arguments)
		{
			Symbols.Method method = Operators.ResolveUserDefinedOperator(Op, Arguments, false);
			if (method != null && !method.ArgumentsValidated && !OverloadResolution.CanMatchArguments(method, Arguments, Symbols.NoArgumentNames, Symbols.NoTypeArguments, false, null))
			{
				return null;
			}
			return method;
		}

		private Operators()
		{
		}

		private static sbyte ToVBBool(IConvertible conv)
		{
			return (-((conv.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
		}

		private static IConvertible ToVBBoolConv(IConvertible conv)
		{
			return (IConvertible)((-((conv.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0);
		}

		private static Type GetEnumResult(object Left, object Right)
		{
			if (Left != null)
			{
				if (Left is Enum)
				{
					if (Right == null)
					{
						return Left.GetType();
					}
					if (Right is Enum)
					{
						Type type = Left.GetType();
						if (type == Right.GetType())
						{
							return type;
						}
					}
				}
			}
			else if (Right is Enum)
			{
				return Right.GetType();
			}
			return null;
		}

		private static Exception GetNoValidOperatorException(Symbols.UserDefinedOperator Op, object Operand)
		{
			return new InvalidCastException(Utils.GetResourceString("UnaryOperand2", new string[]
			{
				Symbols.OperatorNames[(int)Op],
				Utils.VBFriendlyName(Operand)
			}));
		}

		private static Exception GetNoValidOperatorException(Symbols.UserDefinedOperator Op, object Left, object Right)
		{
			string text;
			if (Left == null)
			{
				text = "'Nothing'";
			}
			else
			{
				string text2 = Left as string;
				if (text2 != null)
				{
					text = Utils.GetResourceString("NoValidOperator_StringType1", new string[] { Strings.Left(text2, 32) });
				}
				else
				{
					text = Utils.GetResourceString("NoValidOperator_NonStringType1", new string[] { Utils.VBFriendlyName(Left) });
				}
			}
			string text3;
			if (Right == null)
			{
				text3 = "'Nothing'";
			}
			else
			{
				string text4 = Right as string;
				if (text4 != null)
				{
					text3 = Utils.GetResourceString("NoValidOperator_StringType1", new string[] { Strings.Left(text4, 32) });
				}
				else
				{
					text3 = Utils.GetResourceString("NoValidOperator_NonStringType1", new string[] { Utils.VBFriendlyName(Right) });
				}
			}
			return new InvalidCastException(Utils.GetResourceString("BinaryOperands3", new string[]
			{
				Symbols.OperatorNames[(int)Op],
				text,
				text3
			}));
		}

		public static object CompareObjectEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Equal, new object[] { Left, Right });
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Equal, Left, Right);
			default:
				return compareClass == Operators.CompareClass.Equal;
			}
		}

		public static bool ConditionalCompareObjectEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Equal, new object[] { Left, Right }));
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Equal, Left, Right);
			default:
				return compareClass == Operators.CompareClass.Equal;
			}
		}

		public static object CompareObjectNotEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return true;
			case Operators.CompareClass.UserDefined:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.NotEqual, new object[] { Left, Right });
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.NotEqual, Left, Right);
			default:
				return compareClass != Operators.CompareClass.Equal;
			}
		}

		public static bool ConditionalCompareObjectNotEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return true;
			case Operators.CompareClass.UserDefined:
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.NotEqual, new object[] { Left, Right }));
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.NotEqual, Left, Right);
			default:
				return compareClass != Operators.CompareClass.Equal;
			}
		}

		public static object CompareObjectLess(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Less, new object[] { Left, Right });
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Less, Left, Right);
			default:
				return compareClass < Operators.CompareClass.Equal;
			}
		}

		public static bool ConditionalCompareObjectLess(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Less, new object[] { Left, Right }));
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Less, Left, Right);
			default:
				return compareClass < Operators.CompareClass.Equal;
			}
		}

		public static object CompareObjectLessEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.LessEqual, new object[] { Left, Right });
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.LessEqual, Left, Right);
			default:
				return compareClass <= Operators.CompareClass.Equal;
			}
		}

		public static bool ConditionalCompareObjectLessEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.LessEqual, new object[] { Left, Right }));
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.LessEqual, Left, Right);
			default:
				return compareClass <= Operators.CompareClass.Equal;
			}
		}

		public static object CompareObjectGreaterEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.GreaterEqual, new object[] { Left, Right });
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.GreaterEqual, Left, Right);
			default:
				return compareClass >= Operators.CompareClass.Equal;
			}
		}

		public static bool ConditionalCompareObjectGreaterEqual(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.GreaterEqual, new object[] { Left, Right }));
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.GreaterEqual, Left, Right);
			default:
				return compareClass >= Operators.CompareClass.Equal;
			}
		}

		public static object CompareObjectGreater(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Greater, new object[] { Left, Right });
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Greater, Left, Right);
			default:
				return compareClass > Operators.CompareClass.Equal;
			}
		}

		public static bool ConditionalCompareObjectGreater(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return false;
			case Operators.CompareClass.UserDefined:
				return Conversions.ToBoolean(Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Greater, new object[] { Left, Right }));
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Greater, Left, Right);
			default:
				return compareClass > Operators.CompareClass.Equal;
			}
		}

		public static int CompareObject(object Left, object Right, bool TextCompare)
		{
			Operators.CompareClass compareClass = Operators.CompareObject2(Left, Right, TextCompare);
			switch (compareClass)
			{
			case Operators.CompareClass.Unordered:
				return 0;
			case Operators.CompareClass.UserDefined:
			case Operators.CompareClass.Undefined:
				throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.IsTrue, Left, Right);
			default:
				return (int)compareClass;
			}
		}

		private static Operators.CompareClass CompareObject2(object Left, object Right, bool TextCompare)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object)
			{
				char[] array = Left as char[];
				if (array != null && (typeCode2 == TypeCode.String || typeCode2 == TypeCode.Empty || (typeCode2 == TypeCode.Object && Right is char[])))
				{
					Left = new string(array);
					convertible = (IConvertible)Left;
					typeCode = TypeCode.String;
				}
			}
			if (typeCode2 == TypeCode.Object)
			{
				char[] array2 = Right as char[];
				if (array2 != null && (typeCode == TypeCode.String || typeCode == TypeCode.Empty))
				{
					Right = new string(array2);
					convertible2 = (IConvertible)Right;
					typeCode2 = TypeCode.String;
				}
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.CompareClass.Equal;
			case TypeCode.Boolean:
				return Operators.CompareBoolean(false, convertible2.ToBoolean(null));
			case TypeCode.Char:
				return Operators.CompareChar('\0', convertible2.ToChar(null));
			case TypeCode.SByte:
				return Operators.CompareInt32(0, (int)convertible2.ToSByte(null));
			case TypeCode.Byte:
				return Operators.CompareInt32(0, (int)convertible2.ToByte(null));
			case TypeCode.Int16:
				return Operators.CompareInt32(0, (int)convertible2.ToInt16(null));
			case TypeCode.UInt16:
				return Operators.CompareInt32(0, (int)convertible2.ToUInt16(null));
			case TypeCode.Int32:
				return Operators.CompareInt32(0, convertible2.ToInt32(null));
			case TypeCode.UInt32:
				return Operators.CompareUInt32(0U, convertible2.ToUInt32(null));
			case TypeCode.Int64:
				return Operators.CompareInt64(0L, convertible2.ToInt64(null));
			case TypeCode.UInt64:
				return Operators.CompareUInt64(0UL, convertible2.ToUInt64(null));
			case TypeCode.Single:
				return Operators.CompareSingle(0f, convertible2.ToSingle(null));
			case TypeCode.Double:
				return Operators.CompareDouble(0.0, convertible2.ToDouble(null));
			case TypeCode.Decimal:
				return Operators.CompareDecimal((IConvertible)0m, convertible2);
			case TypeCode.DateTime:
				return Operators.CompareDate(DateTime.MinValue, convertible2.ToDateTime(null));
			case TypeCode.String:
				return (Operators.CompareClass)Operators.CompareString(null, convertible2.ToString(null), TextCompare);
			case (TypeCode)57:
				return Operators.CompareBoolean(convertible.ToBoolean(null), false);
			case (TypeCode)60:
				return Operators.CompareBoolean(convertible.ToBoolean(null), convertible2.ToBoolean(null));
			case (TypeCode)62:
				return Operators.CompareInt32((int)Operators.ToVBBool(convertible), (int)convertible2.ToSByte(null));
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.CompareInt32((int)Operators.ToVBBool(convertible), (int)convertible2.ToInt16(null));
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.CompareInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)67:
			case (TypeCode)68:
				return Operators.CompareInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)69:
			case (TypeCode)72:
				return Operators.CompareDecimal(Operators.ToVBBoolConv(convertible), convertible2);
			case (TypeCode)70:
				return Operators.CompareSingle((float)Operators.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)71:
				return Operators.CompareDouble((double)Operators.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)75:
				return Operators.CompareBoolean(convertible.ToBoolean(null), Conversions.ToBoolean(convertible2.ToString(null)));
			case (TypeCode)76:
				return Operators.CompareChar(convertible.ToChar(null), '\0');
			case (TypeCode)80:
				return Operators.CompareChar(convertible.ToChar(null), convertible2.ToChar(null));
			case (TypeCode)94:
			case (TypeCode)346:
			case (TypeCode)360:
				return (Operators.CompareClass)Operators.CompareString(convertible.ToString(null), convertible2.ToString(null), TextCompare);
			case (TypeCode)95:
				return Operators.CompareInt32((int)convertible.ToSByte(null), 0);
			case (TypeCode)98:
				return Operators.CompareInt32((int)convertible.ToSByte(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)100:
				return Operators.CompareInt32((int)convertible.ToSByte(null), (int)convertible2.ToSByte(null));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
				return Operators.CompareInt32((int)convertible.ToInt16(null), (int)convertible2.ToInt16(null));
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
				return Operators.CompareInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)125:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)163:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
				return Operators.CompareInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)107:
			case (TypeCode)110:
			case (TypeCode)129:
			case (TypeCode)145:
			case (TypeCode)148:
			case (TypeCode)167:
			case (TypeCode)183:
			case (TypeCode)186:
			case (TypeCode)205:
			case (TypeCode)221:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)243:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)300:
				return Operators.CompareDecimal(convertible, convertible2);
			case (TypeCode)108:
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)165:
			case (TypeCode)184:
			case (TypeCode)203:
			case (TypeCode)222:
			case (TypeCode)241:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return Operators.CompareSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)109:
			case (TypeCode)128:
			case (TypeCode)147:
			case (TypeCode)166:
			case (TypeCode)185:
			case (TypeCode)204:
			case (TypeCode)223:
			case (TypeCode)242:
			case (TypeCode)261:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return Operators.CompareDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.CompareDouble(convertible.ToDouble(null), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)114:
				return Operators.CompareInt32((int)convertible.ToByte(null), 0);
			case (TypeCode)117:
				return Operators.CompareInt32((int)convertible.ToInt16(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)120:
				return Operators.CompareInt32((int)convertible.ToByte(null), (int)convertible2.ToByte(null));
			case (TypeCode)122:
			case (TypeCode)158:
			case (TypeCode)160:
				return Operators.CompareInt32((int)convertible.ToUInt16(null), (int)convertible2.ToUInt16(null));
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
			case (TypeCode)200:
				return Operators.CompareUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null));
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
			case (TypeCode)240:
				return Operators.CompareUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null));
			case (TypeCode)133:
				return Operators.CompareInt32((int)convertible.ToInt16(null), 0);
			case (TypeCode)136:
				return Operators.CompareInt32((int)convertible.ToInt16(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)152:
				return Operators.CompareInt32((int)convertible.ToUInt16(null), 0);
			case (TypeCode)155:
				return Operators.CompareInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)171:
				return Operators.CompareInt32(convertible.ToInt32(null), 0);
			case (TypeCode)174:
				return Operators.CompareInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)190:
				return Operators.CompareUInt32(convertible.ToUInt32(null), 0U);
			case (TypeCode)193:
				return Operators.CompareInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)209:
				return Operators.CompareInt64(convertible.ToInt64(null), 0L);
			case (TypeCode)212:
				return Operators.CompareInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)228:
				return Operators.CompareUInt64(convertible.ToUInt64(null), 0UL);
			case (TypeCode)231:
				return Operators.CompareDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)247:
				return Operators.CompareSingle(convertible.ToSingle(null), 0f);
			case (TypeCode)250:
				return Operators.CompareSingle(convertible.ToSingle(null), (float)Operators.ToVBBool(convertible2));
			case (TypeCode)266:
				return Operators.CompareDouble(convertible.ToDouble(null), 0.0);
			case (TypeCode)269:
				return Operators.CompareDouble(convertible.ToDouble(null), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)285:
				return Operators.CompareDecimal(convertible, (IConvertible)0m);
			case (TypeCode)288:
				return Operators.CompareDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)304:
				return Operators.CompareDate(convertible.ToDateTime(null), DateTime.MinValue);
			case (TypeCode)320:
				return Operators.CompareDate(convertible.ToDateTime(null), convertible2.ToDateTime(null));
			case (TypeCode)322:
				return Operators.CompareDate(convertible.ToDateTime(null), Conversions.ToDate(convertible2.ToString(null)));
			case (TypeCode)342:
				return (Operators.CompareClass)Operators.CompareString(convertible.ToString(null), null, TextCompare);
			case (TypeCode)345:
				return Operators.CompareBoolean(Conversions.ToBoolean(convertible.ToString(null)), convertible2.ToBoolean(null));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.CompareDouble(Conversions.ToDouble(convertible.ToString(null)), convertible2.ToDouble(null));
			case (TypeCode)358:
				return Operators.CompareDate(Conversions.ToDate(convertible.ToString(null)), convertible2.ToDateTime(null));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.CompareClass.UserDefined;
			}
			return Operators.CompareClass.Undefined;
		}

		private static Operators.CompareClass CompareBoolean(bool Left, bool Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left < Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareInt32(int Left, int Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareUInt32(uint Left, uint Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareInt64(long Left, long Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareUInt64(ulong Left, ulong Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareDecimal(IConvertible Left, IConvertible Right)
		{
			int num = decimal.Compare(Left.ToDecimal(null), Right.ToDecimal(null));
			if (num == 0)
			{
				return Operators.CompareClass.Equal;
			}
			if (num > 0)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareSingle(float Left, float Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left < Right)
			{
				return Operators.CompareClass.Less;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Unordered;
		}

		private static Operators.CompareClass CompareDouble(double Left, double Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left < Right)
			{
				return Operators.CompareClass.Less;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Unordered;
		}

		private static Operators.CompareClass CompareDate(DateTime Left, DateTime Right)
		{
			int num = DateTime.Compare(Left, Right);
			if (num == 0)
			{
				return Operators.CompareClass.Equal;
			}
			if (num > 0)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		private static Operators.CompareClass CompareChar(char Left, char Right)
		{
			if (Left == Right)
			{
				return Operators.CompareClass.Equal;
			}
			if (Left > Right)
			{
				return Operators.CompareClass.Greater;
			}
			return Operators.CompareClass.Less;
		}

		public static int CompareString(string Left, string Right, bool TextCompare)
		{
			if (Left == Right)
			{
				return 0;
			}
			if (Left == null)
			{
				if (Right.Length == 0)
				{
					return 0;
				}
				return -1;
			}
			else if (Right == null)
			{
				if (Left.Length == 0)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				int num;
				if (TextCompare)
				{
					num = Utils.GetCultureInfo().CompareInfo.Compare(Left, Right, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
				}
				else
				{
					num = string.CompareOrdinal(Left, Right);
				}
				if (num == 0)
				{
					return 0;
				}
				if (num > 0)
				{
					return 1;
				}
				return -1;
			}
		}

		public static object PlusObject(object Operand)
		{
			if (Operand == null)
			{
				return Operators.Boxed_ZeroInteger;
			}
			IConvertible convertible = Operand as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Operand == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Object:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.UnaryPlus, new object[] { Operand });
			case TypeCode.Boolean:
				return (-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0;
			case TypeCode.SByte:
				return convertible.ToSByte(null);
			case TypeCode.Byte:
				return convertible.ToByte(null);
			case TypeCode.Int16:
				return convertible.ToInt16(null);
			case TypeCode.UInt16:
				return convertible.ToUInt16(null);
			case TypeCode.Int32:
				return convertible.ToInt32(null);
			case TypeCode.UInt32:
				return convertible.ToUInt32(null);
			case TypeCode.Int64:
				return convertible.ToInt64(null);
			case TypeCode.UInt64:
				return convertible.ToUInt64(null);
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Operand;
			case TypeCode.String:
				return Conversions.ToDouble(convertible.ToString(null));
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.UnaryPlus, Operand);
		}

		public static object NegateObject(object Operand)
		{
			IConvertible convertible = Operand as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Operand == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Object:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Negate, new object[] { Operand });
			case TypeCode.Boolean:
				if (Operand is bool)
				{
					return Operators.NegateBoolean((bool)Operand);
				}
				return Operators.NegateBoolean(convertible.ToBoolean(null));
			case TypeCode.SByte:
				if (Operand is sbyte)
				{
					return Operators.NegateSByte((sbyte)Operand);
				}
				return Operators.NegateSByte(convertible.ToSByte(null));
			case TypeCode.Byte:
				if (Operand is byte)
				{
					return Operators.NegateByte((byte)Operand);
				}
				return Operators.NegateByte(convertible.ToByte(null));
			case TypeCode.Int16:
				if (Operand is short)
				{
					return Operators.NegateInt16((short)Operand);
				}
				return Operators.NegateInt16(convertible.ToInt16(null));
			case TypeCode.UInt16:
				if (Operand is ushort)
				{
					return Operators.NegateUInt16((ushort)Operand);
				}
				return Operators.NegateUInt16(convertible.ToUInt16(null));
			case TypeCode.Int32:
				if (Operand is int)
				{
					return Operators.NegateInt32((int)Operand);
				}
				return Operators.NegateInt32(convertible.ToInt32(null));
			case TypeCode.UInt32:
				if (Operand is uint)
				{
					return Operators.NegateUInt32((uint)Operand);
				}
				return Operators.NegateUInt32(convertible.ToUInt32(null));
			case TypeCode.Int64:
				if (Operand is long)
				{
					return Operators.NegateInt64((long)Operand);
				}
				return Operators.NegateInt64(convertible.ToInt64(null));
			case TypeCode.UInt64:
				if (Operand is ulong)
				{
					return Operators.NegateUInt64((ulong)Operand);
				}
				return Operators.NegateUInt64(convertible.ToUInt64(null));
			case TypeCode.Single:
				if (Operand is float)
				{
					return Operators.NegateSingle((float)Operand);
				}
				return Operators.NegateSingle(convertible.ToSingle(null));
			case TypeCode.Double:
				if (Operand is double)
				{
					return Operators.NegateDouble((double)Operand);
				}
				return Operators.NegateDouble(convertible.ToDouble(null));
			case TypeCode.Decimal:
				if (Operand is decimal)
				{
					return Operators.NegateDecimal((decimal)Operand);
				}
				return Operators.NegateDecimal(convertible.ToDecimal(null));
			case TypeCode.String:
			{
				string text = Operand as string;
				if (text != null)
				{
					return Operators.NegateString(text);
				}
				return Operators.NegateString(convertible.ToString(null));
			}
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Negate, Operand);
		}

		private static object NegateBoolean(bool Operand)
		{
			return -((-((Operand > false) ? 1 : 0)) ? 1 : 0);
		}

		private static object NegateSByte(sbyte Operand)
		{
			if (Operand == -128)
			{
				return 128;
			}
			return -Operand;
		}

		private static object NegateByte(byte Operand)
		{
			checked
			{
				return (short)unchecked(-(short)Operand);
			}
		}

		private static object NegateInt16(short Operand)
		{
			if (Operand == -32768)
			{
				return 32768;
			}
			return -Operand;
		}

		private static object NegateUInt16(ushort Operand)
		{
			return (int)(checked(0 - Operand));
		}

		private static object NegateInt32(int Operand)
		{
			if (Operand == -2147483648)
			{
				return (long)((ulong)int.MinValue);
			}
			return checked(0 - Operand);
		}

		private static object NegateUInt32(uint Operand)
		{
			checked
			{
				return (long)(0UL - unchecked((ulong)Operand));
			}
		}

		private static object NegateInt64(long Operand)
		{
			if (Operand == -9223372036854775808L)
			{
				return 9223372036854775808m;
			}
			return checked(0L - Operand);
		}

		private static object NegateUInt64(ulong Operand)
		{
			return decimal.Negate(new decimal(Operand));
		}

		private static object NegateDecimal(decimal Operand)
		{
			object obj;
			try
			{
				obj = decimal.Negate(Operand);
			}
			catch (OverflowException ex)
			{
				obj = -Convert.ToDouble(Operand);
			}
			return obj;
		}

		private static object NegateSingle(float Operand)
		{
			return -Operand;
		}

		private static object NegateDouble(double Operand)
		{
			return -Operand;
		}

		private static object NegateString(string Operand)
		{
			return -Conversions.ToDouble(Operand);
		}

		public static object NotObject(object Operand)
		{
			IConvertible convertible = Operand as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Operand == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return -1;
			case TypeCode.Object:
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Not, new object[] { Operand });
			case TypeCode.Boolean:
				return Operators.NotBoolean(convertible.ToBoolean(null));
			case TypeCode.SByte:
				return Operators.NotSByte(convertible.ToSByte(null), Operand.GetType());
			case TypeCode.Byte:
				return Operators.NotByte(convertible.ToByte(null), Operand.GetType());
			case TypeCode.Int16:
				return Operators.NotInt16(convertible.ToInt16(null), Operand.GetType());
			case TypeCode.UInt16:
				return Operators.NotUInt16(convertible.ToUInt16(null), Operand.GetType());
			case TypeCode.Int32:
				return Operators.NotInt32(convertible.ToInt32(null), Operand.GetType());
			case TypeCode.UInt32:
				return Operators.NotUInt32(convertible.ToUInt32(null), Operand.GetType());
			case TypeCode.Int64:
				return Operators.NotInt64(convertible.ToInt64(null), Operand.GetType());
			case TypeCode.UInt64:
				return Operators.NotUInt64(convertible.ToUInt64(null), Operand.GetType());
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Operators.NotInt64(convertible.ToInt64(null));
			case TypeCode.String:
				return Operators.NotInt64(Conversions.ToLong(convertible.ToString(null)));
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Not, Operand);
		}

		private static object NotBoolean(bool Operand)
		{
			return !Operand;
		}

		private static object NotSByte(sbyte Operand, Type OperandType)
		{
			sbyte b = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, b);
			}
			return b;
		}

		private static object NotByte(byte Operand, Type OperandType)
		{
			byte b = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, b);
			}
			return b;
		}

		private static object NotInt16(short Operand, Type OperandType)
		{
			short num = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, num);
			}
			return num;
		}

		private static object NotUInt16(ushort Operand, Type OperandType)
		{
			ushort num = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, num);
			}
			return num;
		}

		private static object NotInt32(int Operand, Type OperandType)
		{
			int num = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, num);
			}
			return num;
		}

		private static object NotUInt32(uint Operand, Type OperandType)
		{
			uint num = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, num);
			}
			return num;
		}

		private static object NotInt64(long Operand)
		{
			return ~Operand;
		}

		private static object NotInt64(long Operand, Type OperandType)
		{
			long num = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, num);
			}
			return num;
		}

		private static object NotUInt64(ulong Operand, Type OperandType)
		{
			ulong num = ~Operand;
			if (OperandType.IsEnum)
			{
				return Enum.ToObject(OperandType, num);
			}
			return num;
		}

		public static object AndObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Boolean:
			case (TypeCode)57:
				return false;
			case TypeCode.SByte:
			case (TypeCode)95:
				return Operators.AndSByte(0, 0, Operators.GetEnumResult(Left, Right));
			case TypeCode.Byte:
			case (TypeCode)114:
				return Operators.AndByte(0, 0, Operators.GetEnumResult(Left, Right));
			case TypeCode.Int16:
			case (TypeCode)133:
				return Operators.AndInt16(0, 0, Operators.GetEnumResult(Left, Right));
			case TypeCode.UInt16:
			case (TypeCode)152:
				return Operators.AndUInt16(0, 0, Operators.GetEnumResult(Left, Right));
			case TypeCode.Int32:
			case (TypeCode)171:
				return Operators.AndInt32(0, 0, Operators.GetEnumResult(Left, Right));
			case TypeCode.UInt32:
			case (TypeCode)190:
				return Operators.AndUInt32(0U, 0U, Operators.GetEnumResult(Left, Right));
			case TypeCode.Int64:
			case (TypeCode)209:
				return Operators.AndInt64(0L, 0L, Operators.GetEnumResult(Left, Right));
			case TypeCode.UInt64:
			case (TypeCode)228:
				return Operators.AndUInt64(0UL, 0UL, Operators.GetEnumResult(Left, Right));
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Operators.AndInt64(0L, convertible2.ToInt64(null), null);
			case TypeCode.String:
				return Operators.AndInt64(0L, Conversions.ToLong(convertible2.ToString(null)), null);
			case (TypeCode)60:
				return Operators.AndBoolean(convertible.ToBoolean(null), convertible2.ToBoolean(null));
			case (TypeCode)62:
				return Operators.AndSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null), null);
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.AndInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null), null);
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.AndInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null), null);
			case (TypeCode)67:
			case (TypeCode)68:
			case (TypeCode)69:
			case (TypeCode)70:
			case (TypeCode)71:
			case (TypeCode)72:
				return Operators.AndInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null), null);
			case (TypeCode)75:
				return Operators.AndBoolean(convertible.ToBoolean(null), Conversions.ToBoolean(convertible2.ToString(null)));
			case (TypeCode)98:
				return Operators.AndSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2), null);
			case (TypeCode)100:
				return Operators.AndSByte(convertible.ToSByte(null), convertible2.ToSByte(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
				return Operators.AndInt16(convertible.ToInt16(null), convertible2.ToInt16(null), null);
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
				return Operators.AndInt32(convertible.ToInt32(null), convertible2.ToInt32(null), null);
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)107:
			case (TypeCode)108:
			case (TypeCode)109:
			case (TypeCode)110:
			case (TypeCode)125:
			case (TypeCode)127:
			case (TypeCode)128:
			case (TypeCode)129:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)145:
			case (TypeCode)146:
			case (TypeCode)147:
			case (TypeCode)148:
			case (TypeCode)163:
			case (TypeCode)165:
			case (TypeCode)166:
			case (TypeCode)167:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)183:
			case (TypeCode)184:
			case (TypeCode)185:
			case (TypeCode)186:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)203:
			case (TypeCode)204:
			case (TypeCode)205:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)221:
			case (TypeCode)222:
			case (TypeCode)223:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)241:
			case (TypeCode)242:
			case (TypeCode)243:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)261:
			case (TypeCode)262:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)298:
			case (TypeCode)299:
			case (TypeCode)300:
				return Operators.AndInt64(convertible.ToInt64(null), convertible2.ToInt64(null), null);
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.AndInt64(convertible.ToInt64(null), Conversions.ToLong(convertible2.ToString(null)), null);
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.AndInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2), null);
			case (TypeCode)120:
				return Operators.AndByte(convertible.ToByte(null), convertible2.ToByte(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)122:
			case (TypeCode)158:
				return Operators.AndUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null), null);
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
				return Operators.AndUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null), null);
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
				return Operators.AndUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null), null);
			case (TypeCode)140:
				return Operators.AndInt16(convertible.ToInt16(null), convertible2.ToInt16(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.AndInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2), null);
			case (TypeCode)160:
				return Operators.AndUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)180:
				return Operators.AndInt32(convertible.ToInt32(null), convertible2.ToInt32(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)193:
			case (TypeCode)212:
			case (TypeCode)231:
			case (TypeCode)250:
			case (TypeCode)269:
			case (TypeCode)288:
				return Operators.AndInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2), null);
			case (TypeCode)200:
				return Operators.AndUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)220:
				return Operators.AndInt64(convertible.ToInt64(null), convertible2.ToInt64(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)240:
				return Operators.AndUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return Operators.AndInt64(convertible.ToInt64(null), 0L, null);
			case (TypeCode)342:
				return Operators.AndInt64(Conversions.ToLong(convertible.ToString(null)), 0L, null);
			case (TypeCode)345:
				return Operators.AndBoolean(Conversions.ToBoolean(convertible.ToString(null)), convertible2.ToBoolean(null));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.AndInt64(Conversions.ToLong(convertible.ToString(null)), convertible2.ToInt64(null), null);
			case (TypeCode)360:
				return Operators.AndInt64(Conversions.ToLong(convertible.ToString(null)), Conversions.ToLong(convertible2.ToString(null)), null);
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.And, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.And, Left, Right);
		}

		private static object AndBoolean(bool Left, bool Right)
		{
			return Left && Right;
		}

		private static object AndSByte(sbyte Left, sbyte Right, Type EnumType = null)
		{
			sbyte b = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, b);
			}
			return b;
		}

		private static object AndByte(byte Left, byte Right, Type EnumType = null)
		{
			byte b = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, b);
			}
			return b;
		}

		private static object AndInt16(short Left, short Right, Type EnumType = null)
		{
			short num = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object AndUInt16(ushort Left, ushort Right, Type EnumType = null)
		{
			ushort num = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object AndInt32(int Left, int Right, Type EnumType = null)
		{
			int num = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object AndUInt32(uint Left, uint Right, Type EnumType = null)
		{
			uint num = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object AndInt64(long Left, long Right, Type EnumType = null)
		{
			long num = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object AndUInt64(ulong Left, ulong Right, Type EnumType = null)
		{
			ulong num = Left & Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		public static object OrObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Boolean:
				return Operators.OrBoolean(false, convertible2.ToBoolean(null));
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
				return Right;
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Operators.OrInt64(0L, convertible2.ToInt64(null), null);
			case TypeCode.String:
				return Operators.OrInt64(0L, Conversions.ToLong(convertible2.ToString(null)), null);
			case (TypeCode)57:
				return Operators.OrBoolean(convertible.ToBoolean(null), false);
			case (TypeCode)60:
				return Operators.OrBoolean(convertible.ToBoolean(null), convertible2.ToBoolean(null));
			case (TypeCode)62:
				return Operators.OrSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null), null);
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.OrInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null), null);
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.OrInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null), null);
			case (TypeCode)67:
			case (TypeCode)68:
			case (TypeCode)69:
			case (TypeCode)70:
			case (TypeCode)71:
			case (TypeCode)72:
				return Operators.OrInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null), null);
			case (TypeCode)75:
				return Operators.OrBoolean(convertible.ToBoolean(null), Conversions.ToBoolean(convertible2.ToString(null)));
			case (TypeCode)95:
			case (TypeCode)114:
			case (TypeCode)133:
			case (TypeCode)152:
			case (TypeCode)171:
			case (TypeCode)190:
			case (TypeCode)209:
			case (TypeCode)228:
				return Left;
			case (TypeCode)98:
				return Operators.OrSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2), null);
			case (TypeCode)100:
				return Operators.OrSByte(convertible.ToSByte(null), convertible2.ToSByte(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
				return Operators.OrInt16(convertible.ToInt16(null), convertible2.ToInt16(null), null);
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
				return Operators.OrInt32(convertible.ToInt32(null), convertible2.ToInt32(null), null);
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)107:
			case (TypeCode)108:
			case (TypeCode)109:
			case (TypeCode)110:
			case (TypeCode)125:
			case (TypeCode)127:
			case (TypeCode)128:
			case (TypeCode)129:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)145:
			case (TypeCode)146:
			case (TypeCode)147:
			case (TypeCode)148:
			case (TypeCode)163:
			case (TypeCode)165:
			case (TypeCode)166:
			case (TypeCode)167:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)183:
			case (TypeCode)184:
			case (TypeCode)185:
			case (TypeCode)186:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)203:
			case (TypeCode)204:
			case (TypeCode)205:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)221:
			case (TypeCode)222:
			case (TypeCode)223:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)241:
			case (TypeCode)242:
			case (TypeCode)243:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)261:
			case (TypeCode)262:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)298:
			case (TypeCode)299:
			case (TypeCode)300:
				return Operators.OrInt64(convertible.ToInt64(null), convertible2.ToInt64(null), null);
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.OrInt64(convertible.ToInt64(null), Conversions.ToLong(convertible2.ToString(null)), null);
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.OrInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2), null);
			case (TypeCode)120:
				return Operators.OrByte(convertible.ToByte(null), convertible2.ToByte(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)122:
			case (TypeCode)158:
				return Operators.OrUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null), null);
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
				return Operators.OrUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null), null);
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
				return Operators.OrUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null), null);
			case (TypeCode)140:
				return Operators.OrInt16(convertible.ToInt16(null), convertible2.ToInt16(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.OrInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2), null);
			case (TypeCode)160:
				return Operators.OrUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)180:
				return Operators.OrInt32(convertible.ToInt32(null), convertible2.ToInt32(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)193:
			case (TypeCode)212:
			case (TypeCode)231:
			case (TypeCode)250:
			case (TypeCode)269:
			case (TypeCode)288:
				return Operators.OrInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2), null);
			case (TypeCode)200:
				return Operators.OrUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)220:
				return Operators.OrInt64(convertible.ToInt64(null), convertible2.ToInt64(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)240:
				return Operators.OrUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return Operators.OrInt64(convertible.ToInt64(null), 0L, null);
			case (TypeCode)342:
				return Operators.OrInt64(Conversions.ToLong(convertible.ToString(null)), 0L, null);
			case (TypeCode)345:
				return Operators.OrBoolean(Conversions.ToBoolean(convertible.ToString(null)), convertible2.ToBoolean(null));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.OrInt64(Conversions.ToLong(convertible.ToString(null)), convertible2.ToInt64(null), null);
			case (TypeCode)360:
				return Operators.OrInt64(Conversions.ToLong(convertible.ToString(null)), Conversions.ToLong(convertible2.ToString(null)), null);
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Or, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Or, Left, Right);
		}

		private static object OrBoolean(bool Left, bool Right)
		{
			return Left || Right;
		}

		private static object OrSByte(sbyte Left, sbyte Right, Type EnumType = null)
		{
			sbyte b = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, b);
			}
			return b;
		}

		private static object OrByte(byte Left, byte Right, Type EnumType = null)
		{
			byte b = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, b);
			}
			return b;
		}

		private static object OrInt16(short Left, short Right, Type EnumType = null)
		{
			short num = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object OrUInt16(ushort Left, ushort Right, Type EnumType = null)
		{
			ushort num = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object OrInt32(int Left, int Right, Type EnumType = null)
		{
			int num = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object OrUInt32(uint Left, uint Right, Type EnumType = null)
		{
			uint num = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object OrInt64(long Left, long Right, Type EnumType = null)
		{
			long num = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object OrUInt64(ulong Left, ulong Right, Type EnumType = null)
		{
			ulong num = Left | Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		public static object XorObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Boolean:
				return Operators.XorBoolean(false, convertible2.ToBoolean(null));
			case TypeCode.SByte:
				return Operators.XorSByte(0, convertible2.ToSByte(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.Byte:
				return Operators.XorByte(0, convertible2.ToByte(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.Int16:
				return Operators.XorInt16(0, convertible2.ToInt16(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.UInt16:
				return Operators.XorUInt16(0, convertible2.ToUInt16(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.Int32:
				return Operators.XorInt32(0, convertible2.ToInt32(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.UInt32:
				return Operators.XorUInt32(0U, convertible2.ToUInt32(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.Int64:
				return Operators.XorInt64(0L, convertible2.ToInt64(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.UInt64:
				return Operators.XorUInt64(0UL, convertible2.ToUInt64(null), Operators.GetEnumResult(Left, Right));
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Operators.XorInt64(0L, convertible2.ToInt64(null), null);
			case TypeCode.String:
				return Operators.XorInt64(0L, Conversions.ToLong(convertible2.ToString(null)), null);
			case (TypeCode)57:
				return Operators.XorBoolean(convertible.ToBoolean(null), false);
			case (TypeCode)60:
				return Operators.XorBoolean(convertible.ToBoolean(null), convertible2.ToBoolean(null));
			case (TypeCode)62:
				return Operators.XorSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null), null);
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.XorInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null), null);
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.XorInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null), null);
			case (TypeCode)67:
			case (TypeCode)68:
			case (TypeCode)69:
			case (TypeCode)70:
			case (TypeCode)71:
			case (TypeCode)72:
				return Operators.XorInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null), null);
			case (TypeCode)75:
				return Operators.XorBoolean(convertible.ToBoolean(null), Conversions.ToBoolean(convertible2.ToString(null)));
			case (TypeCode)95:
				return Operators.XorSByte(convertible.ToSByte(null), 0, Operators.GetEnumResult(Left, Right));
			case (TypeCode)98:
				return Operators.XorSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2), null);
			case (TypeCode)100:
				return Operators.XorSByte(convertible.ToSByte(null), convertible2.ToSByte(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
				return Operators.XorInt16(convertible.ToInt16(null), convertible2.ToInt16(null), null);
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
				return Operators.XorInt32(convertible.ToInt32(null), convertible2.ToInt32(null), null);
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)107:
			case (TypeCode)108:
			case (TypeCode)109:
			case (TypeCode)110:
			case (TypeCode)125:
			case (TypeCode)127:
			case (TypeCode)128:
			case (TypeCode)129:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)145:
			case (TypeCode)146:
			case (TypeCode)147:
			case (TypeCode)148:
			case (TypeCode)163:
			case (TypeCode)165:
			case (TypeCode)166:
			case (TypeCode)167:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)183:
			case (TypeCode)184:
			case (TypeCode)185:
			case (TypeCode)186:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)203:
			case (TypeCode)204:
			case (TypeCode)205:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)221:
			case (TypeCode)222:
			case (TypeCode)223:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)241:
			case (TypeCode)242:
			case (TypeCode)243:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)261:
			case (TypeCode)262:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)298:
			case (TypeCode)299:
			case (TypeCode)300:
				return Operators.XorInt64(convertible.ToInt64(null), convertible2.ToInt64(null), null);
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.XorInt64(convertible.ToInt64(null), Conversions.ToLong(convertible2.ToString(null)), null);
			case (TypeCode)114:
				return Operators.XorByte(convertible.ToByte(null), 0, Operators.GetEnumResult(Left, Right));
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.XorInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2), null);
			case (TypeCode)120:
				return Operators.XorByte(convertible.ToByte(null), convertible2.ToByte(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)122:
			case (TypeCode)158:
				return Operators.XorUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null), null);
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
				return Operators.XorUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null), null);
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
				return Operators.XorUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null), null);
			case (TypeCode)133:
				return Operators.XorInt16(convertible.ToInt16(null), 0, Operators.GetEnumResult(Left, Right));
			case (TypeCode)140:
				return Operators.XorInt16(convertible.ToInt16(null), convertible2.ToInt16(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)152:
				return Operators.XorUInt16(convertible.ToUInt16(null), 0, Operators.GetEnumResult(Left, Right));
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.XorInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2), null);
			case (TypeCode)160:
				return Operators.XorUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)171:
				return Operators.XorInt32(convertible.ToInt32(null), 0, Operators.GetEnumResult(Left, Right));
			case (TypeCode)180:
				return Operators.XorInt32(convertible.ToInt32(null), convertible2.ToInt32(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)190:
				return Operators.XorUInt32(convertible.ToUInt32(null), 0U, Operators.GetEnumResult(Left, Right));
			case (TypeCode)193:
			case (TypeCode)212:
			case (TypeCode)231:
			case (TypeCode)250:
			case (TypeCode)269:
			case (TypeCode)288:
				return Operators.XorInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2), null);
			case (TypeCode)200:
				return Operators.XorUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)209:
				return Operators.XorInt64(convertible.ToInt64(null), 0L, Operators.GetEnumResult(Left, Right));
			case (TypeCode)220:
				return Operators.XorInt64(convertible.ToInt64(null), convertible2.ToInt64(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)228:
				return Operators.XorUInt64(convertible.ToUInt64(null), 0UL, Operators.GetEnumResult(Left, Right));
			case (TypeCode)240:
				return Operators.XorUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null), Operators.GetEnumResult(Left, Right));
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return Operators.XorInt64(convertible.ToInt64(null), 0L, null);
			case (TypeCode)342:
				return Operators.XorInt64(Conversions.ToLong(convertible.ToString(null)), 0L, null);
			case (TypeCode)345:
				return Operators.XorBoolean(Conversions.ToBoolean(convertible.ToString(null)), convertible2.ToBoolean(null));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.XorInt64(Conversions.ToLong(convertible.ToString(null)), convertible2.ToInt64(null), null);
			case (TypeCode)360:
				return Operators.XorInt64(Conversions.ToLong(convertible.ToString(null)), Conversions.ToLong(convertible2.ToString(null)), null);
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Xor, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Xor, Left, Right);
		}

		private static object XorBoolean(bool Left, bool Right)
		{
			return Left ^ Right;
		}

		private static object XorSByte(sbyte Left, sbyte Right, Type EnumType = null)
		{
			sbyte b = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, b);
			}
			return b;
		}

		private static object XorByte(byte Left, byte Right, Type EnumType = null)
		{
			byte b = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, b);
			}
			return b;
		}

		private static object XorInt16(short Left, short Right, Type EnumType = null)
		{
			short num = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object XorUInt16(ushort Left, ushort Right, Type EnumType = null)
		{
			ushort num = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object XorInt32(int Left, int Right, Type EnumType = null)
		{
			int num = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object XorUInt32(uint Left, uint Right, Type EnumType = null)
		{
			uint num = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object XorInt64(long Left, long Right, Type EnumType = null)
		{
			long num = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		private static object XorUInt64(ulong Left, ulong Right, Type EnumType = null)
		{
			ulong num = Left ^ Right;
			if (EnumType != null)
			{
				return Enum.ToObject(EnumType, num);
			}
			return num;
		}

		public static object AddObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object)
			{
				char[] array = Left as char[];
				if (array != null && (typeCode2 == TypeCode.String || typeCode2 == TypeCode.Empty || (typeCode2 == TypeCode.Object && Right is char[])))
				{
					Left = new string(array);
					convertible = (IConvertible)Left;
					typeCode = TypeCode.String;
				}
			}
			if (typeCode2 == TypeCode.Object)
			{
				char[] array2 = Right as char[];
				if (array2 != null && (typeCode == TypeCode.String || typeCode == TypeCode.Empty))
				{
					Right = new string(array2);
					convertible2 = (IConvertible)Right;
					typeCode2 = TypeCode.String;
				}
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Boolean:
				return Operators.AddInt16(0, (short)Operators.ToVBBool(convertible2));
			case TypeCode.Char:
				return Operators.AddString("\0", convertible2.ToString(null));
			case TypeCode.SByte:
				return convertible2.ToSByte(null);
			case TypeCode.Byte:
				return convertible2.ToByte(null);
			case TypeCode.Int16:
				return convertible2.ToInt16(null);
			case TypeCode.UInt16:
				return convertible2.ToUInt16(null);
			case TypeCode.Int32:
				return convertible2.ToInt32(null);
			case TypeCode.UInt32:
				return convertible2.ToUInt32(null);
			case TypeCode.Int64:
				return convertible2.ToInt64(null);
			case TypeCode.UInt64:
				return convertible2.ToUInt64(null);
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.String:
			case (TypeCode)56:
				return Right;
			case TypeCode.DateTime:
				return Operators.AddString(Conversions.ToString(DateTime.MinValue), Conversions.ToString(convertible2.ToDateTime(null)));
			case (TypeCode)57:
				return Operators.AddInt16((short)Operators.ToVBBool(convertible), 0);
			case (TypeCode)60:
				return Operators.AddInt16((short)Operators.ToVBBool(convertible), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)62:
				return Operators.AddSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null));
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.AddInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null));
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.AddInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)67:
			case (TypeCode)68:
				return Operators.AddInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)69:
			case (TypeCode)72:
				return Operators.AddDecimal(Operators.ToVBBoolConv(convertible), (IConvertible)convertible2.ToDecimal(null));
			case (TypeCode)70:
				return Operators.AddSingle((float)Operators.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)71:
				return Operators.AddDouble((double)Operators.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)75:
				return Operators.AddDouble((double)Operators.ToVBBool(convertible), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)76:
				return Operators.AddString(convertible.ToString(null), "\0");
			case (TypeCode)80:
			case (TypeCode)94:
			case (TypeCode)346:
				return Operators.AddString(convertible.ToString(null), convertible2.ToString(null));
			case (TypeCode)95:
				return convertible.ToSByte(null);
			case (TypeCode)98:
				return Operators.AddSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2));
			case (TypeCode)100:
				return Operators.AddSByte(convertible.ToSByte(null), convertible2.ToSByte(null));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
				return Operators.AddInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
				return Operators.AddInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)125:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)163:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
				return Operators.AddInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)107:
			case (TypeCode)110:
			case (TypeCode)129:
			case (TypeCode)145:
			case (TypeCode)148:
			case (TypeCode)167:
			case (TypeCode)183:
			case (TypeCode)186:
			case (TypeCode)205:
			case (TypeCode)221:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)243:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)300:
				return Operators.AddDecimal(convertible, convertible2);
			case (TypeCode)108:
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)165:
			case (TypeCode)184:
			case (TypeCode)203:
			case (TypeCode)222:
			case (TypeCode)241:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return Operators.AddSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)109:
			case (TypeCode)128:
			case (TypeCode)147:
			case (TypeCode)166:
			case (TypeCode)185:
			case (TypeCode)204:
			case (TypeCode)223:
			case (TypeCode)242:
			case (TypeCode)261:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return Operators.AddDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.AddDouble(convertible.ToDouble(null), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)114:
				return convertible.ToByte(null);
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.AddInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)120:
				return Operators.AddByte(convertible.ToByte(null), convertible2.ToByte(null));
			case (TypeCode)122:
			case (TypeCode)158:
			case (TypeCode)160:
				return Operators.AddUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null));
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
			case (TypeCode)200:
				return Operators.AddUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null));
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
			case (TypeCode)240:
				return Operators.AddUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null));
			case (TypeCode)133:
				return convertible.ToInt16(null);
			case (TypeCode)152:
				return convertible.ToUInt16(null);
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.AddInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)171:
				return convertible.ToInt32(null);
			case (TypeCode)190:
				return convertible.ToUInt32(null);
			case (TypeCode)193:
			case (TypeCode)212:
				return Operators.AddInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)209:
				return convertible.ToInt64(null);
			case (TypeCode)228:
				return convertible.ToUInt64(null);
			case (TypeCode)231:
			case (TypeCode)288:
				return Operators.AddDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
			case (TypeCode)342:
			case (TypeCode)344:
				return Left;
			case (TypeCode)250:
				return Operators.AddSingle(convertible.ToSingle(null), (float)Operators.ToVBBool(convertible2));
			case (TypeCode)269:
				return Operators.AddDouble(convertible.ToDouble(null), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)304:
				return Operators.AddString(Conversions.ToString(convertible.ToDateTime(null)), Conversions.ToString(DateTime.MinValue));
			case (TypeCode)320:
				return Operators.AddString(Conversions.ToString(convertible.ToDateTime(null)), Conversions.ToString(convertible2.ToDateTime(null)));
			case (TypeCode)322:
				return Operators.AddString(Conversions.ToString(convertible.ToDateTime(null)), convertible2.ToString(null));
			case (TypeCode)345:
				return Operators.AddDouble(Conversions.ToDouble(convertible.ToString(null)), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.AddDouble(Conversions.ToDouble(convertible.ToString(null)), convertible2.ToDouble(null));
			case (TypeCode)358:
				return Operators.AddString(convertible.ToString(null), Conversions.ToString(convertible2.ToDateTime(null)));
			case (TypeCode)360:
				return Operators.AddString(convertible.ToString(null), convertible2.ToString(null));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Plus, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Plus, Left, Right);
		}

		private static object AddByte(byte Left, byte Right)
		{
			checked
			{
				short num = (short)(unchecked(Left + Right));
				if (num > 255)
				{
					return num;
				}
				return (byte)num;
			}
		}

		private static object AddSByte(sbyte Left, sbyte Right)
		{
			checked
			{
				short num = (short)(unchecked(Left + Right));
				if (num > 127 || num < -128)
				{
					return num;
				}
				return (sbyte)num;
			}
		}

		private static object AddInt16(short Left, short Right)
		{
			checked
			{
				int num = (int)(Left + Right);
				if (num > 32767 || num < -32768)
				{
					return num;
				}
				return (short)num;
			}
		}

		private static object AddUInt16(ushort Left, ushort Right)
		{
			checked
			{
				int num = (int)(Left + Right);
				if (num > 65535)
				{
					return num;
				}
				return (ushort)num;
			}
		}

		private static object AddInt32(int Left, int Right)
		{
			checked
			{
				long num = unchecked((long)Left) + unchecked((long)Right);
				if (num > 2147483647L || num < -2147483648L)
				{
					return num;
				}
				return (int)num;
			}
		}

		private static object AddUInt32(uint Left, uint Right)
		{
			checked
			{
				long num = (long)(unchecked((ulong)Left) + unchecked((ulong)Right));
				if (num > (long)(unchecked((ulong)(-1))))
				{
					return num;
				}
				return (uint)num;
			}
		}

		private static object AddInt64(long Left, long Right)
		{
			object obj;
			try
			{
				obj = checked(Left + Right);
			}
			catch (OverflowException ex)
			{
				obj = decimal.Add(new decimal(Left), new decimal(Right));
			}
			return obj;
		}

		private static object AddUInt64(ulong Left, ulong Right)
		{
			object obj;
			try
			{
				obj = checked(Left + Right);
			}
			catch (OverflowException ex)
			{
				obj = decimal.Add(new decimal(Left), new decimal(Right));
			}
			return obj;
		}

		private static object AddDecimal(IConvertible Left, IConvertible Right)
		{
			decimal num = Left.ToDecimal(null);
			decimal num2 = Right.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Add(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToDouble(num) + Convert.ToDouble(num2);
			}
			return obj;
		}

		private static object AddSingle(float Left, float Right)
		{
			double num = (double)Left + (double)Right;
			if (num <= 3.4028234663852886E+38 && num >= -3.4028234663852886E+38)
			{
				return (float)num;
			}
			if (double.IsInfinity(num) && (float.IsInfinity(Left) || float.IsInfinity(Right)))
			{
				return (float)num;
			}
			return num;
		}

		private static object AddDouble(double Left, double Right)
		{
			return Left + Right;
		}

		private static object AddString(string Left, string Right)
		{
			return Left + Right;
		}

		public static object SubtractObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Boolean:
				return Operators.SubtractInt16(0, (short)Operators.ToVBBool(convertible2));
			case TypeCode.SByte:
				return Operators.SubtractSByte(0, convertible2.ToSByte(null));
			case TypeCode.Byte:
				return Operators.SubtractByte(0, convertible2.ToByte(null));
			case TypeCode.Int16:
				return Operators.SubtractInt16(0, convertible2.ToInt16(null));
			case TypeCode.UInt16:
				return Operators.SubtractUInt16(0, convertible2.ToUInt16(null));
			case TypeCode.Int32:
				return Operators.SubtractInt32(0, convertible2.ToInt32(null));
			case TypeCode.UInt32:
				return Operators.SubtractUInt32(0U, convertible2.ToUInt32(null));
			case TypeCode.Int64:
				return Operators.SubtractInt64(0L, convertible2.ToInt64(null));
			case TypeCode.UInt64:
				return Operators.SubtractUInt64(0UL, convertible2.ToUInt64(null));
			case TypeCode.Single:
				return Operators.SubtractSingle(0f, convertible2.ToSingle(null));
			case TypeCode.Double:
				return Operators.SubtractDouble(0.0, convertible2.ToDouble(null));
			case TypeCode.Decimal:
				return Operators.SubtractDecimal((IConvertible)0m, convertible2);
			case TypeCode.String:
				return Operators.SubtractDouble(0.0, Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)57:
				return Operators.SubtractInt16((short)Operators.ToVBBool(convertible), 0);
			case (TypeCode)60:
				return Operators.SubtractInt16((short)Operators.ToVBBool(convertible), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)62:
				return Operators.SubtractSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null));
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.SubtractInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null));
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.SubtractInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)67:
			case (TypeCode)68:
				return Operators.SubtractInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)69:
			case (TypeCode)72:
				return Operators.SubtractDecimal(Operators.ToVBBoolConv(convertible), (IConvertible)convertible2.ToDecimal(null));
			case (TypeCode)70:
				return Operators.SubtractSingle((float)Operators.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)71:
				return Operators.SubtractDouble((double)Operators.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)75:
				return Operators.SubtractDouble((double)Operators.ToVBBool(convertible), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)95:
				return convertible.ToSByte(null);
			case (TypeCode)98:
				return Operators.SubtractSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2));
			case (TypeCode)100:
				return Operators.SubtractSByte(convertible.ToSByte(null), convertible2.ToSByte(null));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
				return Operators.SubtractInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
				return Operators.SubtractInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)125:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)163:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
				return Operators.SubtractInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)107:
			case (TypeCode)110:
			case (TypeCode)129:
			case (TypeCode)145:
			case (TypeCode)148:
			case (TypeCode)167:
			case (TypeCode)183:
			case (TypeCode)186:
			case (TypeCode)205:
			case (TypeCode)221:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)243:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)300:
				return Operators.SubtractDecimal(convertible, convertible2);
			case (TypeCode)108:
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)165:
			case (TypeCode)184:
			case (TypeCode)203:
			case (TypeCode)222:
			case (TypeCode)241:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return Operators.SubtractSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)109:
			case (TypeCode)128:
			case (TypeCode)147:
			case (TypeCode)166:
			case (TypeCode)185:
			case (TypeCode)204:
			case (TypeCode)223:
			case (TypeCode)242:
			case (TypeCode)261:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return Operators.SubtractDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.SubtractDouble(convertible.ToDouble(null), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)114:
				return convertible.ToByte(null);
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.SubtractInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)120:
				return Operators.SubtractByte(convertible.ToByte(null), convertible2.ToByte(null));
			case (TypeCode)122:
			case (TypeCode)158:
			case (TypeCode)160:
				return Operators.SubtractUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null));
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
			case (TypeCode)200:
				return Operators.SubtractUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null));
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
			case (TypeCode)240:
				return Operators.SubtractUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null));
			case (TypeCode)133:
				return convertible.ToInt16(null);
			case (TypeCode)152:
				return convertible.ToUInt16(null);
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.SubtractInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)171:
				return convertible.ToInt32(null);
			case (TypeCode)190:
				return convertible.ToUInt32(null);
			case (TypeCode)193:
			case (TypeCode)212:
				return Operators.SubtractInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)209:
				return convertible.ToInt64(null);
			case (TypeCode)228:
				return convertible.ToUInt64(null);
			case (TypeCode)231:
			case (TypeCode)288:
				return Operators.SubtractDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return Left;
			case (TypeCode)250:
				return Operators.SubtractSingle(convertible.ToSingle(null), (float)Operators.ToVBBool(convertible2));
			case (TypeCode)269:
				return Operators.SubtractDouble(convertible.ToDouble(null), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)342:
				return Conversions.ToDouble(convertible.ToString(null));
			case (TypeCode)345:
				return Operators.SubtractDouble(Conversions.ToDouble(convertible.ToString(null)), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.SubtractDouble(Conversions.ToDouble(convertible.ToString(null)), convertible2.ToDouble(null));
			case (TypeCode)360:
				return Operators.SubtractDouble(Conversions.ToDouble(convertible.ToString(null)), Conversions.ToDouble(convertible2.ToString(null)));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object || (typeCode == TypeCode.DateTime && typeCode2 == TypeCode.DateTime) || (typeCode == TypeCode.DateTime && typeCode2 == TypeCode.Empty) || (typeCode == TypeCode.Empty && typeCode2 == TypeCode.DateTime))
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Minus, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Minus, Left, Right);
		}

		private static object SubtractByte(byte Left, byte Right)
		{
			checked
			{
				short num = (short)(unchecked(Left - Right));
				if (num < 0)
				{
					return num;
				}
				return (byte)num;
			}
		}

		private static object SubtractSByte(sbyte Left, sbyte Right)
		{
			checked
			{
				short num = (short)(unchecked(Left - Right));
				if (num < -128 || num > 127)
				{
					return num;
				}
				return (sbyte)num;
			}
		}

		private static object SubtractInt16(short Left, short Right)
		{
			checked
			{
				int num = (int)(Left - Right);
				if (num < -32768 || num > 32767)
				{
					return num;
				}
				return (short)num;
			}
		}

		private static object SubtractUInt16(ushort Left, ushort Right)
		{
			checked
			{
				int num = (int)(Left - Right);
				if (num < 0)
				{
					return num;
				}
				return (ushort)num;
			}
		}

		private static object SubtractInt32(int Left, int Right)
		{
			checked
			{
				long num = unchecked((long)Left) - unchecked((long)Right);
				if (num < -2147483648L || num > 2147483647L)
				{
					return num;
				}
				return (int)num;
			}
		}

		private static object SubtractUInt32(uint Left, uint Right)
		{
			checked
			{
				long num = (long)(unchecked((ulong)Left) - unchecked((ulong)Right));
				if (num < 0L)
				{
					return num;
				}
				return (uint)num;
			}
		}

		private static object SubtractInt64(long Left, long Right)
		{
			object obj;
			try
			{
				obj = checked(Left - Right);
			}
			catch (OverflowException ex)
			{
				obj = decimal.Subtract(new decimal(Left), new decimal(Right));
			}
			return obj;
		}

		private static object SubtractUInt64(ulong Left, ulong Right)
		{
			object obj;
			try
			{
				obj = checked(Left - Right);
			}
			catch (OverflowException ex)
			{
				obj = decimal.Subtract(new decimal(Left), new decimal(Right));
			}
			return obj;
		}

		private static object SubtractDecimal(IConvertible Left, IConvertible Right)
		{
			decimal num = Left.ToDecimal(null);
			decimal num2 = Right.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Subtract(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToDouble(num) - Convert.ToDouble(num2);
			}
			return obj;
		}

		private static object SubtractSingle(float Left, float Right)
		{
			double num = (double)Left - (double)Right;
			if (num <= 3.4028234663852886E+38 && num >= -3.4028234663852886E+38)
			{
				return (float)num;
			}
			if (double.IsInfinity(num) && (float.IsInfinity(Left) || float.IsInfinity(Right)))
			{
				return (float)num;
			}
			return num;
		}

		private static object SubtractDouble(double Left, double Right)
		{
			return Left - Right;
		}

		public static object MultiplyObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
			case TypeCode.Int32:
			case (TypeCode)171:
				return Operators.Boxed_ZeroInteger;
			case TypeCode.Boolean:
			case TypeCode.Int16:
			case (TypeCode)57:
			case (TypeCode)133:
				return Operators.Boxed_ZeroShort;
			case TypeCode.SByte:
			case (TypeCode)95:
				return Operators.Boxed_ZeroSByte;
			case TypeCode.Byte:
			case (TypeCode)114:
				return Operators.Boxed_ZeroByte;
			case TypeCode.UInt16:
			case (TypeCode)152:
				return Operators.Boxed_ZeroUShort;
			case TypeCode.UInt32:
			case (TypeCode)190:
				return Operators.Boxed_ZeroUInteger;
			case TypeCode.Int64:
			case (TypeCode)209:
				return Operators.Boxed_ZeroLong;
			case TypeCode.UInt64:
			case (TypeCode)228:
				return Operators.Boxed_ZeroULong;
			case TypeCode.Single:
			case (TypeCode)247:
				return Operators.Boxed_ZeroSinge;
			case TypeCode.Double:
			case (TypeCode)266:
				return Operators.Boxed_ZeroDouble;
			case TypeCode.Decimal:
			case (TypeCode)285:
				return Operators.Boxed_ZeroDecimal;
			case TypeCode.String:
				return Operators.MultiplyDouble(0.0, Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)60:
				return Operators.MultiplyInt16((short)Operators.ToVBBool(convertible), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)62:
				return Operators.MultiplySByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null));
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.MultiplyInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null));
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.MultiplyInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)67:
			case (TypeCode)68:
				return Operators.MultiplyInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)69:
			case (TypeCode)72:
				return Operators.MultiplyDecimal(Operators.ToVBBoolConv(convertible), (IConvertible)convertible2.ToDecimal(null));
			case (TypeCode)70:
				return Operators.MultiplySingle((float)Operators.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)71:
				return Operators.MultiplyDouble((double)Operators.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)75:
				return Operators.MultiplyDouble((double)Operators.ToVBBool(convertible), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)98:
				return Operators.MultiplySByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2));
			case (TypeCode)100:
				return Operators.MultiplySByte(convertible.ToSByte(null), convertible2.ToSByte(null));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
				return Operators.MultiplyInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
				return Operators.MultiplyInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)125:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)163:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
				return Operators.MultiplyInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)107:
			case (TypeCode)110:
			case (TypeCode)129:
			case (TypeCode)145:
			case (TypeCode)148:
			case (TypeCode)167:
			case (TypeCode)183:
			case (TypeCode)186:
			case (TypeCode)205:
			case (TypeCode)221:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)243:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)300:
				return Operators.MultiplyDecimal(convertible, convertible2);
			case (TypeCode)108:
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)165:
			case (TypeCode)184:
			case (TypeCode)203:
			case (TypeCode)222:
			case (TypeCode)241:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return Operators.MultiplySingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)109:
			case (TypeCode)128:
			case (TypeCode)147:
			case (TypeCode)166:
			case (TypeCode)185:
			case (TypeCode)204:
			case (TypeCode)223:
			case (TypeCode)242:
			case (TypeCode)261:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return Operators.MultiplyDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.MultiplyDouble(convertible.ToDouble(null), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.MultiplyInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)120:
				return Operators.MultiplyByte(convertible.ToByte(null), convertible2.ToByte(null));
			case (TypeCode)122:
			case (TypeCode)158:
			case (TypeCode)160:
				return Operators.MultiplyUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null));
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
			case (TypeCode)200:
				return Operators.MultiplyUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null));
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
			case (TypeCode)240:
				return Operators.MultiplyUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null));
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.MultiplyInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)193:
			case (TypeCode)212:
				return Operators.MultiplyInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)231:
			case (TypeCode)288:
				return Operators.MultiplyDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)250:
				return Operators.MultiplySingle(convertible.ToSingle(null), (float)Operators.ToVBBool(convertible2));
			case (TypeCode)269:
				return Operators.MultiplyDouble(convertible.ToDouble(null), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)342:
				return Operators.MultiplyDouble(Conversions.ToDouble(convertible.ToString(null)), 0.0);
			case (TypeCode)345:
				return Operators.MultiplyDouble(Conversions.ToDouble(convertible.ToString(null)), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.MultiplyDouble(Conversions.ToDouble(convertible.ToString(null)), convertible2.ToDouble(null));
			case (TypeCode)360:
				return Operators.MultiplyDouble(Conversions.ToDouble(convertible.ToString(null)), Conversions.ToDouble(convertible2.ToString(null)));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Multiply, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Multiply, Left, Right);
		}

		private static object MultiplyByte(byte Left, byte Right)
		{
			checked
			{
				int num = (int)(Left * Right);
				if (num <= 255)
				{
					return (byte)num;
				}
				if (num > 32767)
				{
					return num;
				}
				return (short)num;
			}
		}

		private static object MultiplySByte(sbyte Left, sbyte Right)
		{
			checked
			{
				short num = (short)(unchecked(Left * Right));
				if (num > 127 || num < -128)
				{
					return num;
				}
				return (sbyte)num;
			}
		}

		private static object MultiplyInt16(short Left, short Right)
		{
			checked
			{
				int num = (int)(Left * Right);
				if (num > 32767 || num < -32768)
				{
					return num;
				}
				return (short)num;
			}
		}

		private static object MultiplyUInt16(ushort Left, ushort Right)
		{
			checked
			{
				long num = (long)(unchecked((ulong)Left) * unchecked((ulong)Right));
				if (num <= 65535L)
				{
					return (ushort)num;
				}
				if (num > 2147483647L)
				{
					return num;
				}
				return (int)num;
			}
		}

		private static object MultiplyInt32(int Left, int Right)
		{
			checked
			{
				long num = unchecked((long)Left) * unchecked((long)Right);
				if (num > 2147483647L || num < -2147483648L)
				{
					return num;
				}
				return (int)num;
			}
		}

		private static object MultiplyUInt32(uint Left, uint Right)
		{
			checked
			{
				ulong num = unchecked((ulong)Left) * unchecked((ulong)Right);
				if (num <= unchecked((ulong)(-1)))
				{
					return (uint)num;
				}
				if (decimal.Compare(new decimal(num), 9223372036854775807m) > 0)
				{
					return new decimal(num);
				}
				return (long)num;
			}
		}

		private static object MultiplyInt64(long Left, long Right)
		{
			try
			{
				return checked(Left * Right);
			}
			catch (OverflowException ex)
			{
			}
			object obj;
			try
			{
				obj = decimal.Multiply(new decimal(Left), new decimal(Right));
			}
			catch (OverflowException ex2)
			{
				obj = (double)Left * (double)Right;
			}
			return obj;
		}

		private static object MultiplyUInt64(ulong Left, ulong Right)
		{
			try
			{
				return checked(Left * Right);
			}
			catch (OverflowException ex)
			{
			}
			object obj;
			try
			{
				obj = decimal.Multiply(new decimal(Left), new decimal(Right));
			}
			catch (OverflowException ex2)
			{
				obj = Left * Right;
			}
			return obj;
		}

		private static object MultiplyDecimal(IConvertible Left, IConvertible Right)
		{
			decimal num = Left.ToDecimal(null);
			decimal num2 = Right.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Multiply(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToDouble(num) * Convert.ToDouble(num2);
			}
			return obj;
		}

		private static object MultiplySingle(float Left, float Right)
		{
			double num = (double)Left * (double)Right;
			if (num <= 3.4028234663852886E+38 && num >= -3.4028234663852886E+38)
			{
				return (float)num;
			}
			if (double.IsInfinity(num) && (float.IsInfinity(Left) || float.IsInfinity(Right)))
			{
				return (float)num;
			}
			return num;
		}

		private static object MultiplyDouble(double Left, double Right)
		{
			return Left * Right;
		}

		public static object DivideObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.DivideDouble(0.0, 0.0);
			case TypeCode.Boolean:
				return Operators.DivideDouble(0.0, (double)Operators.ToVBBool(convertible2));
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Double:
				return Operators.DivideDouble(0.0, convertible2.ToDouble(null));
			case TypeCode.Single:
				return Operators.DivideSingle(0f, convertible2.ToSingle(null));
			case TypeCode.Decimal:
				return Operators.DivideDecimal((IConvertible)0m, convertible2);
			case TypeCode.String:
				return Operators.DivideDouble(0.0, Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)57:
				return Operators.DivideDouble((double)Operators.ToVBBool(convertible), 0.0);
			case (TypeCode)60:
				return Operators.DivideDouble((double)Operators.ToVBBool(convertible), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)62:
			case (TypeCode)63:
			case (TypeCode)64:
			case (TypeCode)65:
			case (TypeCode)66:
			case (TypeCode)67:
			case (TypeCode)68:
			case (TypeCode)69:
			case (TypeCode)71:
				return Operators.DivideDouble((double)Operators.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)70:
				return Operators.DivideSingle((float)Operators.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)72:
				return Operators.DivideDecimal(Operators.ToVBBoolConv(convertible), convertible2);
			case (TypeCode)75:
				return Operators.DivideDouble((double)Operators.ToVBBool(convertible), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)95:
			case (TypeCode)114:
			case (TypeCode)133:
			case (TypeCode)152:
			case (TypeCode)171:
			case (TypeCode)190:
			case (TypeCode)209:
			case (TypeCode)228:
			case (TypeCode)266:
				return Operators.DivideDouble(convertible.ToDouble(null), 0.0);
			case (TypeCode)98:
			case (TypeCode)117:
			case (TypeCode)136:
			case (TypeCode)155:
			case (TypeCode)174:
			case (TypeCode)193:
			case (TypeCode)212:
			case (TypeCode)231:
			case (TypeCode)269:
				return Operators.DivideDouble(convertible.ToDouble(null), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)100:
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)107:
			case (TypeCode)109:
			case (TypeCode)119:
			case (TypeCode)120:
			case (TypeCode)121:
			case (TypeCode)122:
			case (TypeCode)123:
			case (TypeCode)124:
			case (TypeCode)125:
			case (TypeCode)126:
			case (TypeCode)128:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)145:
			case (TypeCode)147:
			case (TypeCode)157:
			case (TypeCode)158:
			case (TypeCode)159:
			case (TypeCode)160:
			case (TypeCode)161:
			case (TypeCode)162:
			case (TypeCode)163:
			case (TypeCode)164:
			case (TypeCode)166:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)183:
			case (TypeCode)185:
			case (TypeCode)195:
			case (TypeCode)196:
			case (TypeCode)197:
			case (TypeCode)198:
			case (TypeCode)199:
			case (TypeCode)200:
			case (TypeCode)201:
			case (TypeCode)202:
			case (TypeCode)204:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
			case (TypeCode)221:
			case (TypeCode)223:
			case (TypeCode)233:
			case (TypeCode)234:
			case (TypeCode)235:
			case (TypeCode)236:
			case (TypeCode)237:
			case (TypeCode)238:
			case (TypeCode)239:
			case (TypeCode)240:
			case (TypeCode)242:
			case (TypeCode)261:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return Operators.DivideDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)108:
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)165:
			case (TypeCode)184:
			case (TypeCode)203:
			case (TypeCode)222:
			case (TypeCode)241:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return Operators.DivideSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)110:
			case (TypeCode)129:
			case (TypeCode)148:
			case (TypeCode)167:
			case (TypeCode)186:
			case (TypeCode)205:
			case (TypeCode)224:
			case (TypeCode)243:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)300:
				return Operators.DivideDecimal(convertible, convertible2);
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.DivideDouble(convertible.ToDouble(null), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)247:
				return Operators.DivideSingle(convertible.ToSingle(null), 0f);
			case (TypeCode)250:
				return Operators.DivideSingle(convertible.ToSingle(null), (float)Operators.ToVBBool(convertible2));
			case (TypeCode)285:
				return Operators.DivideDecimal(convertible, (IConvertible)0m);
			case (TypeCode)288:
				return Operators.DivideDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)342:
				return Operators.DivideDouble(Conversions.ToDouble(convertible.ToString(null)), 0.0);
			case (TypeCode)345:
				return Operators.DivideDouble(Conversions.ToDouble(convertible.ToString(null)), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.DivideDouble(Conversions.ToDouble(convertible.ToString(null)), convertible2.ToDouble(null));
			case (TypeCode)360:
				return Operators.DivideDouble(Conversions.ToDouble(convertible.ToString(null)), Conversions.ToDouble(convertible2.ToString(null)));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Divide, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Divide, Left, Right);
		}

		private static object DivideDecimal(IConvertible Left, IConvertible Right)
		{
			decimal num = Left.ToDecimal(null);
			decimal num2 = Right.ToDecimal(null);
			object obj;
			try
			{
				obj = decimal.Divide(num, num2);
			}
			catch (OverflowException ex)
			{
				obj = Convert.ToSingle(num) / Convert.ToSingle(num2);
			}
			return obj;
		}

		private static object DivideSingle(float Left, float Right)
		{
			float num = Left / Right;
			if (!float.IsInfinity(num))
			{
				return num;
			}
			if (float.IsInfinity(Left) || float.IsInfinity(Right))
			{
				return num;
			}
			return (double)Left / (double)Right;
		}

		private static object DivideDouble(double Left, double Right)
		{
			return Left / Right;
		}

		public static object ExponentObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			double num;
			switch (typeCode)
			{
			case TypeCode.Empty:
				num = 0.0;
				goto IL_00CC;
			case TypeCode.Boolean:
				num = (double)Operators.ToVBBool(convertible);
				goto IL_00CC;
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
				num = convertible.ToDouble(null);
				goto IL_00CC;
			case TypeCode.String:
				num = Conversions.ToDouble(convertible.ToString(null));
				goto IL_00CC;
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Power, Left, Right);
			IL_00CC:
			double num2;
			switch (typeCode2)
			{
			case TypeCode.Empty:
				num2 = 0.0;
				goto IL_015E;
			case TypeCode.Boolean:
				num2 = (double)Operators.ToVBBool(convertible2);
				goto IL_015E;
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
				num2 = convertible2.ToDouble(null);
				goto IL_015E;
			case TypeCode.String:
				num2 = Conversions.ToDouble(convertible2.ToString(null));
				goto IL_015E;
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Power, Left, Right);
			IL_015E:
			return Math.Pow(num, num2);
		}

		public static object ModObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.ModInt32(0, 0);
			case TypeCode.Boolean:
				return Operators.ModInt16(0, (short)Operators.ToVBBool(convertible2));
			case TypeCode.SByte:
				return Operators.ModSByte(0, convertible2.ToSByte(null));
			case TypeCode.Byte:
				return Operators.ModByte(0, convertible2.ToByte(null));
			case TypeCode.Int16:
				return Operators.ModInt16(0, convertible2.ToInt16(null));
			case TypeCode.UInt16:
				return Operators.ModUInt16(0, convertible2.ToUInt16(null));
			case TypeCode.Int32:
				return Operators.ModInt32(0, convertible2.ToInt32(null));
			case TypeCode.UInt32:
				return Operators.ModUInt32(0U, convertible2.ToUInt32(null));
			case TypeCode.Int64:
				return Operators.ModInt64(0L, convertible2.ToInt64(null));
			case TypeCode.UInt64:
				return Operators.ModUInt64(0UL, convertible2.ToUInt64(null));
			case TypeCode.Single:
				return Operators.ModSingle(0f, convertible2.ToSingle(null));
			case TypeCode.Double:
				return Operators.ModDouble(0.0, convertible2.ToDouble(null));
			case TypeCode.Decimal:
				return Operators.ModDecimal((IConvertible)0m, (IConvertible)convertible2.ToDecimal(null));
			case TypeCode.String:
				return Operators.ModDouble(0.0, Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)57:
				return Operators.ModInt16((short)Operators.ToVBBool(convertible), 0);
			case (TypeCode)60:
				return Operators.ModInt16((short)Operators.ToVBBool(convertible), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)62:
				return Operators.ModSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null));
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.ModInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null));
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.ModInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)67:
			case (TypeCode)68:
				return Operators.ModInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)69:
			case (TypeCode)72:
				return Operators.ModDecimal(Operators.ToVBBoolConv(convertible), (IConvertible)convertible2.ToDecimal(null));
			case (TypeCode)70:
				return Operators.ModSingle((float)Operators.ToVBBool(convertible), convertible2.ToSingle(null));
			case (TypeCode)71:
				return Operators.ModDouble((double)Operators.ToVBBool(convertible), convertible2.ToDouble(null));
			case (TypeCode)75:
				return Operators.ModDouble((double)Operators.ToVBBool(convertible), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)95:
				return Operators.ModSByte(convertible.ToSByte(null), 0);
			case (TypeCode)98:
				return Operators.ModSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2));
			case (TypeCode)100:
				return Operators.ModSByte(convertible.ToSByte(null), convertible2.ToSByte(null));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
				return Operators.ModInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
				return Operators.ModInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)125:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)163:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
				return Operators.ModInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)107:
			case (TypeCode)110:
			case (TypeCode)129:
			case (TypeCode)145:
			case (TypeCode)148:
			case (TypeCode)167:
			case (TypeCode)183:
			case (TypeCode)186:
			case (TypeCode)205:
			case (TypeCode)221:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)243:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)300:
				return Operators.ModDecimal(convertible, convertible2);
			case (TypeCode)108:
			case (TypeCode)127:
			case (TypeCode)146:
			case (TypeCode)165:
			case (TypeCode)184:
			case (TypeCode)203:
			case (TypeCode)222:
			case (TypeCode)241:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)262:
			case (TypeCode)298:
				return Operators.ModSingle(convertible.ToSingle(null), convertible2.ToSingle(null));
			case (TypeCode)109:
			case (TypeCode)128:
			case (TypeCode)147:
			case (TypeCode)166:
			case (TypeCode)185:
			case (TypeCode)204:
			case (TypeCode)223:
			case (TypeCode)242:
			case (TypeCode)261:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)299:
				return Operators.ModDouble(convertible.ToDouble(null), convertible2.ToDouble(null));
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.ModDouble(convertible.ToDouble(null), Conversions.ToDouble(convertible2.ToString(null)));
			case (TypeCode)114:
				return Operators.ModByte(convertible.ToByte(null), 0);
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.ModInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)120:
				return Operators.ModByte(convertible.ToByte(null), convertible2.ToByte(null));
			case (TypeCode)122:
			case (TypeCode)158:
			case (TypeCode)160:
				return Operators.ModUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null));
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
			case (TypeCode)200:
				return Operators.ModUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null));
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
			case (TypeCode)240:
				return Operators.ModUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null));
			case (TypeCode)133:
				return Operators.ModInt16(convertible.ToInt16(null), 0);
			case (TypeCode)152:
				return Operators.ModUInt16(convertible.ToUInt16(null), 0);
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.ModInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)171:
				return Operators.ModInt32(convertible.ToInt32(null), 0);
			case (TypeCode)190:
				return Operators.ModUInt32(convertible.ToUInt32(null), 0U);
			case (TypeCode)193:
			case (TypeCode)212:
				return Operators.ModInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)209:
				return Operators.ModInt64(convertible.ToInt64(null), 0L);
			case (TypeCode)228:
				return Operators.ModUInt64(convertible.ToUInt64(null), 0UL);
			case (TypeCode)231:
			case (TypeCode)288:
				return Operators.ModDecimal(convertible, Operators.ToVBBoolConv(convertible2));
			case (TypeCode)247:
				return Operators.ModSingle(convertible.ToSingle(null), 0f);
			case (TypeCode)250:
				return Operators.ModSingle(convertible.ToSingle(null), (float)Operators.ToVBBool(convertible2));
			case (TypeCode)266:
				return Operators.ModDouble(convertible.ToDouble(null), 0.0);
			case (TypeCode)269:
				return Operators.ModDouble(convertible.ToDouble(null), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)285:
				return Operators.ModDecimal(convertible, (IConvertible)0m);
			case (TypeCode)342:
				return Operators.ModDouble(Conversions.ToDouble(convertible.ToString(null)), 0.0);
			case (TypeCode)345:
				return Operators.ModDouble(Conversions.ToDouble(convertible.ToString(null)), (double)Operators.ToVBBool(convertible2));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.ModDouble(Conversions.ToDouble(convertible.ToString(null)), convertible2.ToDouble(null));
			case (TypeCode)360:
				return Operators.ModDouble(Conversions.ToDouble(convertible.ToString(null)), Conversions.ToDouble(convertible2.ToString(null)));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Modulus, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.Modulus, Left, Right);
		}

		private static object ModSByte(sbyte Left, sbyte Right)
		{
			return Left % Right;
		}

		private static object ModByte(byte Left, byte Right)
		{
			return Left % Right;
		}

		private static object ModInt16(short Left, short Right)
		{
			int num = (int)(Left % Right);
			if (num < -32768 || num > 32767)
			{
				return num;
			}
			return checked((short)num);
		}

		private static object ModUInt16(ushort Left, ushort Right)
		{
			return Left % Right;
		}

		private static object ModInt32(int Left, int Right)
		{
			long num = (long)Left % (long)Right;
			if (num < -2147483648L || num > 2147483647L)
			{
				return num;
			}
			return checked((int)num);
		}

		private static object ModUInt32(uint Left, uint Right)
		{
			return Left % Right;
		}

		private static object ModInt64(long Left, long Right)
		{
			if (Left == -9223372036854775808L && Right == -1L)
			{
				return 0L;
			}
			return Left % Right;
		}

		private static object ModUInt64(ulong Left, ulong Right)
		{
			return Left % Right;
		}

		private static object ModDecimal(IConvertible Left, IConvertible Right)
		{
			decimal num = Left.ToDecimal(null);
			decimal num2 = Right.ToDecimal(null);
			return decimal.Remainder(num, num2);
		}

		private static object ModSingle(float Left, float Right)
		{
			return Left % Right;
		}

		private static object ModDouble(double Left, double Right)
		{
			return Left % Right;
		}

		public static object IntDivideObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			switch (checked(typeCode * (TypeCode)19 + (int)typeCode2))
			{
			case TypeCode.Empty:
				return Operators.IntDivideInt32(0, 0);
			case TypeCode.Boolean:
				return Operators.IntDivideInt16(0, (short)Operators.ToVBBool(convertible2));
			case TypeCode.SByte:
				return Operators.IntDivideSByte(0, convertible2.ToSByte(null));
			case TypeCode.Byte:
				return Operators.IntDivideByte(0, convertible2.ToByte(null));
			case TypeCode.Int16:
				return Operators.IntDivideInt16(0, convertible2.ToInt16(null));
			case TypeCode.UInt16:
				return Operators.IntDivideUInt16(0, convertible2.ToUInt16(null));
			case TypeCode.Int32:
				return Operators.IntDivideInt32(0, convertible2.ToInt32(null));
			case TypeCode.UInt32:
				return Operators.IntDivideUInt32(0U, convertible2.ToUInt32(null));
			case TypeCode.Int64:
				return Operators.IntDivideInt64(0L, convertible2.ToInt64(null));
			case TypeCode.UInt64:
				return Operators.IntDivideUInt64(0UL, convertible2.ToUInt64(null));
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return Operators.IntDivideInt64(0L, convertible2.ToInt64(null));
			case TypeCode.String:
				return Operators.IntDivideInt64(0L, Conversions.ToLong(convertible2.ToString(null)));
			case (TypeCode)57:
				return Operators.IntDivideInt16((short)Operators.ToVBBool(convertible), 0);
			case (TypeCode)60:
				return Operators.IntDivideInt16((short)Operators.ToVBBool(convertible), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)62:
				return Operators.IntDivideSByte(Operators.ToVBBool(convertible), convertible2.ToSByte(null));
			case (TypeCode)63:
			case (TypeCode)64:
				return Operators.IntDivideInt16((short)Operators.ToVBBool(convertible), convertible2.ToInt16(null));
			case (TypeCode)65:
			case (TypeCode)66:
				return Operators.IntDivideInt32((int)Operators.ToVBBool(convertible), convertible2.ToInt32(null));
			case (TypeCode)67:
			case (TypeCode)68:
			case (TypeCode)69:
			case (TypeCode)70:
			case (TypeCode)71:
			case (TypeCode)72:
				return Operators.IntDivideInt64((long)Operators.ToVBBool(convertible), convertible2.ToInt64(null));
			case (TypeCode)75:
				return Operators.IntDivideInt64((long)Operators.ToVBBool(convertible), Conversions.ToLong(convertible2.ToString(null)));
			case (TypeCode)95:
				return Operators.IntDivideSByte(convertible.ToSByte(null), 0);
			case (TypeCode)98:
				return Operators.IntDivideSByte(convertible.ToSByte(null), Operators.ToVBBool(convertible2));
			case (TypeCode)100:
				return Operators.IntDivideSByte(convertible.ToSByte(null), convertible2.ToSByte(null));
			case (TypeCode)101:
			case (TypeCode)102:
			case (TypeCode)119:
			case (TypeCode)121:
			case (TypeCode)138:
			case (TypeCode)139:
			case (TypeCode)140:
				return Operators.IntDivideInt16(convertible.ToInt16(null), convertible2.ToInt16(null));
			case (TypeCode)103:
			case (TypeCode)104:
			case (TypeCode)123:
			case (TypeCode)141:
			case (TypeCode)142:
			case (TypeCode)157:
			case (TypeCode)159:
			case (TypeCode)161:
			case (TypeCode)176:
			case (TypeCode)177:
			case (TypeCode)178:
			case (TypeCode)179:
			case (TypeCode)180:
				return Operators.IntDivideInt32(convertible.ToInt32(null), convertible2.ToInt32(null));
			case (TypeCode)105:
			case (TypeCode)106:
			case (TypeCode)107:
			case (TypeCode)108:
			case (TypeCode)109:
			case (TypeCode)110:
			case (TypeCode)125:
			case (TypeCode)127:
			case (TypeCode)128:
			case (TypeCode)129:
			case (TypeCode)143:
			case (TypeCode)144:
			case (TypeCode)145:
			case (TypeCode)146:
			case (TypeCode)147:
			case (TypeCode)148:
			case (TypeCode)163:
			case (TypeCode)165:
			case (TypeCode)166:
			case (TypeCode)167:
			case (TypeCode)181:
			case (TypeCode)182:
			case (TypeCode)183:
			case (TypeCode)184:
			case (TypeCode)185:
			case (TypeCode)186:
			case (TypeCode)195:
			case (TypeCode)197:
			case (TypeCode)199:
			case (TypeCode)201:
			case (TypeCode)203:
			case (TypeCode)204:
			case (TypeCode)205:
			case (TypeCode)214:
			case (TypeCode)215:
			case (TypeCode)216:
			case (TypeCode)217:
			case (TypeCode)218:
			case (TypeCode)219:
			case (TypeCode)220:
			case (TypeCode)221:
			case (TypeCode)222:
			case (TypeCode)223:
			case (TypeCode)224:
			case (TypeCode)233:
			case (TypeCode)235:
			case (TypeCode)237:
			case (TypeCode)239:
			case (TypeCode)241:
			case (TypeCode)242:
			case (TypeCode)243:
			case (TypeCode)252:
			case (TypeCode)253:
			case (TypeCode)254:
			case (TypeCode)255:
			case (TypeCode)256:
			case (TypeCode)257:
			case (TypeCode)258:
			case (TypeCode)259:
			case (TypeCode)260:
			case (TypeCode)261:
			case (TypeCode)262:
			case (TypeCode)271:
			case (TypeCode)272:
			case (TypeCode)273:
			case (TypeCode)274:
			case (TypeCode)275:
			case (TypeCode)276:
			case (TypeCode)277:
			case (TypeCode)278:
			case (TypeCode)279:
			case (TypeCode)280:
			case (TypeCode)281:
			case (TypeCode)290:
			case (TypeCode)291:
			case (TypeCode)292:
			case (TypeCode)293:
			case (TypeCode)294:
			case (TypeCode)295:
			case (TypeCode)296:
			case (TypeCode)297:
			case (TypeCode)298:
			case (TypeCode)299:
			case (TypeCode)300:
				return Operators.IntDivideInt64(convertible.ToInt64(null), convertible2.ToInt64(null));
			case (TypeCode)113:
			case (TypeCode)132:
			case (TypeCode)151:
			case (TypeCode)170:
			case (TypeCode)189:
			case (TypeCode)208:
			case (TypeCode)227:
			case (TypeCode)246:
			case (TypeCode)265:
			case (TypeCode)284:
			case (TypeCode)303:
				return Operators.IntDivideInt64(convertible.ToInt64(null), Conversions.ToLong(convertible2.ToString(null)));
			case (TypeCode)114:
				return Operators.IntDivideByte(convertible.ToByte(null), 0);
			case (TypeCode)117:
			case (TypeCode)136:
				return Operators.IntDivideInt16(convertible.ToInt16(null), (short)Operators.ToVBBool(convertible2));
			case (TypeCode)120:
				return Operators.IntDivideByte(convertible.ToByte(null), convertible2.ToByte(null));
			case (TypeCode)122:
			case (TypeCode)158:
			case (TypeCode)160:
				return Operators.IntDivideUInt16(convertible.ToUInt16(null), convertible2.ToUInt16(null));
			case (TypeCode)124:
			case (TypeCode)162:
			case (TypeCode)196:
			case (TypeCode)198:
			case (TypeCode)200:
				return Operators.IntDivideUInt32(convertible.ToUInt32(null), convertible2.ToUInt32(null));
			case (TypeCode)126:
			case (TypeCode)164:
			case (TypeCode)202:
			case (TypeCode)234:
			case (TypeCode)236:
			case (TypeCode)238:
			case (TypeCode)240:
				return Operators.IntDivideUInt64(convertible.ToUInt64(null), convertible2.ToUInt64(null));
			case (TypeCode)133:
				return Operators.IntDivideInt16(convertible.ToInt16(null), 0);
			case (TypeCode)152:
				return Operators.IntDivideUInt16(convertible.ToUInt16(null), 0);
			case (TypeCode)155:
			case (TypeCode)174:
				return Operators.IntDivideInt32(convertible.ToInt32(null), (int)Operators.ToVBBool(convertible2));
			case (TypeCode)171:
				return Operators.IntDivideInt32(convertible.ToInt32(null), 0);
			case (TypeCode)190:
				return Operators.IntDivideUInt32(convertible.ToUInt32(null), 0U);
			case (TypeCode)193:
			case (TypeCode)212:
			case (TypeCode)231:
			case (TypeCode)250:
			case (TypeCode)269:
			case (TypeCode)288:
				return Operators.IntDivideInt64(convertible.ToInt64(null), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)209:
				return Operators.IntDivideInt64(convertible.ToInt64(null), 0L);
			case (TypeCode)228:
				return Operators.IntDivideUInt64(convertible.ToUInt64(null), 0UL);
			case (TypeCode)247:
			case (TypeCode)266:
			case (TypeCode)285:
				return Operators.IntDivideInt64(convertible.ToInt64(null), 0L);
			case (TypeCode)342:
				return Operators.IntDivideInt64(Conversions.ToLong(convertible.ToString(null)), 0L);
			case (TypeCode)345:
				return Operators.IntDivideInt64(Conversions.ToLong(convertible.ToString(null)), (long)Operators.ToVBBool(convertible2));
			case (TypeCode)347:
			case (TypeCode)348:
			case (TypeCode)349:
			case (TypeCode)350:
			case (TypeCode)351:
			case (TypeCode)352:
			case (TypeCode)353:
			case (TypeCode)354:
			case (TypeCode)355:
			case (TypeCode)356:
			case (TypeCode)357:
				return Operators.IntDivideInt64(Conversions.ToLong(convertible.ToString(null)), convertible2.ToInt64(null));
			case (TypeCode)360:
				return Operators.IntDivideInt64(Conversions.ToLong(convertible.ToString(null)), Conversions.ToLong(convertible2.ToString(null)));
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.IntegralDivide, new object[] { Left, Right });
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.IntegralDivide, Left, Right);
		}

		private static object IntDivideSByte(sbyte Left, sbyte Right)
		{
			if (Left == -128 && Right == -1)
			{
				return 128;
			}
			return Left / Right;
		}

		private static object IntDivideByte(byte Left, byte Right)
		{
			return Left / Right;
		}

		private static object IntDivideInt16(short Left, short Right)
		{
			if (Left == -32768 && Right == -1)
			{
				return 32768;
			}
			return Left / Right;
		}

		private static object IntDivideUInt16(ushort Left, ushort Right)
		{
			return Left / Right;
		}

		private static object IntDivideInt32(int Left, int Right)
		{
			if (Left == -2147483648 && Right == -1)
			{
				return (long)((ulong)int.MinValue);
			}
			return Left / Right;
		}

		private static object IntDivideUInt32(uint Left, uint Right)
		{
			return Left / Right;
		}

		private static object IntDivideInt64(long Left, long Right)
		{
			return Left / Right;
		}

		private static object IntDivideUInt64(ulong Left, ulong Right)
		{
			return Left / Right;
		}

		public static object LeftShiftObject(object Operand, object Amount)
		{
			IConvertible convertible = Operand as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Operand == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Amount as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Amount == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.ShiftLeft, new object[] { Operand, Amount });
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return 0 << Conversions.ToInteger(Amount);
			case TypeCode.Boolean:
				return (short)(((-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0) << (Conversions.ToInteger(Amount) & 15));
			case TypeCode.SByte:
				return (sbyte)(convertible.ToSByte(null) << (Conversions.ToInteger(Amount) & 7));
			case TypeCode.Byte:
				return (byte)(convertible.ToByte(null) << (Conversions.ToInteger(Amount) & 7));
			case TypeCode.Int16:
				return (short)(convertible.ToInt16(null) << (Conversions.ToInteger(Amount) & 15));
			case TypeCode.UInt16:
				return (ushort)(convertible.ToUInt16(null) << (Conversions.ToInteger(Amount) & 15));
			case TypeCode.Int32:
				return convertible.ToInt32(null) << Conversions.ToInteger(Amount);
			case TypeCode.UInt32:
				return convertible.ToUInt32(null) << Conversions.ToInteger(Amount);
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return convertible.ToInt64(null) << Conversions.ToInteger(Amount);
			case TypeCode.UInt64:
				return convertible.ToUInt64(null) << Conversions.ToInteger(Amount);
			case TypeCode.String:
				return Conversions.ToLong(convertible.ToString(null)) << Conversions.ToInteger(Amount);
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.ShiftLeft, Operand);
		}

		public static object RightShiftObject(object Operand, object Amount)
		{
			IConvertible convertible = Operand as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Operand == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Amount as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Amount == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.ShiftRight, new object[] { Operand, Amount });
			}
			switch (typeCode)
			{
			case TypeCode.Empty:
				return 0 >> Conversions.ToInteger(Amount);
			case TypeCode.Boolean:
				return (short)(((-((convertible.ToBoolean(null) > false) ? 1 : 0)) ? 1 : 0) >> (Conversions.ToInteger(Amount) & 15));
			case TypeCode.SByte:
				return (sbyte)(convertible.ToSByte(null) >> (Conversions.ToInteger(Amount) & 7));
			case TypeCode.Byte:
				return (byte)((uint)convertible.ToByte(null) >> (Conversions.ToInteger(Amount) & 7));
			case TypeCode.Int16:
				return (short)(convertible.ToInt16(null) >> (Conversions.ToInteger(Amount) & 15));
			case TypeCode.UInt16:
				return (ushort)((uint)convertible.ToUInt16(null) >> (Conversions.ToInteger(Amount) & 15));
			case TypeCode.Int32:
				return convertible.ToInt32(null) >> Conversions.ToInteger(Amount);
			case TypeCode.UInt32:
				return convertible.ToUInt32(null) >> Conversions.ToInteger(Amount);
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return convertible.ToInt64(null) >> Conversions.ToInteger(Amount);
			case TypeCode.UInt64:
				return convertible.ToUInt64(null) >> Conversions.ToInteger(Amount);
			case TypeCode.String:
				return Conversions.ToLong(convertible.ToString(null)) >> Conversions.ToInteger(Amount);
			}
			throw Operators.GetNoValidOperatorException(Symbols.UserDefinedOperator.ShiftRight, Operand);
		}

		public static object LikeObject(object Source, object Pattern, CompareMethod CompareOption)
		{
			IConvertible convertible = Source as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Source == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Pattern as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Pattern == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object && Source is char[])
			{
				typeCode = TypeCode.String;
			}
			if (typeCode2 == TypeCode.Object && Pattern is char[])
			{
				typeCode2 = TypeCode.String;
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Like, new object[] { Source, Pattern });
			}
			return Operators.LikeString(Conversions.ToString(Source), Conversions.ToString(Pattern), CompareOption);
		}

		public static bool LikeString(string Source, string Pattern, CompareMethod CompareOption)
		{
			if (CompareOption == CompareMethod.Binary)
			{
				return Operators.LikeStringBinary(Source, Pattern);
			}
			return Operators.LikeStringText(Source, Pattern);
		}

		private static bool LikeStringBinary(string Source, string Pattern)
		{
			bool flag = false;
			int num;
			if (Pattern == null)
			{
				num = 0;
			}
			else
			{
				num = Pattern.Length;
			}
			int num2;
			if (Source == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = Source.Length;
			}
			int num3;
			char c;
			if (num3 < num2)
			{
				c = Source[num3];
			}
			checked
			{
				int i;
				bool flag2;
				while (i < num)
				{
					char c2 = Pattern[i];
					bool flag3;
					bool flag4;
					bool flag5;
					if (c2 == '*' && !flag2)
					{
						int num4 = Operators.AsteriskSkip(Pattern.Substring(i + 1), Source.Substring(num3), num2 - num3, CompareMethod.Binary, Strings.m_InvariantCompareInfo);
						if (num4 < 0)
						{
							return false;
						}
						if (num4 > 0)
						{
							num3 += num4;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
					}
					else if (c2 == '?' && !flag2)
					{
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '#' && !flag2)
					{
						if (!char.IsDigit(c))
						{
							break;
						}
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '-' && flag2 && flag3 && !flag && !flag4 && (i + 1 >= num || Pattern[i + 1] != ']'))
					{
						flag4 = true;
					}
					else if (c2 == '!' && flag2 && !flag5)
					{
						flag5 = true;
						bool flag6 = true;
					}
					else if (c2 == '[' && !flag2)
					{
						flag2 = true;
						char c3 = '\0';
						flag3 = false;
					}
					else if (c2 == ']' && flag2)
					{
						flag2 = false;
						bool flag6;
						if (flag3)
						{
							if (!flag6)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						else if (flag4)
						{
							if (!flag6)
							{
								break;
							}
						}
						else if (flag5)
						{
							if ('!' != c)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						flag6 = false;
						flag3 = false;
						flag5 = false;
						flag4 = false;
					}
					else
					{
						flag3 = true;
						flag = false;
						if (flag2)
						{
							if (flag4)
							{
								flag4 = false;
								flag = true;
								char c4 = c2;
								char c3;
								if (c3 > c4)
								{
									throw ExceptionUtils.VbMakeException(93);
								}
								bool flag6;
								if (!flag5 || !flag6)
								{
									if (flag5 || flag6)
									{
										goto IL_0260;
									}
								}
								flag6 = c > c3 && c <= c4;
								if (flag5)
								{
									flag6 = !flag6;
								}
							}
							else
							{
								char c3 = c2;
								bool flag6 = Operators.LikeStringCompareBinary(flag5, flag6, c2, c);
							}
						}
						else
						{
							if (c2 != c && !flag5)
							{
								break;
							}
							flag5 = false;
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
							else if (num3 > num2)
							{
								return false;
							}
						}
					}
					IL_0260:
					i++;
				}
				if (!flag2)
				{
					return i == num && num3 == num2;
				}
				if (num2 == 0)
				{
					return false;
				}
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
			}
		}

		private static bool LikeStringText(string Source, string Pattern)
		{
			bool flag = false;
			int num;
			if (Pattern == null)
			{
				num = 0;
			}
			else
			{
				num = Pattern.Length;
			}
			int num2;
			if (Source == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = Source.Length;
			}
			int num3;
			char c;
			if (num3 < num2)
			{
				c = Source[num3];
			}
			CompareInfo compareInfo = Utils.GetCultureInfo().CompareInfo;
			CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
			checked
			{
				int i;
				bool flag2;
				while (i < num)
				{
					char c2 = Pattern[i];
					bool flag3;
					bool flag4;
					bool flag5;
					if (c2 == '*' && !flag2)
					{
						int num4 = Operators.AsteriskSkip(Pattern.Substring(i + 1), Source.Substring(num3), num2 - num3, CompareMethod.Text, compareInfo);
						if (num4 < 0)
						{
							return false;
						}
						if (num4 > 0)
						{
							num3 += num4;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
					}
					else if (c2 == '?' && !flag2)
					{
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '#' && !flag2)
					{
						if (!char.IsDigit(c))
						{
							break;
						}
						num3++;
						if (num3 < num2)
						{
							c = Source[num3];
						}
					}
					else if (c2 == '-' && flag2 && flag3 && !flag && !flag4 && (i + 1 >= num || Pattern[i + 1] != ']'))
					{
						flag4 = true;
					}
					else if (c2 == '!' && flag2 && !flag5)
					{
						flag5 = true;
						bool flag6 = true;
					}
					else if (c2 == '[' && !flag2)
					{
						flag2 = true;
						char c3 = '\0';
						flag3 = false;
					}
					else if (c2 == ']' && flag2)
					{
						flag2 = false;
						bool flag6;
						if (flag3)
						{
							if (!flag6)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						else if (flag4)
						{
							if (!flag6)
							{
								break;
							}
						}
						else if (flag5)
						{
							if (compareInfo.Compare("!", Conversions.ToString(c)) != 0)
							{
								break;
							}
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
						}
						flag6 = false;
						flag3 = false;
						flag5 = false;
						flag4 = false;
					}
					else
					{
						flag3 = true;
						flag = false;
						if (flag2)
						{
							if (flag4)
							{
								flag4 = false;
								flag = true;
								char c4 = c2;
								char c3;
								if (c3 > c4)
								{
									throw ExceptionUtils.VbMakeException(93);
								}
								bool flag6;
								if (!flag5 || !flag6)
								{
									if (flag5 || flag6)
									{
										goto IL_03A3;
									}
								}
								if (compareOptions == CompareOptions.Ordinal)
								{
									flag6 = c > c3 && c <= c4;
								}
								else
								{
									flag6 = compareInfo.Compare(Conversions.ToString(c3), Conversions.ToString(c), compareOptions) < 0 && compareInfo.Compare(Conversions.ToString(c4), Conversions.ToString(c), compareOptions) >= 0;
								}
								if (flag5)
								{
									flag6 = !flag6;
								}
							}
							else
							{
								char c3 = c2;
								bool flag6 = Operators.LikeStringCompare(compareInfo, flag5, flag6, c2, c, compareOptions);
							}
						}
						else
						{
							if (compareOptions == CompareOptions.Ordinal)
							{
								if (c2 != c && !flag5)
								{
									break;
								}
							}
							else
							{
								string text = Conversions.ToString(c2);
								string text2 = Conversions.ToString(c);
								while (i + 1 < num)
								{
									if (UnicodeCategory.ModifierSymbol != char.GetUnicodeCategory(Pattern[i + 1]) && UnicodeCategory.NonSpacingMark != char.GetUnicodeCategory(Pattern[i + 1]))
									{
										break;
									}
									text += Conversions.ToString(Pattern[i + 1]);
									i++;
								}
								while (num3 + 1 < num2 && (UnicodeCategory.ModifierSymbol == char.GetUnicodeCategory(Source[num3 + 1]) || UnicodeCategory.NonSpacingMark == char.GetUnicodeCategory(Source[num3 + 1])))
								{
									text2 += Conversions.ToString(Source[num3 + 1]);
									num3++;
								}
								if (compareInfo.Compare(text, text2, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) != 0 && !flag5)
								{
									break;
								}
							}
							flag5 = false;
							num3++;
							if (num3 < num2)
							{
								c = Source[num3];
							}
							else if (num3 > num2)
							{
								return false;
							}
						}
					}
					IL_03A3:
					i++;
				}
				if (!flag2)
				{
					return i == num && num3 == num2;
				}
				if (num2 == 0)
				{
					return false;
				}
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Pattern" }));
			}
		}

		private static bool LikeStringCompareBinary(bool SeenNot, bool Match, char p, char s)
		{
			if (SeenNot && Match)
			{
				return p != s;
			}
			if (!SeenNot && !Match)
			{
				return p == s;
			}
			return Match;
		}

		private static bool LikeStringCompare(CompareInfo ci, bool SeenNot, bool Match, char p, char s, CompareOptions Options)
		{
			if (SeenNot && Match)
			{
				if (Options == CompareOptions.Ordinal)
				{
					return p != s;
				}
				return ci.Compare(Conversions.ToString(p), Conversions.ToString(s), Options) != 0;
			}
			else
			{
				if (SeenNot || Match)
				{
					return Match;
				}
				if (Options == CompareOptions.Ordinal)
				{
					return p == s;
				}
				return ci.Compare(Conversions.ToString(p), Conversions.ToString(s), Options) == 0;
			}
		}

		private static int AsteriskSkip(string Pattern, string Source, int SourceEndIndex, CompareMethod CompareOption, CompareInfo ci)
		{
			int num = Strings.Len(Pattern);
			checked
			{
				int i;
				int num2;
				while (i < num)
				{
					char c = Pattern[i];
					char c2 = c;
					bool flag3;
					if (c2 == '*')
					{
						if (num2 > 0)
						{
							bool flag;
							if (flag)
							{
								num2 = Operators.MultipleAsteriskSkip(Pattern, Source, num2, CompareOption);
								return SourceEndIndex - num2;
							}
							string text = Pattern.Substring(0, i);
							CompareOptions compareOptions;
							if (CompareOption == CompareMethod.Binary)
							{
								compareOptions = CompareOptions.Ordinal;
							}
							else
							{
								compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;
							}
							num2 = ci.LastIndexOf(Source, text, compareOptions);
							return num2;
						}
					}
					else if (c2 == '-')
					{
						if (Pattern[i + 1] == ']')
						{
							bool flag2 = true;
						}
					}
					else if (c2 == '!')
					{
						if (Pattern[i + 1] == ']')
						{
							bool flag2 = true;
						}
						else
						{
							bool flag = true;
						}
					}
					else if (c2 == '[')
					{
						if (flag3)
						{
							bool flag2 = true;
						}
						else
						{
							flag3 = true;
						}
					}
					else if (c2 == ']')
					{
						bool flag2;
						if (flag2 || !flag3)
						{
							num2++;
							bool flag = true;
						}
						flag2 = false;
						flag3 = false;
					}
					else if (c2 == '?' || c2 == '#')
					{
						if (flag3)
						{
							bool flag2 = true;
						}
						else
						{
							num2++;
							bool flag = true;
						}
					}
					else if (flag3)
					{
						bool flag2 = true;
					}
					else
					{
						num2++;
					}
					i++;
				}
				return SourceEndIndex - num2;
			}
		}

		private static int MultipleAsteriskSkip(string Pattern, string Source, int Count, CompareMethod CompareOption)
		{
			int num = Strings.Len(Source);
			checked
			{
				while (Count < num)
				{
					string text = Source.Substring(num - Count);
					bool flag;
					try
					{
						flag = Operators.LikeString(text, Pattern, CompareOption);
					}
					catch (StackOverflowException ex)
					{
						throw ex;
					}
					catch (OutOfMemoryException ex2)
					{
						throw ex2;
					}
					catch (ThreadAbortException ex3)
					{
						throw ex3;
					}
					catch (Exception)
					{
						flag = false;
					}
					if (flag)
					{
						break;
					}
					Count++;
				}
				return Count;
			}
		}

		public static object ConcatenateObject(object Left, object Right)
		{
			IConvertible convertible = Left as IConvertible;
			TypeCode typeCode;
			if (convertible == null)
			{
				if (Left == null)
				{
					typeCode = TypeCode.Empty;
				}
				else
				{
					typeCode = TypeCode.Object;
				}
			}
			else
			{
				typeCode = convertible.GetTypeCode();
			}
			IConvertible convertible2 = Right as IConvertible;
			TypeCode typeCode2;
			if (convertible2 == null)
			{
				if (Right == null)
				{
					typeCode2 = TypeCode.Empty;
				}
				else
				{
					typeCode2 = TypeCode.Object;
				}
			}
			else
			{
				typeCode2 = convertible2.GetTypeCode();
			}
			if (typeCode == TypeCode.Object && Left is char[])
			{
				typeCode = TypeCode.String;
			}
			if (typeCode2 == TypeCode.Object && Right is char[])
			{
				typeCode2 = TypeCode.String;
			}
			if (typeCode == TypeCode.Object || typeCode2 == TypeCode.Object)
			{
				return Operators.InvokeUserDefinedOperator(Symbols.UserDefinedOperator.Concatenate, new object[] { Left, Right });
			}
			bool flag = typeCode == TypeCode.DBNull;
			bool flag2 = typeCode2 == TypeCode.DBNull;
			if (flag && flag2)
			{
				return Left;
			}
			if (flag & !flag2)
			{
				Left = "";
			}
			else if (flag2 & !flag)
			{
				Right = "";
			}
			return Conversions.ToString(Left) + Conversions.ToString(Right);
		}

		internal static readonly object Boxed_ZeroDouble = 0.0;

		internal static readonly object Boxed_ZeroSinge = 0f;

		internal static readonly object Boxed_ZeroDecimal = 0m;

		internal static readonly object Boxed_ZeroLong = 0L;

		internal static readonly object Boxed_ZeroInteger = 0;

		internal static readonly object Boxed_ZeroShort = 0;

		internal static readonly object Boxed_ZeroULong = 0UL;

		internal static readonly object Boxed_ZeroUInteger = 0U;

		internal static readonly object Boxed_ZeroUShort = 0;

		internal static readonly object Boxed_ZeroSByte = 0;

		internal static readonly object Boxed_ZeroByte = 0;

		private const int TCMAX = 19;

		private enum CompareClass
		{
			Less = -1,
			Equal,
			Greater,
			Unordered,
			UserDefined,
			Undefined
		}
	}
}
