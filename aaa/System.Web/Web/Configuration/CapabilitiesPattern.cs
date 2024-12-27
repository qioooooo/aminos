using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Web.Configuration
{
	// Token: 0x020001B4 RID: 436
	internal class CapabilitiesPattern
	{
		// Token: 0x06001926 RID: 6438 RVA: 0x0007828A File Offset: 0x0007728A
		internal CapabilitiesPattern()
		{
			this._strings = new string[1];
			this._strings[0] = string.Empty;
			this._rules = new int[1];
			this._rules[0] = 2;
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x000782C0 File Offset: 0x000772C0
		internal CapabilitiesPattern(string text)
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			int num = 0;
			Match match;
			for (;;)
			{
				if ((match = CapabilitiesPattern.textPat.Match(text, num)).Success && match.Length > 0)
				{
					arrayList2.Add(0);
					arrayList.Add(Regex.Unescape(match.ToString()));
					num = match.Index + match.Length;
				}
				if (num == text.Length)
				{
					goto IL_0136;
				}
				if ((match = CapabilitiesPattern.refPat.Match(text, num)).Success)
				{
					arrayList2.Add(1);
					arrayList.Add(match.Groups["name"].Value);
				}
				else
				{
					if (!(match = CapabilitiesPattern.varPat.Match(text, num)).Success)
					{
						break;
					}
					arrayList2.Add(2);
					arrayList.Add(match.Groups["name"].Value);
				}
				num = match.Index + match.Length;
			}
			match = CapabilitiesPattern.errorPat.Match(text, num);
			throw new ArgumentException(SR.GetString("Unrecognized_construct_in_pattern", new object[]
			{
				match.ToString(),
				text
			}));
			IL_0136:
			this._strings = (string[])arrayList.ToArray(typeof(string));
			this._rules = new int[arrayList2.Count];
			for (int i = 0; i < arrayList2.Count; i++)
			{
				this._rules[i] = (int)arrayList2[i];
			}
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0007845C File Offset: 0x0007745C
		internal virtual string Expand(CapabilitiesState matchstate)
		{
			StringBuilder stringBuilder = null;
			string text = null;
			for (int i = 0; i < this._rules.Length; i++)
			{
				if (stringBuilder == null && text != null)
				{
					stringBuilder = new StringBuilder(text);
				}
				switch (this._rules[i])
				{
				case 0:
					text = this._strings[i];
					break;
				case 1:
					text = matchstate.ResolveReference(this._strings[i]);
					break;
				case 2:
					text = matchstate.ResolveVariable(this._strings[i]);
					break;
				}
				if (stringBuilder != null && text != null)
				{
					stringBuilder.Append(text);
				}
			}
			if (stringBuilder != null)
			{
				return stringBuilder.ToString();
			}
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x0400170F RID: 5903
		internal const int Literal = 0;

		// Token: 0x04001710 RID: 5904
		internal const int Reference = 1;

		// Token: 0x04001711 RID: 5905
		internal const int Variable = 2;

		// Token: 0x04001712 RID: 5906
		internal string[] _strings;

		// Token: 0x04001713 RID: 5907
		internal int[] _rules;

		// Token: 0x04001714 RID: 5908
		internal static readonly Regex refPat = new Regex("\\G\\$(?:(?<name>\\d+)|\\{(?<name>\\w+)\\})");

		// Token: 0x04001715 RID: 5909
		internal static readonly Regex varPat = new Regex("\\G\\%\\{(?<name>\\w+)\\}");

		// Token: 0x04001716 RID: 5910
		internal static readonly Regex textPat = new Regex("\\G[^$%\\\\]*(?:\\.[^$%\\\\]*)*");

		// Token: 0x04001717 RID: 5911
		internal static readonly Regex errorPat = new Regex(".{0,8}");

		// Token: 0x04001718 RID: 5912
		internal static readonly CapabilitiesPattern Default = new CapabilitiesPattern();
	}
}
