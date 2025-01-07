﻿using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Xml.Schema
{
	[Serializable]
	public class XmlSchemaValidationException : XmlSchemaException
	{
		protected XmlSchemaValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		public XmlSchemaValidationException()
			: base(null)
		{
		}

		public XmlSchemaValidationException(string message)
			: base(message, null, 0, 0)
		{
		}

		public XmlSchemaValidationException(string message, Exception innerException)
			: base(message, innerException, 0, 0)
		{
		}

		public XmlSchemaValidationException(string message, Exception innerException, int lineNumber, int linePosition)
			: base(message, innerException, lineNumber, linePosition)
		{
		}

		internal XmlSchemaValidationException(string res, string[] args)
			: base(res, args, null, null, 0, 0, null)
		{
		}

		internal XmlSchemaValidationException(string res, string arg)
			: base(res, new string[] { arg }, null, null, 0, 0, null)
		{
		}

		internal XmlSchemaValidationException(string res, string arg, string sourceUri, int lineNumber, int linePosition)
			: base(res, new string[] { arg }, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		internal XmlSchemaValidationException(string res, string sourceUri, int lineNumber, int linePosition)
			: base(res, null, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		internal XmlSchemaValidationException(string res, string[] args, string sourceUri, int lineNumber, int linePosition)
			: base(res, args, null, sourceUri, lineNumber, linePosition, null)
		{
		}

		internal XmlSchemaValidationException(string res, string[] args, Exception innerException, string sourceUri, int lineNumber, int linePosition)
			: base(res, args, innerException, sourceUri, lineNumber, linePosition, null)
		{
		}

		internal XmlSchemaValidationException(string res, string[] args, object sourceNode)
			: base(res, args, null, null, 0, 0, null)
		{
			this.sourceNodeObject = sourceNode;
		}

		internal XmlSchemaValidationException(string res, string[] args, string sourceUri, object sourceNode)
			: base(res, args, null, sourceUri, 0, 0, null)
		{
			this.sourceNodeObject = sourceNode;
		}

		internal XmlSchemaValidationException(string res, string[] args, string sourceUri, int lineNumber, int linePosition, XmlSchemaObject source, object sourceNode)
			: base(res, args, null, sourceUri, lineNumber, linePosition, source)
		{
			this.sourceNodeObject = sourceNode;
		}

		public object SourceObject
		{
			get
			{
				return this.sourceNodeObject;
			}
		}

		protected internal void SetSourceObject(object sourceObject)
		{
			this.sourceNodeObject = sourceObject;
		}

		private object sourceNodeObject;
	}
}
