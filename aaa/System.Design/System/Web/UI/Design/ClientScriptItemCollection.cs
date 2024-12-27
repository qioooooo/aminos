using System;
using System.Collections;

namespace System.Web.UI.Design
{
	// Token: 0x02000322 RID: 802
	public sealed class ClientScriptItemCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06001E2D RID: 7725 RVA: 0x000ABB20 File Offset: 0x000AAB20
		public ClientScriptItemCollection(ClientScriptItem[] clientScriptItems)
		{
			if (clientScriptItems != null)
			{
				foreach (ClientScriptItem clientScriptItem in clientScriptItems)
				{
					base.InnerList.Add(clientScriptItem);
				}
			}
		}
	}
}
