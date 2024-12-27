using System;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000E3 RID: 227
	internal class Output
	{
		// Token: 0x06000A95 RID: 2709 RVA: 0x00033258 File Offset: 0x00032258
		public Output()
		{
			this.Settings = new XmlWriterSettings();
			this.Settings.OutputMethod = XmlOutputMethod.AutoDetect;
			this.Settings.AutoXmlDeclaration = true;
			this.Settings.ConformanceLevel = ConformanceLevel.Auto;
			this.Settings.MergeCDataSections = true;
		}

		// Token: 0x04000701 RID: 1793
		public const int NeverDeclaredPrec = -2147483648;

		// Token: 0x04000702 RID: 1794
		public XmlWriterSettings Settings;

		// Token: 0x04000703 RID: 1795
		public string Version;

		// Token: 0x04000704 RID: 1796
		public string Encoding;

		// Token: 0x04000705 RID: 1797
		public XmlQualifiedName Method;

		// Token: 0x04000706 RID: 1798
		public int MethodPrec = int.MinValue;

		// Token: 0x04000707 RID: 1799
		public int VersionPrec = int.MinValue;

		// Token: 0x04000708 RID: 1800
		public int EncodingPrec = int.MinValue;

		// Token: 0x04000709 RID: 1801
		public int OmitXmlDeclarationPrec = int.MinValue;

		// Token: 0x0400070A RID: 1802
		public int StandalonePrec = int.MinValue;

		// Token: 0x0400070B RID: 1803
		public int DocTypePublicPrec = int.MinValue;

		// Token: 0x0400070C RID: 1804
		public int DocTypeSystemPrec = int.MinValue;

		// Token: 0x0400070D RID: 1805
		public int IndentPrec = int.MinValue;

		// Token: 0x0400070E RID: 1806
		public int MediaTypePrec = int.MinValue;
	}
}
