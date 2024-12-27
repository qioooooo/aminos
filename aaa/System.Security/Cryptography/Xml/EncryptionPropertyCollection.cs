using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000BD RID: 189
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class EncryptionPropertyCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x00016C69 File Offset: 0x00015C69
		public EncryptionPropertyCollection()
		{
			this.m_props = new ArrayList();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00016C7C File Offset: 0x00015C7C
		public IEnumerator GetEnumerator()
		{
			return this.m_props.GetEnumerator();
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00016C89 File Offset: 0x00015C89
		public int Count
		{
			get
			{
				return this.m_props.Count;
			}
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00016C96 File Offset: 0x00015C96
		int IList.Add(object value)
		{
			if (!(value is EncryptionProperty))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			return this.m_props.Add(value);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00016CC1 File Offset: 0x00015CC1
		public int Add(EncryptionProperty value)
		{
			return this.m_props.Add(value);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00016CCF File Offset: 0x00015CCF
		public void Clear()
		{
			this.m_props.Clear();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00016CDC File Offset: 0x00015CDC
		bool IList.Contains(object value)
		{
			if (!(value is EncryptionProperty))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			return this.m_props.Contains(value);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00016D07 File Offset: 0x00015D07
		public bool Contains(EncryptionProperty value)
		{
			return this.m_props.Contains(value);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00016D15 File Offset: 0x00015D15
		int IList.IndexOf(object value)
		{
			if (!(value is EncryptionProperty))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			return this.m_props.IndexOf(value);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00016D40 File Offset: 0x00015D40
		public int IndexOf(EncryptionProperty value)
		{
			return this.m_props.IndexOf(value);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00016D4E File Offset: 0x00015D4E
		void IList.Insert(int index, object value)
		{
			if (!(value is EncryptionProperty))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			this.m_props.Insert(index, value);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00016D7A File Offset: 0x00015D7A
		public void Insert(int index, EncryptionProperty value)
		{
			this.m_props.Insert(index, value);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00016D89 File Offset: 0x00015D89
		void IList.Remove(object value)
		{
			if (!(value is EncryptionProperty))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
			}
			this.m_props.Remove(value);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00016DB4 File Offset: 0x00015DB4
		public void Remove(EncryptionProperty value)
		{
			this.m_props.Remove(value);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00016DC2 File Offset: 0x00015DC2
		public void RemoveAt(int index)
		{
			this.m_props.RemoveAt(index);
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x00016DD0 File Offset: 0x00015DD0
		public bool IsFixedSize
		{
			get
			{
				return this.m_props.IsFixedSize;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x00016DDD File Offset: 0x00015DDD
		public bool IsReadOnly
		{
			get
			{
				return this.m_props.IsReadOnly;
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00016DEA File Offset: 0x00015DEA
		public EncryptionProperty Item(int index)
		{
			return (EncryptionProperty)this.m_props[index];
		}

		// Token: 0x170000EB RID: 235
		[IndexerName("ItemOf")]
		public EncryptionProperty this[int index]
		{
			get
			{
				return (EncryptionProperty)((IList)this)[index];
			}
			set
			{
				((IList)this)[index] = value;
			}
		}

		// Token: 0x170000EC RID: 236
		object IList.this[int index]
		{
			get
			{
				return this.m_props[index];
			}
			set
			{
				if (!(value is EncryptionProperty))
				{
					throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "value");
				}
				this.m_props[index] = value;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00016E4F File Offset: 0x00015E4F
		public void CopyTo(Array array, int index)
		{
			this.m_props.CopyTo(array, index);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00016E5E File Offset: 0x00015E5E
		public void CopyTo(EncryptionProperty[] array, int index)
		{
			this.m_props.CopyTo(array, index);
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x00016E6D File Offset: 0x00015E6D
		public object SyncRoot
		{
			get
			{
				return this.m_props.SyncRoot;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x00016E7A File Offset: 0x00015E7A
		public bool IsSynchronized
		{
			get
			{
				return this.m_props.IsSynchronized;
			}
		}

		// Token: 0x040005A1 RID: 1441
		private ArrayList m_props;
	}
}
