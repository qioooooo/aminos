using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000571 RID: 1393
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class EditCommandColumn : DataGridColumn
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06004478 RID: 17528 RVA: 0x00119D3C File Offset: 0x00118D3C
		// (set) Token: 0x06004479 RID: 17529 RVA: 0x00119D65 File Offset: 0x00118D65
		[DefaultValue(ButtonColumnType.LinkButton)]
		public virtual ButtonColumnType ButtonType
		{
			get
			{
				object obj = base.ViewState["ButtonType"];
				if (obj != null)
				{
					return (ButtonColumnType)obj;
				}
				return ButtonColumnType.LinkButton;
			}
			set
			{
				if (value < ButtonColumnType.LinkButton || value > ButtonColumnType.PushButton)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["ButtonType"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x0600447A RID: 17530 RVA: 0x00119D98 File Offset: 0x00118D98
		// (set) Token: 0x0600447B RID: 17531 RVA: 0x00119DC5 File Offset: 0x00118DC5
		[DefaultValue("")]
		[Localizable(true)]
		public virtual string CancelText
		{
			get
			{
				object obj = base.ViewState["CancelText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["CancelText"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x00119DE0 File Offset: 0x00118DE0
		// (set) Token: 0x0600447D RID: 17533 RVA: 0x00119E09 File Offset: 0x00118E09
		[DefaultValue(true)]
		public virtual bool CausesValidation
		{
			get
			{
				object obj = base.ViewState["CausesValidation"];
				return obj == null || (bool)obj;
			}
			set
			{
				base.ViewState["CausesValidation"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x00119E28 File Offset: 0x00118E28
		// (set) Token: 0x0600447F RID: 17535 RVA: 0x00119E55 File Offset: 0x00118E55
		[DefaultValue("")]
		[Localizable(true)]
		public virtual string EditText
		{
			get
			{
				object obj = base.ViewState["EditText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["EditText"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x00119E70 File Offset: 0x00118E70
		// (set) Token: 0x06004481 RID: 17537 RVA: 0x00119E9D File Offset: 0x00118E9D
		[Localizable(true)]
		[DefaultValue("")]
		public virtual string UpdateText
		{
			get
			{
				object obj = base.ViewState["UpdateText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["UpdateText"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x00119EB8 File Offset: 0x00118EB8
		// (set) Token: 0x06004483 RID: 17539 RVA: 0x00119EE5 File Offset: 0x00118EE5
		[DefaultValue("")]
		public virtual string ValidationGroup
		{
			get
			{
				object obj = base.ViewState["ValidationGroup"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["ValidationGroup"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x00119F00 File Offset: 0x00118F00
		private void AddButtonToCell(TableCell cell, string commandName, string buttonText, bool causesValidation, string validationGroup)
		{
			ControlCollection controls = cell.Controls;
			WebControl webControl;
			if (this.ButtonType == ButtonColumnType.LinkButton)
			{
				LinkButton linkButton = new DataGridLinkButton();
				webControl = linkButton;
				linkButton.CommandName = commandName;
				linkButton.Text = buttonText;
				linkButton.CausesValidation = causesValidation;
				linkButton.ValidationGroup = validationGroup;
			}
			else
			{
				Button button = new Button();
				webControl = button;
				button.CommandName = commandName;
				button.Text = buttonText;
				button.CausesValidation = causesValidation;
				button.ValidationGroup = validationGroup;
			}
			controls.Add(webControl);
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x00119F7C File Offset: 0x00118F7C
		public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
		{
			base.InitializeCell(cell, columnIndex, itemType);
			bool causesValidation = this.CausesValidation;
			if (itemType != ListItemType.Header && itemType != ListItemType.Footer)
			{
				if (itemType == ListItemType.EditItem)
				{
					ControlCollection controls = cell.Controls;
					this.AddButtonToCell(cell, "Update", this.UpdateText, causesValidation, this.ValidationGroup);
					LiteralControl literalControl = new LiteralControl("&nbsp;");
					controls.Add(literalControl);
					this.AddButtonToCell(cell, "Cancel", this.CancelText, false, string.Empty);
					return;
				}
				this.AddButtonToCell(cell, "Edit", this.EditText, false, string.Empty);
			}
		}
	}
}
