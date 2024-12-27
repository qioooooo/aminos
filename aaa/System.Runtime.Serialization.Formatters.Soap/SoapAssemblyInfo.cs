using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200000D RID: 13
	internal sealed class SoapAssemblyInfo
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000051B3 File Offset: 0x000041B3
		internal SoapAssemblyInfo(string assemblyString)
		{
			this.assemblyString = assemblyString;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000051C2 File Offset: 0x000041C2
		internal SoapAssemblyInfo(string assemblyString, Assembly assembly)
		{
			this.assemblyString = assemblyString;
			this.assembly = assembly;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000051D8 File Offset: 0x000041D8
		internal Assembly GetAssembly(ObjectReader objectReader)
		{
			if (this.assembly == null)
			{
				this.assembly = objectReader.LoadAssemblyFromString(this.assemblyString);
				if (this.assembly == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_AssemblyString"), new object[] { this.assemblyString }));
				}
			}
			return this.assembly;
		}

		// Token: 0x0400004B RID: 75
		internal string assemblyString;

		// Token: 0x0400004C RID: 76
		private Assembly assembly;
	}
}
