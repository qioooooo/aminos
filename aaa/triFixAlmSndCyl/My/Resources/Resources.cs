using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl.My.Resources
{
	// Token: 0x02000017 RID: 23
	[StandardModule]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[HideModuleName]
	[DebuggerNonUserCode]
	internal sealed class Resources
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00006494 File Offset: 0x00004894
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("triFixAlmSndCyl.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600023D RID: 573 RVA: 0x000064D4 File Offset: 0x000048D4
		// (set) Token: 0x0600023E RID: 574 RVA: 0x000064E8 File Offset: 0x000048E8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x04000126 RID: 294
		private static ResourceManager resourceMan;

		// Token: 0x04000127 RID: 295
		private static CultureInfo resourceCulture;
	}
}
