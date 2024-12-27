using System;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Permissions;
using System.Xml;
using Microsoft.Win32;

namespace System.Configuration
{
	// Token: 0x02000091 RID: 145
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	public sealed class RsaProtectedConfigurationProvider : ProtectedConfigurationProvider
	{
		// Token: 0x06000541 RID: 1345 RVA: 0x0001A098 File Offset: 0x00019098
		public override XmlNode Decrypt(XmlNode encryptedNode)
		{
			XmlDocument xmlDocument = new XmlDocument();
			RSACryptoServiceProvider cryptoServiceProvider = this.GetCryptoServiceProvider(false, true);
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(encryptedNode.OuterXml);
			EncryptedXml encryptedXml = new EncryptedXml(xmlDocument);
			encryptedXml.AddKeyNameMapping(this._KeyName, cryptoServiceProvider);
			encryptedXml.DecryptDocument();
			cryptoServiceProvider.Clear();
			return xmlDocument.DocumentElement;
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001A0F0 File Offset: 0x000190F0
		public override XmlNode Encrypt(XmlNode node)
		{
			RSACryptoServiceProvider cryptoServiceProvider = this.GetCryptoServiceProvider(false, false);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml("<foo>" + node.OuterXml + "</foo>");
			EncryptedXml encryptedXml = new EncryptedXml(xmlDocument);
			XmlElement documentElement = xmlDocument.DocumentElement;
			SymmetricAlgorithm symmetricAlgorithm = new TripleDESCryptoServiceProvider();
			byte[] randomKey = this.GetRandomKey();
			symmetricAlgorithm.Key = randomKey;
			symmetricAlgorithm.Mode = CipherMode.ECB;
			symmetricAlgorithm.Padding = PaddingMode.PKCS7;
			byte[] array = encryptedXml.EncryptData(documentElement, symmetricAlgorithm, true);
			EncryptedData encryptedData = new EncryptedData();
			encryptedData.Type = "http://www.w3.org/2001/04/xmlenc#Element";
			encryptedData.EncryptionMethod = new EncryptionMethod("http://www.w3.org/2001/04/xmlenc#tripledes-cbc");
			encryptedData.KeyInfo = new KeyInfo();
			EncryptedKey encryptedKey = new EncryptedKey();
			encryptedKey.EncryptionMethod = new EncryptionMethod("http://www.w3.org/2001/04/xmlenc#rsa-1_5");
			encryptedKey.KeyInfo = new KeyInfo();
			encryptedKey.CipherData = new CipherData();
			encryptedKey.CipherData.CipherValue = EncryptedXml.EncryptKey(symmetricAlgorithm.Key, cryptoServiceProvider, this.UseOAEP);
			KeyInfoName keyInfoName = new KeyInfoName();
			keyInfoName.Value = this._KeyName;
			encryptedKey.KeyInfo.AddClause(keyInfoName);
			KeyInfoEncryptedKey keyInfoEncryptedKey = new KeyInfoEncryptedKey(encryptedKey);
			encryptedData.KeyInfo.AddClause(keyInfoEncryptedKey);
			encryptedData.CipherData = new CipherData();
			encryptedData.CipherData.CipherValue = array;
			EncryptedXml.ReplaceElement(documentElement, encryptedData, true);
			foreach (object obj in xmlDocument.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.NodeType == XmlNodeType.Element)
						{
							return xmlNode2;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001A2FC File Offset: 0x000192FC
		public void AddKey(int keySize, bool exportable)
		{
			RSACryptoServiceProvider cryptoServiceProvider = this.GetCryptoServiceProvider(exportable, false);
			cryptoServiceProvider.KeySize = keySize;
			cryptoServiceProvider.PersistKeyInCsp = true;
			cryptoServiceProvider.Clear();
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001A328 File Offset: 0x00019328
		public void DeleteKey()
		{
			RSACryptoServiceProvider cryptoServiceProvider = this.GetCryptoServiceProvider(false, true);
			cryptoServiceProvider.PersistKeyInCsp = false;
			cryptoServiceProvider.Clear();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001A34C File Offset: 0x0001934C
		public void ImportKey(string xmlFileName, bool exportable)
		{
			RSACryptoServiceProvider cryptoServiceProvider = this.GetCryptoServiceProvider(exportable, false);
			cryptoServiceProvider.FromXmlString(File.ReadAllText(xmlFileName));
			cryptoServiceProvider.PersistKeyInCsp = true;
			cryptoServiceProvider.Clear();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001A37C File Offset: 0x0001937C
		public void ExportKey(string xmlFileName, bool includePrivateParameters)
		{
			RSACryptoServiceProvider cryptoServiceProvider = this.GetCryptoServiceProvider(false, false);
			string text = cryptoServiceProvider.ToXmlString(includePrivateParameters);
			File.WriteAllText(xmlFileName, text);
			cryptoServiceProvider.Clear();
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x0001A3A7 File Offset: 0x000193A7
		public string KeyContainerName
		{
			get
			{
				return this._KeyContainerName;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001A3AF File Offset: 0x000193AF
		public string CspProviderName
		{
			get
			{
				return this._CspProviderName;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x0001A3B7 File Offset: 0x000193B7
		public bool UseMachineContainer
		{
			get
			{
				return this._UseMachineContainer;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001A3BF File Offset: 0x000193BF
		public bool UseOAEP
		{
			get
			{
				return this._UseOAEP;
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001A3C8 File Offset: 0x000193C8
		public override void Initialize(string name, NameValueCollection configurationValues)
		{
			base.Initialize(name, configurationValues);
			this._KeyName = "Rsa Key";
			this._KeyContainerName = configurationValues["keyContainerName"];
			configurationValues.Remove("keyContainerName");
			if (this._KeyContainerName == null || this._KeyContainerName.Length < 1)
			{
				this._KeyContainerName = "NetFrameworkConfigurationKey";
			}
			this._CspProviderName = configurationValues["cspProviderName"];
			configurationValues.Remove("cspProviderName");
			this._UseMachineContainer = RsaProtectedConfigurationProvider.GetBooleanValue(configurationValues, "useMachineContainer", true);
			this._UseOAEP = RsaProtectedConfigurationProvider.GetBooleanValue(configurationValues, "useOAEP", false);
			if (configurationValues.Count > 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Unrecognized_initialization_value", new object[] { configurationValues.GetKey(0) }));
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x0001A490 File Offset: 0x00019490
		public RSAParameters RsaPublicKey
		{
			get
			{
				return this.GetCryptoServiceProvider(false, false).ExportParameters(false);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001A4A0 File Offset: 0x000194A0
		private RSACryptoServiceProvider GetCryptoServiceProvider(bool exportable, bool keyMustExist)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider;
			try
			{
				CspParameters cspParameters = new CspParameters();
				cspParameters.KeyContainerName = this.KeyContainerName;
				cspParameters.KeyNumber = 1;
				cspParameters.ProviderType = 1;
				if (this.CspProviderName != null && this.CspProviderName.Length > 0)
				{
					cspParameters.ProviderName = this.CspProviderName;
				}
				if (this.UseMachineContainer)
				{
					cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
				}
				if (!exportable && !keyMustExist)
				{
					cspParameters.Flags |= CspProviderFlags.UseNonExportableKey;
				}
				if (keyMustExist)
				{
					cspParameters.Flags |= CspProviderFlags.UseExistingKey;
				}
				rsacryptoServiceProvider = new RSACryptoServiceProvider(cspParameters);
			}
			catch
			{
				this.ThrowBetterException(keyMustExist);
				throw;
			}
			return rsacryptoServiceProvider;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001A550 File Offset: 0x00019550
		private byte[] GetRandomKey()
		{
			byte[] array = new byte[24];
			new RNGCryptoServiceProvider().GetBytes(array);
			return array;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001A574 File Offset: 0x00019574
		private void ThrowBetterException(bool keyMustExist)
		{
			SafeCryptContextHandle safeCryptContextHandle = null;
			try
			{
				int num = UnsafeNativeMethods.CryptAcquireContext(out safeCryptContextHandle, this.KeyContainerName, this.CspProviderName, 1U, this.UseMachineContainer ? 32U : 0U);
				if (num == 0)
				{
					int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
					if (hrforLastWin32Error != -2146893802 || keyMustExist)
					{
						int num2 = hrforLastWin32Error;
						switch (num2)
						{
						case -2147024891:
						case -2147024890:
							break;
						default:
							if (num2 != -2146893802)
							{
								Marshal.ThrowExceptionForHR(hrforLastWin32Error);
								return;
							}
							break;
						}
						throw new ConfigurationErrorsException(SR.GetString("Key_container_doesnt_exist_or_access_denied"));
					}
				}
			}
			finally
			{
				if (safeCryptContextHandle != null && !safeCryptContextHandle.IsInvalid)
				{
					safeCryptContextHandle.Dispose();
				}
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001A618 File Offset: 0x00019618
		private static bool GetBooleanValue(NameValueCollection configurationValues, string valueName, bool defaultValue)
		{
			string text = configurationValues[valueName];
			if (text == null)
			{
				return defaultValue;
			}
			configurationValues.Remove(valueName);
			if (text == "true")
			{
				return true;
			}
			if (text == "false")
			{
				return false;
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_invalid_boolean_attribute", new object[] { valueName }));
		}

		// Token: 0x0400037C RID: 892
		private const string DefaultRsaKeyContainerName = "NetFrameworkConfigurationKey";

		// Token: 0x0400037D RID: 893
		private const uint PROV_Rsa_FULL = 1U;

		// Token: 0x0400037E RID: 894
		private const uint CRYPT_MACHINE_KEYSET = 32U;

		// Token: 0x0400037F RID: 895
		private string _KeyName;

		// Token: 0x04000380 RID: 896
		private string _KeyContainerName;

		// Token: 0x04000381 RID: 897
		private string _CspProviderName;

		// Token: 0x04000382 RID: 898
		private bool _UseMachineContainer;

		// Token: 0x04000383 RID: 899
		private bool _UseOAEP;
	}
}
