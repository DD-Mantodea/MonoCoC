using System;
using System.Buffers;
using System.IO;

namespace MonoCoC.ModLoader
{
    internal sealed class CocModSubStream : Stream
    {
        private readonly CocModPackage _package;
        private readonly CocModEntry _entry;
        private long _position;
        private bool _disposed;

        public CocModSubStream(CocModPackage package, CocModEntry entry)
        {
            _package = package;
            _entry = entry;
        }

        public override bool CanRead => !_disposed;

        public override bool CanSeek => !_disposed;

        public override bool CanWrite => false;

        public override long Length => _entry.Length;

        public override long Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _entry.Length)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CocModSubStream));

            var remaining = _entry.Length - _position;

            if (remaining <= 0)
                return 0;

            var read = (int)Math.Min(count, remaining);

            var actual = _package.ReadAt(_entry.Offset + _position, buffer, offset, read);

            _position += actual;

            return actual;
        }

        public override int Read(Span<byte> buffer)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(CocModSubStream));

            var remaining = _entry.Length - _position;

            if (remaining <= 0)
                return 0;

            var count = (int)Math.Min(buffer.Length, remaining);

            var bufferArray = ArrayPool<byte>.Shared.Rent(count);

            try
            {
                var actual = _package.ReadAt(_entry.Offset + _position, bufferArray, 0, count);

                bufferArray.AsSpan(0, actual).CopyTo(buffer);

                _position += actual;

                return actual;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(bufferArray);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var target = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => _position + offset,
                SeekOrigin.End => _entry.Length + offset,
                _ => throw new ArgumentOutOfRangeException(nameof(origin))
            };

            if (target < 0 || target > _entry.Length)
                throw new IOException("Seek outside package entry");

            _position = target;

            return _position;
        }

        public override void Flush()
        {
        }

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
