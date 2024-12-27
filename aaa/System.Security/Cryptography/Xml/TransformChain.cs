using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000AB RID: 171
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class TransformChain
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x0001387C File Offset: 0x0001287C
		public TransformChain()
		{
			this.m_transforms = new ArrayList();
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0001388F File Offset: 0x0001288F
		public void Add(Transform transform)
		{
			if (transform != null)
			{
				this.m_transforms.Add(transform);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x000138A1 File Offset: 0x000128A1
		public IEnumerator GetEnumerator()
		{
			return this.m_transforms.GetEnumerator();
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000138AE File Offset: 0x000128AE
		public int Count
		{
			get
			{
				return this.m_transforms.Count;
			}
		}

		// Token: 0x170000B8 RID: 184
		public Transform this[int index]
		{
			get
			{
				if (index >= this.m_transforms.Count)
				{
					throw new ArgumentException(SecurityResources.GetResourceString("ArgumentOutOfRange_Index"), "index");
				}
				return (Transform)this.m_transforms[index];
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x000138F4 File Offset: 0x000128F4
		internal Stream TransformToOctetStream(object inputObject, Type inputType, XmlResolver resolver, string baseUri)
		{
			object obj = inputObject;
			foreach (object obj2 in this.m_transforms)
			{
				Transform transform = (Transform)obj2;
				if (obj == null || transform.AcceptsType(obj.GetType()))
				{
					transform.Resolver = resolver;
					transform.BaseURI = baseUri;
					transform.LoadInput(obj);
					obj = transform.GetOutput();
				}
				else if (obj is Stream)
				{
					if (!transform.AcceptsType(typeof(XmlDocument)))
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"));
					}
					Stream stream = obj as Stream;
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.PreserveWhitespace = true;
					XmlReader xmlReader = Utils.PreProcessStreamInput(stream, resolver, baseUri);
					xmlDocument.Load(xmlReader);
					transform.LoadInput(xmlDocument);
					stream.Close();
					obj = transform.GetOutput();
				}
				else if (obj is XmlNodeList)
				{
					if (!transform.AcceptsType(typeof(Stream)))
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"));
					}
					CanonicalXml canonicalXml = new CanonicalXml((XmlNodeList)obj, resolver, false);
					MemoryStream memoryStream = new MemoryStream(canonicalXml.GetBytes());
					transform.LoadInput(memoryStream);
					obj = transform.GetOutput();
					memoryStream.Close();
				}
				else
				{
					if (!(obj is XmlDocument))
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"));
					}
					if (!transform.AcceptsType(typeof(Stream)))
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"));
					}
					CanonicalXml canonicalXml2 = new CanonicalXml((XmlDocument)obj, resolver);
					MemoryStream memoryStream2 = new MemoryStream(canonicalXml2.GetBytes());
					transform.LoadInput(memoryStream2);
					obj = transform.GetOutput();
					memoryStream2.Close();
				}
			}
			if (obj is Stream)
			{
				return obj as Stream;
			}
			if (obj is XmlNodeList)
			{
				CanonicalXml canonicalXml3 = new CanonicalXml((XmlNodeList)obj, resolver, false);
				return new MemoryStream(canonicalXml3.GetBytes());
			}
			if (obj is XmlDocument)
			{
				CanonicalXml canonicalXml4 = new CanonicalXml((XmlDocument)obj, resolver);
				return new MemoryStream(canonicalXml4.GetBytes());
			}
			throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"));
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00013B38 File Offset: 0x00012B38
		internal Stream TransformToOctetStream(Stream input, XmlResolver resolver, string baseUri)
		{
			return this.TransformToOctetStream(input, typeof(Stream), resolver, baseUri);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00013B4D File Offset: 0x00012B4D
		internal Stream TransformToOctetStream(XmlDocument document, XmlResolver resolver, string baseUri)
		{
			return this.TransformToOctetStream(document, typeof(XmlDocument), resolver, baseUri);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00013B64 File Offset: 0x00012B64
		internal XmlElement GetXml(XmlDocument document, string ns)
		{
			XmlElement xmlElement = document.CreateElement("Transforms", ns);
			foreach (object obj in this.m_transforms)
			{
				Transform transform = (Transform)obj;
				if (transform != null)
				{
					XmlElement xml = transform.GetXml(document);
					if (xml != null)
					{
						xmlElement.AppendChild(xml);
					}
				}
			}
			return xmlElement;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00013BE0 File Offset: 0x00012BE0
		internal void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(value.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			XmlNodeList xmlNodeList = value.SelectNodes("ds:Transform", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Transforms");
			}
			this.m_transforms.Clear();
			for (int i = 0; i < xmlNodeList.Count; i++)
			{
				XmlElement xmlElement = (XmlElement)xmlNodeList.Item(i);
				string attribute = Utils.GetAttribute(xmlElement, "Algorithm", "http://www.w3.org/2000/09/xmldsig#");
				Transform transform = Utils.CreateFromName<Transform>(attribute);
				if (transform == null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
				}
				transform.LoadInnerXml(xmlElement.ChildNodes);
				this.m_transforms.Add(transform);
			}
		}

		// Token: 0x04000555 RID: 1365
		private ArrayList m_transforms;
	}
}
