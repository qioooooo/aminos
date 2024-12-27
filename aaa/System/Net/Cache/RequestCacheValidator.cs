using System;
using System.IO;

namespace System.Net.Cache
{
	// Token: 0x0200055C RID: 1372
	internal abstract class RequestCacheValidator
	{
		// Token: 0x060029B2 RID: 10674
		internal abstract RequestCacheValidator CreateValidator();

		// Token: 0x060029B3 RID: 10675 RVA: 0x000AF0C1 File Offset: 0x000AE0C1
		protected RequestCacheValidator(bool strictCacheErrors, TimeSpan unspecifiedMaxAge)
		{
			this._StrictCacheErrors = strictCacheErrors;
			this._UnspecifiedMaxAge = unspecifiedMaxAge;
			this._ValidationStatus = CacheValidationStatus.DoNotUseCache;
			this._CacheFreshnessStatus = CacheFreshnessStatus.Undefined;
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060029B4 RID: 10676 RVA: 0x000AF0E5 File Offset: 0x000AE0E5
		internal bool StrictCacheErrors
		{
			get
			{
				return this._StrictCacheErrors;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060029B5 RID: 10677 RVA: 0x000AF0ED File Offset: 0x000AE0ED
		internal TimeSpan UnspecifiedMaxAge
		{
			get
			{
				return this._UnspecifiedMaxAge;
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060029B6 RID: 10678 RVA: 0x000AF0F5 File Offset: 0x000AE0F5
		protected internal Uri Uri
		{
			get
			{
				return this._Uri;
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060029B7 RID: 10679 RVA: 0x000AF0FD File Offset: 0x000AE0FD
		protected internal WebRequest Request
		{
			get
			{
				return this._Request;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060029B8 RID: 10680 RVA: 0x000AF105 File Offset: 0x000AE105
		protected internal WebResponse Response
		{
			get
			{
				return this._Response;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060029B9 RID: 10681 RVA: 0x000AF10D File Offset: 0x000AE10D
		protected internal RequestCachePolicy Policy
		{
			get
			{
				return this._Policy;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060029BA RID: 10682 RVA: 0x000AF115 File Offset: 0x000AE115
		protected internal int ResponseCount
		{
			get
			{
				return this._ResponseCount;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060029BB RID: 10683 RVA: 0x000AF11D File Offset: 0x000AE11D
		protected internal CacheValidationStatus ValidationStatus
		{
			get
			{
				return this._ValidationStatus;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060029BC RID: 10684 RVA: 0x000AF125 File Offset: 0x000AE125
		protected internal CacheFreshnessStatus CacheFreshnessStatus
		{
			get
			{
				return this._CacheFreshnessStatus;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060029BD RID: 10685 RVA: 0x000AF12D File Offset: 0x000AE12D
		protected internal RequestCacheEntry CacheEntry
		{
			get
			{
				return this._CacheEntry;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060029BE RID: 10686 RVA: 0x000AF135 File Offset: 0x000AE135
		// (set) Token: 0x060029BF RID: 10687 RVA: 0x000AF13D File Offset: 0x000AE13D
		protected internal Stream CacheStream
		{
			get
			{
				return this._CacheStream;
			}
			set
			{
				this._CacheStream = value;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060029C0 RID: 10688 RVA: 0x000AF146 File Offset: 0x000AE146
		// (set) Token: 0x060029C1 RID: 10689 RVA: 0x000AF14E File Offset: 0x000AE14E
		protected internal long CacheStreamOffset
		{
			get
			{
				return this._CacheStreamOffset;
			}
			set
			{
				this._CacheStreamOffset = value;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x000AF157 File Offset: 0x000AE157
		// (set) Token: 0x060029C3 RID: 10691 RVA: 0x000AF15F File Offset: 0x000AE15F
		protected internal long CacheStreamLength
		{
			get
			{
				return this._CacheStreamLength;
			}
			set
			{
				this._CacheStreamLength = value;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060029C4 RID: 10692 RVA: 0x000AF168 File Offset: 0x000AE168
		protected internal string CacheKey
		{
			get
			{
				return this._CacheKey;
			}
		}

		// Token: 0x060029C5 RID: 10693
		protected internal abstract CacheValidationStatus ValidateRequest();

		// Token: 0x060029C6 RID: 10694
		protected internal abstract CacheFreshnessStatus ValidateFreshness();

		// Token: 0x060029C7 RID: 10695
		protected internal abstract CacheValidationStatus ValidateCache();

		// Token: 0x060029C8 RID: 10696
		protected internal abstract CacheValidationStatus ValidateResponse();

		// Token: 0x060029C9 RID: 10697
		protected internal abstract CacheValidationStatus RevalidateCache();

		// Token: 0x060029CA RID: 10698
		protected internal abstract CacheValidationStatus UpdateCache();

		// Token: 0x060029CB RID: 10699 RVA: 0x000AF170 File Offset: 0x000AE170
		protected internal virtual void FailRequest(WebExceptionStatus webStatus)
		{
			if (Logging.On)
			{
				Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_failing_request_with_exception", new object[] { webStatus.ToString() }));
			}
			if (webStatus == WebExceptionStatus.CacheEntryNotFound)
			{
				throw ExceptionHelper.CacheEntryNotFoundException;
			}
			if (webStatus == WebExceptionStatus.RequestProhibitedByCachePolicy)
			{
				throw ExceptionHelper.RequestProhibitedByCachePolicyException;
			}
			throw new WebException(NetRes.GetWebStatusString("net_requestaborted", webStatus), webStatus);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x000AF1D8 File Offset: 0x000AE1D8
		internal void FetchRequest(Uri uri, WebRequest request)
		{
			this._Request = request;
			this._Policy = request.CachePolicy;
			this._Response = null;
			this._ResponseCount = 0;
			this._ValidationStatus = CacheValidationStatus.DoNotUseCache;
			this._CacheFreshnessStatus = CacheFreshnessStatus.Undefined;
			this._CacheStream = null;
			this._CacheStreamOffset = 0L;
			this._CacheStreamLength = 0L;
			if (!uri.Equals(this._Uri))
			{
				this._CacheKey = uri.GetParts(UriComponents.AbsoluteUri, UriFormat.Unescaped);
			}
			this._Uri = uri;
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x000AF24F File Offset: 0x000AE24F
		internal void FetchCacheEntry(RequestCacheEntry fetchEntry)
		{
			this._CacheEntry = fetchEntry;
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x000AF258 File Offset: 0x000AE258
		internal void FetchResponse(WebResponse fetchResponse)
		{
			this._ResponseCount++;
			this._Response = fetchResponse;
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x000AF26F File Offset: 0x000AE26F
		internal void SetFreshnessStatus(CacheFreshnessStatus status)
		{
			this._CacheFreshnessStatus = status;
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x000AF278 File Offset: 0x000AE278
		internal void SetValidationStatus(CacheValidationStatus status)
		{
			this._ValidationStatus = status;
		}

		// Token: 0x04002884 RID: 10372
		internal WebRequest _Request;

		// Token: 0x04002885 RID: 10373
		internal WebResponse _Response;

		// Token: 0x04002886 RID: 10374
		internal Stream _CacheStream;

		// Token: 0x04002887 RID: 10375
		private RequestCachePolicy _Policy;

		// Token: 0x04002888 RID: 10376
		private Uri _Uri;

		// Token: 0x04002889 RID: 10377
		private string _CacheKey;

		// Token: 0x0400288A RID: 10378
		private RequestCacheEntry _CacheEntry;

		// Token: 0x0400288B RID: 10379
		private int _ResponseCount;

		// Token: 0x0400288C RID: 10380
		private CacheValidationStatus _ValidationStatus;

		// Token: 0x0400288D RID: 10381
		private CacheFreshnessStatus _CacheFreshnessStatus;

		// Token: 0x0400288E RID: 10382
		private long _CacheStreamOffset;

		// Token: 0x0400288F RID: 10383
		private long _CacheStreamLength;

		// Token: 0x04002890 RID: 10384
		private bool _StrictCacheErrors;

		// Token: 0x04002891 RID: 10385
		private TimeSpan _UnspecifiedMaxAge;
	}
}
