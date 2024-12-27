using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000075 RID: 117
	public class ActiveDirectorySchemaClassCollection : CollectionBase
	{
		// Token: 0x060002CC RID: 716 RVA: 0x0000BA7C File Offset: 0x0000AA7C
		internal ActiveDirectorySchemaClassCollection(DirectoryContext context, ActiveDirectorySchemaClass schemaClass, bool isBound, string propertyName, ICollection classNames, bool onlyNames)
		{
			this.schemaClass = schemaClass;
			this.propertyName = propertyName;
			this.isBound = isBound;
			this.context = context;
			foreach (object obj in classNames)
			{
				string text = (string)obj;
				base.InnerList.Add(new ActiveDirectorySchemaClass(context, text, null, null));
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000BB04 File Offset: 0x0000AB04
		internal ActiveDirectorySchemaClassCollection(DirectoryContext context, ActiveDirectorySchemaClass schemaClass, bool isBound, string propertyName, ICollection classes)
		{
			this.schemaClass = schemaClass;
			this.propertyName = propertyName;
			this.isBound = isBound;
			this.context = context;
			foreach (object obj in classes)
			{
				ActiveDirectorySchemaClass activeDirectorySchemaClass = (ActiveDirectorySchemaClass)obj;
				base.InnerList.Add(activeDirectorySchemaClass);
			}
		}

		// Token: 0x170000B6 RID: 182
		public ActiveDirectorySchemaClass this[int index]
		{
			get
			{
				return (ActiveDirectorySchemaClass)base.List[index];
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

		// Token: 0x060002D0 RID: 720 RVA: 0x0000BC14 File Offset: 0x0000AC14
		public int Add(ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			if (!schemaClass.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaClass.Name }));
			}
			if (!this.Contains(schemaClass))
			{
				return base.List.Add(schemaClass);
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { schemaClass }), "schemaClass");
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000BC90 File Offset: 0x0000AC90
		public void AddRange(ActiveDirectorySchemaClass[] schemaClasses)
		{
			if (schemaClasses == null)
			{
				throw new ArgumentNullException("schemaClasses");
			}
			for (int i = 0; i < schemaClasses.Length; i++)
			{
				if (schemaClasses[i] == null)
				{
					throw new ArgumentException("schemaClasses");
				}
			}
			for (int j = 0; j < schemaClasses.Length; j++)
			{
				this.Add(schemaClasses[j]);
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000BCE8 File Offset: 0x0000ACE8
		public void AddRange(ActiveDirectorySchemaClassCollection schemaClasses)
		{
			if (schemaClasses == null)
			{
				throw new ArgumentNullException("schemaClasses");
			}
			using (IEnumerator enumerator = schemaClasses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((ActiveDirectorySchemaClass)enumerator.Current == null)
					{
						throw new ArgumentException("schemaClasses");
					}
				}
			}
			int count = schemaClasses.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(schemaClasses[i]);
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000BD78 File Offset: 0x0000AD78
		public void AddRange(ReadOnlyActiveDirectorySchemaClassCollection schemaClasses)
		{
			if (schemaClasses == null)
			{
				throw new ArgumentNullException("schemaClasses");
			}
			using (IEnumerator enumerator = schemaClasses.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if ((ActiveDirectorySchemaClass)enumerator.Current == null)
					{
						throw new ArgumentException("schemaClasses");
					}
				}
			}
			int count = schemaClasses.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(schemaClasses[i]);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000BE08 File Offset: 0x0000AE08
		public void Remove(ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			if (!schemaClass.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaClass.Name }));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaClass activeDirectorySchemaClass = (ActiveDirectorySchemaClass)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaClass.Name, schemaClass.Name) == 0)
				{
					base.List.Remove(activeDirectorySchemaClass);
					return;
				}
			}
			throw new ArgumentException(Res.GetString("NotFoundInCollection", new object[] { schemaClass }), "schemaClass");
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000BEB4 File Offset: 0x0000AEB4
		public void Insert(int index, ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			if (!schemaClass.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaClass.Name }));
			}
			if (!this.Contains(schemaClass))
			{
				base.List.Insert(index, schemaClass);
				return;
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { schemaClass }), "schemaClass");
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000BF30 File Offset: 0x0000AF30
		public bool Contains(ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			if (!schemaClass.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaClass.Name }));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaClass activeDirectorySchemaClass = (ActiveDirectorySchemaClass)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaClass.Name, schemaClass.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000BFB2 File Offset: 0x0000AFB2
		public void CopyTo(ActiveDirectorySchemaClass[] schemaClasses, int index)
		{
			base.List.CopyTo(schemaClasses, index);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000BFC4 File Offset: 0x0000AFC4
		public int IndexOf(ActiveDirectorySchemaClass schemaClass)
		{
			if (schemaClass == null)
			{
				throw new ArgumentNullException("schemaClass");
			}
			if (!schemaClass.isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { schemaClass.Name }));
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySchemaClass activeDirectorySchemaClass = (ActiveDirectorySchemaClass)base.InnerList[i];
				if (Utils.Compare(activeDirectorySchemaClass.Name, schemaClass.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000C048 File Offset: 0x0000B048
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

		// Token: 0x060002DA RID: 730 RVA: 0x0000C0CC File Offset: 0x0000B0CC
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
					this.classEntry.Properties[this.propertyName].Add(((ActiveDirectorySchemaClass)value).Name);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000C144 File Offset: 0x0000B144
		protected override void OnRemoveComplete(int index, object value)
		{
			if (this.isBound)
			{
				if (this.classEntry == null)
				{
					this.classEntry = this.schemaClass.GetSchemaClassDirectoryEntry();
				}
				string name = ((ActiveDirectorySchemaClass)value).Name;
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

		// Token: 0x060002DC RID: 732 RVA: 0x0000C1F0 File Offset: 0x0000B1F0
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			if (this.isBound)
			{
				this.OnRemoveComplete(index, oldValue);
				this.OnInsertComplete(index, newValue);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000C20C File Offset: 0x0000B20C
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is ActiveDirectorySchemaClass))
			{
				throw new ArgumentException("value");
			}
			if (!((ActiveDirectorySchemaClass)value).isBound)
			{
				throw new InvalidOperationException(Res.GetString("SchemaObjectNotCommitted", new object[] { ((ActiveDirectorySchemaClass)value).Name }));
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000C270 File Offset: 0x0000B270
		internal string[] GetMultiValuedProperty()
		{
			string[] array = new string[base.InnerList.Count];
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				array[i] = ((ActiveDirectorySchemaClass)base.InnerList[i]).Name;
			}
			return array;
		}

		// Token: 0x040002ED RID: 749
		private DirectoryEntry classEntry;

		// Token: 0x040002EE RID: 750
		private string propertyName;

		// Token: 0x040002EF RID: 751
		private ActiveDirectorySchemaClass schemaClass;

		// Token: 0x040002F0 RID: 752
		private bool isBound;

		// Token: 0x040002F1 RID: 753
		private DirectoryContext context;
	}
}
