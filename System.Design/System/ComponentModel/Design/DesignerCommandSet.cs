using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	public class DesignerCommandSet
	{
		public virtual ICollection GetCommands(string name)
		{
			return null;
		}

		public DesignerVerbCollection Verbs
		{
			get
			{
				return (DesignerVerbCollection)this.GetCommands("Verbs");
			}
		}

		public DesignerActionListCollection ActionLists
		{
			get
			{
				return (DesignerActionListCollection)this.GetCommands("ActionLists");
			}
		}
	}
}
