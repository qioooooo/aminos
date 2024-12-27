using System;

namespace System.Xml
{
	// Token: 0x020000DD RID: 221
	internal class XmlElementListListener
	{
		// Token: 0x06000D91 RID: 3473 RVA: 0x0003C1F8 File Offset: 0x0003B1F8
		internal XmlElementListListener(XmlDocument doc, XmlElementList elemList)
		{
			this.doc = doc;
			this.elemList = new WeakReference(elemList);
			this.nodeChangeHandler = new XmlNodeChangedEventHandler(this.OnListChanged);
			doc.NodeInserted += this.nodeChangeHandler;
			doc.NodeRemoved += this.nodeChangeHandler;
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0003C248 File Offset: 0x0003B248
		private void OnListChanged(object sender, XmlNodeChangedEventArgs args)
		{
			XmlElementList xmlElementList = (XmlElementList)this.elemList.Target;
			if (xmlElementList != null)
			{
				xmlElementList.ConcurrencyCheck(args);
				return;
			}
			this.doc.NodeInserted -= this.nodeChangeHandler;
			this.doc.NodeRemoved -= this.nodeChangeHandler;
		}

		// Token: 0x04000957 RID: 2391
		private WeakReference elemList;

		// Token: 0x04000958 RID: 2392
		private XmlDocument doc;

		// Token: 0x04000959 RID: 2393
		private XmlNodeChangedEventHandler nodeChangeHandler;
	}
}
