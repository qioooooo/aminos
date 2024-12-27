using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.Odbc
{
	// Token: 0x020001F9 RID: 505
	[Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ListBindable(false)]
	public sealed class OdbcParameterCollection : DbParameterCollection
	{
		// Token: 0x06001C25 RID: 7205 RVA: 0x0024C678 File Offset: 0x0024BA78
		internal OdbcParameterCollection()
		{
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x0024C68C File Offset: 0x0024BA8C
		// (set) Token: 0x06001C27 RID: 7207 RVA: 0x0024C6A0 File Offset: 0x0024BAA0
		internal bool RebindCollection
		{
			get
			{
				return this._rebindCollection;
			}
			set
			{
				this._rebindCollection = value;
			}
		}

		// Token: 0x170003CB RID: 971
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public OdbcParameter this[int index]
		{
			get
			{
				return (OdbcParameter)this.GetParameter(index);
			}
			set
			{
				this.SetParameter(index, value);
			}
		}

		// Token: 0x170003CC RID: 972
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public OdbcParameter this[string parameterName]
		{
			get
			{
				return (OdbcParameter)this.GetParameter(parameterName);
			}
			set
			{
				this.SetParameter(parameterName, value);
			}
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0024C71C File Offset: 0x0024BB1C
		public OdbcParameter Add(OdbcParameter value)
		{
			this.Add(value);
			return value;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0024C734 File Offset: 0x0024BB34
		[Obsolete("Add(String parameterName, Object value) has been deprecated.  Use AddWithValue(String parameterName, Object value).  http://go.microsoft.com/fwlink/?linkid=14202", false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public OdbcParameter Add(string parameterName, object value)
		{
			return this.Add(new OdbcParameter(parameterName, value));
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0024C750 File Offset: 0x0024BB50
		public OdbcParameter AddWithValue(string parameterName, object value)
		{
			return this.Add(new OdbcParameter(parameterName, value));
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0024C76C File Offset: 0x0024BB6C
		public OdbcParameter Add(string parameterName, OdbcType odbcType)
		{
			return this.Add(new OdbcParameter(parameterName, odbcType));
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0024C788 File Offset: 0x0024BB88
		public OdbcParameter Add(string parameterName, OdbcType odbcType, int size)
		{
			return this.Add(new OdbcParameter(parameterName, odbcType, size));
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0024C7A4 File Offset: 0x0024BBA4
		public OdbcParameter Add(string parameterName, OdbcType odbcType, int size, string sourceColumn)
		{
			return this.Add(new OdbcParameter(parameterName, odbcType, size, sourceColumn));
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x0024C7C4 File Offset: 0x0024BBC4
		public void AddRange(OdbcParameter[] values)
		{
			this.AddRange(values);
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0024C7D8 File Offset: 0x0024BBD8
		internal void Bind(OdbcCommand command, CMDWrapper cmdWrapper, CNativeBuffer parameterBuffer)
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Bind(cmdWrapper.StatementHandle, command, checked((short)(i + 1)), parameterBuffer, true);
			}
			this._rebindCollection = false;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x0024C818 File Offset: 0x0024BC18
		internal int CalcParameterBufferSize(OdbcCommand command)
		{
			int num = 0;
			for (int i = 0; i < this.Count; i++)
			{
				if (this._rebindCollection)
				{
					this[i].HasChanged = true;
				}
				this[i].PrepareForBind(command, (short)(i + 1), ref num);
				num = (num + (IntPtr.Size - 1)) & ~(IntPtr.Size - 1);
			}
			return num;
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0024C874 File Offset: 0x0024BC74
		internal void ClearBindings()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].ClearBinding();
			}
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0024C8A0 File Offset: 0x0024BCA0
		public override bool Contains(string value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x0024C8BC File Offset: 0x0024BCBC
		public bool Contains(OdbcParameter value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0024C8D8 File Offset: 0x0024BCD8
		public void CopyTo(OdbcParameter[] array, int index)
		{
			this.CopyTo(array, index);
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0024C8F0 File Offset: 0x0024BCF0
		private void OnChange()
		{
			this._rebindCollection = true;
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0024C904 File Offset: 0x0024BD04
		internal void GetOutputValues(CMDWrapper cmdWrapper)
		{
			if (!this._rebindCollection)
			{
				CNativeBuffer nativeParameterBuffer = cmdWrapper._nativeParameterBuffer;
				for (int i = 0; i < this.Count; i++)
				{
					this[i].GetOutputValue(nativeParameterBuffer);
				}
			}
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0024C940 File Offset: 0x0024BD40
		public int IndexOf(OdbcParameter value)
		{
			return this.IndexOf(value);
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0024C954 File Offset: 0x0024BD54
		public void Insert(int index, OdbcParameter value)
		{
			this.Insert(index, value);
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0024C96C File Offset: 0x0024BD6C
		public void Remove(OdbcParameter value)
		{
			this.Remove(value);
		}

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x0024C980 File Offset: 0x0024BD80
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

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x0024C9A4 File Offset: 0x0024BDA4
		private List<OdbcParameter> InnerList
		{
			get
			{
				List<OdbcParameter> list = this._items;
				if (list == null)
				{
					list = new List<OdbcParameter>();
					this._items = list;
				}
				return list;
			}
		}

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0024C9CC File Offset: 0x0024BDCC
		public override bool IsFixedSize
		{
			get
			{
				return ((IList)this.InnerList).IsFixedSize;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x0024C9E4 File Offset: 0x0024BDE4
		public override bool IsReadOnly
		{
			get
			{
				return ((IList)this.InnerList).IsReadOnly;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x0024C9FC File Offset: 0x0024BDFC
		public override bool IsSynchronized
		{
			get
			{
				return ((ICollection)this.InnerList).IsSynchronized;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x0024CA14 File Offset: 0x0024BE14
		public override object SyncRoot
		{
			get
			{
				return ((ICollection)this.InnerList).SyncRoot;
			}
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x0024CA2C File Offset: 0x0024BE2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override int Add(object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, value);
			this.InnerList.Add((OdbcParameter)value);
			return this.Count - 1;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0024CA68 File Offset: 0x0024BE68
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
				OdbcParameter odbcParameter = (OdbcParameter)obj2;
				this.Validate(-1, odbcParameter);
				this.InnerList.Add(odbcParameter);
			}
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x0024CB38 File Offset: 0x0024BF38
		private int CheckName(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OdbcParameterCollection.ItemType);
			}
			return num;
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x0024CB60 File Offset: 0x0024BF60
		public override void Clear()
		{
			this.OnChange();
			List<OdbcParameter> innerList = this.InnerList;
			if (innerList != null)
			{
				foreach (OdbcParameter odbcParameter in innerList)
				{
					odbcParameter.ResetParent();
				}
				innerList.Clear();
			}
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x0024CBD0 File Offset: 0x0024BFD0
		public override bool Contains(object value)
		{
			return -1 != this.IndexOf(value);
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x0024CBEC File Offset: 0x0024BFEC
		public override void CopyTo(Array array, int index)
		{
			((ICollection)this.InnerList).CopyTo(array, index);
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x0024CC08 File Offset: 0x0024C008
		public override IEnumerator GetEnumerator()
		{
			return ((IEnumerable)this.InnerList).GetEnumerator();
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x0024CC20 File Offset: 0x0024C020
		protected override DbParameter GetParameter(int index)
		{
			this.RangeCheck(index);
			return this.InnerList[index];
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x0024CC40 File Offset: 0x0024C040
		protected override DbParameter GetParameter(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OdbcParameterCollection.ItemType);
			}
			return this.InnerList[num];
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x0024CC74 File Offset: 0x0024C074
		private static int IndexOf(IEnumerable items, string parameterName)
		{
			if (items != null)
			{
				int num = 0;
				foreach (object obj in items)
				{
					OdbcParameter odbcParameter = (OdbcParameter)obj;
					if (ADP.SrcCompare(parameterName, odbcParameter.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				num = 0;
				foreach (object obj2 in items)
				{
					OdbcParameter odbcParameter2 = (OdbcParameter)obj2;
					if (ADP.DstCompare(parameterName, odbcParameter2.ParameterName) == 0)
					{
						return num;
					}
					num++;
				}
				return -1;
			}
			return -1;
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x0024CD58 File Offset: 0x0024C158
		public override int IndexOf(string parameterName)
		{
			return OdbcParameterCollection.IndexOf(this.InnerList, parameterName);
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x0024CD74 File Offset: 0x0024C174
		public override int IndexOf(object value)
		{
			if (value != null)
			{
				this.ValidateType(value);
				List<OdbcParameter> innerList = this.InnerList;
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

		// Token: 0x06001C50 RID: 7248 RVA: 0x0024CDB8 File Offset: 0x0024C1B8
		public override void Insert(int index, object value)
		{
			this.OnChange();
			this.ValidateType(value);
			this.Validate(-1, (OdbcParameter)value);
			this.InnerList.Insert(index, (OdbcParameter)value);
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x0024CDF4 File Offset: 0x0024C1F4
		private void RangeCheck(int index)
		{
			if (index < 0 || this.Count <= index)
			{
				throw ADP.ParametersMappingIndex(index, this);
			}
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x0024CE18 File Offset: 0x0024C218
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
			if (this != ((OdbcParameter)value).CompareExchangeParent(null, this))
			{
				throw ADP.CollectionRemoveInvalidObject(OdbcParameterCollection.ItemType, this);
			}
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x0024CE64 File Offset: 0x0024C264
		public override void RemoveAt(int index)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.RemoveIndex(index);
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x0024CE88 File Offset: 0x0024C288
		public override void RemoveAt(string parameterName)
		{
			this.OnChange();
			int num = this.CheckName(parameterName);
			this.RemoveIndex(num);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x0024CEAC File Offset: 0x0024C2AC
		private void RemoveIndex(int index)
		{
			List<OdbcParameter> innerList = this.InnerList;
			OdbcParameter odbcParameter = innerList[index];
			innerList.RemoveAt(index);
			odbcParameter.ResetParent();
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x0024CED8 File Offset: 0x0024C2D8
		private void Replace(int index, object newValue)
		{
			List<OdbcParameter> innerList = this.InnerList;
			this.ValidateType(newValue);
			this.Validate(index, newValue);
			OdbcParameter odbcParameter = innerList[index];
			innerList[index] = (OdbcParameter)newValue;
			odbcParameter.ResetParent();
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x0024CF18 File Offset: 0x0024C318
		protected override void SetParameter(int index, DbParameter value)
		{
			this.OnChange();
			this.RangeCheck(index);
			this.Replace(index, value);
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x0024CF3C File Offset: 0x0024C33C
		protected override void SetParameter(string parameterName, DbParameter value)
		{
			this.OnChange();
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw ADP.ParametersSourceIndex(parameterName, this, OdbcParameterCollection.ItemType);
			}
			this.Replace(num, value);
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x0024CF70 File Offset: 0x0024C370
		private void Validate(int index, object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, OdbcParameterCollection.ItemType);
			}
			object obj = ((OdbcParameter)value).CompareExchangeParent(this, null);
			if (obj != null)
			{
				if (this != obj)
				{
					throw ADP.ParametersIsNotParent(OdbcParameterCollection.ItemType, this);
				}
				if (index != this.IndexOf(value))
				{
					throw ADP.ParametersIsParent(OdbcParameterCollection.ItemType, this);
				}
			}
			string text = ((OdbcParameter)value).ParameterName;
			if (text.Length == 0)
			{
				index = 1;
				do
				{
					text = "Parameter" + index.ToString(CultureInfo.CurrentCulture);
					index++;
				}
				while (-1 != this.IndexOf(text));
				((OdbcParameter)value).ParameterName = text;
			}
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x0024D014 File Offset: 0x0024C414
		private void ValidateType(object value)
		{
			if (value == null)
			{
				throw ADP.ParameterNull("value", this, OdbcParameterCollection.ItemType);
			}
			if (!OdbcParameterCollection.ItemType.IsInstanceOfType(value))
			{
				throw ADP.InvalidParameterType(this, OdbcParameterCollection.ItemType, value);
			}
		}

		// Token: 0x0400105A RID: 4186
		private bool _rebindCollection;

		// Token: 0x0400105B RID: 4187
		private static Type ItemType = typeof(OdbcParameter);

		// Token: 0x0400105C RID: 4188
		private List<OdbcParameter> _items;
	}
}
