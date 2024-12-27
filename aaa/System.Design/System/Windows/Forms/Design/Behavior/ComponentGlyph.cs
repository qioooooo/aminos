using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002C8 RID: 712
	public class ComponentGlyph : Glyph
	{
		// Token: 0x06001AF1 RID: 6897 RVA: 0x00093DB1 File Offset: 0x00092DB1
		public ComponentGlyph(IComponent relatedComponent, Behavior behavior)
			: base(behavior)
		{
			this.relatedComponent = relatedComponent;
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x00093DC1 File Offset: 0x00092DC1
		public ComponentGlyph(IComponent relatedComponent)
			: base(null)
		{
			this.relatedComponent = relatedComponent;
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00093DD1 File Offset: 0x00092DD1
		public IComponent RelatedComponent
		{
			get
			{
				return this.relatedComponent;
			}
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x00093DD9 File Offset: 0x00092DD9
		public override Cursor GetHitTest(Point p)
		{
			return null;
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x00093DDC File Offset: 0x00092DDC
		public override void Paint(PaintEventArgs pe)
		{
		}

		// Token: 0x04001550 RID: 5456
		private IComponent relatedComponent;
	}
}
