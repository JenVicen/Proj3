using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using BlazorApp.Entities;
using BlazorApp.Services;

namespace BlazorApp.Services
{
    public class SimulationManager
    {
        private List<ISimulationEntity> entities = new List<ISimulationEntity>();
        private CancellationTokenSource cancellationTokenSource;

        public SimulationManager()
        {
            // Initialize entities with different algorithms and add them to the list
            var randomAlgorithm = new RandomDecisionAlgorithm();
            entities.Add(new Human(randomAlgorithm));
            entities.Add(new Zombie(new ProximityDecisionAlgorithm()));
        }

        public async Task StartSimulation()
        {
            cancellationTokenSource = new CancellationTokenSource(); // Reset the cancellation token source

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                foreach (var entity in entities)
                {
                    entity.Update(); // Ensure this method moves the entity
                }
                await Task.Delay(16); // Reduced delay for smoother movement (approximately 60 FPS)
            }
        }

        public void StopSimulation()
        {
            cancellationTokenSource?.Cancel();
        }

        public IEnumerable<ISimulationEntity> GetEntities() => entities;
    }
}
