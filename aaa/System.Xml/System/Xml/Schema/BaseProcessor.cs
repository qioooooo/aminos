using System;

namespace System.Xml.Schema
{
	// Token: 0x02000184 RID: 388
	internal class BaseProcessor
	{
		// Token: 0x0600148F RID: 5263 RVA: 0x00057ED4 File Offset: 0x00056ED4
		public BaseProcessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler)
			: this(nameTable, schemaNames, eventHandler, new XmlSchemaCompilationSettings())
		{
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00057EE4 File Offset: 0x00056EE4
		public BaseProcessor(XmlNameTable nameTable, SchemaNames schemaNames, ValidationEventHandler eventHandler, XmlSchemaCompilationSettings compilationSettings)
		{
			this.nameTable = nameTable;
			this.schemaNames = schemaNames;
			this.eventHandler = eventHandler;
			this.compilationSettings = compilationSettings;
			this.NsXml = nameTable.Add("http://www.w3.org/XML/1998/namespace");
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001491 RID: 5265 RVA: 0x00057F1A File Offset: 0x00056F1A
		protected XmlNameTable NameTable
		{
			get
			{
				return this.nameTable;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x00057F22 File Offset: 0x00056F22
		protected SchemaNames SchemaNames
		{
			get
			{
				if (this.schemaNames == null)
				{
					this.schemaNames = new SchemaNames(this.nameTable);
				}
				return this.schemaNames;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x00057F43 File Offset: 0x00056F43
		protected ValidationEventHandler EventHandler
		{
			get
			{
				return this.eventHandler;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x00057F4B File Offset: 0x00056F4B
		protected XmlSchemaCompilationSettings CompilationSettings
		{
			get
			{
				return this.compilationSettings;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x00057F53 File Offset: 0x00056F53
		protected bool HasErrors
		{
			get
			{
				return this.errorCount != 0;
			}
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x00057F64 File Offset: 0x00056F64
		protected void AddToTable(XmlSchemaObjectTable table, XmlQualifiedName qname, XmlSchemaObject item)
		{
			if (qname.Name.Length == 0)
			{
				return;
			}
			XmlSchemaObject xmlSchemaObject = table[qname];
			if (xmlSchemaObject == null)
			{
				table.Add(qname, item);
				return;
			}
			if (xmlSchemaObject == item)
			{
				return;
			}
			string text = "Sch_DupGlobalElement";
			if (item is XmlSchemaAttributeGroup)
			{
				string text2 = this.nameTable.Add(qname.Namespace);
				if (Ref.Equal(text2, this.NsXml))
				{
					XmlSchema buildInSchema = Preprocessor.GetBuildInSchema();
					XmlSchemaObject xmlSchemaObject2 = buildInSchema.AttributeGroups[qname];
					if (xmlSchemaObject == xmlSchemaObject2)
					{
						table.Insert(qname, item);
						return;
					}
					if (item == xmlSchemaObject2)
					{
						return;
					}
				}
				else if (this.IsValidAttributeGroupRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				text = "Sch_DupAttributeGroup";
			}
			else if (item is XmlSchemaAttribute)
			{
				string text3 = this.nameTable.Add(qname.Namespace);
				if (Ref.Equal(text3, this.NsXml))
				{
					XmlSchema buildInSchema2 = Preprocessor.GetBuildInSchema();
					XmlSchemaObject xmlSchemaObject3 = buildInSchema2.Attributes[qname];
					if (xmlSchemaObject == xmlSchemaObject3)
					{
						table.Insert(qname, item);
						return;
					}
					if (item == xmlSchemaObject3)
					{
						return;
					}
				}
				text = "Sch_DupGlobalAttribute";
			}
			else if (item is XmlSchemaSimpleType)
			{
				if (this.IsValidTypeRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				text = "Sch_DupSimpleType";
			}
			else if (item is XmlSchemaComplexType)
			{
				if (this.IsValidTypeRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				text = "Sch_DupComplexType";
			}
			else if (item is XmlSchemaGroup)
			{
				if (this.IsValidGroupRedefine(xmlSchemaObject, item, table))
				{
					return;
				}
				text = "Sch_DupGroup";
			}
			else if (item is XmlSchemaNotation)
			{
				text = "Sch_DupNotation";
			}
			else if (item is XmlSchemaIdentityConstraint)
			{
				text = "Sch_DupIdentityConstraint";
			}
			this.SendValidationEvent(text, qname.ToString(), item);
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x000580E4 File Offset: 0x000570E4
		private bool IsValidAttributeGroupRedefine(XmlSchemaObject existingObject, XmlSchemaObject item, XmlSchemaObjectTable table)
		{
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup = item as XmlSchemaAttributeGroup;
			XmlSchemaAttributeGroup xmlSchemaAttributeGroup2 = existingObject as XmlSchemaAttributeGroup;
			if (xmlSchemaAttributeGroup2 == xmlSchemaAttributeGroup.Redefined)
			{
				if (xmlSchemaAttributeGroup2.AttributeUses.Count == 0)
				{
					table.Insert(xmlSchemaAttributeGroup.QualifiedName, xmlSchemaAttributeGroup);
					return true;
				}
			}
			else if (xmlSchemaAttributeGroup2.Redefined == xmlSchemaAttributeGroup)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x00058130 File Offset: 0x00057130
		private bool IsValidGroupRedefine(XmlSchemaObject existingObject, XmlSchemaObject item, XmlSchemaObjectTable table)
		{
			XmlSchemaGroup xmlSchemaGroup = item as XmlSchemaGroup;
			XmlSchemaGroup xmlSchemaGroup2 = existingObject as XmlSchemaGroup;
			if (xmlSchemaGroup2 == xmlSchemaGroup.Redefined)
			{
				if (xmlSchemaGroup2.CanonicalParticle == null)
				{
					table.Insert(xmlSchemaGroup.QualifiedName, xmlSchemaGroup);
					return true;
				}
			}
			else if (xmlSchemaGroup2.Redefined == xmlSchemaGroup)
			{
				return true;
			}
			return false;
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00058178 File Offset: 0x00057178
		private bool IsValidTypeRedefine(XmlSchemaObject existingObject, XmlSchemaObject item, XmlSchemaObjectTable table)
		{
			XmlSchemaType xmlSchemaType = item as XmlSchemaType;
			XmlSchemaType xmlSchemaType2 = existingObject as XmlSchemaType;
			if (xmlSchemaType2 == xmlSchemaType.Redefined)
			{
				if (xmlSchemaType2.ElementDecl == null)
				{
					table.Insert(xmlSchemaType.QualifiedName, xmlSchemaType);
					return true;
				}
			}
			else if (xmlSchemaType2.Redefined == xmlSchemaType)
			{
				return true;
			}
			return false;
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x000581BF File Offset: 0x000571BF
		protected void SendValidationEvent(string code, XmlSchemaObject source)
		{
			this.SendValidationEvent(new XmlSchemaException(code, source), XmlSeverityType.Error);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000581CF File Offset: 0x000571CF
		protected void SendValidationEvent(string code, string msg, XmlSchemaObject source)
		{
			this.SendValidationEvent(new XmlSchemaException(code, msg, source), XmlSeverityType.Error);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x000581E0 File Offset: 0x000571E0
		protected void SendValidationEvent(string code, string msg1, string msg2, XmlSchemaObject source)
		{
			this.SendValidationEvent(new XmlSchemaException(code, new string[] { msg1, msg2 }, source), XmlSeverityType.Error);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x0005820C File Offset: 0x0005720C
		protected void SendValidationEvent(string code, string[] args, Exception innerException, XmlSchemaObject source)
		{
			this.SendValidationEvent(new XmlSchemaException(code, args, innerException, source.SourceUri, source.LineNumber, source.LinePosition, source), XmlSeverityType.Error);
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00058240 File Offset: 0x00057240
		protected void SendValidationEvent(string code, string msg1, string msg2, string sourceUri, int lineNumber, int linePosition)
		{
			this.SendValidationEvent(new XmlSchemaException(code, new string[] { msg1, msg2 }, sourceUri, lineNumber, linePosition), XmlSeverityType.Error);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00058270 File Offset: 0x00057270
		protected void SendValidationEvent(string code, XmlSchemaObject source, XmlSeverityType severity)
		{
			this.SendValidationEvent(new XmlSchemaException(code, source), severity);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00058280 File Offset: 0x00057280
		protected void SendValidationEvent(XmlSchemaException e)
		{
			this.SendValidationEvent(e, XmlSeverityType.Error);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x0005828A File Offset: 0x0005728A
		protected void SendValidationEvent(string code, string msg, XmlSchemaObject source, XmlSeverityType severity)
		{
			this.SendValidationEvent(new XmlSchemaException(code, msg, source), severity);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x0005829C File Offset: 0x0005729C
		protected void SendValidationEvent(XmlSchemaException e, XmlSeverityType severity)
		{
			if (severity == XmlSeverityType.Error)
			{
				this.errorCount++;
			}
			if (this.eventHandler != null)
			{
				this.eventHandler(null, new ValidationEventArgs(e, severity));
				return;
			}
			if (severity == XmlSeverityType.Error)
			{
				throw e;
			}
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x000582D0 File Offset: 0x000572D0
		protected void SendValidationEventNoThrow(XmlSchemaException e, XmlSeverityType severity)
		{
			if (severity == XmlSeverityType.Error)
			{
				this.errorCount++;
			}
			if (this.eventHandler != null)
			{
				this.eventHandler(null, new ValidationEventArgs(e, severity));
			}
		}

		// Token: 0x04000C79 RID: 3193
		private XmlNameTable nameTable;

		// Token: 0x04000C7A RID: 3194
		private SchemaNames schemaNames;

		// Token: 0x04000C7B RID: 3195
		private ValidationEventHandler eventHandler;

		// Token: 0x04000C7C RID: 3196
		private XmlSchemaCompilationSettings compilationSettings;

		// Token: 0x04000C7D RID: 3197
		private int errorCount;

		// Token: 0x04000C7E RID: 3198
		private string NsXml;
	}
}
