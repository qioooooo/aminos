using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200002C RID: 44
	internal sealed class RegexReplacement
	{
		// Token: 0x06000229 RID: 553 RVA: 0x00010F6C File Offset: 0x0000FF6C
		internal RegexReplacement(string rep, RegexNode concat, Hashtable _caps)
		{
			this._rep = rep;
			if (concat.Type() != 25)
			{
				throw new ArgumentException(SR.GetString("ReplacementError"));
			}
			StringBuilder stringBuilder = new StringBuilder();
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			int i = 0;
			while (i < concat.ChildCount())
			{
				RegexNode regexNode = concat.Child(i);
				switch (regexNode.Type())
				{
				case 9:
					stringBuilder.Append(regexNode._ch);
					break;
				case 10:
				case 11:
					goto IL_00FC;
				case 12:
					stringBuilder.Append(regexNode._str);
					break;
				case 13:
				{
					if (stringBuilder.Length > 0)
					{
						arrayList2.Add(arrayList.Count);
						arrayList.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
					int num = regexNode._m;
					if (_caps != null && num >= 0)
					{
						num = (int)_caps[num];
					}
					arrayList2.Add(-5 - num);
					break;
				}
				default:
					goto IL_00FC;
				}
				i++;
				continue;
				IL_00FC:
				throw new ArgumentException(SR.GetString("ReplacementError"));
			}
			if (stringBuilder.Length > 0)
			{
				arrayList2.Add(arrayList.Count);
				arrayList.Add(stringBuilder.ToString());
			}
			this._strings = arrayList;
			this._rules = arrayList2;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000110D0 File Offset: 0x000100D0
		private void ReplacementImpl(StringBuilder sb, Match match)
		{
			for (int i = 0; i < this._rules.Count; i++)
			{
				int num = (int)this._rules[i];
				if (num >= 0)
				{
					sb.Append((string)this._strings[num]);
				}
				else if (num < -4)
				{
					sb.Append(match.GroupToStringImpl(-5 - num));
				}
				else
				{
					switch (-5 - num)
					{
					case -4:
						sb.Append(match.GetOriginalString());
						break;
					case -3:
						sb.Append(match.LastGroupToStringImpl());
						break;
					case -2:
						sb.Append(match.GetRightSubstring());
						break;
					case -1:
						sb.Append(match.GetLeftSubstring());
						break;
					}
				}
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0001119B File Offset: 0x0001019B
		internal string Pattern
		{
			get
			{
				return this._rep;
			}
		}

		// Token: 0x0600022C RID: 556 RVA: 0x000111A4 File Offset: 0x000101A4
		internal string Replacement(Match match)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.ReplacementImpl(stringBuilder, match);
			return stringBuilder.ToString();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000111C8 File Offset: 0x000101C8
		internal string Replace(Regex regex, string input, int count, int startat)
		{
			if (count < -1)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("CountTooSmall"));
			}
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("startat", SR.GetString("BeginIndexNotNegative"));
			}
			if (count == 0)
			{
				return input;
			}
			Match match = regex.Match(input, startat);
			if (!match.Success)
			{
				return input;
			}
			StringBuilder stringBuilder;
			if (!regex.RightToLeft)
			{
				stringBuilder = new StringBuilder();
				int num = 0;
				do
				{
					if (match.Index != num)
					{
						stringBuilder.Append(input, num, match.Index - num);
					}
					num = match.Index + match.Length;
					this.ReplacementImpl(stringBuilder, match);
					if (--count == 0)
					{
						break;
					}
					match = match.NextMatch();
				}
				while (match.Success);
				if (num < input.Length)
				{
					stringBuilder.Append(input, num, input.Length - num);
				}
			}
			else
			{
				ArrayList arrayList = new ArrayList();
				int num2 = input.Length;
				do
				{
					if (match.Index + match.Length != num2)
					{
						arrayList.Add(input.Substring(match.Index + match.Length, num2 - match.Index - match.Length));
					}
					num2 = match.Index;
					for (int i = this._rules.Count - 1; i >= 0; i--)
					{
						int num3 = (int)this._rules[i];
						if (num3 >= 0)
						{
							arrayList.Add((string)this._strings[num3]);
						}
						else
						{
							arrayList.Add(match.GroupToStringImpl(-5 - num3));
						}
					}
					if (--count == 0)
					{
						break;
					}
					match = match.NextMatch();
				}
				while (match.Success);
				stringBuilder = new StringBuilder();
				if (num2 > 0)
				{
					stringBuilder.Append(input, 0, num2);
				}
				for (int j = arrayList.Count - 1; j >= 0; j--)
				{
					stringBuilder.Append((string)arrayList[j]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x000113B8 File Offset: 0x000103B8
		internal static string Replace(MatchEvaluator evaluator, Regex regex, string input, int count, int startat)
		{
			if (evaluator == null)
			{
				throw new ArgumentNullException("evaluator");
			}
			if (count < -1)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("CountTooSmall"));
			}
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("startat", SR.GetString("BeginIndexNotNegative"));
			}
			if (count == 0)
			{
				return input;
			}
			Match match = regex.Match(input, startat);
			if (!match.Success)
			{
				return input;
			}
			StringBuilder stringBuilder;
			if (!regex.RightToLeft)
			{
				stringBuilder = new StringBuilder();
				int num = 0;
				do
				{
					if (match.Index != num)
					{
						stringBuilder.Append(input, num, match.Index - num);
					}
					num = match.Index + match.Length;
					stringBuilder.Append(evaluator(match));
					if (--count == 0)
					{
						break;
					}
					match = match.NextMatch();
				}
				while (match.Success);
				if (num < input.Length)
				{
					stringBuilder.Append(input, num, input.Length - num);
				}
			}
			else
			{
				ArrayList arrayList = new ArrayList();
				int num2 = input.Length;
				do
				{
					if (match.Index + match.Length != num2)
					{
						arrayList.Add(input.Substring(match.Index + match.Length, num2 - match.Index - match.Length));
					}
					num2 = match.Index;
					arrayList.Add(evaluator(match));
					if (--count == 0)
					{
						break;
					}
					match = match.NextMatch();
				}
				while (match.Success);
				stringBuilder = new StringBuilder();
				if (num2 > 0)
				{
					stringBuilder.Append(input, 0, num2);
				}
				for (int i = arrayList.Count - 1; i >= 0; i--)
				{
					stringBuilder.Append((string)arrayList[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00011568 File Offset: 0x00010568
		internal static string[] Split(Regex regex, string input, int count, int startat)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("CountTooSmall"));
			}
			if (startat < 0 || startat > input.Length)
			{
				throw new ArgumentOutOfRangeException("startat", SR.GetString("BeginIndexNotNegative"));
			}
			if (count == 1)
			{
				return new string[] { input };
			}
			count--;
			Match match = regex.Match(input, startat);
			if (!match.Success)
			{
				return new string[] { input };
			}
			ArrayList arrayList = new ArrayList();
			if (!regex.RightToLeft)
			{
				int num = 0;
				do
				{
					arrayList.Add(input.Substring(num, match.Index - num));
					num = match.Index + match.Length;
					for (int i = 1; i < match.Groups.Count; i++)
					{
						if (match.IsMatched(i))
						{
							arrayList.Add(match.Groups[i].ToString());
						}
					}
					if (--count == 0)
					{
						break;
					}
					match = match.NextMatch();
				}
				while (match.Success);
				arrayList.Add(input.Substring(num, input.Length - num));
			}
			else
			{
				int num2 = input.Length;
				do
				{
					arrayList.Add(input.Substring(match.Index + match.Length, num2 - match.Index - match.Length));
					num2 = match.Index;
					for (int j = 1; j < match.Groups.Count; j++)
					{
						if (match.IsMatched(j))
						{
							arrayList.Add(match.Groups[j].ToString());
						}
					}
					if (--count == 0)
					{
						break;
					}
					match = match.NextMatch();
				}
				while (match.Success);
				arrayList.Add(input.Substring(0, num2));
				arrayList.Reverse(0, arrayList.Count);
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x040007B3 RID: 1971
		internal const int Specials = 4;

		// Token: 0x040007B4 RID: 1972
		internal const int LeftPortion = -1;

		// Token: 0x040007B5 RID: 1973
		internal const int RightPortion = -2;

		// Token: 0x040007B6 RID: 1974
		internal const int LastGroup = -3;

		// Token: 0x040007B7 RID: 1975
		internal const int WholeString = -4;

		// Token: 0x040007B8 RID: 1976
		internal string _rep;

		// Token: 0x040007B9 RID: 1977
		internal ArrayList _strings;

		// Token: 0x040007BA RID: 1978
		internal ArrayList _rules;
	}
}
