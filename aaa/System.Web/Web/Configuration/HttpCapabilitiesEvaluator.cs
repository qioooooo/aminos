using System;
using System.Collections;
using System.Globalization;
using System.Security;
using System.Text;
using System.Threading;
using System.Web.Caching;
using System.Web.Compilation;

namespace System.Web.Configuration
{
	// Token: 0x020001F3 RID: 499
	internal class HttpCapabilitiesEvaluator
	{
		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001B4B RID: 6987 RVA: 0x0007E44D File Offset: 0x0007D44D
		// (set) Token: 0x06001B4C RID: 6988 RVA: 0x0007E455 File Offset: 0x0007D455
		internal int UserAgentCacheKeyLength
		{
			get
			{
				return this._userAgentCacheKeyLength;
			}
			set
			{
				this._userAgentCacheKeyLength = value;
			}
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x0007E460 File Offset: 0x0007D460
		internal HttpCapabilitiesEvaluator(HttpCapabilitiesEvaluator parent)
		{
			this._cacheKeyPrefix = "e" + Interlocked.Increment(ref HttpCapabilitiesEvaluator._idCounter).ToString(CultureInfo.InvariantCulture);
			if (parent == null)
			{
				this.ClearParent();
			}
			else
			{
				this._rule = parent._rule;
				if (parent._variables == null)
				{
					this._variables = null;
				}
				else
				{
					this._variables = new Hashtable(parent._variables);
				}
				this._cachetime = parent._cachetime;
				this._resultType = parent._resultType;
			}
			this.AddDependency(string.Empty);
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001B4E RID: 6990 RVA: 0x0007E4F6 File Offset: 0x0007D4F6
		internal BrowserCapabilitiesFactoryBase BrowserCapFactory
		{
			get
			{
				return BrowserCapabilitiesCompiler.BrowserCapabilitiesFactory;
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0007E4FD File Offset: 0x0007D4FD
		internal virtual void ClearParent()
		{
			this._rule = null;
			this._cachetime = TimeSpan.FromSeconds(60.0);
			this._variables = new Hashtable();
			this._resultType = typeof(HttpCapabilitiesBase);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0007E535 File Offset: 0x0007D535
		internal virtual void SetCacheTime(int sec)
		{
			this._cachetime = TimeSpan.FromSeconds((double)sec);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0007E544 File Offset: 0x0007D544
		internal virtual void AddDependency(string variable)
		{
			if (variable.Equals("HTTP_USER_AGENT"))
			{
				variable = string.Empty;
			}
			this._variables[variable] = true;
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x0007E56C File Offset: 0x0007D56C
		internal virtual void AddRuleList(ArrayList rulelist)
		{
			if (rulelist.Count == 0)
			{
				return;
			}
			if (this._rule != null)
			{
				rulelist.Insert(0, this._rule);
			}
			this._rule = new CapabilitiesSection(2, null, null, rulelist);
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x0007E59C File Offset: 0x0007D59C
		internal static string GetUserAgent(HttpRequest request)
		{
			string text;
			if (request.ClientTarget.Length > 0)
			{
				text = HttpCapabilitiesEvaluator.GetUserAgentFromClientTarget(request.Context.ConfigurationPath, request.ClientTarget);
			}
			else
			{
				text = request.UserAgent;
			}
			if (text != null && text.Length > 512)
			{
				text = text.Substring(0, 512);
			}
			return text;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0007E5F8 File Offset: 0x0007D5F8
		internal static string GetUserAgentFromClientTarget(VirtualPath configPath, string clientTarget)
		{
			ClientTargetSection clientTarget2 = RuntimeConfig.GetConfig(configPath).ClientTarget;
			string text = null;
			if (clientTarget2.ClientTargets[clientTarget] != null)
			{
				text = clientTarget2.ClientTargets[clientTarget].UserAgent;
			}
			if (text == null)
			{
				throw new HttpException(SR.GetString("Invalid_client_target", new object[] { clientTarget }));
			}
			return text;
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0007E654 File Offset: 0x0007D654
		private void CacheBrowserCapResult(ref HttpCapabilitiesBase result)
		{
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			if (result.Capabilities == null)
			{
				return;
			}
			string text = "z";
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in result.Capabilities.Keys)
			{
				string text2 = (string)obj;
				if (!string.IsNullOrEmpty(text2))
				{
					string text3 = (string)result.Capabilities[text2];
					if (text3 != null)
					{
						stringBuilder.Append(text2);
						stringBuilder.Append("$");
						stringBuilder.Append(text3);
						stringBuilder.Append("$");
					}
				}
			}
			text += stringBuilder.ToString().GetHashCode().ToString(CultureInfo.InvariantCulture);
			HttpCapabilitiesBase httpCapabilitiesBase = cacheInternal.Get(text) as HttpCapabilitiesBase;
			if (httpCapabilitiesBase != null)
			{
				result = httpCapabilitiesBase;
				return;
			}
			cacheInternal.UtcInsert(text, result, null, Cache.NoAbsoluteExpiration, this._cachetime);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0007E764 File Offset: 0x0007D764
		internal HttpCapabilitiesBase Evaluate(HttpRequest request)
		{
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string userAgent = HttpCapabilitiesEvaluator.GetUserAgent(request);
			string text = userAgent;
			if (text != null && text.Length > this.UserAgentCacheKeyLength)
			{
				text = text.Substring(0, this.UserAgentCacheKeyLength);
			}
			bool flag = false;
			string text2 = this._cacheKeyPrefix + text;
			object obj = cacheInternal.Get(text2);
			HttpCapabilitiesBase httpCapabilitiesBase = obj as HttpCapabilitiesBase;
			if (httpCapabilitiesBase != null)
			{
				return httpCapabilitiesBase;
			}
			if (obj == HttpCapabilitiesEvaluator._disableOptimisticCachingSingleton)
			{
				flag = true;
			}
			else
			{
				httpCapabilitiesBase = this.EvaluateFinal(request, true);
				if (httpCapabilitiesBase.UseOptimizedCacheKey)
				{
					this.CacheBrowserCapResult(ref httpCapabilitiesBase);
					cacheInternal.UtcInsert(text2, httpCapabilitiesBase, null, Cache.NoAbsoluteExpiration, this._cachetime);
					return httpCapabilitiesBase;
				}
			}
			IDictionaryEnumerator enumerator = this._variables.GetEnumerator();
			StringBuilder stringBuilder = new StringBuilder(this._cacheKeyPrefix);
			InternalSecurityPermissions.AspNetHostingPermissionLevelLow.Assert();
			while (enumerator.MoveNext())
			{
				string text3 = (string)enumerator.Key;
				string text4;
				if (text3.Length == 0)
				{
					text4 = userAgent;
				}
				else
				{
					text4 = request.ServerVariables[text3];
				}
				if (text4 != null)
				{
					stringBuilder.Append(text4);
				}
			}
			CodeAccessPermission.RevertAssert();
			stringBuilder.Append(BrowserCapabilitiesFactoryBase.GetBrowserCapKey(this.BrowserCapFactory.InternalGetMatchedHeaders(), request));
			string text5 = stringBuilder.ToString();
			if (userAgent == null || flag)
			{
				httpCapabilitiesBase = cacheInternal.Get(text5) as HttpCapabilitiesBase;
				if (httpCapabilitiesBase != null)
				{
					return httpCapabilitiesBase;
				}
			}
			httpCapabilitiesBase = this.EvaluateFinal(request, false);
			this.CacheBrowserCapResult(ref httpCapabilitiesBase);
			cacheInternal.UtcInsert(text5, httpCapabilitiesBase, null, Cache.NoAbsoluteExpiration, this._cachetime);
			if (text2 != null)
			{
				cacheInternal.UtcInsert(text2, HttpCapabilitiesEvaluator._disableOptimisticCachingSingleton, null, Cache.NoAbsoluteExpiration, this._cachetime);
			}
			return httpCapabilitiesBase;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0007E8F4 File Offset: 0x0007D8F4
		internal HttpCapabilitiesBase EvaluateFinal(HttpRequest request, bool onlyEvaluateUserAgent)
		{
			HttpBrowserCapabilities httpBrowserCapabilities = this.BrowserCapFactory.GetHttpBrowserCapabilities(request);
			CapabilitiesState capabilitiesState = new CapabilitiesState(request, httpBrowserCapabilities.Capabilities);
			if (onlyEvaluateUserAgent)
			{
				capabilitiesState.EvaluateOnlyUserAgent = true;
			}
			if (this._rule != null)
			{
				string text = httpBrowserCapabilities["isMobileDevice"];
				httpBrowserCapabilities.Capabilities["isMobileDevice"] = null;
				this._rule.Evaluate(capabilitiesState);
				string text2 = httpBrowserCapabilities["isMobileDevice"];
				if (text2 == null)
				{
					httpBrowserCapabilities.Capabilities["isMobileDevice"] = text;
				}
				else if (text2.Equals("true"))
				{
					httpBrowserCapabilities.DisableOptimizedCacheKey();
				}
			}
			HttpCapabilitiesBase httpCapabilitiesBase = (HttpCapabilitiesBase)HttpRuntime.CreateNonPublicInstance(this._resultType);
			httpCapabilitiesBase.InitInternal(httpBrowserCapabilities);
			return httpCapabilitiesBase;
		}

		// Token: 0x04001843 RID: 6211
		private const string _isMobileDeviceCapKey = "isMobileDevice";

		// Token: 0x04001844 RID: 6212
		internal CapabilitiesRule _rule;

		// Token: 0x04001845 RID: 6213
		internal Hashtable _variables;

		// Token: 0x04001846 RID: 6214
		internal Type _resultType;

		// Token: 0x04001847 RID: 6215
		internal TimeSpan _cachetime;

		// Token: 0x04001848 RID: 6216
		internal string _cacheKeyPrefix;

		// Token: 0x04001849 RID: 6217
		private int _userAgentCacheKeyLength;

		// Token: 0x0400184A RID: 6218
		private static int _idCounter;

		// Token: 0x0400184B RID: 6219
		private static object _disableOptimisticCachingSingleton = new object();
	}
}
