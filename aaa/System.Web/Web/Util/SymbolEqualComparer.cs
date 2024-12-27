using System;
using System.Collections;
using System.Globalization;

namespace System.Web.Util
{
	// Token: 0x02000790 RID: 1936
	internal class SymbolEqualComparer : IComparer
	{
		// Token: 0x06005D23 RID: 23843 RVA: 0x00175243 File Offset: 0x00174243
		internal SymbolEqualComparer()
		{
		}

		// Token: 0x06005D24 RID: 23844 RVA: 0x0017524C File Offset: 0x0017424C
		int IComparer.Compare(object keyLeft, object keyRight)
		{
			string text = keyLeft as string;
			string text2 = keyRight as string;
			if (text == null)
			{
				throw new ArgumentNullException("keyLeft");
			}
			if (text2 == null)
			{
				throw new ArgumentNullException("keyRight");
			}
			int length = text.Length;
			int length2 = text2.Length;
			if (length != length2)
			{
				return 1;
			}
			for (int i = 0; i < length; i++)
			{
				char c = text[i];
				char c2 = text2[i];
				if (c != c2)
				{
					UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
					UnicodeCategory unicodeCategory2 = char.GetUnicodeCategory(c2);
					if (unicodeCategory == UnicodeCategory.UppercaseLetter && unicodeCategory2 == UnicodeCategory.LowercaseLetter)
					{
						if (char.ToLower(c, CultureInfo.InvariantCulture) == c2)
						{
							goto IL_00A5;
						}
					}
					else if (unicodeCategory2 == UnicodeCategory.UppercaseLetter && unicodeCategory == UnicodeCategory.LowercaseLetter && char.ToLower(c2, CultureInfo.InvariantCulture) == c)
					{
						goto IL_00A5;
					}
					return 1;
				}
				IL_00A5:;
			}
			return 0;
		}

		// Token: 0x040031BA RID: 12730
		internal static readonly IComparer Default = new SymbolEqualComparer();
	}
}
