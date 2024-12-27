using System;
using System.CodeDom.Compiler;

namespace System.Xml.Serialization
{
	// Token: 0x02000311 RID: 785
	public class XmlMemberMapping
	{
		// Token: 0x0600251F RID: 9503 RVA: 0x000AE0D1 File Offset: 0x000AD0D1
		internal XmlMemberMapping(MemberMapping mapping)
		{
			this.mapping = mapping;
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06002520 RID: 9504 RVA: 0x000AE0E0 File Offset: 0x000AD0E0
		internal MemberMapping Mapping
		{
			get
			{
				return this.mapping;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06002521 RID: 9505 RVA: 0x000AE0E8 File Offset: 0x000AD0E8
		internal Accessor Accessor
		{
			get
			{
				return this.mapping.Accessor;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06002522 RID: 9506 RVA: 0x000AE0F5 File Offset: 0x000AD0F5
		public bool Any
		{
			get
			{
				return this.Accessor.Any;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06002523 RID: 9507 RVA: 0x000AE102 File Offset: 0x000AD102
		public string ElementName
		{
			get
			{
				return Accessor.UnescapeName(this.Accessor.Name);
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06002524 RID: 9508 RVA: 0x000AE114 File Offset: 0x000AD114
		public string XsdElementName
		{
			get
			{
				return this.Accessor.Name;
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06002525 RID: 9509 RVA: 0x000AE121 File Offset: 0x000AD121
		public string Namespace
		{
			get
			{
				return this.Accessor.Namespace;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06002526 RID: 9510 RVA: 0x000AE12E File Offset: 0x000AD12E
		public string MemberName
		{
			get
			{
				return this.mapping.Name;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06002527 RID: 9511 RVA: 0x000AE13B File Offset: 0x000AD13B
		public string TypeName
		{
			get
			{
				if (this.Accessor.Mapping == null)
				{
					return string.Empty;
				}
				return this.Accessor.Mapping.TypeName;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06002528 RID: 9512 RVA: 0x000AE160 File Offset: 0x000AD160
		public string TypeNamespace
		{
			get
			{
				if (this.Accessor.Mapping == null)
				{
					return null;
				}
				return this.Accessor.Mapping.Namespace;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06002529 RID: 9513 RVA: 0x000AE181 File Offset: 0x000AD181
		public string TypeFullName
		{
			get
			{
				return this.mapping.TypeDesc.FullName;
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600252A RID: 9514 RVA: 0x000AE193 File Offset: 0x000AD193
		public bool CheckSpecified
		{
			get
			{
				return this.mapping.CheckSpecified != SpecifiedAccessor.None;
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600252B RID: 9515 RVA: 0x000AE1A6 File Offset: 0x000AD1A6
		internal bool IsNullable
		{
			get
			{
				return this.mapping.IsNeedNullable;
			}
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x000AE1B3 File Offset: 0x000AD1B3
		public string GenerateTypeName(CodeDomProvider codeProvider)
		{
			return this.mapping.GetTypeName(codeProvider);
		}

		// Token: 0x0400158B RID: 5515
		private MemberMapping mapping;
	}
}
