using System;
using System.Xml.XPath;

namespace System.Xml.Schema
{
	// Token: 0x02000290 RID: 656
	internal class XmlMiscConverter : XmlBaseConverter
	{
		// Token: 0x06001F46 RID: 8006 RVA: 0x0008D235 File Offset: 0x0008C235
		protected XmlMiscConverter(XmlSchemaType schemaType)
			: base(schemaType)
		{
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0008D23E File Offset: 0x0008C23E
		public static XmlValueConverter Create(XmlSchemaType schemaType)
		{
			return new XmlMiscConverter(schemaType);
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0008D246 File Offset: 0x0008C246
		public override string ToString(string value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return value;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0008D258 File Offset: 0x0008C258
		public override string ToString(object value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.ByteArrayType)
			{
				switch (base.TypeCode)
				{
				case XmlTypeCode.HexBinary:
					return XmlConvert.ToBinHexString((byte[])value);
				case XmlTypeCode.Base64Binary:
					return XmlBaseConverter.Base64BinaryToString((byte[])value);
				}
			}
			if (type == XmlBaseConverter.StringType)
			{
				return (string)value;
			}
			if (XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.UriType) && base.TypeCode == XmlTypeCode.AnyUri)
			{
				return XmlBaseConverter.AnyUriToString((Uri)value);
			}
			if (type == XmlBaseConverter.TimeSpanType)
			{
				XmlTypeCode typeCode = base.TypeCode;
				if (typeCode == XmlTypeCode.Duration)
				{
					return XmlBaseConverter.DurationToString((TimeSpan)value);
				}
				switch (typeCode)
				{
				case XmlTypeCode.YearMonthDuration:
					return XmlBaseConverter.YearMonthDurationToString((TimeSpan)value);
				case XmlTypeCode.DayTimeDuration:
					return XmlBaseConverter.DayTimeDurationToString((TimeSpan)value);
				}
			}
			if (XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XmlQualifiedNameType))
			{
				switch (base.TypeCode)
				{
				case XmlTypeCode.QName:
					return XmlBaseConverter.QNameToString((XmlQualifiedName)value, nsResolver);
				case XmlTypeCode.Notation:
					return XmlBaseConverter.QNameToString((XmlQualifiedName)value, nsResolver);
				}
			}
			return (string)this.ChangeTypeWildcardDestination(value, XmlBaseConverter.StringType, nsResolver);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0008D384 File Offset: 0x0008C384
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
			if (destinationType == XmlBaseConverter.ByteArrayType)
			{
				switch (base.TypeCode)
				{
				case XmlTypeCode.HexBinary:
					return XmlBaseConverter.StringToHexBinary(value);
				case XmlTypeCode.Base64Binary:
					return XmlBaseConverter.StringToBase64Binary(value);
				}
			}
			if (destinationType == XmlBaseConverter.XmlQualifiedNameType)
			{
				switch (base.TypeCode)
				{
				case XmlTypeCode.QName:
					return XmlBaseConverter.StringToQName(value, nsResolver);
				case XmlTypeCode.Notation:
					return XmlBaseConverter.StringToQName(value, nsResolver);
				}
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.TimeSpanType)
			{
				XmlTypeCode typeCode = base.TypeCode;
				if (typeCode == XmlTypeCode.Duration)
				{
					return XmlBaseConverter.StringToDuration(value);
				}
				switch (typeCode)
				{
				case XmlTypeCode.YearMonthDuration:
					return XmlBaseConverter.StringToYearMonthDuration(value);
				case XmlTypeCode.DayTimeDuration:
					return XmlBaseConverter.StringToDayTimeDuration(value);
				}
			}
			if (destinationType == XmlBaseConverter.UriType && base.TypeCode == XmlTypeCode.AnyUri)
			{
				return XmlConvert.ToUri(value);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				return new XmlAtomicValue(base.SchemaType, value, nsResolver);
			}
			return this.ChangeTypeWildcardSource(value, destinationType, nsResolver);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0008D4AC File Offset: 0x0008C4AC
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
			if (destinationType == XmlBaseConverter.ByteArrayType)
			{
				if (type == XmlBaseConverter.ByteArrayType)
				{
					switch (base.TypeCode)
					{
					case XmlTypeCode.HexBinary:
						return (byte[])value;
					case XmlTypeCode.Base64Binary:
						return (byte[])value;
					}
				}
				if (type == XmlBaseConverter.StringType)
				{
					switch (base.TypeCode)
					{
					case XmlTypeCode.HexBinary:
						return XmlBaseConverter.StringToHexBinary((string)value);
					case XmlTypeCode.Base64Binary:
						return XmlBaseConverter.StringToBase64Binary((string)value);
					}
				}
			}
			if (destinationType == XmlBaseConverter.XmlQualifiedNameType)
			{
				if (type == XmlBaseConverter.StringType)
				{
					switch (base.TypeCode)
					{
					case XmlTypeCode.QName:
						return XmlBaseConverter.StringToQName((string)value, nsResolver);
					case XmlTypeCode.Notation:
						return XmlBaseConverter.StringToQName((string)value, nsResolver);
					}
				}
				if (XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XmlQualifiedNameType))
				{
					switch (base.TypeCode)
					{
					case XmlTypeCode.QName:
						return (XmlQualifiedName)value;
					case XmlTypeCode.Notation:
						return (XmlQualifiedName)value;
					}
				}
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return this.ToString(value, nsResolver);
			}
			if (destinationType == XmlBaseConverter.TimeSpanType)
			{
				if (type == XmlBaseConverter.StringType)
				{
					XmlTypeCode typeCode = base.TypeCode;
					if (typeCode == XmlTypeCode.Duration)
					{
						return XmlBaseConverter.StringToDuration((string)value);
					}
					switch (typeCode)
					{
					case XmlTypeCode.YearMonthDuration:
						return XmlBaseConverter.StringToYearMonthDuration((string)value);
					case XmlTypeCode.DayTimeDuration:
						return XmlBaseConverter.StringToDayTimeDuration((string)value);
					}
				}
				if (type == XmlBaseConverter.TimeSpanType)
				{
					XmlTypeCode typeCode2 = base.TypeCode;
					if (typeCode2 == XmlTypeCode.Duration)
					{
						return (TimeSpan)value;
					}
					switch (typeCode2)
					{
					case XmlTypeCode.YearMonthDuration:
						return (TimeSpan)value;
					case XmlTypeCode.DayTimeDuration:
						return (TimeSpan)value;
					}
				}
			}
			if (destinationType == XmlBaseConverter.UriType)
			{
				if (type == XmlBaseConverter.StringType && base.TypeCode == XmlTypeCode.AnyUri)
				{
					return XmlConvert.ToUri((string)value);
				}
				if (XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.UriType) && base.TypeCode == XmlTypeCode.AnyUri)
				{
					return (Uri)value;
				}
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				if (type == XmlBaseConverter.ByteArrayType)
				{
					switch (base.TypeCode)
					{
					case XmlTypeCode.HexBinary:
						return new XmlAtomicValue(base.SchemaType, value);
					case XmlTypeCode.Base64Binary:
						return new XmlAtomicValue(base.SchemaType, value);
					}
				}
				if (type == XmlBaseConverter.StringType)
				{
					return new XmlAtomicValue(base.SchemaType, (string)value, nsResolver);
				}
				if (type == XmlBaseConverter.TimeSpanType)
				{
					XmlTypeCode typeCode3 = base.TypeCode;
					if (typeCode3 == XmlTypeCode.Duration)
					{
						return new XmlAtomicValue(base.SchemaType, value);
					}
					switch (typeCode3)
					{
					case XmlTypeCode.YearMonthDuration:
						return new XmlAtomicValue(base.SchemaType, value);
					case XmlTypeCode.DayTimeDuration:
						return new XmlAtomicValue(base.SchemaType, value);
					}
				}
				if (XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.UriType) && base.TypeCode == XmlTypeCode.AnyUri)
				{
					return new XmlAtomicValue(base.SchemaType, value);
				}
				if (type == XmlBaseConverter.XmlAtomicValueType)
				{
					return (XmlAtomicValue)value;
				}
				if (XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XmlQualifiedNameType))
				{
					switch (base.TypeCode)
					{
					case XmlTypeCode.QName:
						return new XmlAtomicValue(base.SchemaType, value, nsResolver);
					case XmlTypeCode.Notation:
						return new XmlAtomicValue(base.SchemaType, value, nsResolver);
					}
				}
			}
			if (destinationType == XmlBaseConverter.XPathItemType && type == XmlBaseConverter.XmlAtomicValueType)
			{
				return (XmlAtomicValue)value;
			}
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return (XPathItem)this.ChangeType(value, XmlBaseConverter.XmlAtomicValueType, nsResolver);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAs(destinationType, nsResolver);
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0008D864 File Offset: 0x0008C864
		private object ChangeTypeWildcardDestination(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			Type type = value.GetType();
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAs(destinationType, nsResolver);
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x0008D897 File Offset: 0x0008C897
		private object ChangeTypeWildcardSource(object value, Type destinationType, IXmlNamespaceResolver nsResolver)
		{
			if (destinationType == XmlBaseConverter.XPathItemType)
			{
				return (XPathItem)this.ChangeType(value, XmlBaseConverter.XmlAtomicValueType, nsResolver);
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}
	}
}
