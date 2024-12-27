using System;
using System.Design;
using System.Runtime.Serialization;

namespace System.Data.Design
{
	// Token: 0x020000A6 RID: 166
	[Serializable]
	internal class InternalException : Exception, ISerializable
	{
		// Token: 0x060007BC RID: 1980 RVA: 0x00011DEF File Offset: 0x00010DEF
		internal InternalException(string internalMessage)
			: this(internalMessage, null)
		{
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00011DF9 File Offset: 0x00010DF9
		internal InternalException(string internalMessage, Exception innerException)
			: this(innerException, internalMessage, -1, false)
		{
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00011E05 File Offset: 0x00010E05
		internal InternalException(string internalMessage, int errorCode)
			: this(null, internalMessage, errorCode, false)
		{
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00011E11 File Offset: 0x00010E11
		internal InternalException(string internalMessage, int errorCode, bool showTextOnReport)
			: this(null, internalMessage, errorCode, showTextOnReport)
		{
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00011E1D File Offset: 0x00010E1D
		internal InternalException(Exception innerException, string internalMessage, int errorCode, bool showErrorMesageOnReport)
			: this(innerException, internalMessage, errorCode, showErrorMesageOnReport, true)
		{
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00011E2B File Offset: 0x00010E2B
		internal InternalException(Exception innerException, string internalMessage, int errorCode, bool showErrorMesageOnReport, bool needAssert)
		{
			this.internalMessage = string.Empty;
			this.errorCode = -1;
			base..ctor(SR.GetString("ERR_INTERNAL"), innerException);
			this.errorCode = errorCode;
			this.showErrorMesageOnReport = showErrorMesageOnReport;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00011E64 File Offset: 0x00010E64
		private InternalException(SerializationInfo info, StreamingContext context)
		{
			this.internalMessage = string.Empty;
			this.errorCode = -1;
			base..ctor(info, context);
			this.internalMessage = info.GetString("InternalMessage");
			this.errorCode = info.GetInt32("ErrorCode");
			this.showErrorMesageOnReport = info.GetBoolean("ShowErrorMesageOnReport");
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00011EBE File Offset: 0x00010EBE
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("InternalMessage", this.internalMessage);
			info.AddValue("ErrorCode", this.errorCode);
			info.AddValue("ShowErrorMesageOnReport", this.showErrorMesageOnReport);
			base.GetObjectData(info, context);
		}

		// Token: 0x04000BB9 RID: 3001
		private const string internalExceptionMessageID = "ERR_INTERNAL";

		// Token: 0x04000BBA RID: 3002
		private string internalMessage;

		// Token: 0x04000BBB RID: 3003
		private bool showErrorMesageOnReport;

		// Token: 0x04000BBC RID: 3004
		private int errorCode;
	}
}
