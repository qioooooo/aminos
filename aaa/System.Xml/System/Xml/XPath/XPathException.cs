using System;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.XPath
{
	// Token: 0x02000111 RID: 273
	[Serializable]
	public class XPathException : SystemException
	{
		// Token: 0x060010AD RID: 4269 RVA: 0x0004BEF0 File Offset: 0x0004AEF0
		protected XPathException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.res = (string)info.GetValue("res", typeof(string));
			this.args = (string[])info.GetValue("args", typeof(string[]));
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
				this.message = XPathException.CreateMessage(this.res, this.args);
				return;
			}
			this.message = null;
		}

		// Token: 0x060010AE RID: 4270 RVA: 0x0004BFA1 File Offset: 0x0004AFA1
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("res", this.res);
			info.AddValue("args", this.args);
			info.AddValue("version", "2.0");
		}

		// Token: 0x060010AF RID: 4271 RVA: 0x0004BFDD File Offset: 0x0004AFDD
		public XPathException()
			: this(string.Empty, null)
		{
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0004BFEB File Offset: 0x0004AFEB
		public XPathException(string message)
			: this(message, null)
		{
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x0004BFF8 File Offset: 0x0004AFF8
		public XPathException(string message, Exception innerException)
			: this("Xml_UserException", new string[] { message }, innerException)
		{
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x0004C01D File Offset: 0x0004B01D
		internal static XPathException Create(string res)
		{
			return new XPathException(res, null);
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x0004C028 File Offset: 0x0004B028
		internal static XPathException Create(string res, string arg)
		{
			return new XPathException(res, new string[] { arg });
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x0004C048 File Offset: 0x0004B048
		internal static XPathException Create(string res, string arg, string arg2)
		{
			return new XPathException(res, new string[] { arg, arg2 });
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0004C06C File Offset: 0x0004B06C
		internal static XPathException Create(string res, string arg, Exception innerException)
		{
			return new XPathException(res, new string[] { arg }, innerException);
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x0004C08C File Offset: 0x0004B08C
		private XPathException(string res, string[] args)
			: this(res, args, null)
		{
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0004C097 File Offset: 0x0004B097
		private XPathException(string res, string[] args, Exception inner)
			: base(XPathException.CreateMessage(res, args), inner)
		{
			base.HResult = -2146231997;
			this.res = res;
			this.args = args;
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0004C0C0 File Offset: 0x0004B0C0
		private static string CreateMessage(string res, string[] args)
		{
			string text2;
			try
			{
				string text = Res.GetString(res, args);
				if (text == null)
				{
					text = "UNKNOWN(" + res + ")";
				}
				text2 = text;
			}
			catch (MissingManifestResourceException)
			{
				text2 = "UNKNOWN(" + res + ")";
			}
			return text2;
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x0004C114 File Offset: 0x0004B114
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

		// Token: 0x04000AD6 RID: 2774
		private string res;

		// Token: 0x04000AD7 RID: 2775
		private string[] args;

		// Token: 0x04000AD8 RID: 2776
		private string message;
	}
}
