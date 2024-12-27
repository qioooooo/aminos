using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020003D3 RID: 979
	public class HttpListenerPrefixCollection : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06001EE4 RID: 7908 RVA: 0x0007782C File Offset: 0x0007682C
		internal HttpListenerPrefixCollection(HttpListener listener)
		{
			this.m_HttpListener = listener;
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x0007783C File Offset: 0x0007683C
		public void CopyTo(Array array, int offset)
		{
			this.m_HttpListener.CheckDisposed();
			if (this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("array", SR.GetString("net_array_too_small"));
			}
			if (offset + this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num = 0;
			foreach (object obj in this.m_HttpListener.m_UriPrefixes.Keys)
			{
				string text = (string)obj;
				array.SetValue(text, offset + num++);
			}
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x000778F4 File Offset: 0x000768F4
		public void CopyTo(string[] array, int offset)
		{
			this.m_HttpListener.CheckDisposed();
			if (this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("array", SR.GetString("net_array_too_small"));
			}
			if (offset + this.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			int num = 0;
			foreach (object obj in this.m_HttpListener.m_UriPrefixes.Keys)
			{
				string text = (string)obj;
				array[offset + num++] = text;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000779A0 File Offset: 0x000769A0
		public int Count
		{
			get
			{
				return this.m_HttpListener.m_UriPrefixes.Count;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000779B2 File Offset: 0x000769B2
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000779B5 File Offset: 0x000769B5
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000779B8 File Offset: 0x000769B8
		public void Add(string uriPrefix)
		{
			this.m_HttpListener.AddPrefix(uriPrefix);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000779C6 File Offset: 0x000769C6
		public bool Contains(string uriPrefix)
		{
			return this.m_HttpListener.m_UriPrefixes.Contains(uriPrefix);
		}

		// Token: 0x06001EEC RID: 7916 RVA: 0x000779D9 File Offset: 0x000769D9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x000779DC File Offset: 0x000769DC
		public IEnumerator<string> GetEnumerator()
		{
			return new ListenerPrefixEnumerator(this.m_HttpListener.m_UriPrefixes.Keys.GetEnumerator());
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x000779F8 File Offset: 0x000769F8
		public bool Remove(string uriPrefix)
		{
			return this.m_HttpListener.RemovePrefix(uriPrefix);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x00077A06 File Offset: 0x00076A06
		public void Clear()
		{
			this.m_HttpListener.RemoveAll(true);
		}

		// Token: 0x04001E80 RID: 7808
		private HttpListener m_HttpListener;
	}
}
