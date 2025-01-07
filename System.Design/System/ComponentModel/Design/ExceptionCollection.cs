using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	[Serializable]
	public sealed class ExceptionCollection : Exception
	{
		public ExceptionCollection(ArrayList exceptions)
		{
			this.exceptions = exceptions;
		}

		private ExceptionCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.exceptions = (ArrayList)info.GetValue("exceptions", typeof(ArrayList));
		}

		public ArrayList Exceptions
		{
			get
			{
				if (this.exceptions != null)
				{
					return (ArrayList)this.exceptions.Clone();
				}
				return null;
			}
		}

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("exceptions", this.exceptions);
			base.GetObjectData(info, context);
		}

		private ArrayList exceptions;
	}
}
