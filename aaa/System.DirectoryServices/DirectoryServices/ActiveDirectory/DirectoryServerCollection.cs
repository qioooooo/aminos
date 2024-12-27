using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000093 RID: 147
	public class DirectoryServerCollection : CollectionBase
	{
		// Token: 0x060004A7 RID: 1191 RVA: 0x0001A3F4 File Offset: 0x000193F4
		internal DirectoryServerCollection(DirectoryContext context, string siteDN, string transportName)
		{
			Hashtable hashtable = new Hashtable();
			this.changeList = Hashtable.Synchronized(hashtable);
			this.context = context;
			this.siteDN = siteDN;
			this.transportDN = transportName;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0001A43C File Offset: 0x0001943C
		internal DirectoryServerCollection(DirectoryContext context, DirectoryEntry crossRefEntry, bool isADAM, ReadOnlyDirectoryServerCollection servers)
		{
			this.context = context;
			this.crossRefEntry = crossRefEntry;
			this.isADAM = isADAM;
			this.isForNC = true;
			foreach (object obj in servers)
			{
				DirectoryServer directoryServer = (DirectoryServer)obj;
				base.InnerList.Add(directoryServer);
			}
		}

		// Token: 0x17000120 RID: 288
		public DirectoryServer this[int index]
		{
			get
			{
				return (DirectoryServer)base.InnerList[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!this.Contains(value))
				{
					base.List[index] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { value }), "value");
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001A52C File Offset: 0x0001952C
		public int Add(DirectoryServer server)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			if (this.isForNC)
			{
				if (!this.isADAM)
				{
					if (!(server is DomainController))
					{
						throw new ArgumentException(Res.GetString("ServerShouldBeDC"), "server");
					}
					if (((DomainController)server).NumericOSVersion < 5.2)
					{
						throw new ArgumentException(Res.GetString("ServerShouldBeW2K3"), "server");
					}
				}
				if (!this.Contains(server))
				{
					return base.List.Add(server);
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { server }), "server");
			}
			else
			{
				string text = ((server is DomainController) ? ((DomainController)server).SiteObjectName : ((AdamInstance)server).SiteObjectName);
				if (Utils.Compare(this.siteDN, text) != 0)
				{
					throw new ArgumentException(Res.GetString("NotWithinSite"));
				}
				if (!this.Contains(server))
				{
					return base.List.Add(server);
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { server }), "server");
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001A650 File Offset: 0x00019650
		public void AddRange(DirectoryServer[] servers)
		{
			if (servers == null)
			{
				throw new ArgumentNullException("servers");
			}
			for (int i = 0; i < servers.Length; i++)
			{
				if (servers[i] == null)
				{
					throw new ArgumentException("servers");
				}
			}
			for (int j = 0; j < servers.Length; j++)
			{
				this.Add(servers[j]);
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001A6A8 File Offset: 0x000196A8
		public bool Contains(DirectoryServer server)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DirectoryServer directoryServer = (DirectoryServer)base.InnerList[i];
				if (Utils.Compare(directoryServer.Name, server.Name) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0001A701 File Offset: 0x00019701
		public void CopyTo(DirectoryServer[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x0001A710 File Offset: 0x00019710
		public int IndexOf(DirectoryServer server)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DirectoryServer directoryServer = (DirectoryServer)base.InnerList[i];
				if (Utils.Compare(directoryServer.Name, server.Name) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0001A76C File Offset: 0x0001976C
		public void Insert(int index, DirectoryServer server)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			if (this.isForNC)
			{
				if (!this.isADAM)
				{
					if (!(server is DomainController))
					{
						throw new ArgumentException(Res.GetString("ServerShouldBeDC"), "server");
					}
					if (((DomainController)server).NumericOSVersion < 5.2)
					{
						throw new ArgumentException(Res.GetString("ServerShouldBeW2K3"), "server");
					}
				}
				if (!this.Contains(server))
				{
					base.List.Insert(index, server);
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { server }), "server");
			}
			else
			{
				string text = ((server is DomainController) ? ((DomainController)server).SiteObjectName : ((AdamInstance)server).SiteObjectName);
				if (Utils.Compare(this.siteDN, text) != 0)
				{
					throw new ArgumentException(Res.GetString("NotWithinSite"), "server");
				}
				if (!this.Contains(server))
				{
					base.List.Insert(index, server);
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { server }));
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0001A890 File Offset: 0x00019890
		public void Remove(DirectoryServer server)
		{
			if (server == null)
			{
				throw new ArgumentNullException("server");
			}
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DirectoryServer directoryServer = (DirectoryServer)base.InnerList[i];
				if (Utils.Compare(directoryServer.Name, server.Name) == 0)
				{
					base.List.Remove(directoryServer);
					return;
				}
			}
			throw new ArgumentException(Res.GetString("NotFoundInCollection", new object[] { server }), "server");
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0001A914 File Offset: 0x00019914
		protected override void OnClear()
		{
			if (this.initialized && !this.isForNC)
			{
				this.copyList.Clear();
				foreach (object obj in base.List)
				{
					this.copyList.Add(obj);
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001A98C File Offset: 0x0001998C
		protected override void OnClearComplete()
		{
			if (this.isForNC)
			{
				if (this.crossRefEntry == null)
				{
					return;
				}
				try
				{
					if (this.crossRefEntry.Properties.Contains(PropertyManager.MsDSNCReplicaLocations))
					{
						this.crossRefEntry.Properties[PropertyManager.MsDSNCReplicaLocations].Clear();
					}
					return;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
			if (this.initialized)
			{
				for (int i = 0; i < this.copyList.Count; i++)
				{
					this.OnRemoveComplete(i, this.copyList[i]);
				}
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001AA2C File Offset: 0x00019A2C
		protected override void OnInsertComplete(int index, object value)
		{
			if (this.isForNC)
			{
				if (this.crossRefEntry == null)
				{
					return;
				}
				try
				{
					DirectoryServer directoryServer = (DirectoryServer)value;
					string text = ((directoryServer is DomainController) ? ((DomainController)directoryServer).NtdsaObjectName : ((AdamInstance)directoryServer).NtdsaObjectName);
					this.crossRefEntry.Properties[PropertyManager.MsDSNCReplicaLocations].Add(text);
					return;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
			if (this.initialized)
			{
				DirectoryServer directoryServer2 = (DirectoryServer)value;
				string name = directoryServer2.Name;
				string text2 = ((directoryServer2 is DomainController) ? ((DomainController)directoryServer2).ServerObjectName : ((AdamInstance)directoryServer2).ServerObjectName);
				try
				{
					if (this.changeList.Contains(name))
					{
						((DirectoryEntry)this.changeList[name]).Properties["bridgeheadTransportList"].Value = this.transportDN;
					}
					else
					{
						DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
						directoryEntry.Properties["bridgeheadTransportList"].Value = this.transportDN;
						this.changeList.Add(name, directoryEntry);
					}
				}
				catch (COMException ex2)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
				}
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001AB84 File Offset: 0x00019B84
		protected override void OnRemoveComplete(int index, object value)
		{
			if (this.isForNC)
			{
				try
				{
					if (this.crossRefEntry != null)
					{
						string text = ((value is DomainController) ? ((DomainController)value).NtdsaObjectName : ((AdamInstance)value).NtdsaObjectName);
						this.crossRefEntry.Properties[PropertyManager.MsDSNCReplicaLocations].Remove(text);
					}
					return;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
			DirectoryServer directoryServer = (DirectoryServer)value;
			string name = directoryServer.Name;
			string text2 = ((directoryServer is DomainController) ? ((DomainController)directoryServer).ServerObjectName : ((AdamInstance)directoryServer).ServerObjectName);
			try
			{
				if (this.changeList.Contains(name))
				{
					((DirectoryEntry)this.changeList[name]).Properties["bridgeheadTransportList"].Clear();
				}
				else
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
					directoryEntry.Properties["bridgeheadTransportList"].Clear();
					this.changeList.Add(name, directoryEntry);
				}
			}
			catch (COMException ex2)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001ACB8 File Offset: 0x00019CB8
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			this.OnRemoveComplete(index, oldValue);
			this.OnInsertComplete(index, newValue);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001ACCC File Offset: 0x00019CCC
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this.isForNC)
			{
				if (this.isADAM)
				{
					if (!(value is AdamInstance))
					{
						throw new ArgumentException(Res.GetString("ServerShouldBeAI"), "value");
					}
				}
				else if (!(value is DomainController))
				{
					throw new ArgumentException(Res.GetString("ServerShouldBeDC"), "value");
				}
			}
			else if (!(value is DirectoryServer))
			{
				throw new ArgumentException("value");
			}
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001AD44 File Offset: 0x00019D44
		internal string[] GetMultiValuedProperty()
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				DirectoryServer directoryServer = (DirectoryServer)base.InnerList[i];
				string text = ((directoryServer is DomainController) ? ((DomainController)directoryServer).NtdsaObjectName : ((AdamInstance)directoryServer).NtdsaObjectName);
				arrayList.Add(text);
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x040003F8 RID: 1016
		internal string siteDN;

		// Token: 0x040003F9 RID: 1017
		internal string transportDN;

		// Token: 0x040003FA RID: 1018
		internal DirectoryContext context;

		// Token: 0x040003FB RID: 1019
		internal bool initialized;

		// Token: 0x040003FC RID: 1020
		internal Hashtable changeList;

		// Token: 0x040003FD RID: 1021
		private ArrayList copyList = new ArrayList();

		// Token: 0x040003FE RID: 1022
		private DirectoryEntry crossRefEntry;

		// Token: 0x040003FF RID: 1023
		private bool isADAM;

		// Token: 0x04000400 RID: 1024
		private bool isForNC;
	}
}
