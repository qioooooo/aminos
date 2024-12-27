using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Caching;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x02000380 RID: 896
	internal class StateApplication : IHttpHandler
	{
		// Token: 0x06002B5C RID: 11100 RVA: 0x000C10CB File Offset: 0x000C00CB
		internal StateApplication()
		{
			this._removedHandler = new CacheItemRemovedCallback(this.OnCacheItemRemoved);
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000C10E8 File Offset: 0x000C00E8
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = null;
			switch (context.Request.HttpVerb)
			{
			case HttpVerb.GET:
				this.DoGet(context);
				return;
			case HttpVerb.PUT:
				this.DoPut(context);
				return;
			case HttpVerb.HEAD:
				this.DoHead(context);
				return;
			case HttpVerb.DELETE:
				this.DoDelete(context);
				return;
			}
			this.DoUnknown(context);
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000C1156 File Offset: 0x000C0156
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x000C1159 File Offset: 0x000C0159
		private string CreateKey(HttpRequest request)
		{
			return "k" + HttpUtility.UrlDecode(request.RawUrl);
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x000C1170 File Offset: 0x000C0170
		private void ReportInvalidHeader(HttpContext context, string header)
		{
			HttpResponse response = context.Response;
			response.StatusCode = 400;
			response.Write("<html><head><title>Bad Request</title></head>\r\n");
			response.Write("<body><h1>Http/1.1 400 Bad Request</h1>");
			response.Write("Invalid header <b>" + header + "</b></body></html>");
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x000C11BC File Offset: 0x000C01BC
		private void ReportLocked(HttpContext context, CachedContent content)
		{
			HttpResponse response = context.Response;
			response.StatusCode = 423;
			DateTime dateTime = DateTimeUtil.ConvertToLocalTime(content._utcLockDate);
			long num = (DateTime.UtcNow - content._utcLockDate).Ticks / 10000000L;
			response.AppendHeader("LockDate", dateTime.Ticks.ToString(CultureInfo.InvariantCulture));
			response.AppendHeader("LockAge", num.ToString(CultureInfo.InvariantCulture));
			response.AppendHeader("LockCookie", content._lockCookie.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x000C125C File Offset: 0x000C025C
		private void ReportActionFlags(HttpContext context, int flags)
		{
			HttpResponse response = context.Response;
			response.AppendHeader("ActionFlags", flags.ToString(CultureInfo.InvariantCulture));
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x000C1287 File Offset: 0x000C0287
		private void ReportNotFound(HttpContext context)
		{
			context.Response.StatusCode = 404;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000C129C File Offset: 0x000C029C
		private bool GetOptionalNonNegativeInt32HeaderValue(HttpContext context, string header, out int value)
		{
			value = -1;
			string text = context.Request.Headers[header];
			bool flag;
			if (text == null)
			{
				flag = true;
			}
			else
			{
				flag = false;
				try
				{
					value = int.Parse(text, CultureInfo.InvariantCulture);
					if (value >= 0)
					{
						flag = true;
					}
				}
				catch
				{
				}
			}
			if (!flag)
			{
				this.ReportInvalidHeader(context, header);
			}
			return flag;
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000C12FC File Offset: 0x000C02FC
		private bool GetRequiredNonNegativeInt32HeaderValue(HttpContext context, string header, out int value)
		{
			bool flag = this.GetOptionalNonNegativeInt32HeaderValue(context, header, out value);
			if (flag && value == -1)
			{
				flag = false;
				this.ReportInvalidHeader(context, header);
			}
			return flag;
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000C1328 File Offset: 0x000C0328
		private bool GetOptionalInt32HeaderValue(HttpContext context, string header, out int value, out bool found)
		{
			found = false;
			value = 0;
			string text = context.Request.Headers[header];
			bool flag;
			if (text == null)
			{
				flag = true;
			}
			else
			{
				flag = false;
				try
				{
					value = int.Parse(text, CultureInfo.InvariantCulture);
					flag = true;
					found = true;
				}
				catch
				{
				}
			}
			if (!flag)
			{
				this.ReportInvalidHeader(context, header);
			}
			return flag;
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000C138C File Offset: 0x000C038C
		internal void DoGet(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			string text = this.CreateKey(request);
			CacheEntry cacheEntry = (CacheEntry)HttpRuntime.CacheInternal.Get(text, CacheGetOptions.ReturnCacheEntry);
			if (cacheEntry == null)
			{
				this.ReportNotFound(context);
				return;
			}
			string text2 = request.Headers["Http_Exclusive"];
			CachedContent cachedContent = (CachedContent)cacheEntry.Value;
			cachedContent._spinLock.AcquireWriterLock();
			try
			{
				if (cachedContent._content == null)
				{
					this.ReportNotFound(context);
				}
				else
				{
					int extraFlags = cachedContent._extraFlags;
					if ((extraFlags & 1) != 0 && extraFlags == Interlocked.CompareExchange(ref cachedContent._extraFlags, extraFlags & -2, extraFlags))
					{
						this.ReportActionFlags(context, 1);
					}
					if (text2 == "release")
					{
						int num;
						if (this.GetRequiredNonNegativeInt32HeaderValue(context, "Http_LockCookie", out num))
						{
							if (cachedContent._locked)
							{
								if (num == cachedContent._lockCookie)
								{
									cachedContent._locked = false;
								}
								else
								{
									this.ReportLocked(context, cachedContent);
								}
							}
							else
							{
								context.Response.StatusCode = 200;
							}
						}
					}
					else if (cachedContent._locked)
					{
						this.ReportLocked(context, cachedContent);
					}
					else
					{
						if (text2 == "acquire")
						{
							cachedContent._locked = true;
							cachedContent._utcLockDate = DateTime.UtcNow;
							cachedContent._lockCookie++;
							response.AppendHeader("LockCookie", cachedContent._lockCookie.ToString(CultureInfo.InvariantCulture));
						}
						response.AppendHeader("Timeout", ((int)(cacheEntry.SlidingExpiration.Ticks / 600000000L)).ToString(CultureInfo.InvariantCulture));
						Stream outputStream = response.OutputStream;
						byte[] content = cachedContent._content;
						outputStream.Write(content, 0, content.Length);
						response.Flush();
					}
				}
			}
			finally
			{
				cachedContent._spinLock.ReleaseWriterLock();
			}
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000C1588 File Offset: 0x000C0588
		internal void DoPut(HttpContext context)
		{
			IntPtr intPtr = this.FinishPut(context);
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.STWNDDeleteStateItem(intPtr);
			}
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x000C15B0 File Offset: 0x000C05B0
		private unsafe IntPtr FinishPut(HttpContext context)
		{
			HttpRequest request = context.Request;
			HttpResponse response = context.Response;
			int num = 1;
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			Stream inputStream = request.InputStream;
			int num2 = (int)(inputStream.Length - inputStream.Position);
			byte[] array = new byte[num2];
			inputStream.Read(array, 0, array.Length);
			IntPtr intPtr;
			fixed (byte* ptr = array)
			{
				intPtr = (IntPtr)(*(IntPtr*)ptr);
			}
			int num3;
			if (!this.GetOptionalNonNegativeInt32HeaderValue(context, "Http_Timeout", out num3))
			{
				return intPtr;
			}
			if (num3 == -1)
			{
				num3 = 20;
			}
			if (num3 > 525600)
			{
				this.ReportInvalidHeader(context, "Http_Timeout");
				return intPtr;
			}
			TimeSpan timeSpan = new TimeSpan(0, num3, 0);
			int num4;
			bool flag;
			if (!this.GetOptionalInt32HeaderValue(context, "Http_ExtraFlags", out num4, out flag))
			{
				return intPtr;
			}
			if (!flag)
			{
				num4 = 0;
			}
			string text = this.CreateKey(request);
			CacheEntry cacheEntry = (CacheEntry)cacheInternal.Get(text, CacheGetOptions.ReturnCacheEntry);
			if (cacheEntry != null)
			{
				if ((1 & num4) == 1)
				{
					return intPtr;
				}
				int num5;
				if (!this.GetOptionalNonNegativeInt32HeaderValue(context, "Http_LockCookie", out num5))
				{
					return intPtr;
				}
				CachedContent cachedContent = (CachedContent)cacheEntry.Value;
				cachedContent._spinLock.AcquireWriterLock();
				try
				{
					if (cachedContent._content == null)
					{
						this.ReportNotFound(context);
						return intPtr;
					}
					if (cachedContent._locked && (num5 == -1 || num5 != cachedContent._lockCookie))
					{
						this.ReportLocked(context, cachedContent);
						return intPtr;
					}
					if (cacheEntry.SlidingExpiration == timeSpan && cachedContent._content != null)
					{
						IntPtr stateItem = cachedContent._stateItem;
						cachedContent._content = array;
						cachedContent._stateItem = intPtr;
						cachedContent._locked = false;
						return stateItem;
					}
					cachedContent._extraFlags |= 2;
					cachedContent._locked = true;
					cachedContent._lockCookie = 0;
					num = num5;
				}
				finally
				{
					cachedContent._spinLock.ReleaseWriterLock();
				}
			}
			CachedContent cachedContent2 = new CachedContent(array, intPtr, false, DateTime.MinValue, num, num4);
			cacheInternal.UtcInsert(text, cachedContent2, null, Cache.NoAbsoluteExpiration, timeSpan, CacheItemPriority.NotRemovable, this._removedHandler);
			if (cacheEntry == null)
			{
				this.IncrementStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_TOTAL);
				this.IncrementStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_ACTIVE);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000C17EC File Offset: 0x000C07EC
		internal void DoDelete(HttpContext context)
		{
			string text = this.CreateKey(context.Request);
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			CachedContent cachedContent = (CachedContent)cacheInternal.Get(text);
			if (cachedContent == null)
			{
				this.ReportNotFound(context);
				return;
			}
			int num;
			if (!this.GetOptionalNonNegativeInt32HeaderValue(context, "Http_LockCookie", out num))
			{
				return;
			}
			cachedContent._spinLock.AcquireWriterLock();
			try
			{
				if (cachedContent._content == null)
				{
					this.ReportNotFound(context);
					return;
				}
				if (cachedContent._locked && (num == -1 || cachedContent._lockCookie != num))
				{
					this.ReportLocked(context, cachedContent);
					return;
				}
				cachedContent._locked = true;
				cachedContent._lockCookie = 0;
			}
			finally
			{
				cachedContent._spinLock.ReleaseWriterLock();
			}
			cacheInternal.Remove(text);
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000C18A4 File Offset: 0x000C08A4
		internal void DoHead(HttpContext context)
		{
			string text = this.CreateKey(context.Request);
			if (HttpRuntime.CacheInternal.Get(text) == null)
			{
				this.ReportNotFound(context);
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000C18D4 File Offset: 0x000C08D4
		internal void DoUnknown(HttpContext context)
		{
			context.Response.StatusCode = 400;
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000C18E8 File Offset: 0x000C08E8
		private void OnCacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
		{
			CachedContent cachedContent = (CachedContent)value;
			cachedContent._spinLock.AcquireWriterLock();
			IntPtr stateItem;
			try
			{
				stateItem = cachedContent._stateItem;
				cachedContent._content = null;
				cachedContent._stateItem = IntPtr.Zero;
			}
			finally
			{
				cachedContent._spinLock.ReleaseWriterLock();
			}
			UnsafeNativeMethods.STWNDDeleteStateItem(stateItem);
			if ((cachedContent._extraFlags & 2) != 0)
			{
				return;
			}
			switch (reason)
			{
			case CacheItemRemovedReason.Removed:
				this.IncrementStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_ABANDONED);
				break;
			case CacheItemRemovedReason.Expired:
				this.IncrementStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_TIMED_OUT);
				break;
			}
			this.DecrementStateServiceCounter(StateServicePerfCounter.STATE_SERVICE_SESSIONS_ACTIVE);
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000C1980 File Offset: 0x000C0980
		private void DecrementStateServiceCounter(StateServicePerfCounter counter)
		{
			if (HttpRuntime.ShutdownInProgress)
			{
				return;
			}
			PerfCounters.DecrementStateServiceCounter(counter);
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000C1990 File Offset: 0x000C0990
		private void IncrementStateServiceCounter(StateServicePerfCounter counter)
		{
			if (HttpRuntime.ShutdownInProgress)
			{
				return;
			}
			PerfCounters.IncrementStateServiceCounter(counter);
		}

		// Token: 0x0400200F RID: 8207
		private CacheItemRemovedCallback _removedHandler;
	}
}
