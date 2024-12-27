using System;

namespace System.Web
{
	// Token: 0x0200005E RID: 94
	internal sealed class HttpCachePolicySettings
	{
		// Token: 0x0600033B RID: 827 RVA: 0x0000F4DC File Offset: 0x0000E4DC
		internal HttpCachePolicySettings(bool isModified, ValidationCallbackInfo[] validationCallbackInfo, bool hasSetCookieHeader, bool noServerCaching, string cacheExtension, bool noTransforms, bool ignoreRangeRequests, string[] varyByContentEncodings, string[] varyByHeaderValues, string[] varyByParamValues, string varyByCustom, HttpCacheability cacheability, bool noStore, string[] privateFields, string[] noCacheFields, DateTime utcExpires, bool isExpiresSet, TimeSpan maxAge, bool isMaxAgeSet, TimeSpan proxyMaxAge, bool isProxyMaxAgeSet, int slidingExpiration, TimeSpan slidingDelta, DateTime utcTimestampCreated, int validUntilExpires, int allowInHistory, HttpCacheRevalidation revalidation, DateTime utcLastModified, bool isLastModifiedSet, string etag, bool generateLastModifiedFromFiles, bool generateEtagFromFiles, int omitVaryStar, HttpResponseHeader headerCacheControl, HttpResponseHeader headerPragma, HttpResponseHeader headerExpires, HttpResponseHeader headerLastModified, HttpResponseHeader headerEtag, HttpResponseHeader headerVaryBy, bool hasUserProvidedDependencies)
		{
			this._isModified = isModified;
			this._validationCallbackInfo = validationCallbackInfo;
			this._hasSetCookieHeader = hasSetCookieHeader;
			this._noServerCaching = noServerCaching;
			this._cacheExtension = cacheExtension;
			this._noTransforms = noTransforms;
			this._ignoreRangeRequests = ignoreRangeRequests;
			this._varyByContentEncodings = varyByContentEncodings;
			this._varyByHeaderValues = varyByHeaderValues;
			this._varyByParamValues = varyByParamValues;
			this._varyByCustom = varyByCustom;
			this._cacheability = cacheability;
			this._noStore = noStore;
			this._privateFields = privateFields;
			this._noCacheFields = noCacheFields;
			this._utcExpires = utcExpires;
			this._isExpiresSet = isExpiresSet;
			this._maxAge = maxAge;
			this._isMaxAgeSet = isMaxAgeSet;
			this._proxyMaxAge = proxyMaxAge;
			this._isProxyMaxAgeSet = isProxyMaxAgeSet;
			this._slidingExpiration = slidingExpiration;
			this._slidingDelta = slidingDelta;
			this._utcTimestampCreated = utcTimestampCreated;
			this._validUntilExpires = validUntilExpires;
			this._allowInHistory = allowInHistory;
			this._revalidation = revalidation;
			this._utcLastModified = utcLastModified;
			this._isLastModifiedSet = isLastModifiedSet;
			this._etag = etag;
			this._generateLastModifiedFromFiles = generateLastModifiedFromFiles;
			this._generateEtagFromFiles = generateEtagFromFiles;
			this._omitVaryStar = omitVaryStar;
			this._headerCacheControl = headerCacheControl;
			this._headerPragma = headerPragma;
			this._headerExpires = headerExpires;
			this._headerLastModified = headerLastModified;
			this._headerEtag = headerEtag;
			this._headerVaryBy = headerVaryBy;
			this._hasUserProvidedDependencies = hasUserProvidedDependencies;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000F62C File Offset: 0x0000E62C
		internal bool IsModified
		{
			get
			{
				return this._isModified;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000F634 File Offset: 0x0000E634
		internal ValidationCallbackInfo[] ValidationCallbackInfo
		{
			get
			{
				return this._validationCallbackInfo;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000F63C File Offset: 0x0000E63C
		internal HttpResponseHeader HeaderCacheControl
		{
			get
			{
				return this._headerCacheControl;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600033F RID: 831 RVA: 0x0000F644 File Offset: 0x0000E644
		internal HttpResponseHeader HeaderPragma
		{
			get
			{
				return this._headerPragma;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000F64C File Offset: 0x0000E64C
		internal HttpResponseHeader HeaderExpires
		{
			get
			{
				return this._headerExpires;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000F654 File Offset: 0x0000E654
		internal HttpResponseHeader HeaderLastModified
		{
			get
			{
				return this._headerLastModified;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000F65C File Offset: 0x0000E65C
		internal HttpResponseHeader HeaderEtag
		{
			get
			{
				return this._headerEtag;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000F664 File Offset: 0x0000E664
		internal HttpResponseHeader HeaderVaryBy
		{
			get
			{
				return this._headerVaryBy;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000F66C File Offset: 0x0000E66C
		internal bool hasSetCookieHeader
		{
			get
			{
				return this._hasSetCookieHeader;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000F674 File Offset: 0x0000E674
		internal bool NoServerCaching
		{
			get
			{
				return this._noServerCaching;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000F67C File Offset: 0x0000E67C
		internal string CacheExtension
		{
			get
			{
				return this._cacheExtension;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000347 RID: 839 RVA: 0x0000F684 File Offset: 0x0000E684
		internal bool NoTransforms
		{
			get
			{
				return this._noTransforms;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000F68C File Offset: 0x0000E68C
		internal bool IgnoreRangeRequests
		{
			get
			{
				return this._ignoreRangeRequests;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000F694 File Offset: 0x0000E694
		internal string[] VaryByContentEncodings
		{
			get
			{
				if (this._varyByContentEncodings != null)
				{
					return (string[])this._varyByContentEncodings.Clone();
				}
				return null;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000F6B0 File Offset: 0x0000E6B0
		internal string[] VaryByHeaders
		{
			get
			{
				if (this._varyByHeaderValues != null)
				{
					return (string[])this._varyByHeaderValues.Clone();
				}
				return null;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0000F6CC File Offset: 0x0000E6CC
		internal string[] VaryByParams
		{
			get
			{
				if (this._varyByParamValues != null)
				{
					return (string[])this._varyByParamValues.Clone();
				}
				return null;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000F6E8 File Offset: 0x0000E6E8
		internal bool IgnoreParams
		{
			get
			{
				return this._varyByParamValues != null && this._varyByParamValues[0].Length == 0;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0000F704 File Offset: 0x0000E704
		internal HttpCacheability CacheabilityInternal
		{
			get
			{
				return this._cacheability;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000F70C File Offset: 0x0000E70C
		internal bool NoStore
		{
			get
			{
				return this._noStore;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000F714 File Offset: 0x0000E714
		internal string[] PrivateFields
		{
			get
			{
				if (this._privateFields != null)
				{
					return (string[])this._privateFields.Clone();
				}
				return null;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000F730 File Offset: 0x0000E730
		internal string[] NoCacheFields
		{
			get
			{
				if (this._noCacheFields != null)
				{
					return (string[])this._noCacheFields.Clone();
				}
				return null;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000351 RID: 849 RVA: 0x0000F74C File Offset: 0x0000E74C
		internal DateTime UtcExpires
		{
			get
			{
				return this._utcExpires;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000F754 File Offset: 0x0000E754
		internal bool IsExpiresSet
		{
			get
			{
				return this._isExpiresSet;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000353 RID: 851 RVA: 0x0000F75C File Offset: 0x0000E75C
		internal TimeSpan MaxAge
		{
			get
			{
				return this._maxAge;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000F764 File Offset: 0x0000E764
		internal bool IsMaxAgeSet
		{
			get
			{
				return this._isMaxAgeSet;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000F76C File Offset: 0x0000E76C
		internal TimeSpan ProxyMaxAge
		{
			get
			{
				return this._proxyMaxAge;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000F774 File Offset: 0x0000E774
		internal bool IsProxyMaxAgeSet
		{
			get
			{
				return this._isProxyMaxAgeSet;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000F77C File Offset: 0x0000E77C
		internal int SlidingExpirationInternal
		{
			get
			{
				return this._slidingExpiration;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000F784 File Offset: 0x0000E784
		internal bool SlidingExpiration
		{
			get
			{
				return this._slidingExpiration == 1;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000359 RID: 857 RVA: 0x0000F78F File Offset: 0x0000E78F
		internal TimeSpan SlidingDelta
		{
			get
			{
				return this._slidingDelta;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600035A RID: 858 RVA: 0x0000F797 File Offset: 0x0000E797
		internal DateTime UtcTimestampCreated
		{
			get
			{
				return this._utcTimestampCreated;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600035B RID: 859 RVA: 0x0000F79F File Offset: 0x0000E79F
		internal int ValidUntilExpiresInternal
		{
			get
			{
				return this._validUntilExpires;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0000F7A7 File Offset: 0x0000E7A7
		internal bool ValidUntilExpires
		{
			get
			{
				return this._validUntilExpires == 1 && !this.SlidingExpiration && !this.GenerateLastModifiedFromFiles && !this.GenerateEtagFromFiles && this.ValidationCallbackInfo == null;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600035D RID: 861 RVA: 0x0000F7D5 File Offset: 0x0000E7D5
		internal int AllowInHistoryInternal
		{
			get
			{
				return this._allowInHistory;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600035E RID: 862 RVA: 0x0000F7DD File Offset: 0x0000E7DD
		internal HttpCacheRevalidation Revalidation
		{
			get
			{
				return this._revalidation;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0000F7E5 File Offset: 0x0000E7E5
		internal DateTime UtcLastModified
		{
			get
			{
				return this._utcLastModified;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000F7ED File Offset: 0x0000E7ED
		internal bool IsLastModifiedSet
		{
			get
			{
				return this._isLastModifiedSet;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000F7F5 File Offset: 0x0000E7F5
		internal string ETag
		{
			get
			{
				return this._etag;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000F7FD File Offset: 0x0000E7FD
		internal bool GenerateLastModifiedFromFiles
		{
			get
			{
				return this._generateLastModifiedFromFiles;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000363 RID: 867 RVA: 0x0000F805 File Offset: 0x0000E805
		internal bool GenerateEtagFromFiles
		{
			get
			{
				return this._generateEtagFromFiles;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000364 RID: 868 RVA: 0x0000F80D File Offset: 0x0000E80D
		internal string VaryByCustom
		{
			get
			{
				return this._varyByCustom;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000365 RID: 869 RVA: 0x0000F815 File Offset: 0x0000E815
		internal bool HasUserProvidedDependencies
		{
			get
			{
				return this._hasUserProvidedDependencies;
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000F81D File Offset: 0x0000E81D
		internal bool HasValidationPolicy()
		{
			return this.ValidUntilExpires || this.GenerateLastModifiedFromFiles || this.GenerateEtagFromFiles || this.ValidationCallbackInfo != null;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000F845 File Offset: 0x0000E845
		internal int OmitVaryStarInternal
		{
			get
			{
				return this._omitVaryStar;
			}
		}

		// Token: 0x04000F6F RID: 3951
		internal readonly bool _isModified;

		// Token: 0x04000F70 RID: 3952
		internal readonly ValidationCallbackInfo[] _validationCallbackInfo;

		// Token: 0x04000F71 RID: 3953
		internal readonly HttpResponseHeader _headerCacheControl;

		// Token: 0x04000F72 RID: 3954
		internal readonly HttpResponseHeader _headerPragma;

		// Token: 0x04000F73 RID: 3955
		internal readonly HttpResponseHeader _headerExpires;

		// Token: 0x04000F74 RID: 3956
		internal readonly HttpResponseHeader _headerLastModified;

		// Token: 0x04000F75 RID: 3957
		internal readonly HttpResponseHeader _headerEtag;

		// Token: 0x04000F76 RID: 3958
		internal readonly HttpResponseHeader _headerVaryBy;

		// Token: 0x04000F77 RID: 3959
		internal readonly bool _hasSetCookieHeader;

		// Token: 0x04000F78 RID: 3960
		internal readonly bool _noServerCaching;

		// Token: 0x04000F79 RID: 3961
		internal readonly string _cacheExtension;

		// Token: 0x04000F7A RID: 3962
		internal readonly bool _noTransforms;

		// Token: 0x04000F7B RID: 3963
		internal readonly bool _ignoreRangeRequests;

		// Token: 0x04000F7C RID: 3964
		internal readonly string[] _varyByContentEncodings;

		// Token: 0x04000F7D RID: 3965
		internal readonly string[] _varyByHeaderValues;

		// Token: 0x04000F7E RID: 3966
		internal readonly string[] _varyByParamValues;

		// Token: 0x04000F7F RID: 3967
		internal readonly string _varyByCustom;

		// Token: 0x04000F80 RID: 3968
		internal readonly HttpCacheability _cacheability;

		// Token: 0x04000F81 RID: 3969
		internal readonly bool _noStore;

		// Token: 0x04000F82 RID: 3970
		internal readonly string[] _privateFields;

		// Token: 0x04000F83 RID: 3971
		internal readonly string[] _noCacheFields;

		// Token: 0x04000F84 RID: 3972
		internal readonly DateTime _utcExpires;

		// Token: 0x04000F85 RID: 3973
		internal readonly bool _isExpiresSet;

		// Token: 0x04000F86 RID: 3974
		internal readonly TimeSpan _maxAge;

		// Token: 0x04000F87 RID: 3975
		internal readonly bool _isMaxAgeSet;

		// Token: 0x04000F88 RID: 3976
		internal readonly TimeSpan _proxyMaxAge;

		// Token: 0x04000F89 RID: 3977
		internal readonly bool _isProxyMaxAgeSet;

		// Token: 0x04000F8A RID: 3978
		internal readonly int _slidingExpiration;

		// Token: 0x04000F8B RID: 3979
		internal readonly TimeSpan _slidingDelta;

		// Token: 0x04000F8C RID: 3980
		internal readonly DateTime _utcTimestampCreated;

		// Token: 0x04000F8D RID: 3981
		internal readonly int _validUntilExpires;

		// Token: 0x04000F8E RID: 3982
		internal readonly int _allowInHistory;

		// Token: 0x04000F8F RID: 3983
		internal readonly HttpCacheRevalidation _revalidation;

		// Token: 0x04000F90 RID: 3984
		internal readonly DateTime _utcLastModified;

		// Token: 0x04000F91 RID: 3985
		internal readonly bool _isLastModifiedSet;

		// Token: 0x04000F92 RID: 3986
		internal readonly string _etag;

		// Token: 0x04000F93 RID: 3987
		internal readonly bool _generateLastModifiedFromFiles;

		// Token: 0x04000F94 RID: 3988
		internal readonly bool _generateEtagFromFiles;

		// Token: 0x04000F95 RID: 3989
		internal readonly int _omitVaryStar;

		// Token: 0x04000F96 RID: 3990
		internal readonly bool _hasUserProvidedDependencies;
	}
}
