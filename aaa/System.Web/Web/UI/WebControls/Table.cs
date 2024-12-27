using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004F3 RID: 1267
	[ParseChildren(true, "Rows")]
	[SupportsEventValidation]
	[Designer("System.Web.UI.Design.WebControls.TableDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Rows")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Table : WebControl, IPostBackEventHandler
	{
		// Token: 0x06003DCE RID: 15822 RVA: 0x00102BD4 File Offset: 0x00101BD4
		public Table()
			: base(HtmlTextWriterTag.Table)
		{
		}

		// Token: 0x17000E87 RID: 3719
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x00102BDE File Offset: 0x00101BDE
		// (set) Token: 0x06003DD0 RID: 15824 RVA: 0x00102BFE File Offset: 0x00101BFE
		[WebSysDescription("Table_BackImageUrl")]
		[UrlProperty]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string BackImageUrl
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return string.Empty;
				}
				return ((TableStyle)base.ControlStyle).BackImageUrl;
			}
			set
			{
				((TableStyle)base.ControlStyle).BackImageUrl = value;
			}
		}

		// Token: 0x17000E88 RID: 3720
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x00102C14 File Offset: 0x00101C14
		// (set) Token: 0x06003DD2 RID: 15826 RVA: 0x00102C41 File Offset: 0x00101C41
		[WebSysDescription("Table_Caption")]
		[Localizable(true)]
		[WebCategory("Accessibility")]
		[DefaultValue("")]
		public virtual string Caption
		{
			get
			{
				string text = (string)this.ViewState["Caption"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Caption"] = value;
			}
		}

		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x00102C54 File Offset: 0x00101C54
		// (set) Token: 0x06003DD4 RID: 15828 RVA: 0x00102C7D File Offset: 0x00101C7D
		[DefaultValue(TableCaptionAlign.NotSet)]
		[WebCategory("Accessibility")]
		[WebSysDescription("WebControl_CaptionAlign")]
		public virtual TableCaptionAlign CaptionAlign
		{
			get
			{
				object obj = this.ViewState["CaptionAlign"];
				if (obj == null)
				{
					return TableCaptionAlign.NotSet;
				}
				return (TableCaptionAlign)obj;
			}
			set
			{
				if (value < TableCaptionAlign.NotSet || value > TableCaptionAlign.Right)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["CaptionAlign"] = value;
			}
		}

		// Token: 0x17000E8A RID: 3722
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x00102CA8 File Offset: 0x00101CA8
		// (set) Token: 0x06003DD6 RID: 15830 RVA: 0x00102CC4 File Offset: 0x00101CC4
		[WebCategory("Appearance")]
		[DefaultValue(-1)]
		[WebSysDescription("Table_CellPadding")]
		public virtual int CellPadding
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return -1;
				}
				return ((TableStyle)base.ControlStyle).CellPadding;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellPadding = value;
			}
		}

		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06003DD7 RID: 15831 RVA: 0x00102CD7 File Offset: 0x00101CD7
		// (set) Token: 0x06003DD8 RID: 15832 RVA: 0x00102CF3 File Offset: 0x00101CF3
		[WebSysDescription("Table_CellSpacing")]
		[DefaultValue(-1)]
		[WebCategory("Appearance")]
		public virtual int CellSpacing
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return -1;
				}
				return ((TableStyle)base.ControlStyle).CellSpacing;
			}
			set
			{
				((TableStyle)base.ControlStyle).CellSpacing = value;
			}
		}

		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x00102D06 File Offset: 0x00101D06
		// (set) Token: 0x06003DDA RID: 15834 RVA: 0x00102D22 File Offset: 0x00101D22
		[DefaultValue(GridLines.None)]
		[WebSysDescription("Table_GridLines")]
		[WebCategory("Appearance")]
		public virtual GridLines GridLines
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return GridLines.None;
				}
				return ((TableStyle)base.ControlStyle).GridLines;
			}
			set
			{
				((TableStyle)base.ControlStyle).GridLines = value;
			}
		}

		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06003DDB RID: 15835 RVA: 0x00102D35 File Offset: 0x00101D35
		// (set) Token: 0x06003DDC RID: 15836 RVA: 0x00102D3D File Offset: 0x00101D3D
		internal bool HasRowSections
		{
			get
			{
				return this._hasRowSections;
			}
			set
			{
				this._hasRowSections = value;
			}
		}

		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06003DDD RID: 15837 RVA: 0x00102D46 File Offset: 0x00101D46
		// (set) Token: 0x06003DDE RID: 15838 RVA: 0x00102D62 File Offset: 0x00101D62
		[WebCategory("Layout")]
		[DefaultValue(HorizontalAlign.NotSet)]
		[WebSysDescription("Table_HorizontalAlign")]
		public virtual HorizontalAlign HorizontalAlign
		{
			get
			{
				if (!base.ControlStyleCreated)
				{
					return HorizontalAlign.NotSet;
				}
				return ((TableStyle)base.ControlStyle).HorizontalAlign;
			}
			set
			{
				((TableStyle)base.ControlStyle).HorizontalAlign = value;
			}
		}

		// Token: 0x17000E8F RID: 3727
		// (get) Token: 0x06003DDF RID: 15839 RVA: 0x00102D75 File Offset: 0x00101D75
		[MergableProperty(false)]
		[WebSysDescription("Table_Rows")]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public virtual TableRowCollection Rows
		{
			get
			{
				if (this._rows == null)
				{
					this._rows = new TableRowCollection(this);
				}
				return this._rows;
			}
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x00102D94 File Offset: 0x00101D94
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			string text = "0";
			if (base.ControlStyleCreated)
			{
				if (base.EnableLegacyRendering || writer is Html32TextWriter)
				{
					Color borderColor = this.BorderColor;
					if (!borderColor.IsEmpty)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Bordercolor, ColorTranslator.ToHtml(borderColor));
					}
				}
				Unit borderWidth = this.BorderWidth;
				GridLines gridLines = this.GridLines;
				if (gridLines != GridLines.None)
				{
					if (borderWidth.IsEmpty || borderWidth.Type != UnitType.Pixel)
					{
						text = "1";
					}
					else
					{
						text = ((int)borderWidth.Value).ToString(NumberFormatInfo.InvariantInfo);
					}
				}
			}
			writer.AddAttribute(HtmlTextWriterAttribute.Border, text);
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00102E2E File Offset: 0x00101E2E
		protected override ControlCollection CreateControlCollection()
		{
			return new Table.RowControlCollection(this);
		}

		// Token: 0x06003DE2 RID: 15842 RVA: 0x00102E36 File Offset: 0x00101E36
		protected override Style CreateControlStyle()
		{
			return new TableStyle(this.ViewState);
		}

		// Token: 0x06003DE3 RID: 15843 RVA: 0x00102E44 File Offset: 0x00101E44
		protected virtual void RaisePostBackEvent(string argument)
		{
			base.ValidateEvent(this.UniqueID, argument);
			if (this._adapter != null)
			{
				IPostBackEventHandler postBackEventHandler = this._adapter as IPostBackEventHandler;
				if (postBackEventHandler != null)
				{
					postBackEventHandler.RaisePostBackEvent(argument);
				}
			}
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x00102E7C File Offset: 0x00101E7C
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			base.RenderBeginTag(writer);
			string caption = this.Caption;
			if (caption.Length != 0)
			{
				TableCaptionAlign captionAlign = this.CaptionAlign;
				if (captionAlign != TableCaptionAlign.NotSet)
				{
					string text = "Right";
					switch (captionAlign)
					{
					case TableCaptionAlign.Top:
						text = "Top";
						break;
					case TableCaptionAlign.Bottom:
						text = "Bottom";
						break;
					case TableCaptionAlign.Left:
						text = "Left";
						break;
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Align, text);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Caption);
				writer.Write(caption);
				writer.RenderEndTag();
			}
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x00102EFC File Offset: 0x00101EFC
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			TableRowCollection rows = this.Rows;
			int count = rows.Count;
			if (count > 0)
			{
				if (this.HasRowSections)
				{
					TableRowSection tableRowSection = TableRowSection.TableHeader;
					bool flag = false;
					foreach (object obj in rows)
					{
						TableRow tableRow = (TableRow)obj;
						if (tableRow.TableSection < tableRowSection)
						{
							throw new HttpException(SR.GetString("Table_SectionsMustBeInOrder", new object[] { this.ID }));
						}
						if (tableRowSection < tableRow.TableSection || (tableRow.TableSection == TableRowSection.TableHeader && !flag))
						{
							if (flag)
							{
								writer.RenderEndTag();
							}
							tableRowSection = tableRow.TableSection;
							flag = true;
							switch (tableRowSection)
							{
							case TableRowSection.TableHeader:
								writer.RenderBeginTag(HtmlTextWriterTag.Thead);
								break;
							case TableRowSection.TableBody:
								writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
								break;
							case TableRowSection.TableFooter:
								writer.RenderBeginTag(HtmlTextWriterTag.Tfoot);
								break;
							}
						}
						tableRow.RenderControl(writer);
					}
					writer.RenderEndTag();
					return;
				}
				foreach (object obj2 in rows)
				{
					TableRow tableRow2 = (TableRow)obj2;
					tableRow2.RenderControl(writer);
				}
			}
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x00103064 File Offset: 0x00102064
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x0400278A RID: 10122
		private TableRowCollection _rows;

		// Token: 0x0400278B RID: 10123
		private bool _hasRowSections;

		// Token: 0x020004F4 RID: 1268
		protected class RowControlCollection : ControlCollection
		{
			// Token: 0x06003DE7 RID: 15847 RVA: 0x0010306D File Offset: 0x0010206D
			internal RowControlCollection(Control owner)
				: base(owner)
			{
			}

			// Token: 0x06003DE8 RID: 15848 RVA: 0x00103078 File Offset: 0x00102078
			public override void Add(Control child)
			{
				if (child is TableRow)
				{
					base.Add(child);
					return;
				}
				throw new ArgumentException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
				{
					"Table",
					child.GetType().Name.ToString(CultureInfo.InvariantCulture)
				}));
			}

			// Token: 0x06003DE9 RID: 15849 RVA: 0x001030CC File Offset: 0x001020CC
			public override void AddAt(int index, Control child)
			{
				if (child is TableRow)
				{
					base.AddAt(index, child);
					return;
				}
				throw new ArgumentException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
				{
					"Table",
					child.GetType().Name.ToString(CultureInfo.InvariantCulture)
				}));
			}
		}
	}
}
