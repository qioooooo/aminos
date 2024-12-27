using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000150 RID: 336
	internal sealed class OperandQuery : ValueQuery
	{
		// Token: 0x060012A9 RID: 4777 RVA: 0x0005126A File Offset: 0x0005026A
		public OperandQuery(object val)
		{
			this.val = val;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00051279 File Offset: 0x00050279
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			return this.val;
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x00051281 File Offset: 0x00050281
		public override XPathResultType StaticType
		{
			get
			{
				return base.GetXPathType(this.val);
			}
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0005128F File Offset: 0x0005028F
		public override XPathNodeIterator Clone()
		{
			return this;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00051292 File Offset: 0x00050292
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("value", Convert.ToString(this.val, CultureInfo.InvariantCulture));
			w.WriteEndElement();
		}

		// Token: 0x04000BA6 RID: 2982
		internal object val;
	}
}
