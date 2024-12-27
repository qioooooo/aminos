using System;
using System.ComponentModel;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000404 RID: 1028
	[DefaultProperty("FlowDirection")]
	public class FlowLayoutSettings : LayoutSettings
	{
		// Token: 0x06003C79 RID: 15481 RVA: 0x000D99F5 File Offset: 0x000D89F5
		internal FlowLayoutSettings(IArrangedElement owner)
			: base(owner)
		{
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x06003C7A RID: 15482 RVA: 0x000D99FE File Offset: 0x000D89FE
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x06003C7B RID: 15483 RVA: 0x000D9A05 File Offset: 0x000D8A05
		// (set) Token: 0x06003C7C RID: 15484 RVA: 0x000D9A12 File Offset: 0x000D8A12
		[SRCategory("CatLayout")]
		[SRDescription("FlowPanelFlowDirectionDescr")]
		[DefaultValue(FlowDirection.LeftToRight)]
		public FlowDirection FlowDirection
		{
			get
			{
				return FlowLayout.GetFlowDirection(base.Owner);
			}
			set
			{
				FlowLayout.SetFlowDirection(base.Owner, value);
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06003C7D RID: 15485 RVA: 0x000D9A20 File Offset: 0x000D8A20
		// (set) Token: 0x06003C7E RID: 15486 RVA: 0x000D9A2D File Offset: 0x000D8A2D
		[DefaultValue(true)]
		[SRCategory("CatLayout")]
		[SRDescription("FlowPanelWrapContentsDescr")]
		public bool WrapContents
		{
			get
			{
				return FlowLayout.GetWrapContents(base.Owner);
			}
			set
			{
				FlowLayout.SetWrapContents(base.Owner, value);
			}
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x000D9A3C File Offset: 0x000D8A3C
		public void SetFlowBreak(object child, bool value)
		{
			IArrangedElement arrangedElement = FlowLayout.Instance.CastToArrangedElement(child);
			if (this.GetFlowBreak(child) != value)
			{
				CommonProperties.SetFlowBreak(arrangedElement, value);
			}
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x000D9A68 File Offset: 0x000D8A68
		public bool GetFlowBreak(object child)
		{
			IArrangedElement arrangedElement = FlowLayout.Instance.CastToArrangedElement(child);
			return CommonProperties.GetFlowBreak(arrangedElement);
		}
	}
}
