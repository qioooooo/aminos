using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000085 RID: 133
	internal class ApplicationSpec
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x00007EDB File Offset: 0x00006EDB
		internal ApplicationSpec(Assembly asm, RegistrationConfig regConfig)
		{
			this._asm = asm;
			this._regConfig = regConfig;
			this.GenerateNames();
			this.ReadTypes();
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00007EFD File Offset: 0x00006EFD
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x00007F0A File Offset: 0x00006F0A
		internal string Partition
		{
			get
			{
				return this._regConfig.Partition;
			}
			set
			{
				this._regConfig.Partition = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00007F18 File Offset: 0x00006F18
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00007F25 File Offset: 0x00006F25
		internal string Name
		{
			get
			{
				return this._regConfig.Application;
			}
			set
			{
				this._regConfig.Application = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00007F33 File Offset: 0x00006F33
		internal string ID
		{
			get
			{
				return this._appid;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00007F3B File Offset: 0x00006F3B
		internal string TypeLib
		{
			get
			{
				return this._regConfig.TypeLibrary;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00007F48 File Offset: 0x00006F48
		internal string File
		{
			get
			{
				return this._regConfig.AssemblyFile;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00007F55 File Offset: 0x00006F55
		internal string AppRootDir
		{
			get
			{
				return this._regConfig.ApplicationRootDirectory;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00007F62 File Offset: 0x00006F62
		internal Assembly Assembly
		{
			get
			{
				return this._asm;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00007F6A File Offset: 0x00006F6A
		internal Type[] EventTypes
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00007F72 File Offset: 0x00006F72
		internal Type[] NormalTypes
		{
			get
			{
				return this._normal;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00007F7A File Offset: 0x00006F7A
		internal Type[] ConfigurableTypes
		{
			get
			{
				return this._cfgtypes;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00007F82 File Offset: 0x00006F82
		internal string DefinitiveName
		{
			get
			{
				if (this.ID != null)
				{
					return this.ID;
				}
				return this.Name;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00007F9C File Offset: 0x00006F9C
		private string FormatApplicationName(Assembly asm)
		{
			object[] customAttributes = asm.GetCustomAttributes(typeof(ApplicationNameAttribute), true);
			string text;
			if (customAttributes.Length > 0)
			{
				text = ((ApplicationNameAttribute)customAttributes[0]).Value;
			}
			else
			{
				text = asm.GetName().Name;
			}
			return text;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00007FE0 File Offset: 0x00006FE0
		private void GenerateNames()
		{
			if (this._regConfig.TypeLibrary == null || this._regConfig.TypeLibrary.Length == 0)
			{
				string directoryName = Path.GetDirectoryName(this.File);
				this._regConfig.TypeLibrary = Path.Combine(directoryName, this._asm.GetName().Name + ".tlb");
			}
			else
			{
				this._regConfig.TypeLibrary = Path.GetFullPath(this._regConfig.TypeLibrary);
			}
			if (this.Name != null && this.Name.Length != 0 && '{' == this.Name[0])
			{
				this._appid = "{" + new Guid(this.Name) + "}";
				this.Name = null;
			}
			if (this.Name == null || this.Name.Length == 0)
			{
				this.Name = this.FormatApplicationName(this._asm);
			}
			object[] customAttributes = this._asm.GetCustomAttributes(typeof(ApplicationIDAttribute), true);
			if (customAttributes.Length > 0)
			{
				ApplicationIDAttribute applicationIDAttribute = (ApplicationIDAttribute)customAttributes[0];
				this._appid = "{" + new Guid(applicationIDAttribute.Value.ToString()).ToString() + "}";
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000813C File Offset: 0x0000713C
		public bool Matches(ICatalogObject obj)
		{
			if (this.ID != null)
			{
				Guid guid = new Guid(this.ID);
				Guid guid2 = new Guid((string)obj.GetValue("ID"));
				if (guid == guid2)
				{
					return true;
				}
			}
			else
			{
				string text = ((string)obj.GetValue("Name")).ToLower(CultureInfo.InvariantCulture);
				if (this.Name.ToLower(CultureInfo.InvariantCulture) == text)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000081B6 File Offset: 0x000071B6
		public override string ToString()
		{
			if (this.ID != null)
			{
				return "id=" + this.ID;
			}
			return "name=" + this.Name;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x000081E4 File Offset: 0x000071E4
		private void ReadTypes()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			Type[] registrableTypesInAssembly = new RegistrationServices().GetRegistrableTypesInAssembly(this._asm);
			foreach (Type type in registrableTypesInAssembly)
			{
				if (ServicedComponentInfo.IsTypeServicedComponent(type))
				{
					object[] customAttributes = type.GetCustomAttributes(typeof(EventClassAttribute), true);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						arrayList.Add(type);
					}
					else
					{
						arrayList2.Add(type);
					}
				}
			}
			if (arrayList.Count > 0)
			{
				this._events = new Type[arrayList.Count];
				arrayList.CopyTo(this._events);
			}
			else
			{
				this._events = null;
			}
			if (arrayList2.Count > 0)
			{
				this._normal = new Type[arrayList2.Count];
				arrayList2.CopyTo(this._normal);
			}
			else
			{
				this._normal = null;
			}
			int num = ((this._normal != null) ? this._normal.Length : 0) + ((this._events != null) ? this._events.Length : 0);
			if (num > 0)
			{
				this._cfgtypes = new Type[num];
				if (this._events != null)
				{
					this._events.CopyTo(this._cfgtypes, 0);
				}
				if (this._normal != null)
				{
					this._normal.CopyTo(this._cfgtypes, num - this._normal.Length);
				}
			}
		}

		// Token: 0x04000133 RID: 307
		private RegistrationConfig _regConfig;

		// Token: 0x04000134 RID: 308
		private Assembly _asm;

		// Token: 0x04000135 RID: 309
		private Type[] _events;

		// Token: 0x04000136 RID: 310
		private Type[] _normal;

		// Token: 0x04000137 RID: 311
		private Type[] _cfgtypes;

		// Token: 0x04000138 RID: 312
		private string _appid;
	}
}
