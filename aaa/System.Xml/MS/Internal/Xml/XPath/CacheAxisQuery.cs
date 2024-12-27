using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200012B RID: 299
	internal abstract class CacheAxisQuery : BaseAxisQuery
	{
		// Token: 0x06001191 RID: 4497 RVA: 0x0004E08B File Offset: 0x0004D08B
		public CacheAxisQuery(Query qyInput, string name, string prefix, XPathNodeType typeTest)
			: base(qyInput, name, prefix, typeTest)
		{
			this.outputBuffer = new List<XPathNavigator>();
			this.count = 0;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0004E0AA File Offset: 0x0004D0AA
		protected CacheAxisQuery(CacheAxisQuery other)
			: base(other)
		{
			this.outputBuffer = new List<XPathNavigator>(other.outputBuffer);
			this.count = other.count;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004E0D0 File Offset: 0x0004D0D0
		public override void Reset()
		{
			this.count = 0;
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0004E0D9 File Offset: 0x0004D0D9
		public override object Evaluate(XPathNodeIterator context)
		{
			base.Evaluate(context);
			this.outputBuffer.Clear();
			return this;
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0004E0F0 File Offset: 0x0004D0F0
		public override XPathNavigator Advance()
		{
			if (this.count < this.outputBuffer.Count)
			{
				return this.outputBuffer[this.count++];
			}
			return null;
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0004E12E File Offset: 0x0004D12E
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

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x0004E14D File Offset: 0x0004D14D
		public override int CurrentPosition
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x0004E155 File Offset: 0x0004D155
		public override int Count
		{
			get
			{
				return this.outputBuffer.Count;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x0004E162 File Offset: 0x0004D162
		public override QueryProps Properties
		{
			get
			{
				return (QueryProps)23;
			}
		}

		// Token: 0x04000B42 RID: 2882
		protected List<XPathNavigator> outputBuffer;
	}
}
