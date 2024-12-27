using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000A6 RID: 166
	[Serializable]
	internal sealed class DelegateSerializationHolder : IObjectReference, ISerializable
	{
		// Token: 0x06000A17 RID: 2583 RVA: 0x0001EE9C File Offset: 0x0001DE9C
		internal static DelegateSerializationHolder.DelegateEntry GetDelegateSerializationInfo(SerializationInfo info, Type delegateType, object target, MethodInfo method, int targetIndex)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (!method.IsPublic || (method.DeclaringType != null && !method.DeclaringType.IsVisible))
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			Type baseType = delegateType.BaseType;
			if (baseType == null || (baseType != typeof(Delegate) && baseType != typeof(MulticastDelegate)))
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeDelegate"), "type");
			}
			if (method.DeclaringType == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_GlobalMethodSerialization"));
			}
			DelegateSerializationHolder.DelegateEntry delegateEntry = new DelegateSerializationHolder.DelegateEntry(delegateType.FullName, delegateType.Module.Assembly.FullName, target, method.ReflectedType.Module.Assembly.FullName, method.ReflectedType.FullName, method.Name);
			if (info.MemberCount == 0)
			{
				info.SetType(typeof(DelegateSerializationHolder));
				info.AddValue("Delegate", delegateEntry, typeof(DelegateSerializationHolder.DelegateEntry));
			}
			if (target != null)
			{
				string text = "target" + targetIndex;
				info.AddValue(text, delegateEntry.target);
				delegateEntry.target = text;
			}
			string text2 = "method" + targetIndex;
			info.AddValue(text2, method);
			return delegateEntry;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0001EFE8 File Offset: 0x0001DFE8
		private DelegateSerializationHolder(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			bool flag = true;
			try
			{
				this.m_delegateEntry = (DelegateSerializationHolder.DelegateEntry)info.GetValue("Delegate", typeof(DelegateSerializationHolder.DelegateEntry));
			}
			catch
			{
				this.m_delegateEntry = this.OldDelegateWireFormat(info, context);
				flag = false;
			}
			if (flag)
			{
				DelegateSerializationHolder.DelegateEntry delegateEntry = this.m_delegateEntry;
				int num = 0;
				while (delegateEntry != null)
				{
					if (delegateEntry.target != null)
					{
						string text = delegateEntry.target as string;
						if (text != null)
						{
							delegateEntry.target = info.GetValue(text, typeof(object));
						}
					}
					num++;
					delegateEntry = delegateEntry.delegateEntry;
				}
				MethodInfo[] array = new MethodInfo[num];
				int i;
				for (i = 0; i < num; i++)
				{
					string text2 = "method" + i;
					array[i] = (MethodInfo)info.GetValueNoThrow(text2, typeof(MethodInfo));
					if (array[i] == null)
					{
						break;
					}
				}
				if (i == num)
				{
					this.m_methods = array;
				}
			}
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0001F0F8 File Offset: 0x0001E0F8
		private void ThrowInsufficientState(string field)
		{
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InsufficientDeserializationState"), new object[] { field }));
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001F12C File Offset: 0x0001E12C
		private DelegateSerializationHolder.DelegateEntry OldDelegateWireFormat(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			string @string = info.GetString("DelegateType");
			string string2 = info.GetString("DelegateAssembly");
			object value = info.GetValue("Target", typeof(object));
			string string3 = info.GetString("TargetTypeAssembly");
			string string4 = info.GetString("TargetTypeName");
			string string5 = info.GetString("MethodName");
			return new DelegateSerializationHolder.DelegateEntry(@string, string2, value, string3, string4, string5);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001F1A8 File Offset: 0x0001E1A8
		private Delegate GetDelegate(DelegateSerializationHolder.DelegateEntry de, int index)
		{
			Delegate @delegate;
			try
			{
				if (de.methodName == null || de.methodName.Length == 0)
				{
					this.ThrowInsufficientState("MethodName");
				}
				if (de.assembly == null || de.assembly.Length == 0)
				{
					this.ThrowInsufficientState("DelegateAssembly");
				}
				if (de.targetTypeName == null || de.targetTypeName.Length == 0)
				{
					this.ThrowInsufficientState("TargetTypeName");
				}
				Type type = Assembly.Load(de.assembly).GetType(de.type, true, false);
				Type type2 = Assembly.Load(de.targetTypeAssembly).GetType(de.targetTypeName, true, false);
				if (this.m_methods != null)
				{
					object obj = ((de.target != null) ? RemotingServices.CheckCast(de.target, type2) : null);
					@delegate = Delegate.InternalCreateDelegate(type, obj, this.m_methods[index]);
				}
				else if (de.target != null)
				{
					@delegate = Delegate.CreateDelegate(type, RemotingServices.CheckCast(de.target, type2), de.methodName);
				}
				else
				{
					@delegate = Delegate.CreateDelegate(type, type2, de.methodName);
				}
				if ((@delegate.Method != null && !@delegate.Method.IsPublic) || (@delegate.Method.DeclaringType != null && !@delegate.Method.DeclaringType.IsVisible))
				{
					new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
				}
			}
			catch (Exception ex)
			{
				if (ex is SerializationException)
				{
					throw ex;
				}
				throw new SerializationException(ex.Message, ex);
			}
			catch
			{
				throw new SerializationException();
			}
			return @delegate;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001F344 File Offset: 0x0001E344
		public object GetRealObject(StreamingContext context)
		{
			int num = 0;
			for (DelegateSerializationHolder.DelegateEntry delegateEntry = this.m_delegateEntry; delegateEntry != null; delegateEntry = delegateEntry.Entry)
			{
				num++;
			}
			int num2 = num - 1;
			if (num == 1)
			{
				return this.GetDelegate(this.m_delegateEntry, 0);
			}
			object[] array = new object[num];
			for (DelegateSerializationHolder.DelegateEntry delegateEntry2 = this.m_delegateEntry; delegateEntry2 != null; delegateEntry2 = delegateEntry2.Entry)
			{
				num--;
				array[num] = this.GetDelegate(delegateEntry2, num2 - num);
			}
			return ((MulticastDelegate)array[0]).NewMulticastDelegate(array, array.Length);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0001F3C1 File Offset: 0x0001E3C1
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_DelegateSerHolderSerial"));
		}

		// Token: 0x0400038F RID: 911
		private DelegateSerializationHolder.DelegateEntry m_delegateEntry;

		// Token: 0x04000390 RID: 912
		private MethodInfo[] m_methods;

		// Token: 0x020000A7 RID: 167
		[Serializable]
		internal class DelegateEntry
		{
			// Token: 0x06000A1E RID: 2590 RVA: 0x0001F3D2 File Offset: 0x0001E3D2
			internal DelegateEntry(string type, string assembly, object target, string targetTypeAssembly, string targetTypeName, string methodName)
			{
				this.type = type;
				this.assembly = assembly;
				this.target = target;
				this.targetTypeAssembly = targetTypeAssembly;
				this.targetTypeName = targetTypeName;
				this.methodName = methodName;
			}

			// Token: 0x1700010F RID: 271
			// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0001F407 File Offset: 0x0001E407
			// (set) Token: 0x06000A20 RID: 2592 RVA: 0x0001F40F File Offset: 0x0001E40F
			internal DelegateSerializationHolder.DelegateEntry Entry
			{
				get
				{
					return this.delegateEntry;
				}
				set
				{
					this.delegateEntry = value;
				}
			}

			// Token: 0x04000391 RID: 913
			internal string type;

			// Token: 0x04000392 RID: 914
			internal string assembly;

			// Token: 0x04000393 RID: 915
			internal object target;

			// Token: 0x04000394 RID: 916
			internal string targetTypeAssembly;

			// Token: 0x04000395 RID: 917
			internal string targetTypeName;

			// Token: 0x04000396 RID: 918
			internal string methodName;

			// Token: 0x04000397 RID: 919
			internal DelegateSerializationHolder.DelegateEntry delegateEntry;
		}
	}
}
