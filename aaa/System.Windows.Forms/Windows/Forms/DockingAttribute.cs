using System;

namespace System.Windows.Forms
{
	// Token: 0x020003B5 RID: 949
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DockingAttribute : Attribute
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x000D318B File Offset: 0x000D218B
		public DockingAttribute()
		{
			this.dockingBehavior = DockingBehavior.Never;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000D319A File Offset: 0x000D219A
		public DockingAttribute(DockingBehavior dockingBehavior)
		{
			this.dockingBehavior = dockingBehavior;
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060039D3 RID: 14803 RVA: 0x000D31A9 File Offset: 0x000D21A9
		public DockingBehavior DockingBehavior
		{
			get
			{
				return this.dockingBehavior;
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x000D31B4 File Offset: 0x000D21B4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DockingAttribute dockingAttribute = obj as DockingAttribute;
			return dockingAttribute != null && dockingAttribute.DockingBehavior == this.dockingBehavior;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x000D31E1 File Offset: 0x000D21E1
		public override int GetHashCode()
		{
			return this.dockingBehavior.GetHashCode();
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000D31F3 File Offset: 0x000D21F3
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DockingAttribute.Default);
		}

		// Token: 0x04001CEB RID: 7403
		private DockingBehavior dockingBehavior;

		// Token: 0x04001CEC RID: 7404
		public static readonly DockingAttribute Default = new DockingAttribute();
	}
}
