using King.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Push : RecurringTask
    {
        public override void Run()
        {
            var m = new SampleModel
            {
                Id = Guid.NewGuid(),
                Activated = false,
                Time = DateTime.UtcNow,
            };

            //Send data to IoT Hub
        }
    }
}
