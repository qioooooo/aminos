using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Policy
{
	// Token: 0x02000497 RID: 1175
	[ComVisible(true)]
	[Serializable]
	public class PolicyException : SystemException
	{
		// Token: 0x06002F31 RID: 12081 RVA: 0x000A122B File Offset: 0x000A022B
		public PolicyException()
			: base(Environment.GetResourceString("Policy_Default"))
		{
			base.HResult = -2146233322;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000A1248 File Offset: 0x000A0248
		public PolicyException(string message)
			: base(message)
		{
			base.HResult = -2146233322;
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000A125C File Offset: 0x000A025C
		public PolicyException(string message, Exception exception)
			: base(message, exception)
		{
			base.HResult = -2146233322;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x000A1271 File Offset: 0x000A0271
		protected PolicyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x000A127B File Offset: 0x000A027B
		internal PolicyException(string message, int hresult)
			: base(message)
		{
			base.HResult = hresult;
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x000A128B File Offset: 0x000A028B
		internal PolicyException(string message, int hresult, Exception exception)
			: base(message, exception)
		{
			base.HResult = hresult;
		}
	}
}
