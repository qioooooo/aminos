using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000102 RID: 258
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerActionListCollection : CollectionBase
	{
		// Token: 0x06000A93 RID: 2707 RVA: 0x000292FC File Offset: 0x000282FC
		public DesignerActionListCollection()
		{
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00029304 File Offset: 0x00028304
		internal DesignerActionListCollection(DesignerActionList actionList)
		{
			this.Add(actionList);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x00029314 File Offset: 0x00028314
		public DesignerActionListCollection(DesignerActionList[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000177 RID: 375
		public DesignerActionList this[int index]
		{
			get
			{
				return (DesignerActionList)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00029345 File Offset: 0x00028345
		public int Add(DesignerActionList value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x00029354 File Offset: 0x00028354
		public void AddRange(DesignerActionList[] value)
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

		// Token: 0x06000A9A RID: 2714 RVA: 0x00029388 File Offset: 0x00028388
		public void AddRange(DesignerActionListCollection value)
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

		// Token: 0x06000A9B RID: 2715 RVA: 0x000293C4 File Offset: 0x000283C4
		public void Insert(int index, DesignerActionList value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000293D3 File Offset: 0x000283D3
		public int IndexOf(DesignerActionList value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000293E1 File Offset: 0x000283E1
		public bool Contains(DesignerActionList value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x000293EF File Offset: 0x000283EF
		public void Remove(DesignerActionList value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x000293FD File Offset: 0x000283FD
		public void CopyTo(DesignerActionList[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x0002940C File Offset: 0x0002840C
		protected override void OnSet(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0002940E File Offset: 0x0002840E
		protected override void OnInsert(int index, object value)
		{
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00029410 File Offset: 0x00028410
		protected override void OnClear()
		{
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00029412 File Offset: 0x00028412
		protected override void OnRemove(int index, object value)
		{
		}

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00029414 File Offset: 0x00028414
		protected override void OnValidate(object value)
		{
		}
	}
}
