using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200024D RID: 589
	[DefaultProperty("BindingSource")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Designer("System.Windows.Forms.Design.BindingNavigatorDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionBindingNavigator")]
	[DefaultEvent("RefreshItems")]
	[ComVisible(true)]
	public class BindingNavigator : ToolStrip, ISupportInitialize
	{
		// Token: 0x06001E4A RID: 7754 RVA: 0x0003E946 File Offset: 0x0003D946
		[EditorBrowsable(EditorBrowsableState.Never)]
		public BindingNavigator()
			: this(false)
		{
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x0003E94F File Offset: 0x0003D94F
		public BindingNavigator(BindingSource bindingSource)
			: this(true)
		{
			this.BindingSource = bindingSource;
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0003E95F File Offset: 0x0003D95F
		[EditorBrowsable(EditorBrowsableState.Never)]
		public BindingNavigator(IContainer container)
			: this(false)
		{
			container.Add(this);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0003E96F File Offset: 0x0003D96F
		public BindingNavigator(bool addStandardItems)
		{
			if (addStandardItems)
			{
				this.AddStandardItems();
			}
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x0003E990 File Offset: 0x0003D990
		public void BeginInit()
		{
			this.initializing = true;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0003E999 File Offset: 0x0003D999
		public void EndInit()
		{
			this.initializing = false;
			this.RefreshItemsInternal();
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x0003E9A8 File Offset: 0x0003D9A8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.BindingSource = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x0003E9BC File Offset: 0x0003D9BC
		public virtual void AddStandardItems()
		{
			this.MoveFirstItem = new ToolStripButton();
			this.MovePreviousItem = new ToolStripButton();
			this.MoveNextItem = new ToolStripButton();
			this.MoveLastItem = new ToolStripButton();
			this.PositionItem = new ToolStripTextBox();
			this.CountItem = new ToolStripLabel();
			this.AddNewItem = new ToolStripButton();
			this.DeleteItem = new ToolStripButton();
			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
			ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
			ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
			char c = ((string.IsNullOrEmpty(base.Name) || char.IsLower(base.Name[0])) ? 'b' : 'B');
			this.MoveFirstItem.Name = c + "indingNavigatorMoveFirstItem";
			this.MovePreviousItem.Name = c + "indingNavigatorMovePreviousItem";
			this.MoveNextItem.Name = c + "indingNavigatorMoveNextItem";
			this.MoveLastItem.Name = c + "indingNavigatorMoveLastItem";
			this.PositionItem.Name = c + "indingNavigatorPositionItem";
			this.CountItem.Name = c + "indingNavigatorCountItem";
			this.AddNewItem.Name = c + "indingNavigatorAddNewItem";
			this.DeleteItem.Name = c + "indingNavigatorDeleteItem";
			toolStripSeparator.Name = c + "indingNavigatorSeparator";
			toolStripSeparator2.Name = c + "indingNavigatorSeparator";
			toolStripSeparator3.Name = c + "indingNavigatorSeparator";
			this.MoveFirstItem.Text = SR.GetString("BindingNavigatorMoveFirstItemText");
			this.MovePreviousItem.Text = SR.GetString("BindingNavigatorMovePreviousItemText");
			this.MoveNextItem.Text = SR.GetString("BindingNavigatorMoveNextItemText");
			this.MoveLastItem.Text = SR.GetString("BindingNavigatorMoveLastItemText");
			this.AddNewItem.Text = SR.GetString("BindingNavigatorAddNewItemText");
			this.DeleteItem.Text = SR.GetString("BindingNavigatorDeleteItemText");
			this.CountItem.ToolTipText = SR.GetString("BindingNavigatorCountItemTip");
			this.PositionItem.ToolTipText = SR.GetString("BindingNavigatorPositionItemTip");
			this.CountItem.AutoToolTip = false;
			this.PositionItem.AutoToolTip = false;
			this.PositionItem.AccessibleName = SR.GetString("BindingNavigatorPositionAccessibleName");
			Bitmap bitmap = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveFirst.bmp");
			Bitmap bitmap2 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MovePrevious.bmp");
			Bitmap bitmap3 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveNext.bmp");
			Bitmap bitmap4 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.MoveLast.bmp");
			Bitmap bitmap5 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.AddNew.bmp");
			Bitmap bitmap6 = new Bitmap(typeof(BindingNavigator), "BindingNavigator.Delete.bmp");
			bitmap.MakeTransparent(Color.Magenta);
			bitmap2.MakeTransparent(Color.Magenta);
			bitmap3.MakeTransparent(Color.Magenta);
			bitmap4.MakeTransparent(Color.Magenta);
			bitmap5.MakeTransparent(Color.Magenta);
			bitmap6.MakeTransparent(Color.Magenta);
			this.MoveFirstItem.Image = bitmap;
			this.MovePreviousItem.Image = bitmap2;
			this.MoveNextItem.Image = bitmap3;
			this.MoveLastItem.Image = bitmap4;
			this.AddNewItem.Image = bitmap5;
			this.DeleteItem.Image = bitmap6;
			this.MoveFirstItem.RightToLeftAutoMirrorImage = true;
			this.MovePreviousItem.RightToLeftAutoMirrorImage = true;
			this.MoveNextItem.RightToLeftAutoMirrorImage = true;
			this.MoveLastItem.RightToLeftAutoMirrorImage = true;
			this.AddNewItem.RightToLeftAutoMirrorImage = true;
			this.DeleteItem.RightToLeftAutoMirrorImage = true;
			this.MoveFirstItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.MovePreviousItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.MoveNextItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.MoveLastItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.AddNewItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.DeleteItem.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.PositionItem.AutoSize = false;
			this.PositionItem.Width = 50;
			this.Items.AddRange(new ToolStripItem[]
			{
				this.MoveFirstItem, this.MovePreviousItem, toolStripSeparator, this.PositionItem, this.CountItem, toolStripSeparator2, this.MoveNextItem, this.MoveLastItem, toolStripSeparator3, this.AddNewItem,
				this.DeleteItem
			});
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x0003EE83 File Offset: 0x0003DE83
		// (set) Token: 0x06001E53 RID: 7763 RVA: 0x0003EE8B File Offset: 0x0003DE8B
		[TypeConverter(typeof(ReferenceConverter))]
		[SRDescription("BindingNavigatorBindingSourcePropDescr")]
		[DefaultValue(null)]
		[SRCategory("CatData")]
		public BindingSource BindingSource
		{
			get
			{
				return this.bindingSource;
			}
			set
			{
				this.WireUpBindingSource(ref this.bindingSource, value);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001E54 RID: 7764 RVA: 0x0003EE9A File Offset: 0x0003DE9A
		// (set) Token: 0x06001E55 RID: 7765 RVA: 0x0003EEBE File Offset: 0x0003DEBE
		[SRDescription("BindingNavigatorMoveFirstItemPropDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		public ToolStripItem MoveFirstItem
		{
			get
			{
				if (this.moveFirstItem != null && this.moveFirstItem.IsDisposed)
				{
					this.moveFirstItem = null;
				}
				return this.moveFirstItem;
			}
			set
			{
				this.WireUpButton(ref this.moveFirstItem, value, new EventHandler(this.OnMoveFirst));
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001E56 RID: 7766 RVA: 0x0003EED9 File Offset: 0x0003DED9
		// (set) Token: 0x06001E57 RID: 7767 RVA: 0x0003EEFD File Offset: 0x0003DEFD
		[SRCategory("CatItems")]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRDescription("BindingNavigatorMovePreviousItemPropDescr")]
		public ToolStripItem MovePreviousItem
		{
			get
			{
				if (this.movePreviousItem != null && this.movePreviousItem.IsDisposed)
				{
					this.movePreviousItem = null;
				}
				return this.movePreviousItem;
			}
			set
			{
				this.WireUpButton(ref this.movePreviousItem, value, new EventHandler(this.OnMovePrevious));
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001E58 RID: 7768 RVA: 0x0003EF18 File Offset: 0x0003DF18
		// (set) Token: 0x06001E59 RID: 7769 RVA: 0x0003EF3C File Offset: 0x0003DF3C
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorMoveNextItemPropDescr")]
		public ToolStripItem MoveNextItem
		{
			get
			{
				if (this.moveNextItem != null && this.moveNextItem.IsDisposed)
				{
					this.moveNextItem = null;
				}
				return this.moveNextItem;
			}
			set
			{
				this.WireUpButton(ref this.moveNextItem, value, new EventHandler(this.OnMoveNext));
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x0003EF57 File Offset: 0x0003DF57
		// (set) Token: 0x06001E5B RID: 7771 RVA: 0x0003EF7B File Offset: 0x0003DF7B
		[TypeConverter(typeof(ReferenceConverter))]
		[SRDescription("BindingNavigatorMoveLastItemPropDescr")]
		[SRCategory("CatItems")]
		public ToolStripItem MoveLastItem
		{
			get
			{
				if (this.moveLastItem != null && this.moveLastItem.IsDisposed)
				{
					this.moveLastItem = null;
				}
				return this.moveLastItem;
			}
			set
			{
				this.WireUpButton(ref this.moveLastItem, value, new EventHandler(this.OnMoveLast));
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x0003EF96 File Offset: 0x0003DF96
		// (set) Token: 0x06001E5D RID: 7773 RVA: 0x0003EFBA File Offset: 0x0003DFBA
		[SRDescription("BindingNavigatorAddNewItemPropDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		public ToolStripItem AddNewItem
		{
			get
			{
				if (this.addNewItem != null && this.addNewItem.IsDisposed)
				{
					this.addNewItem = null;
				}
				return this.addNewItem;
			}
			set
			{
				this.WireUpButton(ref this.addNewItem, value, new EventHandler(this.OnAddNew));
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0003EFD5 File Offset: 0x0003DFD5
		// (set) Token: 0x06001E5F RID: 7775 RVA: 0x0003EFF9 File Offset: 0x0003DFF9
		[SRDescription("BindingNavigatorDeleteItemPropDescr")]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		public ToolStripItem DeleteItem
		{
			get
			{
				if (this.deleteItem != null && this.deleteItem.IsDisposed)
				{
					this.deleteItem = null;
				}
				return this.deleteItem;
			}
			set
			{
				this.WireUpButton(ref this.deleteItem, value, new EventHandler(this.OnDelete));
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001E60 RID: 7776 RVA: 0x0003F014 File Offset: 0x0003E014
		// (set) Token: 0x06001E61 RID: 7777 RVA: 0x0003F038 File Offset: 0x0003E038
		[SRCategory("CatItems")]
		[TypeConverter(typeof(ReferenceConverter))]
		[SRDescription("BindingNavigatorPositionItemPropDescr")]
		public ToolStripItem PositionItem
		{
			get
			{
				if (this.positionItem != null && this.positionItem.IsDisposed)
				{
					this.positionItem = null;
				}
				return this.positionItem;
			}
			set
			{
				this.WireUpTextBox(ref this.positionItem, value, new KeyEventHandler(this.OnPositionKey), new EventHandler(this.OnPositionLostFocus));
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0003F05F File Offset: 0x0003E05F
		// (set) Token: 0x06001E63 RID: 7779 RVA: 0x0003F083 File Offset: 0x0003E083
		[TypeConverter(typeof(ReferenceConverter))]
		[SRCategory("CatItems")]
		[SRDescription("BindingNavigatorCountItemPropDescr")]
		public ToolStripItem CountItem
		{
			get
			{
				if (this.countItem != null && this.countItem.IsDisposed)
				{
					this.countItem = null;
				}
				return this.countItem;
			}
			set
			{
				this.WireUpLabel(ref this.countItem, value);
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x0003F092 File Offset: 0x0003E092
		// (set) Token: 0x06001E65 RID: 7781 RVA: 0x0003F09A File Offset: 0x0003E09A
		[SRDescription("BindingNavigatorCountItemFormatPropDescr")]
		[SRCategory("CatAppearance")]
		public string CountItemFormat
		{
			get
			{
				return this.countItemFormat;
			}
			set
			{
				if (this.countItemFormat != value)
				{
					this.countItemFormat = value;
					this.RefreshItemsInternal();
				}
			}
		}

		// Token: 0x140000AB RID: 171
		// (add) Token: 0x06001E66 RID: 7782 RVA: 0x0003F0B7 File Offset: 0x0003E0B7
		// (remove) Token: 0x06001E67 RID: 7783 RVA: 0x0003F0D0 File Offset: 0x0003E0D0
		[SRCategory("CatBehavior")]
		[SRDescription("BindingNavigatorRefreshItemsEventDescr")]
		public event EventHandler RefreshItems
		{
			add
			{
				this.onRefreshItems = (EventHandler)Delegate.Combine(this.onRefreshItems, value);
			}
			remove
			{
				this.onRefreshItems = (EventHandler)Delegate.Remove(this.onRefreshItems, value);
			}
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x0003F0EC File Offset: 0x0003E0EC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void RefreshItemsCore()
		{
			int num;
			int num2;
			bool flag;
			bool flag2;
			if (this.bindingSource == null)
			{
				num = 0;
				num2 = 0;
				flag = false;
				flag2 = false;
			}
			else
			{
				num = this.bindingSource.Count;
				num2 = this.bindingSource.Position + 1;
				flag = ((IBindingList)this.bindingSource).AllowNew;
				flag2 = ((IBindingList)this.bindingSource).AllowRemove;
			}
			if (!base.DesignMode)
			{
				if (this.MoveFirstItem != null)
				{
					this.moveFirstItem.Enabled = num2 > 1;
				}
				if (this.MovePreviousItem != null)
				{
					this.movePreviousItem.Enabled = num2 > 1;
				}
				if (this.MoveNextItem != null)
				{
					this.moveNextItem.Enabled = num2 < num;
				}
				if (this.MoveLastItem != null)
				{
					this.moveLastItem.Enabled = num2 < num;
				}
				if (this.AddNewItem != null)
				{
					this.addNewItem.Enabled = flag;
				}
				if (this.DeleteItem != null)
				{
					this.deleteItem.Enabled = flag2 && num > 0;
				}
				if (this.PositionItem != null)
				{
					this.positionItem.Enabled = num2 > 0 && num > 0;
				}
				if (this.CountItem != null)
				{
					this.countItem.Enabled = num > 0;
				}
			}
			if (this.positionItem != null)
			{
				this.positionItem.Text = num2.ToString(CultureInfo.CurrentCulture);
			}
			if (this.countItem != null)
			{
				this.countItem.Text = (base.DesignMode ? this.CountItemFormat : string.Format(CultureInfo.CurrentCulture, this.CountItemFormat, new object[] { num }));
			}
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x0003F270 File Offset: 0x0003E270
		protected virtual void OnRefreshItems()
		{
			this.RefreshItemsCore();
			if (this.onRefreshItems != null)
			{
				this.onRefreshItems(this, EventArgs.Empty);
			}
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x0003F294 File Offset: 0x0003E294
		public bool Validate()
		{
			bool flag;
			return base.ValidateActiveControl(out flag);
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x0003F2AC File Offset: 0x0003E2AC
		private void AcceptNewPosition()
		{
			if (this.positionItem == null || this.bindingSource == null)
			{
				return;
			}
			int num = this.bindingSource.Position;
			try
			{
				num = Convert.ToInt32(this.positionItem.Text, CultureInfo.CurrentCulture) - 1;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			if (num != this.bindingSource.Position)
			{
				this.bindingSource.Position = num;
			}
			this.RefreshItemsInternal();
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0003F334 File Offset: 0x0003E334
		private void CancelNewPosition()
		{
			this.RefreshItemsInternal();
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0003F33C File Offset: 0x0003E33C
		private void OnMoveFirst(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MoveFirst();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0003F35F File Offset: 0x0003E35F
		private void OnMovePrevious(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MovePrevious();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0003F382 File Offset: 0x0003E382
		private void OnMoveNext(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MoveNext();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0003F3A5 File Offset: 0x0003E3A5
		private void OnMoveLast(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.MoveLast();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x0003F3C8 File Offset: 0x0003E3C8
		private void OnAddNew(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.AddNew();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x0003F3EC File Offset: 0x0003E3EC
		private void OnDelete(object sender, EventArgs e)
		{
			if (this.Validate() && this.bindingSource != null)
			{
				this.bindingSource.RemoveCurrent();
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0003F410 File Offset: 0x0003E410
		private void OnPositionKey(object sender, KeyEventArgs e)
		{
			Keys keyCode = e.KeyCode;
			if (keyCode == Keys.Return)
			{
				this.AcceptNewPosition();
				return;
			}
			if (keyCode != Keys.Escape)
			{
				return;
			}
			this.CancelNewPosition();
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0003F43C File Offset: 0x0003E43C
		private void OnPositionLostFocus(object sender, EventArgs e)
		{
			this.AcceptNewPosition();
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0003F444 File Offset: 0x0003E444
		private void OnBindingSourceStateChanged(object sender, EventArgs e)
		{
			this.RefreshItemsInternal();
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0003F44C File Offset: 0x0003E44C
		private void OnBindingSourceListChanged(object sender, ListChangedEventArgs e)
		{
			this.RefreshItemsInternal();
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0003F454 File Offset: 0x0003E454
		private void RefreshItemsInternal()
		{
			if (this.initializing)
			{
				return;
			}
			this.OnRefreshItems();
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0003F465 File Offset: 0x0003E465
		private void ResetCountItemFormat()
		{
			this.countItemFormat = SR.GetString("BindingNavigatorCountItemFormat");
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0003F477 File Offset: 0x0003E477
		private bool ShouldSerializeCountItemFormat()
		{
			return this.countItemFormat != SR.GetString("BindingNavigatorCountItemFormat");
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0003F48E File Offset: 0x0003E48E
		private void WireUpButton(ref ToolStripItem oldButton, ToolStripItem newButton, EventHandler clickHandler)
		{
			if (oldButton == newButton)
			{
				return;
			}
			if (oldButton != null)
			{
				oldButton.Click -= clickHandler;
			}
			if (newButton != null)
			{
				newButton.Click += clickHandler;
			}
			oldButton = newButton;
			this.RefreshItemsInternal();
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0003F4B8 File Offset: 0x0003E4B8
		private void WireUpTextBox(ref ToolStripItem oldTextBox, ToolStripItem newTextBox, KeyEventHandler keyUpHandler, EventHandler lostFocusHandler)
		{
			if (oldTextBox != newTextBox)
			{
				ToolStripControlHost toolStripControlHost = oldTextBox as ToolStripControlHost;
				ToolStripControlHost toolStripControlHost2 = newTextBox as ToolStripControlHost;
				if (toolStripControlHost != null)
				{
					toolStripControlHost.KeyUp -= keyUpHandler;
					toolStripControlHost.LostFocus -= lostFocusHandler;
				}
				if (toolStripControlHost2 != null)
				{
					toolStripControlHost2.KeyUp += keyUpHandler;
					toolStripControlHost2.LostFocus += lostFocusHandler;
				}
				oldTextBox = newTextBox;
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0003F506 File Offset: 0x0003E506
		private void WireUpLabel(ref ToolStripItem oldLabel, ToolStripItem newLabel)
		{
			if (oldLabel != newLabel)
			{
				oldLabel = newLabel;
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0003F518 File Offset: 0x0003E518
		private void WireUpBindingSource(ref BindingSource oldBindingSource, BindingSource newBindingSource)
		{
			if (oldBindingSource != newBindingSource)
			{
				if (oldBindingSource != null)
				{
					oldBindingSource.PositionChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.CurrentChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.CurrentItemChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.DataSourceChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.DataMemberChanged -= this.OnBindingSourceStateChanged;
					oldBindingSource.ListChanged -= this.OnBindingSourceListChanged;
				}
				if (newBindingSource != null)
				{
					newBindingSource.PositionChanged += this.OnBindingSourceStateChanged;
					newBindingSource.CurrentChanged += this.OnBindingSourceStateChanged;
					newBindingSource.CurrentItemChanged += this.OnBindingSourceStateChanged;
					newBindingSource.DataSourceChanged += this.OnBindingSourceStateChanged;
					newBindingSource.DataMemberChanged += this.OnBindingSourceStateChanged;
					newBindingSource.ListChanged += this.OnBindingSourceListChanged;
				}
				oldBindingSource = newBindingSource;
				this.RefreshItemsInternal();
			}
		}

		// Token: 0x040013DD RID: 5085
		private BindingSource bindingSource;

		// Token: 0x040013DE RID: 5086
		private ToolStripItem moveFirstItem;

		// Token: 0x040013DF RID: 5087
		private ToolStripItem movePreviousItem;

		// Token: 0x040013E0 RID: 5088
		private ToolStripItem moveNextItem;

		// Token: 0x040013E1 RID: 5089
		private ToolStripItem moveLastItem;

		// Token: 0x040013E2 RID: 5090
		private ToolStripItem addNewItem;

		// Token: 0x040013E3 RID: 5091
		private ToolStripItem deleteItem;

		// Token: 0x040013E4 RID: 5092
		private ToolStripItem positionItem;

		// Token: 0x040013E5 RID: 5093
		private ToolStripItem countItem;

		// Token: 0x040013E6 RID: 5094
		private string countItemFormat = SR.GetString("BindingNavigatorCountItemFormat");

		// Token: 0x040013E7 RID: 5095
		private EventHandler onRefreshItems;

		// Token: 0x040013E8 RID: 5096
		private bool initializing;
	}
}
