using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200015F RID: 351
	internal class NumberAction : ContainerAction
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x0004AAD0 File Offset: 0x00049AD0
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string text = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Level))
			{
				if (text != "any" && text != "multiple" && text != "single")
				{
					throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "level", text });
				}
				this.level = text;
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Count))
			{
				this.countPattern = text;
				this.countKey = compiler.AddQuery(text, true, true, true);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.From))
			{
				this.from = text;
				this.fromKey = compiler.AddQuery(text, true, true, true);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Value))
			{
				this.value = text;
				this.valueKey = compiler.AddQuery(text);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Format))
			{
				this.formatAvt = Avt.CompileAvt(compiler, text);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Lang))
			{
				this.langAvt = Avt.CompileAvt(compiler, text);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.LetterValue))
			{
				this.letterAvt = Avt.CompileAvt(compiler, text);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.GroupingSeparator))
			{
				this.groupingSepAvt = Avt.CompileAvt(compiler, text);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.GroupingSize))
				{
					return false;
				}
				this.groupingSizeAvt = Avt.CompileAvt(compiler, text);
			}
			return true;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0004AC94 File Offset: 0x00049C94
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckEmpty(compiler);
			this.forwardCompatibility = compiler.ForwardCompatibility;
			this.formatTokens = NumberAction.ParseFormat(CompiledAction.PrecalculateAvt(ref this.formatAvt));
			this.letter = this.ParseLetter(CompiledAction.PrecalculateAvt(ref this.letterAvt));
			this.lang = CompiledAction.PrecalculateAvt(ref this.langAvt);
			this.groupingSep = CompiledAction.PrecalculateAvt(ref this.groupingSepAvt);
			if (this.groupingSep != null && this.groupingSep.Length > 1)
			{
				throw XsltException.Create("Xslt_CharAttribute", new string[] { "grouping-separator" });
			}
			this.groupingSize = CompiledAction.PrecalculateAvt(ref this.groupingSizeAvt);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0004AD4C File Offset: 0x00049D4C
		private int numberAny(Processor processor, ActionFrame frame)
		{
			int num = 0;
			XPathNavigator xpathNavigator = frame.Node;
			if (xpathNavigator.NodeType == XPathNodeType.Attribute || xpathNavigator.NodeType == XPathNodeType.Namespace)
			{
				xpathNavigator = xpathNavigator.Clone();
				xpathNavigator.MoveToParent();
			}
			XPathNavigator xpathNavigator2 = xpathNavigator.Clone();
			if (this.fromKey != -1)
			{
				bool flag = false;
				while (!processor.Matches(xpathNavigator2, this.fromKey))
				{
					if (!xpathNavigator2.MoveToParent())
					{
						IL_0056:
						XPathNodeIterator xpathNodeIterator = xpathNavigator2.SelectDescendants(XPathNodeType.All, true);
						while (xpathNodeIterator.MoveNext())
						{
							if (processor.Matches(xpathNodeIterator.Current, this.fromKey))
							{
								flag = true;
								num = 0;
							}
							else if (this.MatchCountKey(processor, frame.Node, xpathNodeIterator.Current))
							{
								num++;
							}
							if (xpathNodeIterator.Current.IsSamePosition(xpathNavigator))
							{
								break;
							}
						}
						if (!flag)
						{
							return 0;
						}
						return num;
					}
				}
				flag = true;
				goto IL_0056;
			}
			xpathNavigator2.MoveToRoot();
			XPathNodeIterator xpathNodeIterator2 = xpathNavigator2.SelectDescendants(XPathNodeType.All, true);
			while (xpathNodeIterator2.MoveNext())
			{
				if (this.MatchCountKey(processor, frame.Node, xpathNodeIterator2.Current))
				{
					num++;
				}
				if (xpathNodeIterator2.Current.IsSamePosition(xpathNavigator))
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0004AE56 File Offset: 0x00049E56
		private bool checkFrom(Processor processor, XPathNavigator nav)
		{
			if (this.fromKey == -1)
			{
				return true;
			}
			while (!processor.Matches(nav, this.fromKey))
			{
				if (!nav.MoveToParent())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0004AE7D File Offset: 0x00049E7D
		private bool moveToCount(XPathNavigator nav, Processor processor, XPathNavigator contextNode)
		{
			while (this.fromKey == -1 || !processor.Matches(nav, this.fromKey))
			{
				if (this.MatchCountKey(processor, contextNode, nav))
				{
					return true;
				}
				if (!nav.MoveToParent())
				{
					return false;
				}
			}
			return false;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0004AEB0 File Offset: 0x00049EB0
		private int numberCount(XPathNavigator nav, Processor processor, XPathNavigator contextNode)
		{
			XPathNavigator xpathNavigator = nav.Clone();
			int num = 1;
			if (xpathNavigator.MoveToParent())
			{
				xpathNavigator.MoveToFirstChild();
				while (!xpathNavigator.IsSamePosition(nav))
				{
					if (this.MatchCountKey(processor, contextNode, xpathNavigator))
					{
						num++;
					}
					if (!xpathNavigator.MoveToNext())
					{
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0004AEF8 File Offset: 0x00049EF8
		private static object SimplifyValue(object value)
		{
			if (Type.GetTypeCode(value.GetType()) == TypeCode.Object)
			{
				XPathNodeIterator xpathNodeIterator = value as XPathNodeIterator;
				if (xpathNodeIterator != null)
				{
					if (xpathNodeIterator.MoveNext())
					{
						return xpathNodeIterator.Current.Value;
					}
					return string.Empty;
				}
				else
				{
					XPathNavigator xpathNavigator = value as XPathNavigator;
					if (xpathNavigator != null)
					{
						return xpathNavigator.Value;
					}
				}
			}
			return value;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0004AF4C File Offset: 0x00049F4C
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			ArrayList numberList = processor.NumberList;
			switch (frame.State)
			{
			case 0:
				numberList.Clear();
				if (this.valueKey != -1)
				{
					numberList.Add(NumberAction.SimplifyValue(processor.Evaluate(frame, this.valueKey)));
				}
				else if (this.level == "any")
				{
					int num = this.numberAny(processor, frame);
					if (num != 0)
					{
						numberList.Add(num);
					}
				}
				else
				{
					bool flag = this.level == "multiple";
					XPathNavigator node = frame.Node;
					XPathNavigator xpathNavigator = frame.Node.Clone();
					if (xpathNavigator.NodeType == XPathNodeType.Attribute || xpathNavigator.NodeType == XPathNodeType.Namespace)
					{
						xpathNavigator.MoveToParent();
					}
					while (this.moveToCount(xpathNavigator, processor, node))
					{
						numberList.Insert(0, this.numberCount(xpathNavigator, processor, node));
						if (!flag || !xpathNavigator.MoveToParent())
						{
							break;
						}
					}
					if (!this.checkFrom(processor, xpathNavigator))
					{
						numberList.Clear();
					}
				}
				frame.StoredOutput = NumberAction.Format(numberList, (this.formatAvt == null) ? this.formatTokens : NumberAction.ParseFormat(this.formatAvt.Evaluate(processor, frame)), (this.langAvt == null) ? this.lang : this.langAvt.Evaluate(processor, frame), (this.letterAvt == null) ? this.letter : this.ParseLetter(this.letterAvt.Evaluate(processor, frame)), (this.groupingSepAvt == null) ? this.groupingSep : this.groupingSepAvt.Evaluate(processor, frame), (this.groupingSizeAvt == null) ? this.groupingSize : this.groupingSizeAvt.Evaluate(processor, frame));
				break;
			case 1:
				return;
			case 2:
				break;
			default:
				return;
			}
			if (!processor.TextEvent(frame.StoredOutput))
			{
				frame.State = 2;
				return;
			}
			frame.Finished();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0004B124 File Offset: 0x0004A124
		private bool MatchCountKey(Processor processor, XPathNavigator contextNode, XPathNavigator nav)
		{
			if (this.countKey != -1)
			{
				return processor.Matches(nav, this.countKey);
			}
			return contextNode.Name == nav.Name && this.BasicNodeType(contextNode.NodeType) == this.BasicNodeType(nav.NodeType);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0004B178 File Offset: 0x0004A178
		private XPathNodeType BasicNodeType(XPathNodeType type)
		{
			if (type == XPathNodeType.SignificantWhitespace || type == XPathNodeType.Whitespace)
			{
				return XPathNodeType.Text;
			}
			return type;
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0004B188 File Offset: 0x0004A188
		private static string Format(ArrayList numberlist, List<NumberAction.FormatInfo> tokens, string lang, string letter, string groupingSep, string groupingSize)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			if (tokens != null)
			{
				num = tokens.Count;
			}
			NumberAction.NumberingFormat numberingFormat = new NumberAction.NumberingFormat();
			if (groupingSize != null)
			{
				try
				{
					numberingFormat.setGroupingSize(Convert.ToInt32(groupingSize, CultureInfo.InvariantCulture));
				}
				catch (FormatException)
				{
				}
				catch (OverflowException)
				{
				}
			}
			if (groupingSep != null)
			{
				int length = groupingSep.Length;
				numberingFormat.setGroupingSeparator(groupingSep);
			}
			if (0 < num)
			{
				NumberAction.FormatInfo formatInfo = tokens[0];
				NumberAction.FormatInfo formatInfo2 = null;
				if (num % 2 == 1)
				{
					formatInfo2 = tokens[num - 1];
					num--;
				}
				NumberAction.FormatInfo formatInfo3 = ((2 < num) ? tokens[num - 2] : NumberAction.DefaultSeparator);
				NumberAction.FormatInfo formatInfo4 = ((0 < num) ? tokens[num - 1] : NumberAction.DefaultFormat);
				if (formatInfo != null)
				{
					stringBuilder.Append(formatInfo.formatString);
				}
				int count = numberlist.Count;
				for (int i = 0; i < count; i++)
				{
					int num2 = i * 2;
					bool flag = num2 < num;
					if (0 < i)
					{
						NumberAction.FormatInfo formatInfo5 = (flag ? tokens[num2] : formatInfo3);
						stringBuilder.Append(formatInfo5.formatString);
					}
					NumberAction.FormatInfo formatInfo6 = (flag ? tokens[num2 + 1] : formatInfo4);
					numberingFormat.setNumberingType(formatInfo6.numSequence);
					numberingFormat.setMinLen(formatInfo6.length);
					stringBuilder.Append(numberingFormat.FormatItem(numberlist[i]));
				}
				if (formatInfo2 != null)
				{
					stringBuilder.Append(formatInfo2.formatString);
				}
			}
			else
			{
				numberingFormat.setNumberingType(NumberingSequence.FirstDecimal);
				for (int j = 0; j < numberlist.Count; j++)
				{
					if (j != 0)
					{
						stringBuilder.Append(".");
					}
					stringBuilder.Append(numberingFormat.FormatItem(numberlist[j]));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0004B34C File Offset: 0x0004A34C
		private static void mapFormatToken(string wsToken, int startLen, int tokLen, out NumberingSequence seq, out int pminlen)
		{
			char c = wsToken[startLen];
			bool flag = false;
			pminlen = 1;
			seq = NumberingSequence.Nil;
			int num = (int)c;
			if (num <= 2406)
			{
				if (num != 48 && num != 2406)
				{
					goto IL_0070;
				}
			}
			else if (num != 3664 && num != 51067 && num != 65296)
			{
				goto IL_0070;
			}
			do
			{
				pminlen++;
			}
			while (--tokLen > 0 && c == wsToken[++startLen]);
			if (wsToken[startLen] != c + '\u0001')
			{
				flag = true;
			}
			IL_0070:
			if (!flag)
			{
				int num2 = (int)wsToken[startLen];
				if (num2 <= 3585)
				{
					if (num2 <= 1040)
					{
						if (num2 <= 73)
						{
							if (num2 == 49)
							{
								seq = NumberingSequence.FirstDecimal;
								goto IL_0307;
							}
							if (num2 == 65)
							{
								seq = NumberingSequence.FirstAlpha;
								goto IL_0307;
							}
							if (num2 == 73)
							{
								seq = NumberingSequence.FirstSpecial;
								goto IL_0307;
							}
						}
						else
						{
							if (num2 == 97)
							{
								seq = NumberingSequence.LCLetter;
								goto IL_0307;
							}
							if (num2 == 105)
							{
								seq = NumberingSequence.LCRoman;
								goto IL_0307;
							}
							if (num2 == 1040)
							{
								seq = NumberingSequence.UCRus;
								goto IL_0307;
							}
						}
					}
					else if (num2 <= 1571)
					{
						if (num2 == 1072)
						{
							seq = NumberingSequence.LCRus;
							goto IL_0307;
						}
						if (num2 == 1488)
						{
							seq = NumberingSequence.Hebrew;
							goto IL_0307;
						}
						if (num2 == 1571)
						{
							seq = NumberingSequence.ArabicScript;
							goto IL_0307;
						}
					}
					else if (num2 <= 2325)
					{
						if (num2 == 2309)
						{
							seq = NumberingSequence.Hindi2;
							goto IL_0307;
						}
						if (num2 == 2325)
						{
							seq = NumberingSequence.Hindi1;
							goto IL_0307;
						}
					}
					else
					{
						if (num2 == 2407)
						{
							seq = NumberingSequence.Hindi3;
							goto IL_0307;
						}
						if (num2 == 3585)
						{
							seq = NumberingSequence.Thai1;
							goto IL_0307;
						}
					}
				}
				else if (num2 <= 22777)
				{
					if (num2 <= 12593)
					{
						if (num2 == 3665)
						{
							seq = NumberingSequence.Thai2;
							goto IL_0307;
						}
						switch (num2)
						{
						case 12450:
							seq = NumberingSequence.DAiueo;
							goto IL_0307;
						case 12451:
							break;
						case 12452:
							seq = NumberingSequence.DIroha;
							goto IL_0307;
						default:
							if (num2 == 12593)
							{
								seq = NumberingSequence.DChosung;
								goto IL_0307;
							}
							break;
						}
					}
					else
					{
						if (num2 == 19968)
						{
							seq = NumberingSequence.FEDecimal;
							goto IL_0307;
						}
						if (num2 == 22769)
						{
							seq = NumberingSequence.DbNum3;
							goto IL_0307;
						}
						if (num2 == 22777)
						{
							seq = NumberingSequence.ChnCmplx;
							goto IL_0307;
						}
					}
				}
				else if (num2 <= 44032)
				{
					if (num2 == 23376)
					{
						seq = NumberingSequence.Zodiac2;
						goto IL_0307;
					}
					if (num2 != 30002)
					{
						if (num2 == 44032)
						{
							seq = NumberingSequence.Ganada;
							goto IL_0307;
						}
					}
					else
					{
						if (tokLen > 1 && wsToken[startLen + 1] == '子')
						{
							seq = NumberingSequence.Zodiac3;
							tokLen--;
							startLen++;
							goto IL_0307;
						}
						seq = NumberingSequence.Zodiac1;
						goto IL_0307;
					}
				}
				else if (num2 <= 54616)
				{
					if (num2 == 51068)
					{
						seq = NumberingSequence.KorDbNum1;
						goto IL_0307;
					}
					if (num2 == 54616)
					{
						seq = NumberingSequence.KorDbNum3;
						goto IL_0307;
					}
				}
				else
				{
					if (num2 == 65297)
					{
						seq = NumberingSequence.DArabic;
						goto IL_0307;
					}
					switch (num2)
					{
					case 65393:
						seq = NumberingSequence.Aiueo;
						goto IL_0307;
					case 65394:
						seq = NumberingSequence.Iroha;
						goto IL_0307;
					}
				}
				seq = NumberingSequence.FirstDecimal;
			}
			IL_0307:
			if (flag)
			{
				seq = NumberingSequence.FirstDecimal;
				pminlen = 0;
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0004B66C File Offset: 0x0004A66C
		private static List<NumberAction.FormatInfo> ParseFormat(string formatString)
		{
			if (formatString == null || formatString.Length == 0)
			{
				return null;
			}
			int i = 0;
			bool flag = CharUtil.IsAlphaNumeric(formatString[i]);
			List<NumberAction.FormatInfo> list = new List<NumberAction.FormatInfo>();
			int num = 0;
			if (flag)
			{
				list.Add(null);
			}
			while (i <= formatString.Length)
			{
				bool flag2 = ((i < formatString.Length) ? CharUtil.IsAlphaNumeric(formatString[i]) : (!flag));
				if (flag != flag2)
				{
					NumberAction.FormatInfo formatInfo = new NumberAction.FormatInfo();
					if (flag)
					{
						NumberAction.mapFormatToken(formatString, num, i - num, out formatInfo.numSequence, out formatInfo.length);
					}
					else
					{
						formatInfo.isSeparator = true;
						formatInfo.formatString = formatString.Substring(num, i - num);
					}
					num = i;
					i++;
					list.Add(formatInfo);
					flag = flag2;
				}
				else
				{
					i++;
				}
			}
			return list;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0004B72C File Offset: 0x0004A72C
		private string ParseLetter(string letter)
		{
			if (letter == null || letter == "traditional" || letter == "alphabetic")
			{
				return letter;
			}
			if (!this.forwardCompatibility)
			{
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "letter-value", letter });
			}
			return null;
		}

		// Token: 0x0400098E RID: 2446
		private const long msofnfcNil = 0L;

		// Token: 0x0400098F RID: 2447
		private const long msofnfcTraditional = 1L;

		// Token: 0x04000990 RID: 2448
		private const long msofnfcAlwaysFormat = 2L;

		// Token: 0x04000991 RID: 2449
		private const int cchMaxFormat = 63;

		// Token: 0x04000992 RID: 2450
		private const int cchMaxFormatDecimal = 11;

		// Token: 0x04000993 RID: 2451
		private const int OutputNumber = 2;

		// Token: 0x04000994 RID: 2452
		private static NumberAction.FormatInfo DefaultFormat = new NumberAction.FormatInfo(false, "0");

		// Token: 0x04000995 RID: 2453
		private static NumberAction.FormatInfo DefaultSeparator = new NumberAction.FormatInfo(true, ".");

		// Token: 0x04000996 RID: 2454
		private string level;

		// Token: 0x04000997 RID: 2455
		private string countPattern;

		// Token: 0x04000998 RID: 2456
		private int countKey = -1;

		// Token: 0x04000999 RID: 2457
		private string from;

		// Token: 0x0400099A RID: 2458
		private int fromKey = -1;

		// Token: 0x0400099B RID: 2459
		private string value;

		// Token: 0x0400099C RID: 2460
		private int valueKey = -1;

		// Token: 0x0400099D RID: 2461
		private Avt formatAvt;

		// Token: 0x0400099E RID: 2462
		private Avt langAvt;

		// Token: 0x0400099F RID: 2463
		private Avt letterAvt;

		// Token: 0x040009A0 RID: 2464
		private Avt groupingSepAvt;

		// Token: 0x040009A1 RID: 2465
		private Avt groupingSizeAvt;

		// Token: 0x040009A2 RID: 2466
		private List<NumberAction.FormatInfo> formatTokens;

		// Token: 0x040009A3 RID: 2467
		private string lang;

		// Token: 0x040009A4 RID: 2468
		private string letter;

		// Token: 0x040009A5 RID: 2469
		private string groupingSep;

		// Token: 0x040009A6 RID: 2470
		private string groupingSize;

		// Token: 0x040009A7 RID: 2471
		private bool forwardCompatibility;

		// Token: 0x02000160 RID: 352
		internal class FormatInfo
		{
			// Token: 0x06000EF1 RID: 3825 RVA: 0x0004B7BF File Offset: 0x0004A7BF
			public FormatInfo(bool isSeparator, string formatString)
			{
				this.isSeparator = isSeparator;
				this.formatString = formatString;
			}

			// Token: 0x06000EF2 RID: 3826 RVA: 0x0004B7D5 File Offset: 0x0004A7D5
			public FormatInfo()
			{
			}

			// Token: 0x040009A8 RID: 2472
			public bool isSeparator;

			// Token: 0x040009A9 RID: 2473
			public NumberingSequence numSequence;

			// Token: 0x040009AA RID: 2474
			public int length;

			// Token: 0x040009AB RID: 2475
			public string formatString;
		}

		// Token: 0x02000161 RID: 353
		private class NumberingFormat : NumberFormatterBase
		{
			// Token: 0x06000EF3 RID: 3827 RVA: 0x0004B7DD File Offset: 0x0004A7DD
			internal NumberingFormat()
			{
			}

			// Token: 0x06000EF4 RID: 3828 RVA: 0x0004B7E5 File Offset: 0x0004A7E5
			internal void setNumberingType(NumberingSequence seq)
			{
				this.seq = seq;
			}

			// Token: 0x06000EF5 RID: 3829 RVA: 0x0004B7EE File Offset: 0x0004A7EE
			internal void setMinLen(int cMinLen)
			{
				this.cMinLen = cMinLen;
			}

			// Token: 0x06000EF6 RID: 3830 RVA: 0x0004B7F7 File Offset: 0x0004A7F7
			internal void setGroupingSeparator(string separator)
			{
				this.separator = separator;
			}

			// Token: 0x06000EF7 RID: 3831 RVA: 0x0004B800 File Offset: 0x0004A800
			internal void setGroupingSize(int sizeGroup)
			{
				if (0 <= sizeGroup && sizeGroup <= 9)
				{
					this.sizeGroup = sizeGroup;
				}
			}

			// Token: 0x06000EF8 RID: 3832 RVA: 0x0004B814 File Offset: 0x0004A814
			internal string FormatItem(object value)
			{
				double num;
				if (value is int)
				{
					num = (double)((int)value);
				}
				else
				{
					num = XmlConvert.ToXPathDouble(value);
					if (0.5 > num || double.IsPositiveInfinity(num))
					{
						return XmlConvert.ToXPathString(value);
					}
					num = XmlConvert.XPathRound(num);
				}
				NumberingSequence numberingSequence = this.seq;
				if (numberingSequence != NumberingSequence.FirstDecimal)
				{
					switch (numberingSequence)
					{
					case NumberingSequence.FirstAlpha:
					case NumberingSequence.LCLetter:
						if (num <= 2147483647.0)
						{
							StringBuilder stringBuilder = new StringBuilder();
							NumberFormatterBase.ConvertToAlphabetic(stringBuilder, num, (this.seq == NumberingSequence.FirstAlpha) ? 'A' : 'a', 26);
							return stringBuilder.ToString();
						}
						break;
					default:
						switch (numberingSequence)
						{
						case NumberingSequence.FirstSpecial:
						case NumberingSequence.LCRoman:
							if (num <= 32767.0)
							{
								StringBuilder stringBuilder2 = new StringBuilder();
								NumberFormatterBase.ConvertToRoman(stringBuilder2, num, this.seq == NumberingSequence.FirstSpecial);
								return stringBuilder2.ToString();
							}
							break;
						}
						break;
					}
				}
				return NumberAction.NumberingFormat.ConvertToArabic(num, this.cMinLen, this.sizeGroup, this.separator);
			}

			// Token: 0x06000EF9 RID: 3833 RVA: 0x0004B900 File Offset: 0x0004A900
			private static string ConvertToArabic(double val, int minLength, int groupSize, string groupSeparator)
			{
				string text;
				if (groupSize != 0 && groupSeparator != null)
				{
					NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
					numberFormatInfo.NumberGroupSizes = new int[] { groupSize };
					numberFormatInfo.NumberGroupSeparator = groupSeparator;
					if (Math.Floor(val) == val)
					{
						numberFormatInfo.NumberDecimalDigits = 0;
					}
					text = val.ToString("N", numberFormatInfo);
				}
				else
				{
					text = Convert.ToString(val, CultureInfo.InvariantCulture);
				}
				if (text.Length >= minLength)
				{
					return text;
				}
				StringBuilder stringBuilder = new StringBuilder(minLength);
				stringBuilder.Append('0', minLength - text.Length);
				stringBuilder.Append(text);
				return stringBuilder.ToString();
			}

			// Token: 0x040009AC RID: 2476
			private NumberingSequence seq;

			// Token: 0x040009AD RID: 2477
			private int cMinLen;

			// Token: 0x040009AE RID: 2478
			private string separator;

			// Token: 0x040009AF RID: 2479
			private int sizeGroup;
		}
	}
}
