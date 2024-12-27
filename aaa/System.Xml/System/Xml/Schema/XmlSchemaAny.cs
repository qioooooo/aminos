using System;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000235 RID: 565
	public class XmlSchemaAny : XmlSchemaParticle
	{
		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06001AEE RID: 6894 RVA: 0x00080D41 File Offset: 0x0007FD41
		// (set) Token: 0x06001AEF RID: 6895 RVA: 0x00080D49 File Offset: 0x0007FD49
		[XmlAttribute("namespace")]
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x00080D52 File Offset: 0x0007FD52
		// (set) Token: 0x06001AF1 RID: 6897 RVA: 0x00080D5A File Offset: 0x0007FD5A
		[XmlAttribute("processContents")]
		[DefaultValue(XmlSchemaContentProcessing.None)]
		public XmlSchemaContentProcessing ProcessContents
		{
			get
			{
				return this.processContents;
			}
			set
			{
				this.processContents = value;
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06001AF2 RID: 6898 RVA: 0x00080D63 File Offset: 0x0007FD63
		[XmlIgnore]
		internal NamespaceList NamespaceList
		{
			get
			{
				return this.namespaceList;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x00080D6B File Offset: 0x0007FD6B
		[XmlIgnore]
		internal string ResolvedNamespace
		{
			get
			{
				if (this.ns == null || this.ns.Length == 0)
				{
					return "##any";
				}
				return this.ns;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x00080D8E File Offset: 0x0007FD8E
		[XmlIgnore]
		internal XmlSchemaContentProcessing ProcessContentsCorrect
		{
			get
			{
				if (this.processContents != XmlSchemaContentProcessing.None)
				{
					return this.processContents;
				}
				return XmlSchemaContentProcessing.Strict;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x00080DA0 File Offset: 0x0007FDA0
		internal override string NameString
		{
			get
			{
				switch (this.namespaceList.Type)
				{
				case NamespaceList.ListType.Any:
					return "##any:*";
				case NamespaceList.ListType.Other:
					return "##other:*";
				case NamespaceList.ListType.Set:
				{
					StringBuilder stringBuilder = new StringBuilder();
					int num = 1;
					foreach (object obj in this.namespaceList.Enumerate)
					{
						string text = (string)obj;
						stringBuilder.Append(text + ":*");
						if (num < this.namespaceList.Enumerate.Count)
						{
							stringBuilder.Append(" ");
						}
						num++;
					}
					return stringBuilder.ToString();
				}
				default:
					return string.Empty;
				}
			}
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x00080E78 File Offset: 0x0007FE78
		internal void BuildNamespaceList(string targetNamespace)
		{
			if (this.ns != null)
			{
				this.namespaceList = new NamespaceList(this.ns, targetNamespace);
				return;
			}
			this.namespaceList = new NamespaceList();
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x00080EA0 File Offset: 0x0007FEA0
		internal void BuildNamespaceListV1Compat(string targetNamespace)
		{
			if (this.ns != null)
			{
				this.namespaceList = new NamespaceListV1Compat(this.ns, targetNamespace);
				return;
			}
			this.namespaceList = new NamespaceList();
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x00080EC8 File Offset: 0x0007FEC8
		internal bool Allows(XmlQualifiedName qname)
		{
			return this.namespaceList.Allows(qname.Namespace);
		}

		// Token: 0x040010E3 RID: 4323
		private string ns;

		// Token: 0x040010E4 RID: 4324
		private XmlSchemaContentProcessing processContents;

		// Token: 0x040010E5 RID: 4325
		private NamespaceList namespaceList;
	}
}
