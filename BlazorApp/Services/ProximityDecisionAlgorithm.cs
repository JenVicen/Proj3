using BlazorApp.Entities;
using BlazorApp.Services;
using System.Drawing;

namespace BlazorApp.Services
{

    public class ProximityDecisionAlgorithm : IDecisionAlgorithm
    {
        public Point FindNearestTarget(Point currentLocation, Point[] targets)
        {
            if (targets == null || targets.Length == 0)
            {
                // Handle the case where there are no targets
                throw new InvalidOperationException("No targets available to find the nearest one.");
            }

            Point nearestTarget = targets[0];
            double shortestDistance = GetDistance(currentLocation, nearestTarget);

            for (int i = 1; i < targets.Length; i++)
            {
                double distance = GetDistance(currentLocation, targets[i]);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTarget = targets[i];
                }
            }

            return nearestTarget;
        }

        private double GetDistance(Point a, Point b)
        {
            // Calculate the Euclidean distance between two points
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public Point ChooseNextTarget(ISimulationEntity entity, Point currentLocation)
        {
            Point[] targets = GetTargetsForEntity(entity);

            if (targets == null || targets.Length == 0)
            {
                // Log a warning or handle the case where there are no targets
                Console.WriteLine("Warning: No targets available for entity to find the nearest one.");
                return currentLocation; // Stay in the current location or implement other logic
            }

            return FindNearestTarget(currentLocation, targets);
        }

        private Point[] GetTargetsForEntity(ISimulationEntity entity)
        {
            // Example: Provide a variety of target points to encourage movement in all directions
            return new Point[] 
            { 
                new Point(100, 100), 
                new Point(200, 200), 
                new Point(800, 500), 
                new Point(500, 300) 
            }; // Replace with actual logic
        }
    }
}
