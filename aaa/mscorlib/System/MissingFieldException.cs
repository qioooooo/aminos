using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D6 RID: 214
	[ComVisible(true)]
	[Serializable]
	public class MissingFieldException : MissingMemberException, ISerializable
	{
		// Token: 0x06000C07 RID: 3079 RVA: 0x00023EA8 File Offset: 0x00022EA8
		public MissingFieldException()
			: base(Environment.GetResourceString("Arg_MissingFieldException"))
		{
			base.SetErrorCode(-2146233071);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00023EC5 File Offset: 0x00022EC5
		public MissingFieldException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233071);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00023ED9 File Offset: 0x00022ED9
		public MissingFieldException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233071);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00023EEE File Offset: 0x00022EEE
		protected MissingFieldException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00023EF8 File Offset: 0x00022EF8
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingField_Name", new object[] { ((this.Signature != null) ? (MissingMemberException.FormatSignature(this.Signature) + " ") : "") + this.ClassName + "." + this.MemberName }), new object[0]);
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00023F73 File Offset: 0x00022F73
		private MissingFieldException(string className, string fieldName, byte[] signature)
		{
			this.ClassName = className;
			this.MemberName = fieldName;
			this.Signature = signature;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00023F90 File Offset: 0x00022F90
		public MissingFieldException(string className, string fieldName)
		{
			this.ClassName = className;
			this.MemberName = fieldName;
		}
	}
}
