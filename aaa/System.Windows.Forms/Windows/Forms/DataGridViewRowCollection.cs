using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x02000381 RID: 897
	[ListBindable(false)]
	[DesignerSerializer("System.Windows.Forms.Design.DataGridViewRowCollectionCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class DataGridViewRowCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x06003718 RID: 14104 RVA: 0x000C6DDA File Offset: 0x000C5DDA
		int IList.Add(object value)
		{
			return this.Add((DataGridViewRow)value);
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x000C6DE8 File Offset: 0x000C5DE8
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000C6DF0 File Offset: 0x000C5DF0
		bool IList.Contains(object value)
		{
			return this.items.Contains(value);
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000C6DFE File Offset: 0x000C5DFE
		int IList.IndexOf(object value)
		{
			return this.items.IndexOf(value);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000C6E0C File Offset: 0x000C5E0C
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (DataGridViewRow)value);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x000C6E1B File Offset: 0x000C5E1B
		void IList.Remove(object value)
		{
			this.Remove((DataGridViewRow)value);
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x000C6E29 File Offset: 0x000C5E29
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x0600371F RID: 14111 RVA: 0x000C6E32 File Offset: 0x000C5E32
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06003720 RID: 14112 RVA: 0x000C6E35 File Offset: 0x000C5E35
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A30 RID: 2608
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x000C6E48 File Offset: 0x000C5E48
		void ICollection.CopyTo(Array array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06003724 RID: 14116 RVA: 0x000C6E57 File Offset: 0x000C5E57
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003725 RID: 14117 RVA: 0x000C6E5F File Offset: 0x000C5E5F
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06003726 RID: 14118 RVA: 0x000C6E62 File Offset: 0x000C5E62
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x000C6E65 File Offset: 0x000C5E65
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DataGridViewRowCollection.UnsharingRowEnumerator(this);
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x000C6E6D File Offset: 0x000C5E6D
		public DataGridViewRowCollection(DataGridView dataGridView)
		{
			this.InvalidateCachedRowCounts();
			this.InvalidateCachedRowsHeights();
			this.dataGridView = dataGridView;
			this.rowStates = new List<DataGridViewElementStates>();
			this.items = new DataGridViewRowCollection.RowArrayList(this);
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06003729 RID: 14121 RVA: 0x000C6E9F File Offset: 0x000C5E9F
		public int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600372A RID: 14122 RVA: 0x000C6EAC File Offset: 0x000C5EAC
		internal bool IsCollectionChangedListenedTo
		{
			get
			{
				return this.onCollectionChanged != null;
			}
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x0600372B RID: 14123 RVA: 0x000C6EBC File Offset: 0x000C5EBC
		protected ArrayList List
		{
			get
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridViewRow dataGridViewRow = this[i];
				}
				return this.items;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600372C RID: 14124 RVA: 0x000C6EEA File Offset: 0x000C5EEA
		internal ArrayList SharedList
		{
			get
			{
				return this.items;
			}
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x000C6EF2 File Offset: 0x000C5EF2
		public DataGridViewRow SharedRow(int rowIndex)
		{
			return (DataGridViewRow)this.SharedList[rowIndex];
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x0600372E RID: 14126 RVA: 0x000C6F05 File Offset: 0x000C5F05
		protected DataGridView DataGridView
		{
			get
			{
				return this.dataGridView;
			}
		}

		// Token: 0x17000A39 RID: 2617
		public DataGridViewRow this[int index]
		{
			get
			{
				DataGridViewRow dataGridViewRow = this.SharedRow(index);
				if (dataGridViewRow.Index != -1)
				{
					return dataGridViewRow;
				}
				if (index == 0 && this.items.Count == 1)
				{
					dataGridViewRow.IndexInternal = 0;
					dataGridViewRow.StateInternal = this.SharedRowState(0);
					if (this.DataGridView != null)
					{
						this.DataGridView.OnRowUnshared(dataGridViewRow);
					}
					return dataGridViewRow;
				}
				DataGridViewRow dataGridViewRow2 = (DataGridViewRow)dataGridViewRow.Clone();
				dataGridViewRow2.IndexInternal = index;
				dataGridViewRow2.DataGridViewInternal = dataGridViewRow.DataGridView;
				dataGridViewRow2.StateInternal = this.SharedRowState(index);
				this.SharedList[index] = dataGridViewRow2;
				int num = 0;
				foreach (object obj in dataGridViewRow2.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = dataGridViewRow.DataGridView;
					dataGridViewCell.OwningRowInternal = dataGridViewRow2;
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
					num++;
				}
				if (dataGridViewRow2.HasHeaderCell)
				{
					dataGridViewRow2.HeaderCell.DataGridViewInternal = dataGridViewRow.DataGridView;
					dataGridViewRow2.HeaderCell.OwningRowInternal = dataGridViewRow2;
				}
				if (this.DataGridView != null)
				{
					this.DataGridView.OnRowUnshared(dataGridViewRow2);
				}
				return dataGridViewRow2;
			}
		}

		// Token: 0x140001D4 RID: 468
		// (add) Token: 0x06003730 RID: 14128 RVA: 0x000C7060 File Offset: 0x000C6060
		// (remove) Token: 0x06003731 RID: 14129 RVA: 0x000C7079 File Offset: 0x000C6079
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000C7094 File Offset: 0x000C6094
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add()
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddInternal(false, null);
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x000C70E4 File Offset: 0x000C60E4
		internal int AddInternal(bool newRow, object[] values)
		{
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.RowTemplate.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_RowTemplateTooManyCells"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			if (newRow)
			{
				rowTemplateClone.StateInternal = rowTemplateClone.State | DataGridViewElementStates.Visible;
				foreach (object obj in rowTemplateClone.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.Value = dataGridViewCell.DefaultNewRowValue;
				}
			}
			if (values != null)
			{
				rowTemplateClone.SetValuesInternal(values);
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.Insert(num, rowTemplateClone);
				return num;
			}
			DataGridViewElementStates state = rowTemplateClone.State;
			this.DataGridView.OnAddingRow(rowTemplateClone, state, true);
			rowTemplateClone.DataGridViewInternal = this.dataGridView;
			int num2 = 0;
			foreach (object obj2 in rowTemplateClone.Cells)
			{
				DataGridViewCell dataGridViewCell2 = (DataGridViewCell)obj2;
				dataGridViewCell2.DataGridViewInternal = this.dataGridView;
				dataGridViewCell2.OwningColumnInternal = this.DataGridView.Columns[num2];
				num2++;
			}
			if (rowTemplateClone.HasHeaderCell)
			{
				rowTemplateClone.HeaderCell.DataGridViewInternal = this.DataGridView;
				rowTemplateClone.HeaderCell.OwningRowInternal = rowTemplateClone;
			}
			int num3 = this.SharedList.Add(rowTemplateClone);
			this.rowStates.Add(state);
			if (values != null || !this.RowIsSharable(num3) || DataGridViewRowCollection.RowHasValueOrToolTipText(rowTemplateClone) || this.IsCollectionChangedListenedTo)
			{
				rowTemplateClone.IndexInternal = num3;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, rowTemplateClone), num3, 1);
			return num3;
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x000C72FC File Offset: 0x000C62FC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add(params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (this.DataGridView.VirtualMode)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationInVirtualMode"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddInternal(false, values);
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x000C7378 File Offset: 0x000C6378
		public virtual int Add(DataGridViewRow dataGridViewRow)
		{
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddInternal(dataGridViewRow);
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x000C73E8 File Offset: 0x000C63E8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int Add(int count)
		{
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.RowTemplate.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_RowTemplateTooManyCells"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			DataGridViewElementStates state = rowTemplateClone.State;
			rowTemplateClone.DataGridViewInternal = this.dataGridView;
			int num = 0;
			foreach (object obj in rowTemplateClone.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			if (rowTemplateClone.HasHeaderCell)
			{
				rowTemplateClone.HeaderCell.DataGridViewInternal = this.dataGridView;
				rowTemplateClone.HeaderCell.OwningRowInternal = rowTemplateClone;
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num2 = this.Count - 1;
				this.InsertCopiesPrivate(rowTemplateClone, state, num2, count);
				return num2 + count - 1;
			}
			return this.AddCopiesPrivate(rowTemplateClone, state, count);
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x000C7590 File Offset: 0x000C6590
		internal int AddInternal(DataGridViewRow dataGridViewRow)
		{
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			if (dataGridViewRow.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowAlreadyBelongsToDataGridView"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (dataGridViewRow.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new ArgumentException(SR.GetString("DataGridViewRowCollection_TooManyCells"), "dataGridViewRow");
			}
			if (dataGridViewRow.Selected)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CannotAddOrInsertSelectedRow"));
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.InsertInternal(num, dataGridViewRow);
				return num;
			}
			this.DataGridView.CompleteCellsCollection(dataGridViewRow);
			this.DataGridView.OnAddingRow(dataGridViewRow, dataGridViewRow.State, true);
			int num2 = 0;
			foreach (object obj in dataGridViewRow.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				if (dataGridViewCell.ColumnIndex == -1)
				{
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num2];
				}
				num2++;
			}
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.DataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			int num3 = this.SharedList.Add(dataGridViewRow);
			this.rowStates.Add(dataGridViewRow.State);
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			if (!this.RowIsSharable(num3) || DataGridViewRowCollection.RowHasValueOrToolTipText(dataGridViewRow) || this.IsCollectionChangedListenedTo)
			{
				dataGridViewRow.IndexInternal = num3;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewRow), num3, 1);
			return num3;
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x000C776C File Offset: 0x000C676C
		public virtual int AddCopy(int indexSource)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddCopyInternal(indexSource, DataGridViewElementStates.None, DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected, false);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x000C77C0 File Offset: 0x000C67C0
		internal int AddCopyInternal(int indexSource, DataGridViewElementStates dgvesAdd, DataGridViewElementStates dgvesRemove, bool newRow)
		{
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.InsertCopy(indexSource, num);
				return num;
			}
			if (indexSource < 0 || indexSource >= this.Count)
			{
				throw new ArgumentOutOfRangeException("indexSource", SR.GetString("DataGridViewRowCollection_IndexSourceOutOfRange"));
			}
			DataGridViewRow dataGridViewRow = this.SharedRow(indexSource);
			int num2;
			if (dataGridViewRow.Index == -1 && !this.IsCollectionChangedListenedTo && !newRow)
			{
				DataGridViewElementStates dataGridViewElementStates = this.rowStates[indexSource] & ~dgvesRemove;
				dataGridViewElementStates |= dgvesAdd;
				this.DataGridView.OnAddingRow(dataGridViewRow, dataGridViewElementStates, true);
				num2 = this.SharedList.Add(dataGridViewRow);
				this.rowStates.Add(dataGridViewElementStates);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewRow), num2, 1);
				return num2;
			}
			num2 = this.AddDuplicateRow(dataGridViewRow, newRow);
			if (!this.RowIsSharable(num2) || DataGridViewRowCollection.RowHasValueOrToolTipText(this.SharedRow(num2)) || this.IsCollectionChangedListenedTo)
			{
				this.UnshareRow(num2);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(num2)), num2, 1);
			return num2;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x000C78C0 File Offset: 0x000C68C0
		public virtual int AddCopies(int indexSource, int count)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			return this.AddCopiesInternal(indexSource, count);
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000C7910 File Offset: 0x000C6910
		internal int AddCopiesInternal(int indexSource, int count)
		{
			if (this.DataGridView.NewRowIndex != -1)
			{
				int num = this.Count - 1;
				this.InsertCopiesPrivate(indexSource, num, count);
				return num + count - 1;
			}
			return this.AddCopiesInternal(indexSource, count, DataGridViewElementStates.None, DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000C7950 File Offset: 0x000C6950
		internal int AddCopiesInternal(int indexSource, int count, DataGridViewElementStates dgvesAdd, DataGridViewElementStates dgvesRemove)
		{
			if (indexSource < 0 || this.Count <= indexSource)
			{
				throw new ArgumentOutOfRangeException("indexSource", SR.GetString("DataGridViewRowCollection_IndexSourceOutOfRange"));
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			DataGridViewElementStates dataGridViewElementStates = this.rowStates[indexSource] & ~dgvesRemove;
			dataGridViewElementStates |= dgvesAdd;
			return this.AddCopiesPrivate(this.SharedRow(indexSource), dataGridViewElementStates, count);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000C79BC File Offset: 0x000C69BC
		private int AddCopiesPrivate(DataGridViewRow rowTemplate, DataGridViewElementStates rowTemplateState, int count)
		{
			int count2 = this.items.Count;
			int num;
			if (rowTemplate.Index == -1)
			{
				this.DataGridView.OnAddingRow(rowTemplate, rowTemplateState, true);
				for (int i = 0; i < count - 1; i++)
				{
					this.SharedList.Add(rowTemplate);
					this.rowStates.Add(rowTemplateState);
				}
				num = this.SharedList.Add(rowTemplate);
				this.rowStates.Add(rowTemplateState);
				this.DataGridView.OnAddedRow_PreNotification(num);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), count2, count);
				for (int j = 0; j < count; j++)
				{
					this.DataGridView.OnAddedRow_PostNotification(num - (count - 1) + j);
				}
				return num;
			}
			num = this.AddDuplicateRow(rowTemplate, false);
			if (count > 1)
			{
				this.DataGridView.OnAddedRow_PreNotification(num);
				if (this.RowIsSharable(num))
				{
					DataGridViewRow dataGridViewRow = this.SharedRow(num);
					this.DataGridView.OnAddingRow(dataGridViewRow, rowTemplateState, true);
					for (int k = 1; k < count - 1; k++)
					{
						this.SharedList.Add(dataGridViewRow);
						this.rowStates.Add(rowTemplateState);
					}
					num = this.SharedList.Add(dataGridViewRow);
					this.rowStates.Add(rowTemplateState);
					this.DataGridView.OnAddedRow_PreNotification(num);
				}
				else
				{
					this.UnshareRow(num);
					for (int l = 1; l < count; l++)
					{
						num = this.AddDuplicateRow(rowTemplate, false);
						this.UnshareRow(num);
						this.DataGridView.OnAddedRow_PreNotification(num);
					}
				}
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), count2, count);
				for (int m = 0; m < count; m++)
				{
					this.DataGridView.OnAddedRow_PostNotification(num - (count - 1) + m);
				}
				return num;
			}
			if (this.IsCollectionChangedListenedTo)
			{
				this.UnshareRow(num);
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(num)), num, 1);
			return num;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000C7B88 File Offset: 0x000C6B88
		private int AddDuplicateRow(DataGridViewRow rowTemplate, bool newRow)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)rowTemplate.Clone();
			dataGridViewRow.StateInternal = DataGridViewElementStates.None;
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			int num = 0;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				if (newRow)
				{
					dataGridViewCell.Value = dataGridViewCell.DefaultNewRowValue;
				}
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			DataGridViewElementStates dataGridViewElementStates = rowTemplate.State & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.dataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			this.DataGridView.OnAddingRow(dataGridViewRow, dataGridViewElementStates, true);
			this.rowStates.Add(dataGridViewElementStates);
			return this.SharedList.Add(dataGridViewRow);
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000C7C94 File Offset: 0x000C6C94
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual void AddRange(params DataGridViewRow[] dataGridViewRows)
		{
			if (dataGridViewRows == null)
			{
				throw new ArgumentNullException("dataGridViewRows");
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NewRowIndex != -1)
			{
				this.InsertRange(this.Count - 1, dataGridViewRows);
				return;
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			int count = this.items.Count;
			this.DataGridView.OnAddingRows(dataGridViewRows, true);
			foreach (DataGridViewRow dataGridViewRow in dataGridViewRows)
			{
				int num = 0;
				foreach (object obj in dataGridViewRow.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = this.dataGridView;
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
					num++;
				}
				if (dataGridViewRow.HasHeaderCell)
				{
					dataGridViewRow.HeaderCell.DataGridViewInternal = this.dataGridView;
					dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
				}
				int num2 = this.SharedList.Add(dataGridViewRow);
				this.rowStates.Add(dataGridViewRow.State);
				dataGridViewRow.IndexInternal = num2;
				dataGridViewRow.DataGridViewInternal = this.dataGridView;
			}
			this.DataGridView.OnAddedRows_PreNotification(dataGridViewRows);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), count, dataGridViewRows.Length);
			this.DataGridView.OnAddedRows_PostNotification(dataGridViewRows);
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x000C7E5C File Offset: 0x000C6E5C
		public virtual void Clear()
		{
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.DataSource == null)
			{
				this.ClearInternal(true);
				return;
			}
			IBindingList bindingList = this.DataGridView.DataConnection.List as IBindingList;
			if (bindingList != null && bindingList.AllowRemove && bindingList.SupportsChangeNotification)
			{
				bindingList.Clear();
				return;
			}
			throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CantClearRowCollectionWithWrongSource"));
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000C7EDC File Offset: 0x000C6EDC
		internal void ClearInternal(bool recreateNewRow)
		{
			int count = this.items.Count;
			if (count > 0)
			{
				this.DataGridView.OnClearingRows();
				for (int i = 0; i < count; i++)
				{
					this.SharedRow(i).DetachFromDataGridView();
				}
				this.SharedList.Clear();
				this.rowStates.Clear();
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), 0, count, true, false, recreateNewRow, new Point(-1, -1));
				return;
			}
			if (recreateNewRow && this.DataGridView.Columns.Count != 0 && this.DataGridView.AllowUserToAddRowsInternal && this.items.Count == 0)
			{
				this.DataGridView.AddNewRow(false);
			}
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000C7F87 File Offset: 0x000C6F87
		public virtual bool Contains(DataGridViewRow dataGridViewRow)
		{
			return this.items.IndexOf(dataGridViewRow) != -1;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000C7F9B File Offset: 0x000C6F9B
		public void CopyTo(DataGridViewRow[] array, int index)
		{
			this.items.CopyTo(array, index);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000C7FAC File Offset: 0x000C6FAC
		internal int DisplayIndexToRowIndex(int visibleRowIndex)
		{
			int num = -1;
			for (int i = 0; i < this.Count; i++)
			{
				if ((this.GetRowState(i) & DataGridViewElementStates.Visible) == DataGridViewElementStates.Visible)
				{
					num++;
				}
				if (num == visibleRowIndex)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x000C7FE8 File Offset: 0x000C6FE8
		public int GetFirstRow(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				if (this.rowCountsVisible == 0)
				{
					return -1;
				}
				break;
			case DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible:
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				if (this.rowCountsVisibleFrozen == 0)
				{
					return -1;
				}
				break;
			default:
				if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
				{
					if (this.rowCountsVisibleSelected == 0)
					{
						return -1;
					}
				}
				break;
			}
			int num = 0;
			while (num < this.items.Count && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x000C808C File Offset: 0x000C708C
		public int GetFirstRow(DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetFirstRow(includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				if (this.rowCountsVisible == 0)
				{
					return -1;
				}
				break;
			case DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible:
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				if (this.rowCountsVisibleFrozen == 0)
				{
					return -1;
				}
				break;
			default:
				if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
				{
					if (this.rowCountsVisibleSelected == 0)
					{
						return -1;
					}
				}
				break;
			}
			int num = 0;
			while (num < this.items.Count && ((this.GetRowState(num) & includeFilter) != includeFilter || (this.GetRowState(num) & excludeFilter) != DataGridViewElementStates.None))
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x000C816C File Offset: 0x000C716C
		public int GetLastRow(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				if (this.rowCountsVisible == 0)
				{
					return -1;
				}
				break;
			case DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible:
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				if (this.rowCountsVisibleFrozen == 0)
				{
					return -1;
				}
				break;
			default:
				if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
				{
					if (this.rowCountsVisibleSelected == 0)
					{
						return -1;
					}
				}
				break;
			}
			int num = this.items.Count - 1;
			while (num >= 0 && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num--;
			}
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x000C8208 File Offset: 0x000C7208
		internal int GetNextRow(int indexStart, DataGridViewElementStates includeFilter, int skipRows)
		{
			int num = indexStart;
			do
			{
				num = this.GetNextRow(num, includeFilter);
				skipRows--;
			}
			while (skipRows >= 0 && num != -1);
			return num;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x000C8230 File Offset: 0x000C7230
		public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if (indexStart < -1)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					(-1).ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart + 1;
			while (num < this.items.Count && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x000C82E4 File Offset: 0x000C72E4
		public int GetNextRow(int indexStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetNextRow(indexStart, includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			if (indexStart < -1)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					(-1).ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart + 1;
			while (num < this.items.Count && ((this.GetRowState(num) & includeFilter) != includeFilter || (this.GetRowState(num) & excludeFilter) != DataGridViewElementStates.None))
			{
				num++;
			}
			if (num >= this.items.Count)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x000C83D4 File Offset: 0x000C73D4
		public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if (indexStart > this.items.Count)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					this.items.Count.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart - 1;
			while (num >= 0 && (this.GetRowState(num) & includeFilter) != includeFilter)
			{
				num--;
			}
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x000C8488 File Offset: 0x000C7488
		public int GetPreviousRow(int indexStart, DataGridViewElementStates includeFilter, DataGridViewElementStates excludeFilter)
		{
			if (excludeFilter == DataGridViewElementStates.None)
			{
				return this.GetPreviousRow(indexStart, includeFilter);
			}
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			if ((excludeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "excludeFilter" }));
			}
			if (indexStart > this.items.Count)
			{
				throw new ArgumentOutOfRangeException("indexStart", SR.GetString("InvalidHighBoundArgumentEx", new object[]
				{
					"indexStart",
					indexStart.ToString(CultureInfo.CurrentCulture),
					this.items.Count.ToString(CultureInfo.CurrentCulture)
				}));
			}
			int num = indexStart - 1;
			while (num >= 0 && ((this.GetRowState(num) & includeFilter) != includeFilter || (this.GetRowState(num) & excludeFilter) != DataGridViewElementStates.None))
			{
				num--;
			}
			if (num < 0)
			{
				return -1;
			}
			return num;
		}

		// Token: 0x0600374D RID: 14157 RVA: 0x000C8578 File Offset: 0x000C7578
		public int GetRowCount(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				if (this.rowCountsVisible != -1)
				{
					return this.rowCountsVisible;
				}
				break;
			case DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible:
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				if (this.rowCountsVisibleFrozen != -1)
				{
					return this.rowCountsVisibleFrozen;
				}
				break;
			default:
				if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
				{
					if (this.rowCountsVisibleSelected != -1)
					{
						return this.rowCountsVisibleSelected;
					}
				}
				break;
			}
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num++;
				}
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				this.rowCountsVisible = num;
				break;
			case DataGridViewElementStates.Displayed | DataGridViewElementStates.Visible:
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				this.rowCountsVisibleFrozen = num;
				break;
			default:
				if (includeFilter == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible))
				{
					this.rowCountsVisibleSelected = num;
				}
				break;
			}
			return num;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x000C865C File Offset: 0x000C765C
		internal int GetRowCount(DataGridViewElementStates includeFilter, int fromRowIndex, int toRowIndex)
		{
			int num = 0;
			for (int i = fromRowIndex + 1; i <= toRowIndex; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x000C868C File Offset: 0x000C768C
		public int GetRowsHeight(DataGridViewElementStates includeFilter)
		{
			if ((includeFilter & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) != DataGridViewElementStates.None)
			{
				throw new ArgumentException(SR.GetString("DataGridView_InvalidDataGridViewElementStateCombination", new object[] { "includeFilter" }));
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				if (this.rowsHeightVisible != -1)
				{
					return this.rowsHeightVisible;
				}
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				if (this.rowsHeightVisibleFrozen != -1)
				{
					return this.rowsHeightVisibleFrozen;
				}
				break;
			}
			int num = 0;
			for (int i = 0; i < this.items.Count; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num += ((DataGridViewRow)this.items[i]).GetHeight(i);
				}
			}
			switch (includeFilter)
			{
			case DataGridViewElementStates.Visible:
				this.rowsHeightVisible = num;
				break;
			case DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible:
				this.rowsHeightVisibleFrozen = num;
				break;
			}
			return num;
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x000C8764 File Offset: 0x000C7764
		internal int GetRowsHeight(DataGridViewElementStates includeFilter, int fromRowIndex, int toRowIndex)
		{
			int num = 0;
			for (int i = fromRowIndex; i < toRowIndex; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num += ((DataGridViewRow)this.items[i]).GetHeight(i);
				}
			}
			return num;
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x000C87A8 File Offset: 0x000C77A8
		private bool GetRowsHeightExceedLimit(DataGridViewElementStates includeFilter, int fromRowIndex, int toRowIndex, int heightLimit)
		{
			int num = 0;
			for (int i = fromRowIndex; i < toRowIndex; i++)
			{
				if ((this.GetRowState(i) & includeFilter) == includeFilter)
				{
					num += ((DataGridViewRow)this.items[i]).GetHeight(i);
					if (num > heightLimit)
					{
						return true;
					}
				}
			}
			return num > heightLimit;
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x000C87F8 File Offset: 0x000C77F8
		public virtual DataGridViewElementStates GetRowState(int rowIndex)
		{
			if (rowIndex < 0 || rowIndex >= this.items.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_RowIndexOutOfRange"));
			}
			DataGridViewRow dataGridViewRow = this.SharedRow(rowIndex);
			if (dataGridViewRow.Index == -1)
			{
				return this.SharedRowState(rowIndex);
			}
			return dataGridViewRow.GetState(rowIndex);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x000C884C File Offset: 0x000C784C
		public int IndexOf(DataGridViewRow dataGridViewRow)
		{
			return this.items.IndexOf(dataGridViewRow);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x000C885C File Offset: 0x000C785C
		public virtual void Insert(int rowIndex, params object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException("values");
			}
			if (this.DataGridView.VirtualMode)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_InvalidOperationInVirtualMode"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			rowTemplateClone.SetValuesInternal(values);
			this.Insert(rowIndex, rowTemplateClone);
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x000C88D0 File Offset: 0x000C78D0
		public virtual void Insert(int rowIndex, DataGridViewRow dataGridViewRow)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			this.InsertInternal(rowIndex, dataGridViewRow);
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000C8920 File Offset: 0x000C7920
		public virtual void Insert(int rowIndex, int count)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (rowIndex < 0 || this.Count < rowIndex)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_IndexDestinationOutOfRange"));
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (this.DataGridView.RowTemplate.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_RowTemplateTooManyCells"));
			}
			if (this.DataGridView.NewRowIndex != -1 && rowIndex == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			DataGridViewRow rowTemplateClone = this.DataGridView.RowTemplateClone;
			DataGridViewElementStates state = rowTemplateClone.State;
			rowTemplateClone.DataGridViewInternal = this.dataGridView;
			int num = 0;
			foreach (object obj in rowTemplateClone.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			if (rowTemplateClone.HasHeaderCell)
			{
				rowTemplateClone.HeaderCell.DataGridViewInternal = this.dataGridView;
				rowTemplateClone.HeaderCell.OwningRowInternal = rowTemplateClone;
			}
			this.InsertCopiesPrivate(rowTemplateClone, state, rowIndex, count);
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000C8AE8 File Offset: 0x000C7AE8
		internal void InsertInternal(int rowIndex, DataGridViewRow dataGridViewRow)
		{
			if (rowIndex < 0 || this.Count < rowIndex)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_RowIndexOutOfRange"));
			}
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			if (dataGridViewRow.DataGridView != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_RowAlreadyBelongsToDataGridView"));
			}
			if (this.DataGridView.NewRowIndex != -1 && rowIndex == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			if (dataGridViewRow.Cells.Count > this.DataGridView.Columns.Count)
			{
				throw new ArgumentException(SR.GetString("DataGridViewRowCollection_TooManyCells"), "dataGridViewRow");
			}
			if (dataGridViewRow.Selected)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CannotAddOrInsertSelectedRow"));
			}
			this.InsertInternal(rowIndex, dataGridViewRow, false);
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000C8BDC File Offset: 0x000C7BDC
		internal void InsertInternal(int rowIndex, DataGridViewRow dataGridViewRow, bool force)
		{
			Point point = new Point(-1, -1);
			if (force)
			{
				if (this.DataGridView.Columns.Count == 0)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
				}
				if (dataGridViewRow.Cells.Count > this.DataGridView.Columns.Count)
				{
					throw new ArgumentException(SR.GetString("DataGridViewRowCollection_TooManyCells"), "dataGridViewRow");
				}
			}
			this.DataGridView.CompleteCellsCollection(dataGridViewRow);
			this.DataGridView.OnInsertingRow(rowIndex, dataGridViewRow, dataGridViewRow.State, ref point, true, 1, force);
			int num = 0;
			foreach (object obj in dataGridViewRow.Cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				if (dataGridViewCell.ColumnIndex == -1)
				{
					dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				}
				num++;
			}
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.DataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			this.SharedList.Insert(rowIndex, dataGridViewRow);
			this.rowStates.Insert(rowIndex, dataGridViewRow.State);
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			if (!this.RowIsSharable(rowIndex) || DataGridViewRowCollection.RowHasValueOrToolTipText(dataGridViewRow) || this.IsCollectionChangedListenedTo)
			{
				dataGridViewRow.IndexInternal = rowIndex;
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, dataGridViewRow), rowIndex, 1, false, true, false, point);
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x000C8D70 File Offset: 0x000C7D70
		public virtual void InsertCopy(int indexSource, int indexDestination)
		{
			this.InsertCopies(indexSource, indexDestination, 1);
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000C8D7C File Offset: 0x000C7D7C
		public virtual void InsertCopies(int indexSource, int indexDestination, int count)
		{
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			this.InsertCopiesPrivate(indexSource, indexDestination, count);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000C8DCC File Offset: 0x000C7DCC
		private void InsertCopiesPrivate(int indexSource, int indexDestination, int count)
		{
			if (indexSource < 0 || this.Count <= indexSource)
			{
				throw new ArgumentOutOfRangeException("indexSource", SR.GetString("DataGridViewRowCollection_IndexSourceOutOfRange"));
			}
			if (indexDestination < 0 || this.Count < indexDestination)
			{
				throw new ArgumentOutOfRangeException("indexDestination", SR.GetString("DataGridViewRowCollection_IndexDestinationOutOfRange"));
			}
			if (count <= 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("DataGridViewRowCollection_CountOutOfRange"));
			}
			if (this.DataGridView.NewRowIndex != -1 && indexDestination == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			DataGridViewElementStates dataGridViewElementStates = this.GetRowState(indexSource) & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			this.InsertCopiesPrivate(this.SharedRow(indexSource), dataGridViewElementStates, indexDestination, count);
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x000C8E78 File Offset: 0x000C7E78
		private void InsertCopiesPrivate(DataGridViewRow rowTemplate, DataGridViewElementStates rowTemplateState, int indexDestination, int count)
		{
			Point point = new Point(-1, -1);
			if (rowTemplate.Index == -1)
			{
				if (count > 1)
				{
					this.DataGridView.OnInsertingRow(indexDestination, rowTemplate, rowTemplateState, ref point, true, count, false);
					for (int i = 0; i < count; i++)
					{
						this.SharedList.Insert(indexDestination + i, rowTemplate);
						this.rowStates.Insert(indexDestination + i, rowTemplateState);
					}
					this.DataGridView.OnInsertedRow_PreNotification(indexDestination, count);
					this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), indexDestination, count, false, true, false, point);
					for (int j = 0; j < count; j++)
					{
						this.DataGridView.OnInsertedRow_PostNotification(indexDestination + j, point, j == count - 1);
					}
					return;
				}
				this.DataGridView.OnInsertingRow(indexDestination, rowTemplate, rowTemplateState, ref point, true, 1, false);
				this.SharedList.Insert(indexDestination, rowTemplate);
				this.rowStates.Insert(indexDestination, rowTemplateState);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(indexDestination)), indexDestination, count, false, true, false, point);
				return;
			}
			else
			{
				this.InsertDuplicateRow(indexDestination, rowTemplate, true, ref point);
				if (count > 1)
				{
					this.DataGridView.OnInsertedRow_PreNotification(indexDestination, 1);
					if (this.RowIsSharable(indexDestination))
					{
						DataGridViewRow dataGridViewRow = this.SharedRow(indexDestination);
						this.DataGridView.OnInsertingRow(indexDestination + 1, dataGridViewRow, rowTemplateState, ref point, false, count - 1, false);
						for (int k = 1; k < count; k++)
						{
							this.SharedList.Insert(indexDestination + k, dataGridViewRow);
							this.rowStates.Insert(indexDestination + k, rowTemplateState);
						}
						this.DataGridView.OnInsertedRow_PreNotification(indexDestination + 1, count - 1);
						this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), indexDestination, count, false, true, false, point);
					}
					else
					{
						this.UnshareRow(indexDestination);
						for (int l = 1; l < count; l++)
						{
							this.InsertDuplicateRow(indexDestination + l, rowTemplate, false, ref point);
							this.UnshareRow(indexDestination + l);
						}
						this.DataGridView.OnInsertedRow_PreNotification(indexDestination + 1, count - 1);
						this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), indexDestination, count, false, true, false, point);
					}
					for (int m = 0; m < count; m++)
					{
						this.DataGridView.OnInsertedRow_PostNotification(indexDestination + m, point, m == count - 1);
					}
					return;
				}
				if (this.IsCollectionChangedListenedTo)
				{
					this.UnshareRow(indexDestination);
				}
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, this.SharedRow(indexDestination)), indexDestination, 1, false, true, false, point);
				return;
			}
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000C90C0 File Offset: 0x000C80C0
		private void InsertDuplicateRow(int indexDestination, DataGridViewRow rowTemplate, bool firstInsertion, ref Point newCurrentCell)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)rowTemplate.Clone();
			dataGridViewRow.StateInternal = DataGridViewElementStates.None;
			dataGridViewRow.DataGridViewInternal = this.dataGridView;
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			int num = 0;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				dataGridViewCell.DataGridViewInternal = this.dataGridView;
				dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num];
				num++;
			}
			DataGridViewElementStates dataGridViewElementStates = rowTemplate.State & ~(DataGridViewElementStates.Displayed | DataGridViewElementStates.Selected);
			if (dataGridViewRow.HasHeaderCell)
			{
				dataGridViewRow.HeaderCell.DataGridViewInternal = this.dataGridView;
				dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
			}
			this.DataGridView.OnInsertingRow(indexDestination, dataGridViewRow, dataGridViewElementStates, ref newCurrentCell, firstInsertion, 1, false);
			this.SharedList.Insert(indexDestination, dataGridViewRow);
			this.rowStates.Insert(indexDestination, dataGridViewElementStates);
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000C91C4 File Offset: 0x000C81C4
		public virtual void InsertRange(int rowIndex, params DataGridViewRow[] dataGridViewRows)
		{
			if (dataGridViewRows == null)
			{
				throw new ArgumentNullException("dataGridViewRows");
			}
			if (dataGridViewRows.Length == 1)
			{
				this.Insert(rowIndex, dataGridViewRows[0]);
				return;
			}
			if (rowIndex < 0 || rowIndex > this.Count)
			{
				throw new ArgumentOutOfRangeException("rowIndex", SR.GetString("DataGridViewRowCollection_IndexDestinationOutOfRange"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.NewRowIndex != -1 && rowIndex == this.Count)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoInsertionAfterNewRow"));
			}
			if (this.DataGridView.DataSource != null)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_AddUnboundRow"));
			}
			if (this.DataGridView.Columns.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_NoColumns"));
			}
			Point point = new Point(-1, -1);
			this.DataGridView.OnInsertingRows(rowIndex, dataGridViewRows, ref point);
			int num = rowIndex;
			foreach (DataGridViewRow dataGridViewRow in dataGridViewRows)
			{
				int num2 = 0;
				foreach (object obj in dataGridViewRow.Cells)
				{
					DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
					dataGridViewCell.DataGridViewInternal = this.dataGridView;
					if (dataGridViewCell.ColumnIndex == -1)
					{
						dataGridViewCell.OwningColumnInternal = this.DataGridView.Columns[num2];
					}
					num2++;
				}
				if (dataGridViewRow.HasHeaderCell)
				{
					dataGridViewRow.HeaderCell.DataGridViewInternal = this.DataGridView;
					dataGridViewRow.HeaderCell.OwningRowInternal = dataGridViewRow;
				}
				this.SharedList.Insert(num, dataGridViewRow);
				this.rowStates.Insert(num, dataGridViewRow.State);
				dataGridViewRow.IndexInternal = num;
				dataGridViewRow.DataGridViewInternal = this.dataGridView;
				num++;
			}
			this.DataGridView.OnInsertedRows_PreNotification(rowIndex, dataGridViewRows);
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null), rowIndex, dataGridViewRows.Length, false, true, false, point);
			this.DataGridView.OnInsertedRows_PostNotification(dataGridViewRows, point);
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x000C93E4 File Offset: 0x000C83E4
		internal void InvalidateCachedRowCount(DataGridViewElementStates includeFilter)
		{
			if (includeFilter == DataGridViewElementStates.Visible)
			{
				this.InvalidateCachedRowCounts();
				return;
			}
			if (includeFilter == DataGridViewElementStates.Frozen)
			{
				this.rowCountsVisibleFrozen = -1;
				return;
			}
			if (includeFilter == DataGridViewElementStates.Selected)
			{
				this.rowCountsVisibleSelected = -1;
			}
		}

		// Token: 0x06003760 RID: 14176 RVA: 0x000C940C File Offset: 0x000C840C
		internal void InvalidateCachedRowCounts()
		{
			this.rowCountsVisible = (this.rowCountsVisibleFrozen = (this.rowCountsVisibleSelected = -1));
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000C9432 File Offset: 0x000C8432
		internal void InvalidateCachedRowsHeight(DataGridViewElementStates includeFilter)
		{
			if (includeFilter == DataGridViewElementStates.Visible)
			{
				this.InvalidateCachedRowsHeights();
				return;
			}
			if (includeFilter == DataGridViewElementStates.Frozen)
			{
				this.rowsHeightVisibleFrozen = -1;
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000C944C File Offset: 0x000C844C
		internal void InvalidateCachedRowsHeights()
		{
			this.rowsHeightVisible = (this.rowsHeightVisibleFrozen = -1);
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x000C9469 File Offset: 0x000C8469
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, e);
			}
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000C9480 File Offset: 0x000C8480
		private void OnCollectionChanged(CollectionChangeEventArgs e, int rowIndex, int rowCount)
		{
			Point point = new Point(-1, -1);
			DataGridViewRow dataGridViewRow = (DataGridViewRow)e.Element;
			int num = 0;
			if (dataGridViewRow != null && e.Action == CollectionChangeAction.Add)
			{
				num = this.SharedRow(rowIndex).Index;
			}
			this.OnCollectionChanged_PreNotification(e.Action, rowIndex, rowCount, ref dataGridViewRow, false);
			if (num == -1 && this.SharedRow(rowIndex).Index != -1)
			{
				e = new CollectionChangeEventArgs(e.Action, dataGridViewRow);
			}
			this.OnCollectionChanged(e);
			this.OnCollectionChanged_PostNotification(e.Action, rowIndex, rowCount, dataGridViewRow, false, false, false, point);
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000C950C File Offset: 0x000C850C
		private void OnCollectionChanged(CollectionChangeEventArgs e, int rowIndex, int rowCount, bool changeIsDeletion, bool changeIsInsertion, bool recreateNewRow, Point newCurrentCell)
		{
			DataGridViewRow dataGridViewRow = (DataGridViewRow)e.Element;
			int num = 0;
			if (dataGridViewRow != null && e.Action == CollectionChangeAction.Add)
			{
				num = this.SharedRow(rowIndex).Index;
			}
			this.OnCollectionChanged_PreNotification(e.Action, rowIndex, rowCount, ref dataGridViewRow, changeIsInsertion);
			if (num == -1 && this.SharedRow(rowIndex).Index != -1)
			{
				e = new CollectionChangeEventArgs(e.Action, dataGridViewRow);
			}
			this.OnCollectionChanged(e);
			this.OnCollectionChanged_PostNotification(e.Action, rowIndex, rowCount, dataGridViewRow, changeIsDeletion, changeIsInsertion, recreateNewRow, newCurrentCell);
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x000C9594 File Offset: 0x000C8594
		private void OnCollectionChanged_PreNotification(CollectionChangeAction cca, int rowIndex, int rowCount, ref DataGridViewRow dataGridViewRow, bool changeIsInsertion)
		{
			bool flag = false;
			bool flag2 = false;
			switch (cca)
			{
			case CollectionChangeAction.Add:
			{
				int num = 0;
				this.UpdateRowCaches(rowIndex, ref dataGridViewRow, true);
				if ((this.GetRowState(rowIndex) & DataGridViewElementStates.Visible) == DataGridViewElementStates.None)
				{
					flag = true;
					flag2 = changeIsInsertion;
				}
				else
				{
					int firstDisplayedRowIndex = this.DataGridView.FirstDisplayedRowIndex;
					if (firstDisplayedRowIndex != -1)
					{
						num = this.SharedRow(firstDisplayedRowIndex).GetHeight(firstDisplayedRowIndex);
					}
				}
				if (changeIsInsertion)
				{
					this.DataGridView.OnInsertedRow_PreNotification(rowIndex, 1);
					if (!flag)
					{
						if ((this.GetRowState(rowIndex) & DataGridViewElementStates.Frozen) != DataGridViewElementStates.None)
						{
							flag = this.DataGridView.FirstDisplayedScrollingRowIndex == -1 && this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height);
						}
						else if (this.DataGridView.FirstDisplayedScrollingRowIndex != -1 && rowIndex > this.DataGridView.FirstDisplayedScrollingRowIndex)
						{
							flag = this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height + this.DataGridView.VerticalScrollingOffset) && num <= this.DataGridView.LayoutInfo.Data.Height;
						}
					}
				}
				else
				{
					this.DataGridView.OnAddedRow_PreNotification(rowIndex);
					if (!flag)
					{
						int num2 = this.GetRowsHeight(DataGridViewElementStates.Visible) - this.DataGridView.VerticalScrollingOffset - dataGridViewRow.GetHeight(rowIndex);
						dataGridViewRow = this.SharedRow(rowIndex);
						flag = this.DataGridView.LayoutInfo.Data.Height < num2 && num <= this.DataGridView.LayoutInfo.Data.Height;
					}
				}
				break;
			}
			case CollectionChangeAction.Remove:
			{
				DataGridViewElementStates rowState = this.GetRowState(rowIndex);
				bool flag3 = (rowState & DataGridViewElementStates.Visible) != DataGridViewElementStates.None;
				bool flag4 = (rowState & DataGridViewElementStates.Frozen) != DataGridViewElementStates.None;
				this.rowStates.RemoveAt(rowIndex);
				this.SharedList.RemoveAt(rowIndex);
				this.DataGridView.OnRemovedRow_PreNotification(rowIndex);
				if (flag3)
				{
					if (flag4)
					{
						flag = this.DataGridView.FirstDisplayedScrollingRowIndex == -1 && this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height + SystemInformation.HorizontalScrollBarHeight);
					}
					else if (this.DataGridView.FirstDisplayedScrollingRowIndex != -1 && rowIndex > this.DataGridView.FirstDisplayedScrollingRowIndex)
					{
						int num3 = 0;
						int firstDisplayedRowIndex2 = this.DataGridView.FirstDisplayedRowIndex;
						if (firstDisplayedRowIndex2 != -1)
						{
							num3 = this.SharedRow(firstDisplayedRowIndex2).GetHeight(firstDisplayedRowIndex2);
						}
						flag = this.GetRowsHeightExceedLimit(DataGridViewElementStates.Visible, 0, rowIndex, this.DataGridView.LayoutInfo.Data.Height + this.DataGridView.VerticalScrollingOffset + SystemInformation.HorizontalScrollBarHeight) && num3 <= this.DataGridView.LayoutInfo.Data.Height;
					}
				}
				else
				{
					flag = true;
				}
				break;
			}
			case CollectionChangeAction.Refresh:
				this.InvalidateCachedRowCounts();
				this.InvalidateCachedRowsHeights();
				break;
			}
			this.DataGridView.ResetUIState(flag, flag2);
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x000C9890 File Offset: 0x000C8890
		private void OnCollectionChanged_PostNotification(CollectionChangeAction cca, int rowIndex, int rowCount, DataGridViewRow dataGridViewRow, bool changeIsDeletion, bool changeIsInsertion, bool recreateNewRow, Point newCurrentCell)
		{
			if (changeIsDeletion)
			{
				this.DataGridView.OnRowsRemovedInternal(rowIndex, rowCount);
			}
			else
			{
				this.DataGridView.OnRowsAddedInternal(rowIndex, rowCount);
			}
			switch (cca)
			{
			case CollectionChangeAction.Add:
				if (changeIsInsertion)
				{
					this.DataGridView.OnInsertedRow_PostNotification(rowIndex, newCurrentCell, true);
				}
				else
				{
					this.DataGridView.OnAddedRow_PostNotification(rowIndex);
				}
				break;
			case CollectionChangeAction.Remove:
				this.DataGridView.OnRemovedRow_PostNotification(dataGridViewRow, newCurrentCell);
				break;
			case CollectionChangeAction.Refresh:
				if (changeIsDeletion)
				{
					this.DataGridView.OnClearedRows();
				}
				break;
			}
			this.DataGridView.OnRowCollectionChanged_PostNotification(recreateNewRow, newCurrentCell.X == -1, cca, dataGridViewRow, rowIndex);
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x000C9934 File Offset: 0x000C8934
		public virtual void Remove(DataGridViewRow dataGridViewRow)
		{
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			if (dataGridViewRow.DataGridView != this.DataGridView)
			{
				throw new ArgumentException(SR.GetString("DataGridView_RowDoesNotBelongToDataGridView"), "dataGridViewRow");
			}
			if (dataGridViewRow.Index == -1)
			{
				throw new ArgumentException(SR.GetString("DataGridView_RowMustBeUnshared"), "dataGridViewRow");
			}
			this.RemoveAt(dataGridViewRow.Index);
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000C999C File Offset: 0x000C899C
		public virtual void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("DataGridViewRowCollection_RowIndexOutOfRange"));
			}
			if (this.DataGridView.NewRowIndex == index)
			{
				throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CannotDeleteNewRow"));
			}
			if (this.DataGridView.NoDimensionChangeAllowed)
			{
				throw new InvalidOperationException(SR.GetString("DataGridView_ForbiddenOperationInEventHandler"));
			}
			if (this.DataGridView.DataSource == null)
			{
				this.RemoveAtInternal(index, false);
				return;
			}
			IBindingList bindingList = this.DataGridView.DataConnection.List as IBindingList;
			if (bindingList != null && bindingList.AllowRemove && bindingList.SupportsChangeNotification)
			{
				bindingList.RemoveAt(index);
				return;
			}
			throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_CantRemoveRowsWithWrongSource"));
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000C9A5C File Offset: 0x000C8A5C
		internal void RemoveAtInternal(int index, bool force)
		{
			DataGridViewRow dataGridViewRow = this.SharedRow(index);
			Point point = new Point(-1, -1);
			if (this.IsCollectionChangedListenedTo || dataGridViewRow.GetDisplayed(index))
			{
				dataGridViewRow = this[index];
			}
			dataGridViewRow = this.SharedRow(index);
			this.DataGridView.OnRemovingRow(index, out point, force);
			this.UpdateRowCaches(index, ref dataGridViewRow, false);
			if (dataGridViewRow.Index != -1)
			{
				this.rowStates[index] = dataGridViewRow.State;
				dataGridViewRow.DetachFromDataGridView();
			}
			this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, dataGridViewRow), index, 1, true, false, false, point);
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000C9AF0 File Offset: 0x000C8AF0
		private static bool RowHasValueOrToolTipText(DataGridViewRow dataGridViewRow)
		{
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				if (dataGridViewCell.HasValue || dataGridViewCell.HasToolTipText)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000C9B60 File Offset: 0x000C8B60
		internal bool RowIsSharable(int index)
		{
			DataGridViewRow dataGridViewRow = this.SharedRow(index);
			if (dataGridViewRow.Index != -1)
			{
				return false;
			}
			DataGridViewCellCollection cells = dataGridViewRow.Cells;
			foreach (object obj in cells)
			{
				DataGridViewCell dataGridViewCell = (DataGridViewCell)obj;
				if ((dataGridViewCell.State & ~(dataGridViewCell.CellStateFromColumnRowStates(this.rowStates[index]) != DataGridViewElementStates.None)) != DataGridViewElementStates.None)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000C9BF4 File Offset: 0x000C8BF4
		internal void SetRowState(int rowIndex, DataGridViewElementStates state, bool value)
		{
			DataGridViewRow dataGridViewRow = this.SharedRow(rowIndex);
			if (dataGridViewRow.Index == -1)
			{
				if ((this.rowStates[rowIndex] & state) != DataGridViewElementStates.None != value)
				{
					if (state == DataGridViewElementStates.Frozen || state == DataGridViewElementStates.Visible || state == DataGridViewElementStates.ReadOnly)
					{
						dataGridViewRow.OnSharedStateChanging(rowIndex, state);
					}
					if (value)
					{
						this.rowStates[rowIndex] = this.rowStates[rowIndex] | state;
					}
					else
					{
						this.rowStates[rowIndex] = this.rowStates[rowIndex] & ~state;
					}
					dataGridViewRow.OnSharedStateChanged(rowIndex, state);
					return;
				}
			}
			else if (state <= DataGridViewElementStates.Resizable)
			{
				switch (state)
				{
				case DataGridViewElementStates.Displayed:
					dataGridViewRow.DisplayedInternal = value;
					return;
				case DataGridViewElementStates.Frozen:
					dataGridViewRow.Frozen = value;
					return;
				case DataGridViewElementStates.Displayed | DataGridViewElementStates.Frozen:
					break;
				case DataGridViewElementStates.ReadOnly:
					dataGridViewRow.ReadOnlyInternal = value;
					return;
				default:
					if (state != DataGridViewElementStates.Resizable)
					{
						return;
					}
					dataGridViewRow.Resizable = (value ? DataGridViewTriState.True : DataGridViewTriState.False);
					break;
				}
			}
			else
			{
				if (state == DataGridViewElementStates.Selected)
				{
					dataGridViewRow.SelectedInternal = value;
					return;
				}
				if (state != DataGridViewElementStates.Visible)
				{
					return;
				}
				dataGridViewRow.Visible = value;
				return;
			}
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000C9CE7 File Offset: 0x000C8CE7
		internal DataGridViewElementStates SharedRowState(int rowIndex)
		{
			return this.rowStates[rowIndex];
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x000C9CF8 File Offset: 0x000C8CF8
		internal void Sort(IComparer customComparer, bool ascending)
		{
			if (this.items.Count > 0)
			{
				DataGridViewRowCollection.RowComparer rowComparer = new DataGridViewRowCollection.RowComparer(this, customComparer, ascending);
				this.items.CustomSort(rowComparer);
			}
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x000C9D28 File Offset: 0x000C8D28
		internal void SwapSortedRows(int rowIndex1, int rowIndex2)
		{
			this.DataGridView.SwapSortedRows(rowIndex1, rowIndex2);
			DataGridViewRow dataGridViewRow = this.SharedRow(rowIndex1);
			DataGridViewRow dataGridViewRow2 = this.SharedRow(rowIndex2);
			if (dataGridViewRow.Index != -1)
			{
				dataGridViewRow.IndexInternal = rowIndex2;
			}
			if (dataGridViewRow2.Index != -1)
			{
				dataGridViewRow2.IndexInternal = rowIndex1;
			}
			if (this.DataGridView.VirtualMode)
			{
				int count = this.DataGridView.Columns.Count;
				for (int i = 0; i < count; i++)
				{
					DataGridViewCell dataGridViewCell = dataGridViewRow.Cells[i];
					DataGridViewCell dataGridViewCell2 = dataGridViewRow2.Cells[i];
					object valueInternal = dataGridViewCell.GetValueInternal(rowIndex1);
					object valueInternal2 = dataGridViewCell2.GetValueInternal(rowIndex2);
					dataGridViewCell.SetValueInternal(rowIndex1, valueInternal2);
					dataGridViewCell2.SetValueInternal(rowIndex2, valueInternal);
				}
			}
			object obj = this.items[rowIndex1];
			this.items[rowIndex1] = this.items[rowIndex2];
			this.items[rowIndex2] = obj;
			DataGridViewElementStates dataGridViewElementStates = this.rowStates[rowIndex1];
			this.rowStates[rowIndex1] = this.rowStates[rowIndex2];
			this.rowStates[rowIndex2] = dataGridViewElementStates;
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000C9E4A File Offset: 0x000C8E4A
		private void UnshareRow(int rowIndex)
		{
			this.SharedRow(rowIndex).IndexInternal = rowIndex;
			this.SharedRow(rowIndex).StateInternal = this.SharedRowState(rowIndex);
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000C9E6C File Offset: 0x000C8E6C
		private void UpdateRowCaches(int rowIndex, ref DataGridViewRow dataGridViewRow, bool adding)
		{
			if (this.rowCountsVisible != -1 || this.rowCountsVisibleFrozen != -1 || this.rowCountsVisibleSelected != -1 || this.rowsHeightVisible != -1 || this.rowsHeightVisibleFrozen != -1)
			{
				DataGridViewElementStates rowState = this.GetRowState(rowIndex);
				if ((rowState & DataGridViewElementStates.Visible) != DataGridViewElementStates.None)
				{
					int num = (adding ? 1 : (-1));
					int num2 = 0;
					if (this.rowsHeightVisible != -1 || (this.rowsHeightVisibleFrozen != -1 && (rowState & (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)))
					{
						num2 = (adding ? dataGridViewRow.GetHeight(rowIndex) : (-dataGridViewRow.GetHeight(rowIndex)));
						dataGridViewRow = this.SharedRow(rowIndex);
					}
					if (this.rowCountsVisible != -1)
					{
						this.rowCountsVisible += num;
					}
					if (this.rowsHeightVisible != -1)
					{
						this.rowsHeightVisible += num2;
					}
					if ((rowState & (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Frozen | DataGridViewElementStates.Visible))
					{
						if (this.rowCountsVisibleFrozen != -1)
						{
							this.rowCountsVisibleFrozen += num;
						}
						if (this.rowsHeightVisibleFrozen != -1)
						{
							this.rowsHeightVisibleFrozen += num2;
						}
					}
					if ((rowState & (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible)) == (DataGridViewElementStates.Selected | DataGridViewElementStates.Visible) && this.rowCountsVisibleSelected != -1)
					{
						this.rowCountsVisibleSelected += num;
					}
				}
			}
		}

		// Token: 0x04001C03 RID: 7171
		private CollectionChangeEventHandler onCollectionChanged;

		// Token: 0x04001C04 RID: 7172
		private DataGridViewRowCollection.RowArrayList items;

		// Token: 0x04001C05 RID: 7173
		private List<DataGridViewElementStates> rowStates;

		// Token: 0x04001C06 RID: 7174
		private int rowCountsVisible;

		// Token: 0x04001C07 RID: 7175
		private int rowCountsVisibleFrozen;

		// Token: 0x04001C08 RID: 7176
		private int rowCountsVisibleSelected;

		// Token: 0x04001C09 RID: 7177
		private int rowsHeightVisible;

		// Token: 0x04001C0A RID: 7178
		private int rowsHeightVisibleFrozen;

		// Token: 0x04001C0B RID: 7179
		private DataGridView dataGridView;

		// Token: 0x02000382 RID: 898
		private class RowArrayList : ArrayList
		{
			// Token: 0x06003773 RID: 14195 RVA: 0x000C9F81 File Offset: 0x000C8F81
			public RowArrayList(DataGridViewRowCollection owner)
			{
				this.owner = owner;
			}

			// Token: 0x06003774 RID: 14196 RVA: 0x000C9F90 File Offset: 0x000C8F90
			public void CustomSort(DataGridViewRowCollection.RowComparer rowComparer)
			{
				this.rowComparer = rowComparer;
				this.CustomQuickSort(0, this.Count - 1);
			}

			// Token: 0x06003775 RID: 14197 RVA: 0x000C9FA8 File Offset: 0x000C8FA8
			private void CustomQuickSort(int left, int right)
			{
				if (right - left < 2)
				{
					if (right - left > 0 && this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(left), this.rowComparer.GetComparedObject(right), left, right) > 0)
					{
						this.owner.SwapSortedRows(left, right);
					}
					return;
				}
				do
				{
					int num = left + right >> 1;
					object obj = this.Pivot(left, num, right);
					int num2 = left + 1;
					int num3 = right - 1;
					do
					{
						if (num != num2)
						{
							if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(num2), obj, num2, num) < 0)
							{
								num2++;
								continue;
							}
						}
						while (num != num3 && this.rowComparer.CompareObjects(obj, this.rowComparer.GetComparedObject(num3), num, num3) < 0)
						{
							num3--;
						}
						if (num2 > num3)
						{
							break;
						}
						if (num2 < num3)
						{
							this.owner.SwapSortedRows(num2, num3);
							if (num2 == num)
							{
								num = num3;
							}
							else if (num3 == num)
							{
								num = num2;
							}
						}
						num2++;
						num3--;
					}
					while (num2 <= num3);
					if (num3 - left <= right - num2)
					{
						if (left < num3)
						{
							this.CustomQuickSort(left, num3);
						}
						left = num2;
					}
					else
					{
						if (num2 < right)
						{
							this.CustomQuickSort(num2, right);
						}
						right = num3;
					}
				}
				while (left < right);
			}

			// Token: 0x06003776 RID: 14198 RVA: 0x000CA0BC File Offset: 0x000C90BC
			private object Pivot(int left, int center, int right)
			{
				if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(left), this.rowComparer.GetComparedObject(center), left, center) > 0)
				{
					this.owner.SwapSortedRows(left, center);
				}
				if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(left), this.rowComparer.GetComparedObject(right), left, right) > 0)
				{
					this.owner.SwapSortedRows(left, right);
				}
				if (this.rowComparer.CompareObjects(this.rowComparer.GetComparedObject(center), this.rowComparer.GetComparedObject(right), center, right) > 0)
				{
					this.owner.SwapSortedRows(center, right);
				}
				return this.rowComparer.GetComparedObject(center);
			}

			// Token: 0x04001C0C RID: 7180
			private DataGridViewRowCollection owner;

			// Token: 0x04001C0D RID: 7181
			private DataGridViewRowCollection.RowComparer rowComparer;
		}

		// Token: 0x02000383 RID: 899
		private class RowComparer
		{
			// Token: 0x06003777 RID: 14199 RVA: 0x000CA174 File Offset: 0x000C9174
			public RowComparer(DataGridViewRowCollection dataGridViewRows, IComparer customComparer, bool ascending)
			{
				this.dataGridView = dataGridViewRows.DataGridView;
				this.dataGridViewRows = dataGridViewRows;
				this.dataGridViewSortedColumn = this.dataGridView.SortedColumn;
				if (this.dataGridViewSortedColumn == null)
				{
					this.sortedColumnIndex = -1;
				}
				else
				{
					this.sortedColumnIndex = this.dataGridViewSortedColumn.Index;
				}
				this.customComparer = customComparer;
				this.ascending = ascending;
			}

			// Token: 0x06003778 RID: 14200 RVA: 0x000CA1DC File Offset: 0x000C91DC
			internal object GetComparedObject(int rowIndex)
			{
				if (this.dataGridView.NewRowIndex != -1 && rowIndex == this.dataGridView.NewRowIndex)
				{
					return DataGridViewRowCollection.RowComparer.max;
				}
				if (this.customComparer == null)
				{
					DataGridViewRow dataGridViewRow = this.dataGridViewRows.SharedRow(rowIndex);
					return dataGridViewRow.Cells[this.sortedColumnIndex].GetValueInternal(rowIndex);
				}
				return this.dataGridViewRows[rowIndex];
			}

			// Token: 0x06003779 RID: 14201 RVA: 0x000CA244 File Offset: 0x000C9244
			internal int CompareObjects(object value1, object value2, int rowIndex1, int rowIndex2)
			{
				if (value1 is DataGridViewRowCollection.RowComparer.ComparedObjectMax)
				{
					return 1;
				}
				if (value2 is DataGridViewRowCollection.RowComparer.ComparedObjectMax)
				{
					return -1;
				}
				int num = 0;
				if (this.customComparer == null)
				{
					if (!this.dataGridView.OnSortCompare(this.dataGridViewSortedColumn, value1, value2, rowIndex1, rowIndex2, out num))
					{
						if (!(value1 is IComparable) && !(value2 is IComparable))
						{
							if (value1 == null)
							{
								if (value2 == null)
								{
									num = 0;
								}
								else
								{
									num = 1;
								}
							}
							else if (value2 == null)
							{
								num = -1;
							}
							else
							{
								num = Comparer.Default.Compare(value1.ToString(), value2.ToString());
							}
						}
						else
						{
							num = Comparer.Default.Compare(value1, value2);
						}
						if (num == 0)
						{
							if (this.ascending)
							{
								num = rowIndex1 - rowIndex2;
							}
							else
							{
								num = rowIndex2 - rowIndex1;
							}
						}
					}
				}
				else
				{
					num = this.customComparer.Compare(value1, value2);
				}
				if (this.ascending)
				{
					return num;
				}
				return -num;
			}

			// Token: 0x04001C0E RID: 7182
			private DataGridView dataGridView;

			// Token: 0x04001C0F RID: 7183
			private DataGridViewRowCollection dataGridViewRows;

			// Token: 0x04001C10 RID: 7184
			private DataGridViewColumn dataGridViewSortedColumn;

			// Token: 0x04001C11 RID: 7185
			private int sortedColumnIndex;

			// Token: 0x04001C12 RID: 7186
			private IComparer customComparer;

			// Token: 0x04001C13 RID: 7187
			private bool ascending;

			// Token: 0x04001C14 RID: 7188
			private static DataGridViewRowCollection.RowComparer.ComparedObjectMax max = new DataGridViewRowCollection.RowComparer.ComparedObjectMax();

			// Token: 0x02000384 RID: 900
			private class ComparedObjectMax
			{
			}
		}

		// Token: 0x02000385 RID: 901
		private class UnsharingRowEnumerator : IEnumerator
		{
			// Token: 0x0600377C RID: 14204 RVA: 0x000CA31B File Offset: 0x000C931B
			public UnsharingRowEnumerator(DataGridViewRowCollection owner)
			{
				this.owner = owner;
				this.current = -1;
			}

			// Token: 0x0600377D RID: 14205 RVA: 0x000CA331 File Offset: 0x000C9331
			bool IEnumerator.MoveNext()
			{
				if (this.current < this.owner.Count - 1)
				{
					this.current++;
					return true;
				}
				this.current = this.owner.Count;
				return false;
			}

			// Token: 0x0600377E RID: 14206 RVA: 0x000CA36A File Offset: 0x000C936A
			void IEnumerator.Reset()
			{
				this.current = -1;
			}

			// Token: 0x17000A3A RID: 2618
			// (get) Token: 0x0600377F RID: 14207 RVA: 0x000CA374 File Offset: 0x000C9374
			object IEnumerator.Current
			{
				get
				{
					if (this.current == -1)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_EnumNotStarted"));
					}
					if (this.current == this.owner.Count)
					{
						throw new InvalidOperationException(SR.GetString("DataGridViewRowCollection_EnumFinished"));
					}
					return this.owner[this.current];
				}
			}

			// Token: 0x04001C15 RID: 7189
			private DataGridViewRowCollection owner;

			// Token: 0x04001C16 RID: 7190
			private int current;
		}
	}
}
