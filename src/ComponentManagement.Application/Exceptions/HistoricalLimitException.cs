namespace ComponentManagement.Application.Exceptions
{
    public class HistoricalLimitException : Exception
    {
        public HistoricalLimitException()
            : base("Historical limit reached. Maximum allowed is 3 records.")
        {
        }
    }
}
