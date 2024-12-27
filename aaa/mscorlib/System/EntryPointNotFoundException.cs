using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000AD RID: 173
	[ComVisible(true)]
	[Serializable]
	public class EntryPointNotFoundException : TypeLoadException
	{
		// Token: 0x06000A60 RID: 2656 RVA: 0x0001FB91 File Offset: 0x0001EB91
		public EntryPointNotFoundException()
			: base(Environment.GetResourceString("Arg_EntryPointNotFoundException"))
		{
			base.SetErrorCode(-2146233053);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x0001FBAE File Offset: 0x0001EBAE
		public EntryPointNotFoundException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233053);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x0001FBC2 File Offset: 0x0001EBC2
		public EntryPointNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233053);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x0001FBD7 File Offset: 0x0001EBD7
		protected EntryPointNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
