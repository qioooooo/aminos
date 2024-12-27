using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020003B6 RID: 950
	[Serializable]
	public abstract class WebResponse : MarshalByRefObject, ISerializable, IDisposable
	{
		// Token: 0x06001DD1 RID: 7633 RVA: 0x00071411 File Offset: 0x00070411
		protected WebResponse()
		{
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00071419 File Offset: 0x00070419
		protected WebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00071421 File Offset: 0x00070421
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x0007142B File Offset: 0x0007042B
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x0007142D File Offset: 0x0007042D
		public virtual void Close()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x00071434 File Offset: 0x00070434
		void IDisposable.Dispose()
		{
			try
			{
				this.Close();
				this.OnDispose();
			}
			catch
			{
			}
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x00071464 File Offset: 0x00070464
		internal virtual void OnDispose()
		{
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x00071466 File Offset: 0x00070466
		public virtual bool IsFromCache
		{
			get
			{
				return this.m_IsFromCache;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (set) Token: 0x06001DD9 RID: 7641 RVA: 0x0007146E File Offset: 0x0007046E
		internal bool InternalSetFromCache
		{
			set
			{
				this.m_IsFromCache = value;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x00071477 File Offset: 0x00070477
		internal virtual bool IsCacheFresh
		{
			get
			{
				return this.m_IsCacheFresh;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (set) Token: 0x06001DDB RID: 7643 RVA: 0x0007147F File Offset: 0x0007047F
		internal bool InternalSetIsCacheFresh
		{
			set
			{
				this.m_IsCacheFresh = value;
			}
		}

		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06001DDC RID: 7644 RVA: 0x00071488 File Offset: 0x00070488
		public virtual bool IsMutuallyAuthenticated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001DDD RID: 7645 RVA: 0x0007148B File Offset: 0x0007048B
		// (set) Token: 0x06001DDE RID: 7646 RVA: 0x00071492 File Offset: 0x00070492
		public virtual long ContentLength
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001DDF RID: 7647 RVA: 0x00071499 File Offset: 0x00070499
		// (set) Token: 0x06001DE0 RID: 7648 RVA: 0x000714A0 File Offset: 0x000704A0
		public virtual string ContentType
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000714A7 File Offset: 0x000704A7
		public virtual Stream GetResponseStream()
		{
			throw ExceptionHelper.MethodNotImplementedException;
		}

		// Token: 0x170005DA RID: 1498
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x000714AE File Offset: 0x000704AE
		public virtual Uri ResponseUri
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x170005DB RID: 1499
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x000714B5 File Offset: 0x000704B5
		public virtual WebHeaderCollection Headers
		{
			get
			{
				throw ExceptionHelper.PropertyNotImplementedException;
			}
		}

		// Token: 0x04001DA4 RID: 7588
		private bool m_IsCacheFresh;

		// Token: 0x04001DA5 RID: 7589
		private bool m_IsFromCache;
	}
}
