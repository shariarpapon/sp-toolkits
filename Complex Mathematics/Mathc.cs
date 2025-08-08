using System;
using System.Collections.Generic;

namespace SPToolkits.ComplexMathematics
{
    public readonly struct Mathc
    {
        public const float e = 2.7182818f;
        public const float pi = 3.1415926f;

        public static ComplexValue Power(float baseValue, ComplexValue pow, float scaler = 1)
        {
            //using eulers formula : e^ix = cos(x) + isin(x)
            float coeff = (float)(scaler * Math.Pow(baseValue, pow.real));
            float innerConst = (float)(Math.Log(baseValue) * pow.imaginary);
            float rp = (float)(coeff * Math.Cos(innerConst));
            float ip = (float)(coeff * Math.Sin(innerConst));
            return new ComplexValue(rp, ip);
        }

        public static ComplexValue EulerPower(ComplexValue pow, float scaler = 1)
        {
            return Power(e, pow, scaler);
        }

        public static ComplexValue EulerPowerID(float angleRad, float scaler = 1)
        {
            return EulerPower(ComplexValue.ImaginaryUnitVector * angleRad, scaler);
        }

        public static float RiemannZeta(int lower, int upper, float s, float scaler = 1)
        {
            lower = GreaterThan0(lower);
            float sum = 0;
            for (int n = lower; n <= upper; n++) sum += 1 / (float)Math.Pow(n, s);
            return sum * scaler;
        }

        public static ComplexValue RiemannZetaSum(int lower, int upper, ComplexValue s, float scaler = 1)
        {
            lower = GreaterThan0(lower);
            ComplexValue sum = ComplexValue.Origin;
            for (int n = lower; n <= upper; n++) sum = sum + (1 / Power(n, s));
            return sum * scaler;
        }

        public static ComplexValue[] RiemannZetaPartialSums(int lower, int upper, ComplexValue s, float scaler = 1)
        {
            lower = GreaterThan0(lower);
            List<ComplexValue> values = new List<ComplexValue>();
            ComplexValue sum = ComplexValue.Origin;
            for (int n = lower; n <= upper; n++)
            {
                sum = sum + (1 / Power(n, s));
                values.Add(sum * scaler);
            }
            return values.ToArray();
        }

        public static float Clamp(float value, float min, float max)
        {
            return (value > max) ? max : (value < min) ? min : value;
        }

        public static int GreaterThan0(int value)
        {
            return (value <= 0) ? 1 : value;
        }
    }
}