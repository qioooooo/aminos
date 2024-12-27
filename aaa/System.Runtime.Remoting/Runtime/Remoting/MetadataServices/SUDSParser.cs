using System;
using System.Collections;
using System.IO;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x02000072 RID: 114
	internal class SUDSParser
	{
		// Token: 0x0600038B RID: 907 RVA: 0x00010DA7 File Offset: 0x0000FDA7
		internal SUDSParser(TextReader input, string outputDir, ArrayList outCodeStreamList, string locationURL, bool bWrappedProxy, string proxyNamespace)
		{
			this.wsdlParser = new WsdlParser(input, outputDir, outCodeStreamList, locationURL, bWrappedProxy, proxyNamespace);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00010DC3 File Offset: 0x0000FDC3
		internal void Parse()
		{
			this.wsdlParser.Parse();
		}

		// Token: 0x0400028A RID: 650
		private WsdlParser wsdlParser;
	}
}
