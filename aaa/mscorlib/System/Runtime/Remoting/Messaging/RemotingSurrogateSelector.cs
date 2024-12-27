using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000756 RID: 1878
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class RemotingSurrogateSelector : ISurrogateSelector
	{
		// Token: 0x06004385 RID: 17285 RVA: 0x000E7B5F File Offset: 0x000E6B5F
		public RemotingSurrogateSelector()
		{
			this._messageSurrogate = new MessageSurrogate(this);
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x06004387 RID: 17287 RVA: 0x000E7B92 File Offset: 0x000E6B92
		// (set) Token: 0x06004386 RID: 17286 RVA: 0x000E7B89 File Offset: 0x000E6B89
		public MessageSurrogateFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x000E7B9C File Offset: 0x000E6B9C
		public void SetRootObject(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			this._rootObj = obj;
			SoapMessageSurrogate soapMessageSurrogate = this._messageSurrogate as SoapMessageSurrogate;
			if (soapMessageSurrogate != null)
			{
				soapMessageSurrogate.SetRootObject(this._rootObj);
			}
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x000E7BD9 File Offset: 0x000E6BD9
		public object GetRootObject()
		{
			return this._rootObj;
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x000E7BE1 File Offset: 0x000E6BE1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void ChainSelector(ISurrogateSelector selector)
		{
			this._next = selector;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x000E7BEC File Offset: 0x000E6BEC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector ssout)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsMarshalByRef)
			{
				ssout = this;
				return this._remotingSurrogate;
			}
			if (RemotingSurrogateSelector.s_IMethodCallMessageType.IsAssignableFrom(type) || RemotingSurrogateSelector.s_IMethodReturnMessageType.IsAssignableFrom(type))
			{
				ssout = this;
				return this._messageSurrogate;
			}
			if (RemotingSurrogateSelector.s_ObjRefType.IsAssignableFrom(type))
			{
				ssout = this;
				return this._objRefSurrogate;
			}
			if (this._next != null)
			{
				return this._next.GetSurrogate(type, context, out ssout);
			}
			ssout = null;
			return null;
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x000E7C6F File Offset: 0x000E6C6F
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual ISurrogateSelector GetNextSelector()
		{
			return this._next;
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x000E7C77 File Offset: 0x000E6C77
		public virtual void UseSoapFormat()
		{
			this._messageSurrogate = new SoapMessageSurrogate(this);
			((SoapMessageSurrogate)this._messageSurrogate).SetRootObject(this._rootObj);
		}

		// Token: 0x040021A1 RID: 8609
		private static Type s_IMethodCallMessageType = typeof(IMethodCallMessage);

		// Token: 0x040021A2 RID: 8610
		private static Type s_IMethodReturnMessageType = typeof(IMethodReturnMessage);

		// Token: 0x040021A3 RID: 8611
		private static Type s_ObjRefType = typeof(ObjRef);

		// Token: 0x040021A4 RID: 8612
		private object _rootObj;

		// Token: 0x040021A5 RID: 8613
		private ISurrogateSelector _next;

		// Token: 0x040021A6 RID: 8614
		private RemotingSurrogate _remotingSurrogate = new RemotingSurrogate();

		// Token: 0x040021A7 RID: 8615
		private ObjRefSurrogate _objRefSurrogate = new ObjRefSurrogate();

		// Token: 0x040021A8 RID: 8616
		private ISerializationSurrogate _messageSurrogate;

		// Token: 0x040021A9 RID: 8617
		private MessageSurrogateFilter _filter;
	}
}
