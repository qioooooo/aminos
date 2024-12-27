using System;
using System.Collections;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000A7 RID: 167
	public sealed class DiscoveryReferenceCollection : CollectionBase
	{
		// Token: 0x17000130 RID: 304
		public DiscoveryReference this[int i]
		{
			get
			{
				return (DiscoveryReference)base.List[i];
			}
			set
			{
				base.List[i] = value;
			}
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000165EE File Offset: 0x000155EE
		public int Add(DiscoveryReference value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000165FC File Offset: 0x000155FC
		public bool Contains(DiscoveryReference value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001660A File Offset: 0x0001560A
		public void Remove(DiscoveryReference value)
		{
			base.List.Remove(value);
		}
	}
}
