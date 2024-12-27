using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000199 RID: 409
	internal class TemplateManager
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x00054391 File Offset: 0x00053391
		internal XmlQualifiedName Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x00054399 File Offset: 0x00053399
		internal TemplateManager(Stylesheet stylesheet, XmlQualifiedName mode)
		{
			this.mode = mode;
			this.stylesheet = stylesheet;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000543AF File Offset: 0x000533AF
		internal void AddTemplate(TemplateAction template)
		{
			if (this.templates == null)
			{
				this.templates = new ArrayList();
			}
			this.templates.Add(template);
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x000543D1 File Offset: 0x000533D1
		internal void ProcessTemplates()
		{
			if (this.templates != null)
			{
				this.templates.Sort(TemplateManager.s_TemplateComparer);
			}
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000543EC File Offset: 0x000533EC
		internal TemplateAction FindTemplate(Processor processor, XPathNavigator navigator)
		{
			if (this.templates == null)
			{
				return null;
			}
			for (int i = this.templates.Count - 1; i >= 0; i--)
			{
				TemplateAction templateAction = (TemplateAction)this.templates[i];
				int matchKey = templateAction.MatchKey;
				if (matchKey != -1 && processor.Matches(navigator, matchKey))
				{
					return templateAction;
				}
			}
			return null;
		}

		// Token: 0x04000BC6 RID: 3014
		private XmlQualifiedName mode;

		// Token: 0x04000BC7 RID: 3015
		internal ArrayList templates;

		// Token: 0x04000BC8 RID: 3016
		private Stylesheet stylesheet;

		// Token: 0x04000BC9 RID: 3017
		private static TemplateManager.TemplateComparer s_TemplateComparer = new TemplateManager.TemplateComparer();

		// Token: 0x0200019A RID: 410
		private class TemplateComparer : IComparer
		{
			// Token: 0x06001174 RID: 4468 RVA: 0x00054454 File Offset: 0x00053454
			public int Compare(object x, object y)
			{
				TemplateAction templateAction = (TemplateAction)x;
				TemplateAction templateAction2 = (TemplateAction)y;
				if (templateAction.Priority == templateAction2.Priority)
				{
					return templateAction.TemplateId - templateAction2.TemplateId;
				}
				if (templateAction.Priority <= templateAction2.Priority)
				{
					return -1;
				}
				return 1;
			}
		}
	}
}
