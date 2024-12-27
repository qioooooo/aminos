using System;
using System.IO;

namespace System.Windows.Forms
{
	// Token: 0x02000440 RID: 1088
	public interface IFileReaderService
	{
		// Token: 0x06004156 RID: 16726
		Stream OpenFileFromSource(string relativePath);
	}
}
