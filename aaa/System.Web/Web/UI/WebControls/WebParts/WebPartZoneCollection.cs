using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200074C RID: 1868
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartZoneCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06005ACA RID: 23242 RVA: 0x0016E630 File Offset: 0x0016D630
		public WebPartZoneCollection()
		{
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x0016E638 File Offset: 0x0016D638
		public WebPartZoneCollection(ICollection webPartZones)
		{
			if (webPartZones == null)
			{
				throw new ArgumentNullException("webPartZones");
			}
			foreach (object obj in webPartZones)
			{
				if (obj == null)
				{
					throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "webPartZones");
				}
				if (!(obj is WebPartZone))
				{
					throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "WebPartZone" }), "webPartZones");
				}
				base.InnerList.Add(obj);
			}
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x0016E6E8 File Offset: 0x0016D6E8
		internal int Add(WebPartZoneBase value)
		{
			return base.InnerList.Add(value);
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x0016E6F6 File Offset: 0x0016D6F6
		public bool Contains(WebPartZoneBase value)
		{
			return base.InnerList.Contains(value);
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x0016E704 File Offset: 0x0016D704
		public int IndexOf(WebPartZoneBase value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x17001789 RID: 6025
		public WebPartZoneBase this[int index]
		{
			get
			{
				return (WebPartZoneBase)base.InnerList[index];
			}
		}

		// Token: 0x1700178A RID: 6026
		public WebPartZoneBase this[string id]
		{
			get
			{
				WebPartZoneBase webPartZoneBase = null;
				foreach (object obj in base.InnerList)
				{
					WebPartZoneBase webPartZoneBase2 = (WebPartZoneBase)obj;
					if (string.Equals(webPartZoneBase2.ID, id, StringComparison.OrdinalIgnoreCase))
					{
						webPartZoneBase = webPartZoneBase2;
						break;
					}
				}
				return webPartZoneBase;
			}
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x0016E790 File Offset: 0x0016D790
		public void CopyTo(WebPartZoneBase[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}
	}
}
