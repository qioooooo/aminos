using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Text;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000517 RID: 1303
	[Bindable(false)]
	[ControlBuilder(typeof(TableCellControlBuilder))]
	[DefaultProperty("Text")]
	[ParseChildren(false)]
	[ToolboxItem(false)]
	[Designer("System.Web.UI.Design.WebControls.PreviewControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class TableCell : WebControl
	{
		// Token: 0x06003FC7 RID: 16327 RVA: 0x0010961C File Offset: 0x0010861C
		public TableCell()
			: this(HtmlTextWriterTag.Td)
		{
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x00109626 File Offset: 0x00108626
		internal TableCell(HtmlTextWriterTag tagKey)
			: base(tagKey)
		{
			base.PreventAutoID();
		}

		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x00109638 File Offset: 0x00108638
		// (set) Token: 0x06003FCA RID: 16330 RVA: 0x00109670 File Offset: 0x00108670
		[DefaultValue(null)]
		[WebCategory("Accessibility")]
		[TypeConverter(typeof(StringArrayConverter))]
		[WebSysDescription("TableCell_AssociatedHeaderCellID")]
		public virtual string[] AssociatedHeaderCellID
		{
			get
			{
				object obj = this.ViewState["AssociatedHeaderCellID"];
				if (obj == null)
				{
					return new string[0];
				}
				return (string[])((string[])obj).Clone();
			}
			set
			{
				if (value != null)
				{
					this.ViewState["AssociatedHeaderCellID"] = (string[])value.Clone();
					return;
				}
				this.ViewState["AssociatedHeaderCellID"] = null;
			}
		}

		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06003FCB RID: 16331 RVA: 0x001096A4 File Offset: 0x001086A4
		// (set) Token: 0x06003FCC RID: 16332 RVA: 0x001096CD File Offset: 0x001086CD
		[WebSysDescription("TableCell_ColumnSpan")]
		[WebCategory("Appearance")]
		[DefaultValue(0)]
		public virtual int ColumnSpan
		{
			get
			{
				object obj = this.ViewState["ColumnSpan"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["ColumnSpan"] = value;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06003FCD RID: 16333 RVA: 0x001096F4 File Offset: 0x001086F4
		// (set) Token: 0x06003FCE RID: 16334 RVA: 0x00109710 File Offset: 0x00108710
		[DefaultValue(HorizontalAlign.NotSet)]
		[WebCategory("Layout")]
		[WebSysDescription("TableItem_HorizontalAlign")]
		public virtual HorizontalAlign HorizontalAlign
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return HorizontalAlign.NotSet;
				}
				return ((TableItemStyle)base.ControlStyle).HorizontalAlign;
			}
			set
			{
				((TableItemStyle)base.ControlStyle).HorizontalAlign = value;
			}
		}

		// Token: 0x17000F2A RID: 3882
		// (get) Token: 0x06003FCF RID: 16335 RVA: 0x00109724 File Offset: 0x00108724
		// (set) Token: 0x06003FD0 RID: 16336 RVA: 0x0010974D File Offset: 0x0010874D
		[WebSysDescription("TableCell_RowSpan")]
		[WebCategory("Layout")]
		[DefaultValue(0)]
		public virtual int RowSpan
		{
			get
			{
				object obj = this.ViewState["RowSpan"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["RowSpan"] = value;
			}
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x00109774 File Offset: 0x00108774
		// (set) Token: 0x06003FD2 RID: 16338 RVA: 0x001097A1 File Offset: 0x001087A1
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("TableCell_Text")]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public virtual string Text
		{
			get
			{
				object obj = this.ViewState["Text"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				if (this.HasControls())
				{
					this.Controls.Clear();
				}
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06003FD3 RID: 16339 RVA: 0x001097C7 File Offset: 0x001087C7
		// (set) Token: 0x06003FD4 RID: 16340 RVA: 0x001097E3 File Offset: 0x001087E3
		[WebSysDescription("TableItem_VerticalAlign")]
		[WebCategory("Layout")]
		[DefaultValue(VerticalAlign.NotSet)]
		public virtual VerticalAlign VerticalAlign
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return VerticalAlign.NotSet;
				}
				return ((TableItemStyle)base.ControlStyle).VerticalAlign;
			}
			set
			{
				((TableItemStyle)base.ControlStyle).VerticalAlign = value;
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06003FD5 RID: 16341 RVA: 0x001097F6 File Offset: 0x001087F6
		// (set) Token: 0x06003FD6 RID: 16342 RVA: 0x00109812 File Offset: 0x00108812
		[WebCategory("Layout")]
		[WebSysDescription("TableCell_Wrap")]
		[DefaultValue(true)]
		public virtual bool Wrap
		{
			get
			{
				return !base.ControlStyleCreated || ((TableItemStyle)base.ControlStyle).Wrap;
			}
			set
			{
				((TableItemStyle)base.ControlStyle).Wrap = value;
			}
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x00109828 File Offset: 0x00108828
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			int num = this.ColumnSpan;
			if (num > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Colspan, num.ToString(NumberFormatInfo.InvariantInfo));
			}
			num = this.RowSpan;
			if (num > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Rowspan, num.ToString(NumberFormatInfo.InvariantInfo));
			}
			string[] associatedHeaderCellID = this.AssociatedHeaderCellID;
			if (associatedHeaderCellID.Length > 0)
			{
				bool flag = true;
				StringBuilder stringBuilder = new StringBuilder();
				Control namingContainer = this.NamingContainer;
				foreach (string text in associatedHeaderCellID)
				{
					TableHeaderCell tableHeaderCell = namingContainer.FindControl(text) as TableHeaderCell;
					if (tableHeaderCell == null)
					{
						throw new HttpException(SR.GetString("TableCell_AssociatedHeaderCellNotFound", new object[] { text }));
					}
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.Append(tableHeaderCell.ClientID);
				}
				string text2 = stringBuilder.ToString();
				if (!string.IsNullOrEmpty(text2))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Headers, text2);
				}
			}
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x00109928 File Offset: 0x00108928
		protected override void AddParsedSubObject(object obj)
		{
			if (this.HasControls())
			{
				base.AddParsedSubObject(obj);
				return;
			}
			if (obj is LiteralControl)
			{
				if (this._textSetByAddParsedSubObject)
				{
					this.Text += ((LiteralControl)obj).Text;
				}
				else
				{
					this.Text = ((LiteralControl)obj).Text;
				}
				this._textSetByAddParsedSubObject = true;
				return;
			}
			string text = this.Text;
			if (text.Length != 0)
			{
				this.Text = string.Empty;
				base.AddParsedSubObject(new LiteralControl(text));
			}
			base.AddParsedSubObject(obj);
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x001099B9 File Offset: 0x001089B9
		protected override Style CreateControlStyle()
		{
			return new TableItemStyle(this.ViewState);
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x001099C6 File Offset: 0x001089C6
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (base.HasRenderingData())
			{
				base.RenderContents(writer);
				return;
			}
			writer.Write(this.Text);
		}

		// Token: 0x0400281B RID: 10267
		private bool _textSetByAddParsedSubObject;
	}
}
