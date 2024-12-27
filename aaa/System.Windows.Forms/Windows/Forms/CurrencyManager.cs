using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020002BB RID: 699
	public class CurrencyManager : BindingManagerBase
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x0600266F RID: 9839 RVA: 0x0005D7F9 File Offset: 0x0005C7F9
		// (remove) Token: 0x06002670 RID: 9840 RVA: 0x0005D812 File Offset: 0x0005C812
		[SRCategory("CatData")]
		public event ItemChangedEventHandler ItemChanged
		{
			add
			{
				this.onItemChanged = (ItemChangedEventHandler)Delegate.Combine(this.onItemChanged, value);
			}
			remove
			{
				this.onItemChanged = (ItemChangedEventHandler)Delegate.Remove(this.onItemChanged, value);
			}
		}

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x06002671 RID: 9841 RVA: 0x0005D82B File Offset: 0x0005C82B
		// (remove) Token: 0x06002672 RID: 9842 RVA: 0x0005D844 File Offset: 0x0005C844
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Combine(this.onListChanged, value);
			}
			remove
			{
				this.onListChanged = (ListChangedEventHandler)Delegate.Remove(this.onListChanged, value);
			}
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x0005D85D File Offset: 0x0005C85D
		internal CurrencyManager(object dataSource)
		{
			this.SetDataSource(dataSource);
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002674 RID: 9844 RVA: 0x0005D890 File Offset: 0x0005C890
		internal bool AllowAdd
		{
			get
			{
				if (this.list is IBindingList)
				{
					return ((IBindingList)this.list).AllowNew;
				}
				return this.list != null && !this.list.IsReadOnly && !this.list.IsFixedSize;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06002675 RID: 9845 RVA: 0x0005D8E2 File Offset: 0x0005C8E2
		internal bool AllowEdit
		{
			get
			{
				if (this.list is IBindingList)
				{
					return ((IBindingList)this.list).AllowEdit;
				}
				return this.list != null && !this.list.IsReadOnly;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002676 RID: 9846 RVA: 0x0005D91C File Offset: 0x0005C91C
		internal bool AllowRemove
		{
			get
			{
				if (this.list is IBindingList)
				{
					return ((IBindingList)this.list).AllowRemove;
				}
				return this.list != null && !this.list.IsReadOnly && !this.list.IsFixedSize;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x0005D96E File Offset: 0x0005C96E
		public override int Count
		{
			get
			{
				if (this.list == null)
				{
					return 0;
				}
				return this.list.Count;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002678 RID: 9848 RVA: 0x0005D985 File Offset: 0x0005C985
		public override object Current
		{
			get
			{
				return this[this.Position];
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06002679 RID: 9849 RVA: 0x0005D993 File Offset: 0x0005C993
		internal override Type BindType
		{
			get
			{
				return ListBindingHelper.GetListItemType(this.List);
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600267A RID: 9850 RVA: 0x0005D9A0 File Offset: 0x0005C9A0
		internal override object DataSource
		{
			get
			{
				return this.dataSource;
			}
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x0005D9A8 File Offset: 0x0005C9A8
		internal override void SetDataSource(object dataSource)
		{
			if (this.dataSource == dataSource)
			{
				return;
			}
			this.Release();
			this.dataSource = dataSource;
			this.list = null;
			this.finalType = null;
			object obj = dataSource;
			if (obj is Array)
			{
				this.finalType = obj.GetType();
				obj = (Array)obj;
			}
			if (obj is IListSource)
			{
				obj = ((IListSource)obj).GetList();
			}
			if (obj is IList)
			{
				if (this.finalType == null)
				{
					this.finalType = obj.GetType();
				}
				this.list = (IList)obj;
				this.WireEvents(this.list);
				if (this.list.Count > 0)
				{
					this.listposition = 0;
				}
				else
				{
					this.listposition = -1;
				}
				this.OnItemChanged(this.resetEvent);
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1, -1));
				this.UpdateIsBinding();
				return;
			}
			if (obj == null)
			{
				throw new ArgumentNullException("dataSource");
			}
			throw new ArgumentException(SR.GetString("ListManagerSetDataSource", new object[] { obj.GetType().FullName }), "dataSource");
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600267C RID: 9852 RVA: 0x0005DAB9 File Offset: 0x0005CAB9
		internal override bool IsBinding
		{
			get
			{
				return this.bound;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x0600267D RID: 9853 RVA: 0x0005DAC1 File Offset: 0x0005CAC1
		internal bool ShouldBind
		{
			get
			{
				return this.shouldBind;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x0005DAC9 File Offset: 0x0005CAC9
		public IList List
		{
			get
			{
				return this.list;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x0005DAD1 File Offset: 0x0005CAD1
		// (set) Token: 0x06002680 RID: 9856 RVA: 0x0005DADC File Offset: 0x0005CADC
		public override int Position
		{
			get
			{
				return this.listposition;
			}
			set
			{
				if (this.listposition == -1)
				{
					return;
				}
				if (value < 0)
				{
					value = 0;
				}
				int count = this.list.Count;
				if (value >= count)
				{
					value = count - 1;
				}
				this.ChangeRecordState(value, this.listposition != value, true, true, false);
			}
		}

		// Token: 0x17000617 RID: 1559
		internal object this[int index]
		{
			get
			{
				if (index < 0 || index >= this.list.Count)
				{
					throw new IndexOutOfRangeException(SR.GetString("ListManagerNoValue", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
				}
				return this.list[index];
			}
			set
			{
				if (index < 0 || index >= this.list.Count)
				{
					throw new IndexOutOfRangeException(SR.GetString("ListManagerNoValue", new object[] { index.ToString(CultureInfo.CurrentCulture) }));
				}
				this.list[index] = value;
			}
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x0005DBD0 File Offset: 0x0005CBD0
		public override void AddNew()
		{
			IBindingList bindingList = this.list as IBindingList;
			if (bindingList != null)
			{
				bindingList.AddNew();
				this.ChangeRecordState(this.list.Count - 1, this.Position != this.list.Count - 1, this.Position != this.list.Count - 1, true, true);
				return;
			}
			throw new NotSupportedException(SR.GetString("CurrencyManagerCantAddNew"));
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x0005DC4C File Offset: 0x0005CC4C
		public override void CancelCurrentEdit()
		{
			if (this.Count > 0)
			{
				object obj = ((this.Position >= 0 && this.Position < this.list.Count) ? this.list[this.Position] : null);
				IEditableObject editableObject = obj as IEditableObject;
				if (editableObject != null)
				{
					editableObject.CancelEdit();
				}
				ICancelAddNew cancelAddNew = this.list as ICancelAddNew;
				if (cancelAddNew != null)
				{
					cancelAddNew.CancelNew(this.Position);
				}
				this.OnItemChanged(new ItemChangedEventArgs(this.Position));
				if (this.Position != -1)
				{
					this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.Position));
				}
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x0005DCF0 File Offset: 0x0005CCF0
		private void ChangeRecordState(int newPosition, bool validating, bool endCurrentEdit, bool firePositionChange, bool pullData)
		{
			if (newPosition == -1 && this.list.Count == 0)
			{
				if (this.listposition != -1)
				{
					this.listposition = -1;
					this.OnPositionChanged(EventArgs.Empty);
				}
				return;
			}
			if ((newPosition < 0 || newPosition >= this.Count) && this.IsBinding)
			{
				throw new IndexOutOfRangeException(SR.GetString("ListManagerBadPosition"));
			}
			int num = this.listposition;
			if (endCurrentEdit)
			{
				this.inChangeRecordState = true;
				try
				{
					this.EndCurrentEdit();
				}
				finally
				{
					this.inChangeRecordState = false;
				}
			}
			if (validating && pullData)
			{
				this.CurrencyManager_PullData();
			}
			this.listposition = Math.Min(newPosition, this.Count - 1);
			if (validating)
			{
				this.OnCurrentChanged(EventArgs.Empty);
			}
			bool flag = num != this.listposition;
			if (flag && firePositionChange)
			{
				this.OnPositionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x0005DDD0 File Offset: 0x0005CDD0
		protected void CheckEmpty()
		{
			if (this.dataSource == null || this.list == null || this.list.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("ListManagerEmptyList"));
			}
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x0005DE00 File Offset: 0x0005CE00
		private bool CurrencyManager_PushData()
		{
			if (this.pullingData)
			{
				return false;
			}
			int num = this.listposition;
			if (this.lastGoodKnownRow == -1)
			{
				try
				{
					base.PushData();
				}
				catch (Exception ex)
				{
					base.OnDataError(ex);
					this.FindGoodRow();
				}
				this.lastGoodKnownRow = this.listposition;
			}
			else
			{
				try
				{
					base.PushData();
				}
				catch (Exception ex2)
				{
					base.OnDataError(ex2);
					this.listposition = this.lastGoodKnownRow;
					base.PushData();
				}
				this.lastGoodKnownRow = this.listposition;
			}
			return num != this.listposition;
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x0005DEA8 File Offset: 0x0005CEA8
		private bool CurrencyManager_PullData()
		{
			bool flag = true;
			this.pullingData = true;
			try
			{
				base.PullData(out flag);
			}
			finally
			{
				this.pullingData = false;
			}
			return flag;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x0005DEE4 File Offset: 0x0005CEE4
		public override void RemoveAt(int index)
		{
			this.list.RemoveAt(index);
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x0005DEF4 File Offset: 0x0005CEF4
		public override void EndCurrentEdit()
		{
			if (this.Count > 0)
			{
				bool flag = this.CurrencyManager_PullData();
				if (flag)
				{
					object obj = ((this.Position >= 0 && this.Position < this.list.Count) ? this.list[this.Position] : null);
					IEditableObject editableObject = obj as IEditableObject;
					if (editableObject != null)
					{
						editableObject.EndEdit();
					}
					ICancelAddNew cancelAddNew = this.list as ICancelAddNew;
					if (cancelAddNew != null)
					{
						cancelAddNew.EndNew(this.Position);
					}
				}
			}
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x0005DF70 File Offset: 0x0005CF70
		private void FindGoodRow()
		{
			int count = this.list.Count;
			int i = 0;
			while (i < count)
			{
				this.listposition = i;
				try
				{
					base.PushData();
				}
				catch (Exception ex)
				{
					base.OnDataError(ex);
					goto IL_0031;
				}
				goto IL_0029;
				IL_0031:
				i++;
				continue;
				IL_0029:
				this.listposition = i;
				return;
			}
			this.SuspendBinding();
			throw new InvalidOperationException(SR.GetString("DataBindingPushDataException"));
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x0005DFDC File Offset: 0x0005CFDC
		internal void SetSort(PropertyDescriptor property, ListSortDirection sortDirection)
		{
			if (this.list is IBindingList && ((IBindingList)this.list).SupportsSorting)
			{
				((IBindingList)this.list).ApplySort(property, sortDirection);
			}
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x0005E00F File Offset: 0x0005D00F
		internal PropertyDescriptor GetSortProperty()
		{
			if (this.list is IBindingList && ((IBindingList)this.list).SupportsSorting)
			{
				return ((IBindingList)this.list).SortProperty;
			}
			return null;
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x0005E042 File Offset: 0x0005D042
		internal ListSortDirection GetSortDirection()
		{
			if (this.list is IBindingList && ((IBindingList)this.list).SupportsSorting)
			{
				return ((IBindingList)this.list).SortDirection;
			}
			return ListSortDirection.Ascending;
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x0005E078 File Offset: 0x0005D078
		internal int Find(PropertyDescriptor property, object key, bool keepIndex)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (property != null && this.list is IBindingList && ((IBindingList)this.list).SupportsSearching)
			{
				return ((IBindingList)this.list).Find(property, key);
			}
			for (int i = 0; i < this.list.Count; i++)
			{
				object value = property.GetValue(this.list[i]);
				if (key.Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x0005E0FD File Offset: 0x0005D0FD
		internal override string GetListName()
		{
			if (this.list is ITypedList)
			{
				return ((ITypedList)this.list).GetListName(null);
			}
			return this.finalType.Name;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x0005E12C File Offset: 0x0005D12C
		protected internal override string GetListName(ArrayList listAccessors)
		{
			if (this.list is ITypedList)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[listAccessors.Count];
				listAccessors.CopyTo(array, 0);
				return ((ITypedList)this.list).GetListName(array);
			}
			return "";
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x0005E171 File Offset: 0x0005D171
		internal override PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			return ListBindingHelper.GetListItemProperties(this.list, listAccessors);
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x0005E17F File Offset: 0x0005D17F
		public override PropertyDescriptorCollection GetItemProperties()
		{
			return this.GetItemProperties(null);
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x0005E188 File Offset: 0x0005D188
		private void List_ListChanged(object sender, ListChangedEventArgs e)
		{
			ListChangedEventArgs listChangedEventArgs;
			if (e.ListChangedType == ListChangedType.ItemMoved && e.OldIndex < 0)
			{
				listChangedEventArgs = new ListChangedEventArgs(ListChangedType.ItemAdded, e.NewIndex, e.OldIndex);
			}
			else if (e.ListChangedType == ListChangedType.ItemMoved && e.NewIndex < 0)
			{
				listChangedEventArgs = new ListChangedEventArgs(ListChangedType.ItemDeleted, e.OldIndex, e.NewIndex);
			}
			else
			{
				listChangedEventArgs = e;
			}
			int num = this.listposition;
			this.UpdateLastGoodKnownRow(listChangedEventArgs);
			this.UpdateIsBinding();
			if (this.list.Count == 0)
			{
				this.listposition = -1;
				if (num != -1)
				{
					this.OnPositionChanged(EventArgs.Empty);
					this.OnCurrentChanged(EventArgs.Empty);
				}
				if (listChangedEventArgs.ListChangedType == ListChangedType.Reset && e.NewIndex == -1)
				{
					this.OnItemChanged(this.resetEvent);
				}
				if (listChangedEventArgs.ListChangedType == ListChangedType.ItemDeleted)
				{
					this.OnItemChanged(this.resetEvent);
				}
				if (e.ListChangedType == ListChangedType.PropertyDescriptorAdded || e.ListChangedType == ListChangedType.PropertyDescriptorDeleted || e.ListChangedType == ListChangedType.PropertyDescriptorChanged)
				{
					this.OnMetaDataChanged(EventArgs.Empty);
				}
				this.OnListChanged(listChangedEventArgs);
				return;
			}
			this.suspendPushDataInCurrentChanged = true;
			try
			{
				switch (listChangedEventArgs.ListChangedType)
				{
				case ListChangedType.Reset:
					if (this.listposition == -1 && this.list.Count > 0)
					{
						this.ChangeRecordState(0, true, false, true, false);
					}
					else
					{
						this.ChangeRecordState(Math.Min(this.listposition, this.list.Count - 1), true, false, true, false);
					}
					this.UpdateIsBinding(false);
					this.OnItemChanged(this.resetEvent);
					break;
				case ListChangedType.ItemAdded:
					if (listChangedEventArgs.NewIndex <= this.listposition && this.listposition < this.list.Count - 1)
					{
						this.ChangeRecordState(this.listposition + 1, true, true, this.listposition != this.list.Count - 2, false);
						this.UpdateIsBinding();
						this.OnItemChanged(this.resetEvent);
						if (this.listposition == this.list.Count - 1)
						{
							this.OnPositionChanged(EventArgs.Empty);
						}
					}
					else
					{
						if (listChangedEventArgs.NewIndex == this.listposition && this.listposition == this.list.Count - 1 && this.listposition != -1)
						{
							this.OnCurrentItemChanged(EventArgs.Empty);
						}
						if (this.listposition == -1)
						{
							this.ChangeRecordState(0, false, false, true, false);
						}
						this.UpdateIsBinding();
						this.OnItemChanged(this.resetEvent);
					}
					break;
				case ListChangedType.ItemDeleted:
					if (listChangedEventArgs.NewIndex == this.listposition)
					{
						this.ChangeRecordState(Math.Min(this.listposition, this.Count - 1), true, false, true, false);
						this.OnItemChanged(this.resetEvent);
					}
					else if (listChangedEventArgs.NewIndex < this.listposition)
					{
						this.ChangeRecordState(this.listposition - 1, true, false, true, false);
						this.OnItemChanged(this.resetEvent);
					}
					else
					{
						this.OnItemChanged(this.resetEvent);
					}
					break;
				case ListChangedType.ItemMoved:
					if (listChangedEventArgs.OldIndex == this.listposition)
					{
						this.ChangeRecordState(listChangedEventArgs.NewIndex, true, this.Position > -1 && this.Position < this.list.Count, true, false);
					}
					else if (listChangedEventArgs.NewIndex == this.listposition)
					{
						this.ChangeRecordState(listChangedEventArgs.OldIndex, true, this.Position > -1 && this.Position < this.list.Count, true, false);
					}
					this.OnItemChanged(this.resetEvent);
					break;
				case ListChangedType.ItemChanged:
					if (listChangedEventArgs.NewIndex == this.listposition)
					{
						this.OnCurrentItemChanged(EventArgs.Empty);
					}
					this.OnItemChanged(new ItemChangedEventArgs(listChangedEventArgs.NewIndex));
					break;
				case ListChangedType.PropertyDescriptorAdded:
				case ListChangedType.PropertyDescriptorDeleted:
				case ListChangedType.PropertyDescriptorChanged:
					this.lastGoodKnownRow = -1;
					if (this.listposition == -1 && this.list.Count > 0)
					{
						this.ChangeRecordState(0, true, false, true, false);
					}
					else if (this.listposition > this.list.Count - 1)
					{
						this.ChangeRecordState(this.list.Count - 1, true, false, true, false);
					}
					this.OnMetaDataChanged(EventArgs.Empty);
					break;
				}
				this.OnListChanged(listChangedEventArgs);
			}
			finally
			{
				this.suspendPushDataInCurrentChanged = false;
			}
		}

		// Token: 0x1400010C RID: 268
		// (add) Token: 0x06002695 RID: 9877 RVA: 0x0005E5D0 File Offset: 0x0005D5D0
		// (remove) Token: 0x06002696 RID: 9878 RVA: 0x0005E5E9 File Offset: 0x0005D5E9
		[SRCategory("CatData")]
		public event EventHandler MetaDataChanged
		{
			add
			{
				this.onMetaDataChangedHandler = (EventHandler)Delegate.Combine(this.onMetaDataChangedHandler, value);
			}
			remove
			{
				this.onMetaDataChangedHandler = (EventHandler)Delegate.Remove(this.onMetaDataChangedHandler, value);
			}
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x0005E604 File Offset: 0x0005D604
		protected internal override void OnCurrentChanged(EventArgs e)
		{
			if (!this.inChangeRecordState)
			{
				int num = this.lastGoodKnownRow;
				bool flag = false;
				if (!this.suspendPushDataInCurrentChanged)
				{
					flag = this.CurrencyManager_PushData();
				}
				if (this.Count > 0)
				{
					object obj = this.list[this.Position];
					if (obj is IEditableObject)
					{
						((IEditableObject)obj).BeginEdit();
					}
				}
				try
				{
					if (!flag || (flag && num != -1))
					{
						if (this.onCurrentChangedHandler != null)
						{
							this.onCurrentChangedHandler(this, e);
						}
						if (this.onCurrentItemChangedHandler != null)
						{
							this.onCurrentItemChangedHandler(this, e);
						}
					}
				}
				catch (Exception ex)
				{
					base.OnDataError(ex);
				}
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x0005E6B4 File Offset: 0x0005D6B4
		protected internal override void OnCurrentItemChanged(EventArgs e)
		{
			if (this.onCurrentItemChangedHandler != null)
			{
				this.onCurrentItemChangedHandler(this, e);
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0005E6CC File Offset: 0x0005D6CC
		protected virtual void OnItemChanged(ItemChangedEventArgs e)
		{
			bool flag = false;
			if ((e.Index == this.listposition || (e.Index == -1 && this.Position < this.Count)) && !this.inChangeRecordState)
			{
				flag = this.CurrencyManager_PushData();
			}
			try
			{
				if (this.onItemChanged != null)
				{
					this.onItemChanged(this, e);
				}
			}
			catch (Exception ex)
			{
				base.OnDataError(ex);
			}
			if (flag)
			{
				this.OnPositionChanged(EventArgs.Empty);
			}
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x0005E750 File Offset: 0x0005D750
		private void OnListChanged(ListChangedEventArgs e)
		{
			if (this.onListChanged != null)
			{
				this.onListChanged(this, e);
			}
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x0005E767 File Offset: 0x0005D767
		protected internal void OnMetaDataChanged(EventArgs e)
		{
			if (this.onMetaDataChangedHandler != null)
			{
				this.onMetaDataChangedHandler(this, e);
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0005E780 File Offset: 0x0005D780
		protected virtual void OnPositionChanged(EventArgs e)
		{
			try
			{
				if (this.onPositionChangedHandler != null)
				{
					this.onPositionChangedHandler(this, e);
				}
			}
			catch (Exception ex)
			{
				base.OnDataError(ex);
			}
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0005E7C0 File Offset: 0x0005D7C0
		public void Refresh()
		{
			if (this.list.Count > 0)
			{
				if (this.listposition >= this.list.Count)
				{
					this.lastGoodKnownRow = -1;
					this.listposition = 0;
				}
			}
			else
			{
				this.listposition = -1;
			}
			this.List_ListChanged(this.list, new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0005E818 File Offset: 0x0005D818
		internal void Release()
		{
			this.UnwireEvents(this.list);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0005E828 File Offset: 0x0005D828
		public override void ResumeBinding()
		{
			this.lastGoodKnownRow = -1;
			try
			{
				if (!this.shouldBind)
				{
					this.shouldBind = true;
					this.listposition = ((this.list != null && this.list.Count != 0) ? 0 : (-1));
					this.UpdateIsBinding();
				}
			}
			catch
			{
				this.shouldBind = false;
				this.UpdateIsBinding();
				throw;
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0005E894 File Offset: 0x0005D894
		public override void SuspendBinding()
		{
			this.lastGoodKnownRow = -1;
			if (this.shouldBind)
			{
				this.shouldBind = false;
				this.UpdateIsBinding();
			}
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0005E8B2 File Offset: 0x0005D8B2
		internal void UnwireEvents(IList list)
		{
			if (list is IBindingList && ((IBindingList)list).SupportsChangeNotification)
			{
				((IBindingList)list).ListChanged -= this.List_ListChanged;
			}
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0005E8E0 File Offset: 0x0005D8E0
		protected override void UpdateIsBinding()
		{
			this.UpdateIsBinding(true);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0005E8EC File Offset: 0x0005D8EC
		private void UpdateIsBinding(bool raiseItemChangedEvent)
		{
			bool flag = this.list != null && this.list.Count > 0 && this.shouldBind && this.listposition != -1;
			if (this.list != null && this.bound != flag)
			{
				this.bound = flag;
				int num = (flag ? 0 : (-1));
				this.ChangeRecordState(num, this.bound, this.Position != num, true, false);
				int count = base.Bindings.Count;
				for (int i = 0; i < count; i++)
				{
					base.Bindings[i].UpdateIsBinding();
				}
				if (raiseItemChangedEvent)
				{
					this.OnItemChanged(this.resetEvent);
				}
			}
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0005E99C File Offset: 0x0005D99C
		private void UpdateLastGoodKnownRow(ListChangedEventArgs e)
		{
			switch (e.ListChangedType)
			{
			case ListChangedType.Reset:
				this.lastGoodKnownRow = -1;
				return;
			case ListChangedType.ItemAdded:
				if (e.NewIndex <= this.lastGoodKnownRow && this.lastGoodKnownRow < this.List.Count - 1)
				{
					this.lastGoodKnownRow++;
					return;
				}
				break;
			case ListChangedType.ItemDeleted:
				if (e.NewIndex == this.lastGoodKnownRow)
				{
					this.lastGoodKnownRow = -1;
					return;
				}
				break;
			case ListChangedType.ItemMoved:
				if (e.OldIndex == this.lastGoodKnownRow)
				{
					this.lastGoodKnownRow = e.NewIndex;
					return;
				}
				break;
			case ListChangedType.ItemChanged:
				if (e.NewIndex == this.lastGoodKnownRow)
				{
					this.lastGoodKnownRow = -1;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0005EA4B File Offset: 0x0005DA4B
		internal void WireEvents(IList list)
		{
			if (list is IBindingList && ((IBindingList)list).SupportsChangeNotification)
			{
				((IBindingList)list).ListChanged += this.List_ListChanged;
			}
		}

		// Token: 0x0400164A RID: 5706
		private object dataSource;

		// Token: 0x0400164B RID: 5707
		private IList list;

		// Token: 0x0400164C RID: 5708
		private bool bound;

		// Token: 0x0400164D RID: 5709
		private bool shouldBind = true;

		// Token: 0x0400164E RID: 5710
		protected int listposition = -1;

		// Token: 0x0400164F RID: 5711
		private int lastGoodKnownRow = -1;

		// Token: 0x04001650 RID: 5712
		private bool pullingData;

		// Token: 0x04001651 RID: 5713
		private bool inChangeRecordState;

		// Token: 0x04001652 RID: 5714
		private bool suspendPushDataInCurrentChanged;

		// Token: 0x04001653 RID: 5715
		private ItemChangedEventHandler onItemChanged;

		// Token: 0x04001654 RID: 5716
		private ListChangedEventHandler onListChanged;

		// Token: 0x04001655 RID: 5717
		private ItemChangedEventArgs resetEvent = new ItemChangedEventArgs(-1);

		// Token: 0x04001656 RID: 5718
		private EventHandler onMetaDataChangedHandler;

		// Token: 0x04001657 RID: 5719
		protected Type finalType;
	}
}
