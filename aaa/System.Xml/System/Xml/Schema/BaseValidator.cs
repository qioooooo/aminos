using System;
using System.Collections;
using System.Text;

namespace System.Xml.Schema
{
	// Token: 0x02000182 RID: 386
	internal class BaseValidator
	{
		// Token: 0x06001469 RID: 5225 RVA: 0x0005770C File Offset: 0x0005670C
		public BaseValidator(BaseValidator other)
		{
			this.reader = other.reader;
			this.schemaCollection = other.schemaCollection;
			this.eventHandler = other.eventHandler;
			this.nameTable = other.nameTable;
			this.schemaNames = other.schemaNames;
			this.positionInfo = other.positionInfo;
			this.xmlResolver = other.xmlResolver;
			this.baseUri = other.baseUri;
			this.elementName = other.elementName;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0005778B File Offset: 0x0005678B
		public BaseValidator(XmlValidatingReaderImpl reader, XmlSchemaCollection schemaCollection, ValidationEventHandler eventHandler)
		{
			this.reader = reader;
			this.schemaCollection = schemaCollection;
			this.eventHandler = eventHandler;
			this.nameTable = reader.NameTable;
			this.positionInfo = PositionInfo.GetPositionInfo(reader);
			this.elementName = new XmlQualifiedName();
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x000577CB File Offset: 0x000567CB
		public XmlValidatingReaderImpl Reader
		{
			get
			{
				return this.reader;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x000577D3 File Offset: 0x000567D3
		public XmlSchemaCollection SchemaCollection
		{
			get
			{
				return this.schemaCollection;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600146D RID: 5229 RVA: 0x000577DB File Offset: 0x000567DB
		public XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x0600146E RID: 5230 RVA: 0x000577E4 File Offset: 0x000567E4
		public SchemaNames SchemaNames
		{
			get
			{
				if (this.schemaNames != null)
				{
					return this.schemaNames;
				}
				if (this.schemaCollection != null)
				{
					this.schemaNames = this.schemaCollection.GetSchemaNames(this.nameTable);
				}
				else
				{
					this.schemaNames = new SchemaNames(this.nameTable);
				}
				return this.schemaNames;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x0600146F RID: 5231 RVA: 0x00057838 File Offset: 0x00056838
		public PositionInfo PositionInfo
		{
			get
			{
				return this.positionInfo;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001470 RID: 5232 RVA: 0x00057840 File Offset: 0x00056840
		// (set) Token: 0x06001471 RID: 5233 RVA: 0x00057848 File Offset: 0x00056848
		public XmlResolver XmlResolver
		{
			get
			{
				return this.xmlResolver;
			}
			set
			{
				this.xmlResolver = value;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001472 RID: 5234 RVA: 0x00057851 File Offset: 0x00056851
		// (set) Token: 0x06001473 RID: 5235 RVA: 0x00057859 File Offset: 0x00056859
		public Uri BaseUri
		{
			get
			{
				return this.baseUri;
			}
			set
			{
				this.baseUri = value;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x00057862 File Offset: 0x00056862
		// (set) Token: 0x06001475 RID: 5237 RVA: 0x0005786A File Offset: 0x0005686A
		public ValidationEventHandler EventHandler
		{
			get
			{
				return this.eventHandler;
			}
			set
			{
				this.eventHandler = value;
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x00057873 File Offset: 0x00056873
		// (set) Token: 0x06001477 RID: 5239 RVA: 0x0005787B File Offset: 0x0005687B
		public SchemaInfo SchemaInfo
		{
			get
			{
				return this.schemaInfo;
			}
			set
			{
				this.schemaInfo = value;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x00057884 File Offset: 0x00056884
		public virtual bool PreserveWhitespace
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00057887 File Offset: 0x00056887
		public virtual void Validate()
		{
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x00057889 File Offset: 0x00056889
		public virtual void CompleteValidation()
		{
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x0005788B File Offset: 0x0005688B
		public virtual object FindId(string name)
		{
			return null;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x00057890 File Offset: 0x00056890
		public void ValidateText()
		{
			if (this.context.NeedValidateChildren)
			{
				if (this.context.IsNill)
				{
					this.SendValidationEvent("Sch_ContentInNill", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
					return;
				}
				ContentValidator contentValidator = this.context.ElementDecl.ContentValidator;
				XmlSchemaContentType contentType = contentValidator.ContentType;
				if (contentType == XmlSchemaContentType.ElementOnly)
				{
					ArrayList arrayList = contentValidator.ExpectedElements(this.context, false);
					if (arrayList == null)
					{
						this.SendValidationEvent("Sch_InvalidTextInElement", XmlSchemaValidator.BuildElementName(this.context.LocalName, this.context.Namespace));
					}
					else
					{
						this.SendValidationEvent("Sch_InvalidTextInElementExpecting", new string[]
						{
							XmlSchemaValidator.BuildElementName(this.context.LocalName, this.context.Namespace),
							XmlSchemaValidator.PrintExpectedElements(arrayList, false)
						});
					}
				}
				else if (contentType == XmlSchemaContentType.Empty)
				{
					this.SendValidationEvent("Sch_InvalidTextInEmpty", string.Empty);
				}
				if (this.checkDatatype)
				{
					this.SaveTextValue(this.reader.Value);
				}
			}
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x000579A0 File Offset: 0x000569A0
		public void ValidateWhitespace()
		{
			if (this.context.NeedValidateChildren)
			{
				XmlSchemaContentType contentType = this.context.ElementDecl.ContentValidator.ContentType;
				if (this.context.IsNill)
				{
					this.SendValidationEvent("Sch_ContentInNill", XmlSchemaValidator.QNameString(this.context.LocalName, this.context.Namespace));
				}
				if (contentType == XmlSchemaContentType.Empty)
				{
					this.SendValidationEvent("Sch_InvalidWhitespaceInEmpty", string.Empty);
				}
			}
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x00057A18 File Offset: 0x00056A18
		private void SaveTextValue(string value)
		{
			if (this.textString.Length == 0)
			{
				this.textString = value;
				return;
			}
			if (!this.hasSibling)
			{
				this.textValue.Append(this.textString);
				this.hasSibling = true;
			}
			this.textValue.Append(value);
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00057A68 File Offset: 0x00056A68
		protected void SendValidationEvent(string code)
		{
			this.SendValidationEvent(code, string.Empty);
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00057A76 File Offset: 0x00056A76
		protected void SendValidationEvent(string code, string[] args)
		{
			this.SendValidationEvent(new XmlSchemaException(code, args, this.reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00057AA6 File Offset: 0x00056AA6
		protected void SendValidationEvent(string code, string arg)
		{
			this.SendValidationEvent(new XmlSchemaException(code, arg, this.reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x00057AD8 File Offset: 0x00056AD8
		protected void SendValidationEvent(string code, string arg1, string arg2)
		{
			this.SendValidationEvent(new XmlSchemaException(code, new string[] { arg1, arg2 }, this.reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition));
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00057B22 File Offset: 0x00056B22
		protected void SendValidationEvent(XmlSchemaException e)
		{
			this.SendValidationEvent(e, XmlSeverityType.Error);
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x00057B2C File Offset: 0x00056B2C
		protected void SendValidationEvent(string code, string msg, XmlSeverityType severity)
		{
			this.SendValidationEvent(new XmlSchemaException(code, msg, this.reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x00057B5D File Offset: 0x00056B5D
		protected void SendValidationEvent(string code, string[] args, XmlSeverityType severity)
		{
			this.SendValidationEvent(new XmlSchemaException(code, args, this.reader.BaseURI, this.positionInfo.LineNumber, this.positionInfo.LinePosition), severity);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00057B8E File Offset: 0x00056B8E
		protected void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
		{
			if (this.eventHandler != null)
			{
				this.eventHandler(this.reader, new ValidationEventArgs(e, severity));
				return;
			}
			if (severity == XmlSeverityType.Error)
			{
				throw e;
			}
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x00057BB8 File Offset: 0x00056BB8
		protected static void ProcessEntity(SchemaInfo sinfo, string name, object sender, ValidationEventHandler eventhandler, string baseUri, int lineNumber, int linePosition)
		{
			SchemaEntity schemaEntity = (SchemaEntity)sinfo.GeneralEntities[new XmlQualifiedName(name)];
			XmlSchemaException ex = null;
			if (schemaEntity == null)
			{
				ex = new XmlSchemaException("Sch_UndeclaredEntity", name, baseUri, lineNumber, linePosition);
			}
			else if (schemaEntity.NData.IsEmpty)
			{
				ex = new XmlSchemaException("Sch_UnparsedEntityRef", name, baseUri, lineNumber, linePosition);
			}
			if (ex == null)
			{
				return;
			}
			if (eventhandler != null)
			{
				eventhandler(sender, new ValidationEventArgs(ex));
				return;
			}
			throw ex;
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00057C2C File Offset: 0x00056C2C
		public static BaseValidator CreateInstance(ValidationType valType, XmlValidatingReaderImpl reader, XmlSchemaCollection schemaCollection, ValidationEventHandler eventHandler, bool processIdentityConstraints)
		{
			switch (valType)
			{
			case ValidationType.None:
				return new BaseValidator(reader, schemaCollection, eventHandler);
			case ValidationType.Auto:
				return new AutoValidator(reader, schemaCollection, eventHandler);
			case ValidationType.DTD:
				return new DtdValidator(reader, eventHandler, processIdentityConstraints);
			case ValidationType.XDR:
				return new XdrValidator(reader, schemaCollection, eventHandler);
			case ValidationType.Schema:
				return new XsdValidator(reader, schemaCollection, eventHandler);
			default:
				return null;
			}
		}

		// Token: 0x04000C69 RID: 3177
		private XmlSchemaCollection schemaCollection;

		// Token: 0x04000C6A RID: 3178
		private ValidationEventHandler eventHandler;

		// Token: 0x04000C6B RID: 3179
		private XmlNameTable nameTable;

		// Token: 0x04000C6C RID: 3180
		private SchemaNames schemaNames;

		// Token: 0x04000C6D RID: 3181
		private PositionInfo positionInfo;

		// Token: 0x04000C6E RID: 3182
		private XmlResolver xmlResolver;

		// Token: 0x04000C6F RID: 3183
		private Uri baseUri;

		// Token: 0x04000C70 RID: 3184
		protected SchemaInfo schemaInfo;

		// Token: 0x04000C71 RID: 3185
		protected XmlValidatingReaderImpl reader;

		// Token: 0x04000C72 RID: 3186
		protected XmlQualifiedName elementName;

		// Token: 0x04000C73 RID: 3187
		protected ValidationState context;

		// Token: 0x04000C74 RID: 3188
		protected StringBuilder textValue;

		// Token: 0x04000C75 RID: 3189
		protected string textString;

		// Token: 0x04000C76 RID: 3190
		protected bool hasSibling;

		// Token: 0x04000C77 RID: 3191
		protected bool checkDatatype;
	}
}
