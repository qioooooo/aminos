using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.OleDb
{
	// Token: 0x02000234 RID: 564
	[ListBindable(false)]
	[Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public sealed class OleDbParameterCollection : DbParameterCollection
	{
		// Token: 0x06002012 RID: 8210 RVA: 0x00261088 File Offset: 0x00260488
		internal OleDbParameterCollection()
		{
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x0026109C File Offset: 0x0026049C
		internal int ChangeID
		{
			get
			{
				return this._changeID;
			}
		}

		// Token: 0x17000469 RID: 1129
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public OleDbParameter this[int index]
		{
			get
			{
				return (OleDbParameter)this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, value);
			}
		}

		// Token: 0x1700046A RID: 1130
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public OleDbParameter this[string parameterName]
		{
			get
			{
				return (OleDbParameter)this.GetParameter(parameterName);
			}
			set
			{
				this.SetParameter(parameterName, value);
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x00261118 File Offset: 0x00260518
		public OleDbParameter Add(OleDbParameter value)
		{
			this.Add(value);
			return value;
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x00261130 File Offset: 0x00260530
		[Obsolete("Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public OleDbParameter Add(string parameterName, object value)
		{
			return this.Add(new OleDbParameter(parameterName, value));
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0026114C File Offset: 0x0026054C
		public OleDbParameter AddWithValue(string parameterName, object value)
		{
			return this.Add(new OleDbParameter(parameterName, value));
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x00261168 File Offset: 0x00260568
		public OleDbParameter Add(string parameterName, OleDbType oleDbType)
		{
			return this.Add(new OleDbParameter(parameterName, oleDbType));
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x00261184 File Offset: 0x00260584
		public OleDbParameter Add(string parameterName, OleDbType oleDbType, int size)
		{
			return this.Add(new OleDbParameter(parameterName, oleDbType, size));
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x002611A0 File Offset: 0x002605A0
		public OleDbParameter Add(string parameterName, OleDbType oleDbType, int size, string sourceColumn)
		{
			return this.Add(new OleDbParameter(parameterName, oleDbType, size, sourceColumn));
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x002611C0 File Offset: 0x002605C0
		public void AddRange(OleDbParameter[] values)
		{
			this.AddRange(values);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x002611D4 File Offset: 0x002605D4
		public override bool Contains(string value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x002611F0 File Offset: 0x002605F0
		public bool Contains(OleDbParameter value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0026120C File Offset: 0x0026060C
		public void CopyTo(OleDbParameter[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x00261224 File Offset: 0x00260624
		public int IndexOf(OleDbParameter value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x00261238 File Offset: 0x00260638
		public void Insert(int index, OleDbParameter value)
		{
			this.Insert(index, value);
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x00261250 File Offset: 0x00260650
		private void OnChange()
		{
			this._changeID++;
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0026126C File Offset: 0x0026066C
		public void Remove(OleDbParameter value)
		{
			this.Remove(value);
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x00261280 File Offset: 0x00260680
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

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x002612A4 File Offset: 0x002606A4
		private List<OleDbParameter> InnerList
		{
			get
			{
				List<OleDbParameter> list = this._items;
				if (list == null)
				{
					list = new List<OleDbParameter>();
					this._items = list;
				}
				return list;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x002612CC File Offset: 0x002606CC
		public override bool IsFixedSize
		{
			get
			{
				return ((IList)this.InnerList).IsFixedSize;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x002612E4 File Offset: 0x002606E4
		public override bool IsReadOnly
		{
			get
			{
				return ((IList)this.InnerList).IsReadOnly;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x002612FC File Offset: 0x002606FC
		public override bool IsSynchronized
		{
			get
			{
				return ((ICollection)this.InnerList).IsSynchronized;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x00261314 File Offset: 0x00260714
		public override object SyncRoot
		{
			get
			{
				return ((ICollection)this.InnerList).SyncRoot;
			}
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0026132C File Offset: 0x0026072C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int Add(object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, value);
			this.InnerList.Add((OleDbParameter)value);
			return this.Count - 1;
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00261368 File Offset: 0x00260768
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
				OleDbParameter oleDbParameter = (OleDbParameter)obj2;
				this.Validate(-1, oleDbParameter);
				this.InnerList.Add(oleDbParameter);
			}
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x00261438 File Offset: 0x00260838
		private int CheckName(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OleDbParameterCollection.ItemType);
			}
			return num;
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x00261460 File Offset: 0x00260860
		public override void Clear()
		{
			this.OnChange();
			List<OleDbParameter> innerList = this.InnerList;
			if (innerList != null)
			{
				foreach (OleDbParameter oleDbParameter in innerList)
				{
					oleDbParameter.ResetParent();
				}
				innerList.Clear();
			}
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x002614D0 File Offset: 0x002608D0
		public override bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x002614EC File Offset: 0x002608EC
		public override void CopyTo(Array array, int index)
		{
			((ICollection)this.InnerList).CopyTo(array, index);
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x00261508 File Offset: 0x00260908
		public override IEnumerator GetEnumerator()
		{
			return ((IEnumerable)this.InnerList).GetEnumerator();
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x00261520 File Offset: 0x00260920
		protected override DbParameter GetParameter(int index)
		{
			this.RangeCheck(index);
			return this.InnerList[index];
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x00261540 File Offset: 0x00260940
		protected override DbParameter GetParameter(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OleDbParameterCollection.ItemType);
			}
			return this.InnerList[num];
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x00261574 File Offset: 0x00260974
		private static int IndexOf(IEnumerable items, string parameterName)
		{
			if (items != null)
			{
				int num = 0;
				foreach (object obj in items)
				{
					OleDbParameter oleDbParameter = (OleDbParameter)obj;
					if (ADP.SrcCompare(parameterName, oleDbParameter.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				num = 0;
				foreach (object obj2 in items)
				{
					OleDbParameter oleDbParameter2 = (OleDbParameter)obj2;
					if (ADP.DstCompare(parameterName, oleDbParameter2.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x06002036 RID: 8246 RVA: 0x00261658 File Offset: 0x00260A58
		public override int IndexOf(string parameterName)
		{
			return OleDbParameterCollection.IndexOf(this.InnerList, parameterName);
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00261674 File Offset: 0x00260A74
		public override int IndexOf(object value)
		{
			if (value != null)
			{
				this.ValidateType(value);
				List<OleDbParameter> innerList = this.InnerList;
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

		// Token: 0x06002038 RID: 8248 RVA: 0x002616B8 File Offset: 0x00260AB8
		public override void Insert(int index, object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, (OleDbParameter)value);
			this.InnerList.Insert(index, (OleDbParameter)value);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x002616F4 File Offset: 0x00260AF4
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw ADP.ParametersMappingIndex(index, this);
			}
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x00261718 File Offset: 0x00260B18
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
			if (this != ((OleDbParameter)value).CompareExchangeParent(null, this))
			{
				throw ADP.CollectionRemoveInvalidObject(OleDbParameterCollection.ItemType, this);
			}
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x00261764 File Offset: 0x00260B64
		public override void RemoveAt(int index)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00261788 File Offset: 0x00260B88
		public override void RemoveAt(string parameterName)
		{
			this.OnChange();
			int num = this.CheckName(parameterName);
			this.RemoveIndex(num);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x002617AC File Offset: 0x00260BAC
		private void RemoveIndex(int index)
		{
			List<OleDbParameter> innerList = this.InnerList;
			OleDbParameter oleDbParameter = innerList[index];
			innerList.RemoveAt(index);
			oleDbParameter.ResetParent();
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x002617D8 File Offset: 0x00260BD8
		private void Replace(int index, object newValue)
		{
			List<OleDbParameter> innerList = this.InnerList;
			this.ValidateType(newValue);
			this.Validate(index, newValue);
			OleDbParameter oleDbParameter = innerList[index];
			innerList[index] = (OleDbParameter)newValue;
			oleDbParameter.ResetParent();
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x00261818 File Offset: 0x00260C18
		protected override void SetParameter(int index, DbParameter value)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.Replace(index, value);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0026183C File Offset: 0x00260C3C
		protected override void SetParameter(string parameterName, DbParameter value)
		{
			this.OnChange();
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OleDbParameterCollection.ItemType);
			}
			this.Replace(num, value);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x00261870 File Offset: 0x00260C70
		private void Validate(int index, object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, OleDbParameterCollection.ItemType);
			}
			object obj = ((OleDbParameter)value).CompareExchangeParent(this, null);
			if (obj != null)
			{
				if (this != obj)
				{
					throw ADP.ParametersIsNotParent(OleDbParameterCollection.ItemType, this);
				}
				if (index != this.IndexOf(value))
				{
					throw ADP.ParametersIsParent(OleDbParameterCollection.ItemType, this);
				}
			}
			string text = ((OleDbParameter)value).ParameterName;
			if (text.Length == 0)
			{
				index = 1;
				do
				{
					text = "Parameter" + index.ToString(CultureInfo.CurrentCulture);
					index++;
				}
				while (-1 != this.IndexOf(text));
				((OleDbParameter)value).ParameterName = text;
			}
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x00261914 File Offset: 0x00260D14
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, OleDbParameterCollection.ItemType);
			}
			if (!OleDbParameterCollection.ItemType.IsInstanceOfType(value))
			{
				throw ADP.InvalidParameterType(this, OleDbParameterCollection.ItemType, value);
			}
		}

		// Token: 0x04001445 RID: 5189
		private int _changeID;

		// Token: 0x04001446 RID: 5190
		private static Type ItemType = typeof(OleDbParameter);

		// Token: 0x04001447 RID: 5191
		private List<OleDbParameter> _items;
	}
}
