using System;
using System.CodeDom;
using System.Design;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200015D RID: 349
	internal sealed class EventMemberCodeDomSerializer : MemberCodeDomSerializer
	{
		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x00033F98 File Offset: 0x00032F98
		internal static EventMemberCodeDomSerializer Default
		{
			get
			{
				if (EventMemberCodeDomSerializer._default == null)
				{
					EventMemberCodeDomSerializer._default = new EventMemberCodeDomSerializer();
				}
				return EventMemberCodeDomSerializer._default;
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00033FB0 File Offset: 0x00032FB0
		public override void Serialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor, CodeStatementCollection statements)
		{
			EventDescriptor eventDescriptor = descriptor as EventDescriptor;
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (eventDescriptor == null)
			{
				throw new ArgumentNullException("descriptor");
			}
			if (statements == null)
			{
				throw new ArgumentNullException("statements");
			}
			try
			{
				IEventBindingService eventBindingService = (IEventBindingService)manager.GetService(typeof(IEventBindingService));
				if (eventBindingService != null)
				{
					PropertyDescriptor eventProperty = eventBindingService.GetEventProperty(eventDescriptor);
					string text = (string)eventProperty.GetValue(value);
					if (text != null)
					{
						CodeExpression codeExpression = base.SerializeToExpression(manager, value);
						if (codeExpression != null)
						{
							CodeTypeReference codeTypeReference = new CodeTypeReference(eventDescriptor.EventType);
							CodeDelegateCreateExpression codeDelegateCreateExpression = new CodeDelegateCreateExpression(codeTypeReference, EventMemberCodeDomSerializer._thisRef, text);
							CodeEventReferenceExpression codeEventReferenceExpression = new CodeEventReferenceExpression(codeExpression, eventDescriptor.Name);
							CodeAttachEventStatement codeAttachEventStatement = new CodeAttachEventStatement(codeEventReferenceExpression, codeDelegateCreateExpression);
							codeAttachEventStatement.UserData[typeof(Delegate)] = eventDescriptor.EventType;
							statements.Add(codeAttachEventStatement);
						}
					}
				}
			}
			catch (Exception innerException)
			{
				if (innerException is TargetInvocationException)
				{
					innerException = innerException.InnerException;
				}
				manager.ReportError(SR.GetString("SerializerPropertyGenFailed", new object[] { eventDescriptor.Name, innerException.Message }));
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x000340F4 File Offset: 0x000330F4
		public override bool ShouldSerialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor)
		{
			return true;
		}

		// Token: 0x04000EEC RID: 3820
		private static CodeThisReferenceExpression _thisRef = new CodeThisReferenceExpression();

		// Token: 0x04000EED RID: 3821
		private static EventMemberCodeDomSerializer _default;
	}
}
