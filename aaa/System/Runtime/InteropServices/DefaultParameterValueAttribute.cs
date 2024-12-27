using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000216 RID: 534
	[AttributeUsage(AttributeTargets.Parameter)]
	public sealed class DefaultParameterValueAttribute : Attribute
	{
		// Token: 0x06001210 RID: 4624 RVA: 0x0003CF40 File Offset: 0x0003BF40
		public DefaultParameterValueAttribute(object value)
		{
			this.value = value;
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x0003CF4F File Offset: 0x0003BF4F
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x04001082 RID: 4226
		private object value;
	}
}
