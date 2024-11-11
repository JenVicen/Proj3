using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using BlazorApp.Entities;
using BlazorApp.Services;

namespace BlazorApp.Services
{
    public class SimulationManager
    {
        private List<ISimulationEntity> entities = new List<ISimulationEntity>();
        private CancellationTokenSource cancellationTokenSource;
        private Task simulationTask;
        private const int CollisionDistance = 10; 
        public event Action OnSimulationUpdated; 

        public SimulationManager()
        {
            // Initialize entities with different algorithms and add them to the list
            var randomAlgorithm = new RandomDecisionAlgorithm();
            entities.Add(new Human(randomAlgorithm));
            entities.Add(new Zombie(randomAlgorithm));
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
                OnSimulationUpdated?.Invoke();
                await Task.Delay(100); // Reduced delay for smoother movement (approximately 60 FPS)
            }
        }

        public void StopSimulation()
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task RunSimulationLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var entity in entities)
                {
                    entity.Update();
                }

                CheckCollisions();

                OnSimulationUpdated?.Invoke();
                await Task.Delay(100);
            }
        }

        private void CheckCollisions()
        {
            var newZombies = new List<ISimulationEntity>();

            foreach (var entity in entities)
            {
                if (entity is Zombie zombie)
                {
                    foreach (var human in entities.OfType<Human>())
                    {
                        if (IsColliding(zombie.Location, human.Location))
                        {
                            newZombies.Add(new Zombie(zombie.DecisionAlgorithm) { Location = human.Location });
                        }
                    }
                }
            }

            // Convertir los humanos infectados a zombies y eliminar humanos
            entities.RemoveAll(e => e is Human h && newZombies.Exists(z => IsColliding(z.Location, h.Location)));
            entities.AddRange(newZombies);
        }

        private bool IsColliding(Point pos1, Point pos2)
        {
            var distance = Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
            return distance <= CollisionDistance;
        }

        public IEnumerable<ISimulationEntity> GetEntities() => entities;
    }
}
