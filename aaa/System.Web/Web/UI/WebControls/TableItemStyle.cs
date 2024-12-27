using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000544 RID: 1348
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class TableItemStyle : Style
	{
		// Token: 0x06004267 RID: 16999 RVA: 0x0011321C File Offset: 0x0011221C
		public TableItemStyle()
		{
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x00113224 File Offset: 0x00112224
		public TableItemStyle(StateBag bag)
			: base(bag)
		{
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06004269 RID: 17001 RVA: 0x0011322D File Offset: 0x0011222D
		private bool EnableLegacyRendering
		{
			get
			{
				return RuntimeConfig.GetAppConfig().XhtmlConformance.Mode == XhtmlConformanceMode.Legacy;
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x0600426A RID: 17002 RVA: 0x00113241 File Offset: 0x00112241
		// (set) Token: 0x0600426B RID: 17003 RVA: 0x00113267 File Offset: 0x00112267
		[NotifyParentProperty(true)]
		[WebSysDescription("TableItem_HorizontalAlign")]
		[WebCategory("Layout")]
		[DefaultValue(HorizontalAlign.NotSet)]
		public virtual HorizontalAlign HorizontalAlign
		{
			get
			{
				if (base.IsSet(65536))
				{
					return (HorizontalAlign)base.ViewState["HorizontalAlign"];
				}
				return HorizontalAlign.NotSet;
			}
			set
			{
				if (value < HorizontalAlign.NotSet || value > HorizontalAlign.Justify)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["HorizontalAlign"] = value;
				this.SetBit(65536);
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x0600426C RID: 17004 RVA: 0x0011329D File Offset: 0x0011229D
		// (set) Token: 0x0600426D RID: 17005 RVA: 0x001132C3 File Offset: 0x001122C3
		[DefaultValue(VerticalAlign.NotSet)]
		[WebSysDescription("TableItem_VerticalAlign")]
		[WebCategory("Layout")]
		[NotifyParentProperty(true)]
		public virtual VerticalAlign VerticalAlign
		{
			get
			{
				if (base.IsSet(131072))
				{
					return (VerticalAlign)base.ViewState["VerticalAlign"];
				}
				return VerticalAlign.NotSet;
			}
			set
			{
				if (value < VerticalAlign.NotSet || value > VerticalAlign.Bottom)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["VerticalAlign"] = value;
				this.SetBit(131072);
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x001132F9 File Offset: 0x001122F9
		// (set) Token: 0x0600426F RID: 17007 RVA: 0x0011331F File Offset: 0x0011231F
		[NotifyParentProperty(true)]
		[WebCategory("Layout")]
		[DefaultValue(true)]
		[WebSysDescription("TableItemStyle_Wrap")]
		public virtual bool Wrap
		{
			get
			{
				return !base.IsSet(262144) || (bool)base.ViewState["Wrap"];
			}
			set
			{
				base.ViewState["Wrap"] = value;
				this.SetBit(262144);
			}
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x00113344 File Offset: 0x00112344
		public override void AddAttributesToRender(HtmlTextWriter writer, WebControl owner)
		{
			base.AddAttributesToRender(writer, owner);
			if (!this.Wrap)
			{
				if (this.IsControlEnableLegacyRendering(owner))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Nowrap, "nowrap");
				}
				else
				{
					writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
				}
			}
			HorizontalAlign horizontalAlign = this.HorizontalAlign;
			if (horizontalAlign != HorizontalAlign.NotSet)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(HorizontalAlign));
				writer.AddAttribute(HtmlTextWriterAttribute.Align, converter.ConvertToString(horizontalAlign).ToLower(CultureInfo.InvariantCulture));
			}
			VerticalAlign verticalAlign = this.VerticalAlign;
			if (verticalAlign != VerticalAlign.NotSet)
			{
				TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(VerticalAlign));
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, converter2.ConvertToString(verticalAlign).ToLower(CultureInfo.InvariantCulture));
			}
		}

		// Token: 0x06004271 RID: 17009 RVA: 0x001133F8 File Offset: 0x001123F8
		public override void CopyFrom(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				base.CopyFrom(s);
				if (s is TableItemStyle)
				{
					TableItemStyle tableItemStyle = (TableItemStyle)s;
					if (s.RegisteredCssClass.Length != 0)
					{
						if (tableItemStyle.IsSet(262144))
						{
							base.ViewState.Remove("Wrap");
							base.ClearBit(262144);
						}
					}
					else if (tableItemStyle.IsSet(262144))
					{
						this.Wrap = tableItemStyle.Wrap;
					}
					if (tableItemStyle.IsSet(65536))
					{
						this.HorizontalAlign = tableItemStyle.HorizontalAlign;
					}
					if (tableItemStyle.IsSet(131072))
					{
						this.VerticalAlign = tableItemStyle.VerticalAlign;
					}
				}
			}
		}

		// Token: 0x06004272 RID: 17010 RVA: 0x001134B1 File Offset: 0x001124B1
		private bool IsControlEnableLegacyRendering(Control control)
		{
			if (control != null)
			{
				return control.EnableLegacyRendering;
			}
			return this.EnableLegacyRendering;
		}

		// Token: 0x06004273 RID: 17011 RVA: 0x001134C4 File Offset: 0x001124C4
		public override void MergeWith(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				if (this.IsEmpty)
				{
					this.CopyFrom(s);
					return;
				}
				base.MergeWith(s);
				if (s is TableItemStyle)
				{
					TableItemStyle tableItemStyle = (TableItemStyle)s;
					if (s.RegisteredCssClass.Length == 0 && tableItemStyle.IsSet(262144) && !base.IsSet(262144))
					{
						this.Wrap = tableItemStyle.Wrap;
					}
					if (tableItemStyle.IsSet(65536) && !base.IsSet(65536))
					{
						this.HorizontalAlign = tableItemStyle.HorizontalAlign;
					}
					if (tableItemStyle.IsSet(131072) && !base.IsSet(131072))
					{
						this.VerticalAlign = tableItemStyle.VerticalAlign;
					}
				}
			}
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x0011358C File Offset: 0x0011258C
		public override void Reset()
		{
			if (base.IsSet(65536))
			{
				base.ViewState.Remove("HorizontalAlign");
			}
			if (base.IsSet(131072))
			{
				base.ViewState.Remove("VerticalAlign");
			}
			if (base.IsSet(262144))
			{
				base.ViewState.Remove("Wrap");
			}
			base.Reset();
		}

		// Token: 0x06004275 RID: 17013 RVA: 0x001135F6 File Offset: 0x001125F6
		private void ResetWrap()
		{
			base.ViewState.Remove("Wrap");
			base.ClearBit(262144);
		}

		// Token: 0x06004276 RID: 17014 RVA: 0x00113613 File Offset: 0x00112613
		private bool ShouldSerializeWrap()
		{
			return base.IsSet(262144);
		}

		// Token: 0x04002911 RID: 10513
		internal const int PROP_HORZALIGN = 65536;

		// Token: 0x04002912 RID: 10514
		internal const int PROP_VERTALIGN = 131072;

		// Token: 0x04002913 RID: 10515
		internal const int PROP_WRAP = 262144;
	}
}
