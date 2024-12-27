using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200074B RID: 1867
	[SupportsEventValidation]
	[Designer("System.Web.UI.Design.WebControls.WebParts.WebPartZoneDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartZone : WebPartZoneBase
	{
		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x06005AC4 RID: 23236 RVA: 0x0016E4B3 File Offset: 0x0016D4B3
		// (set) Token: 0x06005AC5 RID: 23237 RVA: 0x0016E4BB File Offset: 0x0016D4BB
		[DefaultValue(null)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[Browsable(false)]
		[TemplateInstance(TemplateInstance.Single)]
		public virtual ITemplate ZoneTemplate
		{
			get
			{
				return this._zoneTemplate;
			}
			set
			{
				if (!base.DesignMode && this._registrationComplete)
				{
					throw new InvalidOperationException(SR.GetString("WebPart_SetZoneTemplateTooLate"));
				}
				this._zoneTemplate = value;
			}
		}

		// Token: 0x06005AC6 RID: 23238 RVA: 0x0016E4E4 File Offset: 0x0016D4E4
		private void AddWebPartToList(WebPartCollection webParts, Control control)
		{
			WebPart webPart = control as WebPart;
			if (webPart == null && !(control is LiteralControl))
			{
				WebPartManager webPartManager = base.WebPartManager;
				if (webPartManager != null)
				{
					webPart = webPartManager.CreateWebPart(control);
				}
				else
				{
					webPart = WebPartManager.CreateWebPartStatic(control);
				}
			}
			if (webPart != null)
			{
				webParts.Add(webPart);
			}
		}

		// Token: 0x06005AC7 RID: 23239 RVA: 0x0016E52C File Offset: 0x0016D52C
		protected internal override WebPartCollection GetInitialWebParts()
		{
			WebPartCollection webPartCollection = new WebPartCollection();
			if (this.ZoneTemplate != null)
			{
				Control control = new NonParentingControl();
				this.ZoneTemplate.InstantiateIn(control);
				if (control.HasControls())
				{
					ControlCollection controls = control.Controls;
					foreach (object obj in controls)
					{
						Control control2 = (Control)obj;
						if (control2 is ContentPlaceHolder)
						{
							if (control2.HasControls())
							{
								Control[] array = new Control[control2.Controls.Count];
								control2.Controls.CopyTo(array, 0);
								foreach (Control control3 in array)
								{
									this.AddWebPartToList(webPartCollection, control3);
								}
							}
						}
						else
						{
							this.AddWebPartToList(webPartCollection, control2);
						}
					}
				}
			}
			return webPartCollection;
		}

		// Token: 0x06005AC8 RID: 23240 RVA: 0x0016E618 File Offset: 0x0016D618
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this._registrationComplete = true;
		}

		// Token: 0x040030C8 RID: 12488
		private ITemplate _zoneTemplate;

		// Token: 0x040030C9 RID: 12489
		private bool _registrationComplete;
	}
}
