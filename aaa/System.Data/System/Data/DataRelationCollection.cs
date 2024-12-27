using System;
using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Threading;

namespace System.Data
{
	// Token: 0x0200007D RID: 125
	[DefaultProperty("Table")]
	[Editor("Microsoft.VSDesigner.Data.Design.DataRelationCollectionEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultEvent("CollectionChanged")]
	public abstract class DataRelationCollection : InternalDataCollectionBase
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x001DE6BC File Offset: 0x001DDABC
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x170000D8 RID: 216
		public abstract DataRelation this[int index] { get; }

		// Token: 0x170000D9 RID: 217
		public abstract DataRelation this[string name] { get; }

		// Token: 0x06000731 RID: 1841 RVA: 0x001DE6D0 File Offset: 0x001DDAD0
		public void Add(DataRelation relation)
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataRelationCollection.Add|API> %d#, relation=%d\n", this.ObjectID, (relation != null) ? relation.ObjectID : 0);
			try
			{
				if (this.inTransition != relation)
				{
					this.inTransition = relation;
					try
					{
						this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Add, relation));
						this.AddCore(relation);
						this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Add, relation));
					}
					finally
					{
						this.inTransition = null;
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x001DE778 File Offset: 0x001DDB78
		public virtual void AddRange(DataRelation[] relations)
		{
			if (relations != null)
			{
				foreach (DataRelation dataRelation in relations)
				{
					if (dataRelation != null)
					{
						this.Add(dataRelation);
					}
				}
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x001DE7A8 File Offset: 0x001DDBA8
		public virtual DataRelation Add(string name, DataColumn[] parentColumns, DataColumn[] childColumns)
		{
			DataRelation dataRelation = new DataRelation(name, parentColumns, childColumns);
			this.Add(dataRelation);
			return dataRelation;
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x001DE7C8 File Offset: 0x001DDBC8
		public virtual DataRelation Add(string name, DataColumn[] parentColumns, DataColumn[] childColumns, bool createConstraints)
		{
			DataRelation dataRelation = new DataRelation(name, parentColumns, childColumns, createConstraints);
			this.Add(dataRelation);
			return dataRelation;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x001DE7E8 File Offset: 0x001DDBE8
		public virtual DataRelation Add(DataColumn[] parentColumns, DataColumn[] childColumns)
		{
			DataRelation dataRelation = new DataRelation(null, parentColumns, childColumns);
			this.Add(dataRelation);
			return dataRelation;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x001DE808 File Offset: 0x001DDC08
		public virtual DataRelation Add(string name, DataColumn parentColumn, DataColumn childColumn)
		{
			DataRelation dataRelation = new DataRelation(name, parentColumn, childColumn);
			this.Add(dataRelation);
			return dataRelation;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x001DE828 File Offset: 0x001DDC28
		public virtual DataRelation Add(string name, DataColumn parentColumn, DataColumn childColumn, bool createConstraints)
		{
			DataRelation dataRelation = new DataRelation(name, parentColumn, childColumn, createConstraints);
			this.Add(dataRelation);
			return dataRelation;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x001DE848 File Offset: 0x001DDC48
		public virtual DataRelation Add(DataColumn parentColumn, DataColumn childColumn)
		{
			DataRelation dataRelation = new DataRelation(null, parentColumn, childColumn);
			this.Add(dataRelation);
			return dataRelation;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x001DE868 File Offset: 0x001DDC68
		protected virtual void AddCore(DataRelation relation)
		{
			Bid.Trace("<ds.DataRelationCollection.AddCore|INFO> %d#, relation=%d\n", this.ObjectID, (relation != null) ? relation.ObjectID : 0);
			if (relation == null)
			{
				throw ExceptionBuilder.ArgumentNull("relation");
			}
			relation.CheckState();
			DataSet dataSet = this.GetDataSet();
			if (relation.DataSet == dataSet)
			{
				throw ExceptionBuilder.RelationAlreadyInTheDataSet();
			}
			if (relation.DataSet != null)
			{
				throw ExceptionBuilder.RelationAlreadyInOtherDataSet();
			}
			if (relation.ChildTable.Locale.LCID != relation.ParentTable.Locale.LCID || relation.ChildTable.CaseSensitive != relation.ParentTable.CaseSensitive)
			{
				throw ExceptionBuilder.CaseLocaleMismatch();
			}
			if (relation.Nested)
			{
				relation.CheckNamespaceValidityForNestedRelations(relation.ParentTable.Namespace);
				relation.ValidateMultipleNestedRelations();
				relation.ParentTable.ElementColumnCount++;
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600073A RID: 1850 RVA: 0x001DE93C File Offset: 0x001DDD3C
		// (remove) Token: 0x0600073B RID: 1851 RVA: 0x001DE970 File Offset: 0x001DDD70
		[ResDescription("collectionChangedEventDescr")]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				Bid.Trace("<ds.DataRelationCollection.add_CollectionChanged|API> %d#\n", this.ObjectID);
				this.onCollectionChangedDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChangedDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataRelationCollection.remove_CollectionChanged|API> %d#\n", this.ObjectID);
				this.onCollectionChangedDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChangedDelegate, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600073C RID: 1852 RVA: 0x001DE9A4 File Offset: 0x001DDDA4
		// (remove) Token: 0x0600073D RID: 1853 RVA: 0x001DE9D8 File Offset: 0x001DDDD8
		internal event CollectionChangeEventHandler CollectionChanging
		{
			add
			{
				Bid.Trace("<ds.DataRelationCollection.add_CollectionChanging|INFO> %d#\n", this.ObjectID);
				this.onCollectionChangingDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChangingDelegate, value);
			}
			remove
			{
				Bid.Trace("<ds.DataRelationCollection.remove_CollectionChanging|INFO> %d#\n", this.ObjectID);
				this.onCollectionChangingDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChangingDelegate, value);
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x001DEA0C File Offset: 0x001DDE0C
		internal string AssignName()
		{
			string text = this.MakeName(this.defaultNameIndex);
			this.defaultNameIndex++;
			return text;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x001DEA38 File Offset: 0x001DDE38
		public virtual void Clear()
		{
			IntPtr intPtr;
			Bid.ScopeEnter(out intPtr, "<ds.DataRelationCollection.Clear|API> %d#\n", this.ObjectID);
			try
			{
				int count = this.Count;
				this.OnCollectionChanging(InternalDataCollectionBase.RefreshEventArgs);
				for (int i = count - 1; i >= 0; i--)
				{
					this.inTransition = this[i];
					this.RemoveCore(this.inTransition);
				}
				this.OnCollectionChanged(InternalDataCollectionBase.RefreshEventArgs);
				this.inTransition = null;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x001DEAC8 File Offset: 0x001DDEC8
		public virtual bool Contains(string name)
		{
			return this.InternalIndexOf(name) >= 0;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x001DEAE4 File Offset: 0x001DDEE4
		public void CopyTo(DataRelation[] array, int index)
		{
			if (array == null)
			{
				throw ExceptionBuilder.ArgumentNull("array");
			}
			if (index < 0)
			{
				throw ExceptionBuilder.ArgumentOutOfRange("index");
			}
			ArrayList list = this.List;
			if (array.Length - index < list.Count)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
			for (int i = 0; i < list.Count; i++)
			{
				array[index + i] = (DataRelation)list[i];
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x001DEB4C File Offset: 0x001DDF4C
		public virtual int IndexOf(DataRelation relation)
		{
			int count = this.List.Count;
			for (int i = 0; i < count; i++)
			{
				if (relation == (DataRelation)this.List[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x001DEB88 File Offset: 0x001DDF88
		public virtual int IndexOf(string relationName)
		{
			int num = this.InternalIndexOf(relationName);
			if (num >= 0)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x001DEBA4 File Offset: 0x001DDFA4
		internal int InternalIndexOf(string name)
		{
			int num = -1;
			if (name != null && 0 < name.Length)
			{
				int count = this.List.Count;
				for (int i = 0; i < count; i++)
				{
					DataRelation dataRelation = (DataRelation)this.List[i];
					int num2 = base.NamesEqual(dataRelation.RelationName, name, false, this.GetDataSet().Locale);
					if (num2 == 1)
					{
						return i;
					}
					if (num2 == -1)
					{
						num = ((num == -1) ? i : (-2));
					}
				}
			}
			return num;
		}

		// Token: 0x06000745 RID: 1861
		protected abstract DataSet GetDataSet();

		// Token: 0x06000746 RID: 1862 RVA: 0x001DEC1C File Offset: 0x001DE01C
		private string MakeName(int index)
		{
			if (1 == index)
			{
				return "Relation1";
			}
			return "Relation" + index.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x001DEC4C File Offset: 0x001DE04C
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChangedDelegate != null)
			{
				Bid.Trace("<ds.DataRelationCollection.OnCollectionChanged|INFO> %d#\n", this.ObjectID);
				this.onCollectionChangedDelegate(this, ccevent);
			}
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x001DEC80 File Offset: 0x001DE080
		protected virtual void OnCollectionChanging(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChangingDelegate != null)
			{
				Bid.Trace("<ds.DataRelationCollection.OnCollectionChanging|INFO> %d#\n", this.ObjectID);
				this.onCollectionChangingDelegate(this, ccevent);
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x001DECB4 File Offset: 0x001DE0B4
		internal void RegisterName(string name)
		{
			Bid.Trace("<ds.DataRelationCollection.RegisterName|INFO> %d#, name='%ls'\n", this.ObjectID, name);
			CultureInfo locale = this.GetDataSet().Locale;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (base.NamesEqual(name, this[i].RelationName, true, locale) != 0)
				{
					throw ExceptionBuilder.DuplicateRelation(this[i].RelationName);
				}
			}
			if (base.NamesEqual(name, this.MakeName(this.defaultNameIndex), true, locale) != 0)
			{
				this.defaultNameIndex++;
			}
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x001DED40 File Offset: 0x001DE140
		public virtual bool CanRemove(DataRelation relation)
		{
			return relation != null && relation.DataSet == this.GetDataSet();
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x001DED64 File Offset: 0x001DE164
		public void Remove(DataRelation relation)
		{
			Bid.Trace("<ds.DataRelationCollection.Remove|API> %d#, relation=%d\n", this.ObjectID, (relation != null) ? relation.ObjectID : 0);
			if (this.inTransition == relation)
			{
				return;
			}
			this.inTransition = relation;
			try
			{
				this.OnCollectionChanging(new CollectionChangeEventArgs(CollectionChangeAction.Remove, relation));
				this.RemoveCore(relation);
				this.OnCollectionChanged(new CollectionChangeEventArgs(CollectionChangeAction.Remove, relation));
			}
			finally
			{
				this.inTransition = null;
			}
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x001DEDE8 File Offset: 0x001DE1E8
		public void RemoveAt(int index)
		{
			DataRelation dataRelation = this[index];
			if (dataRelation == null)
			{
				throw ExceptionBuilder.RelationOutOfRange(index);
			}
			this.Remove(dataRelation);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x001DEE14 File Offset: 0x001DE214
		public void Remove(string name)
		{
			DataRelation dataRelation = this[name];
			if (dataRelation == null)
			{
				throw ExceptionBuilder.RelationNotInTheDataSet(name);
			}
			this.Remove(dataRelation);
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x001DEE3C File Offset: 0x001DE23C
		protected virtual void RemoveCore(DataRelation relation)
		{
			Bid.Trace("<ds.DataRelationCollection.RemoveCore|INFO> %d#, relation=%d\n", this.ObjectID, (relation != null) ? relation.ObjectID : 0);
			if (relation == null)
			{
				throw ExceptionBuilder.ArgumentNull("relation");
			}
			DataSet dataSet = this.GetDataSet();
			if (relation.DataSet != dataSet)
			{
				throw ExceptionBuilder.RelationNotInTheDataSet(relation.RelationName);
			}
			if (relation.Nested)
			{
				relation.ParentTable.ElementColumnCount--;
				relation.ParentTable.Columns.UnregisterName(relation.ChildTable.TableName);
			}
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x001DEEC8 File Offset: 0x001DE2C8
		internal void UnregisterName(string name)
		{
			Bid.Trace("<ds.DataRelationCollection.UnregisterName|INFO> %d#, name='%ls'\n", this.ObjectID, name);
			if (base.NamesEqual(name, this.MakeName(this.defaultNameIndex - 1), true, this.GetDataSet().Locale) != 0)
			{
				do
				{
					this.defaultNameIndex--;
				}
				while (this.defaultNameIndex > 1 && !this.Contains(this.MakeName(this.defaultNameIndex - 1)));
			}
		}

		// Token: 0x04000726 RID: 1830
		private DataRelation inTransition;

		// Token: 0x04000727 RID: 1831
		private int defaultNameIndex = 1;

		// Token: 0x04000728 RID: 1832
		private CollectionChangeEventHandler onCollectionChangedDelegate;

		// Token: 0x04000729 RID: 1833
		private CollectionChangeEventHandler onCollectionChangingDelegate;

		// Token: 0x0400072A RID: 1834
		private static int _objectTypeCount;

		// Token: 0x0400072B RID: 1835
		private readonly int _objectID = Interlocked.Increment(ref DataRelationCollection._objectTypeCount);

		// Token: 0x0200007E RID: 126
		internal sealed class DataTableRelationCollection : DataRelationCollection
		{
			// Token: 0x06000751 RID: 1873 RVA: 0x001DEF64 File Offset: 0x001DE364
			internal DataTableRelationCollection(DataTable table, bool fParentCollection)
			{
				if (table == null)
				{
					throw ExceptionBuilder.RelationTableNull();
				}
				this.table = table;
				this.fParentCollection = fParentCollection;
				this.relations = new ArrayList();
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x06000752 RID: 1874 RVA: 0x001DEF9C File Offset: 0x001DE39C
			protected override ArrayList List
			{
				get
				{
					return this.relations;
				}
			}

			// Token: 0x06000753 RID: 1875 RVA: 0x001DEFB0 File Offset: 0x001DE3B0
			private void EnsureDataSet()
			{
				if (this.table.DataSet == null)
				{
					throw ExceptionBuilder.RelationTableWasRemoved();
				}
			}

			// Token: 0x06000754 RID: 1876 RVA: 0x001DEFD0 File Offset: 0x001DE3D0
			protected override DataSet GetDataSet()
			{
				this.EnsureDataSet();
				return this.table.DataSet;
			}

			// Token: 0x170000DB RID: 219
			public override DataRelation this[int index]
			{
				get
				{
					if (index >= 0 && index < this.relations.Count)
					{
						return (DataRelation)this.relations[index];
					}
					throw ExceptionBuilder.RelationOutOfRange(index);
				}
			}

			// Token: 0x170000DC RID: 220
			public override DataRelation this[string name]
			{
				get
				{
					int num = base.InternalIndexOf(name);
					if (num == -2)
					{
						throw ExceptionBuilder.CaseInsensitiveNameConflict(name);
					}
					if (num >= 0)
					{
						return (DataRelation)this.List[num];
					}
					return null;
				}
			}

			// Token: 0x1400000A RID: 10
			// (add) Token: 0x06000757 RID: 1879 RVA: 0x001DF064 File Offset: 0x001DE464
			// (remove) Token: 0x06000758 RID: 1880 RVA: 0x001DF088 File Offset: 0x001DE488
			internal event CollectionChangeEventHandler RelationPropertyChanged
			{
				add
				{
					this.onRelationPropertyChangedDelegate = (CollectionChangeEventHandler)Delegate.Combine(this.onRelationPropertyChangedDelegate, value);
				}
				remove
				{
					this.onRelationPropertyChangedDelegate = (CollectionChangeEventHandler)Delegate.Remove(this.onRelationPropertyChangedDelegate, value);
				}
			}

			// Token: 0x06000759 RID: 1881 RVA: 0x001DF0AC File Offset: 0x001DE4AC
			internal void OnRelationPropertyChanged(CollectionChangeEventArgs ccevent)
			{
				if (!this.fParentCollection)
				{
					this.table.UpdatePropertyDescriptorCollectionCache();
				}
				if (this.onRelationPropertyChangedDelegate != null)
				{
					this.onRelationPropertyChangedDelegate(this, ccevent);
				}
			}

			// Token: 0x0600075A RID: 1882 RVA: 0x001DF0E4 File Offset: 0x001DE4E4
			private void AddCache(DataRelation relation)
			{
				this.relations.Add(relation);
				if (!this.fParentCollection)
				{
					this.table.UpdatePropertyDescriptorCollectionCache();
				}
			}

			// Token: 0x0600075B RID: 1883 RVA: 0x001DF114 File Offset: 0x001DE514
			protected override void AddCore(DataRelation relation)
			{
				if (this.fParentCollection)
				{
					if (relation.ChildTable != this.table)
					{
						throw ExceptionBuilder.ChildTableMismatch();
					}
				}
				else if (relation.ParentTable != this.table)
				{
					throw ExceptionBuilder.ParentTableMismatch();
				}
				this.GetDataSet().Relations.Add(relation);
				this.AddCache(relation);
			}

			// Token: 0x0600075C RID: 1884 RVA: 0x001DF16C File Offset: 0x001DE56C
			public override bool CanRemove(DataRelation relation)
			{
				if (!base.CanRemove(relation))
				{
					return false;
				}
				if (this.fParentCollection)
				{
					if (relation.ChildTable != this.table)
					{
						return false;
					}
				}
				else if (relation.ParentTable != this.table)
				{
					return false;
				}
				return true;
			}

			// Token: 0x0600075D RID: 1885 RVA: 0x001DF1B0 File Offset: 0x001DE5B0
			private void RemoveCache(DataRelation relation)
			{
				for (int i = 0; i < this.relations.Count; i++)
				{
					if (relation == this.relations[i])
					{
						this.relations.RemoveAt(i);
						if (!this.fParentCollection)
						{
							this.table.UpdatePropertyDescriptorCollectionCache();
						}
						return;
					}
				}
				throw ExceptionBuilder.RelationDoesNotExist();
			}

			// Token: 0x0600075E RID: 1886 RVA: 0x001DF208 File Offset: 0x001DE608
			protected override void RemoveCore(DataRelation relation)
			{
				if (this.fParentCollection)
				{
					if (relation.ChildTable != this.table)
					{
						throw ExceptionBuilder.ChildTableMismatch();
					}
				}
				else if (relation.ParentTable != this.table)
				{
					throw ExceptionBuilder.ParentTableMismatch();
				}
				this.GetDataSet().Relations.Remove(relation);
				this.RemoveCache(relation);
			}

			// Token: 0x0400072C RID: 1836
			private readonly DataTable table;

			// Token: 0x0400072D RID: 1837
			private readonly ArrayList relations;

			// Token: 0x0400072E RID: 1838
			private readonly bool fParentCollection;

			// Token: 0x0400072F RID: 1839
			private CollectionChangeEventHandler onRelationPropertyChangedDelegate;
		}

		// Token: 0x0200007F RID: 127
		internal sealed class DataSetRelationCollection : DataRelationCollection
		{
			// Token: 0x0600075F RID: 1887 RVA: 0x001DF260 File Offset: 0x001DE660
			internal DataSetRelationCollection(DataSet dataSet)
			{
				if (dataSet == null)
				{
					throw ExceptionBuilder.RelationDataSetNull();
				}
				this.dataSet = dataSet;
				this.relations = new ArrayList();
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x06000760 RID: 1888 RVA: 0x001DF290 File Offset: 0x001DE690
			protected override ArrayList List
			{
				get
				{
					return this.relations;
				}
			}

			// Token: 0x06000761 RID: 1889 RVA: 0x001DF2A4 File Offset: 0x001DE6A4
			public override void AddRange(DataRelation[] relations)
			{
				if (this.dataSet.fInitInProgress)
				{
					this.delayLoadingRelations = relations;
					return;
				}
				if (relations != null)
				{
					foreach (DataRelation dataRelation in relations)
					{
						if (dataRelation != null)
						{
							base.Add(dataRelation);
						}
					}
				}
			}

			// Token: 0x06000762 RID: 1890 RVA: 0x001DF2E8 File Offset: 0x001DE6E8
			public override void Clear()
			{
				base.Clear();
				if (this.dataSet.fInitInProgress && this.delayLoadingRelations != null)
				{
					this.delayLoadingRelations = null;
				}
			}

			// Token: 0x06000763 RID: 1891 RVA: 0x001DF318 File Offset: 0x001DE718
			protected override DataSet GetDataSet()
			{
				return this.dataSet;
			}

			// Token: 0x170000DE RID: 222
			public override DataRelation this[int index]
			{
				get
				{
					if (index >= 0 && index < this.relations.Count)
					{
						return (DataRelation)this.relations[index];
					}
					throw ExceptionBuilder.RelationOutOfRange(index);
				}
			}

			// Token: 0x170000DF RID: 223
			public override DataRelation this[string name]
			{
				get
				{
					int num = base.InternalIndexOf(name);
					if (num == -2)
					{
						throw ExceptionBuilder.CaseInsensitiveNameConflict(name);
					}
					if (num >= 0)
					{
						return (DataRelation)this.List[num];
					}
					return null;
				}
			}

			// Token: 0x06000766 RID: 1894 RVA: 0x001DF3A0 File Offset: 0x001DE7A0
			protected override void AddCore(DataRelation relation)
			{
				base.AddCore(relation);
				if (relation.ChildTable.DataSet != this.dataSet || relation.ParentTable.DataSet != this.dataSet)
				{
					throw ExceptionBuilder.ForeignRelation();
				}
				relation.CheckState();
				if (relation.Nested)
				{
					relation.CheckNestedRelations();
				}
				if (relation.relationName.Length == 0)
				{
					relation.relationName = base.AssignName();
				}
				else
				{
					base.RegisterName(relation.relationName);
				}
				DataKey childKey = relation.ChildKey;
				for (int i = 0; i < this.relations.Count; i++)
				{
					if (childKey.ColumnsEqual(((DataRelation)this.relations[i]).ChildKey) && relation.ParentKey.ColumnsEqual(((DataRelation)this.relations[i]).ParentKey))
					{
						throw ExceptionBuilder.RelationAlreadyExists();
					}
				}
				this.relations.Add(relation);
				((DataRelationCollection.DataTableRelationCollection)relation.ParentTable.ChildRelations).Add(relation);
				((DataRelationCollection.DataTableRelationCollection)relation.ChildTable.ParentRelations).Add(relation);
				relation.SetDataSet(this.dataSet);
				relation.ChildKey.GetSortIndex().AddRef();
				if (relation.Nested)
				{
					relation.ChildTable.CacheNestedParent();
				}
				ForeignKeyConstraint foreignKeyConstraint = relation.ChildTable.Constraints.FindForeignKeyConstraint(relation.ParentColumnsReference, relation.ChildColumnsReference);
				if (relation.createConstraints && foreignKeyConstraint == null)
				{
					relation.ChildTable.Constraints.Add(foreignKeyConstraint = new ForeignKeyConstraint(relation.ParentColumnsReference, relation.ChildColumnsReference));
					try
					{
						foreignKeyConstraint.ConstraintName = relation.RelationName;
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ExceptionBuilder.TraceExceptionWithoutRethrow(ex);
					}
				}
				UniqueConstraint uniqueConstraint = relation.ParentTable.Constraints.FindKeyConstraint(relation.ParentColumnsReference);
				relation.SetParentKeyConstraint(uniqueConstraint);
				relation.SetChildKeyConstraint(foreignKeyConstraint);
			}

			// Token: 0x06000767 RID: 1895 RVA: 0x001DF5A0 File Offset: 0x001DE9A0
			protected override void RemoveCore(DataRelation relation)
			{
				base.RemoveCore(relation);
				this.dataSet.OnRemoveRelationHack(relation);
				relation.SetDataSet(null);
				relation.ChildKey.GetSortIndex().RemoveRef();
				if (relation.Nested)
				{
					relation.ChildTable.CacheNestedParent();
				}
				for (int i = 0; i < this.relations.Count; i++)
				{
					if (relation == this.relations[i])
					{
						this.relations.RemoveAt(i);
						((DataRelationCollection.DataTableRelationCollection)relation.ParentTable.ChildRelations).Remove(relation);
						((DataRelationCollection.DataTableRelationCollection)relation.ChildTable.ParentRelations).Remove(relation);
						if (relation.Nested)
						{
							relation.ChildTable.CacheNestedParent();
						}
						base.UnregisterName(relation.RelationName);
						relation.SetParentKeyConstraint(null);
						relation.SetChildKeyConstraint(null);
						return;
					}
				}
				throw ExceptionBuilder.RelationDoesNotExist();
			}

			// Token: 0x06000768 RID: 1896 RVA: 0x001DF684 File Offset: 0x001DEA84
			internal void FinishInitRelations()
			{
				if (this.delayLoadingRelations == null)
				{
					return;
				}
				for (int i = 0; i < this.delayLoadingRelations.Length; i++)
				{
					DataRelation dataRelation = this.delayLoadingRelations[i];
					if (dataRelation.parentColumnNames == null || dataRelation.childColumnNames == null)
					{
						base.Add(dataRelation);
					}
					else
					{
						int num = dataRelation.parentColumnNames.Length;
						DataColumn[] array = new DataColumn[num];
						DataColumn[] array2 = new DataColumn[num];
						for (int j = 0; j < num; j++)
						{
							if (dataRelation.parentTableNamespace == null)
							{
								array[j] = this.dataSet.Tables[dataRelation.parentTableName].Columns[dataRelation.parentColumnNames[j]];
							}
							else
							{
								array[j] = this.dataSet.Tables[dataRelation.parentTableName, dataRelation.parentTableNamespace].Columns[dataRelation.parentColumnNames[j]];
							}
							if (dataRelation.childTableNamespace == null)
							{
								array2[j] = this.dataSet.Tables[dataRelation.childTableName].Columns[dataRelation.childColumnNames[j]];
							}
							else
							{
								array2[j] = this.dataSet.Tables[dataRelation.childTableName, dataRelation.childTableNamespace].Columns[dataRelation.childColumnNames[j]];
							}
						}
						base.Add(new DataRelation(dataRelation.relationName, array, array2, false)
						{
							Nested = dataRelation.nested
						});
					}
				}
				this.delayLoadingRelations = null;
			}

			// Token: 0x04000730 RID: 1840
			private readonly DataSet dataSet;

			// Token: 0x04000731 RID: 1841
			private readonly ArrayList relations;

			// Token: 0x04000732 RID: 1842
			private DataRelation[] delayLoadingRelations;
		}
	}
}
