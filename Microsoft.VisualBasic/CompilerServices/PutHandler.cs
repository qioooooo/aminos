using System;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class PutHandler : IRecordEnum
	{
		public PutHandler(VB6File oFile)
		{
			this.m_oFile = oFile;
		}

		public bool Callback(FieldInfo field_info, ref object vValue)
		{
			Type fieldType = field_info.FieldType;
			if (fieldType == null)
			{
				throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Empty" })), 5);
			}
			bool flag;
			if (fieldType.IsArray)
			{
				int num = -1;
				object[] array = field_info.GetCustomAttributes(typeof(VBFixedArrayAttribute), false);
				VBFixedArrayAttribute vbfixedArrayAttribute;
				if (array != null && array.Length != 0)
				{
					vbfixedArrayAttribute = (VBFixedArrayAttribute)array[0];
				}
				else
				{
					vbfixedArrayAttribute = null;
				}
				Type elementType = fieldType.GetElementType();
				if (elementType == typeof(string))
				{
					array = field_info.GetCustomAttributes(typeof(VBFixedStringAttribute), false);
					if (array == null || array.Length == 0)
					{
						num = -1;
					}
					else
					{
						num = ((VBFixedStringAttribute)array[0]).Length;
					}
				}
				if (vbfixedArrayAttribute == null)
				{
					this.m_oFile.PutDynamicArray(0L, (Array)vValue, false, num);
				}
				else
				{
					this.m_oFile.PutFixedArray(0L, (Array)vValue, elementType, num, vbfixedArrayAttribute.FirstBound, vbfixedArrayAttribute.SecondBound);
				}
			}
			else
			{
				switch (Type.GetTypeCode(fieldType))
				{
				case TypeCode.DBNull:
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "DBNull" })), 5);
				case TypeCode.Boolean:
					this.m_oFile.PutBoolean(0L, BooleanType.FromObject(vValue), false);
					return flag;
				case TypeCode.Char:
					this.m_oFile.PutChar(0L, CharType.FromObject(vValue), false);
					return flag;
				case TypeCode.Byte:
					this.m_oFile.PutByte(0L, ByteType.FromObject(vValue), false);
					return flag;
				case TypeCode.Int16:
					this.m_oFile.PutShort(0L, ShortType.FromObject(vValue), false);
					return flag;
				case TypeCode.Int32:
					this.m_oFile.PutInteger(0L, IntegerType.FromObject(vValue), false);
					return flag;
				case TypeCode.Int64:
					this.m_oFile.PutLong(0L, LongType.FromObject(vValue), false);
					return flag;
				case TypeCode.Single:
					this.m_oFile.PutSingle(0L, SingleType.FromObject(vValue), false);
					return flag;
				case TypeCode.Double:
					this.m_oFile.PutDouble(0L, DoubleType.FromObject(vValue), false);
					return flag;
				case TypeCode.Decimal:
					this.m_oFile.PutDecimal(0L, DecimalType.FromObject(vValue), false);
					return flag;
				case TypeCode.DateTime:
					this.m_oFile.PutDate(0L, DateType.FromObject(vValue), false);
					return flag;
				case TypeCode.String:
				{
					string text;
					if (vValue != null)
					{
						text = vValue.ToString();
					}
					else
					{
						text = null;
					}
					object[] customAttributes = field_info.GetCustomAttributes(typeof(VBFixedStringAttribute), false);
					if (customAttributes == null || customAttributes.Length == 0)
					{
						this.m_oFile.PutStringWithLength(0L, text);
						return flag;
					}
					VBFixedStringAttribute vbfixedStringAttribute = (VBFixedStringAttribute)customAttributes[0];
					int num2 = vbfixedStringAttribute.Length;
					if (num2 == 0)
					{
						num2 = -1;
					}
					this.m_oFile.PutFixedLengthString(0L, text, num2);
					return flag;
				}
				}
				if (fieldType == typeof(object))
				{
					this.m_oFile.PutObject(vValue, 0L, true);
				}
				else
				{
					if (fieldType == typeof(Exception))
					{
						throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Exception" })), 5);
					}
					if (fieldType == typeof(Missing))
					{
						throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Missing" })), 5);
					}
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, fieldType.Name })), 5);
				}
			}
			return flag;
		}

		public VB6File m_oFile;
	}
}
