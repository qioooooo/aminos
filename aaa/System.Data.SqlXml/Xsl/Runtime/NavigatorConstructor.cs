using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200007E RID: 126
	internal sealed class NavigatorConstructor
	{
		// Token: 0x06000721 RID: 1825 RVA: 0x00025864 File Offset: 0x00024864
		public XPathNavigator GetNavigator(XmlEventCache events, XmlNameTable nameTable)
		{
			if (this.cache == null)
			{
				XPathDocument xpathDocument = new XPathDocument(nameTable);
				XmlRawWriter xmlRawWriter = xpathDocument.LoadFromWriter(XPathDocument.LoadFlags.AtomizeNames | (events.HasRootNode ? XPathDocument.LoadFlags.None : XPathDocument.LoadFlags.Fragment), events.BaseUri);
				events.EventsToWriter(xmlRawWriter);
				xmlRawWriter.Close();
				this.cache = xpathDocument;
			}
			return ((XPathDocument)this.cache).CreateNavigator();
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000258C0 File Offset: 0x000248C0
		public XPathNavigator GetNavigator(string text, string baseUri, XmlNameTable nameTable)
		{
			if (this.cache == null)
			{
				XPathDocument xpathDocument = new XPathDocument(nameTable);
				XmlRawWriter xmlRawWriter = xpathDocument.LoadFromWriter(XPathDocument.LoadFlags.AtomizeNames, baseUri);
				xmlRawWriter.WriteString(text);
				xmlRawWriter.Close();
				this.cache = xpathDocument;
			}
			return ((XPathDocument)this.cache).CreateNavigator();
		}

		// Token: 0x0400049C RID: 1180
		private object cache;
	}
}
