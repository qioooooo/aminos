using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A6 RID: 678
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DebuggerBrowsableAttribute : Attribute
	{
		// Token: 0x06001AD0 RID: 6864 RVA: 0x00046B1F File Offset: 0x00045B1F
		public DebuggerBrowsableAttribute(DebuggerBrowsableState state)
		{
			if (state < DebuggerBrowsableState.Never || state > DebuggerBrowsableState.RootHidden)
			{
				throw new ArgumentOutOfRangeException("state");
			}
			this.state = state;
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x00046B41 File Offset: 0x00045B41
		public DebuggerBrowsableState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x04000A0D RID: 2573
		private DebuggerBrowsableState state;
	}
}
