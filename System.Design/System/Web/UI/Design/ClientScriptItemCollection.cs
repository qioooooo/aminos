using System;
using System.Collections;

namespace System.Web.UI.Design
{
	public sealed class ClientScriptItemCollection : ReadOnlyCollectionBase
	{
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
