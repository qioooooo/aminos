using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000038 RID: 56
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeArrayCreateExpression : CodeExpression
	{
		// Token: 0x0600025C RID: 604 RVA: 0x00012408 File Offset: 0x00011408
		public CodeArrayCreateExpression()
		{
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0001241B File Offset: 0x0001141B
		public CodeArrayCreateExpression(CodeTypeReference createType, params CodeExpression[] initializers)
		{
			this.createType = createType;
			this.initializers.AddRange(initializers);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00012441 File Offset: 0x00011441
		public CodeArrayCreateExpression(string createType, params CodeExpression[] initializers)
		{
			this.createType = new CodeTypeReference(createType);
			this.initializers.AddRange(initializers);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0001246C File Offset: 0x0001146C
		public CodeArrayCreateExpression(Type createType, params CodeExpression[] initializers)
		{
			this.createType = new CodeTypeReference(createType);
			this.initializers.AddRange(initializers);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00012497 File Offset: 0x00011497
		public CodeArrayCreateExpression(CodeTypeReference createType, int size)
		{
			this.createType = createType;
			this.size = size;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000124B8 File Offset: 0x000114B8
		public CodeArrayCreateExpression(string createType, int size)
		{
			this.createType = new CodeTypeReference(createType);
			this.size = size;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000124DE File Offset: 0x000114DE
		public CodeArrayCreateExpression(Type createType, int size)
		{
			this.createType = new CodeTypeReference(createType);
			this.size = size;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00012504 File Offset: 0x00011504
		public CodeArrayCreateExpression(CodeTypeReference createType, CodeExpression size)
		{
			this.createType = createType;
			this.sizeExpression = size;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00012525 File Offset: 0x00011525
		public CodeArrayCreateExpression(string createType, CodeExpression size)
		{
			this.createType = new CodeTypeReference(createType);
			this.sizeExpression = size;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0001254B File Offset: 0x0001154B
		public CodeArrayCreateExpression(Type createType, CodeExpression size)
		{
			this.createType = new CodeTypeReference(createType);
			this.sizeExpression = size;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00012571 File Offset: 0x00011571
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00012591 File Offset: 0x00011591
		public CodeTypeReference CreateType
		{
			get
			{
				if (this.createType == null)
				{
					this.createType = new CodeTypeReference("");
				}
				return this.createType;
			}
			set
			{
				this.createType = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0001259A File Offset: 0x0001159A
		public CodeExpressionCollection Initializers
		{
			get
			{
				return this.initializers;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000269 RID: 617 RVA: 0x000125A2 File Offset: 0x000115A2
		// (set) Token: 0x0600026A RID: 618 RVA: 0x000125AA File Offset: 0x000115AA
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000125B3 File Offset: 0x000115B3
		// (set) Token: 0x0600026C RID: 620 RVA: 0x000125BB File Offset: 0x000115BB
		public CodeExpression SizeExpression
		{
			get
			{
				return this.sizeExpression;
			}
			set
			{
				this.sizeExpression = value;
			}
		}

		// Token: 0x040007D6 RID: 2006
		private CodeTypeReference createType;

		// Token: 0x040007D7 RID: 2007
		private CodeExpressionCollection initializers = new CodeExpressionCollection();

		// Token: 0x040007D8 RID: 2008
		private CodeExpression sizeExpression;

		// Token: 0x040007D9 RID: 2009
		private int size;
	}
}
