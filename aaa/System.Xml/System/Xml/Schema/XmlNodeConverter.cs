using System;
using System.Xml.XPath;

namespace System.Xml.Schema
{
	// Token: 0x02000294 RID: 660
	internal class XmlNodeConverter : XmlBaseConverter
	{
		// Token: 0x06001F8B RID: 8075 RVA: 0x0008EE9C File Offset: 0x0008DE9C
		protected XmlNodeConverter()
			: base(XmlTypeCode.Node)
		{
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0008EEA8 File Offset: 0x0008DEA8
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
			if (destinationType == XmlBaseConverter.XPathNavigatorType && XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XPathNavigatorType))
			{
				return (XPathNavigator)value;
			}
			if (destinationType == XmlBaseConverter.XPathItemType && XmlBaseConverter.IsDerivedFrom(type, XmlBaseConverter.XPathNavigatorType))
			{
				return (XPathItem)value;
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x040012A8 RID: 4776
		public static readonly XmlValueConverter Node = new XmlNodeConverter();
	}
}
