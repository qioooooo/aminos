using System;
using System.Collections;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200005F RID: 95
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCachePolicy
	{
		// Token: 0x06000368 RID: 872 RVA: 0x0000F84D File Offset: 0x0000E84D
		internal HttpCachePolicy()
		{
			this._varyByContentEncodings = new HttpCacheVaryByContentEncodings();
			this._varyByHeaders = new HttpCacheVaryByHeaders();
			this._varyByParams = new HttpCacheVaryByParams();
			this.Reset();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000F87C File Offset: 0x0000E87C
		internal void Reset()
		{
			this._varyByContentEncodings.Reset();
			this._varyByHeaders.Reset();
			this._varyByParams.Reset();
			this._isModified = false;
			this._hasSetCookieHeader = false;
			this._noServerCaching = false;
			this._cacheExtension = null;
			this._noTransforms = false;
			this._ignoreRangeRequests = false;
			this._varyByCustom = null;
			this._cacheability = (HttpCacheability)6;
			this._noStore = false;
			this._privateFields = null;
			this._noCacheFields = null;
			this._utcExpires = DateTime.MinValue;
			this._isExpiresSet = false;
			this._maxAge = TimeSpan.Zero;
			this._isMaxAgeSet = false;
			this._proxyMaxAge = TimeSpan.Zero;
			this._isProxyMaxAgeSet = false;
			this._slidingExpiration = -1;
			this._slidingDelta = TimeSpan.Zero;
			this._utcTimestampCreated = DateTime.MinValue;
			this._utcTimestampRequest = DateTime.MinValue;
			this._validUntilExpires = -1;
			this._allowInHistory = -1;
			this._revalidation = HttpCacheRevalidation.None;
			this._utcLastModified = DateTime.MinValue;
			this._isLastModifiedSet = false;
			this._etag = null;
			this._generateLastModifiedFromFiles = false;
			this._generateEtagFromFiles = false;
			this._validationCallbackInfo = null;
			this._useCachedHeaders = false;
			this._headerCacheControl = null;
			this._headerPragma = null;
			this._headerExpires = null;
			this._headerLastModified = null;
			this._headerEtag = null;
			this._headerVaryBy = null;
			this._noMaxAgeInCacheControl = false;
			this._hasUserProvidedDependencies = false;
			this._omitVaryStar = -1;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000F9E0 File Offset: 0x0000E9E0
		internal void ResetFromHttpCachePolicySettings(HttpCachePolicySettings settings, DateTime utcTimestampRequest)
		{
			this._utcTimestampRequest = utcTimestampRequest;
			this._varyByContentEncodings.ResetFromContentEncodings(settings.VaryByContentEncodings);
			this._varyByHeaders.ResetFromHeaders(settings.VaryByHeaders);
			this._varyByParams.ResetFromParams(settings.VaryByParams);
			this._isModified = settings.IsModified;
			this._hasSetCookieHeader = settings.hasSetCookieHeader;
			this._noServerCaching = settings.NoServerCaching;
			this._cacheExtension = settings.CacheExtension;
			this._noTransforms = settings.NoTransforms;
			this._ignoreRangeRequests = settings.IgnoreRangeRequests;
			this._varyByCustom = settings.VaryByCustom;
			this._cacheability = settings.CacheabilityInternal;
			this._noStore = settings.NoStore;
			this._utcExpires = settings.UtcExpires;
			this._isExpiresSet = settings.IsExpiresSet;
			this._maxAge = settings.MaxAge;
			this._isMaxAgeSet = settings.IsMaxAgeSet;
			this._proxyMaxAge = settings.ProxyMaxAge;
			this._isProxyMaxAgeSet = settings.IsProxyMaxAgeSet;
			this._slidingExpiration = settings.SlidingExpirationInternal;
			this._slidingDelta = settings.SlidingDelta;
			this._utcTimestampCreated = settings.UtcTimestampCreated;
			this._validUntilExpires = settings.ValidUntilExpiresInternal;
			this._allowInHistory = settings.AllowInHistoryInternal;
			this._revalidation = settings.Revalidation;
			this._utcLastModified = settings.UtcLastModified;
			this._isLastModifiedSet = settings.IsLastModifiedSet;
			this._etag = settings.ETag;
			this._generateLastModifiedFromFiles = settings.GenerateLastModifiedFromFiles;
			this._generateEtagFromFiles = settings.GenerateEtagFromFiles;
			this._omitVaryStar = settings.OmitVaryStarInternal;
			this._hasUserProvidedDependencies = settings.HasUserProvidedDependencies;
			this._useCachedHeaders = true;
			this._headerCacheControl = settings.HeaderCacheControl;
			this._headerPragma = settings.HeaderPragma;
			this._headerExpires = settings.HeaderExpires;
			this._headerLastModified = settings.HeaderLastModified;
			this._headerEtag = settings.HeaderEtag;
			this._headerVaryBy = settings.HeaderVaryBy;
			this._noMaxAgeInCacheControl = false;
			string[] array = settings.PrivateFields;
			if (array != null)
			{
				this._privateFields = new HttpDictionary();
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					this._privateFields.SetValue(array[i], array[i]);
					i++;
				}
			}
			array = settings.NoCacheFields;
			if (array != null)
			{
				this._noCacheFields = new HttpDictionary();
				int i = 0;
				int num = array.Length;
				while (i < num)
				{
					this._noCacheFields.SetValue(array[i], array[i]);
					i++;
				}
			}
			if (settings.ValidationCallbackInfo != null)
			{
				this._validationCallbackInfo = new ArrayList();
				int i = 0;
				int num = settings.ValidationCallbackInfo.Length;
				while (i < num)
				{
					this._validationCallbackInfo.Add(new ValidationCallbackInfo(settings.ValidationCallbackInfo[i].handler, settings.ValidationCallbackInfo[i].data));
					i++;
				}
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000FC8C File Offset: 0x0000EC8C
		internal bool IsModified()
		{
			return this._isModified || this._varyByContentEncodings.IsModified() || this._varyByHeaders.IsModified() || this._varyByParams.IsModified();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000FCBD File Offset: 0x0000ECBD
		private void Dirtied()
		{
			this._isModified = true;
			this._useCachedHeaders = false;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000FCCD File Offset: 0x0000ECCD
		internal static void AppendValueToHeader(StringBuilder s, string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (s.Length > 0)
				{
					s.Append(", ");
				}
				s.Append(value);
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000FCF4 File Offset: 0x0000ECF4
		private void UpdateFromDependencies(HttpResponse response)
		{
			StringBuilder stringBuilder = null;
			if (!this._generateLastModifiedFromFiles && !this._generateEtagFromFiles)
			{
				return;
			}
			string[] fileDependencies = response.GetFileDependencies();
			string[] cacheItemDependencies = response.GetCacheItemDependencies();
			CacheDependency cacheDependency = response.GetCacheDependency();
			CacheDependency virtualPathDependency = response.GetVirtualPathDependency();
			if (fileDependencies == null && cacheItemDependencies == null && cacheDependency == null && virtualPathDependency == null)
			{
				return;
			}
			DateTime dateTime = this._utcLastModified;
			if (this._generateEtagFromFiles)
			{
				stringBuilder = new StringBuilder(HttpRuntime.AppDomainIdInternal);
			}
			if (fileDependencies != null)
			{
				FileChangesMonitor fileChangesMonitor = HttpRuntime.FileChangesMonitor;
				foreach (string text in fileDependencies)
				{
					FileAttributesData fileAttributesData = fileChangesMonitor.GetFileAttributes(text);
					if (fileAttributesData == null)
					{
						fileAttributesData = FileAttributesData.NonExistantAttributesData;
					}
					if (dateTime < fileAttributesData.UtcLastWriteTime)
					{
						dateTime = fileAttributesData.UtcLastWriteTime;
					}
					if (this._generateEtagFromFiles)
					{
						stringBuilder.Append(text);
						stringBuilder.Append(fileAttributesData.UtcLastWriteTime.Ticks.ToString(CultureInfo.InvariantCulture));
						stringBuilder.Append(fileAttributesData.FileSize.ToString(CultureInfo.InvariantCulture));
					}
				}
			}
			if (cacheItemDependencies != null)
			{
				Cache cache = HttpRuntime.Cache;
				foreach (string text2 in cacheItemDependencies)
				{
					CacheEntry cacheEntry = (CacheEntry)cache.Get(text2, CacheGetOptions.ReturnCacheEntry);
					if (cacheEntry != null)
					{
						DateTime utcCreated = cacheEntry.UtcCreated;
						if (dateTime < utcCreated)
						{
							dateTime = utcCreated;
						}
						if (this._generateEtagFromFiles)
						{
							stringBuilder.Append(text2);
							stringBuilder.Append(utcCreated.Ticks.ToString(CultureInfo.InvariantCulture));
						}
					}
				}
			}
			if (cacheDependency != null)
			{
				if (dateTime < cacheDependency.UtcLastModified)
				{
					dateTime = cacheDependency.UtcLastModified;
				}
				if (this._generateEtagFromFiles)
				{
					string uniqueID = cacheDependency.GetUniqueID();
					if (uniqueID == null)
					{
						throw new HttpException(SR.GetString("No_UniqueId_Cache_Dependency"));
					}
					stringBuilder.Append(uniqueID);
				}
			}
			if (virtualPathDependency != null)
			{
				if (dateTime < virtualPathDependency.UtcLastModified)
				{
					dateTime = virtualPathDependency.UtcLastModified;
				}
				if (this._generateEtagFromFiles)
				{
					string uniqueID2 = virtualPathDependency.GetUniqueID();
					if (uniqueID2 == null)
					{
						throw new HttpException(SR.GetString("No_UniqueId_Cache_Dependency"));
					}
					stringBuilder.Append(uniqueID2);
				}
			}
			DateTime utcNow = DateTime.UtcNow;
			if (dateTime > utcNow)
			{
				dateTime = utcNow;
			}
			if (this._generateLastModifiedFromFiles)
			{
				this.UtcSetLastModified(dateTime);
			}
			if (this._generateEtagFromFiles)
			{
				stringBuilder.Append("+LM");
				stringBuilder.Append(dateTime.Ticks.ToString(CultureInfo.InvariantCulture));
				this._etag = MachineKeySection.HashAndBase64EncodeString(stringBuilder.ToString());
				this._etag = "\"" + this._etag + "\"";
			}
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000FFAC File Offset: 0x0000EFAC
		private void UpdateCachedHeaders(HttpResponse response)
		{
			if (this._useCachedHeaders)
			{
				return;
			}
			if (this._utcTimestampCreated == DateTime.MinValue)
			{
				this._utcTimestampCreated = (this._utcTimestampRequest = response.Context.UtcTimestamp);
			}
			if (this._slidingExpiration != 1)
			{
				this._slidingDelta = TimeSpan.Zero;
			}
			else if (this._isMaxAgeSet)
			{
				this._slidingDelta = this._maxAge;
			}
			else if (this._isExpiresSet)
			{
				this._slidingDelta = this._utcExpires - this._utcTimestampCreated;
			}
			else
			{
				this._slidingDelta = TimeSpan.Zero;
			}
			this._headerCacheControl = null;
			this._headerPragma = null;
			this._headerExpires = null;
			this._headerLastModified = null;
			this._headerEtag = null;
			this._headerVaryBy = null;
			this.UpdateFromDependencies(response);
			StringBuilder stringBuilder = new StringBuilder();
			HttpCacheability httpCacheability;
			if (this._cacheability == (HttpCacheability)6)
			{
				httpCacheability = HttpCacheability.Private;
			}
			else
			{
				httpCacheability = this._cacheability;
			}
			HttpCachePolicy.AppendValueToHeader(stringBuilder, HttpCachePolicy.s_cacheabilityTokens[(int)httpCacheability]);
			if (httpCacheability == HttpCacheability.Public && this._privateFields != null)
			{
				HttpCachePolicy.AppendValueToHeader(stringBuilder, "private=\"");
				stringBuilder.Append(this._privateFields.GetKey(0));
				int i = 1;
				int num = this._privateFields.Size;
				while (i < num)
				{
					HttpCachePolicy.AppendValueToHeader(stringBuilder, this._privateFields.GetKey(i));
					i++;
				}
				stringBuilder.Append('"');
			}
			if (httpCacheability != HttpCacheability.NoCache && httpCacheability != HttpCacheability.Server && this._noCacheFields != null)
			{
				HttpCachePolicy.AppendValueToHeader(stringBuilder, "no-cache=\"");
				stringBuilder.Append(this._noCacheFields.GetKey(0));
				int i = 1;
				int num = this._noCacheFields.Size;
				while (i < num)
				{
					HttpCachePolicy.AppendValueToHeader(stringBuilder, this._noCacheFields.GetKey(i));
					i++;
				}
				stringBuilder.Append('"');
			}
			if (this._noStore)
			{
				HttpCachePolicy.AppendValueToHeader(stringBuilder, "no-store");
			}
			HttpCachePolicy.AppendValueToHeader(stringBuilder, HttpCachePolicy.s_revalidationTokens[(int)this._revalidation]);
			if (this._noTransforms)
			{
				HttpCachePolicy.AppendValueToHeader(stringBuilder, "no-transform");
			}
			if (this._cacheExtension != null)
			{
				HttpCachePolicy.AppendValueToHeader(stringBuilder, this._cacheExtension);
			}
			if (this._slidingExpiration == 1 && httpCacheability != HttpCacheability.NoCache && httpCacheability != HttpCacheability.Server)
			{
				if (this._isMaxAgeSet && !this._noMaxAgeInCacheControl)
				{
					HttpCachePolicy.AppendValueToHeader(stringBuilder, "max-age=" + ((long)this._maxAge.TotalSeconds).ToString(CultureInfo.InvariantCulture));
				}
				if (this._isProxyMaxAgeSet && !this._noMaxAgeInCacheControl)
				{
					HttpCachePolicy.AppendValueToHeader(stringBuilder, "s-maxage=" + ((long)this._proxyMaxAge.TotalSeconds).ToString(CultureInfo.InvariantCulture));
				}
			}
			if (stringBuilder.Length > 0)
			{
				this._headerCacheControl = new HttpResponseHeader(0, stringBuilder.ToString());
			}
			if (httpCacheability == HttpCacheability.NoCache || httpCacheability == HttpCacheability.Server)
			{
				if (HttpCachePolicy.s_headerPragmaNoCache == null)
				{
					HttpCachePolicy.s_headerPragmaNoCache = new HttpResponseHeader(4, "no-cache");
				}
				this._headerPragma = HttpCachePolicy.s_headerPragmaNoCache;
				if (this._allowInHistory != 1)
				{
					if (HttpCachePolicy.s_headerExpiresMinus1 == null)
					{
						HttpCachePolicy.s_headerExpiresMinus1 = new HttpResponseHeader(18, "-1");
					}
					this._headerExpires = HttpCachePolicy.s_headerExpiresMinus1;
				}
			}
			else
			{
				if (this._isExpiresSet && this._slidingExpiration != 1)
				{
					string text = HttpUtility.FormatHttpDateTimeUtc(this._utcExpires);
					this._headerExpires = new HttpResponseHeader(18, text);
				}
				if (this._isLastModifiedSet)
				{
					string text2 = HttpUtility.FormatHttpDateTimeUtc(this._utcLastModified);
					this._headerLastModified = new HttpResponseHeader(19, text2);
				}
				if (httpCacheability != HttpCacheability.Private)
				{
					if (this._etag != null)
					{
						this._headerEtag = new HttpResponseHeader(22, this._etag);
					}
					string text3 = null;
					bool flag;
					if (this._omitVaryStar != -1)
					{
						flag = this._omitVaryStar == 1;
					}
					else
					{
						RuntimeConfig lkgconfig = RuntimeConfig.GetLKGConfig(response.Context);
						OutputCacheSection outputCache = lkgconfig.OutputCache;
						flag = outputCache != null && outputCache.OmitVaryStar;
					}
					if (!flag && (httpCacheability == HttpCacheability.ServerAndPrivate || this._varyByCustom != null || (this._varyByParams.IsModified() && !this._varyByParams.IgnoreParams)))
					{
						text3 = "*";
					}
					if (text3 == null)
					{
						text3 = this._varyByHeaders.ToHeaderString();
					}
					if (text3 != null)
					{
						this._headerVaryBy = new HttpResponseHeader(28, text3);
					}
				}
			}
			this._useCachedHeaders = true;
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000103BC File Offset: 0x0000F3BC
		internal void GetHeaders(ArrayList headers, HttpResponse response)
		{
			this.UpdateCachedHeaders(response);
			HttpResponseHeader httpResponseHeader = this._headerExpires;
			HttpResponseHeader httpResponseHeader2 = this._headerCacheControl;
			if (this._cacheability != HttpCacheability.NoCache && this._cacheability != HttpCacheability.Server)
			{
				if (this._slidingExpiration == 1)
				{
					if (this._isExpiresSet)
					{
						DateTime dateTime = this._utcTimestampRequest + this._slidingDelta;
						string text = HttpUtility.FormatHttpDateTimeUtc(dateTime);
						httpResponseHeader = new HttpResponseHeader(18, text);
					}
				}
				else if (this._isMaxAgeSet || this._isProxyMaxAgeSet)
				{
					StringBuilder stringBuilder;
					if (httpResponseHeader2 != null)
					{
						stringBuilder = new StringBuilder(httpResponseHeader2.Value);
					}
					else
					{
						stringBuilder = new StringBuilder();
					}
					TimeSpan timeSpan = this._utcTimestampRequest - this._utcTimestampCreated;
					if (this._isMaxAgeSet)
					{
						TimeSpan timeSpan2 = this._maxAge - timeSpan;
						if (timeSpan2 < TimeSpan.Zero)
						{
							timeSpan2 = TimeSpan.Zero;
						}
						if (!this._noMaxAgeInCacheControl)
						{
							HttpCachePolicy.AppendValueToHeader(stringBuilder, "max-age=" + ((long)timeSpan2.TotalSeconds).ToString(CultureInfo.InvariantCulture));
						}
					}
					if (this._isProxyMaxAgeSet)
					{
						TimeSpan timeSpan3 = this._proxyMaxAge - timeSpan;
						if (timeSpan3 < TimeSpan.Zero)
						{
							timeSpan3 = TimeSpan.Zero;
						}
						if (!this._noMaxAgeInCacheControl)
						{
							HttpCachePolicy.AppendValueToHeader(stringBuilder, "s-maxage=" + ((long)timeSpan3.TotalSeconds).ToString(CultureInfo.InvariantCulture));
						}
					}
					httpResponseHeader2 = new HttpResponseHeader(0, stringBuilder.ToString());
				}
			}
			if (httpResponseHeader2 != null)
			{
				headers.Add(httpResponseHeader2);
			}
			if (this._headerPragma != null)
			{
				headers.Add(this._headerPragma);
			}
			if (httpResponseHeader != null)
			{
				headers.Add(httpResponseHeader);
			}
			if (this._headerLastModified != null)
			{
				headers.Add(this._headerLastModified);
			}
			if (this._headerEtag != null)
			{
				headers.Add(this._headerEtag);
			}
			if (this._headerVaryBy != null)
			{
				headers.Add(this._headerVaryBy);
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000105A0 File Offset: 0x0000F5A0
		internal HttpCachePolicySettings GetCurrentSettings(HttpResponse response)
		{
			this.UpdateCachedHeaders(response);
			string[] contentEncodings = this._varyByContentEncodings.GetContentEncodings();
			string[] headers = this._varyByHeaders.GetHeaders();
			string[] @params = this._varyByParams.GetParams();
			string[] array;
			if (this._privateFields != null)
			{
				array = this._privateFields.GetAllKeys();
			}
			else
			{
				array = null;
			}
			string[] array2;
			if (this._noCacheFields != null)
			{
				array2 = this._noCacheFields.GetAllKeys();
			}
			else
			{
				array2 = null;
			}
			ValidationCallbackInfo[] array3;
			if (this._validationCallbackInfo != null)
			{
				array3 = new ValidationCallbackInfo[this._validationCallbackInfo.Count];
				this._validationCallbackInfo.CopyTo(0, array3, 0, this._validationCallbackInfo.Count);
			}
			else
			{
				array3 = null;
			}
			return new HttpCachePolicySettings(this._isModified, array3, this._hasSetCookieHeader, this._noServerCaching, this._cacheExtension, this._noTransforms, this._ignoreRangeRequests, contentEncodings, headers, @params, this._varyByCustom, this._cacheability, this._noStore, array, array2, this._utcExpires, this._isExpiresSet, this._maxAge, this._isMaxAgeSet, this._proxyMaxAge, this._isProxyMaxAgeSet, this._slidingExpiration, this._slidingDelta, this._utcTimestampCreated, this._validUntilExpires, this._allowInHistory, this._revalidation, this._utcLastModified, this._isLastModifiedSet, this._etag, this._generateLastModifiedFromFiles, this._generateEtagFromFiles, this._omitVaryStar, this._headerCacheControl, this._headerPragma, this._headerExpires, this._headerLastModified, this._headerEtag, this._headerVaryBy, this._hasUserProvidedDependencies);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001071C File Offset: 0x0000F71C
		internal bool HasValidationPolicy()
		{
			return this._generateLastModifiedFromFiles || this._generateEtagFromFiles || this._validationCallbackInfo != null || (this._validUntilExpires == 1 && this._slidingExpiration != 1);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0001074F File Offset: 0x0000F74F
		internal bool HasExpirationPolicy()
		{
			return this._slidingExpiration != 1 && (this._isExpiresSet || this._isMaxAgeSet);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001076C File Offset: 0x0000F76C
		internal bool IsKernelCacheable(HttpRequest request, bool enableKernelCacheForVaryByStar)
		{
			return this._cacheability == HttpCacheability.Public && !this._hasUserProvidedDependencies && !this._hasSetCookieHeader && !this._noServerCaching && this.HasExpirationPolicy() && this._cacheExtension == null && !this._varyByContentEncodings.IsModified() && !this._varyByHeaders.IsModified() && (!this._varyByParams.IsModified() || this._varyByParams.IgnoreParams || (this._varyByParams.IsVaryByStar && enableKernelCacheForVaryByStar)) && !this._noStore && this._varyByCustom == null && this._privateFields == null && this._noCacheFields == null && this._validationCallbackInfo == null && request != null && request.HttpVerb == HttpVerb.GET;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00010835 File Offset: 0x0000F835
		internal bool IsVaryByStar
		{
			get
			{
				return this._varyByParams.IsVaryByStar;
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00010844 File Offset: 0x0000F844
		internal DateTime UtcGetAbsoluteExpiration()
		{
			DateTime dateTime = Cache.NoAbsoluteExpiration;
			if (this._slidingExpiration != 1)
			{
				if (this._isMaxAgeSet)
				{
					dateTime = this._utcTimestampCreated + this._maxAge;
				}
				else if (this._isExpiresSet)
				{
					dateTime = this._utcExpires;
				}
			}
			return dateTime;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001088C File Offset: 0x0000F88C
		public void SetNoServerCaching()
		{
			this.Dirtied();
			this._noServerCaching = true;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001089B File Offset: 0x0000F89B
		internal bool GetNoServerCaching()
		{
			return this._noServerCaching;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000108A3 File Offset: 0x0000F8A3
		internal void SetHasSetCookieHeader()
		{
			this.Dirtied();
			this._hasSetCookieHeader = true;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000108B2 File Offset: 0x0000F8B2
		public void SetVaryByCustom(string custom)
		{
			if (custom == null)
			{
				throw new ArgumentNullException("custom");
			}
			if (this._varyByCustom != null)
			{
				throw new InvalidOperationException(SR.GetString("VaryByCustom_already_set"));
			}
			this.Dirtied();
			this._varyByCustom = custom;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000108E7 File Offset: 0x0000F8E7
		public void AppendCacheExtension(string extension)
		{
			if (extension == null)
			{
				throw new ArgumentNullException("extension");
			}
			this.Dirtied();
			if (this._cacheExtension == null)
			{
				this._cacheExtension = extension;
				return;
			}
			this._cacheExtension = this._cacheExtension + ", " + extension;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00010924 File Offset: 0x0000F924
		public void SetNoTransforms()
		{
			this.Dirtied();
			this._noTransforms = true;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00010933 File Offset: 0x0000F933
		internal void SetIgnoreRangeRequests()
		{
			this.Dirtied();
			this._ignoreRangeRequests = true;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00010942 File Offset: 0x0000F942
		public HttpCacheVaryByContentEncodings VaryByContentEncodings
		{
			get
			{
				return this._varyByContentEncodings;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600037F RID: 895 RVA: 0x0001094A File Offset: 0x0000F94A
		public HttpCacheVaryByHeaders VaryByHeaders
		{
			get
			{
				return this._varyByHeaders;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00010952 File Offset: 0x0000F952
		public HttpCacheVaryByParams VaryByParams
		{
			get
			{
				return this._varyByParams;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001095A File Offset: 0x0000F95A
		public void SetCacheability(HttpCacheability cacheability)
		{
			if (cacheability < HttpCacheability.NoCache || HttpCacheability.ServerAndPrivate < cacheability)
			{
				throw new ArgumentOutOfRangeException("cacheability");
			}
			if (HttpCachePolicy.s_cacheabilityValues[(int)cacheability] < HttpCachePolicy.s_cacheabilityValues[(int)this._cacheability])
			{
				this.Dirtied();
				this._cacheability = cacheability;
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00010991 File Offset: 0x0000F991
		internal HttpCacheability GetCacheability()
		{
			return this._cacheability;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001099C File Offset: 0x0000F99C
		public void SetCacheability(HttpCacheability cacheability, string field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			switch (cacheability)
			{
			case HttpCacheability.NoCache:
				if (this._noCacheFields == null)
				{
					this._noCacheFields = new HttpDictionary();
				}
				this._noCacheFields.SetValue(field, field);
				break;
			case HttpCacheability.Private:
				if (this._privateFields == null)
				{
					this._privateFields = new HttpDictionary();
				}
				this._privateFields.SetValue(field, field);
				break;
			default:
				throw new ArgumentException(SR.GetString("Cacheability_for_field_must_be_private_or_nocache"), "cacheability");
			}
			this.Dirtied();
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00010A2A File Offset: 0x0000FA2A
		public void SetNoStore()
		{
			this.Dirtied();
			this._noStore = true;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00010A39 File Offset: 0x0000FA39
		internal void SetDependencies(bool hasUserProvidedDependencies)
		{
			this.Dirtied();
			this._hasUserProvidedDependencies = hasUserProvidedDependencies;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00010A48 File Offset: 0x0000FA48
		public void SetExpires(DateTime date)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(date);
			DateTime utcNow = DateTime.UtcNow;
			if (dateTime - utcNow > HttpCachePolicy.s_oneYear)
			{
				dateTime = utcNow + HttpCachePolicy.s_oneYear;
			}
			if (!this._isExpiresSet || dateTime < this._utcExpires)
			{
				this.Dirtied();
				this._utcExpires = dateTime;
				this._isExpiresSet = true;
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00010AAC File Offset: 0x0000FAAC
		public void SetMaxAge(TimeSpan delta)
		{
			if (delta < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("delta");
			}
			if (HttpCachePolicy.s_oneYear < delta)
			{
				delta = HttpCachePolicy.s_oneYear;
			}
			if (!this._isMaxAgeSet || delta < this._maxAge)
			{
				this.Dirtied();
				this._maxAge = delta;
				this._isMaxAgeSet = true;
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00010B0F File Offset: 0x0000FB0F
		internal void SetNoMaxAgeInCacheControl()
		{
			this._noMaxAgeInCacheControl = true;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00010B18 File Offset: 0x0000FB18
		public void SetProxyMaxAge(TimeSpan delta)
		{
			if (delta < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("delta");
			}
			if (!this._isProxyMaxAgeSet || delta < this._proxyMaxAge)
			{
				this.Dirtied();
				this._proxyMaxAge = delta;
				this._isProxyMaxAgeSet = true;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00010B67 File Offset: 0x0000FB67
		public void SetSlidingExpiration(bool slide)
		{
			if (this._slidingExpiration == -1 || this._slidingExpiration == 1)
			{
				this.Dirtied();
				this._slidingExpiration = (slide ? 1 : 0);
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00010B8E File Offset: 0x0000FB8E
		public void SetValidUntilExpires(bool validUntilExpires)
		{
			if (this._validUntilExpires == -1 || this._validUntilExpires == 1)
			{
				this.Dirtied();
				this._validUntilExpires = (validUntilExpires ? 1 : 0);
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00010BB5 File Offset: 0x0000FBB5
		public void SetAllowResponseInBrowserHistory(bool allow)
		{
			if (this._allowInHistory == -1 || this._allowInHistory == 1)
			{
				this.Dirtied();
				this._allowInHistory = (allow ? 1 : 0);
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00010BDC File Offset: 0x0000FBDC
		public void SetRevalidation(HttpCacheRevalidation revalidation)
		{
			if (revalidation < HttpCacheRevalidation.AllCaches || HttpCacheRevalidation.None < revalidation)
			{
				throw new ArgumentOutOfRangeException("revalidation");
			}
			if (revalidation < this._revalidation)
			{
				this.Dirtied();
				this._revalidation = revalidation;
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00010C08 File Offset: 0x0000FC08
		public void SetETag(string etag)
		{
			if (etag == null)
			{
				throw new ArgumentNullException("etag");
			}
			if (this._etag != null)
			{
				throw new InvalidOperationException(SR.GetString("Etag_already_set"));
			}
			if (this._generateEtagFromFiles)
			{
				throw new InvalidOperationException(SR.GetString("Cant_both_set_and_generate_Etag"));
			}
			this.Dirtied();
			this._etag = etag;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00010C60 File Offset: 0x0000FC60
		public void SetLastModified(DateTime date)
		{
			DateTime dateTime = DateTimeUtil.ConvertToUniversalTime(date);
			this.UtcSetLastModified(dateTime);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00010C7C File Offset: 0x0000FC7C
		private void UtcSetLastModified(DateTime utcDate)
		{
			utcDate = new DateTime(utcDate.Ticks - utcDate.Ticks % 10000000L);
			if (utcDate > DateTime.UtcNow)
			{
				throw new ArgumentOutOfRangeException("utcDate");
			}
			if (!this._isLastModifiedSet || utcDate > this._utcLastModified)
			{
				this.Dirtied();
				this._utcLastModified = utcDate;
				this._isLastModifiedSet = true;
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00010CE8 File Offset: 0x0000FCE8
		public void SetLastModifiedFromFileDependencies()
		{
			this.Dirtied();
			this._generateLastModifiedFromFiles = true;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00010CF7 File Offset: 0x0000FCF7
		public void SetETagFromFileDependencies()
		{
			if (this._etag != null)
			{
				throw new InvalidOperationException(SR.GetString("Cant_both_set_and_generate_Etag"));
			}
			this.Dirtied();
			this._generateEtagFromFiles = true;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00010D1E File Offset: 0x0000FD1E
		public void SetOmitVaryStar(bool omit)
		{
			this.Dirtied();
			if (this._omitVaryStar == -1 || this._omitVaryStar == 1)
			{
				this.Dirtied();
				this._omitVaryStar = (omit ? 1 : 0);
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010D4B File Offset: 0x0000FD4B
		public void AddValidationCallback(HttpCacheValidateHandler handler, object data)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			this.Dirtied();
			if (this._validationCallbackInfo == null)
			{
				this._validationCallbackInfo = new ArrayList();
			}
			this._validationCallbackInfo.Add(new ValidationCallbackInfo(handler, data));
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00010DA4 File Offset: 0x0000FDA4
		// Note: this type is marked as 'beforefieldinit'.
		static HttpCachePolicy()
		{
			string[] array = new string[7];
			array[1] = "no-cache";
			array[2] = "private";
			array[3] = "no-cache";
			array[4] = "public";
			array[5] = "private";
			HttpCachePolicy.s_cacheabilityTokens = array;
			string[] array2 = new string[4];
			array2[1] = "must-revalidate";
			array2[2] = "proxy-revalidate";
			HttpCachePolicy.s_revalidationTokens = array2;
			HttpCachePolicy.s_cacheabilityValues = new int[] { -1, 0, 2, 1, 4, 3, 100 };
		}

		// Token: 0x04000F97 RID: 3991
		private static TimeSpan s_oneYear = new TimeSpan(315360000000000L);

		// Token: 0x04000F98 RID: 3992
		private static HttpResponseHeader s_headerPragmaNoCache;

		// Token: 0x04000F99 RID: 3993
		private static HttpResponseHeader s_headerExpiresMinus1;

		// Token: 0x04000F9A RID: 3994
		private bool _isModified;

		// Token: 0x04000F9B RID: 3995
		private bool _hasSetCookieHeader;

		// Token: 0x04000F9C RID: 3996
		private bool _noServerCaching;

		// Token: 0x04000F9D RID: 3997
		private string _cacheExtension;

		// Token: 0x04000F9E RID: 3998
		private bool _noTransforms;

		// Token: 0x04000F9F RID: 3999
		private bool _ignoreRangeRequests;

		// Token: 0x04000FA0 RID: 4000
		private HttpCacheVaryByContentEncodings _varyByContentEncodings;

		// Token: 0x04000FA1 RID: 4001
		private HttpCacheVaryByHeaders _varyByHeaders;

		// Token: 0x04000FA2 RID: 4002
		private HttpCacheVaryByParams _varyByParams;

		// Token: 0x04000FA3 RID: 4003
		private string _varyByCustom;

		// Token: 0x04000FA4 RID: 4004
		private HttpCacheability _cacheability;

		// Token: 0x04000FA5 RID: 4005
		private bool _noStore;

		// Token: 0x04000FA6 RID: 4006
		private HttpDictionary _privateFields;

		// Token: 0x04000FA7 RID: 4007
		private HttpDictionary _noCacheFields;

		// Token: 0x04000FA8 RID: 4008
		private DateTime _utcExpires;

		// Token: 0x04000FA9 RID: 4009
		private bool _isExpiresSet;

		// Token: 0x04000FAA RID: 4010
		private TimeSpan _maxAge;

		// Token: 0x04000FAB RID: 4011
		private bool _isMaxAgeSet;

		// Token: 0x04000FAC RID: 4012
		private TimeSpan _proxyMaxAge;

		// Token: 0x04000FAD RID: 4013
		private bool _isProxyMaxAgeSet;

		// Token: 0x04000FAE RID: 4014
		private int _slidingExpiration;

		// Token: 0x04000FAF RID: 4015
		private DateTime _utcTimestampCreated;

		// Token: 0x04000FB0 RID: 4016
		private TimeSpan _slidingDelta;

		// Token: 0x04000FB1 RID: 4017
		private DateTime _utcTimestampRequest;

		// Token: 0x04000FB2 RID: 4018
		private int _validUntilExpires;

		// Token: 0x04000FB3 RID: 4019
		private int _allowInHistory;

		// Token: 0x04000FB4 RID: 4020
		private HttpCacheRevalidation _revalidation;

		// Token: 0x04000FB5 RID: 4021
		private DateTime _utcLastModified;

		// Token: 0x04000FB6 RID: 4022
		private bool _isLastModifiedSet;

		// Token: 0x04000FB7 RID: 4023
		private string _etag;

		// Token: 0x04000FB8 RID: 4024
		private bool _generateLastModifiedFromFiles;

		// Token: 0x04000FB9 RID: 4025
		private bool _generateEtagFromFiles;

		// Token: 0x04000FBA RID: 4026
		private int _omitVaryStar;

		// Token: 0x04000FBB RID: 4027
		private ArrayList _validationCallbackInfo;

		// Token: 0x04000FBC RID: 4028
		private bool _useCachedHeaders;

		// Token: 0x04000FBD RID: 4029
		private HttpResponseHeader _headerCacheControl;

		// Token: 0x04000FBE RID: 4030
		private HttpResponseHeader _headerPragma;

		// Token: 0x04000FBF RID: 4031
		private HttpResponseHeader _headerExpires;

		// Token: 0x04000FC0 RID: 4032
		private HttpResponseHeader _headerLastModified;

		// Token: 0x04000FC1 RID: 4033
		private HttpResponseHeader _headerEtag;

		// Token: 0x04000FC2 RID: 4034
		private HttpResponseHeader _headerVaryBy;

		// Token: 0x04000FC3 RID: 4035
		private bool _noMaxAgeInCacheControl;

		// Token: 0x04000FC4 RID: 4036
		private bool _hasUserProvidedDependencies;

		// Token: 0x04000FC5 RID: 4037
		private static readonly string[] s_cacheabilityTokens;

		// Token: 0x04000FC6 RID: 4038
		private static readonly string[] s_revalidationTokens;

		// Token: 0x04000FC7 RID: 4039
		private static readonly int[] s_cacheabilityValues;
	}
}
