using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C5 RID: 709
	internal abstract class TypeMapping : Mapping
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060021A3 RID: 8611 RVA: 0x0009F184 File Offset: 0x0009E184
		// (set) Token: 0x060021A4 RID: 8612 RVA: 0x0009F18C File Offset: 0x0009E18C
		internal bool ReferencedByTopLevelElement
		{
			get
			{
				return this.referencedByTopLevelElement;
			}
			set
			{
				this.referencedByTopLevelElement = value;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x0009F195 File Offset: 0x0009E195
		// (set) Token: 0x060021A6 RID: 8614 RVA: 0x0009F1A7 File Offset: 0x0009E1A7
		internal bool ReferencedByElement
		{
			get
			{
				return this.referencedByElement || this.referencedByTopLevelElement;
			}
			set
			{
				this.referencedByElement = value;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060021A7 RID: 8615 RVA: 0x0009F1B0 File Offset: 0x0009E1B0
		// (set) Token: 0x060021A8 RID: 8616 RVA: 0x0009F1B8 File Offset: 0x0009E1B8
		internal string Namespace
		{
			get
			{
				return this.typeNs;
			}
			set
			{
				this.typeNs = value;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060021A9 RID: 8617 RVA: 0x0009F1C1 File Offset: 0x0009E1C1
		// (set) Token: 0x060021AA RID: 8618 RVA: 0x0009F1C9 File Offset: 0x0009E1C9
		internal string TypeName
		{
			get
			{
				return this.typeName;
			}
			set
			{
				this.typeName = value;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x0009F1D2 File Offset: 0x0009E1D2
		// (set) Token: 0x060021AC RID: 8620 RVA: 0x0009F1DA File Offset: 0x0009E1DA
		internal TypeDesc TypeDesc
		{
			get
			{
				return this.typeDesc;
			}
			set
			{
				this.typeDesc = value;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060021AD RID: 8621 RVA: 0x0009F1E3 File Offset: 0x0009E1E3
		// (set) Token: 0x060021AE RID: 8622 RVA: 0x0009F1EB File Offset: 0x0009E1EB
		internal bool IncludeInSchema
		{
			get
			{
				return this.includeInSchema;
			}
			set
			{
				this.includeInSchema = value;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060021AF RID: 8623 RVA: 0x0009F1F4 File Offset: 0x0009E1F4
		// (set) Token: 0x060021B0 RID: 8624 RVA: 0x0009F1F7 File Offset: 0x0009E1F7
		internal virtual bool IsList
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x060021B1 RID: 8625 RVA: 0x0009F1F9 File Offset: 0x0009E1F9
		// (set) Token: 0x060021B2 RID: 8626 RVA: 0x0009F201 File Offset: 0x0009E201
		internal bool IsReference
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x0009F20A File Offset: 0x0009E20A
		internal bool IsAnonymousType
		{
			get
			{
				return this.typeName == null || this.typeName.Length == 0;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x0009F224 File Offset: 0x0009E224
		internal virtual string DefaultElementName
		{
			get
			{
				if (!this.IsAnonymousType)
				{
					return this.typeName;
				}
				return XmlConvert.EncodeLocalName(this.typeDesc.Name);
			}
		}

		// Token: 0x0400146E RID: 5230
		private TypeDesc typeDesc;

		// Token: 0x0400146F RID: 5231
		private string typeNs;

		// Token: 0x04001470 RID: 5232
		private string typeName;

		// Token: 0x04001471 RID: 5233
		private bool referencedByElement;

		// Token: 0x04001472 RID: 5234
		private bool referencedByTopLevelElement;

		// Token: 0x04001473 RID: 5235
		private bool includeInSchema = true;

		// Token: 0x04001474 RID: 5236
		private bool reference;
	}
}
