using System;
using System.Collections.Specialized;
using System.Globalization;

namespace System.Management
{
	// Token: 0x0200003E RID: 62
	public class WqlEventQuery : EventQuery
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000BA19 File Offset: 0x0000AA19
		public WqlEventQuery()
			: this(null, TimeSpan.Zero, null, TimeSpan.Zero, null, null)
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000BA30 File Offset: 0x0000AA30
		public WqlEventQuery(string queryOrEventClassName)
		{
			this.groupByPropertyList = new StringCollection();
			if (queryOrEventClassName == null)
			{
				return;
			}
			if (queryOrEventClassName.TrimStart(new char[0]).StartsWith(WqlEventQuery.tokenSelectAll, StringComparison.OrdinalIgnoreCase))
			{
				this.QueryString = queryOrEventClassName;
				return;
			}
			ManagementPath managementPath = new ManagementPath(queryOrEventClassName);
			if (managementPath.IsClass && managementPath.NamespacePath.Length == 0)
			{
				this.EventClassName = queryOrEventClassName;
				return;
			}
			throw new ArgumentException(RC.GetString("INVALID_QUERY"), "queryOrEventClassName");
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000BAAB File Offset: 0x0000AAAB
		public WqlEventQuery(string eventClassName, string condition)
			: this(eventClassName, TimeSpan.Zero, condition, TimeSpan.Zero, null, null)
		{
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000BAC1 File Offset: 0x0000AAC1
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval)
			: this(eventClassName, withinInterval, null, TimeSpan.Zero, null, null)
		{
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000BAD3 File Offset: 0x0000AAD3
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition)
			: this(eventClassName, withinInterval, condition, TimeSpan.Zero, null, null)
		{
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000BAE5 File Offset: 0x0000AAE5
		public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval)
			: this(eventClassName, TimeSpan.Zero, condition, groupWithinInterval, null, null)
		{
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000BAF7 File Offset: 0x0000AAF7
		public WqlEventQuery(string eventClassName, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList)
			: this(eventClassName, TimeSpan.Zero, condition, groupWithinInterval, groupByPropertyList, null)
		{
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000BB0C File Offset: 0x0000AB0C
		public WqlEventQuery(string eventClassName, TimeSpan withinInterval, string condition, TimeSpan groupWithinInterval, string[] groupByPropertyList, string havingCondition)
		{
			this.eventClassName = eventClassName;
			this.withinInterval = withinInterval;
			this.condition = condition;
			this.groupWithinInterval = groupWithinInterval;
			this.groupByPropertyList = new StringCollection();
			if (groupByPropertyList != null)
			{
				this.groupByPropertyList.AddRange(groupByPropertyList);
			}
			this.havingCondition = havingCondition;
			this.BuildQuery();
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000BB66 File Offset: 0x0000AB66
		public override string QueryLanguage
		{
			get
			{
				return base.QueryLanguage;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000BB6E File Offset: 0x0000AB6E
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000BB7C File Offset: 0x0000AB7C
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

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000BB85 File Offset: 0x0000AB85
		// (set) Token: 0x0600023F RID: 575 RVA: 0x0000BB9B File Offset: 0x0000AB9B
		public string EventClassName
		{
			get
			{
				if (this.eventClassName == null)
				{
					return string.Empty;
				}
				return this.eventClassName;
			}
			set
			{
				this.eventClassName = value;
				this.BuildQuery();
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000BBAA File Offset: 0x0000ABAA
		// (set) Token: 0x06000241 RID: 577 RVA: 0x0000BBC0 File Offset: 0x0000ABC0
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
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000BBCF File Offset: 0x0000ABCF
		// (set) Token: 0x06000243 RID: 579 RVA: 0x0000BBD7 File Offset: 0x0000ABD7
		public TimeSpan WithinInterval
		{
			get
			{
				return this.withinInterval;
			}
			set
			{
				this.withinInterval = value;
				this.BuildQuery();
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000BBE6 File Offset: 0x0000ABE6
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000BBEE File Offset: 0x0000ABEE
		public TimeSpan GroupWithinInterval
		{
			get
			{
				return this.groupWithinInterval;
			}
			set
			{
				this.groupWithinInterval = value;
				this.BuildQuery();
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000BBFD File Offset: 0x0000ABFD
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000BC08 File Offset: 0x0000AC08
		public StringCollection GroupByPropertyList
		{
			get
			{
				return this.groupByPropertyList;
			}
			set
			{
				StringCollection stringCollection = new StringCollection();
				foreach (string text in value)
				{
					stringCollection.Add(text);
				}
				this.groupByPropertyList = stringCollection;
				this.BuildQuery();
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000BC70 File Offset: 0x0000AC70
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000BC86 File Offset: 0x0000AC86
		public string HavingCondition
		{
			get
			{
				if (this.havingCondition == null)
				{
					return string.Empty;
				}
				return this.havingCondition;
			}
			set
			{
				this.havingCondition = value;
				this.BuildQuery();
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000BC98 File Offset: 0x0000AC98
		protected internal void BuildQuery()
		{
			if (this.eventClassName == null || this.eventClassName.Length == 0)
			{
				base.SetQueryString(string.Empty);
				return;
			}
			string text = WqlEventQuery.tokenSelectAll;
			text = text + "from " + this.eventClassName;
			if (this.withinInterval != TimeSpan.Zero)
			{
				text = text + " within " + this.withinInterval.TotalSeconds.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(double)));
			}
			if (this.Condition.Length != 0)
			{
				text = text + " where " + this.condition;
			}
			if (this.groupWithinInterval != TimeSpan.Zero)
			{
				text = text + " group within " + this.groupWithinInterval.TotalSeconds.ToString((IFormatProvider)CultureInfo.InvariantCulture.GetFormat(typeof(double)));
				if (this.groupByPropertyList != null && 0 < this.groupByPropertyList.Count)
				{
					int count = this.groupByPropertyList.Count;
					text += " by ";
					for (int i = 0; i < count; i++)
					{
						text = text + this.groupByPropertyList[i] + ((i == count - 1) ? "" : ",");
					}
				}
				if (this.HavingCondition.Length != 0)
				{
					text = text + " having " + this.havingCondition;
				}
			}
			base.SetQueryString(text);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000BE1C File Offset: 0x0000AE1C
		protected internal override void ParseQuery(string query)
		{
			this.eventClassName = null;
			this.withinInterval = TimeSpan.Zero;
			this.condition = null;
			this.groupWithinInterval = TimeSpan.Zero;
			if (this.groupByPropertyList != null)
			{
				this.groupByPropertyList.Clear();
			}
			this.havingCondition = null;
			string text = query.Trim();
			bool flag = false;
			string text2 = ManagementQuery.tokenSelect;
			if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"));
			}
			text = text.Remove(0, text2.Length).TrimStart(null);
			if (!text.StartsWith("*", StringComparison.Ordinal))
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "*");
			}
			text = text.Remove(0, 1).TrimStart(null);
			text2 = "from ";
			if (text.Length < text2.Length || string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(RC.GetString("INVALID_QUERY"), "from");
			}
			ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref this.eventClassName);
			text2 = "within ";
			if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				string text3 = null;
				flag = false;
				ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref text3);
				this.withinInterval = TimeSpan.FromSeconds(((IConvertible)text3).ToDouble(null));
			}
			text2 = "group within ";
			int num;
			string text4;
			if (text.Length >= text2.Length && (num = text.ToLower(CultureInfo.InvariantCulture).IndexOf(text2, StringComparison.Ordinal)) != -1)
			{
				text4 = text.Substring(0, num).Trim();
				text = text.Remove(0, num);
				string text5 = null;
				flag = false;
				ManagementQuery.ParseToken(ref text, text2, null, ref flag, ref text5);
				this.groupWithinInterval = TimeSpan.FromSeconds(((IConvertible)text5).ToDouble(null));
				text2 = "by ";
				if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = text.Remove(0, text2.Length);
					if (this.groupByPropertyList != null)
					{
						this.groupByPropertyList.Clear();
					}
					else
					{
						this.groupByPropertyList = new StringCollection();
					}
					string text6;
					while ((num = text.IndexOf(',')) > 0)
					{
						text6 = text.Substring(0, num);
						text = text.Remove(0, num + 1).TrimStart(null);
						text6 = text6.Trim();
						if (text6.Length > 0)
						{
							this.groupByPropertyList.Add(text6);
						}
					}
					if ((num = text.IndexOf(' ')) <= 0)
					{
						this.groupByPropertyList.Add(text);
						return;
					}
					text6 = text.Substring(0, num);
					text = text.Remove(0, num).TrimStart(null);
					this.groupByPropertyList.Add(text6);
				}
				text2 = "having ";
				flag = false;
				if (text.Length >= text2.Length && string.Compare(text, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
				{
					text = text.Remove(0, text2.Length);
					if (text.Length == 0)
					{
						throw new ArgumentException(RC.GetString("INVALID_QUERY"), "having");
					}
					this.havingCondition = text;
				}
			}
			else
			{
				text4 = text.Trim();
			}
			text2 = "where ";
			if (text4.Length >= text2.Length && string.Compare(text4, 0, text2, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.condition = text4.Substring(text2.Length);
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C194 File Offset: 0x0000B194
		public override object Clone()
		{
			string[] array = null;
			if (this.groupByPropertyList != null)
			{
				int count = this.groupByPropertyList.Count;
				if (0 < count)
				{
					array = new string[count];
					this.groupByPropertyList.CopyTo(array, 0);
				}
			}
			return new WqlEventQuery(this.eventClassName, this.withinInterval, this.condition, this.groupWithinInterval, array, this.havingCondition);
		}

		// Token: 0x0400017A RID: 378
		private static readonly string tokenSelectAll = "select * ";

		// Token: 0x0400017B RID: 379
		private string eventClassName;

		// Token: 0x0400017C RID: 380
		private TimeSpan withinInterval;

		// Token: 0x0400017D RID: 381
		private string condition;

		// Token: 0x0400017E RID: 382
		private TimeSpan groupWithinInterval;

		// Token: 0x0400017F RID: 383
		private StringCollection groupByPropertyList;

		// Token: 0x04000180 RID: 384
		private string havingCondition;
	}
}
