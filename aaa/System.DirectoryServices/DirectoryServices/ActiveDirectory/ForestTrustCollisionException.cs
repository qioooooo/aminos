using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000A4 RID: 164
	[Serializable]
	public class ForestTrustCollisionException : ActiveDirectoryOperationException, ISerializable
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0001E8DD File Offset: 0x0001D8DD
		public ForestTrustCollisionException(string message, Exception inner, ForestTrustRelationshipCollisionCollection collisions)
			: base(message, inner)
		{
			this.collisions = collisions;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001E8F9 File Offset: 0x0001D8F9
		public ForestTrustCollisionException(string message, Exception inner)
			: base(message, inner)
		{
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001E90E File Offset: 0x0001D90E
		public ForestTrustCollisionException(string message)
			: base(message)
		{
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001E922 File Offset: 0x0001D922
		public ForestTrustCollisionException()
			: base(Res.GetString("ForestTrustCollision"))
		{
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001E93F File Offset: 0x0001D93F
		protected ForestTrustCollisionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001E954 File Offset: 0x0001D954
		public ForestTrustRelationshipCollisionCollection Collisions
		{
			get
			{
				return this.collisions;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001E95C File Offset: 0x0001D95C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0400043C RID: 1084
		private ForestTrustRelationshipCollisionCollection collisions = new ForestTrustRelationshipCollisionCollection();
	}
}
