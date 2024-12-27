using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace System.EnterpriseServices
{
	// Token: 0x0200002C RID: 44
	internal class MethodCallMessageWrapperEx : MethodCallMessageWrapper
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x00004100 File Offset: 0x00003100
		public MethodCallMessageWrapperEx(IMethodCallMessage imcmsg, MethodBase mb)
			: base(imcmsg)
		{
			this._mb = mb;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004110 File Offset: 0x00003110
		public override MethodBase MethodBase
		{
			get
			{
				return this._mb;
			}
		}

		// Token: 0x04000058 RID: 88
		private MethodBase _mb;
	}
}
