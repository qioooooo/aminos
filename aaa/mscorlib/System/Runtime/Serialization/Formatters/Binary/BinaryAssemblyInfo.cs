using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C1 RID: 1985
	internal sealed class BinaryAssemblyInfo
	{
		// Token: 0x060046DA RID: 18138 RVA: 0x000F36B7 File Offset: 0x000F26B7
		internal BinaryAssemblyInfo(string assemblyString)
		{
			this.assemblyString = assemblyString;
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x000F36C6 File Offset: 0x000F26C6
		internal BinaryAssemblyInfo(string assemblyString, Assembly assembly)
		{
			this.assemblyString = assemblyString;
			this.assembly = assembly;
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x000F36DC File Offset: 0x000F26DC
		internal Assembly GetAssembly()
		{
			if (this.assembly == null)
			{
				this.assembly = FormatterServices.LoadAssemblyFromStringNoThrow(this.assemblyString);
				if (this.assembly == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_AssemblyNotFound"), new object[] { this.assemblyString }));
				}
			}
			return this.assembly;
		}

		// Token: 0x0400239E RID: 9118
		internal string assemblyString;

		// Token: 0x0400239F RID: 9119
		private Assembly assembly;
	}
}
