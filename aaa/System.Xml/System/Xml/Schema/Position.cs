using System;

namespace System.Xml.Schema
{
	// Token: 0x02000193 RID: 403
	internal struct Position
	{
		// Token: 0x06001542 RID: 5442 RVA: 0x0005EDA8 File Offset: 0x0005DDA8
		public Position(int symbol, object particle)
		{
			this.symbol = symbol;
			this.particle = particle;
		}

		// Token: 0x04000CC1 RID: 3265
		public int symbol;

		// Token: 0x04000CC2 RID: 3266
		public object particle;
	}
}
