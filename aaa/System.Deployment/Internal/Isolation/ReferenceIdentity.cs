using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010B RID: 267
	internal sealed class ReferenceIdentity
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x0001F479 File Offset: 0x0001E479
		internal ReferenceIdentity(IReferenceIdentity i)
		{
			if (i == null)
			{
				throw new ArgumentNullException();
			}
			this._id = i;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001F491 File Offset: 0x0001E491
		private string GetAttribute(string ns, string n)
		{
			return this._id.GetAttribute(ns, n);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001F4A0 File Offset: 0x0001E4A0
		private string GetAttribute(string n)
		{
			return this._id.GetAttribute(null, n);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001F4AF File Offset: 0x0001E4AF
		private void SetAttribute(string ns, string n, string v)
		{
			this._id.SetAttribute(ns, n, v);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0001F4BF File Offset: 0x0001E4BF
		private void SetAttribute(string n, string v)
		{
			this.SetAttribute(null, n, v);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0001F4CA File Offset: 0x0001E4CA
		private void DeleteAttribute(string ns, string n)
		{
			this.SetAttribute(ns, n, null);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0001F4D5 File Offset: 0x0001E4D5
		private void DeleteAttribute(string n)
		{
			this.SetAttribute(null, n, null);
		}

		// Token: 0x04000502 RID: 1282
		internal IReferenceIdentity _id;
	}
}
