using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002CD RID: 717
	[DesignTimeVisible(false)]
	[DefaultProperty("Header")]
	[ToolboxItem(false)]
	public abstract class DataGridColumnStyle : Component, IDataGridColumnStyleEditingNotificationService
	{
		// Token: 0x0600292A RID: 10538 RVA: 0x0006CFA2 File Offset: 0x0006BFA2
		public DataGridColumnStyle()
		{
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x0006CFDE File Offset: 0x0006BFDE
		public DataGridColumnStyle(PropertyDescriptor prop)
			: this()
		{
			this.PropertyDescriptor = prop;
			if (prop != null)
			{
				this.readOnly = prop.IsReadOnly;
			}
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x0006CFFC File Offset: 0x0006BFFC
		internal DataGridColumnStyle(PropertyDescriptor prop, bool isDefault)
			: this(prop)
		{
			this.isDefault = isDefault;
			if (isDefault)
			{
				this.headerName = prop.Name;
				this.mappingName = prop.Name;
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600292D RID: 10541 RVA: 0x0006D027 File Offset: 0x0006C027
		// (set) Token: 0x0600292E RID: 10542 RVA: 0x0006D030 File Offset: 0x0006C030
		[SRCategory("CatDisplay")]
		[Localizable(true)]
		[DefaultValue(HorizontalAlignment.Left)]
		public virtual HorizontalAlignment Alignment
		{
			get
			{
				return this.alignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridLineStyle));
				}
				if (this.alignment != value)
				{
					this.alignment = value;
					this.OnAlignmentChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		// Token: 0x14000121 RID: 289
		// (add) Token: 0x0600292F RID: 10543 RVA: 0x0006D084 File Offset: 0x0006C084
		// (remove) Token: 0x06002930 RID: 10544 RVA: 0x0006D097 File Offset: 0x0006C097
		public event EventHandler AlignmentChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventAlignment, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventAlignment, value);
			}
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x0006D0AA File Offset: 0x0006C0AA
		protected internal virtual void UpdateUI(CurrencyManager source, int rowNum, string displayText)
		{
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002932 RID: 10546 RVA: 0x0006D0AC File Offset: 0x0006C0AC
		[Browsable(false)]
		public AccessibleObject HeaderAccessibleObject
		{
			get
			{
				if (this.headerAccessibleObject == null)
				{
					this.headerAccessibleObject = this.CreateHeaderAccessibleObject();
				}
				return this.headerAccessibleObject;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002933 RID: 10547 RVA: 0x0006D0C8 File Offset: 0x0006C0C8
		// (set) Token: 0x06002934 RID: 10548 RVA: 0x0006D0D0 File Offset: 0x0006C0D0
		[DefaultValue(null)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public virtual PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this.propertyDescriptor;
			}
			set
			{
				if (this.propertyDescriptor != value)
				{
					this.propertyDescriptor = value;
					this.OnPropertyDescriptorChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000122 RID: 290
		// (add) Token: 0x06002935 RID: 10549 RVA: 0x0006D0ED File Offset: 0x0006C0ED
		// (remove) Token: 0x06002936 RID: 10550 RVA: 0x0006D100 File Offset: 0x0006C100
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public event EventHandler PropertyDescriptorChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventPropertyDescriptor, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventPropertyDescriptor, value);
			}
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x0006D113 File Offset: 0x0006C113
		protected virtual AccessibleObject CreateHeaderAccessibleObject()
		{
			return new DataGridColumnStyle.DataGridColumnHeaderAccessibleObject(this);
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x0006D11B File Offset: 0x0006C11B
		protected virtual void SetDataGrid(DataGrid value)
		{
			this.SetDataGridInColumn(value);
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x0006D124 File Offset: 0x0006C124
		protected virtual void SetDataGridInColumn(DataGrid value)
		{
			if (this.PropertyDescriptor == null && value != null)
			{
				CurrencyManager listManager = value.ListManager;
				if (listManager == null)
				{
					return;
				}
				PropertyDescriptorCollection itemProperties = listManager.GetItemProperties();
				int count = itemProperties.Count;
				for (int i = 0; i < itemProperties.Count; i++)
				{
					PropertyDescriptor propertyDescriptor = itemProperties[i];
					if (!typeof(IList).IsAssignableFrom(propertyDescriptor.PropertyType) && propertyDescriptor.Name.Equals(this.HeaderText))
					{
						this.PropertyDescriptor = propertyDescriptor;
						return;
					}
				}
			}
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x0006D1A0 File Offset: 0x0006C1A0
		internal void SetDataGridInternalInColumn(DataGrid value)
		{
			if (value == null || value.Initializing)
			{
				return;
			}
			this.SetDataGridInColumn(value);
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x0600293B RID: 10555 RVA: 0x0006D1B5 File Offset: 0x0006C1B5
		[Browsable(false)]
		public virtual DataGridTableStyle DataGridTableStyle
		{
			get
			{
				return this.dataGridTableStyle;
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x0006D1C0 File Offset: 0x0006C1C0
		internal void SetDataGridTableInColumn(DataGridTableStyle value, bool force)
		{
			if (this.dataGridTableStyle != null && this.dataGridTableStyle.Equals(value) && !force)
			{
				return;
			}
			if (value != null && value.DataGrid != null && !value.DataGrid.Initializing)
			{
				this.SetDataGridInColumn(value.DataGrid);
			}
			this.dataGridTableStyle = value;
		}

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x0006D212 File Offset: 0x0006C212
		protected int FontHeight
		{
			get
			{
				if (this.fontHeight != -1)
				{
					return this.fontHeight;
				}
				if (this.DataGridTableStyle != null)
				{
					return this.DataGridTableStyle.DataGrid.FontHeight;
				}
				return DataGridTableStyle.defaultFontHeight;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x0006D242 File Offset: 0x0006C242
		private bool ShouldSerializeFont()
		{
			return this.font != null;
		}

		// Token: 0x14000123 RID: 291
		// (add) Token: 0x0600293F RID: 10559 RVA: 0x0006D250 File Offset: 0x0006C250
		// (remove) Token: 0x06002940 RID: 10560 RVA: 0x0006D252 File Offset: 0x0006C252
		public event EventHandler FontChanged
		{
			add
			{
			}
			remove
			{
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002941 RID: 10561 RVA: 0x0006D254 File Offset: 0x0006C254
		// (set) Token: 0x06002942 RID: 10562 RVA: 0x0006D25C File Offset: 0x0006C25C
		[SRCategory("CatDisplay")]
		[Localizable(true)]
		public virtual string HeaderText
		{
			get
			{
				return this.headerName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.headerName.Equals(value))
				{
					return;
				}
				this.headerName = value;
				this.OnHeaderTextChanged(EventArgs.Empty);
				if (this.PropertyDescriptor != null)
				{
					this.Invalidate();
				}
			}
		}

		// Token: 0x14000124 RID: 292
		// (add) Token: 0x06002943 RID: 10563 RVA: 0x0006D297 File Offset: 0x0006C297
		// (remove) Token: 0x06002944 RID: 10564 RVA: 0x0006D2AA File Offset: 0x0006C2AA
		public event EventHandler HeaderTextChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventHeaderText, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventHeaderText, value);
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002945 RID: 10565 RVA: 0x0006D2BD File Offset: 0x0006C2BD
		// (set) Token: 0x06002946 RID: 10566 RVA: 0x0006D2C8 File Offset: 0x0006C2C8
		[Localizable(true)]
		[DefaultValue("")]
		[Editor("System.Windows.Forms.Design.DataGridColumnStyleMappingNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string MappingName
		{
			get
			{
				return this.mappingName;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (this.mappingName.Equals(value))
				{
					return;
				}
				string text = this.mappingName;
				this.mappingName = value;
				try
				{
					if (this.dataGridTableStyle != null)
					{
						this.dataGridTableStyle.GridColumnStyles.CheckForMappingNameDuplicates(this);
					}
				}
				catch
				{
					this.mappingName = text;
					throw;
				}
				this.OnMappingNameChanged(EventArgs.Empty);
			}
		}

		// Token: 0x14000125 RID: 293
		// (add) Token: 0x06002947 RID: 10567 RVA: 0x0006D33C File Offset: 0x0006C33C
		// (remove) Token: 0x06002948 RID: 10568 RVA: 0x0006D34F File Offset: 0x0006C34F
		public event EventHandler MappingNameChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventMappingName, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventMappingName, value);
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x0006D362 File Offset: 0x0006C362
		private bool ShouldSerializeHeaderText()
		{
			return this.headerName.Length != 0;
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x0006D375 File Offset: 0x0006C375
		public void ResetHeaderText()
		{
			this.HeaderText = "";
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600294B RID: 10571 RVA: 0x0006D382 File Offset: 0x0006C382
		// (set) Token: 0x0600294C RID: 10572 RVA: 0x0006D38A File Offset: 0x0006C38A
		[SRCategory("CatDisplay")]
		[Localizable(true)]
		public virtual string NullText
		{
			get
			{
				return this.nullText;
			}
			set
			{
				if (this.nullText != null && this.nullText.Equals(value))
				{
					return;
				}
				this.nullText = value;
				this.OnNullTextChanged(EventArgs.Empty);
				this.Invalidate();
			}
		}

		// Token: 0x14000126 RID: 294
		// (add) Token: 0x0600294D RID: 10573 RVA: 0x0006D3BB File Offset: 0x0006C3BB
		// (remove) Token: 0x0600294E RID: 10574 RVA: 0x0006D3CE File Offset: 0x0006C3CE
		public event EventHandler NullTextChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventNullText, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventNullText, value);
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x0600294F RID: 10575 RVA: 0x0006D3E1 File Offset: 0x0006C3E1
		// (set) Token: 0x06002950 RID: 10576 RVA: 0x0006D3E9 File Offset: 0x0006C3E9
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				if (this.readOnly != value)
				{
					this.readOnly = value;
					this.OnReadOnlyChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000127 RID: 295
		// (add) Token: 0x06002951 RID: 10577 RVA: 0x0006D406 File Offset: 0x0006C406
		// (remove) Token: 0x06002952 RID: 10578 RVA: 0x0006D419 File Offset: 0x0006C419
		public event EventHandler ReadOnlyChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventReadOnly, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventReadOnly, value);
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002953 RID: 10579 RVA: 0x0006D42C File Offset: 0x0006C42C
		// (set) Token: 0x06002954 RID: 10580 RVA: 0x0006D434 File Offset: 0x0006C434
		[Localizable(true)]
		[DefaultValue(100)]
		[SRCategory("CatLayout")]
		public virtual int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (this.width != value)
				{
					this.width = value;
					DataGrid dataGrid = ((this.DataGridTableStyle == null) ? null : this.DataGridTableStyle.DataGrid);
					if (dataGrid != null)
					{
						dataGrid.PerformLayout();
						dataGrid.InvalidateInside();
					}
					this.OnWidthChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x14000128 RID: 296
		// (add) Token: 0x06002955 RID: 10581 RVA: 0x0006D482 File Offset: 0x0006C482
		// (remove) Token: 0x06002956 RID: 10582 RVA: 0x0006D495 File Offset: 0x0006C495
		public event EventHandler WidthChanged
		{
			add
			{
				base.Events.AddHandler(DataGridColumnStyle.EventWidth, value);
			}
			remove
			{
				base.Events.RemoveHandler(DataGridColumnStyle.EventWidth, value);
			}
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x0006D4A8 File Offset: 0x0006C4A8
		protected void BeginUpdate()
		{
			this.updating = true;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x0006D4B1 File Offset: 0x0006C4B1
		protected void EndUpdate()
		{
			this.updating = false;
			if (this.invalid)
			{
				this.invalid = false;
				this.Invalidate();
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002959 RID: 10585 RVA: 0x0006D4CF File Offset: 0x0006C4CF
		internal virtual bool WantArrows
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0006D4D2 File Offset: 0x0006C4D2
		internal virtual string GetDisplayText(object value)
		{
			return value.ToString();
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x0006D4DA File Offset: 0x0006C4DA
		private void ResetNullText()
		{
			this.NullText = SR.GetString("DataGridNullText");
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0006D4EC File Offset: 0x0006C4EC
		private bool ShouldSerializeNullText()
		{
			return !SR.GetString("DataGridNullText").Equals(this.nullText);
		}

		// Token: 0x0600295D RID: 10589
		protected internal abstract Size GetPreferredSize(Graphics g, object value);

		// Token: 0x0600295E RID: 10590
		protected internal abstract int GetMinimumHeight();

		// Token: 0x0600295F RID: 10591
		protected internal abstract int GetPreferredHeight(Graphics g, object value);

		// Token: 0x06002960 RID: 10592 RVA: 0x0006D508 File Offset: 0x0006C508
		protected internal virtual object GetColumnValueAtRow(CurrencyManager source, int rowNum)
		{
			this.CheckValidDataSource(source);
			if (this.PropertyDescriptor == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridColumnNoPropertyDescriptor"));
			}
			return this.PropertyDescriptor.GetValue(source[rowNum]);
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x0006D548 File Offset: 0x0006C548
		protected virtual void Invalidate()
		{
			if (this.updating)
			{
				this.invalid = true;
				return;
			}
			DataGridTableStyle dataGridTableStyle = this.DataGridTableStyle;
			if (dataGridTableStyle != null)
			{
				dataGridTableStyle.InvalidateColumn(this);
			}
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x0006D578 File Offset: 0x0006C578
		protected void CheckValidDataSource(CurrencyManager value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value", "DataGridColumnStyle.CheckValidDataSource(DataSource value), value == null");
			}
			if (this.PropertyDescriptor == null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridColumnUnbound", new object[] { this.HeaderText }));
			}
		}

		// Token: 0x06002963 RID: 10595
		protected internal abstract void Abort(int rowNum);

		// Token: 0x06002964 RID: 10596
		protected internal abstract bool Commit(CurrencyManager dataSource, int rowNum);

		// Token: 0x06002965 RID: 10597 RVA: 0x0006D5C3 File Offset: 0x0006C5C3
		protected internal virtual void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly)
		{
			this.Edit(source, rowNum, bounds, readOnly, null, true);
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x0006D5D2 File Offset: 0x0006C5D2
		protected internal virtual void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText)
		{
			this.Edit(source, rowNum, bounds, readOnly, displayText, true);
		}

		// Token: 0x06002967 RID: 10599
		protected internal abstract void Edit(CurrencyManager source, int rowNum, Rectangle bounds, bool readOnly, string displayText, bool cellIsVisible);

		// Token: 0x06002968 RID: 10600 RVA: 0x0006D5E2 File Offset: 0x0006C5E2
		internal virtual bool MouseDown(int rowNum, int x, int y)
		{
			return false;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x0006D5E5 File Offset: 0x0006C5E5
		protected internal virtual void EnterNullValue()
		{
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x0006D5E8 File Offset: 0x0006C5E8
		internal virtual bool KeyPress(int rowNum, Keys keyData)
		{
			if (this.ReadOnly || (this.DataGridTableStyle != null && this.DataGridTableStyle.DataGrid != null && this.DataGridTableStyle.DataGrid.ReadOnly))
			{
				return false;
			}
			if (keyData == (Keys)131168 || keyData == (Keys.ShiftKey | Keys.Space | Keys.Control))
			{
				this.EnterNullValue();
				return true;
			}
			return false;
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x0006D63F File Offset: 0x0006C63F
		protected internal virtual void ConcedeFocus()
		{
		}

		// Token: 0x0600296C RID: 10604
		protected internal abstract void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum);

		// Token: 0x0600296D RID: 10605
		protected internal abstract void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, bool alignToRight);

		// Token: 0x0600296E RID: 10606 RVA: 0x0006D641 File Offset: 0x0006C641
		protected internal virtual void Paint(Graphics g, Rectangle bounds, CurrencyManager source, int rowNum, Brush backBrush, Brush foreBrush, bool alignToRight)
		{
			this.Paint(g, bounds, source, rowNum, alignToRight);
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x0006D650 File Offset: 0x0006C650
		private void OnPropertyDescriptorChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventPropertyDescriptor] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0006D680 File Offset: 0x0006C680
		private void OnAlignmentChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventAlignment] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x0006D6B0 File Offset: 0x0006C6B0
		private void OnHeaderTextChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventHeaderText] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x0006D6E0 File Offset: 0x0006C6E0
		private void OnMappingNameChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventMappingName] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x0006D710 File Offset: 0x0006C710
		private void OnReadOnlyChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventReadOnly] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x0006D740 File Offset: 0x0006C740
		private void OnNullTextChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventNullText] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x0006D770 File Offset: 0x0006C770
		private void OnWidthChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[DataGridColumnStyle.EventWidth] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0006D7A0 File Offset: 0x0006C7A0
		protected internal virtual void SetColumnValueAtRow(CurrencyManager source, int rowNum, object value)
		{
			this.CheckValidDataSource(source);
			if (source.Position != rowNum)
			{
				throw new ArgumentException(SR.GetString("DataGridColumnListManagerPosition"), "rowNum");
			}
			if (source[rowNum] is IEditableObject)
			{
				((IEditableObject)source[rowNum]).BeginEdit();
			}
			this.PropertyDescriptor.SetValue(source[rowNum], value);
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x0006D804 File Offset: 0x0006C804
		protected internal virtual void ColumnStartedEditing(Control editingControl)
		{
			this.DataGridTableStyle.DataGrid.ColumnStartedEditing(editingControl);
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x0006D817 File Offset: 0x0006C817
		void IDataGridColumnStyleEditingNotificationService.ColumnStartedEditing(Control editingControl)
		{
			this.ColumnStartedEditing(editingControl);
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x0006D820 File Offset: 0x0006C820
		protected internal virtual void ReleaseHostedControl()
		{
		}

		// Token: 0x04001749 RID: 5961
		private HorizontalAlignment alignment;

		// Token: 0x0400174A RID: 5962
		private PropertyDescriptor propertyDescriptor;

		// Token: 0x0400174B RID: 5963
		private DataGridTableStyle dataGridTableStyle;

		// Token: 0x0400174C RID: 5964
		private Font font;

		// Token: 0x0400174D RID: 5965
		internal int fontHeight = -1;

		// Token: 0x0400174E RID: 5966
		private string mappingName = "";

		// Token: 0x0400174F RID: 5967
		private string headerName = "";

		// Token: 0x04001750 RID: 5968
		private bool invalid;

		// Token: 0x04001751 RID: 5969
		private string nullText = SR.GetString("DataGridNullText");

		// Token: 0x04001752 RID: 5970
		private bool readOnly;

		// Token: 0x04001753 RID: 5971
		private bool updating;

		// Token: 0x04001754 RID: 5972
		internal int width = -1;

		// Token: 0x04001755 RID: 5973
		private bool isDefault;

		// Token: 0x04001756 RID: 5974
		private AccessibleObject headerAccessibleObject;

		// Token: 0x04001757 RID: 5975
		private static readonly object EventAlignment = new object();

		// Token: 0x04001758 RID: 5976
		private static readonly object EventPropertyDescriptor = new object();

		// Token: 0x04001759 RID: 5977
		private static readonly object EventHeaderText = new object();

		// Token: 0x0400175A RID: 5978
		private static readonly object EventMappingName = new object();

		// Token: 0x0400175B RID: 5979
		private static readonly object EventNullText = new object();

		// Token: 0x0400175C RID: 5980
		private static readonly object EventReadOnly = new object();

		// Token: 0x0400175D RID: 5981
		private static readonly object EventWidth = new object();

		// Token: 0x020002CE RID: 718
		protected class CompModSwitches
		{
			// Token: 0x170006CD RID: 1741
			// (get) Token: 0x0600297B RID: 10619 RVA: 0x0006D877 File Offset: 0x0006C877
			public static TraceSwitch DGEditColumnEditing
			{
				get
				{
					if (DataGridColumnStyle.CompModSwitches.dgEditColumnEditing == null)
					{
						DataGridColumnStyle.CompModSwitches.dgEditColumnEditing = new TraceSwitch("DGEditColumnEditing", "Editing related tracing");
					}
					return DataGridColumnStyle.CompModSwitches.dgEditColumnEditing;
				}
			}

			// Token: 0x0400175E RID: 5982
			private static TraceSwitch dgEditColumnEditing;
		}

		// Token: 0x020002CF RID: 719
		[ComVisible(true)]
		protected class DataGridColumnHeaderAccessibleObject : AccessibleObject
		{
			// Token: 0x0600297D RID: 10621 RVA: 0x0006D8A1 File Offset: 0x0006C8A1
			public DataGridColumnHeaderAccessibleObject(DataGridColumnStyle owner)
				: this()
			{
				this.owner = owner;
			}

			// Token: 0x0600297E RID: 10622 RVA: 0x0006D8B0 File Offset: 0x0006C8B0
			public DataGridColumnHeaderAccessibleObject()
			{
			}

			// Token: 0x170006CE RID: 1742
			// (get) Token: 0x0600297F RID: 10623 RVA: 0x0006D8B8 File Offset: 0x0006C8B8
			public override Rectangle Bounds
			{
				get
				{
					if (this.owner.PropertyDescriptor == null)
					{
						return Rectangle.Empty;
					}
					DataGrid dataGrid = this.DataGrid;
					if (dataGrid.DataGridRowsLength == 0)
					{
						return Rectangle.Empty;
					}
					GridColumnStylesCollection gridColumnStyles = this.owner.dataGridTableStyle.GridColumnStyles;
					int num = -1;
					for (int i = 0; i < gridColumnStyles.Count; i++)
					{
						if (gridColumnStyles[i] == this.owner)
						{
							num = i;
							break;
						}
					}
					Rectangle cellBounds = dataGrid.GetCellBounds(0, num);
					cellBounds.Y = dataGrid.GetColumnHeadersRect().Y;
					return dataGrid.RectangleToScreen(cellBounds);
				}
			}

			// Token: 0x170006CF RID: 1743
			// (get) Token: 0x06002980 RID: 10624 RVA: 0x0006D94C File Offset: 0x0006C94C
			public override string Name
			{
				get
				{
					return this.Owner.headerName;
				}
			}

			// Token: 0x170006D0 RID: 1744
			// (get) Token: 0x06002981 RID: 10625 RVA: 0x0006D959 File Offset: 0x0006C959
			protected DataGridColumnStyle Owner
			{
				get
				{
					return this.owner;
				}
			}

			// Token: 0x170006D1 RID: 1745
			// (get) Token: 0x06002982 RID: 10626 RVA: 0x0006D961 File Offset: 0x0006C961
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.DataGrid.AccessibilityObject;
				}
			}

			// Token: 0x170006D2 RID: 1746
			// (get) Token: 0x06002983 RID: 10627 RVA: 0x0006D96E File Offset: 0x0006C96E
			private DataGrid DataGrid
			{
				get
				{
					return this.owner.dataGridTableStyle.dataGrid;
				}
			}

			// Token: 0x170006D3 RID: 1747
			// (get) Token: 0x06002984 RID: 10628 RVA: 0x0006D980 File Offset: 0x0006C980
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ColumnHeader;
				}
			}

			// Token: 0x06002985 RID: 10629 RVA: 0x0006D984 File Offset: 0x0006C984
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return this.Parent.GetChild(1 + this.Owner.dataGridTableStyle.GridColumnStyles.IndexOf(this.Owner) - 1);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return this.Parent.GetChild(1 + this.Owner.dataGridTableStyle.GridColumnStyles.IndexOf(this.Owner) + 1);
				default:
					return null;
				}
			}

			// Token: 0x0400175F RID: 5983
			private DataGridColumnStyle owner;
		}
	}
}
