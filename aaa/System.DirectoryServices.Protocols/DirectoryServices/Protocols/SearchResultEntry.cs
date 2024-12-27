using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200004E RID: 78
	public class SearchResultEntry
	{
		// Token: 0x06000191 RID: 401 RVA: 0x000076F1 File Offset: 0x000066F1
		internal SearchResultEntry(XmlNode node)
		{
			this.dsmlNode = node;
			this.dsmlNS = NamespaceUtils.GetDsmlNamespaceManager();
			this.dsmlRequest = true;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000771D File Offset: 0x0000671D
		internal SearchResultEntry(string dn, SearchResultAttributeCollection attrs)
		{
			this.distinguishedName = dn;
			this.attributes = attrs;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000773E File Offset: 0x0000673E
		internal SearchResultEntry(string dn)
		{
			this.distinguishedName = dn;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00007758 File Offset: 0x00006758
		public string DistinguishedName
		{
			get
			{
				if (this.dsmlRequest && this.distinguishedName == null)
				{
					this.distinguishedName = this.DNHelper("@dsml:dn", "@dn");
				}
				return this.distinguishedName;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007786 File Offset: 0x00006786
		public SearchResultAttributeCollection Attributes
		{
			get
			{
				if (this.dsmlRequest && this.attributes.Count == 0)
				{
					this.attributes = this.AttributesHelper();
				}
				return this.attributes;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000077B0 File Offset: 0x000067B0
		public DirectoryControl[] Controls
		{
			get
			{
				if (this.dsmlRequest && this.resultControls == null)
				{
					this.resultControls = this.ControlsHelper();
				}
				if (this.resultControls == null)
				{
					return new DirectoryControl[0];
				}
				DirectoryControl[] array = new DirectoryControl[this.resultControls.Length];
				for (int i = 0; i < this.resultControls.Length; i++)
				{
					array[i] = new DirectoryControl(this.resultControls[i].Type, this.resultControls[i].GetValue(), this.resultControls[i].IsCritical, this.resultControls[i].ServerSide);
				}
				DirectoryControl.TransformControls(array);
				return array;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007850 File Offset: 0x00006850
		private string DNHelper(string primaryXPath, string secondaryXPath)
		{
			XmlAttribute xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode(primaryXPath, this.dsmlNS);
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
			xmlAttribute = (XmlAttribute)this.dsmlNode.SelectSingleNode(secondaryXPath, this.dsmlNS);
			if (xmlAttribute == null)
			{
				throw new DsmlInvalidDocumentException(Res.GetString("MissingSearchResultEntryDN"));
			}
			return xmlAttribute.Value;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000078B0 File Offset: 0x000068B0
		private SearchResultAttributeCollection AttributesHelper()
		{
			SearchResultAttributeCollection searchResultAttributeCollection = new SearchResultAttributeCollection();
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes("dsml:attr", this.dsmlNS);
			if (xmlNodeList.Count != 0)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					DirectoryAttribute directoryAttribute = new DirectoryAttribute((XmlElement)xmlNode);
					searchResultAttributeCollection.Add(directoryAttribute.Name, directoryAttribute);
				}
			}
			return searchResultAttributeCollection;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007944 File Offset: 0x00006944
		private DirectoryControl[] ControlsHelper()
		{
			XmlNodeList xmlNodeList = this.dsmlNode.SelectNodes("dsml:control", this.dsmlNS);
			if (xmlNodeList.Count == 0)
			{
				return new DirectoryControl[0];
			}
			DirectoryControl[] array = new DirectoryControl[xmlNodeList.Count];
			int num = 0;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				array[num] = new DirectoryControl((XmlElement)xmlNode);
				num++;
			}
			return array;
		}

		// Token: 0x0400016A RID: 362
		private XmlNode dsmlNode;

		// Token: 0x0400016B RID: 363
		private XmlNamespaceManager dsmlNS;

		// Token: 0x0400016C RID: 364
		private bool dsmlRequest;

		// Token: 0x0400016D RID: 365
		private string distinguishedName;

		// Token: 0x0400016E RID: 366
		private SearchResultAttributeCollection attributes = new SearchResultAttributeCollection();

		// Token: 0x0400016F RID: 367
		private DirectoryControl[] resultControls;
	}
}
