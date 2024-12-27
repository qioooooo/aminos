using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x0200010F RID: 271
	internal class AttributeSet : ProtoTemplate
	{
		// Token: 0x06000BE2 RID: 3042 RVA: 0x0003D32C File Offset: 0x0003C32C
		public AttributeSet(QilName name, XslVersion xslVer)
			: base(XslNodeType.AttributeSet, name, xslVer)
		{
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0003D344 File Offset: 0x0003C344
		public override string GetDebugName()
		{
			BufferBuilder bufferBuilder = new BufferBuilder();
			bufferBuilder.Append("<xsl:attribute-set name=\"");
			bufferBuilder.Append(this.Name.QualifiedName);
			bufferBuilder.Append("\">");
			return bufferBuilder.ToString();
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0003D384 File Offset: 0x0003C384
		public new void AddContent(XslNode node)
		{
			base.AddContent(node);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0003D38D File Offset: 0x0003C38D
		public void MergeContent(AttributeSet other)
		{
			this.UsedAttributeSets.InsertRange(0, other.UsedAttributeSets);
			base.InsertContent(other.Content);
		}

		// Token: 0x04000859 RID: 2137
		public readonly List<QilName> UsedAttributeSets = new List<QilName>();

		// Token: 0x0400085A RID: 2138
		public CycleCheck CycleCheck;
	}
}
