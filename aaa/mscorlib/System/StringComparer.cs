using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000029 RID: 41
	[ComVisible(true)]
	[Serializable]
	public abstract class StringComparer : IComparer, IEqualityComparer, IComparer<string>, IEqualityComparer<string>
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000201 RID: 513 RVA: 0x00009425 File Offset: 0x00008425
		public static StringComparer InvariantCulture
		{
			get
			{
				return StringComparer._invariantCulture;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000942C File Offset: 0x0000842C
		public static StringComparer InvariantCultureIgnoreCase
		{
			get
			{
				return StringComparer._invariantCultureIgnoreCase;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00009433 File Offset: 0x00008433
		public static StringComparer CurrentCulture
		{
			get
			{
				return new CultureAwareComparer(CultureInfo.CurrentCulture, false);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000204 RID: 516 RVA: 0x00009440 File Offset: 0x00008440
		public static StringComparer CurrentCultureIgnoreCase
		{
			get
			{
				return new CultureAwareComparer(CultureInfo.CurrentCulture, true);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000944D File Offset: 0x0000844D
		public static StringComparer Ordinal
		{
			get
			{
				return StringComparer._ordinal;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00009454 File Offset: 0x00008454
		public static StringComparer OrdinalIgnoreCase
		{
			get
			{
				return StringComparer._ordinalIgnoreCase;
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000945B File Offset: 0x0000845B
		public static StringComparer Create(CultureInfo culture, bool ignoreCase)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			return new CultureAwareComparer(culture, ignoreCase);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00009474 File Offset: 0x00008474
		public int Compare(object x, object y)
		{
			if (x == y)
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
			string text = x as string;
			if (text != null)
			{
				string text2 = y as string;
				if (text2 != null)
				{
					return this.Compare(text, text2);
				}
			}
			IComparable comparable = x as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(y);
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_ImplementIComparable"));
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000094D0 File Offset: 0x000084D0
		public bool Equals(object x, object y)
		{
			if (x == y)
			{
				return true;
			}
			if (x == null || y == null)
			{
				return false;
			}
			string text = x as string;
			if (text != null)
			{
				string text2 = y as string;
				if (text2 != null)
				{
					return this.Equals(text, text2);
				}
			}
			return x.Equals(y);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009510 File Offset: 0x00008510
		public int GetHashCode(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			string text = obj as string;
			if (text != null)
			{
				return this.GetHashCode(text);
			}
			return obj.GetHashCode();
		}

		// Token: 0x0600020B RID: 523
		public abstract int Compare(string x, string y);

		// Token: 0x0600020C RID: 524
		public abstract bool Equals(string x, string y);

		// Token: 0x0600020D RID: 525
		public abstract int GetHashCode(string obj);

		// Token: 0x0400009E RID: 158
		private static StringComparer _invariantCulture = new CultureAwareComparer(CultureInfo.InvariantCulture, false);

		// Token: 0x0400009F RID: 159
		private static StringComparer _invariantCultureIgnoreCase = new CultureAwareComparer(CultureInfo.InvariantCulture, true);

		// Token: 0x040000A0 RID: 160
		private static StringComparer _ordinal = new OrdinalComparer(false);

		// Token: 0x040000A1 RID: 161
		private static StringComparer _ordinalIgnoreCase = new OrdinalComparer(true);
	}
}
