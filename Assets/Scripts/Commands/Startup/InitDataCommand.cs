using Data;
using Data.Repository;

namespace Commands.Startup
{
    public class InitDataCommand : Command
    {
        private BaseRepository _baseRepository;

        public InitDataCommand(BaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }
        
        public override void Execute()
        {
            _baseRepository.OnInitComplete += OnInitComplete;
            _baseRepository.Init();
        }

        private void OnInitComplete()
        {
            Complete();
        }
    }
}