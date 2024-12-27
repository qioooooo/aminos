using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200039D RID: 925
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class PropertyEntry
	{
		// Token: 0x06002D15 RID: 11541 RVA: 0x000CA743 File Offset: 0x000C9743
		internal PropertyEntry()
		{
		}

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000CA74B File Offset: 0x000C974B
		// (set) Token: 0x06002D17 RID: 11543 RVA: 0x000CA753 File Offset: 0x000C9753
		public string Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x000CA75C File Offset: 0x000C975C
		// (set) Token: 0x06002D19 RID: 11545 RVA: 0x000CA764 File Offset: 0x000C9764
		internal int Order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x000CA76D File Offset: 0x000C976D
		// (set) Token: 0x06002D1B RID: 11547 RVA: 0x000CA775 File Offset: 0x000C9775
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06002D1C RID: 11548 RVA: 0x000CA77E File Offset: 0x000C977E
		// (set) Token: 0x06002D1D RID: 11549 RVA: 0x000CA786 File Offset: 0x000C9786
		public PropertyInfo PropertyInfo
		{
			get
			{
				return this._propertyInfo;
			}
			set
			{
				this._propertyInfo = value;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x000CA78F File Offset: 0x000C978F
		// (set) Token: 0x06002D1F RID: 11551 RVA: 0x000CA797 File Offset: 0x000C9797
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06002D20 RID: 11552 RVA: 0x000CA7A0 File Offset: 0x000C97A0
		// (set) Token: 0x06002D21 RID: 11553 RVA: 0x000CA7A8 File Offset: 0x000C97A8
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000CA7B1 File Offset: 0x000C97B1
		public Type DeclaringType
		{
			get
			{
				if (this._propertyInfo == null)
				{
					return null;
				}
				return this._propertyInfo.DeclaringType;
			}
		}

		// Token: 0x040020D8 RID: 8408
		private string _filter;

		// Token: 0x040020D9 RID: 8409
		private PropertyInfo _propertyInfo;

		// Token: 0x040020DA RID: 8410
		private string _name;

		// Token: 0x040020DB RID: 8411
		private Type _type;

		// Token: 0x040020DC RID: 8412
		private int _index;

		// Token: 0x040020DD RID: 8413
		private int _order;
	}
}
