using System;

namespace System.Web.UI
{
	// Token: 0x020003D3 RID: 979
	internal class DataBoundLiteralControlBuilder : ControlBuilder
	{
		// Token: 0x06002FC7 RID: 12231 RVA: 0x000D4515 File Offset: 0x000D3515
		internal DataBoundLiteralControlBuilder()
		{
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000D4520 File Offset: 0x000D3520
		internal void AddLiteralString(string s)
		{
			object lastBuilder = base.GetLastBuilder();
			if (lastBuilder != null && lastBuilder is string)
			{
				base.AddSubBuilder(null);
			}
			base.AddSubBuilder(s);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000D4550 File Offset: 0x000D3550
		internal void AddDataBindingExpression(CodeBlockBuilder codeBlockBuilder)
		{
			object lastBuilder = base.GetLastBuilder();
			if (lastBuilder == null || lastBuilder is CodeBlockBuilder)
			{
				base.AddSubBuilder(null);
			}
			base.AddSubBuilder(codeBlockBuilder);
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000D457D File Offset: 0x000D357D
		internal int GetStaticLiteralsCount()
		{
			return (base.SubBuilders.Count + 1) / 2;
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x000D458E File Offset: 0x000D358E
		internal int GetDataBoundLiteralCount()
		{
			return base.SubBuilders.Count / 2;
		}
	}
}
