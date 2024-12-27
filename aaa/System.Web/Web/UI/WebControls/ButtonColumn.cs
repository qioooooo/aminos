using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004D7 RID: 1239
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ButtonColumn : DataGridColumn
	{
		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06003B91 RID: 15249 RVA: 0x000FA720 File Offset: 0x000F9720
		// (set) Token: 0x06003B92 RID: 15250 RVA: 0x000FA749 File Offset: 0x000F9749
		[WebCategory("Appearance")]
		[WebSysDescription("ButtonColumn_ButtonType")]
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

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06003B93 RID: 15251 RVA: 0x000FA77C File Offset: 0x000F977C
		// (set) Token: 0x06003B94 RID: 15252 RVA: 0x000FA7A5 File Offset: 0x000F97A5
		[WebSysDescription("ButtonColumn_CausesValidation")]
		[DefaultValue(false)]
		public virtual bool CausesValidation
		{
			get
			{
				object obj = base.ViewState["CausesValidation"];
				return obj != null && (bool)obj;
			}
			set
			{
				base.ViewState["CausesValidation"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06003B95 RID: 15253 RVA: 0x000FA7C4 File Offset: 0x000F97C4
		// (set) Token: 0x06003B96 RID: 15254 RVA: 0x000FA7F1 File Offset: 0x000F97F1
		[DefaultValue("")]
		[WebCategory("Behavior")]
		[WebSysDescription("WebControl_CommandName")]
		public virtual string CommandName
		{
			get
			{
				object obj = base.ViewState["CommandName"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["CommandName"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06003B97 RID: 15255 RVA: 0x000FA80C File Offset: 0x000F980C
		// (set) Token: 0x06003B98 RID: 15256 RVA: 0x000FA839 File Offset: 0x000F9839
		[WebSysDescription("ButtonColumn_DataTextField")]
		[WebCategory("Data")]
		[DefaultValue("")]
		public virtual string DataTextField
		{
			get
			{
				object obj = base.ViewState["DataTextField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["DataTextField"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06003B99 RID: 15257 RVA: 0x000FA854 File Offset: 0x000F9854
		// (set) Token: 0x06003B9A RID: 15258 RVA: 0x000FA881 File Offset: 0x000F9881
		[WebCategory("Data")]
		[WebSysDescription("ButtonColumn_DataTextFormatString")]
		[DefaultValue("")]
		public virtual string DataTextFormatString
		{
			get
			{
				object obj = base.ViewState["DataTextFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["DataTextFormatString"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06003B9B RID: 15259 RVA: 0x000FA89C File Offset: 0x000F989C
		// (set) Token: 0x06003B9C RID: 15260 RVA: 0x000FA8C9 File Offset: 0x000F98C9
		[WebSysDescription("ButtonColumn_Text")]
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		public virtual string Text
		{
			get
			{
				object obj = base.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["Text"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06003B9D RID: 15261 RVA: 0x000FA8E4 File Offset: 0x000F98E4
		// (set) Token: 0x06003B9E RID: 15262 RVA: 0x000FA911 File Offset: 0x000F9911
		[DefaultValue("")]
		[WebSysDescription("ButtonColumn_ValidationGroup")]
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

		// Token: 0x06003B9F RID: 15263 RVA: 0x000FA92C File Offset: 0x000F992C
		protected virtual string FormatDataTextValue(object dataTextValue)
		{
			string text = string.Empty;
			if (!DataBinder.IsNull(dataTextValue))
			{
				string dataTextFormatString = this.DataTextFormatString;
				if (dataTextFormatString.Length == 0)
				{
					text = dataTextValue.ToString();
				}
				else
				{
					text = string.Format(CultureInfo.CurrentCulture, dataTextFormatString, new object[] { dataTextValue });
				}
			}
			return text;
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x000FA978 File Offset: 0x000F9978
		public override void Initialize()
		{
			base.Initialize();
			this.textFieldDesc = null;
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x000FA988 File Offset: 0x000F9988
		public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
		{
			base.InitializeCell(cell, columnIndex, itemType);
			if (itemType != ListItemType.Header && itemType != ListItemType.Footer)
			{
				WebControl webControl;
				if (this.ButtonType == ButtonColumnType.LinkButton)
				{
					webControl = new DataGridLinkButton
					{
						Text = this.Text,
						CommandName = this.CommandName,
						CausesValidation = this.CausesValidation,
						ValidationGroup = this.ValidationGroup
					};
				}
				else
				{
					webControl = new Button
					{
						Text = this.Text,
						CommandName = this.CommandName,
						CausesValidation = this.CausesValidation,
						ValidationGroup = this.ValidationGroup
					};
				}
				if (this.DataTextField.Length != 0)
				{
					webControl.DataBinding += this.OnDataBindColumn;
				}
				cell.Controls.Add(webControl);
			}
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000FAA54 File Offset: 0x000F9A54
		private void OnDataBindColumn(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			DataGridItem dataGridItem = (DataGridItem)control.NamingContainer;
			object dataItem = dataGridItem.DataItem;
			if (this.textFieldDesc == null)
			{
				string dataTextField = this.DataTextField;
				this.textFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(dataTextField, true);
				if (this.textFieldDesc == null && !base.DesignMode)
				{
					throw new HttpException(SR.GetString("Field_Not_Found", new object[] { dataTextField }));
				}
			}
			string text;
			if (this.textFieldDesc != null)
			{
				object value = this.textFieldDesc.GetValue(dataItem);
				text = this.FormatDataTextValue(value);
			}
			else
			{
				text = SR.GetString("Sample_Databound_Text");
			}
			if (control is LinkButton)
			{
				((LinkButton)control).Text = text;
				return;
			}
			((Button)control).Text = text;
		}

		// Token: 0x040026CB RID: 9931
		private PropertyDescriptor textFieldDesc;
	}
}
