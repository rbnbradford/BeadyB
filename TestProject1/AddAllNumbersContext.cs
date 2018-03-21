using System;
using System.Collections.Generic;
using System.Linq;
using BeadyB;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class AddAllNumbersContext : MySharedContextBase<AddAllNumbersContext>
    {
        private int _result;

        public AddAllNumbersContext(ITestOutputHelper output) : base(output)
        {
        }

        [Step("I add those numbers")]
        public void Method3(IEnumerable<string> args)
        {
            _result = _x + _y;
        }

        [Step("I get *")]
        public void Method4(IEnumerable<string> args)
        {
            Assert.Equal(Convert.ToInt32(args.ElementAt(0)), _result);
        }
    }
}
