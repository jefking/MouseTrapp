namespace DataGenerator
{
    using King.Service;
    using System.Collections.Generic;

    public class Factory : ITaskFactory<object>
    {
        public IEnumerable<IRunnable> Tasks(object passthrough)
        {
            yield return new Push();
        }
    }
}