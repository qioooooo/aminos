using System;
using System.CodeDom;
using System.ComponentModel;

namespace System.Data.Design
{
	internal abstract class Source : DataSourceComponent, IDataSourceNamedObject, INamedObject, ICloneable
	{
		internal Source()
		{
			this.modifier = MemberAttributes.Public;
		}

		[DefaultValue(false)]
		[DataSourceXmlAttribute]
		public bool EnableWebMethods
		{
			get
			{
				return this.webMethod;
			}
			set
			{
				this.webMethod = value;
			}
		}

		internal bool IsMainSource
		{
			get
			{
				DesignTable designTable = this.Owner as DesignTable;
				return designTable != null && designTable.MainSource == this;
			}
		}

		[DefaultValue(MemberAttributes.Public)]
		[DataSourceXmlAttribute]
		public MemberAttributes Modifier
		{
			get
			{
				return this.modifier;
			}
			set
			{
				this.modifier = value;
			}
		}

		[DefaultValue("")]
		[MergableProperty(false)]
		[DataSourceXmlAttribute]
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					if (this.CollectionParent != null)
					{
						this.CollectionParent.ValidateUniqueName(this, value);
					}
					this.name = value;
				}
			}
		}

		internal virtual string DisplayName
		{
			get
			{
				return this.Name;
			}
			set
			{
			}
		}

		[Browsable(false)]
		internal DataSourceComponent Owner
		{
			get
			{
				if (this.owner == null && this.CollectionParent != null)
				{
					SourceCollection sourceCollection = this.CollectionParent as SourceCollection;
					if (sourceCollection != null)
					{
						this.owner = sourceCollection.CollectionHost;
					}
				}
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		[Browsable(false)]
		public virtual string PublicTypeName
		{
			get
			{
				return "Function";
			}
		}

		[DefaultValue("")]
		[DataSourceXmlAttribute]
		public string WebMethodDescription
		{
			get
			{
				return this.webMethodDescription;
			}
			set
			{
				this.webMethodDescription = value;
			}
		}

		public abstract object Clone();

		internal virtual bool NameExist(string nameToCheck)
		{
			return StringUtil.EqualValue(this.Name, nameToCheck, true);
		}

		public override void SetCollection(DataSourceCollectionBase collection)
		{
			base.SetCollection(collection);
			if (collection != null)
			{
				this.Owner = collection.CollectionHost;
				return;
			}
			this.Owner = null;
		}

		public override string ToString()
		{
			return this.PublicTypeName + " " + this.DisplayName;
		}

		[DefaultValue(null)]
		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string UserSourceName
		{
			get
			{
				return this.userSourceName;
			}
			set
			{
				this.userSourceName = value;
			}
		}

		[DataSourceXmlAttribute]
		[Browsable(false)]
		[DefaultValue(null)]
		public string GeneratorSourceName
		{
			get
			{
				return this.generatorSourceName;
			}
			set
			{
				this.generatorSourceName = value;
			}
		}

		[Browsable(false)]
		[DefaultValue(null)]
		[DataSourceXmlAttribute]
		public string GeneratorGetMethodName
		{
			get
			{
				return this.generatorGetMethodName;
			}
			set
			{
				this.generatorGetMethodName = value;
			}
		}

		[DefaultValue(null)]
		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string GeneratorSourceNameForPaging
		{
			get
			{
				return this.generatorSourceNameForPaging;
			}
			set
			{
				this.generatorSourceNameForPaging = value;
			}
		}

		[DefaultValue(null)]
		[DataSourceXmlAttribute]
		[Browsable(false)]
		public string GeneratorGetMethodNameForPaging
		{
			get
			{
				return this.generatorGetMethodNameForPaging;
			}
			set
			{
				this.generatorGetMethodNameForPaging = value;
			}
		}

		[Browsable(false)]
		public override string GeneratorName
		{
			get
			{
				return this.GeneratorSourceName;
			}
		}

		protected string name;

		private MemberAttributes modifier;

		protected DataSourceComponent owner;

		private bool webMethod;

		private string webMethodDescription;

		private string userSourceName;

		private string generatorSourceName;

		private string generatorGetMethodName;

		private string generatorSourceNameForPaging;

		private string generatorGetMethodNameForPaging;
	}
}
