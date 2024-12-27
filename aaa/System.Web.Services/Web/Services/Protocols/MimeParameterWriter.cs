using System;
using System.IO;
using System.Net;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000038 RID: 56
	public abstract class MimeParameterWriter : MimeFormatter
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000054E7 File Offset: 0x000044E7
		public virtual bool UsesWriteRequest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600013E RID: 318 RVA: 0x000054EA File Offset: 0x000044EA
		// (set) Token: 0x0600013F RID: 319 RVA: 0x000054ED File Offset: 0x000044ED
		public virtual Encoding RequestEncoding
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000054EF File Offset: 0x000044EF
		public virtual string GetRequestUrl(string url, object[] parameters)
		{
			return url;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000054F2 File Offset: 0x000044F2
		public virtual void InitializeRequest(WebRequest request, object[] values)
		{
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000054F4 File Offset: 0x000044F4
		public virtual void WriteRequest(Stream requestStream, object[] values)
		{
		}
	}
}
