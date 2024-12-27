using System;
using System.ComponentModel;

namespace System.Web.UI.Design
{
	// Token: 0x0200031A RID: 794
	public abstract class ExpressionEditorSheet
	{
		// Token: 0x06001E05 RID: 7685 RVA: 0x000AB7B2 File Offset: 0x000AA7B2
		protected ExpressionEditorSheet(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x000AB7C1 File Offset: 0x000AA7C1
		[Browsable(false)]
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x000AB7C4 File Offset: 0x000AA7C4
		[Browsable(false)]
		public IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		// Token: 0x06001E08 RID: 7688
		public abstract string GetExpression();

		// Token: 0x04001725 RID: 5925
		private IServiceProvider _serviceProvider;
	}
}
