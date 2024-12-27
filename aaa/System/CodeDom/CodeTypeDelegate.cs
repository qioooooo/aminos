using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200007E RID: 126
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeTypeDelegate : CodeTypeDeclaration
	{
		// Token: 0x06000473 RID: 1139 RVA: 0x00014CDC File Offset: 0x00013CDC
		public CodeTypeDelegate()
		{
			base.TypeAttributes &= ~TypeAttributes.ClassSemanticsMask;
			base.TypeAttributes = base.TypeAttributes;
			base.BaseTypes.Clear();
			base.BaseTypes.Add(new CodeTypeReference("System.Delegate"));
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00014D36 File Offset: 0x00013D36
		public CodeTypeDelegate(string name)
			: this()
		{
			base.Name = name;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x00014D45 File Offset: 0x00013D45
		// (set) Token: 0x06000476 RID: 1142 RVA: 0x00014D65 File Offset: 0x00013D65
		public CodeTypeReference ReturnType
		{
			get
			{
				if (this.returnType == null)
				{
					this.returnType = new CodeTypeReference("");
				}
				return this.returnType;
			}
			set
			{
				this.returnType = value;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00014D6E File Offset: 0x00013D6E
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04000889 RID: 2185
		private CodeParameterDeclarationExpressionCollection parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x0400088A RID: 2186
		private CodeTypeReference returnType;
	}
}
