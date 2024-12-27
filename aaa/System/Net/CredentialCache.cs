using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200039B RID: 923
	public class CredentialCache : ICredentials, ICredentialsByHost, IEnumerable
	{
		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001CC2 RID: 7362 RVA: 0x0006D8AD File Offset: 0x0006C8AD
		internal bool IsDefaultInCache
		{
			get
			{
				return this.m_NumbDefaultCredInCache != 0;
			}
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x0006D8DC File Offset: 0x0006C8DC
		public void Add(Uri uriPrefix, string authType, NetworkCredential cred)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			if (cred is SystemNetworkCredential && string.Compare(authType, "NTLM", StringComparison.OrdinalIgnoreCase) != 0 && (!DigestClient.WDigestAvailable || string.Compare(authType, "Digest", StringComparison.OrdinalIgnoreCase) != 0) && string.Compare(authType, "Kerberos", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(authType, "Negotiate", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("net_nodefaultcreds", new object[] { authType }), "authType");
			}
			this.m_version++;
			CredentialKey credentialKey = new CredentialKey(uriPrefix, authType);
			this.cache.Add(credentialKey, cred);
			if (cred is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache++;
			}
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x0006D9AC File Offset: 0x0006C9AC
		public void Add(string host, int port, string authenticationType, NetworkCredential credential)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException("authenticationType");
			}
			if (host.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "host" }));
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (credential is SystemNetworkCredential && string.Compare(authenticationType, "NTLM", StringComparison.OrdinalIgnoreCase) != 0 && (!DigestClient.WDigestAvailable || string.Compare(authenticationType, "Digest", StringComparison.OrdinalIgnoreCase) != 0) && string.Compare(authenticationType, "Kerberos", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(authenticationType, "Negotiate", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("net_nodefaultcreds", new object[] { authenticationType }), "authenticationType");
			}
			this.m_version++;
			CredentialHostKey credentialHostKey = new CredentialHostKey(host, port, authenticationType);
			this.cacheForHosts.Add(credentialHostKey, credential);
			if (credential is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache++;
			}
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x0006DAB4 File Offset: 0x0006CAB4
		public void Remove(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null || authType == null)
			{
				return;
			}
			this.m_version++;
			CredentialKey credentialKey = new CredentialKey(uriPrefix, authType);
			if (this.cache[credentialKey] is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache--;
			}
			this.cache.Remove(credentialKey);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x0006DB14 File Offset: 0x0006CB14
		public void Remove(string host, int port, string authenticationType)
		{
			if (host == null || authenticationType == null)
			{
				return;
			}
			if (port < 0)
			{
				return;
			}
			this.m_version++;
			CredentialHostKey credentialHostKey = new CredentialHostKey(host, port, authenticationType);
			if (this.cacheForHosts[credentialHostKey] is SystemNetworkCredential)
			{
				this.m_NumbDefaultCredInCache--;
			}
			this.cacheForHosts.Remove(credentialHostKey);
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x0006DB74 File Offset: 0x0006CB74
		public NetworkCredential GetCredential(Uri uriPrefix, string authType)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			int num = -1;
			NetworkCredential networkCredential = null;
			IDictionaryEnumerator enumerator = this.cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialKey credentialKey = (CredentialKey)enumerator.Key;
				if (credentialKey.Match(uriPrefix, authType))
				{
					int uriPrefixLength = credentialKey.UriPrefixLength;
					if (uriPrefixLength > num)
					{
						num = uriPrefixLength;
						networkCredential = (NetworkCredential)enumerator.Value;
					}
				}
			}
			return networkCredential;
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x0006DBF0 File Offset: 0x0006CBF0
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (authenticationType == null)
			{
				throw new ArgumentNullException("authenticationType");
			}
			if (host.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "host" }));
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			NetworkCredential networkCredential = null;
			IDictionaryEnumerator enumerator = this.cacheForHosts.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialHostKey credentialHostKey = (CredentialHostKey)enumerator.Key;
				if (credentialHostKey.Match(host, port, authenticationType))
				{
					networkCredential = (NetworkCredential)enumerator.Value;
				}
			}
			return networkCredential;
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x0006DC8C File Offset: 0x0006CC8C
		public IEnumerator GetEnumerator()
		{
			return new CredentialCache.CredentialEnumerator(this, this.cache, this.cacheForHosts, this.m_version);
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x0006DCA6 File Offset: 0x0006CCA6
		public static ICredentials DefaultCredentials
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME").Demand();
				return SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x0006DCBD File Offset: 0x0006CCBD
		public static NetworkCredential DefaultNetworkCredentials
		{
			get
			{
				new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME").Demand();
				return SystemNetworkCredential.defaultCredential;
			}
		}

		// Token: 0x04001D38 RID: 7480
		private Hashtable cache = new Hashtable();

		// Token: 0x04001D39 RID: 7481
		private Hashtable cacheForHosts = new Hashtable();

		// Token: 0x04001D3A RID: 7482
		internal int m_version;

		// Token: 0x04001D3B RID: 7483
		private int m_NumbDefaultCredInCache;

		// Token: 0x0200039C RID: 924
		private class CredentialEnumerator : IEnumerator
		{
			// Token: 0x06001CCD RID: 7373 RVA: 0x0006DCD4 File Offset: 0x0006CCD4
			internal CredentialEnumerator(CredentialCache cache, Hashtable table, Hashtable hostTable, int version)
			{
				this.m_cache = cache;
				this.m_array = new ICredentials[table.Count + hostTable.Count];
				table.Values.CopyTo(this.m_array, 0);
				hostTable.Values.CopyTo(this.m_array, table.Count);
				this.m_version = version;
			}

			// Token: 0x1700059D RID: 1437
			// (get) Token: 0x06001CCE RID: 7374 RVA: 0x0006DD40 File Offset: 0x0006CD40
			object IEnumerator.Current
			{
				get
				{
					if (this.m_index < 0 || this.m_index >= this.m_array.Length)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumOpCantHappen"));
					}
					if (this.m_version != this.m_cache.m_version)
					{
						throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
					}
					return this.m_array[this.m_index];
				}
			}

			// Token: 0x06001CCF RID: 7375 RVA: 0x0006DDA8 File Offset: 0x0006CDA8
			bool IEnumerator.MoveNext()
			{
				if (this.m_version != this.m_cache.m_version)
				{
					throw new InvalidOperationException(SR.GetString("InvalidOperation_EnumFailedVersion"));
				}
				if (++this.m_index < this.m_array.Length)
				{
					return true;
				}
				this.m_index = this.m_array.Length;
				return false;
			}

			// Token: 0x06001CD0 RID: 7376 RVA: 0x0006DE04 File Offset: 0x0006CE04
			void IEnumerator.Reset()
			{
				this.m_index = -1;
			}

			// Token: 0x04001D3C RID: 7484
			private CredentialCache m_cache;

			// Token: 0x04001D3D RID: 7485
			private ICredentials[] m_array;

			// Token: 0x04001D3E RID: 7486
			private int m_index = -1;

			// Token: 0x04001D3F RID: 7487
			private int m_version;
		}
	}
}
