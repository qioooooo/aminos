using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000036 RID: 54
	public class AddRequest : DirectoryRequest
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00005512 File Offset: 0x00004512
		public AddRequest()
		{
			this.attributeList = new DirectoryAttributeCollection();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005528 File Offset: 0x00004528
		public AddRequest(string distinguishedName, params DirectoryAttribute[] attributes)
			: this()
		{
			this.dn = distinguishedName;
			if (attributes != null)
			{
				for (int i = 0; i < attributes.Length; i++)
				{
					this.attributeList.Add(attributes[i]);
				}
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005564 File Offset: 0x00004564
		public AddRequest(string distinguishedName, string objectClass)
			: this()
		{
			if (objectClass == null)
			{
				throw new ArgumentNullException("objectClass");
			}
			this.dn = distinguishedName;
			DirectoryAttribute directoryAttribute = new DirectoryAttribute();
			directoryAttribute.Name = "objectClass";
			directoryAttribute.Add(objectClass);
			this.attributeList.Add(directoryAttribute);
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000055B2 File Offset: 0x000045B2
		// (set) Token: 0x06000108 RID: 264 RVA: 0x000055BA File Offset: 0x000045BA
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000109 RID: 265 RVA: 0x000055C3 File Offset: 0x000045C3
		public DirectoryAttributeCollection Attributes
		{
			get
			{
				return this.attributeList;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x000055CC File Offset: 0x000045CC
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "addRequest", true, this.dn);
			if (this.attributeList != null)
			{
				foreach (object obj in this.attributeList)
				{
					DirectoryAttribute directoryAttribute = (DirectoryAttribute)obj;
					XmlElement xmlElement2 = directoryAttribute.ToXmlNode(doc, "attr");
					xmlElement.AppendChild(xmlElement2);
				}
			}
			return xmlElement;
		}

		// Token: 0x04000105 RID: 261
		private string dn;

		// Token: 0x04000106 RID: 262
		private DirectoryAttributeCollection attributeList;
	}
}
