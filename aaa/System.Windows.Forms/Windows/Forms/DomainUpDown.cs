using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003BF RID: 959
	[DefaultProperty("Items")]
	[DefaultBindingProperty("SelectedItem")]
	[ComVisible(true)]
	[DefaultEvent("SelectedItemChanged")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[SRDescription("DescriptionDomainUpDown")]
	public class DomainUpDown : UpDownBase
	{
		// Token: 0x06003A5C RID: 14940 RVA: 0x000D48C7 File Offset: 0x000D38C7
		public DomainUpDown()
		{
			base.SetState2(2048, true);
			this.Text = string.Empty;
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003A5D RID: 14941 RVA: 0x000D4903 File Offset: 0x000D3903
		[Localizable(true)]
		[SRCategory("CatData")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[SRDescription("DomainUpDownItemsDescr")]
		public DomainUpDown.DomainUpDownItemCollection Items
		{
			get
			{
				if (this.domainItems == null)
				{
					this.domainItems = new DomainUpDown.DomainUpDownItemCollection(this);
				}
				return this.domainItems;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x000D491F File Offset: 0x000D391F
		// (set) Token: 0x06003A5F RID: 14943 RVA: 0x000D4927 File Offset: 0x000D3927
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		// Token: 0x140001ED RID: 493
		// (add) Token: 0x06003A60 RID: 14944 RVA: 0x000D4930 File Offset: 0x000D3930
		// (remove) Token: 0x06003A61 RID: 14945 RVA: 0x000D4939 File Offset: 0x000D3939
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x000D4942 File Offset: 0x000D3942
		// (set) Token: 0x06003A63 RID: 14947 RVA: 0x000D4954 File Offset: 0x000D3954
		[DefaultValue(-1)]
		[SRDescription("DomainUpDownSelectedIndexDescr")]
		[Browsable(false)]
		[SRCategory("CatAppearance")]
		public int SelectedIndex
		{
			get
			{
				if (base.UserEdit)
				{
					return -1;
				}
				return this.domainIndex;
			}
			set
			{
				if (value < -1 || value >= this.Items.Count)
				{
					throw new ArgumentOutOfRangeException("SelectedIndex", SR.GetString("InvalidArgument", new object[]
					{
						"SelectedIndex",
						value.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (value != this.SelectedIndex)
				{
					this.SelectIndex(value);
				}
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x000D49B8 File Offset: 0x000D39B8
		// (set) Token: 0x06003A65 RID: 14949 RVA: 0x000D49E0 File Offset: 0x000D39E0
		[SRDescription("DomainUpDownSelectedItemDescr")]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				int selectedIndex = this.SelectedIndex;
				if (selectedIndex != -1)
				{
					return this.Items[selectedIndex];
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.SelectedIndex = -1;
					return;
				}
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (value != null && value.Equals(this.Items[i]))
					{
						this.SelectedIndex = i;
						return;
					}
				}
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x06003A66 RID: 14950 RVA: 0x000D4A2D File Offset: 0x000D3A2D
		// (set) Token: 0x06003A67 RID: 14951 RVA: 0x000D4A35 File Offset: 0x000D3A35
		[SRDescription("DomainUpDownSortedDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public bool Sorted
		{
			get
			{
				return this.sorted;
			}
			set
			{
				this.sorted = value;
				if (this.sorted)
				{
					this.SortDomainItems();
				}
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x000D4A4C File Offset: 0x000D3A4C
		// (set) Token: 0x06003A69 RID: 14953 RVA: 0x000D4A54 File Offset: 0x000D3A54
		[SRDescription("DomainUpDownWrapDescr")]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[DefaultValue(false)]
		public bool Wrap
		{
			get
			{
				return this.wrap;
			}
			set
			{
				this.wrap = value;
			}
		}

		// Token: 0x140001EE RID: 494
		// (add) Token: 0x06003A6A RID: 14954 RVA: 0x000D4A5D File Offset: 0x000D3A5D
		// (remove) Token: 0x06003A6B RID: 14955 RVA: 0x000D4A76 File Offset: 0x000D3A76
		[SRCategory("CatBehavior")]
		[SRDescription("DomainUpDownOnSelectedItemChangedDescr")]
		public event EventHandler SelectedItemChanged
		{
			add
			{
				this.onSelectedItemChanged = (EventHandler)Delegate.Combine(this.onSelectedItemChanged, value);
			}
			remove
			{
				this.onSelectedItemChanged = (EventHandler)Delegate.Remove(this.onSelectedItemChanged, value);
			}
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x000D4A8F File Offset: 0x000D3A8F
		protected override AccessibleObject CreateAccessibilityInstance()
		{
			return new DomainUpDown.DomainUpDownAccessibleObject(this);
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x000D4A98 File Offset: 0x000D3A98
		public override void DownButton()
		{
			if (this.domainItems == null)
			{
				return;
			}
			if (this.domainItems.Count <= 0)
			{
				return;
			}
			int num = -1;
			if (base.UserEdit)
			{
				num = this.MatchIndex(this.Text, false, this.domainIndex);
			}
			if (num != -1)
			{
				this.SelectIndex(num);
				return;
			}
			if (this.domainIndex < this.domainItems.Count - 1)
			{
				this.SelectIndex(this.domainIndex + 1);
				return;
			}
			if (this.Wrap)
			{
				this.SelectIndex(0);
			}
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x000D4B1A File Offset: 0x000D3B1A
		internal int MatchIndex(string text, bool complete)
		{
			return this.MatchIndex(text, complete, this.domainIndex);
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x000D4B2C File Offset: 0x000D3B2C
		internal int MatchIndex(string text, bool complete, int startPosition)
		{
			if (this.domainItems == null)
			{
				return -1;
			}
			if (text.Length < 1)
			{
				return -1;
			}
			if (this.domainItems.Count <= 0)
			{
				return -1;
			}
			if (startPosition < 0)
			{
				startPosition = this.domainItems.Count - 1;
			}
			if (startPosition >= this.domainItems.Count)
			{
				startPosition = 0;
			}
			int num = startPosition;
			int num2 = -1;
			if (!complete)
			{
				text = text.ToUpper(CultureInfo.InvariantCulture);
			}
			bool flag;
			do
			{
				if (complete)
				{
					flag = this.Items[num].ToString().Equals(text);
				}
				else
				{
					flag = this.Items[num].ToString().ToUpper(CultureInfo.InvariantCulture).StartsWith(text);
				}
				if (flag)
				{
					num2 = num;
				}
				num++;
				if (num >= this.domainItems.Count)
				{
					num = 0;
				}
			}
			while (!flag && num != startPosition);
			return num2;
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x000D4BF8 File Offset: 0x000D3BF8
		protected override void OnChanged(object source, EventArgs e)
		{
			this.OnSelectedItemChanged(source, e);
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000D4C04 File Offset: 0x000D3C04
		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
		{
			if (base.ReadOnly)
			{
				char[] array = new char[] { e.KeyChar };
				UnicodeCategory unicodeCategory = char.GetUnicodeCategory(array[0]);
				if (unicodeCategory == UnicodeCategory.LetterNumber || unicodeCategory == UnicodeCategory.LowercaseLetter || unicodeCategory == UnicodeCategory.DecimalDigitNumber || unicodeCategory == UnicodeCategory.MathSymbol || unicodeCategory == UnicodeCategory.OtherLetter || unicodeCategory == UnicodeCategory.OtherNumber || unicodeCategory == UnicodeCategory.UppercaseLetter)
				{
					int num = this.MatchIndex(new string(array), false, this.domainIndex + 1);
					if (num != -1)
					{
						this.SelectIndex(num);
					}
					e.Handled = true;
				}
			}
			base.OnTextBoxKeyPress(source, e);
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000D4C82 File Offset: 0x000D3C82
		protected void OnSelectedItemChanged(object source, EventArgs e)
		{
			if (this.onSelectedItemChanged != null)
			{
				this.onSelectedItemChanged(this, e);
			}
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x000D4C9C File Offset: 0x000D3C9C
		private void SelectIndex(int index)
		{
			if (this.domainItems == null || index < -1 || index >= this.domainItems.Count)
			{
				index = -1;
				return;
			}
			this.domainIndex = index;
			if (this.domainIndex >= 0)
			{
				this.stringValue = this.domainItems[this.domainIndex].ToString();
				base.UserEdit = false;
				this.UpdateEditText();
				return;
			}
			base.UserEdit = true;
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000D4D08 File Offset: 0x000D3D08
		private void SortDomainItems()
		{
			if (this.inSort)
			{
				return;
			}
			this.inSort = true;
			try
			{
				if (this.sorted)
				{
					if (this.domainItems != null)
					{
						ArrayList.Adapter(this.domainItems).Sort(new DomainUpDown.DomainUpDownItemCompare());
						if (!base.UserEdit)
						{
							int num = this.MatchIndex(this.stringValue, true);
							if (num != -1)
							{
								this.SelectIndex(num);
							}
						}
					}
				}
			}
			finally
			{
				this.inSort = false;
			}
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x000D4D88 File Offset: 0x000D3D88
		public override string ToString()
		{
			string text = base.ToString();
			if (this.Items != null)
			{
				text = text + ", Items.Count: " + this.Items.Count.ToString(CultureInfo.CurrentCulture);
				text = text + ", SelectedIndex: " + this.SelectedIndex.ToString(CultureInfo.CurrentCulture);
			}
			return text;
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000D4DE8 File Offset: 0x000D3DE8
		public override void UpButton()
		{
			if (this.domainItems == null)
			{
				return;
			}
			if (this.domainItems.Count <= 0)
			{
				return;
			}
			if (this.domainIndex == -1)
			{
				return;
			}
			int num = -1;
			if (base.UserEdit)
			{
				num = this.MatchIndex(this.Text, false, this.domainIndex);
			}
			if (num != -1)
			{
				this.SelectIndex(num);
				return;
			}
			if (this.domainIndex > 0)
			{
				this.SelectIndex(this.domainIndex - 1);
				return;
			}
			if (this.Wrap)
			{
				this.SelectIndex(this.domainItems.Count - 1);
			}
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x000D4E74 File Offset: 0x000D3E74
		protected override void UpdateEditText()
		{
			base.UserEdit = false;
			base.ChangingText = true;
			this.Text = this.stringValue;
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x000D4E90 File Offset: 0x000D3E90
		internal override Size GetPreferredSizeCore(Size proposedConstraints)
		{
			int preferredHeight = base.PreferredHeight;
			int num = LayoutUtils.OldGetLargestStringSizeInCollection(this.Font, this.Items).Width;
			num = base.SizeFromClientSize(num, preferredHeight).Width + this.upDownButtons.Width;
			return new Size(num, preferredHeight) + this.Padding.Size;
		}

		// Token: 0x04001D1C RID: 7452
		private static readonly string DefaultValue = "";

		// Token: 0x04001D1D RID: 7453
		private static readonly bool DefaultWrap = false;

		// Token: 0x04001D1E RID: 7454
		private DomainUpDown.DomainUpDownItemCollection domainItems;

		// Token: 0x04001D1F RID: 7455
		private string stringValue = DomainUpDown.DefaultValue;

		// Token: 0x04001D20 RID: 7456
		private int domainIndex = -1;

		// Token: 0x04001D21 RID: 7457
		private bool sorted;

		// Token: 0x04001D22 RID: 7458
		private bool wrap = DomainUpDown.DefaultWrap;

		// Token: 0x04001D23 RID: 7459
		private EventHandler onSelectedItemChanged;

		// Token: 0x04001D24 RID: 7460
		private bool inSort;

		// Token: 0x020003C0 RID: 960
		public class DomainUpDownItemCollection : ArrayList
		{
			// Token: 0x06003A7A RID: 14970 RVA: 0x000D4F08 File Offset: 0x000D3F08
			internal DomainUpDownItemCollection(DomainUpDown owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000B01 RID: 2817
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public override object this[int index]
			{
				get
				{
					return base[index];
				}
				set
				{
					base[index] = value;
					if (this.owner.SelectedIndex == index)
					{
						this.owner.SelectIndex(index);
					}
					if (this.owner.Sorted)
					{
						this.owner.SortDomainItems();
					}
				}
			}

			// Token: 0x06003A7D RID: 14973 RVA: 0x000D4F5C File Offset: 0x000D3F5C
			public override int Add(object item)
			{
				int num = base.Add(item);
				if (this.owner.Sorted)
				{
					this.owner.SortDomainItems();
				}
				return num;
			}

			// Token: 0x06003A7E RID: 14974 RVA: 0x000D4F8C File Offset: 0x000D3F8C
			public override void Remove(object item)
			{
				int num = this.IndexOf(item);
				if (num == -1)
				{
					throw new ArgumentOutOfRangeException("item", SR.GetString("InvalidArgument", new object[]
					{
						"item",
						item.ToString()
					}));
				}
				this.RemoveAt(num);
			}

			// Token: 0x06003A7F RID: 14975 RVA: 0x000D4FDC File Offset: 0x000D3FDC
			public override void RemoveAt(int item)
			{
				base.RemoveAt(item);
				if (item < this.owner.domainIndex)
				{
					this.owner.SelectIndex(this.owner.domainIndex - 1);
					return;
				}
				if (item == this.owner.domainIndex)
				{
					this.owner.SelectIndex(-1);
				}
			}

			// Token: 0x06003A80 RID: 14976 RVA: 0x000D5031 File Offset: 0x000D4031
			public override void Insert(int index, object item)
			{
				base.Insert(index, item);
				if (this.owner.Sorted)
				{
					this.owner.SortDomainItems();
				}
			}

			// Token: 0x04001D25 RID: 7461
			private DomainUpDown owner;
		}

		// Token: 0x020003C1 RID: 961
		private sealed class DomainUpDownItemCompare : IComparer
		{
			// Token: 0x06003A81 RID: 14977 RVA: 0x000D5053 File Offset: 0x000D4053
			public int Compare(object p, object q)
			{
				if (p == q)
				{
					return 0;
				}
				if (p == null || q == null)
				{
					return 0;
				}
				return string.Compare(p.ToString(), q.ToString(), false, CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x020003C2 RID: 962
		[ComVisible(true)]
		public class DomainUpDownAccessibleObject : Control.ControlAccessibleObject
		{
			// Token: 0x06003A83 RID: 14979 RVA: 0x000D5082 File Offset: 0x000D4082
			public DomainUpDownAccessibleObject(Control owner)
				: base(owner)
			{
			}

			// Token: 0x17000B02 RID: 2818
			// (get) Token: 0x06003A84 RID: 14980 RVA: 0x000D508B File Offset: 0x000D408B
			private DomainUpDown.DomainItemListAccessibleObject ItemList
			{
				get
				{
					if (this.itemList == null)
					{
						this.itemList = new DomainUpDown.DomainItemListAccessibleObject(this);
					}
					return this.itemList;
				}
			}

			// Token: 0x17000B03 RID: 2819
			// (get) Token: 0x06003A85 RID: 14981 RVA: 0x000D50A8 File Offset: 0x000D40A8
			public override AccessibleRole Role
			{
				get
				{
					AccessibleRole accessibleRole = base.Owner.AccessibleRole;
					if (accessibleRole != AccessibleRole.Default)
					{
						return accessibleRole;
					}
					return AccessibleRole.ComboBox;
				}
			}

			// Token: 0x06003A86 RID: 14982 RVA: 0x000D50CC File Offset: 0x000D40CC
			public override AccessibleObject GetChild(int index)
			{
				switch (index)
				{
				case 0:
					return ((UpDownBase)base.Owner).TextBox.AccessibilityObject.Parent;
				case 1:
					return ((UpDownBase)base.Owner).UpDownButtonsInternal.AccessibilityObject.Parent;
				case 2:
					return this.ItemList;
				default:
					return null;
				}
			}

			// Token: 0x06003A87 RID: 14983 RVA: 0x000D512D File Offset: 0x000D412D
			public override int GetChildCount()
			{
				return 3;
			}

			// Token: 0x04001D26 RID: 7462
			private DomainUpDown.DomainItemListAccessibleObject itemList;
		}

		// Token: 0x020003C3 RID: 963
		internal class DomainItemListAccessibleObject : AccessibleObject
		{
			// Token: 0x06003A88 RID: 14984 RVA: 0x000D5130 File Offset: 0x000D4130
			public DomainItemListAccessibleObject(DomainUpDown.DomainUpDownAccessibleObject parent)
			{
				this.parent = parent;
			}

			// Token: 0x17000B04 RID: 2820
			// (get) Token: 0x06003A89 RID: 14985 RVA: 0x000D5140 File Offset: 0x000D4140
			// (set) Token: 0x06003A8A RID: 14986 RVA: 0x000D5166 File Offset: 0x000D4166
			public override string Name
			{
				get
				{
					string name = base.Name;
					if (name == null || name.Length == 0)
					{
						return "Items";
					}
					return name;
				}
				set
				{
					base.Name = value;
				}
			}

			// Token: 0x17000B05 RID: 2821
			// (get) Token: 0x06003A8B RID: 14987 RVA: 0x000D516F File Offset: 0x000D416F
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.parent;
				}
			}

			// Token: 0x17000B06 RID: 2822
			// (get) Token: 0x06003A8C RID: 14988 RVA: 0x000D5177 File Offset: 0x000D4177
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.List;
				}
			}

			// Token: 0x17000B07 RID: 2823
			// (get) Token: 0x06003A8D RID: 14989 RVA: 0x000D517B File Offset: 0x000D417B
			public override AccessibleStates State
			{
				get
				{
					return AccessibleStates.Invisible | AccessibleStates.Offscreen;
				}
			}

			// Token: 0x06003A8E RID: 14990 RVA: 0x000D5182 File Offset: 0x000D4182
			public override AccessibleObject GetChild(int index)
			{
				if (index >= 0 && index < this.GetChildCount())
				{
					return new DomainUpDown.DomainItemAccessibleObject(((DomainUpDown)this.parent.Owner).Items[index].ToString(), this);
				}
				return null;
			}

			// Token: 0x06003A8F RID: 14991 RVA: 0x000D51B9 File Offset: 0x000D41B9
			public override int GetChildCount()
			{
				return ((DomainUpDown)this.parent.Owner).Items.Count;
			}

			// Token: 0x04001D27 RID: 7463
			private DomainUpDown.DomainUpDownAccessibleObject parent;
		}

		// Token: 0x020003C4 RID: 964
		[ComVisible(true)]
		public class DomainItemAccessibleObject : AccessibleObject
		{
			// Token: 0x06003A90 RID: 14992 RVA: 0x000D51D5 File Offset: 0x000D41D5
			public DomainItemAccessibleObject(string name, AccessibleObject parent)
			{
				this.name = name;
				this.parent = (DomainUpDown.DomainItemListAccessibleObject)parent;
			}

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x06003A91 RID: 14993 RVA: 0x000D51F0 File Offset: 0x000D41F0
			// (set) Token: 0x06003A92 RID: 14994 RVA: 0x000D51F8 File Offset: 0x000D41F8
			public override string Name
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

			// Token: 0x17000B09 RID: 2825
			// (get) Token: 0x06003A93 RID: 14995 RVA: 0x000D5201 File Offset: 0x000D4201
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.parent;
				}
			}

			// Token: 0x17000B0A RID: 2826
			// (get) Token: 0x06003A94 RID: 14996 RVA: 0x000D5209 File Offset: 0x000D4209
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ListItem;
				}
			}

			// Token: 0x17000B0B RID: 2827
			// (get) Token: 0x06003A95 RID: 14997 RVA: 0x000D520D File Offset: 0x000D420D
			public override AccessibleStates State
			{
				get
				{
					return AccessibleStates.Selectable;
				}
			}

			// Token: 0x17000B0C RID: 2828
			// (get) Token: 0x06003A96 RID: 14998 RVA: 0x000D5214 File Offset: 0x000D4214
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.name;
				}
			}

			// Token: 0x04001D28 RID: 7464
			private string name;

			// Token: 0x04001D29 RID: 7465
			private DomainUpDown.DomainItemListAccessibleObject parent;
		}
	}
}
