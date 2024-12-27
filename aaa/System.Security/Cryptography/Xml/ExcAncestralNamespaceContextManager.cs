using System;
using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000099 RID: 153
	internal class ExcAncestralNamespaceContextManager : AncestralNamespaceContextManager
	{
		// Token: 0x060002CF RID: 719 RVA: 0x0000F48B File Offset: 0x0000E48B
		internal ExcAncestralNamespaceContextManager(string inclusiveNamespacesPrefixList)
		{
			this.m_inclusivePrefixSet = Utils.TokenizePrefixListString(inclusiveNamespacesPrefixList);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000F4A0 File Offset: 0x0000E4A0
		private bool HasNonRedundantInclusivePrefix(XmlAttribute attr)
		{
			string namespacePrefix = Utils.GetNamespacePrefix(attr);
			int num;
			return this.m_inclusivePrefixSet.ContainsKey(namespacePrefix) && Utils.IsNonRedundantNamespaceDecl(attr, base.GetNearestRenderedNamespaceWithMatchingPrefix(namespacePrefix, out num));
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000F4D4 File Offset: 0x0000E4D4
		private void GatherNamespaceToRender(string nsPrefix, SortedList nsListToRender, Hashtable nsLocallyDeclared)
		{
			foreach (object obj in nsListToRender.GetKeyList())
			{
				if (Utils.HasNamespacePrefix((XmlAttribute)obj, nsPrefix))
				{
					return;
				}
			}
			XmlAttribute xmlAttribute = (XmlAttribute)nsLocallyDeclared[nsPrefix];
			int num;
			XmlAttribute nearestRenderedNamespaceWithMatchingPrefix = base.GetNearestRenderedNamespaceWithMatchingPrefix(nsPrefix, out num);
			if (xmlAttribute != null)
			{
				if (Utils.IsNonRedundantNamespaceDecl(xmlAttribute, nearestRenderedNamespaceWithMatchingPrefix))
				{
					nsLocallyDeclared.Remove(nsPrefix);
					nsListToRender.Add(xmlAttribute, null);
					return;
				}
			}
			else
			{
				int num2;
				XmlAttribute nearestUnrenderedNamespaceWithMatchingPrefix = base.GetNearestUnrenderedNamespaceWithMatchingPrefix(nsPrefix, out num2);
				if (nearestUnrenderedNamespaceWithMatchingPrefix != null && num2 > num && Utils.IsNonRedundantNamespaceDecl(nearestUnrenderedNamespaceWithMatchingPrefix, nearestRenderedNamespaceWithMatchingPrefix))
				{
					nsListToRender.Add(nearestUnrenderedNamespaceWithMatchingPrefix, null);
				}
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F594 File Offset: 0x0000E594
		internal override void GetNamespacesToRender(XmlElement element, SortedList attrListToRender, SortedList nsListToRender, Hashtable nsLocallyDeclared)
		{
			this.GatherNamespaceToRender(element.Prefix, nsListToRender, nsLocallyDeclared);
			foreach (object obj in attrListToRender.GetKeyList())
			{
				string prefix = ((XmlAttribute)obj).Prefix;
				if (prefix.Length > 0)
				{
					this.GatherNamespaceToRender(prefix, nsListToRender, nsLocallyDeclared);
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F610 File Offset: 0x0000E610
		internal override void TrackNamespaceNode(XmlAttribute attr, SortedList nsListToRender, Hashtable nsLocallyDeclared)
		{
			if (!Utils.IsXmlPrefixDefinitionNode(attr))
			{
				if (this.HasNonRedundantInclusivePrefix(attr))
				{
					nsListToRender.Add(attr, null);
					return;
				}
				nsLocallyDeclared.Add(Utils.GetNamespacePrefix(attr), attr);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000F639 File Offset: 0x0000E639
		internal override void TrackXmlNamespaceNode(XmlAttribute attr, SortedList nsListToRender, SortedList attrListToRender, Hashtable nsLocallyDeclared)
		{
			attrListToRender.Add(attr, null);
		}

		// Token: 0x040004EE RID: 1262
		private Hashtable m_inclusivePrefixSet;
	}
}
