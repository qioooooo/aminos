using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Web.UI.Design
{
	// Token: 0x020003BC RID: 956
	public class WebFormsDesignerActionService : DesignerActionService
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x000BE6D2 File Offset: 0x000BD6D2
		public WebFormsDesignerActionService(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000BE6DC File Offset: 0x000BD6DC
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
