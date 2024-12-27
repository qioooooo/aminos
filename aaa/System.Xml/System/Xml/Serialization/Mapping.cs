using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C4 RID: 708
	internal abstract class Mapping
	{
		// Token: 0x060021A0 RID: 8608 RVA: 0x0009F16B File Offset: 0x0009E16B
		internal Mapping()
		{
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060021A1 RID: 8609 RVA: 0x0009F173 File Offset: 0x0009E173
		// (set) Token: 0x060021A2 RID: 8610 RVA: 0x0009F17B File Offset: 0x0009E17B
		internal bool IsSoap
		{
			get
			{
				return this.isSoap;
			}
			set
			{
				this.isSoap = value;
			}
		}

		// Token: 0x0400146D RID: 5229
		private bool isSoap;
	}
}
