using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000184 RID: 388
	internal class PageThemeBuildProvider : BaseTemplateBuildProvider
	{
		// Token: 0x060010BF RID: 4287 RVA: 0x0004AEC4 File Offset: 0x00049EC4
		internal PageThemeBuildProvider(VirtualPath virtualDirPath)
		{
			this._virtualDirPath = virtualDirPath;
			base.SetVirtualPath(virtualDirPath);
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0004AEDA File Offset: 0x00049EDA
		internal virtual string AssemblyNamePrefix
		{
			get
			{
				return "App_Theme_";
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0004AEE1 File Offset: 0x00049EE1
		internal void AddSkinFile(VirtualPath virtualPath)
		{
			if (this._skinFileList == null)
			{
				this._skinFileList = new StringCollection();
			}
			this._skinFileList.Add(virtualPath.VirtualPathString);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0004AF08 File Offset: 0x00049F08
		internal void AddCssFile(VirtualPath virtualPath)
		{
			if (this._cssFileList == null)
			{
				this._cssFileList = new ArrayList();
			}
			this._cssFileList.Add(virtualPath.AppRelativeVirtualPathString);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0004AF2F File Offset: 0x00049F2F
		protected override TemplateParser CreateParser()
		{
			if (this._cssFileList != null)
			{
				this._cssFileList.Sort();
			}
			return new PageThemeParser(this._virtualDirPath, this._skinFileList, this._cssFileList);
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x0004AF5B File Offset: 0x00049F5B
		internal override BaseCodeDomTreeGenerator CreateCodeDomTreeGenerator(TemplateParser parser)
		{
			return new PageThemeCodeDomTreeGenerator((PageThemeParser)parser);
		}

		// Token: 0x04001670 RID: 5744
		private VirtualPath _virtualDirPath;

		// Token: 0x04001671 RID: 5745
		private IList _skinFileList;

		// Token: 0x04001672 RID: 5746
		private ArrayList _cssFileList;
	}
}
