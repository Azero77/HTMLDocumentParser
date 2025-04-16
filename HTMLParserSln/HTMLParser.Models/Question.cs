namespace HTMLParser.Models
{
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public IEnumerable<string> QuestionChoices { get; set; } = null!;
        public string QuestionAnswer { get; set; } = string.Empty;


        public override string ToString()
        {
            var choices = string.Join(", ", QuestionChoices ?? Enumerable.Empty<string>());
            return $"Question: {QuestionText}\nChoices: {choices}\nAnswer: {QuestionAnswer}";
        }
    }

    public class RawQuestion
    {
        public object? QuestionText { get; set; }
        public IEnumerable<object?> QuestionChoices { get; set; } = null!;
        public object? QuestionAnswer { get; set; }
    }
}
