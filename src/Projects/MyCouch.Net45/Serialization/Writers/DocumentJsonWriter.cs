﻿using System.IO;
using EnsureThat;
using MyCouch.Serialization.Conventions;
using MyCouch.Serialization.Meta;
using Newtonsoft.Json;

namespace MyCouch.Serialization.Writers
{
    public class DocumentJsonWriter : JsonTextWriter
    {
        protected readonly DocumentSerializationMeta DocumentMeta;
        protected bool HasWrittenDocumentMeta { get; set; }

        public SerializationConventions Conventions { get; set; }

        public DocumentJsonWriter(DocumentSerializationMeta documentMeta, TextWriter textWriter)
            : base(textWriter)
        {
            Ensure.That(documentMeta, "documentMeta").IsNotNull();

            HasWrittenDocumentMeta = false;
            DocumentMeta = documentMeta;
            Conventions = new SerializationConventions();
        }

        public override void WriteStartObject()
        {
            base.WriteStartObject();

            WriteDocumentMeta(DocumentMeta);
        }

        protected virtual void WriteDocumentMeta(DocumentSerializationMeta meta)
        {
            if (Conventions == null || HasWrittenDocumentMeta)
                return;

            HasWrittenDocumentMeta = true;

            if (Conventions.DocType != null)
            {
                WritePropertyName(Conventions.DocType.PropertyName);
                WriteValue(Conventions.DocType.Convention(meta));
            }
        }

        public override void WriteNull()
        {
            base.WriteRaw(string.Empty);
        }
    }
}