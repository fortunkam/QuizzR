
namespace QuizExperiment.Models
{
    public static class ButtonNamesExtension
    {
        public static string GetButtonNameFromIndex(this int index)
        {
            return index switch
            {
                0 => "Y",
                1 => "X",
                2 => "B",
                3 => "A",
                _ => "_"
            };
        }
    }
}
