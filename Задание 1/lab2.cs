using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

public class Lab2_SyncVersion
{
    public void Execute()
    {
        var stopwatch = Stopwatch.StartNew();
        string[] urls =
        {
            "https://jsonplaceholder.typicode.com/posts/1",
            "https://jsonplaceholder.typicode.com/posts/2",
            "https://jsonplaceholder.typicode.com/posts/1000" // тут намеренная ошибка!
        };

        using (var client = new HttpClient())
        {
            try
            {
                foreach (var url in urls)
                {
                    var response = client.Send(new HttpRequestMessage(HttpMethod.Get, url));

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Ошибка: HTTP {response.StatusCode} | URL: {url}");
                        return;
                    }

                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    Console.WriteLine($"Ответ от {url}:\n{json}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Синхронное выполнение заняло: {stopwatch.Elapsed}\n");
            }
        }
    }
}

public class Lab2_AsyncVersion
{
    public async Task ExecuteAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        string[] urls =
        {
            "https://jsonplaceholder.typicode.com/posts/1",
            "https://jsonplaceholder.typicode.com/posts/2",
            "https://jsonplaceholder.typicode.com/posts/1000" // тут намеренная ошибка!
        };

        using (var client = new HttpClient())
        {
            try
            {
                var tasks = urls.Select(url => client.GetAsync(url)).ToArray();
                var responses = await Task.WhenAll(tasks);

                foreach (var response in responses)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Ошибка: HTTP {response.StatusCode} | URL: {response.RequestMessage.RequestUri}");
                        return;
                    }
                }

                foreach (var response in responses)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ответ от {response.RequestMessage.RequestUri}:\n{json}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Асинхронное выполнение заняло: {stopwatch.Elapsed}\n");
            }
        }
    }
}