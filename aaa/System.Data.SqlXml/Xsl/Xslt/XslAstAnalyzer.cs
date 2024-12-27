using System;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200011C RID: 284
	internal class XslAstAnalyzer : XslVisitor<XslFlags>
	{
		// Token: 0x06000C38 RID: 3128 RVA: 0x0003DB74 File Offset: 0x0003CB74
		public XslFlags Analyze(Compiler compiler)
		{
			this.compiler = compiler;
			this.scope = new CompilerScopeManager<VarPar>();
			this.xpathAnalyzer = new XslAstAnalyzer.XPathAnalyzer(compiler, this.scope);
			foreach (VarPar varPar in compiler.ExternalPars)
			{
				this.scope.AddVariable(varPar.Name, varPar);
			}
			foreach (VarPar varPar2 in compiler.GlobalVars)
			{
				this.scope.AddVariable(varPar2.Name, varPar2);
			}
			foreach (VarPar varPar3 in compiler.ExternalPars)
			{
				this.Visit(varPar3);
				varPar3.Flags |= XslFlags.TypeFilter;
			}
			foreach (VarPar varPar4 in compiler.GlobalVars)
			{
				this.Visit(varPar4);
			}
			XslFlags xslFlags = XslFlags.None;
			foreach (ProtoTemplate protoTemplate in compiler.AllTemplates)
			{
				this.currentTemplate = protoTemplate;
				xslFlags |= this.Visit(protoTemplate);
			}
			foreach (ProtoTemplate protoTemplate2 in compiler.AllTemplates)
			{
				foreach (XslNode xslNode in protoTemplate2.Content)
				{
					if (xslNode.NodeType != XslNodeType.Text)
					{
						if (xslNode.NodeType != XslNodeType.Param)
						{
							break;
						}
						VarPar varPar5 = (VarPar)xslNode;
						if ((varPar5.Flags & XslFlags.MayBeDefault) != XslFlags.None)
						{
							varPar5.Flags |= varPar5.DefValueFlags;
						}
					}
				}
			}
			for (int num = 32; num != 0; num >>= 1)
			{
				this.dataFlow.PropagateFlag((XslFlags)num);
			}
			this.dataFlow = null;
			foreach (KeyValuePair<Template, Stylesheet> keyValuePair in this.dependsOnApplyImports)
			{
				this.AddImportDependencies(compiler.PrincipalStylesheet, keyValuePair.Key);
			}
			this.dependsOnApplyImports = null;
			if ((xslFlags & XslFlags.Current) != XslFlags.None)
			{
				this.focusDonors.PropagateFlag(XslFlags.Current);
			}
			if ((xslFlags & XslFlags.Position) != XslFlags.None)
			{
				this.focusDonors.PropagateFlag(XslFlags.Position);
			}
			if ((xslFlags & XslFlags.Last) != XslFlags.None)
			{
				this.focusDonors.PropagateFlag(XslFlags.Last);
			}
			if ((xslFlags & XslFlags.SideEffects) != XslFlags.None)
			{
				this.PropagateSideEffectsFlag();
			}
			this.focusDonors = null;
			this.sideEffectDonors = null;
			this.dependsOnMode = null;
			this.FillModeFlags(compiler.PrincipalStylesheet);
			this.TraceResults();
			return xslFlags;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0003DEF4 File Offset: 0x0003CEF4
		private void AddImportDependencies(Stylesheet sheet, Template focusDonor)
		{
			foreach (Template template in sheet.Templates)
			{
				if (template.Mode.Equals(focusDonor.Mode))
				{
					this.focusDonors.AddEdge(template, focusDonor);
				}
			}
			foreach (Stylesheet stylesheet in sheet.Imports)
			{
				this.AddImportDependencies(stylesheet, focusDonor);
			}
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0003DF88 File Offset: 0x0003CF88
		private void FillModeFlags(Stylesheet sheet)
		{
			foreach (Template template in sheet.Templates)
			{
				XslFlags xslFlags = template.Flags & XslFlags.FocusFilter;
				if (xslFlags != XslFlags.None)
				{
					XslFlags xslFlags2;
					if (!this.compiler.ModeFlags.TryGetValue(template.Mode, out xslFlags2))
					{
						xslFlags2 = XslFlags.None;
					}
					this.compiler.ModeFlags[template.Mode] = xslFlags2 | xslFlags;
				}
			}
			foreach (Stylesheet stylesheet in sheet.Imports)
			{
				this.FillModeFlags(stylesheet);
			}
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x0003E044 File Offset: 0x0003D044
		private void TraceResults()
		{
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x0003E048 File Offset: 0x0003D048
		protected override XslFlags Visit(XslNode node)
		{
			this.scope.PushScope();
			for (NsDecl nsDecl = node.Namespaces; nsDecl != null; nsDecl = nsDecl.Prev)
			{
				this.scope.AddNamespace(nsDecl.Prefix, nsDecl.NsUri);
			}
			XslFlags xslFlags = base.Visit(node);
			this.scope.PopScope();
			if (this.currentTemplate != null && (node.NodeType == XslNodeType.Variable || node.NodeType == XslNodeType.Param))
			{
				this.scope.AddVariable(node.Name, (VarPar)node);
			}
			return xslFlags;
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x0003E0D4 File Offset: 0x0003D0D4
		protected override XslFlags VisitChildren(XslNode node)
		{
			XslFlags xslFlags = XslFlags.None;
			foreach (XslNode xslNode in node.Content)
			{
				xslFlags |= this.Visit(xslNode);
			}
			return xslFlags;
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0003E128 File Offset: 0x0003D128
		protected override XslFlags VisitAttributeSet(AttributeSet node)
		{
			node.Flags = this.VisitChildren(node);
			return node.Flags;
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x0003E13D File Offset: 0x0003D13D
		protected override XslFlags VisitTemplate(Template node)
		{
			node.Flags = this.VisitChildren(node);
			return node.Flags;
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0003E152 File Offset: 0x0003D152
		protected override XslFlags VisitApplyImports(XslNode node)
		{
			this.dependsOnApplyImports[(Template)this.currentTemplate] = (Stylesheet)node.Arg;
			return XslFlags.Rtf | XslFlags.Current | XslFlags.HasCalls;
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0003E17C File Offset: 0x0003D17C
		protected override XslFlags VisitApplyTemplates(XslNode node)
		{
			XslFlags xslFlags = this.ProcessExpr(node.Select);
			foreach (XslNode xslNode in node.Content)
			{
				xslFlags |= this.Visit(xslNode);
				if (xslNode.NodeType == XslNodeType.WithParam)
				{
					XslAstAnalyzer.ModeName modeName = new XslAstAnalyzer.ModeName(node.Name, xslNode.Name);
					VarPar varPar;
					if (!this.applyTemplatesParams.TryGetValue(modeName, out varPar))
					{
						varPar = (this.applyTemplatesParams[modeName] = AstFactory.WithParam(xslNode.Name));
					}
					if (this.typeDonor != null)
					{
						this.dataFlow.AddEdge(this.typeDonor, varPar);
					}
					else
					{
						varPar.Flags |= xslNode.Flags & XslFlags.TypeFilter;
					}
				}
			}
			if (this.currentTemplate != null)
			{
				this.AddApplyTemplatesEdge(node.Name, this.currentTemplate);
			}
			return XslFlags.Rtf | XslFlags.HasCalls | xslFlags;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x0003E284 File Offset: 0x0003D284
		protected override XslFlags VisitAttribute(NodeCtor node)
		{
			return XslFlags.Rtf | this.ProcessAvt(node.NameAvt) | this.ProcessAvt(node.NsAvt) | this.VisitChildren(node);
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x0003E2AC File Offset: 0x0003D2AC
		protected override XslFlags VisitCallTemplate(XslNode node)
		{
			XslFlags xslFlags = XslFlags.None;
			Template template;
			if (this.compiler.NamedTemplates.TryGetValue(node.Name, out template) && this.currentTemplate != null)
			{
				if (this.forEachDepth == 0)
				{
					this.focusDonors.AddEdge(template, this.currentTemplate);
				}
				else
				{
					this.sideEffectDonors.AddEdge(template, this.currentTemplate);
				}
			}
			VarPar[] array = new VarPar[node.Content.Count];
			int num = 0;
			foreach (XslNode xslNode in node.Content)
			{
				xslFlags |= this.Visit(xslNode);
				array[num++] = this.typeDonor;
			}
			if (template != null)
			{
				foreach (XslNode xslNode2 in template.Content)
				{
					if (xslNode2.NodeType != XslNodeType.Text)
					{
						if (xslNode2.NodeType != XslNodeType.Param)
						{
							break;
						}
						VarPar varPar = (VarPar)xslNode2;
						VarPar varPar2 = null;
						num = 0;
						foreach (XslNode xslNode3 in node.Content)
						{
							if (xslNode3.Name.Equals(varPar.Name))
							{
								varPar2 = (VarPar)xslNode3;
								this.typeDonor = array[num];
								break;
							}
							num++;
						}
						if (varPar2 != null)
						{
							if (this.typeDonor != null)
							{
								this.dataFlow.AddEdge(this.typeDonor, varPar);
							}
							else
							{
								varPar.Flags |= varPar2.Flags & XslFlags.TypeFilter;
							}
						}
						else
						{
							varPar.Flags |= XslFlags.MayBeDefault;
						}
					}
				}
			}
			return XslFlags.Rtf | XslFlags.HasCalls | xslFlags;
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x0003E4A4 File Offset: 0x0003D4A4
		protected override XslFlags VisitComment(XslNode node)
		{
			return XslFlags.Rtf | this.VisitChildren(node);
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x0003E4B0 File Offset: 0x0003D4B0
		protected override XslFlags VisitCopy(XslNode node)
		{
			return XslFlags.Rtf | XslFlags.Current | this.VisitChildren(node);
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x0003E4BF File Offset: 0x0003D4BF
		protected override XslFlags VisitCopyOf(XslNode node)
		{
			return XslFlags.Rtf | this.ProcessExpr(node.Select);
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x0003E4D0 File Offset: 0x0003D4D0
		protected override XslFlags VisitElement(NodeCtor node)
		{
			return XslFlags.Rtf | this.ProcessAvt(node.NameAvt) | this.ProcessAvt(node.NsAvt) | this.VisitChildren(node);
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003E4F6 File Offset: 0x0003D4F6
		protected override XslFlags VisitError(XslNode node)
		{
			return (this.VisitChildren(node) & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf)) | XslFlags.SideEffects;
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x0003E508 File Offset: 0x0003D508
		protected override XslFlags VisitForEach(XslNode node)
		{
			XslFlags xslFlags = this.ProcessExpr(node.Select);
			this.forEachDepth++;
			foreach (XslNode xslNode in node.Content)
			{
				if (xslNode.NodeType == XslNodeType.Sort)
				{
					xslFlags |= this.Visit(xslNode);
				}
				else
				{
					xslFlags |= this.Visit(xslNode) & ~(XslFlags.Current | XslFlags.Position | XslFlags.Last);
				}
			}
			this.forEachDepth--;
			return xslFlags;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003E5A0 File Offset: 0x0003D5A0
		protected override XslFlags VisitIf(XslNode node)
		{
			return this.ProcessExpr(node.Select) | this.VisitChildren(node);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x0003E5B6 File Offset: 0x0003D5B6
		protected override XslFlags VisitLiteralAttribute(XslNode node)
		{
			return XslFlags.Rtf | this.ProcessAvt(node.Select) | this.VisitChildren(node);
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x0003E5CF File Offset: 0x0003D5CF
		protected override XslFlags VisitLiteralElement(XslNode node)
		{
			return XslFlags.Rtf | this.VisitChildren(node);
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x0003E5DB File Offset: 0x0003D5DB
		protected override XslFlags VisitMessage(XslNode node)
		{
			return (this.VisitChildren(node) & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf)) | XslFlags.SideEffects;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0003E5F0 File Offset: 0x0003D5F0
		protected override XslFlags VisitNumber(Number node)
		{
			return XslFlags.Rtf | this.ProcessPattern(node.Count) | this.ProcessPattern(node.From) | ((node.Value != null) ? this.ProcessExpr(node.Value) : XslFlags.Current) | this.ProcessAvt(node.Format) | this.ProcessAvt(node.Lang) | this.ProcessAvt(node.LetterValue) | this.ProcessAvt(node.GroupingSeparator) | this.ProcessAvt(node.GroupingSize);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0003E676 File Offset: 0x0003D676
		protected override XslFlags VisitPI(XslNode node)
		{
			return XslFlags.Rtf | this.ProcessAvt(node.Select) | this.VisitChildren(node);
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0003E690 File Offset: 0x0003D690
		protected override XslFlags VisitSort(Sort node)
		{
			return (this.ProcessExpr(node.Select) & ~(XslFlags.Current | XslFlags.Position | XslFlags.Last)) | this.ProcessAvt(node.Lang) | this.ProcessAvt(node.DataType) | this.ProcessAvt(node.Order) | this.ProcessAvt(node.CaseOrder);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0003E6E3 File Offset: 0x0003D6E3
		protected override XslFlags VisitText(Text node)
		{
			return XslFlags.Rtf | this.VisitChildren(node);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x0003E6F0 File Offset: 0x0003D6F0
		protected override XslFlags VisitUseAttributeSet(XslNode node)
		{
			AttributeSet attributeSet;
			if (this.compiler.AttributeSets.TryGetValue(node.Name, out attributeSet) && this.currentTemplate != null)
			{
				if (this.forEachDepth == 0)
				{
					this.focusDonors.AddEdge(attributeSet, this.currentTemplate);
				}
				else
				{
					this.sideEffectDonors.AddEdge(attributeSet, this.currentTemplate);
				}
			}
			return XslFlags.Rtf | XslFlags.HasCalls;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x0003E752 File Offset: 0x0003D752
		protected override XslFlags VisitValueOf(XslNode node)
		{
			return XslFlags.Rtf | this.ProcessExpr(node.Select);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0003E763 File Offset: 0x0003D763
		protected override XslFlags VisitValueOfDoe(XslNode node)
		{
			return XslFlags.Rtf | this.ProcessExpr(node.Select);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x0003E774 File Offset: 0x0003D774
		protected override XslFlags VisitParam(VarPar node)
		{
			Template template = this.currentTemplate as Template;
			if (template != null && template.Match != null)
			{
				node.Flags |= XslFlags.MayBeDefault;
				XslAstAnalyzer.ModeName modeName = new XslAstAnalyzer.ModeName(template.Mode, node.Name);
				VarPar varPar;
				if (!this.applyTemplatesParams.TryGetValue(modeName, out varPar))
				{
					varPar = (this.applyTemplatesParams[modeName] = AstFactory.WithParam(node.Name));
				}
				this.dataFlow.AddEdge(varPar, node);
			}
			node.DefValueFlags = this.ProcessVarPar(node);
			return node.DefValueFlags & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x0003E80B File Offset: 0x0003D80B
		protected override XslFlags VisitVariable(VarPar node)
		{
			node.Flags = this.ProcessVarPar(node);
			return node.Flags & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x0003E823 File Offset: 0x0003D823
		protected override XslFlags VisitWithParam(VarPar node)
		{
			node.Flags = this.ProcessVarPar(node);
			return node.Flags & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0003E83C File Offset: 0x0003D83C
		private XslFlags ProcessVarPar(VarPar node)
		{
			XslFlags xslFlags;
			if (node.Select != null)
			{
				if (node.Content.Count != 0)
				{
					xslFlags = this.xpathAnalyzer.Analyze(node.Select) | this.VisitChildren(node) | XslFlags.TypeFilter;
					this.typeDonor = null;
				}
				else
				{
					xslFlags = this.xpathAnalyzer.Analyze(node.Select);
					this.typeDonor = this.xpathAnalyzer.TypeDonor;
					if (this.typeDonor != null && node.NodeType != XslNodeType.WithParam)
					{
						this.dataFlow.AddEdge(this.typeDonor, node);
					}
				}
			}
			else if (node.Content.Count != 0)
			{
				xslFlags = XslFlags.Rtf | this.VisitChildren(node);
				this.typeDonor = null;
			}
			else
			{
				xslFlags = XslFlags.String;
				this.typeDonor = null;
			}
			return xslFlags;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0003E8F8 File Offset: 0x0003D8F8
		private XslFlags ProcessExpr(string expr)
		{
			return this.xpathAnalyzer.Analyze(expr) & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x0003E909 File Offset: 0x0003D909
		private XslFlags ProcessAvt(string avt)
		{
			return this.xpathAnalyzer.AnalyzeAvt(avt) & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0003E91A File Offset: 0x0003D91A
		private XslFlags ProcessPattern(string pattern)
		{
			return this.xpathAnalyzer.Analyze(pattern) & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf) & ~(XslFlags.Current | XslFlags.Position | XslFlags.Last);
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0003E934 File Offset: 0x0003D934
		private void AddApplyTemplatesEdge(QilName mode, ProtoTemplate dependentTemplate)
		{
			List<ProtoTemplate> list;
			if (!this.dependsOnMode.TryGetValue(mode, out list))
			{
				list = new List<ProtoTemplate>();
				this.dependsOnMode.Add(mode, list);
			}
			else if (list[list.Count - 1] == dependentTemplate)
			{
				return;
			}
			list.Add(dependentTemplate);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0003E980 File Offset: 0x0003D980
		private void PropagateSideEffectsFlag()
		{
			foreach (ProtoTemplate protoTemplate in this.focusDonors.Keys)
			{
				protoTemplate.Flags &= ~XslFlags.Stop;
			}
			foreach (ProtoTemplate protoTemplate2 in this.sideEffectDonors.Keys)
			{
				protoTemplate2.Flags &= ~XslFlags.Stop;
			}
			foreach (ProtoTemplate protoTemplate3 in this.focusDonors.Keys)
			{
				if ((protoTemplate3.Flags & XslFlags.Stop) == XslFlags.None && (protoTemplate3.Flags & XslFlags.SideEffects) != XslFlags.None)
				{
					this.DepthFirstSearch(protoTemplate3);
				}
			}
			foreach (ProtoTemplate protoTemplate4 in this.sideEffectDonors.Keys)
			{
				if ((protoTemplate4.Flags & XslFlags.Stop) == XslFlags.None && (protoTemplate4.Flags & XslFlags.SideEffects) != XslFlags.None)
				{
					this.DepthFirstSearch(protoTemplate4);
				}
			}
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0003EB00 File Offset: 0x0003DB00
		private void DepthFirstSearch(ProtoTemplate t)
		{
			t.Flags |= XslFlags.SideEffects | XslFlags.Stop;
			foreach (ProtoTemplate protoTemplate in this.focusDonors.GetAdjList(t))
			{
				if ((protoTemplate.Flags & XslFlags.Stop) == XslFlags.None)
				{
					this.DepthFirstSearch(protoTemplate);
				}
			}
			foreach (ProtoTemplate protoTemplate2 in this.sideEffectDonors.GetAdjList(t))
			{
				if ((protoTemplate2.Flags & XslFlags.Stop) == XslFlags.None)
				{
					this.DepthFirstSearch(protoTemplate2);
				}
			}
			Template template = t as Template;
			List<ProtoTemplate> list;
			if (template != null && this.dependsOnMode.TryGetValue(template.Mode, out list))
			{
				this.dependsOnMode.Remove(template.Mode);
				foreach (ProtoTemplate protoTemplate3 in list)
				{
					if ((protoTemplate3.Flags & XslFlags.Stop) == XslFlags.None)
					{
						this.DepthFirstSearch(protoTemplate3);
					}
				}
			}
		}

		// Token: 0x0400087C RID: 2172
		private CompilerScopeManager<VarPar> scope;

		// Token: 0x0400087D RID: 2173
		private Compiler compiler;

		// Token: 0x0400087E RID: 2174
		private int forEachDepth;

		// Token: 0x0400087F RID: 2175
		private XslAstAnalyzer.XPathAnalyzer xpathAnalyzer;

		// Token: 0x04000880 RID: 2176
		private ProtoTemplate currentTemplate;

		// Token: 0x04000881 RID: 2177
		private VarPar typeDonor;

		// Token: 0x04000882 RID: 2178
		private XslAstAnalyzer.Graph<ProtoTemplate> focusDonors = new XslAstAnalyzer.Graph<ProtoTemplate>();

		// Token: 0x04000883 RID: 2179
		private Dictionary<Template, Stylesheet> dependsOnApplyImports = new Dictionary<Template, Stylesheet>();

		// Token: 0x04000884 RID: 2180
		private XslAstAnalyzer.Graph<ProtoTemplate> sideEffectDonors = new XslAstAnalyzer.Graph<ProtoTemplate>();

		// Token: 0x04000885 RID: 2181
		private Dictionary<QilName, List<ProtoTemplate>> dependsOnMode = new Dictionary<QilName, List<ProtoTemplate>>();

		// Token: 0x04000886 RID: 2182
		private XslAstAnalyzer.Graph<VarPar> dataFlow = new XslAstAnalyzer.Graph<VarPar>();

		// Token: 0x04000887 RID: 2183
		private Dictionary<XslAstAnalyzer.ModeName, VarPar> applyTemplatesParams = new Dictionary<XslAstAnalyzer.ModeName, VarPar>();

		// Token: 0x0200011D RID: 285
		internal class Graph<V> : Dictionary<V, List<V>> where V : XslNode
		{
			// Token: 0x06000C60 RID: 3168 RVA: 0x0003ECA8 File Offset: 0x0003DCA8
			public IEnumerable<V> GetAdjList(V v)
			{
				List<V> list;
				if (base.TryGetValue(v, out list) && list != null)
				{
					return list;
				}
				return XslAstAnalyzer.Graph<V>.empty;
			}

			// Token: 0x06000C61 RID: 3169 RVA: 0x0003ECCC File Offset: 0x0003DCCC
			public void AddEdge(V v1, V v2)
			{
				if (v1 == v2)
				{
					return;
				}
				List<V> list;
				if (!base.TryGetValue(v1, out list) || list == null)
				{
					list = (base[v1] = new List<V>());
				}
				list.Add(v2);
				if (!base.TryGetValue(v2, out list))
				{
					base[v2] = null;
				}
			}

			// Token: 0x06000C62 RID: 3170 RVA: 0x0003ED20 File Offset: 0x0003DD20
			public void PropagateFlag(XslFlags flag)
			{
				foreach (V v in base.Keys)
				{
					v.Flags &= ~XslFlags.Stop;
				}
				foreach (V v2 in base.Keys)
				{
					if ((v2.Flags & XslFlags.Stop) == XslFlags.None && (v2.Flags & flag) != XslFlags.None)
					{
						this.DepthFirstSearch(v2, flag);
					}
				}
			}

			// Token: 0x06000C63 RID: 3171 RVA: 0x0003EDE8 File Offset: 0x0003DDE8
			private void DepthFirstSearch(V v, XslFlags flag)
			{
				v.Flags |= flag | XslFlags.Stop;
				foreach (V v2 in this.GetAdjList(v))
				{
					if ((v2.Flags & XslFlags.Stop) == XslFlags.None)
					{
						this.DepthFirstSearch(v2, flag);
					}
				}
			}

			// Token: 0x04000888 RID: 2184
			private static IList<V> empty = new List<V>().AsReadOnly();
		}

		// Token: 0x0200011E RID: 286
		internal struct ModeName
		{
			// Token: 0x06000C66 RID: 3174 RVA: 0x0003EE7D File Offset: 0x0003DE7D
			public ModeName(QilName mode, QilName name)
			{
				this.Mode = mode;
				this.Name = name;
			}

			// Token: 0x06000C67 RID: 3175 RVA: 0x0003EE8D File Offset: 0x0003DE8D
			public override int GetHashCode()
			{
				return this.Mode.GetHashCode() ^ this.Name.GetHashCode();
			}

			// Token: 0x04000889 RID: 2185
			public QilName Mode;

			// Token: 0x0400088A RID: 2186
			public QilName Name;
		}

		// Token: 0x0200011F RID: 287
		internal struct NullErrorHelper : IErrorHelper
		{
			// Token: 0x06000C68 RID: 3176 RVA: 0x0003EEA6 File Offset: 0x0003DEA6
			public void ReportError(string res, params string[] args)
			{
			}

			// Token: 0x06000C69 RID: 3177 RVA: 0x0003EEA8 File Offset: 0x0003DEA8
			public void ReportWarning(string res, params string[] args)
			{
			}
		}

		// Token: 0x02000120 RID: 288
		internal class XPathAnalyzer : IXPathBuilder<XslFlags>
		{
			// Token: 0x1700018C RID: 396
			// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0003EEAA File Offset: 0x0003DEAA
			public VarPar TypeDonor
			{
				get
				{
					return this.typeDonor;
				}
			}

			// Token: 0x06000C6B RID: 3179 RVA: 0x0003EEB2 File Offset: 0x0003DEB2
			public XPathAnalyzer(Compiler compiler, CompilerScopeManager<VarPar> scope)
			{
				this.compiler = compiler;
				this.scope = scope;
			}

			// Token: 0x06000C6C RID: 3180 RVA: 0x0003EED4 File Offset: 0x0003DED4
			public XslFlags Analyze(string xpathExpr)
			{
				this.typeDonor = null;
				if (xpathExpr == null)
				{
					return XslFlags.None;
				}
				XslFlags xslFlags2;
				try
				{
					this.xsltCurrentNeeded = false;
					XPathScanner xpathScanner = new XPathScanner(xpathExpr);
					XslFlags xslFlags = this.xpathParser.Parse(xpathScanner, this, LexKind.Eof);
					if (this.xsltCurrentNeeded)
					{
						xslFlags |= XslFlags.Current;
					}
					xslFlags2 = xslFlags;
				}
				catch (XslLoadException)
				{
					xslFlags2 = XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf | XslFlags.Current | XslFlags.Position | XslFlags.Last;
				}
				return xslFlags2;
			}

			// Token: 0x06000C6D RID: 3181 RVA: 0x0003EF3C File Offset: 0x0003DF3C
			public XslFlags AnalyzeAvt(string source)
			{
				this.typeDonor = null;
				if (source == null)
				{
					return XslFlags.None;
				}
				XslFlags xslFlags2;
				try
				{
					this.xsltCurrentNeeded = false;
					XslFlags xslFlags = XslFlags.None;
					int i = 0;
					while (i < source.Length)
					{
						i = source.IndexOf('{', i);
						if (i == -1)
						{
							break;
						}
						i++;
						if (i < source.Length && source[i] == '{')
						{
							i++;
						}
						else if (i < source.Length)
						{
							XPathScanner xpathScanner = new XPathScanner(source, i);
							xslFlags |= this.xpathParser.Parse(xpathScanner, this, LexKind.RBrace);
							i = xpathScanner.LexStart + 1;
						}
					}
					if (this.xsltCurrentNeeded)
					{
						xslFlags |= XslFlags.Current;
					}
					xslFlags2 = xslFlags & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
				}
				catch (XslLoadException)
				{
					xslFlags2 = XslFlags.FocusFilter;
				}
				return xslFlags2;
			}

			// Token: 0x06000C6E RID: 3182 RVA: 0x0003EFF4 File Offset: 0x0003DFF4
			private VarPar ResolveVariable(string prefix, string name)
			{
				string text = this.ResolvePrefix(prefix);
				if (text == null)
				{
					return null;
				}
				return this.scope.LookupVariable(name, text);
			}

			// Token: 0x06000C6F RID: 3183 RVA: 0x0003F01B File Offset: 0x0003E01B
			private string ResolvePrefix(string prefix)
			{
				if (prefix.Length == 0)
				{
					return string.Empty;
				}
				return this.scope.LookupNamespace(prefix);
			}

			// Token: 0x06000C70 RID: 3184 RVA: 0x0003F037 File Offset: 0x0003E037
			public virtual void StartBuild()
			{
			}

			// Token: 0x06000C71 RID: 3185 RVA: 0x0003F039 File Offset: 0x0003E039
			public virtual XslFlags EndBuild(XslFlags result)
			{
				return result;
			}

			// Token: 0x06000C72 RID: 3186 RVA: 0x0003F03C File Offset: 0x0003E03C
			public virtual XslFlags String(string value)
			{
				this.typeDonor = null;
				return XslFlags.String;
			}

			// Token: 0x06000C73 RID: 3187 RVA: 0x0003F046 File Offset: 0x0003E046
			public virtual XslFlags Number(double value)
			{
				this.typeDonor = null;
				return XslFlags.Number;
			}

			// Token: 0x06000C74 RID: 3188 RVA: 0x0003F050 File Offset: 0x0003E050
			public virtual XslFlags Operator(XPathOperator op, XslFlags left, XslFlags right)
			{
				this.typeDonor = null;
				XslFlags xslFlags = (left | right) & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf);
				return xslFlags | XslAstAnalyzer.XPathAnalyzer.OperatorType[(int)op];
			}

			// Token: 0x06000C75 RID: 3189 RVA: 0x0003F074 File Offset: 0x0003E074
			public virtual XslFlags Axis(XPathAxis xpathAxis, XPathNodeType nodeType, string prefix, string name)
			{
				this.typeDonor = null;
				if (xpathAxis == XPathAxis.Self && nodeType == XPathNodeType.All && prefix == null && name == null)
				{
					return XslFlags.Node | XslFlags.Current;
				}
				return XslFlags.Nodeset | XslFlags.Current;
			}

			// Token: 0x06000C76 RID: 3190 RVA: 0x0003F099 File Offset: 0x0003E099
			public virtual XslFlags JoinStep(XslFlags left, XslFlags right)
			{
				this.typeDonor = null;
				return (left & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf)) | XslFlags.Nodeset;
			}

			// Token: 0x06000C77 RID: 3191 RVA: 0x0003F0A9 File Offset: 0x0003E0A9
			public virtual XslFlags Predicate(XslFlags nodeset, XslFlags predicate, bool isReverseStep)
			{
				this.typeDonor = null;
				return (nodeset & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf)) | XslFlags.Nodeset | (predicate & XslFlags.SideEffects);
			}

			// Token: 0x06000C78 RID: 3192 RVA: 0x0003F0C1 File Offset: 0x0003E0C1
			public virtual XslFlags Variable(string prefix, string name)
			{
				this.typeDonor = this.ResolveVariable(prefix, name);
				if (this.typeDonor == null)
				{
					return XslFlags.TypeFilter;
				}
				return XslFlags.None;
			}

			// Token: 0x06000C79 RID: 3193 RVA: 0x0003F0E0 File Offset: 0x0003E0E0
			public virtual XslFlags Function(string prefix, string name, IList<XslFlags> args)
			{
				this.typeDonor = null;
				XslFlags xslFlags = XslFlags.None;
				foreach (XslFlags xslFlags2 in args)
				{
					xslFlags |= xslFlags2;
				}
				XslFlags xslFlags3 = XslFlags.None;
				if (prefix.Length == 0)
				{
					XPathBuilder.FunctionInfo<XPathBuilder.FuncId> functionInfo;
					XPathBuilder.FunctionInfo<QilGenerator.FuncId> functionInfo2;
					if (XPathBuilder.FunctionTable.TryGetValue(name, out functionInfo))
					{
						XPathBuilder.FuncId id = functionInfo.id;
						xslFlags3 = XslAstAnalyzer.XPathAnalyzer.XPathFunctionFlags[(int)id];
						if (args.Count == 0 && (id == XPathBuilder.FuncId.LocalName || id == XPathBuilder.FuncId.NamespaceUri || id == XPathBuilder.FuncId.Name || id == XPathBuilder.FuncId.String || id == XPathBuilder.FuncId.Number || id == XPathBuilder.FuncId.StringLength || id == XPathBuilder.FuncId.Normalize))
						{
							xslFlags3 |= XslFlags.Current;
						}
					}
					else if (QilGenerator.FunctionTable.TryGetValue(name, out functionInfo2))
					{
						QilGenerator.FuncId id2 = functionInfo2.id;
						xslFlags3 = XslAstAnalyzer.XPathAnalyzer.XsltFunctionFlags[(int)id2];
						if (id2 == QilGenerator.FuncId.Current)
						{
							this.xsltCurrentNeeded = true;
						}
						else if (id2 == QilGenerator.FuncId.GenerateId && args.Count == 0)
						{
							xslFlags3 |= XslFlags.Current;
						}
					}
				}
				else
				{
					string text = this.ResolvePrefix(prefix);
					if (text == "urn:schemas-microsoft-com:xslt")
					{
						switch (name)
						{
						case "node-set":
							xslFlags3 = XslFlags.Nodeset;
							break;
						case "string-compare":
							xslFlags3 = XslFlags.Number;
							break;
						case "utc":
							xslFlags3 = XslFlags.String;
							break;
						case "format-date":
							xslFlags3 = XslFlags.String;
							break;
						case "format-time":
							xslFlags3 = XslFlags.String;
							break;
						case "local-name":
							xslFlags3 = XslFlags.String;
							break;
						case "namespace-uri":
							xslFlags3 = XslFlags.String;
							break;
						case "number":
							xslFlags3 = XslFlags.Number;
							break;
						}
					}
					else if (text == "http://exslt.org/common" && name != null)
					{
						if (!(name == "node-set"))
						{
							if (name == "object-type")
							{
								xslFlags3 = XslFlags.String;
							}
						}
						else
						{
							xslFlags3 = XslFlags.Nodeset;
						}
					}
					if (xslFlags3 == XslFlags.None)
					{
						xslFlags3 = XslFlags.TypeFilter;
						if (this.compiler.Settings.EnableScript && text != null)
						{
							XmlExtensionFunction xmlExtensionFunction = this.compiler.Scripts.ResolveFunction(name, text, args.Count, default(XslAstAnalyzer.NullErrorHelper));
							if (xmlExtensionFunction != null)
							{
								XmlQueryType xmlReturnType = xmlExtensionFunction.XmlReturnType;
								if (xmlReturnType == XmlQueryTypeFactory.StringX)
								{
									xslFlags3 = XslFlags.String;
								}
								else if (xmlReturnType == XmlQueryTypeFactory.DoubleX)
								{
									xslFlags3 = XslFlags.Number;
								}
								else if (xmlReturnType == XmlQueryTypeFactory.BooleanX)
								{
									xslFlags3 = XslFlags.Boolean;
								}
								else if (xmlReturnType == XmlQueryTypeFactory.NodeNotRtf)
								{
									xslFlags3 = XslFlags.Node;
								}
								else if (xmlReturnType == XmlQueryTypeFactory.NodeDodS)
								{
									xslFlags3 = XslFlags.Nodeset;
								}
								else if (xmlReturnType == XmlQueryTypeFactory.ItemS)
								{
									xslFlags3 = XslFlags.TypeFilter;
								}
								else if (xmlReturnType == XmlQueryTypeFactory.Empty)
								{
									xslFlags3 = XslFlags.Nodeset;
								}
							}
						}
						xslFlags3 |= XslFlags.SideEffects;
					}
				}
				return (xslFlags & ~(XslFlags.String | XslFlags.Number | XslFlags.Boolean | XslFlags.Node | XslFlags.Nodeset | XslFlags.Rtf)) | xslFlags3;
			}

			// Token: 0x0400088B RID: 2187
			private XPathParser<XslFlags> xpathParser = new XPathParser<XslFlags>();

			// Token: 0x0400088C RID: 2188
			private CompilerScopeManager<VarPar> scope;

			// Token: 0x0400088D RID: 2189
			private Compiler compiler;

			// Token: 0x0400088E RID: 2190
			private bool xsltCurrentNeeded;

			// Token: 0x0400088F RID: 2191
			private VarPar typeDonor;

			// Token: 0x04000890 RID: 2192
			private static XslFlags[] OperatorType = new XslFlags[]
			{
				XslFlags.TypeFilter,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Nodeset
			};

			// Token: 0x04000891 RID: 2193
			private static XslFlags[] XPathFunctionFlags = new XslFlags[]
			{
				XslFlags.Number | XslFlags.Last,
				XslFlags.Number | XslFlags.Position,
				XslFlags.Number,
				XslFlags.String,
				XslFlags.String,
				XslFlags.String,
				XslFlags.String,
				XslFlags.Number,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.Nodeset | XslFlags.Current,
				XslFlags.String,
				XslFlags.Boolean,
				XslFlags.Boolean,
				XslFlags.String,
				XslFlags.String,
				XslFlags.String,
				XslFlags.Number,
				XslFlags.String,
				XslFlags.String,
				XslFlags.Boolean | XslFlags.Current,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Number,
				XslFlags.Number
			};

			// Token: 0x04000892 RID: 2194
			private static XslFlags[] XsltFunctionFlags = new XslFlags[]
			{
				XslFlags.Node,
				XslFlags.Nodeset,
				XslFlags.Nodeset | XslFlags.Current,
				XslFlags.String,
				XslFlags.String,
				XslFlags.String,
				XslFlags.String | XslFlags.Number,
				XslFlags.Boolean,
				XslFlags.Boolean
			};
		}
	}
}
