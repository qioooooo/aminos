using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002CF RID: 719
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyConfigurationAttribute : Attribute
	{
		// Token: 0x06001CB4 RID: 7348 RVA: 0x0004998E File Offset: 0x0004898E
		public AssemblyConfigurationAttribute(string configuration)
		{
			this.m_configuration = configuration;
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x0004999D File Offset: 0x0004899D
		public string Configuration
		{
			get
			{
				return this.m_configuration;
			}
		}

		// Token: 0x04000A80 RID: 2688
		private string m_configuration;
	}
}
