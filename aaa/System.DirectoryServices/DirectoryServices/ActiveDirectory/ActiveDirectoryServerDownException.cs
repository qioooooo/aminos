using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A1 RID: 161
	[Serializable]
	public class ActiveDirectoryServerDownException : Exception, ISerializable
	{
		// Token: 0x0600054D RID: 1357 RVA: 0x0001E71D File Offset: 0x0001D71D
		public ActiveDirectoryServerDownException(string message, Exception inner, int errorCode, string name)
			: base(message, inner)
		{
			this.errorCode = errorCode;
			this.name = name;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001E736 File Offset: 0x0001D736
		public ActiveDirectoryServerDownException(string message, int errorCode, string name)
			: base(message)
		{
			this.errorCode = errorCode;
			this.name = name;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001E74D File Offset: 0x0001D74D
		public ActiveDirectoryServerDownException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001E757 File Offset: 0x0001D757
		public ActiveDirectoryServerDownException(string message)
			: base(message)
		{
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001E760 File Offset: 0x0001D760
		public ActiveDirectoryServerDownException()
		{
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001E768 File Offset: 0x0001D768
		protected ActiveDirectoryServerDownException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0001E772 File Offset: 0x0001D772
		public int ErrorCode
		{
			get
			{
				return this.errorCode;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x0001E77A File Offset: 0x0001D77A
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x0001E784 File Offset: 0x0001D784
		public override string Message
		{
			get
			{
				string message = base.Message;
				if (this.name != null && this.name.Length != 0)
				{
					return message + Environment.NewLine + Res.GetString("Name", new object[] { this.name }) + Environment.NewLine;
				}
				return message;
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001E7DA File Offset: 0x0001D7DA
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x04000439 RID: 1081
		private int errorCode;

		// Token: 0x0400043A RID: 1082
		private string name;
	}
}
