using System.Text;

namespace ForthInterpreter.IO
{
    public class ConsoleStringBuilder
    {
        public ConsoleStringBuilder()
            : this(80)
        {
        }

        public ConsoleStringBuilder(int maxColumns)
        {
            MaxColumns = maxColumns;
            LastFilledColumn = 0;
        }

        public int MaxColumns { get; private set; }
        public int LastFilledColumn { get; private set; }

        public void Append(string value)
        {
            if (LastFilledColumn + value.Length > MaxColumns)
            {
                if (LastFilledColumn < 80)
                    stringBuilder.AppendLine();
                LastFilledColumn = 0;
            }

            stringBuilder.Append(value);
            LastFilledColumn += value.Length;
        }

        public void AppendFormat(string format, params object[] args)
        {
            Append(string.Format(format, args));
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        private StringBuilder stringBuilder = new StringBuilder();
    }
}
