using System;
using System.Globalization;
using System.IO;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000AF RID: 175
	internal class VsaSite : BaseVsaSite
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x000378A0 File Offset: 0x000368A0
		public VsaSite(TextWriter redirectedOutput)
		{
			this.output = redirectedOutput;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000378C4 File Offset: 0x000368C4
		public override bool OnCompilerError(IVsaError error)
		{
			int severity = error.Severity;
			if (severity > this.warningLevel)
			{
				return true;
			}
			bool flag = severity != 0 && !this.treatWarningsAsErrors;
			this.PrintError(error.SourceMoniker, error.Line, error.StartColumn, flag, error.Number, error.Description);
			return true;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0003791C File Offset: 0x0003691C
		private void PrintError(string sourceFile, int line, int column, bool fIsWarning, int number, string message)
		{
			string text = (10000 + (number & 65535)).ToString(CultureInfo.InvariantCulture).Substring(1);
			if (string.Compare(sourceFile, "no source", StringComparison.Ordinal) != 0)
			{
				this.output.Write(string.Concat(new string[]
				{
					sourceFile,
					"(",
					line.ToString(CultureInfo.InvariantCulture),
					",",
					column.ToString(CultureInfo.InvariantCulture),
					") : "
				}));
			}
			this.output.WriteLine((fIsWarning ? "warning JS" : "error JS") + text + ": " + message);
		}

		// Token: 0x04000446 RID: 1094
		public int warningLevel = 4;

		// Token: 0x04000447 RID: 1095
		public bool treatWarningsAsErrors;

		// Token: 0x04000448 RID: 1096
		public TextWriter output = Console.Out;
	}
}
