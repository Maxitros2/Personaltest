using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaTest
{
    public static class Questions
    {
        public static List<Question> _alllist = new List<Question>();
    }
    public class FailedQuestion:Question
    {
        public List<Answer> FailedAnswers = new List<Answer>();
        public List<Answer> CorrectAnswers = new List<Answer>();
    }
    public class Question
    {
        public List<Answer> answers = new List<Answer>();
        public string text;
        public Question() { }
        public Question(string text)
        {
            this.text = text;
           
        }
        public void AddAnswer(Answer answer)
        {
            this.answers.Add(answer);
        }
        public Question(string text, List<Answer> answers)
        {
            this.text = text;
            this.answers = answers;
        }
    }
    public class Answer
    {
        public string text;
        public bool right;
        public Answer() { }
        public Answer(string text,bool right)
        {
            this.text = text;
            this.right = right;
        }
    }
}
