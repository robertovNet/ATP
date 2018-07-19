namespace ATP.Common.Entities.Requests
{
    public class ReturnRequestBase
    {
        public ReturnRequestBase(string command)
        {
            Command = command;
        }

        public string Command { get; private set; }
    }
}
