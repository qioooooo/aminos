using System;
using System.Xml;

namespace System.Web.Services.Description
{
	// Token: 0x02000107 RID: 263
	public sealed class ServiceDescriptionCollection : ServiceDescriptionBaseCollection
	{
		// Token: 0x0600074C RID: 1868 RVA: 0x0002035C File Offset: 0x0001F35C
		public ServiceDescriptionCollection()
			: base(null)
		{
		}

		// Token: 0x1700021A RID: 538
		public ServiceDescription this[int index]
		{
			get
			{
				return (ServiceDescription)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x1700021B RID: 539
		public ServiceDescription this[string ns]
		{
			get
			{
				return (ServiceDescription)this.Table[ns];
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0002039A File Offset: 0x0001F39A
		public int Add(ServiceDescription serviceDescription)
		{
			return base.List.Add(serviceDescription);
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x000203A8 File Offset: 0x0001F3A8
		public void Insert(int index, ServiceDescription serviceDescription)
		{
			base.List.Insert(index, serviceDescription);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x000203B7 File Offset: 0x0001F3B7
		public int IndexOf(ServiceDescription serviceDescription)
		{
			return base.List.IndexOf(serviceDescription);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000203C5 File Offset: 0x0001F3C5
		public bool Contains(ServiceDescription serviceDescription)
		{
			return base.List.Contains(serviceDescription);
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x000203D3 File Offset: 0x0001F3D3
		public void Remove(ServiceDescription serviceDescription)
		{
			base.List.Remove(serviceDescription);
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x000203E1 File Offset: 0x0001F3E1
		public void CopyTo(ServiceDescription[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000203F0 File Offset: 0x0001F3F0
		protected override string GetKey(object value)
		{
			string targetNamespace = ((ServiceDescription)value).TargetNamespace;
			if (targetNamespace == null)
			{
				return string.Empty;
			}
			return targetNamespace;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00020414 File Offset: 0x0001F414
		private Exception ItemNotFound(XmlQualifiedName name, string type)
		{
			return new Exception(Res.GetString("WebDescriptionMissingItem", new object[] { type, name.Name, name.Namespace }));
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00020450 File Offset: 0x0001F450
		public Message GetMessage(XmlQualifiedName name)
		{
			ServiceDescription serviceDescription = this.GetServiceDescription(name);
			Message message = null;
			while (message == null && serviceDescription != null)
			{
				message = serviceDescription.Messages[name.Name];
				serviceDescription = serviceDescription.Next;
			}
			if (message == null)
			{
				throw this.ItemNotFound(name, "message");
			}
			return message;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x0002049C File Offset: 0x0001F49C
		public PortType GetPortType(XmlQualifiedName name)
		{
			ServiceDescription serviceDescription = this.GetServiceDescription(name);
			PortType portType = null;
			while (portType == null && serviceDescription != null)
			{
				portType = serviceDescription.PortTypes[name.Name];
				serviceDescription = serviceDescription.Next;
			}
			if (portType == null)
			{
				throw this.ItemNotFound(name, "message");
			}
			return portType;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x000204E8 File Offset: 0x0001F4E8
		public Service GetService(XmlQualifiedName name)
		{
			ServiceDescription serviceDescription = this.GetServiceDescription(name);
			Service service = null;
			while (service == null && serviceDescription != null)
			{
				service = serviceDescription.Services[name.Name];
				serviceDescription = serviceDescription.Next;
			}
			if (service == null)
			{
				throw this.ItemNotFound(name, "service");
			}
			return service;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00020534 File Offset: 0x0001F534
		public Binding GetBinding(XmlQualifiedName name)
		{
			ServiceDescription serviceDescription = this.GetServiceDescription(name);
			Binding binding = null;
			while (binding == null && serviceDescription != null)
			{
				binding = serviceDescription.Bindings[name.Name];
				serviceDescription = serviceDescription.Next;
			}
			if (binding == null)
			{
				throw this.ItemNotFound(name, "binding");
			}
			return binding;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00020580 File Offset: 0x0001F580
		private ServiceDescription GetServiceDescription(XmlQualifiedName name)
		{
			ServiceDescription serviceDescription = this[name.Namespace];
			if (serviceDescription == null)
			{
				throw new ArgumentException(Res.GetString("WebDescriptionMissing", new object[]
				{
					name.ToString(),
					name.Namespace
				}), "name");
			}
			return serviceDescription;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000205CD File Offset: 0x0001F5CD
		protected override void SetParent(object value, object parent)
		{
			((ServiceDescription)value).SetParent((ServiceDescriptionCollection)parent);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x000205E0 File Offset: 0x0001F5E0
		protected override void OnInsertComplete(int index, object value)
		{
			string key = this.GetKey(value);
			if (key != null)
			{
				ServiceDescription serviceDescription = (ServiceDescription)this.Table[key];
				((ServiceDescription)value).Next = (ServiceDescription)this.Table[key];
				this.Table[key] = value;
			}
			this.SetParent(value, this);
		}
	}
}
