using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000037 RID: 55
	public class ModifyRequest : DirectoryRequest
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00005654 File Offset: 0x00004654
		public ModifyRequest()
		{
			this.attributeModificationList = new DirectoryAttributeModificationCollection();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005667 File Offset: 0x00004667
		public ModifyRequest(string distinguishedName, params DirectoryAttributeModification[] modifications)
			: this()
		{
			this.dn = distinguishedName;
			this.attributeModificationList.AddRange(modifications);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005684 File Offset: 0x00004684
		public ModifyRequest(string distinguishedName, DirectoryAttributeOperation operation, string attributeName, params object[] values)
			: this()
		{
			this.dn = distinguishedName;
			if (attributeName == null)
			{
				throw new ArgumentNullException("attributeName");
			}
			DirectoryAttributeModification directoryAttributeModification = new DirectoryAttributeModification();
			directoryAttributeModification.Operation = operation;
			directoryAttributeModification.Name = attributeName;
			if (values != null)
			{
				for (int i = 0; i < values.Length; i++)
				{
					directoryAttributeModification.Add(values[i]);
				}
			}
			this.attributeModificationList.Add(directoryAttributeModification);
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000056EB File Offset: 0x000046EB
		// (set) Token: 0x0600010F RID: 271 RVA: 0x000056F3 File Offset: 0x000046F3
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000056FC File Offset: 0x000046FC
		public DirectoryAttributeModificationCollection Modifications
		{
			get
			{
				return this.attributeModificationList;
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00005704 File Offset: 0x00004704
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			XmlElement xmlElement = base.CreateRequestElement(doc, "modifyRequest", true, this.dn);
			if (this.attributeModificationList != null)
			{
				foreach (object obj in this.attributeModificationList)
				{
					DirectoryAttributeModification directoryAttributeModification = (DirectoryAttributeModification)obj;
					XmlElement xmlElement2 = directoryAttributeModification.ToXmlNode(doc);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			return xmlElement;
		}

		// Token: 0x04000107 RID: 263
		private string dn;

		// Token: 0x04000108 RID: 264
		private DirectoryAttributeModificationCollection attributeModificationList;
	}
}
