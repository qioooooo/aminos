using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000CB RID: 203
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class XsltConvert
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x0002CD48 File Offset: 0x0002BD48
		public static bool ToBoolean(XPathItem item)
		{
			if (item.IsNode)
			{
				return true;
			}
			Type valueType = item.ValueType;
			if (valueType == XsltConvert.StringType)
			{
				return item.Value.Length != 0;
			}
			if (valueType == XsltConvert.DoubleType)
			{
				double valueAsDouble = item.ValueAsDouble;
				return valueAsDouble < 0.0 || 0.0 < valueAsDouble;
			}
			return item.ValueAsBoolean;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0002CDB0 File Offset: 0x0002BDB0
		public static bool ToBoolean(IList<XPathItem> listItems)
		{
			return listItems.Count != 0 && XsltConvert.ToBoolean(listItems[0]);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0002CDC8 File Offset: 0x0002BDC8
		public static double ToDouble(string value)
		{
			return XPathConvert.StringToDouble(value);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0002CDD0 File Offset: 0x0002BDD0
		public static double ToDouble(XPathItem item)
		{
			if (item.IsNode)
			{
				return XPathConvert.StringToDouble(item.Value);
			}
			Type valueType = item.ValueType;
			if (valueType == XsltConvert.StringType)
			{
				return XPathConvert.StringToDouble(item.Value);
			}
			if (valueType == XsltConvert.DoubleType)
			{
				return item.ValueAsDouble;
			}
			if (!item.ValueAsBoolean)
			{
				return 0.0;
			}
			return 1.0;
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002CE36 File Offset: 0x0002BE36
		public static double ToDouble(IList<XPathItem> listItems)
		{
			if (listItems.Count == 0)
			{
				return double.NaN;
			}
			return XsltConvert.ToDouble(listItems[0]);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002CE58 File Offset: 0x0002BE58
		public static XPathNavigator ToNode(XPathItem item)
		{
			if (!item.IsNode)
			{
				XPathDocument xpathDocument = new XPathDocument();
				XmlRawWriter xmlRawWriter = xpathDocument.LoadFromWriter(XPathDocument.LoadFlags.AtomizeNames, string.Empty);
				xmlRawWriter.WriteString(XsltConvert.ToString(item));
				xmlRawWriter.Close();
				return xpathDocument.CreateNavigator();
			}
			RtfNavigator rtfNavigator = item as RtfNavigator;
			if (rtfNavigator != null)
			{
				return rtfNavigator.ToNavigator();
			}
			return (XPathNavigator)item;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002CEB0 File Offset: 0x0002BEB0
		public static XPathNavigator ToNode(IList<XPathItem> listItems)
		{
			if (listItems.Count == 1)
			{
				return XsltConvert.ToNode(listItems[0]);
			}
			throw new XslTransformException("Xslt_NodeSetNotNode", new string[] { string.Empty });
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002CEED File Offset: 0x0002BEED
		public static IList<XPathNavigator> ToNodeSet(XPathItem item)
		{
			return new XmlQueryNodeSequence(XsltConvert.ToNode(item));
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002CEFA File Offset: 0x0002BEFA
		public static IList<XPathNavigator> ToNodeSet(IList<XPathItem> listItems)
		{
			if (listItems.Count == 1)
			{
				return new XmlQueryNodeSequence(XsltConvert.ToNode(listItems[0]));
			}
			return XmlILStorageConverter.ItemsToNavigators(listItems);
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002CF1D File Offset: 0x0002BF1D
		public static string ToString(double value)
		{
			return XPathConvert.DoubleToString(value);
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002CF25 File Offset: 0x0002BF25
		public static string ToString(XPathItem item)
		{
			if (!item.IsNode && item.ValueType == XsltConvert.DoubleType)
			{
				return XPathConvert.DoubleToString(item.ValueAsDouble);
			}
			return item.Value;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002CF4E File Offset: 0x0002BF4E
		public static string ToString(IList<XPathItem> listItems)
		{
			if (listItems.Count == 0)
			{
				return string.Empty;
			}
			return XsltConvert.ToString(listItems[0]);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002CF6C File Offset: 0x0002BF6C
		public static string ToString(DateTime value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.DateTime).ToString();
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002CF8E File Offset: 0x0002BF8E
		public static double ToDouble(decimal value)
		{
			return (double)value;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0002CF97 File Offset: 0x0002BF97
		public static double ToDouble(int value)
		{
			return (double)value;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0002CF9B File Offset: 0x0002BF9B
		public static double ToDouble(long value)
		{
			return (double)value;
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0002CF9F File Offset: 0x0002BF9F
		public static decimal ToDecimal(double value)
		{
			return (decimal)value;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002CFA8 File Offset: 0x0002BFA8
		public static int ToInt(double value)
		{
			return checked((int)value);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002CFAC File Offset: 0x0002BFAC
		public static long ToLong(double value)
		{
			return checked((long)value);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002CFB0 File Offset: 0x0002BFB0
		public static DateTime ToDateTime(string value)
		{
			return new XsdDateTime(value, XsdDateTimeFlags.AllXsd);
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002CFC4 File Offset: 0x0002BFC4
		internal static XmlAtomicValue ConvertToType(XmlAtomicValue value, XmlQueryType destinationType)
		{
			XmlTypeCode typeCode = destinationType.TypeCode;
			switch (typeCode)
			{
			case XmlTypeCode.String:
				switch (value.XmlType.TypeCode)
				{
				case XmlTypeCode.String:
				case XmlTypeCode.Boolean:
				case XmlTypeCode.Double:
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToString(value));
				case XmlTypeCode.DateTime:
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToString(value.ValueAsDateTime));
				}
				break;
			case XmlTypeCode.Boolean:
				switch (value.XmlType.TypeCode)
				{
				case XmlTypeCode.String:
				case XmlTypeCode.Boolean:
				case XmlTypeCode.Double:
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToBoolean(value));
				}
				break;
			case XmlTypeCode.Decimal:
				if (value.XmlType.TypeCode == XmlTypeCode.Double)
				{
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToDecimal(value.ValueAsDouble));
				}
				break;
			case XmlTypeCode.Float:
			case XmlTypeCode.Duration:
				break;
			case XmlTypeCode.Double:
			{
				XmlTypeCode typeCode2 = value.XmlType.TypeCode;
				switch (typeCode2)
				{
				case XmlTypeCode.String:
				case XmlTypeCode.Boolean:
				case XmlTypeCode.Double:
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToDouble(value));
				case XmlTypeCode.Decimal:
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToDouble((decimal)value.ValueAs(XsltConvert.DecimalType, null)));
				case XmlTypeCode.Float:
					break;
				default:
					switch (typeCode2)
					{
					case XmlTypeCode.Long:
					case XmlTypeCode.Int:
						return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToDouble(value.ValueAsLong));
					}
					break;
				}
				break;
			}
			case XmlTypeCode.DateTime:
				if (value.XmlType.TypeCode == XmlTypeCode.String)
				{
					return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToDateTime(value.Value));
				}
				break;
			default:
				switch (typeCode)
				{
				case XmlTypeCode.Long:
				case XmlTypeCode.Int:
					if (value.XmlType.TypeCode == XmlTypeCode.Double)
					{
						return new XmlAtomicValue(destinationType.SchemaType, XsltConvert.ToLong(value.ValueAsDouble));
					}
					break;
				}
				break;
			}
			return value;
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002D1B8 File Offset: 0x0002C1B8
		public static IList<XPathNavigator> EnsureNodeSet(IList<XPathItem> listItems)
		{
			if (listItems.Count == 1)
			{
				XPathItem xpathItem = listItems[0];
				if (!xpathItem.IsNode)
				{
					throw new XslTransformException("XPath_NodeSetExpected", new string[] { string.Empty });
				}
				if (xpathItem is RtfNavigator)
				{
					throw new XslTransformException("XPath_RtfInPathExpr", new string[] { string.Empty });
				}
			}
			return XmlILStorageConverter.ItemsToNavigators(listItems);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0002D224 File Offset: 0x0002C224
		internal static XmlQueryType InferXsltType(Type clrType)
		{
			if (clrType == XsltConvert.BooleanType)
			{
				return XmlQueryTypeFactory.BooleanX;
			}
			if (clrType == XsltConvert.ByteType)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.DecimalType)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.DateTimeType)
			{
				return XmlQueryTypeFactory.StringX;
			}
			if (clrType == XsltConvert.DoubleType)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.Int16Type)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.Int32Type)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.Int64Type)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.IXPathNavigableType)
			{
				return XmlQueryTypeFactory.NodeNotRtf;
			}
			if (clrType == XsltConvert.SByteType)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.SingleType)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.StringType)
			{
				return XmlQueryTypeFactory.StringX;
			}
			if (clrType == XsltConvert.UInt16Type)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.UInt32Type)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.UInt64Type)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.XPathNavigatorArrayType)
			{
				return XmlQueryTypeFactory.NodeDodS;
			}
			if (clrType == XsltConvert.XPathNavigatorType)
			{
				return XmlQueryTypeFactory.NodeNotRtf;
			}
			if (clrType == XsltConvert.XPathNodeIteratorType)
			{
				return XmlQueryTypeFactory.NodeDodS;
			}
			if (clrType.IsEnum)
			{
				return XmlQueryTypeFactory.DoubleX;
			}
			if (clrType == XsltConvert.VoidType)
			{
				return XmlQueryTypeFactory.Empty;
			}
			return XmlQueryTypeFactory.ItemS;
		}

		// Token: 0x040005DA RID: 1498
		internal static readonly Type BooleanType = typeof(bool);

		// Token: 0x040005DB RID: 1499
		internal static readonly Type ByteArrayType = typeof(byte[]);

		// Token: 0x040005DC RID: 1500
		internal static readonly Type ByteType = typeof(byte);

		// Token: 0x040005DD RID: 1501
		internal static readonly Type DateTimeType = typeof(DateTime);

		// Token: 0x040005DE RID: 1502
		internal static readonly Type DecimalType = typeof(decimal);

		// Token: 0x040005DF RID: 1503
		internal static readonly Type DoubleType = typeof(double);

		// Token: 0x040005E0 RID: 1504
		internal static readonly Type ICollectionType = typeof(ICollection);

		// Token: 0x040005E1 RID: 1505
		internal static readonly Type IEnumerableType = typeof(IEnumerable);

		// Token: 0x040005E2 RID: 1506
		internal static readonly Type IListType = typeof(IList);

		// Token: 0x040005E3 RID: 1507
		internal static readonly Type Int16Type = typeof(short);

		// Token: 0x040005E4 RID: 1508
		internal static readonly Type Int32Type = typeof(int);

		// Token: 0x040005E5 RID: 1509
		internal static readonly Type Int64Type = typeof(long);

		// Token: 0x040005E6 RID: 1510
		internal static readonly Type IXPathNavigableType = typeof(IXPathNavigable);

		// Token: 0x040005E7 RID: 1511
		internal static readonly Type ObjectType = typeof(object);

		// Token: 0x040005E8 RID: 1512
		internal static readonly Type SByteType = typeof(sbyte);

		// Token: 0x040005E9 RID: 1513
		internal static readonly Type SingleType = typeof(float);

		// Token: 0x040005EA RID: 1514
		internal static readonly Type StringType = typeof(string);

		// Token: 0x040005EB RID: 1515
		internal static readonly Type TimeSpanType = typeof(TimeSpan);

		// Token: 0x040005EC RID: 1516
		internal static readonly Type UInt16Type = typeof(ushort);

		// Token: 0x040005ED RID: 1517
		internal static readonly Type UInt32Type = typeof(uint);

		// Token: 0x040005EE RID: 1518
		internal static readonly Type UInt64Type = typeof(ulong);

		// Token: 0x040005EF RID: 1519
		internal static readonly Type UriType = typeof(Uri);

		// Token: 0x040005F0 RID: 1520
		internal static readonly Type VoidType = typeof(void);

		// Token: 0x040005F1 RID: 1521
		internal static readonly Type XmlAtomicValueType = typeof(XmlAtomicValue);

		// Token: 0x040005F2 RID: 1522
		internal static readonly Type XmlQualifiedNameType = typeof(XmlQualifiedName);

		// Token: 0x040005F3 RID: 1523
		internal static readonly Type XPathItemType = typeof(XPathItem);

		// Token: 0x040005F4 RID: 1524
		internal static readonly Type XPathNavigatorArrayType = typeof(XPathNavigator[]);

		// Token: 0x040005F5 RID: 1525
		internal static readonly Type XPathNavigatorType = typeof(XPathNavigator);

		// Token: 0x040005F6 RID: 1526
		internal static readonly Type XPathNodeIteratorType = typeof(XPathNodeIterator);
	}
}
