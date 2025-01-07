﻿using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data.Design
{
	[Serializable]
	public class TypedDataSetGeneratorException : DataException
	{
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

		public TypedDataSetGeneratorException()
		{
			this.errorList = null;
			base.HResult = -2146232021;
		}

		public TypedDataSetGeneratorException(string message)
			: base(message)
		{
			base.HResult = -2146232021;
		}

		public TypedDataSetGeneratorException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232021;
		}

		public TypedDataSetGeneratorException(IList list)
			: this()
		{
			this.errorList = new ArrayList(list);
			base.HResult = -2146232021;
		}

		public IList ErrorList
		{
			get
			{
				return this.errorList;
			}
		}

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

		private ArrayList errorList;

		private string KEY_ARRAYCOUNT = "KEY_ARRAYCOUNT";

		private string KEY_ARRAYVALUES = "KEY_ARRAYVALUES";
	}
}
