using Data;

namespace Commands.Startup
{
    public class InitDataCommand : Command
    {
        public override void Execute()
        {
            App.Instance.Repository.OnInitComplete += OnInitComplete;
            App.Instance.Repository.Init();
        }

        private void OnInitComplete()
        {
            Complete();
        }
    }
}