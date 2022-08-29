// MultiStream.cs
// Author: Araf Al Jami
// Last Updated: 29-08-2565 21:48

namespace CLown1331
{
    using System.Linq;

    public class MultiStream : System.IO.Stream
    {
        private readonly System.Collections.Generic.List<System.IO.Stream> _streams;

        public MultiStream(params System.IO.Stream[] streams) => this._streams = streams.ToList();

        public override void Flush()
        {
            foreach (System.IO.Stream stream in this._streams)
            {
                stream.Flush();
            }
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            long x = 0;
            foreach (System.IO.Stream stream in this._streams)
            {
                x = stream.Seek(offset, origin);
            }
            return x;
        }

        public override void SetLength(long value)
        {
            foreach (System.IO.Stream stream in this._streams)
            {
                stream.SetLength(value);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._streams.First(x => x.CanRead).Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            foreach (System.IO.Stream stream in this._streams)
            {
                stream.Write(buffer, offset, count);
            }
        }

        public override bool CanRead
        {
            get { return this._streams.Any(x => x.CanRead); }
        }

        public override bool CanSeek
        {
            get { return this._streams.TrueForAll(x => x.CanSeek); }
        }

        public override bool CanWrite
        {
            get { return this._streams.TrueForAll(x => x.CanWrite); }
        }

        public override long Length => this._streams.Min(x => x.Length);

        public override long Position
        {
            get => this._streams.First().Position;
            set { this._streams.ForEach(x => x.Position = value); }
        }
    }
}
