using System;
using System.IO;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x02000074 RID: 116
	internal class SUDSGenerator
	{
		// Token: 0x0600038F RID: 911 RVA: 0x00010DE3 File Offset: 0x0000FDE3
		internal SUDSGenerator(Type[] types, SdlType sdlType, TextWriter output)
		{
			this.wsdlGenerator = new WsdlGenerator(types, sdlType, output);
			this.sdlType = sdlType;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00010E00 File Offset: 0x0000FE00
		internal SUDSGenerator(ServiceType[] serviceTypes, SdlType sdlType, TextWriter output)
		{
			this.wsdlGenerator = new WsdlGenerator(serviceTypes, sdlType, output);
			this.sdlType = sdlType;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00010E1D File Offset: 0x0000FE1D
		internal void Generate()
		{
			this.wsdlGenerator.Generate();
		}

		// Token: 0x0400028B RID: 651
		private WsdlGenerator wsdlGenerator;

		// Token: 0x0400028C RID: 652
		private SdlType sdlType;
	}
}
