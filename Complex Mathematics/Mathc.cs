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

        public static ComplexValue RiemannZeta(int lower, int upper, ComplexValue s, float scaler = 1)
        {
            lower = GreaterThan0(lower);
            ComplexValue sum = ComplexValue.Origin;
            for (int n = lower; n <= upper; n++) sum = sum + (1 / Power(n, s));
            return sum * scaler;
        }

        public static ComplexValue[] RiemannZetaValues(int lower, int upper, ComplexValue s, float scaler = 1)
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

    [System.Serializable]
    public struct ComplexValue
    {
        public float real;
        public float imaginary;

        public ComplexValue(float real, float imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }

        //Operator Overloading
        public override int GetHashCode() => this.ToString().GetHashCode();
        public override bool Equals(object obj) => base.Equals(obj);
        public override string ToString() => $"{real} + {imaginary}i";

        public static explicit operator ComplexValue(float val) => new ComplexValue(val, 0);
        public static explicit operator ComplexValue(string str) => ParseFromString(str);

        public static implicit operator string(ComplexValue val) => val.ToString();

        public static bool operator ==(ComplexValue a, ComplexValue b) => (a.real == b.real && a.imaginary == b.imaginary);

        public static bool operator !=(ComplexValue a, ComplexValue b) => (a.real != b.real || a.imaginary != b.imaginary);

        public static ComplexValue operator +(ComplexValue a, ComplexValue b) => new ComplexValue(a.real + b.real, a.imaginary + b.imaginary);

        public static ComplexValue operator -(ComplexValue a, ComplexValue b) => new ComplexValue(a.real - b.real, a.imaginary - b.imaginary);

        public static ComplexValue operator *(ComplexValue a, ComplexValue b)
        {
            float r1 = a.real * b.real;
            float r2 = -(a.imaginary * b.imaginary);
            float i1 = a.real * a.imaginary;
            float i2 = a.imaginary * b.real;
            return new ComplexValue(r1 + r2, i1 + i2);
        }

        public static ComplexValue operator *(ComplexValue a, float b) => new ComplexValue(a.real * b, a.imaginary * b);
        public static ComplexValue operator /(ComplexValue a, float b) => new ComplexValue(a.real / b, a.imaginary / b);
        public static ComplexValue operator /(float a, ComplexValue b) => (b.Conjugate * a) / (b.Conjugate * b).real;

        //NOTE: [^] is not a logical operator in this context, its raising a complex number to a integer power.
        public static ComplexValue operator ^(ComplexValue a, int b)
        {
            ComplexValue val = a;
            if (b == 0) return RealUnitVector;
            int power = (b < 0) ? b * -1 : b;
            for (int i = 0; i < power - 1; i++) val = val * a;
            if (b < 0) val = 1 / val;
            return val;
        }

        public ComplexValue Conjugate
        {
            get { return new ComplexValue(real, -(imaginary)); }
        }

        //Pre-Defined Values
        public static ComplexValue NaN
        {
            get { return new ComplexValue(float.NaN, float.NaN); }
        }

        public static ComplexValue ImaginaryUnitVector
        {
            get { return new ComplexValue(0, 1); }
        }

        public static ComplexValue RealUnitVector
        {
            get { return new ComplexValue(1, 0); }
        }

        public static ComplexValue ComplexUnitVector
        {
            get { return new ComplexValue(1, 1); }
        }

        public static ComplexValue Origin
        {
            get { return new ComplexValue(0, 0); }
        }

        public static ComplexValue ParseFromString(string str, char separator = '+')
        {
            str.Replace(" ", string.Empty);
            string r = "";
            string im = "";
            bool isImaginary = false;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != separator)
                {
                    if (isImaginary == false) { r += str[i]; }
                    else { im += str[i]; }
                }
                else { isImaginary = true; }
            }

            ComplexValue complexValue = NaN;
            float.TryParse(r, out complexValue.real);
            float.TryParse(im, out complexValue.imaginary);
            return complexValue;
        }

    }
}