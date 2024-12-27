using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Web.Handlers;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003AB RID: 939
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ClientScriptManager
	{
		// Token: 0x06002DE3 RID: 11747 RVA: 0x000CD954 File Offset: 0x000CC954
		internal ClientScriptManager(Page owner)
		{
			this._owner = owner;
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06002DE4 RID: 11748 RVA: 0x000CD963 File Offset: 0x000CC963
		internal bool HasRegisteredHiddenFields
		{
			get
			{
				return this._registeredHiddenFields != null && this._registeredHiddenFields.Count > 0;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x000CD97D File Offset: 0x000CC97D
		internal bool HasSubmitStatements
		{
			get
			{
				return this._registeredOnSubmitStatements != null && this._registeredOnSubmitStatements.Count > 0;
			}
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000CD997 File Offset: 0x000CC997
		private static int ComputeHashKey(string uniqueId, string argument)
		{
			if (string.IsNullOrEmpty(argument))
			{
				return StringUtil.GetStringHashCode(uniqueId);
			}
			return StringUtil.GetStringHashCode(uniqueId) ^ StringUtil.GetStringHashCode(argument);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000CD9B8 File Offset: 0x000CC9B8
		internal string GetEventValidationFieldValue()
		{
			if (this._validEventReferences == null || this._validEventReferences.Count == 0)
			{
				return string.Empty;
			}
			IStateFormatter stateFormatter = this._owner.CreateStateFormatter();
			return stateFormatter.Serialize(this._validEventReferences);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000CD9F8 File Offset: 0x000CC9F8
		public void RegisterForEventValidation(PostBackOptions options)
		{
			this.RegisterForEventValidation(options.TargetControl.UniqueID, options.Argument);
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000CDA11 File Offset: 0x000CCA11
		public void RegisterForEventValidation(string uniqueId)
		{
			this.RegisterForEventValidation(uniqueId, string.Empty);
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000CDA20 File Offset: 0x000CCA20
		public void RegisterForEventValidation(string uniqueId, string argument)
		{
			if (!this._owner.EnableEventValidation || this._owner.DesignMode)
			{
				return;
			}
			if (string.IsNullOrEmpty(uniqueId))
			{
				return;
			}
			if (this._owner.ControlState < ControlState.PreRendered && !this._owner.IsCallback)
			{
				throw new InvalidOperationException(SR.GetString("ClientScriptManager_RegisterForEventValidation_Too_Early"));
			}
			int num = ClientScriptManager.ComputeHashKey(uniqueId, argument);
			string text = this._owner.ClientState;
			if (text == null)
			{
				text = string.Empty;
			}
			if (this._validEventReferences == null)
			{
				if (this._owner.IsCallback)
				{
					this.EnsureEventValidationFieldLoaded();
					if (this._validEventReferences == null)
					{
						this._validEventReferences = new ArrayList();
					}
				}
				else
				{
					this._validEventReferences = new ArrayList();
					this._validEventReferences.Add(StringUtil.GetStringHashCode(text));
				}
			}
			this._validEventReferences.Add(num);
			if (this._owner.PartialCachingControlStack != null)
			{
				foreach (object obj in this._owner.PartialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterForEventValidation(uniqueId, argument);
				}
			}
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000CDB60 File Offset: 0x000CCB60
		internal void SaveEventValidationField()
		{
			string eventValidationFieldValue = this.GetEventValidationFieldValue();
			if (!string.IsNullOrEmpty(eventValidationFieldValue))
			{
				this.RegisterHiddenField("__EVENTVALIDATION", eventValidationFieldValue);
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000CDB88 File Offset: 0x000CCB88
		private void EnsureEventValidationFieldLoaded()
		{
			if (this._eventValidationFieldLoaded)
			{
				return;
			}
			this._eventValidationFieldLoaded = true;
			string text = null;
			if (this._owner.RequestValueCollection != null)
			{
				text = this._owner.RequestValueCollection["__EVENTVALIDATION"];
			}
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			IStateFormatter stateFormatter = this._owner.CreateStateFormatter();
			ArrayList arrayList = null;
			try
			{
				arrayList = stateFormatter.Deserialize(text) as ArrayList;
			}
			catch (Exception ex)
			{
				if (!this._owner.ShouldSuppressMacValidationException(ex))
				{
					ViewStateException.ThrowViewStateError(ex, text);
				}
			}
			if (arrayList == null || arrayList.Count < 1)
			{
				return;
			}
			int num = (int)arrayList[0];
			string requestViewStateString = this._owner.RequestViewStateString;
			if (num != StringUtil.GetStringHashCode(requestViewStateString))
			{
				ViewStateException.ThrowViewStateError(null, text);
			}
			this._clientPostBackValidatedEventTable = new HybridDictionary(arrayList.Count - 1, true);
			for (int i = 1; i < arrayList.Count; i++)
			{
				int num2 = (int)arrayList[i];
				this._clientPostBackValidatedEventTable[num2] = null;
			}
			if (this._owner.IsCallback)
			{
				this._validEventReferences = arrayList;
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000CDCB4 File Offset: 0x000CCCB4
		public void ValidateEvent(string uniqueId)
		{
			this.ValidateEvent(uniqueId, string.Empty);
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000CDCC4 File Offset: 0x000CCCC4
		public void ValidateEvent(string uniqueId, string argument)
		{
			if (!this._owner.EnableEventValidation)
			{
				return;
			}
			if (string.IsNullOrEmpty(uniqueId))
			{
				throw new ArgumentException(SR.GetString("Parameter_NullOrEmpty", new object[] { "uniqueId" }), "uniqueId");
			}
			this.EnsureEventValidationFieldLoaded();
			if (this._clientPostBackValidatedEventTable == null)
			{
				throw new ArgumentException(SR.GetString("ClientScriptManager_InvalidPostBackArgument"));
			}
			int num = ClientScriptManager.ComputeHashKey(uniqueId, argument);
			if (!this._clientPostBackValidatedEventTable.Contains(num))
			{
				throw new ArgumentException(SR.GetString("ClientScriptManager_InvalidPostBackArgument"));
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000CDD55 File Offset: 0x000CCD55
		internal void ClearHiddenFields()
		{
			this._registeredHiddenFields = null;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000CDD5E File Offset: 0x000CCD5E
		internal static ScriptKey CreateScriptKey(Type type, string key)
		{
			return new ScriptKey(type, key);
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000CDD67 File Offset: 0x000CCD67
		internal static ScriptKey CreateScriptIncludeKey(Type type, string key)
		{
			return new ScriptKey(type, key, true);
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000CDD71 File Offset: 0x000CCD71
		public string GetCallbackEventReference(Control control, string argument, string clientCallback, string context)
		{
			return this.GetCallbackEventReference(control, argument, clientCallback, context, false);
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000CDD7F File Offset: 0x000CCD7F
		public string GetCallbackEventReference(Control control, string argument, string clientCallback, string context, bool useAsync)
		{
			return this.GetCallbackEventReference(control, argument, clientCallback, context, null, useAsync);
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000CDD90 File Offset: 0x000CCD90
		public string GetCallbackEventReference(Control control, string argument, string clientCallback, string context, string clientErrorCallback, bool useAsync)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (!(control is ICallbackEventHandler))
			{
				throw new InvalidOperationException(SR.GetString("Page_CallBackTargetInvalid", new object[] { control.UniqueID }));
			}
			return this.GetCallbackEventReference("'" + control.UniqueID + "'", argument, clientCallback, context, clientErrorCallback, useAsync);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000CDDF8 File Offset: 0x000CCDF8
		public string GetCallbackEventReference(string target, string argument, string clientCallback, string context, string clientErrorCallback, bool useAsync)
		{
			this._owner.RegisterWebFormsScript();
			if (this._owner.ClientSupportsJavaScript && this._owner.RequestInternal != null && this._owner.RequestInternal.Browser.SupportsCallback)
			{
				this.RegisterStartupScript(typeof(Page), "PageCallbackScript", (this._owner.RequestInternal != null && string.Equals(this._owner.RequestInternal.Url.Scheme, "https", StringComparison.OrdinalIgnoreCase)) ? ("\r\nvar callBackFrameUrl='" + Util.QuoteJScriptString(this.GetWebResourceUrl(typeof(Page), "SmartNav.htm"), false) + "';\r\nWebForm_InitCallback();") : "\r\nWebForm_InitCallback();", true);
			}
			if (argument == null)
			{
				argument = "null";
			}
			else if (argument.Length == 0)
			{
				argument = "\"\"";
			}
			if (context == null)
			{
				context = "null";
			}
			else if (context.Length == 0)
			{
				context = "\"\"";
			}
			return string.Concat(new string[]
			{
				"WebForm_DoCallback(",
				target,
				",",
				argument,
				",",
				clientCallback,
				",",
				context,
				",",
				(clientErrorCallback == null) ? "null" : clientErrorCallback,
				",",
				useAsync ? "true" : "false",
				")"
			});
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000CDF73 File Offset: 0x000CCF73
		public string GetPostBackClientHyperlink(Control control, string argument)
		{
			return this.GetPostBackClientHyperlink(control, argument, true, false);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000CDF7F File Offset: 0x000CCF7F
		public string GetPostBackClientHyperlink(Control control, string argument, bool registerForEventValidation)
		{
			return this.GetPostBackClientHyperlink(control, argument, true, registerForEventValidation);
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000CDF8B File Offset: 0x000CCF8B
		internal string GetPostBackClientHyperlink(Control control, string argument, bool escapePercent, bool registerForEventValidation)
		{
			return "javascript:" + this.GetPostBackEventReference(control, argument, escapePercent, registerForEventValidation);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000CDFA2 File Offset: 0x000CCFA2
		public string GetPostBackEventReference(Control control, string argument)
		{
			return this.GetPostBackEventReference(control, argument, false, false);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000CDFAE File Offset: 0x000CCFAE
		public string GetPostBackEventReference(Control control, string argument, bool registerForEventValidation)
		{
			return this.GetPostBackEventReference(control, argument, false, registerForEventValidation);
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000CDFBC File Offset: 0x000CCFBC
		private string GetPostBackEventReference(Control control, string argument, bool forUrl, bool registerForEventValidation)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			this._owner.RegisterPostBackScript();
			string text = control.UniqueID;
			if (registerForEventValidation)
			{
				this.RegisterForEventValidation(text, argument);
			}
			if (control.EnableLegacyRendering && this._owner.IsInOnFormRender && text != null && text.IndexOf(':') >= 0)
			{
				text = text.Replace(':', '$');
			}
			string text2 = "__doPostBack('" + text + "','";
			return text2 + Util.QuoteJScriptString(argument, forUrl) + "')";
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000CE048 File Offset: 0x000CD048
		public string GetPostBackEventReference(PostBackOptions options)
		{
			return this.GetPostBackEventReference(options, false);
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000CE054 File Offset: 0x000CD054
		public string GetPostBackEventReference(PostBackOptions options, bool registerForEventValidation)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			if (registerForEventValidation)
			{
				this.RegisterForEventValidation(options);
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (options.RequiresJavaScriptProtocol)
			{
				stringBuilder.Append("javascript:");
			}
			if (options.AutoPostBack)
			{
				stringBuilder.Append("setTimeout('");
			}
			if (!options.PerformValidation && !options.TrackFocus && options.ClientSubmit && string.IsNullOrEmpty(options.ActionUrl))
			{
				string postBackEventReference = this.GetPostBackEventReference(options.TargetControl, options.Argument);
				if (options.AutoPostBack)
				{
					stringBuilder.Append(Util.QuoteJScriptString(postBackEventReference));
					stringBuilder.Append("', 0)");
				}
				else
				{
					stringBuilder.Append(postBackEventReference);
				}
				return stringBuilder.ToString();
			}
			stringBuilder.Append("WebForm_DoPostBackWithOptions");
			stringBuilder.Append("(new WebForm_PostBackOptions(\"");
			stringBuilder.Append(options.TargetControl.UniqueID);
			stringBuilder.Append("\", ");
			if (string.IsNullOrEmpty(options.Argument))
			{
				stringBuilder.Append("\"\", ");
			}
			else
			{
				stringBuilder.Append("\"");
				stringBuilder.Append(Util.QuoteJScriptString(options.Argument));
				stringBuilder.Append("\", ");
			}
			if (options.PerformValidation)
			{
				flag = true;
				stringBuilder.Append("true, ");
			}
			else
			{
				stringBuilder.Append("false, ");
			}
			if (options.ValidationGroup != null && options.ValidationGroup.Length > 0)
			{
				flag = true;
				stringBuilder.Append("\"");
				stringBuilder.Append(options.ValidationGroup);
				stringBuilder.Append("\", ");
			}
			else
			{
				stringBuilder.Append("\"\", ");
			}
			if (options.ActionUrl != null && options.ActionUrl.Length > 0)
			{
				flag = true;
				this._owner.ContainsCrossPagePost = true;
				stringBuilder.Append("\"");
				stringBuilder.Append(Util.QuoteJScriptString(options.ActionUrl));
				stringBuilder.Append("\", ");
			}
			else
			{
				stringBuilder.Append("\"\", ");
			}
			if (options.TrackFocus)
			{
				this._owner.RegisterFocusScript();
				flag = true;
				stringBuilder.Append("true, ");
			}
			else
			{
				stringBuilder.Append("false, ");
			}
			if (options.ClientSubmit)
			{
				flag = true;
				this._owner.RegisterPostBackScript();
				stringBuilder.Append("true))");
			}
			else
			{
				stringBuilder.Append("false))");
			}
			if (options.AutoPostBack)
			{
				stringBuilder.Append("', 0)");
			}
			string text = null;
			if (flag)
			{
				text = stringBuilder.ToString();
				this._owner.RegisterWebFormsScript();
			}
			return text;
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000CE2E3 File Offset: 0x000CD2E3
		public string GetWebResourceUrl(Type type, string resourceName)
		{
			return ClientScriptManager.GetWebResourceUrl(this._owner, type, resourceName, false);
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000CE2F4 File Offset: 0x000CD2F4
		internal static string GetWebResourceUrl(Page owner, Type type, string resourceName, bool htmlEncoded)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (string.IsNullOrEmpty(resourceName))
			{
				throw new ArgumentNullException("resourceName");
			}
			if (owner != null && owner.DesignMode)
			{
				ISite site = ((IComponent)owner).Site;
				if (site != null)
				{
					IResourceUrlGenerator resourceUrlGenerator = site.GetService(typeof(IResourceUrlGenerator)) as IResourceUrlGenerator;
					if (resourceUrlGenerator != null)
					{
						return resourceUrlGenerator.GetResourceUrl(type, resourceName);
					}
				}
				return resourceName;
			}
			return AssemblyResourceLoader.GetWebResourceUrl(type, resourceName, htmlEncoded);
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000CE363 File Offset: 0x000CD363
		public bool IsClientScriptBlockRegistered(string key)
		{
			return this.IsClientScriptBlockRegistered(typeof(Page), key);
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000CE378 File Offset: 0x000CD378
		public bool IsClientScriptBlockRegistered(Type type, string key)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ScriptKey scriptKey = ClientScriptManager.CreateScriptKey(type, key);
			return this._registeredClientScriptBlocks != null && this._registeredClientScriptBlocks[scriptKey] != null;
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000CE3B7 File Offset: 0x000CD3B7
		public bool IsClientScriptIncludeRegistered(string key)
		{
			return this.IsClientScriptIncludeRegistered(typeof(Page), key);
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000CE3CA File Offset: 0x000CD3CA
		public bool IsClientScriptIncludeRegistered(Type type, string key)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return this._registeredClientScriptBlocks != null && this._registeredClientScriptBlocks[ClientScriptManager.CreateScriptIncludeKey(type, key)] != null;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000CE3FC File Offset: 0x000CD3FC
		public bool IsStartupScriptRegistered(string key)
		{
			return this.IsStartupScriptRegistered(typeof(Page), key);
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000CE40F File Offset: 0x000CD40F
		public bool IsStartupScriptRegistered(Type type, string key)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return this._registeredClientStartupScripts != null && this._registeredClientStartupScripts[ClientScriptManager.CreateScriptKey(type, key)] != null;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000CE441 File Offset: 0x000CD441
		public bool IsOnSubmitStatementRegistered(string key)
		{
			return this.IsOnSubmitStatementRegistered(typeof(Page), key);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000CE454 File Offset: 0x000CD454
		public bool IsOnSubmitStatementRegistered(Type type, string key)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			return this._registeredOnSubmitStatements != null && this._registeredOnSubmitStatements[ClientScriptManager.CreateScriptKey(type, key)] != null;
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000CE488 File Offset: 0x000CD488
		public void RegisterArrayDeclaration(string arrayName, string arrayValue)
		{
			if (arrayName == null)
			{
				throw new ArgumentNullException("arrayName");
			}
			if (this._registeredArrayDeclares == null)
			{
				this._registeredArrayDeclares = new ListDictionary();
			}
			if (!this._registeredArrayDeclares.Contains(arrayName))
			{
				this._registeredArrayDeclares[arrayName] = new ArrayList();
			}
			ArrayList arrayList = (ArrayList)this._registeredArrayDeclares[arrayName];
			arrayList.Add(arrayValue);
			if (this._owner.PartialCachingControlStack != null)
			{
				foreach (object obj in this._owner.PartialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterArrayDeclaration(arrayName, arrayValue);
				}
			}
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000CE550 File Offset: 0x000CD550
		internal void RegisterArrayDeclaration(Control control, string arrayName, string arrayValue)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterArrayDeclaration(control, arrayName, arrayValue);
				return;
			}
			this.RegisterArrayDeclaration(arrayName, arrayValue);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000CE586 File Offset: 0x000CD586
		public void RegisterExpandoAttribute(string controlId, string attributeName, string attributeValue)
		{
			this.RegisterExpandoAttribute(controlId, attributeName, attributeValue, true);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000CE594 File Offset: 0x000CD594
		public void RegisterExpandoAttribute(string controlId, string attributeName, string attributeValue, bool encode)
		{
			StringUtil.CheckAndTrimString(controlId, "controlId");
			StringUtil.CheckAndTrimString(attributeName, "attributeName");
			ListDictionary listDictionary = null;
			if (this._registeredControlsWithExpandoAttributes == null)
			{
				this._registeredControlsWithExpandoAttributes = new ListDictionary(StringComparer.Ordinal);
			}
			else
			{
				listDictionary = (ListDictionary)this._registeredControlsWithExpandoAttributes[controlId];
			}
			if (listDictionary == null)
			{
				listDictionary = new ListDictionary(StringComparer.Ordinal);
				this._registeredControlsWithExpandoAttributes.Add(controlId, listDictionary);
			}
			if (encode)
			{
				attributeValue = Util.QuoteJScriptString(attributeValue);
			}
			listDictionary.Add(attributeName, attributeValue);
			if (this._owner.PartialCachingControlStack != null)
			{
				foreach (object obj in this._owner.PartialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterExpandoAttribute(controlId, attributeName, attributeValue);
				}
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000CE678 File Offset: 0x000CD678
		internal void RegisterExpandoAttribute(Control control, string controlId, string attributeName, string attributeValue, bool encode)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterExpandoAttribute(control, controlId, attributeName, attributeValue, encode);
				return;
			}
			this.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000CE6B8 File Offset: 0x000CD6B8
		public void RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue)
		{
			if (hiddenFieldName == null)
			{
				throw new ArgumentNullException("hiddenFieldName");
			}
			if (this._registeredHiddenFields == null)
			{
				this._registeredHiddenFields = new ListDictionary();
			}
			if (!this._registeredHiddenFields.Contains(hiddenFieldName))
			{
				this._registeredHiddenFields.Add(hiddenFieldName, hiddenFieldInitialValue);
			}
			if (this._owner.PartialCachingControlStack != null)
			{
				foreach (object obj in this._owner.PartialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterHiddenField(hiddenFieldName, hiddenFieldInitialValue);
				}
			}
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000CE760 File Offset: 0x000CD760
		internal void RegisterHiddenField(Control control, string hiddenFieldName, string hiddenFieldValue)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterHiddenField(control, hiddenFieldName, hiddenFieldValue);
				return;
			}
			this.RegisterHiddenField(hiddenFieldName, hiddenFieldValue);
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000CE796 File Offset: 0x000CD796
		public void RegisterClientScriptBlock(Type type, string key, string script)
		{
			this.RegisterClientScriptBlock(type, key, script, false);
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000CE7A2 File Offset: 0x000CD7A2
		public void RegisterClientScriptBlock(Type type, string key, string script, bool addScriptTags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (addScriptTags)
			{
				this.RegisterScriptBlock(ClientScriptManager.CreateScriptKey(type, key), script, ClientAPIRegisterType.ClientScriptBlocksWithoutTags);
				return;
			}
			this.RegisterScriptBlock(ClientScriptManager.CreateScriptKey(type, key), script, ClientAPIRegisterType.ClientScriptBlocks);
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000CE7D8 File Offset: 0x000CD7D8
		internal void RegisterClientScriptBlock(Control control, Type type, string key, string script, bool addScriptTags)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterClientScriptBlock(control, type, key, script, addScriptTags);
				return;
			}
			this.RegisterClientScriptBlock(type, key, script, addScriptTags);
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000CE816 File Offset: 0x000CD816
		public void RegisterClientScriptInclude(string key, string url)
		{
			this.RegisterClientScriptInclude(typeof(Page), key, url);
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000CE82C File Offset: 0x000CD82C
		public void RegisterClientScriptInclude(Type type, string key, string url)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (string.IsNullOrEmpty(url))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("url");
			}
			string text = "\r\n<script src=\"" + HttpUtility.HtmlAttributeEncode(url) + "\" type=\"text/javascript\"></script>";
			this.RegisterScriptBlock(ClientScriptManager.CreateScriptIncludeKey(type, key), text, ClientAPIRegisterType.ClientScriptBlocks);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000CE880 File Offset: 0x000CD880
		internal void RegisterClientScriptInclude(Control control, Type type, string key, string url)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterClientScriptInclude(control, type, key, url);
				return;
			}
			this.RegisterClientScriptInclude(type, key, url);
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000CE8BA File Offset: 0x000CD8BA
		public void RegisterClientScriptResource(Type type, string resourceName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.RegisterClientScriptInclude(type, resourceName, this.GetWebResourceUrl(type, resourceName));
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x000CE8DC File Offset: 0x000CD8DC
		internal void RegisterClientScriptResource(Control control, Type type, string resourceName)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterClientScriptResource(control, type, resourceName);
				return;
			}
			this.RegisterClientScriptResource(type, resourceName);
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x000CE914 File Offset: 0x000CD914
		internal void RegisterDefaultButtonScript(Control button, HtmlTextWriter writer, bool useAddAttribute)
		{
			this._owner.RegisterWebFormsScript();
			if (this._owner.EnableLegacyRendering)
			{
				if (useAddAttribute)
				{
					writer.AddAttribute("language", "javascript", false);
				}
				else
				{
					writer.WriteAttribute("language", "javascript", false);
				}
			}
			string text = "javascript:return WebForm_FireDefaultButton(event, '" + button.ClientID + "')";
			if (useAddAttribute)
			{
				writer.AddAttribute("onkeypress", text);
				return;
			}
			writer.WriteAttribute("onkeypress", text);
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x000CE992 File Offset: 0x000CD992
		public void RegisterOnSubmitStatement(Type type, string key, string script)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.RegisterOnSubmitStatementInternal(ClientScriptManager.CreateScriptKey(type, key), script);
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000CE9B0 File Offset: 0x000CD9B0
		internal void RegisterOnSubmitStatement(Control control, Type type, string key, string script)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterOnSubmitStatement(control, type, key, script);
				return;
			}
			this.RegisterOnSubmitStatement(type, key, script);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000CE9EC File Offset: 0x000CD9EC
		internal void RegisterOnSubmitStatementInternal(ScriptKey key, string script)
		{
			if (string.IsNullOrEmpty(script))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("script");
			}
			if (this._registeredOnSubmitStatements == null)
			{
				this._registeredOnSubmitStatements = new ListDictionary();
			}
			int num = script.Length - 1;
			while (num >= 0 && char.IsWhiteSpace(script, num))
			{
				num--;
			}
			if (num >= 0 && script[num] != ';')
			{
				script = script.Substring(0, num + 1) + ";" + script.Substring(num + 1);
			}
			if (this._registeredOnSubmitStatements[key] == null)
			{
				this._registeredOnSubmitStatements.Add(key, script);
			}
			if (this._owner.PartialCachingControlStack != null)
			{
				foreach (object obj in this._owner.PartialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterOnSubmitStatement(key, script);
				}
			}
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000CEAE4 File Offset: 0x000CDAE4
		internal void RegisterScriptBlock(ScriptKey key, string script, ClientAPIRegisterType type)
		{
			switch (type)
			{
			case ClientAPIRegisterType.ClientScriptBlocks:
				this.RegisterScriptBlock(key, script, ref this._registeredClientScriptBlocks, ref this._clientScriptBlocks, false, ref this._clientScriptBlocksInScriptTag);
				break;
			case ClientAPIRegisterType.ClientScriptBlocksWithoutTags:
				this.RegisterScriptBlock(key, script, ref this._registeredClientScriptBlocks, ref this._clientScriptBlocks, true, ref this._clientScriptBlocksInScriptTag);
				break;
			case ClientAPIRegisterType.ClientStartupScripts:
				this.RegisterScriptBlock(key, script, ref this._registeredClientStartupScripts, ref this._clientStartupScripts, false, ref this._clientStartupScriptInScriptTag);
				break;
			case ClientAPIRegisterType.ClientStartupScriptsWithoutTags:
				this.RegisterScriptBlock(key, script, ref this._registeredClientStartupScripts, ref this._clientStartupScripts, true, ref this._clientStartupScriptInScriptTag);
				break;
			}
			if (this._owner.PartialCachingControlStack != null)
			{
				foreach (object obj in this._owner.PartialCachingControlStack)
				{
					BasePartialCachingControl basePartialCachingControl = (BasePartialCachingControl)obj;
					basePartialCachingControl.RegisterScriptBlock(type, key, script);
				}
			}
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000CEBE0 File Offset: 0x000CDBE0
		private void RegisterScriptBlock(ScriptKey key, string script, ref ListDictionary scriptBlocks, ref ArrayList scriptList, bool needsScriptTags, ref bool inScriptBlock)
		{
			if (scriptBlocks == null)
			{
				scriptBlocks = new ListDictionary();
			}
			if (scriptBlocks[key] == null)
			{
				scriptBlocks.Add(key, script);
				if (scriptList == null)
				{
					scriptList = new ArrayList();
					if (needsScriptTags)
					{
						scriptList.Add(this._owner.EnableLegacyRendering ? "\r\n<script type=\"text/javascript\">\r\n<!--\r\n" : "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n");
					}
				}
				else if (needsScriptTags)
				{
					if (!inScriptBlock)
					{
						scriptList.Add(this._owner.EnableLegacyRendering ? "\r\n<script type=\"text/javascript\">\r\n<!--\r\n" : "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n");
					}
				}
				else if (inScriptBlock)
				{
					scriptList.Add(this._owner.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
				}
				scriptList.Add(script);
				inScriptBlock = needsScriptTags;
			}
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000CECA6 File Offset: 0x000CDCA6
		public void RegisterStartupScript(Type type, string key, string script)
		{
			this.RegisterStartupScript(type, key, script, false);
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000CECB2 File Offset: 0x000CDCB2
		public void RegisterStartupScript(Type type, string key, string script, bool addScriptTags)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (addScriptTags)
			{
				this.RegisterScriptBlock(ClientScriptManager.CreateScriptKey(type, key), script, ClientAPIRegisterType.ClientStartupScriptsWithoutTags);
				return;
			}
			this.RegisterScriptBlock(ClientScriptManager.CreateScriptKey(type, key), script, ClientAPIRegisterType.ClientStartupScripts);
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000CECE8 File Offset: 0x000CDCE8
		internal void RegisterStartupScript(Control control, Type type, string key, string script, bool addScriptTags)
		{
			IScriptManager scriptManager = this._owner.ScriptManager;
			if (scriptManager != null && scriptManager.SupportsPartialRendering)
			{
				scriptManager.RegisterStartupScript(control, type, key, script, addScriptTags);
				return;
			}
			this.RegisterStartupScript(type, key, script, addScriptTags);
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000CED28 File Offset: 0x000CDD28
		internal void RenderArrayDeclares(HtmlTextWriter writer)
		{
			if (this._registeredArrayDeclares == null || this._registeredArrayDeclares.Count == 0)
			{
				return;
			}
			writer.Write(this._owner.EnableLegacyRendering ? "\r\n<script type=\"text/javascript\">\r\n<!--\r\n" : "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n");
			IDictionaryEnumerator enumerator = this._registeredArrayDeclares.GetEnumerator();
			while (enumerator.MoveNext())
			{
				writer.Write("var ");
				writer.Write(enumerator.Key);
				writer.Write(" =  new Array(");
				IEnumerator enumerator2 = ((ArrayList)enumerator.Value).GetEnumerator();
				bool flag = true;
				while (enumerator2.MoveNext())
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						writer.Write(", ");
					}
					writer.Write(enumerator2.Current);
				}
				writer.WriteLine(");");
			}
			writer.Write(this._owner.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000CEE08 File Offset: 0x000CDE08
		internal void RenderExpandoAttribute(HtmlTextWriter writer)
		{
			if (this._registeredControlsWithExpandoAttributes == null || this._registeredControlsWithExpandoAttributes.Count == 0)
			{
				return;
			}
			writer.Write(this._owner.EnableLegacyRendering ? "\r\n<script type=\"text/javascript\">\r\n<!--\r\n" : "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n");
			foreach (object obj in this._registeredControlsWithExpandoAttributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				writer.Write("var ");
				writer.Write(text);
				writer.Write(" = document.all ? document.all[\"");
				writer.Write(text);
				writer.Write("\"] : document.getElementById(\"");
				writer.Write(text);
				writer.WriteLine("\");");
				ListDictionary listDictionary = (ListDictionary)dictionaryEntry.Value;
				foreach (object obj2 in listDictionary)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					writer.Write(text);
					writer.Write(".");
					writer.Write(dictionaryEntry2.Key);
					if (dictionaryEntry2.Value == null)
					{
						writer.WriteLine(" = null;");
					}
					else
					{
						writer.Write(" = \"");
						writer.Write(dictionaryEntry2.Value);
						writer.WriteLine("\";");
					}
				}
			}
			writer.Write(this._owner.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000CEFCC File Offset: 0x000CDFCC
		internal void RenderHiddenFields(HtmlTextWriter writer)
		{
			if (this._registeredHiddenFields == null || this._registeredHiddenFields.Count == 0)
			{
				return;
			}
			foreach (object obj in this._registeredHiddenFields)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				if (text == null)
				{
					text = string.Empty;
				}
				writer.WriteLine();
				writer.Write("<input type=\"hidden\" name=\"");
				writer.Write(text);
				writer.Write("\" id=\"");
				writer.Write(text);
				writer.Write("\" value=\"");
				HttpUtility.HtmlEncode((string)dictionaryEntry.Value, writer);
				writer.Write("\" />");
			}
			this.ClearHiddenFields();
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000CF0A4 File Offset: 0x000CE0A4
		internal void RenderClientScriptBlocks(HtmlTextWriter writer)
		{
			if (this._clientScriptBlocks != null)
			{
				writer.WriteLine();
				foreach (object obj in this._clientScriptBlocks)
				{
					string text = (string)obj;
					writer.Write(text);
				}
			}
			if (!string.IsNullOrEmpty(this._owner.ClientOnSubmitEvent) && this._owner.ClientSupportsJavaScript)
			{
				if (!this._clientScriptBlocksInScriptTag)
				{
					writer.Write(this._owner.EnableLegacyRendering ? "\r\n<script type=\"text/javascript\">\r\n<!--\r\n" : "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n");
				}
				writer.Write("function WebForm_OnSubmit() {\r\n");
				if (this._registeredOnSubmitStatements != null)
				{
					foreach (object obj2 in this._registeredOnSubmitStatements.Values)
					{
						string text2 = (string)obj2;
						writer.Write(text2);
					}
				}
				writer.WriteLine("\r\nreturn true;\r\n}");
				writer.Write(this._owner.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
				return;
			}
			if (this._clientScriptBlocksInScriptTag)
			{
				writer.Write(this._owner.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000CF210 File Offset: 0x000CE210
		internal void RenderClientStartupScripts(HtmlTextWriter writer)
		{
			if (this._clientStartupScripts != null)
			{
				writer.WriteLine();
				foreach (object obj in this._clientStartupScripts)
				{
					string text = (string)obj;
					writer.Write(text);
				}
				if (this._clientStartupScriptInScriptTag)
				{
					writer.Write(this._owner.EnableLegacyRendering ? "// -->\r\n</script>\r\n" : "//]]>\r\n</script>\r\n");
				}
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000CF2A0 File Offset: 0x000CE2A0
		internal void RenderWebFormsScript(HtmlTextWriter writer)
		{
			writer.Write("\r\n<script src=\"");
			writer.Write(ClientScriptManager.GetWebResourceUrl(this._owner, typeof(Page), "WebForms.js", true));
			writer.WriteLine("\" type=\"text/javascript\"></script>");
		}

		// Token: 0x04002151 RID: 8529
		private const string IncludeScriptBegin = "\r\n<script src=\"";

		// Token: 0x04002152 RID: 8530
		private const string IncludeScriptEnd = "\" type=\"text/javascript\"></script>";

		// Token: 0x04002153 RID: 8531
		internal const string ClientScriptStart = "\r\n<script type=\"text/javascript\">\r\n//<![CDATA[\r\n";

		// Token: 0x04002154 RID: 8532
		internal const string ClientScriptStartLegacy = "\r\n<script type=\"text/javascript\">\r\n<!--\r\n";

		// Token: 0x04002155 RID: 8533
		internal const string ClientScriptEnd = "//]]>\r\n</script>\r\n";

		// Token: 0x04002156 RID: 8534
		internal const string ClientScriptEndLegacy = "// -->\r\n</script>\r\n";

		// Token: 0x04002157 RID: 8535
		internal const string JscriptPrefix = "javascript:";

		// Token: 0x04002158 RID: 8536
		private const string _callbackFunctionName = "WebForm_DoCallback";

		// Token: 0x04002159 RID: 8537
		private const string _postbackOptionsFunctionName = "WebForm_DoPostBackWithOptions";

		// Token: 0x0400215A RID: 8538
		private const string _postBackFunctionName = "__doPostBack";

		// Token: 0x0400215B RID: 8539
		private const string PageCallbackScriptKey = "PageCallbackScript";

		// Token: 0x0400215C RID: 8540
		private ListDictionary _registeredClientScriptBlocks;

		// Token: 0x0400215D RID: 8541
		private ArrayList _clientScriptBlocks;

		// Token: 0x0400215E RID: 8542
		private bool _clientScriptBlocksInScriptTag;

		// Token: 0x0400215F RID: 8543
		private ListDictionary _registeredClientStartupScripts;

		// Token: 0x04002160 RID: 8544
		private ArrayList _clientStartupScripts;

		// Token: 0x04002161 RID: 8545
		private bool _clientStartupScriptInScriptTag;

		// Token: 0x04002162 RID: 8546
		private bool _eventValidationFieldLoaded;

		// Token: 0x04002163 RID: 8547
		private ListDictionary _registeredOnSubmitStatements;

		// Token: 0x04002164 RID: 8548
		private IDictionary _registeredArrayDeclares;

		// Token: 0x04002165 RID: 8549
		private IDictionary _registeredHiddenFields;

		// Token: 0x04002166 RID: 8550
		private ListDictionary _registeredControlsWithExpandoAttributes;

		// Token: 0x04002167 RID: 8551
		private ArrayList _validEventReferences;

		// Token: 0x04002168 RID: 8552
		private HybridDictionary _clientPostBackValidatedEventTable;

		// Token: 0x04002169 RID: 8553
		private Page _owner;
	}
}
