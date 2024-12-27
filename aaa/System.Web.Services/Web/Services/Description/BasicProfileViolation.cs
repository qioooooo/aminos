using System;
using System.Collections.Specialized;
using System.Text;

namespace System.Web.Services.Description
{
	// Token: 0x0200012E RID: 302
	public class BasicProfileViolation
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x00044AE0 File Offset: 0x00043AE0
		internal BasicProfileViolation(string normativeStatement)
			: this(normativeStatement, null)
		{
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00044AEC File Offset: 0x00043AEC
		internal BasicProfileViolation(string normativeStatement, string element)
		{
			this.normativeStatement = normativeStatement;
			int num = normativeStatement.IndexOf(',');
			if (num >= 0)
			{
				normativeStatement = normativeStatement.Substring(0, num);
			}
			this.details = Res.GetString("HelpGeneratorServiceConformance" + normativeStatement);
			this.recommendation = Res.GetString("HelpGeneratorServiceConformance" + normativeStatement + "_r");
			if (element != null)
			{
				this.Elements.Add(element);
			}
			if (this.normativeStatement == "Rxxxx")
			{
				this.normativeStatement = Res.GetString("Rxxxx");
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x00044B87 File Offset: 0x00043B87
		public WsiProfiles Claims
		{
			get
			{
				return this.claims;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00044B8F File Offset: 0x00043B8F
		public string Details
		{
			get
			{
				if (this.details == null)
				{
					return string.Empty;
				}
				return this.details;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00044BA5 File Offset: 0x00043BA5
		public StringCollection Elements
		{
			get
			{
				if (this.elements == null)
				{
					this.elements = new StringCollection();
				}
				return this.elements;
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x00044BC0 File Offset: 0x00043BC0
		public string NormativeStatement
		{
			get
			{
				return this.normativeStatement;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x00044BC8 File Offset: 0x00043BC8
		public string Recommendation
		{
			get
			{
				return this.recommendation;
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00044BD0 File Offset: 0x00043BD0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.normativeStatement);
			stringBuilder.Append(": ");
			stringBuilder.Append(this.Details);
			foreach (string text in this.Elements)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append("  -  ");
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040005F5 RID: 1525
		private WsiProfiles claims = WsiProfiles.BasicProfile1_1;

		// Token: 0x040005F6 RID: 1526
		private string normativeStatement;

		// Token: 0x040005F7 RID: 1527
		private string details;

		// Token: 0x040005F8 RID: 1528
		private string recommendation;

		// Token: 0x040005F9 RID: 1529
		private StringCollection elements;
	}
}
