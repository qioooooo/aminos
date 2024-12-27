using System;

namespace System.Collections
{
	// Token: 0x0200025E RID: 606
	[Serializable]
	internal class CompatibleComparer : IEqualityComparer
	{
		// Token: 0x06001848 RID: 6216 RVA: 0x0003F62E File Offset: 0x0003E62E
		internal CompatibleComparer(IComparer comparer, IHashCodeProvider hashCodeProvider)
		{
			this._comparer = comparer;
			this._hcp = hashCodeProvider;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0003F644 File Offset: 0x0003E644
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			if (this._comparer != null)
			{
				return this._comparer.Compare(a, b);
			}
			IComparable comparable = a as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(b);
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_ImplementIComparable"));
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0003F698 File Offset: 0x0003E698
		public bool Equals(object a, object b)
		{
			return this.Compare(a, b) == 0;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0003F6A5 File Offset: 0x0003E6A5
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._hcp != null)
			{
				return this._hcp.GetHashCode(obj);
			}
			return obj.GetHashCode();
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x0003F6D0 File Offset: 0x0003E6D0
		internal IComparer Comparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x0003F6D8 File Offset: 0x0003E6D8
		internal IHashCodeProvider HashCodeProvider
		{
			get
			{
				return this._hcp;
			}
		}

		// Token: 0x0400097A RID: 2426
		private IComparer _comparer;

		// Token: 0x0400097B RID: 2427
		private IHashCodeProvider _hcp;
	}
}
