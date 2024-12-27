using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200009F RID: 159
	[Serializable]
	public class ActiveDirectoryObjectNotFoundException : Exception, ISerializable
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x0001E665 File Offset: 0x0001D665
		public ActiveDirectoryObjectNotFoundException(string message, Type type, string name)
			: base(message)
		{
			this.objectType = type;
			this.name = name;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001E67C File Offset: 0x0001D67C
		public ActiveDirectoryObjectNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001E686 File Offset: 0x0001D686
		public ActiveDirectoryObjectNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001E68F File Offset: 0x0001D68F
		public ActiveDirectoryObjectNotFoundException()
		{
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001E697 File Offset: 0x0001D697
		protected ActiveDirectoryObjectNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001E6A1 File Offset: 0x0001D6A1
		public Type Type
		{
			get
			{
				return this.objectType;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x0001E6A9 File Offset: 0x0001D6A9
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001E6B1 File Offset: 0x0001D6B1
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x04000436 RID: 1078
		private Type objectType;

		// Token: 0x04000437 RID: 1079
		private string name;
	}
}
