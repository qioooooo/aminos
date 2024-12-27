using System;

namespace System.Diagnostics
{
	// Token: 0x020001D3 RID: 467
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SwitchLevelAttribute : Attribute
	{
		// Token: 0x06000E82 RID: 3714 RVA: 0x0002E00D File Offset: 0x0002D00D
		public SwitchLevelAttribute(Type switchLevelType)
		{
			this.SwitchLevelType = switchLevelType;
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0002E01C File Offset: 0x0002D01C
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x0002E024 File Offset: 0x0002D024
		public Type SwitchLevelType
		{
			get
			{
				return this.type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.type = value;
			}
		}

		// Token: 0x04000F0B RID: 3851
		private Type type;
	}
}
