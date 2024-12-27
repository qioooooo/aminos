using System;
using System.IO;
using System.Resources;

namespace System.Reflection.Emit
{
	// Token: 0x020007EC RID: 2028
	internal class ResWriterData
	{
		// Token: 0x06004866 RID: 18534 RVA: 0x000FD175 File Offset: 0x000FC175
		internal ResWriterData(ResourceWriter resWriter, Stream memoryStream, string strName, string strFileName, string strFullFileName, ResourceAttributes attribute)
		{
			this.m_resWriter = resWriter;
			this.m_memoryStream = memoryStream;
			this.m_strName = strName;
			this.m_strFileName = strFileName;
			this.m_strFullFileName = strFullFileName;
			this.m_nextResWriter = null;
			this.m_attribute = attribute;
		}

		// Token: 0x04002525 RID: 9509
		internal ResourceWriter m_resWriter;

		// Token: 0x04002526 RID: 9510
		internal string m_strName;

		// Token: 0x04002527 RID: 9511
		internal string m_strFileName;

		// Token: 0x04002528 RID: 9512
		internal string m_strFullFileName;

		// Token: 0x04002529 RID: 9513
		internal Stream m_memoryStream;

		// Token: 0x0400252A RID: 9514
		internal ResWriterData m_nextResWriter;

		// Token: 0x0400252B RID: 9515
		internal ResourceAttributes m_attribute;
	}
}
