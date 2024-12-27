using System;
using System.Resources;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Schema
{
	// Token: 0x0200024B RID: 587
	[Serializable]
	public class XmlSchemaException : SystemException
	{
		// Token: 0x06001C15 RID: 7189 RVA: 0x00082B44 File Offset: 0x00081B44
		protected XmlSchemaException(SerializationInfo info, StreamingContext context)
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
				this.message = XmlSchemaException.CreateMessage(this.res, this.args);
				return;
			}
			this.message = null;
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x00082C58 File Offset: 0x00081C58
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

		// Token: 0x06001C17 RID: 7191 RVA: 0x00082CD2 File Offset: 0x00081CD2
		public XmlSchemaException()
			: this(null)
		{
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x00082CDB File Offset: 0x00081CDB
		public XmlSchemaException(string message)
			: this(message, null, 0, 0)
		{
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x00082CE7 File Offset: 0x00081CE7
		public XmlSchemaException(string message, Exception innerException)
			: this(message, innerException, 0, 0)
		{
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x00082CF4 File Offset: 0x00081CF4
		public XmlSchemaException(string message, Exception innerException, int lineNumber, int linePosition)
			: this((message == null) ? "Sch_DefaultException" : "Xml_UserException", new string[] { message }, innerException, null, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x00082D28 File Offset: 0x00081D28
		internal XmlSchemaException(string res, string[] args)
			: this(res, args, null, null, 0, 0, null)
		{
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x00082D38 File Offset: 0x00081D38
		internal XmlSchemaException(string res, string arg)
			: this(res, new string[] { arg }, null, null, 0, 0, null)
		{
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x00082D60 File Offset: 0x00081D60
		internal XmlSchemaException(string res, string arg, string sourceUri, int lineNumber, int linePosition)
			: this(res, new string[] { arg }, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00082D87 File Offset: 0x00081D87
		internal XmlSchemaException(string res, string sourceUri, int lineNumber, int linePosition)
			: this(res, null, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x00082D97 File Offset: 0x00081D97
		internal XmlSchemaException(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
			: this(res, args, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x00082DA8 File Offset: 0x00081DA8
		internal XmlSchemaException(string res, XmlSchemaObject source)
			: this(res, null, source)
		{
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x00082DB4 File Offset: 0x00081DB4
		internal XmlSchemaException(string res, string arg, XmlSchemaObject source)
			: this(res, new string[] { arg }, source)
		{
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x00082DD5 File Offset: 0x00081DD5
		internal XmlSchemaException(string res, string[] args, XmlSchemaObject source)
			: this(res, args, null, source.SourceUri, source.LineNumber, source.LinePosition, source)
		{
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x00082DF4 File Offset: 0x00081DF4
		internal XmlSchemaException(string res, string[] args, Exception innerException, string sourceUri, int lineNumber, int linePosition, XmlSchemaObject source)
			: base(XmlSchemaException.CreateMessage(res, args), innerException)
		{
			base.HResult = -2146231999;
			this.res = res;
			this.args = args;
			this.sourceUri = sourceUri;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
			this.sourceSchemaObject = source;
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x00082E48 File Offset: 0x00081E48
		internal static string CreateMessage(string res, string[] args)
		{
			string text;
			try
			{
				text = Res.GetString(res, args);
			}
			catch (MissingManifestResourceException)
			{
				text = "UNKNOWN(" + res + ")";
			}
			return text;
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06001C25 RID: 7205 RVA: 0x00082E84 File Offset: 0x00081E84
		internal string GetRes
		{
			get
			{
				return this.res;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06001C26 RID: 7206 RVA: 0x00082E8C File Offset: 0x00081E8C
		internal string[] Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x06001C27 RID: 7207 RVA: 0x00082E94 File Offset: 0x00081E94
		public string SourceUri
		{
			get
			{
				return this.sourceUri;
			}
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06001C28 RID: 7208 RVA: 0x00082E9C File Offset: 0x00081E9C
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x00082EA4 File Offset: 0x00081EA4
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x00082EAC File Offset: 0x00081EAC
		public XmlSchemaObject SourceSchemaObject
		{
			get
			{
				return this.sourceSchemaObject;
			}
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x00082EB4 File Offset: 0x00081EB4
		internal void SetSource(string sourceUri, int lineNumber, int linePosition)
		{
			this.sourceUri = sourceUri;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x00082ECB File Offset: 0x00081ECB
		internal void SetSchemaObject(XmlSchemaObject source)
		{
			this.sourceSchemaObject = source;
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x00082ED4 File Offset: 0x00081ED4
		internal void SetSource(XmlSchemaObject source)
		{
			this.sourceSchemaObject = source;
			this.sourceUri = source.SourceUri;
			this.lineNumber = source.LineNumber;
			this.linePosition = source.LinePosition;
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00082F01 File Offset: 0x00081F01
		internal void SetResourceId(string resourceId)
		{
			this.res = resourceId;
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x06001C2F RID: 7215 RVA: 0x00082F0A File Offset: 0x00081F0A
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

		// Token: 0x04001160 RID: 4448
		private string res;

		// Token: 0x04001161 RID: 4449
		private string[] args;

		// Token: 0x04001162 RID: 4450
		private string sourceUri;

		// Token: 0x04001163 RID: 4451
		private int lineNumber;

		// Token: 0x04001164 RID: 4452
		private int linePosition;

		// Token: 0x04001165 RID: 4453
		[NonSerialized]
		private XmlSchemaObject sourceSchemaObject;

		// Token: 0x04001166 RID: 4454
		private string message;
	}
}
