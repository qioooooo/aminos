using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data.Design
{
	// Token: 0x02000078 RID: 120
	[Serializable]
	public class TypedDataSetGeneratorException : DataException
	{
		// Token: 0x06000510 RID: 1296 RVA: 0x0000882C File Offset: 0x0000782C
		protected TypedDataSetGeneratorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			int num = (int)info.GetValue(this.KEY_ARRAYCOUNT, typeof(int));
			if (num > 0)
			{
				this.errorList = new ArrayList();
				for (int i = 0; i < num; i++)
				{
					this.errorList.Add(info.GetValue(this.KEY_ARRAYVALUES + i, typeof(string)));
				}
				return;
			}
			this.errorList = null;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000088C3 File Offset: 0x000078C3
		public TypedDataSetGeneratorException()
		{
			this.errorList = null;
			base.HResult = -2146232021;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x000088F3 File Offset: 0x000078F3
		public TypedDataSetGeneratorException(string message)
			: base(message)
		{
			base.HResult = -2146232021;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0000891D File Offset: 0x0000791D
		public TypedDataSetGeneratorException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232021;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00008948 File Offset: 0x00007948
		public TypedDataSetGeneratorException(IList list)
			: this()
		{
			this.errorList = new ArrayList(list);
			base.HResult = -2146232021;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x00008967 File Offset: 0x00007967
		public IList ErrorList
		{
			get
			{
				return this.errorList;
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00008970 File Offset: 0x00007970
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			if (this.errorList != null)
			{
				info.AddValue(this.KEY_ARRAYCOUNT, this.errorList.Count);
				for (int i = 0; i < this.errorList.Count; i++)
				{
					info.AddValue(this.KEY_ARRAYVALUES + i, this.errorList[i].ToString());
				}
				return;
			}
			info.AddValue(this.KEY_ARRAYCOUNT, 0);
		}

		// Token: 0x04000AC0 RID: 2752
		private ArrayList errorList;

		// Token: 0x04000AC1 RID: 2753
		private string KEY_ARRAYCOUNT = "KEY_ARRAYCOUNT";

		// Token: 0x04000AC2 RID: 2754
		private string KEY_ARRAYVALUES = "KEY_ARRAYVALUES";
	}
}
