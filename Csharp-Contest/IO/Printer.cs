// Printer.cs
// Author: Araf Al Jami
// Last Updated: 12-02-2023 17:45

namespace CLown1331.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public sealed class Printer : StreamWriter
    {
        public Printer(Stream stream)
            : this(stream, new UTF8Encoding(false, true))
        {
        }

        public Printer(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
#if CLown1331
            this.AutoFlush = true;
#else
                this.AutoFlush = false;
#endif
        }

        public override IFormatProvider FormatProvider
        {
            get { return CultureInfo.InvariantCulture; }
        }
    }
}
