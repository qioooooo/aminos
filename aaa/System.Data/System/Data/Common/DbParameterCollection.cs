using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data.Common
{
	// Token: 0x0200013C RID: 316
	public abstract class DbParameterCollection : MarshalByRefObject, IDataParameterCollection, IList, ICollection, IEnumerable
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x060014BD RID: 5309
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public abstract int Count { get; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x060014BE RID: 5310
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public abstract bool IsFixedSize { get; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x060014BF RID: 5311
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public abstract bool IsReadOnly { get; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x060014C0 RID: 5312
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract bool IsSynchronized { get; }

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x060014C1 RID: 5313
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract object SyncRoot { get; }

		// Token: 0x170002E7 RID: 743
		object IList.this[int index]
		{
			get
			{
				return this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, (DbParameter)value);
			}
		}

		// Token: 0x170002E8 RID: 744
		object IDataParameterCollection.this[string parameterName]
		{
			get
			{
				return this.GetParameter(parameterName);
			}
			set
			{
				this.SetParameter(parameterName, (DbParameter)value);
			}
		}

		// Token: 0x170002E9 RID: 745
		public DbParameter this[int index]
		{
			get
			{
				return this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, value);
			}
		}

		// Token: 0x170002EA RID: 746
		public DbParameter this[string parameterName]
		{
			get
			{
				return this.GetParameter(parameterName);
			}
			set
			{
				this.SetParameter(parameterName, value);
			}
		}

		// Token: 0x060014CA RID: 5322
		public abstract int Add(object value);

		// Token: 0x060014CB RID: 5323
		public abstract void AddRange(Array values);

		// Token: 0x060014CC RID: 5324
		public abstract bool Contains(object value);

		// Token: 0x060014CD RID: 5325
		public abstract bool Contains(string value);

		// Token: 0x060014CE RID: 5326
		public abstract void CopyTo(Array array, int index);

		// Token: 0x060014CF RID: 5327
		public abstract void Clear();

		// Token: 0x060014D0 RID: 5328
		[EditorBrowsable(EditorBrowsableState.Never)]
		public abstract IEnumerator GetEnumerator();

		// Token: 0x060014D1 RID: 5329
		protected abstract DbParameter GetParameter(int index);

		// Token: 0x060014D2 RID: 5330
		protected abstract DbParameter GetParameter(string parameterName);

		// Token: 0x060014D3 RID: 5331
		public abstract int IndexOf(object value);

		// Token: 0x060014D4 RID: 5332
		public abstract int IndexOf(string parameterName);

		// Token: 0x060014D5 RID: 5333
		public abstract void Insert(int index, object value);

		// Token: 0x060014D6 RID: 5334
		public abstract void Remove(object value);

		// Token: 0x060014D7 RID: 5335
		public abstract void RemoveAt(int index);

		// Token: 0x060014D8 RID: 5336
		public abstract void RemoveAt(string parameterName);

		// Token: 0x060014D9 RID: 5337
		protected abstract void SetParameter(int index, DbParameter value);

		// Token: 0x060014DA RID: 5338
		protected abstract void SetParameter(string parameterName, DbParameter value);
	}
}
