using System;

namespace System.Xml.Schema
{
	// Token: 0x0200028E RID: 654
	internal class XmlDateTimeConverter : XmlBaseConverter
	{
		// Token: 0x06001F29 RID: 7977 RVA: 0x0008C824 File Offset: 0x0008B824
		protected XmlDateTimeConverter(XmlSchemaType schemaType)
			: base(schemaType)
		{
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0008C82D File Offset: 0x0008B82D
		public static XmlValueConverter Create(XmlSchemaType schemaType)
		{
			return new XmlDateTimeConverter(schemaType);
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0008C835 File Offset: 0x0008B835
		public override DateTime ToDateTime(DateTime value)
		{
			return value;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0008C838 File Offset: 0x0008B838
		public override DateTime ToDateTime(DateTimeOffset value)
		{
			return XmlBaseConverter.DateTimeOffsetToDateTime(value);
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0008C840 File Offset: 0x0008B840
		public override DateTime ToDateTime(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			switch (base.TypeCode)
			{
			case XmlTypeCode.Time:
				return XmlBaseConverter.StringToTime(value);
			case XmlTypeCode.Date:
				return XmlBaseConverter.StringToDate(value);
			case XmlTypeCode.GYearMonth:
				return XmlBaseConverter.StringToGYearMonth(value);
			case XmlTypeCode.GYear:
				return XmlBaseConverter.StringToGYear(value);
			case XmlTypeCode.GMonthDay:
				return XmlBaseConverter.StringToGMonthDay(value);
			case XmlTypeCode.GDay:
				return XmlBaseConverter.StringToGDay(value);
			case XmlTypeCode.GMonth:
				return XmlBaseConverter.StringToGMonth(value);
			default:
				return XmlBaseConverter.StringToDateTime(value);
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x0008C8C0 File Offset: 0x0008B8C0
		public override DateTime ToDateTime(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DateTimeType)
			{
				return (DateTime)value;
			}
			if (type == XmlBaseConverter.DateTimeOffsetType)
			{
				return this.ToDateTime((DateTimeOffset)value);
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToDateTime((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAsDateTime;
			}
			return (DateTime)this.ChangeListType(value, XmlBaseConverter.DateTimeType, null);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0008C941 File Offset: 0x0008B941
		public override DateTimeOffset ToDateTimeOffset(DateTime value)
		{
			return new DateTimeOffset(value);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x0008C949 File Offset: 0x0008B949
		public override DateTimeOffset ToDateTimeOffset(DateTimeOffset value)
		{
			return value;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x0008C94C File Offset: 0x0008B94C
		public override DateTimeOffset ToDateTimeOffset(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			switch (base.TypeCode)
			{
			case XmlTypeCode.Time:
				return XmlBaseConverter.StringToTimeOffset(value);
			case XmlTypeCode.Date:
				return XmlBaseConverter.StringToDateOffset(value);
			case XmlTypeCode.GYearMonth:
				return XmlBaseConverter.StringToGYearMonthOffset(value);
			case XmlTypeCode.GYear:
				return XmlBaseConverter.StringToGYearOffset(value);
			case XmlTypeCode.GMonthDay:
				return XmlBaseConverter.StringToGMonthDayOffset(value);
			case XmlTypeCode.GDay:
				return XmlBaseConverter.StringToGDayOffset(value);
			case XmlTypeCode.GMonth:
				return XmlBaseConverter.StringToGMonthOffset(value);
			default:
				return XmlBaseConverter.StringToDateTimeOffset(value);
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x0008C9CC File Offset: 0x0008B9CC
		public override DateTimeOffset ToDateTimeOffset(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DateTimeType)
			{
				return this.ToDateTimeOffset((DateTime)value);
			}
			if (type == XmlBaseConverter.DateTimeOffsetType)
			{
				return (DateTimeOffset)value;
			}
			if (type == XmlBaseConverter.StringType)
			{
				return this.ToDateTimeOffset((string)value);
			}
			if (type == XmlBaseConverter.XmlAtomicValueType)
			{
				return ((XmlAtomicValue)value).ValueAsDateTime;
			}
			return (DateTimeOffset)this.ChangeListType(value, XmlBaseConverter.DateTimeOffsetType, null);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x0008CA54 File Offset: 0x0008BA54
		public override string ToString(DateTime value)
		{
			switch (base.TypeCode)
			{
			case XmlTypeCode.Time:
				return XmlBaseConverter.TimeToString(value);
			case XmlTypeCode.Date:
				return XmlBaseConverter.DateToString(value);
			case XmlTypeCode.GYearMonth:
				return XmlBaseConverter.GYearMonthToString(value);
			case XmlTypeCode.GYear:
				return XmlBaseConverter.GYearToString(value);
			case XmlTypeCode.GMonthDay:
				return XmlBaseConverter.GMonthDayToString(value);
			case XmlTypeCode.GDay:
				return XmlBaseConverter.GDayToString(value);
			case XmlTypeCode.GMonth:
				return XmlBaseConverter.GMonthToString(value);
			default:
				return XmlBaseConverter.DateTimeToString(value);
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x0008CAC8 File Offset: 0x0008BAC8
		public override string ToString(DateTimeOffset value)
		{
			switch (base.TypeCode)
			{
			case XmlTypeCode.Time:
				return XmlBaseConverter.TimeOffsetToString(value);
			case XmlTypeCode.Date:
				return XmlBaseConverter.DateOffsetToString(value);
			case XmlTypeCode.GYearMonth:
				return XmlBaseConverter.GYearMonthOffsetToString(value);
			case XmlTypeCode.GYear:
				return XmlBaseConverter.GYearOffsetToString(value);
			case XmlTypeCode.GMonthDay:
				return XmlBaseConverter.GMonthDayOffsetToString(value);
			case XmlTypeCode.GDay:
				return XmlBaseConverter.GDayOffsetToString(value);
			case XmlTypeCode.GMonth:
				return XmlBaseConverter.GMonthOffsetToString(value);
			default:
				return XmlBaseConverter.DateTimeOffsetToString(value);
			}
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x0008CB3A File Offset: 0x0008BB3A
		public override string ToString(string value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return value;
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x0008CB4C File Offset: 0x0008BB4C
		public override string ToString(object value, IXmlNamespaceResolver nsResolver)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			if (type == XmlBaseConverter.DateTimeType)
			{
				return this.ToString((DateTime)value);
			}
			if (type == XmlBaseConverter.DateTimeOffsetType)
			{
				return this.ToString((DateTimeOffset)value);
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

		// Token: 0x06001F37 RID: 7991 RVA: 0x0008CBD0 File Offset: 0x0008BBD0
		public override object ChangeType(DateTime value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DateTimeType)
			{
				return value;
			}
			if (destinationType == XmlBaseConverter.DateTimeOffsetType)
			{
				return this.ToDateTimeOffset(value);
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

		// Token: 0x06001F38 RID: 7992 RVA: 0x0008CC68 File Offset: 0x0008BC68
		public override object ChangeType(DateTimeOffset value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if (destinationType == XmlBaseConverter.ObjectType)
			{
				destinationType = base.DefaultClrType;
			}
			if (destinationType == XmlBaseConverter.DateTimeType)
			{
				return this.ToDateTime(value);
			}
			if (destinationType == XmlBaseConverter.DateTimeOffsetType)
			{
				return value;
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

		// Token: 0x06001F39 RID: 7993 RVA: 0x0008CD0C File Offset: 0x0008BD0C
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
			if (destinationType == XmlBaseConverter.DateTimeType)
			{
				return this.ToDateTime(value);
			}
			if (destinationType == XmlBaseConverter.DateTimeOffsetType)
			{
				return this.ToDateTimeOffset(value);
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

		// Token: 0x06001F3A RID: 7994 RVA: 0x0008CDAC File Offset: 0x0008BDAC
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
			if (destinationType == XmlBaseConverter.DateTimeType)
			{
				return this.ToDateTime(value);
			}
			if (destinationType == XmlBaseConverter.DateTimeOffsetType)
			{
				return this.ToDateTimeOffset(value);
			}
			if (destinationType == XmlBaseConverter.StringType)
			{
				return this.ToString(value, nsResolver);
			}
			if (destinationType == XmlBaseConverter.XmlAtomicValueType)
			{
				if (type == XmlBaseConverter.DateTimeType)
				{
					return new XmlAtomicValue(base.SchemaType, (DateTime)value);
				}
				if (type == XmlBaseConverter.DateTimeOffsetType)
				{
					return new XmlAtomicValue(base.SchemaType, (DateTimeOffset)value);
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
				if (type == XmlBaseConverter.DateTimeType)
				{
					return new XmlAtomicValue(base.SchemaType, (DateTime)value);
				}
				if (type == XmlBaseConverter.DateTimeOffsetType)
				{
					return new XmlAtomicValue(base.SchemaType, (DateTimeOffset)value);
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
