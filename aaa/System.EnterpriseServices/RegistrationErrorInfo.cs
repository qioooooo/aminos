using System;

namespace System.EnterpriseServices
{
	// Token: 0x02000080 RID: 128
	[Serializable]
	public sealed class RegistrationErrorInfo
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00007CA1 File Offset: 0x00006CA1
		public string MajorRef
		{
			get
			{
				return this._majorRef;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x00007CA9 File Offset: 0x00006CA9
		public string MinorRef
		{
			get
			{
				return this._minorRef;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00007CB1 File Offset: 0x00006CB1
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x00007CB9 File Offset: 0x00006CB9
		public int ErrorCode
		{
			get
			{
				return this._errorCode;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00007CC1 File Offset: 0x00006CC1
		public string ErrorString
		{
			get
			{
				return this._errorString;
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00007CCC File Offset: 0x00006CCC
		internal RegistrationErrorInfo(string majorRef, string minorRef, string name, int errorCode)
		{
			this._majorRef = majorRef;
			this._minorRef = minorRef;
			this._name = name;
			this._errorCode = errorCode;
			if (this._majorRef == null)
			{
				this._majorRef = "";
			}
			if (this._minorRef == null)
			{
				this._minorRef = "<invalid>";
			}
			this._errorString = Util.GetErrorString(this._errorCode);
			if (this._errorString == null)
			{
				this._errorString = Resource.FormatString("Err_UnknownHR", this._errorCode);
			}
		}

		// Token: 0x0400012B RID: 299
		private string _majorRef;

		// Token: 0x0400012C RID: 300
		private string _minorRef;

		// Token: 0x0400012D RID: 301
		private string _name;

		// Token: 0x0400012E RID: 302
		private int _errorCode;

		// Token: 0x0400012F RID: 303
		private string _errorString;
	}
}
