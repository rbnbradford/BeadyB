using System;
using System.Collections.Generic;
using System.Linq;
using BeadyB;
using Xunit.Abstractions;

namespace TestProject1
{
    public abstract class MySharedContextBase<T> : Context<T> where T : Context<T>
    {
        protected int _x;
        protected int _y;

        protected MySharedContextBase(ITestOutputHelper output) : base(output)
        {
        }

        [Step("the number *")]
        public void Method1(IEnumerable<string> args)
        {
            _x = Convert.ToInt32(args.ElementAt(0));
        }

        [Step("also the number *")]
        public void Method2(IEnumerable<string> args)
        {
            _y = Convert.ToInt32(args.ElementAt(0));
        }
    }
}
