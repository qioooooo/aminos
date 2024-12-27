using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EC RID: 748
	[ComVisible(true)]
	[Serializable]
	public struct CustomAttributeTypedArgument
	{
		// Token: 0x06001DF8 RID: 7672 RVA: 0x0004B10E File Offset: 0x0004A10E
		public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001DF9 RID: 7673 RVA: 0x0004B123 File Offset: 0x0004A123
		public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001DFA RID: 7674 RVA: 0x0004B13C File Offset: 0x0004A13C
		private static Type CustomAttributeEncodingToType(CustomAttributeEncoding encodedType)
		{
			if (encodedType <= CustomAttributeEncoding.Array)
			{
				switch (encodedType)
				{
				case CustomAttributeEncoding.Boolean:
					return typeof(bool);
				case CustomAttributeEncoding.Char:
					return typeof(char);
				case CustomAttributeEncoding.SByte:
					return typeof(sbyte);
				case CustomAttributeEncoding.Byte:
					return typeof(byte);
				case CustomAttributeEncoding.Int16:
					return typeof(short);
				case CustomAttributeEncoding.UInt16:
					return typeof(ushort);
				case CustomAttributeEncoding.Int32:
					return typeof(int);
				case CustomAttributeEncoding.UInt32:
					return typeof(uint);
				case CustomAttributeEncoding.Int64:
					return typeof(long);
				case CustomAttributeEncoding.UInt64:
					return typeof(ulong);
				case CustomAttributeEncoding.Float:
					return typeof(float);
				case CustomAttributeEncoding.Double:
					return typeof(double);
				case CustomAttributeEncoding.String:
					return typeof(string);
				default:
					if (encodedType == CustomAttributeEncoding.Array)
					{
						return typeof(Array);
					}
					break;
				}
			}
			else
			{
				switch (encodedType)
				{
				case CustomAttributeEncoding.Type:
					return typeof(Type);
				case CustomAttributeEncoding.Object:
					return typeof(object);
				default:
					if (encodedType == CustomAttributeEncoding.Enum)
					{
						return typeof(Enum);
					}
					break;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)encodedType }), "encodedType");
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0004B290 File Offset: 0x0004A290
		private unsafe static object EncodedValueToRawValue(long val, CustomAttributeEncoding encodedType)
		{
			switch (encodedType)
			{
			case CustomAttributeEncoding.Boolean:
				return (byte)val != 0;
			case CustomAttributeEncoding.Char:
				return (char)val;
			case CustomAttributeEncoding.SByte:
				return (sbyte)val;
			case CustomAttributeEncoding.Byte:
				return (byte)val;
			case CustomAttributeEncoding.Int16:
				return (short)val;
			case CustomAttributeEncoding.UInt16:
				return (ushort)val;
			case CustomAttributeEncoding.Int32:
				return (int)val;
			case CustomAttributeEncoding.UInt32:
				return (uint)val;
			case CustomAttributeEncoding.Int64:
				return val;
			case CustomAttributeEncoding.UInt64:
				return (ulong)val;
			case CustomAttributeEncoding.Float:
				return *(float*)(&val);
			case CustomAttributeEncoding.Double:
				return *(double*)(&val);
			default:
				throw new ArgumentException(Environment.GetResourceString("Arg_EnumIllegalVal", new object[] { (int)val }), "val");
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0004B368 File Offset: 0x0004A368
		private static Type ResolveType(Module scope, string typeName)
		{
			Type typeByNameUsingCARules = RuntimeTypeHandle.GetTypeByNameUsingCARules(typeName, scope);
			if (typeByNameUsingCARules == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, Environment.GetResourceString("Arg_CATypeResolutionFailed"), new object[] { typeName }));
			}
			return typeByNameUsingCARules;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0004B3A8 File Offset: 0x0004A3A8
		internal CustomAttributeTypedArgument(object value)
		{
			this.m_argumentType = value.GetType();
			if (!this.m_argumentType.IsEnum)
			{
				this.m_value = value;
				return;
			}
			if (Enum.GetUnderlyingType(this.m_argumentType) == typeof(int))
			{
				this.m_value = (int)value;
				return;
			}
			if (Enum.GetUnderlyingType(this.m_argumentType) == typeof(short))
			{
				this.m_value = (short)value;
				return;
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_EnumIsNotIntOrShort"), "value");
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0004B43C File Offset: 0x0004A43C
		internal CustomAttributeTypedArgument(Module scope, CustomAttributeEncodedArgument encodedArg)
		{
			CustomAttributeEncoding customAttributeEncoding = encodedArg.CustomAttributeType.EncodedType;
			if (customAttributeEncoding == CustomAttributeEncoding.Undefined)
			{
				throw new ArgumentException("encodedArg");
			}
			if (customAttributeEncoding == CustomAttributeEncoding.Enum)
			{
				this.m_argumentType = CustomAttributeTypedArgument.ResolveType(scope, encodedArg.CustomAttributeType.EnumName);
				this.m_value = CustomAttributeTypedArgument.EncodedValueToRawValue(encodedArg.PrimitiveValue, encodedArg.CustomAttributeType.EncodedEnumType);
				return;
			}
			if (customAttributeEncoding == CustomAttributeEncoding.String)
			{
				this.m_argumentType = typeof(string);
				this.m_value = encodedArg.StringValue;
				return;
			}
			if (customAttributeEncoding == CustomAttributeEncoding.Type)
			{
				this.m_argumentType = typeof(Type);
				this.m_value = null;
				if (encodedArg.StringValue != null)
				{
					this.m_value = CustomAttributeTypedArgument.ResolveType(scope, encodedArg.StringValue);
					return;
				}
			}
			else if (customAttributeEncoding == CustomAttributeEncoding.Array)
			{
				customAttributeEncoding = encodedArg.CustomAttributeType.EncodedArrayType;
				Type type;
				if (customAttributeEncoding == CustomAttributeEncoding.Enum)
				{
					type = CustomAttributeTypedArgument.ResolveType(scope, encodedArg.CustomAttributeType.EnumName);
				}
				else
				{
					type = CustomAttributeTypedArgument.CustomAttributeEncodingToType(customAttributeEncoding);
				}
				this.m_argumentType = type.MakeArrayType();
				if (encodedArg.ArrayValue == null)
				{
					this.m_value = null;
					return;
				}
				CustomAttributeTypedArgument[] array = new CustomAttributeTypedArgument[encodedArg.ArrayValue.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new CustomAttributeTypedArgument(scope, encodedArg.ArrayValue[i]);
				}
				this.m_value = Array.AsReadOnly<CustomAttributeTypedArgument>(array);
				return;
			}
			else
			{
				this.m_argumentType = CustomAttributeTypedArgument.CustomAttributeEncodingToType(customAttributeEncoding);
				this.m_value = CustomAttributeTypedArgument.EncodedValueToRawValue(encodedArg.PrimitiveValue, customAttributeEncoding);
			}
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0004B5D3 File Offset: 0x0004A5D3
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0004B5DC File Offset: 0x0004A5DC
		internal string ToString(bool typed)
		{
			if (this.ArgumentType.IsEnum)
			{
				return string.Format(CultureInfo.CurrentCulture, typed ? "{0}" : "({1}){0}", new object[]
				{
					this.Value,
					this.ArgumentType.FullName
				});
			}
			if (this.Value == null)
			{
				return string.Format(CultureInfo.CurrentCulture, typed ? "null" : "({0})null", new object[] { this.ArgumentType.Name });
			}
			if (this.ArgumentType == typeof(string))
			{
				return string.Format(CultureInfo.CurrentCulture, "\"{0}\"", new object[] { this.Value });
			}
			if (this.ArgumentType == typeof(char))
			{
				return string.Format(CultureInfo.CurrentCulture, "'{0}'", new object[] { this.Value });
			}
			if (this.ArgumentType == typeof(Type))
			{
				return string.Format(CultureInfo.CurrentCulture, "typeof({0})", new object[] { ((Type)this.Value).FullName });
			}
			if (this.ArgumentType.IsArray)
			{
				IList<CustomAttributeTypedArgument> list = this.Value as IList<CustomAttributeTypedArgument>;
				Type elementType = this.ArgumentType.GetElementType();
				string text = string.Format(CultureInfo.CurrentCulture, "new {0}[{1}] {{ ", new object[]
				{
					elementType.IsEnum ? elementType.FullName : elementType.Name,
					list.Count
				});
				for (int i = 0; i < list.Count; i++)
				{
					text += string.Format(CultureInfo.CurrentCulture, (i == 0) ? "{0}" : ", {0}", new object[] { list[i].ToString(elementType != typeof(object)) });
				}
				return text + " }";
			}
			return string.Format(CultureInfo.CurrentCulture, typed ? "{0}" : "({1}){0}", new object[]
			{
				this.Value,
				this.ArgumentType.Name
			});
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x0004B830 File Offset: 0x0004A830
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x0004B842 File Offset: 0x0004A842
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x0004B852 File Offset: 0x0004A852
		public Type ArgumentType
		{
			get
			{
				return this.m_argumentType;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0004B85A File Offset: 0x0004A85A
		public object Value
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x04000AC8 RID: 2760
		private object m_value;

		// Token: 0x04000AC9 RID: 2761
		private Type m_argumentType;
	}
}
