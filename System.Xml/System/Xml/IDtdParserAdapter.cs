using System;
using System.Xml.Schema;

namespace System.Xml
{
	internal interface IDtdParserAdapter
	{
		XmlNameTable NameTable { get; }

		XmlNamespaceManager NamespaceManager { get; }

		bool DtdValidation { get; }

		bool Normalization { get; }

		bool Namespaces { get; }

		bool V1CompatibilityMode { get; }

		Uri BaseUri { get; }

		char[] ParsingBuffer { get; }

		int ParsingBufferLength { get; }

		int CurrentPosition { get; set; }

		int LineNo { get; }

		int LineStartPosition { get; }

		bool IsEof { get; }

		int EntityStackLength { get; }

		bool IsEntityEolNormalized { get; }

		int ReadData();

		void OnNewLine(int pos);

		ValidationEventHandler EventHandler { get; set; }

		void SendValidationEvent(XmlSeverityType severity, XmlSchemaException exception);

		int ParseNumericCharRef(BufferBuilder internalSubsetBuilder);

		int ParseNamedCharRef(bool expand, BufferBuilder internalSubsetBuilder);

		void ParsePI(BufferBuilder sb);

		void ParseComment(BufferBuilder sb);

		bool PushEntity(SchemaEntity entity, int entityId);

		bool PopEntity(out SchemaEntity oldEntity, out int newEntityId);

		bool PushExternalSubset(string systemId, string publicId);

		void PushInternalDtd(string baseUri, string internalDtd);

		void OnSystemId(string systemId, LineInfo keywordLineInfo, LineInfo systemLiteralLineInfo);

		void OnPublicId(string publicId, LineInfo keywordLineInfo, LineInfo publicLiteralLineInfo);

		void Throw(Exception e);
	}
}
