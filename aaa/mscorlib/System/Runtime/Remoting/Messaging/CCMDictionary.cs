using System;
using System.Collections;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006FF RID: 1791
	internal class CCMDictionary : MessageDictionary
	{
		// Token: 0x06004077 RID: 16503 RVA: 0x000DC0F3 File Offset: 0x000DB0F3
		public CCMDictionary(IConstructionCallMessage msg, IDictionary idict)
			: base(CCMDictionary.CCMkeys, idict)
		{
			this._ccmsg = msg;
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x000DC108 File Offset: 0x000DB108
		internal override object GetMessageValue(int i)
		{
			switch (i)
			{
			case 0:
				return this._ccmsg.Uri;
			case 1:
				return this._ccmsg.MethodName;
			case 2:
				return this._ccmsg.MethodSignature;
			case 3:
				return this._ccmsg.TypeName;
			case 4:
				return this._ccmsg.Args;
			case 5:
				return this.FetchLogicalCallContext();
			case 6:
				return this._ccmsg.CallSiteActivationAttributes;
			case 7:
				return null;
			case 8:
				return this._ccmsg.ContextProperties;
			case 9:
				return this._ccmsg.Activator;
			case 10:
				return this._ccmsg.ActivationTypeName;
			default:
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x000DC1D0 File Offset: 0x000DB1D0
		private LogicalCallContext FetchLogicalCallContext()
		{
			ConstructorCallMessage constructorCallMessage = this._ccmsg as ConstructorCallMessage;
			if (constructorCallMessage != null)
			{
				return constructorCallMessage.GetLogicalCallContext();
			}
			if (this._ccmsg is ConstructionCall)
			{
				return ((MethodCall)this._ccmsg).GetLogicalCallContext();
			}
			throw new RemotingException(Environment.GetResourceString("Remoting_Message_BadType"));
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x000DC220 File Offset: 0x000DB220
		internal override void SetSpecialKey(int keyNum, object value)
		{
			switch (keyNum)
			{
			case 0:
				((ConstructorCallMessage)this._ccmsg).Uri = (string)value;
				return;
			case 1:
				((ConstructorCallMessage)this._ccmsg).SetLogicalCallContext((LogicalCallContext)value);
				return;
			default:
				throw new RemotingException(Environment.GetResourceString("Remoting_Default"));
			}
		}

		// Token: 0x04002059 RID: 8281
		public static string[] CCMkeys = new string[]
		{
			"__Uri", "__MethodName", "__MethodSignature", "__TypeName", "__Args", "__CallContext", "__CallSiteActivationAttributes", "__ActivationType", "__ContextProperties", "__Activator",
			"__ActivationTypeName"
		};

		// Token: 0x0400205A RID: 8282
		internal IConstructionCallMessage _ccmsg;
	}
}
