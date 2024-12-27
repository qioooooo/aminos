using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004CD RID: 1229
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BoundColumn : DataGridColumn
	{
		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003AFD RID: 15101 RVA: 0x000F8A3C File Offset: 0x000F7A3C
		// (set) Token: 0x06003AFE RID: 15102 RVA: 0x000F8A69 File Offset: 0x000F7A69
		[WebSysDescription("BoundColumn_DataField")]
		[DefaultValue("")]
		[WebCategory("Data")]
		public virtual string DataField
		{
			get
			{
				object obj = base.ViewState["DataField"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["DataField"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003AFF RID: 15103 RVA: 0x000F8A84 File Offset: 0x000F7A84
		// (set) Token: 0x06003B00 RID: 15104 RVA: 0x000F8AB1 File Offset: 0x000F7AB1
		[WebSysDescription("BoundColumn_DataFormatString")]
		[DefaultValue("")]
		[WebCategory("Behavior")]
		public virtual string DataFormatString
		{
			get
			{
				object obj = base.ViewState["DataFormatString"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				base.ViewState["DataFormatString"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003B01 RID: 15105 RVA: 0x000F8ACC File Offset: 0x000F7ACC
		// (set) Token: 0x06003B02 RID: 15106 RVA: 0x000F8AF5 File Offset: 0x000F7AF5
		[DefaultValue(false)]
		[WebSysDescription("BoundColumn_ReadOnly")]
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
				base.ViewState["ReadOnly"] = value;
				this.OnColumnChanged();
			}
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x000F8B14 File Offset: 0x000F7B14
		protected virtual string FormatDataValue(object dataValue)
		{
			string text = string.Empty;
			if (!DataBinder.IsNull(dataValue))
			{
				if (this.formatting.Length == 0)
				{
					text = dataValue.ToString();
				}
				else
				{
					text = string.Format(CultureInfo.CurrentCulture, this.formatting, new object[] { dataValue });
				}
			}
			return text;
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x000F8B63 File Offset: 0x000F7B63
		public override void Initialize()
		{
			base.Initialize();
			this.boundFieldDesc = null;
			this.boundFieldDescValid = false;
			this.boundField = this.DataField;
			this.formatting = this.DataFormatString;
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x000F8B94 File Offset: 0x000F7B94
		public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
		{
			base.InitializeCell(cell, columnIndex, itemType);
			Control control = null;
			Control control2 = null;
			switch (itemType)
			{
			case ListItemType.Header:
			case ListItemType.Footer:
				goto IL_005F;
			case ListItemType.Item:
			case ListItemType.AlternatingItem:
			case ListItemType.SelectedItem:
				break;
			case ListItemType.EditItem:
				if (!this.ReadOnly)
				{
					TextBox textBox = new TextBox();
					control = textBox;
					if (this.boundField.Length != 0)
					{
						control2 = textBox;
						goto IL_005F;
					}
					goto IL_005F;
				}
				break;
			default:
				goto IL_005F;
			}
			if (this.DataField.Length != 0)
			{
				control2 = cell;
			}
			IL_005F:
			if (control != null)
			{
				cell.Controls.Add(control);
			}
			if (control2 != null)
			{
				control2.DataBinding += this.OnDataBindColumn;
			}
		}

		// Token: 0x06003B06 RID: 15110 RVA: 0x000F8C24 File Offset: 0x000F7C24
		private void OnDataBindColumn(object sender, EventArgs e)
		{
			Control control = (Control)sender;
			DataGridItem dataGridItem = (DataGridItem)control.NamingContainer;
			object dataItem = dataGridItem.DataItem;
			if (!this.boundFieldDescValid)
			{
				if (!this.boundField.Equals(BoundColumn.thisExpr))
				{
					this.boundFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(this.boundField, true);
					if (this.boundFieldDesc == null && !base.DesignMode)
					{
						throw new HttpException(SR.GetString("Field_Not_Found", new object[] { this.boundField }));
					}
				}
				this.boundFieldDescValid = true;
			}
			object obj = dataItem;
			string text;
			if (this.boundFieldDesc == null && base.DesignMode)
			{
				text = SR.GetString("Sample_Databound_Text");
			}
			else
			{
				if (this.boundFieldDesc != null)
				{
					obj = this.boundFieldDesc.GetValue(dataItem);
				}
				text = this.FormatDataValue(obj);
			}
			if (control is TableCell)
			{
				if (text.Length == 0)
				{
					text = "&nbsp;";
				}
				((TableCell)control).Text = text;
				return;
			}
			((TextBox)control).Text = text;
		}

		// Token: 0x040026A9 RID: 9897
		public static readonly string thisExpr = "!";

		// Token: 0x040026AA RID: 9898
		private PropertyDescriptor boundFieldDesc;

		// Token: 0x040026AB RID: 9899
		private bool boundFieldDescValid;

		// Token: 0x040026AC RID: 9900
		private string boundField;

		// Token: 0x040026AD RID: 9901
		private string formatting;
	}
}
