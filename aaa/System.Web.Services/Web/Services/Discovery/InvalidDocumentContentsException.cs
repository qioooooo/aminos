using System;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000B0 RID: 176
	internal class InvalidDocumentContentsException : Exception
	{
		// Token: 0x060004A5 RID: 1189 RVA: 0x00017511 File Offset: 0x00016511
		internal InvalidDocumentContentsException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
