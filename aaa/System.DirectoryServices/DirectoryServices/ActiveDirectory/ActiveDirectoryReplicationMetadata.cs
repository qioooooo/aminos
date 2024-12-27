using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200006D RID: 109
	public class ActiveDirectoryReplicationMetadata : DictionaryBase
	{
		// Token: 0x06000278 RID: 632 RVA: 0x0000894C File Offset: 0x0000794C
		internal ActiveDirectoryReplicationMetadata(DirectoryServer server)
		{
			this.server = server;
			Hashtable hashtable = new Hashtable();
			this.nameTable = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x170000A3 RID: 163
		public AttributeMetadata this[string name]
		{
			get
			{
				string text = name.ToLower(CultureInfo.InvariantCulture);
				if (this.Contains(text))
				{
					return (AttributeMetadata)base.InnerHashtable[text];
				}
				return null;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600027A RID: 634 RVA: 0x000089C5 File Offset: 0x000079C5
		public ReadOnlyStringCollection AttributeNames
		{
			get
			{
				return this.dataNameCollection;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600027B RID: 635 RVA: 0x000089CD File Offset: 0x000079CD
		public AttributeMetadataCollection Values
		{
			get
			{
				return this.dataValueCollection;
			}
		}

		// Token: 0x0600027C RID: 636 RVA: 0x000089D8 File Offset: 0x000079D8
		public bool Contains(string attributeName)
		{
			string text = attributeName.ToLower(CultureInfo.InvariantCulture);
			return base.Dictionary.Contains(text);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x000089FD File Offset: 0x000079FD
		public void CopyTo(AttributeMetadata[] array, int index)
		{
			base.Dictionary.Values.CopyTo(array, index);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00008A11 File Offset: 0x00007A11
		private void Add(string name, AttributeMetadata value)
		{
			base.Dictionary.Add(name.ToLower(CultureInfo.InvariantCulture), value);
			this.dataNameCollection.Add(name);
			this.dataValueCollection.Add(value);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00008A44 File Offset: 0x00007A44
		internal void AddHelper(int count, IntPtr info, bool advanced)
		{
			IntPtr intPtr = (IntPtr)0;
			for (int i = 0; i < count; i++)
			{
				if (advanced)
				{
					intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(int)) * 2 + i * Marshal.SizeOf(typeof(DS_REPL_ATTR_META_DATA_2)));
					AttributeMetadata attributeMetadata = new AttributeMetadata(intPtr, true, this.server, this.nameTable);
					this.Add(attributeMetadata.Name, attributeMetadata);
				}
				else
				{
					intPtr = Utils.AddToIntPtr(info, Marshal.SizeOf(typeof(int)) * 2 + i * Marshal.SizeOf(typeof(DS_REPL_ATTR_META_DATA)));
					AttributeMetadata attributeMetadata2 = new AttributeMetadata(intPtr, false, this.server, this.nameTable);
					this.Add(attributeMetadata2.Name, attributeMetadata2);
				}
			}
		}

		// Token: 0x040002A1 RID: 673
		private DirectoryServer server;

		// Token: 0x040002A2 RID: 674
		private Hashtable nameTable;

		// Token: 0x040002A3 RID: 675
		private AttributeMetadataCollection dataValueCollection = new AttributeMetadataCollection();

		// Token: 0x040002A4 RID: 676
		private ReadOnlyStringCollection dataNameCollection = new ReadOnlyStringCollection();
	}
}
