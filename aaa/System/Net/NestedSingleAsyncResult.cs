using System;

namespace System.Net
{
	// Token: 0x020004F3 RID: 1267
	internal class NestedSingleAsyncResult : LazyAsyncResult
	{
		// Token: 0x060027A0 RID: 10144 RVA: 0x000A3142 File Offset: 0x000A2142
		internal NestedSingleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, object result)
			: base(asyncObject, asyncState, asyncCallback, result)
		{
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000A314F File Offset: 0x000A214F
		internal NestedSingleAsyncResult(object asyncObject, object asyncState, AsyncCallback asyncCallback, byte[] buffer, int offset, int size)
			: base(asyncObject, asyncState, asyncCallback)
		{
			this.Buffer = buffer;
			this.Offset = offset;
			this.Size = size;
		}

		// Token: 0x040026C0 RID: 9920
		internal byte[] Buffer;

		// Token: 0x040026C1 RID: 9921
		internal int Offset;

		// Token: 0x040026C2 RID: 9922
		internal int Size;
	}
}
