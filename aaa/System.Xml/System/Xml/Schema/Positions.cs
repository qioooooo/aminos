using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000194 RID: 404
	internal class Positions
	{
		// Token: 0x06001543 RID: 5443 RVA: 0x0005EDB8 File Offset: 0x0005DDB8
		public int Add(int symbol, object particle)
		{
			return this.positions.Add(new Position(symbol, particle));
		}

		// Token: 0x1700051A RID: 1306
		public Position this[int pos]
		{
			get
			{
				return (Position)this.positions[pos];
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x0005EDE4 File Offset: 0x0005DDE4
		public int Count
		{
			get
			{
				return this.positions.Count;
			}
		}

		// Token: 0x04000CC3 RID: 3267
		private ArrayList positions = new ArrayList();
	}
}
