using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Hypothesis
    {
        public string Utterance { get; set; }
        public double Confidence { get; set; }
    }

    // Root response class to represent the overall response
    public class SpeechToTextResponse
    {
        public int Status { get; set; }
        public string Id { get; set; }
        public List<Hypothesis> Hypotheses { get; set; }
    }
}