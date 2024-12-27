using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;
using System.Web.UI;

namespace System.Web.Configuration
{
	// Token: 0x020001A6 RID: 422
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class BrowserCapabilitiesFactoryBase
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0004EAF4 File Offset: 0x0004DAF4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected IDictionary BrowserElements
		{
			get
			{
				if (this._browserElements == null)
				{
					lock (this._lock)
					{
						if (this._browserElements == null)
						{
							this._browserElements = Hashtable.Synchronized(new Hashtable(StringComparer.OrdinalIgnoreCase));
							this.PopulateBrowserElements(this._browserElements);
						}
					}
				}
				return this._browserElements;
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0004EB60 File Offset: 0x0004DB60
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void PopulateBrowserElements(IDictionary dictionary)
		{
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x0004EB62 File Offset: 0x0004DB62
		internal IDictionary InternalGetMatchedHeaders()
		{
			return this.MatchedHeaders;
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x0004EB6A File Offset: 0x0004DB6A
		internal IDictionary InternalGetBrowserElements()
		{
			return this.BrowserElements;
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0004EB74 File Offset: 0x0004DB74
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected IDictionary MatchedHeaders
		{
			get
			{
				if (this._matchedHeaders == null)
				{
					lock (this._lock)
					{
						if (this._matchedHeaders == null)
						{
							this._matchedHeaders = Hashtable.Synchronized(new Hashtable(24, StringComparer.OrdinalIgnoreCase));
							this.PopulateMatchedHeaders(this._matchedHeaders);
						}
					}
				}
				return this._matchedHeaders;
			}
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004EBE0 File Offset: 0x0004DBE0
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void PopulateMatchedHeaders(IDictionary dictionary)
		{
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x0004EBE4 File Offset: 0x0004DBE4
		internal int CompareFilters(string filter1, string filter2)
		{
			bool flag = string.IsNullOrEmpty(filter1);
			bool flag2 = string.IsNullOrEmpty(filter2);
			IDictionary browserElements = this.BrowserElements;
			bool flag3 = browserElements.Contains(filter1) || flag;
			bool flag4 = browserElements.Contains(filter2) || flag2;
			if (!flag3)
			{
				if (!flag4)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (!flag4)
				{
					return 1;
				}
				if (flag && !flag2)
				{
					return 1;
				}
				if (flag2 && !flag)
				{
					return -1;
				}
				if (flag && flag2)
				{
					return 0;
				}
				int num = (int)((Triplet)this.BrowserElements[filter1]).Third;
				int num2 = (int)((Triplet)this.BrowserElements[filter2]).Third;
				return num2 - num;
			}
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0004EC8B File Offset: 0x0004DC8B
		public virtual void ConfigureBrowserCapabilities(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004EC8D File Offset: 0x0004DC8D
		public virtual void ConfigureCustomCapabilities(NameValueCollection headers, HttpBrowserCapabilities browserCaps)
		{
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0004EC90 File Offset: 0x0004DC90
		internal static string GetBrowserCapKey(IDictionary headers, HttpRequest request)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in headers.Keys)
			{
				string text = (string)obj;
				if (text.Length == 0)
				{
					stringBuilder.Append(HttpCapabilitiesEvaluator.GetUserAgent(request));
				}
				else
				{
					stringBuilder.Append(request.Headers[text]);
				}
				stringBuilder.Append("\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x0004ED24 File Offset: 0x0004DD24
		internal HttpBrowserCapabilities GetHttpBrowserCapabilities(HttpRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			NameValueCollection headers = request.Headers;
			HttpBrowserCapabilities httpBrowserCapabilities = new HttpBrowserCapabilities();
			Hashtable hashtable = new Hashtable(180, StringComparer.OrdinalIgnoreCase);
			hashtable[string.Empty] = HttpCapabilitiesEvaluator.GetUserAgent(request);
			httpBrowserCapabilities.Capabilities = hashtable;
			this.ConfigureBrowserCapabilities(headers, httpBrowserCapabilities);
			this.ConfigureCustomCapabilities(headers, httpBrowserCapabilities);
			return httpBrowserCapabilities;
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004ED85 File Offset: 0x0004DD85
		protected bool IsBrowserUnknown(HttpCapabilitiesBase browserCaps)
		{
			return browserCaps.Browsers == null || browserCaps.Browsers.Count <= 1;
		}

		// Token: 0x040016D3 RID: 5843
		private IDictionary _matchedHeaders;

		// Token: 0x040016D4 RID: 5844
		private IDictionary _browserElements;

		// Token: 0x040016D5 RID: 5845
		private object _lock = new object();
	}
}
