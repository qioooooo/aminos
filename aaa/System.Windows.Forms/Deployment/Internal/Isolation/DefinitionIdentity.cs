using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E3 RID: 227
	internal sealed class DefinitionIdentity
	{
		// Token: 0x06000376 RID: 886 RVA: 0x00007E28 File Offset: 0x00006E28
		internal DefinitionIdentity(IDefinitionIdentity i)
		{
			if (i == null)
			{
				throw new ArgumentNullException();
			}
			this._id = i;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00007E40 File Offset: 0x00006E40
		private string GetAttribute(string ns, string n)
		{
			return this._id.GetAttribute(ns, n);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00007E4F File Offset: 0x00006E4F
		private string GetAttribute(string n)
		{
			return this._id.GetAttribute(null, n);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00007E5E File Offset: 0x00006E5E
		private void SetAttribute(string ns, string n, string v)
		{
			this._id.SetAttribute(ns, n, v);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00007E6E File Offset: 0x00006E6E
		private void SetAttribute(string n, string v)
		{
			this.SetAttribute(null, n, v);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00007E79 File Offset: 0x00006E79
		private void DeleteAttribute(string ns, string n)
		{
			this.SetAttribute(ns, n, null);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00007E84 File Offset: 0x00006E84
		private void DeleteAttribute(string n)
		{
			this.SetAttribute(null, n, null);
		}

		// Token: 0x04000D8F RID: 3471
		internal IDefinitionIdentity _id;
	}
}
