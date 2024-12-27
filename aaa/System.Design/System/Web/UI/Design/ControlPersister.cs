using System;
using System.ComponentModel.Design;
using System.IO;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200033B RID: 827
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ControlPersister
	{
		// Token: 0x06001F3B RID: 7995 RVA: 0x000B022F File Offset: 0x000AF22F
		private ControlPersister()
		{
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000B0237 File Offset: 0x000AF237
		public static string PersistInnerProperties(object component, IDesignerHost host)
		{
			return ControlSerializer.SerializeInnerProperties(component, host);
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x000B0240 File Offset: 0x000AF240
		public static void PersistInnerProperties(TextWriter sw, object component, IDesignerHost host)
		{
			ControlSerializer.SerializeInnerProperties(component, host, sw);
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x000B024A File Offset: 0x000AF24A
		public static string PersistControl(Control control)
		{
			return ControlSerializer.SerializeControl(control);
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x000B0252 File Offset: 0x000AF252
		public static string PersistControl(Control control, IDesignerHost host)
		{
			return ControlSerializer.SerializeControl(control, host);
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000B025B File Offset: 0x000AF25B
		public static void PersistControl(TextWriter sw, Control control)
		{
			ControlSerializer.SerializeControl(control, sw);
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x000B0264 File Offset: 0x000AF264
		public static void PersistControl(TextWriter sw, Control control, IDesignerHost host)
		{
			ControlSerializer.SerializeControl(control, host, sw);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000B026E File Offset: 0x000AF26E
		public static string PersistTemplate(ITemplate template, IDesignerHost host)
		{
			return ControlSerializer.SerializeTemplate(template, host);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000B0277 File Offset: 0x000AF277
		public static void PersistTemplate(TextWriter writer, ITemplate template, IDesignerHost host)
		{
			ControlSerializer.SerializeTemplate(template, writer, host);
		}
	}
}
