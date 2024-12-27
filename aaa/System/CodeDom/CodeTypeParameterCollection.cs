using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000082 RID: 130
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeParameterCollection : CollectionBase
	{
		// Token: 0x06000493 RID: 1171 RVA: 0x00014F69 File Offset: 0x00013F69
		public CodeTypeParameterCollection()
		{
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00014F71 File Offset: 0x00013F71
		public CodeTypeParameterCollection(CodeTypeParameterCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00014F80 File Offset: 0x00013F80
		public CodeTypeParameterCollection(CodeTypeParameter[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000E5 RID: 229
		public CodeTypeParameter this[int index]
		{
			get
			{
				return (CodeTypeParameter)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00014FB1 File Offset: 0x00013FB1
		public int Add(CodeTypeParameter value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00014FBF File Offset: 0x00013FBF
		public void Add(string value)
		{
			this.Add(new CodeTypeParameter(value));
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00014FD0 File Offset: 0x00013FD0
		public void AddRange(CodeTypeParameter[] value)
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

		// Token: 0x0600049B RID: 1179 RVA: 0x00015004 File Offset: 0x00014004
		public void AddRange(CodeTypeParameterCollection value)
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

		// Token: 0x0600049C RID: 1180 RVA: 0x00015040 File Offset: 0x00014040
		public bool Contains(CodeTypeParameter value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001504E File Offset: 0x0001404E
		public void CopyTo(CodeTypeParameter[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0001505D File Offset: 0x0001405D
		public int IndexOf(CodeTypeParameter value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0001506B File Offset: 0x0001406B
		public void Insert(int index, CodeTypeParameter value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001507A File Offset: 0x0001407A
		public void Remove(CodeTypeParameter value)
		{
			base.List.Remove(value);
		}
	}
}
