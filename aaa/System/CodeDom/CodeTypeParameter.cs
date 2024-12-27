using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000081 RID: 129
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeParameter : CodeObject
	{
		// Token: 0x0600048B RID: 1163 RVA: 0x00014EEC File Offset: 0x00013EEC
		public CodeTypeParameter()
		{
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00014EF4 File Offset: 0x00013EF4
		public CodeTypeParameter(string name)
		{
			this.name = name;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600048D RID: 1165 RVA: 0x00014F03 File Offset: 0x00013F03
		// (set) Token: 0x0600048E RID: 1166 RVA: 0x00014F19 File Offset: 0x00013F19
		public string Name
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

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00014F22 File Offset: 0x00013F22
		public CodeTypeReferenceCollection Constraints
		{
			get
			{
				if (this.constraints == null)
				{
					this.constraints = new CodeTypeReferenceCollection();
				}
				return this.constraints;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x00014F3D File Offset: 0x00013F3D
		public CodeAttributeDeclarationCollection CustomAttributes
		{
			get
			{
				if (this.customAttributes == null)
				{
					this.customAttributes = new CodeAttributeDeclarationCollection();
				}
				return this.customAttributes;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x00014F58 File Offset: 0x00013F58
		// (set) Token: 0x06000492 RID: 1170 RVA: 0x00014F60 File Offset: 0x00013F60
		public bool HasConstructorConstraint
		{
			get
			{
				return this.hasConstructorConstraint;
			}
			set
			{
				this.hasConstructorConstraint = value;
			}
		}

		// Token: 0x0400088C RID: 2188
		private string name;

		// Token: 0x0400088D RID: 2189
		private CodeAttributeDeclarationCollection customAttributes;

		// Token: 0x0400088E RID: 2190
		private CodeTypeReferenceCollection constraints;

		// Token: 0x0400088F RID: 2191
		private bool hasConstructorConstraint;
	}
}
