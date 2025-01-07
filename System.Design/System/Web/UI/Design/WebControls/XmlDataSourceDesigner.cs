using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.IO;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlDataSourceDesigner : HierarchicalDataSourceDesigner, IDataSourceDesigner
	{
		public override bool CanConfigure
		{
			get
			{
				return true;
			}
		}

		public override bool CanRefreshSchema
		{
			get
			{
				return true;
			}
		}

		public string Data
		{
			get
			{
				return this.XmlDataSource.Data;
			}
			set
			{
				if (value != this.XmlDataSource.Data)
				{
					this.XmlDataSource.Data = value;
					this.OnDataSourceChanged(EventArgs.Empty);
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
			}
		}

		public string DataFile
		{
			get
			{
				return this.XmlDataSource.DataFile;
			}
			set
			{
				if (value != this.XmlDataSource.DataFile)
				{
					this._mappedDataFile = null;
					this.XmlDataSource.DataFile = value;
					this.OnDataSourceChanged(EventArgs.Empty);
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
			}
		}

		public string Transform
		{
			get
			{
				return this.XmlDataSource.Transform;
			}
			set
			{
				if (value != this.XmlDataSource.Transform)
				{
					this.XmlDataSource.Transform = value;
					this.OnDataSourceChanged(EventArgs.Empty);
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
			}
		}

		public string TransformFile
		{
			get
			{
				return this.XmlDataSource.TransformFile;
			}
			set
			{
				if (value != this.XmlDataSource.TransformFile)
				{
					this._mappedTransformFile = null;
					this.XmlDataSource.TransformFile = value;
					this.OnDataSourceChanged(EventArgs.Empty);
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
			}
		}

		private XmlDataSource XmlDataSource
		{
			get
			{
				return this._xmlDataSource;
			}
		}

		public string XPath
		{
			get
			{
				return this.XmlDataSource.XPath;
			}
			set
			{
				if (value != this.XmlDataSource.XPath)
				{
					this.XmlDataSource.XPath = value;
					this.OnDataSourceChanged(EventArgs.Empty);
					this.OnSchemaRefreshed(EventArgs.Empty);
				}
			}
		}

		public override void Configure()
		{
			ControlDesigner.InvokeTransactedChange(base.Component, new TransactedChangeCallback(this.ConfigureDataSourceChangeCallback), null, SR.GetString("DataSource_ConfigureTransactionDescription"));
		}

		private bool ConfigureDataSourceChangeCallback(object context)
		{
			bool flag;
			try
			{
				this.SuppressDataSourceEvents();
				IServiceProvider site = base.Component.Site;
				XmlDataSourceConfigureDataSourceForm xmlDataSourceConfigureDataSourceForm = new XmlDataSourceConfigureDataSourceForm(site, this.XmlDataSource);
				DialogResult dialogResult = UIServiceHelper.ShowDialog(site, xmlDataSourceConfigureDataSourceForm);
				flag = dialogResult == DialogResult.OK;
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
			return flag;
		}

		internal XmlDataSource GetDesignTimeXmlDataSource(string viewPath)
		{
			XmlDataSource xmlDataSource = new XmlDataSource();
			xmlDataSource.EnableCaching = false;
			xmlDataSource.Data = this.XmlDataSource.Data;
			xmlDataSource.Transform = this.XmlDataSource.Transform;
			xmlDataSource.XPath = (string.IsNullOrEmpty(viewPath) ? this.XmlDataSource.XPath : viewPath);
			if (this.XmlDataSource.DataFile.Length > 0)
			{
				if (this._mappedDataFile == null)
				{
					this._mappedDataFile = UrlPath.MapPath(base.Component.Site, this.XmlDataSource.DataFile);
				}
				xmlDataSource.DataFile = this._mappedDataFile;
				if (!File.Exists(xmlDataSource.DataFile))
				{
					return null;
				}
			}
			else if (xmlDataSource.Data.Length == 0)
			{
				return null;
			}
			if (this.XmlDataSource.TransformFile.Length > 0)
			{
				if (this._mappedTransformFile == null)
				{
					this._mappedTransformFile = UrlPath.MapPath(base.Component.Site, this.XmlDataSource.TransformFile);
				}
				xmlDataSource.TransformFile = this._mappedTransformFile;
				if (!File.Exists(xmlDataSource.TransformFile))
				{
					return null;
				}
			}
			return xmlDataSource;
		}

		internal IHierarchicalEnumerable GetHierarchicalRuntimeEnumerable(string path)
		{
			XmlDataSource designTimeXmlDataSource = this.GetDesignTimeXmlDataSource(string.Empty);
			if (designTimeXmlDataSource == null)
			{
				return null;
			}
			HierarchicalDataSourceView hierarchicalView = ((IHierarchicalDataSource)designTimeXmlDataSource).GetHierarchicalView(path);
			if (hierarchicalView == null)
			{
				return null;
			}
			return hierarchicalView.Select();
		}

		internal IEnumerable GetRuntimeEnumerable(string listName)
		{
			XmlDataSource designTimeXmlDataSource = this.GetDesignTimeXmlDataSource(string.Empty);
			if (designTimeXmlDataSource == null)
			{
				return null;
			}
			XmlDataSourceView xmlDataSourceView = (XmlDataSourceView)((IDataSource)designTimeXmlDataSource).GetView(listName);
			if (xmlDataSourceView == null)
			{
				return null;
			}
			IEnumerable enumerable = xmlDataSourceView.Select(DataSourceSelectArguments.Empty);
			ICollection collection = enumerable as ICollection;
			if (collection != null && collection.Count == 0)
			{
				return null;
			}
			return enumerable;
		}

		public override DesignerHierarchicalDataSourceView GetView(string viewPath)
		{
			return new XmlDesignerHierarchicalDataSourceView(this, viewPath);
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(XmlDataSource));
			base.Initialize(component);
			this._xmlDataSource = (XmlDataSource)component;
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			foreach (string text in XmlDataSourceDesigner._shadowProperties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[text];
				properties[text] = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[0]);
			}
		}

		public override void RefreshSchema(bool preferSilent)
		{
			try
			{
				this.SuppressDataSourceEvents();
				this.OnDataSourceChanged(EventArgs.Empty);
				this.OnSchemaRefreshed(EventArgs.Empty);
			}
			finally
			{
				this.ResumeDataSourceEvents();
			}
		}

		bool IDataSourceDesigner.CanConfigure
		{
			get
			{
				return this.CanConfigure;
			}
		}

		bool IDataSourceDesigner.CanRefreshSchema
		{
			get
			{
				return this.CanRefreshSchema;
			}
		}

		event EventHandler IDataSourceDesigner.DataSourceChanged
		{
			add
			{
				base.DataSourceChanged += value;
			}
			remove
			{
				base.DataSourceChanged -= value;
			}
		}

		event EventHandler IDataSourceDesigner.SchemaRefreshed
		{
			add
			{
				base.SchemaRefreshed += value;
			}
			remove
			{
				base.SchemaRefreshed -= value;
			}
		}

		void IDataSourceDesigner.Configure()
		{
			this.Configure();
		}

		DesignerDataSourceView IDataSourceDesigner.GetView(string viewName)
		{
			if (!string.IsNullOrEmpty(viewName))
			{
				return null;
			}
			if (this._view == null)
			{
				this._view = new XmlDesignerDataSourceView(this, string.Empty);
			}
			return this._view;
		}

		string[] IDataSourceDesigner.GetViewNames()
		{
			return new string[0];
		}

		void IDataSourceDesigner.RefreshSchema(bool preferSilent)
		{
			this.RefreshSchema(preferSilent);
		}

		void IDataSourceDesigner.ResumeDataSourceEvents()
		{
			this.ResumeDataSourceEvents();
		}

		void IDataSourceDesigner.SuppressDataSourceEvents()
		{
			this.SuppressDataSourceEvents();
		}

		private string _mappedDataFile;

		private string _mappedTransformFile;

		private XmlDataSource _xmlDataSource;

		private XmlDesignerDataSourceView _view;

		private static readonly string[] _shadowProperties = new string[] { "Data", "DataFile", "Transform", "TransformFile", "XPath" };
	}
}
