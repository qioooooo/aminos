using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x020002BE RID: 702
	internal abstract class Accessor
	{
		// Token: 0x06002172 RID: 8562 RVA: 0x0009EDB9 File Offset: 0x0009DDB9
		internal Accessor()
		{
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002173 RID: 8563 RVA: 0x0009EDC1 File Offset: 0x0009DDC1
		// (set) Token: 0x06002174 RID: 8564 RVA: 0x0009EDC9 File Offset: 0x0009DDC9
		internal TypeMapping Mapping
		{
			get
			{
				return this.mapping;
			}
			set
			{
				this.mapping = value;
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002175 RID: 8565 RVA: 0x0009EDD2 File Offset: 0x0009DDD2
		// (set) Token: 0x06002176 RID: 8566 RVA: 0x0009EDDA File Offset: 0x0009DDDA
		internal object Default
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x0009EDE3 File Offset: 0x0009DDE3
		internal bool HasDefault
		{
			get
			{
				return this.defaultValue != null && this.defaultValue != DBNull.Value;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x0009EDFF File Offset: 0x0009DDFF
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x0009EE15 File Offset: 0x0009DE15
		internal virtual string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x0009EE1E File Offset: 0x0009DE1E
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x0009EE26 File Offset: 0x0009DE26
		internal bool Any
		{
			get
			{
				return this.any;
			}
			set
			{
				this.any = value;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x0009EE2F File Offset: 0x0009DE2F
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x0009EE37 File Offset: 0x0009DE37
		internal string AnyNamespaces
		{
			get
			{
				return this.anyNs;
			}
			set
			{
				this.anyNs = value;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x0600217E RID: 8574 RVA: 0x0009EE40 File Offset: 0x0009DE40
		// (set) Token: 0x0600217F RID: 8575 RVA: 0x0009EE48 File Offset: 0x0009DE48
		internal string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x0009EE51 File Offset: 0x0009DE51
		// (set) Token: 0x06002181 RID: 8577 RVA: 0x0009EE59 File Offset: 0x0009DE59
		internal XmlSchemaForm Form
		{
			get
			{
				return this.form;
			}
			set
			{
				this.form = value;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002182 RID: 8578 RVA: 0x0009EE62 File Offset: 0x0009DE62
		// (set) Token: 0x06002183 RID: 8579 RVA: 0x0009EE6A File Offset: 0x0009DE6A
		internal bool IsFixed
		{
			get
			{
				return this.isFixed;
			}
			set
			{
				this.isFixed = value;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x0009EE73 File Offset: 0x0009DE73
		// (set) Token: 0x06002185 RID: 8581 RVA: 0x0009EE7B File Offset: 0x0009DE7B
		internal bool IsOptional
		{
			get
			{
				return this.isOptional;
			}
			set
			{
				this.isOptional = value;
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x0009EE84 File Offset: 0x0009DE84
		// (set) Token: 0x06002187 RID: 8583 RVA: 0x0009EE8C File Offset: 0x0009DE8C
		internal bool IsTopLevelInSchema
		{
			get
			{
				return this.topLevelInSchema;
			}
			set
			{
				this.topLevelInSchema = value;
			}
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x0009EE95 File Offset: 0x0009DE95
		internal static string EscapeName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return name;
			}
			return XmlConvert.EncodeLocalName(name);
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x0009EEAC File Offset: 0x0009DEAC
		internal static string EscapeQName(string name)
		{
			if (name == null || name.Length == 0)
			{
				return name;
			}
			int num = name.LastIndexOf(':');
			if (num < 0)
			{
				return XmlConvert.EncodeLocalName(name);
			}
			if (num == 0 || num == name.Length - 1)
			{
				throw new ArgumentException(Res.GetString("Xml_InvalidNameChars", new object[] { name }), "name");
			}
			return new XmlQualifiedName(XmlConvert.EncodeLocalName(name.Substring(num + 1)), XmlConvert.EncodeLocalName(name.Substring(0, num))).ToString();
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x0009EF2E File Offset: 0x0009DF2E
		internal static string UnescapeName(string name)
		{
			return XmlConvert.DecodeName(name);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x0009EF38 File Offset: 0x0009DF38
		internal string ToString(string defaultNs)
		{
			if (this.Any)
			{
				return ((this.Namespace == null) ? "##any" : this.Namespace) + ":" + this.Name;
			}
			if (!(this.Namespace == defaultNs))
			{
				return this.Namespace + ":" + this.Name;
			}
			return this.Name;
		}

		// Token: 0x0400145C RID: 5212
		private string name;

		// Token: 0x0400145D RID: 5213
		private object defaultValue;

		// Token: 0x0400145E RID: 5214
		private string ns;

		// Token: 0x0400145F RID: 5215
		private TypeMapping mapping;

		// Token: 0x04001460 RID: 5216
		private bool any;

		// Token: 0x04001461 RID: 5217
		private string anyNs;

		// Token: 0x04001462 RID: 5218
		private bool topLevelInSchema;

		// Token: 0x04001463 RID: 5219
		private bool isFixed;

		// Token: 0x04001464 RID: 5220
		private bool isOptional;

		// Token: 0x04001465 RID: 5221
		private XmlSchemaForm form;
	}
}
