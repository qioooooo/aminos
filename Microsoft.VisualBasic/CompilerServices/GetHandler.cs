using System;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class GetHandler : IRecordEnum
	{
		public GetHandler(VB6File oFile)
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
				object[] customAttributes = field_info.GetCustomAttributes(typeof(VBFixedArrayAttribute), false);
				Array array = null;
				int num = -1;
				object[] customAttributes2 = field_info.GetCustomAttributes(typeof(VBFixedStringAttribute), false);
				if (customAttributes2 != null && customAttributes2.Length > 0)
				{
					VBFixedStringAttribute vbfixedStringAttribute = (VBFixedStringAttribute)customAttributes2[0];
					if (vbfixedStringAttribute.Length > 0)
					{
						num = vbfixedStringAttribute.Length;
					}
				}
				if (customAttributes == null || customAttributes.Length == 0)
				{
					this.m_oFile.GetDynamicArray(ref array, fieldType.GetElementType(), num);
				}
				else
				{
					VBFixedArrayAttribute vbfixedArrayAttribute = (VBFixedArrayAttribute)customAttributes[0];
					int firstBound = vbfixedArrayAttribute.FirstBound;
					int secondBound = vbfixedArrayAttribute.SecondBound;
					array = (Array)vValue;
					this.m_oFile.GetFixedArray(0L, ref array, fieldType.GetElementType(), firstBound, secondBound, num);
				}
				vValue = array;
			}
			else
			{
				switch (Type.GetTypeCode(fieldType))
				{
				case TypeCode.DBNull:
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "DBNull" })), 5);
				case TypeCode.Boolean:
					vValue = this.m_oFile.GetBoolean(0L);
					return flag;
				case TypeCode.Char:
					vValue = this.m_oFile.GetChar(0L);
					return flag;
				case TypeCode.Byte:
					vValue = this.m_oFile.GetByte(0L);
					return flag;
				case TypeCode.Int16:
					vValue = this.m_oFile.GetShort(0L);
					return flag;
				case TypeCode.Int32:
					vValue = this.m_oFile.GetInteger(0L);
					return flag;
				case TypeCode.Int64:
					vValue = this.m_oFile.GetLong(0L);
					return flag;
				case TypeCode.Single:
					vValue = this.m_oFile.GetSingle(0L);
					return flag;
				case TypeCode.Double:
					vValue = this.m_oFile.GetDouble(0L);
					return flag;
				case TypeCode.Decimal:
					vValue = this.m_oFile.GetDecimal(0L);
					return flag;
				case TypeCode.DateTime:
					vValue = this.m_oFile.GetDate(0L);
					return flag;
				case TypeCode.String:
				{
					object[] customAttributes3 = field_info.GetCustomAttributes(typeof(VBFixedStringAttribute), false);
					if (customAttributes3 == null || customAttributes3.Length == 0)
					{
						vValue = this.m_oFile.GetLengthPrefixedString(0L);
						return flag;
					}
					VBFixedStringAttribute vbfixedStringAttribute2 = (VBFixedStringAttribute)customAttributes3[0];
					int num2 = vbfixedStringAttribute2.Length;
					if (num2 == 0)
					{
						num2 = -1;
					}
					vValue = this.m_oFile.GetFixedLengthString(0L, num2);
					return flag;
				}
				}
				if (fieldType == typeof(object))
				{
					this.m_oFile.GetObject(ref vValue, 0L, true);
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

		private VB6File m_oFile;
	}
}
