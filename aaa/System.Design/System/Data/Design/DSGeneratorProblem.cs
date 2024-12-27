using System;

namespace System.Data.Design
{
	// Token: 0x020000A1 RID: 161
	internal sealed class DSGeneratorProblem
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0000F8E2 File Offset: 0x0000E8E2
		internal string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0000F8EA File Offset: 0x0000E8EA
		internal ProblemSeverity Severity
		{
			get
			{
				return this.severity;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0000F8F2 File Offset: 0x0000E8F2
		internal DataSourceComponent ProblemSource
		{
			get
			{
				return this.problemSource;
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0000F8FA File Offset: 0x0000E8FA
		internal DSGeneratorProblem(string message, ProblemSeverity severity, DataSourceComponent problemSource)
		{
			this.message = message;
			this.severity = severity;
			this.problemSource = problemSource;
		}

		// Token: 0x04000B91 RID: 2961
		private string message;

		// Token: 0x04000B92 RID: 2962
		private ProblemSeverity severity;

		// Token: 0x04000B93 RID: 2963
		private DataSourceComponent problemSource;
	}
}
