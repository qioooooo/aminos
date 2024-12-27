using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003E6 RID: 998
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class DesignTimeTemplateParser
	{
		// Token: 0x06003065 RID: 12389 RVA: 0x000D5590 File Offset: 0x000D4590
		public static Control ParseControl(DesignTimeParseData data)
		{
			Control[] array = DesignTimeTemplateParser.ParseControlsInternal(data, true);
			if (array.Length > 0)
			{
				return array[0];
			}
			return null;
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000D55B0 File Offset: 0x000D45B0
		public static Control[] ParseControls(DesignTimeParseData data)
		{
			return DesignTimeTemplateParser.ParseControlsInternal(data, false);
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000D55BC File Offset: 0x000D45BC
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		internal static Control[] ParseControlsInternal(DesignTimeParseData data, bool returnFirst)
		{
			TemplateParser templateParser = new PageParser();
			templateParser.FInDesigner = true;
			templateParser.DesignerHost = data.DesignerHost;
			templateParser.DesignTimeDataBindHandler = data.DataBindingHandler;
			templateParser.Text = data.ParseText;
			templateParser.Parse();
			ArrayList arrayList = new ArrayList();
			ArrayList subBuilders = templateParser.RootBuilder.SubBuilders;
			if (subBuilders != null)
			{
				IEnumerator enumerator = subBuilders.GetEnumerator();
				int num = 0;
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (obj is ControlBuilder && !(obj is CodeBlockBuilder))
					{
						ControlBuilder controlBuilder = (ControlBuilder)obj;
						IServiceProvider serviceProvider;
						if (data.DesignerHost != null)
						{
							serviceProvider = data.DesignerHost;
						}
						else
						{
							ServiceContainer serviceContainer = new ServiceContainer();
							serviceContainer.AddService(typeof(IFilterResolutionService), new DesignTimeTemplateParser.SimpleDesignTimeFilterResolutionService(data.Filter));
							serviceProvider = serviceContainer;
						}
						controlBuilder.SetServiceProvider(serviceProvider);
						try
						{
							Control control = (Control)controlBuilder.BuildObject(data.ShouldApplyTheme);
							arrayList.Add(control);
						}
						finally
						{
							controlBuilder.SetServiceProvider(null);
						}
						if (returnFirst)
						{
							break;
						}
					}
					else if (!returnFirst && obj is string)
					{
						LiteralControl literalControl = new LiteralControl(obj.ToString());
						arrayList.Add(literalControl);
					}
					num++;
				}
			}
			data.SetUserControlRegisterEntries(templateParser.UserControlRegisterEntries, templateParser.TagRegisterEntries);
			return (Control[])arrayList.ToArray(typeof(Control));
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000D572C File Offset: 0x000D472C
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public static ITemplate ParseTemplate(DesignTimeParseData data)
		{
			TemplateParser templateParser = new PageParser();
			templateParser.FInDesigner = true;
			templateParser.DesignerHost = data.DesignerHost;
			templateParser.DesignTimeDataBindHandler = data.DataBindingHandler;
			templateParser.Text = data.ParseText;
			templateParser.Parse();
			templateParser.RootBuilder.Text = data.ParseText;
			templateParser.RootBuilder.SetDesignerHost(data.DesignerHost);
			return templateParser.RootBuilder;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000D5798 File Offset: 0x000D4798
		public static ControlBuilder ParseTheme(IDesignerHost host, string theme, string themePath)
		{
			ControlBuilder rootBuilder;
			try
			{
				TemplateParser templateParser = new DesignTimePageThemeParser(themePath);
				templateParser.FInDesigner = true;
				templateParser.DesignerHost = host;
				templateParser.ThrowOnFirstParseError = true;
				templateParser.Text = theme;
				templateParser.Parse();
				rootBuilder = templateParser.RootBuilder;
			}
			catch (Exception ex)
			{
				throw new Exception(SR.GetString("DesignTimeTemplateParser_ErrorParsingTheme") + " " + ex.Message);
			}
			return rootBuilder;
		}

		// Token: 0x020003E7 RID: 999
		private class SimpleDesignTimeFilterResolutionService : IFilterResolutionService
		{
			// Token: 0x0600306A RID: 12394 RVA: 0x000D580C File Offset: 0x000D480C
			public SimpleDesignTimeFilterResolutionService(string filter)
			{
				this._currentFilter = filter;
			}

			// Token: 0x0600306B RID: 12395 RVA: 0x000D581B File Offset: 0x000D481B
			bool IFilterResolutionService.EvaluateFilter(string filterName)
			{
				return string.IsNullOrEmpty(filterName) || StringUtil.EqualsIgnoreCase((this._currentFilter == null) ? string.Empty : this._currentFilter, filterName);
			}

			// Token: 0x0600306C RID: 12396 RVA: 0x000D5847 File Offset: 0x000D4847
			int IFilterResolutionService.CompareFilters(string filter1, string filter2)
			{
				if (string.IsNullOrEmpty(filter1))
				{
					if (!string.IsNullOrEmpty(filter2))
					{
						return 1;
					}
					return 0;
				}
				else
				{
					if (string.IsNullOrEmpty(filter2))
					{
						return -1;
					}
					return 0;
				}
			}

			// Token: 0x0400221D RID: 8733
			private string _currentFilter;
		}
	}
}
