using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Xsl
{
	// Token: 0x0200001D RID: 29
	[Serializable]
	internal class XslLoadException : XslTransformException
	{
		// Token: 0x0600012C RID: 300 RVA: 0x000095F8 File Offset: 0x000085F8
		protected XslLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			bool flag = (bool)info.GetValue("hasLineInfo", typeof(bool));
			if (flag)
			{
				string text = (string)info.GetValue("Uri", typeof(string));
				int num = (int)info.GetValue("StartLine", typeof(int));
				int num2 = (int)info.GetValue("StartPos", typeof(int));
				int num3 = (int)info.GetValue("EndLine", typeof(int));
				int num4 = (int)info.GetValue("EndPos", typeof(int));
				this.lineInfo = new SourceLineInfo(text, num, num2, num3, num4);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000096CC File Offset: 0x000086CC
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("hasLineInfo", this.lineInfo != null);
			if (this.lineInfo != null)
			{
				info.AddValue("Uri", this.lineInfo.Uri);
				info.AddValue("StartLine", this.lineInfo.StartLine);
				info.AddValue("StartPos", this.lineInfo.StartPos);
				info.AddValue("EndLine", this.lineInfo.EndLine);
				info.AddValue("EndPos", this.lineInfo.EndPos);
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000976E File Offset: 0x0000876E
		internal XslLoadException(string res, params string[] args)
			: base(null, res, args)
		{
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009779 File Offset: 0x00008779
		internal XslLoadException(Exception inner, ISourceLineInfo lineInfo)
			: base(inner, "Xslt_CompileError2", null)
		{
			this.lineInfo = lineInfo;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00009790 File Offset: 0x00008790
		internal XslLoadException(CompilerError error)
			: base("Xml_UserException", new string[] { error.ErrorText })
		{
			this.SetSourceLineInfo(new SourceLineInfo(error.FileName, error.Line, error.Column, error.Line, error.Column));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000097E2 File Offset: 0x000087E2
		internal void SetSourceLineInfo(ISourceLineInfo lineInfo)
		{
			this.lineInfo = lineInfo;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000097EB File Offset: 0x000087EB
		public override string SourceUri
		{
			get
			{
				if (this.lineInfo == null)
				{
					return null;
				}
				return this.lineInfo.Uri;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00009802 File Offset: 0x00008802
		public override int LineNumber
		{
			get
			{
				if (this.lineInfo == null)
				{
					return 0;
				}
				return this.lineInfo.StartLine;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00009819 File Offset: 0x00008819
		public override int LinePosition
		{
			get
			{
				if (this.lineInfo == null)
				{
					return 0;
				}
				return this.lineInfo.StartPos;
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00009830 File Offset: 0x00008830
		private static string AppendLineInfoMessage(string message, ISourceLineInfo lineInfo)
		{
			if (lineInfo != null)
			{
				string fileName = SourceLineInfo.GetFileName(lineInfo.Uri);
				string text = XslTransformException.CreateMessage("Xml_ErrorFilePosition", new string[]
				{
					fileName,
					lineInfo.StartLine.ToString(CultureInfo.InvariantCulture),
					lineInfo.StartPos.ToString(CultureInfo.InvariantCulture)
				});
				if (text != null && text.Length > 0)
				{
					if (message.Length > 0 && !XmlCharType.Instance.IsWhiteSpace(message[message.Length - 1]))
					{
						message += " ";
					}
					message += text;
				}
			}
			return message;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000098DD File Offset: 0x000088DD
		internal static string CreateMessage(ISourceLineInfo lineInfo, string res, params string[] args)
		{
			return XslLoadException.AppendLineInfoMessage(XslTransformException.CreateMessage(res, args), lineInfo);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000098EC File Offset: 0x000088EC
		internal override string FormatDetailedMessage()
		{
			return XslLoadException.AppendLineInfoMessage(this.Message, this.lineInfo);
		}

		// Token: 0x04000149 RID: 329
		private ISourceLineInfo lineInfo;
	}
}
