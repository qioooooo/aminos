using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200003F RID: 63
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeAttributeDeclaration
	{
		// Token: 0x06000298 RID: 664 RVA: 0x00012884 File Offset: 0x00011884
		public CodeAttributeDeclaration()
		{
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00012897 File Offset: 0x00011897
		public CodeAttributeDeclaration(string name)
		{
			this.Name = name;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000128B1 File Offset: 0x000118B1
		public CodeAttributeDeclaration(string name, params CodeAttributeArgument[] arguments)
		{
			this.Name = name;
			this.Arguments.AddRange(arguments);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000128D7 File Offset: 0x000118D7
		public CodeAttributeDeclaration(CodeTypeReference attributeType)
			: this(attributeType, null)
		{
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000128E1 File Offset: 0x000118E1
		public CodeAttributeDeclaration(CodeTypeReference attributeType, params CodeAttributeArgument[] arguments)
		{
			this.attributeType = attributeType;
			if (attributeType != null)
			{
				this.name = attributeType.BaseType;
			}
			if (arguments != null)
			{
				this.Arguments.AddRange(arguments);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00012919 File Offset: 0x00011919
		// (set) Token: 0x0600029E RID: 670 RVA: 0x0001292F File Offset: 0x0001192F
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
				this.attributeType = new CodeTypeReference(this.name);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00012949 File Offset: 0x00011949
		public CodeAttributeArgumentCollection Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00012951 File Offset: 0x00011951
		public CodeTypeReference AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		// Token: 0x040007E5 RID: 2021
		private string name;

		// Token: 0x040007E6 RID: 2022
		private CodeAttributeArgumentCollection arguments = new CodeAttributeArgumentCollection();

		// Token: 0x040007E7 RID: 2023
		[OptionalField]
		private CodeTypeReference attributeType;
	}
}
