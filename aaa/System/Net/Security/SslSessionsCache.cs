using System;
using System.Collections;

namespace System.Net.Security
{
	// Token: 0x0200053D RID: 1341
	internal static class SslSessionsCache
	{
		// Token: 0x060028F7 RID: 10487 RVA: 0x000AA444 File Offset: 0x000A9444
		internal static SafeFreeCredentials TryCachedCredential(byte[] thumbPrint, SchProtocols allowedProtocols)
		{
			if (SslSessionsCache.s_CachedCreds.Count == 0)
			{
				return null;
			}
			object obj = new SslSessionsCache.SslCredKey(thumbPrint, allowedProtocols);
			SafeCredentialReference safeCredentialReference = SslSessionsCache.s_CachedCreds[obj] as SafeCredentialReference;
			if (safeCredentialReference == null || safeCredentialReference.IsClosed || safeCredentialReference._Target.IsInvalid)
			{
				return null;
			}
			return safeCredentialReference._Target;
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000AA4A0 File Offset: 0x000A94A0
		internal static void CacheCredential(SafeFreeCredentials creds, byte[] thumbPrint, SchProtocols allowedProtocols)
		{
			if (creds.IsInvalid)
			{
				return;
			}
			object obj = new SslSessionsCache.SslCredKey(thumbPrint, allowedProtocols);
			SafeCredentialReference safeCredentialReference = SslSessionsCache.s_CachedCreds[obj] as SafeCredentialReference;
			if (safeCredentialReference == null || safeCredentialReference.IsClosed || safeCredentialReference._Target.IsInvalid)
			{
				lock (SslSessionsCache.s_CachedCreds)
				{
					safeCredentialReference = SslSessionsCache.s_CachedCreds[obj] as SafeCredentialReference;
					if (safeCredentialReference == null || safeCredentialReference.IsClosed)
					{
						safeCredentialReference = SafeCredentialReference.CreateReference(creds);
						if (safeCredentialReference != null)
						{
							SslSessionsCache.s_CachedCreds[obj] = safeCredentialReference;
							if (SslSessionsCache.s_CachedCreds.Count % 32 == 0)
							{
								DictionaryEntry[] array = new DictionaryEntry[SslSessionsCache.s_CachedCreds.Count];
								SslSessionsCache.s_CachedCreds.CopyTo(array, 0);
								for (int i = 0; i < array.Length; i++)
								{
									safeCredentialReference = array[i].Value as SafeCredentialReference;
									if (safeCredentialReference != null)
									{
										creds = safeCredentialReference._Target;
										safeCredentialReference.Close();
										if (!creds.IsClosed && !creds.IsInvalid && (safeCredentialReference = SafeCredentialReference.CreateReference(creds)) != null)
										{
											SslSessionsCache.s_CachedCreds[array[i].Key] = safeCredentialReference;
										}
										else
										{
											SslSessionsCache.s_CachedCreds.Remove(array[i].Key);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x040027CA RID: 10186
		private const int c_CheckExpiredModulo = 32;

		// Token: 0x040027CB RID: 10187
		private static Hashtable s_CachedCreds = new Hashtable(32);

		// Token: 0x0200053E RID: 1342
		private struct SslCredKey
		{
			// Token: 0x060028FA RID: 10490 RVA: 0x000AA608 File Offset: 0x000A9608
			internal SslCredKey(byte[] thumbPrint, SchProtocols allowedProtocols)
			{
				this._CertThumbPrint = ((thumbPrint == null) ? SslSessionsCache.SslCredKey.s_EmptyArray : thumbPrint);
				this._HashCode = 0;
				if (thumbPrint != null)
				{
					this._HashCode ^= (int)this._CertThumbPrint[0];
					if (1 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[1] << 8;
					}
					if (2 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[2] << 16;
					}
					if (3 < this._CertThumbPrint.Length)
					{
						this._HashCode ^= (int)this._CertThumbPrint[3] << 24;
					}
				}
				this._AllowedProtocols = allowedProtocols;
				this._HashCode ^= (int)this._AllowedProtocols;
			}

			// Token: 0x060028FB RID: 10491 RVA: 0x000AA6C7 File Offset: 0x000A96C7
			public override int GetHashCode()
			{
				return this._HashCode;
			}

			// Token: 0x060028FC RID: 10492 RVA: 0x000AA6CF File Offset: 0x000A96CF
			public static bool operator ==(SslSessionsCache.SslCredKey sslCredKey1, SslSessionsCache.SslCredKey sslCredKey2)
			{
				return sslCredKey1 == sslCredKey2 || (sslCredKey1 != null && sslCredKey2 != null && sslCredKey1.Equals(sslCredKey2));
			}

			// Token: 0x060028FD RID: 10493 RVA: 0x000AA706 File Offset: 0x000A9706
			public static bool operator !=(SslSessionsCache.SslCredKey sslCredKey1, SslSessionsCache.SslCredKey sslCredKey2)
			{
				return sslCredKey1 != sslCredKey2 && (sslCredKey1 == null || sslCredKey2 == null || !sslCredKey1.Equals(sslCredKey2));
			}

			// Token: 0x060028FE RID: 10494 RVA: 0x000AA740 File Offset: 0x000A9740
			public override bool Equals(object y)
			{
				SslSessionsCache.SslCredKey sslCredKey = (SslSessionsCache.SslCredKey)y;
				if (this._CertThumbPrint.Length != sslCredKey._CertThumbPrint.Length)
				{
					return false;
				}
				if (this._HashCode != sslCredKey._HashCode)
				{
					return false;
				}
				for (int i = 0; i < this._CertThumbPrint.Length; i++)
				{
					if (this._CertThumbPrint[i] != sslCredKey._CertThumbPrint[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x040027CC RID: 10188
			private static readonly byte[] s_EmptyArray = new byte[0];

			// Token: 0x040027CD RID: 10189
			private byte[] _CertThumbPrint;

			// Token: 0x040027CE RID: 10190
			private SchProtocols _AllowedProtocols;

			// Token: 0x040027CF RID: 10191
			private int _HashCode;
		}
	}
}
