namespace FinalMockIdentityXCountry.Models.Utilities
{
    public class StaticPaceCalculator
    {
        
        public static (int,int) CalculatePace(int hours = 0, int minutes = 0, int seconds = 0, double distance=0) 
        {
            int secondsInAnHour = 3600;
            int secondsInAnMinute = 60;

            int time = (hours * secondsInAnHour) + (minutes * secondsInAnMinute) + seconds;

            if (distance != 0)
            {
                int pace = Convert.ToInt32(time / distance); 

                return (pace/60,pace % 60);
            }
             

            return (0,0);
        }
    }
}
