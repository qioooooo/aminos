using System;

namespace System.ComponentModel
{
	// Token: 0x02000193 RID: 403
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class NotifyParentPropertyAttribute : Attribute
	{
		// Token: 0x06000CAE RID: 3246 RVA: 0x000294E1 File Offset: 0x000284E1
		public NotifyParentPropertyAttribute(bool notifyParent)
		{
			this.notifyParent = notifyParent;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000CAF RID: 3247 RVA: 0x000294F0 File Offset: 0x000284F0
		public bool NotifyParent
		{
			get
			{
				return this.notifyParent;
			}
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x000294F8 File Offset: 0x000284F8
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is NotifyParentPropertyAttribute && ((NotifyParentPropertyAttribute)obj).NotifyParent == this.notifyParent);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00029520 File Offset: 0x00028520
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00029528 File Offset: 0x00028528
		public override bool IsDefaultAttribute()
		{
			return this.Equals(NotifyParentPropertyAttribute.Default);
		}

		// Token: 0x04000AE1 RID: 2785
		public static readonly NotifyParentPropertyAttribute Yes = new NotifyParentPropertyAttribute(true);

		// Token: 0x04000AE2 RID: 2786
		public static readonly NotifyParentPropertyAttribute No = new NotifyParentPropertyAttribute(false);

		// Token: 0x04000AE3 RID: 2787
		public static readonly NotifyParentPropertyAttribute Default = NotifyParentPropertyAttribute.No;

		// Token: 0x04000AE4 RID: 2788
		private bool notifyParent;
	}
}
