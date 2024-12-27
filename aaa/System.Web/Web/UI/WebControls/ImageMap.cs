using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005BF RID: 1471
	[SupportsEventValidation]
	[DefaultEvent("Click")]
	[DefaultProperty("HotSpots")]
	[ParseChildren(true, "HotSpots")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ImageMap : Image, IPostBackEventHandler
	{
		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x060047F2 RID: 18418 RVA: 0x00126314 File Offset: 0x00125314
		// (set) Token: 0x060047F3 RID: 18419 RVA: 0x0012631C File Offset: 0x0012531C
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public override bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
			}
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060047F4 RID: 18420 RVA: 0x00126325 File Offset: 0x00125325
		[WebCategory("Behavior")]
		[WebSysDescription("ImageMap_HotSpots")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[NotifyParentProperty(true)]
		public HotSpotCollection HotSpots
		{
			get
			{
				if (this._hotSpots == null)
				{
					this._hotSpots = new HotSpotCollection();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._hotSpots).TrackViewState();
					}
				}
				return this._hotSpots;
			}
		}

		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x060047F5 RID: 18421 RVA: 0x00126354 File Offset: 0x00125354
		// (set) Token: 0x060047F6 RID: 18422 RVA: 0x0012637D File Offset: 0x0012537D
		[WebSysDescription("HotSpot_HotSpotMode")]
		[WebCategory("Behavior")]
		[DefaultValue(HotSpotMode.NotSet)]
		public virtual HotSpotMode HotSpotMode
		{
			get
			{
				object obj = this.ViewState["HotSpotMode"];
				if (obj != null)
				{
					return (HotSpotMode)obj;
				}
				return HotSpotMode.NotSet;
			}
			set
			{
				if (value < HotSpotMode.NotSet || value > HotSpotMode.Inactive)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["HotSpotMode"] = value;
			}
		}

		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x060047F7 RID: 18423 RVA: 0x001263A8 File Offset: 0x001253A8
		// (set) Token: 0x060047F8 RID: 18424 RVA: 0x001263D5 File Offset: 0x001253D5
		[WebCategory("Behavior")]
		[WebSysDescription("HotSpot_Target")]
		[DefaultValue("")]
		public virtual string Target
		{
			get
			{
				object obj = this.ViewState["Target"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Target"] = value;
			}
		}

		// Token: 0x140000CD RID: 205
		// (add) Token: 0x060047F9 RID: 18425 RVA: 0x001263E8 File Offset: 0x001253E8
		// (remove) Token: 0x060047FA RID: 18426 RVA: 0x001263FB File Offset: 0x001253FB
		[WebSysDescription("ImageMap_Click")]
		[Category("Action")]
		public event ImageMapEventHandler Click
		{
			add
			{
				base.Events.AddHandler(ImageMap.EventClick, value);
			}
			remove
			{
				base.Events.RemoveHandler(ImageMap.EventClick, value);
			}
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0012640E File Offset: 0x0012540E
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			if (this._hasHotSpots)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Usemap, "#ImageMap" + this.ClientID, false);
			}
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x00126438 File Offset: 0x00125438
		protected override void LoadViewState(object savedState)
		{
			object obj = null;
			object[] array = null;
			if (savedState != null)
			{
				array = (object[])savedState;
				if (array.Length != 2)
				{
					throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
				}
				obj = array[0];
			}
			base.LoadViewState(obj);
			if (array != null && array[1] != null)
			{
				((IStateManager)this.HotSpots).LoadViewState(array[1]);
			}
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x0012648C File Offset: 0x0012548C
		protected virtual void OnClick(ImageMapEventArgs e)
		{
			ImageMapEventHandler imageMapEventHandler = (ImageMapEventHandler)base.Events[ImageMap.EventClick];
			if (imageMapEventHandler != null)
			{
				imageMapEventHandler(this, e);
			}
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x001264BC File Offset: 0x001254BC
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Enabled && !base.IsEnabled)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
			}
			this._hasHotSpots = this._hotSpots != null && this._hotSpots.Count > 0;
			base.Render(writer);
			if (this._hasHotSpots)
			{
				string text = "ImageMap" + this.ClientID;
				writer.AddAttribute(HtmlTextWriterAttribute.Name, text);
				writer.AddAttribute(HtmlTextWriterAttribute.Id, text);
				writer.RenderBeginTag(HtmlTextWriterTag.Map);
				HotSpotMode hotSpotMode = this.HotSpotMode;
				if (hotSpotMode == HotSpotMode.NotSet)
				{
					hotSpotMode = HotSpotMode.Navigate;
				}
				int num = 0;
				string target = this.Target;
				foreach (object obj in this._hotSpots)
				{
					HotSpot hotSpot = (HotSpot)obj;
					writer.AddAttribute(HtmlTextWriterAttribute.Shape, hotSpot.MarkupName, false);
					writer.AddAttribute(HtmlTextWriterAttribute.Coords, hotSpot.GetCoordinates());
					HotSpotMode hotSpotMode2 = hotSpot.HotSpotMode;
					if (hotSpotMode2 == HotSpotMode.NotSet)
					{
						hotSpotMode2 = hotSpotMode;
					}
					if (hotSpotMode2 == HotSpotMode.PostBack)
					{
						if (this.Page != null)
						{
							this.Page.VerifyRenderingInServerForm(this);
						}
						string text2 = num.ToString(CultureInfo.InvariantCulture);
						writer.AddAttribute(HtmlTextWriterAttribute.Href, this.Page.ClientScript.GetPostBackClientHyperlink(this, text2, true));
					}
					else if (hotSpotMode2 == HotSpotMode.Navigate)
					{
						string text3 = base.ResolveClientUrl(hotSpot.NavigateUrl);
						writer.AddAttribute(HtmlTextWriterAttribute.Href, text3);
						string text4 = hotSpot.Target;
						if (text4.Length == 0)
						{
							text4 = target;
						}
						if (text4.Length > 0)
						{
							writer.AddAttribute(HtmlTextWriterAttribute.Target, text4);
						}
					}
					else if (hotSpotMode2 == HotSpotMode.Inactive)
					{
						writer.AddAttribute("nohref", "true");
					}
					writer.AddAttribute(HtmlTextWriterAttribute.Title, hotSpot.AlternateText);
					writer.AddAttribute(HtmlTextWriterAttribute.Alt, hotSpot.AlternateText);
					string accessKey = hotSpot.AccessKey;
					if (accessKey.Length > 0)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Accesskey, accessKey);
					}
					int tabIndex = (int)hotSpot.TabIndex;
					if (tabIndex != 0)
					{
						writer.AddAttribute(HtmlTextWriterAttribute.Tabindex, tabIndex.ToString(NumberFormatInfo.InvariantInfo));
					}
					writer.RenderBeginTag(HtmlTextWriterTag.Area);
					writer.RenderEndTag();
					num++;
				}
				writer.RenderEndTag();
			}
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x001266F8 File Offset: 0x001256F8
		protected override object SaveViewState()
		{
			object obj = base.SaveViewState();
			object obj2 = null;
			if (this._hotSpots != null && this._hotSpots.Count > 0)
			{
				obj2 = ((IStateManager)this._hotSpots).SaveViewState();
			}
			if (obj != null || obj2 != null)
			{
				return new object[] { obj, obj2 };
			}
			return null;
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x00126748 File Offset: 0x00125748
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._hotSpots != null)
			{
				((IStateManager)this._hotSpots).TrackViewState();
			}
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x00126763 File Offset: 0x00125763
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x0012676C File Offset: 0x0012576C
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			string text = null;
			if (eventArgument != null && this._hotSpots != null)
			{
				int num = int.Parse(eventArgument, CultureInfo.InvariantCulture);
				if (num >= 0 && num < this._hotSpots.Count)
				{
					HotSpot hotSpot = this._hotSpots[num];
					HotSpotMode hotSpotMode = hotSpot.HotSpotMode;
					if (hotSpotMode == HotSpotMode.NotSet)
					{
						hotSpotMode = this.HotSpotMode;
					}
					if (hotSpotMode == HotSpotMode.PostBack)
					{
						text = hotSpot.PostBackValue;
					}
				}
			}
			if (text != null)
			{
				this.OnClick(new ImageMapEventArgs(text));
			}
		}

		// Token: 0x04002ABF RID: 10943
		private static readonly object EventClick = new object();

		// Token: 0x04002AC0 RID: 10944
		private bool _hasHotSpots;

		// Token: 0x04002AC1 RID: 10945
		private HotSpotCollection _hotSpots;
	}
}
