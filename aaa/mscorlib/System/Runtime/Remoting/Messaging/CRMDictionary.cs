using System;
using System.Collections;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000700 RID: 1792
	internal class CRMDictionary : MessageDictionary
	{
		// Token: 0x0600407C RID: 16508 RVA: 0x000DC2F5 File Offset: 0x000DB2F5
		public CRMDictionary(IConstructionReturnMessage msg, IDictionary idict)
			: base((msg.Exception != null) ? CRMDictionary.CRMkeysFault : CRMDictionary.CRMkeysNoFault, idict)
		{
			this.fault = msg.Exception != null;
			this._crmsg = msg;
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x000DC32C File Offset: 0x000DB32C
		internal override object GetMessageValue(int i)
		{
			switch (i)
			{
			case 0:
				return this._crmsg.Uri;
			case 1:
				return this._crmsg.MethodName;
			case 2:
				return this._crmsg.MethodSignature;
			case 3:
				return this._crmsg.TypeName;
			case 4:
				if (!this.fault)
				{
					return this._crmsg.ReturnValue;
				}
				return this.FetchLogicalCallContext();
			case 5:
				return this._crmsg.Args;
			case 6:
				return this.FetchLogicalCallContext();
			default:
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x000DC3CC File Offset: 0x000DB3CC
		private LogicalCallContext FetchLogicalCallContext()
		{
			ReturnMessage returnMessage = this._crmsg as ReturnMessage;
			if (returnMessage != null)
			{
				return returnMessage.GetLogicalCallContext();
			}
			MethodResponse methodResponse = this._crmsg as MethodResponse;
			if (methodResponse != null)
			{
				return methodResponse.GetLogicalCallContext();
			}
			throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadType"));
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x000DC414 File Offset: 0x000DB414
		internal override void SetSpecialKey(int keyNum, object value)
		{
			ReturnMessage returnMessage = this._crmsg as ReturnMessage;
			MethodResponse methodResponse = this._crmsg as MethodResponse;
			switch (keyNum)
			{
			case 0:
				if (returnMessage != null)
				{
					returnMessage.Uri = (string)value;
					return;
				}
				if (methodResponse != null)
				{
					methodResponse.Uri = (string)value;
					return;
				}
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadType"));
			case 1:
				if (returnMessage != null)
				{
					returnMessage.SetLogicalCallContext((LogicalCallContext)value);
					return;
				}
				if (methodResponse != null)
				{
					methodResponse.SetLogicalCallContext((LogicalCallContext)value);
					return;
				}
				throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadType"));
			default:
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
		}

		// Token: 0x0400205B RID: 8283
		public static string[] CRMkeysFault = new string[] { "__Uri", "__MethodName", "__MethodSignature", "__TypeName", "__CallContext" };

		// Token: 0x0400205C RID: 8284
		public static string[] CRMkeysNoFault = new string[] { "__Uri", "__MethodName", "__MethodSignature", "__TypeName", "__Return", "__OutArgs", "__CallContext" };

		// Token: 0x0400205D RID: 8285
		internal IConstructionReturnMessage _crmsg;

		// Token: 0x0400205E RID: 8286
		internal bool fault;
	}
}
