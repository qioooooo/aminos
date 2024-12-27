using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000714 RID: 1812
	public class SettingsPropertyCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x06003769 RID: 14185 RVA: 0x000EB1B8 File Offset: 0x000EA1B8
		public SettingsPropertyCollection()
		{
			this._Hashtable = new Hashtable(10, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000EB1D8 File Offset: 0x000EA1D8
		public void Add(SettingsProperty property)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this.OnAdd(property);
			this._Hashtable.Add(property.Name, property);
			try
			{
				this.OnAddComplete(property);
			}
			catch
			{
				this._Hashtable.Remove(property.Name);
				throw;
			}
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000EB23C File Offset: 0x000EA23C
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			SettingsProperty settingsProperty = (SettingsProperty)this._Hashtable[name];
			if (settingsProperty == null)
			{
				return;
			}
			this.OnRemove(settingsProperty);
			this._Hashtable.Remove(name);
			try
			{
				this.OnRemoveComplete(settingsProperty);
			}
			catch
			{
				this._Hashtable.Add(name, settingsProperty);
				throw;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		public SettingsProperty this[string name]
		{
			get
			{
				return this._Hashtable[name] as SettingsProperty;
			}
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000EB2BF File Offset: 0x000EA2BF
		public IEnumerator GetEnumerator()
		{
			return this._Hashtable.Values.GetEnumerator();
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000EB2D1 File Offset: 0x000EA2D1
		public object Clone()
		{
			return new SettingsPropertyCollection(this._Hashtable);
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x000EB2DE File Offset: 0x000EA2DE
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x000EB2F0 File Offset: 0x000EA2F0
		public void Clear()
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this.OnClear();
			this._Hashtable.Clear();
			this.OnClearComplete();
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000EB317 File Offset: 0x000EA317
		protected virtual void OnAdd(SettingsProperty property)
		{
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000EB319 File Offset: 0x000EA319
		protected virtual void OnAddComplete(SettingsProperty property)
		{
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000EB31B File Offset: 0x000EA31B
		protected virtual void OnClear()
		{
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x000EB31D File Offset: 0x000EA31D
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000EB31F File Offset: 0x000EA31F
		protected virtual void OnRemove(SettingsProperty property)
		{
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x000EB321 File Offset: 0x000EA321
		protected virtual void OnRemoveComplete(SettingsProperty property)
		{
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06003777 RID: 14199 RVA: 0x000EB323 File Offset: 0x000EA323
		public int Count
		{
			get
			{
				return this._Hashtable.Count;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x000EB330 File Offset: 0x000EA330
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06003779 RID: 14201 RVA: 0x000EB333 File Offset: 0x000EA333
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x000EB336 File Offset: 0x000EA336
		public void CopyTo(Array array, int index)
		{
			this._Hashtable.Values.CopyTo(array, index);
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x000EB34A File Offset: 0x000EA34A
		private SettingsPropertyCollection(Hashtable h)
		{
			this._Hashtable = (Hashtable)h.Clone();
		}

		// Token: 0x040031C8 RID: 12744
		private Hashtable _Hashtable;

		// Token: 0x040031C9 RID: 12745
		private bool _ReadOnly;
	}
}
