using System;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x02000081 RID: 129
	[Serializable]
	public sealed class RegistrationException : SystemException
	{
		// Token: 0x060002DA RID: 730 RVA: 0x00007D56 File Offset: 0x00006D56
		public RegistrationException()
		{
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00007D5E File Offset: 0x00006D5E
		public RegistrationException(string msg)
			: base(msg)
		{
			this._errorInfo = null;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00007D6E File Offset: 0x00006D6E
		public RegistrationException(string msg, Exception inner)
			: base(msg, inner)
		{
			this._errorInfo = null;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00007D7F File Offset: 0x00006D7F
		internal RegistrationException(string msg, RegistrationErrorInfo[] errorInfo)
			: base(msg)
		{
			this._errorInfo = errorInfo;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00007D8F File Offset: 0x00006D8F
		internal RegistrationException(string msg, RegistrationErrorInfo[] errorInfo, Exception inner)
			: base(msg, inner)
		{
			this._errorInfo = errorInfo;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00007DA0 File Offset: 0x00006DA0
		internal RegistrationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			try
			{
				this._errorInfo = (RegistrationErrorInfo[])info.GetValue("RegistrationException._errorInfo", typeof(RegistrationErrorInfo[]));
			}
			catch (SerializationException)
			{
				this._errorInfo = null;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00007DF4 File Offset: 0x00006DF4
		public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			if (info == null)
			{
				throw new ArgumentException(Resource.FormatString("Err_info"));
			}
			base.GetObjectData(info, ctx);
			if (this._errorInfo != null)
			{
				info.AddValue("RegistrationException._errorInfo", this._errorInfo, typeof(RegistrationErrorInfo[]));
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00007E34 File Offset: 0x00006E34
		public RegistrationErrorInfo[] ErrorInfo
		{
			get
			{
				return this._errorInfo;
			}
		}

		// Token: 0x04000130 RID: 304
		private RegistrationErrorInfo[] _errorInfo;
	}
}
