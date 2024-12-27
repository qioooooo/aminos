using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000078 RID: 120
	public class ActiveDirectorySchemaPropertyCollection : CollectionBase
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0000DDB4 File Offset: 0x0000CDB4
		internal ActiveDirectorySchemaPropertyCollection(DirectoryContext context, ActiveDirectorySchemaClass schemaClass, bool isBound, string propertyName, ICollection propertyNames, bool onlyNames)
		{
			this.schemaClass = schemaClass;
			this.propertyName = propertyName;
			this.isBound = isBound;
			this.context = context;
			foreach (object obj in propertyNames)
			{
				string text = (string)obj;
				base.InnerList.Add(new ActiveDirectorySchemaProperty(context, text, null, null));
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000DE3C File Offset: 0x0000CE3C
		internal ActiveDirectorySchemaPropertyCollection(DirectoryContext context, ActiveDirectorySchemaClass schemaClass, bool isBound, string propertyName, ICollection properties)
		{
			this.schemaClass = schemaClass;
			this.propertyName = propertyName;
			this.isBound = isBound;
			this.context = context;
			foreach (object obj in properties)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)obj;
				base.InnerList.Add(activeDirectorySchemaProperty);
			}
		}

		// Token: 0x170000C9 RID: 201
		public ActiveDirectorySchemaProperty this[int index]
		{
			get
			{
				return (ActiveDirectorySchemaProperty)base.List[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.isBound)
				{
					throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { value.Name }));
				}
				if (!this.Contains(value))
				{
					base.List[index] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { value }), "value");
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000DF4C File Offset: 0x0000CF4C
		public int Add(ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			if (!schemaProperty.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaProperty.Name }));
			}
			if (!this.Contains(schemaProperty))
			{
				return base.List.Add(schemaProperty);
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { schemaProperty }), "schemaProperty");
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000DFC8 File Offset: 0x0000CFC8
		public void AddRange(ActiveDirectorySchemaProperty[] properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i] == null)
				{
					throw new ArgumentException("properties");
				}
			}
			for (int j = 0; j < properties.Length; j++)
			{
				this.Add(properties[j]);
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000E020 File Offset: 0x0000D020
		public void AddRange(ActiveDirectorySchemaPropertyCollection properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			using (IEnumerator enumerator = properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((ActiveDirectorySchemaProperty)enumerator.Current == null)
					{
						throw new ArgumentException("properties");
					}
				}
			}
			int count = properties.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(properties[i]);
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000E0B0 File Offset: 0x0000D0B0
		public void AddRange(ReadOnlyActiveDirectorySchemaPropertyCollection properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}
			using (IEnumerator enumerator = properties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((ActiveDirectorySchemaProperty)enumerator.Current == null)
					{
						throw new ArgumentException("properties");
					}
				}
			}
			int count = properties.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(properties[i]);
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000E140 File Offset: 0x0000D140
		public void Remove(ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			if (!schemaProperty.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaProperty.Name }));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaProperty.Name, schemaProperty.Name) == 0)
				{
					base.List.Remove(activeDirectorySchemaProperty);
					return;
				}
			}
			throw new ArgumentException(Res.GetString("NotFoundInCollection", new object[] { schemaProperty }), "schemaProperty");
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E1EC File Offset: 0x0000D1EC
		public void Insert(int index, ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			if (!schemaProperty.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaProperty.Name }));
			}
			if (!this.Contains(schemaProperty))
			{
				base.List.Insert(index, schemaProperty);
				return;
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { schemaProperty }), "schemaProperty");
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E268 File Offset: 0x0000D268
		public bool Contains(ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			if (!schemaProperty.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaProperty.Name }));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaProperty.Name, schemaProperty.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E2EC File Offset: 0x0000D2EC
		internal bool Contains(string propertyName)
		{
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaProperty.Name, propertyName) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E332 File Offset: 0x0000D332
		public void CopyTo(ActiveDirectorySchemaProperty[] properties, int index)
		{
			base.List.CopyTo(properties, index);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E344 File Offset: 0x0000D344
		public int IndexOf(ActiveDirectorySchemaProperty schemaProperty)
		{
			if (schemaProperty == null)
			{
				throw new ArgumentNullException("schemaProperty");
			}
			if (!schemaProperty.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaProperty.Name }));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaProperty activeDirectorySchemaProperty = (ActiveDirectorySchemaProperty)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaProperty.Name, schemaProperty.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E3C8 File Offset: 0x0000D3C8
		protected override void OnClearComplete()
		{
			if (this.isBound)
			{
				if (this.classEntry == null)
				{
					this.classEntry = this.schemaClass.GetSchemaClassDirectoryEntry();
				}
				try
				{
					if (this.classEntry.Properties.Contains(this.propertyName))
					{
						this.classEntry.Properties[this.propertyName].Clear();
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E44C File Offset: 0x0000D44C
		protected override void OnInsertComplete(int index, object value)
		{
			if (this.isBound)
			{
				if (this.classEntry == null)
				{
					this.classEntry = this.schemaClass.GetSchemaClassDirectoryEntry();
				}
				try
				{
					this.classEntry.Properties[this.propertyName].Add(((ActiveDirectorySchemaProperty)value).Name);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E4C4 File Offset: 0x0000D4C4
		protected override void OnRemoveComplete(int index, object value)
		{
			if (this.isBound)
			{
				if (this.classEntry == null)
				{
					this.classEntry = this.schemaClass.GetSchemaClassDirectoryEntry();
				}
				string name = ((ActiveDirectorySchemaProperty)value).Name;
				try
				{
					if (!this.classEntry.Properties[this.propertyName].Contains(name))
					{
						throw new ActiveDirectoryOperationException(Res.GetString("ValueCannotBeModified"));
					}
					this.classEntry.Properties[this.propertyName].Remove(name);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E570 File Offset: 0x0000D570
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			if (this.isBound)
			{
				this.OnRemoveComplete(index, oldValue);
				this.OnInsertComplete(index, newValue);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E58C File Offset: 0x0000D58C
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is ActiveDirectorySchemaProperty))
			{
				throw new ArgumentException("value");
			}
			if (!((ActiveDirectorySchemaProperty)value).isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { ((ActiveDirectorySchemaProperty)value).Name }));
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E5F0 File Offset: 0x0000D5F0
		internal string[] GetMultiValuedProperty()
		{
			string[] array = new string[base.InnerList.Count];
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				array[i] = ((ActiveDirectorySchemaProperty)base.InnerList[i]).Name;
			}
			return array;
		}

		// Token: 0x04000324 RID: 804
		private DirectoryEntry classEntry;

		// Token: 0x04000325 RID: 805
		private string propertyName;

		// Token: 0x04000326 RID: 806
		private ActiveDirectorySchemaClass schemaClass;

		// Token: 0x04000327 RID: 807
		private bool isBound;

		// Token: 0x04000328 RID: 808
		private DirectoryContext context;
	}
}
