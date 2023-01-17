﻿using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Extensions
{
    public static class StreamExtensions
    {
        public static SKBitmap ToBitmap(this Stream stream)
        {
            return SKBitmap.Decode(stream);
        }
    }
}