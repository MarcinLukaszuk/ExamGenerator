namespace ExamGenerator.DocumentManager.QRCodeGenerator
{
    using System.Collections.Generic;
    using System.Drawing;
    using ZXing;
    using ZXing.Common;

    public class QrCodeED
    {
        BarcodeWriter bcWriter;
        BarcodeReader bcReader;
        public QrCodeED() : this(BarcodeFormat.QR_CODE)
        {
        }

        public QrCodeED(BarcodeFormat format)
        {
            bcWriter = new BarcodeWriter()
            {
                Format = format,
                Options = new EncodingOptions()
                {
                    Height = 300,
                    Width = 300
                }
            };

            bcReader = new BarcodeReader()
            {
                Options = new DecodingOptions()
                {
                    TryHarder = true,
                    PossibleFormats = new List<BarcodeFormat> { format }
                }
            };
        }

        public Bitmap Encode(string text)
        {
            Bitmap bmp = bcWriter.Write(text);
            return bmp;
        }

        public string Decode(Bitmap btm)
        {
            Result result = bcReader.Decode(btm);

            if (result != null)
                return result?.ToString();
            return null;
        }
    }
}
