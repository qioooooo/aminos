using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000038 RID: 56
	public class CompareRequest : DirectoryRequest
	{
		// Token: 0x06000112 RID: 274 RVA: 0x00005788 File Offset: 0x00004788
		public CompareRequest()
		{
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000579B File Offset: 0x0000479B
		public CompareRequest(string distinguishedName, string attributeName, string value)
		{
			this.CompareRequestHelper(distinguishedName, attributeName, value);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x000057B7 File Offset: 0x000047B7
		public CompareRequest(string distinguishedName, string attributeName, byte[] value)
		{
			this.CompareRequestHelper(distinguishedName, attributeName, value);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000057D3 File Offset: 0x000047D3
		public CompareRequest(string distinguishedName, string attributeName, Uri value)
		{
			this.CompareRequestHelper(distinguishedName, attributeName, value);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000057F0 File Offset: 0x000047F0
		public CompareRequest(string distinguishedName, DirectoryAttribute assertion)
		{
			if (assertion == null)
			{
				throw new ArgumentNullException("assertion");
			}
			if (assertion.Count != 1)
			{
				throw new ArgumentException(Res.GetString("WrongNumValuesCompare"));
			}
			this.CompareRequestHelper(distinguishedName, assertion.Name, assertion[0]);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005849 File Offset: 0x00004849
		private void CompareRequestHelper(string distinguishedName, string attributeName, object value)
		{
			if (attributeName == null)
			{
				throw new ArgumentNullException("attributeName");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.dn = distinguishedName;
			this.attribute.Name = attributeName;
			this.attribute.Add(value);
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005887 File Offset: 0x00004887
		// (set) Token: 0x06000119 RID: 281 RVA: 0x0000588F File Offset: 0x0000488F
		public string DistinguishedName
		{
			get
			{
				return this.dn;
			}
			set
			{
				this.dn = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00005898 File Offset: 0x00004898
		public DirectoryAttribute Assertion
		{
			get
			{
				return this.attribute;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000058A0 File Offset: 0x000048A0
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "compareRequest", true, this.dn);
			if (this.attribute.Count != 1)
			{
				throw new ArgumentException(Res.GetString("WrongNumValuesCompare"));
			}
			XmlElement xmlElement2 = this.attribute.ToXmlNode(doc, "assertion");
			xmlElement.AppendChild(xmlElement2);
			return xmlElement;
		}

		// Token: 0x04000109 RID: 265
		private string dn;

		// Token: 0x0400010A RID: 266
		private DirectoryAttribute attribute = new DirectoryAttribute();
	}
}
