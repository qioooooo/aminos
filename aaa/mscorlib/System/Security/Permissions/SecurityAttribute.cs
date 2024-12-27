using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200061A RID: 1562
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public abstract class SecurityAttribute : Attribute
	{
		// Token: 0x060038D6 RID: 14550 RVA: 0x000C0F37 File Offset: 0x000BFF37
		protected SecurityAttribute(SecurityAction action)
		{
			this.m_action = action;
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060038D7 RID: 14551 RVA: 0x000C0F46 File Offset: 0x000BFF46
		// (set) Token: 0x060038D8 RID: 14552 RVA: 0x000C0F4E File Offset: 0x000BFF4E
		public SecurityAction Action
		{
			get
			{
				return this.m_action;
			}
			set
			{
				this.m_action = value;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000C0F57 File Offset: 0x000BFF57
		// (set) Token: 0x060038DA RID: 14554 RVA: 0x000C0F5F File Offset: 0x000BFF5F
		public bool Unrestricted
		{
			get
			{
				return this.m_unrestricted;
			}
			set
			{
				this.m_unrestricted = value;
			}
		}

		// Token: 0x060038DB RID: 14555
		public abstract IPermission CreatePermission();

		// Token: 0x060038DC RID: 14556 RVA: 0x000C0F68 File Offset: 0x000BFF68
		internal static IntPtr FindSecurityAttributeTypeHandle(string typeName)
		{
			PermissionSet.s_fullTrust.Assert();
			Type type = Type.GetType(typeName, false, false);
			if (type == null)
			{
				return IntPtr.Zero;
			}
			return type.TypeHandle.Value;
		}

		// Token: 0x04001D5D RID: 7517
		internal SecurityAction m_action;

		// Token: 0x04001D5E RID: 7518
		internal bool m_unrestricted;
	}
}
