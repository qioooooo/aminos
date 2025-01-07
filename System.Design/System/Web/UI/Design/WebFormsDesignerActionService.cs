using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Web.UI.Design
{
	public class WebFormsDesignerActionService : DesignerActionService
	{
		public WebFormsDesignerActionService(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		protected override void GetComponentDesignerActions(IComponent component, DesignerActionListCollection actionLists)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			if (actionLists == null)
			{
				throw new ArgumentNullException("actionLists");
			}
			IServiceContainer serviceContainer = component.Site as IServiceContainer;
			if (serviceContainer != null)
			{
				DesignerCommandSet designerCommandSet = (DesignerCommandSet)serviceContainer.GetService(typeof(DesignerCommandSet));
				if (designerCommandSet != null)
				{
					DesignerActionListCollection actionLists2 = designerCommandSet.ActionLists;
					if (actionLists2 != null)
					{
						actionLists.AddRange(actionLists2);
					}
				}
				if (actionLists.Count == 0 || (actionLists.Count == 1 && actionLists[0] is ControlDesigner.ControlDesignerActionList))
				{
					DesignerVerbCollection verbs = designerCommandSet.Verbs;
					if (verbs != null && verbs.Count != 0)
					{
						DesignerVerb[] array = new DesignerVerb[verbs.Count];
						verbs.CopyTo(array, 0);
						actionLists.Add(new DesignerActionVerbList(array));
					}
				}
			}
		}
	}
}
