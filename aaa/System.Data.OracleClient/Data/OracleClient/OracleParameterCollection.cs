using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.OracleClient
{
	// Token: 0x02000073 RID: 115
	[Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ListBindable(false)]
	public sealed class OracleParameterCollection : DbParameterCollection
	{
		// Token: 0x1700012E RID: 302
		public OracleParameter this[int index]
		{
			get
			{
				return (OracleParameter)this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, value);
			}
		}

		// Token: 0x1700012F RID: 303
		public OracleParameter this[string parameterName]
		{
			get
			{
				int num = this.IndexOf(parameterName);
				return (OracleParameter)this.GetParameter(num);
			}
			set
			{
				int num = this.IndexOf(parameterName);
				this.SetParameter(num, value);
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0006D650 File Offset: 0x0006CA50
		public OracleParameter Add(OracleParameter value)
		{
			this.Add(value);
			return value;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0006D668 File Offset: 0x0006CA68
		[Obsolete("Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public OracleParameter Add(string parameterName, object value)
		{
			OracleParameter oracleParameter = new OracleParameter(parameterName, value);
			return this.Add(oracleParameter);
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0006D684 File Offset: 0x0006CA84
		public OracleParameter Add(string parameterName, OracleType dataType)
		{
			OracleParameter oracleParameter = new OracleParameter(parameterName, dataType);
			return this.Add(oracleParameter);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0006D6A0 File Offset: 0x0006CAA0
		public OracleParameter Add(string parameterName, OracleType dataType, int size)
		{
			OracleParameter oracleParameter = new OracleParameter(parameterName, dataType, size);
			return this.Add(oracleParameter);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0006D6C0 File Offset: 0x0006CAC0
		public OracleParameter Add(string parameterName, OracleType dataType, int size, string srcColumn)
		{
			OracleParameter oracleParameter = new OracleParameter(parameterName, dataType, size, srcColumn);
			return this.Add(oracleParameter);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0006D6E0 File Offset: 0x0006CAE0
		public void AddRange(OracleParameter[] values)
		{
			this.AddRange(values);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0006D6F4 File Offset: 0x0006CAF4
		public OracleParameter AddWithValue(string parameterName, object value)
		{
			OracleParameter oracleParameter = new OracleParameter(parameterName, value);
			return this.Add(oracleParameter);
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0006D710 File Offset: 0x0006CB10
		public override bool Contains(string parameterName)
		{
			return -1 != this.IndexOf(parameterName);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0006D72C File Offset: 0x0006CB2C
		public bool Contains(OracleParameter value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0006D748 File Offset: 0x0006CB48
		public void CopyTo(OracleParameter[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0006D760 File Offset: 0x0006CB60
		public int IndexOf(OracleParameter value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0006D774 File Offset: 0x0006CB74
		public void Insert(int index, OracleParameter value)
		{
			this.Insert(index, value);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0006D78C File Offset: 0x0006CB8C
		private void OnChange()
		{
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0006D79C File Offset: 0x0006CB9C
		public void Remove(OracleParameter value)
		{
			this.Remove(value);
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0006D7B0 File Offset: 0x0006CBB0
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000640 RID: 1600 RVA: 0x0006D7D4 File Offset: 0x0006CBD4
		private List<OracleParameter> InnerList
		{
			get
			{
				List<OracleParameter> list = this._items;
				if (list == null)
				{
					list = new List<OracleParameter>();
					this._items = list;
				}
				return list;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0006D7FC File Offset: 0x0006CBFC
		public override bool IsFixedSize
		{
			get
			{
				return ((IList)this.InnerList).IsFixedSize;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0006D814 File Offset: 0x0006CC14
		public override bool IsReadOnly
		{
			get
			{
				return ((IList)this.InnerList).IsReadOnly;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0006D82C File Offset: 0x0006CC2C
		public override bool IsSynchronized
		{
			get
			{
				return ((ICollection)this.InnerList).IsSynchronized;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0006D844 File Offset: 0x0006CC44
		public override object SyncRoot
		{
			get
			{
				return ((ICollection)this.InnerList).SyncRoot;
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0006D85C File Offset: 0x0006CC5C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int Add(object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, value);
			this.InnerList.Add((OracleParameter)value);
			return this.Count - 1;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0006D898 File Offset: 0x0006CC98
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
				OracleParameter oracleParameter = (OracleParameter)obj2;
				this.Validate(-1, oracleParameter);
				this.InnerList.Add(oracleParameter);
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0006D968 File Offset: 0x0006CD68
		private int CheckName(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OracleParameterCollection.ItemType);
			}
			return num;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0006D990 File Offset: 0x0006CD90
		public override void Clear()
		{
			this.OnChange();
			List<OracleParameter> innerList = this.InnerList;
			if (innerList != null)
			{
				foreach (OracleParameter oracleParameter in innerList)
				{
					oracleParameter.ResetParent();
				}
				innerList.Clear();
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0006DA00 File Offset: 0x0006CE00
		public override bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0006DA1C File Offset: 0x0006CE1C
		public override void CopyTo(Array array, int index)
		{
			((ICollection)this.InnerList).CopyTo(array, index);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0006DA38 File Offset: 0x0006CE38
		public override IEnumerator GetEnumerator()
		{
			return ((IEnumerable)this.InnerList).GetEnumerator();
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0006DA50 File Offset: 0x0006CE50
		protected override DbParameter GetParameter(int index)
		{
			this.RangeCheck(index);
			return this.InnerList[index];
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0006DA70 File Offset: 0x0006CE70
		protected override DbParameter GetParameter(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OracleParameterCollection.ItemType);
			}
			return this.InnerList[num];
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0006DAA4 File Offset: 0x0006CEA4
		private static int IndexOf(IEnumerable items, string parameterName)
		{
			if (items != null)
			{
				int num = 0;
				foreach (object obj in items)
				{
					OracleParameter oracleParameter = (OracleParameter)obj;
					if (ADP.SrcCompare(parameterName, oracleParameter.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				num = 0;
				foreach (object obj2 in items)
				{
					OracleParameter oracleParameter2 = (OracleParameter)obj2;
					if (ADP.DstCompare(parameterName, oracleParameter2.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0006DB88 File Offset: 0x0006CF88
		public override int IndexOf(string parameterName)
		{
			return OracleParameterCollection.IndexOf(this.InnerList, parameterName);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0006DBA4 File Offset: 0x0006CFA4
		public override int IndexOf(object value)
		{
			if (value != null)
			{
				this.ValidateType(value);
				List<OracleParameter> innerList = this.InnerList;
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

		// Token: 0x06000651 RID: 1617 RVA: 0x0006DBE8 File Offset: 0x0006CFE8
		public override void Insert(int index, object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, (OracleParameter)value);
			this.InnerList.Insert(index, (OracleParameter)value);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0006DC24 File Offset: 0x0006D024
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw ADP.ParametersMappingIndex(index, this);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0006DC48 File Offset: 0x0006D048
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
			if (this != ((OracleParameter)value).CompareExchangeParent(null, this))
			{
				throw ADP.CollectionRemoveInvalidObject(OracleParameterCollection.ItemType, this);
			}
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x0006DC94 File Offset: 0x0006D094
		public override void RemoveAt(int index)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0006DCB8 File Offset: 0x0006D0B8
		public override void RemoveAt(string parameterName)
		{
			this.OnChange();
			int num = this.CheckName(parameterName);
			this.RemoveIndex(num);
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0006DCDC File Offset: 0x0006D0DC
		private void RemoveIndex(int index)
		{
			List<OracleParameter> innerList = this.InnerList;
			OracleParameter oracleParameter = innerList[index];
			innerList.RemoveAt(index);
			oracleParameter.ResetParent();
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0006DD08 File Offset: 0x0006D108
		private void Replace(int index, object newValue)
		{
			List<OracleParameter> innerList = this.InnerList;
			this.ValidateType(newValue);
			this.Validate(index, newValue);
			OracleParameter oracleParameter = innerList[index];
			innerList[index] = (OracleParameter)newValue;
			oracleParameter.ResetParent();
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0006DD48 File Offset: 0x0006D148
		protected override void SetParameter(int index, DbParameter value)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.Replace(index, value);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0006DD6C File Offset: 0x0006D16C
		protected override void SetParameter(string parameterName, DbParameter value)
		{
			this.OnChange();
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OracleParameterCollection.ItemType);
			}
			this.Replace(num, value);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0006DDA0 File Offset: 0x0006D1A0
		private void Validate(int index, object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, OracleParameterCollection.ItemType);
			}
			object obj = ((OracleParameter)value).CompareExchangeParent(this, null);
			if (obj != null)
			{
				if (this != obj)
				{
					throw ADP.ParametersIsNotParent(OracleParameterCollection.ItemType, this);
				}
				if (index != this.IndexOf(value))
				{
					throw ADP.ParametersIsParent(OracleParameterCollection.ItemType, this);
				}
			}
			string text = ((OracleParameter)value).ParameterName;
			if (text.Length == 0)
			{
				index = 1;
				do
				{
					text = "Parameter" + index.ToString(CultureInfo.CurrentCulture);
					index++;
				}
				while (-1 != this.IndexOf(text));
				((OracleParameter)value).ParameterName = text;
			}
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0006DE44 File Offset: 0x0006D244
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, OracleParameterCollection.ItemType);
			}
			if (!OracleParameterCollection.ItemType.IsInstanceOfType(value))
			{
				throw ADP.InvalidParameterType(this, OracleParameterCollection.ItemType, value);
			}
		}

		// Token: 0x040004A4 RID: 1188
		private static Type ItemType = typeof(OracleParameter);

		// Token: 0x040004A5 RID: 1189
		private List<OracleParameter> _items;
	}
}
