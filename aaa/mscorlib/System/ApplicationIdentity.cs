using System;
using System.Deployment.Internal.Isolation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000069 RID: 105
	[ComVisible(false)]
	[Serializable]
	public sealed class ApplicationIdentity : ISerializable
	{
		// Token: 0x0600062A RID: 1578 RVA: 0x00015534 File Offset: 0x00014534
		private ApplicationIdentity()
		{
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001553C File Offset: 0x0001453C
		private ApplicationIdentity(SerializationInfo info, StreamingContext context)
		{
			string text = (string)info.GetValue("FullName", typeof(string));
			if (text == null)
			{
				throw new ArgumentNullException("fullName");
			}
			this._appId = IsolationInterop.AppIdAuthority.TextToDefinition(0U, text);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0001558A File Offset: 0x0001458A
		public ApplicationIdentity(string applicationIdentityFullName)
		{
			if (applicationIdentityFullName == null)
			{
				throw new ArgumentNullException("applicationIdentityFullName");
			}
			this._appId = IsolationInterop.AppIdAuthority.TextToDefinition(0U, applicationIdentityFullName);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000155B2 File Offset: 0x000145B2
		internal ApplicationIdentity(IDefinitionAppId applicationIdentity)
		{
			this._appId = applicationIdentity;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x000155C1 File Offset: 0x000145C1
		public string FullName
		{
			get
			{
				return IsolationInterop.AppIdAuthority.DefinitionToText(0U, this._appId);
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x000155D4 File Offset: 0x000145D4
		public string CodeBase
		{
			get
			{
				return this._appId.get_Codebase();
			}
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x000155E1 File Offset: 0x000145E1
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000155E9 File Offset: 0x000145E9
		internal IDefinitionAppId Identity
		{
			get
			{
				return this._appId;
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x000155F1 File Offset: 0x000145F1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("FullName", this.FullName, typeof(string));
		}

		// Token: 0x040001F0 RID: 496
		private IDefinitionAppId _appId;
	}
}
