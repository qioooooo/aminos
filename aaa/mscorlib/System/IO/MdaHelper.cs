using System;

namespace System.IO
{
	// Token: 0x020005B8 RID: 1464
	internal sealed class MdaHelper
	{
		// Token: 0x06003735 RID: 14133 RVA: 0x000BAF15 File Offset: 0x000B9F15
		internal MdaHelper(StreamWriter sw, string cs)
		{
			this.streamWriter = sw;
			this.allocatedCallstack = cs;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x000BAF2C File Offset: 0x000B9F2C
		protected override void Finalize()
		{
			try
			{
				if (this.streamWriter.charPos != 0 && this.streamWriter.stream != null && this.streamWriter.stream != Stream.Null)
				{
					string text = ((this.streamWriter.stream is FileStream) ? ((FileStream)this.streamWriter.stream).NameInternal : "<unknown>");
					string text2 = Environment.GetResourceString("IO_StreamWriterBufferedDataLost", new object[]
					{
						this.streamWriter.stream.GetType().FullName,
						text
					});
					if (this.allocatedCallstack != null)
					{
						string text3 = text2;
						text2 = string.Concat(new string[]
						{
							text3,
							Environment.NewLine,
							Environment.GetResourceString("AllocatedFrom"),
							Environment.NewLine,
							this.allocatedCallstack
						});
					}
					Mda.StreamWriterBufferedDataLost(text2);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x04001C86 RID: 7302
		private StreamWriter streamWriter;

		// Token: 0x04001C87 RID: 7303
		private string allocatedCallstack;
	}
}
