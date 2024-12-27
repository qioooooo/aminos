using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004C4 RID: 1220
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BoundField : DataControlField
	{
		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x000F5740 File Offset: 0x000F4740
		// (set) Token: 0x06003A17 RID: 14871 RVA: 0x000F5769 File Offset: 0x000F4769
		[WebSysDescription("BoundField_ApplyFormatInEditMode")]
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		public virtual bool ApplyFormatInEditMode
		{
			get
			{
				object obj = base.ViewState["ApplyFormatInEditMode"];
				return obj != null && (bool)obj;
			}
			set
			{
				base.ViewState["ApplyFormatInEditMode"] = value;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003A18 RID: 14872 RVA: 0x000F5784 File Offset: 0x000F4784
		// (set) Token: 0x06003A19 RID: 14873 RVA: 0x000F57AD File Offset: 0x000F47AD
		[WebSysDescription("BoundField_ConvertEmptyStringToNull")]
		[DefaultValue(true)]
		[WebCategory("Behavior")]
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

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06003A1A RID: 14874 RVA: 0x000F57C8 File Offset: 0x000F47C8
		// (set) Token: 0x06003A1B RID: 14875 RVA: 0x000F5810 File Offset: 0x000F4810
		[TypeConverter("System.Web.UI.Design.DataSourceViewSchemaConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[WebCategory("Data")]
		[WebSysDescription("BoundField_DataField")]
		[DefaultValue("")]
		public virtual string DataField
		{
			get
			{
				if (this._dataField == null)
				{
					object obj = base.ViewState["DataField"];
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
				if (!object.Equals(value, base.ViewState["DataField"]))
				{
					base.ViewState["DataField"] = value;
					this._dataField = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x000F5848 File Offset: 0x000F4848
		// (set) Token: 0x06003A1D RID: 14877 RVA: 0x000F5890 File Offset: 0x000F4890
		[DefaultValue("")]
		[WebCategory("Data")]
		[WebSysDescription("BoundField_DataFormatString")]
		public virtual string DataFormatString
		{
			get
			{
				if (this._dataFormatString == null)
				{
					object obj = base.ViewState["DataFormatString"];
					if (obj != null)
					{
						this._dataFormatString = (string)obj;
					}
					else
					{
						this._dataFormatString = string.Empty;
					}
				}
				return this._dataFormatString;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["DataFormatString"]))
				{
					base.ViewState["DataFormatString"] = value;
					this._dataFormatString = value;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06003A1E RID: 14878 RVA: 0x000F58C8 File Offset: 0x000F48C8
		// (set) Token: 0x06003A1F RID: 14879 RVA: 0x000F58D0 File Offset: 0x000F48D0
		public override string HeaderText
		{
			get
			{
				return base.HeaderText;
			}
			set
			{
				if (!object.Equals(value, base.ViewState["HeaderText"]))
				{
					base.ViewState["HeaderText"] = value;
					if (!this._suppressHeaderTextFieldChange)
					{
						this.OnFieldChanged();
					}
				}
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06003A20 RID: 14880 RVA: 0x000F590C File Offset: 0x000F490C
		// (set) Token: 0x06003A21 RID: 14881 RVA: 0x000F5958 File Offset: 0x000F4958
		[WebSysDescription("BoundField_HtmlEncode")]
		[DefaultValue(true)]
		[WebCategory("Behavior")]
		public virtual bool HtmlEncode
		{
			get
			{
				if (!this._htmlEncodeSet)
				{
					object obj = base.ViewState["HtmlEncode"];
					if (obj != null)
					{
						this._htmlEncode = (bool)obj;
					}
					else
					{
						this._htmlEncode = true;
					}
					this._htmlEncodeSet = true;
				}
				return this._htmlEncode;
			}
			set
			{
				object obj = base.ViewState["HtmlEncode"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["HtmlEncode"] = value;
					this._htmlEncode = value;
					this._htmlEncodeSet = true;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003A22 RID: 14882 RVA: 0x000F59AC File Offset: 0x000F49AC
		// (set) Token: 0x06003A23 RID: 14883 RVA: 0x000F59F8 File Offset: 0x000F49F8
		[DefaultValue(true)]
		[WebCategory("Behavior")]
		public virtual bool HtmlEncodeFormatString
		{
			get
			{
				if (!this._htmlEncodeFormatStringSet)
				{
					object obj = base.ViewState["HtmlEncodeFormatString"];
					if (obj != null)
					{
						this._htmlEncodeFormatString = (bool)obj;
					}
					else
					{
						this._htmlEncodeFormatString = true;
					}
					this._htmlEncodeFormatStringSet = true;
				}
				return this._htmlEncodeFormatString;
			}
			set
			{
				object obj = base.ViewState["HtmlEncodeFormatString"];
				if (obj == null || (bool)obj != value)
				{
					base.ViewState["HtmlEncodeFormatString"] = value;
					this._htmlEncodeFormatString = value;
					this._htmlEncodeFormatStringSet = true;
					this.OnFieldChanged();
				}
			}
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06003A24 RID: 14884 RVA: 0x000F5A4C File Offset: 0x000F4A4C
		// (set) Token: 0x06003A25 RID: 14885 RVA: 0x000F5A79 File Offset: 0x000F4A79
		[DefaultValue("")]
		[WebSysDescription("BoundField_NullDisplayText")]
		[WebCategory("Behavior")]
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

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x000F5AAC File Offset: 0x000F4AAC
		// (set) Token: 0x06003A27 RID: 14887 RVA: 0x000F5AD8 File Offset: 0x000F4AD8
		[WebSysDescription("BoundField_ReadOnly")]
		[DefaultValue(false)]
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

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x000F5B1E File Offset: 0x000F4B1E
		protected virtual bool SupportsHtmlEncode
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x000F5B24 File Offset: 0x000F4B24
		protected override void CopyProperties(DataControlField newField)
		{
			((BoundField)newField).ApplyFormatInEditMode = this.ApplyFormatInEditMode;
			((BoundField)newField).ConvertEmptyStringToNull = this.ConvertEmptyStringToNull;
			((BoundField)newField).DataField = this.DataField;
			((BoundField)newField).DataFormatString = this.DataFormatString;
			((BoundField)newField).HtmlEncode = this.HtmlEncode;
			((BoundField)newField).HtmlEncodeFormatString = this.HtmlEncodeFormatString;
			((BoundField)newField).NullDisplayText = this.NullDisplayText;
			((BoundField)newField).ReadOnly = this.ReadOnly;
			base.CopyProperties(newField);
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x000F5BC0 File Offset: 0x000F4BC0
		protected override DataControlField CreateField()
		{
			return new BoundField();
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x000F5BC8 File Offset: 0x000F4BC8
		public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
		{
			string dataField = this.DataField;
			object obj = null;
			string nullDisplayText = this.NullDisplayText;
			if ((rowState & DataControlRowState.Insert) != DataControlRowState.Normal && !this.InsertVisible)
			{
				return;
			}
			if (cell.Controls.Count > 0)
			{
				Control control = cell.Controls[0];
				TextBox textBox = control as TextBox;
				if (textBox != null)
				{
					obj = textBox.Text;
				}
			}
			else if (includeReadOnly)
			{
				string text = cell.Text;
				if (text == "&nbsp;")
				{
					obj = string.Empty;
				}
				else if (this.SupportsHtmlEncode && this.HtmlEncode)
				{
					obj = HttpUtility.HtmlDecode(text);
				}
				else
				{
					obj = text;
				}
			}
			if (obj != null)
			{
				if (obj is string && ((string)obj).Length == 0 && this.ConvertEmptyStringToNull)
				{
					obj = null;
				}
				if (obj is string && (string)obj == nullDisplayText && nullDisplayText.Length > 0)
				{
					obj = null;
				}
				if (dictionary.Contains(dataField))
				{
					dictionary[dataField] = obj;
					return;
				}
				dictionary.Add(dataField, obj);
			}
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x000F5CC4 File Offset: 0x000F4CC4
		protected virtual string FormatDataValue(object dataValue, bool encode)
		{
			string text = string.Empty;
			if (!DataBinder.IsNull(dataValue))
			{
				string text2 = dataValue.ToString();
				string dataFormatString = this.DataFormatString;
				int length = text2.Length;
				if (!this.HtmlEncodeFormatString)
				{
					if (length > 0 && encode)
					{
						text2 = HttpUtility.HtmlEncode(text2);
					}
					if (length == 0 && this.ConvertEmptyStringToNull)
					{
						text = this.NullDisplayText;
					}
					else if (dataFormatString.Length == 0)
					{
						text = text2;
					}
					else if (encode)
					{
						text = string.Format(CultureInfo.CurrentCulture, dataFormatString, new object[] { text2 });
					}
					else
					{
						text = string.Format(CultureInfo.CurrentCulture, dataFormatString, new object[] { dataValue });
					}
				}
				else
				{
					if (length == 0 && this.ConvertEmptyStringToNull)
					{
						text2 = this.NullDisplayText;
					}
					else
					{
						if (!string.IsNullOrEmpty(dataFormatString))
						{
							text2 = string.Format(CultureInfo.CurrentCulture, dataFormatString, new object[] { dataValue });
						}
						if (!string.IsNullOrEmpty(text2) && encode)
						{
							text2 = HttpUtility.HtmlEncode(text2);
						}
					}
					text = text2;
				}
			}
			else
			{
				text = this.NullDisplayText;
			}
			return text;
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x000F5DC5 File Offset: 0x000F4DC5
		protected virtual object GetDesignTimeValue()
		{
			return SR.GetString("Sample_Databound_Text");
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x000F5DD4 File Offset: 0x000F4DD4
		protected virtual object GetValue(Control controlContainer)
		{
			string dataField = this.DataField;
			if (controlContainer == null)
			{
				throw new HttpException(SR.GetString("DataControlField_NoContainer"));
			}
			object dataItem = DataBinder.GetDataItem(controlContainer);
			if (dataItem == null && !base.DesignMode)
			{
				throw new HttpException(SR.GetString("DataItem_Not_Found"));
			}
			if (this._boundFieldDesc == null && !dataField.Equals(BoundField.ThisExpression))
			{
				this._boundFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(dataField, true);
				if (this._boundFieldDesc == null && !base.DesignMode)
				{
					throw new HttpException(SR.GetString("Field_Not_Found", new object[] { dataField }));
				}
			}
			object obj;
			if (this._boundFieldDesc != null && dataItem != null)
			{
				obj = this._boundFieldDesc.GetValue(dataItem);
			}
			else if (base.DesignMode)
			{
				obj = this.GetDesignTimeValue();
			}
			else
			{
				obj = dataItem;
			}
			return obj;
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x000F5EA3 File Offset: 0x000F4EA3
		public override bool Initialize(bool enableSorting, Control control)
		{
			base.Initialize(enableSorting, control);
			this._boundFieldDesc = null;
			return false;
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x000F5EB8 File Offset: 0x000F4EB8
		public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
		{
			string text = null;
			bool flag = false;
			bool flag2 = false;
			if (cellType == DataControlCellType.Header && this.SupportsHtmlEncode && this.HtmlEncode)
			{
				text = this.HeaderText;
				flag2 = true;
			}
			if (flag2 && !string.IsNullOrEmpty(text))
			{
				this._suppressHeaderTextFieldChange = true;
				this.HeaderText = HttpUtility.HtmlEncode(text);
				flag = true;
			}
			base.InitializeCell(cell, cellType, rowState, rowIndex);
			if (flag)
			{
				this.HeaderText = text;
				this._suppressHeaderTextFieldChange = false;
			}
			if (cellType != DataControlCellType.DataCell)
			{
				return;
			}
			this.InitializeDataCell(cell, rowState);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x000F5F34 File Offset: 0x000F4F34
		protected virtual void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
		{
			Control control = null;
			Control control2 = null;
			if (((rowState & DataControlRowState.Edit) != DataControlRowState.Normal && !this.ReadOnly) || (rowState & DataControlRowState.Insert) != DataControlRowState.Normal)
			{
				TextBox textBox = new TextBox();
				textBox.ToolTip = this.HeaderText;
				control = textBox;
				if (this.DataField.Length != 0 && (rowState & DataControlRowState.Edit) != DataControlRowState.Normal)
				{
					control2 = textBox;
				}
			}
			else if (this.DataField.Length != 0)
			{
				control2 = cell;
			}
			if (control != null)
			{
				cell.Controls.Add(control);
			}
			if (control2 != null && base.Visible)
			{
				control2.DataBinding += this.OnDataBindField;
			}
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x000F5FC0 File Offset: 0x000F4FC0
		protected virtual void OnDataBindField(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			Control namingContainer = control.NamingContainer;
			object value = this.GetValue(namingContainer);
			bool flag = this.SupportsHtmlEncode && this.HtmlEncode && control is TableCell;
			string text = this.FormatDataValue(value, flag);
			if (control is TableCell)
			{
				if (text.Length == 0)
				{
					text = "&nbsp;";
				}
				((TableCell)control).Text = text;
				return;
			}
			if (!(control is TextBox))
			{
				throw new HttpException(SR.GetString("BoundField_WrongControlType", new object[] { this.DataField }));
			}
			if (this.ApplyFormatInEditMode)
			{
				((TextBox)control).Text = text;
			}
			else if (value != null)
			{
				((TextBox)control).Text = value.ToString();
			}
			if (value != null && value.GetType().IsPrimitive)
			{
				((TextBox)control).Columns = 5;
			}
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x000F60A3 File Offset: 0x000F50A3
		protected override void LoadViewState(object state)
		{
			this._dataField = null;
			this._dataFormatString = null;
			this._htmlEncodeSet = false;
			this._htmlEncodeFormatStringSet = false;
			base.LoadViewState(state);
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x000F60C8 File Offset: 0x000F50C8
		public override void ValidateSupportsCallback()
		{
		}

		// Token: 0x04002675 RID: 9845
		public static readonly string ThisExpression = "!";

		// Token: 0x04002676 RID: 9846
		private PropertyDescriptor _boundFieldDesc;

		// Token: 0x04002677 RID: 9847
		private string _dataField;

		// Token: 0x04002678 RID: 9848
		private string _dataFormatString;

		// Token: 0x04002679 RID: 9849
		private bool _htmlEncode;

		// Token: 0x0400267A RID: 9850
		private bool _htmlEncodeSet;

		// Token: 0x0400267B RID: 9851
		private bool _suppressHeaderTextFieldChange;

		// Token: 0x0400267C RID: 9852
		private bool _htmlEncodeFormatString;

		// Token: 0x0400267D RID: 9853
		private bool _htmlEncodeFormatStringSet;
	}
}
