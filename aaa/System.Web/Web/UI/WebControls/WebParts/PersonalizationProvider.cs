using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.Configuration;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006E0 RID: 1760
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class PersonalizationProvider : ProviderBase
	{
		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x06005669 RID: 22121
		// (set) Token: 0x0600566A RID: 22122
		public abstract string ApplicationName { get; set; }

		// Token: 0x0600566B RID: 22123 RVA: 0x0015CBA0 File Offset: 0x0015BBA0
		protected virtual IList CreateSupportedUserCapabilities()
		{
			return new ArrayList
			{
				WebPartPersonalization.EnterSharedScopeUserCapability,
				WebPartPersonalization.ModifyStateUserCapability
			};
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x0015CBCC File Offset: 0x0015BBCC
		public virtual PersonalizationScope DetermineInitialScope(WebPartManager webPartManager, PersonalizationState loadedState)
		{
			if (webPartManager == null)
			{
				throw new ArgumentNullException("webPartManager");
			}
			Page page = webPartManager.Page;
			if (page == null)
			{
				throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "Page" }), "webPartManager");
			}
			HttpRequest requestInternal = page.RequestInternal;
			if (requestInternal == null)
			{
				throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "Page.Request" }), "webPartManager");
			}
			PersonalizationScope personalizationScope = webPartManager.Personalization.InitialScope;
			IPrincipal principal = null;
			if (requestInternal.IsAuthenticated)
			{
				principal = page.User;
			}
			if (principal == null)
			{
				personalizationScope = PersonalizationScope.Shared;
			}
			else
			{
				if (page.IsPostBack)
				{
					string text = page.Request["__WPPS"];
					if (text == "s")
					{
						personalizationScope = PersonalizationScope.Shared;
					}
					else if (text == "u")
					{
						personalizationScope = PersonalizationScope.User;
					}
				}
				else if (page.PreviousPage != null && !page.PreviousPage.IsCrossPagePostBack)
				{
					WebPartManager currentWebPartManager = WebPartManager.GetCurrentWebPartManager(page.PreviousPage);
					if (currentWebPartManager != null)
					{
						personalizationScope = currentWebPartManager.Personalization.Scope;
					}
				}
				else if (page.IsExportingWebPart)
				{
					personalizationScope = (page.IsExportingWebPartShared ? PersonalizationScope.Shared : PersonalizationScope.User);
				}
				if (personalizationScope == PersonalizationScope.Shared && !webPartManager.Personalization.CanEnterSharedScope)
				{
					personalizationScope = PersonalizationScope.User;
				}
			}
			string text2 = ((personalizationScope == PersonalizationScope.Shared) ? "s" : "u");
			page.ClientScript.RegisterHiddenField("__WPPS", text2);
			return personalizationScope;
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x0015CD34 File Offset: 0x0015BD34
		public virtual IDictionary DetermineUserCapabilities(WebPartManager webPartManager)
		{
			if (webPartManager == null)
			{
				throw new ArgumentNullException("webPartManager");
			}
			Page page = webPartManager.Page;
			if (page == null)
			{
				throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "Page" }), "webPartManager");
			}
			HttpRequest requestInternal = page.RequestInternal;
			if (requestInternal == null)
			{
				throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "Page.Request" }), "webPartManager");
			}
			IPrincipal principal = null;
			if (requestInternal.IsAuthenticated)
			{
				principal = page.User;
			}
			if (principal != null)
			{
				if (this._supportedUserCapabilities == null)
				{
					this._supportedUserCapabilities = this.CreateSupportedUserCapabilities();
				}
				if (this._supportedUserCapabilities != null && this._supportedUserCapabilities.Count != 0)
				{
					WebPartsSection webParts = RuntimeConfig.GetConfig().WebParts;
					if (webParts != null)
					{
						WebPartsPersonalizationAuthorization authorization = webParts.Personalization.Authorization;
						if (authorization != null)
						{
							IDictionary dictionary = new HybridDictionary();
							foreach (object obj in this._supportedUserCapabilities)
							{
								WebPartUserCapability webPartUserCapability = (WebPartUserCapability)obj;
								if (authorization.IsUserAllowed(principal, webPartUserCapability.Name))
								{
									dictionary[webPartUserCapability] = webPartUserCapability;
								}
							}
							return dictionary;
						}
					}
				}
			}
			return new HybridDictionary();
		}

		// Token: 0x0600566E RID: 22126
		public abstract PersonalizationStateInfoCollection FindState(PersonalizationScope scope, PersonalizationStateQuery query, int pageIndex, int pageSize, out int totalRecords);

		// Token: 0x0600566F RID: 22127
		public abstract int GetCountOfState(PersonalizationScope scope, PersonalizationStateQuery query);

		// Token: 0x06005670 RID: 22128 RVA: 0x0015CE90 File Offset: 0x0015BE90
		private void GetParameters(WebPartManager webPartManager, out string path, out string userName)
		{
			if (webPartManager == null)
			{
				throw new ArgumentNullException("webPartManager");
			}
			Page page = webPartManager.Page;
			if (page == null)
			{
				throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "Page" }), "webPartManager");
			}
			HttpRequest requestInternal = page.RequestInternal;
			if (requestInternal == null)
			{
				throw new ArgumentException(SR.GetString("PropertyCannotBeNull", new object[] { "Page.Request" }), "webPartManager");
			}
			path = requestInternal.AppRelativeCurrentExecutionFilePath;
			userName = null;
			if (webPartManager.Personalization.Scope == PersonalizationScope.User && page.Request.IsAuthenticated)
			{
				userName = page.User.Identity.Name;
			}
		}

		// Token: 0x06005671 RID: 22129
		protected abstract void LoadPersonalizationBlobs(WebPartManager webPartManager, string path, string userName, ref byte[] sharedDataBlob, ref byte[] userDataBlob);

		// Token: 0x06005672 RID: 22130 RVA: 0x0015CF40 File Offset: 0x0015BF40
		public virtual PersonalizationState LoadPersonalizationState(WebPartManager webPartManager, bool ignoreCurrentUser)
		{
			if (webPartManager == null)
			{
				throw new ArgumentNullException("webPartManager");
			}
			string text;
			string text2;
			this.GetParameters(webPartManager, out text, out text2);
			if (ignoreCurrentUser)
			{
				text2 = null;
			}
			byte[] array = null;
			byte[] array2 = null;
			this.LoadPersonalizationBlobs(webPartManager, text, text2, ref array, ref array2);
			BlobPersonalizationState blobPersonalizationState = new BlobPersonalizationState(webPartManager);
			blobPersonalizationState.LoadDataBlobs(array, array2);
			return blobPersonalizationState;
		}

		// Token: 0x06005673 RID: 22131
		protected abstract void ResetPersonalizationBlob(WebPartManager webPartManager, string path, string userName);

		// Token: 0x06005674 RID: 22132 RVA: 0x0015CF90 File Offset: 0x0015BF90
		public virtual void ResetPersonalizationState(WebPartManager webPartManager)
		{
			if (webPartManager == null)
			{
				throw new ArgumentNullException("webPartManager");
			}
			string text;
			string text2;
			this.GetParameters(webPartManager, out text, out text2);
			this.ResetPersonalizationBlob(webPartManager, text, text2);
		}

		// Token: 0x06005675 RID: 22133
		public abstract int ResetState(PersonalizationScope scope, string[] paths, string[] usernames);

		// Token: 0x06005676 RID: 22134
		public abstract int ResetUserState(string path, DateTime userInactiveSinceDate);

		// Token: 0x06005677 RID: 22135
		protected abstract void SavePersonalizationBlob(WebPartManager webPartManager, string path, string userName, byte[] dataBlob);

		// Token: 0x06005678 RID: 22136 RVA: 0x0015CFC0 File Offset: 0x0015BFC0
		public virtual void SavePersonalizationState(PersonalizationState state)
		{
			if (state == null)
			{
				throw new ArgumentNullException("state");
			}
			BlobPersonalizationState blobPersonalizationState = state as BlobPersonalizationState;
			if (blobPersonalizationState == null)
			{
				throw new ArgumentException(SR.GetString("PersonalizationProvider_WrongType"), "state");
			}
			WebPartManager webPartManager = blobPersonalizationState.WebPartManager;
			string text;
			string text2;
			this.GetParameters(webPartManager, out text, out text2);
			byte[] array = null;
			bool flag = blobPersonalizationState.IsEmpty;
			if (!flag)
			{
				array = blobPersonalizationState.SaveDataBlob();
				flag = array == null || array.Length == 0;
			}
			if (flag)
			{
				this.ResetPersonalizationBlob(webPartManager, text, text2);
				return;
			}
			this.SavePersonalizationBlob(webPartManager, text, text2, array);
		}

		// Token: 0x04002F5A RID: 12122
		private const string scopeFieldName = "__WPPS";

		// Token: 0x04002F5B RID: 12123
		private const string sharedScopeFieldValue = "s";

		// Token: 0x04002F5C RID: 12124
		private const string userScopeFieldValue = "u";

		// Token: 0x04002F5D RID: 12125
		private ICollection _supportedUserCapabilities;
	}
}
