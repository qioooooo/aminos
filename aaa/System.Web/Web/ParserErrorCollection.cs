using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000071 RID: 113
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class ParserErrorCollection : CollectionBase
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x000142DA File Offset: 0x000132DA
		public ParserErrorCollection()
		{
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x000142E2 File Offset: 0x000132E2
		public ParserErrorCollection(ParserError[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170001DC RID: 476
		public ParserError this[int index]
		{
			get
			{
				return (ParserError)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00014313 File Offset: 0x00013313
		public int Add(ParserError value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00014324 File Offset: 0x00013324
		public void AddRange(ParserError[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00014358 File Offset: 0x00013358
		public void AddRange(ParserErrorCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			foreach (object obj in value)
			{
				ParserError parserError = (ParserError)obj;
				this.Add(parserError);
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000143BC File Offset: 0x000133BC
		public bool Contains(ParserError value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000143CA File Offset: 0x000133CA
		public void CopyTo(ParserError[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x000143D9 File Offset: 0x000133D9
		public int IndexOf(ParserError value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x000143E7 File Offset: 0x000133E7
		public void Insert(int index, ParserError value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x000143F6 File Offset: 0x000133F6
		public void Remove(ParserError value)
		{
			base.List.Remove(value);
		}
	}
}
