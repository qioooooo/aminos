using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Xml.Xsl.XPath
{
	// Token: 0x020000DA RID: 218
	[Serializable]
	internal class XPathCompileException : XslLoadException
	{
		// Token: 0x06000A18 RID: 2584 RVA: 0x00030898 File Offset: 0x0002F898
		protected XPathCompileException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.queryString = (string)info.GetValue("QueryString", typeof(string));
			this.startChar = (int)info.GetValue("StartChar", typeof(int));
			this.endChar = (int)info.GetValue("EndChar", typeof(int));
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0003090D File Offset: 0x0002F90D
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("QueryString", this.queryString);
			info.AddValue("StartChar", this.startChar);
			info.AddValue("EndChar", this.endChar);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0003094A File Offset: 0x0002F94A
		internal XPathCompileException(string queryString, int startChar, int endChar, string resId, params string[] args)
			: base(resId, args)
		{
			this.queryString = queryString;
			this.startChar = startChar;
			this.endChar = endChar;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0003096B File Offset: 0x0002F96B
		internal XPathCompileException(string resId, params string[] args)
			: base(resId, args)
		{
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00030978 File Offset: 0x0002F978
		private static void AppendTrimmed(StringBuilder sb, string value, int startIndex, int count, XPathCompileException.TrimType trimType)
		{
			if (count <= 32)
			{
				sb.Append(value, startIndex, count);
				return;
			}
			switch (trimType)
			{
			case XPathCompileException.TrimType.Left:
				sb.Append("...");
				sb.Append(value, startIndex + count - 32, 32);
				return;
			case XPathCompileException.TrimType.Right:
				sb.Append(value, startIndex, 32);
				sb.Append("...");
				return;
			case XPathCompileException.TrimType.Middle:
				sb.Append(value, startIndex, 16);
				sb.Append("...");
				sb.Append(value, startIndex + count - 16, 16);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00030A08 File Offset: 0x0002FA08
		internal string MarkOutError()
		{
			if (this.queryString == null || this.queryString.Trim(new char[] { ' ' }).Length == 0)
			{
				return null;
			}
			int num = this.endChar - this.startChar;
			StringBuilder stringBuilder = new StringBuilder();
			XPathCompileException.AppendTrimmed(stringBuilder, this.queryString, 0, this.startChar, XPathCompileException.TrimType.Left);
			if (num > 0)
			{
				stringBuilder.Append(" -->");
				XPathCompileException.AppendTrimmed(stringBuilder, this.queryString, this.startChar, num, XPathCompileException.TrimType.Middle);
			}
			stringBuilder.Append("<-- ");
			XPathCompileException.AppendTrimmed(stringBuilder, this.queryString, this.endChar, this.queryString.Length - this.endChar, XPathCompileException.TrimType.Right);
			return stringBuilder.ToString();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00030AC4 File Offset: 0x0002FAC4
		internal override string FormatDetailedMessage()
		{
			string text = this.Message;
			string text2 = this.MarkOutError();
			if (text2 != null && text2.Length > 0)
			{
				if (text.Length > 0)
				{
					text += Environment.NewLine;
				}
				text += text2;
			}
			return text;
		}

		// Token: 0x0400068C RID: 1676
		public string queryString;

		// Token: 0x0400068D RID: 1677
		public int startChar;

		// Token: 0x0400068E RID: 1678
		public int endChar;

		// Token: 0x020000DB RID: 219
		private enum TrimType
		{
			// Token: 0x04000690 RID: 1680
			Left,
			// Token: 0x04000691 RID: 1681
			Right,
			// Token: 0x04000692 RID: 1682
			Middle
		}
	}
}
