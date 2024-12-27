using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x02000716 RID: 1814
	[Serializable]
	public class SettingsPropertyNotFoundException : Exception
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x000EB388 File Offset: 0x000EA388
		public SettingsPropertyNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x000EB391 File Offset: 0x000EA391
		public SettingsPropertyNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x000EB39B File Offset: 0x000EA39B
		protected SettingsPropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x000EB3A5 File Offset: 0x000EA3A5
		public SettingsPropertyNotFoundException()
		{
		}
	}
}
