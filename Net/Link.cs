namespace Net
{
    public class Link
    {
        private INeuron start;
        private INeuron end;
        private double? value = null;
        private double? derrivate = null;
        private double _cumulatedAdjustment = 0;

        public double Weight { get; private set; }

        public static double Step { get; set; } = 5;
        private static double _renewalFactor = 0.000003;
        public static double RenewalFactor
        {
            get
            {
                return _renewalFactor;
            }
            set
            {
                if (_renewalFactor != value)
                    _renewalFactor = value;
            }
        } 

        public Link(INeuron start, INeuron end)
        {
            this.start = start;
            this.end = end;
            Weight = RandomNumberProvider.Next();
        }

        public double Value()
        {
            if (!value.HasValue)
                value = start.Value * Weight;

            return value.Value;
        }

        public void AdjustWeight()
        {
            if (RandomNumberProvider.Next() < RenewalFactor)
            {
                Weight = RandomNumberProvider.Next();
            }
            {
                
                Weight += GetAdjustment();
            }
        }

        public void CumulateAdjustment()
        {
            _cumulatedAdjustment += GetAdjustment();
        }

        public void ApplyAdjustment()
        {
            if (RandomNumberProvider.Next() < RenewalFactor)
            {
                Weight = RandomNumberProvider.Next();
            }
            Weight += _cumulatedAdjustment;
            _cumulatedAdjustment = 0;
        }

        private double GetAdjustment()
        {
            return Step * end.Derrivate * start.Value;
        }

        public double Derrivate()
        {
            if (!derrivate.HasValue)
                derrivate = end.Derrivate * Weight;

            return derrivate.Value;
        }

        public void ResetValue()
        {
            value = null;
        }

        public void ResetDerrivate()
        {
            derrivate = null;
        }
    }
}