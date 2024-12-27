using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x020008EE RID: 2286
	public sealed class AceEnumerator : IEnumerator
	{
		// Token: 0x06005325 RID: 21285 RVA: 0x0012DC6F File Offset: 0x0012CC6F
		internal AceEnumerator(GenericAcl collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this._acl = collection;
			this.Reset();
		}

		// Token: 0x17000E64 RID: 3684
		// (get) Token: 0x06005326 RID: 21286 RVA: 0x0012DC92 File Offset: 0x0012CC92
		object IEnumerator.Current
		{
			get
			{
				if (this._current == -1 || this._current >= this._acl.Count)
				{
					throw new InvalidOperationException(Environment.GetResourceString("Arg_InvalidOperationException"));
				}
				return this._acl[this._current];
			}
		}

		// Token: 0x17000E65 RID: 3685
		// (get) Token: 0x06005327 RID: 21287 RVA: 0x0012DCD1 File Offset: 0x0012CCD1
		public GenericAce Current
		{
			get
			{
				return ((IEnumerator)this).Current as GenericAce;
			}
		}

		// Token: 0x06005328 RID: 21288 RVA: 0x0012DCDE File Offset: 0x0012CCDE
		public bool MoveNext()
		{
			this._current++;
			return this._current < this._acl.Count;
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x0012DD01 File Offset: 0x0012CD01
		public void Reset()
		{
			this._current = -1;
		}

		// Token: 0x04002AFF RID: 11007
		private int _current;

		// Token: 0x04002B00 RID: 11008
		private readonly GenericAcl _acl;
	}
}
