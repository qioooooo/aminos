using System;

namespace System.Xml.Schema
{
	internal sealed class SchemaAttDef : SchemaDeclBase
	{
		public SchemaAttDef(XmlQualifiedName name, string prefix)
			: base(name, prefix)
		{
			this.reserved = SchemaAttDef.Reserve.None;
		}

		private SchemaAttDef()
		{
		}

		public SchemaAttDef Clone()
		{
			return (SchemaAttDef)base.MemberwiseClone();
		}

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

		internal bool DefaultValueChecked
		{
			get
			{
				return this.defaultValueChecked;
			}
		}

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

		internal void CheckDefaultValue(SchemaInfo schemaInfo, IDtdParserAdapter readerAdapter)
		{
			DtdValidator.CheckDefaultValue(this, schemaInfo, readerAdapter);
			this.defaultValueChecked = true;
		}

		private SchemaAttDef.Reserve reserved;

		private string defExpanded;

		private bool hasEntityRef;

		private XmlSchemaAttribute schemaAttribute;

		private bool defaultValueChecked;

		private int lineNum;

		private int linePos;

		private int valueLineNum;

		private int valueLinePos;

		public static readonly SchemaAttDef Empty = new SchemaAttDef();

		public enum Reserve
		{
			None,
			XmlSpace,
			XmlLang
		}
	}
}
