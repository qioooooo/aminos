using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.EnterpriseServices
{
	// Token: 0x02000035 RID: 53
	[Serializable]
	internal sealed class ComObjRef : IObjectReference, ISerializable
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00004B04 File Offset: 0x00003B04
		public ComObjRef(SerializationInfo info, StreamingContext ctx)
		{
			byte[] array = null;
			IntPtr intPtr = IntPtr.Zero;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("buffer"))
				{
					array = (byte[])enumerator.Value;
				}
			}
			try
			{
				intPtr = Proxy.UnmarshalObject(array);
				this._realobj = Marshal.GetObjectForIUnknown(intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.Release(intPtr);
				}
			}
			if (this._realobj == null)
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004B98 File Offset: 0x00003B98
		public object GetRealObject(StreamingContext ctx)
		{
			return this._realobj;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004BA0 File Offset: 0x00003BA0
		public void GetObjectData(SerializationInfo info, StreamingContext ctx)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000070 RID: 112
		private object _realobj;
	}
}
