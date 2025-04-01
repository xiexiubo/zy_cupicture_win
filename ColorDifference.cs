using System;

namespace zy_cutPicture
{ 
    public class ColorDifference
    {
        // RGB 转 XYZ 颜色空间
        private static double[] RGBToXYZ(double[] rgb)
        {
            double r = rgb[0] / 255.0;
            double g = rgb[1] / 255.0;
            double b = rgb[2] / 255.0;

            r = r > 0.04045 ? Math.Pow((r + 0.055) / 1.055, 2.4) : r / 12.92;
            g = g > 0.04045 ? Math.Pow((g + 0.055) / 1.055, 2.4) : g / 12.92;
            b = b > 0.04045 ? Math.Pow((b + 0.055) / 1.055, 2.4) : b / 12.92;

            double X = r * 0.4124 + g * 0.3576 + b * 0.1805;
            double Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
            double Z = r * 0.0193 + g * 0.1192 + b * 0.9505;

            return new double[] { X, Y, Z };
        }

        // XYZ 转 CIELAB 颜色空间
        private static double[] XYZToLab(double[] xyz)
        {
            double Xn = 0.95047;
            double Yn = 1.00000;
            double Zn = 1.08883;

            double x = xyz[0] / Xn;
            double y = xyz[1] / Yn;
            double z = xyz[2] / Zn;

            x = x > 0.008856 ? Math.Pow(x, 1.0 / 3.0) : 7.787 * x + 16.0 / 116.0;
            y = y > 0.008856 ? Math.Pow(y, 1.0 / 3.0) : 7.787 * y + 16.0 / 116.0;
            z = z > 0.008856 ? Math.Pow(z, 1.0 / 3.0) : 7.787 * z + 16.0 / 116.0;

            double L = 116 * y - 16;
            double a = 500 * (x - y);
            double b = 200 * (y - z);

            return new double[] { L, a, b };
        }

        // RGB 转 CIELAB 颜色空间
        private static double[] RGBToLab(double[] rgb)
        {
            double[] xyz = RGBToXYZ(rgb);
            return XYZToLab(xyz);
        }

        // CIE76 色差公式
        public static double CIE76(double[] rgb1, double[] rgb2)
        {
            double[] lab1 = RGBToLab(rgb1);
            double[] lab2 = RGBToLab(rgb2);

            double deltaL = lab1[0] - lab2[0];
            double deltaA = lab1[1] - lab2[1];
            double deltaB = lab1[2] - lab2[2];

            return Math.Sqrt(deltaL * deltaL + deltaA * deltaA + deltaB * deltaB);
        }

        // CIEDE2000 色差公式
        public static double CIEDE2000(double[] rgb1, double[] rgb2)
        {
            double[] lab1 = RGBToLab(rgb1);
            double[] lab2 = RGBToLab(rgb2);

            double L1 = lab1[0];
            double a1 = lab1[1];
            double b1 = lab1[2];
            double L2 = lab2[0];
            double a2 = lab2[1];
            double b2 = lab2[2];

            double C1 = Math.Sqrt(a1 * a1 + b1 * b1);
            double C2 = Math.Sqrt(a2 * a2 + b2 * b2);
            double CabBar = (C1 + C2) / 2;

            double G = 0.5 * (1 - Math.Sqrt(Math.Pow(CabBar, 7) / (Math.Pow(CabBar, 7) + Math.Pow(25, 7))));

            double a1Prime = (1 + G) * a1;
            double a2Prime = (1 + G) * a2;

            double C1Prime = Math.Sqrt(a1Prime * a1Prime + b1 * b1);
            double C2Prime = Math.Sqrt(a2Prime * a2Prime + b2 * b2);

            double h1Prime = Math.Atan2(b1, a1Prime);
            if (h1Prime < 0) h1Prime += 2 * Math.PI;
            double h2Prime = Math.Atan2(b2, a2Prime);
            if (h2Prime < 0) h2Prime += 2 * Math.PI;

            double deltaLPrime = L2 - L1;
            double deltaCPrime = C2Prime - C1Prime;

            double deltahPrime;
            if (C1Prime * C2Prime == 0)
            {
                deltahPrime = 0;
            }
            else if (Math.Abs(h2Prime - h1Prime) <= Math.PI)
            {
                deltahPrime = h2Prime - h1Prime;
            }
            else if (h2Prime - h1Prime > Math.PI)
            {
                deltahPrime = h2Prime - h1Prime - 2 * Math.PI;
            }
            else
            {
                deltahPrime = h2Prime - h1Prime + 2 * Math.PI;
            }

            double deltaHPrime = 2 * Math.Sqrt(C1Prime * C2Prime) * Math.Sin(deltahPrime / 2);

            double LPrimeBar = (L1 + L2) / 2;
            double CPrimeBar = (C1Prime + C2Prime) / 2;

            double hPrimeBar;
            if (C1Prime * C2Prime == 0)
            {
                hPrimeBar = h1Prime + h2Prime;
            }
            else if (Math.Abs(h1Prime - h2Prime) <= Math.PI)
            {
                hPrimeBar = (h1Prime + h2Prime) / 2;
            }
            else if (h1Prime + h2Prime < 2 * Math.PI)
            {
                hPrimeBar = (h1Prime + h2Prime + 2 * Math.PI) / 2;
            }
            else
            {
                hPrimeBar = (h1Prime + h2Prime - 2 * Math.PI) / 2;
            }

            double T = 1 - 0.17 * Math.Cos(hPrimeBar - Math.PI / 6) + 0.24 * Math.Cos(2 * hPrimeBar) + 0.32 * Math.Cos(3 * hPrimeBar + Math.PI / 30) - 0.20 * Math.Cos(4 * hPrimeBar - 63 * Math.PI / 180);

            double deltaTheta = 30 * Math.Exp(-Math.Pow((hPrimeBar * 180 / Math.PI - 275) / 25, 2));

            double RC = 2 * Math.Sqrt(Math.Pow(CPrimeBar, 7) / (Math.Pow(CPrimeBar, 7) + Math.Pow(25, 7)));

            double SL = 1 + (0.015 * Math.Pow(LPrimeBar - 50, 2)) / Math.Sqrt(20 + Math.Pow(LPrimeBar - 50, 2));
            double SC = 1 + 0.045 * CPrimeBar;
            double SH = 1 + 0.015 * CPrimeBar * T;

            double RT = -Math.Sin(2 * deltaTheta * Math.PI / 180) * RC;

            return Math.Sqrt(
                Math.Pow(deltaLPrime / SL, 2) +
                Math.Pow(deltaCPrime / SC, 2) +
                Math.Pow(deltaHPrime / SH, 2) +
                RT * (deltaCPrime / SC) * (deltaHPrime / SH)
            );
        }
    }
}
