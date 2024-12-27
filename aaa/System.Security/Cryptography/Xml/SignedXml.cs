using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Xml;
using Microsoft.Win32;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A9 RID: 169
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class SignedXml
	{
		// Token: 0x0600037B RID: 891 RVA: 0x00012455 File Offset: 0x00011455
		public SignedXml()
		{
			this.Initialize(null);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00012464 File Offset: 0x00011464
		public SignedXml(XmlDocument document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.Initialize(document.DocumentElement);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00012486 File Offset: 0x00011486
		public SignedXml(XmlElement elem)
		{
			if (elem == null)
			{
				throw new ArgumentNullException("elem");
			}
			this.Initialize(elem);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000124A4 File Offset: 0x000114A4
		private void Initialize(XmlElement element)
		{
			this.m_containingDocument = ((element == null) ? null : element.OwnerDocument);
			this.m_context = element;
			this.m_signature = new Signature();
			this.m_signature.SignedXml = this;
			this.m_signature.SignedInfo = new SignedInfo();
			this.m_signingKey = null;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600037F RID: 895 RVA: 0x000124F8 File Offset: 0x000114F8
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00012500 File Offset: 0x00011500
		public string SigningKeyName
		{
			get
			{
				return this.m_strSigningKeyName;
			}
			set
			{
				this.m_strSigningKeyName = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00012509 File Offset: 0x00011509
		[ComVisible(false)]
		public XmlResolver Resolver
		{
			set
			{
				this.m_xmlResolver = value;
				this.m_bResolverSet = true;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00012519 File Offset: 0x00011519
		internal bool ResolverSet
		{
			get
			{
				return this.m_bResolverSet;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00012521 File Offset: 0x00011521
		// (set) Token: 0x06000384 RID: 900 RVA: 0x00012529 File Offset: 0x00011529
		public AsymmetricAlgorithm SigningKey
		{
			get
			{
				return this.m_signingKey;
			}
			set
			{
				this.m_signingKey = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00012532 File Offset: 0x00011532
		// (set) Token: 0x06000386 RID: 902 RVA: 0x00012553 File Offset: 0x00011553
		[ComVisible(false)]
		public EncryptedXml EncryptedXml
		{
			get
			{
				if (this.m_exml == null)
				{
					this.m_exml = new EncryptedXml(this.m_containingDocument);
				}
				return this.m_exml;
			}
			set
			{
				this.m_exml = value;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0001255C File Offset: 0x0001155C
		public Signature Signature
		{
			get
			{
				return this.m_signature;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00012564 File Offset: 0x00011564
		public SignedInfo SignedInfo
		{
			get
			{
				return this.m_signature.SignedInfo;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00012571 File Offset: 0x00011571
		public string SignatureMethod
		{
			get
			{
				return this.m_signature.SignedInfo.SignatureMethod;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00012583 File Offset: 0x00011583
		public string SignatureLength
		{
			get
			{
				return this.m_signature.SignedInfo.SignatureLength;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00012595 File Offset: 0x00011595
		public byte[] SignatureValue
		{
			get
			{
				return this.m_signature.SignatureValue;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000125A2 File Offset: 0x000115A2
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000125AF File Offset: 0x000115AF
		public KeyInfo KeyInfo
		{
			get
			{
				return this.m_signature.KeyInfo;
			}
			set
			{
				this.m_signature.KeyInfo = value;
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000125BD File Offset: 0x000115BD
		public XmlElement GetXml()
		{
			if (this.m_containingDocument != null)
			{
				return this.m_signature.GetXml(this.m_containingDocument);
			}
			return this.m_signature.GetXml();
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000125E4 File Offset: 0x000115E4
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_signature.LoadXml(value);
			this.m_context = value;
			this.bCacheValid = false;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001260E File Offset: 0x0001160E
		public void AddReference(Reference reference)
		{
			this.m_signature.SignedInfo.AddReference(reference);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00012621 File Offset: 0x00011621
		public void AddObject(DataObject dataObject)
		{
			this.m_signature.AddObject(dataObject);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00012630 File Offset: 0x00011630
		public bool CheckSignature()
		{
			bool flag = false;
			AsymmetricAlgorithm publicKey;
			do
			{
				publicKey = this.GetPublicKey();
				if (publicKey != null)
				{
					flag = this.CheckSignature(publicKey);
				}
			}
			while (publicKey != null && !flag);
			return flag;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00012658 File Offset: 0x00011658
		public bool CheckSignatureReturningKey(out AsymmetricAlgorithm signingKey)
		{
			bool flag = false;
			AsymmetricAlgorithm publicKey;
			do
			{
				publicKey = this.GetPublicKey();
				if (publicKey != null)
				{
					flag = this.CheckSignature(publicKey);
				}
			}
			while (publicKey != null && !flag);
			signingKey = publicKey;
			return flag;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00012685 File Offset: 0x00011685
		public bool CheckSignature(AsymmetricAlgorithm key)
		{
			return SignedXml.DefaultSignatureFormatValidator(this) && this.CheckSignedInfo(key) && this.CheckDigestedReferences();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000126A2 File Offset: 0x000116A2
		public bool CheckSignature(KeyedHashAlgorithm macAlg)
		{
			return SignedXml.DefaultSignatureFormatValidator(this) && this.CheckSignedInfo(macAlg) && this.CheckDigestedReferences();
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000126C0 File Offset: 0x000116C0
		[ComVisible(false)]
		public bool CheckSignature(X509Certificate2 certificate, bool verifySignatureOnly)
		{
			if (!verifySignatureOnly)
			{
				foreach (X509Extension x509Extension in certificate.Extensions)
				{
					if (string.Compare(x509Extension.Oid.Value, "2.5.29.15", StringComparison.OrdinalIgnoreCase) == 0)
					{
						X509KeyUsageExtension x509KeyUsageExtension = new X509KeyUsageExtension();
						x509KeyUsageExtension.CopyFrom(x509Extension);
						if ((x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.DigitalSignature) == X509KeyUsageFlags.None && (x509KeyUsageExtension.KeyUsages & X509KeyUsageFlags.NonRepudiation) == X509KeyUsageFlags.None)
						{
							return false;
						}
						break;
					}
				}
				X509Chain x509Chain = new X509Chain();
				x509Chain.ChainPolicy.ExtraStore.AddRange(this.BuildBagOfCerts());
				if (!x509Chain.Build(certificate))
				{
					return false;
				}
			}
			return SignedXml.DefaultSignatureFormatValidator(this) && this.CheckSignedInfo(certificate.PublicKey.Key) && this.CheckDigestedReferences();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001278C File Offset: 0x0001178C
		public void ComputeSignature()
		{
			this.BuildDigestedReferences();
			AsymmetricAlgorithm signingKey = this.SigningKey;
			if (signingKey == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_LoadKeyFailed"));
			}
			if (this.SignedInfo.SignatureMethod == null)
			{
				if (signingKey is DSA)
				{
					this.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";
				}
				else
				{
					if (!(signingKey is RSA))
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CreatedKeyFailed"));
					}
					if (this.SignedInfo.SignatureMethod == null)
					{
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
					}
				}
			}
			SignatureDescription signatureDescription = this.CreateSignatureDescriptionFromName(this.SignedInfo.SignatureMethod);
			if (signatureDescription == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_SignatureDescriptionNotCreated"));
			}
			HashAlgorithm hashAlgorithm = signatureDescription.CreateDigest();
			if (hashAlgorithm == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CreateHashAlgorithmFailed"));
			}
			this.GetC14NDigest(hashAlgorithm);
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = signatureDescription.CreateFormatter(signingKey);
			this.m_signature.SignatureValue = asymmetricSignatureFormatter.CreateSignature(hashAlgorithm);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00012878 File Offset: 0x00011878
		public void ComputeSignature(KeyedHashAlgorithm macAlg)
		{
			if (macAlg == null)
			{
				throw new ArgumentNullException("macAlg");
			}
			HMAC hmac = macAlg as HMAC;
			if (hmac == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_SignatureMethodKeyMismatch"));
			}
			int num;
			if (this.m_signature.SignedInfo.SignatureLength == null)
			{
				num = hmac.HashSize;
			}
			else
			{
				num = Convert.ToInt32(this.m_signature.SignedInfo.SignatureLength, null);
			}
			if (num < 0 || num > hmac.HashSize)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidSignatureLength"));
			}
			if (num % 8 != 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidSignatureLength2"));
			}
			this.BuildDigestedReferences();
			string hashName;
			if ((hashName = hmac.HashName) != null)
			{
				if (<PrivateImplementationDetails>{6EBA855F-517E-41A8-B681-C465BA41A5A8}.$$method0x6000394-1 == null)
				{
					<PrivateImplementationDetails>{6EBA855F-517E-41A8-B681-C465BA41A5A8}.$$method0x6000394-1 = new Dictionary<string, int>(6)
					{
						{ "SHA1", 0 },
						{ "SHA256", 1 },
						{ "SHA384", 2 },
						{ "SHA512", 3 },
						{ "MD5", 4 },
						{ "RIPEMD160", 5 }
					};
				}
				int num2;
				if (<PrivateImplementationDetails>{6EBA855F-517E-41A8-B681-C465BA41A5A8}.$$method0x6000394-1.TryGetValue(hashName, out num2))
				{
					switch (num2)
					{
					case 0:
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";
						break;
					case 1:
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";
						break;
					case 2:
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384";
						break;
					case 3:
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512";
						break;
					case 4:
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#hmac-md5";
						break;
					case 5:
						this.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#hmac-ripemd160";
						break;
					default:
						goto IL_019E;
					}
					byte[] c14NDigest = this.GetC14NDigest(hmac);
					this.m_signature.SignatureValue = new byte[num / 8];
					Buffer.BlockCopy(c14NDigest, 0, this.m_signature.SignatureValue, 0, num / 8);
					return;
				}
			}
			IL_019E:
			throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_SignatureMethodKeyMismatch"));
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00012A64 File Offset: 0x00011A64
		protected virtual AsymmetricAlgorithm GetPublicKey()
		{
			if (this.KeyInfo == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_KeyInfoRequired"));
			}
			if (this.m_x509Enum != null)
			{
				AsymmetricAlgorithm nextCertificatePublicKey = this.GetNextCertificatePublicKey();
				if (nextCertificatePublicKey != null)
				{
					return nextCertificatePublicKey;
				}
			}
			if (this.m_keyInfoEnum == null)
			{
				this.m_keyInfoEnum = this.KeyInfo.GetEnumerator();
			}
			while (this.m_keyInfoEnum.MoveNext())
			{
				RSAKeyValue rsakeyValue = this.m_keyInfoEnum.Current as RSAKeyValue;
				if (rsakeyValue != null)
				{
					return rsakeyValue.Key;
				}
				DSAKeyValue dsakeyValue = this.m_keyInfoEnum.Current as DSAKeyValue;
				if (dsakeyValue != null)
				{
					return dsakeyValue.Key;
				}
				KeyInfoX509Data keyInfoX509Data = this.m_keyInfoEnum.Current as KeyInfoX509Data;
				if (keyInfoX509Data != null)
				{
					this.m_x509Collection = Utils.BuildBagOfCerts(keyInfoX509Data, CertUsageType.Verification);
					if (this.m_x509Collection.Count > 0)
					{
						this.m_x509Enum = this.m_x509Collection.GetEnumerator();
						AsymmetricAlgorithm nextCertificatePublicKey2 = this.GetNextCertificatePublicKey();
						if (nextCertificatePublicKey2 != null)
						{
							return nextCertificatePublicKey2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00012B54 File Offset: 0x00011B54
		private X509Certificate2Collection BuildBagOfCerts()
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			if (this.KeyInfo != null)
			{
				foreach (object obj in this.KeyInfo)
				{
					KeyInfoClause keyInfoClause = (KeyInfoClause)obj;
					KeyInfoX509Data keyInfoX509Data = keyInfoClause as KeyInfoX509Data;
					if (keyInfoX509Data != null)
					{
						x509Certificate2Collection.AddRange(Utils.BuildBagOfCerts(keyInfoX509Data, CertUsageType.Verification));
					}
				}
			}
			return x509Certificate2Collection;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00012BD0 File Offset: 0x00011BD0
		private AsymmetricAlgorithm GetNextCertificatePublicKey()
		{
			while (this.m_x509Enum.MoveNext())
			{
				X509Certificate2 x509Certificate = (X509Certificate2)this.m_x509Enum.Current;
				if (x509Certificate != null)
				{
					return x509Certificate.PublicKey.Key;
				}
			}
			return null;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00012C0D File Offset: 0x00011C0D
		public virtual XmlElement GetIdElement(XmlDocument document, string idValue)
		{
			return SignedXml.DefaultGetIdElement(document, idValue);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00012C18 File Offset: 0x00011C18
		internal static XmlElement DefaultGetIdElement(XmlDocument document, string idValue)
		{
			if (document == null)
			{
				return null;
			}
			if (Utils.RequireNCNameIdentifier())
			{
				try
				{
					XmlConvert.VerifyNCName(idValue);
				}
				catch (XmlException)
				{
					return null;
				}
			}
			XmlElement xmlElement = document.GetElementById(idValue);
			if (xmlElement != null)
			{
				if (!Utils.AllowAmbiguousReferenceTargets())
				{
					XmlDocument xmlDocument = (XmlDocument)document.CloneNode(true);
					XmlElement elementById = xmlDocument.GetElementById(idValue);
					if (elementById != null)
					{
						elementById.Attributes.RemoveAll();
						XmlElement elementById2 = xmlDocument.GetElementById(idValue);
						if (elementById2 != null)
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidReference"));
						}
					}
				}
				return xmlElement;
			}
			xmlElement = SignedXml.GetSingleReferenceTarget(document, "Id", idValue);
			if (xmlElement != null)
			{
				return xmlElement;
			}
			xmlElement = SignedXml.GetSingleReferenceTarget(document, "id", idValue);
			if (xmlElement != null)
			{
				return xmlElement;
			}
			return SignedXml.GetSingleReferenceTarget(document, "ID", idValue);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00012CDC File Offset: 0x00011CDC
		private byte[] GetC14NDigest(HashAlgorithm hash)
		{
			if (!this.bCacheValid || !this.SignedInfo.CacheValid)
			{
				string text = ((this.m_containingDocument == null) ? null : this.m_containingDocument.BaseURI);
				XmlResolver xmlResolver = (this.m_bResolverSet ? this.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
				XmlDocument xmlDocument = Utils.PreProcessElementInput(this.SignedInfo.GetXml(), xmlResolver, text);
				CanonicalXmlNodeList canonicalXmlNodeList = ((this.m_context == null) ? null : Utils.GetPropagatedAttributes(this.m_context));
				Utils.AddNamespaces(xmlDocument.DocumentElement, canonicalXmlNodeList);
				Transform canonicalizationMethodObject = this.SignedInfo.CanonicalizationMethodObject;
				canonicalizationMethodObject.Resolver = xmlResolver;
				canonicalizationMethodObject.BaseURI = text;
				canonicalizationMethodObject.LoadInput(xmlDocument);
				this._digestedSignedInfo = canonicalizationMethodObject.GetDigestedOutput(hash);
				this.bCacheValid = true;
			}
			return this._digestedSignedInfo;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00012DAC File Offset: 0x00011DAC
		private int GetReferenceLevel(int index, ArrayList references)
		{
			if (this.m_refProcessed[index])
			{
				return this.m_refLevelCache[index];
			}
			this.m_refProcessed[index] = true;
			Reference reference = (Reference)references[index];
			if (reference.Uri == null || reference.Uri.Length == 0 || (reference.Uri.Length > 0 && reference.Uri[0] != '#'))
			{
				this.m_refLevelCache[index] = 0;
				return 0;
			}
			if (reference.Uri.Length <= 0 || reference.Uri[0] != '#')
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidReference"));
			}
			string text = Utils.ExtractIdFromLocalUri(reference.Uri);
			if (text == "xpointer(/)")
			{
				this.m_refLevelCache[index] = 0;
				return 0;
			}
			for (int i = 0; i < references.Count; i++)
			{
				if (((Reference)references[i]).Id == text)
				{
					this.m_refLevelCache[index] = this.GetReferenceLevel(i, references) + 1;
					return this.m_refLevelCache[index];
				}
			}
			this.m_refLevelCache[index] = 0;
			return 0;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00012EC4 File Offset: 0x00011EC4
		private void BuildDigestedReferences()
		{
			ArrayList references = this.SignedInfo.References;
			this.m_refProcessed = new bool[references.Count];
			this.m_refLevelCache = new int[references.Count];
			SignedXml.ReferenceLevelSortOrder referenceLevelSortOrder = new SignedXml.ReferenceLevelSortOrder();
			referenceLevelSortOrder.References = references;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in references)
			{
				Reference reference = (Reference)obj;
				arrayList.Add(reference);
			}
			arrayList.Sort(referenceLevelSortOrder);
			CanonicalXmlNodeList canonicalXmlNodeList = new CanonicalXmlNodeList();
			foreach (object obj2 in this.m_signature.ObjectList)
			{
				DataObject dataObject = (DataObject)obj2;
				canonicalXmlNodeList.Add(dataObject.GetXml());
			}
			foreach (object obj3 in arrayList)
			{
				Reference reference2 = (Reference)obj3;
				if (reference2.DigestMethod == null)
				{
					reference2.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
				}
				reference2.UpdateHashValue(this.m_containingDocument, canonicalXmlNodeList);
				if (reference2.Id != null)
				{
					canonicalXmlNodeList.Add(reference2.GetXml());
				}
			}
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0001304C File Offset: 0x0001204C
		private bool CheckDigestedReferences()
		{
			ArrayList references = this.m_signature.SignedInfo.References;
			int i = 0;
			while (i < references.Count)
			{
				Reference reference = (Reference)references[i];
				if (!this.ReferenceUsesSafeTransformMethods(reference))
				{
					return false;
				}
				byte[] array = null;
				try
				{
					array = reference.CalculateHashValue(this.m_containingDocument, this.m_signature.ReferencedItems);
				}
				catch (CryptoSignedXmlRecursionException)
				{
					return false;
				}
				if (!SignedXml.CryptographicEquals(array, reference.DigestValue))
				{
					return false;
				}
				i++;
				continue;
			}
			return true;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000130DC File Offset: 0x000120DC
		private static bool CryptographicEquals(byte[] a, byte[] b)
		{
			int num = 0;
			if (a.Length != b.Length)
			{
				return false;
			}
			int num2 = a.Length;
			for (int i = 0; i < num2; i++)
			{
				num |= (int)(a[i] - b[i]);
			}
			return 0 == num;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00013114 File Offset: 0x00012114
		private bool CheckSignedInfo(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			SignatureDescription signatureDescription = this.CreateSignatureDescriptionFromName(this.SignatureMethod);
			if (signatureDescription == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_SignatureDescriptionNotCreated"));
			}
			Type type = Type.GetType(signatureDescription.KeyAlgorithm);
			Type type2 = key.GetType();
			if (type != type2 && !type.IsSubclassOf(type2) && !type2.IsSubclassOf(type))
			{
				return false;
			}
			HashAlgorithm hashAlgorithm = signatureDescription.CreateDigest();
			if (hashAlgorithm == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CreateHashAlgorithmFailed"));
			}
			byte[] c14NDigest = this.GetC14NDigest(hashAlgorithm);
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = signatureDescription.CreateDeformatter(key);
			return asymmetricSignatureDeformatter.VerifySignature(c14NDigest, this.m_signature.SignatureValue);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x000131BC File Offset: 0x000121BC
		private bool CheckSignedInfo(KeyedHashAlgorithm macAlg)
		{
			if (macAlg == null)
			{
				throw new ArgumentNullException("macAlg");
			}
			int num;
			if (this.m_signature.SignedInfo.SignatureLength == null)
			{
				num = macAlg.HashSize;
			}
			else
			{
				num = Convert.ToInt32(this.m_signature.SignedInfo.SignatureLength, null);
			}
			if (num < 0 || num > macAlg.HashSize)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidSignatureLength"));
			}
			if (num % 8 != 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidSignatureLength2"));
			}
			if (this.m_signature.SignatureValue == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_SignatureValueRequired"));
			}
			if (this.m_signature.SignatureValue.Length != num / 8)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidSignatureLength"));
			}
			byte[] c14NDigest = this.GetC14NDigest(macAlg);
			for (int i = 0; i < this.m_signature.SignatureValue.Length; i++)
			{
				if (this.m_signature.SignatureValue[i] != c14NDigest[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000132AF File Offset: 0x000122AF
		private static bool AllowHmacTruncation
		{
			get
			{
				if (SignedXml.s_allowHmacTruncation == null)
				{
					SignedXml.s_allowHmacTruncation = new bool?(SignedXml.ReadHmacTruncationSetting());
				}
				return SignedXml.s_allowHmacTruncation.Value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x000132D8 File Offset: 0x000122D8
		private static IList<string> SafeCanonicalizationMethods
		{
			get
			{
				if (SignedXml.s_safeCanonicalizationMethods == null)
				{
					List<string> list = SignedXml.ReadAdditionalSafeCanonicalizationMethods();
					list.Add("http://www.w3.org/TR/2001/REC-xml-c14n-20010315");
					list.Add("http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments");
					list.Add("http://www.w3.org/2001/10/xml-exc-c14n#");
					list.Add("http://www.w3.org/2001/10/xml-exc-c14n#WithComments");
					SignedXml.s_safeCanonicalizationMethods = list;
				}
				return SignedXml.s_safeCanonicalizationMethods;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0001332C File Offset: 0x0001232C
		private static IList<string> DefaultSafeTransformMethods
		{
			get
			{
				if (SignedXml.s_defaultSafeTransformMethods == null)
				{
					List<string> list = SignedXml.ReadAdditionalSafeTransformMethods();
					list.Add("http://www.w3.org/2000/09/xmldsig#enveloped-signature");
					list.Add("http://www.w3.org/2000/09/xmldsig#base64");
					list.Add("urn:mpeg:mpeg21:2003:01-REL-R-NS:licenseTransform");
					list.Add("http://www.w3.org/2002/07/decrypt#XML");
					SignedXml.s_defaultSafeTransformMethods = list;
				}
				return SignedXml.s_defaultSafeTransformMethods;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001337D File Offset: 0x0001237D
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static List<string> ReadAdditionalSafeCanonicalizationMethods()
		{
			return SignedXml.ReadFxSecurityStringValues("SafeCanonicalizationMethods");
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00013389 File Offset: 0x00012389
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static List<string> ReadAdditionalSafeTransformMethods()
		{
			return SignedXml.ReadFxSecurityStringValues("SafeTransformMethods");
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00013398 File Offset: 0x00012398
		private static List<string> ReadFxSecurityStringValues(string subkey)
		{
			List<string> list = new List<string>();
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\Security\\" + subkey, false))
				{
					if (registryKey != null)
					{
						foreach (string text in registryKey.GetValueNames())
						{
							if (registryKey.GetValueKind(text) == RegistryValueKind.String)
							{
								string text2 = registryKey.GetValue(text) as string;
								if (!string.IsNullOrEmpty(text2))
								{
									list.Add(text2);
								}
							}
						}
					}
				}
			}
			catch (SecurityException)
			{
			}
			return list;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001343C File Offset: 0x0001243C
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool ReadHmacTruncationSetting()
		{
			bool flag;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework", false))
				{
					if (registryKey == null)
					{
						flag = false;
					}
					else
					{
						object value = registryKey.GetValue("AllowHMACTruncation");
						if (value == null)
						{
							flag = false;
						}
						else if (registryKey.GetValueKind("AllowHMACTruncation") != RegistryValueKind.DWord)
						{
							flag = false;
						}
						else
						{
							flag = (int)value != 0;
						}
					}
				}
			}
			catch (SecurityException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000134C4 File Offset: 0x000124C4
		private static bool DefaultSignatureFormatValidator(SignedXml signedXml)
		{
			return (SignedXml.AllowHmacTruncation || !signedXml.DoesSignatureUseTruncatedHmac()) && signedXml.DoesSignatureUseSafeCanonicalizationMethod();
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000134E4 File Offset: 0x000124E4
		private bool DoesSignatureUseSafeCanonicalizationMethod()
		{
			foreach (string text in SignedXml.SafeCanonicalizationMethods)
			{
				if (string.Equals(text, this.SignedInfo.CanonicalizationMethod, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00013544 File Offset: 0x00012544
		private bool ReferenceUsesSafeTransformMethods(Reference reference)
		{
			TransformChain transformChain = reference.TransformChain;
			int count = transformChain.Count;
			for (int i = 0; i < count; i++)
			{
				Transform transform = transformChain[i];
				if (!this.IsSafeTransform(transform.Algorithm))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00013584 File Offset: 0x00012584
		private bool IsSafeTransform(string transformAlgorithm)
		{
			foreach (string text in SignedXml.SafeCanonicalizationMethods)
			{
				if (string.Equals(text, transformAlgorithm, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			foreach (string text2 in SignedXml.DefaultSafeTransformMethods)
			{
				if (string.Equals(text2, transformAlgorithm, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013624 File Offset: 0x00012624
		private bool DoesSignatureUseTruncatedHmac()
		{
			if (this.SignedInfo == null || this.SignedInfo.SignatureLength == null)
			{
				return false;
			}
			HMAC hmac = Utils.CreateFromName<HMAC>(this.SignatureMethod);
			if (hmac == null)
			{
				if (string.Equals(this.SignatureMethod, "http://www.w3.org/2000/09/xmldsig#hmac-sha1", StringComparison.Ordinal))
				{
					hmac = new HMACSHA1();
				}
				else if (string.Equals(this.SignatureMethod, "http://www.w3.org/2001/04/xmldsig-more#hmac-md5", StringComparison.Ordinal))
				{
					hmac = new HMACMD5();
				}
			}
			if (hmac == null)
			{
				return false;
			}
			int num = 0;
			if (!int.TryParse(this.SignedInfo.SignatureLength, NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
			{
				return true;
			}
			int num2 = Math.Max(80, hmac.HashSize / 2);
			return num < num2;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000136C4 File Offset: 0x000126C4
		private static XmlElement GetSingleReferenceTarget(XmlDocument document, string idAttributeName, string idValue)
		{
			string text = string.Concat(new string[] { "//*[@", idAttributeName, "=\"", idValue, "\"]" });
			if (Utils.AllowAmbiguousReferenceTargets())
			{
				return document.SelectSingleNode(text) as XmlElement;
			}
			XmlNodeList xmlNodeList = document.SelectNodes(text);
			if (xmlNodeList == null || xmlNodeList.Count == 0)
			{
				return null;
			}
			if (xmlNodeList.Count == 1)
			{
				return xmlNodeList[0] as XmlElement;
			}
			throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidReference"));
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00013750 File Offset: 0x00012750
		private SignatureDescription CreateSignatureDescriptionFromName(string name)
		{
			SignatureDescription signatureDescription = Utils.CreateFromName<SignatureDescription>(name);
			if (signatureDescription != null)
			{
				return signatureDescription;
			}
			StringComparison stringComparison = StringComparison.OrdinalIgnoreCase;
			if (name.Equals("http://www.w3.org/2001/04/xmldsig-more#rsa-sha256", stringComparison))
			{
				return new RSAPKCS1SHA256SignatureDescription();
			}
			if (name.Equals("http://www.w3.org/2001/04/xmldsig-more#rsa-sha384", stringComparison))
			{
				return new RSAPKCS1SHA384SignatureDescription();
			}
			if (name.Equals("http://www.w3.org/2001/04/xmldsig-more#rsa-sha512", stringComparison))
			{
				return new RSAPKCS1SHA512SignatureDescription();
			}
			return null;
		}

		// Token: 0x04000524 RID: 1316
		private const string XmlDsigMoreHMACMD5Url = "http://www.w3.org/2001/04/xmldsig-more#hmac-md5";

		// Token: 0x04000525 RID: 1317
		private const string XmlDsigMoreHMACSHA256Url = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

		// Token: 0x04000526 RID: 1318
		private const string XmlDsigMoreHMACSHA384Url = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha384";

		// Token: 0x04000527 RID: 1319
		private const string XmlDsigMoreHMACSHA512Url = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha512";

		// Token: 0x04000528 RID: 1320
		private const string XmlDsigMoreHMACRIPEMD160Url = "http://www.w3.org/2001/04/xmldsig-more#hmac-ripemd160";

		// Token: 0x04000529 RID: 1321
		public const string XmlDsigNamespaceUrl = "http://www.w3.org/2000/09/xmldsig#";

		// Token: 0x0400052A RID: 1322
		public const string XmlDsigMinimalCanonicalizationUrl = "http://www.w3.org/2000/09/xmldsig#minimal";

		// Token: 0x0400052B RID: 1323
		public const string XmlDsigCanonicalizationUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";

		// Token: 0x0400052C RID: 1324
		public const string XmlDsigCanonicalizationWithCommentsUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";

		// Token: 0x0400052D RID: 1325
		public const string XmlDsigSHA1Url = "http://www.w3.org/2000/09/xmldsig#sha1";

		// Token: 0x0400052E RID: 1326
		public const string XmlDsigDSAUrl = "http://www.w3.org/2000/09/xmldsig#dsa-sha1";

		// Token: 0x0400052F RID: 1327
		public const string XmlDsigRSASHA1Url = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";

		// Token: 0x04000530 RID: 1328
		internal const string XmlDsigSHA256Url = "http://www.w3.org/2001/04/xmlenc#sha256";

		// Token: 0x04000531 RID: 1329
		internal const string XmlDsigRSASHA256Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

		// Token: 0x04000532 RID: 1330
		internal const string XmlDsigSHA384Url = "http://www.w3.org/2001/04/xmldsig-more#sha384";

		// Token: 0x04000533 RID: 1331
		internal const string XmlDsigRSASHA384Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha384";

		// Token: 0x04000534 RID: 1332
		internal const string XmlDsigSHA512Url = "http://www.w3.org/2001/04/xmlenc#sha512";

		// Token: 0x04000535 RID: 1333
		internal const string XmlDsigRSASHA512Url = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";

		// Token: 0x04000536 RID: 1334
		public const string XmlDsigHMACSHA1Url = "http://www.w3.org/2000/09/xmldsig#hmac-sha1";

		// Token: 0x04000537 RID: 1335
		public const string XmlDsigC14NTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";

		// Token: 0x04000538 RID: 1336
		public const string XmlDsigC14NWithCommentsTransformUrl = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments";

		// Token: 0x04000539 RID: 1337
		public const string XmlDsigExcC14NTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#";

		// Token: 0x0400053A RID: 1338
		public const string XmlDsigExcC14NWithCommentsTransformUrl = "http://www.w3.org/2001/10/xml-exc-c14n#WithComments";

		// Token: 0x0400053B RID: 1339
		public const string XmlDsigBase64TransformUrl = "http://www.w3.org/2000/09/xmldsig#base64";

		// Token: 0x0400053C RID: 1340
		public const string XmlDsigXPathTransformUrl = "http://www.w3.org/TR/1999/REC-xpath-19991116";

		// Token: 0x0400053D RID: 1341
		public const string XmlDsigXsltTransformUrl = "http://www.w3.org/TR/1999/REC-xslt-19991116";

		// Token: 0x0400053E RID: 1342
		public const string XmlDsigEnvelopedSignatureTransformUrl = "http://www.w3.org/2000/09/xmldsig#enveloped-signature";

		// Token: 0x0400053F RID: 1343
		public const string XmlDecryptionTransformUrl = "http://www.w3.org/2002/07/decrypt#XML";

		// Token: 0x04000540 RID: 1344
		public const string XmlLicenseTransformUrl = "urn:mpeg:mpeg21:2003:01-REL-R-NS:licenseTransform";

		// Token: 0x04000541 RID: 1345
		private const string AllowHMACTruncationValue = "AllowHMACTruncation";

		// Token: 0x04000542 RID: 1346
		protected Signature m_signature;

		// Token: 0x04000543 RID: 1347
		protected string m_strSigningKeyName;

		// Token: 0x04000544 RID: 1348
		private AsymmetricAlgorithm m_signingKey;

		// Token: 0x04000545 RID: 1349
		private XmlDocument m_containingDocument;

		// Token: 0x04000546 RID: 1350
		private IEnumerator m_keyInfoEnum;

		// Token: 0x04000547 RID: 1351
		private X509Certificate2Collection m_x509Collection;

		// Token: 0x04000548 RID: 1352
		private IEnumerator m_x509Enum;

		// Token: 0x04000549 RID: 1353
		private bool[] m_refProcessed;

		// Token: 0x0400054A RID: 1354
		private int[] m_refLevelCache;

		// Token: 0x0400054B RID: 1355
		internal XmlResolver m_xmlResolver;

		// Token: 0x0400054C RID: 1356
		internal XmlElement m_context;

		// Token: 0x0400054D RID: 1357
		private bool m_bResolverSet;

		// Token: 0x0400054E RID: 1358
		private EncryptedXml m_exml;

		// Token: 0x0400054F RID: 1359
		private static bool? s_allowHmacTruncation;

		// Token: 0x04000550 RID: 1360
		private static List<string> s_safeCanonicalizationMethods;

		// Token: 0x04000551 RID: 1361
		private static List<string> s_defaultSafeTransformMethods;

		// Token: 0x04000552 RID: 1362
		private bool bCacheValid;

		// Token: 0x04000553 RID: 1363
		private byte[] _digestedSignedInfo;

		// Token: 0x020000AA RID: 170
		private class ReferenceLevelSortOrder : IComparer
		{
			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x060003B4 RID: 948 RVA: 0x000137B0 File Offset: 0x000127B0
			// (set) Token: 0x060003B5 RID: 949 RVA: 0x000137B8 File Offset: 0x000127B8
			public ArrayList References
			{
				get
				{
					return this.m_references;
				}
				set
				{
					this.m_references = value;
				}
			}

			// Token: 0x060003B6 RID: 950 RVA: 0x000137C4 File Offset: 0x000127C4
			public int Compare(object a, object b)
			{
				Reference reference = a as Reference;
				Reference reference2 = b as Reference;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				foreach (object obj in this.References)
				{
					Reference reference3 = (Reference)obj;
					if (reference3 == reference)
					{
						num = num3;
					}
					if (reference3 == reference2)
					{
						num2 = num3;
					}
					num3++;
				}
				int referenceLevel = reference.SignedXml.GetReferenceLevel(num, this.References);
				int referenceLevel2 = reference2.SignedXml.GetReferenceLevel(num2, this.References);
				return referenceLevel.CompareTo(referenceLevel2);
			}

			// Token: 0x04000554 RID: 1364
			private ArrayList m_references;
		}
	}
}
