using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200006A RID: 106
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeObjectCreateExpression : CodeExpression
	{
		// Token: 0x060003ED RID: 1005 RVA: 0x00014236 File Offset: 0x00013236
		public CodeObjectCreateExpression()
		{
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014249 File Offset: 0x00013249
		public CodeObjectCreateExpression(CodeTypeReference createType, params CodeExpression[] parameters)
		{
			this.CreateType = createType;
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001426F File Offset: 0x0001326F
		public CodeObjectCreateExpression(string createType, params CodeExpression[] parameters)
		{
			this.CreateType = new CodeTypeReference(createType);
			this.Parameters.AddRange(parameters);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001429A File Offset: 0x0001329A
		public Code