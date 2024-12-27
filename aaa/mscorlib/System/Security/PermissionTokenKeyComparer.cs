using System;
using System.Collections;
using System.Globalization;

namespace System.Security
{
	// Token: 0x02000665 RID: 1637
	[Serializable]
	internal sealed class PermissionTokenKeyComparer : IEqualityComparer
	{
		// Token: 0x06003B97 RID: 15255 RVA: 0x000CC164 File Offset: 0x000CB164
		public PermissionTokenKeyComparer(CultureInfo culture)
		{
			this._caseSensitiveComparer = new Comparer(culture);
			this._info = culture.TextInfo;
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x000CC184 File Offset: 0x000CB184
		public int Compare(object a, object b)
		{
			string text = a as string;
			string text2 = b as string;
			if (text == null || text2 == null)
			{
				return this._caseSensitiveComparer.Compare(a, b);
			}
			int num = this._caseSensitiveComparer.Compare(a, b);
			if (num == 0)
			{
				return 0;
			}
			if (SecurityManager._IsSameType(text, text2))
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x000CC1D2 File Offset: 0x000CB1D2
		public bool Equals(object a, object b)
		{
			return a == b || (a != null && b != null && this.Compare(a, b) == 0);
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x000CC1F0 File Offset: 0x000CB1F0
		public int GetHashCode(object obj)
		{
			string text = obj as string;
			if (text == null)
			{
				return obj.GetHashCode();
			}
			int num = text.IndexOf(',');
			if (num == -1)
			{
				num = text.Length;
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				num2 = (num2 << 7) ^ (int)text[i] ^ (num2 >> 25);
			}
			return num2;
		}

		// Token: 0x04001E9A RID: 7834
		private Comparer _caseSensitiveComparer;

		// Token: 0x04001E9B RID: 7835
		private TextInfo _info;
	}
}
