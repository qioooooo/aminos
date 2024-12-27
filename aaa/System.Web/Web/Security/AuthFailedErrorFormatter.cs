using System;

namespace System.Web.Security
{
	// Token: 0x0200032C RID: 812
	internal class AuthFailedErrorFormatter : ErrorFormatter
	{
		// Token: 0x060027E4 RID: 10212 RVA: 0x000AEDB8 File Offset: 0x000ADDB8
		internal AuthFailedErrorFormatter()
		{
		}

		// Token: 0x060027E5 RID: 10213 RVA: 0x000AEDC0 File Offset: 0x000ADDC0
		internal static string GetErrorText()
		{
			if (AuthFailedErrorFormatter._strErrorText != null)
			{
				return AuthFailedErrorFormatter._strErrorText;
			}
			lock (AuthFailedErrorFormatter._syncObject)
			{
				if (AuthFailedErrorFormatter._strErrorText == null)
				{
					AuthFailedErrorFormatter._strErrorText = new AuthFailedErrorFormatter().GetErrorMessage();
				}
			}
			return AuthFailedErrorFormatter._strErrorText;
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x000AEE1C File Offset: 0x000ADE1C
		protected override string ErrorTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_Title");
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x060027E7 RID: 10215 RVA: 0x000AEE28 File Offset: 0x000ADE28
		protected override string Description
		{
			get
			{
				return SR.GetString("Assess_Denied_Description1");
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x000AEE34 File Offset: 0x000ADE34
		protected override string MiscSectionTitle
		{
			get
			{
				return SR.GetString("Assess_Denied_MiscTitle1");
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000AEE40 File Offset: 0x000ADE40
		protected override string MiscSectionContent
		{
			get
			{
				string @string = SR.GetString("Assess_Denied_MiscContent1");
				this.AdaptiveMiscContent.Add(@string);
				return @string;
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x000AEE66 File Offset: 0x000ADE66
		protected override string ColoredSquareTitle
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x000AEE69 File Offset: 0x000ADE69
		protected override string ColoredSquareContent
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x000AEE6C File Offset: 0x000ADE6C
		protected override bool ShowSourceFileInfo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001E71 RID: 7793
		private static string _strErrorText;

		// Token: 0x04001E72 RID: 7794
		private static object _syncObject = new object();
	}
}
