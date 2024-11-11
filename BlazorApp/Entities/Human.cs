using BlazorApp.Services;
using System;
using System.Drawing;

namespace BlazorApp.Entities
{
    public class Human : ISimulationEntity
    {
        public Point Location { get; set; }
        public int Stamina { get; set; }
        private IDecisionAlgorithm DecisionAlgorithm;

        public Human(IDecisionAlgorithm decisionAlgorithm)
        {
            DecisionAlgorithm = decisionAlgorithm;
            Location = new Point(new Random().Next(0, 1000 - 10), new Random().Next(0, 600 - 10));
            Stamina = new Random().Next(50, 100); // Random initial stamina
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
            Stamina--;
            Move();
        }
    }
}
