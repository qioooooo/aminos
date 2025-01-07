using System;
using System.Collections;

namespace System.Xml.Schema
{
	internal class SchemaInfo
	{
		public SchemaInfo()
		{
			this.schemaType = SchemaType.None;
		}

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

		public Hashtable TargetNamespaces
		{
			get
			{
				return this.targetNamespaces;
			}
		}

		public Hashtable ElementDecls
		{
			get
			{
				return this.elementDecls;
			}
		}

		public Hashtable UndeclaredElementDecls
		{
			get
			{
				return this.undeclaredElementDecls;
			}
		}

		public Hashtable ElementDeclsByType
		{
			get
			{
				return this.elementDeclsByType;
			}
		}

		public Hashtable AttributeDecls
		{
			get
			{
				return this.attributeDecls;
			}
		}

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

		public SchemaElementDecl GetElementDecl(XmlQualifiedName qname)
		{
			return (SchemaElementDecl)this.elementDecls[qname];
		}

		public SchemaElementDecl GetTypeDecl(XmlQualifiedName qname)
		{
			return (SchemaElementDecl)this.elementDeclsByType[qname];
		}

		public XmlSchemaElement GetElement(XmlQualifiedName qname)
		{
			SchemaElementDecl elementDecl = this.GetElementDecl(qname);
			if (elementDecl != null)
			{
				return elementDecl.SchemaElement;
			}
			return null;
		}

		public XmlSchemaAttribute GetAttribute(XmlQualifiedName qname)
		{
			SchemaAttDef schemaAttDef = (SchemaAttDef)this.attributeDecls[qname];
			if (schemaAttDef != null)
			{
				return schemaAttDef.SchemaAttribute;
			}
			return null;
		}

		public XmlSchemaElement GetType(XmlQualifiedName qname)
		{
			SchemaElementDecl elementDecl = this.GetElementDecl(qname);
			if (elementDecl != null)
			{
				return elementDecl.SchemaElement;
			}
			return null;
		}

		public bool HasSchema(string ns)
		{
			return this.targetNamespaces[ns] != null;
		}

		public bool Contains(string ns)
		{
			return this.targetNamespaces[ns] != null;
		}

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

		private SchemaType schemaType;

		private Hashtable targetNamespaces = new Hashtable();

		private Hashtable elementDecls = new Hashtable();

		private Hashtable undeclaredElementDecls = new Hashtable();

		private Hashtable elementDeclsByType = new Hashtable();

		private Hashtable attributeDecls = new Hashtable();

		private Hashtable generalEntities;

		private Hashtable parameterEntities;

		private Hashtable notations;

		private XmlQualifiedName docTypeName = XmlQualifiedName.Empty;

		private int errorCount;

		private bool hasNonCDataAttributes;

		private bool hasDefaultAttributes;
	}
}
