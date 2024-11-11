using BlazorApp.Services;
using System.Drawing;
using System;

namespace BlazorApp.Entities
{
    public class Zombie : ISimulationEntity
    {
        public Point Location { get; set; }
        public int Lifespan { get; private set; }
        public IDecisionAlgorithm DecisionAlgorithm;
        

        public Zombie(IDecisionAlgorithm decisionAlgorithm)
        {
            DecisionAlgorithm = decisionAlgorithm;
            Location = new Point(new Random().Next(0, 1000 - 10), new Random().Next(0, 600 - 10));
            Lifespan = new Random().Next(100, 200); // Random initial lifespan
        }

        public void Move()
        {
            var newLocation = DecisionAlgorithm.ChooseNextTarget(this, Location);
            Location = new Point(
                Math.Clamp(newLocation.X, 0, 1000 - 10), // 1000 is the width of the map-container, 10 is the entity width
                Math.Clamp(newLocation.Y, 0, 600 - 10)  // 600 is the height of the map-container, 10 is the entity height
            );
        }

        public void Update()
        {
            Lifespan--;
            Move();
        }
    }
}
