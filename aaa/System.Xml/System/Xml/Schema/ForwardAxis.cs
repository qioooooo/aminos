using System;

namespace System.Xml.Schema
{
	// Token: 0x02000180 RID: 384
	internal class ForwardAxis
	{
		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x0600145B RID: 5211 RVA: 0x00057262 File Offset: 0x00056262
		internal DoubleLinkAxis RootNode
		{
			get
			{
				return this.rootNode;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x0600145C RID: 5212 RVA: 0x0005726A File Offset: 0x0005626A
		internal DoubleLinkAxis TopNode
		{
			get
			{
				return this.topNode;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x0600145D RID: 5213 RVA: 0x00057272 File Offset: 0x00056272
		internal bool IsAttribute
		{
			get
			{
				return this.isAttribute;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x0600145E RID: 5214 RVA: 0x0005727A File Offset: 0x0005627A
		internal bool IsDss
		{
			get
			{
				return this.isDss;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x0600145F RID: 5215 RVA: 0x00057282 File Offset: 0x00056282
		internal bool IsSelfAxis
		{
			get
			{
				return this.isSelfAxis;
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0005728C File Offset: 0x0005628C
		public ForwardAxis(DoubleLinkAxis axis, bool isdesorself)
		{
			this.isDss = isdesorself;
			this.isAttribute = Asttree.IsAttribute(axis);
			this.topNode = axis;
			this.rootNode = axis;
			while (this.rootNode.Input != null)
			{
				this.rootNode = (DoubleLinkAxis)this.rootNode.Input;
			}
			this.isSelfAxis = Asttree.IsSelf(this.topNode);
		}

		// Token: 0x04000C60 RID: 3168
		private DoubleLinkAxis topNode;

		// Token: 0x04000C61 RID: 3169
		private DoubleLinkAxis rootNode;

		// Token: 0x04000C62 RID: 3170
		private bool isAttribute;

		// Token: 0x04000C63 RID: 3171
		private bool isDss;

		// Token: 0x04000C64 RID: 3172
		private bool isSelfAxis;
	}
}
