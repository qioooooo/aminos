using System;
using System.Globalization;

namespace System.Collections.Specialized
{
	// Token: 0x0200025A RID: 602
	[Serializable]
	internal class CompatibleComparer : IEqualityComparer
	{
		// Token: 0x060014B1 RID: 5297 RVA: 0x00044CD0 File Offset: 0x00043CD0
		internal CompatibleComparer(IComparer comparer, IHashCodeProvider hashCodeProvider)
		{
			this._comparer = comparer;
			this._hcp = hashCodeProvider;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00044CE8 File Offset: 0x00043CE8
		public bool Equals(object a, object b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			try
			{
				if (this._comparer != null)
				{
					return this._comparer.Compare(a, b) == 0;
				}
				IComparable comparable = a as IComparable;
				if (comparable != null)
				{
					return comparable.CompareTo(b) == 0;
				}
			}
			catch (ArgumentException)
			{
				return false;
			}
			return a.Equals(b);
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00044D58 File Offset: 0x00043D58
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

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x00044D83 File Offset: 0x00043D83
		public IComparer Comparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060014B5 RID: 5301 RVA: 0x00044D8B File Offset: 0x00043D8B
		public IHashCodeProvider HashCodeProvider
		{
			get
			{
				return this._hcp;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x060014B6 RID: 5302 RVA: 0x00044D93 File Offset: 0x00043D93
		public static IComparer DefaultComparer
		{
			get
			{
				if (CompatibleComparer.defaultComparer == null)
				{
					CompatibleComparer.defaultComparer = new CaseInsensitiveComparer(CultureInfo.InvariantCulture);
				}
				return CompatibleComparer.defaultComparer;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x060014B7 RID: 5303 RVA: 0x00044DB0 File Offset: 0x00043DB0
		public static IHashCodeProvider DefaultHashCodeProvider
		{
			get
			{
				if (CompatibleComparer.defaultHashProvider == null)
				{
					CompatibleComparer.defaultHashProvider = new CaseInsensitiveHashCodeProvider(CultureInfo.InvariantCulture);
				}
				return CompatibleComparer.defaultHashProvider;
			}
		}

		// Token: 0x0400119F RID: 4511
		private IComparer _comparer;

		// Token: 0x040011A0 RID: 4512
		private static IComparer defaultComparer;

		// Token: 0x040011A1 RID: 4513
		private IHashCodeProvider _hcp;

		// Token: 0x040011A2 RID: 4514
		private static IHashCodeProvider defaultHashProvider;
	}
}
