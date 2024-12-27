using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005BE RID: 1470
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ImageField : DataControlField
	{
		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x060047D3 RID: 18387 RVA: 0x00125980 File Offset: 0x00124980
		// (set) Token: 0x060047D4 RID: 18388 RVA: 0x001259AD File Offset: 0x001249AD
		[DefaultValue("")]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("ImageField_AlternateText")]
		public virtual string AlternateText
		{
			get
			{
				object obj = base.ViewState["AlternateText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["AlternateText"]))
				{
					base.ViewState["AlternateText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x060047D5 RID: 18389 RVA: 0x001259E0 File Offset: 0x001249E0
		// (set) Token: 0x060047D6 RID: 18390 RVA: 0x00125A09 File Offset: 0x00124A09
		[WebSysDescription("ImageField_ConvertEmptyStringToNull")]
		[WebCategory("Behavior")]
		[DefaultValue(true)]
		public virtual bool ConvertEmptyStringToNull
		{
			get
			{
				object obj = base.ViewState["ConvertEmptyStringToNull"];
				return obj == null || (bool)obj;
			}
			set
			{
				base.ViewState["ConvertEmptyStringToNull"] = value;
			}
		}

		// Token: 0x170011B7 RID: 4535
		// (get) Token: 0x060047D7 RID: 18391 RVA: 0x00125A24 File Offset: 0x00124A24
		// (set) Token: 0x060047D8 RID: 18392 RVA: 0x00125A51 File Offset: 0x00124A51
		[WebSysDescription("ImageField_DataAlternateTextField")]
		[DefaultValue("")]
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Data")]
		public virtual string DataAlternateTextField
		{
			get
			{
				object obj = base.ViewState["DataAlternateTextField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DataAlternateTextField"]))
				{
					base.ViewState["DataAlternateTextField"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011B8 RID: 4536
		// (get) Token: 0x060047D9 RID: 18393 RVA: 0x00125A84 File Offset: 0x00124A84
		// (set) Token: 0x060047DA RID: 18394 RVA: 0x00125AB1 File Offset: 0x00124AB1
		[WebCategory("Data")]
		[WebSysDescription("ImageField_DataAlternateTextFormatString")]
		[DefaultValue("")]
		public virtual string DataAlternateTextFormatString
		{
			get
			{
				object obj = base.ViewState["DataAlternateTextFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DataAlternateTextFormatString"]))
				{
					base.ViewState["DataAlternateTextFormatString"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011B9 RID: 4537
		// (get) Token: 0x060047DB RID: 18395 RVA: 0x00125AE4 File Offset: 0x00124AE4
		// (set) Token: 0x060047DC RID: 18396 RVA: 0x00125B2C File Offset: 0x00124B2C
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Data")]
		[WebSysDescription("ImageField_ImageUrlField")]
		[DefaultValue("")]
		public virtual string DataImageUrlField
		{
			get
			{
				if (this._dataField == null)
				{
					object obj = base.ViewState["DataImageUrlField"];
					if (obj != null)
					{
						this._dataField = (string)obj;
					}
					else
					{
						this._dataField = string.Empty;
					}
				}
				return this._dataField;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DataImageUrlField"]))
				{
					base.ViewState["DataImageUrlField"] = value;
					this._dataField = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011BA RID: 4538
		// (get) Token: 0x060047DD RID: 18397 RVA: 0x00125B64 File Offset: 0x00124B64
		// (set) Token: 0x060047DE RID: 18398 RVA: 0x00125B91 File Offset: 0x00124B91
		[WebSysDescription("ImageField_ImageUrlFormatString")]
		[DefaultValue("")]
		[WebCategory("Data")]
		public virtual string DataImageUrlFormatString
		{
			get
			{
				object obj = base.ViewState["DataImageUrlFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DataImageUrlFormatString"]))
				{
					base.ViewState["DataImageUrlFormatString"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011BB RID: 4539
		// (get) Token: 0x060047DF RID: 18399 RVA: 0x00125BC4 File Offset: 0x00124BC4
		// (set) Token: 0x060047E0 RID: 18400 RVA: 0x00125BF1 File Offset: 0x00124BF1
		[Localizable(true)]
		[WebSysDescription("BoundField_NullDisplayText")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		public virtual string NullDisplayText
		{
			get
			{
				object obj = base.ViewState["NullDisplayText"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["NullDisplayText"]))
				{
					base.ViewState["NullDisplayText"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x060047E1 RID: 18401 RVA: 0x00125C24 File Offset: 0x00124C24
		// (set) Token: 0x060047E2 RID: 18402 RVA: 0x00125C51 File Offset: 0x00124C51
		[UrlProperty]
		[WebSysDescription("ImageField_NullImageUrl")]
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string NullImageUrl
		{
			get
			{
				object obj = base.ViewState["NullImageUrl"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["NullImageUrl"]))
				{
					base.ViewState["NullImageUrl"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x060047E3 RID: 18403 RVA: 0x00125C84 File Offset: 0x00124C84
		// (set) Token: 0x060047E4 RID: 18404 RVA: 0x00125CB0 File Offset: 0x00124CB0
		[DefaultValue(false)]
		[WebSysDescription("ImageField_ReadOnly")]
		[WebCategory("Behavior")]
		public virtual bool ReadOnly
		{
			get
			{
				object obj = base.ViewState["ReadOnly"];
				return obj != null && (bool)obj;
			}
			set
			{
				object obj = base.ViewState["ReadOnly"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["ReadOnly"] = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x00125CF8 File Offset: 0x00124CF8
		protected override void CopyProperties(DataControlField newField)
		{
			((ImageField)newField).AlternateText = this.AlternateText;
			((ImageField)newField).ConvertEmptyStringToNull = this.ConvertEmptyStringToNull;
			((ImageField)newField).DataAlternateTextField = this.DataAlternateTextField;
			((ImageField)newField).DataAlternateTextFormatString = this.DataAlternateTextFormatString;
			((ImageField)newField).DataImageUrlField = this.DataImageUrlField;
			((ImageField)newField).DataImageUrlFormatString = this.DataImageUrlFormatString;
			((ImageField)newField).NullDisplayText = this.NullDisplayText;
			((ImageField)newField).NullImageUrl = this.NullImageUrl;
			((ImageField)newField).ReadOnly = this.ReadOnly;
			base.CopyProperties(newField);
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x00125DA5 File Offset: 0x00124DA5
		protected override DataControlField CreateField()
		{
			return new ImageField();
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x00125DAC File Offset: 0x00124DAC
		public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
		{
			string dataImageUrlField = this.DataImageUrlField;
			object obj = null;
			bool flag = false;
			if ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal && !this.InsertVisible)
			{
				return;
			}
			if (cell.Controls.Count == 0)
			{
				return;
			}
			Control control = cell.Controls[0];
			Image image = control as Image;
			if (image != null)
			{
				if (includeReadOnly)
				{
					flag = true;
					if (image.Visible)
					{
						obj = image.ImageUrl;
					}
				}
			}
			else
			{
				TextBox textBox = control as TextBox;
				if (textBox != null)
				{
					obj = textBox.Text;
					flag = true;
				}
			}
			if (obj != null || flag)
			{
				if (this.ConvertEmptyStringToNull && obj is string && ((string)obj).Length == 0)
				{
					obj = null;
				}
				if (dictionary.Contains(dataImageUrlField))
				{
					dictionary[dataImageUrlField] = obj;
					return;
				}
				dictionary.Add(dataImageUrlField, obj);
			}
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x00125E6C File Offset: 0x00124E6C
		protected virtual string FormatImageUrlValue(object dataValue)
		{
			string text = string.Empty;
			string dataImageUrlFormatString = this.DataImageUrlFormatString;
			if (!DataBinder.IsNull(dataValue))
			{
				string text2 = dataValue.ToString();
				if (text2.Length > 0)
				{
					if (dataImageUrlFormatString.Length == 0)
					{
						text = text2;
					}
					else
					{
						text = string.Format(CultureInfo.CurrentCulture, dataImageUrlFormatString, new object[] { dataValue });
					}
				}
				return text;
			}
			return null;
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x00125EC8 File Offset: 0x00124EC8
		protected virtual string GetFormattedAlternateText(Control controlContainer)
		{
			string dataAlternateTextField = this.DataAlternateTextField;
			string dataAlternateTextFormatString = this.DataAlternateTextFormatString;
			string text2;
			if (dataAlternateTextField.Length > 0)
			{
				object value = this.GetValue(controlContainer, dataAlternateTextField, ref this._altTextFieldDesc);
				string text = string.Empty;
				if (!DataBinder.IsNull(value))
				{
					text = value.ToString();
				}
				if (dataAlternateTextFormatString.Length > 0)
				{
					text2 = string.Format(CultureInfo.CurrentCulture, dataAlternateTextFormatString, new object[] { value });
				}
				else
				{
					text2 = text;
				}
			}
			else
			{
				text2 = this.AlternateText;
			}
			return text2;
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x00125F45 File Offset: 0x00124F45
		protected virtual string GetDesignTimeValue()
		{
			return SR.GetString("Sample_Databound_Text");
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x00125F54 File Offset: 0x00124F54
		protected virtual object GetValue(Control controlContainer, string fieldName, ref PropertyDescriptor cachedDescriptor)
		{
			object obj = null;
			if (controlContainer == null)
			{
				throw new HttpException(SR.GetString("DataControlField_NoContainer"));
			}
			object dataItem = DataBinder.GetDataItem(controlContainer);
			if (dataItem == null && !base.DesignMode)
			{
				throw new HttpException(SR.GetString("DataItem_Not_Found"));
			}
			if (cachedDescriptor == null && !fieldName.Equals(ImageField.ThisExpression))
			{
				cachedDescriptor = TypeDescriptor.GetProperties(dataItem).Find(fieldName, true);
				if (cachedDescriptor == null && !base.DesignMode)
				{
					throw new HttpException(SR.GetString("Field_Not_Found", new object[] { fieldName }));
				}
			}
			if (cachedDescriptor != null && dataItem != null)
			{
				obj = cachedDescriptor.GetValue(dataItem);
			}
			else if (!base.DesignMode)
			{
				obj = dataItem;
			}
			return obj;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x00125FFF File Offset: 0x00124FFF
		public override bool Initialize(bool enableSorting, Control control)
		{
			base.Initialize(enableSorting, control);
			this._imageFieldDesc = null;
			this._altTextFieldDesc = null;
			return false;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x0012601C File Offset: 0x0012501C
		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (cellType != DataControlCellType.DataCell)
			{
				return;
			}
			this.InitializeDataCell(cell, rowState);
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x00126044 File Offset: 0x00125044
		protected virtual void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
		{
			Control control = null;
			if (((rowState & DataControlRowState.Edit) != DataControlRowState.Normal && !this.ReadOnly) || (rowState & DataControlRowState.Insert) != DataControlRowState.Normal)
			{
				TextBox textBox = new TextBox();
				cell.Controls.Add(textBox);
				if (this.DataImageUrlField.Length != 0 && (rowState & DataControlRowState.Edit) != DataControlRowState.Normal)
				{
					control = textBox;
				}
			}
			else if (this.DataImageUrlField.Length != 0)
			{
				control = cell;
				Image image = new Image();
				Label label = new Label();
				cell.Controls.Add(image);
				cell.Controls.Add(label);
			}
			if (control != null && base.Visible)
			{
				control.DataBinding += this.OnDataBindField;
			}
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x001260E0 File Offset: 0x001250E0
		protected virtual void OnDataBindField(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			Control namingContainer = control.NamingContainer;
			string nullImageUrl = this.NullImageUrl;
			string formattedAlternateText = this.GetFormattedAlternateText(namingContainer);
			if (base.DesignMode && control is TableCell)
			{
				if (control.Controls.Count == 0 || !(control.Controls[0] is Image))
				{
					throw new HttpException(SR.GetString("ImageField_WrongControlType", new object[] { this.DataImageUrlField }));
				}
				((Image)control.Controls[0]).Visible = false;
				((TableCell)control).Text = this.GetDesignTimeValue();
				return;
			}
			else
			{
				object value = this.GetValue(namingContainer, this.DataImageUrlField, ref this._imageFieldDesc);
				string text = this.FormatImageUrlValue(value);
				if (control is TableCell)
				{
					TableCell tableCell = (TableCell)control;
					if (tableCell.Controls.Count < 2 || !(tableCell.Controls[0] is Image) || !(tableCell.Controls[1] is Label))
					{
						throw new HttpException(SR.GetString("ImageField_WrongControlType", new object[] { this.DataImageUrlField }));
					}
					Image image = (Image)tableCell.Controls[0];
					Label label = (Label)tableCell.Controls[1];
					label.Visible = false;
					if (text == null || (this.ConvertEmptyStringToNull && text.Length == 0))
					{
						if (nullImageUrl.Length > 0)
						{
							text = nullImageUrl;
						}
						else
						{
							image.Visible = false;
							label.Text = this.NullDisplayText;
							label.Visible = true;
						}
					}
					if (!CrossSiteScriptingValidation.IsDangerousUrl(text))
					{
						image.ImageUrl = text;
					}
					image.AlternateText = formattedAlternateText;
					return;
				}
				else
				{
					if (!(control is TextBox))
					{
						throw new HttpException(SR.GetString("ImageField_WrongControlType", new object[] { this.DataImageUrlField }));
					}
					((TextBox)control).Text = value.ToString();
					((TextBox)control).ToolTip = formattedAlternateText;
					if (value != null && value.GetType().IsPrimitive)
					{
						((TextBox)control).Columns = 5;
					}
					return;
				}
			}
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x00126306 File Offset: 0x00125306
		public override void ValidateSupportsCallback()
		{
		}

		// Token: 0x04002ABB RID: 10939
		public static readonly string ThisExpression = "!";

		// Token: 0x04002ABC RID: 10940
		private PropertyDescriptor _imageFieldDesc;

		// Token: 0x04002ABD RID: 10941
		private PropertyDescriptor _altTextFieldDesc;

		// Token: 0x04002ABE RID: 10942
		private string _dataField;
	}
}
