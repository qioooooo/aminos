using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A3 RID: 163
	[Serializable]
	public class SyncFromAllServersOperationException : ActiveDirectoryOperationException, ISerializable
	{
		// Token: 0x0600055B RID: 1371 RVA: 0x0001E809 File Offset: 0x0001D809
		public SyncFromAllServersOperationException(string message, Exception inner, SyncFromAllServersErrorInformation[] errors)
			: base(message, inner)
		{
			this.errors = errors;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0001E81A File Offset: 0x0001D81A
		public SyncFromAllServersOperationException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0001E824 File Offset: 0x0001D824
		public SyncFromAllServersOperationException(string message)
			: base(message)
		{
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0001E82D File Offset: 0x0001D82D
		public SyncFromAllServersOperationException()
			: base(Res.GetString("DSSyncAllFailure"))
		{
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0001E83F File Offset: 0x0001D83F
		protected SyncFromAllServersOperationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001E84C File Offset: 0x0001D84C
		public SyncFromAllServersErrorInformation[] ErrorInformation
		{
			get
			{
				if (this.errors == null)
				{
					return new SyncFromAllServersErrorInformation[0];
				}
				SyncFromAllServersErrorInformation[] array = new SyncFromAllServersErrorInformation[this.errors.Length];
				for (int i = 0; i < this.errors.Length; i++)
				{
					array[i] = new SyncFromAllServersErrorInformation(this.errors[i].ErrorCategory, this.errors[i].ErrorCode, this.errors[i].ErrorMessage, this.errors[i].SourceServer, this.errors[i].TargetServer);
				}
				return array;
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001E8D3 File Offset: 0x0001D8D3
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0400043B RID: 1083
		private SyncFromAllServersErrorInformation[] errors;
	}
}
