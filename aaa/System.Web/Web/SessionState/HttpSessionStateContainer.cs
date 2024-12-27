using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace System.Web.SessionState
{
	// Token: 0x0200036D RID: 877
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpSessionStateContainer : IHttpSessionState
	{
		// Token: 0x06002A81 RID: 10881 RVA: 0x000BC918 File Offset: 0x000BB918
		public HttpSessionStateContainer(string id, ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout, bool newSession, HttpCookieMode cookieMode, SessionStateMode mode, bool isReadonly)
			: this(null, id, sessionItems, staticObjects, timeout, newSession, cookieMode, mode, isReadonly)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000BC948 File Offset: 0x000BB948
		internal HttpSessionStateContainer(SessionStateModule stateModule, string id, ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout, bool newSession, HttpCookieMode cookieMode, SessionStateMode mode, bool isReadonly)
		{
			this._stateModule = stateModule;
			this._id = id;
			this._sessionItems = sessionItems;
			this._staticObjects = staticObjects;
			this._timeout = timeout;
			this._newSession = newSession;
			this._cookieMode = cookieMode;
			this._mode = mode;
			this._isReadonly = isReadonly;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x000BC9A0 File Offset: 0x000BB9A0
		internal HttpSessionStateContainer()
		{
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x000BC9A8 File Offset: 0x000BB9A8
		public string SessionID
		{
			get
			{
				if (this._id == null)
				{
					this._id = this._stateModule.DelayedGetSessionId();
				}
				return this._id;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000BC9C9 File Offset: 0x000BB9C9
		// (set) Token: 0x06002A86 RID: 10886 RVA: 0x000BC9D4 File Offset: 0x000BB9D4
		public int Timeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException(SR.GetString("Timeout_must_be_positive"));
				}
				if (value > 525600 && (this.Mode == SessionStateMode.InProc || this.Mode == SessionStateMode.StateServer))
				{
					throw new ArgumentException(SR.GetString("Invalid_cache_based_session_timeout"));
				}
				this._timeout = value;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06002A87 RID: 10887 RVA: 0x000BCA26 File Offset: 0x000BBA26
		public bool IsNewSession
		{
			get
			{
				return this._newSession;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06002A88 RID: 10888 RVA: 0x000BCA2E File Offset: 0x000BBA2E
		public SessionStateMode Mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06002A89 RID: 10889 RVA: 0x000BCA36 File Offset: 0x000BBA36
		public bool IsCookieless
		{
			get
			{
				if (this._stateModule != null)
				{
					return this._stateModule.SessionIDManagerUseCookieless;
				}
				return this.CookieMode == HttpCookieMode.UseUri;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06002A8A RID: 10890 RVA: 0x000BCA55 File Offset: 0x000BBA55
		public HttpCookieMode CookieMode
		{
			get
			{
				return this._cookieMode;
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000BCA5D File Offset: 0x000BBA5D
		public void Abandon()
		{
			this._abandon = true;
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06002A8C RID: 10892 RVA: 0x000BCA66 File Offset: 0x000BBA66
		// (set) Token: 0x06002A8D RID: 10893 RVA: 0x000BCA77 File Offset: 0x000BBA77
		public int LCID
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture.LCID;
			}
			set
			{
				Thread.CurrentThread.CurrentCulture = HttpServerUtility.CreateReadOnlyCultureInfo(value);
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06002A8E RID: 10894 RVA: 0x000BCA89 File Offset: 0x000BBA89
		// (set) Token: 0x06002A8F RID: 10895 RVA: 0x000BCAB1 File Offset: 0x000BBAB1
		public int CodePage
		{
			get
			{
				if (HttpContext.Current != null)
				{
					return HttpContext.Current.Response.ContentEncoding.CodePage;
				}
				return Encoding.Default.CodePage;
			}
			set
			{
				if (HttpContext.Current != null)
				{
					HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding(value);
				}
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000BCACF File Offset: 0x000BBACF
		public bool IsAbandoned
		{
			get
			{
				return this._abandon;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06002A91 RID: 10897 RVA: 0x000BCAD7 File Offset: 0x000BBAD7
		public HttpStaticObjectsCollection StaticObjects
		{
			get
			{
				return this._staticObjects;
			}
		}

		// Token: 0x1700091C RID: 2332
		public object this[string name]
		{
			get
			{
				return this._sessionItems[name];
			}
			set
			{
				this._sessionItems[name] = value;
			}
		}

		// Token: 0x1700091D RID: 2333
		public object this[int index]
		{
			get
			{
				return this._sessionItems[index];
			}
			set
			{
				this._sessionItems[index] = value;
			}
		}

		// Token: 0x06002A96 RID: 10902 RVA: 0x000BCB19 File Offset: 0x000BBB19
		public void Add(string name, object value)
		{
			this._sessionItems[name] = value;
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x000BCB28 File Offset: 0x000BBB28
		public void Remove(string name)
		{
			this._sessionItems.Remove(name);
		}

		// Token: 0x06002A98 RID: 10904 RVA: 0x000BCB36 File Offset: 0x000BBB36
		public void RemoveAt(int index)
		{
			this._sessionItems.RemoveAt(index);
		}

		// Token: 0x06002A99 RID: 10905 RVA: 0x000BCB44 File Offset: 0x000BBB44
		public void Clear()
		{
			this._sessionItems.Clear();
		}

		// Token: 0x06002A9A RID: 10906 RVA: 0x000BCB51 File Offset: 0x000BBB51
		public void RemoveAll()
		{
			this.Clear();
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000BCB59 File Offset: 0x000BBB59
		public int Count
		{
			get
			{
				return this._sessionItems.Count;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x000BCB66 File Offset: 0x000BBB66
		public NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return this._sessionItems.Keys;
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000BCB73 File Offset: 0x000BBB73
		public IEnumerator GetEnumerator()
		{
			return this._sessionItems.GetEnumerator();
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000BCB80 File Offset: 0x000BBB80
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000BCBB0 File Offset: 0x000BBBB0
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x000BCBB3 File Offset: 0x000BBBB3
		public bool IsReadOnly
		{
			get
			{
				return this._isReadonly;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000BCBBB File Offset: 0x000BBBBB
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001F6A RID: 8042
		private string _id;

		// Token: 0x04001F6B RID: 8043
		private ISessionStateItemCollection _sessionItems;

		// Token: 0x04001F6C RID: 8044
		private HttpStaticObjectsCollection _staticObjects;

		// Token: 0x04001F6D RID: 8045
		private int _timeout;

		// Token: 0x04001F6E RID: 8046
		private bool _newSession;

		// Token: 0x04001F6F RID: 8047
		private HttpCookieMode _cookieMode;

		// Token: 0x04001F70 RID: 8048
		private SessionStateMode _mode;

		// Token: 0x04001F71 RID: 8049
		private bool _abandon;

		// Token: 0x04001F72 RID: 8050
		private bool _isReadonly;

		// Token: 0x04001F73 RID: 8051
		private SessionStateModule _stateModule;
	}
}
