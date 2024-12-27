using System;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x02000034 RID: 52
	internal sealed class ComSurrogateSelector : ISurrogateSelector, ISerializationSurrogate
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x00004A76 File Offset: 0x00003A76
		public ComSurrogateSelector()
		{
			this._deleg = new RemotingSurrogateSelector();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004A89 File Offset: 0x00003A89
		public void ChainSelector(ISurrogateSelector next)
		{
			this._deleg.ChainSelector(next);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004A97 File Offset: 0x00003A97
		public ISurrogateSelector GetNextSelector()
		{
			return this._deleg.GetNextSelector();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00004AA4 File Offset: 0x00003AA4
		public ISerializationSurrogate GetSurrogate(Type type, StreamingContext ctx, out ISurrogateSelector selector)
		{
			selector = null;
			if (type.IsCOMObject)
			{
				selector = this;
				return this;
			}
			return this._deleg.GetSurrogate(type, ctx, out selector);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00004AC4 File Offset: 0x00003AC4
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext ctx)
		{
			if (!obj.GetType().IsCOMObject)
			{
				throw new NotSupportedException();
			}
			info.SetType(typeof(ComObjRef));
			info.AddValue("buffer", ComponentServices.GetDCOMBuffer(obj));
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004AFA File Offset: 0x00003AFA
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext ctx, ISurrogateSelector sel)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400006F RID: 111
		private ISurrogateSelector _deleg;
	}
}
