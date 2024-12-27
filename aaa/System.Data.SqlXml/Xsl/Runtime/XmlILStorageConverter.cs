using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Schema;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A8 RID: 168
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class XmlILStorageConverter
	{
		// Token: 0x060007E9 RID: 2025 RVA: 0x000284D8 File Offset: 0x000274D8
		public static XmlAtomicValue StringToAtomicValue(string value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000284EC File Offset: 0x000274EC
		public static XmlAtomicValue DecimalToAtomicValue(decimal value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00028505 File Offset: 0x00027505
		public static XmlAtomicValue Int64ToAtomicValue(long value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00028519 File Offset: 0x00027519
		public static XmlAtomicValue Int32ToAtomicValue(int value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x0002852D File Offset: 0x0002752D
		public static XmlAtomicValue BooleanToAtomicValue(bool value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00028541 File Offset: 0x00027541
		public static XmlAtomicValue DoubleToAtomicValue(double value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00028555 File Offset: 0x00027555
		public static XmlAtomicValue SingleToAtomicValue(float value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, (double)value);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x0002856A File Offset: 0x0002756A
		public static XmlAtomicValue DateTimeToAtomicValue(DateTime value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0002857E File Offset: 0x0002757E
		public static XmlAtomicValue XmlQualifiedNameToAtomicValue(XmlQualifiedName value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00028592 File Offset: 0x00027592
		public static XmlAtomicValue TimeSpanToAtomicValue(TimeSpan value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000285AB File Offset: 0x000275AB
		public static XmlAtomicValue BytesToAtomicValue(byte[] value, int index, XmlQueryRuntime runtime)
		{
			return new XmlAtomicValue(runtime.GetXmlType(index).SchemaType, value);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x000285C0 File Offset: 0x000275C0
		public static IList<XPathItem> NavigatorsToItems(IList<XPathNavigator> listNavigators)
		{
			IList<XPathItem> list = listNavigators as IList<XPathItem>;
			if (list != null)
			{
				return list;
			}
			return new XmlQueryNodeSequence(listNavigators);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000285E0 File Offset: 0x000275E0
		public static IList<XPathNavigator> ItemsToNavigators(IList<XPathItem> listItems)
		{
			IList<XPathNavigator> list = listItems as IList<XPathNavigator>;
			if (list != null)
			{
				return list;
			}
			XmlQueryNodeSequence xmlQueryNodeSequence = new XmlQueryNodeSequence(listItems.Count);
			for (int i = 0; i < listItems.Count; i++)
			{
				xmlQueryNodeSequence.Add((XPathNavigator)listItems[i]);
			}
			return xmlQueryNodeSequence;
		}
	}
}
