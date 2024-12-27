using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace System.Net.Cache
{
	// Token: 0x02000580 RID: 1408
	internal class RequestCacheProtocol
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000B73B7 File Offset: 0x000B63B7
		internal CacheValidationStatus ProtocolStatus
		{
			get
			{
				return this._ProtocolStatus;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000B73BF File Offset: 0x000B63BF
		internal Exception ProtocolException
		{
			get
			{
				return this._ProtocolException;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002B1C RID: 11036 RVA: 0x000B73C7 File Offset: 0x000B63C7
		internal Stream ResponseStream
		{
			get
			{
				return this._ResponseStream;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002B1D RID: 11037 RVA: 0x000B73CF File Offset: 0x000B63CF
		internal long ResponseStreamLength
		{
			get
			{
				return this._ResponseStreamLength;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x000B73D7 File Offset: 0x000B63D7
		internal RequestCacheValidator Validator
		{
			get
			{
				return this._Validator;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000B73DF File Offset: 0x000B63DF
		internal bool IsCacheFresh
		{
			get
			{
				return this._Validator != null && this._Validator.CacheFreshnessStatus == CacheFreshnessStatus.Fresh;
			}
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000B73F9 File Offset: 0x000B63F9
		internal RequestCacheProtocol(RequestCache cache, RequestCacheValidator defaultValidator)
		{
			this._RequestCache = cache;
			this._Validator = defaultValidator;
			this._CanTakeNewRequest = true;
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000B7418 File Offset: 0x000B6418
		internal CacheValidationStatus GetRetrieveStatus(Uri cacheUri, WebRequest request)
		{
			if (cacheUri == null)
			{
				throw new ArgumentNullException("cacheUri");
			}
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (!this._CanTakeNewRequest || this._ProtocolStatus == CacheValidationStatus.RetryResponseFromServer)
			{
				return CacheValidationStatus.Continue;
			}
			this._CanTakeNewRequest = false;
			this._ResponseStream = null;
			this._ResponseStreamLength = 0L;
			this._ProtocolStatus = CacheValidationStatus.Continue;
			this._ProtocolException = null;
			if (Logging.On)
			{
				Logging.Enter(Logging.RequestCache, this, "GetRetrieveStatus", request);
			}
			try
			{
				if (request.CachePolicy == null || request.CachePolicy.Level == RequestCacheLevel.BypassCache)
				{
					this._ProtocolStatus = CacheValidationStatus.DoNotUseCache;
					return this._ProtocolStatus;
				}
				if (this._RequestCache == null || this._Validator == null)
				{
					this._ProtocolStatus = CacheValidationStatus.DoNotUseCache;
					return this._ProtocolStatus;
				}
				this._Validator.FetchRequest(cacheUri, request);
				CacheValidationStatus cacheValidationStatus = (this._ProtocolStatus = this.ValidateRequest());
				switch (cacheValidationStatus)
				{
				case CacheValidationStatus.DoNotUseCache:
				case CacheValidationStatus.DoNotTakeFromCache:
					break;
				case CacheValidationStatus.Fail:
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[] { "ValidateRequest" }));
					break;
				default:
					if (cacheValidationStatus != CacheValidationStatus.Continue)
					{
						this._ProtocolStatus = CacheValidationStatus.Fail;
						this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
						{
							"ValidateRequest",
							this._Validator.ValidationStatus.ToString()
						}));
						if (Logging.On)
						{
							Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
							{
								"ValidateRequest()",
								this._Validator.ValidationStatus.ToString()
							}));
						}
					}
					break;
				}
				if (this._ProtocolStatus != CacheValidationStatus.Continue)
				{
					return this._ProtocolStatus;
				}
				this.CheckRetrieveBeforeSubmit();
			}
			catch (Exception ex)
			{
				this._ProtocolException = ex;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, this, "GetRetrieveStatus", "result = " + this._ProtocolStatus.ToString());
				}
			}
			return this._ProtocolStatus;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000B76F8 File Offset: 0x000B66F8
		internal CacheValidationStatus GetRevalidateStatus(WebResponse response, Stream responseStream)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			if (this._ProtocolStatus == CacheValidationStatus.DoNotUseCache)
			{
				return CacheValidationStatus.DoNotUseCache;
			}
			if (this._ProtocolStatus == CacheValidationStatus.ReturnCachedResponse)
			{
				this._ProtocolStatus = CacheValidationStatus.DoNotUseCache;
				return this._ProtocolStatus;
			}
			try
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.RequestCache, this, "GetRevalidateStatus", (this._Validator == null) ? null : this._Validator.Request);
				}
				this._Validator.FetchResponse(response);
				if (this._ProtocolStatus != CacheValidationStatus.Continue && this._ProtocolStatus != CacheValidationStatus.RetryResponseFromServer)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_revalidation_not_needed", new object[] { "GetRevalidateStatus()" }));
					}
					return this._ProtocolStatus;
				}
				this.CheckRetrieveOnResponse(responseStream);
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, this, "GetRevalidateStatus", "result = " + this._ProtocolStatus.ToString());
				}
			}
			return this._ProtocolStatus;
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000B7808 File Offset: 0x000B6808
		internal CacheValidationStatus GetUpdateStatus(WebResponse response, Stream responseStream)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			if (this._ProtocolStatus == CacheValidationStatus.DoNotUseCache)
			{
				return CacheValidationStatus.DoNotUseCache;
			}
			try
			{
				if (Logging.On)
				{
					Logging.Enter(Logging.RequestCache, this, "GetUpdateStatus", null);
				}
				if (this._Validator.Response == null)
				{
					this._Validator.FetchResponse(response);
				}
				if (this._ProtocolStatus == CacheValidationStatus.RemoveFromCache)
				{
					this.EnsureCacheRemoval(this._Validator.CacheKey);
					return this._ProtocolStatus;
				}
				if (this._ProtocolStatus != CacheValidationStatus.DoNotTakeFromCache && this._ProtocolStatus != CacheValidationStatus.ReturnCachedResponse && this._ProtocolStatus != CacheValidationStatus.CombineCachedAndServerResponse)
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_not_updated_based_on_cache_protocol_status", new object[]
						{
							"GetUpdateStatus()",
							this._ProtocolStatus.ToString()
						}));
					}
					return this._ProtocolStatus;
				}
				this.CheckUpdateOnResponse(responseStream);
			}
			catch (Exception ex)
			{
				this._ProtocolException = ex;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (Logging.On)
				{
					Logging.Exit(Logging.RequestCache, this, "GetUpdateStatus", "result = " + this._ProtocolStatus.ToString());
				}
			}
			return this._ProtocolStatus;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000B79F0 File Offset: 0x000B69F0
		internal void Reset()
		{
			this._CanTakeNewRequest = true;
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000B79FC File Offset: 0x000B69FC
		internal void Abort()
		{
			if (this._CanTakeNewRequest)
			{
				return;
			}
			Stream responseStream = this._ResponseStream;
			if (responseStream != null)
			{
				try
				{
					if (Logging.On)
					{
						Logging.PrintWarning(Logging.RequestCache, SR.GetString("net_log_cache_closing_cache_stream", new object[]
						{
							"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
							"Abort()",
							responseStream.GetType().FullName,
							this._Validator.CacheKey
						}));
					}
					ICloseEx closeEx = responseStream as ICloseEx;
					if (closeEx != null)
					{
						closeEx.CloseEx(CloseExState.Abort | CloseExState.Silent);
					}
					else
					{
						responseStream.Close();
					}
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_exception_ignored", new object[]
						{
							"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
							"stream.Close()",
							ex.ToString()
						}));
					}
				}
			}
			this.Reset();
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000B7B20 File Offset: 0x000B6B20
		private void CheckRetrieveBeforeSubmit()
		{
			try
			{
				CacheValidationStatus protocolStatus;
				for (;;)
				{
					if (this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
					{
						this._Validator.CacheStream.Close();
						this._Validator.CacheStream = Stream.Null;
					}
					RequestCacheEntry requestCacheEntry;
					if (this._Validator.StrictCacheErrors)
					{
						this._Validator.CacheStream = this._RequestCache.Retrieve(this._Validator.CacheKey, out requestCacheEntry);
					}
					else
					{
						Stream stream;
						this._RequestCache.TryRetrieve(this._Validator.CacheKey, out requestCacheEntry, out stream);
						this._Validator.CacheStream = stream;
					}
					if (requestCacheEntry == null)
					{
						requestCacheEntry = new RequestCacheEntry();
						requestCacheEntry.IsPrivateEntry = this._RequestCache.IsPrivateCache;
						this._Validator.FetchCacheEntry(requestCacheEntry);
					}
					if (this._Validator.CacheStream == null)
					{
						this._Validator.CacheStream = Stream.Null;
					}
					this.ValidateFreshness(requestCacheEntry);
					this._ProtocolStatus = this.ValidateCache();
					protocolStatus = this._ProtocolStatus;
					switch (protocolStatus)
					{
					case CacheValidationStatus.DoNotUseCache:
					case CacheValidationStatus.DoNotTakeFromCache:
						goto IL_035A;
					case CacheValidationStatus.Fail:
						goto IL_029E;
					case CacheValidationStatus.RetryResponseFromCache:
						continue;
					case CacheValidationStatus.RetryResponseFromServer:
						goto IL_02CB;
					case CacheValidationStatus.ReturnCachedResponse:
						goto IL_0123;
					}
					break;
				}
				if (protocolStatus != CacheValidationStatus.Continue)
				{
					goto IL_02CB;
				}
				this._ResponseStream = this._Validator.CacheStream;
				goto IL_035A;
				IL_0123:
				if (this._Validator.CacheStream == null || this._Validator.CacheStream == Stream.Null)
				{
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_no_cache_entry", new object[] { "ValidateCache()" }));
					}
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_no_stream", new object[] { this._Validator.CacheKey }));
					goto IL_035A;
				}
				Stream stream2 = this._Validator.CacheStream;
				this._RequestCache.UnlockEntry(this._Validator.CacheStream);
				if (this._Validator.CacheStreamOffset != 0L || this._Validator.CacheStreamLength != this._Validator.CacheEntry.StreamSize)
				{
					stream2 = new RangeStream(stream2, this._Validator.CacheStreamOffset, this._Validator.CacheStreamLength);
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_returned_range_cache", new object[]
						{
							"ValidateCache()",
							this._Validator.CacheStreamOffset,
							this._Validator.CacheStreamLength
						}));
					}
				}
				this._ResponseStream = stream2;
				this._ResponseStreamLength = this._Validator.CacheStreamLength;
				goto IL_035A;
				IL_029E:
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[] { "ValidateCache" }));
				goto IL_035A;
				IL_02CB:
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"ValidateCache",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"ValidateCache()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_035A:;
			}
			catch (Exception ex)
			{
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = ex;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (this._ResponseStream == null && this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
				{
					this._Validator.CacheStream.Close();
					this._Validator.CacheStream = Stream.Null;
				}
			}
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000B7F98 File Offset: 0x000B6F98
		private void CheckRetrieveOnResponse(Stream responseStream)
		{
			bool flag = true;
			try
			{
				CacheValidationStatus cacheValidationStatus = (this._ProtocolStatus = this.ValidateResponse());
				switch (cacheValidationStatus)
				{
				case CacheValidationStatus.DoNotUseCache:
					goto IL_0107;
				case CacheValidationStatus.Fail:
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[] { "ValidateResponse" }));
					goto IL_0107;
				case CacheValidationStatus.DoNotTakeFromCache:
				case CacheValidationStatus.RetryResponseFromCache:
					break;
				case CacheValidationStatus.RetryResponseFromServer:
					flag = false;
					goto IL_0107;
				default:
					if (cacheValidationStatus == CacheValidationStatus.Continue)
					{
						flag = false;
						goto IL_0107;
					}
					break;
				}
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"ValidateResponse",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"ValidateResponse()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_0107:;
			}
			catch (Exception ex)
			{
				flag = true;
				this._ProtocolException = ex;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex is WebException) ? ex.Message : ex.ToString()
					}));
				}
			}
			finally
			{
				if (flag && this._ResponseStream != null)
				{
					this._ResponseStream.Close();
					this._ResponseStream = null;
					this._Validator.CacheStream = Stream.Null;
				}
			}
			if (this._ProtocolStatus != CacheValidationStatus.Continue)
			{
				return;
			}
			try
			{
				switch (this._ProtocolStatus = this.RevalidateCache())
				{
				case CacheValidationStatus.DoNotUseCache:
				case CacheValidationStatus.DoNotTakeFromCache:
				case CacheValidationStatus.RemoveFromCache:
					flag = true;
					goto IL_04FF;
				case CacheValidationStatus.Fail:
					flag = true;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[] { "RevalidateCache" }));
					goto IL_04FF;
				case CacheValidationStatus.ReturnCachedResponse:
					if (this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
					{
						Stream stream = this._Validator.CacheStream;
						if (this._Validator.CacheStreamOffset != 0L || this._Validator.CacheStreamLength != this._Validator.CacheEntry.StreamSize)
						{
							stream = new RangeStream(stream, this._Validator.CacheStreamOffset, this._Validator.CacheStreamLength);
							if (Logging.On)
							{
								Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_returned_range_cache", new object[]
								{
									"RevalidateCache()",
									this._Validator.CacheStreamOffset,
									this._Validator.CacheStreamLength
								}));
							}
						}
						this._ResponseStream = stream;
						this._ResponseStreamLength = this._Validator.CacheStreamLength;
						goto IL_04FF;
					}
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_no_stream", new object[] { this._Validator.CacheKey }));
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_null_cached_stream", new object[] { "RevalidateCache()" }));
						goto IL_04FF;
					}
					goto IL_04FF;
				case CacheValidationStatus.CombineCachedAndServerResponse:
					if (this._Validator.CacheStream != null && this._Validator.CacheStream != Stream.Null)
					{
						Stream stream;
						if (responseStream != null)
						{
							stream = new CombinedReadStream(this._Validator.CacheStream, responseStream);
						}
						else
						{
							stream = this._Validator.CacheStream;
						}
						this._ResponseStream = stream;
						this._ResponseStreamLength = this._Validator.CacheStreamLength;
						goto IL_04FF;
					}
					this._ProtocolStatus = CacheValidationStatus.Fail;
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_no_stream", new object[] { this._Validator.CacheKey }));
					if (Logging.On)
					{
						Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_requested_combined_but_null_cached_stream", new object[] { "RevalidateCache()" }));
						goto IL_04FF;
					}
					goto IL_04FF;
				}
				flag = true;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"RevalidateCache",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"RevalidateCache()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_04FF:;
			}
			catch (Exception ex2)
			{
				flag = true;
				this._ProtocolException = ex2;
				this._ProtocolStatus = CacheValidationStatus.Fail;
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_object_and_exception", new object[]
					{
						"CacheProtocol#" + this.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
						(ex2 is WebException) ? ex2.Message : ex2.ToString()
					}));
				}
			}
			finally
			{
				if (flag && this._ResponseStream != null)
				{
					this._ResponseStream.Close();
					this._ResponseStream = null;
					this._Validator.CacheStream = Stream.Null;
				}
			}
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000B85CC File Offset: 0x000B75CC
		private void CheckUpdateOnResponse(Stream responseStream)
		{
			if (this._Validator.CacheEntry == null)
			{
				RequestCacheEntry requestCacheEntry = new RequestCacheEntry();
				requestCacheEntry.IsPrivateEntry = this._RequestCache.IsPrivateCache;
				this._Validator.FetchCacheEntry(requestCacheEntry);
			}
			string cacheKey = this._Validator.CacheKey;
			bool flag = true;
			try
			{
				switch (this._ProtocolStatus = this.UpdateCache())
				{
				case CacheValidationStatus.DoNotUseCache:
				case CacheValidationStatus.DoNotUpdateCache:
					goto IL_0327;
				case CacheValidationStatus.Fail:
					this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_fail", new object[] { "UpdateCache" }));
					goto IL_0327;
				case CacheValidationStatus.CacheResponse:
				{
					Stream stream;
					if (this._Validator.StrictCacheErrors)
					{
						stream = this._RequestCache.Store(this._Validator.CacheKey, this._Validator.CacheEntry.StreamSize, this._Validator.CacheEntry.ExpiresUtc, this._Validator.CacheEntry.LastModifiedUtc, this._Validator.CacheEntry.MaxStale, this._Validator.CacheEntry.EntryMetadata, this._Validator.CacheEntry.SystemMetadata);
					}
					else
					{
						this._RequestCache.TryStore(this._Validator.CacheKey, this._Validator.CacheEntry.StreamSize, this._Validator.CacheEntry.ExpiresUtc, this._Validator.CacheEntry.LastModifiedUtc, this._Validator.CacheEntry.MaxStale, this._Validator.CacheEntry.EntryMetadata, this._Validator.CacheEntry.SystemMetadata, out stream);
					}
					if (stream == null)
					{
						this._ProtocolStatus = CacheValidationStatus.DoNotUpdateCache;
						goto IL_0327;
					}
					this._ResponseStream = new ForwardingReadStream(responseStream, stream, this._Validator.CacheStreamOffset, this._Validator.StrictCacheErrors);
					this._ProtocolStatus = CacheValidationStatus.UpdateResponseInformation;
					goto IL_0327;
				}
				case CacheValidationStatus.UpdateResponseInformation:
					this._ResponseStream = new MetadataUpdateStream(responseStream, this._RequestCache, this._Validator.CacheKey, this._Validator.CacheEntry.ExpiresUtc, this._Validator.CacheEntry.LastModifiedUtc, this._Validator.CacheEntry.LastSynchronizedUtc, this._Validator.CacheEntry.MaxStale, this._Validator.CacheEntry.EntryMetadata, this._Validator.CacheEntry.SystemMetadata, this._Validator.StrictCacheErrors);
					flag = false;
					this._ProtocolStatus = CacheValidationStatus.UpdateResponseInformation;
					goto IL_0327;
				case CacheValidationStatus.RemoveFromCache:
					this.EnsureCacheRemoval(cacheKey);
					flag = false;
					goto IL_0327;
				}
				this._ProtocolStatus = CacheValidationStatus.Fail;
				this._ProtocolException = new InvalidOperationException(SR.GetString("net_cache_validator_result", new object[]
				{
					"UpdateCache",
					this._Validator.ValidationStatus.ToString()
				}));
				if (Logging.On)
				{
					Logging.PrintError(Logging.RequestCache, SR.GetString("net_log_cache_unexpected_status", new object[]
					{
						"UpdateCache()",
						this._Validator.ValidationStatus.ToString()
					}));
				}
				IL_0327:;
			}
			finally
			{
				if (flag)
				{
					this._RequestCache.UnlockEntry(this._Validator.CacheStream);
				}
			}
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000B8938 File Offset: 0x000B7938
		private CacheValidationStatus ValidateRequest()
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, string.Concat(new object[]
				{
					"Request#",
					this._Validator.Request.GetHashCode().ToString(NumberFormatInfo.InvariantInfo),
					", Policy = ",
					this._Validator.Request.CachePolicy.ToString(),
					", Cache Uri = ",
					this._Validator.Uri
				}));
			}
			CacheValidationStatus cacheValidationStatus = this._Validator.ValidateRequest();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, "Selected cache Key = " + this._Validator.CacheKey);
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000B8A00 File Offset: 0x000B7A00
		private void ValidateFreshness(RequestCacheEntry fetchEntry)
		{
			this._Validator.FetchCacheEntry(fetchEntry);
			if (this._Validator.CacheStream == null || this._Validator.CacheStream == Stream.Null)
			{
				if (Logging.On)
				{
					Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_entry_not_found_freshness_undefined", new object[] { "ValidateFreshness()" }));
				}
				this._Validator.SetFreshnessStatus(CacheFreshnessStatus.Undefined);
				return;
			}
			if (Logging.On && Logging.IsVerbose(Logging.RequestCache))
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_dumping_cache_context"));
				if (fetchEntry == null)
				{
					Logging.PrintInfo(Logging.RequestCache, "<null>");
				}
				else
				{
					string[] array = fetchEntry.ToString(Logging.IsVerbose(Logging.RequestCache)).Split(RequestCache.LineSplits);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Length != 0)
						{
							Logging.PrintInfo(Logging.RequestCache, array[i]);
						}
					}
				}
			}
			CacheFreshnessStatus cacheFreshnessStatus = this._Validator.ValidateFreshness();
			this._Validator.SetFreshnessStatus(cacheFreshnessStatus);
			this._IsCacheFresh = cacheFreshnessStatus == CacheFreshnessStatus.Fresh;
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"ValidateFreshness()",
					cacheFreshnessStatus.ToString()
				}));
			}
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000B8B4C File Offset: 0x000B7B4C
		private CacheValidationStatus ValidateCache()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.ValidateCache();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"ValidateCache()",
					cacheValidationStatus.ToString()
				}));
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000B8BAC File Offset: 0x000B7BAC
		private CacheValidationStatus RevalidateCache()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.RevalidateCache();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"RevalidateCache()",
					cacheValidationStatus.ToString()
				}));
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000B8C0C File Offset: 0x000B7C0C
		private CacheValidationStatus ValidateResponse()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.ValidateResponse();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.RequestCache, SR.GetString("net_log_cache_result", new object[]
				{
					"ValidateResponse()",
					cacheValidationStatus.ToString()
				}));
			}
			return cacheValidationStatus;
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000B8C6C File Offset: 0x000B7C6C
		private CacheValidationStatus UpdateCache()
		{
			CacheValidationStatus cacheValidationStatus = this._Validator.UpdateCache();
			this._Validator.SetValidationStatus(cacheValidationStatus);
			return cacheValidationStatus;
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000B8C94 File Offset: 0x000B7C94
		private void EnsureCacheRemoval(string retrieveKey)
		{
			this._RequestCache.UnlockEntry(this._Validator.CacheStream);
			if (this._Validator.StrictCacheErrors)
			{
				this._RequestCache.Remove(retrieveKey);
			}
			else
			{
				this._RequestCache.TryRemove(retrieveKey);
			}
			if (retrieveKey != this._Validator.CacheKey)
			{
				if (this._Validator.StrictCacheErrors)
				{
					this._RequestCache.Remove(this._Validator.CacheKey);
					return;
				}
				this._RequestCache.TryRemove(this._Validator.CacheKey);
			}
		}

		// Token: 0x040029A3 RID: 10659
		private CacheValidationStatus _ProtocolStatus;

		// Token: 0x040029A4 RID: 10660
		private Exception _ProtocolException;

		// Token: 0x040029A5 RID: 10661
		private Stream _ResponseStream;

		// Token: 0x040029A6 RID: 10662
		private long _ResponseStreamLength;

		// Token: 0x040029A7 RID: 10663
		private RequestCacheValidator _Validator;

		// Token: 0x040029A8 RID: 10664
		private RequestCache _RequestCache;

		// Token: 0x040029A9 RID: 10665
		private bool _IsCacheFresh;

		// Token: 0x040029AA RID: 10666
		private bool _CanTakeNewRequest;
	}
}
