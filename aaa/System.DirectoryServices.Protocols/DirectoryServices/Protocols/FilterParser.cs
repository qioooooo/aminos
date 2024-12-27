using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000069 RID: 105
	internal class FilterParser
	{
		// Token: 0x06000229 RID: 553 RVA: 0x00009BD8 File Offset: 0x00008BD8
		public static ADFilter ParseFilterString(string filter)
		{
			Match match = FilterParser.mFilter.Match(filter);
			if (!match.Success)
			{
				return null;
			}
			ADFilter adfilter = new ADFilter();
			if (match.Groups["item"].ToString().Length == 0)
			{
				ArrayList arrayList = new ArrayList();
				string text = match.Groups["filterlist"].ToString().Trim();
				while (text.Length > 0)
				{
					if (text[0] != '(')
					{
						return null;
					}
					int num = 1;
					int num2 = 1;
					bool flag = false;
					while (num < text.Length && !flag)
					{
						if (text[num] == '(')
						{
							num2++;
						}
						if (text[num] == ')')
						{
							if (num2 < 1)
							{
								return null;
							}
							if (num2 == 1)
							{
								flag = true;
							}
							else
							{
								num2--;
							}
						}
						num++;
					}
					if (!flag)
					{
						return null;
					}
					arrayList.Add(text.Substring(0, num));
					text = text.Substring(num).TrimStart(new char[0]);
				}
				string text2;
				if ((text2 = match.Groups["filtercomp"].ToString()) != null)
				{
					if (!(text2 == "|"))
					{
						if (!(text2 == "&"))
						{
							if (!(text2 == "!"))
							{
								goto IL_0614;
							}
							adfilter.Type = ADFilter.FilterType.Not;
							ADFilter adfilter2 = FilterParser.ParseFilterString((string)arrayList[0]);
							if (arrayList.Count > 1 || adfilter2 == null)
							{
								return null;
							}
							adfilter.Filter.Not = adfilter2;
							return adfilter;
						}
						else
						{
							adfilter.Type = ADFilter.FilterType.And;
							adfilter.Filter.And = new ArrayList();
							foreach (object obj in arrayList)
							{
								string text3 = (string)obj;
								ADFilter adfilter2 = FilterParser.ParseFilterString(text3);
								if (adfilter2 == null)
								{
									return null;
								}
								adfilter.Filter.And.Add(adfilter2);
							}
							if (adfilter.Filter.And.Count < 1)
							{
								return null;
							}
							return adfilter;
						}
					}
					else
					{
						adfilter.Type = ADFilter.FilterType.Or;
						adfilter.Filter.Or = new ArrayList();
						foreach (object obj2 in arrayList)
						{
							string text4 = (string)obj2;
							ADFilter adfilter2 = FilterParser.ParseFilterString(text4);
							if (adfilter2 == null)
							{
								return null;
							}
							adfilter.Filter.Or.Add(adfilter2);
						}
						if (adfilter.Filter.Or.Count < 1)
						{
							return null;
						}
						return adfilter;
					}
					ADFilter adfilter3;
					return adfilter3;
				}
				IL_0614:
				return null;
			}
			if (match.Groups["present"].ToString().Length != 0)
			{
				adfilter.Type = ADFilter.FilterType.Present;
				adfilter.Filter.Present = match.Groups["presentattr"].ToString();
			}
			else
			{
				if (match.Groups["simple"].ToString().Length != 0)
				{
					ADAttribute adattribute = new ADAttribute();
					if (match.Groups["simplevalue"].ToString().Length != 0)
					{
						ADValue advalue = FilterParser.StringFilterValueToADValue(match.Groups["simplevalue"].ToString());
						adattribute.Values.Add(advalue);
					}
					adattribute.Name = match.Groups["simpleattr"].ToString();
					string text5;
					if ((text5 = match.Groups["filtertype"].ToString()) != null)
					{
						if (text5 == "=")
						{
							adfilter.Type = ADFilter.FilterType.EqualityMatch;
							adfilter.Filter.EqualityMatch = adattribute;
							return adfilter;
						}
						if (text5 == "~=")
						{
							adfilter.Type = ADFilter.FilterType.ApproxMatch;
							adfilter.Filter.ApproxMatch = adattribute;
							return adfilter;
						}
						if (text5 == "<=")
						{
							adfilter.Type = ADFilter.FilterType.LessOrEqual;
							adfilter.Filter.LessOrEqual = adattribute;
							return adfilter;
						}
						if (text5 == ">=")
						{
							adfilter.Type = ADFilter.FilterType.GreaterOrEqual;
							adfilter.Filter.GreaterOrEqual = adattribute;
							return adfilter;
						}
					}
					return null;
				}
				if (match.Groups["substr"].ToString().Length != 0)
				{
					adfilter.Type = ADFilter.FilterType.Substrings;
					ADSubstringFilter adsubstringFilter = new ADSubstringFilter();
					adsubstringFilter.Initial = FilterParser.StringFilterValueToADValue(match.Groups["initialvalue"].ToString());
					adsubstringFilter.Final = FilterParser.StringFilterValueToADValue(match.Groups["finalvalue"].ToString());
					if (match.Groups["anyvalue"].ToString().Length != 0)
					{
						foreach (object obj3 in match.Groups["anyvalue"].Captures)
						{
							Capture capture = (Capture)obj3;
							adsubstringFilter.Any.Add(FilterParser.StringFilterValueToADValue(capture.ToString()));
						}
					}
					adsubstringFilter.Name = match.Groups["substrattr"].ToString();
					adfilter.Filter.Substrings = adsubstringFilter;
				}
				else
				{
					if (match.Groups["extensible"].ToString().Length == 0)
					{
						return null;
					}
					adfilter.Type = ADFilter.FilterType.ExtensibleMatch;
					adfilter.Filter.ExtensibleMatch = new ADExtenMatchFilter
					{
						Value = FilterParser.StringFilterValueToADValue(match.Groups["extenvalue"].ToString()),
						DNAttributes = (match.Groups["dnattr"].ToString().Length != 0),
						Name = match.Groups["extenattr"].ToString(),
						MatchingRule = match.Groups["matchrule"].ToString()
					};
				}
			}
			return adfilter;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000A228 File Offset: 0x00009228
		protected static ADValue StringFilterValueToADValue(string strVal)
		{
			if (strVal == null || strVal.Length == 0)
			{
				return null;
			}
			ADValue advalue = new ADValue();
			string[] array = strVal.Split(new char[] { '\\' });
			if (array.Length == 1)
			{
				advalue.IsBinary = false;
				advalue.StringVal = strVal;
				advalue.BinaryVal = null;
			}
			else
			{
				ArrayList arrayList = new ArrayList(array.Length);
				UTF8Encoding utf8Encoding = new UTF8Encoding();
				advalue.IsBinary = true;
				advalue.StringVal = null;
				if (array[0].Length != 0)
				{
					arrayList.Add(utf8Encoding.GetBytes(array[0]));
				}
				for (int i = 1; i < array.Length; i++)
				{
					string text = array[i].Substring(0, 2);
					arrayList.Add(new byte[] { byte.Parse(text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture) });
					if (array[i].Length > 2)
					{
						arrayList.Add(utf8Encoding.GetBytes(array[i].Substring(2)));
					}
				}
				int num = 0;
				foreach (object obj in arrayList)
				{
					byte[] array2 = (byte[])obj;
					num += array2.Length;
				}
				advalue.BinaryVal = new byte[num];
				int num2 = 0;
				foreach (object obj2 in arrayList)
				{
					byte[] array3 = (byte[])obj2;
					array3.CopyTo(advalue.BinaryVal, num2);
					num2 += array3.Length;
				}
			}
			return advalue;
		}

		// Token: 0x04000202 RID: 514
		private const string mAttrRE = "(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*";

		// Token: 0x04000203 RID: 515
		private const string mValueRE = "(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?";

		// Token: 0x04000204 RID: 516
		private const string mExtenAttrRE = "(?<extenattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*";

		// Token: 0x04000205 RID: 517
		private const string mExtenValueRE = "(?<extenvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*";

		// Token: 0x04000206 RID: 518
		private const string mDNAttrRE = "(?<dnattr>\\:dn){0,1}\\s*";

		// Token: 0x04000207 RID: 519
		private const string mMatchRuleOptionalRE = "(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+))){0,1}\\s*";

		// Token: 0x04000208 RID: 520
		private const string mMatchRuleRE = "(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+)))\\s*";

		// Token: 0x04000209 RID: 521
		private const string mExtenRE = "(?<extensible>(((?<extenattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+))){0,1}\\s*)|((?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+)))\\s*))\\:\\=\\s*(?<extenvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*";

		// Token: 0x0400020A RID: 522
		private const string mSubstrAttrRE = "(?<substrattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*";

		// Token: 0x0400020B RID: 523
		private const string mInitialRE = "\\s*(?<initialvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*";

		// Token: 0x0400020C RID: 524
		private const string mFinalRE = "\\s*(?<finalvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*";

		// Token: 0x0400020D RID: 525
		private const string mAnyRE = "(\\*\\s*((?<anyvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\*\\s*)*)";

		// Token: 0x0400020E RID: 526
		private const string mSubstrRE = "(?<substr>(?<substrattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*\\=\\s*\\s*(?<initialvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*(\\*\\s*((?<anyvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\*\\s*)*)\\s*(?<finalvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*)\\s*";

		// Token: 0x0400020F RID: 527
		private const string mSimpleValueRE = "(?<simplevalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*";

		// Token: 0x04000210 RID: 528
		private const string mSimpleAttrRE = "(?<simpleattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*";

		// Token: 0x04000211 RID: 529
		private const string mFiltertypeRE = "(?<filtertype>\\=|\\~\\=|\\>\\=|\\<\\=)\\s*";

		// Token: 0x04000212 RID: 530
		private const string mSimpleRE = "(?<simple>(?<simpleattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<filtertype>\\=|\\~\\=|\\>\\=|\\<\\=)\\s*(?<simplevalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*";

		// Token: 0x04000213 RID: 531
		private const string mPresentRE = "(?<present>(?<presentattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\=\\*)\\s*";

		// Token: 0x04000214 RID: 532
		private const string mItemRE = "(?<item>(?<simple>(?<simpleattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<filtertype>\\=|\\~\\=|\\>\\=|\\<\\=)\\s*(?<simplevalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*|(?<present>(?<presentattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\=\\*)\\s*|(?<substr>(?<substrattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*\\=\\s*\\s*(?<initialvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*(\\*\\s*((?<anyvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\*\\s*)*)\\s*(?<finalvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*)\\s*|(?<extensible>(((?<extenattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+))){0,1}\\s*)|((?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+)))\\s*))\\:\\=\\s*(?<extenvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*)\\s*";

		// Token: 0x04000215 RID: 533
		private const string mFiltercompRE = "(?<filtercomp>\\!|\\&|\\|)\\s*";

		// Token: 0x04000216 RID: 534
		private const string mFilterlistRE = "(?<filterlist>.+)\\s*";

		// Token: 0x04000217 RID: 535
		private const string mFilterRE = "^\\s*\\(\\s*(((?<filtercomp>\\!|\\&|\\|)\\s*(?<filterlist>.+)\\s*)|((?<item>(?<simple>(?<simpleattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<filtertype>\\=|\\~\\=|\\>\\=|\\<\\=)\\s*(?<simplevalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*|(?<present>(?<presentattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\=\\*)\\s*|(?<substr>(?<substrattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*\\=\\s*\\s*(?<initialvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*(\\*\\s*((?<anyvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\*\\s*)*)\\s*(?<finalvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*)\\s*|(?<extensible>(((?<extenattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+))){0,1}\\s*)|((?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+)))\\s*))\\:\\=\\s*(?<extenvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*)\\s*))\\)\\s*$";

		// Token: 0x04000218 RID: 536
		private static Regex mFilter = new Regex("^\\s*\\(\\s*(((?<filtercomp>\\!|\\&|\\|)\\s*(?<filterlist>.+)\\s*)|((?<item>(?<simple>(?<simpleattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<filtertype>\\=|\\~\\=|\\>\\=|\\<\\=)\\s*(?<simplevalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*|(?<present>(?<presentattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\=\\*)\\s*|(?<substr>(?<substrattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*\\=\\s*\\s*(?<initialvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*(\\*\\s*((?<anyvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\*\\s*)*)\\s*(?<finalvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?){0,1}\\s*)\\s*|(?<extensible>(((?<extenattr>(([0-2](\\.[0-9]+)+)|([a-zA-Z]+([a-zA-Z0-9]|[-])*))(;([a-zA-Z0-9]|[-])+)*)\\s*(?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+))){0,1}\\s*)|((?<dnattr>\\:dn){0,1}\\s*(\\:(?<matchrule>([a-zA-Z][a-zA-Z0-9]*)|([0-9]+(\\.[0-9]+)+)))\\s*))\\:\\=\\s*(?<extenvalue>(([^\\*\\(\\)\\\\])|(\\\\[a-fA-F0-9][a-fA-F0-9]))+?)\\s*)\\s*)\\s*))\\)\\s*$", RegexOptions.ExplicitCapture);
	}
}
