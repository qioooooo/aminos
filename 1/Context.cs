using System;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000050 RID: 80
	public class Context
	{
		// Token: 0x060003BB RID: 955 RVA: 0x00017F2C File Offset: 0x00016F2C
		internal Context(DocumentContext document, string source_string)
		{
			this.document = document;
			this.source_string = source_string;
			this.lineNumber = 1;
			this.startLinePos = 0;
			this.startPos = 0;
			this.endLineNumber = 1;
			this.endLinePos = 0;
			this.endPos = ((source_string == null) ? (-1) : source_string.Length);
			this.token = JSToken.None;
			this.errorReported = 1000000;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00017F94 File Offset: 0x00016F94
		internal Context(DocumentContext document, string source_string, int lineNumber, int startLinePos, int startPos, int endLineNumber, int endLinePos, int endPos, JSToken token)
		{
			this.document = document;
			this.source_string = source_string;
			this.lineNumber = lineNumber;
			this.startLinePos = startLinePos;
			this.startPos = startPos;
			this.endLineNumber = endLineNumber;
			this.endLinePos = endLinePos;
			this.endPos = endPos;
			this.token = token;
			this.errorReported = 1000000;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00017FF8 File Offset: 0x00016FF8
		internal Context Clone()
		{
			return new Context(this.document, this.source_string, this.lineNumber, this.startLinePos, this.startPos, this.endLineNumber, this.endLinePos, this.endPos, this.token)
			{
				errorReported = this.errorReported
			};
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00018050 File Offset: 0x00017050
		internal Context CombineWith(Context other)
		{
			return new Context(this.document, this.source_string, this.lineNumber, this.startLinePos, this.startPos, other.endLineNumber, other.endLinePos, other.endPos, this.token);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00018098 File Offset: 0x00017098
		internal void EmitLineInfo(ILGenerator ilgen)
		{
			this.document.EmitLineInfo(ilgen, this.StartLine, this.StartColumn, this.EndLine, this.EndColumn);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x000180BE File Offset: 0x000170BE
		internal void EmitFirstLineInfo(ILGenerator ilgen)
		{
			this.document.EmitFirstLineInfo(ilgen, this.StartLine, this.StartColumn, this.EndLine, this.EndColumn);
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x000180E4 File Offset: 0x000170E4
		public int EndColumn
		{
			get
			{
				return this.endPos - this.endLinePos;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x000180F3 File Offset: 0x000170F3
		public int EndLine
		{
			get
			{
				return this.endLineNumber;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x000180FB File Offset: 0x000170FB
		public int EndPosition
		{
			get
			{
				return this.endPos;
			}
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00018104 File Offset: 0x00017104
		internal bool Equals(string str)
		{
			int num = this.endPos - this.startPos;
			return num == str.Length && string.CompareOrdinal(this.source_string, this.startPos, str, 0, num) == 0;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00018144 File Offset: 0x00017144
		internal bool Equals(Context ctx)
		{
			return this.source_string == ctx.source_string && this.lineNumber == ctx.lineNumber && this.startLinePos == ctx.startLinePos && this.startPos == ctx.startPos && this.endLineNumber == ctx.endLineNumber && this.endLinePos == ctx.endLinePos && this.endPos == ctx.endPos && this.token == ctx.token;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000181C8 File Offset: 0x000171C8
		public string GetCode()
		{
			if (this.endPos > this.startPos && this.endPos <= this.source_string.Length)
			{
				return this.source_string.Substring(this.startPos, this.endPos - this.startPos);
			}
			return null;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00018216 File Offset: 0x00017216
		public JSToken GetToken()
		{
			return this.token;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001821E File Offset: 0x0001721E
		internal void HandleError(JSError errorId)
		{
			this.HandleError(errorId, null, false);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00018229 File Offset: 0x00017229
		internal void HandleError(JSError errorId, bool treatAsError)
		{
			this.HandleError(errorId, null, treatAsError);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00018234 File Offset: 0x00017234
		internal void HandleError(JSError errorId, string message)
		{
			this.HandleError(errorId, message, false);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00018240 File Offset: 0x00017240
		internal void HandleError(JSError errorId, string message, bool treatAsError)
		{
			if (errorId == JSError.UndeclaredVariable && this.document.HasAlreadySeenErrorFor(this.GetCode()))
			{
				return;
			}
			JScriptException ex = new JScriptException(errorId, this);
			if (message != null)
			{
				ex.value = message;
			}
			if (treatAsError)
			{
				ex.isError = treatAsError;
			}
			int severity = ex.Severity;
			if (severity < this.errorReported)
			{
				this.document.HandleError(ex);
				this.errorReported = severity;
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000182A8 File Offset: 0x000172A8
		internal void SetSourceContext(DocumentContext document, string source)
		{
			this.source_string = source;
			this.endPos = source.Length;
			this.document = document;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060003CD RID: 973 RVA: 0x000182C4 File Offset: 0x000172C4
		public int StartColumn
		{
			get
			{
				return this.startPos - this.startLinePos;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000182D3 File Offset: 0x000172D3
		public int StartLine
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060003CF RID: 975 RVA: 0x000182DB File Offset: 0x000172DB
		public int StartPosition
		{
			get
			{
				return this.startPos;
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000182E3 File Offset: 0x000172E3
		internal void UpdateWith(Context other)
		{
			this.endPos = other.endPos;
			this.endLineNumber = other.endLineNumber;
			this.endLinePos = other.endLinePos;
		}

		// Token: 0x040001E2 RID: 482
		internal DocumentContext document;

		// Token: 0x040001E3 RID: 483
		internal string source_string;

		// Token: 0x040001E4 RID: 484
		internal int lineNumber;

		// Token: 0x040001E5 RID: 485
		internal int startLinePos;

		// Token: 0x040001E6 RID: 486
		internal int startPos;

		// Token: 0x040001E7 RID: 487
		internal int endLineNumber;

		// Token: 0x040001E8 RID: 488
		internal int endLinePos;

		// Token: 0x040001E9 RID: 489
		internal int endPos;

		// Token: 0x040001EA RID: 490
		internal JSToken token;

		// Token: 0x040001EB RID: 491
		internal int errorReported;
	}
}
