using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x02000030 RID: 48
	internal class IntermediaryObjRef : ObjRef
	{
		// Token: 0x060000EB RID: 235 RVA: 0x000046F8 File Offset: 0x000036F8
		public IntermediaryObjRef(MarshalByRefObject mbro, Type reqtype, RealProxy pxy)
			: base(mbro, reqtype)
		{
			this._custom = pxy.CreateObjRef(reqtype);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004710 File Offset: 0x00003710
		public override void GetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			object data = CallContext.GetData("__ClientIsClr");
			bool flag = data != null && (bool)data;
			if (flag)
			{
				base.GetObjectData(info, ctx);
				return;
			}
			this._custom.GetObjectData(info, ctx);
		}

		// Token: 0x0400006A RID: 106
		private ObjRef _custom;
	}
}
