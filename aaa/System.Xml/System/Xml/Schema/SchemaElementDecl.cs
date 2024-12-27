using System;
using System.Collections;

namespace System.Xml.Schema
{
	// Token: 0x02000210 RID: 528
	internal sealed class SchemaElementDecl : SchemaDeclBase
	{
		// Token: 0x06001949 RID: 6473 RVA: 0x00079A04 File Offset: 0x00078A04
		public SchemaElementDecl()
		{
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x00079A22 File Offset: 0x00078A22
		public SchemaElementDecl(XmlSchemaDatatype dtype)
		{
			base.Datatype = dtype;
			this.contentValidator = ContentValidator.TextOnly;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00079A52 File Offset: 0x00078A52
		public SchemaElementDecl(XmlQualifiedName name, string prefix, SchemaType schemaType)
			: base(name, prefix)
		{
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00079A74 File Offset: 0x00078A74
		public static SchemaElementDecl CreateAnyTypeElementDecl()
		{
			return new SchemaElementDecl
			{
				Datatype = DatatypeImplementation.AnySimpleType.Datatype
			};
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00079A98 File Offset: 0x00078A98
		public SchemaElementDecl Clone()
		{
			return (SchemaElementDecl)base.MemberwiseClone();
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600194E RID: 6478 RVA: 0x00079AA5 File Offset: 0x00078AA5
		// (set) Token: 0x0600194F RID: 6479 RVA: 0x00079AAD File Offset: 0x00078AAD
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

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06001950 RID: 6480 RVA: 0x00079AB6 File Offset: 0x00078AB6
		// (set) Token: 0x06001951 RID: 6481 RVA: 0x00079ABE File Offset: 0x00078ABE
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

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06001952 RID: 6482 RVA: 0x00079AC7 File Offset: 0x00078AC7
		// (set) Token: 0x06001953 RID: 6483 RVA: 0x00079ACF File Offset: 0x00078ACF
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

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00079AD8 File Offset: 0x00078AD8
		// (set) Token: 0x06001955 RID: 6485 RVA: 0x00079AE0 File Offset: 0x00078AE0
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

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x00079AE9 File Offset: 0x00078AE9
		// (set) Token: 0x06001957 RID: 6487 RVA: 0x00079AF1 File Offset: 0x00078AF1
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

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x00079AFA File Offset: 0x00078AFA
		public bool HasDefaultAttribute
		{
			get
			{
				return this.defaultAttdefs != null;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x00079B08 File Offset: 0x00078B08
		// (set) Token: 0x0600195A RID: 6490 RVA: 0x00079B10 File Offset: 0x00078B10
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

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x00079B19 File Offset: 0x00078B19
		// (set) Token: 0x0600195C RID: 6492 RVA: 0x00079B21 File Offset: 0x00078B21
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

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x00079B2A File Offset: 0x00078B2A
		// (set) Token: 0x0600195E RID: 6494 RVA: 0x00079B32 File Offset: 0x00078B32
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

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x00079B3B File Offset: 0x00078B3B
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x00079B43 File Offset: 0x00078B43
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

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x00079B4C File Offset: 0x00078B4C
		// (set) Token: 0x06001962 RID: 6498 RVA: 0x00079B54 File Offset: 0x00078B54
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

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x00079B5D File Offset: 0x00078B5D
		// (set) Token: 0x06001964 RID: 6500 RVA: 0x00079B65 File Offset: 0x00078B65
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

		// Token: 0x06001965 RID: 6501 RVA: 0x00079B70 File Offset: 0x00078B70
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

		// Token: 0x06001966 RID: 6502 RVA: 0x00079BD9 File Offset: 0x00078BD9
		public void EndAddAttDef()
		{
			if (this.tmpDefaultAttdefs != null)
			{
				this.defaultAttdefs = (SchemaAttDef[])this.tmpDefaultAttdefs.ToArray(typeof(SchemaAttDef));
				this.tmpDefaultAttdefs = null;
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x00079C0A File Offset: 0x00078C0A
		public SchemaAttDef GetAttDef(XmlQualifiedName qname)
		{
			return (SchemaAttDef)this.attdefs[qname];
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x00079C1D File Offset: 0x00078C1D
		public Hashtable AttDefs
		{
			get
			{
				return this.attdefs;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001969 RID: 6505 RVA: 0x00079C25 File Offset: 0x00078C25
		public SchemaAttDef[] DefaultAttDefs
		{
			get
			{
				return this.defaultAttdefs;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x00079C2D File Offset: 0x00078C2D
		public Hashtable ProhibitedAttributes
		{
			get
			{
				return this.prohibitedAttributes;
			}
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00079C38 File Offset: 0x00078C38
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

		// Token: 0x04000EC5 RID: 3781
		private ContentValidator contentValidator;

		// Token: 0x04000EC6 RID: 3782
		private Hashtable attdefs = new Hashtable();

		// Token: 0x04000EC7 RID: 3783
		private Hashtable prohibitedAttributes = new Hashtable();

		// Token: 0x04000EC8 RID: 3784
		private ArrayList tmpDefaultAttdefs;

		// Token: 0x04000EC9 RID: 3785
		private SchemaAttDef[] defaultAttdefs;

		// Token: 0x04000ECA RID: 3786
		private bool isAbstract;

		// Token: 0x04000ECB RID: 3787
		private bool isNillable;

		// Token: 0x04000ECC RID: 3788
		private XmlSchemaDerivationMethod block;

		// Token: 0x04000ECD RID: 3789
		private bool isIdDeclared;

		// Token: 0x04000ECE RID: 3790
		private bool isNotationDeclared;

		// Token: 0x04000ECF RID: 3791
		private bool hasRequiredAttribute;

		// Token: 0x04000ED0 RID: 3792
		private bool hasNonCDataAttribute;

		// Token: 0x04000ED1 RID: 3793
		private XmlSchemaAnyAttribute anyAttribute;

		// Token: 0x04000ED2 RID: 3794
		private CompiledIdentityConstraint[] constraints;

		// Token: 0x04000ED3 RID: 3795
		private XmlSchemaElement schemaElement;

		// Token: 0x04000ED4 RID: 3796
		public static readonly SchemaElementDecl Empty = new SchemaElementDecl();
	}
}
