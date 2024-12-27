using System;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Utils;

namespace System.Xml.Xsl
{
	// Token: 0x0200001C RID: 28
	[Serializable]
	internal class XslTransformException : XsltException
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00009485 File Offset: 0x00008485
		protected XslTransformException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000948F File Offset: 0x0000848F
		public XslTransformException(Exception inner, string res, params string[] args)
			: base(XslTransformException.CreateMessage(res, args), inner)
		{
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000949F File Offset: 0x0000849F
		public XslTransformException(string message)
			: base(XslTransformException.CreateMessage(message, null), null)
		{
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000094AF File Offset: 0x000084AF
		internal XslTransformException(string res, params string[] args)
			: this(null, res, args)
		{
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000094BC File Offset: 0x000084BC
		internal static string CreateMessage(string res, params string[] args)
		{
			string text = null;
			try
			{
				text = Res.GetString(res, args);
			}
			catch (MissingManifestResourceException)
			{
			}
			if (text != null)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(res);
			if (args != null && args.Length > 0)
			{
				stringBuilder.Append('(');
				stringBuilder.Append(args[0]);
				for (int i = 1; i < args.Length; i++)
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(args[i]);
				}
				stringBuilder.Append(')');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009544 File Offset: 0x00008544
		internal virtual string FormatDetailedMessage()
		{
			return this.Message;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000954C File Offset: 0x0000854C
		public override string ToString()
		{
			string text = base.GetType().FullName;
			string text2 = this.FormatDetailedMessage();
			if (text2 != null && text2.Length > 0)
			{
				text = text + ": " + text2;
			}
			if (base.InnerException != null)
			{
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					" ---> ",
					base.InnerException.ToString(),
					Environment.NewLine,
					"   ",
					XslTransformException.CreateMessage("Xml_EndOfInnerExceptionStack", new string[0])
				});
			}
			if (this.StackTrace != null)
			{
				text = text + Environment.NewLine + this.StackTrace;
			}
			return text;
		}
	}
}
