using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x02000307 RID: 775
	[ListBindable(false)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class SqlParameterCollection : DbParameterCollection
	{
		// Token: 0x06002889 RID: 10377 RVA: 0x00290154 File Offset: 0x0028F554
		internal SqlParameterCollection()
		{
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x00290168 File Offset: 0x0028F568
		// (set) Token: 0x0600288B RID: 10379 RVA: 0x0029017C File Offset: 0x0028F57C
		internal bool IsDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				this._isDirty = value;
			}
		}

		// Token: 0x170006AD RID: 1709
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public SqlParameter this[int index]
		{
			get
			{
				return (SqlParameter)this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, value);
			}
		}

		// Token: 0x170006AE RID: 1710
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public SqlParameter this[string parameterName]
		{
			get
			{
				return (SqlParameter)this.GetParameter(parameterName);
			}
			set
			{
				this.SetParameter(parameterName, value);
			}
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x002901F8 File Offset: 0x0028F5F8
		public SqlParameter Add(SqlParameter value)
		{
			this.Add(value);
			return value;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x00290210 File Offset: 0x0028F610
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		public SqlParameter Add(string parameterName, object value)
		{
			return this.Add(new SqlParameter(parameterName, value));
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x0029022C File Offset: 0x0028F62C
		public SqlParameter AddWithValue(string parameterName, object value)
		{
			return this.Add(new SqlParameter(parameterName, value));
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x00290248 File Offset: 0x0028F648
		public SqlParameter Add(string parameterName, SqlDbType sqlDbType)
		{
			return this.Add(new SqlParameter(parameterName, sqlDbType));
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x00290264 File Offset: 0x0028F664
		public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size)
		{
			return this.Add(new SqlParameter(parameterName, sqlDbType, size));
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x00290280 File Offset: 0x0028F680
		public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, string sourceColumn)
		{
			return this.Add(new SqlParameter(parameterName, sqlDbType, size, sourceColumn));
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x002902A0 File Offset: 0x0028F6A0
		public void AddRange(SqlParameter[] values)
		{
			this.AddRange(values);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x002902B4 File Offset: 0x0028F6B4
		public override bool Contains(string value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x002902D0 File Offset: 0x0028F6D0
		public bool Contains(SqlParameter value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x002902EC File Offset: 0x0028F6EC
		public void CopyTo(SqlParameter[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x00290304 File Offset: 0x0028F704
		public int IndexOf(SqlParameter value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x00290318 File Offset: 0x0028F718
		public void Insert(int index, SqlParameter value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x00290330 File Offset: 0x0028F730
		private void OnChange()
		{
			this.IsDirty = true;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x00290344 File Offset: 0x0028F744
		public void Remove(SqlParameter value)
		{
			this.Remove(value);
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x00290358 File Offset: 0x0028F758
		public override int Count
		{
			get
			{
				if (this._items == null)
				{
					return 0;
				}
				return this._items.Count;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x0600289F RID: 10399 RVA: 0x0029037C File Offset: 0x0028F77C
		private List<SqlParameter> InnerList
		{
			get
			{
				List<SqlParameter> list = this._items;
				if (list == null)
				{
					list = new List<SqlParameter>();
					this._items = list;
				}
				return list;
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060028A0 RID: 10400 RVA: 0x002903A4 File Offset: 0x0028F7A4
		public override bool IsFixedSize
		{
			get
			{
				return ((IList)this.InnerList).IsFixedSize;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060028A1 RID: 10401 RVA: 0x002903BC File Offset: 0x0028F7BC
		public override bool IsReadOnly
		{
			get
			{
				return ((IList)this.InnerList).IsReadOnly;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060028A2 RID: 10402 RVA: 0x002903D4 File Offset: 0x0028F7D4
		public override bool IsSynchronized
		{
			get
			{
				return ((ICollection)this.InnerList).IsSynchronized;
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060028A3 RID: 10403 RVA: 0x002903EC File Offset: 0x0028F7EC
		public override object SyncRoot
		{
			get
			{
				return ((ICollection)this.InnerList).SyncRoot;
			}
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x00290404 File Offset: 0x0028F804
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int Add(object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, value);
			this.InnerList.Add((SqlParameter)value);
			return this.Count - 1;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x00290440 File Offset: 0x0028F840
		public override void AddRange(Array values)
		{
			this.OnChange();
			if (values == null)
			{
				throw ADP.ArgumentNull("values");
			}
			foreach (object obj in values)
			{
				this.ValidateType(obj);
			}
			foreach (object obj2 in values)
			{
				SqlParameter sqlParameter = (SqlParameter)obj2;
				this.Validate(-1, sqlParameter);
				this.InnerList.Add(sqlParameter);
			}
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x00290510 File Offset: 0x0028F910
		private int CheckName(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, SqlParameterCollection.ItemType);
			}
			return num;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x00290538 File Offset: 0x0028F938
		public override void Clear()
		{
			this.OnChange();
			List<SqlParameter> innerList = this.InnerList;
			if (innerList != null)
			{
				foreach (SqlParameter sqlParameter in innerList)
				{
					sqlParameter.ResetParent();
				}
				innerList.Clear();
			}
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x002905A8 File Offset: 0x0028F9A8
		public override bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x002905C4 File Offset: 0x0028F9C4
		public override void CopyTo(Array array, int index)
		{
			((ICollection)this.InnerList).CopyTo(array, index);
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x002905E0 File Offset: 0x0028F9E0
		public override IEnumerator GetEnumerator()
		{
			return ((IEnumerable)this.InnerList).GetEnumerator();
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x002905F8 File Offset: 0x0028F9F8
		protected override DbParameter GetParameter(int index)
		{
			this.RangeCheck(index);
			return this.InnerList[index];
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x00290618 File Offset: 0x0028FA18
		protected override DbParameter GetParameter(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, SqlParameterCollection.ItemType);
			}
			return this.InnerList[num];
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x0029064C File Offset: 0x0028FA4C
		private static int IndexOf(IEnumerable items, string parameterName)
		{
			if (items != null)
			{
				int num = 0;
				foreach (object obj in items)
				{
					SqlParameter sqlParameter = (SqlParameter)obj;
					if (ADP.SrcCompare(parameterName, sqlParameter.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				num = 0;
				foreach (object obj2 in items)
				{
					SqlParameter sqlParameter2 = (SqlParameter)obj2;
					if (ADP.DstCompare(parameterName, sqlParameter2.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x00290730 File Offset: 0x0028FB30
		public override int IndexOf(string parameterName)
		{
			return SqlParameterCollection.IndexOf(this.InnerList, parameterName);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x0029074C File Offset: 0x0028FB4C
		public override int IndexOf(object value)
		{
			if (value != null)
			{
				this.ValidateType(value);
				List<SqlParameter> innerList = this.InnerList;
				if (innerList != null)
				{
					int count = innerList.Count;
					for (int i = 0; i < count; i++)
					{
						if (value == innerList[i])
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x00290790 File Offset: 0x0028FB90
		public override void Insert(int index, object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, (SqlParameter)value);
			this.InnerList.Insert(index, (SqlParameter)value);
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x002907CC File Offset: 0x0028FBCC
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw ADP.ParametersMappingIndex(index, this);
			}
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x002907F0 File Offset: 0x0028FBF0
		public override void Remove(object value)
		{
			this.OnChange();
			this.ValidateType(value);
			int num = this.IndexOf(value);
			if (-1 != num)
			{
				this.RemoveIndex(num);
				return;
			}
			if (this != ((SqlParameter)value).CompareExchangeParent(null, this))
			{
				throw ADP.CollectionRemoveInvalidObject(SqlParameterCollection.ItemType, this);
			}
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x0029083C File Offset: 0x0028FC3C
		public override void RemoveAt(int index)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x00290860 File Offset: 0x0028FC60
		public override void RemoveAt(string parameterName)
		{
			this.OnChange();
			int num = this.CheckName(parameterName);
			this.RemoveIndex(num);
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x00290884 File Offset: 0x0028FC84
		private void RemoveIndex(int index)
		{
			List<SqlParameter> innerList = this.InnerList;
			SqlParameter sqlParameter = innerList[index];
			innerList.RemoveAt(index);
			sqlParameter.ResetParent();
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x002908B0 File Offset: 0x0028FCB0
		private void Replace(int index, object newValue)
		{
			List<SqlParameter> innerList = this.InnerList;
			this.ValidateType(newValue);
			this.Validate(index, newValue);
			SqlParameter sqlParameter = innerList[index];
			innerList[index] = (SqlParameter)newValue;
			sqlParameter.ResetParent();
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x002908F0 File Offset: 0x0028FCF0
		protected override void SetParameter(int index, DbParameter value)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.Replace(index, value);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x00290914 File Offset: 0x0028FD14
		protected override void SetParameter(string parameterName, DbParameter value)
		{
			this.OnChange();
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, SqlParameterCollection.ItemType);
			}
			this.Replace(num, value);
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x00290948 File Offset: 0x0028FD48
		private void Validate(int index, object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, SqlParameterCollection.ItemType);
			}
			object obj = ((SqlParameter)value).CompareExchangeParent(this, null);
			if (obj != null)
			{
				if (this != obj)
				{
					throw ADP.ParametersIsNotParent(SqlParameterCollection.ItemType, this);
				}
				if (index != this.IndexOf(value))
				{
					throw ADP.ParametersIsParent(SqlParameterCollection.ItemType, this);
				}
			}
			string text = ((SqlParameter)value).ParameterName;
			if (text.Length == 0)
			{
				index = 1;
				do
				{
					text = "Parameter" + index.ToString(CultureInfo.CurrentCulture);
					index++;
				}
				while (-1 != this.IndexOf(text));
				((SqlParameter)value).ParameterName = text;
			}
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x002909EC File Offset: 0x0028FDEC
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, SqlParameterCollection.ItemType);
			}
			if (!SqlParameterCollection.ItemType.IsInstanceOfType(value))
			{
				throw ADP.InvalidParameterType(this, SqlParameterCollection.ItemType, value);
			}
		}

		// Token: 0x0400196A RID: 6506
		private bool _isDirty;

		// Token: 0x0400196B RID: 6507
		private static Type ItemType = typeof(SqlParameter);

		// Token: 0x0400196C RID: 6508
		private List<SqlParameter> _items;
	}
}
