using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000043 RID: 67
	public class ExtendedResponse : DirectoryResponse
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00006800 File Offset: 0x00005800
		internal ExtendedResponse(XmlNode node)
			: base(node)
		{
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006809 File Offset: 0x00005809
		internal ExtendedResponse(string dn, DirectoryControl[] controls, ResultCode result, string message, Uri[] referral)
			: base(dn, controls, result, message, referral)
		{
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00006818 File Offset: 0x00005818
		public string ResponseName
		{
			get
			{
				if (this.dsmlRequest && this.name == null)
				{
					XmlElement xmlElement = (XmlElement)this.dsmlNode.SelectSingleNode("dsml:responseName", this.dsmlNS);
					if (xmlElement != null)
					{
						this.name = xmlElement.InnerText;
					}
				}
				return this.name;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00006868 File Offset: 0x00005868
		public byte[] ResponseValue
		{
			get
			{
				if (this.dsmlRequest && this.value == null)
				{
					XmlElement xmlElement = (XmlElement)this.dsmlNode.SelectSingleNode("dsml:response", this.dsmlNS);
					if (xmlElement != null)
					{
						string innerText = xmlElement.InnerText;
						try
						{
							this.value = Convert.FromBase64String(innerText);
						}
						catch (FormatException)
						{
							throw new DsmlInvalidDocumentException(Res.GetString("BadBase64Value"));
						}
					}
				}
				if (this.value == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.value.Length];
				for (int i = 0; i < this.value.Length; i++)
				{
					array[i] = this.value[i];
				}
				return array;
			}
		}

		// Token: 0x04000123 RID: 291
		internal string name;

		// Token: 0x04000124 RID: 292
		internal byte[] value;
	}
}
