using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004DA RID: 1242
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ButtonField : ButtonFieldBase
	{
		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06003BAE RID: 15278 RVA: 0x000FAD3C File Offset: 0x000F9D3C
		// (set) Token: 0x06003BAF RID: 15279 RVA: 0x000FAD69 File Offset: 0x000F9D69
		[WebSysDescription("WebControl_CommandName")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
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
				if (!object.Equals(value, base.ViewState["CommandName"]))
				{
					base.ViewState["CommandName"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06003BB0 RID: 15280 RVA: 0x000FAD9C File Offset: 0x000F9D9C
		// (set) Token: 0x06003BB1 RID: 15281 RVA: 0x000FADC9 File Offset: 0x000F9DC9
		[DefaultValue("")]
		[WebCategory("Data")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebSysDescription("ButtonField_DataTextField")]
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
				if (!object.Equals(value, base.ViewState["DataTextField"]))
				{
					base.ViewState["DataTextField"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06003BB2 RID: 15282 RVA: 0x000FADFC File Offset: 0x000F9DFC
		// (set) Token: 0x06003BB3 RID: 15283 RVA: 0x000FAE29 File Offset: 0x000F9E29
		[WebSysDescription("ButtonField_DataTextFormatString")]
		[WebCategory("Data")]
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
				if (!object.Equals(value, base.ViewState["DataTextFormatString"]))
				{
					base.ViewState["DataTextFormatString"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06003BB4 RID: 15284 RVA: 0x000FAE5C File Offset: 0x000F9E5C
		// (set) Token: 0x06003BB5 RID: 15285 RVA: 0x000FAE89 File Offset: 0x000F9E89
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[UrlProperty]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("ButtonField_ImageUrl")]
		public virtual string ImageUrl
		{
			get
			{
				object obj = base.ViewState["ImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["ImageUrl"]))
				{
					base.ViewState["ImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06003BB6 RID: 15286 RVA: 0x000FAEBC File Offset: 0x000F9EBC
		// (set) Token: 0x06003BB7 RID: 15287 RVA: 0x000FAEE9 File Offset: 0x000F9EE9
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("ButtonField_Text")]
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
				if (!object.Equals(value, base.ViewState["Text"]))
				{
					base.ViewState["Text"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000FAF1C File Offset: 0x000F9F1C
		protected override void CopyProperties(DataControlField newField)
		{
			((ButtonField)newField).CommandName = this.CommandName;
			((ButtonField)newField).DataTextField = this.DataTextField;
			((ButtonField)newField).DataTextFormatString = this.DataTextFormatString;
			((ButtonField)newField).ImageUrl = this.ImageUrl;
			((ButtonField)newField).Text = this.Text;
			base.CopyProperties(newField);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x000FAF85 File Offset: 0x000F9F85
		protected override DataControlField CreateField()
		{
			return new ButtonField();
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x000FAF8C File Offset: 0x000F9F8C
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

		// Token: 0x06003BBB RID: 15291 RVA: 0x000FAFD8 File Offset: 0x000F9FD8
		public override bool Initialize(bool sortingEnabled, Control control)
		{
			base.Initialize(sortingEnabled, control);
			this.textFieldDesc = null;
			return false;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x000FAFEC File Offset: 0x000F9FEC
		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (cellType != DataControlCellType.Header && cellType != DataControlCellType.Footer)
			{
				IPostBackContainer postBackContainer = base.Control as IPostBackContainer;
				bool causesValidation = this.CausesValidation;
				bool flag = true;
				IButtonControl buttonControl;
				switch (this.ButtonType)
				{
				case ButtonType.Button:
					if (postBackContainer != null && !causesValidation)
					{
						buttonControl = new DataControlButton(postBackContainer);
						flag = false;
						goto IL_00A5;
					}
					buttonControl = new Button();
					goto IL_00A5;
				case ButtonType.Link:
					if (postBackContainer != null && !causesValidation)
					{
						buttonControl = new DataControlLinkButton(postBackContainer);
						flag = false;
						goto IL_00A5;
					}
					buttonControl = new DataControlLinkButton(null);
					goto IL_00A5;
				}
				if (postBackContainer != null && !causesValidation)
				{
					buttonControl = new DataControlImageButton(postBackContainer);
					flag = false;
				}
				else
				{
					buttonControl = new ImageButton();
				}
				((ImageButton)buttonControl).ImageUrl = this.ImageUrl;
				IL_00A5:
				buttonControl.Text = this.Text;
				buttonControl.CommandName = this.CommandName;
				buttonControl.CommandArgument = rowIndex.ToString(CultureInfo.InvariantCulture);
				if (flag)
				{
					buttonControl.CausesValidation = causesValidation;
				}
				buttonControl.ValidationGroup = this.ValidationGroup;
				if (this.DataTextField.Length != 0 && base.Visible)
				{
					((WebControl)buttonControl).DataBinding += this.OnDataBindField;
				}
				cell.Controls.Add((WebControl)buttonControl);
			}
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x000FB11C File Offset: 0x000FA11C
		private void OnDataBindField(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			Control namingContainer = control.NamingContainer;
			if (namingContainer == null)
			{
				throw new HttpException(SR.GetString("DataControlField_NoContainer"));
			}
			object dataItem = DataBinder.GetDataItem(namingContainer);
			if (dataItem == null && !base.DesignMode)
			{
				throw new HttpException(SR.GetString("DataItem_Not_Found"));
			}
			if (this.textFieldDesc == null && dataItem != null)
			{
				string dataTextField = this.DataTextField;
				this.textFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(dataTextField, true);
				if (this.textFieldDesc == null && !base.DesignMode)
				{
					throw new HttpException(SR.GetString("Field_Not_Found", new object[] { dataTextField }));
				}
			}
			string text;
			if (this.textFieldDesc != null && dataItem != null)
			{
				object value = this.textFieldDesc.GetValue(dataItem);
				text = this.FormatDataTextValue(value);
			}
			else
			{
				text = SR.GetString("Sample_Databound_Text");
			}
			((IButtonControl)control).Text = text;
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x000FB200 File Offset: 0x000FA200
		public override void ValidateSupportsCallback()
		{
		}

		// Token: 0x040026CF RID: 9935
		private PropertyDescriptor textFieldDesc;
	}
}
