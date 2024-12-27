using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x0200036C RID: 876
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpSessionState : ICollection, IEnumerable
	{
		// Token: 0x06002A61 RID: 10849 RVA: 0x000BC778 File Offset: 0x000BB778
		internal HttpSessionState(IHttpSessionState container)
		{
			this._container = container;
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x000BC787 File Offset: 0x000BB787
		internal IHttpSessionState Container
		{
			get
			{
				return this._container;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06002A63 RID: 10851 RVA: 0x000BC78F File Offset: 0x000BB78F
		public string SessionID
		{
			get
			{
				return this._container.SessionID;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x000BC79C File Offset: 0x000BB79C
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x000BC7A9 File Offset: 0x000BB7A9
		public int Timeout
		{
			get
			{
				return this._container.Timeout;
			}
			set
			{
				this._container.Timeout = value;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002A66 RID: 10854 RVA: 0x000BC7B7 File Offset: 0x000BB7B7
		public bool IsNewSession
		{
			get
			{
				return this._container.IsNewSession;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002A67 RID: 10855 RVA: 0x000BC7C4 File Offset: 0x000BB7C4
		public SessionStateMode Mode
		{
			get
			{
				return this._container.Mode;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06002A68 RID: 10856 RVA: 0x000BC7D1 File Offset: 0x000BB7D1
		public bool IsCookieless
		{
			get
			{
				return this._container.IsCookieless;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002A69 RID: 10857 RVA: 0x000BC7DE File Offset: 0x000BB7DE
		public HttpCookieMode CookieMode
		{
			get
			{
				return this._container.CookieMode;
			}
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000BC7EB File Offset: 0x000BB7EB
		public void Abandon()
		{
			this._container.Abandon();
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000BC7F8 File Offset: 0x000BB7F8
		// (set) Token: 0x06002A6C RID: 10860 RVA: 0x000BC805 File Offset: 0x000BB805
		public int LCID
		{
			get
			{
				return this._container.LCID;
			}
			set
			{
				this._container.LCID = value;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000BC813 File Offset: 0x000BB813
		// (set) Token: 0x06002A6E RID: 10862 RVA: 0x000BC820 File Offset: 0x000BB820
		public int CodePage
		{
			get
			{
				return this._container.CodePage;
			}
			set
			{
				this._container.CodePage = value;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000BC82E File Offset: 0x000BB82E
		public HttpSessionState Contents
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06002A70 RID: 10864 RVA: 0x000BC831 File Offset: 0x000BB831
		public HttpStaticObjectsCollection StaticObjects
		{
			get
			{
				return this._container.StaticObjects;
			}
		}

		// Token: 0x1700090B RID: 2315
		public object this[string name]
		{
			get
			{
				return this._container[name];
			}
			set
			{
				this._container[name] = value;
			}
		}

		// Token: 0x1700090C RID: 2316
		public object this[int index]
		{
			get
			{
				return this._container[index];
			}
			set
			{
				this._container[index] = value;
			}
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000BC878 File Offset: 0x000BB878
		public void Add(string name, object value)
		{
			this._container[name] = value;
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000BC887 File Offset: 0x000BB887
		public void Remove(string name)
		{
			this._container.Remove(name);
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000BC895 File Offset: 0x000BB895
		public void RemoveAt(int index)
		{
			this._container.RemoveAt(index);
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000BC8A3 File Offset: 0x000BB8A3
		public void Clear()
		{
			this._container.Clear();
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x000BC8B0 File Offset: 0x000BB8B0
		public void RemoveAll()
		{
			this.Clear();
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06002A7A RID: 10874 RVA: 0x000BC8B8 File Offset: 0x000BB8B8
		public int Count
		{
			get
			{
				return this._container.Count;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06002A7B RID: 10875 RVA: 0x000BC8C5 File Offset: 0x000BB8C5
		public NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return this._container.Keys;
			}
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000BC8D2 File Offset: 0x000BB8D2
		public IEnumerator GetEnumerator()
		{
			return this._container.GetEnumerator();
		}

		// Token: 0x06002A7D RID: 10877 RVA: 0x000BC8DF File Offset: 0x000BB8DF
		public void CopyTo(Array array, int index)
		{
			this._container.CopyTo(array, index);
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06002A7E RID: 10878 RVA: 0x000BC8EE File Offset: 0x000BB8EE
		public object SyncRoot
		{
			get
			{
				return this._container.SyncRoot;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000BC8FB File Offset: 0x000BB8FB
		public bool IsReadOnly
		{
			get
			{
				return this._container.IsReadOnly;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x000BC908 File Offset: 0x000BB908
		public bool IsSynchronized
		{
			get
			{
				return this._container.IsSynchronized;
			}
		}

		// Token: 0x04001F69 RID: 8041
		private IHttpSessionState _container;
	}
}
