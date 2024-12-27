using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x02000505 RID: 1285
	internal abstract class ProxyChain : IEnumerable<Uri>, IEnumerable, IDisposable
	{
		// Token: 0x060027EA RID: 10218 RVA: 0x000A4BFB File Offset: 0x000A3BFB
		protected ProxyChain(Uri destination)
		{
			this.m_Destination = destination;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000A4C18 File Offset: 0x000A3C18
		public IEnumerator<Uri> GetEnumerator()
		{
			ProxyChain.ProxyEnumerator proxyEnumerator = new ProxyChain.ProxyEnumerator(this);
			if (this.m_MainEnumerator == null)
			{
				this.m_MainEnumerator = proxyEnumerator;
			}
			return proxyEnumerator;
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x000A4C3C File Offset: 0x000A3C3C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x000A4C44 File Offset: 0x000A3C44
		public virtual void Dispose()
		{
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x000A4C46 File Offset: 0x000A3C46
		internal IEnumerator<Uri> Enumerator
		{
			get
			{
				if (this.m_MainEnumerator != null)
				{
					return this.m_MainEnumerator;
				}
				return this.GetEnumerator();
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x060027EF RID: 10223 RVA: 0x000A4C5D File Offset: 0x000A3C5D
		internal Uri Destination
		{
			get
			{
				return this.m_Destination;
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000A4C65 File Offset: 0x000A3C65
		internal virtual void Abort()
		{
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000A4C67 File Offset: 0x000A3C67
		internal bool HttpAbort(HttpWebRequest request, WebException webException)
		{
			this.Abort();
			return true;
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x000A4C70 File Offset: 0x000A3C70
		internal HttpAbortDelegate HttpAbortDelegate
		{
			get
			{
				if (this.m_HttpAbortDelegate == null)
				{
					this.m_HttpAbortDelegate = new HttpAbortDelegate(this.HttpAbort);
				}
				return this.m_HttpAbortDelegate;
			}
		}

		// Token: 0x060027F3 RID: 10227
		protected abstract bool GetNextProxy(out Uri proxy);

		// Token: 0x0400273C RID: 10044
		private List<Uri> m_Cache = new List<Uri>();

		// Token: 0x0400273D RID: 10045
		private bool m_CacheComplete;

		// Token: 0x0400273E RID: 10046
		private ProxyChain.ProxyEnumerator m_MainEnumerator;

		// Token: 0x0400273F RID: 10047
		private Uri m_Destination;

		// Token: 0x04002740 RID: 10048
		private HttpAbortDelegate m_HttpAbortDelegate;

		// Token: 0x02000506 RID: 1286
		private class ProxyEnumerator : IEnumerator<Uri>, IDisposable, IEnumerator
		{
			// Token: 0x060027F4 RID: 10228 RVA: 0x000A4C92 File Offset: 0x000A3C92
			internal ProxyEnumerator(ProxyChain chain)
			{
				this.m_Chain = chain;
			}

			// Token: 0x1700083E RID: 2110
			// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000A4CA8 File Offset: 0x000A3CA8
			public Uri Current
			{
				get
				{
					if (this.m_Finished || this.m_CurrentIndex < 0)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					return this.m_Chain.m_Cache[this.m_CurrentIndex];
				}
			}

			// Token: 0x1700083F RID: 2111
			// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000A4CE1 File Offset: 0x000A3CE1
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060027F7 RID: 10231 RVA: 0x000A4CEC File Offset: 0x000A3CEC
			public bool MoveNext()
			{
				if (this.m_Finished)
				{
					return false;
				}
				checked
				{
					this.m_CurrentIndex++;
					if (this.m_Chain.m_Cache.Count > this.m_CurrentIndex)
					{
						return true;
					}
					if (this.m_Chain.m_CacheComplete)
					{
						this.m_Finished = true;
						return false;
					}
					bool flag;
					lock (this.m_Chain.m_Cache)
					{
						if (this.m_Chain.m_Cache.Count > this.m_CurrentIndex)
						{
							flag = true;
						}
						else if (this.m_Chain.m_CacheComplete)
						{
							this.m_Finished = true;
							flag = false;
						}
						else
						{
							Uri uri;
							while (this.m_Chain.GetNextProxy(out uri))
							{
								if (uri == null)
								{
									if (this.m_TriedDirect)
									{
										continue;
									}
									this.m_TriedDirect = true;
								}
								this.m_Chain.m_Cache.Add(uri);
								return true;
							}
							this.m_Finished = true;
							this.m_Chain.m_CacheComplete = true;
							flag = false;
						}
					}
					return flag;
				}
			}

			// Token: 0x060027F8 RID: 10232 RVA: 0x000A4DF4 File Offset: 0x000A3DF4
			public void Reset()
			{
				this.m_Finished = false;
				this.m_CurrentIndex = -1;
			}

			// Token: 0x060027F9 RID: 10233 RVA: 0x000A4E04 File Offset: 0x000A3E04
			public void Dispose()
			{
			}

			// Token: 0x04002741 RID: 10049
			private ProxyChain m_Chain;

			// Token: 0x04002742 RID: 10050
			private bool m_Finished;

			// Token: 0x04002743 RID: 10051
			private int m_CurrentIndex = -1;

			// Token: 0x04002744 RID: 10052
			private bool m_TriedDirect;
		}
	}
}
