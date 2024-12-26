using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x02000070 RID: 112
	public class EnumeratorObject : JSObject
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x00025CEE File Offset: 0x00024CEE
		internal EnumeratorObject(EnumeratorPrototype parent)
			: base(parent)
		{
			this.enumerator = null;
			this.collection = null;
			this.noExpando = false;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00025D0C File Offset: 0x00024D0C
		internal EnumeratorObject(EnumeratorPrototype parent, IEnumerable collection)
			: base(parent)
		{
			this.collection = collection;
			if (collection != null)
			{
				this.enumerator = collection.GetEnumerator();
			}
			this.LoadObject();
			this.noExpando = false;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00025D38 File Offset: 0x00024D38
		internal virtual bool atEnd()
		{
			return this.enumerator == null || this.obj == null;
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00025D4D File Offset: 0x00024D4D
		internal virtual object item()
		{
			if (this.enumerator != null)
			{
				return this.obj;
			}
			return null;
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00025D5F File Offset: 0x00024D5F
		protected void LoadObject()
		{
			if (this.enumerator != null && this.enumerator.MoveNext())
			{
				this.obj = this.enumerator.Current;
				return;
			}
			this.obj = null;
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00025D8F File Offset: 0x00024D8F
		internal virtual void moveFirst()
		{
			if (this.collection != null)
			{
				this.enumerator = this.collection.GetEnumerator();
			}
			this.LoadObject();
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00025DB0 File Offset: 0x00024DB0
		internal virtual void moveNext()
		{
			if (this.enumerator != null)
			{
				this.LoadObject();
			}
		}

		// Token: 0x0400024A RID: 586
		private IEnumerable collection;

		// Token: 0x0400024B RID: 587
		protected IEnumerator enumerator;

		// Token: 0x0400024C RID: 588
		private object obj;
	}
}
