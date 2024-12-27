using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006AB RID: 1707
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ToolZone : WebZone, IPostBackEventHandler
	{
		// Token: 0x0600537D RID: 21373 RVA: 0x00152D1C File Offset: 0x00151D1C
		protected ToolZone(ICollection associatedDisplayModes)
		{
			if (associatedDisplayModes == null || associatedDisplayModes.Count == 0)
			{
				throw new ArgumentNullException("associatedDisplayModes");
			}
			this._associatedDisplayModes = new WebPartDisplayModeCollection();
			foreach (object obj in associatedDisplayModes)
			{
				WebPartDisplayMode webPartDisplayMode = (WebPartDisplayMode)obj;
				this._associatedDisplayModes.Add(webPartDisplayMode);
			}
			this._associatedDisplayModes.SetReadOnly("ToolZone_DisplayModesReadOnly");
		}

		// Token: 0x0600537E RID: 21374 RVA: 0x00152DB0 File Offset: 0x00151DB0
		protected ToolZone(WebPartDisplayMode associatedDisplayMode)
		{
			if (associatedDisplayMode == null)
			{
				throw new ArgumentNullException("associatedDisplayMode");
			}
			this._associatedDisplayModes = new WebPartDisplayModeCollection();
			this._associatedDisplayModes.Add(associatedDisplayMode);
			this._associatedDisplayModes.SetReadOnly("ToolZone_DisplayModesReadOnly");
		}

		// Token: 0x17001542 RID: 5442
		// (get) Token: 0x0600537F RID: 21375 RVA: 0x00152DEE File Offset: 0x00151DEE
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public WebPartDisplayModeCollection AssociatedDisplayModes
		{
			get
			{
				return this._associatedDisplayModes;
			}
		}

		// Token: 0x17001543 RID: 5443
		// (get) Token: 0x06005380 RID: 21376 RVA: 0x00152DF8 File Offset: 0x00151DF8
		protected virtual bool Display
		{
			get
			{
				if (base.WebPartManager != null)
				{
					WebPartDisplayModeCollection associatedDisplayModes = this.AssociatedDisplayModes;
					if (associatedDisplayModes != null)
					{
						return associatedDisplayModes.Contains(base.WebPartManager.DisplayMode);
					}
				}
				return false;
			}
		}

		// Token: 0x17001544 RID: 5444
		// (get) Token: 0x06005381 RID: 21377 RVA: 0x00152E2A File Offset: 0x00151E2A
		[WebSysDescription("ToolZone_EditUIStyle")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public Style EditUIStyle
		{
			get
			{
				if (this._editUIStyle == null)
				{
					this._editUIStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._editUIStyle).TrackViewState();
					}
				}
				return this._editUIStyle;
			}
		}

		// Token: 0x17001545 RID: 5445
		// (get) Token: 0x06005382 RID: 21378 RVA: 0x00152E58 File Offset: 0x00151E58
		[DefaultValue(null)]
		[WebSysDescription("ToolZone_HeaderCloseVerb")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		public virtual WebPartVerb HeaderCloseVerb
		{
			get
			{
				if (this._headerCloseVerb == null)
				{
					this._headerCloseVerb = new WebPartHeaderCloseVerb();
					this._headerCloseVerb.EventArgument = "headerClose";
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._headerCloseVerb).TrackViewState();
					}
				}
				return this._headerCloseVerb;
			}
		}

		// Token: 0x17001546 RID: 5446
		// (get) Token: 0x06005383 RID: 21379 RVA: 0x00152E96 File Offset: 0x00151E96
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("ToolZone_HeaderVerbStyle")]
		public Style HeaderVerbStyle
		{
			get
			{
				if (this._headerVerbStyle == null)
				{
					this._headerVerbStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._headerVerbStyle).TrackViewState();
					}
				}
				return this._headerVerbStyle;
			}
		}

		// Token: 0x17001547 RID: 5447
		// (get) Token: 0x06005384 RID: 21380 RVA: 0x00152EC4 File Offset: 0x00151EC4
		// (set) Token: 0x06005385 RID: 21381 RVA: 0x00152EF1 File Offset: 0x00151EF1
		[WebSysDefaultValue("")]
		[Localizable(true)]
		[WebCategory("Behavior")]
		[WebSysDescription("ToolZone_InstructionText")]
		public virtual string InstructionText
		{
			get
			{
				string text = (string)this.ViewState["InstructionText"];
				if (text != null)
				{
					return text;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["InstructionText"] = value;
			}
		}

		// Token: 0x17001548 RID: 5448
		// (get) Token: 0x06005386 RID: 21382 RVA: 0x00152F04 File Offset: 0x00151F04
		[DefaultValue(null)]
		[WebSysDescription("ToolZone_InstructionTextStyle")]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Styles")]
		public Style InstructionTextStyle
		{
			get
			{
				if (this._instructionTextStyle == null)
				{
					this._instructionTextStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._instructionTextStyle).TrackViewState();
					}
				}
				return this._instructionTextStyle;
			}
		}

		// Token: 0x17001549 RID: 5449
		// (get) Token: 0x06005387 RID: 21383 RVA: 0x00152F32 File Offset: 0x00151F32
		[WebSysDescription("ToolZone_LabelStyle")]
		[WebCategory("Styles")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public Style LabelStyle
		{
			get
			{
				if (this._labelStyle == null)
				{
					this._labelStyle = new Style();
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._labelStyle).TrackViewState();
					}
				}
				return this._labelStyle;
			}
		}

		// Token: 0x1700154A RID: 5450
		// (get) Token: 0x06005388 RID: 21384 RVA: 0x00152F60 File Offset: 0x00151F60
		// (set) Token: 0x06005389 RID: 21385 RVA: 0x00152F72 File Offset: 0x00151F72
		[Browsable(false)]
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool Visible
		{
			get
			{
				return this.Display && base.Visible;
			}
			set
			{
				if (!base.DesignMode)
				{
					throw new InvalidOperationException(SR.GetString("ToolZone_CantSetVisible"));
				}
			}
		}

		// Token: 0x0600538A RID: 21386
		protected abstract void Close();

		// Token: 0x0600538B RID: 21387 RVA: 0x00152F8C File Offset: 0x00151F8C
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 7)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.EditUIStyle).LoadViewState(array[1]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.HeaderCloseVerb).LoadViewState(array[3]);
			}
			if (array[4] != null)
			{
				((IStateManager)this.HeaderVerbStyle).LoadViewState(array[4]);
			}
			if (array[5] != null)
			{
				((IStateManager)this.InstructionTextStyle).LoadViewState(array[5]);
			}
			if (array[6] != null)
			{
				((IStateManager)this.LabelStyle).LoadViewState(array[6]);
			}
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x00153029 File Offset: 0x00152029
		protected virtual void OnDisplayModeChanged(object sender, WebPartDisplayModeEventArgs e)
		{
		}

		// Token: 0x0600538D RID: 21389 RVA: 0x0015302C File Offset: 0x0015202C
		protected internal override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			WebPartManager webPartManager = base.WebPartManager;
			if (webPartManager != null)
			{
				webPartManager.DisplayModeChanged += this.OnDisplayModeChanged;
				webPartManager.SelectedWebPartChanged += this.OnSelectedWebPartChanged;
			}
		}

		// Token: 0x0600538E RID: 21390 RVA: 0x00153070 File Offset: 0x00152070
		protected virtual void OnSelectedWebPartChanged(object sender, WebPartEventArgs e)
		{
		}

		// Token: 0x0600538F RID: 21391 RVA: 0x00153072 File Offset: 0x00152072
		protected virtual void RaisePostBackEvent(string eventArgument)
		{
			base.ValidateEvent(this.UniqueID, eventArgument);
			if (string.Equals(eventArgument, "headerClose", StringComparison.OrdinalIgnoreCase) && this.HeaderCloseVerb.Visible && this.HeaderCloseVerb.Enabled)
			{
				this.Close();
			}
		}

		// Token: 0x06005390 RID: 21392 RVA: 0x001530AF File Offset: 0x001520AF
		protected override void RenderFooter(HtmlTextWriter writer)
		{
			writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "4px");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			this.RenderVerbs(writer);
			writer.RenderEndTag();
		}

		// Token: 0x06005391 RID: 21393 RVA: 0x001530D4 File Offset: 0x001520D4
		protected override void RenderHeader(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "2");
			writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			TitleStyle headerStyle = base.HeaderStyle;
			if (!headerStyle.IsEmpty)
			{
				Style style = new Style();
				if (!headerStyle.ForeColor.IsEmpty)
				{
					style.ForeColor = headerStyle.ForeColor;
				}
				style.Font.CopyFrom(headerStyle.Font);
				if (!headerStyle.Font.Size.IsEmpty)
				{
					style.Font.Size = new FontUnit(new Unit(100.0, UnitType.Percentage));
				}
				if (!style.IsEmpty)
				{
					style.AddAttributesToRender(writer, this);
				}
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Table);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);
			HorizontalAlign horizontalAlign = headerStyle.HorizontalAlign;
			if (horizontalAlign != HorizontalAlign.NotSet)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(HorizontalAlign));
				writer.AddAttribute(HtmlTextWriterAttribute.Align, converter.ConvertToString(horizontalAlign));
			}
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);
			writer.Write(this.HeaderText);
			writer.RenderEndTag();
			WebPartVerb headerCloseVerb = this.HeaderCloseVerb;
			if (headerCloseVerb.Visible)
			{
				writer.AddStyleAttribute(HtmlTextWriterStyle.WhiteSpace, "nowrap");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				ZoneLinkButton zoneLinkButton = new ZoneLinkButton(this, headerCloseVerb.EventArgument);
				zoneLinkButton.Text = headerCloseVerb.Text;
				zoneLinkButton.ImageUrl = headerCloseVerb.ImageUrl;
				zoneLinkButton.ToolTip = headerCloseVerb.Description;
				zoneLinkButton.Enabled = headerCloseVerb.Enabled;
				zoneLinkButton.Page = this.Page;
				zoneLinkButton.ApplyStyle(this.HeaderVerbStyle);
				zoneLinkButton.RenderControl(writer);
				writer.RenderEndTag();
			}
			writer.RenderEndTag();
			writer.RenderEndTag();
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x001532AF File Offset: 0x001522AF
		protected virtual void RenderVerbs(HtmlTextWriter writer)
		{
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x001532B4 File Offset: 0x001522B4
		internal void RenderVerbsInternal(HtmlTextWriter writer, ICollection verbs)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in verbs)
			{
				WebPartVerb webPartVerb = (WebPartVerb)obj;
				if (webPartVerb.Visible)
				{
					arrayList.Add(webPartVerb);
				}
			}
			if (arrayList.Count > 0)
			{
				bool flag = true;
				foreach (object obj2 in arrayList)
				{
					WebPartVerb webPartVerb2 = (WebPartVerb)obj2;
					if (!flag)
					{
						writer.Write("&nbsp;");
					}
					this.RenderVerb(writer, webPartVerb2);
					flag = false;
				}
			}
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x00153384 File Offset: 0x00152384
		protected virtual void RenderVerb(HtmlTextWriter writer, WebPartVerb verb)
		{
			string eventArgument = verb.EventArgument;
			WebControl webControl;
			if (this.VerbButtonType == ButtonType.Button)
			{
				webControl = new ZoneButton(this, eventArgument)
				{
					Text = verb.Text
				};
			}
			else
			{
				ZoneLinkButton zoneLinkButton = new ZoneLinkButton(this, eventArgument);
				zoneLinkButton.Text = verb.Text;
				if (this.VerbButtonType == ButtonType.Image)
				{
					zoneLinkButton.ImageUrl = verb.ImageUrl;
				}
				webControl = zoneLinkButton;
			}
			webControl.ApplyStyle(base.VerbStyle);
			webControl.ToolTip = verb.Description;
			webControl.Enabled = verb.Enabled;
			webControl.Page = this.Page;
			webControl.RenderControl(writer);
		}

		// Token: 0x06005395 RID: 21397 RVA: 0x0015341C File Offset: 0x0015241C
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._editUIStyle != null) ? ((IStateManager)this._editUIStyle).SaveViewState() : null,
				null,
				(this._headerCloseVerb != null) ? ((IStateManager)this._headerCloseVerb).SaveViewState() : null,
				(this._headerVerbStyle != null) ? ((IStateManager)this._headerVerbStyle).SaveViewState() : null,
				(this._instructionTextStyle != null) ? ((IStateManager)this._instructionTextStyle).SaveViewState() : null,
				(this._labelStyle != null) ? ((IStateManager)this._labelStyle).SaveViewState() : null
			};
			for (int i = 0; i < 7; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x001534CC File Offset: 0x001524CC
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._editUIStyle != null)
			{
				((IStateManager)this._editUIStyle).TrackViewState();
			}
			if (this._headerCloseVerb != null)
			{
				((IStateManager)this._headerCloseVerb).TrackViewState();
			}
			if (this._headerVerbStyle != null)
			{
				((IStateManager)this._headerVerbStyle).TrackViewState();
			}
			if (this._instructionTextStyle != null)
			{
				((IStateManager)this._instructionTextStyle).TrackViewState();
			}
			if (this._labelStyle != null)
			{
				((IStateManager)this._labelStyle).TrackViewState();
			}
		}

		// Token: 0x06005397 RID: 21399 RVA: 0x0015353E File Offset: 0x0015253E
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
		{
			this.RaisePostBackEvent(eventArgument);
		}

		// Token: 0x04002E6F RID: 11887
		private const string headerCloseEventArgument = "headerClose";

		// Token: 0x04002E70 RID: 11888
		private const int baseIndex = 0;

		// Token: 0x04002E71 RID: 11889
		private const int editUIStyleIndex = 1;

		// Token: 0x04002E72 RID: 11890
		private const int headerCloseVerbIndex = 3;

		// Token: 0x04002E73 RID: 11891
		private const int headerVerbStyleIndex = 4;

		// Token: 0x04002E74 RID: 11892
		private const int instructionTextStyleIndex = 5;

		// Token: 0x04002E75 RID: 11893
		private const int labelStyleIndex = 6;

		// Token: 0x04002E76 RID: 11894
		private const int viewStateArrayLength = 7;

		// Token: 0x04002E77 RID: 11895
		private Style _editUIStyle;

		// Token: 0x04002E78 RID: 11896
		private WebPartVerb _headerCloseVerb;

		// Token: 0x04002E79 RID: 11897
		private Style _headerVerbStyle;

		// Token: 0x04002E7A RID: 11898
		private Style _instructionTextStyle;

		// Token: 0x04002E7B RID: 11899
		private Style _labelStyle;

		// Token: 0x04002E7C RID: 11900
		private WebPartDisplayModeCollection _associatedDisplayModes;
	}
}
