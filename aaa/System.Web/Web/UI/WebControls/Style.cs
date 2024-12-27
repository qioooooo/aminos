using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000543 RID: 1347
	[ToolboxItem(false)]
	[TypeConverter(typeof(EmptyStringExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Style : Component, IStateManager
	{
		// Token: 0x0600423B RID: 16955 RVA: 0x001122AB File Offset: 0x001112AB
		public Style()
			: this(null)
		{
			this.ownStateBag = true;
		}

		// Token: 0x0600423C RID: 16956 RVA: 0x001122BB File Offset: 0x001112BB
		public Style(StateBag bag)
		{
			this.statebag = bag;
			this.marked = false;
			this.setBits = 0;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x0600423D RID: 16957 RVA: 0x001122DE File Offset: 0x001112DE
		// (set) Token: 0x0600423E RID: 16958 RVA: 0x00112304 File Offset: 0x00111304
		[NotifyParentProperty(true)]
		[TypeConverter(typeof(WebColorConverter))]
		[WebCategory("Appearance")]
		[DefaultValue(typeof(Color), "")]
		[WebSysDescription("Style_BackColor")]
		public Color BackColor
		{
			get
			{
				if (this.IsSet(8))
				{
					return (Color)this.ViewState["BackColor"];
				}
				return Color.Empty;
			}
			set
			{
				this.ViewState["BackColor"] = value;
				this.SetBit(8);
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x0600423F RID: 16959 RVA: 0x00112323 File Offset: 0x00111323
		// (set) Token: 0x06004240 RID: 16960 RVA: 0x0011234A File Offset: 0x0011134A
		[WebCategory("Appearance")]
		[WebSysDescription("Style_BorderColor")]
		[NotifyParentProperty(true)]
		[TypeConverter(typeof(WebColorConverter))]
		[DefaultValue(typeof(Color), "")]
		public Color BorderColor
		{
			get
			{
				if (this.IsSet(16))
				{
					return (Color)this.ViewState["BorderColor"];
				}
				return Color.Empty;
			}
			set
			{
				this.ViewState["BorderColor"] = value;
				this.SetBit(16);
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06004241 RID: 16961 RVA: 0x0011236A File Offset: 0x0011136A
		// (set) Token: 0x06004242 RID: 16962 RVA: 0x00112394 File Offset: 0x00111394
		[WebCategory("Appearance")]
		[NotifyParentProperty(true)]
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("Style_BorderWidth")]
		public Unit BorderWidth
		{
			get
			{
				if (this.IsSet(32))
				{
					return (Unit)this.ViewState["BorderWidth"];
				}
				return Unit.Empty;
			}
			set
			{
				if (value.Type == UnitType.Percentage || value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Style_InvalidBorderWidth"));
				}
				this.ViewState["BorderWidth"] = value;
				this.SetBit(32);
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06004243 RID: 16963 RVA: 0x001123F0 File Offset: 0x001113F0
		// (set) Token: 0x06004244 RID: 16964 RVA: 0x00112413 File Offset: 0x00111413
		[DefaultValue(BorderStyle.NotSet)]
		[WebSysDescription("Style_BorderStyle")]
		[NotifyParentProperty(true)]
		[WebCategory("Appearance")]
		public BorderStyle BorderStyle
		{
			get
			{
				if (this.IsSet(64))
				{
					return (BorderStyle)this.ViewState["BorderStyle"];
				}
				return BorderStyle.NotSet;
			}
			set
			{
				if (value < BorderStyle.NotSet || value > BorderStyle.Outset)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["BorderStyle"] = value;
				this.SetBit(64);
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06004245 RID: 16965 RVA: 0x00112448 File Offset: 0x00111448
		// (set) Token: 0x06004246 RID: 16966 RVA: 0x00112484 File Offset: 0x00111484
		[NotifyParentProperty(true)]
		[WebSysDescription("Style_CSSClass")]
		[CssClassProperty]
		[DefaultValue("")]
		[WebCategory("Appearance")]
		public string CssClass
		{
			get
			{
				if (!this.IsSet(2))
				{
					return string.Empty;
				}
				string text = (string)this.ViewState["CssClass"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CssClass"] = value;
				this.SetBit(2);
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06004247 RID: 16967 RVA: 0x0011249E File Offset: 0x0011149E
		[NotifyParentProperty(true)]
		[WebCategory("Appearance")]
		[WebSysDescription("Style_Font")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public FontInfo Font
		{
			get
			{
				if (this.fontInfo == null)
				{
					this.fontInfo = new FontInfo(this);
				}
				return this.fontInfo;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06004248 RID: 16968 RVA: 0x001124BA File Offset: 0x001114BA
		// (set) Token: 0x06004249 RID: 16969 RVA: 0x001124E0 File Offset: 0x001114E0
		[NotifyParentProperty(true)]
		[WebSysDescription("Style_ForeColor")]
		[WebCategory("Appearance")]
		[TypeConverter(typeof(WebColorConverter))]
		[DefaultValue(typeof(Color), "")]
		public Color ForeColor
		{
			get
			{
				if (this.IsSet(4))
				{
					return (Color)this.ViewState["ForeColor"];
				}
				return Color.Empty;
			}
			set
			{
				this.ViewState["ForeColor"] = value;
				this.SetBit(4);
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x0600424A RID: 16970 RVA: 0x001124FF File Offset: 0x001114FF
		// (set) Token: 0x0600424B RID: 16971 RVA: 0x0011252C File Offset: 0x0011152C
		[WebCategory("Layout")]
		[NotifyParentProperty(true)]
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("Style_Height")]
		public Unit Height
		{
			get
			{
				if (this.IsSet(128))
				{
					return (Unit)this.ViewState["Height"];
				}
				return Unit.Empty;
			}
			set
			{
				if (value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Style_InvalidHeight"));
				}
				this.ViewState["Height"] = value;
				this.SetBit(128);
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x0600424C RID: 16972 RVA: 0x00112581 File Offset: 0x00111581
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public virtual bool IsEmpty
		{
			get
			{
				return this.setBits == 0 && this.RegisteredCssClass.Length == 0;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x0600424D RID: 16973 RVA: 0x0011259B File Offset: 0x0011159B
		protected bool IsTrackingViewState
		{
			get
			{
				return this.marked;
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x0600424E RID: 16974 RVA: 0x001125A3 File Offset: 0x001115A3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string RegisteredCssClass
		{
			get
			{
				if (this.registeredCssClass == null)
				{
					return string.Empty;
				}
				return this.registeredCssClass;
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x001125B9 File Offset: 0x001115B9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected internal StateBag ViewState
		{
			get
			{
				if (this.statebag == null)
				{
					this.statebag = new StateBag(false);
					if (this.IsTrackingViewState)
					{
						this.statebag.TrackViewState();
					}
				}
				return this.statebag;
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06004250 RID: 16976 RVA: 0x001125E8 File Offset: 0x001115E8
		// (set) Token: 0x06004251 RID: 16977 RVA: 0x00112614 File Offset: 0x00111614
		[NotifyParentProperty(true)]
		[WebCategory("Layout")]
		[DefaultValue(typeof(Unit), "")]
		[WebSysDescription("Style_Width")]
		public Unit Width
		{
			get
			{
				if (this.IsSet(256))
				{
					return (Unit)this.ViewState["Width"];
				}
				return Unit.Empty;
			}
			set
			{
				if (value.Value < 0.0)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Style_InvalidWidth"));
				}
				this.ViewState["Width"] = value;
				this.SetBit(256);
			}
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x00112669 File Offset: 0x00111669
		public void AddAttributesToRender(HtmlTextWriter writer)
		{
			this.AddAttributesToRender(writer, null);
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x00112674 File Offset: 0x00111674
		public virtual void AddAttributesToRender(HtmlTextWriter writer, WebControl owner)
		{
			string text = string.Empty;
			bool flag = true;
			if (this.IsSet(2))
			{
				text = (string)this.ViewState["CssClass"];
				if (text == null)
				{
					text = string.Empty;
				}
			}
			if (!string.IsNullOrEmpty(this.registeredCssClass))
			{
				flag = false;
				if (text.Length != 0)
				{
					text = text + " " + this.registeredCssClass;
				}
				else
				{
					text = this.registeredCssClass;
				}
			}
			if (text.Length > 0)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, text);
			}
			if (flag)
			{
				CssStyleCollection styleAttributes = this.GetStyleAttributes(owner);
				styleAttributes.Render(writer);
			}
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x00112707 File Offset: 0x00111707
		internal void ClearBit(int bit)
		{
			this.setBits &= ~bit;
		}

		// Token: 0x06004255 RID: 16981 RVA: 0x00112718 File Offset: 0x00111718
		public virtual void CopyFrom(Style s)
		{
			if (this.RegisteredCssClass.Length != 0)
			{
				throw new InvalidOperationException(SR.GetString("Style_RegisteredStylesAreReadOnly"));
			}
			if (s != null && !s.IsEmpty)
			{
				this.Font.CopyFrom(s.Font);
				if (s.IsSet(2))
				{
					this.CssClass = s.CssClass;
				}
				if (s.RegisteredCssClass.Length != 0)
				{
					if (this.IsSet(2))
					{
						this.CssClass = this.CssClass + " " + s.RegisteredCssClass;
					}
					else
					{
						this.CssClass = s.RegisteredCssClass;
					}
					if (s.IsSet(8) && s.BackColor != Color.Empty)
					{
						this.ViewState.Remove("BackColor");
						this.ClearBit(8);
					}
					if (s.IsSet(4) && s.ForeColor != Color.Empty)
					{
						this.ViewState.Remove("ForeColor");
						this.ClearBit(4);
					}
					if (s.IsSet(16) && s.BorderColor != Color.Empty)
					{
						this.ViewState.Remove("BorderColor");
						this.ClearBit(16);
					}
					if (s.IsSet(32) && s.BorderWidth != Unit.Empty)
					{
						this.ViewState.Remove("BorderWidth");
						this.ClearBit(32);
					}
					if (s.IsSet(64))
					{
						this.ViewState.Remove("BorderStyle");
						this.ClearBit(64);
					}
					if (s.IsSet(128) && s.Height != Unit.Empty)
					{
						this.ViewState.Remove("Height");
						this.ClearBit(128);
					}
					if (s.IsSet(256) && s.Width != Unit.Empty)
					{
						this.ViewState.Remove("Width");
						this.ClearBit(256);
						return;
					}
				}
				else
				{
					if (s.IsSet(8) && s.BackColor != Color.Empty)
					{
						this.BackColor = s.BackColor;
					}
					if (s.IsSet(4) && s.ForeColor != Color.Empty)
					{
						this.ForeColor = s.ForeColor;
					}
					if (s.IsSet(16) && s.BorderColor != Color.Empty)
					{
						this.BorderColor = s.BorderColor;
					}
					if (s.IsSet(32) && s.BorderWidth != Unit.Empty)
					{
						this.BorderWidth = s.BorderWidth;
					}
					if (s.IsSet(64))
					{
						this.BorderStyle = s.BorderStyle;
					}
					if (s.IsSet(128) && s.Height != Unit.Empty)
					{
						this.Height = s.Height;
					}
					if (s.IsSet(256) && s.Width != Unit.Empty)
					{
						this.Width = s.Width;
					}
				}
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00112A30 File Offset: 0x00111A30
		protected virtual void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
		{
			StateBag viewState = this.ViewState;
			if (this.IsSet(4))
			{
				Color color = (Color)viewState["ForeColor"];
				if (!color.IsEmpty)
				{
					attributes.Add(HtmlTextWriterStyle.Color, ColorTranslator.ToHtml(color));
				}
			}
			if (this.IsSet(8))
			{
				Color color = (Color)viewState["BackColor"];
				if (!color.IsEmpty)
				{
					attributes.Add(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(color));
				}
			}
			if (this.IsSet(16))
			{
				Color color = (Color)viewState["BorderColor"];
				if (!color.IsEmpty)
				{
					attributes.Add(HtmlTextWriterStyle.BorderColor, ColorTranslator.ToHtml(color));
				}
			}
			BorderStyle borderStyle = this.BorderStyle;
			Unit borderWidth = this.BorderWidth;
			if (!borderWidth.IsEmpty)
			{
				attributes.Add(HtmlTextWriterStyle.BorderWidth, borderWidth.ToString(CultureInfo.InvariantCulture));
				if (borderStyle == BorderStyle.NotSet)
				{
					if (borderWidth.Value != 0.0)
					{
						attributes.Add(HtmlTextWriterStyle.BorderStyle, "solid");
					}
				}
				else
				{
					attributes.Add(HtmlTextWriterStyle.BorderStyle, Style.borderStyles[(int)borderStyle]);
				}
			}
			else if (borderStyle != BorderStyle.NotSet)
			{
				attributes.Add(HtmlTextWriterStyle.BorderStyle, Style.borderStyles[(int)borderStyle]);
			}
			FontInfo font = this.Font;
			string[] names = font.Names;
			if (names.Length > 0)
			{
				attributes.Add(HtmlTextWriterStyle.FontFamily, Style.FormatStringArray(names, ','));
			}
			FontUnit size = font.Size;
			if (!size.IsEmpty)
			{
				attributes.Add(HtmlTextWriterStyle.FontSize, size.ToString(CultureInfo.InvariantCulture));
			}
			if (this.IsSet(2048))
			{
				if (font.Bold)
				{
					attributes.Add(HtmlTextWriterStyle.FontWeight, "bold");
				}
				else
				{
					attributes.Add(HtmlTextWriterStyle.FontWeight, "normal");
				}
			}
			if (this.IsSet(4096))
			{
				if (font.Italic)
				{
					attributes.Add(HtmlTextWriterStyle.FontStyle, "italic");
				}
				else
				{
					attributes.Add(HtmlTextWriterStyle.FontStyle, "normal");
				}
			}
			string text = string.Empty;
			if (font.Underline)
			{
				text = "underline";
			}
			if (font.Overline)
			{
				text += " overline";
			}
			if (font.Strikeout)
			{
				text += " line-through";
			}
			if (text.Length > 0)
			{
				attributes.Add(HtmlTextWriterStyle.TextDecoration, text);
			}
			else if (this.IsSet(8192) || this.IsSet(16384) || this.IsSet(32768))
			{
				attributes.Add(HtmlTextWriterStyle.TextDecoration, "none");
			}
			if (this.IsSet(128))
			{
				Unit unit = (Unit)viewState["Height"];
				if (!unit.IsEmpty)
				{
					attributes.Add(HtmlTextWriterStyle.Height, unit.ToString(CultureInfo.InvariantCulture));
				}
			}
			if (this.IsSet(256))
			{
				Unit unit = (Unit)viewState["Width"];
				if (!unit.IsEmpty)
				{
					attributes.Add(HtmlTextWriterStyle.Width, unit.ToString(CultureInfo.InvariantCulture));
				}
			}
		}

		// Token: 0x06004257 RID: 16983 RVA: 0x00112CFC File Offset: 0x00111CFC
		private static string FormatStringArray(string[] array, char delimiter)
		{
			int num = array.Length;
			if (num == 1)
			{
				return array[0];
			}
			if (num == 0)
			{
				return string.Empty;
			}
			return string.Join(delimiter.ToString(CultureInfo.InvariantCulture), array);
		}

		// Token: 0x06004258 RID: 16984 RVA: 0x00112D30 File Offset: 0x00111D30
		public CssStyleCollection GetStyleAttributes(IUrlResolutionService urlResolver)
		{
			CssStyleCollection cssStyleCollection = new CssStyleCollection();
			this.FillStyleAttributes(cssStyleCollection, urlResolver);
			return cssStyleCollection;
		}

		// Token: 0x06004259 RID: 16985 RVA: 0x00112D4C File Offset: 0x00111D4C
		internal bool IsSet(int propKey)
		{
			return (this.setBits & propKey) != 0;
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x00112D5C File Offset: 0x00111D5C
		protected internal void LoadViewState(object state)
		{
			if (state != null && this.ownStateBag)
			{
				this.ViewState.LoadViewState(state);
			}
			if (this.statebag != null)
			{
				object obj = this.ViewState["_!SB"];
				if (obj != null)
				{
					this.markedBits = (int)obj;
					this.setBits |= this.markedBits;
				}
			}
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x00112DBB File Offset: 0x00111DBB
		protected internal virtual void TrackViewState()
		{
			if (this.ownStateBag)
			{
				this.ViewState.TrackViewState();
			}
			this.marked = true;
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x00112DD8 File Offset: 0x00111DD8
		public virtual void MergeWith(Style s)
		{
			if (this.RegisteredCssClass.Length != 0)
			{
				throw new InvalidOperationException(SR.GetString("Style_RegisteredStylesAreReadOnly"));
			}
			if (s == null || s.IsEmpty)
			{
				return;
			}
			if (this.IsEmpty)
			{
				this.CopyFrom(s);
				return;
			}
			this.Font.MergeWith(s.Font);
			if (s.IsSet(2) && !this.IsSet(2))
			{
				this.CssClass = s.CssClass;
			}
			if (s.RegisteredCssClass.Length == 0)
			{
				if (s.IsSet(8) && (!this.IsSet(8) || this.BackColor == Color.Empty))
				{
					this.BackColor = s.BackColor;
				}
				if (s.IsSet(4) && (!this.IsSet(4) || this.ForeColor == Color.Empty))
				{
					this.ForeColor = s.ForeColor;
				}
				if (s.IsSet(16) && (!this.IsSet(16) || this.BorderColor == Color.Empty))
				{
					this.BorderColor = s.BorderColor;
				}
				if (s.IsSet(32) && (!this.IsSet(32) || this.BorderWidth == Unit.Empty))
				{
					this.BorderWidth = s.BorderWidth;
				}
				if (s.IsSet(64) && !this.IsSet(64))
				{
					this.BorderStyle = s.BorderStyle;
				}
				if (s.IsSet(128) && (!this.IsSet(128) || this.Height == Unit.Empty))
				{
					this.Height = s.Height;
				}
				if (s.IsSet(256) && (!this.IsSet(256) || this.Width == Unit.Empty))
				{
					this.Width = s.Width;
					return;
				}
			}
			else
			{
				if (this.IsSet(2))
				{
					this.CssClass = this.CssClass + " " + s.RegisteredCssClass;
					return;
				}
				this.CssClass = s.RegisteredCssClass;
			}
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x00112FE4 File Offset: 0x00111FE4
		public virtual void Reset()
		{
			if (this.statebag != null)
			{
				if (this.IsSet(2))
				{
					this.ViewState.Remove("CssClass");
				}
				if (this.IsSet(8))
				{
					this.ViewState.Remove("BackColor");
				}
				if (this.IsSet(4))
				{
					this.ViewState.Remove("ForeColor");
				}
				if (this.IsSet(16))
				{
					this.ViewState.Remove("BorderColor");
				}
				if (this.IsSet(32))
				{
					this.ViewState.Remove("BorderWidth");
				}
				if (this.IsSet(64))
				{
					this.ViewState.Remove("BorderStyle");
				}
				if (this.IsSet(128))
				{
					this.ViewState.Remove("Height");
				}
				if (this.IsSet(256))
				{
					this.ViewState.Remove("Width");
				}
				this.Font.Reset();
				this.ViewState.Remove("_!SB");
				this.markedBits = 0;
			}
			this.setBits = 0;
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x001130F8 File Offset: 0x001120F8
		protected internal virtual object SaveViewState()
		{
			if (this.statebag != null)
			{
				if (this.markedBits != 0)
				{
					this.ViewState["_!SB"] = this.markedBits;
				}
				if (this.ownStateBag)
				{
					return this.ViewState.SaveViewState();
				}
			}
			return null;
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x00113145 File Offset: 0x00112145
		protected internal virtual void SetBit(int bit)
		{
			this.setBits |= bit;
			if (this.IsTrackingViewState)
			{
				this.markedBits |= bit;
			}
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x0011316B File Offset: 0x0011216B
		public void SetDirty()
		{
			this.ViewState.SetDirty(true);
			this.markedBits = this.setBits;
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x00113185 File Offset: 0x00112185
		internal void SetRegisteredCssClass(string cssClass)
		{
			this.registeredCssClass = cssClass;
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06004262 RID: 16994 RVA: 0x0011318E File Offset: 0x0011218E
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.IsTrackingViewState;
			}
		}

		// Token: 0x06004263 RID: 16995 RVA: 0x00113196 File Offset: 0x00112196
		void IStateManager.LoadViewState(object state)
		{
			this.LoadViewState(state);
		}

		// Token: 0x06004264 RID: 16996 RVA: 0x0011319F File Offset: 0x0011219F
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x06004265 RID: 16997 RVA: 0x001131A7 File Offset: 0x001121A7
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x040028F8 RID: 10488
		internal const int UNUSED = 1;

		// Token: 0x040028F9 RID: 10489
		internal const int PROP_CSSCLASS = 2;

		// Token: 0x040028FA RID: 10490
		internal const int PROP_FORECOLOR = 4;

		// Token: 0x040028FB RID: 10491
		internal const int PROP_BACKCOLOR = 8;

		// Token: 0x040028FC RID: 10492
		internal const int PROP_BORDERCOLOR = 16;

		// Token: 0x040028FD RID: 10493
		internal const int PROP_BORDERWIDTH = 32;

		// Token: 0x040028FE RID: 10494
		internal const int PROP_BORDERSTYLE = 64;

		// Token: 0x040028FF RID: 10495
		internal const int PROP_HEIGHT = 128;

		// Token: 0x04002900 RID: 10496
		internal const int PROP_WIDTH = 256;

		// Token: 0x04002901 RID: 10497
		internal const int PROP_FONT_NAMES = 512;

		// Token: 0x04002902 RID: 10498
		internal const int PROP_FONT_SIZE = 1024;

		// Token: 0x04002903 RID: 10499
		internal const int PROP_FONT_BOLD = 2048;

		// Token: 0x04002904 RID: 10500
		internal const int PROP_FONT_ITALIC = 4096;

		// Token: 0x04002905 RID: 10501
		internal const int PROP_FONT_UNDERLINE = 8192;

		// Token: 0x04002906 RID: 10502
		internal const int PROP_FONT_OVERLINE = 16384;

		// Token: 0x04002907 RID: 10503
		internal const int PROP_FONT_STRIKEOUT = 32768;

		// Token: 0x04002908 RID: 10504
		internal const string SetBitsKey = "_!SB";

		// Token: 0x04002909 RID: 10505
		private StateBag statebag;

		// Token: 0x0400290A RID: 10506
		private FontInfo fontInfo;

		// Token: 0x0400290B RID: 10507
		private string registeredCssClass;

		// Token: 0x0400290C RID: 10508
		private bool ownStateBag;

		// Token: 0x0400290D RID: 10509
		private bool marked;

		// Token: 0x0400290E RID: 10510
		private int setBits;

		// Token: 0x0400290F RID: 10511
		private int markedBits;

		// Token: 0x04002910 RID: 10512
		internal static readonly string[] borderStyles = new string[] { "NotSet", "None", "Dotted", "Dashed", "Solid", "Double", "Groove", "Ridge", "Inset", "Outset" };
	}
}
