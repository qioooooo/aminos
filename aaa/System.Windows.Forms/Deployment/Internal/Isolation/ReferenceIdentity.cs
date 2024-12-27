using System;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E0 RID: 224
	internal sealed class ReferenceIdentity
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00007DC1 File Offset: 0x00006DC1
		internal ReferenceIdentity(IReferenceIdentity i)
		{
			if (i == null)
			{
				throw new ArgumentNullException();
			}
			this._id = i;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00007DD9 File Offset: 0x00006DD9
		private string GetAttribute(string ns, string n)
		{
			return this._id.GetAttribute(ns, n);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00007DE8 File Offset: 0x00006DE8
		private string GetAttribute(string n)
		{
			return this._id.GetAttribute(null, n);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00007DF7 File Offset: 0x00006DF7
		private void SetAttribute(string ns, string n, string v)
		{
			this._id.SetAttribute(ns, n, v);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00007E07 File Offset: 0x00006E07
		private void SetAttribute(string n, string v)
		{
			this.SetAttribute(null, n, v);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00007E12 File Offset: 0x00006E12
		private void DeleteAttribute(string ns, string n)
		{
			this.SetAttribute(ns, n, null);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00007E1D File Offset: 0x00006E1D
		private void DeleteAttribute(string n)
		{
			this.SetAttribute(null, n, null);
		}

		// Token: 0x04000D8E RID: 3470
		internal IReferenceIdentity _id;
	}
}
