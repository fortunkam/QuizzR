using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizExperiment.Models
{
    public class TypeTheAnswer : Answer
    {
        public IEnumerable<string> Answers { get; set; }
    }
}
