using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000C4 RID: 196
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class ReferenceList : IList, ICollection, IEnumerable
	{
		// Token: 0x060004B0 RID: 1200 RVA: 0x00017BD5 File Offset: 0x00016BD5
		public ReferenceList()
		{
			this.m_references = new ArrayList();
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00017BE8 File Offset: 0x00016BE8
		public IEnumerator GetEnumerator()
		{
			return this.m_references.GetEnumerator();
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00017BF5 File Offset: 0x00016BF5
		public int Count
		{
			get
			{
				return this.m_references.Count;
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00017C04 File Offset: 0x00016C04
		public int Add(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is DataReference) && !(value is KeyReference))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			return this.m_references.Add(value);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017C50 File Offset: 0x00016C50
		public void Clear()
		{
			this.m_references.Clear();
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00017C5D File Offset: 0x00016C5D
		public bool Contains(object value)
		{
			return this.m_references.Contains(value);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00017C6B File Offset: 0x00016C6B
		public int IndexOf(object value)
		{
			return this.m_references.IndexOf(value);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00017C7C File Offset: 0x00016C7C
		public void Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is DataReference) && !(value is KeyReference))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			this.m_references.Insert(index, value);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00017CC9 File Offset: 0x00016CC9
		public void Remove(object value)
		{
			this.m_references.Remove(value);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00017CD7 File Offset: 0x00016CD7
		public void RemoveAt(int index)
		{
			this.m_references.RemoveAt(index);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00017CE5 File Offset: 0x00016CE5
		public EncryptedReference Item(int index)
		{
			return (EncryptedReference)this.m_references[index];
		}

		// Token: 0x170000F8 RID: 248
		[IndexerName("ItemOf")]
		public EncryptedReference this[int index]
		{
			get
			{
				return this.Item(index);
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		// Token: 0x170000F9 RID: 249
		object IList.this[int index]
		{
			get
			{
				return this.m_references[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(value is DataReference) && !(value is KeyReference))
				{
					throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
				}
				this.m_references[index] = value;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00017D69 File Offset: 0x00016D69
		public void CopyTo(Array array, int index)
		{
			this.m_references.CopyTo(array, index);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00017D78 File Offset: 0x00016D78
		bool IList.IsFixedSize
		{
			get
			{
				return this.m_references.IsFixedSize;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00017D85 File Offset: 0x00016D85
		bool IList.IsReadOnly
		{
			get
			{
				return this.m_references.IsReadOnly;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x00017D92 File Offset: 0x00016D92
		public object SyncRoot
		{
			get
			{
				return this.m_references.SyncRoot;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00017D9F File Offset: 0x00016D9F
		public bool IsSynchronized
		{
			get
			{
				return this.m_references.IsSynchronized;
			}
		}

		// Token: 0x040005AA RID: 1450
		private ArrayList m_references;
	}
}
