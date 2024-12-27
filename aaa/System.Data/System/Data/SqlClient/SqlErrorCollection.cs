using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data.SqlClient
{
	// Token: 0x020002F3 RID: 755
	[ListBindable(false)]
	[Serializable]
	public sealed class SqlErrorCollection : ICollection, IEnumerable
	{
		// Token: 0x06002733 RID: 10035 RVA: 0x00289A6C File Offset: 0x00288E6C
		internal SqlErrorCollection()
		{
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x00289A8C File Offset: 0x00288E8C
		public void CopyTo(Array array, int index)
		{
			this.errors.CopyTo(array, index);
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x00289AA8 File Offset: 0x00288EA8
		public void CopyTo(SqlError[] array, int index)
		{
			this.errors.CopyTo(array, index);
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002736 RID: 10038 RVA: 0x00289AC4 File Offset: 0x00288EC4
		public int Count
		{
			get
			{
				return this.errors.Count;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002737 RID: 10039 RVA: 0x00289ADC File Offset: 0x00288EDC
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002738 RID: 10040 RVA: 0x00289AEC File Offset: 0x00288EEC
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700063B RID: 1595
		public SqlError this[int index]
		{
			get
			{
				return (SqlError)this.errors[index];
			}
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x00289B1C File Offset: 0x00288F1C
		public IEnumerator GetEnumerator()
		{
			return this.errors.GetEnumerator();
		}

		// Token: 0x0600273B RID: 10043 RVA: 0x00289B34 File Offset: 0x00288F34
		internal void Add(SqlError error)
		{
			this.errors.Add(error);
		}

		// Token: 0x040018E1 RID: 6369
		private ArrayList errors = new ArrayList();
	}
}
