using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x020004AC RID: 1196
	[ComVisible(true)]
	[Serializable]
	public sealed class Publisher : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x000A70D0 File Offset: 0x000A60D0
		internal Publisher()
		{
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000A70D8 File Offset: 0x000A60D8
		public Publisher(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			this.m_cert = cert;
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000A70F5 File Offset: 0x000A60F5
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new PublisherIdentityPermission(this.m_cert);
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000A7104 File Offset: 0x000A6104
		public override bool Equals(object o)
		{
			Publisher publisher = o as Publisher;
			return publisher != null && Publisher.PublicKeyEquals(this.m_cert, publisher.m_cert);
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000A7130 File Offset: 0x000A6130
		internal static bool PublicKeyEquals(X509Certificate cert1, X509Certificate cert2)
		{
			if (cert1 == null)
			{
				return cert2 == null;
			}
			if (cert2 == null)
			{
				return false;
			}
			byte[] publicKey = cert1.GetPublicKey();
			string keyAlgorithm = cert1.GetKeyAlgorithm();
			byte[] keyAlgorithmParameters = cert1.GetKeyAlgorithmParameters();
			byte[] publicKey2 = cert2.GetPublicKey();
			string keyAlgorithm2 = cert2.GetKeyAlgorithm();
			byte[] keyAlgorithmParameters2 = cert2.GetKeyAlgorithmParameters();
			int num = publicKey.Length;
			if (num != publicKey2.Length)
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (publicKey[i] != publicKey2[i])
				{
					return false;
				}
			}
			if (!keyAlgorithm.Equals(keyAlgorithm2))
			{
				return false;
			}
			num = keyAlgorithmParameters.Length;
			if (keyAlgorithmParameters2.Length != num)
			{
				return false;
			}
			for (int j = 0; j < num; j++)
			{
				if (keyAlgorithmParameters[j] != keyAlgorithmParameters2[j])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000A71DB File Offset: 0x000A61DB
		public override int GetHashCode()
		{
			return this.m_cert.GetHashCode();
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x000A71E8 File Offset: 0x000A61E8
		public X509Certificate Certificate
		{
			get
			{
				if (this.m_cert == null)
				{
					return null;
				}
				return new X509Certificate(this.m_cert);
			}
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000A7200 File Offset: 0x000A6200
		public object Copy()
		{
			Publisher publisher = new Publisher();
			if (this.m_cert != null)
			{
				publisher.m_cert = new X509Certificate(this.m_cert);
			}
			return publisher;
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x000A7230 File Offset: 0x000A6230
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Publisher");
			securityElement.AddAttribute("version", "1");
			securityElement.AddChild(new SecurityElement("X509v3Certificate", (this.m_cert != null) ? this.m_cert.GetRawCertDataString() : ""));
			return securityElement;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000A7284 File Offset: 0x000A6284
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\u0001';
			byte[] rawCertData = this.Certificate.GetRawCertData();
			int num = rawCertData.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(num, buffer, position);
				position += 2;
			}
			Buffer.InternalBlockCopy(rawCertData, 0, buffer, position * 2, num);
			return (num - 1) / 2 + 1 + position;
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000A72D0 File Offset: 0x000A62D0
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			int num = (this.Certificate.GetRawCertData().Length - 1) / 2 + 1;
			if (verbose)
			{
				return num + 3;
			}
			return num + 1;
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000A72FC File Offset: 0x000A62FC
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			byte[] array = new byte[intFromCharArray];
			int num = (intFromCharArray - 1) / 2 + 1;
			Buffer.InternalBlockCopy(buffer, position * 2, array, 0, intFromCharArray);
			this.m_cert = new X509Certificate(array);
			return position + num;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000A7340 File Offset: 0x000A6340
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000A7350 File Offset: 0x000A6350
		internal object Normalize()
		{
			return new MemoryStream(this.m_cert.GetRawCertData())
			{
				Position = 0L
			};
		}

		// Token: 0x04001829 RID: 6185
		private X509Certificate m_cert;
	}
}
