using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI.HtmlControls;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x02000451 RID: 1105
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class BasePartialCachingControl : Control
	{
		// Token: 0x06003494 RID: 13460 RVA: 0x000E3C24 File Offset: 0x000E2C24
		internal override void InitRecursive(Control namingContainer)
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			this._cacheKey = this.ComputeNonVaryCacheKey(hashCodeCombiner);
			this._nonVaryHashCode = hashCodeCombiner.CombinedHash;
			BasePartialCachingControl.PartialCachingCacheEntry partialCachingCacheEntry = null;
			object obj = HttpRuntime.CacheInternal.Get(this._cacheKey);
			if (obj != null)
			{
				ControlCachedVary controlCachedVary = obj as ControlCachedVary;
				if (controlCachedVary != null)
				{
					string text = this.ComputeVaryCacheKey(hashCodeCombiner, controlCachedVary);
					partialCachingCacheEntry = (BasePartialCachingControl.PartialCachingCacheEntry)HttpRuntime.CacheInternal.Get(text);
				}
				else
				{
					partialCachingCacheEntry = (BasePartialCachingControl.PartialCachingCacheEntry)obj;
				}
			}
			if (partialCachingCacheEntry == null)
			{
				this._cacheEntry = new BasePartialCachingControl.PartialCachingCacheEntry();
				this._cachedCtrl = this.CreateCachedControl();
				this.Controls.Add(this._cachedCtrl);
				this.Page.PushCachingControl(this);
				base.InitRecursive(namingContainer);
				this.Page.PopCachingControl();
				return;
			}
			this._outputString = partialCachingCacheEntry.OutputString;
			this._cssStyleString = partialCachingCacheEntry.CssStyleString;
			if (partialCachingCacheEntry.RegisteredClientCalls != null)
			{
				foreach (object obj2 in partialCachingCacheEntry.RegisteredClientCalls)
				{
					BasePartialCachingControl.RegisterCallData registerCallData = (BasePartialCachingControl.RegisterCallData)obj2;
					switch (registerCallData.Type)
					{
					case ClientAPIRegisterType.WebFormsScript:
						this.Page.RegisterWebFormsScript();
						break;
					case ClientAPIRegisterType.PostBackScript:
						this.Page.RegisterPostBackScript();
						break;
					case ClientAPIRegisterType.FocusScript:
						this.Page.RegisterFocusScript();
						break;
					case ClientAPIRegisterType.ClientScriptBlocks:
					case ClientAPIRegisterType.ClientScriptBlocksWithoutTags:
					case ClientAPIRegisterType.ClientStartupScripts:
					case ClientAPIRegisterType.ClientStartupScriptsWithoutTags:
						this.Page.ClientScript.RegisterScriptBlock(registerCallData.Key, registerCallData.StringParam2, registerCallData.Type);
						break;
					case ClientAPIRegisterType.OnSubmitStatement:
						this.Page.ClientScript.RegisterOnSubmitStatementInternal(registerCallData.Key, registerCallData.StringParam2);
						break;
					case ClientAPIRegisterType.ArrayDeclaration:
						this.Page.ClientScript.RegisterArrayDeclaration(registerCallData.StringParam1, registerCallData.StringParam2);
						break;
					case ClientAPIRegisterType.HiddenField:
						this.Page.ClientScript.RegisterHiddenField(registerCallData.StringParam1, registerCallData.StringParam2);
						break;
					case ClientAPIRegisterType.ExpandoAttribute:
						this.Page.ClientScript.RegisterExpandoAttribute(registerCallData.StringParam1, registerCallData.StringParam2, registerCallData.StringParam3, false);
						break;
					case ClientAPIRegisterType.EventValidation:
						if (this._registeredCallDataForEventValidation == null)
						{
							this._registeredCallDataForEventValidation = new ArrayList();
						}
						this._registeredCallDataForEventValidation.Add(registerCallData);
						break;
					}
				}
			}
			base.InitRecursive(namingContainer);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x000E3EB4 File Offset: 0x000E2EB4
		internal override void LoadRecursive()
		{
			if (this._outputString != null)
			{
				base.LoadRecursive();
				return;
			}
			this.Page.PushCachingControl(this);
			base.LoadRecursive();
			this.Page.PopCachingControl();
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x000E3EE4 File Offset: 0x000E2EE4
		internal override void PreRenderRecursiveInternal()
		{
			if (this._outputString != null)
			{
				base.PreRenderRecursiveInternal();
				if (this._cssStyleString != null && this.Page.Header != null)
				{
					this.Page.Header.RegisterCssStyleString(this._cssStyleString);
				}
				return;
			}
			this.Page.PushCachingControl(this);
			base.PreRenderRecursiveInternal();
			this.Page.PopCachingControl();
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x000E3F48 File Offset: 0x000E2F48
		public override void Dispose()
		{
			if (this._cacheDependency != null)
			{
				this._cacheDependency.Dispose();
				this._cacheDependency = null;
			}
			base.Dispose();
		}

		// Token: 0x06003498 RID: 13464
		internal abstract Control CreateCachedControl();

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06003499 RID: 13465 RVA: 0x000E3F6A File Offset: 0x000E2F6A
		// (set) Token: 0x0600349A RID: 13466 RVA: 0x000E3F72 File Offset: 0x000E2F72
		public CacheDependency Dependency
		{
			get
			{
				return this._cacheDependency;
			}
			set
			{
				this._cacheDependency = value;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600349B RID: 13467 RVA: 0x000E3F7B File Offset: 0x000E2F7B
		public ControlCachePolicy CachePolicy
		{
			get
			{
				if (this._cachePolicy == null)
				{
					this._cachePolicy = new ControlCachePolicy(this);
				}
				return this._cachePolicy;
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600349C RID: 13468 RVA: 0x000E3F97 File Offset: 0x000E2F97
		internal HttpCacheVaryByParams VaryByParams
		{
			get
			{
				if (this._varyByParamsCollection == null)
				{
					this._varyByParamsCollection = new HttpCacheVaryByParams();
					this._varyByParamsCollection.IgnoreParams = true;
				}
				return this._varyByParamsCollection;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600349D RID: 13469 RVA: 0x000E3FBE File Offset: 0x000E2FBE
		// (set) Token: 0x0600349E RID: 13470 RVA: 0x000E3FE0 File Offset: 0x000E2FE0
		internal string VaryByControl
		{
			get
			{
				if (this._varyByControlsCollection == null)
				{
					return string.Empty;
				}
				return string.Join(";", this._varyByControlsCollection);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this._varyByControlsCollection = null;
					return;
				}
				this._varyByControlsCollection = value.Split(new char[] { ';' });
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600349F RID: 13471 RVA: 0x000E4016 File Offset: 0x000E3016
		// (set) Token: 0x060034A0 RID: 13472 RVA: 0x000E4040 File Offset: 0x000E3040
		internal TimeSpan Duration
		{
			get
			{
				if (this._utcExpirationTime == DateTime.MaxValue)
				{
					return TimeSpan.MaxValue;
				}
				return this._utcExpirationTime - DateTime.UtcNow;
			}
			set
			{
				if (value == TimeSpan.MaxValue)
				{
					this._utcExpirationTime = DateTime.MaxValue;
					return;
				}
				this._utcExpirationTime = DateTime.UtcNow.Add(value);
			}
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x000E407C File Offset: 0x000E307C
		private void RegisterValidationEvents()
		{
			if (this._registeredCallDataForEventValidation != null)
			{
				foreach (object obj in this._registeredCallDataForEventValidation)
				{
					BasePartialCachingControl.RegisterCallData registerCallData = (BasePartialCachingControl.RegisterCallData)obj;
					this.Page.ClientScript.RegisterForEventValidation(registerCallData.StringParam1, registerCallData.StringParam2);
				}
			}
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x000E40F4 File Offset: 0x000E30F4
		internal void RegisterStyleInfo(SelectorStyleInfo selectorInfo)
		{
			if (this._registeredStyleInfo == null)
			{
				this._registeredStyleInfo = new ArrayList();
			}
			this._registeredStyleInfo.Add(selectorInfo);
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x000E4118 File Offset: 0x000E3118
		protected internal override void Render(HtmlTextWriter output)
		{
			CacheDependency cacheDependency = null;
			if (this._outputString != null)
			{
				output.Write(this._outputString);
				this.RegisterValidationEvents();
				return;
			}
			if (this._cachingDisabled || !RuntimeConfig.GetAppConfig().OutputCache.EnableFragmentCache)
			{
				this._cachedCtrl.RenderControl(output);
				return;
			}
			if (this._sqlDependency != null)
			{
				cacheDependency = SqlCacheDependency.CreateOutputCacheDependency(this._sqlDependency);
			}
			this._cacheEntry.CssStyleString = this.GetCssStyleRenderString(output.GetType());
			StringWriter stringWriter = new StringWriter();
			HtmlTextWriter htmlTextWriter = Page.CreateHtmlTextWriterFromType(stringWriter, output.GetType());
			TextWriter textWriter = this.Context.Response.SwitchWriter(stringWriter);
			try
			{
				this.Page.PushCachingControl(this);
				this._cachedCtrl.RenderControl(htmlTextWriter);
				this.Page.PopCachingControl();
			}
			finally
			{
				this.Context.Response.SwitchWriter(textWriter);
			}
			this._cacheEntry.OutputString = stringWriter.ToString();
			output.Write(this._cacheEntry.OutputString);
			CacheDependency cacheDependency2 = this._cacheDependency;
			if (cacheDependency != null)
			{
				if (cacheDependency2 == null)
				{
					cacheDependency2 = cacheDependency;
				}
				else
				{
					AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
					aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency2 });
					aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency });
					cacheDependency2 = aggregateCacheDependency;
				}
			}
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text;
			if (this._varyByParamsCollection == null && this._varyByControlsCollection == null && this._varyByCustom == null)
			{
				text = this._cacheKey;
			}
			else
			{
				string[] array = null;
				if (this._varyByParamsCollection != null)
				{
					array = this._varyByParamsCollection.GetParams();
				}
				ControlCachedVary controlCachedVary = new ControlCachedVary(array, this._varyByControlsCollection, this._varyByCustom);
				HashCodeCombiner hashCodeCombiner = new HashCodeCombiner(this._nonVaryHashCode);
				text = this.ComputeVaryCacheKey(hashCodeCombiner, controlCachedVary);
				ControlCachedVary controlCachedVary2 = (ControlCachedVary)cacheInternal.UtcAdd(this._cacheKey, controlCachedVary, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
				if (controlCachedVary2 != null && !controlCachedVary.Equals(controlCachedVary2))
				{
					cacheInternal.UtcInsert(this._cacheKey, controlCachedVary);
				}
				CacheDependency cacheDependency3 = new CacheDependency(0, null, new string[] { this._cacheKey });
				if (cacheDependency2 == null)
				{
					cacheDependency2 = cacheDependency3;
				}
				else
				{
					AggregateCacheDependency aggregateCacheDependency2 = new AggregateCacheDependency();
					aggregateCacheDependency2.Add(new CacheDependency[] { cacheDependency2, cacheDependency3 });
					cacheDependency2 = aggregateCacheDependency2;
				}
			}
			DateTime dateTime;
			TimeSpan timeSpan;
			if (this._useSlidingExpiration)
			{
				dateTime = Cache.NoAbsoluteExpiration;
				timeSpan = this._utcExpirationTime - DateTime.UtcNow;
			}
			else
			{
				dateTime = this._utcExpirationTime;
				timeSpan = Cache.NoSlidingExpiration;
			}
			cacheInternal.UtcInsert(text, this._cacheEntry, cacheDependency2, dateTime, timeSpan);
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x000E43B0 File Offset: 0x000E33B0
		private string ComputeNonVaryCacheKey(HashCodeCombiner combinedHashCode)
		{
			combinedHashCode.AddObject(this._guid);
			HttpBrowserCapabilities browser = this.Context.Request.Browser;
			if (browser != null)
			{
				combinedHashCode.AddObject(browser.TagWriter);
			}
			return "l" + combinedHashCode.CombinedHashString;
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x000E43FC File Offset: 0x000E33FC
		private string ComputeVaryCacheKey(HashCodeCombiner combinedHashCode, ControlCachedVary cachedVary)
		{
			combinedHashCode.AddInt(1);
			NameValueCollection nameValueCollection = this.Page.RequestValueCollection;
			if (nameValueCollection == null)
			{
				nameValueCollection = this.Page.GetCollectionBasedOnMethod(true);
			}
			if (cachedVary._varyByParams != null)
			{
				ICollection collection;
				if (cachedVary._varyByParams.Length == 1 && cachedVary._varyByParams[0] == "*")
				{
					collection = nameValueCollection;
				}
				else
				{
					collection = cachedVary._varyByParams;
				}
				foreach (object obj in collection)
				{
					string text = (string)obj;
					combinedHashCode.AddCaseInsensitiveString(text);
					string text2 = nameValueCollection[text];
					if (text2 != null)
					{
						combinedHashCode.AddObject(text2);
					}
				}
			}
			if (cachedVary._varyByControls != null)
			{
				string text3;
				if (this.NamingContainer == this.Page)
				{
					text3 = string.Empty;
				}
				else
				{
					text3 = this.NamingContainer.UniqueID;
					text3 += base.IdSeparator;
				}
				text3 = text3 + this._ctrlID + base.IdSeparator;
				foreach (string text4 in cachedVary._varyByControls)
				{
					string text5 = text3 + text4.Trim();
					combinedHashCode.AddCaseInsensitiveString(text5);
					string text6 = nameValueCollection[text5];
					if (text6 != null)
					{
						combinedHashCode.AddObject(nameValueCollection[text5]);
					}
				}
			}
			if (cachedVary._varyByCustom != null)
			{
				string varyByCustomString = this.Context.ApplicationInstance.GetVaryByCustomString(this.Context, cachedVary._varyByCustom);
				if (varyByCustomString != null)
				{
					combinedHashCode.AddObject(varyByCustomString);
				}
			}
			return "l" + combinedHashCode.CombinedHashString;
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x000E45B4 File Offset: 0x000E35B4
		private string GetCssStyleRenderString(Type htmlTextWriterType)
		{
			if (this._registeredStyleInfo == null)
			{
				return null;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			HtmlTextWriter htmlTextWriter = Page.CreateHtmlTextWriterFromType(stringWriter, htmlTextWriterType);
			CssTextWriter cssTextWriter = new CssTextWriter(htmlTextWriter);
			foreach (object obj in this._registeredStyleInfo)
			{
				SelectorStyleInfo selectorStyleInfo = (SelectorStyleInfo)obj;
				HtmlHead.RenderCssRule(cssTextWriter, selectorStyleInfo.selector, selectorStyleInfo.style, selectorStyleInfo.urlResolver);
			}
			return stringWriter.ToString();
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x000E4650 File Offset: 0x000E3650
		internal void SetVaryByParamsCollectionFromString(string varyByParams)
		{
			if (varyByParams == null)
			{
				return;
			}
			string[] array = varyByParams.Split(new char[] { ';' });
			this._varyByParamsCollection = new HttpCacheVaryByParams();
			this._varyByParamsCollection.ResetFromParams(array);
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x000E468C File Offset: 0x000E368C
		internal void RegisterPostBackScript()
		{
			this.RegisterClientCall(ClientAPIRegisterType.PostBackScript, string.Empty, null);
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x000E469B File Offset: 0x000E369B
		internal void RegisterFocusScript()
		{
			this.RegisterClientCall(ClientAPIRegisterType.FocusScript, string.Empty, null);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x000E46AA File Offset: 0x000E36AA
		internal void RegisterWebFormsScript()
		{
			this.RegisterClientCall(ClientAPIRegisterType.WebFormsScript, string.Empty, null);
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x000E46BC File Offset: 0x000E36BC
		private void RegisterClientCall(ClientAPIRegisterType type, ScriptKey scriptKey, string stringParam2)
		{
			BasePartialCachingControl.RegisterCallData registerCallData = new BasePartialCachingControl.RegisterCallData();
			registerCallData.Type = type;
			registerCallData.Key = scriptKey;
			registerCallData.StringParam2 = stringParam2;
			if (this._cacheEntry.RegisteredClientCalls == null)
			{
				this._cacheEntry.RegisteredClientCalls = new ArrayList();
			}
			this._cacheEntry.RegisteredClientCalls.Add(registerCallData);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x000E4713 File Offset: 0x000E3713
		private void RegisterClientCall(ClientAPIRegisterType type, string stringParam1, string stringParam2)
		{
			this.RegisterClientCall(type, stringParam1, stringParam2, null);
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x000E4720 File Offset: 0x000E3720
		private void RegisterClientCall(ClientAPIRegisterType type, string stringParam1, string stringParam2, string stringParam3)
		{
			BasePartialCachingControl.RegisterCallData registerCallData = new BasePartialCachingControl.RegisterCallData();
			registerCallData.Type = type;
			registerCallData.StringParam1 = stringParam1;
			registerCallData.StringParam2 = stringParam2;
			registerCallData.StringParam3 = stringParam3;
			if (this._cacheEntry.RegisteredClientCalls == null)
			{
				this._cacheEntry.RegisteredClientCalls = new ArrayList();
			}
			this._cacheEntry.RegisteredClientCalls.Add(registerCallData);
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x000E477F File Offset: 0x000E377F
		internal void RegisterScriptBlock(ClientAPIRegisterType type, ScriptKey key, string script)
		{
			this.RegisterClientCall(type, key, script);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x000E478A File Offset: 0x000E378A
		internal void RegisterOnSubmitStatement(ScriptKey key, string script)
		{
			this.RegisterClientCall(ClientAPIRegisterType.OnSubmitStatement, key, script);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x000E4795 File Offset: 0x000E3795
		internal void RegisterArrayDeclaration(string arrayName, string arrayValue)
		{
			this.RegisterClientCall(ClientAPIRegisterType.ArrayDeclaration, arrayName, arrayValue);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x000E47A0 File Offset: 0x000E37A0
		internal void RegisterHiddenField(string hiddenFieldName, string hiddenFieldInitialValue)
		{
			this.RegisterClientCall(ClientAPIRegisterType.HiddenField, hiddenFieldName, hiddenFieldInitialValue);
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x000E47AC File Offset: 0x000E37AC
		internal void RegisterExpandoAttribute(string controlID, string attributeName, string attributeValue)
		{
			this.RegisterClientCall(ClientAPIRegisterType.ExpandoAttribute, controlID, attributeName, attributeValue);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000E47B9 File Offset: 0x000E37B9
		internal void RegisterForEventValidation(string uniqueID, string argument)
		{
			this.RegisterClientCall(ClientAPIRegisterType.EventValidation, uniqueID, argument);
		}

		// Token: 0x040024C8 RID: 9416
		internal const char varySeparator = ';';

		// Token: 0x040024C9 RID: 9417
		internal const string varySeparatorString = ";";

		// Token: 0x040024CA RID: 9418
		internal Control _cachedCtrl;

		// Token: 0x040024CB RID: 9419
		private long _nonVaryHashCode;

		// Token: 0x040024CC RID: 9420
		internal string _ctrlID;

		// Token: 0x040024CD RID: 9421
		internal string _guid;

		// Token: 0x040024CE RID: 9422
		internal DateTime _utcExpirationTime;

		// Token: 0x040024CF RID: 9423
		internal bool _useSlidingExpiration;

		// Token: 0x040024D0 RID: 9424
		internal HttpCacheVaryByParams _varyByParamsCollection;

		// Token: 0x040024D1 RID: 9425
		internal string[] _varyByControlsCollection;

		// Token: 0x040024D2 RID: 9426
		internal string _varyByCustom;

		// Token: 0x040024D3 RID: 9427
		internal string _sqlDependency;

		// Token: 0x040024D4 RID: 9428
		internal bool _cachingDisabled;

		// Token: 0x040024D5 RID: 9429
		private string _outputString;

		// Token: 0x040024D6 RID: 9430
		private string _cssStyleString;

		// Token: 0x040024D7 RID: 9431
		private string _cacheKey;

		// Token: 0x040024D8 RID: 9432
		private CacheDependency _cacheDependency;

		// Token: 0x040024D9 RID: 9433
		private BasePartialCachingControl.PartialCachingCacheEntry _cacheEntry;

		// Token: 0x040024DA RID: 9434
		private ControlCachePolicy _cachePolicy;

		// Token: 0x040024DB RID: 9435
		private ArrayList _registeredCallDataForEventValidation;

		// Token: 0x040024DC RID: 9436
		private ArrayList _registeredStyleInfo;

		// Token: 0x02000452 RID: 1106
		private class RegisterCallData
		{
			// Token: 0x040024DD RID: 9437
			internal ClientAPIRegisterType Type;

			// Token: 0x040024DE RID: 9438
			internal ScriptKey Key;

			// Token: 0x040024DF RID: 9439
			internal string StringParam1;

			// Token: 0x040024E0 RID: 9440
			internal string StringParam2;

			// Token: 0x040024E1 RID: 9441
			internal string StringParam3;
		}

		// Token: 0x02000453 RID: 1107
		private class PartialCachingCacheEntry
		{
			// Token: 0x040024E2 RID: 9442
			internal string OutputString;

			// Token: 0x040024E3 RID: 9443
			internal string CssStyleString;

			// Token: 0x040024E4 RID: 9444
			internal ArrayList RegisteredClientCalls;
		}
	}
}
