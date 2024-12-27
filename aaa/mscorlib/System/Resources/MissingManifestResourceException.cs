using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Resources
{
	// Token: 0x0200041C RID: 1052
	[ComVisible(true)]
	[Serializable]
	public class MissingManifestResourceException : SystemException
	{
		// Token: 0x06002B7D RID: 11133 RVA: 0x000925AC File Offset: 0x000915AC
		public MissingManifestResourceException()
			: base(Environment.GetResourceString("Arg_MissingManifestResourceException"))
		{
			base.SetErrorCode(-2146233038);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000925C9 File Offset: 0x000915C9
		public MissingManifestResourceException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233038);
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000925DD File Offset: 0x000915DD
		public MissingManifestResourceException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233038);
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000925F2 File Offset: 0x000915F2
		protected MissingManifestResourceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
