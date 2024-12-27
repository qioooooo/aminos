using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000318 RID: 792
	internal class DataGridViewCellLinkedListEnumerator : IEnumerator
	{
		// Token: 0x0600334F RID: 13135 RVA: 0x000B407F File Offset: 0x000B307F
		public DataGridViewCellLinkedListEnumerator(DataGridViewCellLinkedListElement headElement)
		{
			this.headElement = headElement;
			this.reset = true;
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x000B4095 File Offset: 0x000B3095
		object IEnumerator.Current
		{
			get
			{
				return this.current.DataGridViewCell;
			}
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x000B40A2 File Offset: 0x000B30A2
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

		// Token: 0x06003352 RID: 13138 RVA: 0x000B40DE File Offset: 0x000B30DE
		void IEnumerator.Reset()
		{
			this.reset = true;
			this.current = null;
		}

		// Token: 0x04001AB3 RID: 6835
		private DataGridViewCellLinkedListElement headElement;

		// Token: 0x04001AB4 RID: 6836
		private DataGridViewCellLinkedListElement current;

		// Token: 0x04001AB5 RID: 6837
		private bool reset;
	}
}
