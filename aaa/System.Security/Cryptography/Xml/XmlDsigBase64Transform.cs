using System;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B1 RID: 177
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlDsigBase64Transform : Transform
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x000145E0 File Offset: 0x000135E0
		public XmlDsigBase64Transform()
		{
			base.Algorithm = "http://www.w3.org/2000/09/xmldsig#base64";
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0001464E File Offset: 0x0001364E
		public override Type[] InputTypes
		{
			get
			{
				return this._inputTypes;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00014656 File Offset: 0x00013656
		public override Type[] OutputTypes
		{
			get
			{
				return this._outputTypes;
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001465E File Offset: 0x0001365E
		public override void LoadInnerXml(XmlNodeList nodeList)
		{
			if (!Utils.GetAllowAdditionalSignatureNodes() && nodeList != null && nodeList.Count > 0)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00014683 File Offset: 0x00013683
		protected override XmlNodeList GetInnerXml()
		{
			return null;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00014688 File Offset: 0x00013688
		public override void LoadInput(object obj)
		{
			if (obj is Stream)
			{
				this.LoadStreamInput((Stream)obj);
				return;
			}
			if (obj is XmlNodeList)
			{
				this.LoadXmlNodeListInput((XmlNodeList)obj);
				return;
			}
			if (obj is XmlDocument)
			{
				this.LoadXmlNodeListInput(((XmlDocument)obj).SelectNodes("//."));
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x000146E0 File Offset: 0x000136E0
		private void LoadStreamInput(Stream inputStream)
		{
			if (inputStream == null)
			{
				throw new ArgumentException("obj");
			}
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = new byte[1024];
			int num;
			do
			{
				num = inputStream.Read(array, 0, 1024);
				if (num > 0)
				{
					int i = 0;
					while (i < num && !char.IsWhiteSpace((char)array[i]))
					{
						i++;
					}
					int num2 = i;
					for (i++; i < num; i++)
					{
						if (!char.IsWhiteSpace((char)array[i]))
						{
							array[num2] = array[i];
							num2++;
						}
					}
					memoryStream.Write(array, 0, num2);
				}
			}
			while (num > 0);
			memoryStream.Position = 0L;
			this._cs = new CryptoStream(memoryStream, new FromBase64Transform(), CryptoStreamMode.Read);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014790 File Offset: 0x00013790
		private void LoadXmlNodeListInput(XmlNodeList nodeList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object obj in nodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("self::text()");
				if (xmlNode2 != null)
				{
					stringBuilder.Append(xmlNode2.OuterXml);
				}
			}
			UTF8Encoding utf8Encoding = new UTF8Encoding(false);
			byte[] bytes = utf8Encoding.GetBytes(stringBuilder.ToString());
			int i = 0;
			while (i < bytes.Length && !char.IsWhiteSpace((char)bytes[i]))
			{
				i++;
			}
			int num = i;
			for (i++; i < bytes.Length; i++)
			{
				if (!char.IsWhiteSpace((char)bytes[i]))
				{
					bytes[num] = bytes[i];
					num++;
				}
			}
			MemoryStream memoryStream = new MemoryStream(bytes, 0, num);
			this._cs = new CryptoStream(memoryStream, new FromBase64Transform(), CryptoStreamMode.Read);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00014894 File Offset: 0x00013894
		public override object GetOutput()
		{
			return this._cs;
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0001489C File Offset: 0x0001389C
		public override object GetOutput(Type type)
		{
			if (type != typeof(Stream) && !type.IsSubclassOf(typeof(Stream)))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"), "type");
			}
			return this._cs;
		}

		// Token: 0x04000567 RID: 1383
		private Type[] _inputTypes = new Type[]
		{
			typeof(Stream),
			typeof(XmlNodeList),
			typeof(XmlDocument)
		};

		// Token: 0x04000568 RID: 1384
		private Type[] _outputTypes = new Type[] { typeof(Stream) };

		// Token: 0x04000569 RID: 1385
		private CryptoStream _cs;
	}
}
