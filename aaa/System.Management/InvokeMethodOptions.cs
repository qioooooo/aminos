using System;

namespace System.Management
{
	// Token: 0x02000033 RID: 51
	public class InvokeMethodOptions : ManagementOptions
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00008A43 File Offset: 0x00007A43
		public InvokeMethodOptions()
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008A4B File Offset: 0x00007A4B
		public InvokeMethodOptions(ManagementNamedValueCollection context, TimeSpan timeout)
			: base(context, timeout)
		{
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008A58 File Offset: 0x00007A58
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new InvokeMethodOptions(managementNamedValueCollection, base.Timeout);
		}
	}
}
