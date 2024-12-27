using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006A1 RID: 1697
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BehaviorEditorPart : EditorPart
	{
		// Token: 0x1700151C RID: 5404
		// (get) Token: 0x060052F5 RID: 21237 RVA: 0x0014F804 File Offset: 0x0014E804
		// (set) Token: 0x060052F6 RID: 21238 RVA: 0x0014F80C File Offset: 0x0014E80C
		[Themeable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string DefaultButton
		{
			get
			{
				return base.DefaultButton;
			}
			set
			{
				base.DefaultButton = value;
			}
		}

		// Token: 0x1700151D RID: 5405
		// (get) Token: 0x060052F7 RID: 21239 RVA: 0x0014F815 File Offset: 0x0014E815
		public override bool Display
		{
			get
			{
				return (base.WebPartToEdit == null || !base.WebPartToEdit.IsShared || base.WebPartManager == null || base.WebPartManager.Personalization.Scope != PersonalizationScope.User) && base.Display;
			}
		}

		// Token: 0x1700151E RID: 5406
		// (get) Token: 0x060052F8 RID: 21240 RVA: 0x0014F850 File Offset: 0x0014E850
		private bool HasError
		{
			get
			{
				return this._allowCloseErrorMessage != null || this._allowConnectErrorMessage != null || this._allowHideErrorMessage != null || this._allowMinimizeErrorMessage != null || this._allowZoneChangeErrorMessage != null || this._exportModeErrorMessage != null || this._helpModeErrorMessage != null || this._descriptionErrorMessage != null || this._titleUrlErrorMessage != null || this._titleIconImageUrlErrorMessage != null || this._catalogIconImageUrlErrorMessage != null || this._helpUrlErrorMessage != null || this._importErrorMessageErrorMessage != null || this._authorizationFilterErrorMessage != null || this._allowEditErrorMessage != null;
			}
		}

		// Token: 0x1700151F RID: 5407
		// (get) Token: 0x060052F9 RID: 21241 RVA: 0x0014F8DC File Offset: 0x0014E8DC
		// (set) Token: 0x060052FA RID: 21242 RVA: 0x0014F90E File Offset: 0x0014E90E
		[WebSysDefaultValue("BehaviorEditorPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("BehaviorEditorPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x0014F924 File Offset: 0x0014E924
		public override bool ApplyChanges()
		{
			WebPart webPartToEdit = base.WebPartToEdit;
			if (webPartToEdit != null)
			{
				this.EnsureChildControls();
				bool allowLayoutChange = webPartToEdit.Zone.AllowLayoutChange;
				if (allowLayoutChange)
				{
					try
					{
						webPartToEdit.AllowClose = this._allowClose.Checked;
					}
					catch (Exception ex)
					{
						this._allowCloseErrorMessage = base.CreateErrorMessage(ex.Message);
					}
				}
				try
				{
					webPartToEdit.AllowConnect = this._allowConnect.Checked;
				}
				catch (Exception ex2)
				{
					this._allowConnectErrorMessage = base.CreateErrorMessage(ex2.Message);
				}
				if (allowLayoutChange)
				{
					try
					{
						webPartToEdit.AllowHide = this._allowHide.Checked;
					}
					catch (Exception ex3)
					{
						this._allowHideErrorMessage = base.CreateErrorMessage(ex3.Message);
					}
				}
				if (allowLayoutChange)
				{
					try
					{
						webPartToEdit.AllowMinimize = this._allowMinimize.Checked;
					}
					catch (Exception ex4)
					{
						this._allowMinimizeErrorMessage = base.CreateErrorMessage(ex4.Message);
					}
				}
				if (allowLayoutChange)
				{
					try
					{
						webPartToEdit.AllowZoneChange = this._allowZoneChange.Checked;
					}
					catch (Exception ex5)
					{
						this._allowZoneChangeErrorMessage = base.CreateErrorMessage(ex5.Message);
					}
				}
				try
				{
					TypeConverter converter = TypeDescriptor.GetConverter(typeof(WebPartExportMode));
					webPartToEdit.ExportMode = (WebPartExportMode)converter.ConvertFromString(this._exportMode.SelectedValue);
				}
				catch (Exception ex6)
				{
					this._exportModeErrorMessage = base.CreateErrorMessage(ex6.Message);
				}
				try
				{
					TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(WebPartHelpMode));
					webPartToEdit.HelpMode = (WebPartHelpMode)converter2.ConvertFromString(this._helpMode.SelectedValue);
				}
				catch (Exception ex7)
				{
					this._helpModeErrorMessage = base.CreateErrorMessage(ex7.Message);
				}
				try
				{
					webPartToEdit.Description = this._description.Text;
				}
				catch (Exception ex8)
				{
					this._descriptionErrorMessage = base.CreateErrorMessage(ex8.Message);
				}
				string text = this._titleUrl.Text;
				if (CrossSiteScriptingValidation.IsDangerousUrl(text))
				{
					this._titleUrlErrorMessage = SR.GetString("EditorPart_ErrorBadUrl");
				}
				else
				{
					try
					{
						webPartToEdit.TitleUrl = text;
					}
					catch (Exception ex9)
					{
						this._titleUrlErrorMessage = base.CreateErrorMessage(ex9.Message);
					}
				}
				text = this._titleIconImageUrl.Text;
				if (CrossSiteScriptingValidation.IsDangerousUrl(text))
				{
					this._titleIconImageUrlErrorMessage = SR.GetString("EditorPart_ErrorBadUrl");
				}
				else
				{
					try
					{
						webPartToEdit.TitleIconImageUrl = text;
					}
					catch (Exception ex10)
					{
						this._titleIconImageUrlErrorMessage = base.CreateErrorMessage(ex10.Message);
					}
				}
				text = this._catalogIconImageUrl.Text;
				if (CrossSiteScriptingValidation.IsDangerousUrl(text))
				{
					this._catalogIconImageUrlErrorMessage = SR.GetString("EditorPart_ErrorBadUrl");
				}
				else
				{
					try
					{
						webPartToEdit.CatalogIconImageUrl = text;
					}
					catch (Exception ex11)
					{
						this._catalogIconImageUrlErrorMessage = base.CreateErrorMessage(ex11.Message);
					}
				}
				text = this._helpUrl.Text;
				if (CrossSiteScriptingValidation.IsDangerousUrl(text))
				{
					this._helpUrlErrorMessage = SR.GetString("EditorPart_ErrorBadUrl");
				}
				else
				{
					try
					{
						webPartToEdit.HelpUrl = text;
					}
					catch (Exception ex12)
					{
						this._helpUrlErrorMessage = base.CreateErrorMessage(ex12.Message);
					}
				}
				try
				{
					webPartToEdit.ImportErrorMessage = this._importErrorMessage.Text;
				}
				catch (Exception ex13)
				{
					this._importErrorMessageErrorMessage = base.CreateErrorMessage(ex13.Message);
				}
				try
				{
					webPartToEdit.AuthorizationFilter = this._authorizationFilter.Text;
				}
				catch (Exception ex14)
				{
					this._authorizationFilterErrorMessage = base.CreateErrorMessage(ex14.Message);
				}
				try
				{
					webPartToEdit.AllowEdit = this._allowEdit.Checked;
				}
				catch (Exception ex15)
				{
					this._allowEditErrorMessage = base.CreateErrorMessage(ex15.Message);
				}
			}
			return !this.HasError;
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x0014FD48 File Offset: 0x0014ED48
		protected internal override void CreateChildControls()
		{
			ControlCollection controls = this.Controls;
			controls.Clear();
			this._allowClose = new CheckBox();
			controls.Add(this._allowClose);
			this._allowConnect = new CheckBox();
			controls.Add(this._allowConnect);
			this._allowHide = new CheckBox();
			controls.Add(this._allowHide);
			this._allowMinimize = new CheckBox();
			controls.Add(this._allowMinimize);
			this._allowZoneChange = new CheckBox();
			controls.Add(this._allowZoneChange);
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(WebPartExportMode));
			this._exportMode = new DropDownList();
			this._exportMode.Items.AddRange(new ListItem[]
			{
				new ListItem(SR.GetString("BehaviorEditorPart_ExportModeNone"), converter.ConvertToString(WebPartExportMode.None)),
				new ListItem(SR.GetString("BehaviorEditorPart_ExportModeAll"), converter.ConvertToString(WebPartExportMode.All)),
				new ListItem(SR.GetString("BehaviorEditorPart_ExportModeNonSensitiveData"), converter.ConvertToString(WebPartExportMode.NonSensitiveData))
			});
			controls.Add(this._exportMode);
			TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(WebPartHelpMode));
			this._helpMode = new DropDownList();
			this._helpMode.Items.AddRange(new ListItem[]
			{
				new ListItem(SR.GetString("BehaviorEditorPart_HelpModeModal"), converter2.ConvertToString(WebPartHelpMode.Modal)),
				new ListItem(SR.GetString("BehaviorEditorPart_HelpModeModeless"), converter2.ConvertToString(WebPartHelpMode.Modeless)),
				new ListItem(SR.GetString("BehaviorEditorPart_HelpModeNavigate"), converter2.ConvertToString(WebPartHelpMode.Navigate))
			});
			controls.Add(this._helpMode);
			this._description = new TextBox();
			this._description.Columns = 30;
			controls.Add(this._description);
			this._titleUrl = new TextBox();
			this._titleUrl.Columns = 30;
			controls.Add(this._titleUrl);
			this._titleIconImageUrl = new TextBox();
			this._titleIconImageUrl.Columns = 30;
			controls.Add(this._titleIconImageUrl);
			this._catalogIconImageUrl = new TextBox();
			this._catalogIconImageUrl.Columns = 30;
			controls.Add(this._catalogIconImageUrl);
			this._helpUrl = new TextBox();
			this._helpUrl.Columns = 30;
			controls.Add(this._helpUrl);
			this._importErrorMessage = new TextBox();
			this._importErrorMessage.Columns = 30;
			controls.Add(this._importErrorMessage);
			this._authorizationFilter = new TextBox();
			this._authorizationFilter.Columns = 30;
			controls.Add(this._authorizationFilter);
			this._allowEdit = new CheckBox();
			controls.Add(this._allowEdit);
			foreach (object obj in controls)
			{
				Control control = (Control)obj;
				control.EnableViewState = false;
			}
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x00150074 File Offset: 0x0014F074
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Display && this.Visible && !this.HasError)
			{
				this.SyncChanges();
			}
		}

		// Token: 0x060052FE RID: 21246 RVA: 0x0015009C File Offset: 0x0014F09C
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this.EnsureChildControls();
			string[] array = new string[]
			{
				SR.GetString("BehaviorEditorPart_Description"),
				SR.GetString("BehaviorEditorPart_TitleLink"),
				SR.GetString("BehaviorEditorPart_TitleIconImageLink"),
				SR.GetString("BehaviorEditorPart_CatalogIconImageLink"),
				SR.GetString("BehaviorEditorPart_HelpLink"),
				SR.GetString("BehaviorEditorPart_HelpMode"),
				SR.GetString("BehaviorEditorPart_ImportErrorMessage"),
				SR.GetString("BehaviorEditorPart_ExportMode"),
				SR.GetString("BehaviorEditorPart_AuthorizationFilter"),
				SR.GetString("BehaviorEditorPart_AllowClose"),
				SR.GetString("BehaviorEditorPart_AllowConnect"),
				SR.GetString("BehaviorEditorPart_AllowEdit"),
				SR.GetString("BehaviorEditorPart_AllowHide"),
				SR.GetString("BehaviorEditorPart_AllowMinimize"),
				SR.GetString("BehaviorEditorPart_AllowZoneChange")
			};
			WebControl[] array2 = new WebControl[]
			{
				this._description, this._titleUrl, this._titleIconImageUrl, this._catalogIconImageUrl, this._helpUrl, this._helpMode, this._importErrorMessage, this._exportMode, this._authorizationFilter, this._allowClose,
				this._allowConnect, this._allowEdit, this._allowHide, this._allowMinimize, this._allowZoneChange
			};
			string[] array3 = new string[]
			{
				this._descriptionErrorMessage, this._titleUrlErrorMessage, this._titleIconImageUrlErrorMessage, this._catalogIconImageUrlErrorMessage, this._helpUrlErrorMessage, this._helpModeErrorMessage, this._importErrorMessageErrorMessage, this._exportModeErrorMessage, this._authorizationFilterErrorMessage, this._allowCloseErrorMessage,
				this._allowConnectErrorMessage, this._allowEditErrorMessage, this._allowHideErrorMessage, this._allowMinimizeErrorMessage, this._allowZoneChangeErrorMessage
			};
			base.RenderPropertyEditors(writer, array, null, array2, array3);
		}

		// Token: 0x060052FF RID: 21247 RVA: 0x001502F4 File Offset: 0x0014F2F4
		public override void SyncChanges()
		{
			WebPart webPartToEdit = base.WebPartToEdit;
			if (webPartToEdit != null)
			{
				bool allowLayoutChange = webPartToEdit.Zone.AllowLayoutChange;
				this.EnsureChildControls();
				this._allowClose.Checked = webPartToEdit.AllowClose;
				this._allowClose.Enabled = allowLayoutChange;
				this._allowConnect.Checked = webPartToEdit.AllowConnect;
				this._allowHide.Checked = webPartToEdit.AllowHide;
				this._allowHide.Enabled = allowLayoutChange;
				this._allowMinimize.Checked = webPartToEdit.AllowMinimize;
				this._allowMinimize.Enabled = allowLayoutChange;
				this._allowZoneChange.Checked = webPartToEdit.AllowZoneChange;
				this._allowZoneChange.Enabled = allowLayoutChange;
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(WebPartExportMode));
				this._exportMode.SelectedValue = converter.ConvertToString(webPartToEdit.ExportMode);
				TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(WebPartHelpMode));
				this._helpMode.SelectedValue = converter2.ConvertToString(webPartToEdit.HelpMode);
				this._description.Text = webPartToEdit.Description;
				this._titleUrl.Text = webPartToEdit.TitleUrl;
				this._titleIconImageUrl.Text = webPartToEdit.TitleIconImageUrl;
				this._catalogIconImageUrl.Text = webPartToEdit.CatalogIconImageUrl;
				this._helpUrl.Text = webPartToEdit.HelpUrl;
				this._importErrorMessage.Text = webPartToEdit.ImportErrorMessage;
				this._authorizationFilter.Text = webPartToEdit.AuthorizationFilter;
				this._allowEdit.Checked = webPartToEdit.AllowEdit;
			}
		}

		// Token: 0x04002E1C RID: 11804
		private const int TextBoxColumns = 30;

		// Token: 0x04002E1D RID: 11805
		private CheckBox _allowClose;

		// Token: 0x04002E1E RID: 11806
		private CheckBox _allowConnect;

		// Token: 0x04002E1F RID: 11807
		private CheckBox _allowHide;

		// Token: 0x04002E20 RID: 11808
		private CheckBox _allowMinimize;

		// Token: 0x04002E21 RID: 11809
		private CheckBox _allowZoneChange;

		// Token: 0x04002E22 RID: 11810
		private DropDownList _exportMode;

		// Token: 0x04002E23 RID: 11811
		private DropDownList _helpMode;

		// Token: 0x04002E24 RID: 11812
		private TextBox _description;

		// Token: 0x04002E25 RID: 11813
		private TextBox _titleUrl;

		// Token: 0x04002E26 RID: 11814
		private TextBox _titleIconImageUrl;

		// Token: 0x04002E27 RID: 11815
		private TextBox _catalogIconImageUrl;

		// Token: 0x04002E28 RID: 11816
		private TextBox _helpUrl;

		// Token: 0x04002E29 RID: 11817
		private TextBox _importErrorMessage;

		// Token: 0x04002E2A RID: 11818
		private TextBox _authorizationFilter;

		// Token: 0x04002E2B RID: 11819
		private CheckBox _allowEdit;

		// Token: 0x04002E2C RID: 11820
		private string _allowCloseErrorMessage;

		// Token: 0x04002E2D RID: 11821
		private string _allowConnectErrorMessage;

		// Token: 0x04002E2E RID: 11822
		private string _allowHideErrorMessage;

		// Token: 0x04002E2F RID: 11823
		private string _allowMinimizeErrorMessage;

		// Token: 0x04002E30 RID: 11824
		private string _allowZoneChangeErrorMessage;

		// Token: 0x04002E31 RID: 11825
		private string _exportModeErrorMessage;

		// Token: 0x04002E32 RID: 11826
		private string _helpModeErrorMessage;

		// Token: 0x04002E33 RID: 11827
		private string _descriptionErrorMessage;

		// Token: 0x04002E34 RID: 11828
		private string _titleUrlErrorMessage;

		// Token: 0x04002E35 RID: 11829
		private string _titleIconImageUrlErrorMessage;

		// Token: 0x04002E36 RID: 11830
		private string _catalogIconImageUrlErrorMessage;

		// Token: 0x04002E37 RID: 11831
		private string _helpUrlErrorMessage;

		// Token: 0x04002E38 RID: 11832
		private string _importErrorMessageErrorMessage;

		// Token: 0x04002E39 RID: 11833
		private string _authorizationFilterErrorMessage;

		// Token: 0x04002E3A RID: 11834
		private string _allowEditErrorMessage;
	}
}
