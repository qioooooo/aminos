using System;
using System.Globalization;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Utils;

namespace System.Xml.Xsl
{
	// Token: 0x02000177 RID: 375
	[Serializable]
	public class XsltException : SystemException
	{
		// Token: 0x060013F5 RID: 5109 RVA: 0x0005607C File Offset: 0x0005507C
		protected XsltException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.res = (string)info.GetValue("res", typeof(string));
			this.args = (string[])info.GetValue("args", typeof(string[]));
			this.sourceUri = (string)info.GetValue("sourceUri", typeof(string));
			this.lineNumber = (int)info.GetValue("lineNumber", typeof(int));
			this.linePosition = (int)info.GetValue("linePosition", typeof(int));
			string text = null;
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "version")
				{
					text = (string)serializationEntry.Value;
				}
			}
			if (text == null)
			{
				this.message = XsltException.CreateMessage(this.res, this.args, this.sourceUri, this.lineNumber, this.linePosition);
				return;
			}
			this.message = null;
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x000561A0 File Offset: 0x000551A0
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("res", this.res);
			info.AddValue("args", this.args);
			info.AddValue("sourceUri", this.sourceUri);
			info.AddValue("lineNumber", this.lineNumber);
			info.AddValue("linePosition", this.linePosition);
			info.AddValue("version", "2.0");
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x0005621A File Offset: 0x0005521A
		public XsltException()
			: this(string.Empty, null)
		{
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x00056228 File Offset: 0x00055228
		public XsltException(string message)
			: this(message, null)
		{
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x00056234 File Offset: 0x00055234
		public XsltException(string message, Exception innerException)
			: this("Xml_UserException", new string[] { message }, null, 0, 0, innerException)
		{
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x0005625C File Offset: 0x0005525C
		internal static XsltException Create(string res, params string[] args)
		{
			return new XsltException(res, args, null, 0, 0, null);
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00056269 File Offset: 0x00055269
		internal static XsltException Create(string res, string[] args, Exception inner)
		{
			return new XsltException(res, args, null, 0, 0, inner);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00056276 File Offset: 0x00055276
		internal XsltException(string res, string[] args, string sourceUri, int lineNumber, int linePosition, Exception inner)
			: base(XsltException.CreateMessage(res, args, sourceUri, lineNumber, linePosition), inner)
		{
			base.HResult = -2146231998;
			this.res = res;
			this.sourceUri = sourceUri;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x000562B5 File Offset: 0x000552B5
		public virtual string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x000562BD File Offset: 0x000552BD
		public virtual int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x060013FF RID: 5119 RVA: 0x000562C5 File Offset: 0x000552C5
		public virtual int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06001400 RID: 5120 RVA: 0x000562CD File Offset: 0x000552CD
		public override string Message
		{
			get
			{
				if (this.message != null)
				{
					return this.message;
				}
				return base.Message;
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000562E4 File Offset: 0x000552E4
		private static string CreateMessage(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
		{
			string text2;
			try
			{
				string text = XsltException.FormatMessage(res, args);
				if (res != "Xslt_CompileError" && lineNumber != 0)
				{
					text = text + " " + XsltException.FormatMessage("Xml_ErrorFilePosition", new string[]
					{
						sourceUri,
						lineNumber.ToString(CultureInfo.InvariantCulture),
						linePosition.ToString(CultureInfo.InvariantCulture)
					});
				}
				text2 = text;
			}
			catch (MissingManifestResourceException)
			{
				text2 = "UNKNOWN(" + res + ")";
			}
			return text2;
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00056374 File Offset: 0x00055374
		private static string FormatMessage(string key, params string[] args)
		{
			string text = Res.GetString(key);
			if (text != null && args != null)
			{
				text = string.Format(CultureInfo.InvariantCulture, text, args);
			}
			return text;
		}

		// Token: 0x04000C3D RID: 3133
		private string res;

		// Token: 0x04000C3E RID: 3134
		private string[] args;

		// Token: 0x04000C3F RID: 3135
		private string sourceUri;

		// Token: 0x04000C40 RID: 3136
		private int lineNumber;

		// Token: 0x04000C41 RID: 3137
		private int linePosition;

		// Token: 0x04000C42 RID: 3138
		private string message;
	}
}
