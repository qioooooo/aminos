using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200039A RID: 922
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BindableTemplateBuilder : TemplateBuilder, IBindableTemplate, ITemplate
	{
		// Token: 0x06002D09 RID: 11529 RVA: 0x000CA4C8 File Offset: 0x000C94C8
		private IOrderedDictionary ExtractTemplateValuesMethod(Control container)
		{
			OrderedDictionary orderedDictionary = new OrderedDictionary();
			if (this != null)
			{
				this.ExtractTemplateValuesRecursive(this.SubBuilders, orderedDictionary, container);
			}
			return orderedDictionary;
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000CA4F0 File Offset: 0x000C94F0
		private void ExtractTemplateValuesRecursive(ArrayList subBuilders, OrderedDictionary table, Control container)
		{
			foreach (object obj in subBuilders)
			{
				ControlBuilder controlBuilder = obj as ControlBuilder;
				if (controlBuilder != null)
				{
					ICollection collection;
					if (!controlBuilder.HasFilteredBoundEntries)
					{
						collection = controlBuilder.BoundPropertyEntries;
					}
					else
					{
						ServiceContainer serviceContainer = new ServiceContainer();
						serviceContainer.AddService(typeof(IFilterResolutionService), controlBuilder.TemplateControl);
						try
						{
							controlBuilder.SetServiceProvider(serviceContainer);
							collection = controlBuilder.GetFilteredPropertyEntrySet(controlBuilder.BoundPropertyEntries);
						}
						finally
						{
							controlBuilder.SetServiceProvider(null);
						}
					}
					string text = null;
					Control control = null;
					foreach (object obj2 in collection)
					{
						BoundPropertyEntry boundPropertyEntry = (BoundPropertyEntry)obj2;
						if (boundPropertyEntry.TwoWayBound)
						{
							bool flag = string.Compare(text, boundPropertyEntry.ControlID, StringComparison.Ordinal) != 0;
							text = boundPropertyEntry.ControlID;
							if (flag)
							{
								control = container.FindControl(boundPropertyEntry.ControlID);
								if (control == null || !boundPropertyEntry.ControlType.IsInstanceOfType(control))
								{
									continue;
								}
							}
							string text2;
							object obj3 = PropertyMapper.LocatePropertyObject(control, boundPropertyEntry.Name, out text2, base.InDesigner);
							table[boundPropertyEntry.FieldName] = FastPropertyAccessor.GetProperty(obj3, text2, base.InDesigner);
						}
					}
					this.ExtractTemplateValuesRecursive(controlBuilder.SubBuilders, table, container);
				}
			}
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000CA6B4 File Offset: 0x000C96B4
		public IOrderedDictionary ExtractValues(Control container)
		{
			if (this._extractTemplateValuesMethod != null && !base.InDesigner)
			{
				return this._extractTemplateValuesMethod(container);
			}
			return new OrderedDictionary();
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000CA6D8 File Offset: 0x000C96D8
		public override void OnAppendToParentBuilder(ControlBuilder parentBuilder)
		{
			base.OnAppendToParentBuilder(parentBuilder);
			if (base.HasTwoWayBoundProperties)
			{
				this._extractTemplateValuesMethod = new ExtractTemplateValuesMethod(this.ExtractTemplateValuesMethod);
			}
		}

		// Token: 0x040020D5 RID: 8405
		private ExtractTemplateValuesMethod _extractTemplateValuesMethod;
	}
}
