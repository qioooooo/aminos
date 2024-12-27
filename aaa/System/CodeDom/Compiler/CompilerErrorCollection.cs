using System;
using System.Collections;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F0 RID: 496
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerErrorCollection : CollectionBase
	{
		// Token: 0x060010B5 RID: 4277 RVA: 0x000370DD File Offset: 0x000360DD
		public CompilerErrorCollection()
		{
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x000370E5 File Offset: 0x000360E5
		public CompilerErrorCollection(CompilerErrorCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x000370F4 File Offset: 0x000360F4
		public CompilerErrorCollection(CompilerError[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000349 RID: 841
		public CompilerError this[int index]
		{
			get
			{
				return (CompilerError)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00037125 File Offset: 0x00036125
		public int Add(CompilerError value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00037134 File Offset: 0x00036134
		public void AddRange(CompilerError[] value)
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

		// Token: 0x060010BC RID: 4284 RVA: 0x00037168 File Offset: 0x00036168
		public void AddRange(CompilerErrorCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x000371A4 File Offset: 0x000361A4
		public bool Contains(CompilerError value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x000371B2 File Offset: 0x000361B2
		public void CopyTo(CompilerError[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x000371C4 File Offset: 0x000361C4
		public bool HasErrors
		{
			get
			{
				if (base.Count > 0)
				{
					foreach (object obj in this)
					{
						CompilerError compilerError = (CompilerError)obj;
						if (!compilerError.IsWarning)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0003722C File Offset: 0x0003622C
		public bool HasWarnings
		{
			get
			{
				if (base.Count > 0)
				{
					foreach (object obj in this)
					{
						CompilerError compilerError = (CompilerError)obj;
						if (compilerError.IsWarning)
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00037294 File Offset: 0x00036294
		public int IndexOf(CompilerError value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x000372A2 File Offset: 0x000362A2
		public void Insert(int index, CompilerError value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x000372B1 File Offset: 0x000362B1
		public void Remove(CompilerError value)
		{
			base.List.Remove(value);
		}
	}
}
