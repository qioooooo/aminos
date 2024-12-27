using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002D1 RID: 721
	internal class MembersMapping : TypeMapping
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x0009FE85 File Offset: 0x0009EE85
		// (set) Token: 0x0600221D RID: 8733 RVA: 0x0009FE8D File Offset: 0x0009EE8D
		internal MemberMapping[] Members
		{
			get
			{
				return this.members;
			}
			set
			{
				this.members = value;
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x0009FE96 File Offset: 0x0009EE96
		// (set) Token: 0x0600221F RID: 8735 RVA: 0x0009FE9E File Offset: 0x0009EE9E
		internal MemberMapping XmlnsMember
		{
			get
			{
				return this.xmlnsMember;
			}
			set
			{
				this.xmlnsMember = value;
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002220 RID: 8736 RVA: 0x0009FEA7 File Offset: 0x0009EEA7
		// (set) Token: 0x06002221 RID: 8737 RVA: 0x0009FEAF File Offset: 0x0009EEAF
		internal bool HasWrapperElement
		{
			get
			{
				return this.hasWrapperElement;
			}
			set
			{
				this.hasWrapperElement = value;
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002222 RID: 8738 RVA: 0x0009FEB8 File Offset: 0x0009EEB8
		// (set) Token: 0x06002223 RID: 8739 RVA: 0x0009FEC0 File Offset: 0x0009EEC0
		internal bool ValidateRpcWrapperElement
		{
			get
			{
				return this.validateRpcWrapperElement;
			}
			set
			{
				this.validateRpcWrapperElement = value;
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002224 RID: 8740 RVA: 0x0009FEC9 File Offset: 0x0009EEC9
		// (set) Token: 0x06002225 RID: 8741 RVA: 0x0009FED1 File Offset: 0x0009EED1
		internal bool WriteAccessors
		{
			get
			{
				return this.writeAccessors;
			}
			set
			{
				this.writeAccessors = value;
			}
		}

		// Token: 0x04001499 RID: 5273
		private MemberMapping[] members;

		// Token: 0x0400149A RID: 5274
		private bool hasWrapperElement = true;

		// Token: 0x0400149B RID: 5275
		private bool validateRpcWrapperElement;

		// Token: 0x0400149C RID: 5276
		private bool writeAccessors = true;

		// Token: 0x0400149D RID: 5277
		private MemberMapping xmlnsMember;
	}
}
