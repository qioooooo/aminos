using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002A8 RID: 680
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Delegate, AllowMultiple = true)]
	public sealed class DebuggerDisplayAttribute : Attribute
	{
		// Token: 0x06001AD9 RID: 6873 RVA: 0x00046BBE File Offset: 0x00045BBE
		public DebuggerDisplayAttribute(string value)
		{
			if (value == null)
			{
				this.value = "";
			}
			else
			{
				this.value = value;
			}
			this.name = "";
			this.type = "";
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001ADA RID: 6874 RVA: 0x00046BF3 File Offset: 0x00045BF3
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x00046BFB File Offset: 0x00045BFB
		// (set) Token: 0x06001ADC RID: 6876 RVA: 0x00046C03 File Offset: 0x00045C03
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x00046C0C File Offset: 0x00045C0C
		// (set) Token: 0x06001ADE RID: 6878 RVA: 0x00046C14 File Offset: 0x00045C14
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001AE0 RID: 6880 RVA: 0x00046C40 File Offset: 0x00045C40
		// (set) Token: 0x06001ADF RID: 6879 RVA: 0x00046C1D File Offset: 0x00045C1D
		public Type Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.targetName = value.AssemblyQualifiedName;
				this.target = value;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x00046C48 File Offset: 0x00045C48
		// (set) Token: 0x06001AE2 RID: 6882 RVA: 0x00046C50 File Offset: 0x00045C50
		public string TargetTypeName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x04000A11 RID: 2577
		private string name;

		// Token: 0x04000A12 RID: 2578
		private string value;

		// Token: 0x04000A13 RID: 2579
		private string type;

		// Token: 0x04000A14 RID: 2580
		private string targetName;

		// Token: 0x04000A15 RID: 2581
		private Type target;
	}
}
