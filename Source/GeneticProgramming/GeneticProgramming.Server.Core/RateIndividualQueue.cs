using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;
using MoreLinq;

namespace GeneticProgramming.Server.Core
{
    public class RateIndividualQueue
    {
        private Queue<RateIndividualTask> _queue = new Queue<RateIndividualTask>();
        private readonly Dictionary<int, RateIndividualTask> _taskByIds = new Dictionary<int, RateIndividualTask>();

        public void Enqueue(RateIndividualTask newTask)
        {
            if (!_taskByIds.ContainsKey(newTask.Id))
            {
                _taskByIds.Add(newTask.Id, newTask);
            }
            _queue.Enqueue(newTask);
        }

        public void Cleanup()
        {
            _queue.ForEach(DisableAlreadyComputed);
            _queue = new Queue<RateIndividualTask>(_queue.Where(IsTaskStillRelevant));
        }

        public bool IsTaskStillRelevant(RateIndividualTask task)
        {
            return task.Evaluate || task.Validate;
        }

        private void DisableAlreadyComputed(RateIndividualTask task)
        {
            Individual ind = task.IndividualToRate;
            if (ind.MultiObjectiveFitness != null)
            {
                task.Evaluate = false;
            }
            if (ind.MultiObjectiveValidation != null)
            {
                task.Validate = false;
            }
        }

        public RateIndividualTask Dequeue()
        {
            return _queue.Dequeue();
        }

        public RateIndividualTask GetById(int id)
        {
            return _taskByIds[id];
        }

        public bool Any()
        {
            while (_queue.Any())
            {
                var task = _queue.Peek();
                DisableAlreadyComputed(task);
                if (IsTaskStillRelevant(task))
                {
                    return true;
                }
                _queue.Dequeue();
            }
            return false;
        }
    }
}
