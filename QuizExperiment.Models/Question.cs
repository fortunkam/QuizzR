namespace QuizExperiment.Models
{
    public abstract class Question
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public string ImageUrl { get; set; }

        public IEnumerable<Answer> PossibleAnswers {  get; set; }
    }
}
