using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000464 RID: 1124
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ThemeProvider
	{
		// Token: 0x0600352A RID: 13610 RVA: 0x000E637C File Offset: 0x000E537C
		public ThemeProvider(IDesignerHost host, string name, string themeDefinition, string[] cssFiles, string themePath)
		{
			this._themeName = name;
			this._themePath = themePath;
			this._cssFiles = cssFiles;
			this._host = host;
			ControlBuilder controlBuilder = DesignTimeTemplateParser.ParseTheme(host, themeDefinition, themePath);
			this._contentHashCode = themeDefinition.GetHashCode();
			ArrayList subBuilders = controlBuilder.SubBuilders;
			this._skinBuilders = new Hashtable();
			for (int i = 0; i < subBuilders.Count; i++)
			{
				ControlBuilder controlBuilder2 = subBuilders[i] as ControlBuilder;
				if (controlBuilder2 != null)
				{
					IDictionary dictionary = this._skinBuilders[controlBuilder2.ControlType] as IDictionary;
					if (dictionary == null)
					{
						dictionary = new SortedList(StringComparer.OrdinalIgnoreCase);
						this._skinBuilders[controlBuilder2.ControlType] = dictionary;
					}
					Control control = controlBuilder2.BuildObject() as Control;
					if (control != null)
					{
						dictionary[control.SkinID] = controlBuilder2;
					}
				}
			}
		}

		// Token: 0x17000BE2 RID: 3042
		// (get) Token: 0x0600352B RID: 13611 RVA: 0x000E6451 File Offset: 0x000E5451
		public int ContentHashCode
		{
			get
			{
				return this._contentHashCode;
			}
		}

		// Token: 0x17000BE3 RID: 3043
		// (get) Token: 0x0600352C RID: 13612 RVA: 0x000E6459 File Offset: 0x000E5459
		public ICollection CssFiles
		{
			get
			{
				return this._cssFiles;
			}
		}

		// Token: 0x17000BE4 RID: 3044
		// (get) Token: 0x0600352D RID: 13613 RVA: 0x000E6461 File Offset: 0x000E5461
		public IDesignerHost DesignerHost
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x17000BE5 RID: 3045
		// (get) Token: 0x0600352E RID: 13614 RVA: 0x000E6469 File Offset: 0x000E5469
		public string ThemeName
		{
			get
			{
				return this._themeName;
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x000E6474 File Offset: 0x000E5474
		public ICollection GetSkinsForControl(Type type)
		{
			IDictionary dictionary = this._skinBuilders[type] as IDictionary;
			if (dictionary == null)
			{
				return new ArrayList();
			}
			return dictionary.Keys;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x000E64A4 File Offset: 0x000E54A4
		public SkinBuilder GetSkinBuilder(Control control)
		{
			IDictionary dictionary = this._skinBuilders[control.GetType()] as IDictionary;
			if (dictionary == null)
			{
				return null;
			}
			ControlBuilder controlBuilder = dictionary[control.SkinID] as ControlBuilder;
			if (controlBuilder == null)
			{
				return null;
			}
			return new SkinBuilder(this, control, controlBuilder, this._themePath);
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x000E64F4 File Offset: 0x000E54F4
		public IDictionary GetSkinControlBuildersForControlType(Type type)
		{
			return this._skinBuilders[type] as IDictionary;
		}

		// Token: 0x04002523 RID: 9507
		private IDictionary _skinBuilders;

		// Token: 0x04002524 RID: 9508
		private string[] _cssFiles;

		// Token: 0x04002525 RID: 9509
		private string _themeName;

		// Token: 0x04002526 RID: 9510
		private string _themePath;

		// Token: 0x04002527 RID: 9511
		private int _contentHashCode;

		// Token: 0x04002528 RID: 9512
		private IDesignerHost _host;
	}
}
