using System;
using System.Drawing;

namespace BlazorApp.Entities
{
    public interface ISimulationEntity
    {
        Point Location { get; set; }
        void Move();
        void Update();
    }
}
