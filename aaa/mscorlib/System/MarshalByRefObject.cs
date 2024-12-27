using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x02000053 RID: 83
	[ComVisible(true)]
	[Serializable]
	public abstract class MarshalByRefObject
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00010C38 File Offset: 0x0000FC38
		// (set) Token: 0x06000437 RID: 1079 RVA: 0x00010C40 File Offset: 0x0000FC40
		private object Identity
		{
			get
			{
				return this.__identity;
			}
			set
			{
				this.__identity = value;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00010C4C File Offset: 0x0000FC4C
		internal IntPtr GetComIUnknown(bool fIsBeingMarshalled)
		{
			IntPtr intPtr;
			if (RemotingServices.IsTransparentProxy(this))
			{
				intPtr = RemotingServices.GetRealProxy(this).GetCOMIUnknown(fIsBeingMarshalled);
			}
			else
			{
				intPtr = Marshal.GetIUnknownForObject(this);
			}
			return intPtr;
		}

		// Token: 0x06000439 RID: 1081
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr GetComIUnknown(MarshalByRefObject o);

		// Token: 0x0600043A RID: 1082 RVA: 0x00010C78 File Offset: 0x0000FC78
		internal bool IsInstanceOfType(Type T)
		{
			return T.IsInstanceOfType(this);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00010C84 File Offset: 0x0000FC84
		internal object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			Type type = base.GetType();
			if (!type.IsCOMObject)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_InvokeMember"));
			}
			return type.InvokeMember(name, invokeAttr, binder, this, args, modifiers, culture, namedParameters);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00010CC4 File Offset: 0x0000FCC4
		protected MarshalByRefObject MemberwiseClone(bool cloneIdentity)
		{
			MarshalByRefObject marshalByRefObject = (MarshalByRefObject)base.MemberwiseClone();
			if (!cloneIdentity)
			{
				marshalByRefObject.Identity = null;
			}
			return marshalByRefObject;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00010CE8 File Offset: 0x0000FCE8
		internal static Identity GetIdentity(MarshalByRefObject obj, out bool fServer)
		{
			fServer = true;
			Identity identity = null;
			if (obj != null)
			{
				if (!RemotingServices.IsTransparentProxy(obj))
				{
					identity = (Identity)obj.Identity;
				}
				else
				{
					fServer = false;
					identity = RemotingServices.GetRealProxy(obj).IdentityObject;
				}
			}
			return identity;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00010D24 File Offset: 0x0000FD24
		internal static Identity GetIdentity(MarshalByRefObject obj)
		{
			bool flag;
			return MarshalByRefObject.GetIdentity(obj, out flag);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00010D39 File Offset: 0x0000FD39
		internal ServerIdentity __RaceSetServerIdentity(ServerIdentity id)
		{
			if (this.__identity == null)
			{
				if (!id.IsContextBound)
				{
					id.RaceSetTransparentProxy(this);
				}
				Interlocked.CompareExchange(ref this.__identity, id, null);
			}
			return (ServerIdentity)this.__identity;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00010D6C File Offset: 0x0000FD6C
		internal void __ResetServerIdentity()
		{
			this.__identity = null;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00010D75 File Offset: 0x0000FD75
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public object GetLifetimeService()
		{
			return LifetimeServices.GetLease(this);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00010D7D File Offset: 0x0000FD7D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual object InitializeLifetimeService()
		{
			return LifetimeServices.GetLeaseInitial(this);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00010D85 File Offset: 0x0000FD85
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public virtual ObjRef CreateObjRef(Type requestedType)
		{
			if (this.__identity == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_NoIdentityEntry"));
			}
			return new ObjRef(this, requestedType);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00010DA8 File Offset: 0x0000FDA8
		internal bool CanCastToXmlType(string xmlTypeName, string xmlTypeNamespace)
		{
			Type type = SoapServices.GetInteropTypeFromXmlType(xmlTypeName, xmlTypeNamespace);
			if (type == null)
			{
				string text;
				string text2;
				if (!SoapServices.DecodeXmlNamespaceForClrTypeNamespace(xmlTypeNamespace, out text, out text2))
				{
					return false;
				}
				string text3;
				if (text != null && text.Length > 0)
				{
					text3 = text + "." + xmlTypeName;
				}
				else
				{
					text3 = xmlTypeName;
				}
				try
				{
					Assembly assembly = Assembly.Load(text2);
					type = assembly.GetType(text3, false, false);
				}
				catch
				{
					return false;
				}
			}
			return type != null && type.IsAssignableFrom(base.GetType());
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00010E2C File Offset: 0x0000FE2C
		internal static bool CanCastToXmlTypeHelper(Type castType, MarshalByRefObject o)
		{
			if (castType == null)
			{
				throw new ArgumentNullException("castType");
			}
			if (!castType.IsInterface && !castType.IsMarshalByRef)
			{
				return false;
			}
			string text = null;
			string text2 = null;
			if (!SoapServices.GetXmlTypeForInteropType(castType, out text, out text2))
			{
				text = castType.Name;
				text2 = SoapServices.CodeXmlNamespaceForClrTypeNamespace(castType.Namespace, castType.Module.Assembly.nGetSimpleName());
			}
			return o.CanCastToXmlType(text, text2);
		}

		// Token: 0x04000194 RID: 404
		private object __identity;
	}
}
