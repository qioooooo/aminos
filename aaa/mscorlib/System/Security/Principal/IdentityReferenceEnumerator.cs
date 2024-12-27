using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Security.Principal
{
	// Token: 0x0200092B RID: 2347
	[ComVisible(false)]
	internal class IdentityReferenceEnumerator : IEnumerator<IdentityReference>, IDisposable, IEnumerator
	{
		// Token: 0x0600552E RID: 21806 RVA: 0x00135934 File Offset: 0x00134934
		internal IdentityReferenceEnumerator(IdentityReferenceCollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this._Collection = collection;
			this._Current = -1;
		}

		// Token: 0x17000ED5 RID: 3797
		// (get) Token: 0x0600552F RID: 21807 RVA: 0x00135958 File Offset: 0x00134958
		object IEnumerator.Current
		{
			get
			{
				return this._Collection.Identities[this._Current];
			}
		}

		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06005530 RID: 21808 RVA: 0x00135970 File Offset: 0x00134970
		public IdentityReference Current
		{
			get
			{
				return ((IEnumerator)this).Current as IdentityReference;
			}
		}

		// Token: 0x06005531 RID: 21809 RVA: 0x0013597D File Offset: 0x0013497D
		public bool MoveNext()
		{
			this._Current++;
			return this._Current < this._Collection.Count;
		}

		// Token: 0x06005532 RID: 21810 RVA: 0x001359A0 File Offset: 0x001349A0
		public void Reset()
		{
			this._Current = -1;
		}

		// Token: 0x06005533 RID: 21811 RVA: 0x001359A9 File Offset: 0x001349A9
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x06005534 RID: 21812 RVA: 0x001359B1 File Offset: 0x001349B1
		protected void Dispose()
		{
		}

		// Token: 0x04002C12 RID: 11282
		private int _Current;

		// Token: 0x04002C13 RID: 11283
		private readonly IdentityReferenceCollection _Collection;
	}
}
