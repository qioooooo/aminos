using System;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing
{
	// Token: 0x02000123 RID: 291
	[Serializable]
	public sealed class PrintingPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x0002D90D File Offset: 0x0002C90D
		public PrintingPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.printingLevel = PrintingPermissionLevel.AllPrinting;
				return;
			}
			if (state == PermissionState.None)
			{
				this.printingLevel = PrintingPermissionLevel.NoPrinting;
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidPermissionState"));
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0002D93B File Offset: 0x0002C93B
		public PrintingPermission(PrintingPermissionLevel printingLevel)
		{
			PrintingPermission.VerifyPrintingLevel(printingLevel);
			this.printingLevel = printingLevel;
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0002D950 File Offset: 0x0002C950
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x0002D958 File Offset: 0x0002C958
		public PrintingPermissionLevel Level
		{
			get
			{
				return this.printingLevel;
			}
			set
			{
				PrintingPermission.VerifyPrintingLevel(value);
				this.printingLevel = value;
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0002D967 File Offset: 0x0002C967
		private static void VerifyPrintingLevel(PrintingPermissionLevel level)
		{
			if (level < PrintingPermissionLevel.NoPrinting || level > PrintingPermissionLevel.AllPrinting)
			{
				throw new ArgumentException(SR.GetString("InvalidPermissionLevel"));
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x0002D981 File Offset: 0x0002C981
		public bool IsUnrestricted()
		{
			return this.printingLevel == PrintingPermissionLevel.AllPrinting;
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x0002D98C File Offset: 0x0002C98C
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.printingLevel == PrintingPermissionLevel.NoPrinting;
			}
			PrintingPermission printingPermission = target as PrintingPermission;
			if (printingPermission == null)
			{
				throw new ArgumentException(SR.GetString("TargetNotPrintingPermission"));
			}
			return this.printingLevel <= printingPermission.printingLevel;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0002D9D4 File Offset: 0x0002C9D4
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			PrintingPermission printingPermission = target as PrintingPermission;
			if (printingPermission == null)
			{
				throw new ArgumentException(SR.GetString("TargetNotPrintingPermission"));
			}
			PrintingPermissionLevel printingPermissionLevel = ((this.printingLevel < printingPermission.printingLevel) ? this.printingLevel : printingPermission.printingLevel);
			if (printingPermissionLevel == PrintingPermissionLevel.NoPrinting)
			{
				return null;
			}
			return new PrintingPermission(printingPermissionLevel);
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0002DA28 File Offset: 0x0002CA28
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			PrintingPermission printingPermission = target as PrintingPermission;
			if (printingPermission == null)
			{
				throw new ArgumentException(SR.GetString("TargetNotPrintingPermission"));
			}
			PrintingPermissionLevel printingPermissionLevel = ((this.printingLevel > printingPermission.printingLevel) ? this.printingLevel : printingPermission.printingLevel);
			if (printingPermissionLevel == PrintingPermissionLevel.NoPrinting)
			{
				return null;
			}
			return new PrintingPermission(printingPermissionLevel);
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0002DA81 File Offset: 0x0002CA81
		public override IPermission Copy()
		{
			return new PrintingPermission(this.printingLevel);
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0002DA90 File Offset: 0x0002CA90
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Level", Enum.GetName(typeof(PrintingPermissionLevel), this.printingLevel));
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0002DB38 File Offset: 0x0002CB38
		public override void FromXml(SecurityElement esd)
		{
			if (esd == null)
			{
				throw new ArgumentNullException("esd");
			}
			string text = esd.Attribute("class");
			if (text == null || text.IndexOf(base.GetType().FullName) == -1)
			{
				throw new ArgumentException(SR.GetString("InvalidClassName"));
			}
			string text2 = esd.Attribute("Unrestricted");
			if (text2 != null && string.Equals(text2, "true", StringComparison.OrdinalIgnoreCase))
			{
				this.printingLevel = PrintingPermissionLevel.AllPrinting;
				return;
			}
			this.printingLevel = PrintingPermissionLevel.NoPrinting;
			string text3 = esd.Attribute("Level");
			if (text3 != null)
			{
				this.printingLevel = (PrintingPermissionLevel)Enum.Parse(typeof(PrintingPermissionLevel), text3);
			}
		}

		// Token: 0x04000C67 RID: 3175
		private PrintingPermissionLevel printingLevel;
	}
}
