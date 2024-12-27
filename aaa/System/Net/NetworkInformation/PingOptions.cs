using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x02000627 RID: 1575
	public class PingOptions
	{
		// Token: 0x0600306E RID: 12398 RVA: 0x000D16E5 File Offset: 0x000D06E5
		internal PingOptions(IPOptions options)
		{
			this.ttl = (int)options.ttl;
			this.dontFragment = (options.flags & 2) > 0;
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000D171B File Offset: 0x000D071B
		public PingOptions(int ttl, bool dontFragment)
		{
			if (ttl <= 0)
			{
				throw new ArgumentOutOfRangeException("ttl");
			}
			this.ttl = ttl;
			this.dontFragment = dontFragment;
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000D174B File Offset: 0x000D074B
		public PingOptions()
		{
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x000D175E File Offset: 0x000D075E
		// (set) Token: 0x06003072 RID: 12402 RVA: 0x000D1766 File Offset: 0x000D0766
		public int Ttl
		{
			get
			{
				return this.ttl;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ttl = value;
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003073 RID: 12403 RVA: 0x000D177E File Offset: 0x000D077E
		// (set) Token: 0x06003074 RID: 12404 RVA: 0x000D1786 File Offset: 0x000D0786
		public bool DontFragment
		{
			get
			{
				return this.dontFragment;
			}
			set
			{
				this.dontFragment = value;
			}
		}

		// Token: 0x04002E1B RID: 11803
		private const int DontFragmentFlag = 2;

		// Token: 0x04002E1C RID: 11804
		private int ttl = 128;

		// Token: 0x04002E1D RID: 11805
		private bool dontFragment;
	}
}
