using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x020000AC RID: 172
	[Serializable]
	internal class ModuleLoadException : Exception
	{
		// Token: 0x06000113 RID: 275 RVA: 0x00006184 File Offset: 0x00005584
		protected ModuleLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000616C File Offset: 0x0000556C
		public ModuleLoadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00006158 File Offset: 0x00005558
		public ModuleLoadException(string message)
			: base(message)
		{
		}

		// Token: 0x040000BD RID: 189
		public const string Nested = "A nested exception occurred after the primary exception that caused the C++ module to fail to load.\n";
	}
}
