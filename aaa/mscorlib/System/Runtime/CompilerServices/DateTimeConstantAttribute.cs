using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005CB RID: 1483
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DateTimeConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x0600378B RID: 14219 RVA: 0x000BB7D0 File Offset: 0x000BA7D0
		public DateTimeConstantAttribute(long ticks)
		{
			this.date = new DateTime(ticks);
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x0600378C RID: 14220 RVA: 0x000BB7E4 File Offset: 0x000BA7E4
		public override object Value
		{
			get
			{
				return this.date;
			}
		}

		// Token: 0x04001C96 RID: 7318
		private DateTime date;
	}
}
