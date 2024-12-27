using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000213 RID: 531
	internal class SchemaInfo
	{
		// Token: 0x06001988 RID: 6536 RVA: 0x00079E64 File Offset: 0x00078E64
		public SchemaInfo()
		{
			this.schemaType = SchemaType.None;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001989 RID: 6537 RVA: 0x00079EC0 File Offset: 0x00078EC0
		// (set) Token: 0x0600198A RID: 6538 RVA: 0x00079EC8 File Offset: 0x00078EC8
		public SchemaType SchemaType
		{
			get
			{
				return this.schemaType;
			}
			set
			{
				this.schemaType = value;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600198B RID: 6539 RVA: 0x00079ED1 File Offset: 0x00078ED1
		public Hashtable TargetNamespaces
		{
			get
			{
				return this.targetNamespaces;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x0600198C RID: 6540 RVA: 0x00079ED9 File Offset: 0x00078ED9
		public Hashtable ElementDecls
		{
			get
			{
				return this.elementDecls;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x0600198D RID: 6541 RVA: 0x00079EE1 File Offset: 0x00078EE1
		public Hashtable UndeclaredElementDecls
		{
			get
			{
				return this.undeclaredElementDecls;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x00079EE9 File Offset: 0x00078EE9
		public Hashtable ElementDeclsByType
		{
			get
			{
				return this.elementDeclsByType;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x0600198F RID: 6543 RVA: 0x00079EF1 File Offset: 0x00078EF1
		public Hashtable AttributeDecls
		{
			get
			{
				return this.attributeDecls;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001990 RID: 6544 RVA: 0x00079EF9 File Offset: 0x00078EF9
		public Hashtable GeneralEntities
		{
			get
			{
				if (this.generalEntities == null)
				{
					this.generalEntities = new Hashtable();
				}
				return this.generalEntities;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001991 RID: 6545 RVA: 0x00079F14 File Offset: 0x00078F14
		public Hashtable ParameterEntities
		{
			get
			{
				if (this.parameterEntities == null)
				{
					this.parameterEntities = new Hashtable();
				}
				return this.parameterEntities;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001992 RID: 6546 RVA: 0x00079F2F File Offset: 0x00078F2F
		public Hashtable Notations
		{
			get
			{
				if (this.notations == null)
				{
					this.notations = new Hashtable();
				}
				return this.notations;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001993 RID: 6547 RVA: 0x00079F4A File Offset: 0x00078F4A
		// (set) Token: 0x06001994 RID: 6548 RVA: 0x00079F52 File Offset: 0x00078F52
		public XmlQualifiedName DocTypeName
		{
			get
			{
				return this.docTypeName;
			}
			set
			{
				this.docTypeName = value;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06001995 RID: 6549 RVA: 0x00079F5B File Offset: 0x00078F5B
		// (set) Token: 0x06001996 RID: 6550 RVA: 0x00079F63 File Offset: 0x00078F63
		public int ErrorCount
		{
			get
			{
				return this.errorCount;
			}
			set
			{
				this.errorCount = value;
			}
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x00079F6C File Offset: 0x00078F6C
		public SchemaElementDecl GetElementDecl(XmlQualifiedName qname)
		{
			return (SchemaElementDecl)this.elementDecls[qname];
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x00079F7F File Offset: 0x00078F7F
		public SchemaElementDecl GetTypeDecl(XmlQualifiedName qname)
		{
			return (SchemaElementDecl)this.elementDeclsByType[qname];
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x00079F94 File Offset: 0x00078F94
		public XmlSchemaElement GetElement(XmlQualifiedName qname)
		{
			SchemaElementDecl elementDecl = this.GetElementDecl(qname);
			if (elementDecl != null)
			{
				return elementDecl.SchemaElement;
			}
			return null;
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00079FB4 File Offset: 0x00078FB4
		public XmlSchemaAttribute GetAttribute(XmlQualifiedName qname)
		{
			SchemaAttDef schemaAttDef = (SchemaAttDef)this.attributeDecls[qname];
			if (schemaAttDef != null)
			{
				return schemaAttDef.SchemaAttribute;
			}
			return null;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x00079FE0 File Offset: 0x00078FE0
		public XmlSchemaElement GetType(XmlQualifiedName qname)
		{
			SchemaElementDecl elementDecl = this.GetElementDecl(qname);
			if (elementDecl != null)
			{
				return elementDecl.SchemaElement;
			}
			return null;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0007A000 File Offset: 0x00079000
		public bool HasSchema(string ns)
		{
			return this.targetNamespaces[ns] != null;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0007A014 File Offset: 0x00079014
		public bool Contains(string ns)
		{
			return this.targetNamespaces[ns] != null;
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0007A028 File Offset: 0x00079028
		public SchemaAttDef GetAttributeXdr(SchemaElementDecl ed, XmlQualifiedName qname)
		{
			SchemaAttDef schemaAttDef = null;
			if (ed != null)
			{
				schemaAttDef = ed.GetAttDef(qname);
				if (schemaAttDef == null)
				{
					if (!ed.ContentValidator.IsOpen || qname.Namespace.Length == 0)
					{
						throw new XmlSchemaException("Sch_UndeclaredAttribute", qname.ToString());
					}
					schemaAttDef = (SchemaAttDef)this.attributeDecls[qname];
					if (schemaAttDef == null && this.targetNamespaces.Contains(qname.Namespace))
					{
						throw new XmlSchemaException("Sch_UndeclaredAttribute", qname.ToString());
					}
				}
			}
			return schemaAttDef;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0007A0AC File Offset: 0x000790AC
		public SchemaAttDef GetAttributeXsd(SchemaElementDecl ed, XmlQualifiedName qname, XmlSchemaObject partialValidationType, out AttributeMatchState attributeMatchState)
		{
			SchemaAttDef schemaAttDef = null;
			attributeMatchState = AttributeMatchState.UndeclaredAttribute;
			if (ed != null)
			{
				schemaAttDef = ed.GetAttDef(qname);
				if (schemaAttDef != null)
				{
					attributeMatchState = AttributeMatchState.AttributeFound;
					return schemaAttDef;
				}
				XmlSchemaAnyAttribute anyAttribute = ed.AnyAttribute;
				if (anyAttribute != null)
				{
					if (!anyAttribute.NamespaceList.Allows(qname))
					{
						attributeMatchState = AttributeMatchState.ProhibitedAnyAttribute;
					}
					else if (anyAttribute.ProcessContentsCorrect != XmlSchemaContentProcessing.Skip)
					{
						schemaAttDef = (SchemaAttDef)this.attributeDecls[qname];
						if (schemaAttDef != null)
						{
							if (schemaAttDef.Datatype.TypeCode == XmlTypeCode.Id)
							{
								attributeMatchState = AttributeMatchState.AnyIdAttributeFound;
							}
							else
							{
								attributeMatchState = AttributeMatchState.AttributeFound;
							}
						}
						else if (anyAttribute.ProcessContentsCorrect == XmlSchemaContentProcessing.Lax)
						{
							attributeMatchState = AttributeMatchState.AnyAttributeLax;
						}
					}
					else
					{
						attributeMatchState = AttributeMatchState.AnyAttributeSkip;
					}
				}
				else if (ed.ProhibitedAttributes[qname] != null)
				{
					attributeMatchState = AttributeMatchState.ProhibitedAttribute;
				}
			}
			else if (partialValidationType != null)
			{
				XmlSchemaAttribute xmlSchemaAttribute = partialValidationType as XmlSchemaAttribute;
				if (xmlSchemaAttribute != null)
				{
					if (qname.Equals(xmlSchemaAttribute.QualifiedName))
					{
						schemaAttDef = xmlSchemaAttribute.AttDef;
						attributeMatchState = AttributeMatchState.AttributeFound;
					}
					else
					{
						attributeMatchState = AttributeMatchState.AttributeNameMismatch;
					}
				}
				else
				{
					attributeMatchState = AttributeMatchState.ValidateAttributeInvalidCall;
				}
			}
			else
			{
				schemaAttDef = (SchemaAttDef)this.attributeDecls[qname];
				if (schemaAttDef != null)
				{
					attributeMatchState = AttributeMatchState.AttributeFound;
				}
				else
				{
					attributeMatchState = AttributeMatchState.UndeclaredElementAndAttribute;
				}
			}
			return schemaAttDef;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0007A1B4 File Offset: 0x000791B4
		public SchemaAttDef GetAttributeXsd(SchemaElementDecl ed, XmlQualifiedName qname, ref bool skip)
		{
			AttributeMatchState attributeMatchState;
			SchemaAttDef attributeXsd = this.GetAttributeXsd(ed, qname, null, out attributeMatchState);
			switch (attributeMatchState)
			{
			case AttributeMatchState.UndeclaredAttribute:
				throw new XmlSchemaException("Sch_UndeclaredAttribute", qname.ToString());
			case AttributeMatchState.AnyAttributeSkip:
				skip = true;
				break;
			case AttributeMatchState.ProhibitedAnyAttribute:
			case AttributeMatchState.ProhibitedAttribute:
				throw new XmlSchemaException("Sch_ProhibitedAttribute", qname.ToString());
			}
			return attributeXsd;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0007A220 File Offset: 0x00079220
		public void Add(SchemaInfo sinfo, ValidationEventHandler eventhandler)
		{
			if (this.schemaType == SchemaType.None)
			{
				this.schemaType = sinfo.SchemaType;
			}
			else if (this.schemaType != sinfo.SchemaType)
			{
				if (eventhandler != null)
				{
					eventhandler(this, new ValidationEventArgs(new XmlSchemaException("Sch_MixSchemaTypes", string.Empty)));
				}
				return;
			}
			foreach (object obj in sinfo.TargetNamespaces.Keys)
			{
				string text = (string)obj;
				if (!this.targetNamespaces.ContainsKey(text))
				{
					this.targetNamespaces.Add(text, true);
				}
			}
			foreach (object obj2 in sinfo.elementDecls)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				if (!this.elementDecls.ContainsKey(dictionaryEntry.Key))
				{
					this.elementDecls.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			foreach (object obj3 in sinfo.elementDeclsByType)
			{
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj3;
				if (!this.elementDeclsByType.ContainsKey(dictionaryEntry2.Key))
				{
					this.elementDeclsByType.Add(dictionaryEntry2.Key, dictionaryEntry2.Value);
				}
			}
			foreach (object obj4 in sinfo.AttributeDecls.Values)
			{
				SchemaAttDef schemaAttDef = (SchemaAttDef)obj4;
				if (!this.attributeDecls.ContainsKey(schemaAttDef.Name))
				{
					this.attributeDecls.Add(schemaAttDef.Name, schemaAttDef);
				}
			}
			foreach (object obj5 in sinfo.Notations.Values)
			{
				SchemaNotation schemaNotation = (SchemaNotation)obj5;
				if (!this.Notations.ContainsKey(schemaNotation.Name.Name))
				{
					this.Notations.Add(schemaNotation.Name.Name, schemaNotation);
				}
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0007A4C0 File Offset: 0x000794C0
		public void Finish()
		{
			Hashtable hashtable = this.elementDecls;
			for (int i = 0; i < 2; i++)
			{
				foreach (object obj in hashtable.Values)
				{
					SchemaElementDecl schemaElementDecl = (SchemaElementDecl)obj;
					schemaElementDecl.EndAddAttDef();
					if (schemaElementDecl.HasNonCDataAttribute)
					{
						this.hasNonCDataAttributes = true;
					}
					if (schemaElementDecl.DefaultAttDefs != null)
					{
						this.hasDefaultAttributes = true;
					}
				}
				hashtable = this.undeclaredElementDecls;
			}
		}

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x060019A3 RID: 6563 RVA: 0x0007A554 File Offset: 0x00079554
		// (set) Token: 0x060019A4 RID: 6564 RVA: 0x0007A55C File Offset: 0x0007955C
		internal bool HasDefaultAttributes
		{
			get
			{
				return this.hasDefaultAttributes;
			}
			set
			{
				this.hasDefaultAttributes = value;
			}
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060019A5 RID: 6565 RVA: 0x0007A565 File Offset: 0x00079565
		// (set) Token: 0x060019A6 RID: 6566 RVA: 0x0007A56D File Offset: 0x0007956D
		internal bool HasNonCDataAttributes
		{
			get
			{
				return this.hasNonCDataAttributes;
			}
			set
			{
				this.hasNonCDataAttributes = value;
			}
		}

		// Token: 0x04000EED RID: 3821
		private SchemaType schemaType;

		// Token: 0x04000EEE RID: 3822
		private Hashtable targetNamespaces = new Hashtable();

		// Token: 0x04000EEF RID: 3823
		private Hashtable elementDecls = new Hashtable();

		// Token: 0x04000EF0 RID: 3824
		private Hashtable undeclaredElementDecls = new Hashtable();

		// Token: 0x04000EF1 RID: 3825
		private Hashtable elementDeclsByType = new Hashtable();

		// Token: 0x04000EF2 RID: 3826
		private Hashtable attributeDecls = new Hashtable();

		// Token: 0x04000EF3 RID: 3827
		private Hashtable generalEntities;

		// Token: 0x04000EF4 RID: 3828
		private Hashtable parameterEntities;

		// Token: 0x04000EF5 RID: 3829
		private Hashtable notations;

		// Token: 0x04000EF6 RID: 3830
		private XmlQualifiedName docTypeName = XmlQualifiedName.Empty;

		// Token: 0x04000EF7 RID: 3831
		private int errorCount;

		// Token: 0x04000EF8 RID: 3832
		private bool hasNonCDataAttributes;

		// Token: 0x04000EF9 RID: 3833
		private bool hasDefaultAttributes;
	}
}
