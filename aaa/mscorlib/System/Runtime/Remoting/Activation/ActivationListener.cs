using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x02000689 RID: 1673
	internal class ActivationListener : MarshalByRefObject, IActivator
	{
		// Token: 0x06003D01 RID: 15617 RVA: 0x000D1C78 File Offset: 0x000D0C78
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06003D02 RID: 15618 RVA: 0x000D1C7B File Offset: 0x000D0C7B
		// (set) Token: 0x06003D03 RID: 15619 RVA: 0x000D1C7E File Offset: 0x000D0C7E
		public virtual IActivator NextActivator
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06003D04 RID: 15620 RVA: 0x000D1C85 File Offset: 0x000D0C85
		public virtual ActivatorLevel Level
		{
			get
			{
				return ActivatorLevel.AppDomain;
			}
		}

		// Token: 0x06003D05 RID: 15621 RVA: 0x000D1C8C File Offset: 0x000D0C8C
		[ComVisible(true)]
		public virtual IConstructionReturnMessage Activate(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null || RemotingServices.IsTransparentProxy(ctorMsg))
			{
				throw new ArgumentNullException("ctorMsg");
			}
			ctorMsg.Properties["Permission"] = "allowed";
			string activationTypeName = ctorMsg.ActivationTypeName;
			if (!RemotingConfigHandler.IsActivationAllowed(activationTypeName))
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Activation_PermissionDenied"), new object[] { ctorMsg.ActivationTypeName }));
			}
			if (ctorMsg.ActivationType == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { ctorMsg.ActivationTypeName }));
			}
			return ActivationServices.GetActivator().Activate(ctorMsg);
		}
	}
}
