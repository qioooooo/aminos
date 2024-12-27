using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020005AB RID: 1451
	[ComVisible(true)]
	[Serializable]
	public class PathTooLongException : IOException
	{
		// Token: 0x0600365A RID: 13914 RVA: 0x000B89FA File Offset: 0x000B79FA
		public PathTooLongException()
			: base(Environment.GetResourceString("IO.PathTooLong"))
		{
			base.SetErrorCode(-2147024690);
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x000B8A17 File Offset: 0x000B7A17
		public PathTooLongException(string message)
			: base(message)
		{
			base.SetErrorCode(-2147024690);
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x000B8A2B File Offset: 0x000B7A2B
		public PathTooLongException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetErrorCode(-2147024690);
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000B8A40 File Offset: 0x000B7A40
		protected PathTooLongException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
