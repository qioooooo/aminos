using System;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200033A RID: 826
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ControlParser
	{
		// Token: 0x06001F34 RID: 7988 RVA: 0x000B0123 File Offset: 0x000AF123
		private ControlParser()
		{
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000B012B File Offset: 0x000AF12B
		public static Control ParseControl(IDesignerHost designerHost, string controlText)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			if (controlText == null || controlText.Length == 0)
			{
				throw new ArgumentNullException("controlText");
			}
			return ControlSerializer.DeserializeControl(controlText, designerHost);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000B0158 File Offset: 0x000AF158
		internal static Control ParseControl(IDesignerHost designerHost, string controlText, bool applyTheme)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			if (controlText == null || controlText.Length == 0)
			{
				throw new ArgumentNullException("controlText");
			}
			return ControlSerializer.DeserializeControlInternal(controlText, designerHost, applyTheme);
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000B0188 File Offset: 0x000AF188
		public static Control ParseControl(IDesignerHost designerHost, string controlText, string directives)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			if (controlText == null || controlText.Length == 0)
			{
				throw new ArgumentNullException("controlText");
			}
			if (directives != null && directives.Length != 0)
			{
				controlText = directives + controlText;
			}
			return ControlSerializer.DeserializeControl(controlText, designerHost);
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000B01D4 File Offset: 0x000AF1D4
		public static Control[] ParseControls(IDesignerHost designerHost, string controlText)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			if (controlText == null || controlText.Length == 0)
			{
				throw new ArgumentNullException("controlText");
			}
			return ControlSerializer.DeserializeControls(controlText, designerHost);
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000B0201 File Offset: 0x000AF201
		public static ITemplate ParseTemplate(IDesignerHost designerHost, string templateText)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			return ControlSerializer.DeserializeTemplate(templateText, designerHost);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000B0218 File Offset: 0x000AF218
		public static ITemplate ParseTemplate(IDesignerHost designerHost, string templateText, string directives)
		{
			if (designerHost == null)
			{
				throw new ArgumentNullException("designerHost");
			}
			return ControlSerializer.DeserializeTemplate(templateText, designerHost);
		}
	}
}
