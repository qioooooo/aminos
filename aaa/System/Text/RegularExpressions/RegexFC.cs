using System;
using System.Globalization;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200001E RID: 30
	internal sealed class RegexFC
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000B0AF File Offset: 0x0000A0AF
		internal RegexFC(bool nullable)
		{
			this._cc = new RegexCharClass();
			this._nullable = nullable;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000B0CC File Offset: 0x0000A0CC
		internal RegexFC(char ch, bool not, bool nullable, bool caseInsensitive)
		{
			this._cc = new RegexCharClass();
			if (not)
			{
				if (ch > '\0')
				{
					this._cc.AddRange('\0', ch - '\u0001');
				}
				if (ch < '\uffff')
				{
					this._cc.AddRange(ch + '\u0001', char.MaxValue);
				}
			}
			else
			{
				this._cc.AddRange(ch, ch);
			}
			this._caseInsensitive = caseInsensitive;
			this._nullable = nullable;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000B13B File Offset: 0x0000A13B
		internal RegexFC(string charClass, bool nullable, bool caseInsensitive)
		{
			this._cc = RegexCharClass.Parse(charClass);
			this._nullable = nullable;
			this._caseInsensitive = caseInsensitive;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000B160 File Offset: 0x0000A160
		internal bool AddFC(RegexFC fc, bool concatenate)
		{
			if (!this._cc.CanMerge || !fc._cc.CanMerge)
			{
				return false;
			}
			if (concatenate)
			{
				if (!this._nullable)
				{
					return true;
				}
				if (!fc._nullable)
				{
					this._nullable = false;
				}
			}
			else if (fc._nullable)
			{
				this._nullable = true;
			}
			this._caseInsensitive |= fc._caseInsensitive;
			this._cc.AddCharClass(fc._cc);
			return true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000B1DB File Offset: 0x0000A1DB
		internal string GetFirstChars(CultureInfo culture)
		{
			if (this._caseInsensitive)
			{
				this._cc.AddLowercase(culture);
			}
			return this._cc.ToStringClass();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000B1FC File Offset: 0x0000A1FC
		internal bool IsCaseInsensitive()
		{
			return this._caseInsensitive;
		}

		// Token: 0x04000723 RID: 1827
		internal RegexCharClass _cc;

		// Token: 0x04000724 RID: 1828
		internal bool _nullable;

		// Token: 0x04000725 RID: 1829
		internal bool _caseInsensitive;
	}
}
