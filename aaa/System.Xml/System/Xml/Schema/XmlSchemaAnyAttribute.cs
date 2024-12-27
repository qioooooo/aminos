using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000236 RID: 566
	public class XmlSchemaAnyAttribute : XmlSchemaAnnotated
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001AFA RID: 6906 RVA: 0x00080EE3 File Offset: 0x0007FEE3
		// (set) Token: 0x06001AFB RID: 6907 RVA: 0x00080EEB File Offset: 0x0007FEEB
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

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001AFC RID: 6908 RVA: 0x00080EF4 File Offset: 0x0007FEF4
		// (set) Token: 0x06001AFD RID: 6909 RVA: 0x00080EFC File Offset: 0x0007FEFC
		[DefaultValue(XmlSchemaContentProcessing.None)]
		[XmlAttribute("processContents")]
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

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x00080F05 File Offset: 0x0007FF05
		[XmlIgnore]
		internal NamespaceList NamespaceList
		{
			get
			{
				return this.namespaceList;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x00080F0D File Offset: 0x0007FF0D
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

		// Token: 0x06001B00 RID: 6912 RVA: 0x00080F1F File Offset: 0x0007FF1F
		internal void BuildNamespaceList(string targetNamespace)
		{
			if (this.ns != null)
			{
				this.namespaceList = new NamespaceList(this.ns, targetNamespace);
				return;
			}
			this.namespaceList = new NamespaceList();
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x00080F47 File Offset: 0x0007FF47
		internal void BuildNamespaceListV1Compat(string targetNamespace)
		{
			if (this.ns != null)
			{
				this.namespaceList = new NamespaceListV1Compat(this.ns, targetNamespace);
				return;
			}
			this.namespaceList = new NamespaceList();
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x00080F6F File Offset: 0x0007FF6F
		internal bool Allows(XmlQualifiedName qname)
		{
			return this.namespaceList.Allows(qname.Namespace);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x00080F82 File Offset: 0x0007FF82
		internal static bool IsSubset(XmlSchemaAnyAttribute sub, XmlSchemaAnyAttribute super)
		{
			return NamespaceList.IsSubset(sub.NamespaceList, super.NamespaceList);
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x00080F98 File Offset: 0x0007FF98
		internal static XmlSchemaAnyAttribute Intersection(XmlSchemaAnyAttribute o1, XmlSchemaAnyAttribute o2, bool v1Compat)
		{
			NamespaceList namespaceList = NamespaceList.Intersection(o1.NamespaceList, o2.NamespaceList, v1Compat);
			if (namespaceList != null)
			{
				return new XmlSchemaAnyAttribute
				{
					namespaceList = namespaceList,
					ProcessContents = o1.ProcessContents,
					Annotation = o1.Annotation
				};
			}
			return null;
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x00080FE4 File Offset: 0x0007FFE4
		internal static XmlSchemaAnyAttribute Union(XmlSchemaAnyAttribute o1, XmlSchemaAnyAttribute o2, bool v1Compat)
		{
			NamespaceList namespaceList = NamespaceList.Union(o1.NamespaceList, o2.NamespaceList, v1Compat);
			if (namespaceList != null)
			{
				return new XmlSchemaAnyAttribute
				{
					namespaceList = namespaceList,
					processContents = o1.processContents,
					Annotation = o1.Annotation
				};
			}
			return null;
		}

		// Token: 0x040010E6 RID: 4326
		private string ns;

		// Token: 0x040010E7 RID: 4327
		private XmlSchemaContentProcessing processContents;

		// Token: 0x040010E8 RID: 4328
		private NamespaceList namespaceList;
	}
}
