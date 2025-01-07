using System;
using System.ComponentModel;
using System.Reflection;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class StructUtils
	{
		private StructUtils()
		{
		}

		internal static object EnumerateUDT(ValueType oStruct, IRecordEnum intfRecEnum, bool fGet)
		{
			Type type = oStruct.GetType();
			VariantType variantType = Information.VarTypeFromComType(type);
			if (variantType != VariantType.UserDefinedType || type.IsPrimitive)
			{
				throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "oStruct" }));
			}
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
			int num = 0;
			int upperBound = fields.GetUpperBound(0);
			int num2 = num;
			int num3 = upperBound;
			checked
			{
				for (int i = num2; i <= num3; i++)
				{
					FieldInfo fieldInfo = fields[i];
					Type fieldType = fieldInfo.FieldType;
					object value = fieldInfo.GetValue(oStruct);
					if (Information.VarTypeFromComType(fieldType) == VariantType.UserDefinedType)
					{
						if (fieldType.IsPrimitive)
						{
							throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { fieldInfo.Name, fieldType.Name })), 5);
						}
						StructUtils.EnumerateUDT((ValueType)value, intfRecEnum, fGet);
					}
					else
					{
						intfRecEnum.Callback(fieldInfo, ref value);
					}
					if (fGet)
					{
						fieldInfo.SetValue(oStruct, value);
					}
				}
				return null;
			}
		}

		internal static int GetRecordLength(object o, int PackSize = -1)
		{
			if (o == null)
			{
				return 0;
			}
			StructUtils.StructByteLengthHandler structByteLengthHandler = new StructUtils.StructByteLengthHandler(PackSize);
			IRecordEnum recordEnum = structByteLengthHandler;
			if (recordEnum == null)
			{
				throw ExceptionUtils.VbMakeException(5);
			}
			StructUtils.EnumerateUDT((ValueType)o, recordEnum, false);
			return structByteLengthHandler.Length;
		}

		private sealed class StructByteLengthHandler : IRecordEnum
		{
			internal StructByteLengthHandler(int PackSize)
			{
				this.m_PackSize = PackSize;
			}

			internal int Length
			{
				get
				{
					if (this.m_PackSize == 1)
					{
						return this.m_StructLength;
					}
					return checked(this.m_StructLength + this.m_StructLength % this.m_PackSize);
				}
			}

			internal void SetAlignment(int size)
			{
				checked
				{
					if (this.m_PackSize != 1)
					{
						this.m_StructLength += this.m_StructLength % size;
					}
				}
			}

			internal bool Callback(FieldInfo field_info, ref object vValue)
			{
				Type fieldType = field_info.FieldType;
				if (fieldType == null)
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Empty" })), 5);
				}
				checked
				{
					int num3;
					if (fieldType.IsArray)
					{
						object[] customAttributes = field_info.GetCustomAttributes(typeof(VBFixedArrayAttribute), false);
						VBFixedArrayAttribute vbfixedArrayAttribute;
						if (customAttributes != null && customAttributes.Length != 0)
						{
							vbfixedArrayAttribute = (VBFixedArrayAttribute)customAttributes[0];
						}
						else
						{
							vbfixedArrayAttribute = null;
						}
						Type elementType = fieldType.GetElementType();
						int num;
						int num2;
						if (vbfixedArrayAttribute == null)
						{
							num = 1;
							num2 = 4;
						}
						else
						{
							num = vbfixedArrayAttribute.Length;
							this.GetFieldSize(field_info, elementType, ref num3, ref num2);
						}
						this.SetAlignment(num3);
						this.m_StructLength += num * num2;
						return false;
					}
					int num4;
					this.GetFieldSize(field_info, fieldType, ref num3, ref num4);
					this.SetAlignment(num3);
					this.m_StructLength += num4;
					return false;
				}
			}

			private void GetFieldSize(FieldInfo field_info, Type FieldType, ref int align, ref int size)
			{
				switch (Type.GetTypeCode(FieldType))
				{
				case TypeCode.DBNull:
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "DBNull" })), 5);
				case TypeCode.Boolean:
					align = 2;
					size = 2;
					break;
				case TypeCode.Char:
					align = 2;
					size = 2;
					break;
				case TypeCode.Byte:
					align = 1;
					size = 1;
					break;
				case TypeCode.Int16:
					align = 2;
					size = 2;
					break;
				case TypeCode.Int32:
					align = 4;
					size = 4;
					break;
				case TypeCode.Int64:
					align = 8;
					size = 8;
					break;
				case TypeCode.Single:
					align = 4;
					size = 4;
					break;
				case TypeCode.Double:
					align = 8;
					size = 8;
					break;
				case TypeCode.Decimal:
					align = 16;
					size = 16;
					break;
				case TypeCode.DateTime:
					align = 8;
					size = 8;
					break;
				case TypeCode.String:
				{
					object[] customAttributes = field_info.GetCustomAttributes(typeof(VBFixedStringAttribute), false);
					if (customAttributes == null || customAttributes.Length == 0)
					{
						align = 4;
						size = 4;
					}
					else
					{
						VBFixedStringAttribute vbfixedStringAttribute = (VBFixedStringAttribute)customAttributes[0];
						int num = vbfixedStringAttribute.Length;
						if (num == 0)
						{
							num = -1;
						}
						size = num;
					}
					break;
				}
				}
				if (FieldType == typeof(Exception))
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Exception" })), 5);
				}
				if (FieldType == typeof(Missing))
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Missing" })), 5);
				}
				if (FieldType == typeof(object))
				{
					throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_UnsupportedFieldType2", new string[] { field_info.Name, "Object" })), 5);
				}
			}

			private int m_StructLength;

			private int m_PackSize;
		}
	}
}
