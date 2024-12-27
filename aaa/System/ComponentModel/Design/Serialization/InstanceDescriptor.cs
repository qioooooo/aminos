using System;
using System.Collections;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001AE RID: 430
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class InstanceDescriptor
	{
		// Token: 0x06000D2F RID: 3375 RVA: 0x0002A62B File Offset: 0x0002962B
		public InstanceDescriptor(MemberInfo member, ICollection arguments)
			: this(member, arguments, true)
		{
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x0002A638 File Offset: 0x00029638
		public InstanceDescriptor(MemberInfo member, ICollection arguments, bool isComplete)
		{
			this.member = member;
			this.isComplete = isComplete;
			if (arguments == null)
			{
				this.arguments = new object[0];
			}
			else
			{
				object[] array = new object[arguments.Count];
				arguments.CopyTo(array, 0);
				this.arguments = array;
			}
			if (member is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)member;
				if (!fieldInfo.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeStatic"));
				}
				if (this.arguments.Count != 0)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorLengthMismatch"));
				}
			}
			else if (member is ConstructorInfo)
			{
				ConstructorInfo constructorInfo = (ConstructorInfo)member;
				if (constructorInfo.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorCannotBeStatic"));
				}
				if (this.arguments.Count != constructorInfo.GetParameters().Length)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorLengthMismatch"));
				}
			}
			else if (member is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)member;
				if (!methodInfo.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeStatic"));
				}
				if (this.arguments.Count != methodInfo.GetParameters().Length)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorLengthMismatch"));
				}
			}
			else if (member is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)member;
				if (!propertyInfo.CanRead)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeReadable"));
				}
				MethodInfo getMethod = propertyInfo.GetGetMethod();
				if (getMethod != null && !getMethod.IsStatic)
				{
					throw new ArgumentException(SR.GetString("InstanceDescriptorMustBeStatic"));
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000D31 RID: 3377 RVA: 0x0002A7B6 File Offset: 0x000297B6
		public ICollection Arguments
		{
			get
			{
				return this.arguments;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0002A7BE File Offset: 0x000297BE
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000D33 RID: 3379 RVA: 0x0002A7C6 File Offset: 0x000297C6
		public MemberInfo MemberInfo
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x0002A7D0 File Offset: 0x000297D0
		public object Invoke()
		{
			object[] array = new object[this.arguments.Count];
			this.arguments.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] is InstanceDescriptor)
				{
					array[i] = ((InstanceDescriptor)array[i]).Invoke();
				}
			}
			if (this.member is ConstructorInfo)
			{
				return ((ConstructorInfo)this.member).Invoke(array);
			}
			if (this.member is MethodInfo)
			{
				return ((MethodInfo)this.member).Invoke(null, array);
			}
			if (this.member is PropertyInfo)
			{
				return ((PropertyInfo)this.member).GetValue(null, array);
			}
			if (this.member is FieldInfo)
			{
				return ((FieldInfo)this.member).GetValue(null);
			}
			return null;
		}

		// Token: 0x04000EAD RID: 3757
		private MemberInfo member;

		// Token: 0x04000EAE RID: 3758
		private ICollection arguments;

		// Token: 0x04000EAF RID: 3759
		private bool isComplete;
	}
}
