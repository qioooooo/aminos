using System;
using System.Collections;

namespace System.Xml.Serialization.Advanced
{
	// Token: 0x02000348 RID: 840
	public class SchemaImporterExtensionCollection : CollectionBase
	{
		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x060028D3 RID: 10451 RVA: 0x000D1EE4 File Offset: 0x000D0EE4
		internal Hashtable Names
		{
			get
			{
				if (this.exNames == null)
				{
					this.exNames = new Hashtable();
				}
				return this.exNames;
			}
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000D1EFF File Offset: 0x000D0EFF
		public int Add(SchemaImporterExtension extension)
		{
			return this.Add(extension.GetType().FullName, extension);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000D1F14 File Offset: 0x000D0F14
		public int Add(string name, Type type)
		{
			if (type.IsSubclassOf(typeof(SchemaImporterExtension)))
			{
				return this.Add(name, (SchemaImporterExtension)Activator.CreateInstance(type));
			}
			throw new ArgumentException(Res.GetString("XmlInvalidSchemaExtension", new object[] { type }));
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000D1F61 File Offset: 0x000D0F61
		public void Remove(string name)
		{
			if (this.Names[name] != null)
			{
				base.List.Remove(this.Names[name]);
				this.Names[name] = null;
			}
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000D1F95 File Offset: 0x000D0F95
		public new void Clear()
		{
			this.Names.Clear();
			base.List.Clear();
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000D1FB0 File Offset: 0x000D0FB0
		internal SchemaImporterExtensionCollection Clone()
		{
			SchemaImporterExtensionCollection schemaImporterExtensionCollection = new SchemaImporterExtensionCollection();
			schemaImporterExtensionCollection.exNames = (Hashtable)this.Names.Clone();
			foreach (object obj in base.List)
			{
				schemaImporterExtensionCollection.List.Add(obj);
			}
			return schemaImporterExtensionCollection;
		}

		// Token: 0x170009B4 RID: 2484
		public SchemaImporterExtension this[int index]
		{
			get
			{
				return (SchemaImporterExtension)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000D204C File Offset: 0x000D104C
		internal int Add(string name, SchemaImporterExtension extension)
		{
			if (this.Names[name] == null)
			{
				this.Names[name] = extension;
				return base.List.Add(extension);
			}
			if (this.Names[name].GetType() != extension.GetType())
			{
				throw new InvalidOperationException(Res.GetString("XmlConfigurationDuplicateExtension", new object[] { name }));
			}
			return -1;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000D20B7 File Offset: 0x000D10B7
		public void Insert(int index, SchemaImporterExtension extension)
		{
			base.List.Insert(index, extension);
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000D20C6 File Offset: 0x000D10C6
		public int IndexOf(SchemaImporterExtension extension)
		{
			return base.List.IndexOf(extension);
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000D20D4 File Offset: 0x000D10D4
		public bool Contains(SchemaImporterExtension extension)
		{
			return base.List.Contains(extension);
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000D20E2 File Offset: 0x000D10E2
		public void Remove(SchemaImporterExtension extension)
		{
			base.List.Remove(extension);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000D20F0 File Offset: 0x000D10F0
		public void CopyTo(SchemaImporterExtension[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0400169C RID: 5788
		private Hashtable exNames;
	}
}
