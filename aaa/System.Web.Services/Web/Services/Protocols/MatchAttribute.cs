using System;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200004C RID: 76
	[AttributeUsage(AttributeTargets.All)]
	public sealed class MatchAttribute : Attribute
	{
		// Token: 0x060001AA RID: 426 RVA: 0x0000752A File Offset: 0x0000652A
		public MatchAttribute(string pattern)
		{
			this.pattern = pattern;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007547 File Offset: 0x00006547
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0000755D File Offset: 0x0000655D
		public string Pattern
		{
			get
			{
				if (this.pattern != null)
				{
					return this.pattern;
				}
				return string.Empty;
			}
			set
			{
				this.pattern = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00007566 File Offset: 0x00006566
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000756E File Offset: 0x0000656E
		public int Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00007577 File Offset: 0x00006577
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0000757F File Offset: 0x0000657F
		public int Capture
		{
			get
			{
				return this.capture;
			}
			set
			{
				this.capture = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00007588 File Offset: 0x00006588
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00007590 File Offset: 0x00006590
		public bool IgnoreCase
		{
			get
			{
				return this.ignoreCase;
			}
			set
			{
				this.ignoreCase = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00007599 File Offset: 0x00006599
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000075A1 File Offset: 0x000065A1
		public int MaxRepeats
		{
			get
			{
				return this.repeats;
			}
			set
			{
				this.repeats = value;
			}
		}

		// Token: 0x040002AA RID: 682
		private string pattern;

		// Token: 0x040002AB RID: 683
		private int group = 1;

		// Token: 0x040002AC RID: 684
		private int capture;

		// Token: 0x040002AD RID: 685
		private bool ignoreCase;

		// Token: 0x040002AE RID: 686
		private int repeats = -1;
	}
}
