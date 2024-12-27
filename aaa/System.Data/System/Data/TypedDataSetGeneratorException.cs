using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data
{
	// Token: 0x02000105 RID: 261
	[Serializable]
	public class TypedDataSetGeneratorException : DataException
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x00215B74 File Offset: 0x00214F74
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

		// Token: 0x06000F55 RID: 3925 RVA: 0x00215C0C File Offset: 0x0021500C
		public TypedDataSetGeneratorException()
		{
			this.errorList = null;
			base.HResult = -2146232021;
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00215C48 File Offset: 0x00215048
		public TypedDataSetGeneratorException(string message)
			: base(message)
		{
			base.HResult = -2146232021;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00215C80 File Offset: 0x00215080
		public TypedDataSetGeneratorException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232021;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00215CB8 File Offset: 0x002150B8
		public TypedDataSetGeneratorException(ArrayList list)
			: this()
		{
			this.errorList = list;
			base.HResult = -2146232021;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00215CE0 File Offset: 0x002150E0
		public ArrayList ErrorList
		{
			get
			{
				return this.errorList;
			}
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00215CF4 File Offset: 0x002150F4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
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

		// Token: 0x04000AAA RID: 2730
		private ArrayList errorList;

		// Token: 0x04000AAB RID: 2731
		private string KEY_ARRAYCOUNT = "KEY_ARRAYCOUNT";

		// Token: 0x04000AAC RID: 2732
		private string KEY_ARRAYVALUES = "KEY_ARRAYVALUES";
	}
}
