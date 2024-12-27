using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x020006B3 RID: 1715
	[Serializable]
	internal class InternalSink
	{
		// Token: 0x06003E63 RID: 15971 RVA: 0x000D6BD0 File Offset: 0x000D5BD0
		internal static IMessage ValidateMessage(IMessage reqMsg)
		{
			IMessage message = null;
			if (reqMsg == null)
			{
				message = new ReturnMessage(new ArgumentNullException("reqMsg"), null);
			}
			return message;
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x000D6BF4 File Offset: 0x000D5BF4
		internal static IMessage DisallowAsyncActivation(IMessage reqMsg)
		{
			if (reqMsg is IConstructionCallMessage)
			{
				return new ReturnMessage(new RemotingException(Environment.GetResourceString("Remoting_Activation_AsyncUnsupported")), null);
			}
			return null;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x000D6C18 File Offset: 0x000D5C18
		internal static Identity GetIdentity(IMessage reqMsg)
		{
			Identity identity = null;
			if (reqMsg is IInternalMessage)
			{
				identity = ((IInternalMessage)reqMsg).IdentityObject;
			}
			else if (reqMsg is InternalMessageWrapper)
			{
				identity = (Identity)((InternalMessageWrapper)reqMsg).GetIdentityObject();
			}
			if (identity == null)
			{
				string uri = InternalSink.GetURI(reqMsg);
				identity = IdentityHolder.ResolveIdentity(uri);
				if (identity == null)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_ServerObjectNotFound"), new object[] { uri }));
				}
			}
			return identity;
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x000D6C94 File Offset: 0x000D5C94
		internal static ServerIdentity GetServerIdentity(IMessage reqMsg)
		{
			ServerIdentity serverIdentity = null;
			bool flag = false;
			IInternalMessage internalMessage = reqMsg as IInternalMessage;
			if (internalMessage != null)
			{
				serverIdentity = ((IInternalMessage)reqMsg).ServerIdentityObject;
				flag = true;
			}
			else if (reqMsg is InternalMessageWrapper)
			{
				serverIdentity = (ServerIdentity)((InternalMessageWrapper)reqMsg).GetServerIdentityObject();
			}
			if (serverIdentity == null)
			{
				string uri = InternalSink.GetURI(reqMsg);
				Identity identity = IdentityHolder.ResolveIdentity(uri);
				if (identity is ServerIdentity)
				{
					serverIdentity = (ServerIdentity)identity;
					if (flag)
					{
						internalMessage.ServerIdentityObject = serverIdentity;
					}
				}
			}
			return serverIdentity;
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x000D6D08 File Offset: 0x000D5D08
		internal static string GetURI(IMessage msg)
		{
			string text = null;
			IMethodMessage methodMessage = msg as IMethodMessage;
			if (methodMessage != null)
			{
				text = methodMessage.Uri;
			}
			else
			{
				IDictionary properties = msg.Properties;
				if (properties != null)
				{
					text = (string)properties["__Uri"];
				}
			}
			return text;
		}
	}
}
