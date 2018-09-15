namespace Prime
{
    /**
     * Represent an exponential number
     */
    public class Power
    {
        private uint _mantissa;
        private uint _exponent;

        // Only getter, unmutable property
        public uint Mantissa { get => _mantissa; }
        public uint Exponent { get => _exponent; set => _exponent = value; }

        public Power(uint mantissa, uint exponent)
        {
            this._mantissa = mantissa;
            this.Exponent = exponent;
        }
    }
}
