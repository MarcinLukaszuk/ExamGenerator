using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Aztec.Internal;
using ZXing.Common;
using ZXing.Multi.QrCode.Internal;
using ZXing.QrCode;

namespace ExamGenerator.DocumentManager.QRCodeGenerator
{
    public static class QrCodeEncoderDecoder
    {
        static readonly int height = 300;
        static readonly int width = 300;
        static readonly BarcodeFormat format = BarcodeFormat.QR_CODE;

        public static Bitmap Encode(string text)
        {
            return new BarcodeWriter()
            {
                Format = format,
                Options = new EncodingOptions()
                {
                    Height = height,
                    Width = width
                }
            }.Write(text);
        }

        public static string Decode(Bitmap btm)
        {
            return new BarcodeReader()
            {
                Options = new DecodingOptions()
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat> {
                        format
                    }
                }
            }.Decode(btm)?.ToString();
        }
    }
}
