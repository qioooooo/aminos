using System;

namespace System.Web
{
	// Token: 0x02000024 RID: 36
	internal class StaticErrorFormatterHelper
	{
		// Token: 0x04000D3C RID: 3388
		internal const string ChtmlErrorBeginTemplate = "<html>\r\n<body>\r\n<form>\r\n<font color=\"Red\" size=\"5\">{0}</font><br/>\r\n<font color=\"Maroon\">{1}</font><br/>\r\n";

		// Token: 0x04000D3D RID: 3389
		internal const string ChtmlErrorEndTemplate = "</form>\r\n</body>\r\n</html>";

		// Token: 0x04000D3E RID: 3390
		internal const string WmlErrorBeginTemplate = "<?xml version='1.0'?>\r\n<!DOCTYPE wml PUBLIC '-//WAPFORUM//DTD WML 1.1//EN' 'http://www.wapforum.org/DTD/wml_1.1.xml'><wml><head>\r\n<meta http-equiv=\"Cache-Control\" content=\"max-age=0\" forua=\"true\"/>\r\n</head>\r\n<card>\r\n<p>\r\n<b><big>{0}</big></b><br/>\r\n<b><i>{1}</i></b><br/>\r\n";

		// Token: 0x04000D3F RID: 3391
		internal const string WmlErrorEndTemplate = "</p>\r\n</card>\r\n</wml>\r\n";

		// Token: 0x04000D40 RID: 3392
		internal const string XhtmlErrorBeginTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<title></title>\r\n</head>\r\n<body>\r\n<form>\r\n<div>\r\n<span style=\"color:Red;font-size:Large;font-weight:bold;\">{0}</span><br/>\r\n<span style=\"color:Maroon;font-weight:bold;font-style:italic;\">{1}</span><br/>\r\n";

		// Token: 0x04000D41 RID: 3393
		internal const string XhtmlErrorEndTemplate = "</div>\r\n</form>\r\n</body>\r\n</html>";

		// Token: 0x04000D42 RID: 3394
		internal const string Break = "<br/>\r\n";
	}
}
