using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200073E RID: 1854
	[TypeConverter(typeof(EmptyStringExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebPartPersonalization
	{
		// Token: 0x060059F3 RID: 23027 RVA: 0x0016B4C2 File Offset: 0x0016A4C2
		public WebPartPersonalization(WebPartManager owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			this._enabled = true;
		}

		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x060059F4 RID: 23028 RVA: 0x0016B4E8 File Offset: 0x0016A4E8
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CanEnterSharedScope
		{
			get
			{
				IDictionary userCapabilities = this.UserCapabilities;
				return userCapabilities != null && userCapabilities.Contains(WebPartPersonalization.EnterSharedScopeUserCapability);
			}
		}

		// Token: 0x1700173B RID: 5947
		// (get) Token: 0x060059F5 RID: 23029 RVA: 0x0016B50F File Offset: 0x0016A50F
		// (set) Token: 0x060059F6 RID: 23030 RVA: 0x0016B518 File Offset: 0x0016A518
		[NotifyParentProperty(true)]
		[DefaultValue(true)]
		[WebSysDescription("WebPartPersonalization_Enabled")]
		public virtual bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (!this.WebPartManager.DesignMode && this._initializedSet && value != this.Enabled)
				{
					throw new InvalidOperationException(SR.GetString("WebPartPersonalization_MustSetBeforeInit", new object[] { "Enabled", "WebPartPersonalization" }));
				}
				this._enabled = value;
			}
		}

		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x060059F7 RID: 23031 RVA: 0x0016B574 File Offset: 0x0016A574
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool HasPersonalizationState
		{
			get
			{
				if (this._provider == null)
				{
					throw new InvalidOperationException(SR.GetString("WebPartPersonalization_CantUsePropertyBeforeInit", new object[] { "HasPersonalizationState", "WebPartPersonalization" }));
				}
				Page page = this.WebPartManager.Page;
				if (page == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page" }));
				}
				HttpRequest requestInternal = page.RequestInternal;
				if (requestInternal == null)
				{
					throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page.Request" }));
				}
				PersonalizationStateQuery personalizationStateQuery = new PersonalizationStateQuery();
				personalizationStateQuery.PathToMatch = requestInternal.AppRelativeCurrentExecutionFilePath;
				if (this.Scope == PersonalizationScope.User && requestInternal.IsAuthenticated)
				{
					personalizationStateQuery.UsernameToMatch = page.User.Identity.Name;
				}
				return this._provider.GetCountOfState(this.Scope, personalizationStateQuery) > 0;
			}
		}

		// Token: 0x1700173D RID: 5949
		// (get) Token: 0x060059F8 RID: 23032 RVA: 0x0016B65D File Offset: 0x0016A65D
		// (set) Token: 0x060059F9 RID: 23033 RVA: 0x0016B668 File Offset: 0x0016A668
		[NotifyParentProperty(true)]
		[WebSysDescription("WebPartPersonalization_InitialScope")]
		[DefaultValue(PersonalizationScope.User)]
		public virtual PersonalizationScope InitialScope
		{
			get
			{
				return this._initialScope;
			}
			set
			{
				if (value < PersonalizationScope.User || value > PersonalizationScope.Shared)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (!this.WebPartManager.DesignMode && this._initializedSet && value != this.InitialScope)
				{
					throw new InvalidOperationException(SR.GetString("WebPartPersonalization_MustSetBeforeInit", new object[] { "InitialScope", "WebPartPersonalization" }));
				}
				this._initialScope = value;
			}
		}

		// Token: 0x1700173E RID: 5950
		// (get) Token: 0x060059FA RID: 23034 RVA: 0x0016B6D5 File Offset: 0x0016A6D5
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsEnabled
		{
			get
			{
				return this.IsInitialized;
			}
		}

		// Token: 0x1700173F RID: 5951
		// (get) Token: 0x060059FB RID: 23035 RVA: 0x0016B6DD File Offset: 0x0016A6DD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		protected bool IsInitialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x17001740 RID: 5952
		// (get) Token: 0x060059FC RID: 23036 RVA: 0x0016B6E8 File Offset: 0x0016A6E8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsModifiable
		{
			get
			{
				IDictionary userCapabilities = this.UserCapabilities;
				return userCapabilities != null && userCapabilities.Contains(WebPartPersonalization.ModifyStateUserCapability);
			}
		}

		// Token: 0x17001741 RID: 5953
		// (get) Token: 0x060059FD RID: 23037 RVA: 0x0016B70F File Offset: 0x0016A70F
		// (set) Token: 0x060059FE RID: 23038 RVA: 0x0016B728 File Offset: 0x0016A728
		[DefaultValue("")]
		[WebSysDescription("WebPartPersonalization_ProviderName")]
		[NotifyParentProperty(true)]
		public virtual string ProviderName
		{
			get
			{
				if (this._providerName == null)
				{
					return string.Empty;
				}
				return this._providerName;
			}
			set
			{
				if (!this.WebPartManager.DesignMode && this._initializedSet && !string.Equals(value, this.ProviderName, StringComparison.Ordinal))
				{
					throw new InvalidOperationException(SR.GetString("WebPartPersonalization_MustSetBeforeInit", new object[] { "ProviderName", "WebPartPersonalization" }));
				}
				this._providerName = value;
			}
		}

		// Token: 0x17001742 RID: 5954
		// (get) Token: 0x060059FF RID: 23039 RVA: 0x0016B788 File Offset: 0x0016A788
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public PersonalizationScope Scope
		{
			get
			{
				return this._currentScope;
			}
		}

		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x06005A00 RID: 23040 RVA: 0x0016B790 File Offset: 0x0016A790
		internal bool ScopeToggled
		{
			get
			{
				return this._scopeToggled;
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x06005A01 RID: 23041 RVA: 0x0016B798 File Offset: 0x0016A798
		// (set) Token: 0x06005A02 RID: 23042 RVA: 0x0016B7A0 File Offset: 0x0016A7A0
		protected bool ShouldResetPersonalizationState
		{
			get
			{
				return this._shouldResetPersonalizationState;
			}
			set
			{
				this._shouldResetPersonalizationState = value;
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x06005A03 RID: 23043 RVA: 0x0016B7A9 File Offset: 0x0016A7A9
		protected virtual IDictionary UserCapabilities
		{
			get
			{
				if (this._userCapabilities == null)
				{
					this._userCapabilities = new HybridDictionary();
				}
				return this._userCapabilities;
			}
		}

		// Token: 0x17001746 RID: 5958
		// (get) Token: 0x06005A04 RID: 23044 RVA: 0x0016B7C4 File Offset: 0x0016A7C4
		protected WebPartManager WebPartManager
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x0016B7CC File Offset: 0x0016A7CC
		protected internal virtual void ApplyPersonalizationState()
		{
			if (this.IsEnabled)
			{
				this.EnsurePersonalizationState();
				this._personalizationState.ApplyWebPartManagerPersonalization();
			}
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x0016B7E7 File Offset: 0x0016A7E7
		protected internal virtual void ApplyPersonalizationState(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (this.IsEnabled)
			{
				this.EnsurePersonalizationState();
				this._personalizationState.ApplyWebPartPersonalization(webPart);
			}
		}

		// Token: 0x06005A07 RID: 23047 RVA: 0x0016B814 File Offset: 0x0016A814
		private void ApplyPersonalizationState(Control control, WebPartPersonalization.PersonalizationInfo info)
		{
			ITrackingPersonalizable trackingPersonalizable = control as ITrackingPersonalizable;
			IPersonalizable personalizable = control as IPersonalizable;
			if (trackingPersonalizable != null)
			{
				trackingPersonalizable.BeginLoad();
			}
			if (personalizable != null && info.CustomProperties != null && info.CustomProperties.Count > 0)
			{
				personalizable.Load(info.CustomProperties);
			}
			if (info.Properties != null && info.Properties.Count > 0)
			{
				BlobPersonalizationState.SetPersonalizedProperties(control, info.Properties);
			}
			if (trackingPersonalizable != null)
			{
				trackingPersonalizable.EndLoad();
			}
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x0016B889 File Offset: 0x0016A889
		protected virtual void ChangeScope(PersonalizationScope scope)
		{
			PersonalizationProviderHelper.CheckPersonalizationScope(scope);
			if (scope == this._currentScope)
			{
				return;
			}
			if (scope == PersonalizationScope.Shared && !this.CanEnterSharedScope)
			{
				throw new InvalidOperationException(SR.GetString("WebPartPersonalization_CannotEnterSharedScope"));
			}
			this._currentScope = scope;
			this._scopeToggled = true;
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x0016B8C8 File Offset: 0x0016A8C8
		protected internal virtual void CopyPersonalizationState(WebPart webPartA, WebPart webPartB)
		{
			if (webPartA == null)
			{
				throw new ArgumentNullException("webPartA");
			}
			if (webPartB == null)
			{
				throw new ArgumentNullException("webPartB");
			}
			if (webPartA.GetType() != webPartB.GetType())
			{
				throw new ArgumentException(SR.GetString("WebPartPersonalization_SameType", new object[] { "webPartA", "webPartB" }));
			}
			this.CopyPersonalizationState(webPartA, webPartB);
			GenericWebPart genericWebPart = webPartA as GenericWebPart;
			GenericWebPart genericWebPart2 = webPartB as GenericWebPart;
			if (genericWebPart != null && genericWebPart2 != null)
			{
				Control childControl = genericWebPart.ChildControl;
				Control childControl2 = genericWebPart2.ChildControl;
				if (childControl == null)
				{
					throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "ChildControl" }), "webPartA");
				}
				if (childControl2 == null)
				{
					throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "ChildControl" }), "webPartB");
				}
				if (childControl.GetType() != childControl2.GetType())
				{
					throw new ArgumentException(SR.GetString("WebPartPersonalization_SameType", new object[] { "webPartA.ChildControl", "webPartB.ChildControl" }));
				}
				this.CopyPersonalizationState(childControl, childControl2);
			}
			this.SetDirty(webPartB);
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x0016B9FC File Offset: 0x0016A9FC
		private void CopyPersonalizationState(Control controlA, Control controlB)
		{
			WebPartPersonalization.PersonalizationInfo personalizationInfo = this.ExtractPersonalizationState(controlA);
			this.ApplyPersonalizationState(controlB, personalizationInfo);
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x0016BA1C File Offset: 0x0016AA1C
		private void DeterminePersonalizationProvider()
		{
			string providerName = this.ProviderName;
			if (string.IsNullOrEmpty(providerName))
			{
				this._provider = PersonalizationAdministration.Provider;
				return;
			}
			PersonalizationProvider personalizationProvider = PersonalizationAdministration.Providers[providerName];
			if (personalizationProvider != null)
			{
				this._provider = personalizationProvider;
				return;
			}
			throw new ProviderException(SR.GetString("WebPartPersonalization_ProviderNotFound", new object[] { providerName }));
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x0016BA78 File Offset: 0x0016AA78
		public void EnsureEnabled(bool ensureModifiable)
		{
			if (!(ensureModifiable ? this.IsModifiable : this.IsEnabled))
			{
				string text;
				if (ensureModifiable)
				{
					text = SR.GetString("WebPartPersonalization_PersonalizationNotModifiable");
				}
				else
				{
					text = SR.GetString("WebPartPersonalization_PersonalizationNotEnabled");
				}
				throw new InvalidOperationException(text);
			}
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x0016BABC File Offset: 0x0016AABC
		private void EnsurePersonalizationState()
		{
			if (this._personalizationState == null)
			{
				throw new InvalidOperationException(SR.GetString("WebPartPersonalization_PersonalizationStateNotLoaded"));
			}
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x0016BAD6 File Offset: 0x0016AAD6
		protected internal virtual void ExtractPersonalizationState()
		{
			if (this.IsEnabled && !this.ShouldResetPersonalizationState)
			{
				this.EnsurePersonalizationState();
				this._personalizationState.ExtractWebPartManagerPersonalization();
			}
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x0016BAF9 File Offset: 0x0016AAF9
		protected internal virtual void ExtractPersonalizationState(WebPart webPart)
		{
			if (this.IsEnabled && !this.ShouldResetPersonalizationState)
			{
				this.EnsurePersonalizationState();
				this._personalizationState.ExtractWebPartPersonalization(webPart);
			}
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x0016BB20 File Offset: 0x0016AB20
		private WebPartPersonalization.PersonalizationInfo ExtractPersonalizationState(Control control)
		{
			ITrackingPersonalizable trackingPersonalizable = control as ITrackingPersonalizable;
			IPersonalizable personalizable = control as IPersonalizable;
			if (trackingPersonalizable != null)
			{
				trackingPersonalizable.BeginSave();
			}
			WebPartPersonalization.PersonalizationInfo personalizationInfo = new WebPartPersonalization.PersonalizationInfo();
			if (personalizable != null)
			{
				personalizationInfo.CustomProperties = new PersonalizationDictionary();
				personalizable.Save(personalizationInfo.CustomProperties);
			}
			personalizationInfo.Properties = BlobPersonalizationState.GetPersonalizedProperties(control, PersonalizationScope.Shared);
			if (trackingPersonalizable != null)
			{
				trackingPersonalizable.EndSave();
			}
			return personalizationInfo;
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x0016BB7B File Offset: 0x0016AB7B
		protected internal virtual string GetAuthorizationFilter(string webPartID)
		{
			if (string.IsNullOrEmpty(webPartID))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("webPartID");
			}
			this.EnsureEnabled(false);
			this.EnsurePersonalizationState();
			return this._personalizationState.GetAuthorizationFilter(webPartID);
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x0016BBA9 File Offset: 0x0016ABA9
		internal void LoadInternal()
		{
			if (this.Enabled)
			{
				this._currentScope = this.Load();
				this._initialized = true;
			}
			this._initializedSet = true;
		}

		// Token: 0x06005A13 RID: 23059 RVA: 0x0016BBD0 File Offset: 0x0016ABD0
		protected virtual PersonalizationScope Load()
		{
			if (!this.Enabled)
			{
				throw new InvalidOperationException(SR.GetString("WebPartPersonalization_PersonalizationNotEnabled"));
			}
			this.DeterminePersonalizationProvider();
			Page page = this.WebPartManager.Page;
			if (page == null)
			{
				throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page" }));
			}
			HttpRequest requestInternal = page.RequestInternal;
			if (requestInternal == null)
			{
				throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page.Request" }));
			}
			if (requestInternal.IsAuthenticated)
			{
				this._userCapabilities = this._provider.DetermineUserCapabilities(this.WebPartManager);
			}
			this._personalizationState = this._provider.LoadPersonalizationState(this.WebPartManager, false);
			if (this._personalizationState == null)
			{
				throw new ProviderException(SR.GetString("WebPartPersonalization_CannotLoadPersonalization"));
			}
			return this._provider.DetermineInitialScope(this.WebPartManager, this._personalizationState);
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x0016BCBC File Offset: 0x0016ACBC
		public virtual void ResetPersonalizationState()
		{
			this.EnsureEnabled(true);
			if (this._provider == null)
			{
				throw new InvalidOperationException(SR.GetString("WebPartPersonalization_CantCallMethodBeforeInit", new object[] { "ResetPersonalizationState", "WebPartPersonalization" }));
			}
			this._provider.ResetPersonalizationState(this.WebPartManager);
			this.ShouldResetPersonalizationState = true;
			Page page = this.WebPartManager.Page;
			if (page == null)
			{
				throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page" }));
			}
			this.TransferToCurrentPage(page);
		}

		// Token: 0x06005A15 RID: 23061 RVA: 0x0016BD4E File Offset: 0x0016AD4E
		internal void SaveInternal()
		{
			if (this.IsModifiable)
			{
				this.Save();
			}
		}

		// Token: 0x06005A16 RID: 23062 RVA: 0x0016BD60 File Offset: 0x0016AD60
		protected virtual void Save()
		{
			this.EnsureEnabled(true);
			this.EnsurePersonalizationState();
			if (this._provider == null)
			{
				throw new InvalidOperationException(SR.GetString("WebPartPersonalization_CantCallMethodBeforeInit", new object[] { "Save", "WebPartPersonalization" }));
			}
			if (this._personalizationState.IsDirty && !this.ShouldResetPersonalizationState)
			{
				this._provider.SavePersonalizationState(this._personalizationState);
			}
		}

		// Token: 0x06005A17 RID: 23063 RVA: 0x0016BDD0 File Offset: 0x0016ADD0
		protected internal virtual void SetDirty()
		{
			if (this.IsEnabled)
			{
				this.EnsurePersonalizationState();
				this._personalizationState.SetWebPartManagerDirty();
			}
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x0016BDEB File Offset: 0x0016ADEB
		protected internal virtual void SetDirty(WebPart webPart)
		{
			if (this.IsEnabled)
			{
				this.EnsurePersonalizationState();
				this._personalizationState.SetWebPartDirty(webPart);
			}
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x0016BE08 File Offset: 0x0016AE08
		public virtual void ToggleScope()
		{
			this.EnsureEnabled(false);
			Page page = this.WebPartManager.Page;
			if (page == null)
			{
				throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page" }));
			}
			if (page.IsExportingWebPart)
			{
				return;
			}
			Page previousPage = page.PreviousPage;
			if (previousPage != null && !previousPage.IsCrossPagePostBack)
			{
				WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(previousPage);
				if (currentWebPartManager != null && currentWebPartManager.Personalization.ScopeToggled)
				{
					return;
				}
			}
			if (this._currentScope == PersonalizationScope.Shared)
			{
				this.ChangeScope(PersonalizationScope.User);
			}
			else
			{
				this.ChangeScope(PersonalizationScope.Shared);
			}
			this.TransferToCurrentPage(page);
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x0016BEA0 File Offset: 0x0016AEA0
		private void TransferToCurrentPage(Page page)
		{
			HttpRequest requestInternal = page.RequestInternal;
			if (requestInternal == null)
			{
				throw new InvalidOperationException(SR.GetString("PropertyCannotBeNull", new object[] { "WebPartManager.Page.Request" }));
			}
			string text = requestInternal.CurrentExecutionFilePath;
			if (page.Form == null || string.Equals(page.Form.Method, "post", StringComparison.OrdinalIgnoreCase))
			{
				string clientQueryString = page.ClientQueryString;
				if (!string.IsNullOrEmpty(clientQueryString))
				{
					text = text + "?" + clientQueryString;
				}
			}
			IScriptManager scriptManager = page.ScriptManager;
			if (scriptManager != null && scriptManager.IsInAsyncPostBack)
			{
				requestInternal.Response.Redirect(text);
				return;
			}
			page.Server.Transfer(text, false);
		}

		// Token: 0x0400306B RID: 12395
		public static readonly WebPartUserCapability ModifyStateUserCapability = new WebPartUserCapability("modifyState");

		// Token: 0x0400306C RID: 12396
		public static readonly WebPartUserCapability EnterSharedScopeUserCapability = new WebPartUserCapability("enterSharedScope");

		// Token: 0x0400306D RID: 12397
		private WebPartManager _owner;

		// Token: 0x0400306E RID: 12398
		private bool _enabled;

		// Token: 0x0400306F RID: 12399
		private string _providerName;

		// Token: 0x04003070 RID: 12400
		private PersonalizationScope _initialScope;

		// Token: 0x04003071 RID: 12401
		private bool _initialized;

		// Token: 0x04003072 RID: 12402
		private bool _initializedSet;

		// Token: 0x04003073 RID: 12403
		private PersonalizationProvider _provider;

		// Token: 0x04003074 RID: 12404
		private PersonalizationScope _currentScope;

		// Token: 0x04003075 RID: 12405
		private IDictionary _userCapabilities;

		// Token: 0x04003076 RID: 12406
		private PersonalizationState _personalizationState;

		// Token: 0x04003077 RID: 12407
		private bool _scopeToggled;

		// Token: 0x04003078 RID: 12408
		private bool _shouldResetPersonalizationState;

		// Token: 0x0200073F RID: 1855
		private sealed class PersonalizationInfo
		{
			// Token: 0x04003079 RID: 12409
			public IDictionary Properties;

			// Token: 0x0400307A RID: 12410
			public PersonalizationDictionary CustomProperties;
		}
	}
}
