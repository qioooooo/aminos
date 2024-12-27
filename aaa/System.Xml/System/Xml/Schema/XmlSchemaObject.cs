using System;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200022C RID: 556
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class XmlSchemaObject
	{
		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x0007FD37 File Offset: 0x0007ED37
		// (set) Token: 0x06001A69 RID: 6761 RVA: 0x0007FD3F File Offset: 0x0007ED3F
		[XmlIgnore]
		public int LineNumber
		{
			get
			{
				return this.lineNum;
			}
			set
			{
				this.lineNum = value;
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x0007FD48 File Offset: 0x0007ED48
		// (set) Token: 0x06001A6B RID: 6763 RVA: 0x0007FD50 File Offset: 0x0007ED50
		[XmlIgnore]
		public int LinePosition
		{
			get
			{
				return this.linePos;
			}
			set
			{
				this.linePos = value;
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06001A6C RID: 6764 RVA: 0x0007FD59 File Offset: 0x0007ED59
		// (set) Token: 0x06001A6D RID: 6765 RVA: 0x0007FD61 File Offset: 0x0007ED61
		[XmlIgnore]
		public string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
			set
			{
				this.sourceUri = value;
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0007FD6A File Offset: 0x0007ED6A
		// (set) Token: 0x06001A6F RID: 6767 RVA: 0x0007FD72 File Offset: 0x0007ED72
		[XmlIgnore]
		public XmlSchemaObject Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001A70 RID: 6768 RVA: 0x0007FD7B File Offset: 0x0007ED7B
		// (set) Token: 0x06001A71 RID: 6769 RVA: 0x0007FD96 File Offset: 0x0007ED96
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces Namespaces
		{
			get
			{
				if (this.namespaces == null)
				{
					this.namespaces = new XmlSerializerNamespaces();
				}
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0007FD9F File Offset: 0x0007ED9F
		internal virtual void OnAdd(XmlSchemaObjectCollection container, object item)
		{
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0007FDA1 File Offset: 0x0007EDA1
		internal virtual void OnRemove(XmlSchemaObjectCollection container, object item)
		{
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x0007FDA3 File Offset: 0x0007EDA3
		internal virtual void OnClear(XmlSchemaObjectCollection container)
		{
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06001A75 RID: 6773 RVA: 0x0007FDA5 File Offset: 0x0007EDA5
		// (set) Token: 0x06001A76 RID: 6774 RVA: 0x0007FDA8 File Offset: 0x0007EDA8
		[XmlIgnore]
		internal virtual string IdAttribute
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x0007FDAA File Offset: 0x0007EDAA
		internal virtual void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
		{
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x0007FDAC File Offset: 0x0007EDAC
		internal virtual void AddAnnotation(XmlSchemaAnnotation annotation)
		{
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x0007FDAE File Offset: 0x0007EDAE
		// (set) Token: 0x06001A7A RID: 6778 RVA: 0x0007FDB1 File Offset: 0x0007EDB1
		[XmlIgnore]
		internal virtual string NameAttribute
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06001A7B RID: 6779 RVA: 0x0007FDB3 File Offset: 0x0007EDB3
		// (set) Token: 0x06001A7C RID: 6780 RVA: 0x0007FDBB File Offset: 0x0007EDBB
		[XmlIgnore]
		internal bool IsProcessing
		{
			get
			{
				return this.isProcessing;
			}
			set
			{
				this.isProcessing = value;
			}
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x0007FDC4 File Offset: 0x0007EDC4
		internal virtual XmlSchemaObject Clone()
		{
			return (XmlSchemaObject)base.MemberwiseClone();
		}

		// Token: 0x040010AD RID: 4269
		private int lineNum;

		// Token: 0x040010AE RID: 4270
		private int linePos;

		// Token: 0x040010AF RID: 4271
		private string sourceUri;

		// Token: 0x040010B0 RID: 4272
		private XmlSerializerNamespaces namespaces;

		// Token: 0x040010B1 RID: 4273
		private XmlSchemaObject parent;

		// Token: 0x040010B2 RID: 4274
		private bool isProcessing;
	}
}
