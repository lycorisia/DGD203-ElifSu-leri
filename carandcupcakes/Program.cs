using System;
using System.Collections.Generic;

namespace carandcupcakes
{
    public class Vehicle
    {
        public float Fuel { get; private set; }

        public Vehicle(float fuel)
        {
            Fuel = Math.Clamp(fuel, 0f, 100f);
        }

        public void UseFuel(float amount)
        {
            Fuel = Math.Clamp(Fuel - amount, 0f, 100f);
            FuelWarning();
        }

        public void Refuel(float amount)
        {
            Fuel = Math.Clamp(Fuel + amount, 0f, 100f);
            FuelWarning();
        }

        // Check fuel level and display a warning if it's below 10%
        private void FuelWarning()
        {
            if (Fuel < 10)
            {
                Console.WriteLine("Warning: Fuel is low!");
            }
        }
    }

    public class Engine
    {
        public int Horsepower { get; }
        public bool IsRunning { get; private set; }

        public Engine(int horsepower)
        {
            Horsepower = horsepower;
        }

        public void Start()
        {
            IsRunning = true;
            Console.WriteLine("Engine started.");
        }

        public void Stop()
        {
            IsRunning = false;
            Console.WriteLine("Engine stopped.");
        }
    }

    public class Car : Vehicle
    {
        public float Speed { get; private set; }
        public float MaxSpeed { get; }
        private readonly Engine _engine;
        private readonly List<string> _cupcakes;
        private const int MaxCupcakes = 1;  // Only 1 cupcake in the slot at a time
        private const float CupcakeFuelConsumption = 15f; // Fuel consumed per cupcake throw
        private const float AccelerateFuelConsumptionRate = 5f; // Fuel consumed per second while accelerating
        private const float LowFuelThreshold = 10f; // Threshold for low fuel warning before throwing a cupcake

        public Car(float fuel, float maxSpeed, int horsepower) : base(fuel)
        {
            MaxSpeed = maxSpeed;
            _engine = new Engine(horsepower);
            _cupcakes = new List<string> { "Cupcake" }; // Start with one cupcake in the slot
        }

        public void StartEngine() => _engine.Start();

        public void StopEngine()
        {
            _engine.Stop();
            Speed = 0;
        }

        public void Accelerate(float time)
        {
            if (!_engine.IsRunning)
            {
                Console.WriteLine("Cannot accelerate, engine is off.");
                return;
            }
            if (Fuel <= 0)
            {
                Console.WriteLine("Cannot accelerate, out of fuel!");
                return;
            }

            Speed = Math.Clamp(Speed + 10 * time, 0, MaxSpeed);
            UseFuel(time * AccelerateFuelConsumptionRate); // Increased fuel consumption rate
            Console.WriteLine($"Accelerating: {Speed} km/h, Fuel: {Fuel}%");
        }

        public void Brake(float time)
        {
            Speed = Math.Max(Speed - 10 * time, 0);
            Console.WriteLine($"Braking: {Speed} km/h");
        }

        public void ThrowCupcake()
        {
            if (_cupcakes.Count > 0)
            {
                if (Fuel < CupcakeFuelConsumption)
                {
                    Console.WriteLine("Not enough fuel to throw a cupcake!");
                    return;
                }

                if (Fuel < LowFuelThreshold)
                {
                    Console.WriteLine("Warning: Fuel is low! Throwing a cupcake will consume more fuel.");
                }

                _cupcakes.RemoveAt(0);
                UseFuel(CupcakeFuelConsumption); // Increased fuel consumption per cupcake throw
                Console.WriteLine("Threw a cupcake! It hits a cat! 🐱 Cupcake slot empty, please reload.");
            }
            else
            {
                Console.WriteLine("No cupcakes loaded! Please reload before throwing.");
            }
        }

        public void ReloadCupcakes()
        {
            if (_cupcakes.Count < MaxCupcakes)
            {
                _cupcakes.Add("Cupcake");
                Console.WriteLine("Reloaded one cupcake! Ready to throw.");
            }
            else
            {
                Console.WriteLine("Cupcake slot is already loaded.");
            }
        }

        public void DisplayStatus()
        {
            Console.WriteLine($"Speed: {Speed} km/h, Fuel: {Fuel}%, Engine Running: {_engine.IsRunning}, Cupcakes Loaded: {_cupcakes.Count}");
        }
    }

    internal class Program
    {
        public static void Main()
        {
            var playerCar = new Car(fuel: 50, maxSpeed: 200, horsepower: 300);  // Start with reduced fuel to see low fuel warning sooner

            playerCar.DisplayStatus();
            playerCar.StartEngine();
            playerCar.Accelerate(2);  // Accelerate for 2 seconds
            playerCar.DisplayStatus();
            playerCar.ThrowCupcake();  // Throw a cupcake, consuming fuel
            playerCar.ReloadCupcakes(); // Reload after throw
            playerCar.ThrowCupcake();  // Throw again, consuming fuel
            playerCar.ReloadCupcakes(); // Reload again
            playerCar.DisplayStatus();
            playerCar.Brake(1);
            playerCar.StopEngine();
            playerCar.DisplayStatus();
        }
    }
}
