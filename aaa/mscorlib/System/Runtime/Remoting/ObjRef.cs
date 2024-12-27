using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Runtime.Remoting
{
	// Token: 0x02000721 RID: 1825
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[Serializable]
	public class ObjRef : IObjectReference, ISerializable
	{
		// Token: 0x060041C0 RID: 16832 RVA: 0x000E061E File Offset: 0x000DF61E
		internal void SetServerIdentity(GCHandle hndSrvIdentity)
		{
			this.srvIdentity = hndSrvIdentity;
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x000E0627 File Offset: 0x000DF627
		internal GCHandle GetServerIdentity()
		{
			return this.srvIdentity;
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x000E062F File Offset: 0x000DF62F
		internal void SetDomainID(int id)
		{
			this.domainID = id;
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x000E0638 File Offset: 0x000DF638
		internal int GetDomainID()
		{
			return this.domainID;
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x000E0640 File Offset: 0x000DF640
		private ObjRef(ObjRef o)
		{
			this.uri = o.uri;
			this.typeInfo = o.typeInfo;
			this.envoyInfo = o.envoyInfo;
			this.channelInfo = o.channelInfo;
			this.objrefFlags = o.objrefFlags;
			this.SetServerIdentity(o.GetServerIdentity());
			this.SetDomainID(o.GetDomainID());
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x000E06A8 File Offset: 0x000DF6A8
		public ObjRef(MarshalByRefObject o, Type requestedType)
		{
			bool flag;
			Identity identity = MarshalByRefObject.GetIdentity(o, out flag);
			this.Init(o, identity, requestedType);
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x000E06D0 File Offset: 0x000DF6D0
		protected ObjRef(SerializationInfo info, StreamingContext context)
		{
			string text = null;
			bool flag = false;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name.Equals("uri"))
				{
					this.uri = (string)enumerator.Value;
				}
				else if (enumerator.Name.Equals("typeInfo"))
				{
					this.typeInfo = (IRemotingTypeInfo)enumerator.Value;
				}
				else if (enumerator.Name.Equals("envoyInfo"))
				{
					this.envoyInfo = (IEnvoyInfo)enumerator.Value;
				}
				else if (enumerator.Name.Equals("channelInfo"))
				{
					this.channelInfo = (IChannelInfo)enumerator.Value;
				}
				else if (enumerator.Name.Equals("objrefFlags"))
				{
					object value = enumerator.Value;
					if (value.GetType() == typeof(string))
					{
						this.objrefFlags = ((IConvertible)value).ToInt32(null);
					}
					else
					{
						this.objrefFlags = (int)value;
					}
				}
				else if (enumerator.Name.Equals("fIsMarshalled"))
				{
					object value2 = enumerator.Value;
					int num;
					if (value2.GetType() == typeof(string))
					{
						num = ((IConvertible)value2).ToInt32(null);
					}
					else
					{
						num = (int)value2;
					}
					if (num == 0)
					{
						flag = true;
					}
				}
				else if (enumerator.Name.Equals("url"))
				{
					text = (string)enumerator.Value;
				}
				else if (enumerator.Name.Equals("SrvIdentity"))
				{
					this.SetServerIdentity((GCHandle)enumerator.Value);
				}
				else if (enumerator.Name.Equals("DomainId"))
				{
					this.SetDomainID((int)enumerator.Value);
				}
			}
			if (!flag)
			{
				this.objrefFlags |= 1;
			}
			else
			{
				this.objrefFlags &= -2;
			}
			if (text != null)
			{
				this.uri = text;
				this.objrefFlags |= 4;
			}
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x000E08E4 File Offset: 0x000DF8E4
		internal bool CanSmuggle()
		{
			if (base.GetType() != typeof(ObjRef) || this.IsObjRefLite())
			{
				return false;
			}
			Type type = null;
			if (this.typeInfo != null)
			{
				type = this.typeInfo.GetType();
			}
			Type type2 = null;
			if (this.channelInfo != null)
			{
				type2 = this.channelInfo.GetType();
			}
			if ((type == null || type == typeof(TypeInfo) || type == typeof(DynamicTypeInfo)) && this.envoyInfo == null && (type2 == null || type2 == typeof(ChannelInfo)))
			{
				if (this.channelInfo != null)
				{
					foreach (object obj in this.channelInfo.ChannelData)
					{
						if (!(obj is CrossAppDomainData))
						{
							return false;
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x000E09AD File Offset: 0x000DF9AD
		internal ObjRef CreateSmuggleableCopy()
		{
			return new ObjRef(this);
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x000E09B8 File Offset: 0x000DF9B8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.SetType(ObjRef.orType);
			if (!this.IsObjRefLite())
			{
				info.AddValue("uri", this.uri, typeof(string));
				info.AddValue("objrefFlags", this.objrefFlags);
				info.AddValue("typeInfo", this.typeInfo, typeof(IRemotingTypeInfo));
				info.AddValue("envoyInfo", this.envoyInfo, typeof(IEnvoyInfo));
				info.AddValue("channelInfo", this.GetChannelInfoHelper(), typeof(IChannelInfo));
				return;
			}
			info.AddValue("url", this.uri, typeof(string));
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x000E0A80 File Offset: 0x000DFA80
		private IChannelInfo GetChannelInfoHelper()
		{
			ChannelInfo channelInfo = this.channelInfo as ChannelInfo;
			if (channelInfo == null)
			{
				return this.channelInfo;
			}
			object[] channelData = channelInfo.ChannelData;
			if (channelData == null)
			{
				return channelInfo;
			}
			string[] array = (string[])CallContext.GetData("__bashChannelUrl");
			if (array == null)
			{
				return channelInfo;
			}
			string text = array[0];
			string text2 = array[1];
			ChannelInfo channelInfo2 = new ChannelInfo();
			channelInfo2.ChannelData = new object[channelData.Length];
			for (int i = 0; i < channelData.Length; i++)
			{
				channelInfo2.ChannelData[i] = channelData[i];
				ChannelDataStore channelDataStore = channelInfo2.ChannelData[i] as ChannelDataStore;
				if (channelDataStore != null)
				{
					string[] channelUris = channelDataStore.ChannelUris;
					if (channelUris != null && channelUris.Length == 1 && channelUris[0].Equals(text))
					{
						ChannelDataStore channelDataStore2 = channelDataStore.InternalShallowCopy();
						channelDataStore2.ChannelUris = new string[1];
						channelDataStore2.ChannelUris[0] = text2;
						channelInfo2.ChannelData[i] = channelDataStore2;
					}
				}
			}
			return channelInfo2;
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x060041CB RID: 16843 RVA: 0x000E0B67 File Offset: 0x000DFB67
		// (set) Token: 0x060041CC RID: 16844 RVA: 0x000E0B6F File Offset: 0x000DFB6F
		public virtual string URI
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x000E0B78 File Offset: 0x000DFB78
		// (set) Token: 0x060041CE RID: 16846 RVA: 0x000E0B80 File Offset: 0x000DFB80
		public virtual IRemotingTypeInfo TypeInfo
		{
			get
			{
				return this.typeInfo;
			}
			set
			{
				this.typeInfo = value;
			}
		}

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060041CF RID: 16847 RVA: 0x000E0B89 File Offset: 0x000DFB89
		// (set) Token: 0x060041D0 RID: 16848 RVA: 0x000E0B91 File Offset: 0x000DFB91
		public virtual IEnvoyInfo EnvoyInfo
		{
			get
			{
				return this.envoyInfo;
			}
			set
			{
				this.envoyInfo = value;
			}
		}

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x060041D1 RID: 16849 RVA: 0x000E0B9A File Offset: 0x000DFB9A
		// (set) Token: 0x060041D2 RID: 16850 RVA: 0x000E0BA2 File Offset: 0x000DFBA2
		public virtual IChannelInfo ChannelInfo
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return this.channelInfo;
			}
			set
			{
				this.channelInfo = value;
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x000E0BAB File Offset: 0x000DFBAB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public virtual object GetRealObject(StreamingContext context)
		{
			return this.GetRealObjectHelper();
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x000E0BB4 File Offset: 0x000DFBB4
		internal object GetRealObjectHelper()
		{
			if (!this.IsMarshaledObject())
			{
				return this;
			}
			if (this.IsObjRefLite())
			{
				int num = this.uri.IndexOf(RemotingConfiguration.ApplicationId);
				if (num > 0)
				{
					this.uri = this.uri.Substring(num - 1);
				}
			}
			bool flag = base.GetType() != typeof(ObjRef);
			object obj = RemotingServices.Unmarshal(this, flag);
			return this.GetCustomMarshaledCOMObject(obj);
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x000E0C24 File Offset: 0x000DFC24
		private object GetCustomMarshaledCOMObject(object ret)
		{
			DynamicTypeInfo dynamicTypeInfo = this.TypeInfo as DynamicTypeInfo;
			if (dynamicTypeInfo != null)
			{
				IntPtr intPtr = Win32Native.NULL;
				if (this.IsFromThisProcess() && !this.IsFromThisAppDomain())
				{
					try
					{
						bool flag;
						intPtr = ((__ComObject)ret).GetIUnknown(out flag);
						if (intPtr != Win32Native.NULL && !flag)
						{
							string typeName = this.TypeInfo.TypeName;
							string text = null;
							string text2 = null;
							global::System.Runtime.Remoting.TypeInfo.ParseTypeAndAssembly(typeName, out text, out text2);
							Assembly assembly = FormatterServices.LoadAssemblyFromStringNoThrow(text2);
							if (assembly == null)
							{
								throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_AssemblyNotFound"), new object[] { text2 }));
							}
							Type type = assembly.GetType(text, false, false);
							if (type != null && !type.IsVisible)
							{
								type = null;
							}
							object typedObjectForIUnknown = Marshal.GetTypedObjectForIUnknown(intPtr, type);
							if (typedObjectForIUnknown != null)
							{
								ret = typedObjectForIUnknown;
							}
						}
					}
					finally
					{
						if (intPtr != Win32Native.NULL)
						{
							Marshal.Release(intPtr);
						}
					}
				}
			}
			return ret;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x000E0D30 File Offset: 0x000DFD30
		public ObjRef()
		{
			this.objrefFlags = 0;
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x000E0D3F File Offset: 0x000DFD3F
		internal bool IsMarshaledObject()
		{
			return (this.objrefFlags & 1) == 1;
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x000E0D4C File Offset: 0x000DFD4C
		internal void SetMarshaledObject()
		{
			this.objrefFlags |= 1;
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x000E0D5C File Offset: 0x000DFD5C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal bool IsWellKnown()
		{
			return (this.objrefFlags & 2) == 2;
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x000E0D69 File Offset: 0x000DFD69
		internal void SetWellKnown()
		{
			this.objrefFlags |= 2;
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x000E0D79 File Offset: 0x000DFD79
		internal bool HasProxyAttribute()
		{
			return (this.objrefFlags & 8) == 8;
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x000E0D86 File Offset: 0x000DFD86
		internal void SetHasProxyAttribute()
		{
			this.objrefFlags |= 8;
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x000E0D96 File Offset: 0x000DFD96
		internal bool IsObjRefLite()
		{
			return (this.objrefFlags & 4) == 4;
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x000E0DA3 File Offset: 0x000DFDA3
		internal void SetObjRefLite()
		{
			this.objrefFlags |= 4;
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x000E0DB4 File Offset: 0x000DFDB4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		private CrossAppDomainData GetAppDomainChannelData()
		{
			for (int i = 0; i < this.ChannelInfo.ChannelData.Length; i++)
			{
				CrossAppDomainData crossAppDomainData = this.ChannelInfo.ChannelData[i] as CrossAppDomainData;
				if (crossAppDomainData != null)
				{
					return crossAppDomainData;
				}
			}
			return null;
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x000E0DF4 File Offset: 0x000DFDF4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool IsFromThisProcess()
		{
			if (this.IsWellKnown())
			{
				return false;
			}
			CrossAppDomainData appDomainChannelData = this.GetAppDomainChannelData();
			return appDomainChannelData != null && appDomainChannelData.IsFromThisProcess();
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x000E0E20 File Offset: 0x000DFE20
		public bool IsFromThisAppDomain()
		{
			CrossAppDomainData appDomainChannelData = this.GetAppDomainChannelData();
			return appDomainChannelData != null && appDomainChannelData.IsFromThisAppDomain();
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x000E0E40 File Offset: 0x000DFE40
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal int GetServerDomainId()
		{
			if (!this.IsFromThisProcess())
			{
				return 0;
			}
			CrossAppDomainData appDomainChannelData = this.GetAppDomainChannelData();
			return appDomainChannelData.DomainID;
		}

		// Token: 0x060041E3 RID: 16867 RVA: 0x000E0E64 File Offset: 0x000DFE64
		internal IntPtr GetServerContext(out int domainId)
		{
			IntPtr intPtr = IntPtr.Zero;
			domainId = 0;
			if (this.IsFromThisProcess())
			{
				CrossAppDomainData appDomainChannelData = this.GetAppDomainChannelData();
				domainId = appDomainChannelData.DomainID;
				if (AppDomain.IsDomainIdValid(appDomainChannelData.DomainID))
				{
					intPtr = appDomainChannelData.ContextID;
				}
			}
			return intPtr;
		}

		// Token: 0x060041E4 RID: 16868 RVA: 0x000E0EA8 File Offset: 0x000DFEA8
		internal void Init(object o, Identity idObj, Type requestedType)
		{
			this.uri = idObj.URI;
			MarshalByRefObject tporObject = idObj.TPOrObject;
			Type type;
			if (!RemotingServices.IsTransparentProxy(tporObject))
			{
				type = tporObject.GetType();
			}
			else
			{
				type = RemotingServices.GetRealProxy(tporObject).GetProxiedType();
			}
			Type type2 = ((requestedType == null) ? type : requestedType);
			if (requestedType != null && !requestedType.IsAssignableFrom(type) && !typeof(IMessageSink).IsAssignableFrom(type))
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_InvalidRequestedType"), new object[] { requestedType.ToString() }));
			}
			if (type.IsCOMObject)
			{
				DynamicTypeInfo dynamicTypeInfo = new DynamicTypeInfo(type2);
				this.TypeInfo = dynamicTypeInfo;
			}
			else
			{
				RemotingTypeCachedData reflectionCachedData = InternalRemotingServices.GetReflectionCachedData(type2);
				this.TypeInfo = reflectionCachedData.TypeInfo;
			}
			if (!idObj.IsWellKnown())
			{
				this.EnvoyInfo = global::System.Runtime.Remoting.EnvoyInfo.CreateEnvoyInfo(idObj as ServerIdentity);
				IChannelInfo channelInfo = new ChannelInfo();
				if (o is AppDomain)
				{
					object[] channelData = channelInfo.ChannelData;
					int num = channelData.Length;
					object[] array = new object[num];
					Array.Copy(channelData, array, num);
					for (int i = 0; i < num; i++)
					{
						if (!(array[i] is CrossAppDomainData))
						{
							array[i] = null;
						}
					}
					channelInfo.ChannelData = array;
				}
				this.ChannelInfo = channelInfo;
				if (type.HasProxyAttribute)
				{
					this.SetHasProxyAttribute();
				}
			}
			else
			{
				this.SetWellKnown();
			}
			if (ObjRef.ShouldUseUrlObjRef())
			{
				if (this.IsWellKnown())
				{
					this.SetObjRefLite();
					return;
				}
				string text = ChannelServices.FindFirstHttpUrlForObject(this.URI);
				if (text != null)
				{
					this.URI = text;
					this.SetObjRefLite();
				}
			}
		}

		// Token: 0x060041E5 RID: 16869 RVA: 0x000E1035 File Offset: 0x000E0035
		internal static bool ShouldUseUrlObjRef()
		{
			return RemotingConfigHandler.UrlObjRefMode;
		}

		// Token: 0x060041E6 RID: 16870 RVA: 0x000E103C File Offset: 0x000E003C
		internal static bool IsWellFormed(ObjRef objectRef)
		{
			bool flag = true;
			if (objectRef == null || objectRef.URI == null || (!objectRef.IsWellKnown() && !objectRef.IsObjRefLite() && objectRef.GetType() == ObjRef.orType && objectRef.ChannelInfo == null))
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x040020CB RID: 8395
		internal const int FLG_MARSHALED_OBJECT = 1;

		// Token: 0x040020CC RID: 8396
		internal const int FLG_WELLKNOWN_OBJREF = 2;

		// Token: 0x040020CD RID: 8397
		internal const int FLG_LITE_OBJREF = 4;

		// Token: 0x040020CE RID: 8398
		internal const int FLG_PROXY_ATTRIBUTE = 8;

		// Token: 0x040020CF RID: 8399
		internal string uri;

		// Token: 0x040020D0 RID: 8400
		internal IRemotingTypeInfo typeInfo;

		// Token: 0x040020D1 RID: 8401
		internal IEnvoyInfo envoyInfo;

		// Token: 0x040020D2 RID: 8402
		internal IChannelInfo channelInfo;

		// Token: 0x040020D3 RID: 8403
		internal int objrefFlags;

		// Token: 0x040020D4 RID: 8404
		internal GCHandle srvIdentity;

		// Token: 0x040020D5 RID: 8405
		internal int domainID;

		// Token: 0x040020D6 RID: 8406
		private static Type orType = typeof(ObjRef);
	}
}
