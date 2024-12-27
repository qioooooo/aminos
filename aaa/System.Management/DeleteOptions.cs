using System;

namespace System.Management
{
	// Token: 0x02000032 RID: 50
	public class DeleteOptions : ManagementOptions
	{
		// Token: 0x06000187 RID: 391 RVA: 0x000089FF File Offset: 0x000079FF
		public DeleteOptions()
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008A07 File Offset: 0x00007A07
		public DeleteOptions(ManagementNamedValueCollection context, TimeSpan timeout)
			: base(context, timeout)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008A14 File Offset: 0x00007A14
		public override object Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = null;
			if (base.Context != null)
			{
				managementNamedValueCollection = base.Context.Clone();
			}
			return new DeleteOptions(managementNamedValueCollection, base.Timeout);
		}
	}
}
