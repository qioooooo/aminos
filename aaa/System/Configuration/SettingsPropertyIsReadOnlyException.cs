using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x02000715 RID: 1813
	[Serializable]
	public class SettingsPropertyIsReadOnlyException : Exception
	{
		// Token: 0x0600377C RID: 14204 RVA: 0x000EB363 File Offset: 0x000EA363
		public SettingsPropertyIsReadOnlyException(string message)
			: base(message)
		{
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000EB36C File Offset: 0x000EA36C
		public SettingsPropertyIsReadOnlyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x000EB376 File Offset: 0x000EA376
		protected SettingsPropertyIsReadOnlyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x000EB380 File Offset: 0x000EA380
		public SettingsPropertyIsReadOnlyException()
		{
		}
	}
}
