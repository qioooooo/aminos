using System;

namespace System.Configuration
{
	// Token: 0x02000024 RID: 36
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public sealed class ConfigurationCollectionAttribute : Attribute
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x0000CAE4 File Offset: 0x0000BAE4
		public ConfigurationCollectionAttribute(Type itemType)
		{
			if (itemType == null)
			{
				throw new ArgumentNullException("itemType");
			}
			this._itemType = itemType;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000CB08 File Offset: 0x0000BB08
		public Type ItemType
		{
			get
			{
				return this._itemType;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000CB10 File Offset: 0x0000BB10
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x0000CB26 File Offset: 0x0000BB26
		public string AddItemName
		{
			get
			{
				if (this._addItemName == null)
				{
					return "add";
				}
				return this._addItemName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = null;
				}
				this._addItemName = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000CB3A File Offset: 0x0000BB3A
		// (set) Token: 0x060001DB RID: 475 RVA: 0x0000CB50 File Offset: 0x0000BB50
		public string RemoveItemName
		{
			get
			{
				if (this._removeItemName == null)
				{
					return "remove";
				}
				return this._removeItemName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = null;
				}
				this._removeItemName = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000CB64 File Offset: 0x0000BB64
		// (set) Token: 0x060001DD RID: 477 RVA: 0x0000CB7A File Offset: 0x0000BB7A
		public string ClearItemsName
		{
			get
			{
				if (this._clearItemsName == null)
				{
					return "clear";
				}
				return this._clearItemsName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					value = null;
				}
				this._clearItemsName = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000CB8E File Offset: 0x0000BB8E
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000CB96 File Offset: 0x0000BB96
		public ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return this._collectionType;
			}
			set
			{
				this._collectionType = value;
			}
		}

		// Token: 0x04000223 RID: 547
		private string _addItemName;

		// Token: 0x04000224 RID: 548
		private string _removeItemName;

		// Token: 0x04000225 RID: 549
		private string _clearItemsName;

		// Token: 0x04000226 RID: 550
		private Type _itemType;

		// Token: 0x04000227 RID: 551
		private ConfigurationElementCollectionType _collectionType = ConfigurationElementCollectionType.AddRemoveClearMap;
	}
}
