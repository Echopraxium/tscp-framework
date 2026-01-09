using System;
using System.Linq;
using TSCP.Core;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;

namespace TSCP.CLI
{
    // FIX : Qualification complète de l'attribut (TSCP.Core.Domain.UuidAttribute)
    [TSCP.Core.Domain.Uuid("48a2c5b1-79e3-4d6f-8a1b-9c2d3e4f5a6b")]
    class Program
    {
        // État de session (Qualified name pour éviter ambiguïté)
        private static TSCP.Core.Domain.SessionState _state = TSCP.Core.Domain.SessionState.Default;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=== TSCP Systemic Framework (Core 5 Engine) ===");
            Console.WriteLine("Architecture: [Structure | Information | Dynamics | Teleonomy]");
            
            // FIX : Réflexion avec le type complet
            var uuid = typeof(Program).GetCustomAttributes(typeof(TSCP.Core.Domain.UuidAttribute), false)
                                      .FirstOrDefault() as TSCP.Core.Domain.UuidAttribute;
            
            if (uuid != null) Console.WriteLine($"Module ID:    {uuid.Uuid}");

            Console.ResetColor();
            Console.WriteLine("Type 'help' for a list of commands.");

            try 
            {
                var concepts = Catalog.loadAll();
                Console.WriteLine($"[Init] System initialized. {concepts.Length} concepts loaded via Catalog.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Init Error] {ex.Message}");
                Console.ResetColor();
            }

            // Boucle REPL
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

                case "clear":
                case "cls":
                    Console.Clear();
                    break;

                case "reload":
                    try 
                    {
                        var reloaded = Catalog.loadAll();
                        Console.WriteLine($"[Reload] Catalog synchronized. {reloaded.Length} concepts active.");
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine($"[Error] Reload failed: {ex.Message}");
                    }
                    break;

                case "sysinfo":
                case "version":
                    // FIX : Utilisation du type complet TSCP.Core.Domain.UuidAttribute
                    var programUuid = typeof(Program).GetCustomAttributes(typeof(TSCP.Core.Domain.UuidAttribute), false)
                                            .FirstOrDefault() as TSCP.Core.Domain.UuidAttribute;
                    Console.WriteLine("--- System Information ---");
                    Console.WriteLine($"CLI Module:  {programUuid?.Uuid ?? "Unknown"}");
                    Console.WriteLine($"Core State:  {_state.ActiveLayer} (Layer)");
                    Console.WriteLine($"Framework:   .NET 10");
                    break;

                case "catalog":
                case "ls":
                    HandleCatalog();
                    break;

                case "load":
                case "simulate":
                case "analyze":
                case "analyse":
                    RunEngineCommand(input);
                    break;

                case "inspect":
                case "show":
                    if (args.Count > 0) HandleInspect(args[0]);
                    else Console.WriteLine("Usage: inspect <id_or_index>");
                    break;

                case "history":
                    HandleHistory();
                    break;

                case "reset":
                    _state = TSCP.Core.Domain.SessionState.Default;
                    Console.WriteLine("Session reset to default.");
                    break;

                default:
                    RunEngineCommand(input);
                    break;
            }
        }

        static void HandleCatalog()
        {
            var items = Catalog.loadAll();
            if (items.Length == 0) Console.WriteLine("Warning: Catalog is empty.");
            else 
            {
                Console.WriteLine("\n--- Available Concepts ---");
                int i = 1;
                foreach (var c in items)
                {
                    Console.Write($"{i}. [{c.Id}] {c.Name} ");
                    PrintVector(c.Signature);
                    i++;
                }
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
            else Console.WriteLine("Concept not found.");
        }

        static void HandleHistory()
        {
            Console.WriteLine("\n--- Session History ---");
            foreach (var entry in _state.History) 
            {
                if (entry.IsLogEntry) 
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"[LOG] {entry.GetLogContent()}");
                    Console.ResetColor();
                }
                else if (entry.IsConceptEntry) 
                {
                    var optC = entry.GetConcept();
                    if (OptionModule.IsSome(optC))
                    {
                        var c = optC.Value;
                        Console.Write($"[CONCEPT] {c.Name} ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine($"[T:{c.Signature.Teleonomy:F1}]");
                        Console.ResetColor();
                    }
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
        }

        static void PrintHelp()
        {
            Console.WriteLine("\n=== COMMANDS ===");
            Console.WriteLine(" catalog / ls      : List available concepts");
            Console.WriteLine(" load <id>         : Load a concept");
            Console.WriteLine(" simulate <id>     : Simulate interaction");
            Console.WriteLine(" inspect <id>      : View details");
            Console.WriteLine(" analyze           : Compute Entropy");
            // --- AJOUT ICI ---
            Console.WriteLine(" profile <id>      : Semantic Profiling (Nature & Peers)");
            // -----------------
            Console.WriteLine(" history           : Show logs");
            Console.WriteLine(" sysinfo           : Show Module UUIDs");
            Console.WriteLine(" reload            : Reload Catalog");
            Console.WriteLine(" clear             : Clear screen");
            Console.WriteLine(" reset             : Clear session");
            Console.WriteLine(" exit              : Quit");
        }

        static TSCP.Core.Domain.Concept? ResolveConcept(string idOrIdx)
        {
            var items = Catalog.loadAll();
            if (int.TryParse(idOrIdx, out int idx) && idx > 0 && idx <= items.Length) return items[idx - 1];
            return items.FirstOrDefault(c => c.Id.Equals(idOrIdx, StringComparison.OrdinalIgnoreCase));
        }

        static void PrintVector(TSCP.Core.Domain.SystemicVector v)
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