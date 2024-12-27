using System;

namespace System.Web.UI
{
	// Token: 0x02000448 RID: 1096
	internal class FileLevelPageThemeBuilder : RootBuilder
	{
		// Token: 0x0600343C RID: 13372 RVA: 0x000E2F58 File Offset: 0x000E1F58
		public override void AppendLiteralString(string s)
		{
			if (s != null && !Util.IsWhiteSpaceString(s))
			{
				throw new HttpException(SR.GetString("Literal_content_not_allowed", new object[]
				{
					SR.GetString("Page_theme_skin_file"),
					s.Trim()
				}));
			}
			base.AppendLiteralString(s);
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x000E2FA8 File Offset: 0x000E1FA8
		public override void AppendSubBuilder(ControlBuilder subBuilder)
		{
			Type controlType = subBuilder.ControlType;
			if (!typeof(Control).IsAssignableFrom(controlType))
			{
				throw new HttpException(SR.GetString("Page_theme_only_controls_allowed", new object[] { (controlType == null) ? string.Empty : controlType.ToString() }));
			}
			if (base.InPageTheme && !ThemeableAttribute.IsTypeThemeable(subBuilder.ControlType))
			{
				throw new HttpParseException(SR.GetString("Type_theme_disabled", new object[] { subBuilder.ControlType.FullName }), null, subBuilder.VirtualPath, null, subBuilder.Line);
			}
			base.AppendSubBuilder(subBuilder);
		}
	}
}
