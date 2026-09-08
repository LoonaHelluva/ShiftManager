using System.Net.Http.Json;
using System.Text.Json;

const string defaultBaseUrl = "http://localhost:5241";
string baseUrl = args.Length > 0 ? args[0].TrimEnd('/') : defaultBaseUrl;

using HttpClient httpClient = new()
{
    BaseAddress = new Uri(baseUrl)
};

JsonSerializerOptions jsonOptions = new(JsonSerializerDefaults.Web)
{
    WriteIndented = true
};

while (true)
{
    Console.WriteLine();
    Console.WriteLine($"Helipad Manager CLI ({baseUrl})");
    Console.WriteLine("1. Получить список вертолётов");
    Console.WriteLine("2. Добавить вертолёт");
    Console.WriteLine("3. Обновить вертолёт");
    Console.WriteLine("4. Удалить вертолёт");
    Console.WriteLine("0. Выход");
    Console.Write("Выберите действие: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                await SendAsync(() => httpClient.GetAsync("/helis/"));
                break;
            case "2":
                await CreateHelicopterAsync();
                break;
            case "3":
                await UpdateHelicopterAsync();
                break;
            case "4":
                await DeleteHelicopterAsync();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Неизвестный пункт меню.");
                break;
        }
    }
    catch (HttpRequestException exception)
    {
        Console.WriteLine($"Ошибка подключения: {exception.Message}");
    }
    catch (JsonException exception)
    {
        Console.WriteLine($"Ошибка JSON: {exception.Message}");
    }
}

async Task CreateHelicopterAsync()
{
    int tailNum = ReadInt("Tail number: ");
    string usability = ReadText("Usability: ");
    string flightStatus = ReadText("Flight status: ");

    AddHelicopterRequest request = new(tailNum, usability, flightStatus);
    await SendAsync(() => httpClient.PostAsJsonAsync("/helis/", request, jsonOptions));
}

async Task UpdateHelicopterAsync()
{
    int id = ReadInt("Id вертолёта: ");
    Console.WriteLine("Оставьте поле пустым, чтобы не менять его.");

    string? tailNumInput = ReadOptionalText("Tail number: ");
    string? usability = ReadOptionalText("Usability: ");
    string? flightStatus = ReadOptionalText("Flight status: ");

    int tailNum = string.IsNullOrWhiteSpace(tailNumInput)
        ? -1
        : int.Parse(tailNumInput);

    UpdateHelicopterRequest request = new(tailNum, usability, flightStatus);
    await SendAsync(() => httpClient.PutAsJsonAsync($"/helis/{id}", request, jsonOptions));
}

async Task DeleteHelicopterAsync()
{
    int id = ReadInt("Id вертолёта: ");
    await SendAsync(() => httpClient.DeleteAsync($"/helis/{id}"));
}

async Task SendAsync(Func<Task<HttpResponseMessage>> sendRequest)
{
    using HttpResponseMessage response = await sendRequest();
    string responseBody = await response.Content.ReadAsStringAsync();

    Console.WriteLine();
    Console.WriteLine($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}");
    Console.WriteLine(string.IsNullOrWhiteSpace(responseBody)
        ? "(пустое тело ответа)"
        : TryFormatJson(responseBody));
}

int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int value))
        {
            return value;
        }

        Console.WriteLine("Введите целое число.");
    }
}

string ReadText(string prompt)
{
    while (true)
    {
        string? value = ReadOptionalText(prompt);
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        Console.WriteLine("Поле не может быть пустым.");
    }
}

string? ReadOptionalText(string prompt)
{
    Console.Write(prompt);
    return Console.ReadLine();
}

string TryFormatJson(string responseBody)
{
    try
    {
        using JsonDocument document = JsonDocument.Parse(responseBody);
        return JsonSerializer.Serialize(document, jsonOptions);
    }
    catch (JsonException)
    {
        return responseBody;
    }
}

record AddHelicopterRequest(int TailNum, string Usability, string FlightStatus);
record UpdateHelicopterRequest(int TailNum = -1, string? Usability = null, string? FlightStatus = null);
