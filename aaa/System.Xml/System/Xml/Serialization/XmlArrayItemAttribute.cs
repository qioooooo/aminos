using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	// Token: 0x02000300 RID: 768
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true)]
	public class XmlArrayItemAttribute : Attribute
	{
		// Token: 0x060023EF RID: 9199 RVA: 0x000AA422 File Offset: 0x000A9422
		public XmlArrayItemAttribute()
		{
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000AA42A File Offset: 0x000A942A
		public XmlArrayItemAttribute(string elementName)
		{
			this.elementName = elementName;
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x000AA439 File Offset: 0x000A9439
		public XmlArrayItemAttribute(Type type)
		{
			this.type = type;
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000AA448 File Offset: 0x000A9448
		public XmlArrayItemAttribute(string elementName, Type type)
		{
			this.elementName = elementName;
			this.type = type;
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x060023F3 RID: 9203 RVA: 0x000AA45E File Offset: 0x000A945E
		// (set) Token: 0x060023F4 RID: 9204 RVA: 0x000AA466 File Offset: 0x000A9466
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x060023F5 RID: 9205 RVA: 0x000AA46F File Offset: 0x000A946F
		// (set) Token: 0x060023F6 RID: 9206 RVA: 0x000AA485 File Offset: 0x000A9485
		public string ElementName
		{
			get
			{
				if (this.elementName != null)
				{
					return this.elementName;
				}
				return string.Empty;
			}
			set
			{
				this.elementName = value;
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x060023F7 RID: 9207 RVA: 0x000AA48E File Offset: 0x000A948E
		// (set) Token: 0x060023F8 RID: 9208 RVA: 0x000AA496 File Offset: 0x000A9496
		public string Namespace
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

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000AA49F File Offset: 0x000A949F
		// (set) Token: 0x060023FA RID: 9210 RVA: 0x000AA4A7 File Offset: 0x000A94A7
		public int NestingLevel
		{
			get
			{
				return this.nestingLevel;
			}
			set
			{
				this.nestingLevel = value;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x060023FB RID: 9211 RVA: 0x000AA4B0 File Offset: 0x000A94B0
		// (set) Token: 0x060023FC RID: 9212 RVA: 0x000AA4C6 File Offset: 0x000A94C6
		public string DataType
		{
			get
			{
				if (this.dataType != null)
				{
					return this.dataType;
				}
				return string.Empty;
			}
			set
			{
				this.dataType = value;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x060023FD RID: 9213 RVA: 0x000AA4CF File Offset: 0x000A94CF
		// (set) Token: 0x060023FE RID: 9214 RVA: 0x000AA4D7 File Offset: 0x000A94D7
		public bool IsNullable
		{
			get
			{
				return this.nullable;
			}
			set
			{
				this.nullable = value;
				this.nullableSpecified = true;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x060023FF RID: 9215 RVA: 0x000AA4E7 File Offset: 0x000A94E7
		internal bool IsNullableSpecified
		{
			get
			{
				return this.nullableSpecified;
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x000AA4EF File Offset: 0x000A94EF
		// (set) Token: 0x06002401 RID: 9217 RVA: 0x000AA4F7 File Offset: 0x000A94F7
		public XmlSchemaForm Form
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

		// Token: 0x04001545 RID: 5445
		private string elementName;

		// Token: 0x04001546 RID: 5446
		private Type type;

		// Token: 0x04001547 RID: 5447
		private string ns;

		// Token: 0x04001548 RID: 5448
		private string dataType;

		// Token: 0x04001549 RID: 5449
		private bool nullable;

		// Token: 0x0400154A RID: 5450
		private bool nullableSpecified;

		// Token: 0x0400154B RID: 5451
		private XmlSchemaForm form;

		// Token: 0x0400154C RID: 5452
		private int nestingLevel;
	}
}
