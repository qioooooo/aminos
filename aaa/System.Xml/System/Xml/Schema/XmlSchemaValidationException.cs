using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Schema
{
	// Token: 0x02000280 RID: 640
	[Serializable]
	public class XmlSchemaValidationException : XmlSchemaException
	{
		// Token: 0x06001D63 RID: 7523 RVA: 0x00085F8C File Offset: 0x00084F8C
		protected XmlSchemaValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x00085F96 File Offset: 0x00084F96
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x06001D65 RID: 7525 RVA: 0x00085FA0 File Offset: 0x00084FA0
		public XmlSchemaValidationException()
			: base(null)
		{
		}

		// Token: 0x06001D66 RID: 7526 RVA: 0x00085FA9 File Offset: 0x00084FA9
		public XmlSchemaValidationException(string message)
			: base(message, null, 0, 0)
		{
		}

		// Token: 0x06001D67 RID: 7527 RVA: 0x00085FB5 File Offset: 0x00084FB5
		public XmlSchemaValidationException(string message, Exception innerException)
			: base(message, innerException, 0, 0)
		{
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x00085FC1 File Offset: 0x00084FC1
		public XmlSchemaValidationException(string message, Exception innerException, int lineNumber, int linePosition)
			: base(message, innerException, lineNumber, linePosition)
		{
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x00085FCE File Offset: 0x00084FCE
		internal XmlSchemaValidationException(string res, string[] args)
			: base(res, args, null, null, 0, 0, null)
		{
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x00085FE0 File Offset: 0x00084FE0
		internal XmlSchemaValidationException(string res, string arg)
			: base(res, new string[] { arg }, null, null, 0, 0, null)
		{
		}

		// Token: 0x06001D6B RID: 7531 RVA: 0x00086008 File Offset: 0x00085008
		internal XmlSchemaValidationException(string res, string arg, string sourceUri, int lineNumber, int linePosition)
			: base(res, new string[] { arg }, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001D6C RID: 7532 RVA: 0x0008602F File Offset: 0x0008502F
		internal XmlSchemaValidationException(string res, string sourceUri, int lineNumber, int linePosition)
			: base(res, null, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x0008603F File Offset: 0x0008503F
		internal XmlSchemaValidationException(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
			: base(res, args, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x00086050 File Offset: 0x00085050
		internal XmlSchemaValidationException(string res, string[] args, Exception innerException, string sourceUri, int lineNumber, int linePosition)
			: base(res, args, innerException, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x00086062 File Offset: 0x00085062
		internal XmlSchemaValidationException(string res, string[] args, object sourceNode)
			: base(res, args, null, null, 0, 0, null)
		{
			this.sourceNodeObject = sourceNode;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x00086078 File Offset: 0x00085078
		internal XmlSchemaValidationException(string res, string[] args, string sourceUri, object sourceNode)
			: base(res, args, null, sourceUri, 0, 0, null)
		{
			this.sourceNodeObject = sourceNode;
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x0008608F File Offset: 0x0008508F
		internal XmlSchemaValidationException(string res, string[] args, string sourceUri, int lineNumber, int linePosition, XmlSchemaObject source, object sourceNode)
			: base(res, args, null, sourceUri, lineNumber, linePosition, source)
		{
			this.sourceNodeObject = sourceNode;
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x000860A9 File Offset: 0x000850A9
		public object SourceObject
		{
			get
			{
				return this.sourceNodeObject;
			}
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000860B1 File Offset: 0x000850B1
		protected internal void SetSourceObject(object sourceObject)
		{
			this.sourceNodeObject = sourceObject;
		}

		// Token: 0x040011EA RID: 4586
		private object sourceNodeObject;
	}
}
