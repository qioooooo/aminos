using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x020004AA RID: 1194
	[ParseChildren(true, "Rows")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlTable : HtmlContainerControl
	{
		// Token: 0x060037DF RID: 14303 RVA: 0x000EF3CE File Offset: 0x000EE3CE
		public HtmlTable()
			: base("table")
		{
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060037E0 RID: 14304 RVA: 0x000EF3DC File Offset: 0x000EE3DC
		// (set) Token: 0x060037E1 RID: 14305 RVA: 0x000EF404 File Offset: 0x000EE404
		[WebCategory("Layout")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		public string Align
		{
			get
			{
				string text = base.Attributes["align"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["align"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x060037E2 RID: 14306 RVA: 0x000EF41C File Offset: 0x000EE41C
		// (set) Token: 0x060037E3 RID: 14307 RVA: 0x000EF444 File Offset: 0x000EE444
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		public string BgColor
		{
			get
			{
				string text = base.Attributes["bgcolor"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["bgcolor"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x060037E4 RID: 14308 RVA: 0x000EF45C File Offset: 0x000EE45C
		// (set) Token: 0x060037E5 RID: 14309 RVA: 0x000EF48A File Offset: 0x000EE48A
		[WebCategory("Appearance")]
		[DefaultValue(-1)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Border
		{
			get
			{
				string text = base.Attributes["border"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["border"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060037E6 RID: 14310 RVA: 0x000EF4A4 File Offset: 0x000EE4A4
		// (set) Token: 0x060037E7 RID: 14311 RVA: 0x000EF4CC File Offset: 0x000EE4CC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		public string BorderColor
		{
			get
			{
				string text = base.Attributes["bordercolor"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["bordercolor"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C7C RID: 3196
		// (get) Token: 0x060037E8 RID: 14312 RVA: 0x000EF4E4 File Offset: 0x000EE4E4
		// (set) Token: 0x060037E9 RID: 14313 RVA: 0x000EF512 File Offset: 0x000EE512
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Appearance")]
		public int CellPadding
		{
			get
			{
				string text = base.Attributes["cellpadding"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["cellpadding"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x000EF52C File Offset: 0x000EE52C
		// (set) Token: 0x060037EB RID: 14315 RVA: 0x000EF55A File Offset: 0x000EE55A
		[DefaultValue("")]
		[WebCategory("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int CellSpacing
		{
			get
			{
				string text = base.Attributes["cellspacing"];
				if (text == null)
				{
					return -1;
				}
				return int.Parse(text, CultureInfo.InvariantCulture);
			}
			set
			{
				base.Attributes["cellspacing"] = HtmlControl.MapIntegerAttributeToString(value);
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060037EC RID: 14316 RVA: 0x000EF574 File Offset: 0x000EE574
		// (set) Token: 0x060037ED RID: 14317 RVA: 0x000EF5A8 File Offset: 0x000EE5A8
		public override string InnerHtml
		{
			get
			{
				throw new NotSupportedException(SR.GetString("InnerHtml_not_supported", new object[] { base.GetType().Name }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("InnerHtml_not_supported", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x17000C7F RID: 3199
		// (get) Token: 0x060037EE RID: 14318 RVA: 0x000EF5DC File Offset: 0x000EE5DC
		// (set) Token: 0x060037EF RID: 14319 RVA: 0x000EF610 File Offset: 0x000EE610
		public override string InnerText
		{
			get
			{
				throw new NotSupportedException(SR.GetString("InnerText_not_supported", new object[] { base.GetType().Name }));
			}
			set
			{
				throw new NotSupportedException(SR.GetString("InnerText_not_supported", new object[] { base.GetType().Name }));
			}
		}

		// Token: 0x17000C80 RID: 3200
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x000EF644 File Offset: 0x000EE644
		// (set) Token: 0x060037F1 RID: 14321 RVA: 0x000EF66C File Offset: 0x000EE66C
		[DefaultValue("")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Layout")]
		public string Height
		{
			get
			{
				string text = base.Attributes["height"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["height"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C81 RID: 3201
		// (get) Token: 0x060037F2 RID: 14322 RVA: 0x000EF684 File Offset: 0x000EE684
		// (set) Token: 0x060037F3 RID: 14323 RVA: 0x000EF6AC File Offset: 0x000EE6AC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[WebCategory("Layout")]
		[DefaultValue("")]
		public string Width
		{
			get
			{
				string text = base.Attributes["width"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base.Attributes["width"] = HtmlControl.MapStringAttributeToString(value);
			}
		}

		// Token: 0x17000C82 RID: 3202
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x000EF6C4 File Offset: 0x000EE6C4
		[IgnoreUnknownContent]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual HtmlTableRowCollection Rows
		{
			get
			{
				if (this.rows == null)
				{
					this.rows = new HtmlTableRowCollection(this);
				}
				return this.rows;
			}
		}

		// Token: 0x060037F5 RID: 14325 RVA: 0x000EF6E0 File Offset: 0x000EE6E0
		protected internal override void RenderChildren(HtmlTextWriter writer)
		{
			writer.WriteLine();
			writer.Indent++;
			base.RenderChildren(writer);
			writer.Indent--;
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x000EF70B File Offset: 0x000EE70B
		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			base.RenderEndTag(writer);
			writer.WriteLine();
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x000EF71A File Offset: 0x000EE71A
		protected override ControlCollection CreateControlCollection()
		{
			return new HtmlTable.HtmlTableRowControlCollection(this);
		}

		// Token: 0x040025D8 RID: 9688
		private HtmlTableRowCollection rows;

		// Token: 0x020004AB RID: 1195
		protected class HtmlTableRowControlCollection : ControlCollection
		{
			// Token: 0x060037F8 RID: 14328 RVA: 0x000EF722 File Offset: 0x000EE722
			internal HtmlTableRowControlCollection(Control owner)
				: base(owner)
			{
			}

			// Token: 0x060037F9 RID: 14329 RVA: 0x000EF72C File Offset: 0x000EE72C
			public override void Add(Control child)
			{
				if (child is HtmlTableRow)
				{
					base.Add(child);
					return;
				}
				throw new ArgumentException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
				{
					"HtmlTable",
					child.GetType().Name.ToString(CultureInfo.InvariantCulture)
				}));
			}

			// Token: 0x060037FA RID: 14330 RVA: 0x000EF780 File Offset: 0x000EE780
			public override void AddAt(int index, Control child)
			{
				if (child is HtmlTableRow)
				{
					base.AddAt(index, child);
					return;
				}
				throw new ArgumentException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
				{
					"HtmlTable",
					child.GetType().Name.ToString(CultureInfo.InvariantCulture)
				}));
			}
		}
	}
}
