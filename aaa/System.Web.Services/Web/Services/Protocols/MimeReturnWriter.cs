using System;
using System.IO;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200004D RID: 77
	internal abstract class MimeReturnWriter : MimeFormatter
	{
		// Token: 0x060001B5 RID: 437
		internal abstract void Write(HttpResponse response, Stream outputStream, object returnValue);
	}
}
