using System;

namespace System.ComponentModel
{
	// Token: 0x02000137 RID: 311
	[AttributeUsage(AttributeTargets.Class)]
	public class RunInstallerAttribute : Attribute
	{
		// Token: 0x06000A37 RID: 2615 RVA: 0x00023CF0 File Offset: 0x00022CF0
		public RunInstallerAttribute(bool runInstaller)
		{
			this.runInstaller = runInstaller;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00023CFF File Offset: 0x00022CFF
		public bool RunInstaller
		{
			get
			{
				return this.runInstaller;
			}
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00023D08 File Offset: 0x00022D08
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RunInstallerAttribute runInstallerAttribute = obj as RunInstallerAttribute;
			return runInstallerAttribute != null && runInstallerAttribute.RunInstaller == this.runInstaller;
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00023D35 File Offset: 0x00022D35
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00023D3D File Offset: 0x00022D3D
		public override bool IsDefaultAttribute()
		{
			return this.Equals(RunInstallerAttribute.Default);
		}

		// Token: 0x04000A64 RID: 2660
		private bool runInstaller;

		// Token: 0x04000A65 RID: 2661
		public static readonly RunInstallerAttribute Yes = new RunInstallerAttribute(true);

		// Token: 0x04000A66 RID: 2662
		public static readonly RunInstallerAttribute No = new RunInstallerAttribute(false);

		// Token: 0x04000A67 RID: 2663
		public static readonly RunInstallerAttribute Default = RunInstallerAttribute.No;
	}
}
