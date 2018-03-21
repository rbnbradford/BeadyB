using System;

namespace BeadyB
{
    public class StepNotDefinedException : Exception
    {
        public StepNotDefinedException(string stepName) : base($"\"{stepName}\"")
        {
        }
    }
}
