using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000078 RID: 120
	[AttributeUsage(AttributeTargets.Assembly, Inherited = true)]
	[ComVisible(false)]
	public sealed class ApplicationIDAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600029A RID: 666 RVA: 0x000070AE File Offset: 0x000060AE
		public ApplicationIDAttribute(string guid)
		{
			this._value = new Guid(guid);
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600029B RID: 667 RVA: 0x000070C2 File Offset: 0x000060C2
		public Guid Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000070CA File Offset: 0x000060CA
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application";
		}

		// Token: 0x0600029D RID: 669 RVA: 0x000070D7 File Offset: 0x000060D7
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			return false;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000070DA File Offset: 0x000060DA
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x04000107 RID: 263
		private Guid _value;
	}
}
