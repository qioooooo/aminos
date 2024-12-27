using System;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000AF RID: 175
	internal class InvalidContentTypeException : Exception
	{
		// Token: 0x060004A3 RID: 1187 RVA: 0x000174F9 File Offset: 0x000164F9
		internal InvalidContentTypeException(string message, string contentType)
			: base(message)
		{
			this.contentType = contentType;
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x00017509 File Offset: 0x00016509
		internal string ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x040003D1 RID: 977
		private string contentType;
	}
}
