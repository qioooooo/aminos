using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x0200036B RID: 875
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHttpSessionState
	{
		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06002A44 RID: 10820
		string SessionID { get; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002A45 RID: 10821
		// (set) Token: 0x06002A46 RID: 10822
		int Timeout { get; set; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002A47 RID: 10823
		bool IsNewSession { get; }

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002A48 RID: 10824
		SessionStateMode Mode { get; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002A49 RID: 10825
		bool IsCookieless { get; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002A4A RID: 10826
		HttpCookieMode CookieMode { get; }

		// Token: 0x06002A4B RID: 10827
		void Abandon();

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002A4C RID: 10828
		// (set) Token: 0x06002A4D RID: 10829
		int LCID { get; set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002A4E RID: 10830
		// (set) Token: 0x06002A4F RID: 10831
		int CodePage { get; set; }

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002A50 RID: 10832
		HttpStaticObjectsCollection StaticObjects { get; }

		// Token: 0x170008F9 RID: 2297
		object this[string name] { get; set; }

		// Token: 0x170008FA RID: 2298
		object this[int index] { get; set; }

		// Token: 0x06002A55 RID: 10837
		void Add(string name, object value);

		// Token: 0x06002A56 RID: 10838
		void Remove(string name);

		// Token: 0x06002A57 RID: 10839
		void RemoveAt(int index);

		// Token: 0x06002A58 RID: 10840
		void Clear();

		// Token: 0x06002A59 RID: 10841
		void RemoveAll();

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002A5A RID: 10842
		int Count { get; }

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002A5B RID: 10843
		NameObjectCollectionBase.KeysCollection Keys { get; }

		// Token: 0x06002A5C RID: 10844
		IEnumerator GetEnumerator();

		// Token: 0x06002A5D RID: 10845
		void CopyTo(Array array, int index);

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002A5E RID: 10846
		object SyncRoot { get; }

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002A5F RID: 10847
		bool IsReadOnly { get; }

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06002A60 RID: 10848
		bool IsSynchronized { get; }
	}
}
