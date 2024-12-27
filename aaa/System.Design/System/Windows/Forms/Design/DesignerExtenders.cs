using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000216 RID: 534
	internal class DesignerExtenders
	{
		// Token: 0x06001421 RID: 5153 RVA: 0x000667B4 File Offset: 0x000657B4
		public DesignerExtenders(IExtenderProviderService ex)
		{
			this.extenderService = ex;
			if (this.providers == null)
			{
				this.providers = new IExtenderProvider[]
				{
					new DesignerExtenders.NameExtenderProvider(),
					new DesignerExtenders.NameInheritedExtenderProvider()
				};
			}
			for (int i = 0; i < this.providers.Length; i++)
			{
				ex.AddExtenderProvider(this.providers[i]);
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00066818 File Offset: 0x00065818
		public void Dispose()
		{
			if (this.extenderService != null && this.providers != null)
			{
				for (int i = 0; i < this.providers.Length; i++)
				{
					this.extenderService.RemoveExtenderProvider(this.providers[i]);
				}
				this.providers = null;
				this.extenderService = null;
			}
		}

		// Token: 0x040011F0 RID: 4592
		private IExtenderProvider[] providers;

		// Token: 0x040011F1 RID: 4593
		private IExtenderProviderService extenderService;

		// Token: 0x02000217 RID: 535
		[ProvideProperty("Name", typeof(IComponent))]
		private class NameExtenderProvider : IExtenderProvider
		{
			// Token: 0x06001423 RID: 5155 RVA: 0x00066869 File Offset: 0x00065869
			internal NameExtenderProvider()
			{
			}

			// Token: 0x06001424 RID: 5156 RVA: 0x00066874 File Offset: 0x00065874
			protected IComponent GetBaseComponent(object o)
			{
				if (this.baseComponent == null)
				{
					ISite site = ((IComponent)o).Site;
					if (site != null)
					{
						IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
						if (designerHost != null)
						{
							this.baseComponent = designerHost.RootComponent;
						}
					}
				}
				return this.baseComponent;
			}

			// Token: 0x06001425 RID: 5157 RVA: 0x000668C4 File Offset: 0x000658C4
			public virtual bool CanExtend(object o)
			{
				IComponent component = this.GetBaseComponent(o);
				return component == o || TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited);
			}

			// Token: 0x06001426 RID: 5158 RVA: 0x00066904 File Offset: 0x00065904
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			[SRDescription("DesignerPropName")]
			[MergableProperty(false)]
			[ParenthesizePropertyName(true)]
			[Category("Design")]
			public virtual string GetName(IComponent comp)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					return site.Name;
				}
				return null;
			}

			// Token: 0x06001427 RID: 5159 RVA: 0x00066924 File Offset: 0x00065924
			public void SetName(IComponent comp, string newName)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					site.Name = newName;
				}
			}

			// Token: 0x040011F2 RID: 4594
			private IComponent baseComponent;
		}

		// Token: 0x02000218 RID: 536
		private class NameInheritedExtenderProvider : DesignerExtenders.NameExtenderProvider
		{
			// Token: 0x06001428 RID: 5160 RVA: 0x00066942 File Offset: 0x00065942
			internal NameInheritedExtenderProvider()
			{
			}

			// Token: 0x06001429 RID: 5161 RVA: 0x0006694C File Offset: 0x0006594C
			public override bool CanExtend(object o)
			{
				IComponent baseComponent = base.GetBaseComponent(o);
				return baseComponent != o && !TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited);
			}

			// Token: 0x0600142A RID: 5162 RVA: 0x0006698B File Offset: 0x0006598B
			[ReadOnly(true)]
			public override string GetName(IComponent comp)
			{
				return base.GetName(comp);
			}
		}
	}
}
