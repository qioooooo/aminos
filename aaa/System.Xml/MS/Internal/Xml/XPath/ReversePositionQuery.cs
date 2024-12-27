using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200015A RID: 346
	internal sealed class ReversePositionQuery : ForwardPositionQuery
	{
		// Token: 0x060012E2 RID: 4834 RVA: 0x000523CC File Offset: 0x000513CC
		public ReversePositionQuery(Query input)
			: base(input)
		{
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x000523D5 File Offset: 0x000513D5
		private ReversePositionQuery(ReversePositionQuery other)
			: base(other)
		{
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x000523DE File Offset: 0x000513DE
		public override XPathNodeIterator Clone()
		{
			return new ReversePositionQuery(this);
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060012E5 RID: 4837 RVA: 0x000523E6 File Offset: 0x000513E6
		public override int CurrentPosition
		{
			get
			{
				return this.outputBuffer.Count - this.count + 1;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x000523FC File Offset: 0x000513FC
		public override QueryProps Properties
		{
			get
			{
				return base.Properties | QueryProps.Reverse;
			}
		}
	}
}
