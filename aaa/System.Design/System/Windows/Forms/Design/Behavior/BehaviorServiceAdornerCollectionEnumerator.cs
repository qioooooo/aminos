using System;
using System.Collections;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002E4 RID: 740
	public class BehaviorServiceAdornerCollectionEnumerator : IEnumerator
	{
		// Token: 0x06001C6F RID: 7279 RVA: 0x0009F8D8 File Offset: 0x0009E8D8
		public BehaviorServiceAdornerCollectionEnumerator(BehaviorServiceAdornerCollection mappings)
		{
			this.temp = mappings;
			this.baseEnumerator = this.temp.GetEnumerator();
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x0009F8F8 File Offset: 0x0009E8F8
		public Adorner Current
		{
			get
			{
				return (Adorner)this.baseEnumerator.Current;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001C71 RID: 7281 RVA: 0x0009F90A File Offset: 0x0009E90A
		object IEnumerator.Current
		{
			get
			{
				return this.baseEnumerator.Current;
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x0009F917 File Offset: 0x0009E917
		public bool MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x0009F924 File Offset: 0x0009E924
		bool IEnumerator.MoveNext()
		{
			return this.baseEnumerator.MoveNext();
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x0009F931 File Offset: 0x0009E931
		public void Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0009F93E File Offset: 0x0009E93E
		void IEnumerator.Reset()
		{
			this.baseEnumerator.Reset();
		}

		// Token: 0x040015E4 RID: 5604
		private IEnumerator baseEnumerator;

		// Token: 0x040015E5 RID: 5605
		private IEnumerable temp;
	}
}
