using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class Versioned
	{
		private Versioned()
		{
		}

		public static object CallByName(object Instance, string MethodName, CallType UseCallType, params object[] Arguments)
		{
			switch (UseCallType)
			{
			case CallType.Method:
				return NewLateBinding.LateCall(Instance, null, MethodName, Arguments, null, null, null, false);
			case CallType.Get:
				return NewLateBinding.LateGet(Instance, null, MethodName, Arguments, null, null, null);
			case CallType.Let:
			case CallType.Set:
				NewLateBinding.LateSet(Instance, null, MethodName, Arguments, null, null, false, false, UseCallType);
				return null;
			}
			throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "CallType" }));
		}

		public static bool IsNumeric(object Expression)
		{
			IConvertible convertible = Expression as IConvertible;
			if (convertible == null)
			{
				return false;
			}
			switch (convertible.GetTypeCode())
			{
			case TypeCode.Boolean:
				return true;
			case TypeCode.Char:
			case TypeCode.String:
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
				catch (FormatException ex)
				{
					return false;
				}
				double num2;
				return Conversions.TryParseDouble(text, ref num2);
			}
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
				return true;
			}
			return false;
		}

		public static string TypeName(object Expression)
		{
			if (Expression == null)
			{
				return "Nothing";
			}
			Type type = Expression.GetType();
			string text;
			if (type.IsCOMObject && string.CompareOrdinal(type.Name, "__ComObject") == 0)
			{
				text = Information.TypeNameOfCOMObject(Expression, true);
			}
			else
			{
				text = Utils.VBFriendlyNameOfType(type, false);
			}
			return text;
		}

		public static string SystemTypeName(string VbName)
		{
			string text = Strings.Trim(VbName).ToUpperInvariant();
			if (Operators.CompareString(text, "BOOLEAN", false) == 0)
			{
				return "System.Boolean";
			}
			if (Operators.CompareString(text, "SBYTE", false) == 0)
			{
				return "System.SByte";
			}
			if (Operators.CompareString(text, "BYTE", false) == 0)
			{
				return "System.Byte";
			}
			if (Operators.CompareString(text, "SHORT", false) == 0)
			{
				return "System.Int16";
			}
			if (Operators.CompareString(text, "USHORT", false) == 0)
			{
				return "System.UInt16";
			}
			if (Operators.CompareString(text, "INTEGER", false) == 0)
			{
				return "System.Int32";
			}
			if (Operators.CompareString(text, "UINTEGER", false) == 0)
			{
				return "System.UInt32";
			}
			if (Operators.CompareString(text, "LONG", false) == 0)
			{
				return "System.Int64";
			}
			if (Operators.CompareString(text, "ULONG", false) == 0)
			{
				return "System.UInt64";
			}
			if (Operators.CompareString(text, "DECIMAL", false) == 0)
			{
				return "System.Decimal";
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
			if (Operators.CompareString(text, "CHAR", false) == 0)
			{
				return "System.Char";
			}
			if (Operators.CompareString(text, "STRING", false) == 0)
			{
				return "System.String";
			}
			if (Operators.CompareString(text, "OBJECT", false) == 0)
			{
				return "System.Object";
			}
			return null;
		}

		public static string VbTypeName(string SystemName)
		{
			SystemName = Strings.Trim(SystemName).ToUpperInvariant();
			if (Operators.CompareString(Strings.Left(SystemName, 7), "SYSTEM.", false) == 0)
			{
				SystemName = Strings.Mid(SystemName, 8);
			}
			string text = SystemName;
			if (Operators.CompareString(text, "BOOLEAN", false) == 0)
			{
				return "Boolean";
			}
			if (Operators.CompareString(text, "SBYTE", false) == 0)
			{
				return "SByte";
			}
			if (Operators.CompareString(text, "BYTE", false) == 0)
			{
				return "Byte";
			}
			if (Operators.CompareString(text, "INT16", false) == 0)
			{
				return "Short";
			}
			if (Operators.CompareString(text, "UINT16", false) == 0)
			{
				return "UShort";
			}
			if (Operators.CompareString(text, "INT32", false) == 0)
			{
				return "Integer";
			}
			if (Operators.CompareString(text, "UINT32", false) == 0)
			{
				return "UInteger";
			}
			if (Operators.CompareString(text, "INT64", false) == 0)
			{
				return "Long";
			}
			if (Operators.CompareString(text, "UINT64", false) == 0)
			{
				return "ULong";
			}
			if (Operators.CompareString(text, "DECIMAL", false) == 0)
			{
				return "Decimal";
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
			if (Operators.CompareString(text, "CHAR", false) == 0)
			{
				return "Char";
			}
			if (Operators.CompareString(text, "STRING", false) == 0)
			{
				return "String";
			}
			if (Operators.CompareString(text, "OBJECT", false) == 0)
			{
				return "Object";
			}
			return null;
		}
	}
}
