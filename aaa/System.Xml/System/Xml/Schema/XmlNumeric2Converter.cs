using System;

namespace System.Xml.Schema
{
	// Token: 0x0200028D RID: 653
	internal class XmlNumeric2Converter : XmlBaseConverter
	{
		// Token: 0x06001F17 RID: 7959 RVA: 0x0008C2C6 File Offset: 0x0008B2C6
		protected XmlNumeric2Converter(XmlSchemaType schemaType)
			: base(schemaType)
		{
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0008C2CF File Offset: 0x0008B2CF
		public static XmlValueConverter Create(XmlSchemaType schemaType)
		{
			return new XmlNumeric2Converter(schemaType);
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0008C2D7 File Offset: 0x0008B2D7
		public override double ToDouble(double value)
		{
			return value;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0008C2DB File Offset: 0x0008B2DB
		public override double ToDouble(float value)
		{
			return (double)value;
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0008C2E0 File Offset: 0x0008B2E0
		public override double ToDouble(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (base.TypeCode == XmlTypeCode.Float)
			{
				return (double)XmlConvert.ToSingle(value);
			}
			return XmlConvert.ToDouble(value);
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0008C308 File Offset: 0x0008B308
		public override double ToDouble(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DoubleType)
			{
				return (double)value;
			}
			if (type == XmlBaseConverter.SingleType)
			{
				return (double)((float)value);
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToDouble((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAsDouble;
			}
			return (double)this.ChangeListType(value, XmlBaseConverter.DoubleType, null);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x0008C384 File Offset: 0x0008B384
		public override float ToSingle(double value)
		{
			return (float)value;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0008C389 File Offset: 0x0008B389
		public override float ToSingle(float value)
		{
			return value;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0008C38D File Offset: 0x0008B38D
		public override float ToSingle(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (base.TypeCode == XmlTypeCode.Float)
			{
				return XmlConvert.ToSingle(value);
			}
			return (float)XmlConvert.ToDouble(value);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0008C3B8 File Offset: 0x0008B3B8
		public override float ToSingle(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DoubleType)
			{
				return (float)((double)value);
			}
			if (type == XmlBaseConverter.SingleType)
			{
				return (float)value;
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToSingle((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return (float)((XmlAtomicValue)value).ValueAs(XmlBaseConverter.SingleType);
			}
			return (float)this.ChangeListType(value, XmlBaseConverter.SingleType, null);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0008C43E File Offset: 0x0008B43E
		public override string ToString(double value)
		{
			if (base.TypeCode == XmlTypeCode.Float)
			{
				return XmlConvert.ToString(this.ToSingle(value));
			}
			return XmlConvert.ToString(value);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0008C45F File Offset: 0x0008B45F
		public override string ToString(float value)
		{
			if (base.TypeCode == XmlTypeCode.Float)
			{
				return XmlConvert.ToString(value);
			}
			return XmlConvert.ToString((double)value);
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0008C47B File Offset: 0x0008B47B
		public override string ToString(string value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return value;
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0008C48C File Offset: 0x0008B48C
		public override string ToString(object value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DoubleType)
			{
				return this.ToString((double)value);
			}
			if (type == XmlBaseConverter.SingleType)
			{
				return this.ToString((float)value);
			}
			if (type == XmlBaseConverter.StringType)
			{
				return (string)value;
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).Value;
			}
			return (string)this.ChangeListType(value, XmlBaseConverter.StringType, nsResolver);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0008C510 File Offset: 0x0008B510
		public override object ChangeType(double value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DoubleType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.SingleType)
			{
				return (float)value;
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
			return this.ChangeListType(value, destinationType, null);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0008C5A8 File Offset: 0x0008B5A8
		public override object ChangeType(float value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DoubleType)
			{
				return (double)value;
			}
			if (destinationType == XmlBaseConverter.SingleType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return this.ToString(value);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				return new XmlAtomicValue(base.SchemaType, (double)value);
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return new XmlAtomicValue(base.SchemaType, (double)value);
			}
			return this.ChangeListType(value, destinationType, null);
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0008C640 File Offset: 0x0008B640
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
			if (destinationType == XmlBaseConverter.DoubleType)
			{
				return this.ToDouble(value);
			}
			if (destinationType == XmlBaseConverter.SingleType)
			{
				return this.ToSingle(value);
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
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0008C6E0 File Offset: 0x0008B6E0
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
			if (destinationType == XmlBaseConverter.DoubleType)
			{
				return this.ToDouble(value);
			}
			if (destinationType == XmlBaseConverter.SingleType)
			{
				return this.ToSingle(value);
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return this.ToString(value, nsResolver);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				if (type == XmlBaseConverter.DoubleType)
				{
					return new XmlAtomicValue(base.SchemaType, (double)value);
				}
				if (type == XmlBaseConverter.SingleType)
				{
					return new XmlAtomicValue(base.SchemaType, value);
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
				if (type == XmlBaseConverter.DoubleType)
				{
					return new XmlAtomicValue(base.SchemaType, (double)value);
				}
				if (type == XmlBaseConverter.SingleType)
				{
					return new XmlAtomicValue(base.SchemaType, value);
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
			return this.ChangeListType(value, destinationType, nsResolver);
		}
	}
}
