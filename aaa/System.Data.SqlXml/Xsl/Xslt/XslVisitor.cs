using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200011B RID: 283
	internal abstract class XslVisitor<T>
	{
		// Token: 0x06000C15 RID: 3093 RVA: 0x0003D820 File Offset: 0x0003C820
		protected virtual T Visit(XslNode node)
		{
			switch (node.NodeType)
			{
			case XslNodeType.ApplyImports:
				return this.VisitApplyImports(node);
			case XslNodeType.ApplyTemplates:
				return this.VisitApplyTemplates(node);
			case XslNodeType.Attribute:
				return this.VisitAttribute((NodeCtor)node);
			case XslNodeType.AttributeSet:
				return this.VisitAttributeSet((AttributeSet)node);
			case XslNodeType.CallTemplate:
				return this.VisitCallTemplate(node);
			case XslNodeType.Choose:
				return this.VisitChoose(node);
			case XslNodeType.Comment:
				return this.VisitComment(node);
			case XslNodeType.Copy:
				return this.VisitCopy(node);
			case XslNodeType.CopyOf:
				return this.VisitCopyOf(node);
			case XslNodeType.Element:
				return this.VisitElement((NodeCtor)node);
			case XslNodeType.Error:
				return this.VisitError(node);
			case XslNodeType.ForEach:
				return this.VisitForEach(node);
			case XslNodeType.If:
				return this.VisitIf(node);
			case XslNodeType.Key:
				return this.VisitKey((Key)node);
			case XslNodeType.List:
				return this.VisitList(node);
			case XslNodeType.LiteralAttribute:
				return this.VisitLiteralAttribute(node);
			case XslNodeType.LiteralElement:
				return this.VisitLiteralElement(node);
			case XslNodeType.Message:
				return this.VisitMessage(node);
			case XslNodeType.Nop:
				return this.VisitNop(node);
			case XslNodeType.Number:
				return this.VisitNumber((Number)node);
			case XslNodeType.Otherwise:
				return this.VisitOtherwise(node);
			case XslNodeType.Param:
				return this.VisitParam((VarPar)node);
			case XslNodeType.PI:
				return this.VisitPI(node);
			case XslNodeType.Sort:
				return this.VisitSort((Sort)node);
			case XslNodeType.Template:
				return this.VisitTemplate((Template)node);
			case XslNodeType.Text:
				return this.VisitText((Text)node);
			case XslNodeType.UseAttributeSet:
				return this.VisitUseAttributeSet(node);
			case XslNodeType.ValueOf:
				return this.VisitValueOf(node);
			case XslNodeType.ValueOfDoe:
				return this.VisitValueOfDoe(node);
			case XslNodeType.Variable:
				return this.VisitVariable((VarPar)node);
			case XslNodeType.WithParam:
				return this.VisitWithParam((VarPar)node);
			default:
				return this.VisitUnknown(node);
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0003D9F3 File Offset: 0x0003C9F3
		protected virtual T VisitApplyImports(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0003D9FC File Offset: 0x0003C9FC
		protected virtual T VisitApplyTemplates(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0003DA05 File Offset: 0x0003CA05
		protected virtual T VisitAttribute(NodeCtor node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0003DA0E File Offset: 0x0003CA0E
		protected virtual T VisitAttributeSet(AttributeSet node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0003DA17 File Offset: 0x0003CA17
		protected virtual T VisitCallTemplate(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0003DA20 File Offset: 0x0003CA20
		protected virtual T VisitChoose(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0003DA29 File Offset: 0x0003CA29
		protected virtual T VisitComment(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0003DA32 File Offset: 0x0003CA32
		protected virtual T VisitCopy(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0003DA3B File Offset: 0x0003CA3B
		protected virtual T VisitCopyOf(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0003DA44 File Offset: 0x0003CA44
		protected virtual T VisitElement(NodeCtor node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0003DA4D File Offset: 0x0003CA4D
		protected virtual T VisitError(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0003DA56 File Offset: 0x0003CA56
		protected virtual T VisitForEach(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0003DA5F File Offset: 0x0003CA5F
		protected virtual T VisitIf(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0003DA68 File Offset: 0x0003CA68
		protected virtual T VisitKey(Key node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0003DA71 File Offset: 0x0003CA71
		protected virtual T VisitList(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0003DA7A File Offset: 0x0003CA7A
		protected virtual T VisitLiteralAttribute(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0003DA83 File Offset: 0x0003CA83
		protected virtual T VisitLiteralElement(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0003DA8C File Offset: 0x0003CA8C
		protected virtual T VisitMessage(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0003DA95 File Offset: 0x0003CA95
		protected virtual T VisitNop(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0003DA9E File Offset: 0x0003CA9E
		protected virtual T VisitNumber(Number node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0003DAA7 File Offset: 0x0003CAA7
		protected virtual T VisitOtherwise(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0003DAB0 File Offset: 0x0003CAB0
		protected virtual T VisitParam(VarPar node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0003DAB9 File Offset: 0x0003CAB9
		protected virtual T VisitPI(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0003DAC2 File Offset: 0x0003CAC2
		protected virtual T VisitSort(Sort node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0003DACB File Offset: 0x0003CACB
		protected virtual T VisitTemplate(Template node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0003DAD4 File Offset: 0x0003CAD4
		protected virtual T VisitText(Text node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x0003DADD File Offset: 0x0003CADD
		protected virtual T VisitUseAttributeSet(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0003DAE6 File Offset: 0x0003CAE6
		protected virtual T VisitValueOf(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0003DAEF File Offset: 0x0003CAEF
		protected virtual T VisitValueOfDoe(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0003DAF8 File Offset: 0x0003CAF8
		protected virtual T VisitVariable(VarPar node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0003DB01 File Offset: 0x0003CB01
		protected virtual T VisitWithParam(VarPar node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0003DB0A File Offset: 0x0003CB0A
		protected virtual T VisitUnknown(XslNode node)
		{
			return this.VisitChildren(node);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0003DB14 File Offset: 0x0003CB14
		protected virtual T VisitChildren(XslNode node)
		{
			foreach (XslNode xslNode in node.Content)
			{
				this.Visit(xslNode);
			}
			return default(T);
		}
	}
}
