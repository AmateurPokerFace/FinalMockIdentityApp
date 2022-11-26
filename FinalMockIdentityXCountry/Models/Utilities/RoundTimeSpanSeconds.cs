namespace FinalMockIdentityXCountry.Models.Utilities
{
    public class RoundTimeSpanSeconds
    {
        public static TimeSpan RoundSeconds(TimeSpan span)
        {
            return TimeSpan.FromSeconds(Math.Round(span.TotalSeconds));
        }
    }
}
