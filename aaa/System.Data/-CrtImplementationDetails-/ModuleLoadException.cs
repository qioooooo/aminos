using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	internal class ModuleLoadException : Exception
	{
		// Token: 0x0600008B RID: 139 RVA: 0x001C5534 File Offset: 0x001C4934
		protected ModuleLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600008C RID: 140 RVA: 0x001C551C File Offset: 0x001C491C
		public ModuleLoadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x001C5508 File Offset: 0x001C4908
		public ModuleLoadException(string message)
			: base(message)
		{
		}

		// Token: 0x0400006B RID: 107
		public const string Nested = "A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n";
	}
}
