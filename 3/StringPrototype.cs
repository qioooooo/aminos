using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000DE RID: 222
	public class StringPrototype : StringObject
	{
		// Token: 0x060009DA RID: 2522 RVA: 0x0004ADAB File Offset: 0x00049DAB
		internal StringPrototype(FunctionPrototype funcprot, ObjectPrototype parent)
			: base(parent, "")
		{
			this.noExpando = true;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0004ADC0 File Offset: 0x00049DC0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_anchor)]
		public static string anchor(object thisob, object anchorName)
		{
			string text = Convert.ToString(thisob);
			string text2 = Convert.ToString(anchorName);
			return string.Concat(new string[] { "<A NAME=\"", text2, "\">", text, "</A>" });
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0004AE08 File Offset: 0x00049E08
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_big)]
		public static string big(object thisob)
		{
			return "<BIG>" + Convert.ToString(thisob) + "</BIG>";
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0004AE1F File Offset: 0x00049E1F
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_blink)]
		public static string blink(object thisob)
		{
			return "<BLINK>" + Convert.ToString(thisob) + "</BLINK>";
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0004AE36 File Offset: 0x00049E36
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_bold)]
		public static string bold(object thisob)
		{
			return "<B>" + Convert.ToString(thisob) + "</B>";
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0004AE50 File Offset: 0x00049E50
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_charAt)]
		public static string charAt(object thisob, double pos)
		{
			string text = Convert.ToString(thisob);
			double num = Convert.ToInteger(pos);
			if (num < 0.0 || num >= (double)text.Length)
			{
				return "";
			}
			return text.Substring((int)num, 1);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0004AE90 File Offset: 0x00049E90
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_charCodeAt)]
		public static object charCodeAt(object thisob, double pos)
		{
			string text = Convert.ToString(thisob);
			double num = Convert.ToInteger(pos);
			if (num < 0.0 || num >= (double)text.Length)
			{
				return double.NaN;
			}
			return (int)text[(int)num];
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0004AEE0 File Offset: 0x00049EE0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.String_concat)]
		public static string concat(object thisob, params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder(Convert.ToString(thisob));
			for (int i = 0; i < args.Length; i++)
			{
				stringBuilder.Append(Convert.ToString(args[i]));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060009E2 RID: 2530 RVA: 0x0004AF1C File Offset: 0x00049F1C
		public static StringConstructor constructor
		{
			get
			{
				return StringPrototype._constructor;
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0004AF23 File Offset: 0x00049F23
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_fixed)]
		public static string @fixed(object thisob)
		{
			return "<TT>" + Convert.ToString(thisob) + "</TT>";
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0004AF3C File Offset: 0x00049F3C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_fontcolor)]
		public static string fontcolor(object thisob, object colorName)
		{
			string text = Convert.ToString(thisob);
			string text2 = Convert.ToString(thisob);
			return string.Concat(new string[] { "<FONT COLOR=\"", text2, "\">", text, "</FONT>" });
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0004AF84 File Offset: 0x00049F84
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_fontsize)]
		public static string fontsize(object thisob, object fontSize)
		{
			string text = Convert.ToString(thisob);
			string text2 = Convert.ToString(fontSize);
			return string.Concat(new string[] { "<FONT SIZE=\"", text2, "\">", text, "</FONT>" });
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0004AFCC File Offset: 0x00049FCC
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_indexOf)]
		public static int indexOf(object thisob, object searchString, double position)
		{
			string text = Convert.ToString(thisob);
			string text2 = Convert.ToString(searchString);
			double num = Convert.ToInteger(position);
			int length = text.Length;
			if (num < 0.0)
			{
				num = 0.0;
			}
			if (num < (double)length)
			{
				return text.IndexOf(text2, (int)num);
			}
			if (text2.Length != 0)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0004B025 File Offset: 0x0004A025
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_italics)]
		public static string italics(object thisob)
		{
			return "<I>" + Convert.ToString(thisob) + "</I>";
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0004B03C File Offset: 0x0004A03C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_lastIndexOf)]
		public static int lastIndexOf(object thisob, object searchString, double position)
		{
			string text = Convert.ToString(thisob);
			string text2 = Convert.ToString(searchString);
			int length = text.Length;
			int num = ((position != position || position > (double)length) ? length : ((int)position));
			if (num < 0)
			{
				num = 0;
			}
			if (num >= length)
			{
				num = length;
			}
			int length2 = text2.Length;
			if (length2 == 0)
			{
				return num;
			}
			int num2 = num - 1 + length2;
			if (num2 >= length)
			{
				num2 = length - 1;
			}
			if (num2 < 0)
			{
				return -1;
			}
			return text.LastIndexOf(text2, num2);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0004B0AC File Offset: 0x0004A0AC
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_link)]
		public static string link(object thisob, object linkRef)
		{
			string text = Convert.ToString(thisob);
			string text2 = Convert.ToString(linkRef);
			return string.Concat(new string[] { "<A HREF=\"", text2, "\">", text, "</A>" });
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0004B0F4 File Offset: 0x0004A0F4
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_localeCompare)]
		public static int localeCompare(object thisob, object thatob)
		{
			return string.Compare(Convert.ToString(thisob), Convert.ToString(thatob), StringComparison.CurrentCulture);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0004B108 File Offset: 0x0004A108
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.String_match)]
		public static object match(object thisob, VsaEngine engine, object regExp)
		{
			string text = Convert.ToString(thisob);
			RegExpObject regExpObject = StringPrototype.ToRegExpObject(regExp, engine);
			if (!regExpObject.globalInt)
			{
				Match match = regExpObject.regex.Match(text);
				if (!match.Success)
				{
					regExpObject.lastIndexInt = 0;
					return DBNull.Value;
				}
				if (regExpObject.regExpConst != null)
				{
					regExpObject.lastIndexInt = regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match, text);
					return new RegExpMatch(regExpObject.regExpConst.arrayPrototype, regExpObject.regex, match, text);
				}
				return new RegExpMatch(engine.Globals.globalObject.originalRegExp.arrayPrototype, regExpObject.regex, match, text);
			}
			else
			{
				MatchCollection matchCollection = regExpObject.regex.Matches(text);
				if (matchCollection.Count == 0)
				{
					regExpObject.lastIndexInt = 0;
					return DBNull.Value;
				}
				Match match = matchCollection[matchCollection.Count - 1];
				regExpObject.lastIndexInt = regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match, text);
				return new RegExpMatch(regExpObject.regExpConst.arrayPrototype, regExpObject.regex, matchCollection, text);
			}
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0004B224 File Offset: 0x0004A224
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_replace)]
		public static string replace(object thisob, object regExp, object replacement)
		{
			string text = Convert.ToString(thisob);
			RegExpObject regExpObject = regExp as RegExpObject;
			if (regExpObject != null)
			{
				return StringPrototype.ReplaceWithRegExp(text, regExpObject, replacement);
			}
			Regex regex = regExp as Regex;
			if (regex != null)
			{
				return StringPrototype.ReplaceWithRegExp(text, new RegExpObject(regex), replacement);
			}
			return StringPrototype.ReplaceWithString(text, Convert.ToString(regExp), Convert.ToString(replacement));
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0004B278 File Offset: 0x0004A278
		private static string ReplaceWithRegExp(string thisob, RegExpObject regExpObject, object replacement)
		{
			RegExpReplace regExpReplace = ((replacement is ScriptFunction) ? new ReplaceUsingFunction(regExpObject.regex, (ScriptFunction)replacement, thisob) : new ReplaceWithString(Convert.ToString(replacement)));
			MatchEvaluator matchEvaluator = new MatchEvaluator(regExpReplace.Evaluate);
			string text = (regExpObject.globalInt ? regExpObject.regex.Replace(thisob, matchEvaluator) : regExpObject.regex.Replace(thisob, matchEvaluator, 1));
			regExpObject.lastIndexInt = ((regExpReplace.lastMatch == null) ? 0 : regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, regExpReplace.lastMatch, thisob));
			return text;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0004B310 File Offset: 0x0004A310
		private static string ReplaceWithString(string thisob, string searchString, string replaceString)
		{
			int num = thisob.IndexOf(searchString);
			if (num < 0)
			{
				return thisob;
			}
			StringBuilder stringBuilder = new StringBuilder(thisob.Substring(0, num));
			stringBuilder.Append(replaceString);
			stringBuilder.Append(thisob.Substring(num + searchString.Length));
			return stringBuilder.ToString();
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0004B35C File Offset: 0x0004A35C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.String_search)]
		public static int search(object thisob, VsaEngine engine, object regExp)
		{
			string text = Convert.ToString(thisob);
			RegExpObject regExpObject = StringPrototype.ToRegExpObject(regExp, engine);
			Match match = regExpObject.regex.Match(text);
			if (!match.Success)
			{
				regExpObject.lastIndexInt = 0;
				return -1;
			}
			regExpObject.lastIndexInt = regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match, text);
			return match.Index;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0004B3C0 File Offset: 0x0004A3C0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_slice)]
		public static string slice(object thisob, double start, object end)
		{
			string text = Convert.ToString(thisob);
			int length = text.Length;
			double num = Convert.ToInteger(start);
			double num2 = ((end == null || end is Missing) ? ((double)length) : Convert.ToInteger(end));
			if (num < 0.0)
			{
				num = (double)length + num;
				if (num < 0.0)
				{
					num = 0.0;
				}
			}
			else if (num > (double)length)
			{
				num = (double)length;
			}
			if (num2 < 0.0)
			{
				num2 = (double)length + num2;
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
			}
			else if (num2 > (double)length)
			{
				num2 = (double)length;
			}
			int num3 = (int)(num2 - num);
			if (num3 <= 0)
			{
				return "";
			}
			return text.Substring((int)num, num3);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0004B475 File Offset: 0x0004A475
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_small)]
		public static string small(object thisob)
		{
			return "<SMALL>" + Convert.ToString(thisob) + "</SMALL>";
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0004B48C File Offset: 0x0004A48C
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.String_split)]
		public static ArrayObject split(object thisob, VsaEngine engine, object separator, object limit)
		{
			string text = Convert.ToString(thisob);
			uint num = uint.MaxValue;
			if (limit != null && !(limit is Missing) && limit != DBNull.Value)
			{
				double num2 = Convert.ToInteger(limit);
				if (num2 >= 0.0 && num2 < 4294967295.0)
				{
					num = (uint)num2;
				}
			}
			if (num == 0U)
			{
				return engine.GetOriginalArrayConstructor().Construct();
			}
			if (separator == null || separator is Missing)
			{
				ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
				arrayObject.SetValueAtIndex(0U, thisob);
				return arrayObject;
			}
			RegExpObject regExpObject = separator as RegExpObject;
			if (regExpObject != null)
			{
				return StringPrototype.SplitWithRegExp(text, engine, regExpObject, num);
			}
			Regex regex = separator as Regex;
			if (regex != null)
			{
				return StringPrototype.SplitWithRegExp(text, engine, new RegExpObject(regex), num);
			}
			return StringPrototype.SplitWithString(text, engine, Convert.ToString(separator), num);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0004B548 File Offset: 0x0004A548
		private static ArrayObject SplitWithRegExp(string thisob, VsaEngine engine, RegExpObject regExpObject, uint limit)
		{
			ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
			Match match = regExpObject.regex.Match(thisob);
			if (!match.Success)
			{
				arrayObject.SetValueAtIndex(0U, thisob);
				regExpObject.lastIndexInt = 0;
				return arrayObject;
			}
			int num = 0;
			uint num2 = 0U;
			Match match2;
			for (;;)
			{
				int num3 = match.Index - num;
				if (num3 > 0)
				{
					arrayObject.SetValueAtIndex(num2++, thisob.Substring(num, num3));
					if (limit > 0U && num2 >= limit)
					{
						break;
					}
				}
				num = match.Index + match.Length;
				match2 = match;
				match = match.NextMatch();
				if (!match.Success)
				{
					goto Block_5;
				}
			}
			regExpObject.lastIndexInt = regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match, thisob);
			return arrayObject;
			Block_5:
			if (num < thisob.Length)
			{
				arrayObject.SetValueAtIndex(num2, thisob.Substring(num));
			}
			regExpObject.lastIndexInt = regExpObject.regExpConst.UpdateConstructor(regExpObject.regex, match2, thisob);
			return arrayObject;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0004B638 File Offset: 0x0004A638
		private static ArrayObject SplitWithString(string thisob, VsaEngine engine, string separator, uint limit)
		{
			ArrayObject arrayObject = engine.GetOriginalArrayConstructor().Construct();
			if (separator.Length == 0)
			{
				if ((ulong)limit > (ulong)((long)thisob.Length))
				{
					limit = (uint)thisob.Length;
				}
				int num = 0;
				while ((long)num < (long)((ulong)limit))
				{
					arrayObject.SetValueAtIndex((uint)num, thisob[num].ToString());
					num++;
				}
			}
			else
			{
				int num2 = 0;
				uint num3 = 0U;
				int num4;
				while ((num4 = thisob.IndexOf(separator, num2)) >= 0)
				{
					arrayObject.SetValueAtIndex(num3++, thisob.Substring(num2, num4 - num2));
					if (num3 >= limit)
					{
						return arrayObject;
					}
					num2 = num4 + separator.Length;
				}
				if (num3 == 0U)
				{
					arrayObject.SetValueAtIndex(0U, thisob);
				}
				else
				{
					arrayObject.SetValueAtIndex(num3, thisob.Substring(num2));
				}
			}
			return arrayObject;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0004B6E9 File Offset: 0x0004A6E9
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_strike)]
		public static string strike(object thisob)
		{
			return "<STRIKE>" + Convert.ToString(thisob) + "</STRIKE>";
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0004B700 File Offset: 0x0004A700
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_sub)]
		public static string sub(object thisob)
		{
			return "<SUB>" + Convert.ToString(thisob) + "</SUB>";
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0004B718 File Offset: 0x0004A718
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_substr)]
		[NotRecommended("substr")]
		public static string substr(object thisob, double start, object count)
		{
			string text = thisob as string;
			if (text == null)
			{
				text = Convert.ToString(thisob);
			}
			int length = text.Length;
			double num = Convert.ToInteger(start);
			if (num < 0.0)
			{
				num += (double)length;
			}
			if (num < 0.0)
			{
				num = 0.0;
			}
			else if (num > (double)length)
			{
				num = (double)length;
			}
			int num2 = ((count is int) ? ((int)count) : ((count == null || count is Missing) ? (length - (int)Runtime.DoubleToInt64(num)) : ((int)Runtime.DoubleToInt64(Convert.ToInteger(count)))));
			if (num + (double)num2 > (double)length)
			{
				num2 = length - (int)num;
			}
			if (num2 <= 0)
			{
				return "";
			}
			return text.Substring((int)num, num2);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0004B7C8 File Offset: 0x0004A7C8
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_substring)]
		public static string substring(object thisob, double start, object end)
		{
			string text = thisob as string;
			if (text == null)
			{
				text = Convert.ToString(thisob);
			}
			int length = text.Length;
			double num = Convert.ToInteger(start);
			if (num < 0.0)
			{
				num = 0.0;
			}
			else if (num > (double)length)
			{
				num = (double)length;
			}
			double num2 = ((end == null || end is Missing) ? ((double)length) : Convert.ToInteger(end));
			if (num2 < 0.0)
			{
				num2 = 0.0;
			}
			else if (num2 > (double)length)
			{
				num2 = (double)length;
			}
			if (num > num2)
			{
				double num3 = num;
				num = num2;
				num2 = num3;
			}
			return text.Substring((int)num, (int)(num2 - num));
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0004B862 File Offset: 0x0004A862
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_sup)]
		public static string sup(object thisob)
		{
			return "<SUP>" + Convert.ToString(thisob) + "</SUP>";
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0004B879 File Offset: 0x0004A879
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toLocaleLowerCase)]
		public static string toLocaleLowerCase(object thisob)
		{
			return Convert.ToString(thisob).ToLower(CultureInfo.CurrentUICulture);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0004B88B File Offset: 0x0004A88B
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toLocaleUpperCase)]
		public static string toLocaleUpperCase(object thisob)
		{
			return Convert.ToString(thisob).ToUpperInvariant();
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0004B898 File Offset: 0x0004A898
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toLowerCase)]
		public static string toLowerCase(object thisob)
		{
			return Convert.ToString(thisob).ToLowerInvariant();
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0004B8A8 File Offset: 0x0004A8A8
		private static RegExpObject ToRegExpObject(object regExp, VsaEngine engine)
		{
			if (regExp == null || regExp is Missing)
			{
				return (RegExpObject)engine.GetOriginalRegExpConstructor().Construct("", false, false, false);
			}
			RegExpObject regExpObject = regExp as RegExpObject;
			if (regExpObject != null)
			{
				return regExpObject;
			}
			Regex regex = regExp as Regex;
			if (regex != null)
			{
				return new RegExpObject(regex);
			}
			return (RegExpObject)engine.GetOriginalRegExpConstructor().Construct(Convert.ToString(regExp), false, false, false);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0004B910 File Offset: 0x0004A910
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toString)]
		public static string toString(object thisob)
		{
			StringObject stringObject = thisob as StringObject;
			if (stringObject != null)
			{
				return stringObject.value;
			}
			ConcatString concatString = thisob as ConcatString;
			if (concatString != null)
			{
				return concatString.ToString();
			}
			IConvertible iconvertible = Convert.GetIConvertible(thisob);
			if (Convert.GetTypeCode(thisob, iconvertible) == TypeCode.String)
			{
				return iconvertible.ToString(null);
			}
			throw new JScriptException(JSError.StringExpected);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0004B963 File Offset: 0x0004A963
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_toUpperCase)]
		public static string toUpperCase(object thisob)
		{
			return Convert.ToString(thisob).ToUpperInvariant();
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0004B970 File Offset: 0x0004A970
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.String_valueOf)]
		public static object valueOf(object thisob)
		{
			return StringPrototype.toString(thisob);
		}

		// Token: 0x0400062E RID: 1582
		internal static readonly StringPrototype ob = new StringPrototype(FunctionPrototype.ob, ObjectPrototype.ob);

		// Token: 0x0400062F RID: 1583
		internal static StringConstructor _constructor;
	}
}
