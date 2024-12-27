using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004E6 RID: 1254
	[ToolboxItem(false)]
	[DefaultProperty("Cells")]
	[ParseChildren(true, "Cells")]
	[Bindable(false)]
	[Designer("System.Web.UI.Design.WebControls.PreviewControlDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class TableRow : WebControl
	{
		// Token: 0x06003CEB RID: 15595 RVA: 0x00100346 File Offset: 0x000FF346
		public TableRow()
			: base(HtmlTextWriterTag.Tr)
		{
			base.PreventAutoID();
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06003CEC RID: 15596 RVA: 0x00100356 File Offset: 0x000FF356
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[MergableProperty(false)]
		[WebSysDescription("TableRow_Cells")]
		public virtual TableCellCollection Cells
		{
			get
			{
				if (this.cells == null)
				{
					this.cells = new TableCellCollection(this);
				}
				return this.cells;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06003CED RID: 15597 RVA: 0x00100372 File Offset: 0x000FF372
		// (set) Token: 0x06003CEE RID: 15598 RVA: 0x0010038E File Offset: 0x000FF38E
		[DefaultValue(HorizontalAlign.NotSet)]
		[WebSysDescription("TableItem_HorizontalAlign")]
		[WebCategory("Layout")]
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

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06003CEF RID: 15599 RVA: 0x001003A4 File Offset: 0x000FF3A4
		// (set) Token: 0x06003CF0 RID: 15600 RVA: 0x001003D0 File Offset: 0x000FF3D0
		[WebSysDescription("TableRow_TableSection")]
		[DefaultValue(TableRowSection.TableBody)]
		[WebCategory("Accessibility")]
		public virtual TableRowSection TableSection
		{
			get
			{
				object obj = this.ViewState["TableSection"];
				if (obj != null)
				{
					return (TableRowSection)obj;
				}
				return TableRowSection.TableBody;
			}
			set
			{
				if (value < TableRowSection.TableHeader || value > TableRowSection.TableFooter)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["TableSection"] = value;
				if (value != TableRowSection.TableBody)
				{
					Control parent = this.Parent;
					if (parent != null)
					{
						Table table = parent as Table;
						if (table != null)
						{
							table.HasRowSections = true;
						}
					}
				}
			}
		}

		// Token: 0x17000E2E RID: 3630
		// (get) Token: 0x06003CF1 RID: 15601 RVA: 0x00100425 File Offset: 0x000FF425
		// (set) Token: 0x06003CF2 RID: 15602 RVA: 0x00100441 File Offset: 0x000FF441
		[DefaultValue(VerticalAlign.NotSet)]
		[WebCategory("Layout")]
		[WebSysDescription("TableItem_VerticalAlign")]
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

		// Token: 0x06003CF3 RID: 15603 RVA: 0x00100454 File Offset: 0x000FF454
		protected override Style CreateControlStyle()
		{
			return new TableItemStyle(this.ViewState);
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x00100461 File Offset: 0x000FF461
		protected override ControlCollection CreateControlCollection()
		{
			return new TableRow.CellControlCollection(this);
		}

		// Token: 0x0400274B RID: 10059
		private TableCellCollection cells;

		// Token: 0x020004E7 RID: 1255
		protected class CellControlCollection : ControlCollection
		{
			// Token: 0x06003CF5 RID: 15605 RVA: 0x00100469 File Offset: 0x000FF469
			internal CellControlCollection(Control owner)
				: base(owner)
			{
			}

			// Token: 0x06003CF6 RID: 15606 RVA: 0x00100474 File Offset: 0x000FF474
			public override void Add(Control child)
			{
				if (child is TableCell)
				{
					base.Add(child);
					return;
				}
				throw new ArgumentException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
				{
					"TableRow",
					child.GetType().Name.ToString(CultureInfo.InvariantCulture)
				}));
			}

			// Token: 0x06003CF7 RID: 15607 RVA: 0x001004C8 File Offset: 0x000FF4C8
			public override void AddAt(int index, Control child)
			{
				if (child is TableCell)
				{
					base.AddAt(index, child);
					return;
				}
				throw new ArgumentException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
				{
					"TableRow",
					child.GetType().Name.ToString(CultureInfo.InvariantCulture)
				}));
			}
		}
	}
}
