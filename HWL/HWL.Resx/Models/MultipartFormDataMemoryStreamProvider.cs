using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HWL.Resx.Models
{
    public class MultipartFormDataMemoryStreamProvider : MultipartFormDataStreamProvider
    {
        public MultipartFormDataMemoryStreamProvider()
            : base("/")
        {
        }

        public override Stream GetStream(HttpContent parent, HttpContentHeaders headers)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent");
            }
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            MemoryStream stream = new MemoryStream();
            if (IsFileContent(parent, headers))
            {
                MultipartFileData item = new MultipartFileDataStream(headers, string.Empty, stream);
                this.FileData.Add(item);
            }
            return stream;
        }
        private bool IsFileContent(HttpContent parent, HttpContentHeaders headers)
        {
            ContentDispositionHeaderValue contentDisposition = headers.ContentDisposition;
            if (contentDisposition == null)
            {
                throw new InvalidOperationException("Content-Disposition error");
            }
            return !string.IsNullOrEmpty(contentDisposition.FileName);
        }
    }

    public class MultipartFileDataStream : MultipartFileData, IDisposable
    {
        /// <summary>
        /// file content stream
        /// </summary>
        public Stream Stream { get; private set; }
        public MultipartFileDataStream(HttpContentHeaders headers, string localFileName, Stream stream)
            : base(headers, localFileName)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            this.Stream = stream;
        }
        public void Dispose()
        {
            this.Stream.Dispose();
        }
    }
}