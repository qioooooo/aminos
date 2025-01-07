using System;
using System.Design;
using System.Runtime.Serialization;

namespace System.Data.Design
{
	[Serializable]
	internal class InternalException : Exception, ISerializable
	{
		internal InternalException(string internalMessage)
			: this(internalMessage, null)
		{
		}

		internal InternalException(string internalMessage, Exception innerException)
			: this(innerException, internalMessage, -1, false)
		{
		}

		internal InternalException(string internalMessage, int errorCode)
			: this(null, internalMessage, errorCode, false)
		{
		}

		internal InternalException(string internalMessage, int errorCode, bool showTextOnReport)
			: this(null, internalMessage, errorCode, showTextOnReport)
		{
		}

		internal InternalException(Exception innerException, string internalMessage, int errorCode, bool showErrorMesageOnReport)
			: this(innerException, internalMessage, errorCode, showErrorMesageOnReport, true)
		{
		}

		internal InternalException(Exception innerException, string internalMessage, int errorCode, bool showErrorMesageOnReport, bool needAssert)
		{
			this.internalMessage = string.Empty;
			this.errorCode = -1;
			base..ctor(SR.GetString("ERR_INTERNAL"), innerException);
			this.errorCode = errorCode;
			this.showErrorMesageOnReport = showErrorMesageOnReport;
		}

		private InternalException(SerializationInfo info, StreamingContext context)
		{
			this.internalMessage = string.Empty;
			this.errorCode = -1;
			base..ctor(info, context);
			this.internalMessage = info.GetString("InternalMessage");
			this.errorCode = info.GetInt32("ErrorCode");
			this.showErrorMesageOnReport = info.GetBoolean("ShowErrorMesageOnReport");
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("InternalMessage", this.internalMessage);
			info.AddValue("ErrorCode", this.errorCode);
			info.AddValue("ShowErrorMesageOnReport", this.showErrorMesageOnReport);
			base.GetObjectData(info, context);
		}

		private const string internalExceptionMessageID = "ERR_INTERNAL";

		private string internalMessage;

		private bool showErrorMesageOnReport;

		private int errorCode;
	}
}
