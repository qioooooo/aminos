using System;

namespace System.Web.UI
{
	// Token: 0x02000421 RID: 1057
	internal sealed class ResourceBasedLiteralControl : LiteralControl
	{
		// Token: 0x060032EC RID: 13036 RVA: 0x000DD96C File Offset: 0x000DC96C
		internal ResourceBasedLiteralControl(TemplateControl tplControl, int offset, int size, bool fAsciiOnly)
		{
			if (offset < 0 || offset + size > tplControl.MaxResourceOffset)
			{
				throw new ArgumentException();
			}
			this._tplControl = tplControl;
			this._offset = offset;
			this._size = size;
			this._fAsciiOnly = fAsciiOnly;
			base.PreventAutoID();
			this.EnableViewState = false;
		}

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x000DD9BE File Offset: 0x000DC9BE
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x000DD9EB File Offset: 0x000DC9EB
		public override string Text
		{
			get
			{
				if (this._size == 0)
				{
					return base.Text;
				}
				return StringResourceManager.ResourceToString(this._tplControl.StringResourcePointer, this._offset, this._size);
			}
			set
			{
				this._size = 0;
				base.Text = value;
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x000DD9FB File Offset: 0x000DC9FB
		protected internal override void Render(HtmlTextWriter output)
		{
			if (this._size == 0)
			{
				base.Render(output);
				return;
			}
			output.WriteUTF8ResourceString(this._tplControl.StringResourcePointer, this._offset, this._size, this._fAsciiOnly);
		}

		// Token: 0x040023D6 RID: 9174
		private TemplateControl _tplControl;

		// Token: 0x040023D7 RID: 9175
		private int _offset;

		// Token: 0x040023D8 RID: 9176
		private int _size;

		// Token: 0x040023D9 RID: 9177
		private bool _fAsciiOnly;
	}
}
