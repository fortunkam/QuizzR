namespace QuizExperiment.Models
{
    public class QuestionSet
    {
        public Guid Id {  get; set; }
        public string Description { get; set;  }

        public Guid userId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModified { get; set; }

        public IEnumerable<Question> Questions { get; set; }
    }
}