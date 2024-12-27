using System;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200004B RID: 75
	internal abstract class SmiStream
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002E2 RID: 738
		public abstract bool CanRead { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060002E3 RID: 739
		public abstract bool CanSeek { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002E4 RID: 740
		public abstract bool CanWrite { get; }

		// Token: 0x060002E5 RID: 741
		public abstract long GetLength(SmiEventSink sink);

		// Token: 0x060002E6 RID: 742
		public abstract long GetPosition(SmiEventSink sink);

		// Token: 0x060002E7 RID: 743
		public abstract void SetPosition(SmiEventSink sink, long position);

		// Token: 0x060002E8 RID: 744
		public abstract void Flush(SmiEventSink sink);

		// Token: 0x060002E9 RID: 745
		public abstract long Seek(SmiEventSink sink, long offset, SeekOrigin origin);

		// Token: 0x060002EA RID: 746
		public abstract void SetLength(SmiEventSink sink, long value);

		// Token: 0x060002EB RID: 747
		public abstract int Read(SmiEventSink sink, byte[] buffer, int offset, int count);

		// Token: 0x060002EC RID: 748
		public abstract void Write(SmiEventSink sink, byte[] buffer, int offset, int count);

		// Token: 0x060002ED RID: 749
		public abstract int Read(SmiEventSink sink, char[] buffer, int offset, int count);

		// Token: 0x060002EE RID: 750
		public abstract void Write(SmiEventSink sink, char[] buffer, int offset, int count);
	}
}
