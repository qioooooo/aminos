using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000238 RID: 568
	public class BaseCollection : MarshalByRefObject, ICollection, IEnumerable
	{
		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x0003402F File Offset: 0x0003302F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual int Count
		{
			get
			{
				return this.List.Count;
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0003403C File Offset: 0x0003303C
		public void CopyTo(Array ar, int index)
		{
			this.List.CopyTo(ar, index);
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0003404B File Offset: 0x0003304B
		public IEnumerator GetEnumerator()
		{
			return this.List.GetEnumerator();
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001B07 RID: 6919 RVA: 0x00034058 File Offset: 0x00033058
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x0003405B File Offset: 0x0003305B
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001B09 RID: 6921 RVA: 0x0003405E File Offset: 0x0003305E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x00034061 File Offset: 0x00033061
		protected virtual ArrayList List
		{
			get
			{
				return null;
			}
		}
	}
}
