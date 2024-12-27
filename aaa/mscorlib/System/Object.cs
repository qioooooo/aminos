using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;

namespace System
{
	// Token: 0x02000002 RID: 2
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[Serializable]
	public class Object
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public Object()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020D2 File Offset: 0x000010D2
		public virtual string ToString()
		{
			return this.GetType().ToString();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020DF File Offset: 0x000010DF
		public virtual bool Equals(object obj)
		{
			return object.InternalEquals(this, obj);
		}

		// Token: 0x06000004 RID: 4
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalEquals(object objA, object objB);

		// Token: 0x06000005 RID: 5 RVA: 0x000020E8 File Offset: 0x000010E8
		public static bool Equals(object objA, object objB)
		{
			return objA == objB || (objA != null && objB != null && objA.Equals(objB));
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020FF File Offset: 0x000010FF
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool ReferenceEquals(object objA, object objB)
		{
			return objA == objB;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002105 File Offset: 0x00001105
		public virtual int GetHashCode()
		{
			return object.InternalGetHashCode(this);
		}

		// Token: 0x06000008 RID: 8
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int InternalGetHashCode(object obj);

		// Token: 0x06000009 RID: 9
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Type GetType();

		// Token: 0x0600000A RID: 10 RVA: 0x0000210D File Offset: 0x0000110D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected virtual void Finalize()
		{
		}

		// Token: 0x0600000B RID: 11
		[MethodImpl(MethodImplOptions.InternalCall)]
		protected extern object MemberwiseClone();

		// Token: 0x0600000C RID: 12 RVA: 0x00002110 File Offset: 0x00001110
		private void FieldSetter(string typeName, string fieldName, object val)
		{
			FieldInfo fieldInfo = this.GetFieldInfo(typeName, fieldName);
			if (fieldInfo.IsInitOnly)
			{
				throw new FieldAccessException(Environment.GetResourceString("FieldAccess_InitOnly"));
			}
			Message.CoerceArg(val, fieldInfo.FieldType);
			fieldInfo.SetValue(this, val);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002154 File Offset: 0x00001154
		private void FieldGetter(string typeName, string fieldName, ref object val)
		{
			FieldInfo fieldInfo = this.GetFieldInfo(typeName, fieldName);
			val = fieldInfo.GetValue(this);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002174 File Offset: 0x00001174
		private FieldInfo GetFieldInfo(string typeName, string fieldName)
		{
			Type type = this.GetType();
			while (type != null && !type.FullName.Equals(typeName))
			{
				type = type.BaseType;
			}
			if (type == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { typeName }));
			}
			FieldInfo field = type.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (field == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadField"), new object[] { fieldName, typeName }));
			}
			return field;
		}
	}
}
