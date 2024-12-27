using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000318 RID: 792
	internal class RecursionLimiter
	{
		// Token: 0x06002586 RID: 9606 RVA: 0x000B34E5 File Offset: 0x000B24E5
		internal RecursionLimiter()
		{
			this.depth = 0;
			this.maxDepth = (DiagnosticsSwitches.NonRecursiveTypeLoading.Enabled ? 1 : int.MaxValue);
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x000B350E File Offset: 0x000B250E
		internal bool IsExceededLimit
		{
			get
			{
				return this.depth > this.maxDepth;
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06002588 RID: 9608 RVA: 0x000B351E File Offset: 0x000B251E
		// (set) Token: 0x06002589 RID: 9609 RVA: 0x000B3526 File Offset: 0x000B2526
		internal int Depth
		{
			get
			{
				return this.depth;
			}
			set
			{
				this.depth = value;
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x0600258A RID: 9610 RVA: 0x000B352F File Offset: 0x000B252F
		internal WorkItems DeferredWorkItems
		{
			get
			{
				if (this.deferredWorkItems == null)
				{
					this.deferredWorkItems = new WorkItems();
				}
				return this.deferredWorkItems;
			}
		}

		// Token: 0x040015A5 RID: 5541
		private int maxDepth;

		// Token: 0x040015A6 RID: 5542
		private int depth;

		// Token: 0x040015A7 RID: 5543
		private WorkItems deferredWorkItems;
	}
}
