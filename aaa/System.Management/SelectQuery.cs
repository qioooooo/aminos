using System;
using System.Collections.Specialized;

namespace System.Management
{
	// Token: 0x0200003B RID: 59
	public class SelectQuery : WqlObjectQuery
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x00009F6B File Offset: 0x00008F6B
		public SelectQuery()
			: this(null)
		{
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00009F74 File Offset: 0x00008F74
		public SelectQuery(string queryOrClassName)
		{
			this.selectedProperties = new StringCollection();
			if (queryOrClassName == null)
			{
				return;
			}
			if (queryOrClassName.TrimStart(new char[0]).StartsWith(ManagementQuery.tokenSelect, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrClassName;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrClassName);
			if (managementPath.IsClass && managementPath.NamespacePath.Length == 0)
			{
				this.ClassName = queryOrClassName;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrClassName");
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00009FEF File Offset: 0x00008FEF
		public SelectQuery(string className, string condition)
			: this(className, condition, null)
		{
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00009FFA File Offset: 0x00008FFA
		public SelectQuery(string className, string condition, string[] selectedProperties)
		{
			this.isSchemaQuery = false;
			this.className = className;
			this.condition = condition;
			this.selectedProperties = new StringCollection();
			if (selectedProperties != null)
			{
				this.selectedProperties.AddRange(selectedProperties);
			}
			this.BuildQuery();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A038 File Offset: 0x00009038
		public SelectQuery(bool isSchemaQuery, string condition)
		{
			if (!isSchemaQuery)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "isSchemaQuery");
			}
			this.isSchemaQuery = true;
			this.className = null;
			this.condition = condition;
			this.selectedProperties = null;
			this.BuildQuery();
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A085 File Offset: 0x00009085
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A093 File Offset: 0x00009093
		public override string QueryString
		{
			get
			{
				this.BuildQuery();
				return base.QueryString;
			}
			set
			{
				base.QueryString = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A09C File Offset: 0x0000909C
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000A0A4 File Offset: 0x000090A4
		public bool IsSchemaQuery
		{
			get
			{
				return this.isSchemaQuery;
			}
			set
			{
				this.isSchemaQuery = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000A0B9 File Offset: 0x000090B9
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0000A0CF File Offset: 0x000090CF
		public string ClassName
		{
			get
			{
				if (this.className == null)
				{
					return string.Empty;
				}
				return this.className;
			}
			set
			{
				this.className = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000A0E4 File Offset: 0x000090E4
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0000A0FA File Offset: 0x000090FA
		public string Condition
		{
			get
			{
				if (this.condition == null)
				{
					return string.Empty;
				}
				return this.condition;
			}
			set
			{
				this.condition = value;
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000A10F File Offset: 0x0000910F
		// (set) Token: 0x060001FF RID: 511 RVA: 0x0000A118 File Offset: 0x00009118
		public StringCollection SelectedProperties
		{
			get
			{
				return this.selectedProperties;
			}
			set
			{
				if (value != null)
				{
					StringCollection stringCollection = new StringCollection();
					foreach (string text in value)
					{
						stringCollection.Add(text);
					}
					this.selectedProperties = stringCollection;
				}
				else
				{
					this.selectedProperties = new StringCollection();
				}
				this.BuildQuery();
				base.FireIdentifierChanged();
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A198 File Offset: 0x00009198
		protected internal void BuildQuery()
		{
			string text;
			if (!this.isSchemaQuery)
			{
				if (this.className == null)
				{
					base.SetQueryString(string.Empty);
				}
				if (this.className == null || this.className.Length == 0)
				{
					return;
				}
				text = ManagementQuery.tokenSelect;
				if (this.selectedProperties != null && 0 < this.selectedProperties.Count)
				{
					int count = this.selectedProperties.Count;
					for (int i = 0; i < count; i++)
					{
						text = text + this.selectedProperties[i] + ((i == count - 1) ? " " : ",");
					}
				}
				else
				{
					text += "* ";
				}
				text = text + "from " + this.className;
			}
			else
			{
				text = "select * from meta_class";
			}
			if (this.Condition != null && this.Condition.Length != 0)
			{
				text = text + " where " + this.condition;
			}
			base.SetQueryString(text);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A288 File Offset: 0x00009288
		protected internal override void ParseQuery(string query)
		{
			this.className = null;
			this.condition = null;
			if (this.selectedProperties != null)
			{
				this.selectedProperties.Clear();
			}
			string text = query.Trim();
			bool flag = false;
			if (!this.isSchemaQuery)
			{
				string text2 = ManagementQuery.tokenSelect;
				if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				ManagementQuery.ParseToken(ref text, text2, ref flag);
				if (text[0] != '*')
				{
					if (this.selectedProperties != null)
					{
						this.selectedProperties.Clear();
					}
					else
					{
						this.selectedProperties = new StringCollection();
					}
					int num;
					string text3;
					while ((num = text.IndexOf(',')) > 0)
					{
						text3 = text.Substring(0, num);
						text = text.Remove(0, num + 1).TrimStart(null);
						text3 = text3.Trim();
						if (text3.Length > 0)
						{
							this.selectedProperties.Add(text3);
						}
					}
					if ((num = text.IndexOf(' ')) <= 0)
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"));
					}
					text3 = text.Substring(0, num);
					text = text.Remove(0, num).TrimStart(null);
					this.selectedProperties.Add(text3);
				}
				else
				{
					text = text.Remove(0, 1).TrimStart(null);
				}
				text2 = "from ";
				flag = false;
				if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"));
				}
				ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref this.className);
				text2 = "where ";
				if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.condition = text.Substring(text2.Length).Trim();
					return;
				}
			}
			else
			{
				string text4 = "select";
				if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "select");
				}
				text = text.Remove(0, text4.Length).TrimStart(null);
				if (text.IndexOf('*', 0) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "*");
				}
				text = text.Remove(0, 1).TrimStart(null);
				text4 = "from";
				if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "from");
				}
				text = text.Remove(0, text4.Length).TrimStart(null);
				text4 = "meta_class";
				if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new ArgumentException(RC.GetString("INVALID_QUERY"), "meta_class");
				}
				text = text.Remove(0, text4.Length).TrimStart(null);
				if (0 < text.Length)
				{
					text4 = "where";
					if (text.Length < text4.Length || string.Compare(text, 0, text4, 0, text4.Length, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"), "where");
					}
					text = text.Remove(0, text4.Length);
					if (text.Length == 0 || !char.IsWhiteSpace(text[0]))
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"));
					}
					text = text.TrimStart(null);
					this.condition = text;
				}
				else
				{
					this.condition = string.Empty;
				}
				this.className = null;
				this.selectedProperties = null;
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A64C File Offset: 0x0000964C
		public override object Clone()
		{
			string[] array = null;
			if (this.selectedProperties != null)
			{
				int count = this.selectedProperties.Count;
				if (0 < count)
				{
					array = new string[count];
					this.selectedProperties.CopyTo(array, 0);
				}
			}
			if (!this.isSchemaQuery)
			{
				return new SelectQuery(this.className, this.condition, array);
			}
			return new SelectQuery(true, this.condition);
		}

		// Token: 0x04000154 RID: 340
		private bool isSchemaQuery;

		// Token: 0x04000155 RID: 341
		private string className;

		// Token: 0x04000156 RID: 342
		private string condition;

		// Token: 0x04000157 RID: 343
		private StringCollection selectedProperties;
	}
}
