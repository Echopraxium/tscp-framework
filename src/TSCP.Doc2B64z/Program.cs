using System.IO.Compression;
using System.Text;

namespace TSCP.Doc2B64z;

class Program
{
    // WHITELIST STRICTE (Documents & Data Sémantique)
    private static readonly HashSet<string> ValidExtensions = new() 
    { 
        ".md", ".txt", ".rtf", ".html", ".tex", 
        ".json", ".jsonld", ".ttl", ".nt", ".dot", ".sparql"
    };

    private static Dictionary<int, string> _fileIndex = new();

    static void Main(string[] args)
    {
        SetProjectScope();

        Console.Title = "TSCP.Doc2B64z - Content Focused I/O";
        Console.WriteLine($"TSCP.Doc2B64z [Net 10.0]");
        Console.WriteLine($"Working Dir: {Directory.GetCurrentDirectory()}");
        Console.WriteLine("--------------------------------------------------");

        // Batch Mode
        if (args.Length > 0)
        {
            ProcessCommand(args);
            return; 
        }

        // Interactive Mode (REPL)
        Console.WriteLine("Interactive Mode activated.");
        ShowHelp(); // Affiche l'aide au démarrage
        
        bool running = true;
        while (running)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nTSCP.Doc2B64z> ");
            Console.ResetColor();

            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            string[] parts = input.Trim().Split(' ');
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "exit": case "quit": case "x": 
                    running = false; 
                    break;
                
                case "cls": case "clear": 
                    Console.Clear(); 
                    break;
                
                case "list": case "ls": case "l": 
                    ListFiles(); 
                    break;
                
                case "help": case "h": case "?":
                    ShowHelp(); 
                    break;

                case "test": 
                    RunSafeTestSequence(); 
                    break;

                default: 
                    ProcessCommand(parts); 
                    break;
            }
        }
    }

    static void ShowHelp()
    {
        Console.WriteLine("\nAvailable Commands:");
        Console.WriteLine("  list       (or l) : List valid files (Docs/Data only).");
        Console.WriteLine("  compress   (or c) : Compress a file (usage: c <id|path>).");
        Console.WriteLine("  decompress (or d) : Decompress a .b64z archive (usage: d <id|path>).");
        Console.WriteLine("  test              : Run integrity check on 'samples' folder.");
        Console.WriteLine("  help       (or h) : Show this message.");
        Console.WriteLine("  exit       (or x) : Exit application.");
    }

    static void SetProjectScope()
    {
        string current = Directory.GetCurrentDirectory();
        string targetFile = "TSCP.Doc2B64z.csproj";

        if (File.Exists(Path.Combine(current, targetFile))) return;

        string subPath = Path.Combine(current, "src", "TSCP.Doc2B64z");
        if (Directory.Exists(subPath) && File.Exists(Path.Combine(subPath, targetFile)))
        {
            Directory.SetCurrentDirectory(subPath);
            return;
        }

        DirectoryInfo? dir = new DirectoryInfo(current);
        while (dir != null)
        {
            if (File.Exists(Path.Combine(dir.FullName, targetFile)))
            {
                Directory.SetCurrentDirectory(dir.FullName);
                return;
            }
            dir = dir.Parent;
        }
    }

    static void ListFiles()
    {
        _fileIndex.Clear();
        string currentDir = Directory.GetCurrentDirectory();
        
        var rootFiles = Directory.GetFiles(currentDir);
        var sampleFiles = Enumerable.Empty<string>();
        
        string sampleDir = Path.Combine(currentDir, "samples");
        if (Directory.Exists(sampleDir))
        {
            sampleFiles = Directory.GetFiles(sampleDir);
        }

        // Filtrage Strict (Whitelist OU .b64z)
        var allFiles = rootFiles.Concat(sampleFiles)
            .Where(f => IsVisibleFile(f))
            .OrderBy(f => f)
            .ToArray();

        Console.WriteLine($"\nDirectory Content (Filtered: Docs & Data only)");
        Console.WriteLine("------------------------------------------");

        if (allFiles.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("(No relevant files found. Use 'test' to generate samples)");
            Console.ResetColor();
        }

        for (int i = 0; i < allFiles.Length; i++)
        {
            int index = i + 1;
            _fileIndex[index] = allFiles[i];
            
            string relativePath = Path.GetRelativePath(currentDir, allFiles[i]);
            
            if (relativePath.EndsWith(".b64z")) Console.ForegroundColor = ConsoleColor.DarkYellow;
            else Console.ForegroundColor = ConsoleColor.Green;
            
            Console.WriteLine($" [{index}] {relativePath}");
            Console.ResetColor();
        }
    }

    static bool IsVisibleFile(string path)
    {
        if (path.EndsWith(".b64z", StringComparison.OrdinalIgnoreCase)) return true;
        string ext = Path.GetExtension(path).ToLower();
        return ValidExtensions.Contains(ext);
    }

    static void ProcessCommand(string[] args)
    {
        string command = args[0].ToLower();
        if (args.Length < 2) { ShowError("Invalid syntax. Usage: c <id> or d <id>"); return; }

        string targetPath = ResolvePath(args[1]);
        if (string.IsNullOrEmpty(targetPath)) return; 

        try
        {
            switch (command)
            {
                case "c": case "compress":
                    CompressFile(targetPath);
                    break;
                case "d": case "decompress":
                    string? outputOverride = args.Length > 2 ? args[2] : null;
                    DecompressFile(targetPath, outputOverride);
                    break;
                default:
                    ShowError($"Unknown command '{command}'. Type 'h' for help.");
                    break;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    // ---------------------------------------------------------
    // ENGINE
    // ---------------------------------------------------------

    static string CompressFile(string inputPath, bool silent = false)
    {
        if (!File.Exists(inputPath)) throw new FileNotFoundException(inputPath);

        if (inputPath.EndsWith(".b64z", StringComparison.OrdinalIgnoreCase))
        {
            ShowError($"File is already a .b64z archive.");
            Console.WriteLine("    -> Please use 'd' (Decompress) instead.");
            return inputPath;
        }

        string originalExt = Path.GetExtension(inputPath); 

        // Warning hors whitelist (cas d'usage forcé manuellement)
        if (!ValidExtensions.Contains(originalExt.ToLower()))
        {
             Console.ForegroundColor = ConsoleColor.Yellow;
             Console.WriteLine($"[WARNING] Extension '{originalExt}' is not in the whitelist, but compressing anyway.");
             Console.ResetColor();
        }

        byte[] originalBytes = File.ReadAllBytes(inputPath);

        using var memoryStream = new MemoryStream();
        using (var gzip = new GZipStream(memoryStream, CompressionLevel.Optimal))
        {
            gzip.Write(originalBytes, 0, originalBytes.Length);
        }
        byte[] compressedBytes = memoryStream.ToArray();
        string base64Body = Convert.ToBase64String(compressedBytes);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(originalExt); 
        sb.Append(base64Body);

        string outputPath = inputPath + ".b64z";
        File.WriteAllText(outputPath, sb.ToString());

        if (!silent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[OK] Generated: {Path.GetFileName(outputPath)}");
            Console.ResetColor();
            double ratio = (double)new FileInfo(outputPath).Length / originalBytes.Length;
            Console.WriteLine($" (Ratio: {ratio:P0})");
        }

        return outputPath;
    }

    static void DecompressFile(string inputPath, string? outputOverride = null, bool silent = false)
    {
        if (!File.Exists(inputPath)) throw new FileNotFoundException(inputPath);

        if (!inputPath.EndsWith(".b64z", StringComparison.OrdinalIgnoreCase))
        {
            ShowError($"File is not a .b64z archive.");
            Console.WriteLine("    -> Please use 'c' (Compress) instead.");
            return;
        }

        string[] lines = File.ReadAllLines(inputPath);
        if (lines.Length < 2) throw new Exception("Invalid format (Missing Header).");

        string originalExt = lines[0].Trim(); 
        
        StringBuilder base64Builder = new StringBuilder();
        for(int i=1; i<lines.Length; i++) base64Builder.Append(lines[i]);
        string base64Content = base64Builder.ToString();

        string finalOutputPath;
        if (!string.IsNullOrEmpty(outputOverride))
        {
            finalOutputPath = outputOverride;
        }
        else
        {
            string basePath = inputPath.Replace(".b64z", ""); 
            finalOutputPath = GetSafeOutputName(basePath, originalExt);
        }

        byte[] compressedBytes = Convert.FromBase64String(base64Content);
        using var inputStream = new MemoryStream(compressedBytes);
        using var gzip = new GZipStream(inputStream, CompressionMode.Decompress);
        using var outputStream = new MemoryStream();
        gzip.CopyTo(outputStream);
        
        byte[] restoredBytes = outputStream.ToArray();
        File.WriteAllBytes(finalOutputPath, restoredBytes);

        if (!silent)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"[OK] Restored to: {Path.GetFileName(finalOutputPath)}");
            Console.ResetColor();

            string presumedOriginal = inputPath.Substring(0, inputPath.Length - 5); 

            if (File.Exists(presumedOriginal))
            {
                byte[] originalBytes = File.ReadAllBytes(presumedOriginal);
                if (originalBytes.SequenceEqual(restoredBytes))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($" [INTEGRITY: VALID]");
                    Console.ResetColor();
                    Console.WriteLine($"    (Restored file matches original source '{Path.GetFileName(presumedOriginal)}')");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" [INTEGRITY: CORRUPTED]");
                    Console.ResetColor();
                    Console.WriteLine($"    (WARNING: Content mismatch with '{Path.GetFileName(presumedOriginal)}')");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" [Original source not found for comparison]");
                Console.ResetColor();
            }
        }
    }

    static void RunSafeTestSequence()
    {
        string sampleDir = Path.Combine(Directory.GetCurrentDirectory(), "samples");
        Console.WriteLine($"\n[TEST MODE] Auditing folder: {sampleDir}");
        
        if (!Directory.Exists(sampleDir))
        {
            Directory.CreateDirectory(sampleDir);
            File.WriteAllText(Path.Combine(sampleDir, "TSCP_Phase_4.md"), "# TSCP Phase 4\nSimulation Data.");
            Console.WriteLine($"[+] 'samples' directory created with dummy file.");
        }

        var files = Directory.GetFiles(sampleDir);
        int processed = 0;
        int errors = 0;

        foreach (var file in files)
        {
            if (!IsVisibleFile(file) || file.EndsWith(".b64z") || file.Contains("_B64unz")) continue;

            string ext = Path.GetExtension(file).ToLower();
            processed++;
            Console.WriteLine($"\n[>] Source: {Path.GetFileName(file)}");

            string compressedPath = CompressFile(file, silent: true);
            string expectedRestoredPath = GetSafeOutputName(file.Replace(".b64z", ""), ext);
            
            if (File.Exists(expectedRestoredPath)) File.Delete(expectedRestoredPath);

            DecompressFile(compressedPath, outputOverride: null, silent: true);

            if (!File.Exists(expectedRestoredPath)) { errors++; continue; }

            bool isIdentical = File.ReadAllBytes(file).SequenceEqual(File.ReadAllBytes(expectedRestoredPath));

            if (isIdentical)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"    [OK] Loopback Check Success.");
                Console.ResetColor();
                Console.WriteLine($"    -> Artifacts kept: {Path.GetFileName(compressedPath)}"); 
                Console.WriteLine($"                       {Path.GetFileName(expectedRestoredPath)}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"    [FAILURE] Content Divergence!");
                Console.ResetColor();
                errors++;
            }
        }
        Console.WriteLine($"\n[END] {processed} files processed, {errors} errors.");
    }

    static string GetSafeOutputName(string basePath, string originalExt)
    {
        if (basePath.EndsWith(originalExt, StringComparison.OrdinalIgnoreCase))
        {
            string nameWithoutExt = basePath.Substring(0, basePath.Length - originalExt.Length);
            return nameWithoutExt + "_B64unz" + originalExt;
        }
        return basePath + "_B64unz" + originalExt;
    }

    static string ResolvePath(string input)
    {
        if (int.TryParse(input, out int index))
        {
            return _fileIndex.ContainsKey(index) ? _fileIndex[index] : string.Empty;
        }
        return input;
    }

    static void ShowError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[!] {msg}");
        Console.ResetColor();
    }
}