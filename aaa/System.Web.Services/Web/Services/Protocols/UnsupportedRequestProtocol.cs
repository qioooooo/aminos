using System;
using System.IO;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000090 RID: 144
	internal class UnsupportedRequestProtocol : ServerProtocol
	{
		// Token: 0x060003BE RID: 958 RVA: 0x00012DC6 File Offset: 0x00011DC6
		internal UnsupportedRequestProtocol(int httpCode)
		{
			this.httpCode = httpCode;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00012DD5 File Offset: 0x00011DD5
		internal int HttpCode
		{
			get
			{
				return this.httpCode;
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00012DDD File Offset: 0x00011DDD
		internal override bool Initialize()
		{
			return true;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00012DE0 File Offset: 0x00011DE0
		internal override bool IsOneWay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00012DE3 File Offset: 0x00011DE3
		internal override LogicalMethodInfo MethodInfo
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00012DE6 File Offset: 0x00011DE6
		internal override ServerType ServerType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00012DE9 File Offset: 0x00011DE9
		internal override object[] ReadParameters()
		{
			return new object[0];
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00012DF1 File Offset: 0x00011DF1
		internal override void WriteReturns(object[] returnValues, Stream outputStream)
		{
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00012DF3 File Offset: 0x00011DF3
		internal override bool WriteException(Exception e, Stream outputStream)
		{
			return false;
		}

		// Token: 0x04000395 RID: 917
		private int httpCode;
	}
}
