using System;
using System.Collections;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000095 RID: 149
	internal abstract class AncestralNamespaceContextManager
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000EA98 File Offset: 0x0000DA98
		internal NamespaceFrame GetScopeAt(int i)
		{
			return (NamespaceFrame)this.m_ancestorStack[i];
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000EAAB File Offset: 0x0000DAAB
		internal NamespaceFrame GetCurrentScope()
		{
			return this.GetScopeAt(this.m_ancestorStack.Count - 1);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000EAC0 File Offset: 0x0000DAC0
		protected XmlAttribute GetNearestRenderedNamespaceWithMatchingPrefix(string nsPrefix, out int depth)
		{
			depth = -1;
			for (int i = this.m_ancestorStack.Count - 1; i >= 0; i--)
			{
				XmlAttribute rendered;
				if ((rendered = this.GetScopeAt(i).GetRendered(nsPrefix)) != null)
				{
					depth = i;
					return rendered;
				}
			}
			return null;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000EB04 File Offset: 0x0000DB04
		protected XmlAttribute GetNearestUnrenderedNamespaceWithMatchingPrefix(string nsPrefix, out int depth)
		{
			depth = -1;
			for (int i = this.m_ancestorStack.Count - 1; i >= 0; i--)
			{
				XmlAttribute unrendered;
				if ((unrendered = this.GetScopeAt(i).GetUnrendered(nsPrefix)) != null)
				{
					depth = i;
					return unrendered;
				}
			}
			return null;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000EB45 File Offset: 0x0000DB45
		internal void EnterElementContext()
		{
			this.m_ancestorStack.Add(new NamespaceFrame());
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000EB58 File Offset: 0x0000DB58
		internal void ExitElementContext()
		{
			this.m_ancestorStack.RemoveAt(this.m_ancestorStack.Count - 1);
		}

		// Token: 0x060002B3 RID: 691
		internal abstract void TrackNamespaceNode(XmlAttribute attr, SortedList nsListToRender, Hashtable nsLocallyDeclared);

		// Token: 0x060002B4 RID: 692
		internal abstract void TrackXmlNamespaceNode(XmlAttribute attr, SortedList nsListToRender, SortedList attrListToRender, Hashtable nsLocallyDeclared);

		// Token: 0x060002B5 RID: 693
		internal abstract void GetNamespacesToRender(XmlElement element, SortedList attrListToRender, SortedList nsListToRender, Hashtable nsLocallyDeclared);

		// Token: 0x060002B6 RID: 694 RVA: 0x0000EB74 File Offset: 0x0000DB74
		internal void LoadUnrenderedNamespaces(Hashtable nsLocallyDeclared)
		{
			object[] array = new object[nsLocallyDeclared.Count];
			nsLocallyDeclared.Values.CopyTo(array, 0);
			foreach (object obj in array)
			{
				this.AddUnrendered((XmlAttribute)obj);
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000EBBC File Offset: 0x0000DBBC
		internal void LoadRenderedNamespaces(SortedList nsRenderedList)
		{
			foreach (object obj in nsRenderedList.GetKeyList())
			{
				this.AddRendered((XmlAttribute)obj);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000EC18 File Offset: 0x0000DC18
		internal void AddRendered(XmlAttribute attr)
		{
			this.GetCurrentScope().AddRendered(attr);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000EC26 File Offset: 0x0000DC26
		internal void AddUnrendered(XmlAttribute attr)
		{
			this.GetCurrentScope().AddUnrendered(attr);
		}

		// Token: 0x040004E9 RID: 1257
		internal ArrayList m_ancestorStack = new ArrayList();
	}
}
