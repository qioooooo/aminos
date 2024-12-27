using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200010E RID: 270
	internal sealed class DefinitionIdentity
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x0001F4E0 File Offset: 0x0001E4E0
		internal DefinitionIdentity(IDefinitionIdentity i)
		{
			if (i == null)
			{
				throw new ArgumentNullException();
			}
			this._id = i;
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001F4F8 File Offset: 0x0001E4F8
		private string GetAttribute(string ns, string n)
		{
			return this._id.GetAttribute(ns, n);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001F507 File Offset: 0x0001E507
		private string GetAttribute(string n)
		{
			return this._id.GetAttribute(null, n);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001F516 File Offset: 0x0001E516
		private void SetAttribute(string ns, string n, string v)
		{
			this._id.SetAttribute(ns, n, v);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001F526 File Offset: 0x0001E526
		private void SetAttribute(string n, string v)
		{
			this.SetAttribute(null, n, v);
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001F531 File Offset: 0x0001E531
		private void DeleteAttribute(string ns, string n)
		{
			this.SetAttribute(ns, n, null);
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001F53C File Offset: 0x0001E53C
		private void DeleteAttribute(string n)
		{
			this.SetAttribute(null, n, null);
		}

		// Token: 0x04000503 RID: 1283
		internal IDefinitionIdentity _id;
	}
}
