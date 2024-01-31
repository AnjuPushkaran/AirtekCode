using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        // Load orders from the provided JSON file
        List<Order> orders = LoadOrders("coding-assigment-orders.json");

        // Create a scheduler
        Scheduler scheduler = new Scheduler();

        // Create flights for Day 1 and Day 2
        Flight day1Flight1 = new Flight("YUL", "YYZ", 20, 1);
        Flight day1Flight2 = new Flight("YUL", "YYC", 20, 1);
        Flight day1Flight3 = new Flight("YUL", "YVR", 20, 1);

        Flight day2Flight4 = new Flight("YUL", "YYZ", 20, 2);
        Flight day2Flight5 = new Flight("YUL", "YYC", 20, 2);
        Flight day2Flight6 = new Flight("YUL", "YVR", 20, 2);

        // Register flights with the scheduler
        scheduler.RegisterFlights(day1Flight1, day1Flight2, day1Flight3, day2Flight4, day2Flight5, day2Flight6);

        // Schedule orders for each flight
        scheduler.ScheduleOrders(orders);

        // Display the orders for scheduled flights
        scheduler.DisplayOrderSchedule();

        // Display the flight schedules
        scheduler.DisplayFlightSchedules();

        // Generate flight itineraries and display
        scheduler.GenerateFlightItineraries(orders);

        Console.ReadLine();
    }

    static List<Order> LoadOrders(string filePath)
    {
        string json = File.ReadAllText(filePath);
        var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, Order>>(json);

        // Set order numbers based on the keys in the JSON file
        return orderDictionary.Values.Select((order, index) =>
        {
            order.OrderNumber = orderDictionary.ElementAt(index).Key;
            return order;
        }).ToList();
    }
}

class Order
{
    public string OrderNumber { get; set; }
    public string Destination { get; set; }
}

class Flight
{
    public string Source { get; }
    public string Destination { get; }
    public int Capacity { get; }
    public List<Order> LoadedBoxes { get; }
    public int Day { get; }

    public Flight(string source, string destination, int capacity, int day)
    {
        Source = source;
        Destination = destination;
        Capacity = capacity;
        LoadedBoxes = new List<Order>();
        Day = day;
    }

    public bool HasCapacity() => LoadedBoxes.Count < Capacity;

    public void LoadOrder(Order order) => LoadedBoxes.Add(order);

    public string GetFlightSchedule() =>
        $"order: {LoadedBoxes.First().OrderNumber}, flightNumber: {Destination}, departure: {Source}, arrival: {Destination}, day: {Day}";
}

class Scheduler
{
    private List<Flight> flights;

    public Scheduler()
    {
        flights = new List<Flight>();
    }

    public void RegisterFlights(params Flight[] flights) => this.flights.AddRange(flights);

    public void ScheduleOrders(List<Order> orders)
    {
        foreach (var order in orders)
        {
            bool scheduled = false;

            foreach (var flight in flights)
            {
                if (flight.Destination == order.Destination && flight.HasCapacity())
                {
                    flight.LoadOrder(order);
                    scheduled = true;
                    break;
                }
            }

        
        }
    }

    public void DisplayFlightSchedules() => flights.ForEach(flight => Console.WriteLine(flight.GetFlightSchedule()));

    public void DisplayOrderSchedule()
    {
        foreach (var flight in flights)
        {
            Console.WriteLine($"Flight from {flight.Source} to {flight.Destination}:");

            if (flight.LoadedBoxes.Any())
            {
                Console.WriteLine("Loaded boxes:");
                flight.LoadedBoxes.ForEach(order => Console.WriteLine($"- {order.OrderNumber}"));
            }
            else
            {
                Console.WriteLine("No boxes loaded for this flight.");
            }

            Console.WriteLine();
        }
    }

    public void GenerateFlightItineraries(List<Order> orders)
    {
        foreach (var order in orders)
        {
            bool scheduled = false;

            foreach (var flight in flights)
            {
                if (flight.Destination == order.Destination && flight.HasCapacity())
                {
                    Console.WriteLine($"order: {order.OrderNumber}, flightNumber: {flight.Destination}, departure: {flight.Source}, arrival: {flight.Destination}, day: {flight.Day}");
                    scheduled = true;
                    flight.LoadOrder(order);
                    break;
                }
            }

            if (!scheduled)
            {
                Console.WriteLine($"order: {order.OrderNumber}, flight Number: not scheduled");
            }
        }
    }
}
