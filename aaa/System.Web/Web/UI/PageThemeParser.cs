using System;
using System.Collections;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x0200044B RID: 1099
	internal class PageThemeParser : BaseTemplateParser
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06003454 RID: 13396 RVA: 0x000E32D2 File Offset: 0x000E22D2
		internal VirtualPath VirtualDirPath
		{
			get
			{
				return this._virtualDirPath;
			}
		}

		// Token: 0x06003455 RID: 13397 RVA: 0x000E32DA File Offset: 0x000E22DA
		internal PageThemeParser(VirtualPath virtualDirPath, IList skinFileList, IList cssFileList)
		{
			this._virtualDirPath = virtualDirPath;
			this._skinFileList = skinFileList;
			this._cssFileList = cssFileList;
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06003456 RID: 13398 RVA: 0x000E32F7 File Offset: 0x000E22F7
		internal ICollection CssFileList
		{
			get
			{
				return this._cssFileList;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06003457 RID: 13399 RVA: 0x000E32FF File Offset: 0x000E22FF
		internal override Type DefaultBaseType
		{
			get
			{
				return typeof(PageTheme);
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06003458 RID: 13400 RVA: 0x000E330B File Offset: 0x000E230B
		internal override string DefaultDirectiveName
		{
			get
			{
				return "skin";
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06003459 RID: 13401 RVA: 0x000E3312 File Offset: 0x000E2312
		internal override bool IsCodeAllowed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x000E3315 File Offset: 0x000E2315
		// (set) Token: 0x0600345B RID: 13403 RVA: 0x000E331D File Offset: 0x000E231D
		internal ControlBuilder CurrentSkinBuilder
		{
			get
			{
				return this._currentSkinBuilder;
			}
			set
			{
				this._currentSkinBuilder = value;
			}
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x000E3326 File Offset: 0x000E2326
		internal override RootBuilder CreateDefaultFileLevelBuilder()
		{
			return new FileLevelPageThemeBuilder();
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x000E3330 File Offset: 0x000E2330
		internal override void ParseInternal()
		{
			if (this._skinFileList != null)
			{
				foreach (object obj in this._skinFileList)
				{
					string text = (string)obj;
					base.ParseFile(null, text);
				}
			}
			base.AddSourceDependency(this._virtualDirPath);
		}

		// Token: 0x0600345E RID: 13406 RVA: 0x000E33A0 File Offset: 0x000E23A0
		internal override void ProcessDirective(string directiveName, IDictionary directive)
		{
			if (directiveName == null || directiveName.Length == 0 || StringUtil.EqualsIgnoreCase(directiveName, this.DefaultDirectiveName))
			{
				if (this._mainDirectiveProcessed)
				{
					base.ProcessError(SR.GetString("Only_one_directive_allowed", new object[] { this.DefaultDirectiveName }));
					return;
				}
				this.ProcessMainDirective(directive);
				this._mainDirectiveProcessed = true;
				return;
			}
			else
			{
				if (StringUtil.EqualsIgnoreCase(directiveName, "register"))
				{
					base.ProcessDirective(directiveName, directive);
					return;
				}
				base.ProcessError(SR.GetString("Unknown_directive", new object[] { directiveName }));
				return;
			}
		}

		// Token: 0x0600345F RID: 13407 RVA: 0x000E3434 File Offset: 0x000E2434
		internal override bool ProcessMainDirectiveAttribute(string deviceName, string name, string value, IDictionary parseData)
		{
			if (name != null && (name == "classname" || name == "compilationmode" || name == "inherits"))
			{
				base.ProcessError(SR.GetString("Attr_not_supported_in_directive", new object[] { name, this.DefaultDirectiveName }));
				return false;
			}
			return base.ProcessMainDirectiveAttribute(deviceName, name, value, parseData);
		}

		// Token: 0x040024B0 RID: 9392
		internal const string defaultDirectiveName = "skin";

		// Token: 0x040024B1 RID: 9393
		private bool _mainDirectiveProcessed;

		// Token: 0x040024B2 RID: 9394
		private IList _skinFileList;

		// Token: 0x040024B3 RID: 9395
		private IList _cssFileList;

		// Token: 0x040024B4 RID: 9396
		private ControlBuilder _currentSkinBuilder;

		// Token: 0x040024B5 RID: 9397
		private VirtualPath _virtualDirPath;
	}
}
