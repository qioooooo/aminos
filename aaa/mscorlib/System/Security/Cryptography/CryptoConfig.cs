using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Util;
using System.Threading;

namespace System.Security.Cryptography
{
	// Token: 0x0200085E RID: 2142
	[ComVisible(true)]
	public class CryptoConfig
	{
		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x06004E8B RID: 20107 RVA: 0x00110B64 File Offset: 0x0010FB64
		private static object InternalSyncObject
		{
			get
			{
				if (CryptoConfig.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref CryptoConfig.s_InternalSyncObject, obj, null);
				}
				return CryptoConfig.s_InternalSyncObject;
			}
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x06004E8C RID: 20108 RVA: 0x00110B90 File Offset: 0x0010FB90
		private static Hashtable DefaultOidHT
		{
			get
			{
				if (CryptoConfig.defaultOidHT == null)
				{
					CryptoConfig.defaultOidHT = new Hashtable(StringComparer.OrdinalIgnoreCase)
					{
						{ "SHA", "1.3.14.3.2.26" },
						{ "SHA1", "1.3.14.3.2.26" },
						{ "System.Security.Cryptography.SHA1", "1.3.14.3.2.26" },
						{ "System.Security.Cryptography.SHA1CryptoServiceProvider", "1.3.14.3.2.26" },
						{ "System.Security.Cryptography.SHA1Managed", "1.3.14.3.2.26" },
						{ "SHA256", "2.16.840.1.101.3.4.2.1" },
						{ "System.Security.Cryptography.SHA256", "2.16.840.1.101.3.4.2.1" },
						{ "System.Security.Cryptography.SHA256Managed", "2.16.840.1.101.3.4.2.1" },
						{ "SHA384", "2.16.840.1.101.3.4.2.2" },
						{ "System.Security.Cryptography.SHA384", "2.16.840.1.101.3.4.2.2" },
						{ "System.Security.Cryptography.SHA384Managed", "2.16.840.1.101.3.4.2.2" },
						{ "SHA512", "2.16.840.1.101.3.4.2.3" },
						{ "System.Security.Cryptography.SHA512", "2.16.840.1.101.3.4.2.3" },
						{ "System.Security.Cryptography.SHA512Managed", "2.16.840.1.101.3.4.2.3" },
						{ "RIPEMD160", "1.3.36.3.2.1" },
						{ "System.Security.Cryptography.RIPEMD160", "1.3.36.3.2.1" },
						{ "System.Security.Cryptography.RIPEMD160Managed", "1.3.36.3.2.1" },
						{ "MD5", "1.2.840.113549.2.5" },
						{ "System.Security.Cryptography.MD5", "1.2.840.113549.2.5" },
						{ "System.Security.Cryptography.MD5CryptoServiceProvider", "1.2.840.113549.2.5" },
						{ "System.Security.Cryptography.MD5Managed", "1.2.840.113549.2.5" },
						{ "TripleDESKeyWrap", "1.2.840.113549.1.9.16.3.6" },
						{ "RC2", "1.2.840.113549.3.2" },
						{ "System.Security.Cryptography.RC2CryptoServiceProvider", "1.2.840.113549.3.2" },
						{ "DES", "1.3.14.3.2.7" },
						{ "System.Security.Cryptography.DESCryptoServiceProvider", "1.3.14.3.2.7" },
						{ "TripleDES", "1.2.840.113549.3.7" },
						{ "System.Security.Cryptography.TripleDESCryptoServiceProvider", "1.2.840.113549.3.7" }
					};
				}
				return CryptoConfig.defaultOidHT;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x06004E8D RID: 20109 RVA: 0x00110D80 File Offset: 0x0010FD80
		private static Hashtable DefaultNameHT
		{
			get
			{
				if (CryptoConfig.defaultNameHT == null)
				{
					Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
					Type typeFromHandle = typeof(SHA1CryptoServiceProvider);
					Type typeFromHandle2 = typeof(MD5CryptoServiceProvider);
					Type typeFromHandle3 = typeof(SHA256Managed);
					Type typeFromHandle4 = typeof(SHA384Managed);
					Type typeFromHandle5 = typeof(SHA512Managed);
					Type typeFromHandle6 = typeof(RIPEMD160Managed);
					Type typeFromHandle7 = typeof(HMACMD5);
					Type typeFromHandle8 = typeof(HMACRIPEMD160);
					Type typeFromHandle9 = typeof(HMACSHA1);
					Type typeFromHandle10 = typeof(HMACSHA256);
					Type typeFromHandle11 = typeof(HMACSHA384);
					Type typeFromHandle12 = typeof(HMACSHA512);
					Type typeFromHandle13 = typeof(MACTripleDES);
					Type typeFromHandle14 = typeof(RSACryptoServiceProvider);
					Type typeFromHandle15 = typeof(DSACryptoServiceProvider);
					Type typeFromHandle16 = typeof(DESCryptoServiceProvider);
					Type typeFromHandle17 = typeof(TripleDESCryptoServiceProvider);
					Type typeFromHandle18 = typeof(RC2CryptoServiceProvider);
					Type typeFromHandle19 = typeof(RijndaelManaged);
					Type typeFromHandle20 = typeof(DSASignatureDescription);
					Type typeFromHandle21 = typeof(RSAPKCS1SHA1SignatureDescription);
					Type typeFromHandle22 = typeof(RNGCryptoServiceProvider);
					hashtable.Add("RandomNumberGenerator", typeFromHandle22);
					hashtable.Add("System.Security.Cryptography.RandomNumberGenerator", typeFromHandle22);
					hashtable.Add("SHA", typeFromHandle);
					hashtable.Add("SHA1", typeFromHandle);
					hashtable.Add("System.Security.Cryptography.SHA1", typeFromHandle);
					hashtable.Add("System.Security.Cryptography.HashAlgorithm", typeFromHandle);
					hashtable.Add("MD5", typeFromHandle2);
					hashtable.Add("System.Security.Cryptography.MD5", typeFromHandle2);
					hashtable.Add("SHA256", typeFromHandle3);
					hashtable.Add("SHA-256", typeFromHandle3);
					hashtable.Add("System.Security.Cryptography.SHA256", typeFromHandle3);
					hashtable.Add("SHA384", typeFromHandle4);
					hashtable.Add("SHA-384", typeFromHandle4);
					hashtable.Add("System.Security.Cryptography.SHA384", typeFromHandle4);
					hashtable.Add("SHA512", typeFromHandle5);
					hashtable.Add("SHA-512", typeFromHandle5);
					hashtable.Add("System.Security.Cryptography.SHA512", typeFromHandle5);
					hashtable.Add("RIPEMD160", typeFromHandle6);
					hashtable.Add("RIPEMD-160", typeFromHandle6);
					hashtable.Add("System.Security.Cryptography.RIPEMD160", typeFromHandle6);
					hashtable.Add("System.Security.Cryptography.RIPEMD160Managed", typeFromHandle6);
					hashtable.Add("System.Security.Cryptography.HMAC", typeFromHandle9);
					hashtable.Add("System.Security.Cryptography.KeyedHashAlgorithm", typeFromHandle9);
					hashtable.Add("HMACMD5", typeFromHandle7);
					hashtable.Add("System.Security.Cryptography.HMACMD5", typeFromHandle7);
					hashtable.Add("HMACRIPEMD160", typeFromHandle8);
					hashtable.Add("System.Security.Cryptography.HMACRIPEMD160", typeFromHandle8);
					hashtable.Add("HMACSHA1", typeFromHandle9);
					hashtable.Add("System.Security.Cryptography.HMACSHA1", typeFromHandle9);
					hashtable.Add("HMACSHA256", typeFromHandle10);
					hashtable.Add("System.Security.Cryptography.HMACSHA256", typeFromHandle10);
					hashtable.Add("HMACSHA384", typeFromHandle11);
					hashtable.Add("System.Security.Cryptography.HMACSHA384", typeFromHandle11);
					hashtable.Add("HMACSHA512", typeFromHandle12);
					hashtable.Add("System.Security.Cryptography.HMACSHA512", typeFromHandle12);
					hashtable.Add("MACTripleDES", typeFromHandle13);
					hashtable.Add("System.Security.Cryptography.MACTripleDES", typeFromHandle13);
					hashtable.Add("RSA", typeFromHandle14);
					hashtable.Add("System.Security.Cryptography.RSA", typeFromHandle14);
					hashtable.Add("System.Security.Cryptography.AsymmetricAlgorithm", typeFromHandle14);
					hashtable.Add("DSA", typeFromHandle15);
					hashtable.Add("System.Security.Cryptography.DSA", typeFromHandle15);
					hashtable.Add("DES", typeFromHandle16);
					hashtable.Add("System.Security.Cryptography.DES", typeFromHandle16);
					hashtable.Add("3DES", typeFromHandle17);
					hashtable.Add("TripleDES", typeFromHandle17);
					hashtable.Add("Triple DES", typeFromHandle17);
					hashtable.Add("System.Security.Cryptography.TripleDES", typeFromHandle17);
					hashtable.Add("RC2", typeFromHandle18);
					hashtable.Add("System.Security.Cryptography.RC2", typeFromHandle18);
					hashtable.Add("Rijndael", typeFromHandle19);
					hashtable.Add("System.Security.Cryptography.Rijndael", typeFromHandle19);
					hashtable.Add("System.Security.Cryptography.SymmetricAlgorithm", typeFromHandle19);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig#dsa-sha1", typeFromHandle20);
					hashtable.Add("System.Security.Cryptography.DSASignatureDescription", typeFromHandle20);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig#rsa-sha1", typeFromHandle21);
					hashtable.Add("System.Security.Cryptography.RSASignatureDescription", typeFromHandle21);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig#sha1", typeFromHandle);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#sha256", typeFromHandle3);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#sha512", typeFromHandle5);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#ripemd160", typeFromHandle6);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#des-cbc", typeFromHandle16);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#tripledes-cbc", typeFromHandle17);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#kw-tripledes", typeFromHandle17);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#aes128-cbc", typeFromHandle19);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#kw-aes128", typeFromHandle19);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#aes192-cbc", typeFromHandle19);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#kw-aes192", typeFromHandle19);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#aes256-cbc", typeFromHandle19);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc#kw-aes256", typeFromHandle19);
					hashtable.Add("http://www.w3.org/TR/2001/REC-xml-c14n-20010315", "System.Security.Cryptography.Xml.XmlDsigC14NTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments", "System.Security.Cryptography.Xml.XmlDsigC14NWithCommentsTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2001/10/xml-exc-c14n#", "System.Security.Cryptography.Xml.XmlDsigExcC14NTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2001/10/xml-exc-c14n#WithComments", "System.Security.Cryptography.Xml.XmlDsigExcC14NWithCommentsTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig#base64", "System.Security.Cryptography.Xml.XmlDsigBase64Transform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/TR/1999/REC-xpath-19991116", "System.Security.Cryptography.Xml.XmlDsigXPathTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/TR/1999/REC-xslt-19991116", "System.Security.Cryptography.Xml.XmlDsigXsltTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig#enveloped-signature", "System.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2002/07/decrypt#XML", "System.Security.Cryptography.Xml.XmlDecryptionTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("urn:mpeg:mpeg21:2003:01-REL-R-NS:licenseTransform", "System.Security.Cryptography.Xml.XmlLicenseTransform, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig# X509Data", "System.Security.Cryptography.Xml.KeyInfoX509Data, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig# KeyName", "System.Security.Cryptography.Xml.KeyInfoName, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig# KeyValue/DSAKeyValue", "System.Security.Cryptography.Xml.DSAKeyValue, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig# KeyValue/RSAKeyValue", "System.Security.Cryptography.Xml.RSAKeyValue, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2000/09/xmldsig# RetrievalMethod", "System.Security.Cryptography.Xml.KeyInfoRetrievalMethod, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2001/04/xmlenc# EncryptedKey", "System.Security.Cryptography.Xml.KeyInfoEncryptedKey, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#md5", typeFromHandle2);
					hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#sha384", typeFromHandle4);
					hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-ripemd160", typeFromHandle8);
					hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", typeFromHandle10);
					hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-sha384", typeFromHandle11);
					hashtable.Add("http://www.w3.org/2001/04/xmldsig-more#hmac-sha512", typeFromHandle12);
					hashtable.Add("2.5.29.10", "System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension, System, Culture=neutral, PublicKeyToken=b77a5c561934e089, Version=" + CryptoConfig._Version);
					hashtable.Add("2.5.29.19", "System.Security.Cryptography.X509Certificates.X509BasicConstraintsExtension, System, Culture=neutral, PublicKeyToken=b77a5c561934e089, Version=" + CryptoConfig._Version);
					hashtable.Add("2.5.29.14", "System.Security.Cryptography.X509Certificates.X509SubjectKeyIdentifierExtension, System, Culture=neutral, PublicKeyToken=b77a5c561934e089, Version=" + CryptoConfig._Version);
					hashtable.Add("2.5.29.15", "System.Security.Cryptography.X509Certificates.X509KeyUsageExtension, System, Culture=neutral, PublicKeyToken=b77a5c561934e089, Version=" + CryptoConfig._Version);
					hashtable.Add("2.5.29.37", "System.Security.Cryptography.X509Certificates.X509EnhancedKeyUsageExtension, System, Culture=neutral, PublicKeyToken=b77a5c561934e089, Version=" + CryptoConfig._Version);
					hashtable.Add("X509Chain", "System.Security.Cryptography.X509Certificates.X509Chain, System, Culture=neutral, PublicKeyToken=b77a5c561934e089, Version=" + CryptoConfig._Version);
					hashtable.Add("1.2.840.113549.1.9.3", "System.Security.Cryptography.Pkcs.Pkcs9ContentType, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("1.2.840.113549.1.9.4", "System.Security.Cryptography.Pkcs.Pkcs9MessageDigest, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("1.2.840.113549.1.9.5", "System.Security.Cryptography.Pkcs.Pkcs9SigningTime, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("1.3.6.1.4.1.311.88.2.1", "System.Security.Cryptography.Pkcs.Pkcs9DocumentName, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					hashtable.Add("1.3.6.1.4.1.311.88.2.2", "System.Security.Cryptography.Pkcs.Pkcs9DocumentDescription, System.Security, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a, Version=" + CryptoConfig._Version);
					CryptoConfig.defaultNameHT = hashtable;
				}
				return CryptoConfig.defaultNameHT;
			}
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x00111540 File Offset: 0x00110540
		private static void InitializeConfigInfo()
		{
			Type typeFromHandle = typeof(CryptoConfig);
			CryptoConfig._Version = typeFromHandle.Assembly.GetVersion().ToString();
			if (CryptoConfig.machineNameHT == null && CryptoConfig.machineOidHT == null)
			{
				lock (CryptoConfig.InternalSyncObject)
				{
					string text = CryptoConfig.machineConfigDir + CryptoConfig.machineConfigFilename;
					new FileIOPermission(FileIOPermissionAccess.Read, text).Assert();
					if (File.Exists(text))
					{
						ConfigTreeParser configTreeParser = new ConfigTreeParser();
						ConfigNode configNode = configTreeParser.Parse(text, "configuration");
						if (configNode != null)
						{
							ArrayList children = configNode.Children;
							ConfigNode configNode2 = null;
							foreach (object obj in children)
							{
								ConfigNode configNode3 = (ConfigNode)obj;
								if (configNode3.Name.Equals("mscorlib"))
								{
									ArrayList attributes = configNode3.Attributes;
									if (attributes.Count > 0)
									{
										DictionaryEntry dictionaryEntry = (DictionaryEntry)configNode3.Attributes[0];
										if (dictionaryEntry.Key.Equals("version") && dictionaryEntry.Value.Equals(CryptoConfig._Version))
										{
											configNode2 = configNode3;
											break;
										}
									}
									else
									{
										configNode2 = configNode3;
									}
								}
							}
							if (configNode2 != null)
							{
								ArrayList children2 = configNode2.Children;
								ConfigNode configNode4 = null;
								foreach (object obj2 in children2)
								{
									ConfigNode configNode5 = (ConfigNode)obj2;
									if (configNode5.Name.Equals("cryptographySettings"))
									{
										configNode4 = configNode5;
										break;
									}
								}
								if (configNode4 != null)
								{
									ConfigNode configNode6 = null;
									foreach (object obj3 in configNode4.Children)
									{
										ConfigNode configNode7 = (ConfigNode)obj3;
										if (configNode7.Name.Equals("cryptoNameMapping"))
										{
											configNode6 = configNode7;
											break;
										}
									}
									if (configNode6 != null)
									{
										ArrayList children3 = configNode6.Children;
										ConfigNode configNode8 = null;
										foreach (object obj4 in children3)
										{
											ConfigNode configNode9 = (ConfigNode)obj4;
											if (configNode9.Name.Equals("cryptoClasses"))
											{
												configNode8 = configNode9;
												break;
											}
										}
										if (configNode8 != null)
										{
											Hashtable hashtable = new Hashtable();
											Hashtable hashtable2 = new Hashtable();
											foreach (object obj5 in configNode8.Children)
											{
												ConfigNode configNode10 = (ConfigNode)obj5;
												if (configNode10.Name.Equals("cryptoClass") && configNode10.Attributes.Count > 0)
												{
													DictionaryEntry dictionaryEntry2 = (DictionaryEntry)configNode10.Attributes[0];
													hashtable.Add(dictionaryEntry2.Key, dictionaryEntry2.Value);
												}
											}
											foreach (object obj6 in children3)
											{
												ConfigNode configNode11 = (ConfigNode)obj6;
												if (configNode11.Name.Equals("nameEntry"))
												{
													string text2 = null;
													string text3 = null;
													foreach (object obj7 in configNode11.Attributes)
													{
														DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj7;
														if (((string)dictionaryEntry3.Key).Equals("name"))
														{
															text2 = (string)dictionaryEntry3.Value;
														}
														else if (((string)dictionaryEntry3.Key).Equals("class"))
														{
															text3 = (string)dictionaryEntry3.Value;
														}
													}
													if (text2 != null && text3 != null)
													{
														string text4 = (string)hashtable[text3];
														if (text4 != null)
														{
															hashtable2.Add(text2, text4);
														}
													}
												}
											}
											CryptoConfig.machineNameHT = hashtable2;
										}
									}
									ConfigNode configNode12 = null;
									foreach (object obj8 in configNode4.Children)
									{
										ConfigNode configNode13 = (ConfigNode)obj8;
										if (configNode13.Name.Equals("oidMap"))
										{
											configNode12 = configNode13;
											break;
										}
									}
									if (configNode12 != null)
									{
										Hashtable hashtable3 = new Hashtable();
										foreach (object obj9 in configNode12.Children)
										{
											ConfigNode configNode14 = (ConfigNode)obj9;
											if (configNode14.Name.Equals("oidEntry"))
											{
												string text5 = null;
												string text6 = null;
												foreach (object obj10 in configNode14.Attributes)
												{
													DictionaryEntry dictionaryEntry4 = (DictionaryEntry)obj10;
													if (((string)dictionaryEntry4.Key).Equals("OID"))
													{
														text5 = (string)dictionaryEntry4.Value;
													}
													else if (((string)dictionaryEntry4.Key).Equals("name"))
													{
														text6 = (string)dictionaryEntry4.Value;
													}
												}
												if (text6 != null && text5 != null)
												{
													hashtable3.Add(text6, text5);
												}
											}
										}
										CryptoConfig.machineOidHT = hashtable3;
									}
								}
							}
						}
					}
				}
			}
			CryptoConfig.isInitialized = true;
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x00111BF0 File Offset: 0x00110BF0
		public static object CreateFromName(string name, params object[] args)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			Type type = null;
			if (!CryptoConfig.isInitialized)
			{
				CryptoConfig.InitializeConfigInfo();
			}
			if (CryptoConfig.machineNameHT != null)
			{
				string text = (string)CryptoConfig.machineNameHT[name];
				if (text != null)
				{
					type = Type.GetType(text, false, false);
					if (type != null && !type.IsVisible)
					{
						type = null;
					}
				}
			}
			if (type == null)
			{
				object obj = CryptoConfig.DefaultNameHT[name];
				if (obj != null)
				{
					if (obj is Type)
					{
						type = (Type)obj;
					}
					else if (obj is string)
					{
						type = Type.GetType((string)obj, false, false);
						if (type != null && !type.IsVisible)
						{
							type = null;
						}
					}
				}
			}
			if (type == null)
			{
				type = Type.GetType(name, false, false);
				if (type != null && !type.IsVisible)
				{
					type = null;
				}
			}
			if (type == null)
			{
				return null;
			}
			RuntimeType runtimeType = type as RuntimeType;
			if (runtimeType == null)
			{
				return null;
			}
			if (args == null)
			{
				args = new object[0];
			}
			MethodBase[] array = runtimeType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance);
			if (array == null)
			{
				return null;
			}
			ArrayList arrayList = new ArrayList();
			foreach (MethodBase methodBase in array)
			{
				if (methodBase.GetParameters().Length == args.Length)
				{
					arrayList.Add(methodBase);
				}
			}
			if (arrayList.Count == 0)
			{
				return null;
			}
			array = arrayList.ToArray(typeof(MethodBase)) as MethodBase[];
			object obj2;
			RuntimeConstructorInfo runtimeConstructorInfo = Type.DefaultBinder.BindToMethod(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, array, ref args, null, null, null, out obj2) as RuntimeConstructorInfo;
			if (runtimeConstructorInfo == null || typeof(Delegate).IsAssignableFrom(runtimeConstructorInfo.DeclaringType))
			{
				return null;
			}
			object obj3 = runtimeConstructorInfo.Invoke(BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, Type.DefaultBinder, args, null);
			if (obj2 != null)
			{
				Type.DefaultBinder.ReorderArgumentArray(ref args, obj2);
			}
			return obj3;
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x00111D9B File Offset: 0x00110D9B
		public static object CreateFromName(string name)
		{
			return CryptoConfig.CreateFromName(name, null);
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x00111DA4 File Offset: 0x00110DA4
		public static string MapNameToOID(string name)
		{
			return CryptoConfig.MapNameToOID(name, OidGroup.AllGroups);
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x00111DB0 File Offset: 0x00110DB0
		internal static string MapNameToOID(string name, OidGroup group)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (!CryptoConfig.isInitialized)
			{
				CryptoConfig.InitializeConfigInfo();
			}
			string text = null;
			if (CryptoConfig.machineOidHT != null)
			{
				text = CryptoConfig.machineOidHT[name] as string;
			}
			if (text == null)
			{
				text = CryptoConfig.DefaultOidHT[name] as string;
			}
			if (text == null)
			{
				text = X509Utils._GetOidFromFriendlyName(name, group);
			}
			return text;
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x00111E14 File Offset: 0x00110E14
		public static byte[] EncodeOID(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			char[] array = new char[] { '.' };
			string[] array2 = str.Split(array);
			uint[] array3 = new uint[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array3[i] = (uint)int.Parse(array2[i], CultureInfo.InvariantCulture);
			}
			byte[] array4 = new byte[array3.Length * 5];
			int num = 0;
			if (array3.Length < 2)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_InvalidOID"));
			}
			uint num2 = array3[0] * 40U + array3[1];
			byte[] array5 = CryptoConfig.EncodeSingleOIDNum(num2);
			Array.Copy(array5, 0, array4, num, array5.Length);
			num += array5.Length;
			for (int j = 2; j < array3.Length; j++)
			{
				array5 = CryptoConfig.EncodeSingleOIDNum(array3[j]);
				Buffer.InternalBlockCopy(array5, 0, array4, num, array5.Length);
				num += array5.Length;
			}
			if (num > 127)
			{
				throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("Cryptography_Config_EncodedOIDError"));
			}
			array5 = new byte[num + 2];
			array5[0] = 6;
			array5[1] = (byte)num;
			Buffer.InternalBlockCopy(array4, 0, array5, 2, num);
			return array5;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x00111F34 File Offset: 0x00110F34
		private static byte[] EncodeSingleOIDNum(uint dwValue)
		{
			if (dwValue < 128U)
			{
				return new byte[] { (byte)dwValue };
			}
			if (dwValue < 16384U)
			{
				return new byte[]
				{
					(byte)((dwValue >> 7) | 128U),
					(byte)(dwValue & 127U)
				};
			}
			if (dwValue < 2097152U)
			{
				return new byte[]
				{
					(byte)((dwValue >> 14) | 128U),
					(byte)((dwValue >> 7) | 128U),
					(byte)(dwValue & 127U)
				};
			}
			if (dwValue < 268435456U)
			{
				return new byte[]
				{
					(byte)((dwValue >> 21) | 128U),
					(byte)((dwValue >> 14) | 128U),
					(byte)((dwValue >> 7) | 128U),
					(byte)(dwValue & 127U)
				};
			}
			return new byte[]
			{
				(byte)((dwValue >> 28) | 128U),
				(byte)((dwValue >> 21) | 128U),
				(byte)((dwValue >> 14) | 128U),
				(byte)((dwValue >> 7) | 128U),
				(byte)(dwValue & 127U)
			};
		}

		// Token: 0x0400286C RID: 10348
		private static Hashtable defaultOidHT = null;

		// Token: 0x0400286D RID: 10349
		private static Hashtable defaultNameHT = null;

		// Token: 0x0400286E RID: 10350
		private static string machineConfigDir = Config.MachineDirectory;

		// Token: 0x0400286F RID: 10351
		private static Hashtable machineOidHT = null;

		// Token: 0x04002870 RID: 10352
		private static Hashtable machineNameHT = null;

		// Token: 0x04002871 RID: 10353
		private static string machineConfigFilename = "machine.config";

		// Token: 0x04002872 RID: 10354
		private static bool isInitialized = false;

		// Token: 0x04002873 RID: 10355
		private static string _Version = null;

		// Token: 0x04002874 RID: 10356
		private static object s_InternalSyncObject;
	}
}
