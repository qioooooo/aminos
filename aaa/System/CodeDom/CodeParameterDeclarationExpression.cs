using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200006B RID: 107
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeParameterDeclarationExpression : CodeExpression
	{
		// Token: 0x060003F4 RID: 1012 RVA: 0x000142F6 File Offset: 0x000132F6
		public CodeParameterDeclarationExpression()
		{
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000142FE File Offset: 0x000132FE
		public CodeParameterDeclarationExpression(CodeTypeReference type, string name)
		{
			this.Type = type;
			this.Name = name;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00014314 File Offset: 0x00013314
		public CodeParameterDeclarationExpression(string type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001432F File Offset: 0x0001332F
		public CodeParameterDeclarationExpression(Type type, string name)
		{
			this.Type = new CodeTypeReference(type);
			this.Name = name;
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x0001434A File Offset: 0x0001334A
		// (set) Token: 0x060003F9 RID: 1017 RVA: 0x00014365 File Offset: 0x00013365
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
			set
			{
				this.customAttributes = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x0001436E File Offset: 0x0001336E
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x00014376 File Offset: 0x00013376
		public FieldDirection Direction
		{
			get
			{
				return this.dir;
			}
			set
			{
				this.dir = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x0001437F File Offset: 0x0001337F
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0001439F File Offset: 0x0001339F
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000143A8 File Offset: 0x000133A8
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x000143BE File Offset: 0x000133BE
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

		// Token: 0x04000864 RID: 2148
		private CodeTypeReference type;

		// Token: 0x04000865 RID: 2149
		private string name;

		// Token: 0x04000866 RID: 2150
		private CodeAttributeDeclarationCollection customAttributes;

		// Token: 0x04000867 RID: 2151
		private FieldDirection dir;
	}
}
