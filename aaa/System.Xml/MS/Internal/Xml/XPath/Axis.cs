using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000126 RID: 294
	internal class Axis : AstNode
	{
		// Token: 0x0600116A RID: 4458 RVA: 0x0004DC3B File Offset: 0x0004CC3B
		public Axis(Axis.AxisType axisType, AstNode input, string prefix, string name, XPathNodeType nodetype)
		{
			this.axisType = axisType;
			this.input = input;
			this.prefix = prefix;
			this.name = name;
			this.nodeType = nodetype;
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x0004DC73 File Offset: 0x0004CC73
		public Axis(Axis.AxisType axisType, AstNode input)
			: this(axisType, input, string.Empty, string.Empty, XPathNodeType.All)
		{
			this.abbrAxis = true;
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x0004DC90 File Offset: 0x0004CC90
		public override AstNode.AstType Type
		{
			get
			{
				return AstNode.AstType.Axis;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x0004DC93 File Offset: 0x0004CC93
		public override XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.NodeSet;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x0004DC96 File Offset: 0x0004CC96
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x0004DC9E File Offset: 0x0004CC9E
		public AstNode Input
		{
			get
			{
				return this.input;
			}
			set
			{
				this.input = value;
			}
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x0004DCA7 File Offset: 0x0004CCA7
		public string Prefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001171 RID: 4465 RVA: 0x0004DCAF File Offset: 0x0004CCAF
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x0004DCB7 File Offset: 0x0004CCB7
		public XPathNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x0004DCBF File Offset: 0x0004CCBF
		public Axis.AxisType TypeOfAxis
		{
			get
			{
				return this.axisType;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06001174 RID: 4468 RVA: 0x0004DCC7 File Offset: 0x0004CCC7
		public bool AbbrAxis
		{
			get
			{
				return this.abbrAxis;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x0004DCCF File Offset: 0x0004CCCF
		// (set) Token: 0x06001176 RID: 4470 RVA: 0x0004DCD7 File Offset: 0x0004CCD7
		public string Urn
		{
			get
			{
				return this.urn;
			}
			set
			{
				this.urn = value;
			}
		}

		// Token: 0x04000B27 RID: 2855
		private Axis.AxisType axisType;

		// Token: 0x04000B28 RID: 2856
		private AstNode input;

		// Token: 0x04000B29 RID: 2857
		private string prefix;

		// Token: 0x04000B2A RID: 2858
		private string name;

		// Token: 0x04000B2B RID: 2859
		private XPathNodeType nodeType;

		// Token: 0x04000B2C RID: 2860
		protected bool abbrAxis;

		// Token: 0x04000B2D RID: 2861
		private string urn = string.Empty;

		// Token: 0x02000127 RID: 295
		public enum AxisType
		{
			// Token: 0x04000B2F RID: 2863
			Ancestor,
			// Token: 0x04000B30 RID: 2864
			AncestorOrSelf,
			// Token: 0x04000B31 RID: 2865
			Attribute,
			// Token: 0x04000B32 RID: 2866
			Child,
			// Token: 0x04000B33 RID: 2867
			Descendant,
			// Token: 0x04000B34 RID: 2868
			DescendantOrSelf,
			// Token: 0x04000B35 RID: 2869
			Following,
			// Token: 0x04000B36 RID: 2870
			FollowingSibling,
			// Token: 0x04000B37 RID: 2871
			Namespace,
			// Token: 0x04000B38 RID: 2872
			Parent,
			// Token: 0x04000B39 RID: 2873
			Preceding,
			// Token: 0x04000B3A RID: 2874
			PrecedingSibling,
			// Token: 0x04000B3B RID: 2875
			Self,
			// Token: 0x04000B3C RID: 2876
			None
		}
	}
}
