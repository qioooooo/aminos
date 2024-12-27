using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x02000402 RID: 1026
	[SRDescription("DescriptionFlowLayoutPanel")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ProvideProperty("FlowBreak", typeof(Control))]
	[ComVisible(true)]
	[DefaultProperty("FlowDirection")]
	[Designer("System.Windows.Forms.Design.FlowLayoutPanelDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[Docking(DockingBehavior.Ask)]
	public class FlowLayoutPanel : Panel, IExtenderProvider
	{
		// Token: 0x06003C6C RID: 15468 RVA: 0x000D9942 File Offset: 0x000D8942
		public FlowLayoutPanel()
		{
			this._flowLayoutSettings = FlowLayout.CreateSettings(this);
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06003C6D RID: 15469 RVA: 0x000D9956 File Offset: 0x000D8956
		public override LayoutEngine LayoutEngine
		{
			get
			{
				return FlowLayout.Instance;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06003C6E RID: 15470 RVA: 0x000D995D File Offset: 0x000D895D
		// (set) Token: 0x06003C6F RID: 15471 RVA: 0x000D996A File Offset: 0x000D896A
		[DefaultValue(FlowDirection.LeftToRight)]
		[SRCategory("CatLayout")]
		[Localizable(true)]
		[SRDescription("FlowPanelFlowDirectionDescr")]
		public FlowDirection FlowDirection
		{
			get
			{
				return this._flowLayoutSettings.FlowDirection;
			}
			set
			{
				this._flowLayoutSettings.FlowDirection = value;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x000D9978 File Offset: 0x000D8978
		// (set) Token: 0x06003C71 RID: 15473 RVA: 0x000D9985 File Offset: 0x000D8985
		[SRCategory("CatLayout")]
		[SRDescription("FlowPanelWrapContentsDescr")]
		[DefaultValue(true)]
		[Localizable(true)]
		public bool WrapContents
		{
			get
			{
				return this._flowLayoutSettings.WrapContents;
			}
			set
			{
				this._flowLayoutSettings.WrapContents = value;
			}
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x000D9994 File Offset: 0x000D8994
		bool IExtenderProvider.CanExtend(object obj)
		{
			Control control = obj as Control;
			return control != null && control.Parent == this;
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x000D99B6 File Offset: 0x000D89B6
		[DefaultValue(false)]
		[DisplayName("FlowBreak")]
		public bool GetFlowBreak(Control control)
		{
			return this._flowLayoutSettings.GetFlowBreak(control);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x000D99C4 File Offset: 0x000D89C4
		[DisplayName("FlowBreak")]
		public void SetFlowBreak(Control control, bool value)
		{
			this._flowLayoutSettings.SetFlowBreak(control, value);
		}

		// Token: 0x04001E1C RID: 7708
		private FlowLayoutSettings _flowLayoutSettings;
	}
}
