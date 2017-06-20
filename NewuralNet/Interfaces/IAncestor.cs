namespace NewuralNet.Interfaces
{
    public interface IAncestor
    {
        ISimpleCell Cell { get; set; }

        float Weight { get; set; }

        void IncreaseWeight();

        void DecreaseWeight();
    }
}