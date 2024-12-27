using System;
using System.Diagnostics;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D2 RID: 2002
	internal sealed class MessageEnd : IStreamable
	{
		// Token: 0x0600473F RID: 18239 RVA: 0x000F50BD File Offset: 0x000F40BD
		internal MessageEnd()
		{
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x000F50C5 File Offset: 0x000F40C5
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(11);
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x000F50CF File Offset: 0x000F40CF
		public void Read(__BinaryParser input)
		{
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x000F50D1 File Offset: 0x000F40D1
		public void Dump()
		{
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x000F50D3 File Offset: 0x000F40D3
		public void Dump(Stream sout)
		{
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x000F50D5 File Offset: 0x000F40D5
		[Conditional("_LOGGING")]
		private void DumpInternal(Stream sout)
		{
			if (BCLDebug.CheckEnabled("BINARY") && sout != null && sout.CanSeek)
			{
				long length = sout.Length;
			}
		}
	}
}
