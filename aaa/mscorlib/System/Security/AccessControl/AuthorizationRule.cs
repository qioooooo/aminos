using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x020008F8 RID: 2296
	public abstract class AuthorizationRule
	{
		// Token: 0x0600538E RID: 21390 RVA: 0x0012FEF4 File Offset: 0x0012EEF4
		protected internal AuthorizationRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (!identity.IsValidTargetType(typeof(SecurityIdentifier)))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeIdentityReferenceType"), "identity");
			}
			if (accessMask == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_ArgumentZero"), "accessMask");
			}
			if ((inheritanceFlags < InheritanceFlags.None) || inheritanceFlags > (InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit))
			{
				throw new ArgumentOutOfRangeException("inheritanceFlags", Environment.GetResourceString("Argument_InvalidEnumValue", new object[] { inheritanceFlags, "InheritanceFlags" }));
			}
			if ((propagationFlags < PropagationFlags.None) || propagationFlags > (PropagationFlags.NoPropagateInherit | PropagationFlags.InheritOnly))
			{
				throw new ArgumentOutOfRangeException("propagationFlags", Environment.GetResourceString("Argument_InvalidEnumValue", new object[] { inheritanceFlags, "PropagationFlags" }));
			}
			this._identity = identity;
			this._accessMask = accessMask;
			this._isInherited = isInherited;
			this._inheritanceFlags = inheritanceFlags;
			if (inheritanceFlags != InheritanceFlags.None)
			{
				this._propagationFlags = propagationFlags;
				return;
			}
			this._propagationFlags = PropagationFlags.None;
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x0600538F RID: 21391 RVA: 0x0012FFFD File Offset: 0x0012EFFD
		public IdentityReference IdentityReference
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06005390 RID: 21392 RVA: 0x00130005 File Offset: 0x0012F005
		protected internal int AccessMask
		{
			get
			{
				return this._accessMask;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06005391 RID: 21393 RVA: 0x0013000D File Offset: 0x0012F00D
		public bool IsInherited
		{
			get
			{
				return this._isInherited;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06005392 RID: 21394 RVA: 0x00130015 File Offset: 0x0012F015
		public InheritanceFlags InheritanceFlags
		{
			get
			{
				return this._inheritanceFlags;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005393 RID: 21395 RVA: 0x0013001D File Offset: 0x0012F01D
		public PropagationFlags PropagationFlags
		{
			get
			{
				return this._propagationFlags;
			}
		}

		// Token: 0x04002B33 RID: 11059
		private readonly IdentityReference _identity;

		// Token: 0x04002B34 RID: 11060
		private readonly int _accessMask;

		// Token: 0x04002B35 RID: 11061
		private readonly bool _isInherited;

		// Token: 0x04002B36 RID: 11062
		private readonly InheritanceFlags _inheritanceFlags;

		// Token: 0x04002B37 RID: 11063
		private readonly PropagationFlags _propagationFlags;
	}
}
