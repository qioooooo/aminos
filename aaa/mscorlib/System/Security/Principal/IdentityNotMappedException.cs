using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.Principal
{
	// Token: 0x02000933 RID: 2355
	[ComVisible(false)]
	[Serializable]
	public sealed class IdentityNotMappedException : SystemException
	{
		// Token: 0x06005577 RID: 21879 RVA: 0x001371D8 File Offset: 0x001361D8
		public IdentityNotMappedException()
			: base(Environment.GetResourceString("IdentityReference_IdentityNotMapped"))
		{
		}

		// Token: 0x06005578 RID: 21880 RVA: 0x001371EA File Offset: 0x001361EA
		public IdentityNotMappedException(string message)
			: base(message)
		{
		}

		// Token: 0x06005579 RID: 21881 RVA: 0x001371F3 File Offset: 0x001361F3
		public IdentityNotMappedException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x0600557A RID: 21882 RVA: 0x001371FD File Offset: 0x001361FD
		internal IdentityNotMappedException(string message, IdentityReferenceCollection unmappedIdentities)
			: this(message)
		{
			this.unmappedIdentities = unmappedIdentities;
		}

		// Token: 0x0600557B RID: 21883 RVA: 0x0013720D File Offset: 0x0013620D
		internal IdentityNotMappedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600557C RID: 21884 RVA: 0x00137217 File Offset: 0x00136217
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x0600557D RID: 21885 RVA: 0x00137221 File Offset: 0x00136221
		public IdentityReferenceCollection UnmappedIdentities
		{
			get
			{
				if (this.unmappedIdentities == null)
				{
					this.unmappedIdentities = new IdentityReferenceCollection();
				}
				return this.unmappedIdentities;
			}
		}

		// Token: 0x04002C89 RID: 11401
		private IdentityReferenceCollection unmappedIdentities;
	}
}
