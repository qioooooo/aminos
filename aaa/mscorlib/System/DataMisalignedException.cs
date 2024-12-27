using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x0200003A RID: 58
	[ComVisible(true)]
	[Serializable]
	public sealed class DataMisalignedException : SystemException
	{
		// Token: 0x0600037D RID: 893 RVA: 0x0000E35E File Offset: 0x0000D35E
		public DataMisalignedException()
			: base(Environment.GetResourceString("Arg_DataMisalignedException"))
		{
			base.SetErrorCode(-2146233023);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x0000E37B File Offset: 0x0000D37B
		public DataMisalignedException(string message)
			: base(message)
		{
			base.SetErrorCode(-2146233023);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x0000E38F File Offset: 0x0000D38F
		public DataMisalignedException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2146233023);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0000E3A4 File Offset: 0x0000D3A4
		internal DataMisalignedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
