﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinalSpeed.utils
{
    public class ByteIntConvert
    {
        public static int toInt(byte[] b, int offset)
        {
            return b[offset + 3] & 0xff | (b[offset + 2] & 0xff) << 8
            | (b[offset + 1] & 0xff) << 16 | (b[offset] & 0xff) << 24;
        }

        public static void toByteArray(int n, byte[] buf, int offset)
        {
            buf[offset] = (byte)(n >> 24);
            buf[offset + 1] = (byte)(n >> 16);
            buf[offset + 2] = (byte)(n >> 8);
            buf[offset + 3] = (byte)n;
        }
    }
}
