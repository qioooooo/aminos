using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Management;
using System.Web.Security;
using System.Web.UI;
using System.Web.Util;
using Microsoft.Win32.SafeHandles;

namespace System.Web
{
	// Token: 0x02000084 RID: 132
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpResponse
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x000185A8 File Offset: 0x000175A8
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x000185B0 File Offset: 0x000175B0
		internal HttpContext Context
		{
			get
			{
				return this._context;
			}
			set
			{
				this._context = value;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x000185B9 File Offset: 0x000175B9
		internal HttpRequest Request
		{
			get
			{
				if (this._context == null)
				{
					return null;
				}
				return this._context.Request;
			}
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000185D0 File Offset: 0x000175D0
		internal HttpResponse(HttpWorkerRequest wr, HttpContext context)
		{
			this._wr = wr;
			this._context = context;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001860C File Offset: 0x0001760C
		public HttpResponse(TextWriter writer)
		{
			this._wr = null;
			this._httpWriter = null;
			this._writer = writer;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x00018658 File Offset: 0x00017658
		private bool UsingHttpWriter
		{
			get
			{
				return this._httpWriter != null && this._writer == this._httpWriter;
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00018672 File Offset: 0x00017672
		internal void Dispose()
		{
			if (this._httpWriter != null)
			{
				this._httpWriter.RecycleBuffers();
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00018687 File Offset: 0x00017687
		internal void InitResponseWriter()
		{
			if (this._httpWriter == null)
			{
				this._httpWriter = new HttpWriter(this);
				this._writer = this._httpWriter;
			}
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000186AC File Offset: 0x000176AC
		private void AppendHeader(HttpResponseHeader h)
		{
			if (this._customHeaders == null)
			{
				this._customHeaders = new ArrayList();
			}
			this._customHeaders.Add(h);
			if (this._cachePolicy != null && StringUtil.EqualsIgnoreCase("Set-Cookie", h.Name))
			{
				this._cachePolicy.SetHasSetCookieHeader();
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x000186FE File Offset: 0x000176FE
		// (set) Token: 0x060005DD RID: 1501 RVA: 0x00018706 File Offset: 0x00017706
		internal bool HeadersWritten
		{
			get
			{
				return this._headersWritten;
			}
			set
			{
				this._headersWritten = value;
			}
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x00018710 File Offset: 0x00017710
		internal ArrayList GenerateResponseHeadersIntegrated(bool forCache)
		{
			ArrayList arrayList = new ArrayList();
			HttpHeaderCollection httpHeaderCollection = this.Headers as HttpHeaderCollection;
			foreach (object obj in httpHeaderCollection)
			{
				string text = (string)obj;
				int knownResponseHeaderIndex = HttpWorkerRequest.GetKnownResponseHeaderIndex(text);
				if (knownResponseHeaderIndex < 0 || !forCache || (knownResponseHeaderIndex != 26 && knownResponseHeaderIndex != 27 && knownResponseHeaderIndex != 0 && knownResponseHeaderIndex != 18 && knownResponseHeaderIndex != 19 && knownResponseHeaderIndex != 22 && knownResponseHeaderIndex != 28))
				{
					if (knownResponseHeaderIndex >= 0)
					{
						arrayList.Add(new HttpResponseHeader(knownResponseHeaderIndex, httpHeaderCollection[text]));
					}
					else
					{
						arrayList.Add(new HttpResponseHeader(text, httpHeaderCollection[text]));
					}
				}
			}
			return arrayList;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000187D8 File Offset: 0x000177D8
		internal void GenerateResponseHeadersForCookies()
		{
			if (this._cookies == null || (this._cookies.Count == 0 && !this._cookies.Changed))
			{
				return;
			}
			HttpHeaderCollection httpHeaderCollection = this.Headers as HttpHeaderCollection;
			bool flag = false;
			if (!this._cookies.Changed)
			{
				for (int i = 0; i < this._cookies.Count; i++)
				{
					HttpCookie httpCookie = this._cookies[i];
					if (httpCookie.Added)
					{
						HttpResponseHeader httpResponseHeader = httpCookie.GetSetCookieHeader(this._context);
						httpHeaderCollection.SetHeader(httpResponseHeader.Name, httpResponseHeader.Value, false);
						httpCookie.Added = false;
						httpCookie.Changed = false;
					}
					else if (httpCookie.Changed)
					{
						flag = true;
						break;
					}
				}
			}
			if (this._cookies.Changed || flag)
			{
				httpHeaderCollection.Remove("Set-Cookie");
				for (int j = 0; j < this._cookies.Count; j++)
				{
					HttpCookie httpCookie = this._cookies[j];
					HttpResponseHeader httpResponseHeader = httpCookie.GetSetCookieHeader(this._context);
					httpHeaderCollection.SetHeader(httpResponseHeader.Name, httpResponseHeader.Value, false);
					httpCookie.Added = false;
					httpCookie.Changed = false;
				}
				this._cookies.Changed = false;
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x00018910 File Offset: 0x00017910
		internal void GenerateResponseHeadersForHandler()
		{
			if (!(this._wr is IIS7WorkerRequest))
			{
				return;
			}
			string text = null;
			if (!this._headersWritten && !this._handlerHeadersGenerated)
			{
				try
				{
					RuntimeConfig lkgconfig = RuntimeConfig.GetLKGConfig(this._context);
					HttpRuntimeSection httpRuntime = lkgconfig.HttpRuntime;
					if (httpRuntime != null)
					{
						text = httpRuntime.VersionHeader;
						this._sendCacheControlHeader = httpRuntime.SendCacheControlHeader;
					}
					OutputCacheSection outputCache = lkgconfig.OutputCache;
					if (outputCache != null)
					{
						this._sendCacheControlHeader &= outputCache.SendCacheControlHeader;
					}
					if (this._sendCacheControlHeader && !this._cacheControlHeaderAdded)
					{
						this.Headers.Set("Cache-Control", "private");
					}
					if (!string.IsNullOrEmpty(text))
					{
						this.Headers.Set("X-AspNet-Version", text);
					}
					this._contentTypeSet = true;
				}
				finally
				{
					this._handlerHeadersGenerated = true;
				}
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000189E8 File Offset: 0x000179E8
		internal ArrayList GenerateResponseHeaders(bool forCache)
		{
			ArrayList arrayList = new ArrayList();
			bool flag = true;
			if (!forCache && !this._versionHeaderSent)
			{
				string text = null;
				RuntimeConfig lkgconfig = RuntimeConfig.GetLKGConfig(this._context);
				HttpRuntimeSection httpRuntime = lkgconfig.HttpRuntime;
				if (httpRuntime != null)
				{
					text = httpRuntime.VersionHeader;
					flag = httpRuntime.SendCacheControlHeader;
				}
				OutputCacheSection outputCache = lkgconfig.OutputCache;
				if (outputCache != null)
				{
					flag &= outputCache.SendCacheControlHeader;
				}
				if (!string.IsNullOrEmpty(text))
				{
					arrayList.Add(new HttpResponseHeader("X-AspNet-Version", text));
				}
				this._versionHeaderSent = true;
			}
			if (this._customHeaders != null)
			{
				int count = this._customHeaders.Count;
				for (int i = 0; i < count; i++)
				{
					arrayList.Add(this._customHeaders[i]);
				}
			}
			if (this._redirectLocation != null)
			{
				arrayList.Add(new HttpResponseHeader(23, this._redirectLocation));
			}
			if (!forCache)
			{
				if (this._cookies != null)
				{
					int count2 = this._cookies.Count;
					for (int j = 0; j < count2; j++)
					{
						arrayList.Add(this._cookies[j].GetSetCookieHeader(this.Context));
					}
				}
				if (this._cachePolicy != null && this._cachePolicy.IsModified())
				{
					this._cachePolicy.GetHeaders(arrayList, this);
				}
				else
				{
					if (this._cacheHeaders != null)
					{
						arrayList.AddRange(this._cacheHeaders);
					}
					if (!this._cacheControlHeaderAdded && flag)
					{
						arrayList.Add(new HttpResponseHeader(0, "private"));
					}
				}
			}
			if (this._statusCode != 204 && this._contentType != null)
			{
				string text2 = this.AppendCharSetToContentType(this._contentType);
				arrayList.Add(new HttpResponseHeader(12, text2));
			}
			return arrayList;
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00018B94 File Offset: 0x00017B94
		internal string AppendCharSetToContentType(string contentType)
		{
			string text = contentType;
			if ((this._customCharSet || (this._httpWriter != null && this._httpWriter.ResponseEncodingUsed)) && contentType.IndexOf("charset=", StringComparison.Ordinal) < 0)
			{
				string charset = this.Charset;
				if (charset.Length > 0)
				{
					text = contentType + "; charset=" + charset;
				}
			}
			return text;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x00018BED File Offset: 0x00017BED
		// (set) Token: 0x060005E4 RID: 1508 RVA: 0x00018BF5 File Offset: 0x00017BF5
		internal bool UseAdaptiveError
		{
			get
			{
				return this._useAdaptiveError;
			}
			set
			{
				this._useAdaptiveError = value;
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00018C00 File Offset: 0x00017C00
		private void WriteHeaders()
		{
			if (this._wr == null)
			{
				return;
			}
			if (this._context != null && this._context.ApplicationInstance != null)
			{
				this._context.ApplicationInstance.RaiseOnPreSendRequestHeaders();
			}
			if (this.UseAdaptiveError)
			{
				int statusCode = this.StatusCode;
				if (statusCode >= 400 && statusCode < 600)
				{
					this.StatusCode = 200;
				}
			}
			this._wr.SendStatus(this.StatusCode, this.StatusDescription);
			this._wr.SetHeaderEncoding(this.HeaderEncoding);
			ArrayList arrayList = this.GenerateResponseHeaders(false);
			int num = ((arrayList != null) ? arrayList.Count : 0);
			for (int i = 0; i < num; i++)
			{
				HttpResponseHeader httpResponseHeader = arrayList[i] as HttpResponseHeader;
				httpResponseHeader.Send(this._wr);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00018CCE File Offset: 0x00017CCE
		internal int GetBufferedLength()
		{
			if (this._httpWriter == null)
			{
				return 0;
			}
			return Convert.ToInt32(this._httpWriter.GetBufferedLength());
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00018CEC File Offset: 0x00017CEC
		private void Flush(bool finalFlush)
		{
			if (this._completed || this._flushing)
			{
				return;
			}
			if (this._httpWriter == null)
			{
				this._writer.Flush();
				return;
			}
			this._flushing = true;
			try
			{
				IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
				if (iis7WorkerRequest != null)
				{
					this.GenerateResponseHeadersForHandler();
					this.UpdateNativeResponse(true);
					iis7WorkerRequest.ExplicitFlush();
					this._headersWritten = true;
				}
				else
				{
					long num = 0L;
					if (!this._headersWritten)
					{
						if (!this._suppressHeaders && !this._clientDisconnected)
						{
							if (finalFlush)
							{
								num = this._httpWriter.GetBufferedLength();
								if (!this._contentLengthSet && num == 0L && this._httpWriter != null)
								{
									this._contentType = null;
								}
								if (this._cachePolicy != null && this._cookies != null && this._cookies.Count != 0)
								{
									this._cachePolicy.SetHasSetCookieHeader();
									this.DisableKernelCache();
								}
								this.WriteHeaders();
								num = this._httpWriter.GetBufferedLength();
								if (!this._contentLengthSet && this._statusCode != 304)
								{
									this._wr.SendCalculatedContentLength(num);
								}
							}
							else
							{
								if (!this._contentLengthSet && !this._transferEncodingSet && this._statusCode == 200)
								{
									string httpVersion = this._wr.GetHttpVersion();
									if (httpVersion != null && httpVersion.Equals("HTTP/1.1"))
									{
										this.AppendHeader(new HttpResponseHeader(6, "chunked"));
										this._chunked = true;
									}
									num = this._httpWriter.GetBufferedLength();
								}
								this.WriteHeaders();
							}
						}
						this._headersWritten = true;
					}
					else
					{
						num = this._httpWriter.GetBufferedLength();
					}
					if (!this._filteringCompleted)
					{
						this._httpWriter.Filter(false);
						num = this._httpWriter.GetBufferedLength();
					}
					if (!this._suppressContentSet && this.Request != null && this.Request.HttpVerb == HttpVerb.HEAD)
					{
						this._suppressContent = true;
					}
					if (this._suppressContent || this._ended)
					{
						this._httpWriter.ClearBuffers();
						num = 0L;
					}
					if (!this._clientDisconnected)
					{
						if (this._context != null && this._context.ApplicationInstance != null)
						{
							this._context.ApplicationInstance.RaiseOnPreSendRequestContent();
						}
						if (this._chunked)
						{
							if (num > 0L)
							{
								byte[] bytes = Encoding.ASCII.GetBytes(Convert.ToString(num, 16) + "\r\n");
								this._wr.SendResponseFromMemory(bytes, bytes.Length);
								this._httpWriter.Send(this._wr);
								this._wr.SendResponseFromMemory(HttpResponse.s_chunkSuffix, HttpResponse.s_chunkSuffix.Length);
							}
							if (finalFlush)
							{
								this._wr.SendResponseFromMemory(HttpResponse.s_chunkEnd, HttpResponse.s_chunkEnd.Length);
							}
						}
						else
						{
							this._httpWriter.Send(this._wr);
						}
						this._wr.FlushResponse(finalFlush);
						this._wr.UpdateResponseCounters(finalFlush, (int)num);
						if (!finalFlush)
						{
							this._httpWriter.ClearBuffers();
						}
					}
				}
			}
			finally
			{
				this._flushing = false;
				if (finalFlush)
				{
					this._completed = true;
				}
			}
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00018FFC File Offset: 0x00017FFC
		internal void FinalFlushAtTheEndOfRequestProcessing()
		{
			this.FinalFlushAtTheEndOfRequestProcessing(false);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00019005 File Offset: 0x00018005
		internal void FinalFlushAtTheEndOfRequestProcessing(bool needPipelineCompletion)
		{
			this.Flush(true);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00019010 File Offset: 0x00018010
		internal string SetupKernelCaching(string originalCacheUrl)
		{
			if (this._cookies != null && this._cookies.Count != 0)
			{
				this._cachePolicy.SetHasSetCookieHeader();
				return null;
			}
			bool flag = this.IsKernelCacheEnabledForVaryByStar();
			if (!this._cachePolicy.IsKernelCacheable(this.Request, flag))
			{
				return null;
			}
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetLKGConfig(this._context).HttpRuntime;
			if (httpRuntime == null || !httpRuntime.EnableKernelOutputCache)
			{
				return null;
			}
			double totalSeconds = (this._cachePolicy.UtcGetAbsoluteExpiration() - DateTime.UtcNow).TotalSeconds;
			if (totalSeconds <= 0.0)
			{
				return null;
			}
			int num = ((totalSeconds < 2147483647.0) ? ((int)totalSeconds) : int.MaxValue);
			string text = this._wr.SetupKernelCaching(num, originalCacheUrl, flag);
			if (text != null)
			{
				this._cachePolicy.SetNoMaxAgeInCacheControl();
			}
			return text;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000190DE File Offset: 0x000180DE
		public void DisableKernelCache()
		{
			if (this._wr == null)
			{
				return;
			}
			this._wr.DisableKernelCache();
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000190F4 File Offset: 0x000180F4
		private bool IsKernelCacheEnabledForVaryByStar()
		{
			OutputCacheSection outputCache = RuntimeConfig.GetAppConfig().OutputCache;
			return this._cachePolicy.IsVaryByStar && outputCache.EnableKernelCacheForVaryByStar;
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00019124 File Offset: 0x00018124
		internal void FilterOutput()
		{
			try
			{
				if (this.UsingHttpWriter)
				{
					IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
					if (iis7WorkerRequest != null)
					{
						this._httpWriter.FilterIntegrated(true, iis7WorkerRequest);
					}
					else
					{
						this._httpWriter.Filter(true);
					}
				}
			}
			finally
			{
				this._filteringCompleted = true;
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x00019180 File Offset: 0x00018180
		internal void IgnoreFurtherWrites()
		{
			if (this.UsingHttpWriter)
			{
				this._httpWriter.IgnoreFurtherWrites();
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00019195 File Offset: 0x00018195
		internal bool IsBuffered()
		{
			return !this._headersWritten && this.UsingHttpWriter;
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x000191A7 File Offset: 0x000181A7
		public HttpCookieCollection Cookies
		{
			get
			{
				if (this._cookies == null)
				{
					this._cookies = new HttpCookieCollection(this, false);
				}
				return this._cookies;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000191C4 File Offset: 0x000181C4
		public NameValueCollection Headers
		{
			get
			{
				if (!(this._wr is IIS7WorkerRequest))
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				if (this._headers == null)
				{
					this._headers = new HttpHeaderCollection(this._wr, this, 16);
				}
				return this._headers;
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00019210 File Offset: 0x00018210
		public void AddFileDependency(string filename)
		{
			this._fileDependencyList.AddDependency(filename, "filename");
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00019223 File Offset: 0x00018223
		public void AddFileDependencies(ArrayList filenames)
		{
			this._fileDependencyList.AddDependencies(filenames, "filenames");
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00019236 File Offset: 0x00018236
		public void AddFileDependencies(string[] filenames)
		{
			this._fileDependencyList.AddDependencies(filenames, "filenames");
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00019249 File Offset: 0x00018249
		internal void AddVirtualPathDependencies(string[] virtualPaths)
		{
			this._virtualPathDependencyList.AddDependencies(virtualPaths, "virtualPaths", false, this.Request.Path);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00019268 File Offset: 0x00018268
		internal void AddFileDependencies(string[] filenames, DateTime utcTime)
		{
			this._fileDependencyList.AddDependencies(filenames, "filenames", false, utcTime);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001927D File Offset: 0x0001827D
		public void AddCacheItemDependency(string cacheKey)
		{
			this._cacheItemDependencyList.AddDependency(cacheKey, "cacheKey");
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00019290 File Offset: 0x00018290
		public void AddCacheItemDependencies(ArrayList cacheKeys)
		{
			this._cacheItemDependencyList.AddDependencies(cacheKeys, "cacheKeys");
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x000192A3 File Offset: 0x000182A3
		public void AddCacheItemDependencies(string[] cacheKeys)
		{
			this._cacheItemDependencyList.AddDependencies(cacheKeys, "cacheKeys");
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000192B6 File Offset: 0x000182B6
		public void AddCacheDependency(params CacheDependency[] dependencies)
		{
			if (this._aggDependency == null)
			{
				this._aggDependency = new AggregateCacheDependency();
			}
			this._aggDependency.Add(dependencies);
			this.Cache.SetDependencies(true);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000192E4 File Offset: 0x000182E4
		public static void RemoveOutputCacheItem(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (StringUtil.StringStartsWith(path, "\\\\") || path.IndexOf(':') >= 0 || !UrlPath.IsRooted(path))
			{
				throw new ArgumentException(SR.GetString("Invalid_path_for_remove", new object[] { path }));
			}
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text = OutputCacheModule.CreateOutputCachedItemKey(path, HttpVerb.GET, null, null);
			cacheInternal.Remove(text);
			text = OutputCacheModule.CreateOutputCachedItemKey(path, HttpVerb.POST, null, null);
			cacheInternal.Remove(text);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00019365 File Offset: 0x00018365
		internal string[] GetFileDependencies()
		{
			return this._fileDependencyList.GetDependencies();
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00019372 File Offset: 0x00018372
		internal string[] GetCacheItemDependencies()
		{
			return this._cacheItemDependencyList.GetDependencies();
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x0001937F File Offset: 0x0001837F
		internal bool HasFileDependencies()
		{
			return this._fileDependencyList.HasDependencies();
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001938C File Offset: 0x0001838C
		internal bool HasCacheItemDependencies()
		{
			return this._cacheItemDependencyList.HasDependencies();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00019399 File Offset: 0x00018399
		internal CacheDependency GetCacheDependency()
		{
			return this._aggDependency;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000193A1 File Offset: 0x000183A1
		internal CacheDependency GetVirtualPathDependency()
		{
			return this._virtualPathDependencyList.CreateCacheDependency(CacheDependencyType.VirtualPaths, null);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000193B0 File Offset: 0x000183B0
		internal CacheDependency CreateCacheDependencyForResponse(CacheDependency dependencyVary)
		{
			CacheDependency cacheDependency = this._cacheItemDependencyList.CreateCacheDependency(CacheDependencyType.CacheItems, dependencyVary);
			cacheDependency = this._fileDependencyList.CreateCacheDependency(CacheDependencyType.Files, cacheDependency);
			cacheDependency = this._virtualPathDependencyList.CreateCacheDependency(CacheDependencyType.VirtualPaths, cacheDependency);
			AggregateCacheDependency aggregateCacheDependency;
			if (this._aggDependency != null)
			{
				aggregateCacheDependency = this._aggDependency;
			}
			else
			{
				aggregateCacheDependency = new AggregateCacheDependency();
			}
			if (cacheDependency != null)
			{
				aggregateCacheDependency.Add(new CacheDependency[] { cacheDependency });
			}
			return aggregateCacheDependency;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00019414 File Offset: 0x00018414
		internal HttpRawResponse GetSnapshot()
		{
			int num = 200;
			string text = null;
			ArrayList arrayList = null;
			ArrayList arrayList2 = null;
			bool flag = false;
			if (!this.IsBuffered())
			{
				throw new HttpException(SR.GetString("Cannot_get_snapshot_if_not_buffered"));
			}
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (!this._suppressContent)
			{
				if (iis7WorkerRequest != null)
				{
					arrayList2 = this._httpWriter.GetIntegratedSnapshot(out flag, iis7WorkerRequest);
				}
				else
				{
					arrayList2 = this._httpWriter.GetSnapshot(out flag);
				}
			}
			if (!this._suppressHeaders)
			{
				num = this._statusCode;
				text = this._statusDescription;
				if (iis7WorkerRequest != null)
				{
					arrayList = this.GenerateResponseHeadersIntegrated(true);
				}
				else
				{
					arrayList = this.GenerateResponseHeaders(true);
				}
			}
			return new HttpRawResponse(num, text, arrayList, arrayList2, flag);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000194B8 File Offset: 0x000184B8
		internal void UseSnapshot(HttpRawResponse rawResponse, bool sendBody)
		{
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_use_snapshot_after_headers_sent"));
			}
			if (this._httpWriter == null)
			{
				throw new HttpException(SR.GetString("Cannot_use_snapshot_for_TextWriter"));
			}
			this.ClearAll();
			this.StatusCode = rawResponse.StatusCode;
			this.StatusDescription = rawResponse.StatusDescription;
			ArrayList headers = rawResponse.Headers;
			int num = ((headers != null) ? headers.Count : 0);
			for (int i = 0; i < num; i++)
			{
				HttpResponseHeader httpResponseHeader = (HttpResponseHeader)headers[i];
				this.AppendHeader(httpResponseHeader.Name, httpResponseHeader.Value);
			}
			this._httpWriter.UseSnapshot(rawResponse.Buffers);
			this._suppressContent = !sendBody;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001956D File Offset: 0x0001856D
		internal void CloseConnectionAfterError()
		{
			this._closeConnectionAfterError = true;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00019578 File Offset: 0x00018578
		private void WriteErrorMessage(Exception e, bool dontShowSensitiveErrors)
		{
			CultureInfo cultureInfo = null;
			CultureInfo cultureInfo2 = null;
			bool flag = false;
			if (this._context.DynamicUICulture != null)
			{
				cultureInfo = this._context.DynamicUICulture;
			}
			else
			{
				GlobalizationSection globalization = RuntimeConfig.GetLKGConfig(this._context).Globalization;
				if (globalization != null && !string.IsNullOrEmpty(globalization.UICulture))
				{
					try
					{
						cultureInfo = HttpServerUtility.CreateReadOnlyCultureInfo(globalization.UICulture);
					}
					catch
					{
					}
				}
			}
			this.GenerateResponseHeadersForHandler();
			if (cultureInfo != null)
			{
				cultureInfo2 = Thread.CurrentThread.CurrentUICulture;
				Thread.CurrentThread.CurrentUICulture = cultureInfo;
				flag = true;
			}
			try
			{
				try
				{
					ErrorFormatter errorFormatter = this.GetErrorFormatter(e);
					if (dontShowSensitiveErrors && !errorFormatter.CanBeShownToAllUsers)
					{
						errorFormatter = new GenericApplicationErrorFormatter(this.Request.IsLocal);
					}
					if (ErrorFormatter.RequiresAdaptiveErrorReporting(this.Context))
					{
						this._writer.Write(errorFormatter.GetAdaptiveErrorMessage(this.Context, dontShowSensitiveErrors));
					}
					else
					{
						this._writer.Write(errorFormatter.GetHtmlErrorMessage(dontShowSensitiveErrors));
						if (!dontShowSensitiveErrors && HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Medium))
						{
							this._writer.Write("<!-- \r\n");
							this.WriteExceptionStack(e);
							this._writer.Write("-->");
						}
						if (!dontShowSensitiveErrors && !this.Request.IsLocal)
						{
							this._writer.Write("<!-- \r\n");
							this._writer.Write(SR.GetString("Information_Disclosure_Warning"));
							this._writer.Write("-->");
						}
					}
					if (this._closeConnectionAfterError)
					{
						this.Flush();
						this.Close();
					}
				}
				finally
				{
					if (flag)
					{
						Thread.CurrentThread.CurrentUICulture = cultureInfo2;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00019750 File Offset: 0x00018750
		internal void SetOverrideErrorFormatter(ErrorFormatter errorFormatter)
		{
			this._overrideErrorFormatter = errorFormatter;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001975C File Offset: 0x0001875C
		internal ErrorFormatter GetErrorFormatter(Exception e)
		{
			if (this._overrideErrorFormatter != null)
			{
				return this._overrideErrorFormatter;
			}
			ErrorFormatter errorFormatter = HttpException.GetErrorFormatter(e);
			if (errorFormatter == null)
			{
				ConfigurationException ex = e as ConfigurationException;
				if (ex != null && !string.IsNullOrEmpty(ex.Filename))
				{
					errorFormatter = new ConfigErrorFormatter(ex);
				}
			}
			if (errorFormatter == null)
			{
				if (this._statusCode == 404)
				{
					errorFormatter = new PageNotFoundErrorFormatter(this.Request.Path);
				}
				else if (this._statusCode == 403)
				{
					errorFormatter = new PageForbiddenErrorFormatter(this.Request.Path);
				}
				else if (e is SecurityException)
				{
					errorFormatter = new SecurityErrorFormatter(e);
				}
				else
				{
					errorFormatter = new UnhandledErrorFormatter(e);
				}
			}
			ConfigErrorFormatter configErrorFormatter = errorFormatter as ConfigErrorFormatter;
			if (configErrorFormatter != null)
			{
				configErrorFormatter.AllowSourceCode = this.Request.IsLocal;
			}
			return errorFormatter;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001981C File Offset: 0x0001881C
		private void WriteOneExceptionStack(Exception e)
		{
			Exception innerException = e.InnerException;
			if (innerException != null)
			{
				this.WriteOneExceptionStack(innerException);
			}
			string text = "[" + e.GetType().Name + "]";
			if (e.Message != null && e.Message.Length > 0)
			{
				text = text + ": " + HttpUtility.HtmlEncode(e.Message);
			}
			this._writer.WriteLine(text);
			if (e.StackTrace != null)
			{
				this._writer.WriteLine(e.StackTrace);
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000198A8 File Offset: 0x000188A8
		private void WriteExceptionStack(Exception e)
		{
			ConfigurationErrorsException ex = e as ConfigurationErrorsException;
			if (ex == null)
			{
				this.WriteOneExceptionStack(e);
				return;
			}
			this.WriteOneExceptionStack(e);
			ICollection errors = ex.Errors;
			if (errors.Count > 1)
			{
				bool flag = false;
				foreach (object obj in errors)
				{
					ConfigurationException ex2 = (ConfigurationException)obj;
					if (!flag)
					{
						flag = true;
					}
					else
					{
						this._writer.WriteLine("---");
						this.WriteOneExceptionStack(ex2);
					}
				}
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00019948 File Offset: 0x00018948
		internal void ReportRuntimeError(Exception e, bool canThrow, bool localExecute)
		{
			CustomErrorsSection customErrorsSection = null;
			bool flag = false;
			int num = -1;
			if (this._completed)
			{
				return;
			}
			if (this._wr != null)
			{
				this._wr.TrySkipIisCustomErrors = true;
			}
			if (!localExecute)
			{
				num = HttpException.GetHttpCodeForException(e);
				if (num != 404)
				{
					WebBaseEvent.RaiseRuntimeError(e, this);
				}
				customErrorsSection = CustomErrorsSection.GetSettings(this._context, canThrow);
				flag = customErrorsSection == null || customErrorsSection.CustomErrorsEnabled(this.Request);
			}
			if (!this._headersWritten)
			{
				if (num == -1)
				{
					num = HttpException.GetHttpCodeForException(e);
				}
				if (num == 401 && !this._context.IsClientImpersonationConfigured)
				{
					num = 500;
				}
				if (this._context.TraceIsEnabled)
				{
					this._context.Trace.StatusCode = num;
				}
				if (localExecute || !flag)
				{
					this.ClearAll();
					this.StatusCode = num;
					this.WriteErrorMessage(e, false);
					return;
				}
				string text = ((customErrorsSection != null) ? customErrorsSection.GetRedirectString(num) : null);
				if (text == null || !this.RedirectToErrorPage(text, customErrorsSection.RedirectMode))
				{
					this.ClearAll();
					this.StatusCode = num;
					this.WriteErrorMessage(e, true);
					return;
				}
			}
			else
			{
				this.Clear();
				if (this._contentType != null && this._contentType.Equals("text/html"))
				{
					this.Write("\r\n\r\n</pre></table></table></table></table></table>");
					this.Write("</font></font></font></font></font>");
					this.Write("</i></i></i></i></i></b></b></b></b></b></u></u></u></u></u>");
					this.Write("<p>&nbsp;</p><hr>\r\n\r\n");
				}
				this.WriteErrorMessage(e, flag);
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00019AAA File Offset: 0x00018AAA
		internal void SynchronizeStatus(int statusCode, int subStatusCode, string description)
		{
			this._statusCode = statusCode;
			this._subStatusCode = subStatusCode;
			this._statusDescription = description;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00019AC4 File Offset: 0x00018AC4
		internal void SynchronizeHeader(int knownHeaderIndex, string name, string value)
		{
			HttpHeaderCollection httpHeaderCollection = this.Headers as HttpHeaderCollection;
			httpHeaderCollection.SynchronizeHeader(name, value);
			if (knownHeaderIndex < 0)
			{
				return;
			}
			bool headersWritten = this.HeadersWritten;
			this.HeadersWritten = false;
			try
			{
				if (knownHeaderIndex <= 12)
				{
					if (knownHeaderIndex != 0)
					{
						if (knownHeaderIndex == 12)
						{
							this._contentType = value;
							this._contentTypeSet = false;
						}
					}
					else
					{
						this._cacheControlHeaderAdded = true;
					}
				}
				else if (knownHeaderIndex != 23)
				{
					if (knownHeaderIndex == 27)
					{
						if (value != null)
						{
							HttpCookie httpCookie = HttpRequest.CreateCookieFromString(value);
							this.Cookies.Set(httpCookie);
							httpCookie.Changed = false;
							httpCookie.Added = false;
						}
					}
				}
				else
				{
					this._redirectLocation = value;
					this._redirectLocationSet = false;
				}
			}
			finally
			{
				this.HeadersWritten = headersWritten;
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00019B80 File Offset: 0x00018B80
		internal void SyncStatusIntegrated()
		{
			if (!this._headersWritten && this._statusSet)
			{
				this._wr.SendStatus(this._statusCode, this._subStatusCode, this.StatusDescription);
				this._statusSet = false;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00019BB6 File Offset: 0x00018BB6
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x00019BBE File Offset: 0x00018BBE
		public int StatusCode
		{
			get
			{
				return this._statusCode;
			}
			set
			{
				if (this._headersWritten)
				{
					throw new HttpException(SR.GetString("Cannot_set_status_after_headers_sent"));
				}
				if (this._statusCode != value)
				{
					this._statusCode = value;
					this._subStatusCode = 0;
					this._statusDescription = null;
					this._statusSet = true;
				}
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x00019BFD File Offset: 0x00018BFD
		// (set) Token: 0x06000612 RID: 1554 RVA: 0x00019C24 File Offset: 0x00018C24
		public int SubStatusCode
		{
			get
			{
				if (!(this._wr is IIS7WorkerRequest))
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				return this._subStatusCode;
			}
			set
			{
				if (!(this._wr is IIS7WorkerRequest))
				{
					throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_Integrated_Mode"));
				}
				if (this._headersWritten)
				{
					throw new HttpException(SR.GetString("Cannot_set_status_after_headers_sent"));
				}
				this._subStatusCode = value;
				this._statusSet = true;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x00019C74 File Offset: 0x00018C74
		// (set) Token: 0x06000614 RID: 1556 RVA: 0x00019C98 File Offset: 0x00018C98
		public string StatusDescription
		{
			get
			{
				if (this._statusDescription == null)
				{
					this._statusDescription = HttpWorkerRequest.GetStatusDescription(this._statusCode);
				}
				return this._statusDescription;
			}
			set
			{
				if (this._headersWritten)
				{
					throw new HttpException(SR.GetString("Cannot_set_status_after_headers_sent"));
				}
				if (value != null && value.Length > 512)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._statusDescription = value;
				this._statusSet = true;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x00019CE6 File Offset: 0x00018CE6
		// (set) Token: 0x06000616 RID: 1558 RVA: 0x00019CFD File Offset: 0x00018CFD
		public bool TrySkipIisCustomErrors
		{
			get
			{
				return this._wr != null && this._wr.TrySkipIisCustomErrors;
			}
			set
			{
				if (this._wr != null)
				{
					this._wr.TrySkipIisCustomErrors = value;
				}
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x00019D13 File Offset: 0x00018D13
		// (set) Token: 0x06000618 RID: 1560 RVA: 0x00019D1B File Offset: 0x00018D1B
		public bool BufferOutput
		{
			get
			{
				return this._bufferOutput;
			}
			set
			{
				if (this._bufferOutput != value)
				{
					this._bufferOutput = value;
					if (this._httpWriter != null)
					{
						this._httpWriter.UpdateResponseBuffering();
					}
				}
			}
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00019D40 File Offset: 0x00018D40
		internal string GetHttpHeaderContentEncoding()
		{
			string text = null;
			if (this._wr is IIS7WorkerRequest)
			{
				if (this._headers != null)
				{
					text = this._headers["Content-Encoding"];
				}
			}
			else if (this._customHeaders != null)
			{
				int count = this._customHeaders.Count;
				for (int i = 0; i < count; i++)
				{
					HttpResponseHeader httpResponseHeader = (HttpResponseHeader)this._customHeaders[i];
					if (httpResponseHeader.Name == "Content-Encoding")
					{
						text = httpResponseHeader.Value;
						break;
					}
				}
			}
			return text;
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00019DC5 File Offset: 0x00018DC5
		// (set) Token: 0x0600061B RID: 1563 RVA: 0x00019DCD File Offset: 0x00018DCD
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				if (!this._headersWritten)
				{
					this._contentTypeSet = true;
					this._contentType = value;
					return;
				}
				if (this._contentType == value)
				{
					return;
				}
				throw new HttpException(SR.GetString("Cannot_set_content_type_after_headers_sent"));
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00019E04 File Offset: 0x00018E04
		// (set) Token: 0x0600061D RID: 1565 RVA: 0x00019E25 File Offset: 0x00018E25
		public string Charset
		{
			get
			{
				if (this._charSet == null)
				{
					this._charSet = this.ContentEncoding.WebName;
				}
				return this._charSet;
			}
			set
			{
				if (this._headersWritten)
				{
					throw new HttpException(SR.GetString("Cannot_set_content_type_after_headers_sent"));
				}
				if (value != null)
				{
					this._charSet = value;
				}
				else
				{
					this._charSet = string.Empty;
				}
				this._customCharSet = true;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00019E60 File Offset: 0x00018E60
		// (set) Token: 0x0600061F RID: 1567 RVA: 0x00019EB0 File Offset: 0x00018EB0
		public Encoding ContentEncoding
		{
			get
			{
				if (this._encoding == null)
				{
					GlobalizationSection globalization = RuntimeConfig.GetLKGConfig(this._context).Globalization;
					if (globalization != null)
					{
						this._encoding = globalization.ResponseEncoding;
					}
					if (this._encoding == null)
					{
						this._encoding = Encoding.Default;
					}
				}
				return this._encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this._encoding == null || !this._encoding.Equals(value))
				{
					this._encoding = value;
					this._encoder = null;
					if (this._httpWriter != null)
					{
						this._httpWriter.UpdateResponseEncoding();
					}
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00019F04 File Offset: 0x00018F04
		// (set) Token: 0x06000621 RID: 1569 RVA: 0x00019F64 File Offset: 0x00018F64
		public Encoding HeaderEncoding
		{
			get
			{
				if (this._headerEncoding == null)
				{
					GlobalizationSection globalization = RuntimeConfig.GetLKGConfig(this._context).Globalization;
					if (globalization != null)
					{
						this._headerEncoding = globalization.ResponseHeaderEncoding;
					}
					if (this._headerEncoding == null || this._headerEncoding.Equals(Encoding.Unicode))
					{
						this._headerEncoding = Encoding.UTF8;
					}
				}
				return this._headerEncoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Equals(Encoding.Unicode))
				{
					throw new HttpException(SR.GetString("Invalid_header_encoding", new object[] { value.WebName }));
				}
				if (this._headerEncoding == null || !this._headerEncoding.Equals(value))
				{
					if (this._headersWritten)
					{
						throw new HttpException(SR.GetString("Cannot_set_header_encoding_after_headers_sent"));
					}
					this._headerEncoding = value;
				}
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x00019FE4 File Offset: 0x00018FE4
		internal Encoder ContentEncoder
		{
			get
			{
				if (this._encoder == null)
				{
					Encoding contentEncoding = this.ContentEncoding;
					this._encoder = contentEncoding.GetEncoder();
					if (!contentEncoding.Equals(Encoding.UTF8))
					{
						bool flag = false;
						GlobalizationSection globalization = RuntimeConfig.GetLKGConfig(this._context).Globalization;
						if (globalization != null)
						{
							flag = globalization.EnableBestFitResponseEncoding;
						}
						if (!flag)
						{
							this._encoder.Fallback = new EncoderReplacementFallback();
						}
					}
				}
				return this._encoder;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0001A04F File Offset: 0x0001904F
		public HttpCachePolicy Cache
		{
			get
			{
				if (this._cachePolicy == null)
				{
					this._cachePolicy = new HttpCachePolicy();
				}
				return this._cachePolicy;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001A06A File Offset: 0x0001906A
		internal bool HasCachePolicy
		{
			get
			{
				return this._cachePolicy != null;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0001A078 File Offset: 0x00019078
		public bool IsClientConnected
		{
			get
			{
				if (this._clientDisconnected)
				{
					return false;
				}
				if (this._wr != null && !this._wr.IsClientConnected())
				{
					this._clientDisconnected = true;
					return false;
				}
				return true;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001A0A3 File Offset: 0x000190A3
		public bool IsRequestBeingRedirected
		{
			get
			{
				return this._isRequestBeingRedirected;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001A0AB File Offset: 0x000190AB
		// (set) Token: 0x06000628 RID: 1576 RVA: 0x0001A0B3 File Offset: 0x000190B3
		public string RedirectLocation
		{
			get
			{
				return this._redirectLocation;
			}
			set
			{
				if (this._headersWritten)
				{
					throw new HttpException(SR.GetString("Cannot_append_header_after_headers_sent"));
				}
				this._redirectLocation = value;
				this._redirectLocationSet = true;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001A0DB File Offset: 0x000190DB
		public void Close()
		{
			if (!this._clientDisconnected && !this._completed && this._wr != null)
			{
				this._wr.CloseConnection();
				this._clientDisconnected = true;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x0001A107 File Offset: 0x00019107
		public TextWriter Output
		{
			get
			{
				return this._writer;
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001A110 File Offset: 0x00019110
		internal TextWriter SwitchWriter(TextWriter writer)
		{
			TextWriter writer2 = this._writer;
			this._writer = writer;
			return writer2;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001A12C File Offset: 0x0001912C
		public Stream OutputStream
		{
			get
			{
				if (!this.UsingHttpWriter)
				{
					throw new HttpException(SR.GetString("OutputStream_NotAvail"));
				}
				return this._httpWriter.OutputStream;
			}
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001A151 File Offset: 0x00019151
		public void BinaryWrite(byte[] buffer)
		{
			this.OutputStream.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x0001A163 File Offset: 0x00019163
		public void Pics(string value)
		{
			this.AppendHeader("PICS-Label", value);
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x0001A171 File Offset: 0x00019171
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x0001A188 File Offset: 0x00019188
		public Stream Filter
		{
			get
			{
				if (this.UsingHttpWriter)
				{
					return this._httpWriter.GetCurrentFilter();
				}
				return null;
			}
			set
			{
				if (!this.UsingHttpWriter)
				{
					throw new HttpException(SR.GetString("Filtering_not_allowed"));
				}
				this._httpWriter.InstallFilter(value);
				IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
				if (iis7WorkerRequest != null)
				{
					iis7WorkerRequest.ResponseFilterInstalled();
					return;
				}
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001A1CF File Offset: 0x000191CF
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x0001A1D7 File Offset: 0x000191D7
		public bool SuppressContent
		{
			get
			{
				return this._suppressContent;
			}
			set
			{
				this._suppressContent = value;
				this._suppressContentSet = true;
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001A1E8 File Offset: 0x000191E8
		public void AppendHeader(string name, string value)
		{
			bool flag = false;
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_append_header_after_headers_sent"));
			}
			int knownResponseHeaderIndex = HttpWorkerRequest.GetKnownResponseHeaderIndex(name);
			int num = knownResponseHeaderIndex;
			if (num <= 6)
			{
				if (num != 0)
				{
					if (num != 6)
					{
						goto IL_0095;
					}
					this._transferEncodingSet = true;
					goto IL_0095;
				}
				else
				{
					this._cacheControlHeaderAdded = true;
				}
			}
			else
			{
				switch (num)
				{
				case 11:
					this._contentLengthSet = true;
					goto IL_0095;
				case 12:
					this.ContentType = value;
					return;
				default:
					switch (num)
					{
					case 18:
					case 19:
					case 22:
						break;
					case 20:
					case 21:
						goto IL_0095;
					case 23:
						this.RedirectLocation = value;
						return;
					default:
						if (num != 28)
						{
							goto IL_0095;
						}
						break;
					}
					break;
				}
			}
			flag = true;
			IL_0095:
			if (this._wr is IIS7WorkerRequest)
			{
				this.Headers.Add(HttpResponseHeader.MaybeEncodeHeader(name), HttpResponseHeader.MaybeEncodeHeader(value));
				return;
			}
			if (flag)
			{
				if (this._cacheHeaders == null)
				{
					this._cacheHeaders = new ArrayList();
				}
				this._cacheHeaders.Add(new HttpResponseHeader(knownResponseHeaderIndex, value));
				return;
			}
			HttpResponseHeader httpResponseHeader;
			if (knownResponseHeaderIndex >= 0)
			{
				httpResponseHeader = new HttpResponseHeader(knownResponseHeaderIndex, value);
			}
			else
			{
				httpResponseHeader = new HttpResponseHeader(name, value);
			}
			this.AppendHeader(httpResponseHeader);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0001A2F6 File Offset: 0x000192F6
		public void AppendCookie(HttpCookie cookie)
		{
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_append_cookie_after_headers_sent"));
			}
			this.Cookies.AddCookie(cookie, true);
			this.OnCookieAdd(cookie);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0001A324 File Offset: 0x00019324
		public void SetCookie(HttpCookie cookie)
		{
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_append_cookie_after_headers_sent"));
			}
			this.Cookies.AddCookie(cookie, false);
			this.OnCookieCollectionChange();
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0001A351 File Offset: 0x00019351
		internal void BeforeCookieCollectionChange()
		{
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_modify_cookies_after_headers_sent"));
			}
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0001A36B File Offset: 0x0001936B
		internal void OnCookieAdd(HttpCookie cookie)
		{
			this.Request.AddResponseCookie(cookie);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0001A379 File Offset: 0x00019379
		internal void OnCookieCollectionChange()
		{
			this.Request.ResetCookies();
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0001A388 File Offset: 0x00019388
		public void ClearHeaders()
		{
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_clear_headers_after_headers_sent"));
			}
			this.StatusCode = 200;
			this._subStatusCode = 0;
			this._statusDescription = null;
			this._contentType = "text/html";
			this._charSet = null;
			this._customCharSet = false;
			this._contentLengthSet = false;
			this._redirectLocation = null;
			this._redirectLocationSet = false;
			this._isRequestBeingRedirected = false;
			this._customHeaders = null;
			if (this._headers != null)
			{
				this._headers.ClearInternal();
			}
			this._transferEncodingSet = false;
			this._chunked = false;
			if (this._cookies != null)
			{
				this._cookies.Reset();
				this.Request.ResetCookies();
			}
			if (this._cachePolicy != null)
			{
				this._cachePolicy.Reset();
			}
			this._cacheControlHeaderAdded = false;
			this._cacheHeaders = null;
			this._suppressHeaders = false;
			this._suppressContent = false;
			this._suppressContentSet = false;
			this._expiresInMinutes = 0;
			this._expiresInMinutesSet = false;
			this._expiresAbsolute = DateTime.MinValue;
			this._expiresAbsoluteSet = false;
			this._cacheControl = null;
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				this.ClearNativeResponse(false, true, iis7WorkerRequest);
				if (this._handlerHeadersGenerated && this._sendCacheControlHeader)
				{
					this.Headers.Set("Cache-Control", "private");
				}
				this._handlerHeadersGenerated = false;
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0001A4E2 File Offset: 0x000194E2
		public void ClearContent()
		{
			this.Clear();
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0001A4EC File Offset: 0x000194EC
		public void Clear()
		{
			if (this.UsingHttpWriter)
			{
				this._httpWriter.ClearBuffers();
			}
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest != null)
			{
				this.ClearNativeResponse(true, false, iis7WorkerRequest);
			}
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0001A524 File Offset: 0x00019524
		internal void ClearAll()
		{
			if (!this._headersWritten)
			{
				this.ClearHeaders();
			}
			this.Clear();
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0001A53A File Offset: 0x0001953A
		public void Flush()
		{
			if (this._completed)
			{
				throw new HttpException(SR.GetString("Cannot_flush_completed_response"));
			}
			this.Flush(false);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0001A55B File Offset: 0x0001955B
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
		public void AppendToLog(string param)
		{
			if (this._wr is ISAPIWorkerRequest)
			{
				((ISAPIWorkerRequest)this._wr).AppendLogParameter(param);
				return;
			}
			if (this._wr is IIS7WorkerRequest)
			{
				this._context.Request.AppendToLogQueryString(param);
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0001A59A File Offset: 0x0001959A
		public void Redirect(string url)
		{
			this.Redirect(url, true);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0001A5A4 File Offset: 0x000195A4
		public void Redirect(string url, bool endResponse)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.IndexOf('\n') >= 0)
			{
				throw new ArgumentException(SR.GetString("Cannot_redirect_to_newline"));
			}
			if (this._headersWritten)
			{
				throw new HttpException(SR.GetString("Cannot_redirect_after_headers_sent"));
			}
			Page page = this._context.Handler as Page;
			if (page != null && page.IsCallback)
			{
				throw new ApplicationException(SR.GetString("Redirect_not_allowed_in_callback"));
			}
			url = this.ApplyRedirectQueryStringIfRequired(url);
			url = this.ApplyAppPathModifier(url);
			url = this.ConvertToFullyQualifiedRedirectUrlIfRequired(url);
			url = this.UrlEncodeRedirect(url);
			this.Clear();
			if (page != null && page.IsPostBack && page.SmartNavigation && this.Request["__smartNavPostBack"] == "true")
			{
				this.Write("<BODY><ASP_SMARTNAV_RDIR url=\"");
				this.Write(HttpUtility.HtmlEncode(url));
				this.Write("\"></ASP_SMARTNAV_RDIR>");
				this.Write("</BODY>");
			}
			else
			{
				this.StatusCode = 302;
				this.RedirectLocation = url;
				if (url.StartsWith("http:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("ftp:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("file:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("news:", StringComparison.OrdinalIgnoreCase))
				{
					url = HttpUtility.HtmlAttributeEncode(url);
				}
				else
				{
					url = HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(url));
				}
				this.Write("<html><head><title>Object moved</title></head><body>\r\n");
				this.Write("<h2>Object moved to <a href=\"" + url + "\">here</a>.</h2>\r\n");
				this.Write("</body></html>\r\n");
			}
			this._isRequestBeingRedirected = true;
			if (endResponse)
			{
				this.End();
			}
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001A754 File Offset: 0x00019754
		internal string ApplyRedirectQueryStringIfRequired(string url)
		{
			if (this.Request == null || this.Request.Browser["requiresPostRedirectionHandling"] != "true")
			{
				return url;
			}
			Page page = this._context.Handler as Page;
			if (page != null && !page.IsPostBack)
			{
				return url;
			}
			int num = url.IndexOf(HttpResponse.RedirectQueryStringAssignment, StringComparison.Ordinal);
			if (num == -1)
			{
				num = url.IndexOf('?');
				if (num >= 0)
				{
					url = url.Insert(num + 1, HttpResponse._redirectQueryStringInline);
				}
				else
				{
					url += HttpResponse._redirectQueryString;
				}
			}
			return url;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001A7E8 File Offset: 0x000197E8
		internal bool RedirectToErrorPage(string url, CustomErrorsRedirectMode redirectMode)
		{
			try
			{
				if (string.IsNullOrEmpty(url))
				{
					return false;
				}
				if (this._headersWritten)
				{
					return false;
				}
				if (this.Request.QueryString["aspxerrorpath"] != null)
				{
					return false;
				}
				if (redirectMode == CustomErrorsRedirectMode.ResponseRewrite)
				{
					this.Context.Server.Execute(url);
				}
				else
				{
					if (url.IndexOf('?') < 0)
					{
						url = url + "?aspxerrorpath=" + HttpUtility.UrlEncodeSpaces(this.Request.Path);
					}
					this.Redirect(url, false);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001A88C File Offset: 0x0001988C
		internal bool CanExecuteUrlForEntireResponse
		{
			get
			{
				return !this._headersWritten && this._wr != null && this._wr.SupportsExecuteUrl && this.UsingHttpWriter && this._httpWriter.GetBufferedLength() == 0L && !this._httpWriter.FilterInstalled && (this._cachePolicy == null || !this._cachePolicy.IsModified());
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001A8FC File Offset: 0x000198FC
		internal IAsyncResult BeginExecuteUrlForEntireResponse(string pathOverride, NameValueCollection requestHeaders, AsyncCallback cb, object state)
		{
			string text;
			string text2;
			if (this._context != null && this._context.User != null)
			{
				text = this._context.User.Identity.Name;
				text2 = this._context.User.Identity.AuthenticationType;
			}
			else
			{
				text = string.Empty;
				text2 = string.Empty;
			}
			string text3 = this.Request.RewrittenUrl;
			if (pathOverride != null)
			{
				text3 = pathOverride;
			}
			string text4 = null;
			if (requestHeaders != null)
			{
				int count = requestHeaders.Count;
				if (count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < count; i++)
					{
						stringBuilder.Append(requestHeaders.GetKey(i));
						stringBuilder.Append(": ");
						stringBuilder.Append(requestHeaders.Get(i));
						stringBuilder.Append("\r\n");
					}
					text4 = stringBuilder.ToString();
				}
			}
			byte[] array = null;
			if (this._context != null && this._context.Request != null)
			{
				array = this._context.Request.EntityBody;
			}
			IAsyncResult asyncResult = this._wr.BeginExecuteUrl(text3, null, text4, true, true, this._wr.GetUserToken(), text, text2, array, cb, state);
			this._headersWritten = true;
			this._ended = true;
			return asyncResult;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001AA33 File Offset: 0x00019A33
		internal void EndExecuteUrlForEntireResponse(IAsyncResult result)
		{
			this._wr.EndExecuteUrl(result);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001AA41 File Offset: 0x00019A41
		public void Write(string s)
		{
			this._writer.Write(s);
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001AA4F File Offset: 0x00019A4F
		public void Write(object obj)
		{
			this._writer.Write(obj);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001AA5D File Offset: 0x00019A5D
		public void Write(char ch)
		{
			this._writer.Write(ch);
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001AA6B File Offset: 0x00019A6B
		public void Write(char[] buffer, int index, int count)
		{
			this._writer.Write(buffer, index, count);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001AA7C File Offset: 0x00019A7C
		public void WriteSubstitution(HttpResponseSubstitutionCallback callback)
		{
			if (callback.Target != null && callback.Target is Control)
			{
				throw new ArgumentException(SR.GetString("Invalid_substitution_callback"), "callback");
			}
			if (this.UsingHttpWriter)
			{
				this._httpWriter.WriteSubstBlock(callback, this._wr as IIS7WorkerRequest);
			}
			else
			{
				this._writer.Write(callback(this._context));
			}
			if (this._cachePolicy != null && this._cachePolicy.GetCacheability() == HttpCacheability.Public)
			{
				this._cachePolicy.SetCacheability(HttpCacheability.Server);
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001AB10 File Offset: 0x00019B10
		private void WriteStreamAsText(Stream f, long offset, long size)
		{
			if (size < 0L)
			{
				size = f.Length - offset;
			}
			if (size > 0L)
			{
				if (offset > 0L)
				{
					f.Seek(offset, SeekOrigin.Begin);
				}
				byte[] array = new byte[(int)size];
				int num = f.Read(array, 0, (int)size);
				this._writer.Write(Encoding.Default.GetChars(array, 0, num));
			}
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001AB6C File Offset: 0x00019B6C
		internal void WriteVirtualFile(VirtualFile vf)
		{
			using (Stream stream = vf.Open())
			{
				if (this.UsingHttpWriter)
				{
					long length = stream.Length;
					if (length > 0L)
					{
						byte[] array = new byte[(int)length];
						int num = stream.Read(array, 0, (int)length);
						this._httpWriter.WriteBytes(array, 0, num);
					}
				}
				else
				{
					this.WriteStreamAsText(stream, 0L, -1L);
				}
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001ABE0 File Offset: 0x00019BE0
		private string GetNormalizedFilename(string fn)
		{
			if (!UrlPath.IsAbsolutePhysicalPath(fn))
			{
				if (this.Request != null)
				{
					fn = this.Request.MapPath(fn);
				}
				else
				{
					fn = HostingEnvironment.MapPath(fn);
				}
			}
			return fn;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001AC0B File Offset: 0x00019C0B
		public void WriteFile(string filename)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			this.WriteFile(filename, false);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001AC24 File Offset: 0x00019C24
		public void WriteFile(string filename, bool readIntoMemory)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			filename = this.GetNormalizedFilename(filename);
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				if (this.UsingHttpWriter)
				{
					long length = fileStream.Length;
					if (length > 0L)
					{
						if (readIntoMemory)
						{
							byte[] array = new byte[(int)length];
							int num = fileStream.Read(array, 0, (int)length);
							this._httpWriter.WriteBytes(array, 0, num);
						}
						else
						{
							fileStream.Close();
							fileStream = null;
							this._httpWriter.WriteFile(filename, 0L, length);
						}
					}
				}
				else
				{
					this.WriteStreamAsText(fileStream, 0L, -1L);
				}
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001ACD0 File Offset: 0x00019CD0
		public void TransmitFile(string filename)
		{
			this.TransmitFile(filename, 0L, -1L);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001ACE0 File Offset: 0x00019CE0
		public void TransmitFile(string filename, long offset, long length)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (offset < 0L)
			{
				throw new ArgumentException(SR.GetString("Invalid_range"), "offset");
			}
			if (length < -1L)
			{
				throw new ArgumentException(SR.GetString("Invalid_range"), "length");
			}
			filename = this.GetNormalizedFilename(filename);
			using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				long length2 = fileStream.Length;
				if (length == -1L)
				{
					length = length2 - offset;
				}
				if (length2 < offset)
				{
					throw new ArgumentException(SR.GetString("Invalid_range"), "offset");
				}
				if (length2 - offset < length)
				{
					throw new ArgumentException(SR.GetString("Invalid_range"), "length");
				}
				if (!this.UsingHttpWriter)
				{
					this.WriteStreamAsText(fileStream, offset, length);
					return;
				}
			}
			if (length > 0L)
			{
				bool flag = this._wr != null && this._wr.SupportsLongTransmitFile;
				this._httpWriter.TransmitFile(filename, offset, length, this._context.IsClientImpersonationConfigured || HttpRuntime.IsOnUNCShareInternal, flag);
			}
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001ADF8 File Offset: 0x00019DF8
		private void ValidateFileRange(string filename, long offset, long length)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				long length2 = fileStream.Length;
				if (length == -1L)
				{
					length = length2 - offset;
				}
				if (offset < 0L || length > length2 - offset)
				{
					throw new HttpException(SR.GetString("Invalid_range"));
				}
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001AE5C File Offset: 0x00019E5C
		public void WriteFile(string filename, long offset, long size)
		{
			if (filename == null)
			{
				throw new ArgumentNullException("filename");
			}
			if (size == 0L)
			{
				return;
			}
			filename = this.GetNormalizedFilename(filename);
			this.ValidateFileRange(filename, offset, size);
			if (this.UsingHttpWriter)
			{
				InternalSecurityPermissions.FileReadAccess(filename).Demand();
				this._httpWriter.WriteFile(filename, offset, size);
				return;
			}
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
				this.WriteStreamAsText(fileStream, offset, size);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0001AEE4 File Offset: 0x00019EE4
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public void WriteFile(IntPtr fileHandle, long offset, long size)
		{
			if (size <= 0L)
			{
				return;
			}
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(new SafeFileHandle(fileHandle, false), FileAccess.Read);
				if (this.UsingHttpWriter)
				{
					long length = fileStream.Length;
					if (size == -1L)
					{
						size = length - offset;
					}
					if (offset < 0L || size > length - offset)
					{
						throw new HttpException(SR.GetString("Invalid_range"));
					}
					if (offset > 0L)
					{
						fileStream.Seek(offset, SeekOrigin.Begin);
					}
					byte[] array = new byte[(int)size];
					int num = fileStream.Read(array, 0, (int)size);
					this._httpWriter.WriteBytes(array, 0, num);
				}
				else
				{
					this.WriteStreamAsText(fileStream, offset, size);
				}
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x0001AF94 File Offset: 0x00019F94
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x0001AFC4 File Offset: 0x00019FC4
		public string Status
		{
			get
			{
				return this.StatusCode.ToString(NumberFormatInfo.InvariantInfo) + " " + this.StatusDescription;
			}
			set
			{
				int num = 200;
				string text = "OK";
				try
				{
					int num2 = value.IndexOf(' ');
					num = int.Parse(value.Substring(0, num2), CultureInfo.InvariantCulture);
					text = value.Substring(num2 + 1);
				}
				catch
				{
					throw new HttpException(SR.GetString("Invalid_status_string"));
				}
				this.StatusCode = num;
				this.StatusDescription = text;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0001B034 File Offset: 0x0001A034
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x0001B03C File Offset: 0x0001A03C
		public bool Buffer
		{
			get
			{
				return this.BufferOutput;
			}
			set
			{
				this.BufferOutput = value;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0001B045 File Offset: 0x0001A045
		public void AddHeader(string name, string value)
		{
			this.AppendHeader(name, value);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0001B050 File Offset: 0x0001A050
		public void End()
		{
			if (this._context.IsInCancellablePeriod)
			{
				InternalSecurityPermissions.ControlThread.Assert();
				Thread.CurrentThread.Abort(new HttpApplication.CancelModuleException(false));
				return;
			}
			if (!this._flushing)
			{
				this.Flush();
				this._ended = true;
				if (this._context.ApplicationInstance != null)
				{
					this._context.ApplicationInstance.CompleteRequest();
				}
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001B0B7 File Offset: 0x0001A0B7
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0001B0C0 File Offset: 0x0001A0C0
		public int Expires
		{
			get
			{
				return this._expiresInMinutes;
			}
			set
			{
				if (!this._expiresInMinutesSet || value < this._expiresInMinutes)
				{
					this._expiresInMinutes = value;
					this.Cache.SetExpires(this._context.Timestamp + new TimeSpan(0, this._expiresInMinutes, 0));
				}
			}
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001B10D File Offset: 0x0001A10D
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x0001B115 File Offset: 0x0001A115
		public DateTime ExpiresAbsolute
		{
			get
			{
				return this._expiresAbsolute;
			}
			set
			{
				if (!this._expiresAbsoluteSet || value < this._expiresAbsolute)
				{
					this._expiresAbsolute = value;
					this.Cache.SetExpires(this._expiresAbsolute);
				}
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x0001B145 File Offset: 0x0001A145
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x0001B15C File Offset: 0x0001A15C
		public string CacheControl
		{
			get
			{
				if (this._cacheControl == null)
				{
					return "private";
				}
				return this._cacheControl;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this._cacheControl = null;
					this.Cache.SetCacheability(HttpCacheability.NoCache);
					return;
				}
				if (StringUtil.EqualsIgnoreCase(value, "private"))
				{
					this._cacheControl = value;
					this.Cache.SetCacheability(HttpCacheability.Private);
					return;
				}
				if (StringUtil.EqualsIgnoreCase(value, "public"))
				{
					this._cacheControl = value;
					this.Cache.SetCacheability(HttpCacheability.Public);
					return;
				}
				if (StringUtil.EqualsIgnoreCase(value, "no-cache"))
				{
					this._cacheControl = value;
					this.Cache.SetCacheability(HttpCacheability.NoCache);
					return;
				}
				throw new ArgumentException(SR.GetString("Invalid_value_for_CacheControl", new object[] { value }));
			}
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001B204 File Offset: 0x0001A204
		internal void SetAppPathModifier(string appPathModifier)
		{
			if (appPathModifier != null && (appPathModifier.Length == 0 || appPathModifier[0] == '/' || appPathModifier[appPathModifier.Length - 1] == '/'))
			{
				throw new ArgumentException(SR.GetString("InvalidArgumentValue", new object[] { "appPathModifier" }));
			}
			this._appPathModifier = appPathModifier;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001B260 File Offset: 0x0001A260
		public string ApplyAppPathModifier(string virtualPath)
		{
			CookielessHelperClass cookielessHelper = this._context.CookielessHelper;
			if (virtualPath == null)
			{
				return null;
			}
			if (UrlPath.IsRelativeUrl(virtualPath))
			{
				virtualPath = UrlPath.Combine(this.Request.ClientBaseDir.VirtualPathString, virtualPath);
			}
			else
			{
				if (!UrlPath.IsRooted(virtualPath) || virtualPath.StartsWith("//", StringComparison.Ordinal))
				{
					return virtualPath;
				}
				virtualPath = UrlPath.Reduce(virtualPath);
			}
			if (this._appPathModifier == null || virtualPath.IndexOf(this._appPathModifier, StringComparison.Ordinal) >= 0)
			{
				return virtualPath;
			}
			string appDomainAppVirtualPathString = HttpRuntime.AppDomainAppVirtualPathString;
			int num = appDomainAppVirtualPathString.Length;
			bool flag = virtualPath.Length == appDomainAppVirtualPathString.Length - 1;
			if (flag)
			{
				num--;
			}
			if (virtualPath.Length < num)
			{
				return virtualPath;
			}
			if (!StringUtil.EqualsIgnoreCase(virtualPath, 0, appDomainAppVirtualPathString, 0, num))
			{
				return virtualPath;
			}
			if (flag)
			{
				virtualPath += "/";
			}
			if (virtualPath.Length == appDomainAppVirtualPathString.Length)
			{
				virtualPath = virtualPath.Substring(0, appDomainAppVirtualPathString.Length) + this._appPathModifier + "/";
			}
			else
			{
				virtualPath = virtualPath.Substring(0, appDomainAppVirtualPathString.Length) + this._appPathModifier + "/" + virtualPath.Substring(appDomainAppVirtualPathString.Length);
			}
			return virtualPath;
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001B384 File Offset: 0x0001A384
		internal string RemoveAppPathModifier(string virtualPath)
		{
			if (string.IsNullOrEmpty(this._appPathModifier))
			{
				return virtualPath;
			}
			int num = virtualPath.IndexOf(this._appPathModifier, StringComparison.Ordinal);
			if (num <= 0 || virtualPath[num - 1] != '/')
			{
				return virtualPath;
			}
			if (!AppSettings.RestoreAggressiveCookielessPathRemoval && (virtualPath.Length < num + this._appPathModifier.Length + 1 || virtualPath[num + this._appPathModifier.Length] != '/'))
			{
				return virtualPath;
			}
			return virtualPath.Substring(0, num - 1) + virtualPath.Substring(num + this._appPathModifier.Length);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001B41C File Offset: 0x0001A41C
		private string ConvertToFullyQualifiedRedirectUrlIfRequired(string url)
		{
			HttpRuntimeSection httpRuntime = RuntimeConfig.GetConfig(this._context).HttpRuntime;
			if (httpRuntime.UseFullyQualifiedRedirectUrl || (this.Request != null && this.Request.Browser["requiresFullyQualifiedRedirectUrl"] == "true"))
			{
				return new Uri(this.Request.Url, url).AbsoluteUri;
			}
			return url;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001B484 File Offset: 0x0001A484
		private string UrlEncodeRedirect(string url)
		{
			int num = url.IndexOf('?');
			if (num >= 0)
			{
				Encoding encoding = ((this.Request != null) ? this.Request.ContentEncoding : this.ContentEncoding);
				url = HttpUtility.UrlEncodeSpaces(HttpUtility.UrlEncodeNonAscii(url.Substring(0, num), Encoding.UTF8)) + HttpUtility.UrlEncodeNonAscii(url.Substring(num), encoding);
			}
			else
			{
				url = HttpUtility.UrlEncodeSpaces(HttpUtility.UrlEncodeNonAscii(url, Encoding.UTF8));
			}
			return url;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001B4FC File Offset: 0x0001A4FC
		internal void UpdateNativeResponse(bool sendHeaders)
		{
			IIS7WorkerRequest iis7WorkerRequest = this._wr as IIS7WorkerRequest;
			if (iis7WorkerRequest == null)
			{
				return;
			}
			if ((this._suppressContent && this.Request != null && this.Request.HttpVerb != HttpVerb.HEAD) || this._ended)
			{
				this.Clear();
			}
			bool flag = false;
			long num = this._httpWriter.GetBufferedLength();
			if (!this._headersWritten)
			{
				if (this.UseAdaptiveError)
				{
					int statusCode = this.StatusCode;
					if (statusCode >= 400 && statusCode < 600)
					{
						this.StatusCode = 200;
					}
				}
				if (this._statusSet)
				{
					this._wr.SendStatus(this.StatusCode, this.SubStatusCode, this.StatusDescription);
					this._statusSet = false;
				}
				if (!this._suppressHeaders && !this._clientDisconnected)
				{
					if (this._redirectLocation != null && this._redirectLocationSet)
					{
						HttpHeaderCollection httpHeaderCollection = this.Headers as HttpHeaderCollection;
						httpHeaderCollection.Set("Location", this._redirectLocation);
						this._redirectLocationSet = false;
					}
					if (this._contentType != null && this._contentTypeSet && (num > 0L || this._contentLengthSet))
					{
						HttpHeaderCollection httpHeaderCollection2 = this.Headers as HttpHeaderCollection;
						string text = this.AppendCharSetToContentType(this._contentType);
						httpHeaderCollection2.Set("Content-Type", text);
						this._contentTypeSet = false;
					}
					this.GenerateResponseHeadersForCookies();
					if (sendHeaders)
					{
						if (this._cachePolicy != null)
						{
							if (this._cookies != null && this._cookies.Count != 0)
							{
								this._cachePolicy.SetHasSetCookieHeader();
								this.DisableKernelCache();
							}
							if (this._cachePolicy.IsModified())
							{
								ArrayList arrayList = new ArrayList();
								this._cachePolicy.GetHeaders(arrayList, this);
								HttpHeaderCollection httpHeaderCollection3 = this.Headers as HttpHeaderCollection;
								foreach (object obj in arrayList)
								{
									HttpResponseHeader httpResponseHeader = (HttpResponseHeader)obj;
									httpHeaderCollection3.Set(httpResponseHeader.Name, httpResponseHeader.Value);
								}
							}
						}
						flag = true;
					}
				}
			}
			if (this._flushing && !this._filteringCompleted)
			{
				this._httpWriter.FilterIntegrated(false, iis7WorkerRequest);
				num = this._httpWriter.GetBufferedLength();
			}
			if (!this._clientDisconnected && (num > 0L || flag))
			{
				if (num == 0L && this._httpWriter.IgnoringFurtherWrites)
				{
					return;
				}
				this._httpWriter.Send(this._wr);
				iis7WorkerRequest.PushResponseToNative();
				this._httpWriter.DisposeIntegratedBuffers();
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001B788 File Offset: 0x0001A788
		private void ClearNativeResponse(bool clearEntity, bool clearHeaders, IIS7WorkerRequest wr)
		{
			wr.ClearResponse(clearEntity, clearHeaders);
			if (clearEntity)
			{
				this._httpWriter.ClearSubstitutionBlocks();
			}
		}

		// Token: 0x040010A8 RID: 4264
		private HttpWorkerRequest _wr;

		// Token: 0x040010A9 RID: 4265
		private HttpContext _context;

		// Token: 0x040010AA RID: 4266
		private HttpWriter _httpWriter;

		// Token: 0x040010AB RID: 4267
		private TextWriter _writer;

		// Token: 0x040010AC RID: 4268
		private HttpHeaderCollection _headers;

		// Token: 0x040010AD RID: 4269
		private bool _headersWritten;

		// Token: 0x040010AE RID: 4270
		private bool _completed;

		// Token: 0x040010AF RID: 4271
		private bool _ended;

		// Token: 0x040010B0 RID: 4272
		private bool _flushing;

		// Token: 0x040010B1 RID: 4273
		private bool _clientDisconnected;

		// Token: 0x040010B2 RID: 4274
		private bool _filteringCompleted;

		// Token: 0x040010B3 RID: 4275
		private bool _closeConnectionAfterError;

		// Token: 0x040010B4 RID: 4276
		private int _statusCode = 200;

		// Token: 0x040010B5 RID: 4277
		private string _statusDescription;

		// Token: 0x040010B6 RID: 4278
		private bool _bufferOutput = true;

		// Token: 0x040010B7 RID: 4279
		private string _contentType = "text/html";

		// Token: 0x040010B8 RID: 4280
		private string _charSet;

		// Token: 0x040010B9 RID: 4281
		private bool _customCharSet;

		// Token: 0x040010BA RID: 4282
		private bool _contentLengthSet;

		// Token: 0x040010BB RID: 4283
		private string _redirectLocation;

		// Token: 0x040010BC RID: 4284
		private bool _redirectLocationSet;

		// Token: 0x040010BD RID: 4285
		private Encoding _encoding;

		// Token: 0x040010BE RID: 4286
		private Encoder _encoder;

		// Token: 0x040010BF RID: 4287
		private Encoding _headerEncoding;

		// Token: 0x040010C0 RID: 4288
		private bool _cacheControlHeaderAdded;

		// Token: 0x040010C1 RID: 4289
		private HttpCachePolicy _cachePolicy;

		// Token: 0x040010C2 RID: 4290
		private ArrayList _cacheHeaders;

		// Token: 0x040010C3 RID: 4291
		private bool _suppressHeaders;

		// Token: 0x040010C4 RID: 4292
		private bool _suppressContentSet;

		// Token: 0x040010C5 RID: 4293
		private bool _suppressContent;

		// Token: 0x040010C6 RID: 4294
		private string _appPathModifier;

		// Token: 0x040010C7 RID: 4295
		private bool _isRequestBeingRedirected;

		// Token: 0x040010C8 RID: 4296
		private bool _useAdaptiveError;

		// Token: 0x040010C9 RID: 4297
		private bool _handlerHeadersGenerated;

		// Token: 0x040010CA RID: 4298
		private bool _sendCacheControlHeader;

		// Token: 0x040010CB RID: 4299
		private ArrayList _customHeaders;

		// Token: 0x040010CC RID: 4300
		private HttpCookieCollection _cookies;

		// Token: 0x040010CD RID: 4301
		private ResponseDependencyList _fileDependencyList;

		// Token: 0x040010CE RID: 4302
		private ResponseDependencyList _virtualPathDependencyList;

		// Token: 0x040010CF RID: 4303
		private ResponseDependencyList _cacheItemDependencyList;

		// Token: 0x040010D0 RID: 4304
		private AggregateCacheDependency _aggDependency;

		// Token: 0x040010D1 RID: 4305
		private ErrorFormatter _overrideErrorFormatter;

		// Token: 0x040010D2 RID: 4306
		private int _expiresInMinutes;

		// Token: 0x040010D3 RID: 4307
		private bool _expiresInMinutesSet;

		// Token: 0x040010D4 RID: 4308
		private DateTime _expiresAbsolute;

		// Token: 0x040010D5 RID: 4309
		private bool _expiresAbsoluteSet;

		// Token: 0x040010D6 RID: 4310
		private string _cacheControl;

		// Token: 0x040010D7 RID: 4311
		private bool _statusSet;

		// Token: 0x040010D8 RID: 4312
		private int _subStatusCode;

		// Token: 0x040010D9 RID: 4313
		private bool _versionHeaderSent;

		// Token: 0x040010DA RID: 4314
		private bool _contentTypeSet = true;

		// Token: 0x040010DB RID: 4315
		private bool _transferEncodingSet;

		// Token: 0x040010DC RID: 4316
		private bool _chunked;

		// Token: 0x040010DD RID: 4317
		internal static readonly string RedirectQueryStringVariable = "__redir";

		// Token: 0x040010DE RID: 4318
		internal static readonly string RedirectQueryStringValue = "1";

		// Token: 0x040010DF RID: 4319
		internal static readonly string RedirectQueryStringAssignment = HttpResponse.RedirectQueryStringVariable + "=" + HttpResponse.RedirectQueryStringValue;

		// Token: 0x040010E0 RID: 4320
		private static readonly string _redirectQueryString = "?" + HttpResponse.RedirectQueryStringAssignment;

		// Token: 0x040010E1 RID: 4321
		private static readonly string _redirectQueryStringInline = HttpResponse.RedirectQueryStringAssignment + "&";

		// Token: 0x040010E2 RID: 4322
		private static byte[] s_chunkSuffix = new byte[] { 13, 10 };

		// Token: 0x040010E3 RID: 4323
		private static byte[] s_chunkEnd = new byte[] { 48, 13, 10, 13, 10 };
	}
}
