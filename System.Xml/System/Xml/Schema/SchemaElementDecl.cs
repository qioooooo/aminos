using System;
using System.Collections;

namespace System.Xml.Schema
{
	internal sealed class SchemaElementDecl : SchemaDeclBase
	{
		public SchemaElementDecl()
		{
		}

		public SchemaElementDecl(XmlSchemaDatatype dtype)
		{
			base.Datatype = dtype;
			this.contentValidator = ContentValidator.TextOnly;
		}

		public SchemaElementDecl(XmlQualifiedName name, string prefix, SchemaType schemaType)
			: base(name, prefix)
		{
		}

		public static SchemaElementDecl CreateAnyTypeElementDecl()
		{
			return new SchemaElementDecl
			{
				Datatype = DatatypeImplementation.AnySimpleType.Datatype
			};
		}

		public SchemaElementDecl Clone()
		{
			return (SchemaElementDecl)base.MemberwiseClone();
		}

		public bool IsAbstract
		{
			get
			{
				return this.isAbstract;
			}
			set
			{
				this.isAbstract = value;
			}
		}

		public bool IsNillable
		{
			get
			{
				return this.isNillable;
			}
			set
			{
				this.isNillable = value;
			}
		}

		public XmlSchemaDerivationMethod Block
		{
			get
			{
				return this.block;
			}
			set
			{
				this.block = value;
			}
		}

		public bool IsIdDeclared
		{
			get
			{
				return this.isIdDeclared;
			}
			set
			{
				this.isIdDeclared = value;
			}
		}

		public bool IsNotationDeclared
		{
			get
			{
				return this.isNotationDeclared;
			}
			set
			{
				this.isNotationDeclared = value;
			}
		}

		public bool HasDefaultAttribute
		{
			get
			{
				return this.defaultAttdefs != null;
			}
		}

		public bool HasRequiredAttribute
		{
			get
			{
				return this.hasRequiredAttribute;
			}
			set
			{
				this.hasRequiredAttribute = value;
			}
		}

		public bool HasNonCDataAttribute
		{
			get
			{
				return this.hasNonCDataAttribute;
			}
			set
			{
				this.hasNonCDataAttribute = value;
			}
		}

		public ContentValidator ContentValidator
		{
			get
			{
				return this.contentValidator;
			}
			set
			{
				this.contentValidator = value;
			}
		}

		public XmlSchemaAnyAttribute AnyAttribute
		{
			get
			{
				return this.anyAttribute;
			}
			set
			{
				this.anyAttribute = value;
			}
		}

		public CompiledIdentityConstraint[] Constraints
		{
			get
			{
				return this.constraints;
			}
			set
			{
				this.constraints = value;
			}
		}

		public XmlSchemaElement SchemaElement
		{
			get
			{
				return this.schemaElement;
			}
			set
			{
				this.schemaElement = value;
			}
		}

		public void AddAttDef(SchemaAttDef attdef)
		{
			this.attdefs.Add(attdef.Name, attdef);
			if (attdef.Presence == SchemaDeclBase.Use.Required || attdef.Presence == SchemaDeclBase.Use.RequiredFixed)
			{
				this.hasRequiredAttribute = true;
			}
			if (attdef.Presence == SchemaDeclBase.Use.Default || attdef.Presence == SchemaDeclBase.Use.Fixed)
			{
				if (this.tmpDefaultAttdefs == null)
				{
					this.tmpDefaultAttdefs = new ArrayList();
				}
				this.tmpDefaultAttdefs.Add(attdef);
			}
		}

		public void EndAddAttDef()
		{
			if (this.tmpDefaultAttdefs != null)
			{
				this.defaultAttdefs = (SchemaAttDef[])this.tmpDefaultAttdefs.ToArray(typeof(SchemaAttDef));
				this.tmpDefaultAttdefs = null;
			}
		}

		public SchemaAttDef GetAttDef(XmlQualifiedName qname)
		{
			return (SchemaAttDef)this.attdefs[qname];
		}

		public Hashtable AttDefs
		{
			get
			{
				return this.attdefs;
			}
		}

		public SchemaAttDef[] DefaultAttDefs
		{
			get
			{
				return this.defaultAttdefs;
			}
		}

		public Hashtable ProhibitedAttributes
		{
			get
			{
				return this.prohibitedAttributes;
			}
		}

		public void CheckAttributes(Hashtable presence, bool standalone)
		{
			foreach (object obj in this.attdefs.Values)
			{
				SchemaAttDef schemaAttDef = (SchemaAttDef)obj;
				if (presence[schemaAttDef.Name] == null)
				{
					if (schemaAttDef.Presence == SchemaDeclBase.Use.Required)
					{
						throw new XmlSchemaException("Sch_MissRequiredAttribute", schemaAttDef.Name.ToString());
					}
					if (standalone && schemaAttDef.IsDeclaredInExternal && (schemaAttDef.Presence == SchemaDeclBase.Use.Default || schemaAttDef.Presence == SchemaDeclBase.Use.Fixed))
					{
						throw new XmlSchemaException("Sch_StandAlone", string.Empty);
					}
				}
			}
		}

		private ContentValidator contentValidator;

		private Hashtable attdefs = new Hashtable();

		private Hashtable prohibitedAttributes = new Hashtable();

		private ArrayList tmpDefaultAttdefs;

		private SchemaAttDef[] defaultAttdefs;

		private bool isAbstract;

		private bool isNillable;

		private XmlSchemaDerivationMethod block;

		private bool isIdDeclared;

		private bool isNotationDeclared;

		private bool hasRequiredAttribute;

		private bool hasNonCDataAttribute;

		private XmlSchemaAnyAttribute anyAttribute;

		private CompiledIdentityConstraint[] constraints;

		private XmlSchemaElement schemaElement;

		public static readonly SchemaElementDecl Empty = new SchemaElementDecl();
	}
}
