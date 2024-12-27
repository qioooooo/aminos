using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Schema
{
	// Token: 0x020002AB RID: 683
	[Serializable]
	public class XmlSchemaInferenceException : XmlSchemaException
	{
		// Token: 0x060020D1 RID: 8401 RVA: 0x0009B475 File Offset: 0x0009A475
		protected XmlSchemaInferenceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x0009B47F File Offset: 0x0009A47F
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x0009B489 File Offset: 0x0009A489
		public XmlSchemaInferenceException()
			: base(null)
		{
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x0009B492 File Offset: 0x0009A492
		public XmlSchemaInferenceException(string message)
			: base(message, null, 0, 0)
		{
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x0009B49E File Offset: 0x0009A49E
		public XmlSchemaInferenceException(string message, Exception innerException)
			: base(message, innerException, 0, 0)
		{
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x0009B4AA File Offset: 0x0009A4AA
		public XmlSchemaInferenceException(string message, Exception innerException, int lineNumber, int linePosition)
			: base(message, innerException, lineNumber, linePosition)
		{
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x0009B4B7 File Offset: 0x0009A4B7
		internal XmlSchemaInferenceException(string res, string[] args)
			: base(res, args, null, null, 0, 0, null)
		{
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x0009B4C8 File Offset: 0x0009A4C8
		internal XmlSchemaInferenceException(string res, string arg)
			: base(res, new string[] { arg }, null, null, 0, 0, null)
		{
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x0009B4F0 File Offset: 0x0009A4F0
		internal XmlSchemaInferenceException(string res, string arg, string sourceUri, int lineNumber, int linePosition)
			: base(res, new string[] { arg }, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x0009B517 File Offset: 0x0009A517
		internal XmlSchemaInferenceException(string res, string sourceUri, int lineNumber, int linePosition)
			: base(res, null, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x0009B527 File Offset: 0x0009A527
		internal XmlSchemaInferenceException(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
			: base(res, args, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x0009B538 File Offset: 0x0009A538
		internal XmlSchemaInferenceException(string res, int lineNumber, int linePosition)
			: base(res, null, null, null, lineNumber, linePosition, null)
		{
		}
	}
}
