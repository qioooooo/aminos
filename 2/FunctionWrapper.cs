using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x02000087 RID: 135
	public sealed class FunctionWrapper : ScriptFunction
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x0002CAE0 File Offset: 0x0002BAE0
		internal FunctionWrapper(string name, object obj, MemberInfo[] members)
			: base(FunctionPrototype.ob, name, 0)
		{
			this.obj = obj;
			this.members = members;
			foreach (MemberInfo memberInfo in members)
			{
				if (memberInfo is MethodInfo)
				{
					this.ilength = ((MethodInfo)memberInfo).GetParameters().Length;
					return;
				}
			}
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0002CB38 File Offset: 0x0002BB38
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object Call(object[] args, object thisob)
		{
			return this.Call(args, thisob, null, null);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0002CB44 File Offset: 0x0002BB44
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
		{
			MethodInfo methodInfo = this.members[0] as MethodInfo;
			if (thisob is GlobalScope || thisob == null || (methodInfo != null && (methodInfo.Attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope))
			{
				thisob = this.obj;
			}
			else if (!this.obj.GetType().IsInstanceOfType(thisob) && !(this.obj is ClassScope))
			{
				if (this.members.Length == 1)
				{
					JSWrappedMethod jswrappedMethod = this.members[0] as JSWrappedMethod;
					if (jswrappedMethod != null && jswrappedMethod.DeclaringType == Typeob.Object)
					{
						return LateBinding.CallOneOfTheMembers(new MemberInfo[] { jswrappedMethod.method }, args, false, thisob, binder, culture, null, this.engine);
					}
				}
				throw new JScriptException(JSError.TypeMismatch);
			}
			return LateBinding.CallOneOfTheMembers(this.members, args, false, thisob, binder, culture, null, this.engine);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002CC0F File Offset: 0x0002BC0F
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal Delegate ConvertToDelegate(Type delegateType)
		{
			return Delegate.CreateDelegate(delegateType, this.obj, this.name);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0002CC24 File Offset: 0x0002BC24
		public override string ToString()
		{
			Type declaringType = this.members[0].DeclaringType;
			MethodInfo methodInfo = ((declaringType == null) ? null : declaringType.GetMethod(this.name + " source"));
			if (methodInfo != null)
			{
				return (string)methodInfo.Invoke(null, null);
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (MemberInfo memberInfo in this.members)
			{
				if (memberInfo is MethodInfo || (memberInfo is PropertyInfo && JSProperty.GetGetMethod((PropertyInfo)memberInfo, false) != null))
				{
					if (!flag)
					{
						stringBuilder.Append("\n");
					}
					else
					{
						flag = false;
					}
					stringBuilder.Append(memberInfo.ToString());
				}
			}
			if (stringBuilder.Length > 0)
			{
				return stringBuilder.ToString();
			}
			return "function " + this.name + "() {\n    [native code]\n}";
		}

		// Token: 0x040002BE RID: 702
		private object obj;

		// Token: 0x040002BF RID: 703
		private MemberInfo[] members;
	}
}
