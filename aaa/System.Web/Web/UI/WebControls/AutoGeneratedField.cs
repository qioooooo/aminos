﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004C5 RID: 1221
	[EditorBrowsable(EditorBrowsableState.Never)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AutoGeneratedField : BoundField
	{
		// Token: 0x06003A36 RID: 14902 RVA: 0x000F60D6 File Offset: 0x000F50D6
		public AutoGeneratedField(string dataField)
		{
			this.DataField = dataField;
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x06003A37 RID: 14903 RVA: 0x000F60E5 File Offset: 0x000F50E5
		// (set) Token: 0x06003A38 RID: 14904 RVA: 0x000F60ED File Offset: 0x000F50ED
		public override string DataFormatString
		{
			get
			{
				return base.DataFormatString;
			}
			set
			{
				if (!this._suppressPropertyThrows)
				{
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x06003A39 RID: 14905 RVA: 0x000F6100 File Offset: 0x000F5100
		// (set) Token: 0x06003A3A RID: 14906 RVA: 0x000F6132 File Offset: 0x000F5132
		public Type DataType
		{
			get
			{
				object obj = base.ViewState["DataType"];
				if (obj != null)
				{
					return (Type)obj;
				}
				return typeof(string);
			}
			set
			{
				base.ViewState["DataType"] = value;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x06003A3B RID: 14907 RVA: 0x000F6145 File Offset: 0x000F5145
		// (set) Token: 0x06003A3C RID: 14908 RVA: 0x000F614D File Offset: 0x000F514D
		public override bool InsertVisible
		{
			get
			{
				return base.InsertVisible;
			}
			set
			{
				if (!this._suppressPropertyThrows)
				{
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x000F615D File Offset: 0x000F515D
		// (set) Token: 0x06003A3E RID: 14910 RVA: 0x000F6165 File Offset: 0x000F5165
		public override bool ConvertEmptyStringToNull
		{
			get
			{
				return base.ConvertEmptyStringToNull;
			}
			set
			{
				if (!this._suppressPropertyThrows)
				{
					throw new NotSupportedException();
				}
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06003A3F RID: 14911 RVA: 0x000F6178 File Offset: 0x000F5178
		private bool UseCheckBox
		{
			get
			{
				if (!this._useCheckBoxValid)
				{
					this._useCheckBox = this.DataType == typeof(bool) || this.DataType == typeof(bool?);
					this._useCheckBoxValid = true;
				}
				return this._useCheckBox;
			}
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x000F61C7 File Offset: 0x000F51C7
		protected override void CopyProperties(DataControlField newField)
		{
			((AutoGeneratedField)newField).DataType = this.DataType;
			this._suppressPropertyThrows = true;
			((AutoGeneratedField)newField)._suppressPropertyThrows = true;
			base.CopyProperties(newField);
			this._suppressPropertyThrows = false;
			((AutoGeneratedField)newField)._suppressPropertyThrows = false;
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x000F6207 File Offset: 0x000F5207
		protected override DataControlField CreateField()
		{
			return new AutoGeneratedField(this.DataField);
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x000F6214 File Offset: 0x000F5214
		protected override object GetDesignTimeValue()
		{
			if (this.UseCheckBox)
			{
				return true;
			}
			return base.GetDesignTimeValue();
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x000F622C File Offset: 0x000F522C
		public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
		{
			if (this.UseCheckBox)
			{
				string dataField = this.DataField;
				object obj = null;
				if (cell.Controls.Count > 0)
				{
					Control control = cell.Controls[0];
					CheckBox checkBox = control as CheckBox;
					if (checkBox != null && (includeReadOnly || checkBox.Enabled))
					{
						obj = checkBox.Checked;
					}
				}
				if (obj != null)
				{
					if (dictionary.Contains(dataField))
					{
						dictionary[dataField] = obj;
						return;
					}
					dictionary.Add(dataField, obj);
					return;
				}
			}
			else
			{
				base.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly);
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x000F62B4 File Offset: 0x000F52B4
		protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
		{
			if (this.UseCheckBox)
			{
				CheckBox checkBox = null;
				CheckBox checkBox2 = null;
				if (((rowState & DataControlRowState.Edit) != DataControlRowState.Normal && !this.ReadOnly) || (rowState & DataControlRowState.Insert) != DataControlRowState.Normal)
				{
					CheckBox checkBox3 = new CheckBox();
					checkBox = checkBox3;
					if (this.DataField.Length != 0 && (rowState & DataControlRowState.Edit) != DataControlRowState.Normal)
					{
						checkBox2 = checkBox3;
					}
				}
				else if (this.DataField.Length != 0)
				{
					CheckBox checkBox4 = new CheckBox();
					checkBox4.Enabled = false;
					checkBox = checkBox4;
					checkBox2 = checkBox4;
				}
				if (checkBox != null)
				{
					checkBox.ToolTip = this.HeaderText;
					cell.Controls.Add(checkBox);
				}
				if (checkBox2 != null)
				{
					checkBox2.DataBinding += this.OnDataBindField;
					return;
				}
			}
			else
			{
				base.InitializeDataCell(cell, rowState);
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000F6358 File Offset: 0x000F5358
		protected override void OnDataBindField(object sender, EventArgs e)
		{
			if (this.UseCheckBox)
			{
				Control control = (Control)sender;
				Control namingContainer = control.NamingContainer;
				object value = this.GetValue(namingContainer);
				if (!(control is CheckBox))
				{
					throw new HttpException(SR.GetString("CheckBoxField_WrongControlType", new object[] { this.DataField }));
				}
				if (DataBinder.IsNull(value))
				{
					((CheckBox)control).Checked = false;
					return;
				}
				if (value is bool)
				{
					((CheckBox)control).Checked = (bool)value;
					return;
				}
				try
				{
					((CheckBox)control).Checked = bool.Parse(value.ToString());
					return;
				}
				catch (FormatException ex)
				{
					throw new HttpException(SR.GetString("CheckBoxField_CouldntParseAsBoolean", new object[] { this.DataField }), ex);
				}
			}
			base.OnDataBindField(sender, e);
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000F6438 File Offset: 0x000F5438
		public override void ValidateSupportsCallback()
		{
		}

		// Token: 0x0400267E RID: 9854
		private bool _suppressPropertyThrows;

		// Token: 0x0400267F RID: 9855
		private bool _useCheckBox;

		// Token: 0x04002680 RID: 9856
		private bool _useCheckBoxValid;
	}
}
