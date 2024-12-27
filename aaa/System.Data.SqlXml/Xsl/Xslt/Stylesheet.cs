using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000104 RID: 260
	internal class Stylesheet
	{
		// Token: 0x06000BA8 RID: 2984 RVA: 0x0003C7A8 File Offset: 0x0003B7A8
		public Stylesheet(Compiler compiler, int importPrecedence)
		{
			this.compiler = compiler;
			this.importPrecedence = importPrecedence;
			this.WhitespaceRules[0] = new List<WhitespaceRule>();
			this.WhitespaceRules[1] = new List<WhitespaceRule>();
			this.WhitespaceRules[2] = new List<WhitespaceRule>();
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0003C83E File Offset: 0x0003B83E
		public int ImportPrecedence
		{
			get
			{
				return this.importPrecedence;
			}
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0003C846 File Offset: 0x0003B846
		public void AddWhitespaceRule(int index, WhitespaceRule rule)
		{
			this.WhitespaceRules[index].Add(rule);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0003C858 File Offset: 0x0003B858
		public bool AddVarPar(VarPar var)
		{
			foreach (XslNode xslNode in this.GlobalVarPars)
			{
				if (xslNode.Name.Equals(var.Name))
				{
					return this.compiler.AllGlobalVarPars.ContainsKey(var.Name);
				}
			}
			this.GlobalVarPars.Add(var);
			return true;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0003C8E0 File Offset: 0x0003B8E0
		public bool AddTemplate(Template template)
		{
			template.ImportPrecedence = this.importPrecedence;
			template.OrderNumber = this.orderNumber++;
			this.compiler.AllTemplates.Add(template);
			if (template.Name != null)
			{
				Template template2;
				if (!this.compiler.NamedTemplates.TryGetValue(template.Name, out template2))
				{
					this.compiler.NamedTemplates[template.Name] = template;
				}
				else if (template2.ImportPrecedence == template.ImportPrecedence)
				{
					return false;
				}
			}
			if (template.Match != null)
			{
				this.Templates.Add(template);
			}
			return true;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0003C984 File Offset: 0x0003B984
		public void AddTemplateMatch(Template template, QilLoop filter)
		{
			List<TemplateMatch> list;
			if (!this.TemplateMatches.TryGetValue(template.Mode, out list))
			{
				list = (this.TemplateMatches[template.Mode] = new List<TemplateMatch>());
			}
			list.Add(new TemplateMatch(template, filter));
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0003C9D0 File Offset: 0x0003B9D0
		public void SortTemplateMatches()
		{
			foreach (QilName qilName in this.TemplateMatches.Keys)
			{
				this.TemplateMatches[qilName].Sort(TemplateMatch.Comparer);
			}
		}

		// Token: 0x0400080F RID: 2063
		private Compiler compiler;

		// Token: 0x04000810 RID: 2064
		public List<Uri> ImportHrefs = new List<Uri>();

		// Token: 0x04000811 RID: 2065
		public Stylesheet[] Imports;

		// Token: 0x04000812 RID: 2066
		public List<Template> Templates = new List<Template>();

		// Token: 0x04000813 RID: 2067
		public List<XslNode> GlobalVarPars = new List<XslNode>();

		// Token: 0x04000814 RID: 2068
		public Dictionary<QilName, AttributeSet> AttributeSets = new Dictionary<QilName, AttributeSet>();

		// Token: 0x04000815 RID: 2069
		public Dictionary<QilName, List<TemplateMatch>> TemplateMatches = new Dictionary<QilName, List<TemplateMatch>>();

		// Token: 0x04000816 RID: 2070
		public Dictionary<QilName, List<QilFunction>> ApplyImportsFunctions = new Dictionary<QilName, List<QilFunction>>();

		// Token: 0x04000817 RID: 2071
		private int importPrecedence;

		// Token: 0x04000818 RID: 2072
		private int orderNumber;

		// Token: 0x04000819 RID: 2073
		public List<WhitespaceRule>[] WhitespaceRules = new List<WhitespaceRule>[3];
	}
}
