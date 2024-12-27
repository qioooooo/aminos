using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200032D RID: 813
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HtmlControlDesigner : ComponentDesigner
	{
		// Token: 0x06001E6B RID: 7787 RVA: 0x000AC948 File Offset: 0x000AB948
		public HtmlControlDesigner()
		{
			this.shouldCodeSerialize = true;
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x000AC957 File Offset: 0x000AB957
		[Obsolete("Error: This property can no longer be referenced, and is included to support existing compiled applications. The design-time element may not always provide access to the element in the markup. There are alternate methods on WebFormsRootDesigner for handling client script and controls. http://go.microsoft.com/fwlink/?linkid=14202", true)]
		protected object DesignTimeElement
		{
			get
			{
				return this.DesignTimeElementInternal;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000AC95F File Offset: 0x000AB95F
		internal object DesignTimeElementInternal
		{
			get
			{
				if (this.behavior == null)
				{
					return null;
				}
				return this.behavior.DesignTimeElement;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x000AC976 File Offset: 0x000AB976
		// (set) Token: 0x06001E6F RID: 7791 RVA: 0x000AC97E File Offset: 0x000AB97E
		[Obsolete("The recommended alternative is ControlDesigner.Tag. http://go.microsoft.com/fwlink/?linkid=14202")]
		public IHtmlControlDesignerBehavior Behavior
		{
			get
			{
				return this.BehaviorInternal;
			}
			set
			{
				this.BehaviorInternal = value;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x000AC987 File Offset: 0x000AB987
		// (set) Token: 0x06001E71 RID: 7793 RVA: 0x000AC98F File Offset: 0x000AB98F
		internal virtual IHtmlControlDesignerBehavior BehaviorInternal
		{
			get
			{
				return this.behavior;
			}
			set
			{
				if (this.behavior != value)
				{
					if (this.behavior != null)
					{
						this.OnBehaviorDetaching();
						this.behavior.Designer = null;
						this.behavior = null;
					}
					if (value != null)
					{
						this.behavior = value;
						this.OnBehaviorAttached();
					}
				}
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x000AC9CB File Offset: 0x000AB9CB
		public DataBindingCollection DataBindings
		{
			get
			{
				return ((IDataBindingsAccessor)base.Component).DataBindings;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001E73 RID: 7795 RVA: 0x000AC9DD File Offset: 0x000AB9DD
		public ExpressionBindingCollection Expressions
		{
			get
			{
				return ((IExpressionsAccessor)base.Component).Expressions;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000AC9EF File Offset: 0x000AB9EF
		// (set) Token: 0x06001E75 RID: 7797 RVA: 0x000AC9F7 File Offset: 0x000AB9F7
		[Obsolete("Use of this property is not recommended because code serialization is not supported. http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual bool ShouldCodeSerialize
		{
			get
			{
				return this.ShouldCodeSerializeInternal;
			}
			set
			{
				this.ShouldCodeSerializeInternal = value;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x000ACA00 File Offset: 0x000ABA00
		// (set) Token: 0x06001E77 RID: 7799 RVA: 0x000ACA08 File Offset: 0x000ABA08
		internal virtual bool ShouldCodeSerializeInternal
		{
			get
			{
				return this.shouldCodeSerialize;
			}
			set
			{
				this.shouldCodeSerialize = value;
			}
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000ACA11 File Offset: 0x000ABA11
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.BehaviorInternal != null)
			{
				this.BehaviorInternal.Designer = null;
				this.BehaviorInternal = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000ACA38 File Offset: 0x000ABA38
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Control));
			base.Initialize(component);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000ACA51 File Offset: 0x000ABA51
		[Obsolete("The recommended alternative is ControlDesigner.Tag. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void OnBehaviorAttached()
		{
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000ACA53 File Offset: 0x000ABA53
		[Obsolete("The recommended alternative is ControlDesigner.Tag. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void OnBehaviorDetaching()
		{
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000ACA55 File Offset: 0x000ABA55
		public virtual void OnSetParent()
		{
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000ACA58 File Offset: 0x000ABA58
		protected override void PreFilterEvents(IDictionary events)
		{
			base.PreFilterEvents(events);
			if (!this.ShouldCodeSerializeInternal)
			{
				ICollection values = events.Values;
				if (values != null && values.Count != 0)
				{
					object[] array = new object[values.Count];
					values.CopyTo(array, 0);
					foreach (EventDescriptor eventDescriptor in array)
					{
						eventDescriptor = TypeDescriptor.CreateEvent(eventDescriptor.ComponentType, eventDescriptor, new Attribute[] { BrowsableAttribute.No });
						events[eventDescriptor.Name] = eventDescriptor;
					}
				}
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000ACAE0 File Offset: 0x000ABAE0
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties["Modifiers"];
			if (propertyDescriptor != null)
			{
				properties["Modifiers"] = TypeDescriptor.CreateProperty(propertyDescriptor.ComponentType, propertyDescriptor, new Attribute[] { BrowsableAttribute.No });
			}
			properties["Expressions"] = TypeDescriptor.CreateProperty(base.GetType(), "Expressions", typeof(ExpressionBindingCollection), new Attribute[]
			{
				DesignerSerializationVisibilityAttribute.Hidden,
				CategoryAttribute.Data,
				new EditorAttribute(typeof(ExpressionsCollectionEditor), typeof(UITypeEditor)),
				new TypeConverterAttribute(typeof(ExpressionsCollectionConverter)),
				new ParenthesizePropertyNameAttribute(true),
				MergablePropertyAttribute.No,
				new DescriptionAttribute(SR.GetString("Control_Expressions"))
			});
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000ACBBD File Offset: 0x000ABBBD
		[Obsolete("The recommended alternative is to handle the Changed event on the DataBindings collection. The DataBindings collection allows more control of the databindings associated with the control. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected virtual void OnBindingsCollectionChanged(string propName)
		{
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000ACBBF File Offset: 0x000ABBBF
		internal void OnBindingsCollectionChangedInternal(string propName)
		{
			this.OnBindingsCollectionChanged(propName);
		}

		// Token: 0x04001747 RID: 5959
		private IHtmlControlDesignerBehavior behavior;

		// Token: 0x04001748 RID: 5960
		private bool shouldCodeSerialize;
	}
}
