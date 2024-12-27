using System;

namespace System.IO.Compression
{
	// Token: 0x02000208 RID: 520
	internal class Match
	{
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060011AD RID: 4525 RVA: 0x0003A0D7 File Offset: 0x000390D7
		// (set) Token: 0x060011AE RID: 4526 RVA: 0x0003A0DF File Offset: 0x000390DF
		internal MatchState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x0003A0E8 File Offset: 0x000390E8
		// (set) Token: 0x060011B0 RID: 4528 RVA: 0x0003A0F0 File Offset: 0x000390F0
		internal int Position
		{
			get
			{
				return this.pos;
			}
			set
			{
				this.pos = value;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0003A0F9 File Offset: 0x000390F9
		// (set) Token: 0x060011B2 RID: 4530 RVA: 0x0003A101 File Offset: 0x00039101
		internal int Length
		{
			get
			{
				return this.len;
			}
			set
			{
				this.len = value;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x0003A10A File Offset: 0x0003910A
		// (set) Token: 0x060011B4 RID: 4532 RVA: 0x0003A112 File Offset: 0x00039112
		internal byte Symbol
		{
			get
			{
				return this.symbol;
			}
			set
			{
				this.symbol = value;
			}
		}

		// Token: 0x04000FEC RID: 4076
		private MatchState state;

		// Token: 0x04000FED RID: 4077
		private int pos;

		// Token: 0x04000FEE RID: 4078
		private int len;

		// Token: 0x04000FEF RID: 4079
		private byte symbol;
	}
}
