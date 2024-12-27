using System;

namespace System.Xml.Schema
{
	// Token: 0x0200028C RID: 652
	internal class XmlNumeric10Converter : XmlBaseConverter
	{
		// Token: 0x06001EFA RID: 7930 RVA: 0x0008B898 File Offset: 0x0008A898
		protected XmlNumeric10Converter(XmlSchemaType schemaType)
			: base(schemaType)
		{
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0008B8A1 File Offset: 0x0008A8A1
		public static XmlValueConverter Create(XmlSchemaType schemaType)
		{
			return new XmlNumeric10Converter(schemaType);
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0008B8A9 File Offset: 0x0008A8A9
		public override decimal ToDecimal(decimal value)
		{
			return value;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0008B8AC File Offset: 0x0008A8AC
		public override decimal ToDecimal(int value)
		{
			return value;
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0008B8B4 File Offset: 0x0008A8B4
		public override decimal ToDecimal(long value)
		{
			return value;
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0008B8BC File Offset: 0x0008A8BC
		public override decimal ToDecimal(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (base.TypeCode == XmlTypeCode.Decimal)
			{
				return XmlConvert.ToDecimal(value);
			}
			return XmlConvert.ToInteger(value);
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0008B8E4 File Offset: 0x0008A8E4
		public override decimal ToDecimal(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DecimalType)
			{
				return (decimal)value;
			}
			if (type == XmlBaseConverter.Int32Type)
			{
				return (int)value;
			}
			if (type == XmlBaseConverter.Int64Type)
			{
				return (long)value;
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToDecimal((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return (decimal)((XmlAtomicValue)value).ValueAs(XmlBaseConverter.DecimalType);
			}
			return (decimal)this.ChangeTypeWildcardDestination(value, XmlBaseConverter.DecimalType, null);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0008B982 File Offset: 0x0008A982
		public override int ToInt32(decimal value)
		{
			return XmlBaseConverter.DecimalToInt32(value);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0008B98A File Offset: 0x0008A98A
		public override int ToInt32(int value)
		{
			return value;
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0008B98D File Offset: 0x0008A98D
		public override int ToInt32(long value)
		{
			return XmlBaseConverter.Int64ToInt32(value);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0008B995 File Offset: 0x0008A995
		public override int ToInt32(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (base.TypeCode == XmlTypeCode.Decimal)
			{
				return XmlBaseConverter.DecimalToInt32(XmlConvert.ToDecimal(value));
			}
			return XmlConvert.ToInt32(value);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0008B9C4 File Offset: 0x0008A9C4
		public override int ToInt32(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DecimalType)
			{
				return XmlBaseConverter.DecimalToInt32((decimal)value);
			}
			if (type == XmlBaseConverter.Int32Type)
			{
				return (int)value;
			}
			if (type == XmlBaseConverter.Int64Type)
			{
				return XmlBaseConverter.Int64ToInt32((long)value);
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToInt32((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAsInt;
			}
			return (int)this.ChangeTypeWildcardDestination(value, XmlBaseConverter.Int32Type, null);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0008BA58 File Offset: 0x0008AA58
		public override long ToInt64(decimal value)
		{
			return XmlBaseConverter.DecimalToInt64(value);
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0008BA60 File Offset: 0x0008AA60
		public override long ToInt64(int value)
		{
			return (long)value;
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0008BA64 File Offset: 0x0008AA64
		public override long ToInt64(long value)
		{
			return value;
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0008BA67 File Offset: 0x0008AA67
		public override long ToInt64(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (base.TypeCode == XmlTypeCode.Decimal)
			{
				return XmlBaseConverter.DecimalToInt64(XmlConvert.ToDecimal(value));
			}
			return XmlConvert.ToInt64(value);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0008BA94 File Offset: 0x0008AA94
		public override long ToInt64(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DecimalType)
			{
				return XmlBaseConverter.DecimalToInt64((decimal)value);
			}
			if (type == XmlBaseConverter.Int32Type)
			{
				return (long)((int)value);
			}
			if (type == XmlBaseConverter.Int64Type)
			{
				return (long)value;
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToInt64((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAsLong;
			}
			return (long)this.ChangeTypeWildcardDestination(value, XmlBaseConverter.Int64Type, null);
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x0008BB24 File Offset: 0x0008AB24
		public override string ToString(decimal value)
		{
			if (base.TypeCode == XmlTypeCode.Decimal)
			{
				return XmlConvert.ToString(value);
			}
			return XmlConvert.ToString(decimal.Truncate(value));
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x0008BB42 File Offset: 0x0008AB42
		public override string ToString(int value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x0008BB4A File Offset: 0x0008AB4A
		public override string ToString(long value)
		{
			return XmlConvert.ToString(value);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x0008BB52 File Offset: 0x0008AB52
		public override string ToString(string value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return value;
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x0008BB64 File Offset: 0x0008AB64
		public override string ToString(object value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DecimalType)
			{
				return this.ToString((decimal)value);
			}
			if (type == XmlBaseConverter.Int32Type)
			{
				return XmlConvert.ToString((int)value);
			}
			if (type == XmlBaseConverter.Int64Type)
			{
				return XmlConvert.ToString((long)value);
			}
			if (type == XmlBaseConverter.StringType)
			{
				return (string)value;
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).Value;
			}
			return (string)this.ChangeTypeWildcardDestination(value, XmlBaseConverter.StringType, nsResolver);
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x0008BBF8 File Offset: 0x0008ABF8
		public override object ChangeType(decimal value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DecimalType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.Int32Type)
			{
				return XmlBaseConverter.DecimalToInt32(value);
			}
			if (destinationType == XmlBaseConverter.Int64Type)
			{
				return XmlBaseConverter.DecimalToInt64(value);
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return this.ToString(value);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			return this.ChangeTypeWildcardSource(value, destinationType, null);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x0008BCAC File Offset: 0x0008ACAC
		public override object ChangeType(int value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DecimalType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.Int32Type)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.Int64Type)
			{
				return (long)value;
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return XmlConvert.ToString(value);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			return this.ChangeTypeWildcardSource(value, destinationType, null);
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0008BD54 File Offset: 0x0008AD54
		public override object ChangeType(long value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DecimalType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.Int32Type)
			{
				return XmlBaseConverter.Int64ToInt32(value);
			}
			if (destinationType == XmlBaseConverter.Int64Type)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return XmlConvert.ToString(value);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			return this.ChangeTypeWildcardSource(value, destinationType, null);
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0008BE00 File Offset: 0x0008AE00
		public override object ChangeType(string value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DecimalType)
			{
				return this.ToDecimal(value);
			}
			if (destinationType == XmlBaseConverter.Int32Type)
			{
				return this.ToInt32(value);
			}
			if (destinationType == XmlBaseConverter.Int64Type)
			{
				return this.ToInt64(value);
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return new XmlAtomicValue(base.SchemaType, value);
			}
			return this.ChangeTypeWildcardSource(value, destinationType, nsResolver);
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x0008BEB8 File Offset: 0x0008AEB8
		public override object ChangeType(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			Type type = value.GetType();
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DecimalType)
			{
				return this.ToDecimal(value);
			}
			if (destinationType == XmlBaseConverter.Int32Type)
			{
				return this.ToInt32(value);
			}
			if (destinationType == XmlBaseConverter.Int64Type)
			{
				return this.ToInt64(value);
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return this.ToString(value, nsResolver);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				if (type == XmlBaseConverter.DecimalType)
				{
					return new XmlAtomicValue(base.SchemaType, value);
				}
				if (type == XmlBaseConverter.Int32Type)
				{
					return new XmlAtomicValue(base.SchemaType, (int)value);
				}
				if (type == XmlBaseConverter.Int64Type)
				{
					return new XmlAtomicValue(base.SchemaType, (long)value);
				}
				if (type == XmlBaseConverter.StringType)
				{
					return new XmlAtomicValue(base.SchemaType, (string)value);
				}
				if (type == XmlBaseConverter.XmlAtomicValueType)
				{
					return (XmlAtomicValue)value;
				}
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				if (type == XmlBaseConverter.DecimalType)
				{
					return new XmlAtomicValue(base.SchemaType, value);
				}
				if (type == XmlBaseConverter.Int32Type)
				{
					return new XmlAtomicValue(base.SchemaType, (int)value);
				}
				if (type == XmlBaseConverter.Int64Type)
				{
					return new XmlAtomicValue(base.SchemaType, (long)value);
				}
				if (type == XmlBaseConverter.StringType)
				{
					return new XmlAtomicValue(base.SchemaType, (string)value);
				}
				if (type == XmlBaseConverter.XmlAtomicValueType)
				{
					return (XmlAtomicValue)value;
				}
			}
			if (destinationType == XmlBaseConverter.ByteType)
			{
				return XmlBaseConverter.Int32ToByte(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.Int16Type)
			{
				return XmlBaseConverter.Int32ToInt16(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.SByteType)
			{
				return XmlBaseConverter.Int32ToSByte(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.UInt16Type)
			{
				return XmlBaseConverter.Int32ToUInt16(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.UInt32Type)
			{
				return XmlBaseConverter.Int64ToUInt32(this.ToInt64(value));
			}
			if (destinationType == XmlBaseConverter.UInt64Type)
			{
				return XmlBaseConverter.DecimalToUInt64(this.ToDecimal(value));
			}
			if (type == XmlBaseConverter.ByteType)
			{
				return this.ChangeType((int)((byte)value), destinationType);
			}
			if (type == XmlBaseConverter.Int16Type)
			{
				return this.ChangeType((int)((short)value), destinationType);
			}
			if (type == XmlBaseConverter.SByteType)
			{
				return this.ChangeType((int)((sbyte)value), destinationType);
			}
			if (type == XmlBaseConverter.UInt16Type)
			{
				return this.ChangeType((int)((ushort)value), destinationType);
			}
			if (type == XmlBaseConverter.UInt32Type)
			{
				return this.ChangeType((long)((ulong)((uint)value)), destinationType);
			}
			if (type == XmlBaseConverter.UInt64Type)
			{
				return this.ChangeType((ulong)value, destinationType);
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x0008C16C File Offset: 0x0008B16C
		private object ChangeTypeWildcardDestination(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			Type type = value.GetType();
			if (type == XmlBaseConverter.ByteType)
			{
				return this.ChangeType((int)((byte)value), destinationType);
			}
			if (type == XmlBaseConverter.Int16Type)
			{
				return this.ChangeType((int)((short)value), destinationType);
			}
			if (type == XmlBaseConverter.SByteType)
			{
				return this.ChangeType((int)((sbyte)value), destinationType);
			}
			if (type == XmlBaseConverter.UInt16Type)
			{
				return this.ChangeType((int)((ushort)value), destinationType);
			}
			if (type == XmlBaseConverter.UInt32Type)
			{
				return this.ChangeType((long)((ulong)((uint)value)), destinationType);
			}
			if (type == XmlBaseConverter.UInt64Type)
			{
				return this.ChangeType((ulong)value, destinationType);
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0008C214 File Offset: 0x0008B214
		private object ChangeTypeWildcardSource(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			if (destinationType == XmlBaseConverter.ByteType)
			{
				return XmlBaseConverter.Int32ToByte(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.Int16Type)
			{
				return XmlBaseConverter.Int32ToInt16(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.SByteType)
			{
				return XmlBaseConverter.Int32ToSByte(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.UInt16Type)
			{
				return XmlBaseConverter.Int32ToUInt16(this.ToInt32(value));
			}
			if (destinationType == XmlBaseConverter.UInt32Type)
			{
				return XmlBaseConverter.Int64ToUInt32(this.ToInt64(value));
			}
			if (destinationType == XmlBaseConverter.UInt64Type)
			{
				return XmlBaseConverter.DecimalToUInt64(this.ToDecimal(value));
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}
	}
}
