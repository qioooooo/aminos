using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A3 RID: 419
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class ContextStack
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0002A398 File Offset: 0x00029398
		public object Current
		{
			get
			{
				if (this.contextStack != null && this.contextStack.Count > 0)
				{
					return this.contextStack[this.contextStack.Count - 1];
				}
				return null;
			}
		}

		// Token: 0x1700028F RID: 655
		public object this[int level]
		{
			get
			{
				if (level < 0)
				{
					throw new ArgumentOutOfRangeException("level");
				}
				if (this.contextStack != null && level < this.contextStack.Count)
				{
					return this.contextStack[this.contextStack.Count - 1 - level];
				}
				return null;
			}
		}

		// Token: 0x17000290 RID: 656
		public object this[Type type]
		{
			get
			{
				if (type == null)
				{
					throw new ArgumentNullException("type");
				}
				if (this.contextStack != null)
				{
					int i = this.contextStack.Count;
					while (i > 0)
					{
						object obj = this.contextStack[--i];
						if (type.IsInstanceOfType(obj))
						{
							return obj;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0002A46E File Offset: 0x0002946E
		public void Append(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (this.contextStack == null)
			{
				this.contextStack = new ArrayList();
			}
			this.contextStack.Insert(0, context);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0002A4A0 File Offset: 0x000294A0
		public object Pop()
		{
			object obj = null;
			if (this.contextStack != null && this.contextStack.Count > 0)
			{
				int num = this.contextStack.Count - 1;
				obj = this.contextStack[num];
				this.contextStack.RemoveAt(num);
			}
			return obj;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0002A4ED File Offset: 0x000294ED
		public void Push(object context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (this.contextStack == null)
			{
				this.contextStack = new ArrayList();
			}
			this.contextStack.Add(context);
		}

		// Token: 0x04000EA8 RID: 3752
		private ArrayList contextStack;
	}
}
