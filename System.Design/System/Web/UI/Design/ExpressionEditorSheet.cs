using System;
using System.ComponentModel;

namespace System.Web.UI.Design
{
	public abstract class ExpressionEditorSheet
	{
		protected ExpressionEditorSheet(IServiceProvider serviceProvider)
		{
			this._serviceProvider = serviceProvider;
		}

		[Browsable(false)]
		public virtual bool IsValid
		{
			get
			{
				return true;
			}
		}

		[Browsable(false)]
		public IServiceProvider ServiceProvider
		{
			get
			{
				return this._serviceProvider;
			}
		}

		public abstract string GetExpression();

		private IServiceProvider _serviceProvider;
	}
}
