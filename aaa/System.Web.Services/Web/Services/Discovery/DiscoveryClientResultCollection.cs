using System;
using System.Collections;

namespace System.Web.Services.Discovery
{
	// Token: 0x0200009C RID: 156
	public sealed class DiscoveryClientResultCollection : CollectionBase
	{
		// Token: 0x1700011F RID: 287
		public DiscoveryClientResult this[int i]
		{
			get
			{
				return (DiscoveryClientResult)base.List[i];
			}
			set
			{
				base.List[i] = value;
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00014C95 File Offset: 0x00013C95
		public int Add(DiscoveryClientResult value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00014CA3 File Offset: 0x00013CA3
		public bool Contains(DiscoveryClientResult value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00014CB1 File Offset: 0x00013CB1
		public void Remove(DiscoveryClientResult value)
		{
			base.List.Remove(value);
		}
	}
}
