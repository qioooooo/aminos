using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class AssemblyInfo
	{
		public AssemblyInfo(Assembly currentAssembly)
		{
			this.m_Description = null;
			this.m_Title = null;
			this.m_ProductName = null;
			this.m_CompanyName = null;
			this.m_Trademark = null;
			this.m_Copyright = null;
			if (currentAssembly == null)
			{
				throw ExceptionUtils.GetArgumentNullException("CurrentAssembly");
			}
			this.m_Assembly = currentAssembly;
		}

		public string Description
		{
			get
			{
				if (this.m_Description == null)
				{
					AssemblyDescriptionAttribute assemblyDescriptionAttribute = (AssemblyDescriptionAttribute)this.GetAttribute(typeof(AssemblyDescriptionAttribute));
					if (assemblyDescriptionAttribute == null)
					{
						this.m_Description = "";
					}
					else
					{
						this.m_Description = assemblyDescriptionAttribute.Description;
					}
				}
				return this.m_Description;
			}
		}

		public string CompanyName
		{
			get
			{
				if (this.m_CompanyName == null)
				{
					AssemblyCompanyAttribute assemblyCompanyAttribute = (AssemblyCompanyAttribute)this.GetAttribute(typeof(AssemblyCompanyAttribute));
					if (assemblyCompanyAttribute == null)
					{
						this.m_CompanyName = "";
					}
					else
					{
						this.m_CompanyName = assemblyCompanyAttribute.Company;
					}
				}
				return this.m_CompanyName;
			}
		}

		public string Title
		{
			get
			{
				if (this.m_Title == null)
				{
					AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)this.GetAttribute(typeof(AssemblyTitleAttribute));
					if (assemblyTitleAttribute == null)
					{
						this.m_Title = "";
					}
					else
					{
						this.m_Title = assemblyTitleAttribute.Title;
					}
				}
				return this.m_Title;
			}
		}

		public string Copyright
		{
			get
			{
				if (this.m_Copyright == null)
				{
					AssemblyCopyrightAttribute assemblyCopyrightAttribute = (AssemblyCopyrightAttribute)this.GetAttribute(typeof(AssemblyCopyrightAttribute));
					if (assemblyCopyrightAttribute == null)
					{
						this.m_Copyright = "";
					}
					else
					{
						this.m_Copyright = assemblyCopyrightAttribute.Copyright;
					}
				}
				return this.m_Copyright;
			}
		}

		public string Trademark
		{
			get
			{
				if (this.m_Trademark == null)
				{
					AssemblyTrademarkAttribute assemblyTrademarkAttribute = (AssemblyTrademarkAttribute)this.GetAttribute(typeof(AssemblyTrademarkAttribute));
					if (assemblyTrademarkAttribute == null)
					{
						this.m_Trademark = "";
					}
					else
					{
						this.m_Trademark = assemblyTrademarkAttribute.Trademark;
					}
				}
				return this.m_Trademark;
			}
		}

		public string ProductName
		{
			get
			{
				if (this.m_ProductName == null)
				{
					AssemblyProductAttribute assemblyProductAttribute = (AssemblyProductAttribute)this.GetAttribute(typeof(AssemblyProductAttribute));
					if (assemblyProductAttribute == null)
					{
						this.m_ProductName = "";
					}
					else
					{
						this.m_ProductName = assemblyProductAttribute.Product;
					}
				}
				return this.m_ProductName;
			}
		}

		public Version Version
		{
			get
			{
				return this.m_Assembly.GetName().Version;
			}
		}

		public string AssemblyName
		{
			get
			{
				return this.m_Assembly.GetName().Name;
			}
		}

		public string DirectoryPath
		{
			get
			{
				return Path.GetDirectoryName(this.m_Assembly.Location);
			}
		}

		public ReadOnlyCollection<Assembly> LoadedAssemblies
		{
			get
			{
				Collection<Assembly> collection = new Collection<Assembly>();
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					collection.Add(assembly);
				}
				return new ReadOnlyCollection<Assembly>(collection);
			}
		}

		public string StackTrace
		{
			get
			{
				return Environment.StackTrace;
			}
		}

		public long WorkingSet
		{
			get
			{
				return Environment.WorkingSet;
			}
		}

		private object GetAttribute(Type AttributeType)
		{
			object[] customAttributes = this.m_Assembly.GetCustomAttributes(AttributeType, true);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			return customAttributes[0];
		}

		private Assembly m_Assembly;

		private string m_Description;

		private string m_Title;

		private string m_ProductName;

		private string m_CompanyName;

		private string m_Trademark;

		private string m_Copyright;
	}
}
