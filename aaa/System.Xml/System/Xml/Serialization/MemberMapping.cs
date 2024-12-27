using System;
using System.CodeDom.Compiler;

namespace System.Xml.Serialization
{
	// Token: 0x020002D0 RID: 720
	internal class MemberMapping : AccessorMapping
	{
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x0009FD56 File Offset: 0x0009ED56
		// (set) Token: 0x0600220D RID: 8717 RVA: 0x0009FD5E File Offset: 0x0009ED5E
		internal bool CheckShouldPersist
		{
			get
			{
				return this.checkShouldPersist;
			}
			set
			{
				this.checkShouldPersist = value;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x0600220E RID: 8718 RVA: 0x0009FD67 File Offset: 0x0009ED67
		// (set) Token: 0x0600220F RID: 8719 RVA: 0x0009FD6F File Offset: 0x0009ED6F
		internal SpecifiedAccessor CheckSpecified
		{
			get
			{
				return this.checkSpecified;
			}
			set
			{
				this.checkSpecified = value;
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002210 RID: 8720 RVA: 0x0009FD78 File Offset: 0x0009ED78
		// (set) Token: 0x06002211 RID: 8721 RVA: 0x0009FD8E File Offset: 0x0009ED8E
		internal string Name
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

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002212 RID: 8722 RVA: 0x0009FD97 File Offset: 0x0009ED97
		// (set) Token: 0x06002213 RID: 8723 RVA: 0x0009FD9F File Offset: 0x0009ED9F
		internal bool IsReturnValue
		{
			get
			{
				return this.isReturnValue;
			}
			set
			{
				this.isReturnValue = value;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x0009FDA8 File Offset: 0x0009EDA8
		// (set) Token: 0x06002215 RID: 8725 RVA: 0x0009FDB0 File Offset: 0x0009EDB0
		internal bool ReadOnly
		{
			get
			{
				return this.readOnly;
			}
			set
			{
				this.readOnly = value;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x0009FDB9 File Offset: 0x0009EDB9
		internal bool IsSequence
		{
			get
			{
				return this.sequenceId >= 0;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002217 RID: 8727 RVA: 0x0009FDC7 File Offset: 0x0009EDC7
		// (set) Token: 0x06002218 RID: 8728 RVA: 0x0009FDCF File Offset: 0x0009EDCF
		internal int SequenceId
		{
			get
			{
				return this.sequenceId;
			}
			set
			{
				this.sequenceId = value;
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x0009FDD8 File Offset: 0x0009EDD8
		private string GetNullableType(TypeDesc td)
		{
			if (td.IsMappedType || (!td.IsValueType && (base.Elements[0].IsSoap || td.ArrayElementTypeDesc == null)))
			{
				return td.FullName;
			}
			if (td.ArrayElementTypeDesc != null)
			{
				return this.GetNullableType(td.ArrayElementTypeDesc) + "[]";
			}
			return "System.Nullable`1[" + td.FullName + "]";
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x0009FE47 File Offset: 0x0009EE47
		internal string GetTypeName(CodeDomProvider codeProvider)
		{
			if (base.IsNeedNullable && codeProvider.Supports(GeneratorSupport.GenericTypeReference))
			{
				return this.GetNullableType(base.TypeDesc);
			}
			return base.TypeDesc.FullName;
		}

		// Token: 0x04001493 RID: 5267
		private string name;

		// Token: 0x04001494 RID: 5268
		private bool checkShouldPersist;

		// Token: 0x04001495 RID: 5269
		private SpecifiedAccessor checkSpecified;

		// Token: 0x04001496 RID: 5270
		private bool isReturnValue;

		// Token: 0x04001497 RID: 5271
		private bool readOnly;

		// Token: 0x04001498 RID: 5272
		private int sequenceId = -1;
	}
}
