using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001C4 RID: 452
	[ConfigurationCollection(typeof(CodeSubDirectory), CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CodeSubDirectoriesCollection : ConfigurationElementCollection
	{
		// Token: 0x060019A2 RID: 6562 RVA: 0x000794E4 File Offset: 0x000784E4
		public CodeSubDirectoriesCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060019A3 RID: 6563 RVA: 0x000794F1 File Offset: 0x000784F1
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CodeSubDirectoriesCollection._properties;
			}
		}

		// Token: 0x170004BD RID: 1213
		public CodeSubDirectory this[int index]
		{
			get
			{
				return (CodeSubDirectory)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00079520 File Offset: 0x00078520
		public void Add(CodeSubDirectory codeSubDirectory)
		{
			this.BaseAdd(codeSubDirectory);
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00079529 File Offset: 0x00078529
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x00079531 File Offset: 0x00078531
		public void Remove(string directoryName)
		{
			base.BaseRemove(directoryName);
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0007953A File Offset: 0x0007853A
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x00079543 File Offset: 0x00078543
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00079546 File Offset: 0x00078546
		protected override ConfigurationElement CreateNewElement()
		{
			return new CodeSubDirectory();
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060019AC RID: 6572 RVA: 0x0007954D File Offset: 0x0007854D
		protected override string ElementName
		{
			get
			{
				return "add";
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00079554 File Offset: 0x00078554
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((CodeSubDirectory)element).DirectoryName;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00079564 File Offset: 0x00078564
		internal void EnsureRuntimeValidation()
		{
			if (this._didRuntimeValidation)
			{
				return;
			}
			foreach (object obj in this)
			{
				CodeSubDirectory codeSubDirectory = (CodeSubDirectory)obj;
				codeSubDirectory.DoRuntimeValidation();
			}
			this._didRuntimeValidation = true;
		}

		// Token: 0x04001784 RID: 6020
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001785 RID: 6021
		private bool _didRuntimeValidation;
	}
}
