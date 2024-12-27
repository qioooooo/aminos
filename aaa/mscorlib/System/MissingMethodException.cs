using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x020000D7 RID: 215
	[ComVisible(true)]
	[Serializable]
	public class MissingMethodException : MissingMemberException, ISerializable
	{
		// Token: 0x06000C0E RID: 3086 RVA: 0x00023FA6 File Offset: 0x00022FA6
		public MissingMethodException()
			: base(Environment.GetResourceString("Arg_MissingMethodException"))
		{
			base.SetErrorCode(-2146233069);
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x00023FC3 File Offset: 0x00022FC3
		public MissingMethodException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233069);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x00023FD7 File Offset: 0x00022FD7
		public MissingMethodException(string message, Exception inner)
			: base(message, inner)
		{
			base.SetErrorCode(-2146233069);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x00023FEC File Offset: 0x00022FEC
		protected MissingMethodException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00023FF8 File Offset: 0x00022FF8
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingMethod_Name", new object[] { this.ClassName + "." + this.MemberName + ((this.Signature != null) ? (" " + MissingMemberException.FormatSignature(this.Signature)) : "") }), new object[0]);
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00024073 File Offset: 0x00023073
		private MissingMethodException(string className, string methodName, byte[] signature)
		{
			this.ClassName = className;
			this.MemberName = methodName;
			this.Signature = signature;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x00024090 File Offset: 0x00023090
		public MissingMethodException(string className, string methodName)
		{
			this.ClassName = className;
			this.MemberName = methodName;
		}
	}
}
