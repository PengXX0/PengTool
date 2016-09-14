using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNet.Utilities
{
    public class QrCodeHelpers
    {
        /// <summary>
        /// 生成二维码并写到二进制里面
        /// </summary>
        /// <param name="data">内容</param>
        /// <param name="imageFormat">图片格式</param>
        /// <param name="moduleSize">模块大小</param>
        ///  <param name="errorCorrectionLevel">二维码纠错级别</param>
        /// <param name="quietZoneModules">外层模块大小</param>
        /// <returns>返回二进制</returns>
        public static byte[] CreateQrCode(string data, ImageFormat imageFormat, int moduleSize, ErrorCorrectionLevel errorCorrectionLevel, QuietZoneModules quietZoneModules)
        {
            var qrEncoder = new QrEncoder(errorCorrectionLevel);
            QrCode qrCode; qrEncoder.TryEncode(data, out qrCode);
            var stream = new MemoryStream();
            var renderer = new GraphicsRenderer(new FixedModuleSize(moduleSize, quietZoneModules), Brushes.Black, Brushes.White);
            renderer.WriteToStream(qrCode.Matrix, imageFormat, stream); stream.Dispose();
            return stream.ToArray();
        }
    }
}
