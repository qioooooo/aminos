using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Web.Services.Description
{
	// Token: 0x0200012F RID: 303
	public class BasicProfileViolationCollection : CollectionBase, IEnumerable<BasicProfileViolation>, IEnumerable
	{
		// Token: 0x1700026C RID: 620
		public BasicProfileViolation this[int index]
		{
			get
			{
				return (BasicProfileViolation)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00044C94 File Offset: 0x00043C94
		internal int Add(BasicProfileViolation violation)
		{
			BasicProfileViolation basicProfileViolation = (BasicProfileViolation)this.violations[violation.NormativeStatement];
			if (basicProfileViolation == null)
			{
				this.violations[violation.NormativeStatement] = violation;
				return base.List.Add(violation);
			}
			foreach (string text in violation.Elements)
			{
				basicProfileViolation.Elements.Add(text);
			}
			return this.IndexOf(basicProfileViolation);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00044D30 File Offset: 0x00043D30
		internal int Add(string normativeStatement)
		{
			return this.Add(new BasicProfileViolation(normativeStatement));
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00044D3E File Offset: 0x00043D3E
		internal int Add(string normativeStatement, string element)
		{
			return this.Add(new BasicProfileViolation(normativeStatement, element));
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00044D4D File Offset: 0x00043D4D
		IEnumerator<BasicProfileViolation> IEnumerable<BasicProfileViolation>.GetEnumerator()
		{
			return new BasicProfileViolationEnumerator(this);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00044D55 File Offset: 0x00043D55
		public void Insert(int index, BasicProfileViolation violation)
		{
			base.List.Insert(index, violation);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00044D64 File Offset: 0x00043D64
		public int IndexOf(BasicProfileViolation violation)
		{
			return base.List.IndexOf(violation);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00044D72 File Offset: 0x00043D72
		public bool Contains(BasicProfileViolation violation)
		{
			return base.List.Contains(violation);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x00044D80 File Offset: 0x00043D80
		public void Remove(BasicProfileViolation violation)
		{
			base.List.Remove(violation);
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x00044D8E File Offset: 0x00043D8E
		public void CopyTo(BasicProfileViolation[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00044DA0 File Offset: 0x00043DA0
		public override string ToString()
		{
			if (base.List.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < base.List.Count; i++)
				{
					BasicProfileViolation basicProfileViolation = this[i];
					if (i != 0)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.Append(basicProfileViolation.NormativeStatement);
					stringBuilder.Append(": ");
					stringBuilder.Append(basicProfileViolation.Details);
					foreach (string text in basicProfileViolation.Elements)
					{
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append("  -  ");
						stringBuilder.Append(text);
					}
					if (basicProfileViolation.Recommendation != null && basicProfileViolation.Recommendation.Length > 0)
					{
						stringBuilder.Append(Environment.NewLine);
						stringBuilder.Append(basicProfileViolation.Recommendation);
					}
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}

		// Token: 0x040005FA RID: 1530
		private Hashtable violations = new Hashtable();
	}
}
