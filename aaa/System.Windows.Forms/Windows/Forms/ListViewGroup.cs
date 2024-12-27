using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200048B RID: 1163
	[DefaultProperty("Header")]
	[DesignTimeVisible(false)]
	[TypeConverter(typeof(ListViewGroupConverter))]
	[ToolboxItem(false)]
	[Serializable]
	public sealed class ListViewGroup : ISerializable
	{
		// Token: 0x0600456A RID: 17770 RVA: 0x000FC89C File Offset: 0x000FB89C
		public ListViewGroup()
			: this(SR.GetString("ListViewGroupDefaultHeader", new object[] { ListViewGroup.nextHeader++ }))
		{
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x000FC8D6 File Offset: 0x000FB8D6
		private ListViewGroup(SerializationInfo info, StreamingContext context)
			: this()
		{
			this.Deserialize(info, context);
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x000FC8E6 File Offset: 0x000FB8E6
		public ListViewGroup(string key, string headerText)
			: this()
		{
			this.name = key;
			this.header = headerText;
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x000FC8FC File Offset: 0x000FB8FC
		public ListViewGroup(string header)
		{
			this.header = header;
			this.id = ListViewGroup.nextID++;
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x000FC91E File Offset: 0x000FB91E
		public ListViewGroup(string header, HorizontalAlignment headerAlignment)
			: this(header)
		{
			this.headerAlignment = headerAlignment;
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x000FC92E File Offset: 0x000FB92E
		// (set) Token: 0x06004570 RID: 17776 RVA: 0x000FC944 File Offset: 0x000FB944
		[SRCategory("CatAppearance")]
		public string Header
		{
			get
			{
				if (this.header != null)
				{
					return this.header;
				}
				return "";
			}
			set
			{
				if (this.header != value)
				{
					this.header = value;
					if (this.listView != null)
					{
						this.listView.RecreateHandleInternal();
					}
				}
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06004571 RID: 17777 RVA: 0x000FC96E File Offset: 0x000FB96E
		// (set) Token: 0x06004572 RID: 17778 RVA: 0x000FC976 File Offset: 0x000FB976
		[DefaultValue(HorizontalAlignment.Left)]
		[SRCategory("CatAppearance")]
		public HorizontalAlignment HeaderAlignment
		{
			get
			{
				return this.headerAlignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(HorizontalAlignment));
				}
				if (this.headerAlignment != value)
				{
					this.headerAlignment = value;
					this.UpdateListView();
				}
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004573 RID: 17779 RVA: 0x000FC9B4 File Offset: 0x000FB9B4
		internal int ID
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06004574 RID: 17780 RVA: 0x000FC9BC File Offset: 0x000FB9BC
		[Browsable(false)]
		public ListView.ListViewItemCollection Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = new ListView.ListViewItemCollection(new ListViewGroupItemCollection(this));
				}
				return this.items;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06004575 RID: 17781 RVA: 0x000FC9DD File Offset: 0x000FB9DD
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ListView ListView
		{
			get
			{
				return this.listView;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x06004576 RID: 17782 RVA: 0x000FC9E5 File Offset: 0x000FB9E5
		// (set) Token: 0x06004577 RID: 17783 RVA: 0x000FC9ED File Offset: 0x000FB9ED
		internal ListView ListViewInternal
		{
			get
			{
				return this.listView;
			}
			set
			{
				if (this.listView != value)
				{
					this.listView = value;
				}
			}
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x000FC9FF File Offset: 0x000FB9FF
		// (set) Token: 0x06004579 RID: 17785 RVA: 0x000FCA07 File Offset: 0x000FBA07
		[SRCategory("CatBehavior")]
		[SRDescription("ListViewGroupNameDescr")]
		[Browsable(true)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x000FCA10 File Offset: 0x000FBA10
		// (set) Token: 0x0600457B RID: 17787 RVA: 0x000FCA18 File Offset: 0x000FBA18
		[Bindable(true)]
		[Localizable(false)]
		[SRCategory("CatData")]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x000FCA24 File Offset: 0x000FBA24
		private void Deserialize(SerializationInfo info, StreamingContext context)
		{
			int num = 0;
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "Header")
				{
					this.Header = (string)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "HeaderAlignment")
				{
					this.HeaderAlignment = (HorizontalAlignment)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "Tag")
				{
					this.Tag = serializationEntry.Value;
				}
				else if (serializationEntry.Name == "ItemsCount")
				{
					num = (int)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "Name")
				{
					this.Name = (string)serializationEntry.Value;
				}
			}
			if (num > 0)
			{
				ListViewItem[] array = new ListViewItem[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = (ListViewItem)info.GetValue("Item" + i, typeof(ListViewItem));
				}
				this.Items.AddRange(array);
			}
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x000FCB59 File Offset: 0x000FBB59
		public override string ToString()
		{
			return this.Header;
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x000FCB61 File Offset: 0x000FBB61
		private void UpdateListView()
		{
			if (this.listView != null && this.listView.IsHandleCreated)
			{
				this.listView.UpdateGroupNative(this);
			}
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x000FCB84 File Offset: 0x000FBB84
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Header", this.Header);
			info.AddValue("HeaderAlignment", this.HeaderAlignment);
			info.AddValue("Tag", this.Tag);
			if (!string.IsNullOrEmpty(this.Name))
			{
				info.AddValue("Name", this.Name);
			}
			if (this.items != null && this.items.Count > 0)
			{
				info.AddValue("ItemsCount", this.Items.Count);
				for (int i = 0; i < this.Items.Count; i++)
				{
					info.AddValue("Item" + i.ToString(CultureInfo.InvariantCulture), this.Items[i], typeof(ListViewItem));
				}
			}
		}

		// Token: 0x0400215D RID: 8541
		private ListView listView;

		// Token: 0x0400215E RID: 8542
		private int id;

		// Token: 0x0400215F RID: 8543
		private string header;

		// Token: 0x04002160 RID: 8544
		private HorizontalAlignment headerAlignment;

		// Token: 0x04002161 RID: 8545
		private ListView.ListViewItemCollection items;

		// Token: 0x04002162 RID: 8546
		private static int nextID;

		// Token: 0x04002163 RID: 8547
		private static int nextHeader = 1;

		// Token: 0x04002164 RID: 8548
		private object userData;

		// Token: 0x04002165 RID: 8549
		private string name;
	}
}
