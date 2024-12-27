using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace System.Web.UI
{
	// Token: 0x020003CB RID: 971
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CssStyleCollection
	{
		// Token: 0x06002F55 RID: 12117 RVA: 0x000D2EBF File Offset: 0x000D1EBF
		internal CssStyleCollection()
			: this(null)
		{
		}

		// Token: 0x06002F56 RID: 12118 RVA: 0x000D2EC8 File Offset: 0x000D1EC8
		internal CssStyleCollection(StateBag state)
		{
			this._state = state;
		}

		// Token: 0x17000A4D RID: 2637
		public string this[string key]
		{
			get
			{
				if (this._table == null)
				{
					this.ParseString();
				}
				string text = (string)this._table[key];
				if (text == null)
				{
					HtmlTextWriterStyle styleKey = CssTextWriter.GetStyleKey(key);
					if (styleKey != (HtmlTextWriterStyle)(-1))
					{
						text = this[styleKey];
					}
				}
				return text;
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x17000A4E RID: 2638
		public string this[HtmlTextWriterStyle key]
		{
			get
			{
				if (this._intTable == null)
				{
					return null;
				}
				return (string)this._intTable[(int)key];
			}
			set
			{
				this.Add(key, value);
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000D2F54 File Offset: 0x000D1F54
		public ICollection Keys
		{
			get
			{
				if (this._table == null)
				{
					this.ParseString();
				}
				if (this._intTable != null)
				{
					string[] array = new string[this._table.Count + this._intTable.Count];
					int num = 0;
					foreach (object obj in this._table.Keys)
					{
						string text = (string)obj;
						array[num] = text;
						num++;
					}
					foreach (object obj2 in this._intTable.Keys)
					{
						HtmlTextWriterStyle htmlTextWriterStyle = (HtmlTextWriterStyle)obj2;
						array[num] = CssTextWriter.GetStyleName(htmlTextWriterStyle);
						num++;
					}
					return array;
				}
				return this._table.Keys;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000D305C File Offset: 0x000D205C
		public int Count
		{
			get
			{
				if (this._table == null)
				{
					this.ParseString();
				}
				return this._table.Count + ((this._intTable != null) ? this._intTable.Count : 0);
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002F5D RID: 12125 RVA: 0x000D308E File Offset: 0x000D208E
		// (set) Token: 0x06002F5E RID: 12126 RVA: 0x000D30C8 File Offset: 0x000D20C8
		public string Value
		{
			get
			{
				if (this._state == null)
				{
					if (this._style == null)
					{
						this._style = this.BuildString();
					}
					return this._style;
				}
				return (string)this._state["style"];
			}
			set
			{
				if (this._state == null)
				{
					this._style = value;
				}
				else
				{
					this._state["style"] = value;
				}
				this._table = null;
			}
		}

		// Token: 0x06002F5F RID: 12127 RVA: 0x000D30F4 File Offset: 0x000D20F4
		public void Add(string key, string value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException("key");
			}
			if (this._table == null)
			{
				this.ParseString();
			}
			this._table[key] = value;
			if (this._intTable != null)
			{
				HtmlTextWriterStyle styleKey = CssTextWriter.GetStyleKey(key);
				if (styleKey != (HtmlTextWriterStyle)(-1))
				{
					this._intTable.Remove(styleKey);
				}
			}
			if (this._state != null)
			{
				this._state["style"] = this.BuildString();
			}
			this._style = null;
		}

		// Token: 0x06002F60 RID: 12128 RVA: 0x000D3178 File Offset: 0x000D2178
		public void Add(HtmlTextWriterStyle key, string value)
		{
			if (this._intTable == null)
			{
				this._intTable = new HybridDictionary();
			}
			this._intTable[(int)key] = value;
			string styleName = CssTextWriter.GetStyleName(key);
			if (styleName.Length != 0)
			{
				if (this._table == null)
				{
					this.ParseString();
				}
				this._table.Remove(styleName);
			}
			if (this._state != null)
			{
				this._state["style"] = this.BuildString();
			}
			this._style = null;
		}

		// Token: 0x06002F61 RID: 12129 RVA: 0x000D31F8 File Offset: 0x000D21F8
		public void Remove(string key)
		{
			if (this._table == null)
			{
				this.ParseString();
			}
			if (this._table[key] != null)
			{
				this._table.Remove(key);
				if (this._state != null)
				{
					this._state["style"] = this.BuildString();
				}
				this._style = null;
			}
		}

		// Token: 0x06002F62 RID: 12130 RVA: 0x000D3254 File Offset: 0x000D2254
		public void Remove(HtmlTextWriterStyle key)
		{
			if (this._intTable == null)
			{
				return;
			}
			this._intTable.Remove((int)key);
			if (this._state != null)
			{
				this._state["style"] = this.BuildString();
			}
			this._style = null;
		}

		// Token: 0x06002F63 RID: 12131 RVA: 0x000D32A0 File Offset: 0x000D22A0
		public void Clear()
		{
			this._table = null;
			this._intTable = null;
			if (this._state != null)
			{
				this._state.Remove("style");
			}
			this._style = null;
		}

		// Token: 0x06002F64 RID: 12132 RVA: 0x000D32D0 File Offset: 0x000D22D0
		private string BuildString()
		{
			if ((this._table == null || this._table.Count == 0) && (this._intTable == null || this._intTable.Count == 0))
			{
				return null;
			}
			StringWriter stringWriter = new StringWriter();
			CssTextWriter cssTextWriter = new CssTextWriter(stringWriter);
			this.Render(cssTextWriter);
			return stringWriter.ToString();
		}

		// Token: 0x06002F65 RID: 12133 RVA: 0x000D3324 File Offset: 0x000D2324
		private void ParseString()
		{
			this._table = new HybridDictionary(true);
			string text = ((this._state == null) ? this._style : ((string)this._state["style"]));
			Match match;
			if (text != null && (match = CssStyleCollection._styleAttribRegex.Match(text, 0)).Success)
			{
				CaptureCollection captures = match.Groups["stylename"].Captures;
				CaptureCollection captures2 = match.Groups["styleval"].Captures;
				for (int i = 0; i < captures.Count; i++)
				{
					string text2 = captures[i].ToString();
					string text3 = captures2[i].ToString();
					this._table[text2] = text3;
				}
			}
		}

		// Token: 0x06002F66 RID: 12134 RVA: 0x000D33EC File Offset: 0x000D23EC
		internal void Render(CssTextWriter writer)
		{
			if (this._table != null && this._table.Count > 0)
			{
				foreach (object obj in this._table)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					writer.WriteAttribute((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				}
			}
			if (this._intTable != null && this._intTable.Count > 0)
			{
				foreach (object obj2 in this._intTable)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					writer.WriteAttribute((HtmlTextWriterStyle)dictionaryEntry2.Key, (string)dictionaryEntry2.Value);
				}
			}
		}

		// Token: 0x06002F67 RID: 12135 RVA: 0x000D34F0 File Offset: 0x000D24F0
		internal void Render(HtmlTextWriter writer)
		{
			if (this._table != null && this._table.Count > 0)
			{
				foreach (object obj in this._table)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					writer.AddStyleAttribute((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				}
			}
			if (this._intTable != null && this._intTable.Count > 0)
			{
				foreach (object obj2 in this._intTable)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					writer.AddStyleAttribute((HtmlTextWriterStyle)dictionaryEntry2.Key, (string)dictionaryEntry2.Value);
				}
			}
		}

		// Token: 0x040021DB RID: 8667
		private static readonly Regex _styleAttribRegex = new Regex("\\G(\\s*(;\\s*)*(?<stylename>[^:]+?)\\s*:\\s*(?<styleval>[^;]*))*\\s*(;\\s*)*$", RegexOptions.Multiline | RegexOptions.ExplicitCapture | RegexOptions.Singleline);

		// Token: 0x040021DC RID: 8668
		private StateBag _state;

		// Token: 0x040021DD RID: 8669
		private string _style;

		// Token: 0x040021DE RID: 8670
		private IDictionary _table;

		// Token: 0x040021DF RID: 8671
		private IDictionary _intTable;
	}
}
