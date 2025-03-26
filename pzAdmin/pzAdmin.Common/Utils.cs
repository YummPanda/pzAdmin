using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pzAdmin.Common
{
      public static class Utils
      {
            // 允许的字符集合（排除易混淆字符）
            private static readonly char[] ValidChars = {
        '2', '3', '4', '5', '6', '7', '8', '9',
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J',
        'K', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U',
        'V', 'W', 'X', 'Y', 'Z'
    };
            /// <summary>
            /// 生成验证码图片和对应验证码
            /// </summary>
            /// <param name="length">验证码长度</param>
            /// <returns>包含验证码和图片字节数组的元组</returns>

            public static (string Code, byte[] ImageBytes) CreateYzmImage(int length)
            {
                  // 参数校验
                  if (length <= 0) throw new ArgumentException("验证码长度必须大于0");

                  // 生成随机验证码
                  var code = GenerateRandomCode(length);

                  // 创建位图对象（宽度根据字符数量自动调整，每个字符约占用25像素）
                  using var bitmap = new Bitmap(length * 25, 40);
                  using var graphics = Graphics.FromImage(bitmap);

                  // 设置高质量绘图模式
                  graphics.SmoothingMode = SmoothingMode.AntiAlias;
                  graphics.CompositingQuality = CompositingQuality.HighQuality;

                  // 绘制背景（浅灰色背景）
                  graphics.Clear(Color.FromArgb(240, 240, 240));

                  // 添加随机噪点（20条随机直线）
                  AddNoise(graphics, bitmap.Width, bitmap.Height, 20);

                  // 绘制验证码文字（带随机旋转和颜色）
                  DrawText(graphics, code, bitmap.Width, bitmap.Height);

                  // 转换为PNG格式字节数组
                  using var stream = new MemoryStream();
                  bitmap.Save(stream, ImageFormat.Png);
                  return (code, stream.ToArray());
            }
            /// <summary>
            /// 生成随机验证码字符串
            /// </summary>
            private static string GenerateRandomCode(int length)
            {
                  var sb = new StringBuilder();
                  var random = new Random();

                  for (int i = 0; i < length; i++)
                  {
                        sb.Append(ValidChars[random.Next(ValidChars.Length)]);
                  }
                  return sb.ToString();
            }

            /// <summary>
            /// 添加随机噪点（干扰线）
            /// </summary>
            private static void AddNoise(Graphics graphics, int width, int height, int lineCount)
            {
                  var random = new Random();
                  for (int i = 0; i < lineCount; i++)
                  {
                        // 随机颜色（深色系）
                        using var pen = new Pen(Color.FromArgb(
                            random.Next(50, 150),
                            random.Next(50, 150),
                            random.Next(50, 150)), 1);

                        // 随机起点终点
                        graphics.DrawLine(pen,
                            new Point(random.Next(width), random.Next(height)),
                            new Point(random.Next(width), random.Next(height)));
                  }
            }
            /// <summary>
            /// 绘制验证码文字（带随机效果）
            /// </summary>
            private static void DrawText(Graphics graphics, string code, int width, int height)
            {
                  var random = new Random();
                  var charWidth = width / code.Length;

                  for (int i = 0; i < code.Length; i++)
                  {
                        // 随机字体大小（20-25pt）
                        var fontSize = random.Next(20, 25);
                        using var font = new Font("Arial", fontSize, FontStyle.Bold);

                        // 随机颜色（深色系）
                        using var brush = new SolidBrush(Color.FromArgb(
                            random.Next(50, 150),
                            random.Next(50, 150),
                            random.Next(50, 150)));

                        // 字符位置基础计算
                        var x = i * charWidth + random.Next(-5, 5);
                        var y = random.Next(5, 10);

                        // 保存原始绘图状态
                        var state = graphics.Save();

                        // 添加随机旋转（-30到30度之间）
                        graphics.TranslateTransform(x, y);
                        graphics.RotateTransform(random.Next(-30, 30));

                        // 绘制字符
                        graphics.DrawString(code[i].ToString(), font, brush, 0, 0);

                        // 恢复绘图状态
                        graphics.Restore(state);
                  }
            }
      }
}
