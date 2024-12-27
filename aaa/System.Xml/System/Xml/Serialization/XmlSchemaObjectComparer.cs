using System;
using System.Collections;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002E4 RID: 740
	internal class XmlSchemaObjectComparer : IComparer
	{
		// Token: 0x0600228C RID: 8844 RVA: 0x000A1599 File Offset: 0x000A0599
		public int Compare(object o1, object o2)
		{
			return this.comparer.Compare(XmlSchemaObjectComparer.NameOf((XmlSchemaObject)o1), XmlSchemaObjectComparer.NameOf((XmlSchemaObject)o2));
		}

		// Token: 0x0600228D RID: 8845 RVA: 0x000A15BC File Offset: 0x000A05BC
		internal static XmlQualifiedName NameOf(XmlSchemaObject o)
		{
			if (o is XmlSchemaAttribute)
			{
				return ((XmlSchemaAttribute)o).QualifiedName;
			}
			if (o is XmlSchemaAttributeGroup)
			{
				return ((XmlSchemaAttributeGroup)o).QualifiedName;
			}
			if (o is XmlSchemaComplexType)
			{
				return ((XmlSchemaComplexType)o).QualifiedName;
			}
			if (o is XmlSchemaSimpleType)
			{
				return ((XmlSchemaSimpleType)o).QualifiedName;
			}
			if (o is XmlSchemaElement)
			{
				return ((XmlSchemaElement)o).QualifiedName;
			}
			if (o is XmlSchemaGroup)
			{
				return ((XmlSchemaGroup)o).QualifiedName;
			}
			if (o is XmlSchemaGroupRef)
			{
				return ((XmlSchemaGroupRef)o).RefName;
			}
			if (o is XmlSchemaNotation)
			{
				return ((XmlSchemaNotation)o).QualifiedName;
			}
			if (o is XmlSchemaSequence)
			{
				XmlSchemaSequence xmlSchemaSequence = (XmlSchemaSequence)o;
				if (xmlSchemaSequence.Items.Count == 0)
				{
					return new XmlQualifiedName(".sequence", XmlSchemaObjectComparer.Namespace(o));
				}
				return XmlSchemaObjectComparer.NameOf(xmlSchemaSequence.Items[0]);
			}
			else if (o is XmlSchemaAll)
			{
				XmlSchemaAll xmlSchemaAll = (XmlSchemaAll)o;
				if (xmlSchemaAll.Items.Count == 0)
				{
					return new XmlQualifiedName(".all", XmlSchemaObjectComparer.Namespace(o));
				}
				return XmlSchemaObjectComparer.NameOf(xmlSchemaAll.Items);
			}
			else if (o is XmlSchemaChoice)
			{
				XmlSchemaChoice xmlSchemaChoice = (XmlSchemaChoice)o;
				if (xmlSchemaChoice.Items.Count == 0)
				{
					return new XmlQualifiedName(".choice", XmlSchemaObjectComparer.Namespace(o));
				}
				return XmlSchemaObjectComparer.NameOf(xmlSchemaChoice.Items);
			}
			else
			{
				if (o is XmlSchemaAny)
				{
					return new XmlQualifiedName("*", SchemaObjectWriter.ToString(((XmlSchemaAny)o).NamespaceList));
				}
				if (o is XmlSchemaIdentityConstraint)
				{
					return ((XmlSchemaIdentityConstraint)o).QualifiedName;
				}
				return new XmlQualifiedName("?", XmlSchemaObjectComparer.Namespace(o));
			}
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000A1764 File Offset: 0x000A0764
		internal static XmlQualifiedName NameOf(XmlSchemaObjectCollection items)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < items.Count; i++)
			{
				arrayList.Add(XmlSchemaObjectComparer.NameOf(items[i]));
			}
			arrayList.Sort(new QNameComparer());
			return (XmlQualifiedName)arrayList[0];
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000A17B2 File Offset: 0x000A07B2
		internal static string Namespace(XmlSchemaObject o)
		{
			while (o != null && !(o is XmlSchema))
			{
				o = o.Parent;
			}
			if (o != null)
			{
				return ((XmlSchema)o).TargetNamespace;
			}
			return "";
		}

		// Token: 0x040014CD RID: 5325
		private QNameComparer comparer = new QNameComparer();
	}
}
