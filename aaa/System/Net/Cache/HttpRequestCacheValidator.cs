using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x0200055D RID: 1373
	internal class HttpRequestCacheValidator : RequestCacheValidator
	{
		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000AF281 File Offset: 0x000AE281
		// (set) Token: 0x060029D2 RID: 10706 RVA: 0x000AF289 File Offset: 0x000AE289
		internal HttpStatusCode CacheStatusCode
		{
			get
			{
				return this.m_StatusCode;
			}
			set
			{
				this.m_StatusCode = value;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060029D3 RID: 10707 RVA: 0x000AF292 File Offset: 0x000AE292
		// (set) Token: 0x060029D4 RID: 10708 RVA: 0x000AF29A File Offset: 0x000AE29A
		internal string CacheStatusDescription
		{
			get
			{
				return this.m_StatusDescription;
			}
			set
			{
				this.m_StatusDescription = value;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x000AF2A3 File Offset: 0x000AE2A3
		// (set) Token: 0x060029D6 RID: 10710 RVA: 0x000AF2AB File Offset: 0x000AE2AB
		internal Version CacheHttpVersion
		{
			get
			{
				return this.m_HttpVersion;
			}
			set
			{
				this.m_HttpVersion = value;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060029D7 RID: 10711 RVA: 0x000AF2B4 File Offset: 0x000AE2B4
		// (set) Token: 0x060029D8 RID: 10712 RVA: 0x000AF2BC File Offset: 0x000AE2BC
		internal WebHeaderCollection CacheHeaders
		{
			get
			{
				return this.m_Headers;
			}
			set
			{
				this.m_Headers = value;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x000AF2C8 File Offset: 0x000AE2C8
		internal new HttpRequestCachePolicy Policy
		{
			get
			{
				if (this.m_HttpPolicy != null)
				{
					return this.m_HttpPolicy;
				}
				this.m_HttpPolicy = base.Policy as HttpRequestCachePolicy;
				if (this.m_HttpPolicy != null)
				{
					return this.m_HttpPolicy;
				}
				this.m_HttpPolicy = new HttpRequestCachePolicy((HttpRequestCacheLevel)base.Policy.Level);
				return this.m_HttpPolicy;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x000AF320 File Offset: 0x000AE320
		// (set) Token: 0x060029DB RID: 10715 RVA: 0x000AF328 File Offset: 0x000AE328
		internal NameValueCollection SystemMeta
		{
			get
			{
				return this.m_SystemMeta;
			}
			set
			{
				this.m_SystemMeta = value;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x000AF331 File Offset: 0x000AE331
		// (set) Token: 0x060029DD RID: 10717 RVA: 0x000AF33E File Offset: 0x000AE33E
		internal HttpMethod RequestMethod
		{
			get
			{
				return this.m_RequestVars.Method;
			}
			set
			{
				this.m_RequestVars.Method = value;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000AF34C File Offset: 0x000AE34C
		// (set) Token: 0x060029DF RID: 10719 RVA: 0x000AF359 File Offset: 0x000AE359
		internal bool RequestRangeCache
		{
			get
			{
				return this.m_RequestVars.IsCacheRange;
			}
			set
			{
				this.m_RequestVars.IsCacheRange = value;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x000AF367 File Offset: 0x000AE367
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x000AF374 File Offset: 0x000AE374
		internal bool RequestRangeUser
		{
			get
			{
				return this.m_RequestVars.IsUserRange;
			}
			set
			{
				this.m_RequestVars.IsUserRange = value;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x000AF382 File Offset: 0x000AE382
		// (set) Token: 0x060029E3 RID: 10723 RVA: 0x000AF38F File Offset: 0x000AE38F
		internal string RequestIfHeader1
		{
			get
			{
				return this.m_RequestVars.IfHeader1;
			}
			set
			{
				this.m_RequestVars.IfHeader1 = value;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x000AF39D File Offset: 0x000AE39D
		// (set) Token: 0x060029E5 RID: 10725 RVA: 0x000AF3AA File Offset: 0x000AE3AA
		internal string RequestValidator1
		{
			get
			{
				return this.m_RequestVars.Validator1;
			}
			set
			{
				this.m_RequestVars.Validator1 = value;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x000AF3B8 File Offset: 0x000AE3B8
		// (set) Token: 0x060029E7 RID: 10727 RVA: 0x000AF3C5 File Offset: 0x000AE3C5
		internal string RequestIfHeader2
		{
			get
			{
				return this.m_RequestVars.IfHeader2;
			}
			set
			{
				this.m_RequestVars.IfHeader2 = value;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060029E8 RID: 10728 RVA: 0x000AF3D3 File Offset: 0x000AE3D3
		// (set) Token: 0x060029E9 RID: 10729 RVA: 0x000AF3E0 File Offset: 0x000AE3E0
		internal string RequestValidator2
		{
			get
			{
				return this.m_RequestVars.Validator2;
			}
			set
			{
				this.m_RequestVars.Validator2 = value;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060029EA RID: 10730 RVA: 0x000AF3EE File Offset: 0x000AE3EE
		// (set) Token: 0x060029EB RID: 10731 RVA: 0x000AF3F6 File Offset: 0x000AE3F6
		internal bool CacheDontUpdateHeaders
		{
			get
			{
				return this.m_DontUpdateHeaders;
			}
			set
			{
				this.m_DontUpdateHeaders = value;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060029EC RID: 10732 RVA: 0x000AF3FF File Offset: 0x000AE3FF
		// (set) Token: 0x060029ED RID: 10733 RVA: 0x000AF40C File Offset: 0x000AE40C
		internal DateTime CacheDate
		{
			get
			{
				return this.m_CacheVars.Date;
			}
			set
			{
				this.m_CacheVars.Date = value;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x000AF41A File Offset: 0x000AE41A
		// (set) Token: 0x060029EF RID: 10735 RVA: 0x000AF427 File Offset: 0x000AE427
		internal DateTime CacheExpires
		{
			get
			{
				return this.m_CacheVars.Expires;
			}
			set
			{
				this.m_CacheVars.Expires = value;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060029F0 RID: 10736 RVA: 0x000AF435 File Offset: 0x000AE435
		// (set) Token: 0x060029F1 RID: 10737 RVA: 0x000AF442 File Offset: 0x000AE442
		internal DateTime CacheLastModified
		{
			get
			{
				return this.m_CacheVars.LastModified;
			}
			set
			{
				this.m_CacheVars.LastModified = value;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x000AF450 File Offset: 0x000AE450
		// (set) Token: 0x060029F3 RID: 10739 RVA: 0x000AF45D File Offset: 0x000AE45D
		internal long CacheEntityLength
		{
			get
			{
				return this.m_CacheVars.EntityLength;
			}
			set
			{
				this.m_CacheVars.EntityLength = value;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x000AF46B File Offset: 0x000AE46B
		// (set) Token: 0x060029F5 RID: 10741 RVA: 0x000AF478 File Offset: 0x000AE478
		internal TimeSpan CacheAge
		{
			get
			{
				return this.m_CacheVars.Age;
			}
			set
			{
				this.m_CacheVars.Age = value;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x000AF486 File Offset: 0x000AE486
		// (set) Token: 0x060029F7 RID: 10743 RVA: 0x000AF493 File Offset: 0x000AE493
		internal TimeSpan CacheMaxAge
		{
			get
			{
				return this.m_CacheVars.MaxAge;
			}
			set
			{
				this.m_CacheVars.MaxAge = value;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060029F8 RID: 10744 RVA: 0x000AF4A1 File Offset: 0x000AE4A1
		// (set) Token: 0x060029F9 RID: 10745 RVA: 0x000AF4A9 File Offset: 0x000AE4A9
		internal bool HeuristicExpiration
		{
			get
			{
				return this.m_HeuristicExpiration;
			}
			set
			{
				this.m_HeuristicExpiration = value;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060029FA RID: 10746 RVA: 0x000AF4B2 File Offset: 0x000AE4B2
		// (set) Token: 0x060029FB RID: 10747 RVA: 0x000AF4BF File Offset: 0x000AE4BF
		internal ResponseCacheControl CacheCacheControl
		{
			get
			{
				return this.m_CacheVars.CacheControl;
			}
			set
			{
				this.m_CacheVars.CacheControl = value;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060029FC RID: 10748 RVA: 0x000AF4CD File Offset: 0x000AE4CD
		// (set) Token: 0x060029FD RID: 10749 RVA: 0x000AF4DA File Offset: 0x000AE4DA
		internal DateTime ResponseDate
		{
			get
			{
				return this.m_ResponseVars.Date;
			}
			set
			{
				this.m_ResponseVars.Date = value;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x000AF4E8 File Offset: 0x000AE4E8
		// (set) Token: 0x060029FF RID: 10751 RVA: 0x000AF4F5 File Offset: 0x000AE4F5
		internal DateTime ResponseExpires
		{
			get
			{
				return this.m_ResponseVars.Expires;
			}
			set
			{
				this.m_ResponseVars.Expires = value;
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x06002A00 RID: 10752 RVA: 0x000AF503 File Offset: 0x000AE503
		// (set) Token: 0x06002A01 RID: 10753 RVA: 0x000AF510 File Offset: 0x000AE510
		internal DateTime ResponseLastModified
		{
			get
			{
				return this.m_ResponseVars.LastModified;
			}
			set
			{
				this.m_ResponseVars.LastModified = value;
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06002A02 RID: 10754 RVA: 0x000AF51E File Offset: 0x000AE51E
		// (set) Token: 0x06002A03 RID: 10755 RVA: 0x000AF52B File Offset: 0x000AE52B
		internal long ResponseEntityLength
		{
			get
			{
				return this.m_ResponseVars.EntityLength;
			}
			set
			{
				this.m_ResponseVars.EntityLength = value;
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06002A04 RID: 10756 RVA: 0x000AF539 File Offset: 0x000AE539
		// (set) Token: 0x06002A05 RID: 10757 RVA: 0x000AF546 File Offset: 0x000AE546
		internal long ResponseRangeStart
		{
			get
			{
				return this.m_ResponseVars.RangeStart;
			}
			set
			{
				this.m_ResponseVars.RangeStart = value;
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06002A06 RID: 10758 RVA: 0x000AF554 File Offset: 0x000AE554
		// (set) Token: 0x06002A07 RID: 10759 RVA: 0x000AF561 File Offset: 0x000AE561
		internal long ResponseRangeEnd
		{
			get
			{
				return this.m_ResponseVars.RangeEnd;
			}
			set
			{
				this.m_ResponseVars.RangeEnd = value;
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06002A08 RID: 10760 RVA: 0x000AF56F File Offset: 0x000AE56F
		// (set) Token: 0x06002A09 RID: 10761 RVA: 0x000AF57C File Offset: 0x000AE57C
		internal TimeSpan ResponseAge
		{
			get
			{
				return this.m_ResponseVars.Age;
			}
			set
			{
				this.m_ResponseVars.Age = value;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06002A0A RID: 10762 RVA: 0x000AF58A File Offset: 0x000AE58A
		// (set) Token: 0x06002A0B RID: 10763 RVA: 0x000AF597 File Offset: 0x000AE597
		internal ResponseCacheControl ResponseCacheControl
		{
			get
			{
				return this.m_ResponseVars.CacheControl;
			}
			set
			{
				this.m_ResponseVars.CacheControl = value;
			}
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000AF5A8 File Offset: 0x000AE5A8
		private void ZeroPrivateVars()
		{
			this.m_RequestVars = default(HttpRequestCacheValidator.RequestVars);
			this.m_HttpPolicy = null;
			this.m_StatusCode = (HttpStatusCode)0;
			this.m_StatusDescription = null;
			this.m_HttpVersion = null;
			this.m_Headers = null;
			this.m_SystemMeta = null;
			this.m_DontUpdateHeaders = false;
			this.m_HeuristicExpiration = false;
			this.m_CacheVars = default(HttpRequestCacheValidator.Vars);
			this.m_CacheVars.Initialize();
			this.m_ResponseVars = default(HttpRequestCacheValidator.Vars);
			this.m_ResponseVars.Initialize();
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000AF627 File Offset: 0x000AE627
		internal override RequestCacheValidator CreateValidator()
		{
			return new HttpRequestCacheValidator(base.StrictCacheErrors, base.UnspecifiedMaxAge);
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000AF63A File Offset: 0x000AE63A
		internal HttpRequestCacheValidator(bool strictCacheErrors, TimeSpan unspecifiedMaxAge)
			: base(strictCacheErrors, unspecifiedMaxAge)
		{
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000AF644 File Offset: 0x000AE644
		protected internal override CacheValidationStatus ValidateRequest()
		{
			this.ZeroPrivateVars();
			string text = base.Request.Method.ToUpper(CultureInfo.InvariantCulture);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_request_method", new object[] { text }));
			}
			string text2;
			switch (text2 = text)
			{
			case "GET":
				this.RequestMethod = HttpMethod.Get;
				goto IL_0149;
			case "POST":
				this.RequestMethod = HttpMethod.Post;
				goto IL_0149;
			case "HEAD":
				this.RequestMethod = HttpMethod.Head;
				goto IL_0149;
			case "PUT":
				this.RequestMethod = HttpMethod.Put;
				goto IL_0149;
			case "DELETE":
				this.RequestMethod = HttpMethod.Delete;
				goto IL_0149;
			case "OPTIONS":
				this.RequestMethod = HttpMethod.Options;
				goto IL_0149;
			case "TRACE":
				this.RequestMethod = HttpMethod.Trace;
				goto IL_0149;
			case "CONNECT":
				this.RequestMethod = HttpMethod.Connect;
				goto IL_0149;
			}
			this.RequestMethod = HttpMethod.Other;
			IL_0149:
			return Rfc2616.OnValidateRequest(this);
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000AF7A0 File Offset: 0x000AE7A0
		protected internal override CacheFreshnessStatus ValidateFreshness()
		{
			string text = this.ParseStatusLine();
			if (Logging.On)
			{
				if (this.CacheStatusCode == (HttpStatusCode)0)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_http_status_parse_failure", new object[] { (text == null) ? "null" : text }));
				}
				else
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_http_status_line", new object[]
					{
						(this.CacheHttpVersion != null) ? this.CacheHttpVersion.ToString() : "null",
						(int)this.CacheStatusCode,
						this.CacheStatusDescription
					}));
				}
			}
			this.CreateCacheHeaders(this.CacheStatusCode != (HttpStatusCode)0);
			this.CreateSystemMeta();
			this.FetchHeaderValues(true);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_cache_control", new object[] { this.CacheCacheControl.ToString() }));
			}
			return Rfc2616.OnValidateFreshness(this);
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000AF8A0 File Offset: 0x000AE8A0
		protected internal override CacheValidationStatus ValidateCache()
		{
			if (this.Policy.Level != HttpRequestCacheLevel.Revalidate && base.Policy.Level >= RequestCacheLevel.Reload)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_validator_invalid_for_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (base.CacheStream == Stream.Null || this.CacheStatusCode == (HttpStatusCode)0 || this.CacheStatusCode == HttpStatusCode.NotModified)
			{
				if (this.Policy.Level == HttpRequestCacheLevel.CacheOnly)
				{
					this.FailRequest(WebExceptionStatus.CacheEntryNotFound);
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (this.RequestMethod == HttpMethod.Head)
			{
				base.CacheStream.Close();
				base.CacheStream = new SyncMemoryStream(new byte[0]);
			}
			this.RemoveWarnings_1xx();
			base.CacheStreamOffset = 0L;
			base.CacheStreamLength = base.CacheEntry.StreamSize;
			CacheValidationStatus cacheValidationStatus = Rfc2616.OnValidateCache(this);
			if (cacheValidationStatus != CacheValidationStatus.ReturnCachedResponse && this.Policy.Level == HttpRequestCacheLevel.CacheOnly)
			{
				this.FailRequest(WebExceptionStatus.CacheEntryNotFound);
			}
			if (cacheValidationStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				if (base.CacheFreshnessStatus == CacheFreshnessStatus.Stale)
				{
					this.CacheHeaders.Add("Warning", "110 Response is stale");
				}
				if (base.Policy.Level == RequestCacheLevel.CacheOnly)
				{
					this.CacheHeaders.Add("Warning", "112 Disconnected operation");
				}
				if (this.HeuristicExpiration && (int)this.CacheAge.TotalSeconds >= 86400)
				{
					this.CacheHeaders.Add("Warning", "113 Heuristic expiration");
				}
			}
			if (cacheValidationStatus == CacheValidationStatus.DoNotTakeFromCache)
			{
				this.CacheStatusCode = (HttpStatusCode)0;
			}
			else if (cacheValidationStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this.CacheHeaders["Age"] = ((int)this.CacheAge.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000AFA54 File Offset: 0x000AEA54
		protected internal override CacheValidationStatus RevalidateCache()
		{
			if (this.Policy.Level != HttpRequestCacheLevel.Revalidate && base.Policy.Level >= RequestCacheLevel.Reload)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_validator_invalid_for_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (base.CacheStream == Stream.Null || this.CacheStatusCode == (HttpStatusCode)0 || this.CacheStatusCode == HttpStatusCode.NotModified)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			CacheValidationStatus cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
			HttpWebResponse httpWebResponse = base.Response as HttpWebResponse;
			if (httpWebResponse == null)
			{
				return CacheValidationStatus.DoNotTakeFromCache;
			}
			if (httpWebResponse.StatusCode >= HttpStatusCode.InternalServerError)
			{
				if (Rfc2616.Common.ValidateCacheOn5XXResponse(this) == CacheValidationStatus.ReturnCachedResponse)
				{
					if (base.CacheFreshnessStatus == CacheFreshnessStatus.Stale)
					{
						this.CacheHeaders.Add("Warning", "110 Response is stale");
					}
					if (this.HeuristicExpiration && (int)this.CacheAge.TotalSeconds >= 86400)
					{
						this.CacheHeaders.Add("Warning", "113 Heuristic expiration");
					}
				}
			}
			else if (base.ResponseCount > 1)
			{
				cacheValidationStatus = CacheValidationStatus.DoNotTakeFromCache;
			}
			else
			{
				this.CacheAge = TimeSpan.Zero;
				cacheValidationStatus = Rfc2616.Common.ValidateCacheAfterResponse(this, httpWebResponse);
			}
			if (cacheValidationStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this.CacheHeaders["Age"] = ((int)this.CacheAge.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000AFBA4 File Offset: 0x000AEBA4
		protected internal override CacheValidationStatus ValidateResponse()
		{
			if (this.Policy.Level != HttpRequestCacheLevel.CacheOrNextCacheOnly && this.Policy.Level != HttpRequestCacheLevel.Default && this.Policy.Level != HttpRequestCacheLevel.Revalidate)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_response_valid_based_on_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.Continue;
			}
			HttpWebResponse httpWebResponse = base.Response as HttpWebResponse;
			if (httpWebResponse == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_null_response_failure"));
				}
				return CacheValidationStatus.Continue;
			}
			this.FetchHeaderValues(false);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, string.Concat(new object[]
				{
					"StatusCode=",
					((int)httpWebResponse.StatusCode).ToString(CultureInfo.InvariantCulture),
					' ',
					httpWebResponse.StatusCode.ToString(),
					(httpWebResponse.StatusCode == HttpStatusCode.PartialContent) ? (", Content-Range: " + httpWebResponse.Headers["Content-Range"]) : string.Empty
				}));
			}
			return Rfc2616.OnValidateResponse(this);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000AFCD8 File Offset: 0x000AECD8
		protected internal override CacheValidationStatus UpdateCache()
		{
			if (this.Policy.Level == HttpRequestCacheLevel.NoCacheNoStore)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_removed_existing_based_on_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.RemoveFromCache;
			}
			if (this.Policy.Level == HttpRequestCacheLevel.CacheOnly)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_policy", new object[] { this.Policy.ToString() }));
				}
				return CacheValidationStatus.DoNotUpdateCache;
			}
			if (this.CacheHeaders == null)
			{
				this.CacheHeaders = new WebHeaderCollection();
			}
			if (this.SystemMeta == null)
			{
				this.SystemMeta = new NameValueCollection(1, CaseInsensitiveAscii.StaticInstance);
			}
			if (this.ResponseCacheControl == null)
			{
				this.FetchHeaderValues(false);
			}
			CacheValidationStatus cacheValidationStatus = Rfc2616.OnUpdateCache(this);
			if (cacheValidationStatus == CacheValidationStatus.UpdateResponseInformation || cacheValidationStatus == CacheValidationStatus.CacheResponse)
			{
				this.FinallyUpdateCacheEntry();
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000AFDBC File Offset: 0x000AEDBC
		private void FinallyUpdateCacheEntry()
		{
			base.CacheEntry.EntryMetadata = null;
			base.CacheEntry.SystemMetadata = null;
			if (this.CacheHeaders == null)
			{
				return;
			}
			base.CacheEntry.EntryMetadata = new StringCollection();
			base.CacheEntry.SystemMetadata = new StringCollection();
			if (this.CacheHttpVersion == null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_invalid_http_version"));
				}
				this.CacheHttpVersion = new Version(1, 0);
			}
			StringBuilder stringBuilder = new StringBuilder(this.CacheStatusDescription.Length + 20);
			stringBuilder.Append("HTTP/");
			stringBuilder.Append(this.CacheHttpVersion.ToString(2));
			stringBuilder.Append(' ');
			stringBuilder.Append(((int)this.CacheStatusCode).ToString(NumberFormatInfo.InvariantInfo));
			stringBuilder.Append(' ');
			stringBuilder.Append(this.CacheStatusDescription);
			base.CacheEntry.EntryMetadata.Add(stringBuilder.ToString());
			HttpRequestCacheValidator.UpdateStringCollection(base.CacheEntry.EntryMetadata, this.CacheHeaders, false);
			if (this.SystemMeta != null)
			{
				HttpRequestCacheValidator.UpdateStringCollection(base.CacheEntry.SystemMetadata, this.SystemMeta, true);
			}
			if (this.ResponseExpires != DateTime.MinValue)
			{
				base.CacheEntry.ExpiresUtc = this.ResponseExpires;
			}
			if (this.ResponseLastModified != DateTime.MinValue)
			{
				base.CacheEntry.LastModifiedUtc = this.ResponseLastModified;
			}
			if (this.Policy.Level == HttpRequestCacheLevel.Default)
			{
				base.CacheEntry.MaxStale = this.Policy.MaxStale;
			}
			base.CacheEntry.LastSynchronizedUtc = DateTime.UtcNow;
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000AFF74 File Offset: 0x000AEF74
		private static void UpdateStringCollection(StringCollection result, NameValueCollection cc, bool winInetCompat)
		{
			for (int i = 0; i < cc.Count; i++)
			{
				StringBuilder stringBuilder = new StringBuilder(40);
				string key = cc.GetKey(i);
				stringBuilder.Append(key).Append(':');
				string[] values = cc.GetValues(i);
				if (values.Length != 0)
				{
					if (winInetCompat)
					{
						stringBuilder.Append(values[0]);
					}
					else
					{
						stringBuilder.Append(' ').Append(values[0]);
					}
				}
				for (int j = 1; j < values.Length; j++)
				{
					stringBuilder.Append(key).Append(", ").Append(values[j]);
				}
				result.Add(stringBuilder.ToString());
			}
			result.Add(string.Empty);
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000B002C File Offset: 0x000AF02C
		private string ParseStatusLine()
		{
			this.CacheStatusCode = (HttpStatusCode)0;
			if (base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0)
			{
				return null;
			}
			string text = base.CacheEntry.EntryMetadata[0];
			if (text == null)
			{
				return null;
			}
			int num = 0;
			char c = '\0';
			while (++num < text.Length && (c = text[num]) != '/')
			{
			}
			if (num == text.Length)
			{
				return text;
			}
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			while (++num < text.Length && (c = text[num]) >= '0' && c <= '9')
			{
				num2 = ((num2 < 0) ? 0 : (num2 * 10)) + (int)(c - '0');
			}
			if (num2 < 0 || c != '.')
			{
				return text;
			}
			while (++num < text.Length && (c = text[num]) >= '0' && c <= '9')
			{
				num3 = ((num3 < 0) ? 0 : (num3 * 10)) + (int)(c - '0');
			}
			if (num3 < 0 || (c != ' ' && c != '\t'))
			{
				return text;
			}
			while (++num < text.Length && ((c = text[num]) == ' ' || c == '\t'))
			{
			}
			if (num >= text.Length)
			{
				return text;
			}
			while (c >= '0' && c <= '9')
			{
				num4 = ((num4 < 0) ? 0 : (num4 * 10)) + (int)(c - '0');
				if (++num == text.Length)
				{
					break;
				}
				c = text[num];
			}
			if (num4 < 0 || (num <= text.Length && c != ' ' && c != '\t'))
			{
				return text;
			}
			while (num < text.Length && (text[num] == ' ' || text[num] == '\t'))
			{
				num++;
			}
			this.CacheStatusDescription = text.Substring(num);
			this.CacheHttpVersion = new Version(num2, num3);
			this.CacheStatusCode = (HttpStatusCode)num4;
			return text;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000B01E8 File Offset: 0x000AF1E8
		private void CreateCacheHeaders(bool ignoreFirstString)
		{
			if (this.CacheHeaders == null)
			{
				this.CacheHeaders = new WebHeaderCollection();
			}
			if (base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_no_http_response_header"));
				}
				return;
			}
			string text = this.ParseNameValues(this.CacheHeaders, base.CacheEntry.EntryMetadata, ignoreFirstString ? 1 : 0);
			if (text != null)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_http_header_parse_error", new object[] { text }));
				}
				this.CacheHeaders.Clear();
			}
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x000B0298 File Offset: 0x000AF298
		private void CreateSystemMeta()
		{
			if (this.SystemMeta == null)
			{
				this.SystemMeta = new NameValueCollection((base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0) ? 2 : base.CacheEntry.EntryMetadata.Count, CaseInsensitiveAscii.StaticInstance);
			}
			if (base.CacheEntry.EntryMetadata == null || base.CacheEntry.EntryMetadata.Count == 0)
			{
				return;
			}
			string text = this.ParseNameValues(this.SystemMeta, base.CacheEntry.SystemMetadata, 0);
			if (text != null && Logging.On)
			{
				Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_metadata_name_value_parse_error", new object[] { text }));
			}
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x000B0354 File Offset: 0x000AF354
		private string ParseNameValues(NameValueCollection cc, StringCollection sc, int start)
		{
			WebHeaderCollection webHeaderCollection = cc as WebHeaderCollection;
			string text = null;
			if (sc != null)
			{
				for (int i = start; i < sc.Count; i++)
				{
					string text2 = sc[i];
					if (text2 == null || text2.Length == 0)
					{
						return null;
					}
					if (text2[0] == ' ' || text2[0] == '\t')
					{
						if (text == null)
						{
							return text2;
						}
						if (webHeaderCollection != null)
						{
							webHeaderCollection.AddInternal(text, text2);
						}
						else
						{
							cc.Add(text, text2);
						}
					}
					int num = text2.IndexOf(':');
					if (num < 0)
					{
						return text2;
					}
					text = text2.Substring(0, num);
					while (++num < text2.Length)
					{
						if (text2[num] != ' ' && text2[num] != '\t')
						{
							break;
						}
					}
					try
					{
						if (webHeaderCollection != null)
						{
							webHeaderCollection.AddInternal(text, text2.Substring(num));
						}
						else
						{
							cc.Add(text, text2.Substring(num));
						}
					}
					catch (Exception ex)
					{
						if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
						{
							throw;
						}
						return text2;
					}
				}
			}
			return null;
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x000B046C File Offset: 0x000AF46C
		private void FetchHeaderValues(bool forCache)
		{
			WebHeaderCollection webHeaderCollection = (forCache ? this.CacheHeaders : base.Response.Headers);
			this.FetchCacheControl(webHeaderCollection.CacheControl, forCache);
			string text = webHeaderCollection.Date;
			DateTime dateTime = DateTime.MinValue;
			if (text != null && HttpDateParse.ParseHttpDate(text, out dateTime))
			{
				dateTime = dateTime.ToUniversalTime();
			}
			if (forCache)
			{
				this.CacheDate = dateTime;
			}
			else
			{
				this.ResponseDate = dateTime;
			}
			text = webHeaderCollection.Expires;
			dateTime = DateTime.MinValue;
			if (text != null && HttpDateParse.ParseHttpDate(text, out dateTime))
			{
				dateTime = dateTime.ToUniversalTime();
			}
			if (forCache)
			{
				this.CacheExpires = dateTime;
			}
			else
			{
				this.ResponseExpires = dateTime;
			}
			text = webHeaderCollection.LastModified;
			dateTime = DateTime.MinValue;
			if (text != null && HttpDateParse.ParseHttpDate(text, out dateTime))
			{
				dateTime = dateTime.ToUniversalTime();
			}
			if (forCache)
			{
				this.CacheLastModified = dateTime;
			}
			else
			{
				this.ResponseLastModified = dateTime;
			}
			long num = -1L;
			long num2 = -1L;
			long num3 = -1L;
			HttpWebResponse httpWebResponse = base.Response as HttpWebResponse;
			if ((forCache ? this.CacheStatusCode : httpWebResponse.StatusCode) != HttpStatusCode.PartialContent)
			{
				text = webHeaderCollection.ContentLength;
				if (text != null && text.Length != 0)
				{
					int num4 = 0;
					char c = text[0];
					while (num4 < text.Length && c == ' ')
					{
						c = text[++num4];
					}
					if (num4 != text.Length && c >= '0' && c <= '9')
					{
						num = (long)(c - '0');
						while (++num4 < text.Length && (c = text[num4]) >= '0')
						{
							if (c > '9')
							{
								break;
							}
							num = num * 10L + (long)(c - '0');
						}
					}
				}
			}
			else
			{
				text = webHeaderCollection["Content-Range"];
				if (text == null || !Rfc2616.Common.GetBytesRange(text, ref num2, ref num3, ref num, false))
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_content_range_error", new object[] { (text == null) ? "<null>" : text }));
					}
					num3 = (num2 = (num = -1L));
				}
				else if (forCache && num == base.CacheEntry.StreamSize)
				{
					num2 = -1L;
					num3 = -1L;
					this.CacheStatusCode = HttpStatusCode.OK;
					this.CacheStatusDescription = "OK";
				}
			}
			if (forCache)
			{
				this.CacheEntityLength = num;
				this.ResponseRangeStart = num2;
				this.ResponseRangeEnd = num3;
			}
			else
			{
				this.ResponseEntityLength = num;
				this.ResponseRangeStart = num2;
				this.ResponseRangeEnd = num3;
			}
			TimeSpan timeSpan = TimeSpan.MinValue;
			text = webHeaderCollection["Age"];
			if (text != null)
			{
				int i = 0;
				int num5 = 0;
				while (i < text.Length)
				{
					if (text[i++] != ' ')
					{
						break;
					}
				}
				while (i < text.Length && text[i] >= '0' && text[i] <= '9')
				{
					num5 = num5 * 10 + (int)(text[i++] - '0');
				}
				timeSpan = TimeSpan.FromSeconds((double)num5);
			}
			if (forCache)
			{
				this.CacheAge = timeSpan;
				return;
			}
			this.ResponseAge = timeSpan;
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x000B076C File Offset: 0x000AF76C
		private void FetchCacheControl(string s, bool forCache)
		{
			/*
An exception occurred when decompiling this method (06002A1C)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Net.Cache.HttpRequestCacheValidator::FetchCacheControl(System.String,System.Boolean)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.HashSet`1.System.Collections.Generic.IEnumerable<T>.GetEnumerator()
   at System.Linq.Enumerable.TryGetFirst[TSource](IEnumerable`1 source, Boolean& found)
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindDominatedNodes(HashSet`1 scope, ControlFlowNode head) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 450
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 409
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 410
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 410
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 405
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 410
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 69
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000B12DC File Offset: 0x000B02DC
		private void RemoveWarnings_1xx()
		{
			string[] values = this.CacheHeaders.GetValues("Warning");
			if (values == null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			HttpRequestCacheValidator.ParseHeaderValues(values, HttpRequestCacheValidator.ParseWarningsCallback, arrayList);
			this.CacheHeaders.Remove("Warning");
			for (int i = 0; i < arrayList.Count; i++)
			{
				this.CacheHeaders.Add("Warning", (string)arrayList[i]);
			}
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000B134D File Offset: 0x000B034D
		private static void ParseWarningsCallbackMethod(string s, int start, int end, IList list)
		{
			if (end >= start && s[start] != '1')
			{
				HttpRequestCacheValidator.ParseValuesCallbackMethod(s, start, end, list);
			}
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000B1367 File Offset: 0x000B0367
		private static void ParseValuesCallbackMethod(string s, int start, int end, IList list)
		{
			while (end >= start && s[end] == ' ')
			{
				end--;
			}
			if (end >= start)
			{
				list.Add(s.Substring(start, end - start + 1));
			}
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x000B1398 File Offset: 0x000B0398
		internal static void ParseHeaderValues(string[] values, HttpRequestCacheValidator.ParseCallback calback, IList list)
		{
			/*
An exception occurred when decompiling this method (06002A20)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Net.Cache.HttpRequestCacheValidator::ParseHeaderValues(System.String[],System.Net.Cache.HttpRequestCacheValidator/ParseCallback,System.Collections.IList)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 217
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.Run(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 49
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 417
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04002892 RID: 10386
		internal const string Warning_110 = "110 Response is stale";

		// Token: 0x04002893 RID: 10387
		internal const string Warning_111 = "111 Revalidation failed";

		// Token: 0x04002894 RID: 10388
		internal const string Warning_112 = "112 Disconnected operation";

		// Token: 0x04002895 RID: 10389
		internal const string Warning_113 = "113 Heuristic expiration";

		// Token: 0x04002896 RID: 10390
		private const long LO = 9007336695791648L;

		// Token: 0x04002897 RID: 10391
		private const int LOI = 2097184;

		// Token: 0x04002898 RID: 10392
		private const long _prox = 33777473954119792L;

		// Token: 0x04002899 RID: 10393
		private const long _y_re = 28429462276997241L;

		// Token: 0x0400289A RID: 10394
		private const long _vali = 29555336417443958L;

		// Token: 0x0400289B RID: 10395
		private const long _date = 28429470870339684L;

		// Token: 0x0400289C RID: 10396
		private const long _publ = 30399718399213680L;

		// Token: 0x0400289D RID: 10397
		private const int _ic = 6488169;

		// Token: 0x0400289E RID: 10398
		private const long _priv = 33214498230894704L;

		// Token: 0x0400289F RID: 10399
		private const int _at = 7602273;

		// Token: 0x040028A0 RID: 10400
		private const long _no_c = 27866215975157870L;

		// Token: 0x040028A1 RID: 10401
		private const long _ache = 28429419330863201L;

		// Token: 0x040028A2 RID: 10402
		private const long _no_s = 32369815602528366L;

		// Token: 0x040028A3 RID: 10403
		private const long _tore = 28429462281322612L;

		// Token: 0x040028A4 RID: 10404
		private const long _must = 32651591227342957L;

		// Token: 0x040028A5 RID: 10405
		private const long __rev = 33214481051025453L;

		// Token: 0x040028A6 RID: 10406
		private const long _alid = 28147948649709665L;

		// Token: 0x040028A7 RID: 10407
		private const long _max_ = 12666889354412141L;

		// Token: 0x040028A8 RID: 10408
		private const int _ag = 6750305;

		// Token: 0x040028A9 RID: 10409
		private const long _s_ma = 27303540895318131L;

		// Token: 0x040028AA RID: 10410
		private const long _xage = 28429415035764856L;

		// Token: 0x040028AB RID: 10411
		private HttpRequestCachePolicy m_HttpPolicy;

		// Token: 0x040028AC RID: 10412
		private HttpStatusCode m_StatusCode;

		// Token: 0x040028AD RID: 10413
		private string m_StatusDescription;

		// Token: 0x040028AE RID: 10414
		private Version m_HttpVersion;

		// Token: 0x040028AF RID: 10415
		private WebHeaderCollection m_Headers;

		// Token: 0x040028B0 RID: 10416
		private NameValueCollection m_SystemMeta;

		// Token: 0x040028B1 RID: 10417
		private bool m_DontUpdateHeaders;

		// Token: 0x040028B2 RID: 10418
		private bool m_HeuristicExpiration;

		// Token: 0x040028B3 RID: 10419
		private HttpRequestCacheValidator.Vars m_CacheVars;

		// Token: 0x040028B4 RID: 10420
		private HttpRequestCacheValidator.Vars m_ResponseVars;

		// Token: 0x040028B5 RID: 10421
		private HttpRequestCacheValidator.RequestVars m_RequestVars;

		// Token: 0x040028B6 RID: 10422
		private static readonly HttpRequestCacheValidator.ParseCallback ParseWarningsCallback = new HttpRequestCacheValidator.ParseCallback(HttpRequestCacheValidator.ParseWarningsCallbackMethod);

		// Token: 0x040028B7 RID: 10423
		internal static readonly HttpRequestCacheValidator.ParseCallback ParseValuesCallback = new HttpRequestCacheValidator.ParseCallback(HttpRequestCacheValidator.ParseValuesCallbackMethod);

		// Token: 0x0200055E RID: 1374
		private struct RequestVars
		{
			// Token: 0x040028B8 RID: 10424
			internal HttpMethod Method;

			// Token: 0x040028B9 RID: 10425
			internal bool IsCacheRange;

			// Token: 0x040028BA RID: 10426
			internal bool IsUserRange;

			// Token: 0x040028BB RID: 10427
			internal string IfHeader1;

			// Token: 0x040028BC RID: 10428
			internal string Validator1;

			// Token: 0x040028BD RID: 10429
			internal string IfHeader2;

			// Token: 0x040028BE RID: 10430
			internal string Validator2;
		}

		// Token: 0x0200055F RID: 1375
		private struct Vars
		{
			// Token: 0x06002A22 RID: 10786 RVA: 0x000B14C8 File Offset: 0x000B04C8
			internal void Initialize()
			{
				this.EntityLength = (this.RangeStart = (this.RangeEnd = -1L));
				this.Date = DateTime.MinValue;
				this.Expires = DateTime.MinValue;
				this.LastModified = DateTime.MinValue;
				this.Age = TimeSpan.MinValue;
				this.MaxAge = TimeSpan.MinValue;
			}

			// Token: 0x040028BF RID: 10431
			internal DateTime Date;

			// Token: 0x040028C0 RID: 10432
			internal DateTime Expires;

			// Token: 0x040028C1 RID: 10433
			internal DateTime LastModified;

			// Token: 0x040028C2 RID: 10434
			internal long EntityLength;

			// Token: 0x040028C3 RID: 10435
			internal TimeSpan Age;

			// Token: 0x040028C4 RID: 10436
			internal TimeSpan MaxAge;

			// Token: 0x040028C5 RID: 10437
			internal ResponseCacheControl CacheControl;

			// Token: 0x040028C6 RID: 10438
			internal long RangeStart;

			// Token: 0x040028C7 RID: 10439
			internal long RangeEnd;
		}

		// Token: 0x02000560 RID: 1376
		// (Invoke) Token: 0x06002A24 RID: 10788
		internal delegate void ParseCallback(string s, int start, int end, IList list);
	}
}
