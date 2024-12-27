using System;

namespace System.Diagnostics
{
	// Token: 0x0200029B RID: 667
	internal class DefaultFilter : AssertFilter
	{
		// Token: 0x06001AB9 RID: 6841 RVA: 0x000469BF File Offset: 0x000459BF
		internal DefaultFilter()
		{
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x000469C7 File Offset: 0x000459C7
		public override AssertFilters AssertFailure(string condition, string message, StackTrace location)
		{
			return (AssertFilters)Assert.ShowDefaultAssertDialog(condition, message);
		}
	}
}
