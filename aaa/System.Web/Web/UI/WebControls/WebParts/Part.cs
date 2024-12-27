using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200069D RID: 1693
	[PersistChildren(false)]
	[Designer("System.Web.UI.Design.WebControls.WebParts.PartDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ParseChildren(true)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class Part : Panel, INamingContainer, ICompositeControlDesignerAccessor
	{
		// Token: 0x060052C7 RID: 21191 RVA: 0x0014E719 File Offset: 0x0014D719
		internal Part()
		{
		}

		// Token: 0x1700150C RID: 5388
		// (get) Token: 0x060052C8 RID: 21192 RVA: 0x0014E724 File Offset: 0x0014D724
		// (set) Token: 0x060052C9 RID: 21193 RVA: 0x0014E74D File Offset: 0x0014D74D
		[DefaultValue(PartChromeState.Normal)]
		[WebSysDescription("Part_ChromeState")]
		[WebCategory("WebPartAppearance")]
		public virtual PartChromeState ChromeState
		{
			get
			{
				object obj = this.ViewState["ChromeState"];
				if (obj == null)
				{
					return PartChromeState.Normal;
				}
				return (PartChromeState)obj;
			}
			set
			{
				if (value < PartChromeState.Normal || value > PartChromeState.Minimized)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["ChromeState"] = value;
			}
		}

		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x060052CA RID: 21194 RVA: 0x0014E778 File Offset: 0x0014D778
		// (set) Token: 0x060052CB RID: 21195 RVA: 0x0014E7A1 File Offset: 0x0014D7A1
		[WebCategory("WebPartAppearance")]
		[DefaultValue(PartChromeType.Default)]
		[WebSysDescription("Part_ChromeType")]
		public virtual PartChromeType ChromeType
		{
			get
			{
				object obj = this.ViewState["ChromeType"];
				if (obj == null)
				{
					return PartChromeType.Default;
				}
				return (PartChromeType)((int)obj);
			}
			set
			{
				if (value < PartChromeType.Default || value > PartChromeType.BorderOnly)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["ChromeType"] = (int)value;
			}
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x060052CC RID: 21196 RVA: 0x0014E7CC File Offset: 0x0014D7CC
		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		// Token: 0x1700150F RID: 5391
		// (get) Token: 0x060052CD RID: 21197 RVA: 0x0014E7DC File Offset: 0x0014D7DC
		// (set) Token: 0x060052CE RID: 21198 RVA: 0x0014E809 File Offset: 0x0014D809
		[WebSysDescription("Part_Description")]
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("WebPartAppearance")]
		public virtual string Description
		{
			get
			{
				string text = (string)this.ViewState["Description"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Description"] = value;
			}
		}

		// Token: 0x17001510 RID: 5392
		// (get) Token: 0x060052CF RID: 21199 RVA: 0x0014E81C File Offset: 0x0014D81C
		// (set) Token: 0x060052D0 RID: 21200 RVA: 0x0014E849 File Offset: 0x0014D849
		[WebSysDefaultValue("")]
		[Localizable(true)]
		[WebCategory("WebPartAppearance")]
		[WebSysDescription("Part_Title")]
		public virtual string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x0014E85C File Offset: 0x0014D85C
		public override void DataBind()
		{
			this.OnDataBinding(EventArgs.Empty);
			this.EnsureChildControls();
			this.DataBindChildren();
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x0014E875 File Offset: 0x0014D875
		void ICompositeControlDesignerAccessor.RecreateChildControls()
		{
			base.ChildControlsCreated = false;
			this.EnsureChildControls();
		}
	}
}
