using System;
using System.Xml.XPath;
using MS.Internal.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200016A RID: 362
	internal class TemplateAction : TemplateBaseAction
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0004C77F File Offset: 0x0004B77F
		internal int MatchKey
		{
			get
			{
				return this.matchKey;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000F28 RID: 3880 RVA: 0x0004C787 File Offset: 0x0004B787
		internal XmlQualifiedName Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0004C78F File Offset: 0x0004B78F
		internal double Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000F2A RID: 3882 RVA: 0x0004C797 File Offset: 0x0004B797
		internal XmlQualifiedName Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0004C79F File Offset: 0x0004B79F
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x0004C7A7 File Offset: 0x0004B7A7
		internal int TemplateId
		{
			get
			{
				return this.templateId;
			}
			set
			{
				this.templateId = value;
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x0004C7B0 File Offset: 0x0004B7B0
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			if (this.matchKey == -1)
			{
				if (this.name == null)
				{
					throw XsltException.Create("Xslt_TemplateNoAttrib", new string[0]);
				}
				if (this.mode != null)
				{
					throw XsltException.Create("Xslt_InvalidModeAttribute", new string[0]);
				}
			}
			compiler.BeginTemplate(this);
			if (compiler.Recurse())
			{
				this.CompileParameters(compiler);
				base.CompileTemplate(compiler);
				compiler.ToParent();
			}
			compiler.EndTemplate();
			this.AnalyzePriority(compiler);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0004C83C File Offset: 0x0004B83C
		internal virtual void CompileSingle(Compiler compiler)
		{
			this.matchKey = compiler.AddQuery("/", false, true, true);
			this.priority = 0.5;
			base.CompileOnceTemplate(compiler);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0004C868 File Offset: 0x0004B868
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Match))
			{
				this.matchKey = compiler.AddQuery(value, false, true, true);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Name))
			{
				this.name = compiler.CreateXPathQName(value);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Priority))
			{
				this.priority = XmlConvert.ToXPathDouble(value);
				if (double.IsNaN(this.priority) && !compiler.ForwardCompatibility)
				{
					throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "priority", value });
				}
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.Mode))
				{
					return false;
				}
				if (compiler.AllowBuiltInMode && value == "*")
				{
					this.mode = Compiler.BuiltInMode;
				}
				else
				{
					this.mode = compiler.CreateXPathQName(value);
				}
			}
			return true;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0004C974 File Offset: 0x0004B974
		private void AnalyzePriority(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			if (!double.IsNaN(this.priority) || this.matchKey == -1)
			{
				return;
			}
			TheQuery theQuery = compiler.QueryStore[this.MatchKey];
			CompiledXpathExpr compiledQuery = theQuery.CompiledQuery;
			Query query = compiledQuery.QueryTree;
			UnionExpr unionExpr;
			while ((unionExpr = query as UnionExpr) != null)
			{
				TemplateAction templateAction = this.CloneWithoutName();
				compiler.QueryStore.Add(new TheQuery(new CompiledXpathExpr(unionExpr.qy2, compiledQuery.Expression, false), theQuery._ScopeManager));
				templateAction.matchKey = compiler.QueryStore.Count - 1;
				templateAction.priority = unionExpr.qy2.XsltDefaultPriority;
				compiler.AddTemplate(templateAction);
				query = unionExpr.qy1;
			}
			if (compiledQuery.QueryTree != query)
			{
				compiler.QueryStore[this.MatchKey] = new TheQuery(new CompiledXpathExpr(query, compiledQuery.Expression, false), theQuery._ScopeManager);
			}
			this.priority = query.XsltDefaultPriority;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x0004CA70 File Offset: 0x0004BA70
		protected void CompileParameters(Compiler compiler)
		{
			NavigatorInput input = compiler.Input;
			for (;;)
			{
				switch (input.NodeType)
				{
				case XPathNodeType.Element:
					if (!Keywords.Equals(input.NamespaceURI, input.Atoms.XsltNamespace) || !Keywords.Equals(input.LocalName, input.Atoms.Param))
					{
						return;
					}
					compiler.PushNamespaceScope();
					base.AddAction(compiler.CreateVariableAction(VariableType.LocalParameter));
					compiler.PopScope();
					break;
				case XPathNodeType.Text:
					return;
				case XPathNodeType.SignificantWhitespace:
					base.AddEvent(compiler.CreateTextEvent());
					break;
				}
				if (!input.Advance())
				{
					return;
				}
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x0004CB0C File Offset: 0x0004BB0C
		private TemplateAction CloneWithoutName()
		{
			return new TemplateAction
			{
				containedActions = this.containedActions,
				mode = this.mode,
				variableCount = this.variableCount,
				replaceNSAliasesDone = true
			};
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0004CB4B File Offset: 0x0004BB4B
		internal override void ReplaceNamespaceAlias(Compiler compiler)
		{
			if (!this.replaceNSAliasesDone)
			{
				base.ReplaceNamespaceAlias(compiler);
				this.replaceNSAliasesDone = true;
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x0004CB64 File Offset: 0x0004BB64
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			switch (frame.State)
			{
			case 0:
				if (this.variableCount > 0)
				{
					frame.AllocateVariables(this.variableCount);
				}
				if (this.containedActions != null && this.containedActions.Count > 0)
				{
					processor.PushActionFrame(frame);
					frame.State = 1;
					return;
				}
				frame.Finished();
				return;
			case 1:
				frame.Finished();
				return;
			default:
				return;
			}
		}

		// Token: 0x040009D4 RID: 2516
		private int matchKey = -1;

		// Token: 0x040009D5 RID: 2517
		private XmlQualifiedName name;

		// Token: 0x040009D6 RID: 2518
		private double priority = double.NaN;

		// Token: 0x040009D7 RID: 2519
		private XmlQualifiedName mode;

		// Token: 0x040009D8 RID: 2520
		private int templateId;

		// Token: 0x040009D9 RID: 2521
		private bool replaceNSAliasesDone;
	}
}
