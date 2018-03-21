using System;

namespace BeadyB
{
    [AttributeUsage(AttributeTargets.Method)]
    public class StepAttribute : Attribute
    {
        public string StepTemplate { get; }
        public StepAttribute(string stepTemplate) => StepTemplate = stepTemplate;
    }
}
