using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000377 RID: 887
	internal class DataGridViewIntLinkedListEnumerator : IEnumerator
	{
		// Token: 0x0600365E RID: 13918 RVA: 0x000C2510 File Offset: 0x000C1510
		public DataGridViewIntLinkedListEnumerator(DataGridViewIntLinkedListElement headElement)
		{
			this.headElement = headElement;
			this.reset = true;
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x0600365F RID: 13919 RVA: 0x000C2526 File Offset: 0x000C1526
		object IEnumerator.Current
		{
			get
			{
				return this.current.Int;
			}
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x000C2538 File Offset: 0x000C1538
		bool IEnumerator.MoveNext()
		{
			if (this.reset)
			{
				this.current = this.headElement;
				this.reset = false;
			}
			else
			{
				this.current = this.current.Next;
			}
			return this.current != null;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000C2574 File Offset: 0x000C1574
		void IEnumerator.Reset()
		{
			this.reset = true;
			this.current = null;
		}

		// Token: 0x04001BD4 RID: 7124
		private DataGridViewIntLinkedListElement headElement;

		// Token: 0x04001BD5 RID: 7125
		private DataGridViewIntLinkedListElement current;

		// Token: 0x04001BD6 RID: 7126
		private bool reset;
	}
}
