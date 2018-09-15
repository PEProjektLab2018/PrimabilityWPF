namespace Prime
{
    /**
     * Represent an exponential number
     */
    public class Power
    {
        private ulong _mantissa;
        private ulong _exponent;

        // Only getter, unmutable property
        public ulong Mantissa { get => _mantissa; }
        public ulong Exponent { get => _exponent; set => _exponent = value; }

        public Power(ulong mantissa, ulong exponent)
        {
            this._mantissa = mantissa;
            this._exponent = exponent;
        }
    }
}
