using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic
{
	[StandardModule]
	public sealed class Information
	{
		public static ErrObject Err()
		{
			ProjectData projectData = ProjectData.GetProjectData();
			if (projectData.m_Err == null)
			{
				projectData.m_Err = new ErrObject();
			}
			return projectData.m_Err;
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		public static int Erl()
		{
			ProjectData projectData = ProjectData.GetProjectData();
			return projectData.m_Err.Erl;
		}

		public static bool IsArray(object VarName)
		{
			return VarName != null && VarName is Array;
		}

		public static bool IsDate(object Expression)
		{
			if (Expression == null)
			{
				return false;
			}
			if (Expression is DateTime)
			{
				return true;
			}
			string text = Expression as string;
			DateTime dateTime;
			return text != null && Conversions.TryParseDate(text, ref dateTime);
		}

		public static bool IsDBNull(object Expression)
		{
			return Expression != null && Expression is DBNull;
		}

		public static bool IsNothing(object Expression)
		{
			return Expression == null;
		}

		public static bool IsError(object Expression)
		{
			return Expression != null && Expression is Exception;
		}

		public static bool IsReference(object Expression)
		{
			return !(Expression is ValueType);
		}

		public static int LBound(Array Array, int Rank = 1)
		{
			if (Array == null)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Array" })), 9);
			}
			if (Rank < 1 || Rank > Array.Rank)
			{
				throw new RankException(Utils.GetResourceString("Argument_InvalidRank1", new string[] { "Rank" }));
			}
			return Array.GetLowerBound(checked(Rank - 1));
		}

		public static int UBound(Array Array, int Rank = 1)
		{
			if (Array == null)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Array" })), 9);
			}
			if (Rank < 1 || Rank > Array.Rank)
			{
				throw new RankException(Utils.GetResourceString("Argument_InvalidRank1", new string[] { "Rank" }));
			}
			return Array.GetUpperBound(checked(Rank - 1));
		}

		internal static string TypeNameOfCOMObject(object VarName, bool bThrowException)
		{
			string text = "__ComObject";
			try
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
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
			catch (Exception ex4)
			{
				if (bThrowException)
				{
					throw ex4;
				}
				goto IL_00BD;
			}
			UnsafeNativeMethods.ITypeInfo typeInfo = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			UnsafeNativeMethods.IProvideClassInfo provideClassInfo = VarName as UnsafeNativeMethods.IProvideClassInfo;
			if (provideClassInfo != null)
			{
				try
				{
					typeInfo = provideClassInfo.GetClassInfo();
					int num2;
					int num = typeInfo.GetDocumentation(-1, out text2, out text3, out num2, out text4);
					if (num >= 0)
					{
						text = text2;
						goto IL_00BD;
					}
					typeInfo = null;
				}
				catch (StackOverflowException ex5)
				{
					throw ex5;
				}
				catch (OutOfMemoryException ex6)
				{
					throw ex6;
				}
				catch (ThreadAbortException ex7)
				{
					throw ex7;
				}
				catch (Exception)
				{
				}
			}
			UnsafeNativeMethods.IDispatch dispatch = VarName as UnsafeNativeMethods.IDispatch;
			if (dispatch != null)
			{
				int num = dispatch.GetTypeInfo(0, 1033, out typeInfo);
				if (num >= 0)
				{
					int num2;
					num = typeInfo.GetDocumentation(-1, out text2, out text3, out num2, out text4);
					if (num >= 0)
					{
						text = text2;
					}
				}
			}
			IL_00BD:
			if (text[0] == '_')
			{
				text = text.Substring(1);
			}
			return text;
		}

		public static int QBColor(int Color)
		{
			if ((Color & 65520) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Color" }));
			}
			return Information.QBColorTable[Color];
		}

		public static int RGB(int Red, int Green, int Blue)
		{
			if ((Red & -2147483648) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Red" }));
			}
			if ((Green & -2147483648) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Green" }));
			}
			if ((Blue & -2147483648) != 0)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Blue" }));
			}
			if (Red > 255)
			{
				Red = 255;
			}
			if (Green > 255)
			{
				Green = 255;
			}
			if (Blue > 255)
			{
				Blue = 255;
			}
			return checked(Blue * 65536 + Green * 256 + Red);
		}

		public static VariantType VarType(object VarName)
		{
			if (VarName == null)
			{
				return VariantType.Object;
			}
			return Information.VarTypeFromComType(VarName.GetType());
		}

		internal static VariantType VarTypeFromComType(Type typ)
		{
			if (typ == null)
			{
				return VariantType.Object;
			}
			if (typ.IsArray)
			{
				typ = typ.GetElementType();
				if (typ.IsArray)
				{
					return (VariantType)8201;
				}
				VariantType variantType = Information.VarTypeFromComType(typ);
				if ((variantType & VariantType.Array) != VariantType.Empty)
				{
					return (VariantType)8201;
				}
				return variantType | VariantType.Array;
			}
			else
			{
				if (typ.IsEnum)
				{
					typ = Enum.GetUnderlyingType(typ);
				}
				if (typ == null)
				{
					return VariantType.Empty;
				}
				switch (Type.GetTypeCode(typ))
				{
				case TypeCode.DBNull:
					return VariantType.Null;
				case TypeCode.Boolean:
					return VariantType.Boolean;
				case TypeCode.Char:
					return VariantType.Char;
				case TypeCode.Byte:
					return VariantType.Byte;
				case TypeCode.Int16:
					return VariantType.Short;
				case TypeCode.Int32:
					return VariantType.Integer;
				case TypeCode.Int64:
					return VariantType.Long;
				case TypeCode.Single:
					return VariantType.Single;
				case TypeCode.Double:
					return VariantType.Double;
				case TypeCode.Decimal:
					return VariantType.Decimal;
				case TypeCode.DateTime:
					return VariantType.Date;
				case TypeCode.String:
					return VariantType.String;
				}
				if (typ != typeof(Missing))
				{
					if (typ != typeof(Exception))
					{
						if (!typ.IsSubclassOf(typeof(Exception)))
						{
							if (typ.IsValueType)
							{
								return VariantType.UserDefinedType;
							}
							return VariantType.Object;
						}
					}
				}
				return VariantType.Error;
			}
		}

		internal static bool IsOldNumericTypeCode(TypeCode TypCode)
		{
			switch (TypCode)
			{
			case TypeCode.Boolean:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
				return true;
			}
			return false;
		}

		public static bool IsNumeric(object Expression)
		{
			IConvertible convertible = Expression as IConvertible;
			if (convertible == null)
			{
				char[] array = Expression as char[];
				if (array == null)
				{
					return false;
				}
				Expression = new string(array);
			}
			TypeCode typeCode = convertible.GetTypeCode();
			if (typeCode == TypeCode.String || typeCode == TypeCode.Char)
			{
				string text = convertible.ToString(null);
				try
				{
					long num;
					if (Utils.IsHexOrOctValue(text, ref num))
					{
						return true;
					}
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
					return false;
				}
				double num2;
				return DoubleType.TryParse(text, ref num2);
			}
			return Information.IsOldNumericTypeCode(typeCode);
		}

		internal static string OldVBFriendlyNameOfTypeName(string typename)
		{
			string text = null;
			checked
			{
				int num = typename.Length - 1;
				if (typename[num] == ']')
				{
					int num2 = typename.IndexOf('[');
					if (num2 + 1 == num)
					{
						text = "()";
					}
					else
					{
						text = typename.Substring(num2, num - num2 + 1).Replace('[', '(').Replace(']', ')');
					}
					typename = typename.Substring(0, num2);
				}
				string text2 = Information.OldVbTypeName(typename);
				if (text2 == null)
				{
					text2 = typename;
				}
				if (text == null)
				{
					return text2;
				}
				return text2 + Utils.AdjustArraySuffix(text);
			}
		}

		public static string TypeName(object VarName)
		{
			if (VarName == null)
			{
				return "Nothing";
			}
			Type type = VarName.GetType();
			bool flag;
			if (type.IsArray)
			{
				flag = true;
				Type type2 = type;
				type = type2.GetElementType();
			}
			string text;
			if (type.IsEnum)
			{
				text = type.Name;
			}
			else
			{
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.DBNull:
					text = "DBNull";
					goto IL_0136;
				case TypeCode.Boolean:
					text = "Boolean";
					goto IL_0136;
				case TypeCode.Char:
					text = "Char";
					goto IL_0136;
				case TypeCode.Byte:
					text = "Byte";
					goto IL_0136;
				case TypeCode.Int16:
					text = "Short";
					goto IL_0136;
				case TypeCode.Int32:
					text = "Integer";
					goto IL_0136;
				case TypeCode.Int64:
					text = "Long";
					goto IL_0136;
				case TypeCode.Single:
					text = "Single";
					goto IL_0136;
				case TypeCode.Double:
					text = "Double";
					goto IL_0136;
				case TypeCode.Decimal:
					text = "Decimal";
					goto IL_0136;
				case TypeCode.DateTime:
					text = "Date";
					goto IL_0136;
				case TypeCode.String:
					text = "String";
					goto IL_0136;
				}
				text = type.Name;
				if (type.IsCOMObject && string.CompareOrdinal(text, "__ComObject") == 0)
				{
					text = Information.LegacyTypeNameOfCOMObject(VarName, true);
				}
			}
			int num = text.IndexOf('+');
			if (num >= 0)
			{
				text = text.Substring(checked(num + 1));
			}
			IL_0136:
			if (flag)
			{
				Array array = (Array)VarName;
				if (array.Rank == 1)
				{
					text += "[]";
				}
				else
				{
					text = text + "[" + new string(',', checked(array.Rank - 1)) + "]";
				}
				text = Information.OldVBFriendlyNameOfTypeName(text);
			}
			return text;
		}

		public static string SystemTypeName(string VbName)
		{
			string text = Strings.Trim(VbName).ToUpperInvariant();
			if (Operators.CompareString(text, "OBJECT", false) == 0)
			{
				return "System.Object";
			}
			if (Operators.CompareString(text, "SHORT", false) == 0)
			{
				return "System.Int16";
			}
			if (Operators.CompareString(text, "INTEGER", false) == 0)
			{
				return "System.Int32";
			}
			if (Operators.CompareString(text, "SINGLE", false) == 0)
			{
				return "System.Single";
			}
			if (Operators.CompareString(text, "DOUBLE", false) == 0)
			{
				return "System.Double";
			}
			if (Operators.CompareString(text, "DATE", false) == 0)
			{
				return "System.DateTime";
			}
			if (Operators.CompareString(text, "STRING", false) == 0)
			{
				return "System.String";
			}
			if (Operators.CompareString(text, "BOOLEAN", false) == 0)
			{
				return "System.Boolean";
			}
			if (Operators.CompareString(text, "DECIMAL", false) == 0)
			{
				return "System.Decimal";
			}
			if (Operators.CompareString(text, "BYTE", false) == 0)
			{
				return "System.Byte";
			}
			if (Operators.CompareString(text, "CHAR", false) == 0)
			{
				return "System.Char";
			}
			if (Operators.CompareString(text, "LONG", false) == 0)
			{
				return "System.Int64";
			}
			return null;
		}

		public static string VbTypeName(string UrtName)
		{
			return Information.OldVbTypeName(UrtName);
		}

		internal static string OldVbTypeName(string UrtName)
		{
			UrtName = Strings.Trim(UrtName).ToUpperInvariant();
			if (Operators.CompareString(Strings.Left(UrtName, 7), "SYSTEM.", false) == 0)
			{
				UrtName = Strings.Mid(UrtName, 8);
			}
			string text = UrtName;
			if (Operators.CompareString(text, "OBJECT", false) == 0)
			{
				return "Object";
			}
			if (Operators.CompareString(text, "INT16", false) == 0)
			{
				return "Short";
			}
			if (Operators.CompareString(text, "INT32", false) == 0)
			{
				return "Integer";
			}
			if (Operators.CompareString(text, "SINGLE", false) == 0)
			{
				return "Single";
			}
			if (Operators.CompareString(text, "DOUBLE", false) == 0)
			{
				return "Double";
			}
			if (Operators.CompareString(text, "DATETIME", false) == 0)
			{
				return "Date";
			}
			if (Operators.CompareString(text, "STRING", false) == 0)
			{
				return "String";
			}
			if (Operators.CompareString(text, "BOOLEAN", false) == 0)
			{
				return "Boolean";
			}
			if (Operators.CompareString(text, "DECIMAL", false) == 0)
			{
				return "Decimal";
			}
			if (Operators.CompareString(text, "BYTE", false) == 0)
			{
				return "Byte";
			}
			if (Operators.CompareString(text, "CHAR", false) == 0)
			{
				return "Char";
			}
			if (Operators.CompareString(text, "INT64", false) == 0)
			{
				return "Long";
			}
			return null;
		}

		internal static string LegacyTypeNameOfCOMObject(object VarName, bool bThrowException)
		{
			string text = "__ComObject";
			try
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
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
			catch (Exception ex4)
			{
				if (bThrowException)
				{
					throw ex4;
				}
				goto IL_0072;
			}
			UnsafeNativeMethods.ITypeInfo typeInfo = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			UnsafeNativeMethods.IDispatch dispatch = VarName as UnsafeNativeMethods.IDispatch;
			if (dispatch != null)
			{
				int num = dispatch.GetTypeInfo(0, 1033, out typeInfo);
				if (num >= 0)
				{
					int num2;
					num = typeInfo.GetDocumentation(-1, out text2, out text3, out num2, out text4);
					if (num >= 0)
					{
						text = text2;
					}
				}
			}
			IL_0072:
			if (text[0] == '_')
			{
				text = text.Substring(1);
			}
			return text;
		}

		private static readonly int[] QBColorTable = new int[]
		{
			0, 8388608, 32768, 8421376, 128, 8388736, 32896, 12632256, 8421504, 16711680,
			65280, 16776960, 255, 16711935, 65535, 16777215
		};

		internal const string COMObjectName = "__ComObject";
	}
}
