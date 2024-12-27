using System;
using System.Collections;

namespace System.Security
{
	// Token: 0x02000661 RID: 1633
	internal class PermissionSetEnumerator : IEnumerator
	{
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06003B8E RID: 15246 RVA: 0x000CC073 File Offset: 0x000CB073
		public object Current
		{
			get
			{
				return this.enm.Current;
			}
		}

		// Token: 0x06003B8F RID: 15247 RVA: 0x000CC080 File Offset: 0x000CB080
		public bool MoveNext()
		{
			return this.enm.MoveNext();
		}

		// Token: 0x06003B90 RID: 15248 RVA: 0x000CC08D File Offset: 0x000CB08D
		public void Reset()
		{
			this.enm.Reset();
		}

		// Token: 0x06003B91 RID: 15249 RVA: 0x000CC09A File Offset: 0x000CB09A
		internal PermissionSetEnumerator(PermissionSet permSet)
		{
			this.enm = new PermissionSetEnumeratorInternal(permSet);
		}

		// Token: 0x04001E8D RID: 7821
		private PermissionSetEnumeratorInternal enm;
	}
}
