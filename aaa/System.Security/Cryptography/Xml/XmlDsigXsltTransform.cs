using System;
using System.IO;
using System.Security.Permissions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B3 RID: 179
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlDsigXsltTransform : Transform
	{
		// Token: 0x0600040B RID: 1035 RVA: 0x00014E1C File Offset: 0x00013E1C
		public XmlDsigXsltTransform()
		{
			base.Algorithm = "http://www.w3.org/TR/1999/REC-xslt-19991116";
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00014E8C File Offset: 0x00013E8C
		public XmlDsigXsltTransform(bool includeComments)
		{
			this._includeComments = includeComments;
			base.Algorithm = "http://www.w3.org/TR/1999/REC-xslt-19991116";
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00014F01 File Offset: 0x00013F01
		public override Type[] InputTypes
		{
			get
			{
				return this._inputTypes;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00014F09 File Offset: 0x00013F09
		public override Type[] OutputTypes
		{
			get
			{
				return this._outputTypes;
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00014F14 File Offset: 0x00013F14
		public override void LoadInnerXml(XmlNodeList nodeList)
		{
			if (nodeList == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
			}
			XmlElement xmlElement = null;
			int num = 0;
			foreach (object obj in nodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!(xmlNode is XmlWhitespace))
				{
					if (xmlNode is XmlElement)
					{
						if (num != 0)
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
						}
						xmlElement = xmlNode as XmlElement;
						num++;
					}
					else
					{
						num++;
					}
				}
			}
			if (num != 1 || xmlElement == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
			}
			this._xslNodes = nodeList;
			this._xslFragment = xmlElement.OuterXml.Trim(null);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00014FE0 File Offset: 0x00013FE0
		protected override XmlNodeList GetInnerXml()
		{
			return this._xslNodes;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00014FE8 File Offset: 0x00013FE8
		public override void LoadInput(object obj)
		{
			if (this._inputStream != null)
			{
				this._inputStream.Close();
			}
			this._inputStream = new MemoryStream();
			if (obj is Stream)
			{
				this._inputStream = (Stream)obj;
				return;
			}
			if (!(obj is XmlNodeList))
			{
				if (obj is XmlDocument)
				{
					CanonicalXml canonicalXml = new CanonicalXml((XmlDocument)obj, null, this._includeComments);
					byte[] bytes = canonicalXml.GetBytes();
					if (bytes == null)
					{
						return;
					}
					this._inputStream.Write(bytes, 0, bytes.Length);
					this._inputStream.Flush();
					this._inputStream.Position = 0L;
				}
				return;
			}
			CanonicalXml canonicalXml2 = new CanonicalXml((XmlNodeList)obj, null, this._includeComments);
			byte[] bytes2 = canonicalXml2.GetBytes();
			if (bytes2 == null)
			{
				return;
			}
			this._inputStream.Write(bytes2, 0, bytes2.Length);
			this._inputStream.Flush();
			this._inputStream.Position = 0L;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000150C8 File Offset: 0x000140C8
		public override object GetOutput()
		{
			XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.XmlResolver = null;
			xmlReaderSettings.MaxCharactersFromEntities = Utils.GetMaxCharactersFromEntities();
			object obj;
			using (StringReader stringReader = new StringReader(this._xslFragment))
			{
				XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings, null);
				xslCompiledTransform.Load(xmlReader, XsltSettings.Default, null);
				XmlReader xmlReader2 = XmlReader.Create(this._inputStream, xmlReaderSettings, base.BaseURI);
				XPathDocument xpathDocument = new XPathDocument(xmlReader2, XmlSpace.Preserve);
				MemoryStream memoryStream = new MemoryStream();
				XmlWriter xmlWriter = new XmlTextWriter(memoryStream, null);
				xslCompiledTransform.Transform(xpathDocument, null, xmlWriter);
				memoryStream.Position = 0L;
				obj = memoryStream;
			}
			return obj;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001517C File Offset: 0x0001417C
		public override object GetOutput(Type type)
		{
			if (type != typeof(Stream) && !type.IsSubclassOf(typeof(Stream)))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"), "type");
			}
			return (Stream)this.GetOutput();
		}

		// Token: 0x0400056F RID: 1391
		private Type[] _inputTypes = new Type[]
		{
			typeof(Stream),
			typeof(XmlDocument),
			typeof(XmlNodeList)
		};

		// Token: 0x04000570 RID: 1392
		private Type[] _outputTypes = new Type[] { typeof(Stream) };

		// Token: 0x04000571 RID: 1393
		private XmlNodeList _xslNodes;

		// Token: 0x04000572 RID: 1394
		private string _xslFragment;

		// Token: 0x04000573 RID: 1395
		private Stream _inputStream;

		// Token: 0x04000574 RID: 1396
		private bool _includeComments;
	}
}
