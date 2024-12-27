using System;

namespace System.Web.Security
{
	// Token: 0x02000333 RID: 819
	internal class FileAccessFailedErrorFormatter : ErrorFormatter
	{
		// Token: 0x06002810 RID: 10256 RVA: 0x000AFE28 File Offset: 0x000AEE28
		internal FileAccessFailedErrorFormatter(string strFile)
		{
			this._strFile = strFile;
			if (this._strFile == null)
			{
				this._strFile = string.Empty;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002811 RID: 10257 RVA: 0x000AFE4A File Offset: 0x000AEE4A
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_Title");
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000AFE56 File Offset: 0x000AEE56
		protected override string Description
		{
			get
			{
				return SR.GetString("Assess_Denied_Description3");
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002813 RID: 10259 RVA: 0x000AFE62 File Offset: 0x000AEE62
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_Section_Title3");
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002814 RID: 10260 RVA: 0x000AFE70 File Offset: 0x000AEE70
		protected override string MiscSectionContent
		{
			get
			{
				string text;
				if (this._strFile.Length > 0)
				{
					text = SR.GetString("Assess_Denied_Misc_Content3", new object[] { HttpRuntime.GetSafePath(this._strFile) });
				}
				else
				{
					text = SR.GetString("Assess_Denied_Misc_Content3_2");
				}
				this.AdaptiveMiscContent.Add(text);
				return text;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x000AFEC7 File Offset: 0x000AEEC7
		protected override string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x000AFECA File Offset: 0x000AEECA
		protected override string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x000AFECD File Offset: 0x000AEECD
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001E8A RID: 7818
		private string _strFile;
	}
}
