using System;
using System.Globalization;

namespace System
{
	// Token: 0x0200002A RID: 42
	[Serializable]
	internal sealed class CultureAwareComparer : StringComparer
	{
		// Token: 0x06000210 RID: 528 RVA: 0x00009583 File Offset: 0x00008583
		internal CultureAwareComparer(CultureInfo culture, bool ignoreCase)
		{
			this._compareInfo = culture.CompareInfo;
			this._ignoreCase = ignoreCase;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000959E File Offset: 0x0000859E
		public override int Compare(string x, string y)
		{
			if (object.ReferenceEquals(x, y))
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			return this._compareInfo.Compare(x, y, this._ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000095CE File Offset: 0x000085CE
		public override bool Equals(string x, string y)
		{
			return object.ReferenceEquals(x, y) || (x != null && y != null && this._compareInfo.Compare(x, y, this._ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None) == 0);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000095FF File Offset: 0x000085FF
		public override int GetHashCode(string obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (this._ignoreCase)
			{
				return this._compareInfo.GetHashCodeOfString(obj, CompareOptions.IgnoreCase);
			}
			return this._compareInfo.GetHashCodeOfString(obj, CompareOptions.None);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009634 File Offset: 0x00008634
		public override bool Equals(object obj)
		{
			CultureAwareComparer cultureAwareComparer = obj as CultureAwareComparer;
			return cultureAwareComparer != null && this._ignoreCase == cultureAwareComparer._ignoreCase && this._compareInfo.Equals(cultureAwareComparer._compareInfo);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009670 File Offset: 0x00008670
		public override int GetHashCode()
		{
			int hashCode = this._compareInfo.GetHashCode();
			if (!this._ignoreCase)
			{
				return hashCode;
			}
			return ~hashCode;
		}

		// Token: 0x040000A2 RID: 162
		private CompareInfo _compareInfo;

		// Token: 0x040000A3 RID: 163
		private bool _ignoreCase;
	}
}
