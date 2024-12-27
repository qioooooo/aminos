using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000356 RID: 854
	internal sealed class SurrogateForCyclicalReference : ISerializationSurrogate
	{
		// Token: 0x0600222E RID: 8750 RVA: 0x00056D4A File Offset: 0x00055D4A
		internal SurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
		{
			if (innerSurrogate == null)
			{
				throw new ArgumentNullException("innerSurrogate");
			}
			this.innerSurrogate = innerSurrogate;
		}

		// Token: 0x0600222F RID: 8751 RVA: 0x00056D67 File Offset: 0x00055D67
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			this.innerSurrogate.GetObjectData(obj, info, context);
		}

		// Token: 0x06002230 RID: 8752 RVA: 0x00056D77 File Offset: 0x00055D77
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			return this.innerSurrogate.SetObjectData(obj, info, context, selector);
		}

		// Token: 0x04000E37 RID: 3639
		private ISerializationSurrogate innerSurrogate;
	}
}
