using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200016F RID: 367
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DesignerVerbCollection : CollectionBase
	{
		// Token: 0x06000BE7 RID: 3047 RVA: 0x00028D01 File Offset: 0x00027D01
		public DesignerVerbCollection()
		{
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x00028D09 File Offset: 0x00027D09
		public DesignerVerbCollection(DesignerVerb[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000268 RID: 616
		public DesignerVerb this[int index]
		{
			get
			{
				return (DesignerVerb)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00028D3A File Offset: 0x00027D3A
		public int Add(DesignerVerb value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00028D48 File Offset: 0x00027D48
		public void AddRange(DesignerVerb[] value)
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

		// Token: 0x06000BED RID: 3053 RVA: 0x00028D7C File Offset: 0x00027D7C
		public void AddRange(DesignerVerbCollection value)
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

		// Token: 0x06000BEE RID: 3054 RVA: 0x00028DB8 File Offset: 0x00027DB8
		public void Insert(int index, DesignerVerb value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00028DC7 File Offset: 0x00027DC7
		public int IndexOf(DesignerVerb value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00028DD5 File Offset: 0x00027DD5
		public bool Contains(DesignerVerb value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00028DE3 File Offset: 0x00027DE3
		public void Remove(DesignerVerb value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00028DF1 File Offset: 0x00027DF1
		public void CopyTo(DesignerVerb[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00028E00 File Offset: 0x00027E00
		protected override void OnSet(int index, object oldValue, object newValue)
		{
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00028E02 File Offset: 0x00027E02
		protected override void OnInsert(int index, object value)
		{
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00028E04 File Offset: 0x00027E04
		protected override void OnClear()
		{
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00028E06 File Offset: 0x00027E06
		protected override void OnRemove(int index, object value)
		{
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00028E08 File Offset: 0x00027E08
		protected override void OnValidate(object value)
		{
		}
	}
}
