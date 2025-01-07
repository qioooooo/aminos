using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	public class DesignerAutoFormatStyle : Style
	{
		public VerticalAlign VerticalAlign
		{
			get
			{
				return this._verticalAlign;
			}
			set
			{
				this._verticalAlign = value;
			}
		}

		private VerticalAlign _verticalAlign;
	}
}
