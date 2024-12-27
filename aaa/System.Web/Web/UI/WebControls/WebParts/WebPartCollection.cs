using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200070F RID: 1807
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartCollection : ReadOnlyCollectionBase
	{
		// Token: 0x060057F0 RID: 22512 RVA: 0x001628FD File Offset: 0x001618FD
		public WebPartCollection()
		{
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x00162908 File Offset: 0x00161908
		public WebPartCollection(ICollection webParts)
		{
			if (webParts == null)
			{
				throw new ArgumentNullException("webParts");
			}
			foreach (object obj in webParts)
			{
				if (obj == null)
				{
					throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "webParts");
				}
				if (!(obj is WebPart))
				{
					throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "WebPart" }), "webParts");
				}
				base.InnerList.Add(obj);
			}
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x001629B8 File Offset: 0x001619B8
		internal int Add(WebPart value)
		{
			return base.InnerList.Add(value);
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x001629C6 File Offset: 0x001619C6
		public bool Contains(WebPart value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x001629D4 File Offset: 0x001619D4
		public int IndexOf(WebPart value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x170016B3 RID: 5811
		public WebPart this[int index]
		{
			get
			{
				return (WebPart)base.InnerList[index];
			}
		}

		// Token: 0x170016B4 RID: 5812
		public WebPart this[string id]
		{
			get
			{
				foreach (object obj in base.InnerList)
				{
					WebPart webPart = (WebPart)obj;
					if (string.Equals(webPart.ID, id, StringComparison.OrdinalIgnoreCase))
					{
						return webPart;
					}
					GenericWebPart genericWebPart = webPart as GenericWebPart;
					if (genericWebPart != null)
					{
						Control childControl = genericWebPart.ChildControl;
						if (childControl != null && string.Equals(childControl.ID, id, StringComparison.OrdinalIgnoreCase))
						{
							return genericWebPart;
						}
					}
					ProxyWebPart proxyWebPart = webPart as ProxyWebPart;
					if (proxyWebPart != null && (string.Equals(proxyWebPart.OriginalID, id, StringComparison.OrdinalIgnoreCase) || string.Equals(proxyWebPart.GenericWebPartID, id, StringComparison.OrdinalIgnoreCase)))
					{
						return proxyWebPart;
					}
				}
				return null;
			}
		}

		// Token: 0x060057F7 RID: 22519 RVA: 0x00162AC0 File Offset: 0x00161AC0
		public void CopyTo(WebPart[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
