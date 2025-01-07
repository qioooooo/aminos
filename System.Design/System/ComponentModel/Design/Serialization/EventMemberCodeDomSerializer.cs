using System;
using System.CodeDom;
using System.Design;
using System.Reflection;

namespace System.ComponentModel.Design.Serialization
{
	internal sealed class EventMemberCodeDomSerializer : MemberCodeDomSerializer
	{
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

		public override bool ShouldSerialize(IDesignerSerializationManager manager, object value, MemberDescriptor descriptor)
		{
			return true;
		}

		private static CodeThisReferenceExpression _thisRef = new CodeThisReferenceExpression();

		private static EventMemberCodeDomSerializer _default;
	}
}
