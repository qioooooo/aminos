using System;
using System.Security.Permissions;
using System.Text;

namespace System.Web.Management
{
	// Token: 0x020002E6 RID: 742
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebEventFormatter
	{
		// Token: 0x06002568 RID: 9576 RVA: 0x000A0FD8 File Offset: 0x0009FFD8
		private void AddTab()
		{
			for (int i = this._level; i > 0; i--)
			{
				this._sb.Append(' ', this._tabSize);
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000A100A File Offset: 0x000A000A
		internal WebEventFormatter()
		{
			this._level = 0;
			this._sb = new StringBuilder();
			this._tabSize = 4;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000A102B File Offset: 0x000A002B
		public void AppendLine(string s)
		{
			this.AddTab();
			this._sb.Append(s);
			this._sb.Append('\n');
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000A104E File Offset: 0x000A004E
		public new string ToString()
		{
			return this._sb.ToString();
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x000A105B File Offset: 0x000A005B
		// (set) Token: 0x0600256D RID: 9581 RVA: 0x000A1063 File Offset: 0x000A0063
		public int IndentationLevel
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = Math.Max(value, 0);
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x000A1072 File Offset: 0x000A0072
		// (set) Token: 0x0600256F RID: 9583 RVA: 0x000A107A File Offset: 0x000A007A
		public int TabSize
		{
			get
			{
				return this._tabSize;
			}
			set
			{
				this._tabSize = Math.Max(value, 0);
			}
		}

		// Token: 0x04001D60 RID: 7520
		private int _level;

		// Token: 0x04001D61 RID: 7521
		private StringBuilder _sb;

		// Token: 0x04001D62 RID: 7522
		private int _tabSize;
	}
}
