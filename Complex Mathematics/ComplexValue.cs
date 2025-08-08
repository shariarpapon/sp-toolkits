namespace SPToolkits.ComplexMathematics
{

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