using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Win32;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000083 RID: 131
	internal class Utils
	{
		// Token: 0x06000213 RID: 531 RVA: 0x0000C135 File Offset: 0x0000B135
		private Utils()
		{
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000C13D File Offset: 0x0000B13D
		private static bool HasNamespace(XmlElement element, string prefix, string value)
		{
			return Utils.IsCommittedNamespace(element, prefix, value) || (element.Prefix == prefix && element.NamespaceURI == value);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000C16C File Offset: 0x0000B16C
		internal static bool IsCommittedNamespace(XmlElement element, string prefix, string value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			string text = ((prefix.Length > 0) ? ("xmlns:" + prefix) : "xmlns");
			return element.HasAttribute(text) && element.GetAttribute(text) == value;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000C1C0 File Offset: 0x0000B1C0
		internal static bool IsRedundantNamespace(XmlElement element, string prefix, string value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			for (XmlNode xmlNode = element.ParentNode; xmlNode != null; xmlNode = xmlNode.ParentNode)
			{
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null && Utils.HasNamespace(xmlElement, prefix, value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000C208 File Offset: 0x0000B208
		internal static string GetAttribute(XmlElement element, string localName, string namespaceURI)
		{
			string text = (element.HasAttribute(localName) ? element.GetAttribute(localName) : null);
			if (text == null && element.HasAttribute(localName, namespaceURI))
			{
				text = element.GetAttribute(localName, namespaceURI);
			}
			return text;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000C240 File Offset: 0x0000B240
		internal static bool HasAttribute(XmlElement element, string localName, string namespaceURI)
		{
			return element.HasAttribute(localName) || element.HasAttribute(localName, namespaceURI);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000C255 File Offset: 0x0000B255
		internal static bool IsNamespaceNode(XmlNode n)
		{
			return n.NodeType == XmlNodeType.Attribute && (n.Prefix.Equals("xmlns") || (n.Prefix.Length == 0 && n.LocalName.Equals("xmlns")));
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C295 File Offset: 0x0000B295
		internal static bool IsXmlNamespaceNode(XmlNode n)
		{
			return n.NodeType == XmlNodeType.Attribute && n.Prefix.Equals("xml");
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C2B4 File Offset: 0x0000B2B4
		internal static bool IsDefaultNamespaceNode(XmlNode n)
		{
			bool flag = n.NodeType == XmlNodeType.Attribute && n.Prefix.Length == 0 && n.LocalName.Equals("xmlns");
			bool flag2 = Utils.IsXmlNamespaceNode(n);
			return flag || flag2;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C2F8 File Offset: 0x0000B2F8
		internal static bool IsEmptyDefaultNamespaceNode(XmlNode n)
		{
			return Utils.IsDefaultNamespaceNode(n) && n.Value.Length == 0;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C312 File Offset: 0x0000B312
		internal static string GetNamespacePrefix(XmlAttribute a)
		{
			if (a.Prefix.Length != 0)
			{
				return a.LocalName;
			}
			return string.Empty;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C32D File Offset: 0x0000B32D
		internal static bool HasNamespacePrefix(XmlAttribute a, string nsPrefix)
		{
			return Utils.GetNamespacePrefix(a).Equals(nsPrefix);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000C33B File Offset: 0x0000B33B
		internal static bool IsNonRedundantNamespaceDecl(XmlAttribute a, XmlAttribute nearestAncestorWithSamePrefix)
		{
			if (nearestAncestorWithSamePrefix == null)
			{
				return !Utils.IsEmptyDefaultNamespaceNode(a);
			}
			return !nearestAncestorWithSamePrefix.Value.Equals(a.Value);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000C35E File Offset: 0x0000B35E
		internal static bool IsXmlPrefixDefinitionNode(XmlAttribute a)
		{
			return false;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C361 File Offset: 0x0000B361
		internal static string DiscardWhiteSpaces(string inputBuffer)
		{
			return Utils.DiscardWhiteSpaces(inputBuffer, 0, inputBuffer.Length);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C370 File Offset: 0x0000B370
		internal static string DiscardWhiteSpaces(string inputBuffer, int inputOffset, int inputCount)
		{
			int num = 0;
			for (int i = 0; i < inputCount; i++)
			{
				if (char.IsWhiteSpace(inputBuffer[inputOffset + i]))
				{
					num++;
				}
			}
			char[] array = new char[inputCount - num];
			num = 0;
			for (int i = 0; i < inputCount; i++)
			{
				if (!char.IsWhiteSpace(inputBuffer[inputOffset + i]))
				{
					array[num++] = inputBuffer[inputOffset + i];
				}
			}
			return new string(array);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000C3DC File Offset: 0x0000B3DC
		internal static void SBReplaceCharWithString(StringBuilder sb, char oldChar, string newString)
		{
			int i = 0;
			int length = newString.Length;
			while (i < sb.Length)
			{
				if (sb[i] == oldChar)
				{
					sb.Remove(i, 1);
					sb.Insert(i, newString);
					i += length;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C424 File Offset: 0x0000B424
		internal static XmlReader PreProcessStreamInput(Stream inputStream, XmlResolver xmlResolver, string baseUri)
		{
			XmlReaderSettings secureXmlReaderSettings = Utils.GetSecureXmlReaderSettings(xmlResolver);
			return XmlReader.Create(inputStream, secureXmlReaderSettings, baseUri);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000C444 File Offset: 0x0000B444
		internal static XmlReaderSettings GetSecureXmlReaderSettings(XmlResolver xmlResolver)
		{
			return new XmlReaderSettings
			{
				XmlResolver = xmlResolver,
				ProhibitDtd = false,
				MaxCharactersFromEntities = Utils.GetMaxCharactersFromEntities()
			};
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C474 File Offset: 0x0000B474
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static long GetMaxCharactersFromEntities()
		{
			if (Utils.maxCharactersFromEntities != null)
			{
				return Utils.maxCharactersFromEntities.Value;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlMaxCharactersFromEntities", 10000000L);
			Utils.maxCharactersFromEntities = new long?(netFxSecurityRegistryValue);
			return Utils.maxCharactersFromEntities.Value;
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C4C0 File Offset: 0x0000B4C0
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool AllowAmbiguousReferenceTargets()
		{
			if (Utils.s_allowAmbiguousReferenceTarget != null)
			{
				return Utils.s_allowAmbiguousReferenceTarget.Value;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlAllowAmbiguousReferenceTargets", 0L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_allowAmbiguousReferenceTarget = new bool?(flag);
			return Utils.s_allowAmbiguousReferenceTarget.Value;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C510 File Offset: 0x0000B510
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool AllowDetachedSignature()
		{
			if (Utils.s_allowDetachedSignature != null)
			{
				return Utils.s_allowDetachedSignature.Value;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlAllowDetachedSignature", 0L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_allowDetachedSignature = new bool?(flag);
			return Utils.s_allowDetachedSignature.Value;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C560 File Offset: 0x0000B560
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool RequireNCNameIdentifier()
		{
			if (Utils.s_readRequireNCNameIdentifier)
			{
				return Utils.s_requireNCNameIdentifier;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlRequireNCNameIdentifier", 1L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_requireNCNameIdentifier = flag;
			Thread.MemoryBarrier();
			Utils.s_readRequireNCNameIdentifier = true;
			return Utils.s_requireNCNameIdentifier;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C5A8 File Offset: 0x0000B5A8
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static long GetMaxTransformsPerReference()
		{
			if (Utils.s_readMaxTransformsPerReference)
			{
				return Utils.s_maxTransformsPerReference;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlMaxTransformsPerReference", 10L);
			Utils.s_maxTransformsPerReference = netFxSecurityRegistryValue;
			Thread.MemoryBarrier();
			Utils.s_readMaxTransformsPerReference = true;
			return Utils.s_maxTransformsPerReference;
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C5E8 File Offset: 0x0000B5E8
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static long GetMaxReferencesPerSignedInfo()
		{
			if (Utils.s_readMaxReferencesPerSignedInfo)
			{
				return Utils.s_maxReferencesPerSignedInfo;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlMaxReferencesPerSignedInfo", 100L);
			Utils.s_maxReferencesPerSignedInfo = netFxSecurityRegistryValue;
			Thread.MemoryBarrier();
			Utils.s_readMaxReferencesPerSignedInfo = true;
			return Utils.s_maxReferencesPerSignedInfo;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C628 File Offset: 0x0000B628
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool GetAllowAdditionalSignatureNodes()
		{
			if (Utils.s_readAllowAdditionalSignatureNodes)
			{
				return Utils.s_allowAdditionalSignatureNodes;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlAllowAdditionalSignatureNodes", 0L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_allowAdditionalSignatureNodes = flag;
			Thread.MemoryBarrier();
			Utils.s_readAllowAdditionalSignatureNodes = true;
			return Utils.s_allowAdditionalSignatureNodes;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C670 File Offset: 0x0000B670
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool GetSkipSignatureAttributeEnforcement()
		{
			if (Utils.s_readSkipSignatureAttributeEnforcement)
			{
				return Utils.s_skipSignatureAttributeEnforcement;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedXmlSkipSignatureAttributeEnforcement", 0L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_skipSignatureAttributeEnforcement = flag;
			Thread.MemoryBarrier();
			Utils.s_readSkipSignatureAttributeEnforcement = true;
			return Utils.s_skipSignatureAttributeEnforcement;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000C6B8 File Offset: 0x0000B6B8
		internal static bool VerifyAttributes(XmlElement element, string expectedAttrName)
		{
			return Utils.VerifyAttributes(element, (expectedAttrName == null) ? null : new string[] { expectedAttrName });
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000C6E0 File Offset: 0x0000B6E0
		internal static bool VerifyAttributes(XmlElement element, string[] expectedAttrNames)
		{
			if (!Utils.GetSkipSignatureAttributeEnforcement())
			{
				foreach (object obj in element.Attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					bool flag = xmlAttribute.Name == "xmlns" || xmlAttribute.Name.StartsWith("xmlns:") || xmlAttribute.Name == "xml:space" || xmlAttribute.Name == "xml:lang" || xmlAttribute.Name == "xml:base";
					int num = 0;
					while (!flag && expectedAttrNames != null && num < expectedAttrNames.Length)
					{
						flag = xmlAttribute.Name == expectedAttrNames[num];
						num++;
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000C7D4 File Offset: 0x0000B7D4
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool GetAllowBareTypeReference()
		{
			if (Utils.s_readAllowBareTypeReference)
			{
				return Utils.s_allowBareTypeReference;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("CryptoXmlAllowBareTypeReference", 0L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_allowBareTypeReference = flag;
			Thread.MemoryBarrier();
			Utils.s_readAllowBareTypeReference = true;
			return Utils.s_allowBareTypeReference;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000C81C File Offset: 0x0000B81C
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static bool GetLeaveCipherValueUnchecked()
		{
			if (Utils.s_readLeaveCipherValueUnchecked)
			{
				return Utils.s_leaveCipherValueUnchecked;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("EncryptedXmlLeaveCipherValueUnchecked", 0L);
			bool flag = netFxSecurityRegistryValue != 0L;
			Utils.s_leaveCipherValueUnchecked = flag;
			Thread.MemoryBarrier();
			Utils.s_readLeaveCipherValueUnchecked = true;
			return Utils.s_leaveCipherValueUnchecked;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000C864 File Offset: 0x0000B864
		internal static T CreateFromName<T>(string key) where T : class
		{
			if (Utils.GetAllowBareTypeReference())
			{
				return CryptoConfig.CreateFromName(key) as T;
			}
			if (key == null || key.IndexOfAny(Utils.s_invalidChars) >= 0)
			{
				return default(T);
			}
			T t;
			try
			{
				t = CryptoConfig.CreateFromName(key) as T;
			}
			catch (Exception)
			{
				t = default(T);
			}
			return t;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C8D8 File Offset: 0x0000B8D8
		private static long GetNetFxSecurityRegistryValue(string regValueName, long defaultValue)
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework\\Security", false))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue(regValueName);
						if (value != null)
						{
							RegistryValueKind valueKind = registryKey.GetValueKind(regValueName);
							if (valueKind == RegistryValueKind.DWord || valueKind == RegistryValueKind.QWord)
							{
								return Convert.ToInt64(value, CultureInfo.InvariantCulture);
							}
						}
					}
				}
			}
			catch (SecurityException)
			{
			}
			return defaultValue;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C954 File Offset: 0x0000B954
		internal static XmlDocument PreProcessDocumentInput(XmlDocument document, XmlResolver xmlResolver, string baseUri)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			MyXmlDocument myXmlDocument = new MyXmlDocument();
			myXmlDocument.PreserveWhitespace = document.PreserveWhitespace;
			using (TextReader textReader = new StringReader(document.OuterXml))
			{
				XmlReader xmlReader = XmlReader.Create(textReader, new XmlReaderSettings
				{
					XmlResolver = xmlResolver,
					ProhibitDtd = false,
					MaxCharactersFromEntities = Utils.GetMaxCharactersFromEntities()
				}, baseUri);
				myXmlDocument.Load(xmlReader);
			}
			return myXmlDocument;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C9DC File Offset: 0x0000B9DC
		internal static XmlDocument PreProcessElementInput(XmlElement elem, XmlResolver xmlResolver, string baseUri)
		{
			if (elem == null)
			{
				throw new ArgumentNullException("elem");
			}
			MyXmlDocument myXmlDocument = new MyXmlDocument();
			myXmlDocument.PreserveWhitespace = true;
			using (TextReader textReader = new StringReader(elem.OuterXml))
			{
				XmlReader xmlReader = XmlReader.Create(textReader, new XmlReaderSettings
				{
					XmlResolver = xmlResolver,
					ProhibitDtd = false,
					MaxCharactersFromEntities = Utils.GetMaxCharactersFromEntities()
				}, baseUri);
				myXmlDocument.Load(xmlReader);
			}
			return myXmlDocument;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000CA5C File Offset: 0x0000BA5C
		internal static XmlDocument DiscardComments(XmlDocument document)
		{
			XmlNodeList xmlNodeList = document.SelectNodes("//comment()");
			if (xmlNodeList != null)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					xmlNode.ParentNode.RemoveChild(xmlNode);
				}
			}
			return document;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000CAC8 File Offset: 0x0000BAC8
		internal static XmlNodeList AllDescendantNodes(XmlNode node, bool includeComments)
		{
			CanonicalXmlNodeList canonicalXmlNodeList = new CanonicalXmlNodeList();
			CanonicalXmlNodeList canonicalXmlNodeList2 = new CanonicalXmlNodeList();
			CanonicalXmlNodeList canonicalXmlNodeList3 = new CanonicalXmlNodeList();
			CanonicalXmlNodeList canonicalXmlNodeList4 = new CanonicalXmlNodeList();
			int num = 0;
			canonicalXmlNodeList2.Add(node);
			do
			{
				XmlNode xmlNode = canonicalXmlNodeList2[num];
				XmlNodeList childNodes = xmlNode.ChildNodes;
				if (childNodes != null)
				{
					foreach (object obj in childNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj;
						if (includeComments || !(xmlNode2 is XmlComment))
						{
							canonicalXmlNodeList2.Add(xmlNode2);
						}
					}
				}
				XmlAttributeCollection attributes = xmlNode.Attributes;
				if (attributes != null)
				{
					foreach (object obj2 in xmlNode.Attributes)
					{
						XmlNode xmlNode3 = (XmlNode)obj2;
						if (xmlNode3.LocalName == "xmlns" || xmlNode3.Prefix == "xmlns")
						{
							canonicalXmlNodeList4.Add(xmlNode3);
						}
						else
						{
							canonicalXmlNodeList3.Add(xmlNode3);
						}
					}
				}
				num++;
			}
			while (num < canonicalXmlNodeList2.Count);
			foreach (object obj3 in canonicalXmlNodeList2)
			{
				XmlNode xmlNode4 = (XmlNode)obj3;
				canonicalXmlNodeList.Add(xmlNode4);
			}
			foreach (object obj4 in canonicalXmlNodeList3)
			{
				XmlNode xmlNode5 = (XmlNode)obj4;
				canonicalXmlNodeList.Add(xmlNode5);
			}
			foreach (object obj5 in canonicalXmlNodeList4)
			{
				XmlNode xmlNode6 = (XmlNode)obj5;
				canonicalXmlNodeList.Add(xmlNode6);
			}
			return canonicalXmlNodeList;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000CCFC File Offset: 0x0000BCFC
		internal static bool NodeInList(XmlNode node, XmlNodeList nodeList)
		{
			foreach (object obj in nodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode == node)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000CD54 File Offset: 0x0000BD54
		internal static string GetIdFromLocalUri(string uri, out bool discardComments)
		{
			string text = uri.Substring(1);
			discardComments = true;
			if (text.StartsWith("xpointer(id(", StringComparison.Ordinal))
			{
				int num = text.IndexOf("id(", StringComparison.Ordinal);
				int num2 = text.IndexOf(")", StringComparison.Ordinal);
				if (num2 < 0 || num2 < num + 3)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidReference"));
				}
				text = text.Substring(num + 3, num2 - num - 3);
				text = text.Replace("'", "");
				text = text.Replace("\"", "");
				discardComments = false;
			}
			return text;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000CDE4 File Offset: 0x0000BDE4
		internal static string ExtractIdFromLocalUri(string uri)
		{
			string text = uri.Substring(1);
			if (text.StartsWith("xpointer(id(", StringComparison.Ordinal))
			{
				int num = text.IndexOf("id(", StringComparison.Ordinal);
				int num2 = text.IndexOf(")", StringComparison.Ordinal);
				if (num2 < 0 || num2 < num + 3)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidReference"));
				}
				text = text.Substring(num + 3, num2 - num - 3);
				text = text.Replace("'", "");
				text = text.Replace("\"", "");
			}
			return text;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000CE70 File Offset: 0x0000BE70
		internal static void RemoveAllChildren(XmlElement inputElement)
		{
			XmlNode nextSibling;
			for (XmlNode xmlNode = inputElement.FirstChild; xmlNode != null; xmlNode = nextSibling)
			{
				nextSibling = xmlNode.NextSibling;
				inputElement.RemoveChild(xmlNode);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000CE9C File Offset: 0x0000BE9C
		internal static long Pump(Stream input, Stream output)
		{
			MemoryStream memoryStream = input as MemoryStream;
			if (memoryStream != null && memoryStream.Position == 0L)
			{
				memoryStream.WriteTo(output);
				return memoryStream.Length;
			}
			byte[] array = new byte[4096];
			long num = 0L;
			int num2;
			while ((num2 = input.Read(array, 0, 4096)) > 0)
			{
				output.Write(array, 0, num2);
				num += (long)num2;
			}
			return num;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000CEFC File Offset: 0x0000BEFC
		internal static Hashtable TokenizePrefixListString(string s)
		{
			Hashtable hashtable = new Hashtable();
			if (s != null)
			{
				string[] array = s.Split(null);
				foreach (string text in array)
				{
					if (text.Equals("#default"))
					{
						hashtable.Add(string.Empty, true);
					}
					else if (text.Length > 0)
					{
						hashtable.Add(text, true);
					}
				}
			}
			return hashtable;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000CF6C File Offset: 0x0000BF6C
		internal static string EscapeWhitespaceData(string data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(data);
			Utils.SBReplaceCharWithString(stringBuilder, '\r', "&#xD;");
			return stringBuilder.ToString();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000CF9C File Offset: 0x0000BF9C
		internal static string EscapeTextData(string data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(data);
			stringBuilder.Replace("&", "&amp;");
			stringBuilder.Replace("<", "&lt;");
			stringBuilder.Replace(">", "&gt;");
			Utils.SBReplaceCharWithString(stringBuilder, '\r', "&#xD;");
			return stringBuilder.ToString();
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000CFFD File Offset: 0x0000BFFD
		internal static string EscapeCData(string data)
		{
			return Utils.EscapeTextData(data);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000D008 File Offset: 0x0000C008
		internal static string EscapeAttributeValue(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			stringBuilder.Replace("&", "&amp;");
			stringBuilder.Replace("<", "&lt;");
			stringBuilder.Replace("\"", "&quot;");
			Utils.SBReplaceCharWithString(stringBuilder, '\t', "&#x9;");
			Utils.SBReplaceCharWithString(stringBuilder, '\n', "&#xA;");
			Utils.SBReplaceCharWithString(stringBuilder, '\r', "&#xD;");
			return stringBuilder.ToString();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000D084 File Offset: 0x0000C084
		internal static XmlDocument GetOwnerDocument(XmlNodeList nodeList)
		{
			foreach (object obj in nodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.OwnerDocument != null)
				{
					return xmlNode.OwnerDocument;
				}
			}
			return null;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D0E8 File Offset: 0x0000C0E8
		internal static void AddNamespaces(XmlElement elem, CanonicalXmlNodeList namespaces)
		{
			if (namespaces != null)
			{
				foreach (object obj in namespaces)
				{
					XmlNode xmlNode = (XmlNode)obj;
					string text = ((xmlNode.Prefix.Length > 0) ? (xmlNode.Prefix + ":" + xmlNode.LocalName) : xmlNode.LocalName);
					if (!elem.HasAttribute(text) && (!text.Equals("xmlns") || elem.Prefix.Length != 0))
					{
						XmlAttribute xmlAttribute = elem.OwnerDocument.CreateAttribute(text);
						xmlAttribute.Value = xmlNode.Value;
						elem.SetAttributeNode(xmlAttribute);
					}
				}
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000D1B4 File Offset: 0x0000C1B4
		internal static void AddNamespaces(XmlElement elem, Hashtable namespaces)
		{
			if (namespaces != null)
			{
				foreach (object obj in namespaces.Keys)
				{
					string text = (string)obj;
					if (!elem.HasAttribute(text))
					{
						XmlAttribute xmlAttribute = elem.OwnerDocument.CreateAttribute(text);
						xmlAttribute.Value = namespaces[text] as string;
						elem.SetAttributeNode(xmlAttribute);
					}
				}
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D23C File Offset: 0x0000C23C
		internal static CanonicalXmlNodeList GetPropagatedAttributes(XmlElement elem)
		{
			if (elem == null)
			{
				return null;
			}
			CanonicalXmlNodeList canonicalXmlNodeList = new CanonicalXmlNodeList();
			XmlNode xmlNode = elem;
			if (xmlNode == null)
			{
				return null;
			}
			bool flag = true;
			while (xmlNode != null)
			{
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement == null)
				{
					xmlNode = xmlNode.ParentNode;
				}
				else
				{
					if (!Utils.IsCommittedNamespace(xmlElement, xmlElement.Prefix, xmlElement.NamespaceURI) && !Utils.IsRedundantNamespace(xmlElement, xmlElement.Prefix, xmlElement.NamespaceURI))
					{
						string text = ((xmlElement.Prefix.Length > 0) ? ("xmlns:" + xmlElement.Prefix) : "xmlns");
						XmlAttribute xmlAttribute = elem.OwnerDocument.CreateAttribute(text);
						xmlAttribute.Value = xmlElement.NamespaceURI;
						canonicalXmlNodeList.Add(xmlAttribute);
					}
					if (xmlElement.HasAttributes)
					{
						XmlAttributeCollection attributes = xmlElement.Attributes;
						foreach (object obj in attributes)
						{
							XmlAttribute xmlAttribute2 = (XmlAttribute)obj;
							if (flag && xmlAttribute2.LocalName == "xmlns")
							{
								XmlAttribute xmlAttribute3 = elem.OwnerDocument.CreateAttribute("xmlns");
								xmlAttribute3.Value = xmlAttribute2.Value;
								canonicalXmlNodeList.Add(xmlAttribute3);
								flag = false;
							}
							else if (xmlAttribute2.Prefix == "xmlns" || xmlAttribute2.Prefix == "xml")
							{
								canonicalXmlNodeList.Add(xmlAttribute2);
							}
							else if (xmlAttribute2.NamespaceURI.Length > 0 && !Utils.IsCommittedNamespace(xmlElement, xmlAttribute2.Prefix, xmlAttribute2.NamespaceURI) && !Utils.IsRedundantNamespace(xmlElement, xmlAttribute2.Prefix, xmlAttribute2.NamespaceURI))
							{
								string text2 = ((xmlAttribute2.Prefix.Length > 0) ? ("xmlns:" + xmlAttribute2.Prefix) : "xmlns");
								XmlAttribute xmlAttribute4 = elem.OwnerDocument.CreateAttribute(text2);
								xmlAttribute4.Value = xmlAttribute2.NamespaceURI;
								canonicalXmlNodeList.Add(xmlAttribute4);
							}
						}
					}
					xmlNode = xmlNode.ParentNode;
				}
			}
			return canonicalXmlNodeList;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000D46C File Offset: 0x0000C46C
		internal static byte[] ConvertIntToByteArray(int dwInput)
		{
			byte[] array = new byte[8];
			int num = 0;
			if (dwInput == 0)
			{
				return new byte[1];
			}
			int i = dwInput;
			while (i > 0)
			{
				int num2 = i % 256;
				array[num] = (byte)num2;
				i = (i - num2) / 256;
				num++;
			}
			byte[] array2 = new byte[num];
			for (int j = 0; j < num; j++)
			{
				array2[j] = array[num - j - 1];
			}
			return array2;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000D4D8 File Offset: 0x0000C4D8
		internal static int GetHexArraySize(byte[] hex)
		{
			int num = hex.Length;
			while (num-- > 0 && hex[num] == 0)
			{
			}
			return num + 1;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000D4FC File Offset: 0x0000C4FC
		internal static X509Certificate2Collection BuildBagOfCerts(KeyInfoX509Data keyInfoX509Data, CertUsageType certUsageType)
		{
			X509Certificate2Collection x509Certificate2Collection = new X509Certificate2Collection();
			ArrayList arrayList = ((certUsageType == CertUsageType.Decryption) ? new ArrayList() : null);
			if (keyInfoX509Data.Certificates != null)
			{
				foreach (object obj in keyInfoX509Data.Certificates)
				{
					X509Certificate2 x509Certificate = (X509Certificate2)obj;
					switch (certUsageType)
					{
					case CertUsageType.Verification:
						x509Certificate2Collection.Add(x509Certificate);
						break;
					case CertUsageType.Decryption:
						arrayList.Add(new X509IssuerSerial(x509Certificate.IssuerName.Name, x509Certificate.SerialNumber));
						break;
					}
				}
			}
			if (keyInfoX509Data.SubjectNames == null && keyInfoX509Data.IssuerSerials == null && keyInfoX509Data.SubjectKeyIds == null && arrayList == null)
			{
				return x509Certificate2Collection;
			}
			StorePermission storePermission = new StorePermission(StorePermissionFlags.OpenStore);
			storePermission.Assert();
			X509Store[] array = new X509Store[2];
			string text = ((certUsageType == CertUsageType.Verification) ? "AddressBook" : "My");
			array[0] = new X509Store(text, StoreLocation.CurrentUser);
			array[1] = new X509Store(text, StoreLocation.LocalMachine);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					X509Certificate2Collection x509Certificate2Collection2 = null;
					try
					{
						array[i].Open(OpenFlags.OpenExistingOnly);
						x509Certificate2Collection2 = array[i].Certificates;
						array[i].Close();
						if (keyInfoX509Data.SubjectNames != null)
						{
							foreach (object obj2 in keyInfoX509Data.SubjectNames)
							{
								string text2 = (string)obj2;
								x509Certificate2Collection2 = x509Certificate2Collection2.Find(X509FindType.FindBySubjectDistinguishedName, text2, false);
							}
						}
						if (keyInfoX509Data.IssuerSerials != null)
						{
							foreach (object obj3 in keyInfoX509Data.IssuerSerials)
							{
								X509IssuerSerial x509IssuerSerial = (X509IssuerSerial)obj3;
								x509Certificate2Collection2 = x509Certificate2Collection2.Find(X509FindType.FindByIssuerDistinguishedName, x509IssuerSerial.IssuerName, false);
								x509Certificate2Collection2 = x509Certificate2Collection2.Find(X509FindType.FindBySerialNumber, x509IssuerSerial.SerialNumber, false);
							}
						}
						if (keyInfoX509Data.SubjectKeyIds != null)
						{
							foreach (object obj4 in keyInfoX509Data.SubjectKeyIds)
							{
								byte[] array2 = (byte[])obj4;
								string text3 = X509Utils.EncodeHexString(array2);
								x509Certificate2Collection2 = x509Certificate2Collection2.Find(X509FindType.FindBySubjectKeyIdentifier, text3, false);
							}
						}
						if (arrayList != null)
						{
							foreach (object obj5 in arrayList)
							{
								X509IssuerSerial x509IssuerSerial2 = (X509IssuerSerial)obj5;
								x509Certificate2Collection2 = x509Certificate2Collection2.Find(X509FindType.FindByIssuerDistinguishedName, x509IssuerSerial2.IssuerName, false);
								x509Certificate2Collection2 = x509Certificate2Collection2.Find(X509FindType.FindBySerialNumber, x509IssuerSerial2.SerialNumber, false);
							}
						}
					}
					catch (CryptographicException)
					{
					}
					if (x509Certificate2Collection2 != null)
					{
						x509Certificate2Collection.AddRange(x509Certificate2Collection2);
					}
				}
			}
			return x509Certificate2Collection;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000D860 File Offset: 0x0000C860
		[SecuritySafeCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static int GetXmlDsigSearchDepth()
		{
			if (Utils.xmlDsigSearchDepth != null)
			{
				return Utils.xmlDsigSearchDepth.Value;
			}
			long netFxSecurityRegistryValue = Utils.GetNetFxSecurityRegistryValue("SignedDigitalSignatureXmlMaxDepth", 20L);
			Utils.xmlDsigSearchDepth = new int?((int)netFxSecurityRegistryValue);
			return Utils.xmlDsigSearchDepth.Value;
		}

		// Token: 0x040004C2 RID: 1218
		private static long? maxCharactersFromEntities = null;

		// Token: 0x040004C3 RID: 1219
		private static bool? s_allowAmbiguousReferenceTarget = null;

		// Token: 0x040004C4 RID: 1220
		private static bool? s_allowDetachedSignature = null;

		// Token: 0x040004C5 RID: 1221
		private static bool s_readRequireNCNameIdentifier = false;

		// Token: 0x040004C6 RID: 1222
		private static bool s_requireNCNameIdentifier = true;

		// Token: 0x040004C7 RID: 1223
		private static bool s_readMaxTransformsPerReference = false;

		// Token: 0x040004C8 RID: 1224
		private static long s_maxTransformsPerReference = 10L;

		// Token: 0x040004C9 RID: 1225
		private static bool s_readMaxReferencesPerSignedInfo = false;

		// Token: 0x040004CA RID: 1226
		private static long s_maxReferencesPerSignedInfo = 100L;

		// Token: 0x040004CB RID: 1227
		private static bool s_readAllowAdditionalSignatureNodes = false;

		// Token: 0x040004CC RID: 1228
		private static bool s_allowAdditionalSignatureNodes = false;

		// Token: 0x040004CD RID: 1229
		private static bool s_readSkipSignatureAttributeEnforcement = false;

		// Token: 0x040004CE RID: 1230
		private static bool s_skipSignatureAttributeEnforcement = false;

		// Token: 0x040004CF RID: 1231
		private static bool s_readAllowBareTypeReference = false;

		// Token: 0x040004D0 RID: 1232
		private static bool s_allowBareTypeReference = false;

		// Token: 0x040004D1 RID: 1233
		private static bool s_readLeaveCipherValueUnchecked = false;

		// Token: 0x040004D2 RID: 1234
		private static bool s_leaveCipherValueUnchecked = false;

		// Token: 0x040004D3 RID: 1235
		private static readonly char[] s_invalidChars = new char[] { ',', '`', '[', '*', '&' };

		// Token: 0x040004D4 RID: 1236
		private static int? xmlDsigSearchDepth = null;
	}
}
