using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006BD RID: 1725
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class EditorZoneBase : ToolZone
	{
		// Token: 0x060054BA RID: 21690 RVA: 0x001589B8 File Offset: 0x001579B8
		protected EditorZoneBase()
			: base(WebPartManager.EditDisplayMode)
		{
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x060054BB RID: 21691 RVA: 0x001589C5 File Offset: 0x001579C5
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[WebCategory("Verbs")]
		[WebSysDescription("EditorZoneBase_ApplyVerb")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual WebPartVerb ApplyVerb
		{
			get
			{
				if (this._applyVerb == null)
				{
					this._applyVerb = new WebPartEditorApplyVerb();
					this._applyVerb.EventArgument = "apply";
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._applyVerb).TrackViewState();
					}
				}
				return this._applyVerb;
			}
		}

		// Token: 0x170015B0 RID: 5552
		// (get) Token: 0x060054BC RID: 21692 RVA: 0x00158A03 File Offset: 0x00157A03
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[WebSysDescription("EditorZoneBase_CancelVerb")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebCategory("Verbs")]
		[DefaultValue(null)]
		public virtual WebPartVerb CancelVerb
		{
			get
			{
				if (this._cancelVerb == null)
				{
					this._cancelVerb = new WebPartEditorCancelVerb();
					this._cancelVerb.EventArgument = "cancel";
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._cancelVerb).TrackViewState();
					}
				}
				return this._cancelVerb;
			}
		}

		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x060054BD RID: 21693 RVA: 0x00158A41 File Offset: 0x00157A41
		protected override bool Display
		{
			get
			{
				return base.Display && this.WebPartToEdit != null;
			}
		}

		// Token: 0x170015B2 RID: 5554
		// (get) Token: 0x060054BE RID: 21694 RVA: 0x00158A59 File Offset: 0x00157A59
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public EditorPartChrome EditorPartChrome
		{
			get
			{
				if (this._editorPartChrome == null)
				{
					this._editorPartChrome = this.CreateEditorPartChrome();
				}
				return this._editorPartChrome;
			}
		}

		// Token: 0x170015B3 RID: 5555
		// (get) Token: 0x060054BF RID: 21695 RVA: 0x00158A78 File Offset: 0x00157A78
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public EditorPartCollection EditorParts
		{
			get
			{
				if (this._editorParts == null)
				{
					WebPart webPartToEdit = this.WebPartToEdit;
					EditorPartCollection editorPartCollection = null;
					if (webPartToEdit != null && webPartToEdit != null)
					{
						editorPartCollection = ((IWebEditable)webPartToEdit).CreateEditorParts();
					}
					EditorPartCollection editorPartCollection2 = new EditorPartCollection(editorPartCollection, this.CreateEditorParts());
					if (!base.DesignMode)
					{
						foreach (object obj in editorPartCollection2)
						{
							EditorPart editorPart = (EditorPart)obj;
							if (string.IsNullOrEmpty(editorPart.ID))
							{
								throw new InvalidOperationException(SR.GetString("EditorZoneBase_NoEditorPartID"));
							}
						}
					}
					this._editorParts = editorPartCollection2;
					this.EnsureChildControls();
				}
				return this._editorParts;
			}
		}

		// Token: 0x170015B4 RID: 5556
		// (get) Token: 0x060054C0 RID: 21696 RVA: 0x00158B34 File Offset: 0x00157B34
		// (set) Token: 0x060054C1 RID: 21697 RVA: 0x00158B66 File Offset: 0x00157B66
		[WebSysDefaultValue("EditorZoneBase_DefaultEmptyZoneText")]
		public override string EmptyZoneText
		{
			get
			{
				string text = (string)this.ViewState["EmptyZoneText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("EditorZoneBase_DefaultEmptyZoneText");
			}
			set
			{
				this.ViewState["EmptyZoneText"] = value;
			}
		}

		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x060054C2 RID: 21698 RVA: 0x00158B7C File Offset: 0x00157B7C
		// (set) Token: 0x060054C3 RID: 21699 RVA: 0x00158BAE File Offset: 0x00157BAE
		[WebSysDescription("EditorZoneBase_ErrorText")]
		[WebSysDefaultValue("EditorZoneBase_DefaultErrorText")]
		[WebCategory("Behavior")]
		[Localizable(true)]
		public virtual string ErrorText
		{
			get
			{
				string text = (string)this.ViewState["ErrorText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("EditorZoneBase_DefaultErrorText");
			}
			set
			{
				this.ViewState["ErrorText"] = value;
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x060054C4 RID: 21700 RVA: 0x00158BC4 File Offset: 0x00157BC4
		// (set) Token: 0x060054C5 RID: 21701 RVA: 0x00158BF6 File Offset: 0x00157BF6
		[WebSysDefaultValue("EditorZoneBase_DefaultHeaderText")]
		public override string HeaderText
		{
			get
			{
				string text = (string)this.ViewState["HeaderText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("EditorZoneBase_DefaultHeaderText");
			}
			set
			{
				this.ViewState["HeaderText"] = value;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x060054C6 RID: 21702 RVA: 0x00158C0C File Offset: 0x00157C0C
		// (set) Token: 0x060054C7 RID: 21703 RVA: 0x00158C3E File Offset: 0x00157C3E
		[WebSysDefaultValue("EditorZoneBase_DefaultInstructionText")]
		public override string InstructionText
		{
			get
			{
				string text = (string)this.ViewState["InstructionText"];
				if (text != null)
				{
					return text;
				}
				return SR.GetString("EditorZoneBase_DefaultInstructionText");
			}
			set
			{
				this.ViewState["InstructionText"] = value;
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x060054C8 RID: 21704 RVA: 0x00158C51 File Offset: 0x00157C51
		[WebCategory("Verbs")]
		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NotifyParentProperty(true)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[WebSysDescription("EditorZoneBase_OKVerb")]
		public virtual WebPartVerb OKVerb
		{
			get
			{
				if (this._okVerb == null)
				{
					this._okVerb = new WebPartEditorOKVerb();
					this._okVerb.EventArgument = "ok";
					if (base.IsTrackingViewState)
					{
						((IStateManager)this._okVerb).TrackViewState();
					}
				}
				return this._okVerb;
			}
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x060054C9 RID: 21705 RVA: 0x00158C8F File Offset: 0x00157C8F
		protected WebPart WebPartToEdit
		{
			get
			{
				if (base.WebPartManager != null && base.WebPartManager.DisplayMode == WebPartManager.EditDisplayMode)
				{
					return base.WebPartManager.SelectedWebPart;
				}
				return null;
			}
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x00158CB8 File Offset: 0x00157CB8
		private void ApplyAndSyncChanges()
		{
			WebPart webPartToEdit = this.WebPartToEdit;
			if (webPartToEdit != null)
			{
				EditorPartCollection editorParts = this.EditorParts;
				foreach (object obj in editorParts)
				{
					EditorPart editorPart = (EditorPart)obj;
					if (editorPart.Display && editorPart.Visible && editorPart.ChromeState == PartChromeState.Normal && !editorPart.ApplyChanges())
					{
						this._applyError = true;
					}
				}
				if (!this._applyError)
				{
					foreach (object obj2 in editorParts)
					{
						EditorPart editorPart2 = (EditorPart)obj2;
						editorPart2.SyncChanges();
					}
				}
			}
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x00158D98 File Offset: 0x00157D98
		protected override void Close()
		{
			if (base.WebPartManager != null)
			{
				base.WebPartManager.EndWebPartEditing();
			}
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x00158DB0 File Offset: 0x00157DB0
		protected internal override void CreateChildControls()
		{
			ControlCollection controls = this.Controls;
			controls.Clear();
			WebPart webPartToEdit = this.WebPartToEdit;
			foreach (object obj in this.EditorParts)
			{
				EditorPart editorPart = (EditorPart)obj;
				if (webPartToEdit != null)
				{
					editorPart.SetWebPartToEdit(webPartToEdit);
					editorPart.SetWebPartManager(base.WebPartManager);
				}
				editorPart.SetZone(this);
				controls.Add(editorPart);
			}
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x00158E40 File Offset: 0x00157E40
		protected virtual EditorPartChrome CreateEditorPartChrome()
		{
			return new EditorPartChrome(this);
		}

		// Token: 0x060054CE RID: 21710
		protected abstract EditorPartCollection CreateEditorParts();

		// Token: 0x060054CF RID: 21711 RVA: 0x00158E48 File Offset: 0x00157E48
		protected void InvalidateEditorParts()
		{
			this._editorParts = null;
			base.ChildControlsCreated = false;
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x00158E58 File Offset: 0x00157E58
		protected override void LoadViewState(object savedState)
		{
			if (savedState == null)
			{
				base.LoadViewState(null);
				return;
			}
			object[] array = (object[])savedState;
			if (array.Length != 4)
			{
				throw new ArgumentException(SR.GetString("ViewState_InvalidViewState"));
			}
			base.LoadViewState(array[0]);
			if (array[1] != null)
			{
				((IStateManager)this.ApplyVerb).LoadViewState(array[1]);
			}
			if (array[2] != null)
			{
				((IStateManager)this.CancelVerb).LoadViewState(array[2]);
			}
			if (array[3] != null)
			{
				((IStateManager)this.OKVerb).LoadViewState(array[3]);
			}
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x00158ECF File Offset: 0x00157ECF
		protected override void OnDisplayModeChanged(object sender, WebPartDisplayModeEventArgs e)
		{
			this.InvalidateEditorParts();
			base.OnDisplayModeChanged(sender, e);
		}

		// Token: 0x060054D2 RID: 21714 RVA: 0x00158EDF File Offset: 0x00157EDF
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			this.EditorPartChrome.PerformPreRender();
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x00158EF4 File Offset: 0x00157EF4
		protected override void OnSelectedWebPartChanged(object sender, WebPartEventArgs e)
		{
			if (base.WebPartManager != null && base.WebPartManager.DisplayMode == WebPartManager.EditDisplayMode)
			{
				this.InvalidateEditorParts();
				if (e.WebPart != null)
				{
					foreach (object obj in this.EditorParts)
					{
						EditorPart editorPart = (EditorPart)obj;
						editorPart.SyncChanges();
					}
				}
			}
			base.OnSelectedWebPartChanged(sender, e);
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x00158F7C File Offset: 0x00157F7C
		protected override void RaisePostBackEvent(string eventArgument)
		{
			if (string.Equals(eventArgument, "apply", StringComparison.OrdinalIgnoreCase))
			{
				if (this.ApplyVerb.Visible && this.ApplyVerb.Enabled && this.WebPartToEdit != null)
				{
					this.ApplyAndSyncChanges();
					return;
				}
			}
			else if (string.Equals(eventArgument, "cancel", StringComparison.OrdinalIgnoreCase))
			{
				if (this.CancelVerb.Visible && this.CancelVerb.Enabled && this.WebPartToEdit != null)
				{
					this.Close();
					return;
				}
			}
			else if (string.Equals(eventArgument, "ok", StringComparison.OrdinalIgnoreCase))
			{
				if (this.OKVerb.Visible && this.OKVerb.Enabled && this.WebPartToEdit != null)
				{
					this.ApplyAndSyncChanges();
					if (!this._applyError)
					{
						this.Close();
						return;
					}
				}
			}
			else
			{
				base.RaisePostBackEvent(eventArgument);
			}
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x0015904C File Offset: 0x0015804C
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			base.Render(writer);
		}

		// Token: 0x060054D6 RID: 21718 RVA: 0x0015906C File Offset: 0x0015806C
		protected override void RenderBody(HtmlTextWriter writer)
		{
			base.RenderBodyTableBeginTag(writer);
			if (base.DesignMode)
			{
				base.RenderDesignerRegionBeginTag(writer, Orientation.Vertical);
			}
			if (this.HasControls())
			{
				bool flag = true;
				this.RenderInstructionText(writer, ref flag);
				if (this._applyError)
				{
					this.RenderErrorText(writer, ref flag);
				}
				EditorPartChrome editorPartChrome = this.EditorPartChrome;
				foreach (object obj in this.EditorParts)
				{
					EditorPart editorPart = (EditorPart)obj;
					if (editorPart.Display && editorPart.Visible)
					{
						writer.RenderBeginTag(HtmlTextWriterTag.Tr);
						if (!flag)
						{
							writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingTop, "0");
						}
						else
						{
							flag = false;
						}
						writer.RenderBeginTag(HtmlTextWriterTag.Td);
						editorPartChrome.RenderEditorPart(writer, editorPart);
						writer.RenderEndTag();
						writer.RenderEndTag();
					}
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
				writer.AddStyleAttribute(HtmlTextWriterStyle.Height, "100%");
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
			else
			{
				this.RenderEmptyZoneText(writer);
			}
			if (base.DesignMode)
			{
				WebZone.RenderDesignerRegionEndTag(writer);
			}
			WebZone.RenderBodyTableEndTag(writer);
		}

		// Token: 0x060054D7 RID: 21719 RVA: 0x001591A4 File Offset: 0x001581A4
		private void RenderEmptyZoneText(HtmlTextWriter writer)
		{
			string emptyZoneText = this.EmptyZoneText;
			if (!string.IsNullOrEmpty(emptyZoneText))
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
				Style emptyZoneTextStyle = base.EmptyZoneTextStyle;
				if (!emptyZoneTextStyle.IsEmpty)
				{
					emptyZoneTextStyle.AddAttributesToRender(writer, this);
				}
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				writer.Write(emptyZoneText);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
		}

		// Token: 0x060054D8 RID: 21720 RVA: 0x00159208 File Offset: 0x00158208
		private void RenderErrorText(HtmlTextWriter writer, ref bool firstCell)
		{
			string errorText = this.ErrorText;
			if (!string.IsNullOrEmpty(errorText))
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				firstCell = false;
				Label label = new Label();
				label.Text = errorText;
				label.Page = this.Page;
				label.ApplyStyle(base.ErrorStyle);
				label.RenderControl(writer);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
		}

		// Token: 0x060054D9 RID: 21721 RVA: 0x00159270 File Offset: 0x00158270
		private void RenderInstructionText(HtmlTextWriter writer, ref bool firstCell)
		{
			string instructionText = this.InstructionText;
			if (!string.IsNullOrEmpty(instructionText))
			{
				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
				writer.RenderBeginTag(HtmlTextWriterTag.Td);
				firstCell = false;
				Label label = new Label();
				label.Text = instructionText;
				label.Page = this.Page;
				label.ApplyStyle(base.InstructionTextStyle);
				label.RenderControl(writer);
				writer.RenderEndTag();
				writer.RenderEndTag();
			}
		}

		// Token: 0x060054DA RID: 21722 RVA: 0x001592D8 File Offset: 0x001582D8
		protected override void RenderVerbs(HtmlTextWriter writer)
		{
			base.RenderVerbsInternal(writer, new WebPartVerb[] { this.OKVerb, this.CancelVerb, this.ApplyVerb });
		}

		// Token: 0x060054DB RID: 21723 RVA: 0x00159310 File Offset: 0x00158310
		protected override object SaveViewState()
		{
			object[] array = new object[]
			{
				base.SaveViewState(),
				(this._applyVerb != null) ? ((IStateManager)this._applyVerb).SaveViewState() : null,
				(this._cancelVerb != null) ? ((IStateManager)this._cancelVerb).SaveViewState() : null,
				(this._okVerb != null) ? ((IStateManager)this._okVerb).SaveViewState() : null
			};
			for (int i = 0; i < 4; i++)
			{
				if (array[i] != null)
				{
					return array;
				}
			}
			return null;
		}

		// Token: 0x060054DC RID: 21724 RVA: 0x0015938C File Offset: 0x0015838C
		protected override void TrackViewState()
		{
			base.TrackViewState();
			if (this._applyVerb != null)
			{
				((IStateManager)this._applyVerb).TrackViewState();
			}
			if (this._cancelVerb != null)
			{
				((IStateManager)this._cancelVerb).TrackViewState();
			}
			if (this._okVerb != null)
			{
				((IStateManager)this._okVerb).TrackViewState();
			}
		}

		// Token: 0x04002EE9 RID: 12009
		private const int baseIndex = 0;

		// Token: 0x04002EEA RID: 12010
		private const int applyVerbIndex = 1;

		// Token: 0x04002EEB RID: 12011
		private const int cancelVerbIndex = 2;

		// Token: 0x04002EEC RID: 12012
		private const int okVerbIndex = 3;

		// Token: 0x04002EED RID: 12013
		private const int viewStateArrayLength = 4;

		// Token: 0x04002EEE RID: 12014
		private const string applyEventArgument = "apply";

		// Token: 0x04002EEF RID: 12015
		private const string cancelEventArgument = "cancel";

		// Token: 0x04002EF0 RID: 12016
		private const string okEventArgument = "ok";

		// Token: 0x04002EF1 RID: 12017
		private EditorPartCollection _editorParts;

		// Token: 0x04002EF2 RID: 12018
		private WebPartVerb _applyVerb;

		// Token: 0x04002EF3 RID: 12019
		private WebPartVerb _cancelVerb;

		// Token: 0x04002EF4 RID: 12020
		private WebPartVerb _okVerb;

		// Token: 0x04002EF5 RID: 12021
		private bool _applyError;

		// Token: 0x04002EF6 RID: 12022
		private EditorPartChrome _editorPartChrome;
	}
}
