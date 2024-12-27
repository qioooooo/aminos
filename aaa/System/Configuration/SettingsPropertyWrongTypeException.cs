using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x02000719 RID: 1817
	[Serializable]
	public class SettingsPropertyWrongTypeException : Exception
	{
		// Token: 0x060037A2 RID: 14242 RVA: 0x000EBCFE File Offset: 0x000EACFE
		public SettingsPropertyWrongTypeException(string message)
			: base(message)
		{
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x000EBD07 File Offset: 0x000EAD07
		public SettingsPropertyWrongTypeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060037A4 RID: 14244 RVA: 0x000EBD11 File Offset: 0x000EAD11
		protected SettingsPropertyWrongTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060037A5 RID: 14245 RVA: 0x000EBD1B File Offset: 0x000EAD1B
		public SettingsPropertyWrongTypeException()
		{
		}
	}
}
