using System;

namespace System.Web
{
	// Token: 0x0200001E RID: 30
	internal class GenericApplicationErrorFormatter : ErrorFormatter
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00004920 File Offset: 0x00003920
		internal GenericApplicationErrorFormatter(bool local)
		{
			this._local = local;
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000492F File Offset: 0x0000392F
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Generic_Err_Title");
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000493B File Offset: 0x0000393B
		protected override string Description
		{
			get
			{
				return SR.GetString(this._local ? "Generic_Err_Local_Desc" : "Generic_Err_Remote_Desc");
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004956 File Offset: 0x00003956
		protected override string MiscSectionTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004959 File Offset: 0x00003959
		protected override string MiscSectionContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000495C File Offset: 0x0000395C
		protected override string ColoredSquareTitle
		{
			get
			{
				string @string = SR.GetString("Generic_Err_Details_Title");
				this.AdaptiveMiscContent.Add(@string);
				return @string;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004984 File Offset: 0x00003984
		protected override string ColoredSquareDescription
		{
			get
			{
				string text = SR.GetString(this._local ? "Generic_Err_Local_Details_Desc" : "Generic_Err_Remote_Details_Desc");
				text = HttpUtility.HtmlEncode(text);
				this.AdaptiveMiscContent.Add(text);
				return text;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000049C0 File Offset: 0x000039C0
		protected override string ColoredSquareContent
		{
			get
			{
				string text = HttpUtility.HtmlEncode(SR.GetString(this._local ? "Generic_Err_Local_Details_Sample" : "Generic_Err_Remote_Details_Sample"));
				return base.WrapWithLeftToRightTextFormatIfNeeded(text);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000049F4 File Offset: 0x000039F4
		protected override string ColoredSquare2Title
		{
			get
			{
				string @string = SR.GetString("Generic_Err_Notes_Title");
				this.AdaptiveMiscContent.Add(@string);
				return @string;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004A1C File Offset: 0x00003A1C
		protected override string ColoredSquare2Description
		{
			get
			{
				string text = SR.GetString("Generic_Err_Notes_Desc");
				text = HttpUtility.HtmlEncode(text);
				this.AdaptiveMiscContent.Add(text);
				return text;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004A4C File Offset: 0x00003A4C
		protected override string ColoredSquare2Content
		{
			get
			{
				string text = HttpUtility.HtmlEncode(SR.GetString(this._local ? "Generic_Err_Local_Notes_Sample" : "Generic_Err_Remote_Notes_Sample"));
				return base.WrapWithLeftToRightTextFormatIfNeeded(text);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004A7F File Offset: 0x00003A7F
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004A82 File Offset: 0x00003A82
		internal override bool CanBeShownToAllUsers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000D28 RID: 3368
		private bool _local;
	}
}
