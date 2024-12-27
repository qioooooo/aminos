using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000AD RID: 173
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlDsigC14NTransform : Transform
	{
		// Token: 0x060003DB RID: 987 RVA: 0x00013FFC File Offset: 0x00012FFC
		public XmlDsigC14NTransform()
		{
			base.Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001406C File Offset: 0x0001306C
		public XmlDsigC14NTransform(bool includeComments)
		{
			this._includeComments = includeComments;
			base.Algorithm = (includeComments ? "http://www.w3.org/TR/2001/REC-xml-c14n-20010315#WithComments" : "http://www.w3.org/TR/2001/REC-xml-c14n-20010315");
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003DD RID: 989 RVA: 0x000140EB File Offset: 0x000130EB
		public override Type[] InputTypes
		{
			get
			{
				return this._inputTypes;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003DE RID: 990 RVA: 0x000140F3 File Offset: 0x000130F3
		public override Type[] OutputTypes
		{
			get
			{
				return this._outputTypes;
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000140FB File Offset: 0x000130FB
		public override void LoadInnerXml(XmlNodeList nodeList)
		{
			if (!Utils.GetAllowAdditionalSignatureNodes() && nodeList != null && nodeList.Count > 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014120 File Offset: 0x00013120
		protected override XmlNodeList GetInnerXml()
		{
			return null;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00014124 File Offset: 0x00013124
		public override void LoadInput(object obj)
		{
			XmlResolver xmlResolver = (base.ResolverSet ? this.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), base.BaseURI));
			if (obj is Stream)
			{
				this._cXml = new CanonicalXml((Stream)obj, this._includeComments, xmlResolver, base.BaseURI);
				return;
			}
			if (obj is XmlDocument)
			{
				this._cXml = new CanonicalXml((XmlDocument)obj, xmlResolver, this._includeComments);
				return;
			}
			if (obj is XmlNodeList)
			{
				this._cXml = new CanonicalXml((XmlNodeList)obj, xmlResolver, this._includeComments);
				return;
			}
			throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "obj");
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000141CF File Offset: 0x000131CF
		public override object GetOutput()
		{
			return new MemoryStream(this._cXml.GetBytes());
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000141E4 File Offset: 0x000131E4
		public override object GetOutput(Type type)
		{
			if (type != typeof(Stream) && !type.IsSubclassOf(typeof(Stream)))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"), "type");
			}
			return new MemoryStream(this._cXml.GetBytes());
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00014235 File Offset: 0x00013235
		[ComVisible(false)]
		public override byte[] GetDigestedOutput(HashAlgorithm hash)
		{
			return this._cXml.GetDigestedBytes(hash);
		}

		// Token: 0x0400055E RID: 1374
		private Type[] _inputTypes = new Type[]
		{
			typeof(Stream),
			typeof(XmlDocument),
			typeof(XmlNodeList)
		};

		// Token: 0x0400055F RID: 1375
		private Type[] _outputTypes = new Type[] { typeof(Stream) };

		// Token: 0x04000560 RID: 1376
		private CanonicalXml _cXml;

		// Token: 0x04000561 RID: 1377
		private bool _includeComments;
	}
}
