
namespace QuizExperiment.Models
{
    public static class PositionSuffixExtension
    {
        public static string GetSuffix(this int position)
        {
            return position switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }
    }
}
