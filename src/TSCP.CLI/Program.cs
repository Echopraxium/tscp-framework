// uuid: e9d8c7b6-a5f4-4321-8019-e1d2c3b4a5f6
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using TSCP.Core;
using static TSCP.Core.Domain; // Access to Concept, SystemicVector, etc.
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace TSCP.CLI
{
    class Program
    {
        // Initialize State with the new F# Record structure
        private static SessionState _state = SessionManager.createDefault();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== TSCP Systemic Framework (Core 4 Engine) ===");
            Console.WriteLine("Architecture: [Structure | Information | Dynamics | Teleonomy]");
            Console.ResetColor();
            Console.WriteLine("Type 'help' for a list of commands.");

            // Initial load of catalog to verify M3 binding
            var initialCount = Catalog.loadFromDisk().Length;
            Console.WriteLine($"System initialized. {initialCount} concepts available in Catalog.");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\n[Layer:{_state.ActiveLayer:F1}] > ");
                Console.ResetColor();
                
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                try 
                {
                    ProcessCommand(input);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"! System Error: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        static void ProcessCommand(string input)
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return;

            var cmd = parts[0].ToLower();
            var args = parts.Skip(1).ToList();

            switch (cmd)
            {
                case "exit":
                case "quit":
                    Environment.Exit(0);
                    break;

                case "help":
                    PrintHelp();
                    break;

                case "pwd":
                    Console.WriteLine($"Catalog Root: {Catalog.getCatalogPath()}");
                    break;

                case "catalog":
                case "ls":
                    HandleCatalog();
                    break;

                case "load":
                    if (args.Count > 0) HandleLoad(args[0]);
                    else Console.WriteLine("Usage: load <id_or_index>");
                    break;

                case "inspect":
                case "show":
                    if (args.Count > 0) HandleInspect(args[0]);
                    else Console.WriteLine("Usage: inspect <id_or_index>");
                    break;

                case "analyse":
                case "analyze":
                    HandleAnalyze();
                    break;

                case "list":
                case "history":
                    HandleHistory();
                    break;

                case "reset":
                    _state = SessionManager.createDefault();
                    Console.WriteLine("Session reset to M3 default.");
                    break;

                case "export":
                    Console.WriteLine("Export functionality is being refactored for M3 Vectors.");
                    break;

                default:
                    // Delegate generic commands to the F# Engine
                    RunEngineCommand(cmd);
                    break;
            }
        }

        static void HandleCatalog()
        {
            var items = Catalog.loadFromDisk();
            if (items.IsEmpty) 
            {
                Console.WriteLine("Warning: Catalog is empty.");
            }
            else 
            {
                Console.WriteLine("\n--- Available Concepts (M2/M1) ---");
                int i = 1;
                foreach (var c in items)
                {
                    Console.Write($"{i}. [{c.Id}] {c.Name} ");
                    PrintVector(c.Signature);
                    i++;
                }
            }
        }

        static void HandleLoad(string idOrIdx)
        {
            var concept = ResolveConcept(idOrIdx);

            if (concept != null) 
            {
                // Create a new ActiveConcept entry
                var entry = SessionEntry.NewActiveConcept(concept);
                
                // Update State (Immutable F# list prepend)
                var newHistory = FSharpList<SessionEntry>.Cons(entry, _state.History);
                
                // Update the state record
                _state = new SessionState(newHistory, _state.ActiveLayer);

                Console.WriteLine($"Loaded '{concept.Name}' into active context.");
                PrintVector(concept.Signature);
            } 
            else 
            {
                Console.WriteLine($"Error: Concept '{idOrIdx}' not found.");
            }
        }

        static void HandleInspect(string idOrIdx)
        {
            var c = ResolveConcept(idOrIdx);
            if (c != null)
            {
                Console.WriteLine($"\n=== CONCEPT: {c.Name} ({c.Id}) ===");
                Console.WriteLine($"Description: {c.Description}");
                Console.Write("Signature:   ");
                PrintVector(c.Signature);
                Console.WriteLine($"Tags:        {string.Join(", ", c.Tags)}");
                Console.WriteLine("========================================");
            }
            else
            {
                Console.WriteLine("Concept not found.");
            }
        }

        static void HandleAnalyze()
        {
            // Extract ActiveConcepts from F# List history
            var concepts = _state.History
                .Where(e => e.IsActiveConcept)
                .Select(e => ((SessionEntry.ActiveConcept)e).Item)
                .ToList();

            // Convert C# List to F# List for the Engine
            var fsList = ListModule.OfSeq(concepts);

            // Call Engine
            var metrics = Engine.calculateMetrics(fsList);

            Console.WriteLine("\n--- Systemic Analysis (Core 4) ---");
            Console.WriteLine($"Dominant Invariant:   {metrics.DominantInvariant}");
            Console.WriteLine($"System Entropy:       {metrics.Entropy:F4}");
            Console.WriteLine($"Teleonomic Coherence: {metrics.Coherence:F4}");
            Console.WriteLine("Average Vector:");
            PrintVector(metrics.AverageVector);
        }

        static void HandleHistory()
        {
            Console.WriteLine("\n--- Session History ---");
            foreach (var entry in _state.History) 
            {
                if (entry.IsLog) 
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[LOG] {((SessionEntry.Log)entry).Item}");
                    Console.ResetColor();
                }
                else if (entry.IsActiveConcept) 
                {
                    var c = ((SessionEntry.ActiveConcept)entry).Item;
                    Console.Write($"[CONCEPT] {c.Name} ");
                    // Short vector display
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"[T:{c.Signature.Teleonomy:F1}]");
                    Console.ResetColor();
                }
            }
        }

        static void RunEngineCommand(string cmd)
        {
            var result = Engine.executeCommand(cmd, _state);
            _state = result.Item1;
            
            if (OptionModule.IsSome(result.Item2)) 
            {
                Console.WriteLine(result.Item2.Value);
            }
            else
            {
                Console.WriteLine($"Unknown command: {cmd}");
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("\n=== COMMANDS ===");
            Console.WriteLine(" catalog / ls      : List available concepts with Vector Signatures");
            Console.WriteLine(" load <id/idx>     : Load a concept into active memory");
            Console.WriteLine(" inspect <id/idx>  : View full details of a concept");
            Console.WriteLine(" analyse           : Compute Entropy and Coherence of active system");
            Console.WriteLine(" history / list    : Show session logs");
            Console.WriteLine(" sync              : Simulate continuum shift");
            Console.WriteLine(" reset             : Clear session");
            Console.WriteLine(" exit              : Quit");
        }

        // FIX: Changed return type to 'Concept?' (nullable) to resolve CS8603
        static Concept? ResolveConcept(string idOrIdx)
        {
            var items = Catalog.loadFromDisk();
            
            // 1. Try index (1-based)
            if (int.TryParse(idOrIdx, out int idx))
            {
                // items is an F# list, convert to Enumerable to index
                if (idx > 0 && idx <= items.Length)
                {
                    return items.ElementAt(idx - 1);
                }
            }
            
            // 2. Try ID match
            return items.FirstOrDefault(c => c.Id.Equals(idOrIdx, StringComparison.OrdinalIgnoreCase));
        }

        // Helper to pretty-print the S-I-D-T vector
        static void PrintVector(SystemicVector v)
        {
            Console.Write("[ ");
            Console.ForegroundColor = ConsoleColor.White; Console.Write($"S:{v.Structure:F1} ");
            Console.ForegroundColor = ConsoleColor.Cyan;  Console.Write($"I:{v.Information:F1} ");
            Console.ForegroundColor = ConsoleColor.Red;   Console.Write($"D:{v.Dynamics:F1} ");
            Console.ForegroundColor = ConsoleColor.Yellow;Console.Write($"T:{v.Teleonomy:F1} ");
            Console.ResetColor();
            Console.WriteLine("]");
        }
    }
}