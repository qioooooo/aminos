using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x02000686 RID: 1670
	[AttributeUsage(AttributeTargets.Class)]
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class ContextAttribute : Attribute, IContextAttribute, IContextProperty
	{
		// Token: 0x06003CEC RID: 15596 RVA: 0x000D1861 File Offset: 0x000D0861
		public ContextAttribute(string name)
		{
			this.AttributeName = name;
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06003CED RID: 15597 RVA: 0x000D1870 File Offset: 0x000D0870
		public virtual string Name
		{
			get
			{
				return this.AttributeName;
			}
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x000D1878 File Offset: 0x000D0878
		public virtual bool IsNewContextOK(Context newCtx)
		{
			return true;
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x000D187B File Offset: 0x000D087B
		public virtual void Freeze(Context newContext)
		{
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x000D1880 File Offset: 0x000D0880
		public override bool Equals(object o)
		{
			IContextProperty contextProperty = o as IContextProperty;
			return contextProperty != null && this.AttributeName.Equals(contextProperty.Name);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x000D18AA File Offset: 0x000D08AA
		public override int GetHashCode()
		{
			return this.AttributeName.GetHashCode();
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x000D18B8 File Offset: 0x000D08B8
		public virtual bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			if (ctx == null)
			{
				throw new ArgumentNullException("ctx");
			}
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			if (!ctorMsg.ActivationType.IsContextful)
			{
				return true;
			}
			object property = ctx.GetProperty(this.AttributeName);
			return property != null && this.Equals(property);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x000D190C File Offset: 0x000D090C
		public virtual void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
			{
				throw new ArgumentNullException("ctorMsg");
			}
			ctorMsg.ContextProperties.Add(this);
		}

		// Token: 0x04001F17 RID: 7959
		protected string AttributeName;
	}
}
