using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000034 RID: 52
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class PropertyValueCollection : CollectionBase
	{
		// Token: 0x06000170 RID: 368 RVA: 0x000066DC File Offset: 0x000056DC
		internal PropertyValueCollection(DirectoryEntry entry, string propertyName)
		{
			this.entry = entry;
			this.propertyName = propertyName;
			this.PopulateList();
			ArrayList arrayList = new ArrayList();
			this.changeList = ArrayList.Synchronized(arrayList);
			this.allowMultipleChange = entry.allowMultipleChange;
			string path = entry.Path;
			if (path == null || path.Length == 0)
			{
				this.needNewBehavior = true;
				return;
			}
			if (path.StartsWith("LDAP:", StringComparison.Ordinal))
			{
				this.needNewBehavior = true;
			}
		}

		// Token: 0x17000063 RID: 99
		public object this[int index]
		{
			get
			{
				return base.List[index];
			}
			set
			{
				if (this.needNewBehavior && !this.allowMultipleChange)
				{
					throw new NotSupportedException();
				}
				base.List[index] = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000678A File Offset: 0x0000578A
		[ComVisible(false)]
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00006794 File Offset: 0x00005794
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000067DC File Offset: 0x000057DC
		public object Value
		{
			get
			{
				if (base.Count == 0)
				{
					return null;
				}
				if (base.Count == 1)
				{
					return base.List[0];
				}
				object[] array = new object[base.Count];
				base.List.CopyTo(array, 0);
				return array;
			}
			set
			{
				try
				{
					base.Clear();
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147467259 || value == null)
					{
						throw;
					}
				}
				if (value == null)
				{
					return;
				}
				this.changeList.Clear();
				if (value is Array)
				{
					if (value is byte[])
					{
						this.changeList.Add(value);
					}
					else if (value is object[])
					{
						this.changeList.AddRange((object[])value);
					}
					else
					{
						object[] array = new object[((Array)value).Length];
						((Array)value).CopyTo(array, 0);
						this.changeList.AddRange(array);
					}
				}
				else
				{
					this.changeList.Add(value);
				}
				object[] array2 = new object[this.changeList.Count];
				this.changeList.CopyTo(array2, 0);
				this.entry.AdsObject.PutEx(2, this.propertyName, array2);
				this.entry.CommitIfNotCaching();
				this.PopulateList();
			}
		}

		// Token: 0x06000176 RID: 374 RVA: 0x000068E0 File Offset: 0x000058E0
		public int Add(object value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x000068F0 File Offset: 0x000058F0
		public void AddRange(object[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00006924 File Offset: 0x00005924
		public void AddRange(PropertyValueCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00006960 File Offset: 0x00005960
		public bool Contains(object value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000696E File Offset: 0x0000596E
		public void CopyTo(object[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000697D File Offset: 0x0000597D
		public int IndexOf(object value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000698B File Offset: 0x0000598B
		public void Insert(int index, object value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000699C File Offset: 0x0000599C
		private void PopulateList()
		{
			object obj;
			int ex = this.entry.AdsObject.GetEx(this.propertyName, out obj);
			if (ex != 0)
			{
				if (ex == -2147463155 || ex == -2147463162)
				{
					return;
				}
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			else
			{
				if (obj is ICollection)
				{
					base.InnerList.AddRange((ICollection)obj);
					return;
				}
				base.InnerList.Add(obj);
				return;
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006A04 File Offset: 0x00005A04
		public void Remove(object value)
		{
			if (this.needNewBehavior)
			{
				try
				{
					base.List.Remove(value);
					return;
				}
				catch (ArgumentException)
				{
					this.OnRemoveComplete(0, value);
					return;
				}
			}
			base.List.Remove(value);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00006A50 File Offset: 0x00005A50
		protected override void OnClearComplete()
		{
			if (this.needNewBehavior && !this.allowMultipleChange && this.updateType != PropertyValueCollection.UpdateType.None && this.updateType != PropertyValueCollection.UpdateType.Update)
			{
				throw new InvalidOperationException(Res.GetString("DSPropertyValueSupportOneOperation"));
			}
			this.entry.AdsObject.PutEx(1, this.propertyName, null);
			this.updateType = PropertyValueCollection.UpdateType.Update;
			try
			{
				this.entry.CommitIfNotCaching();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147016694)
				{
					throw;
				}
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006AE0 File Offset: 0x00005AE0
		protected override void OnInsertComplete(int index, object value)
		{
			if (this.needNewBehavior)
			{
				if (!this.allowMultipleChange)
				{
					if (this.updateType != PropertyValueCollection.UpdateType.None && this.updateType != PropertyValueCollection.UpdateType.Add)
					{
						throw new InvalidOperationException(Res.GetString("DSPropertyValueSupportOneOperation"));
					}
					this.changeList.Add(value);
					object[] array = new object[this.changeList.Count];
					this.changeList.CopyTo(array, 0);
					this.entry.AdsObject.PutEx(3, this.propertyName, array);
					this.updateType = PropertyValueCollection.UpdateType.Add;
				}
				else
				{
					this.entry.AdsObject.PutEx(3, this.propertyName, new object[] { value });
				}
			}
			else
			{
				object[] array2 = new object[base.InnerList.Count];
				base.InnerList.CopyTo(array2, 0);
				this.entry.AdsObject.PutEx(2, this.propertyName, array2);
			}
			this.entry.CommitIfNotCaching();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00006BD4 File Offset: 0x00005BD4
		protected override void OnRemoveComplete(int index, object value)
		{
			if (this.needNewBehavior)
			{
				if (!this.allowMultipleChange)
				{
					if (this.updateType != PropertyValueCollection.UpdateType.None && this.updateType != PropertyValueCollection.UpdateType.Delete)
					{
						throw new InvalidOperationException(Res.GetString("DSPropertyValueSupportOneOperation"));
					}
					this.changeList.Add(value);
					object[] array = new object[this.changeList.Count];
					this.changeList.CopyTo(array, 0);
					this.entry.AdsObject.PutEx(4, this.propertyName, array);
					this.updateType = PropertyValueCollection.UpdateType.Delete;
				}
				else
				{
					this.entry.AdsObject.PutEx(4, this.propertyName, new object[] { value });
				}
			}
			else
			{
				object[] array2 = new object[base.InnerList.Count];
				base.InnerList.CopyTo(array2, 0);
				this.entry.AdsObject.PutEx(2, this.propertyName, array2);
			}
			this.entry.CommitIfNotCaching();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00006CC8 File Offset: 0x00005CC8
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			if (base.Count <= 1)
			{
				this.entry.AdsObject.Put(this.propertyName, newValue);
			}
			else if (this.needNewBehavior)
			{
				this.entry.AdsObject.PutEx(4, this.propertyName, new object[] { oldValue });
				this.entry.AdsObject.PutEx(3, this.propertyName, new object[] { newValue });
			}
			else
			{
				object[] array = new object[base.InnerList.Count];
				base.InnerList.CopyTo(array, 0);
				this.entry.AdsObject.PutEx(2, this.propertyName, array);
			}
			this.entry.CommitIfNotCaching();
		}

		// Token: 0x040001CA RID: 458
		private DirectoryEntry entry;

		// Token: 0x040001CB RID: 459
		private string propertyName;

		// Token: 0x040001CC RID: 460
		private PropertyValueCollection.UpdateType updateType = PropertyValueCollection.UpdateType.None;

		// Token: 0x040001CD RID: 461
		private ArrayList changeList;

		// Token: 0x040001CE RID: 462
		private bool allowMultipleChange;

		// Token: 0x040001CF RID: 463
		private bool needNewBehavior;

		// Token: 0x02000035 RID: 53
		internal enum UpdateType
		{
			// Token: 0x040001D1 RID: 465
			Add,
			// Token: 0x040001D2 RID: 466
			Delete,
			// Token: 0x040001D3 RID: 467
			Update,
			// Token: 0x040001D4 RID: 468
			None
		}
	}
}
