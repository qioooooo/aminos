using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Text;

namespace System.Web.UI
{
	// Token: 0x020003D5 RID: 981
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataBoundLiteralControl : Control, ITextControl
	{
		// Token: 0x06002FCE RID: 12238 RVA: 0x000D459D File Offset: 0x000D359D
		public DataBoundLiteralControl(int staticLiteralsCount, int dataBoundLiteralCount)
		{
			this._staticLiterals = new string[staticLiteralsCount];
			this._dataBoundLiteral = new string[dataBoundLiteralCount];
			base.PreventAutoID();
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x000D45C3 File Offset: 0x000D35C3
		public void SetStaticString(int index, string s)
		{
			this._staticLiterals[index] = s;
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x000D45CE File Offset: 0x000D35CE
		public void SetDataBoundString(int index, string s)
		{
			this._dataBoundLiteral[index] = s;
			this._hasDataBoundStrings = true;
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002FD1 RID: 12241 RVA: 0x000D45E0 File Offset: 0x000D35E0
		public string Text
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = this._dataBoundLiteral.Length;
				for (int i = 0; i < this._staticLiterals.Length; i++)
				{
					if (this._staticLiterals[i] != null)
					{
						stringBuilder.Append(this._staticLiterals[i]);
					}
					if (i < num && this._dataBoundLiteral[i] != null)
					{
						stringBuilder.Append(this._dataBoundLiteral[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000D464B File Offset: 0x000D364B
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000D4653 File Offset: 0x000D3653
		protected override void LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				this._dataBoundLiteral = (string[])savedState;
				this._hasDataBoundStrings = true;
			}
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x000D466B File Offset: 0x000D366B
		protected override object SaveViewState()
		{
			if (!this._hasDataBoundStrings)
			{
				return null;
			}
			return this._dataBoundLiteral;
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x000D4680 File Offset: 0x000D3680
		protected internal override void Render(HtmlTextWriter output)
		{
			int num = this._dataBoundLiteral.Length;
			for (int i = 0; i < this._staticLiterals.Length; i++)
			{
				if (this._staticLiterals[i] != null)
				{
					output.Write(this._staticLiterals[i]);
				}
				if (i < num && this._dataBoundLiteral[i] != null)
				{
					output.Write(this._dataBoundLiteral[i]);
				}
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x000D46DD File Offset: 0x000D36DD
		// (set) Token: 0x06002FD7 RID: 12247 RVA: 0x000D46E5 File Offset: 0x000D36E5
		string ITextControl.Text
		{
			get
			{
				return this.Text;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x040021F4 RID: 8692
		private string[] _staticLiterals;

		// Token: 0x040021F5 RID: 8693
		private string[] _dataBoundLiteral;

		// Token: 0x040021F6 RID: 8694
		private bool _hasDataBoundStrings;
	}
}
