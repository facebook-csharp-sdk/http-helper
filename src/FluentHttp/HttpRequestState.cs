namespace FluentHttp
{
    using System;
    using System.IO;
    using System.Net;

    class HttpRequestState : IDisposable
    {
        public int BufferSize { get; private set; }

        public HttpRequestState(int bufferSize, FluentHttpAsyncResultOld asyncResult)
        {
            BufferSize = bufferSize;
            AsyncResult = asyncResult;
        }

        /// <summary>
        /// Gets or sets the total bytes read during current transfer.
        /// </summary>
        public long BytesRead { get; set; }

        /// <summary>
        /// Gets or sets buffer to read data into.
        /// </summary>
        public byte[] Buffer { get; set; }

        public FluentHttpRequestOld Request { get; set; }

        public FluentHttpResponse Response { get; set; }

        public Stream Stream { get; set; }

        public HttpWebRequest HttpWebRequest { get; set; }
        public HttpWebResponse HttpWebResponse { get; set; }

        public FluentHttpAsyncResultOld AsyncResult { get; set; }

        public Exception Exception { get; set; }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (Stream == null) return;
            Stream.Dispose();
            Stream = null;
        }

        #endregion

        public Stream RequestStreamBody { get; set; }
    }
}