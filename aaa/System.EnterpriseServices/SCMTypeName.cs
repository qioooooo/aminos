using System;
using System.Runtime.Remoting;

namespace System.EnterpriseServices
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	internal class SCMTypeName : IRemotingTypeInfo
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00005514 File Offset: 0x00004514
		internal SCMTypeName(Type serverType)
		{
			this._serverType = serverType;
			this._serverTypeName = serverType.AssemblyQualifiedName;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000552F File Offset: 0x0000452F
		// (set) Token: 0x06000129 RID: 297 RVA: 0x00005537 File Offset: 0x00004537
		public virtual string TypeName
		{
			get
			{
				return this._serverTypeName;
			}
			set
			{
				this._serverTypeName = value;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005540 File Offset: 0x00004540
		public virtual bool CanCastTo(Type castType, object o)
		{
			return castType.IsAssignableFrom(this._serverType);
		}

		// Token: 0x04000087 RID: 135
		private Type _serverType;

		// Token: 0x04000088 RID: 136
		private string _serverTypeName;
	}
}
