using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020003D2 RID: 978
	internal class ListenerPrefixEnumerator : IEnumerator<string>, IDisposable, IEnumerator
	{
		// Token: 0x06001EDE RID: 7902 RVA: 0x000777E2 File Offset: 0x000767E2
		internal ListenerPrefixEnumerator(IEnumerator enumerator)
		{
			this.enumerator = enumerator;
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000777F1 File Offset: 0x000767F1
		public string Current
		{
			get
			{
				return (string)this.enumerator.Current;
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x00077803 File Offset: 0x00076803
		public bool MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x00077810 File Offset: 0x00076810
		public void Dispose()
		{
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x00077812 File Offset: 0x00076812
		void IEnumerator.Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x0007781F File Offset: 0x0007681F
		object IEnumerator.Current
		{
			get
			{
				return this.enumerator.Current;
			}
		}

		// Token: 0x04001E7F RID: 7807
		private IEnumerator enumerator;
	}
}
