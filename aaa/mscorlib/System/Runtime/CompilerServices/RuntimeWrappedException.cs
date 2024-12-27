using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005F5 RID: 1525
	[Serializable]
	public sealed class RuntimeWrappedException : Exception
	{
		// Token: 0x060037B9 RID: 14265 RVA: 0x000BBA05 File Offset: 0x000BAA05
		private RuntimeWrappedException(object thrownObject)
			: base(Environment.GetResourceString("RuntimeWrappedException"))
		{
			base.SetErrorCode(-2146233026);
			this.m_wrappedException = thrownObject;
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060037BA RID: 14266 RVA: 0x000BBA29 File Offset: 0x000BAA29
		public object WrappedException
		{
			get
			{
				return this.m_wrappedException;
			}
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000BBA31 File Offset: 0x000BAA31
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("WrappedException", this.m_wrappedException, typeof(object));
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000BBA64 File Offset: 0x000BAA64
		internal RuntimeWrappedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.m_wrappedException = info.GetValue("WrappedException", typeof(object));
		}

		// Token: 0x04001CB9 RID: 7353
		private object m_wrappedException;
	}
}
