using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200012E RID: 302
	internal abstract class CacheOutputQuery : Query
	{
		// Token: 0x060011A7 RID: 4519 RVA: 0x0004E4F4 File Offset: 0x0004D4F4
		public CacheOutputQuery(Query input)
		{
			this.input = input;
			this.outputBuffer = new List<XPathNavigator>();
			this.count = 0;
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0004E515 File Offset: 0x0004D515
		protected CacheOutputQuery(CacheOutputQuery other)
			: base(other)
		{
			this.input = Query.Clone(other.input);
			this.outputBuffer = new List<XPathNavigator>(other.outputBuffer);
			this.count = other.count;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0004E54C File Offset: 0x0004D54C
		public override void Reset()
		{
			this.count = 0;
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004E555 File Offset: 0x0004D555
		public override void SetXsltContext(XsltContext context)
		{
			this.input.SetXsltContext(context);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0004E563 File Offset: 0x0004D563
		public override object Evaluate(XPathNodeIterator context)
		{
			this.outputBuffer.Clear();
			this.count = 0;
			return this.input.Evaluate(context);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0004E584 File Offset: 0x0004D584
		public override XPathNavigator Advance()
		{
			if (this.count < this.outputBuffer.Count)
			{
				return this.outputBuffer[this.count++];
			}
			return null;
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x0004E5C2 File Offset: 0x0004D5C2
		public override XPathNavigator Current
		{
			get
			{
				if (this.count == 0)
				{
					return null;
				}
				return this.outputBuffer[this.count - 1];
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x0004E5E1 File Offset: 0x0004D5E1
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x0004E5E4 File Offset: 0x0004D5E4
		public override int CurrentPosition
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0004E5EC File Offset: 0x0004D5EC
		public override int Count
		{
			get
			{
				return this.outputBuffer.Count;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0004E5F9 File Offset: 0x0004D5F9
		public override QueryProps Properties
		{
			get
			{
				return (QueryProps)23;
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0004E5FD File Offset: 0x0004D5FD
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			this.input.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x04000B48 RID: 2888
		internal Query input;

		// Token: 0x04000B49 RID: 2889
		protected List<XPathNavigator> outputBuffer;
	}
}
