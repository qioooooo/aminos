using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200016E RID: 366
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerVerb : MenuCommand
	{
		// Token: 0x06000BE1 RID: 3041 RVA: 0x00028C15 File Offset: 0x00027C15
		public DesignerVerb(string text, EventHandler handler)
			: base(handler, StandardCommands.VerbFirst)
		{
			this.Properties["Text"] = ((text == null) ? null : Regex.Replace(text, "\\(\\&.\\)", ""));
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00028C49 File Offset: 0x00027C49
		public DesignerVerb(string text, EventHandler handler, CommandID startCommandID)
			: base(handler, startCommandID)
		{
			this.Properties["Text"] = ((text == null) ? null : Regex.Replace(text, "\\(\\&.\\)", ""));
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00028C7C File Offset: 0x00027C7C
		// (set) Token: 0x06000BE4 RID: 3044 RVA: 0x00028CA9 File Offset: 0x00027CA9
		public string Description
		{
			get
			{
				object obj = this.Properties["Description"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
			set
			{
				this.Properties["Description"] = value;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00028CBC File Offset: 0x00027CBC
		public string Text
		{
			get
			{
				object obj = this.Properties["Text"];
				if (obj == null)
				{
					return string.Empty;
				}
				return (string)obj;
			}
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00028CE9 File Offset: 0x00027CE9
		public override string ToString()
		{
			return this.Text + " : " + base.ToString();
		}
	}
}
