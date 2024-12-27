using System;
using System.Collections;

namespace System.Web.Services.Description
{
	// Token: 0x020000D3 RID: 211
	internal class MimeParameterCollection : CollectionBase
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x0001B68A File Offset: 0x0001A68A
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x0001B692 File Offset: 0x0001A692
		internal Type WriterType
		{
			get
			{
				return this.writerType;
			}
			set
			{
				this.writerType = value;
			}
		}

		// Token: 0x1700019A RID: 410
		internal MimeParameter this[int index]
		{
			get
			{
				return (MimeParameter)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B6BD File Offset: 0x0001A6BD
		internal int Add(MimeParameter parameter)
		{
			return base.List.Add(parameter);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B6CB File Offset: 0x0001A6CB
		internal void Insert(int index, MimeParameter parameter)
		{
			base.List.Insert(index, parameter);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001B6DA File Offset: 0x0001A6DA
		internal int IndexOf(MimeParameter parameter)
		{
			return base.List.IndexOf(parameter);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B6E8 File Offset: 0x0001A6E8
		internal bool Contains(MimeParameter parameter)
		{
			return base.List.Contains(parameter);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B6F6 File Offset: 0x0001A6F6
		internal void Remove(MimeParameter parameter)
		{
			base.List.Remove(parameter);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001B704 File Offset: 0x0001A704
		internal void CopyTo(MimeParameter[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x04000429 RID: 1065
		private Type writerType;
	}
}
