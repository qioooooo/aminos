using System;

namespace System.Xml.Schema
{
	// Token: 0x0200020A RID: 522
	internal sealed class SchemaAttDef : SchemaDeclBase
	{
		// Token: 0x060018C7 RID: 6343 RVA: 0x00071B19 File Offset: 0x00070B19
		public SchemaAttDef(XmlQualifiedName name, string prefix)
			: base(name, prefix)
		{
			this.reserved = SchemaAttDef.Reserve.None;
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x00071B2A File Offset: 0x00070B2A
		private SchemaAttDef()
		{
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x00071B32 File Offset: 0x00070B32
		public SchemaAttDef Clone()
		{
			return (SchemaAttDef)base.MemberwiseClone();
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x00071B3F File Offset: 0x00070B3F
		// (set) Token: 0x060018CB RID: 6347 RVA: 0x00071B47 File Offset: 0x00070B47
		internal int LinePos
		{
			get
			{
				return this.linePos;
			}
			set
			{
				this.linePos = value;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x00071B50 File Offset: 0x00070B50
		// (set) Token: 0x060018CD RID: 6349 RVA: 0x00071B58 File Offset: 0x00070B58
		internal int LineNum
		{
			get
			{
				return this.lineNum;
			}
			set
			{
				this.lineNum = value;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060018CE RID: 6350 RVA: 0x00071B61 File Offset: 0x00070B61
		// (set) Token: 0x060018CF RID: 6351 RVA: 0x00071B69 File Offset: 0x00070B69
		internal int ValueLinePos
		{
			get
			{
				return this.valueLinePos;
			}
			set
			{
				this.valueLinePos = value;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x00071B72 File Offset: 0x00070B72
		// (set) Token: 0x060018D1 RID: 6353 RVA: 0x00071B7A File Offset: 0x00070B7A
		internal int ValueLineNum
		{
			get
			{
				return this.valueLineNum;
			}
			set
			{
				this.valueLineNum = value;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060018D2 RID: 6354 RVA: 0x00071B83 File Offset: 0x00070B83
		internal bool DefaultValueChecked
		{
			get
			{
				return this.defaultValueChecked;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060018D3 RID: 6355 RVA: 0x00071B8B File Offset: 0x00070B8B
		// (set) Token: 0x060018D4 RID: 6356 RVA: 0x00071BA1 File Offset: 0x00070BA1
		public string DefaultValueExpanded
		{
			get
			{
				if (this.defExpanded == null)
				{
					return string.Empty;
				}
				return this.defExpanded;
			}
			set
			{
				this.defExpanded = value;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060018D5 RID: 6357 RVA: 0x00071BAA File Offset: 0x00070BAA
		// (set) Token: 0x060018D6 RID: 6358 RVA: 0x00071BB2 File Offset: 0x00070BB2
		public SchemaAttDef.Reserve Reserved
		{
			get
			{
				return this.reserved;
			}
			set
			{
				this.reserved = value;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060018D7 RID: 6359 RVA: 0x00071BBB File Offset: 0x00070BBB
		// (set) Token: 0x060018D8 RID: 6360 RVA: 0x00071BC3 File Offset: 0x00070BC3
		public bool HasEntityRef
		{
			get
			{
				return this.hasEntityRef;
			}
			set
			{
				this.hasEntityRef = value;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060018D9 RID: 6361 RVA: 0x00071BCC File Offset: 0x00070BCC
		// (set) Token: 0x060018DA RID: 6362 RVA: 0x00071BD4 File Offset: 0x00070BD4
		public XmlSchemaAttribute SchemaAttribute
		{
			get
			{
				return this.schemaAttribute;
			}
			set
			{
				this.schemaAttribute = value;
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x00071BE0 File Offset: 0x00070BE0
		public void CheckXmlSpace(ValidationEventHandler eventhandler)
		{
			if (this.datatype.TokenizedType == XmlTokenizedType.ENUMERATION && this.values != null && this.values.Count <= 2)
			{
				string text = this.values[0].ToString();
				if (this.values.Count == 2)
				{
					string text2 = this.values[1].ToString();
					if ((text == "default" || text2 == "default") && (text == "preserve" || text2 == "preserve"))
					{
						return;
					}
				}
				else if (text == "default" || text == "preserve")
				{
					return;
				}
			}
			eventhandler(this, new ValidationEventArgs(new XmlSchemaException("Sch_XmlSpace", string.Empty)));
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x00071CB8 File Offset: 0x00070CB8
		internal void CheckDefaultValue(SchemaInfo schemaInfo, IDtdParserAdapter readerAdapter)
		{
			DtdValidator.CheckDefaultValue(this, schemaInfo, readerAdapter);
			this.defaultValueChecked = true;
		}

		// Token: 0x04000E9D RID: 3741
		private SchemaAttDef.Reserve reserved;

		// Token: 0x04000E9E RID: 3742
		private string defExpanded;

		// Token: 0x04000E9F RID: 3743
		private bool hasEntityRef;

		// Token: 0x04000EA0 RID: 3744
		private XmlSchemaAttribute schemaAttribute;

		// Token: 0x04000EA1 RID: 3745
		private bool defaultValueChecked;

		// Token: 0x04000EA2 RID: 3746
		private int lineNum;

		// Token: 0x04000EA3 RID: 3747
		private int linePos;

		// Token: 0x04000EA4 RID: 3748
		private int valueLineNum;

		// Token: 0x04000EA5 RID: 3749
		private int valueLinePos;

		// Token: 0x04000EA6 RID: 3750
		public static readonly SchemaAttDef Empty = new SchemaAttDef();

		// Token: 0x0200020B RID: 523
		public enum Reserve
		{
			// Token: 0x04000EA8 RID: 3752
			None,
			// Token: 0x04000EA9 RID: 3753
			XmlSpace,
			// Token: 0x04000EAA RID: 3754
			XmlLang
		}
	}
}
