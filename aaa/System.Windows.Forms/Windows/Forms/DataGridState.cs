using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020002DF RID: 735
	internal sealed class DataGridState : ICloneable
	{
		// Token: 0x06002AB1 RID: 10929 RVA: 0x00072944 File Offset: 0x00071944
		public DataGridState()
		{
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x00072958 File Offset: 0x00071958
		public DataGridState(DataGrid dataGrid)
		{
			this.PushState(dataGrid);
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002AB3 RID: 10931 RVA: 0x00072973 File Offset: 0x00071973
		internal AccessibleObject ParentRowAccessibleObject
		{
			get
			{
				if (this.parentRowAccessibleObject == null)
				{
					this.parentRowAccessibleObject = new DataGridState.DataGridStateParentRowAccessibleObject(this);
				}
				return this.parentRowAccessibleObject;
			}
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x00072990 File Offset: 0x00071990
		public object Clone()
		{
			return new DataGridState
			{
				DataGridRows = this.DataGridRows,
				DataSource = this.DataSource,
				DataMember = this.DataMember,
				FirstVisibleRow = this.FirstVisibleRow,
				FirstVisibleCol = this.FirstVisibleCol,
				CurrentRow = this.CurrentRow,
				CurrentCol = this.CurrentCol,
				GridColumnStyles = this.GridColumnStyles,
				ListManager = this.ListManager,
				DataGrid = this.DataGrid
			};
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x00072A1C File Offset: 0x00071A1C
		public void PushState(DataGrid dataGrid)
		{
			this.DataSource = dataGrid.DataSource;
			this.DataMember = dataGrid.DataMember;
			this.DataGrid = dataGrid;
			this.DataGridRows = dataGrid.DataGridRows;
			this.DataGridRowsLength = dataGrid.DataGridRowsLength;
			this.FirstVisibleRow = dataGrid.firstVisibleRow;
			this.FirstVisibleCol = dataGrid.firstVisibleCol;
			this.CurrentRow = dataGrid.currentRow;
			this.GridColumnStyles = new GridColumnStylesCollection(dataGrid.myGridTable);
			this.GridColumnStyles.Clear();
			foreach (object obj in dataGrid.myGridTable.GridColumnStyles)
			{
				DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)obj;
				this.GridColumnStyles.Add(dataGridColumnStyle);
			}
			this.ListManager = dataGrid.ListManager;
			this.ListManager.ItemChanged += this.DataSource_Changed;
			this.ListManager.MetaDataChanged += this.DataSource_MetaDataChanged;
			this.CurrentCol = dataGrid.currentCol;
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x00072B40 File Offset: 0x00071B40
		public void RemoveChangeNotification()
		{
			this.ListManager.ItemChanged -= this.DataSource_Changed;
			this.ListManager.MetaDataChanged -= this.DataSource_MetaDataChanged;
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x00072B70 File Offset: 0x00071B70
		public void PullState(DataGrid dataGrid, bool createColumn)
		{
			dataGrid.Set_ListManager(this.DataSource, this.DataMember, true, createColumn);
			dataGrid.firstVisibleRow = this.FirstVisibleRow;
			dataGrid.firstVisibleCol = this.FirstVisibleCol;
			dataGrid.currentRow = this.CurrentRow;
			dataGrid.currentCol = this.CurrentCol;
			dataGrid.SetDataGridRows(this.DataGridRows, this.DataGridRowsLength);
		}

		// Token: 0x06002AB8 RID: 10936 RVA: 0x00072BD3 File Offset: 0x00071BD3
		private void DataSource_Changed(object sender, ItemChangedEventArgs e)
		{
			if (this.DataGrid != null && this.ListManager.Position == e.Index)
			{
				this.DataGrid.InvalidateParentRows();
				return;
			}
			if (this.DataGrid != null)
			{
				this.DataGrid.ParentRowsDataChanged();
			}
		}

		// Token: 0x06002AB9 RID: 10937 RVA: 0x00072C0F File Offset: 0x00071C0F
		private void DataSource_MetaDataChanged(object sender, EventArgs e)
		{
			if (this.DataGrid != null)
			{
				this.DataGrid.ParentRowsDataChanged();
			}
		}

		// Token: 0x040017C1 RID: 6081
		public object DataSource;

		// Token: 0x040017C2 RID: 6082
		public string DataMember;

		// Token: 0x040017C3 RID: 6083
		public CurrencyManager ListManager;

		// Token: 0x040017C4 RID: 6084
		public DataGridRow[] DataGridRows = new DataGridRow[0];

		// Token: 0x040017C5 RID: 6085
		public DataGrid DataGrid;

		// Token: 0x040017C6 RID: 6086
		public int DataGridRowsLength;

		// Token: 0x040017C7 RID: 6087
		public GridColumnStylesCollection GridColumnStyles;

		// Token: 0x040017C8 RID: 6088
		public int FirstVisibleRow;

		// Token: 0x040017C9 RID: 6089
		public int FirstVisibleCol;

		// Token: 0x040017CA RID: 6090
		public int CurrentRow;

		// Token: 0x040017CB RID: 6091
		public int CurrentCol;

		// Token: 0x040017CC RID: 6092
		public DataGridRow LinkingRow;

		// Token: 0x040017CD RID: 6093
		private AccessibleObject parentRowAccessibleObject;

		// Token: 0x020002E0 RID: 736
		[ComVisible(true)]
		internal class DataGridStateParentRowAccessibleObject : AccessibleObject
		{
			// Token: 0x06002ABA RID: 10938 RVA: 0x00072C24 File Offset: 0x00071C24
			public DataGridStateParentRowAccessibleObject(DataGridState owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000714 RID: 1812
			// (get) Token: 0x06002ABB RID: 10939 RVA: 0x00072C34 File Offset: 0x00071C34
			public override Rectangle Bounds
			{
				get
				{
					DataGridParentRows dataGridParentRows = ((DataGridParentRows.DataGridParentRowsAccessibleObject)this.Parent).Owner;
					DataGrid dataGrid = this.owner.LinkingRow.DataGrid;
					Rectangle boundsForDataGridStateAccesibility = dataGridParentRows.GetBoundsForDataGridStateAccesibility(this.owner);
					boundsForDataGridStateAccesibility.Y += dataGrid.ParentRowsBounds.Y;
					return dataGrid.RectangleToScreen(boundsForDataGridStateAccesibility);
				}
			}

			// Token: 0x17000715 RID: 1813
			// (get) Token: 0x06002ABC RID: 10940 RVA: 0x00072C93 File Offset: 0x00071C93
			public override string Name
			{
				get
				{
					return SR.GetString("AccDGParentRow");
				}
			}

			// Token: 0x17000716 RID: 1814
			// (get) Token: 0x06002ABD RID: 10941 RVA: 0x00072C9F File Offset: 0x00071C9F
			public override AccessibleObject Parent
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					return this.owner.LinkingRow.DataGrid.ParentRowsAccessibleObject;
				}
			}

			// Token: 0x17000717 RID: 1815
			// (get) Token: 0x06002ABE RID: 10942 RVA: 0x00072CB6 File Offset: 0x00071CB6
			public override AccessibleRole Role
			{
				get
				{
					return AccessibleRole.ListItem;
				}
			}

			// Token: 0x17000718 RID: 1816
			// (get) Token: 0x06002ABF RID: 10943 RVA: 0x00072CBC File Offset: 0x00071CBC
			public override string Value
			{
				[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					CurrencyManager currencyManager = (CurrencyManager)this.owner.LinkingRow.DataGrid.BindingContext[this.owner.DataSource, this.owner.DataMember];
					stringBuilder.Append(this.owner.ListManager.GetListName());
					stringBuilder.Append(": ");
					bool flag = false;
					foreach (object obj in this.owner.GridColumnStyles)
					{
						DataGridColumnStyle dataGridColumnStyle = (DataGridColumnStyle)obj;
						if (flag)
						{
							stringBuilder.Append(", ");
						}
						string headerText = dataGridColumnStyle.HeaderText;
						string text = dataGridColumnStyle.PropertyDescriptor.Converter.ConvertToString(dataGridColumnStyle.PropertyDescriptor.GetValue(currencyManager.Current));
						stringBuilder.Append(headerText);
						stringBuilder.Append(": ");
						stringBuilder.Append(text);
						flag = true;
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x06002AC0 RID: 10944 RVA: 0x00072DDC File Offset: 0x00071DDC
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			public override AccessibleObject Navigate(AccessibleNavigation navdir)
			{
				DataGridParentRows.DataGridParentRowsAccessibleObject dataGridParentRowsAccessibleObject = (DataGridParentRows.DataGridParentRowsAccessibleObject)this.Parent;
				switch (navdir)
				{
				case AccessibleNavigation.Up:
				case AccessibleNavigation.Left:
				case AccessibleNavigation.Previous:
					return dataGridParentRowsAccessibleObject.GetPrev(this);
				case AccessibleNavigation.Down:
				case AccessibleNavigation.Right:
				case AccessibleNavigation.Next:
					return dataGridParentRowsAccessibleObject.GetNext(this);
				default:
					return null;
				}
			}

			// Token: 0x040017CE RID: 6094
			private DataGridState owner;
		}
	}
}
