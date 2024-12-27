using System;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x020004A9 RID: 1193
	[Serializable]
	internal class DelayedRegex
	{
		// Token: 0x06002483 RID: 9347 RVA: 0x0008F8F8 File Offset: 0x0008E8F8
		internal DelayedRegex(string regexString)
		{
			if (regexString == null)
			{
				throw new ArgumentNullException("regexString");
			}
			this._AsString = regexString;
		}

		// Token: 0x06002484 RID: 9348 RVA: 0x0008F915 File Offset: 0x0008E915
		internal DelayedRegex(Regex regex)
		{
			if (regex == null)
			{
				throw new ArgumentNullException("regex");
			}
			this._AsRegex = regex;
		}

		// Token: 0x17000792 RID: 1938
		// (get) Token: 0x06002485 RID: 9349 RVA: 0x0008F932 File Offset: 0x0008E932
		internal Regex AsRegex
		{
			get
			{
				if (this._AsRegex == null)
				{
					this._AsRegex = new Regex(this._AsString + "[/]?", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);
				}
				return this._AsRegex;
			}
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x0008F964 File Offset: 0x0008E964
		public override string ToString()
		{
			if (this._AsString == null)
			{
				return this._AsString = this._AsRegex.ToString();
			}
			return this._AsString;
		}

		// Token: 0x040024BF RID: 9407
		private Regex _AsRegex;

		// Token: 0x040024C0 RID: 9408
		private string _AsString;
	}
}
