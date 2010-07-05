
namespace ForthInterpreter.DataTypes
{
    public class CountedString
    {
        public CountedString(int counterAddress)
        {
            CounterAddress = counterAddress;
        }

        public int CounterAddress { get; private set; }
    }
}
