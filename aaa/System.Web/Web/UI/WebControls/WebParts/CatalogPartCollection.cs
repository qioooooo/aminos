using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006A9 RID: 1705
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CatalogPartCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06005347 RID: 21319 RVA: 0x00152112 File Offset: 0x00151112
		public CatalogPartCollection()
		{
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x0015211A File Offset: 0x0015111A
		public CatalogPartCollection(ICollection catalogParts)
		{
			this.Initialize(null, catalogParts);
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x0015212A File Offset: 0x0015112A
		public CatalogPartCollection(CatalogPartCollection existingCatalogParts, ICollection catalogParts)
		{
			this.Initialize(existingCatalogParts, catalogParts);
		}

		// Token: 0x1700152C RID: 5420
		public CatalogPart this[int index]
		{
			get
			{
				return (CatalogPart)base.InnerList[index];
			}
		}

		// Token: 0x1700152D RID: 5421
		public CatalogPart this[string id]
		{
			get
			{
				foreach (object obj in base.InnerList)
				{
					CatalogPart catalogPart = (CatalogPart)obj;
					if (string.Equals(catalogPart.ID, id, StringComparison.OrdinalIgnoreCase))
					{
						return catalogPart;
					}
				}
				return null;
			}
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x001521B8 File Offset: 0x001511B8
		internal int Add(CatalogPart value)
		{
			return base.InnerList.Add(value);
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x001521C6 File Offset: 0x001511C6
		public bool Contains(CatalogPart catalogPart)
		{
			return base.InnerList.Contains(catalogPart);
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x001521D4 File Offset: 0x001511D4
		public void CopyTo(CatalogPart[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x001521E3 File Offset: 0x001511E3
		public int IndexOf(CatalogPart catalogPart)
		{
			return base.InnerList.IndexOf(catalogPart);
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001521F4 File Offset: 0x001511F4
		private void Initialize(CatalogPartCollection existingCatalogParts, ICollection catalogParts)
		{
			if (existingCatalogParts != null)
			{
				foreach (object obj in existingCatalogParts)
				{
					CatalogPart catalogPart = (CatalogPart)obj;
					base.InnerList.Add(catalogPart);
				}
			}
			if (catalogParts != null)
			{
				foreach (object obj2 in catalogParts)
				{
					if (obj2 == null)
					{
						throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "catalogParts");
					}
					if (!(obj2 is CatalogPart))
					{
						throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "CatalogPart" }), "catalogParts");
					}
					base.InnerList.Add(obj2);
				}
			}
		}

		// Token: 0x04002E5B RID: 11867
		public static readonly CatalogPartCollection Empty = new CatalogPartCollection();
	}
}
