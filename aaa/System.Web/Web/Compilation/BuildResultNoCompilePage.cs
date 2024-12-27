using System;
using System.Collections;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200014F RID: 335
	internal class BuildResultNoCompilePage : BuildResultNoCompileTemplateControl
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x0004514C File Offset: 0x0004414C
		internal BuildResultNoCompilePage(Type baseType, TemplateParser parser)
			: base(baseType, parser)
		{
			PageParser pageParser = (PageParser)parser;
			this._traceEnabled = pageParser.TraceEnabled;
			this._traceMode = pageParser.TraceMode;
			if (pageParser.OutputCacheParameters != null)
			{
				this._outputCacheData = pageParser.OutputCacheParameters;
				if (this._outputCacheData.Duration == 0 || this._outputCacheData.Location == OutputCacheLocation.None)
				{
					this._outputCacheData = null;
				}
				else
				{
					this._fileDependencies = new string[pageParser.SourceDependencies.Count];
					int num = 0;
					foreach (object obj in ((IEnumerable)pageParser.SourceDependencies))
					{
						string text = (string)obj;
						this._fileDependencies[num++] = text;
					}
				}
			}
			this._validateRequest = pageParser.ValidateRequest;
			this._stylesheetTheme = pageParser.StyleSheetTheme;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00045240 File Offset: 0x00044240
		internal override void FrameworkInitialize(TemplateControl templateControl)
		{
			Page page = (Page)templateControl;
			page.StyleSheetTheme = this._stylesheetTheme;
			page.InitializeStyleSheet();
			base.FrameworkInitialize(templateControl);
			if (this._traceEnabled != TraceEnable.Default)
			{
				page.TraceEnabled = this._traceEnabled == TraceEnable.Enable;
			}
			if (this._traceMode != TraceMode.Default)
			{
				page.TraceModeValue = this._traceMode;
			}
			if (this._outputCacheData != null)
			{
				page.AddWrappedFileDependencies(this._fileDependencies);
				page.InitOutputCache(this._outputCacheData);
			}
			if (this._validateRequest)
			{
				page.Request.ValidateInput();
			}
		}

		// Token: 0x040015EA RID: 5610
		private TraceEnable _traceEnabled;

		// Token: 0x040015EB RID: 5611
		private TraceMode _traceMode;

		// Token: 0x040015EC RID: 5612
		private OutputCacheParameters _outputCacheData;

		// Token: 0x040015ED RID: 5613
		private string[] _fileDependencies;

		// Token: 0x040015EE RID: 5614
		private bool _validateRequest;

		// Token: 0x040015EF RID: 5615
		private string _stylesheetTheme;
	}
}
