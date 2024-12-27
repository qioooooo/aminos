using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000296 RID: 662
	internal class XmlAnyListConverter : XmlListConverter
	{
		// Token: 0x06001FA5 RID: 8101 RVA: 0x0008F8E7 File Offset: 0x0008E8E7
		protected XmlAnyListConverter(XmlBaseConverter atomicConverter)
			: base(atomicConverter)
		{
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0008F8F0 File Offset: 0x0008E8F0
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
			if (!(value is IEnumerable) || value.GetType() == XmlBaseConverter.StringType || value.GetType() == XmlBaseConverter.ByteArrayType)
			{
				value = new object[] { value };
			}
			return this.ChangeListType(value, destinationType, nsResolver);
		}

		// Token: 0x040012AB RID: 4779
		public static readonly XmlValueConverter ItemList = new XmlAnyListConverter((XmlBaseConverter)XmlAnyConverter.Item);

		// Token: 0x040012AC RID: 4780
		public static readonly XmlValueConverter AnyAtomicList = new XmlAnyListConverter((XmlBaseConverter)XmlAnyConverter.AnyAtomic);
	}
}
