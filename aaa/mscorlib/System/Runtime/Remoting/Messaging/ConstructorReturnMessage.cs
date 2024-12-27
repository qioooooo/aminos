using System;
using System.Collections;
using System.Runtime.Remoting.Activation;
using System.Threading;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006FC RID: 1788
	internal class ConstructorReturnMessage : ReturnMessage, IConstructionReturnMessage, IMethodReturnMessage, IMethodMessage, IMessage
	{
		// Token: 0x0600403A RID: 16442 RVA: 0x000DB92B File Offset: 0x000DA92B
		public ConstructorReturnMessage(MarshalByRefObject o, object[] outArgs, int outArgsCount, LogicalCallContext callCtx, IConstructionCallMessage ccm)
			: base(o, outArgs, outArgsCount, callCtx, ccm)
		{
			this._o = o;
			this._iFlags = 1;
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x000DB948 File Offset: 0x000DA948
		public ConstructorReturnMessage(Exception e, IConstructionCallMessage ccm)
			: base(e, ccm)
		{
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x0600403C RID: 16444 RVA: 0x000DB952 File Offset: 0x000DA952
		public override object ReturnValue
		{
			get
			{
				if (this._iFlags == 1)
				{
					return RemotingServices.MarshalInternal(this._o, null, null);
				}
				return base.ReturnValue;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x0600403D RID: 16445 RVA: 0x000DB974 File Offset: 0x000DA974
		public override IDictionary Properties
		{
			get
			{
				if (this._properties == null)
				{
					object obj = new CRMDictionary(this, new Hashtable());
					Interlocked.CompareExchange(ref this._properties, obj, null);
				}
				return (IDictionary)this._properties;
			}
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x000DB9AE File Offset: 0x000DA9AE
		internal object GetObject()
		{
			return this._o;
		}

		// Token: 0x04002048 RID: 8264
		private const int Intercept = 1;

		// Token: 0x04002049 RID: 8265
		private MarshalByRefObject _o;

		// Token: 0x0400204A RID: 8266
		private int _iFlags;
	}
}
