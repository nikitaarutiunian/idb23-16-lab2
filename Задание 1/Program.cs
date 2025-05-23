using System;

Console.WriteLine("=== СИНХРОННАЯ ВЕРСИЯ ===\n");
new Lab2_SyncVersion().Execute();

Console.WriteLine("=== АСИНХРОННАЯ ВЕРСИЯ ===\n");
new Lab2_AsyncVersion().ExecuteAsync().GetAwaiter().GetResult();
