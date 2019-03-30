using Data;
using Data.Repository;

namespace Commands.Startup
{
    public class InitDataCommand : Command
    {
        private Repository _repository;

        public InitDataCommand(Repository repository)
        {
            _repository = repository;
        }
        
        public override void Execute()
        {
            _repository.OnInitComplete += OnInitComplete;
            _repository.Init();
        }

        private void OnInitComplete()
        {
            Complete();
        }
    }
}