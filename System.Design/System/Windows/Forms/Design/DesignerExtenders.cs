using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class DesignerExtenders
	{
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

		private IExtenderProvider[] providers;

		private IExtenderProviderService extenderService;

		[ProvideProperty("Name", typeof(IComponent))]
		private class NameExtenderProvider : IExtenderProvider
		{
			internal NameExtenderProvider()
			{
			}

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

			public virtual bool CanExtend(object o)
			{
				IComponent component = this.GetBaseComponent(o);
				return component == o || TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited);
			}

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

			public void SetName(IComponent comp, string newName)
			{
				ISite site = comp.Site;
				if (site != null)
				{
					site.Name = newName;
				}
			}

			private IComponent baseComponent;
		}

		private class NameInheritedExtenderProvider : DesignerExtenders.NameExtenderProvider
		{
			internal NameInheritedExtenderProvider()
			{
			}

			public override bool CanExtend(object o)
			{
				IComponent baseComponent = base.GetBaseComponent(o);
				return baseComponent != o && !TypeDescriptor.GetAttributes(o)[typeof(InheritanceAttribute)].Equals(InheritanceAttribute.NotInherited);
			}

			[ReadOnly(true)]
			public override string GetName(IComponent comp)
			{
				return base.GetName(comp);
			}
		}
	}
}
