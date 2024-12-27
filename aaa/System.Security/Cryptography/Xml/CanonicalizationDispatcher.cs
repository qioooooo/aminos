using System;
using System.Text;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x02000086 RID: 134
	internal class CanonicalizationDispatcher
	{
		// Token: 0x0600024F RID: 591 RVA: 0x0000D95B File Offset: 0x0000C95B
		private CanonicalizationDispatcher()
		{
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D963 File Offset: 0x0000C963
		public static void Write(XmlNode node, StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (node is ICanonicalizableNode)
			{
				((ICanonicalizableNode)node).Write(strBuilder, docPos, anc);
				return;
			}
			CanonicalizationDispatcher.WriteGenericNode(node, strBuilder, docPos, anc);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D988 File Offset: 0x0000C988
		public static void WriteGenericNode(XmlNode node, StringBuilder strBuilder, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			XmlNodeList childNodes = node.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				CanonicalizationDispatcher.Write(xmlNode, strBuilder, docPos, anc);
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D9F4 File Offset: 0x0000C9F4
		public static void WriteHash(XmlNode node, HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (node is ICanonicalizableNode)
			{
				((ICanonicalizableNode)node).WriteHash(hash, docPos, anc);
				return;
			}
			CanonicalizationDispatcher.WriteHashGenericNode(node, hash, docPos, anc);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000DA18 File Offset: 0x0000CA18
		public static void WriteHashGenericNode(XmlNode node, HashAlgorithm hash, DocPosition docPos, AncestralNamespaceContextManager anc)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			XmlNodeList childNodes = node.ChildNodes;
			foreach (object obj in childNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				CanonicalizationDispatcher.WriteHash(xmlNode, hash, docPos, anc);
			}
		}
	}
}
