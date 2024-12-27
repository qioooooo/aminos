using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x020006B1 RID: 1713
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ContextProperty
	{
		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06003E5B RID: 15963 RVA: 0x000D6BA8 File Offset: 0x000D5BA8
		public virtual string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06003E5C RID: 15964 RVA: 0x000D6BB0 File Offset: 0x000D5BB0
		public virtual object Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x000D6BB8 File Offset: 0x000D5BB8
		internal ContextProperty(string name, object prop)
		{
			this._name = name;
			this._property = prop;
		}

		// Token: 0x04001F96 RID: 8086
		internal string _name;

		// Token: 0x04001F97 RID: 8087
		internal object _property;
	}
}
