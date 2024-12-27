using System;

namespace System.ComponentModel
{
	// Token: 0x0200012F RID: 303
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ReadOnlyAttribute : Attribute
	{
		// Token: 0x060009B7 RID: 2487 RVA: 0x000201B0 File Offset: 0x0001F1B0
		public ReadOnlyAttribute(bool isReadOnly)
		{
			this.isReadOnly = isReadOnly;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000201BF File Offset: 0x0001F1BF
		public bool IsReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x000201C8 File Offset: 0x0001F1C8
		public override bool Equals(object value)
		{
			if (this == value)
			{
				return true;
			}
			ReadOnlyAttribute readOnlyAttribute = value as ReadOnlyAttribute;
			return readOnlyAttribute != null && readOnlyAttribute.IsReadOnly == this.IsReadOnly;
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000201F5 File Offset: 0x0001F1F5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000201FD File Offset: 0x0001F1FD
		public override bool IsDefaultAttribute()
		{
			return this.IsReadOnly == ReadOnlyAttribute.Default.IsReadOnly;
		}

		// Token: 0x04000A1E RID: 2590
		private bool isReadOnly;

		// Token: 0x04000A1F RID: 2591
		public static readonly ReadOnlyAttribute Yes = new ReadOnlyAttribute(true);

		// Token: 0x04000A20 RID: 2592
		public static readonly ReadOnlyAttribute No = new ReadOnlyAttribute(false);

		// Token: 0x04000A21 RID: 2593
		public static readonly ReadOnlyAttribute Default = ReadOnlyAttribute.No;
	}
}
