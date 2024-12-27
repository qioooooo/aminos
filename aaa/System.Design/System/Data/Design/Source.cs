using System;
using System.CodeDom;
using System.ComponentModel;

namespace System.Data.Design
{
	// Token: 0x02000089 RID: 137
	internal abstract class Source : DataSourceComponent, IDataSourceNamedObject, INamedObject, ICloneable
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x0000A953 File Offset: 0x00009953
		internal Source()
		{
			this.modifier = MemberAttributes.Public;
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0000A966 File Offset: 0x00009966
		// (set) Token: 0x06000565 RID: 1381 RVA: 0x0000A96E File Offset: 0x0000996E
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0000A978 File Offset: 0x00009978
		internal bool IsMainSource
		{
			get
			{
				DesignTable designTable = this.Owner as DesignTable;
				return designTable != null && designTable.MainSource == this;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0000A99F File Offset: 0x0000999F
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x0000A9A7 File Offset: 0x000099A7
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0000A9B0 File Offset: 0x000099B0
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x0000A9B8 File Offset: 0x000099B8
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

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000A9E4 File Offset: 0x000099E4
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x0000A9EC File Offset: 0x000099EC
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

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000A9F0 File Offset: 0x000099F0
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0000AA2E File Offset: 0x00009A2E
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

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0000AA37 File Offset: 0x00009A37
		[Browsable(false)]
		public virtual string PublicTypeName
		{
			get
			{
				return "Function";
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0000AA3E File Offset: 0x00009A3E
		// (set) Token: 0x06000571 RID: 1393 RVA: 0x0000AA46 File Offset: 0x00009A46
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

		// Token: 0x06000572 RID: 1394
		public abstract object Clone();

		// Token: 0x06000573 RID: 1395 RVA: 0x0000AA4F File Offset: 0x00009A4F
		internal virtual bool NameExist(string nameToCheck)
		{
			return StringUtil.EqualValue(this.Name, nameToCheck, true);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000AA5E File Offset: 0x00009A5E
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

		// Token: 0x06000575 RID: 1397 RVA: 0x0000AA7E File Offset: 0x00009A7E
		public override string ToString()
		{
			return this.PublicTypeName + " " + this.DisplayName;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x0000AA96 File Offset: 0x00009A96
		// (set) Token: 0x06000577 RID: 1399 RVA: 0x0000AA9E File Offset: 0x00009A9E
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0000AAA7 File Offset: 0x00009AA7
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0000AAAF File Offset: 0x00009AAF
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0000AAB8 File Offset: 0x00009AB8
		// (set) Token: 0x0600057B RID: 1403 RVA: 0x0000AAC0 File Offset: 0x00009AC0
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0000AAC9 File Offset: 0x00009AC9
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x0000AAD1 File Offset: 0x00009AD1
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0000AADA File Offset: 0x00009ADA
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0000AAE2 File Offset: 0x00009AE2
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000AAEB File Offset: 0x00009AEB
		[Browsable(false)]
		public override string GeneratorName
		{
			get
			{
				return this.GeneratorSourceName;
			}
		}

		// Token: 0x04000AE5 RID: 2789
		protected string name;

		// Token: 0x04000AE6 RID: 2790
		private MemberAttributes modifier;

		// Token: 0x04000AE7 RID: 2791
		protected DataSourceComponent owner;

		// Token: 0x04000AE8 RID: 2792
		private bool webMethod;

		// Token: 0x04000AE9 RID: 2793
		private string webMethodDescription;

		// Token: 0x04000AEA RID: 2794
		private string userSourceName;

		// Token: 0x04000AEB RID: 2795
		private string generatorSourceName;

		// Token: 0x04000AEC RID: 2796
		private string generatorGetMethodName;

		// Token: 0x04000AED RID: 2797
		private string generatorSourceNameForPaging;

		// Token: 0x04000AEE RID: 2798
		private string generatorGetMethodNameForPaging;
	}
}
