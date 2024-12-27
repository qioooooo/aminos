using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000F9 RID: 249
	internal class MatcherBuilder
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x00034F2C File Offset: 0x00033F2C
		public MatcherBuilder(XPathQilFactory f, ReferenceReplacer refReplacer, InvokeGenerator invkGen)
		{
			this.f = f;
			this.refReplacer = refReplacer;
			this.invkGen = invkGen;
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00034FB4 File Offset: 0x00033FB4
		private void Clear()
		{
			this.priority = -1;
			this.elementPatterns.Clear();
			this.attributePatterns.Clear();
			this.textPatterns.Clear();
			this.documentPatterns.Clear();
			this.commentPatterns.Clear();
			this.piPatterns.Clear();
			this.heterogenousPatterns.Clear();
			this.allMatches.Clear();
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00035020 File Offset: 0x00034020
		private void AddPatterns(List<TemplateMatch> matches)
		{
			foreach (TemplateMatch templateMatch in matches)
			{
				Pattern pattern = new Pattern(templateMatch, ++this.priority);
				XmlNodeKindFlags nodeKind = templateMatch.NodeKind;
				if (nodeKind <= XmlNodeKindFlags.Text)
				{
					switch (nodeKind)
					{
					case XmlNodeKindFlags.Document:
						this.documentPatterns.Add(pattern);
						continue;
					case XmlNodeKindFlags.Element:
						this.elementPatterns.Add(pattern);
						continue;
					case XmlNodeKindFlags.Document | XmlNodeKindFlags.Element:
						break;
					case XmlNodeKindFlags.Attribute:
						this.attributePatterns.Add(pattern);
						continue;
					default:
						if (nodeKind == XmlNodeKindFlags.Text)
						{
							this.textPatterns.Add(pattern);
							continue;
						}
						break;
					}
				}
				else
				{
					if (nodeKind == XmlNodeKindFlags.Comment)
					{
						this.commentPatterns.Add(pattern);
						continue;
					}
					if (nodeKind == XmlNodeKindFlags.PI)
					{
						this.piPatterns.Add(pattern);
						continue;
					}
				}
				this.heterogenousPatterns.Add(pattern);
			}
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00035124 File Offset: 0x00034124
		private void CollectPatterns(Stylesheet sheet, QilName mode)
		{
			foreach (Stylesheet stylesheet in sheet.Imports)
			{
				this.CollectPatterns(stylesheet, mode);
			}
			List<TemplateMatch> list;
			if (sheet.TemplateMatches.TryGetValue(mode, out list))
			{
				this.AddPatterns(list);
				this.allMatches.Add(list);
			}
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00035178 File Offset: 0x00034178
		public void CollectPatterns(Stylesheet sheet, QilName mode, bool applyImports)
		{
			this.Clear();
			if (applyImports)
			{
				foreach (Stylesheet stylesheet in sheet.Imports)
				{
					this.CollectPatterns(stylesheet, mode);
				}
				return;
			}
			this.CollectPatterns(sheet, mode);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x000351B8 File Offset: 0x000341B8
		private QilNode MatchPattern(QilIterator it, TemplateMatch match)
		{
			QilNode qilNode = match.Condition;
			if (qilNode == null)
			{
				return this.f.True();
			}
			qilNode = qilNode.DeepClone(this.f.BaseFactory);
			return this.refReplacer.Replace(qilNode, match.Iterator, it);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00035200 File Offset: 0x00034200
		private QilNode MatchPatterns(QilIterator it, List<Pattern> patternList)
		{
			QilNode qilNode = this.f.Int32(-1);
			foreach (Pattern pattern in patternList)
			{
				qilNode = this.f.Conditional(this.MatchPattern(it, pattern.Match), this.f.Int32(pattern.Priority), qilNode);
			}
			return qilNode;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00035284 File Offset: 0x00034284
		private QilNode MatchPatterns(QilIterator it, XmlQueryType xt, List<Pattern> patternList, QilNode otherwise)
		{
			if (patternList.Count == 0)
			{
				return otherwise;
			}
			return this.f.Conditional(this.f.IsType(it, xt), this.MatchPatterns(it, patternList), otherwise);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000352B3 File Offset: 0x000342B3
		private bool IsNoMatch(QilNode matcher)
		{
			return matcher.NodeType == QilNodeType.LiteralInt32;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x000352C4 File Offset: 0x000342C4
		private QilNode MatchPatternsWhosePriorityGreater(QilIterator it, List<Pattern> patternList, QilNode matcher)
		{
			if (patternList.Count == 0)
			{
				return matcher;
			}
			if (this.IsNoMatch(matcher))
			{
				return this.MatchPatterns(it, patternList);
			}
			QilIterator qilIterator = this.f.Let(matcher);
			QilNode qilNode = this.f.Int32(-1);
			int num = -1;
			foreach (Pattern pattern in patternList)
			{
				if (pattern.Priority > num + 1)
				{
					qilNode = this.f.Conditional(this.f.Gt(qilIterator, this.f.Int32(num)), qilIterator, qilNode);
				}
				qilNode = this.f.Conditional(this.MatchPattern(it, pattern.Match), this.f.Int32(pattern.Priority), qilNode);
				num = pattern.Priority;
			}
			if (num != this.priority)
			{
				qilNode = this.f.Conditional(this.f.Gt(qilIterator, this.f.Int32(num)), qilIterator, qilNode);
			}
			return this.f.Loop(qilIterator, qilNode);
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000353E8 File Offset: 0x000343E8
		private QilNode MatchPatterns(QilIterator it, XmlQueryType xt, PatternBag patternBag, QilNode otherwise)
		{
			if (patternBag.FixedNamePatternsNames.Count == 0)
			{
				return this.MatchPatterns(it, xt, patternBag.NonFixedNamePatterns, otherwise);
			}
			QilNode qilNode = this.f.Int32(-1);
			foreach (QilName qilName in patternBag.FixedNamePatternsNames)
			{
				qilNode = this.f.Conditional(this.f.Eq(this.f.NameOf(it), qilName.ShallowClone(this.f.BaseFactory)), this.MatchPatterns(it, patternBag.FixedNamePatterns[qilName]), qilNode);
			}
			qilNode = this.MatchPatternsWhosePriorityGreater(it, patternBag.NonFixedNamePatterns, qilNode);
			return this.f.Conditional(this.f.IsType(it, xt), qilNode, otherwise);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x000354D4 File Offset: 0x000344D4
		public QilNode BuildMatcher(QilIterator it, IList<XslNode> actualArgs, QilNode otherwise)
		{
			QilNode qilNode = this.f.Int32(-1);
			qilNode = this.MatchPatterns(it, XmlQueryTypeFactory.PI, this.piPatterns, qilNode);
			qilNode = this.MatchPatterns(it, XmlQueryTypeFactory.Comment, this.commentPatterns, qilNode);
			qilNode = this.MatchPatterns(it, XmlQueryTypeFactory.Document, this.documentPatterns, qilNode);
			qilNode = this.MatchPatterns(it, XmlQueryTypeFactory.Text, this.textPatterns, qilNode);
			qilNode = this.MatchPatterns(it, XmlQueryTypeFactory.Attribute, this.attributePatterns, qilNode);
			qilNode = this.MatchPatterns(it, XmlQueryTypeFactory.Element, this.elementPatterns, qilNode);
			qilNode = this.MatchPatternsWhosePriorityGreater(it, this.heterogenousPatterns, qilNode);
			if (this.IsNoMatch(qilNode))
			{
				return otherwise;
			}
			QilNode[] array = new QilNode[this.priority + 2];
			int num = -1;
			foreach (List<TemplateMatch> list in this.allMatches)
			{
				foreach (TemplateMatch templateMatch in list)
				{
					array[++num] = this.invkGen.GenerateInvoke(templateMatch.TemplateFunction, actualArgs);
				}
			}
			array[++num] = otherwise;
			return this.f.Choice(qilNode, this.f.BranchList(array));
		}

		// Token: 0x040007AC RID: 1964
		private const int NoMatch = -1;

		// Token: 0x040007AD RID: 1965
		private XPathQilFactory f;

		// Token: 0x040007AE RID: 1966
		private ReferenceReplacer refReplacer;

		// Token: 0x040007AF RID: 1967
		private InvokeGenerator invkGen;

		// Token: 0x040007B0 RID: 1968
		private int priority = -1;

		// Token: 0x040007B1 RID: 1969
		private PatternBag elementPatterns = new PatternBag();

		// Token: 0x040007B2 RID: 1970
		private PatternBag attributePatterns = new PatternBag();

		// Token: 0x040007B3 RID: 1971
		private List<Pattern> textPatterns = new List<Pattern>();

		// Token: 0x040007B4 RID: 1972
		private List<Pattern> documentPatterns = new List<Pattern>();

		// Token: 0x040007B5 RID: 1973
		private List<Pattern> commentPatterns = new List<Pattern>();

		// Token: 0x040007B6 RID: 1974
		private PatternBag piPatterns = new PatternBag();

		// Token: 0x040007B7 RID: 1975
		private List<Pattern> heterogenousPatterns = new List<Pattern>();

		// Token: 0x040007B8 RID: 1976
		private List<List<TemplateMatch>> allMatches = new List<List<TemplateMatch>>();
	}
}
