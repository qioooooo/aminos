using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003B1 RID: 945
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ComplexPropertyEntry : BuilderPropertyEntry
	{
		// Token: 0x06002E35 RID: 11829 RVA: 0x000CF549 File Offset: 0x000CE549
		internal ComplexPropertyEntry()
		{
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000CF551 File Offset: 0x000CE551
		internal ComplexPropertyEntry(bool isCollectionItem)
		{
			this._isCollectionItem = isCollectionItem;
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06002E37 RID: 11831 RVA: 0x000CF560 File Offset: 0x000CE560
		public bool IsCollectionItem
		{
			get
			{
				return this._isCollectionItem;
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06002E38 RID: 11832 RVA: 0x000CF568 File Offset: 0x000CE568
		// (set) Token: 0x06002E39 RID: 11833 RVA: 0x000CF570 File Offset: 0x000CE570
		public bool ReadOnly
		{
			get
			{
				return this._readOnly;
			}
			set
			{
				this._readOnly = value;
			}
		}

		// Token: 0x04002176 RID: 8566
		private bool _readOnly;

		// Token: 0x04002177 RID: 8567
		private bool _isCollectionItem;
	}
}
