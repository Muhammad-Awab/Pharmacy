using POSPrintExample;
using PrinterUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy
{
    class POS
    {
      public  void print(string cn , string disc, string discp, string g_total , string billId ,string total , string date,DataTable dataTable )
        {
            PrinterUtility.EscPosEpsonCommands.EscPosEpson obj = new PrinterUtility.EscPosEpsonCommands.EscPosEpson();
            var BytesValue = Encoding.ASCII.GetBytes(string.Empty);
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.Nomarl());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.FontSelect.FontA());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("ABDULLAH MEDICAL COMPLEX\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Main Tarnol Chowk Islamabad\n Cell:0331-5301030 \n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.Nomarl());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Invoice\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Left());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Invoice No. : "+billId+"\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Customer : "+cn+"\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Date        : "+date+"\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Itm                      Unit Price      Qty   Sub Total\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());

            
            foreach (DataRow row in dataTable.Rows)
            {
                StringBuilder sb = new StringBuilder();
                object[] itemArray = row.ItemArray;
                String line = String.Format("{0,-40}{1,6}{2,9}{3,9:N2}", itemArray[0].ToString(), itemArray[1].ToString(), itemArray[2].ToString(), itemArray[3].ToString());
                sb.AppendLine(line);
               BytesValue = PrintExtensions.AddBytes(BytesValue,sb.ToString());
                sb.Clear();
            }

            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Right());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Total : " + total + "\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Discount %age : "+discp+"\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Discount (Rs) : "+disc+"\n"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, Encoding.ASCII.GetBytes("Grand Total : "+g_total+"\n" ));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Lf());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.Nomarl());
            BytesValue = PrintExtensions.AddBytes(BytesValue, "FRIDGE ITEMS ARE NOT REFUNDABLE \n All ITEMS ARE RETURNABLE WITHIN 3 DAYS\n MUST BRING RECEIPT WITH YOU FOR RETURN \n Home Delivery Seervice is also available\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "-------------------Thank you for coming------------------------\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, "Sofware by : www.cyberoide.tech\n +92-308-3162302\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Left());
            BytesValue = PrintExtensions.AddBytes(BytesValue, CutPage());
         // PrinterUtility.PrintExtensions.Print(BytesValue, POSPrintExample.Properties.Settings.Default.PrinterPath);
         //  if (File.Exists(".\\tmpPrint.print"))
         //     File.Delete(".\\tmpPrint.print");
            File.WriteAllBytes(".\\tmpPrint"+billId+".print", BytesValue);
            RawPrinterHelper.SendFileToPrinter("BC-95AC", ".\\tmpPrint" + billId + ".print");
            try
            {
                File.Delete(".\\tmpPrint" + billId + ".print");
            }
            catch
            {

            }
        }
public byte[] CutPage()
{
    List<byte> oby = new List<byte>();
    oby.Add(Convert.ToByte(Convert.ToChar(0x1D)));
    oby.Add(Convert.ToByte('V'));
    oby.Add((byte)66);
    oby.Add((byte)3);
    return oby.ToArray();
}
public byte[] GetLogo(string LogoPath)
{
    List<byte> byteList = new List<byte>();
    if (!File.Exists(LogoPath))
        return null;
    BitmapData data = GetBitmapData(LogoPath);
    BitArray dots = data.Dots;
    byte[] width = BitConverter.GetBytes(data.Width);

    int offset = 0;
    MemoryStream stream = new MemoryStream();
    // BinaryWriter bw = new BinaryWriter(stream);
    byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
    //bw.Write((char));
    byteList.Add(Convert.ToByte('@'));
    //bw.Write('@');
    byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
    // bw.Write((char)0x1B);
    byteList.Add(Convert.ToByte('3'));
    //bw.Write('3');
    //bw.Write((byte)24);
    byteList.Add((byte)24);
    while (offset < data.Height)
    {
        byteList.Add(Convert.ToByte(Convert.ToChar(0x1B)));
        byteList.Add(Convert.ToByte('*'));
        //bw.Write((char)0x1B);
        //bw.Write('*');         // bit-image mode
        byteList.Add((byte)33);
        //bw.Write((byte)33);    // 24-dot double-density
        byteList.Add(width[0]);
        byteList.Add(width[1]);
        //bw.Write(width[0]);  // width low byte
        //bw.Write(width[1]);  // width high byte

        for (int x = 0; x < data.Width; ++x)
        {
            for (int k = 0; k < 3; ++k)
            {
                byte slice = 0;
                for (int b = 0; b < 8; ++b)
                {
                    int y = (((offset / 8) + k) * 8) + b;
                    // Calculate the location of the pixel we want in the bit array.
                    // It'll be at (y * width) + x.
                    int i = (y * data.Width) + x;

                    // If the image is shorter than 24 dots, pad with zero.
                    bool v = false;
                    if (i < dots.Length)
                    {
                        v = dots[i];
                    }
                    slice |= (byte)((v ? 1 : 0) << (7 - b));
                }
                byteList.Add(slice);
                //bw.Write(slice);
            }
        }
        offset += 24;
        byteList.Add(Convert.ToByte(0x0A));
        //bw.Write((char));
    }
    // Restore the line spacing to the default of 30 dots.
    byteList.Add(Convert.ToByte(0x1B));
    byteList.Add(Convert.ToByte('3'));
    //bw.Write('3');
    byteList.Add((byte)30);
    return byteList.ToArray();
    //bw.Flush();
    //byte[] bytes = stream.ToArray();
    //return logo + Encoding.Default.GetString(bytes);
}

public BitmapData GetBitmapData(string bmpFileName)
{
    using (var bitmap = (Bitmap)Bitmap.FromFile(bmpFileName))
    {
        var threshold = 127;
        var index = 0;
        double multiplier = 570; // this depends on your printer model. for Beiyang you should use 1000
        double scale = (double)(multiplier / (double)bitmap.Width);
        int xheight = (int)(bitmap.Height * scale);
        int xwidth = (int)(bitmap.Width * scale);
        var dimensions = xwidth * xheight;
        var dots = new BitArray(dimensions);

        for (var y = 0; y < xheight; y++)
        {
            for (var x = 0; x < xwidth; x++)
            {
                var _x = (int)(x / scale);
                var _y = (int)(y / scale);
                var color = bitmap.GetPixel(_x, _y);
                var luminance = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);
                dots[index] = (luminance < threshold);
                index++;
            }
        }

        return new BitmapData()
        {
            Dots = dots,
            Height = (int)(bitmap.Height * scale),
            Width = (int)(bitmap.Width * scale)
        };
    }
}

public class BitmapData
{
    public BitArray Dots
    {
        get;
        set;
    }

    public int Height
    {
        get;
        set;
    }

    public int Width
    {
        get;
        set;
    }
}
    }
}
