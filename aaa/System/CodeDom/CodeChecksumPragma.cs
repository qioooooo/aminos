using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000048 RID: 72
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeChecksumPragma : CodeDirective
	{
		// Token: 0x060002D6 RID: 726 RVA: 0x00012D24 File Offset: 0x00011D24
		public CodeChecksumPragma()
		{
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00012D2C File Offset: 0x00011D2C
		public CodeChecksumPragma(string fileName, Guid checksumAlgorithmId, byte[] checksumData)
		{
			this.fileName = fileName;
			this.checksumAlgorithmId = checksumAlgorithmId;
			this.checksumData = checksumData;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00012D49 File Offset: 0x00011D49
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00012D5F File Offset: 0x00011D5F
		public string FileName
		{
			get
			{
				if (this.fileName != null)
				{
					return this.fileName;
				}
				return string.Empty;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00012D68 File Offset: 0x00011D68
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00012D70 File Offset: 0x00011D70
		public Guid ChecksumAlgorithmId
		{
			get
			{
				return this.checksumAlgorithmId;
			}
			set
			{
				this.checksumAlgorithmId = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002DC RID: 732 RVA: 0x00012D79 File Offset: 0x00011D79
		// (set) Token: 0x060002DD RID: 733 RVA: 0x00012D81 File Offset: 0x00011D81
		public byte[] ChecksumData
		{
			get
			{
				return this.checksumData;
			}
			set
			{
				this.checksumData = value;
			}
		}

		// Token: 0x04000802 RID: 2050
		private string fileName;

		// Token: 0x04000803 RID: 2051
		private byte[] checksumData;

		// Token: 0x04000804 RID: 2052
		private Guid checksumAlgorithmId;
	}
}
