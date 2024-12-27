using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004FF RID: 1279
	[Themeable(true)]
	[PersistChildren(false)]
	[ToolboxItem(false)]
	[ControlBuilder(typeof(WizardStepControlBuilder))]
	[Bindable(false)]
	[ParseChildren(true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class TemplatedWizardStep : WizardStepBase
	{
		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06003E88 RID: 16008 RVA: 0x00104CB2 File Offset: 0x00103CB2
		// (set) Token: 0x06003E89 RID: 16009 RVA: 0x00104CBA File Offset: 0x00103CBA
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("TemplatedWizardStep_ContentTemplate")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Browsable(false)]
		[TemplateContainer(typeof(Wizard))]
		[DefaultValue(null)]
		public virtual ITemplate ContentTemplate
		{
			get
			{
				return this._contentTemplate;
			}
			set
			{
				this._contentTemplate = value;
				if (this.Owner != null && base.ControlState > ControlState.Constructed)
				{
					this.Owner.RequiresControlsRecreation();
				}
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06003E8A RID: 16010 RVA: 0x00104CDF File Offset: 0x00103CDF
		// (set) Token: 0x06003E8B RID: 16011 RVA: 0x00104CE7 File Offset: 0x00103CE7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control ContentTemplateContainer
		{
			get
			{
				return this._contentContainer;
			}
			internal set
			{
				this._contentContainer = value;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x00104CF0 File Offset: 0x00103CF0
		// (set) Token: 0x06003E8D RID: 16013 RVA: 0x00104CF8 File Offset: 0x00103CF8
		[WebSysDescription("TemplatedWizardStep_CustomNavigationTemplate")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(Wizard))]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DefaultValue(null)]
		[Browsable(false)]
		public virtual ITemplate CustomNavigationTemplate
		{
			get
			{
				return this._navigationTemplate;
			}
			set
			{
				this._navigationTemplate = value;
				if (this.Owner != null && base.ControlState > ControlState.Constructed)
				{
					this.Owner.RequiresControlsRecreation();
				}
			}
		}

		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x06003E8E RID: 16014 RVA: 0x00104D1D File Offset: 0x00103D1D
		// (set) Token: 0x06003E8F RID: 16015 RVA: 0x00104D25 File Offset: 0x00103D25
		[Bindable(false)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control CustomNavigationTemplateContainer
		{
			get
			{
				return this._navigationContainer;
			}
			internal set
			{
				this._navigationContainer = value;
			}
		}

		// Token: 0x17000ECE RID: 3790
		// (get) Token: 0x06003E90 RID: 16016 RVA: 0x00104D2E File Offset: 0x00103D2E
		// (set) Token: 0x06003E91 RID: 16017 RVA: 0x00104D36 File Offset: 0x00103D36
		[Browsable(true)]
		public override string SkinID
		{
			get
			{
				return base.SkinID;
			}
			set
			{
				base.SkinID = value;
			}
		}

		// Token: 0x0400279A RID: 10138
		private ITemplate _contentTemplate;

		// Token: 0x0400279B RID: 10139
		private Control _contentContainer;

		// Token: 0x0400279C RID: 10140
		private ITemplate _navigationTemplate;

		// Token: 0x0400279D RID: 10141
		private Control _navigationContainer;
	}
}
