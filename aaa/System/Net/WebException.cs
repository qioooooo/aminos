using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200049E RID: 1182
	[Serializable]
	public class WebException : InvalidOperationException, ISerializable
	{
		// Token: 0x0600240D RID: 9229 RVA: 0x0008D240 File Offset: 0x0008C240
		public WebException()
		{
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x0008D250 File Offset: 0x0008C250
		public WebException(string message)
			: this(message, null)
		{
		}

		// Token: 0x0600240F RID: 9231 RVA: 0x0008D25A File Offset: 0x0008C25A
		public WebException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x0008D26C File Offset: 0x0008C26C
		public WebException(string message, WebExceptionStatus status)
			: this(message, null, status, null)
		{
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x0008D278 File Offset: 0x0008C278
		internal WebException(string message, WebExceptionStatus status, WebExceptionInternalStatus internalStatus, Exception innerException)
			: this(message, innerException, status, null, internalStatus)
		{
		}

		// Token: 0x06002412 RID: 9234 RVA: 0x0008D286 File Offset: 0x0008C286
		public WebException(string message, Exception innerException, WebExceptionStatus status, WebResponse response)
			: this(message, null, innerException, status, response)
		{
		}

		// Token: 0x06002413 RID: 9235 RVA: 0x0008D294 File Offset: 0x0008C294
		internal WebException(string message, string data, Exception innerException, WebExceptionStatus status, WebResponse response)
			: base(message + ((data != null) ? (": '" + data + "'") : ""), innerException)
		{
			this.m_Status = status;
			this.m_Response = response;
		}

		// Token: 0x06002414 RID: 9236 RVA: 0x0008D2E0 File Offset: 0x0008C2E0
		internal WebException(string message, Exception innerException, WebExceptionStatus status, WebResponse response, WebExceptionInternalStatus internalStatus)
			: this(message, null, innerException, status, response, internalStatus)
		{
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x0008D2F0 File Offset: 0x0008C2F0
		internal WebException(string message, string data, Exception innerException, WebExceptionStatus status, WebResponse response, WebExceptionInternalStatus internalStatus)
			: base(message + ((data != null) ? (": '" + data + "'") : ""), innerException)
		{
			this.m_Status = status;
			this.m_Response = response;
			this.m_InternalStatus = internalStatus;
		}

		// Token: 0x06002416 RID: 9238 RVA: 0x0008D344 File Offset: 0x0008C344
		protected WebException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06002417 RID: 9239 RVA: 0x0008D356 File Offset: 0x0008C356
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06002418 RID: 9240 RVA: 0x0008D360 File Offset: 0x0008C360
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x0008D36A File Offset: 0x0008C36A
		public WebExceptionStatus Status
		{
			get
			{
				return this.m_Status;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x0008D372 File Offset: 0x0008C372
		public WebResponse Response
		{
			get
			{
				return this.m_Response;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x0008D37A File Offset: 0x0008C37A
		internal WebExceptionInternalStatus InternalStatus
		{
			get
			{
				return this.m_InternalStatus;
			}
		}

		// Token: 0x04002467 RID: 9319
		private WebExceptionStatus m_Status = WebExceptionStatus.UnknownError;

		// Token: 0x04002468 RID: 9320
		private WebResponse m_Response;

		// Token: 0x04002469 RID: 9321
		[NonSerialized]
		private WebExceptionInternalStatus m_InternalStatus;
	}
}
