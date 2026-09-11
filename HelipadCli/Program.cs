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
    Console.WriteLine("5. Получить список задач");
    Console.WriteLine("6. Получить задачу по Id");
    Console.WriteLine("7. Добавить задачу");
    Console.WriteLine("8. Обновить задачу");
    Console.WriteLine("9. Удалить задачу");
    Console.WriteLine("10. Получить список смен");
    Console.WriteLine("11. Получить смену по Id");
    Console.WriteLine("12. Добавить смену");
    Console.WriteLine("13. Обновить смену");
    Console.WriteLine("14. Удалить смену");
    Console.WriteLine("15. Получить список сотрудников");
    Console.WriteLine("16. Получить сотрудника по Id");
    Console.WriteLine("17. Добавить сотрудника");
    Console.WriteLine("18. Обновить сотрудника");
    Console.WriteLine("19. Удалить сотрудника");
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
            case "5":
                await SendAsync(() => httpClient.GetAsync("/tasks/"));
                break;
            case "6":
                await GetTaskAsync();
                break;
            case "7":
                await CreateTaskAsync();
                break;
            case "8":
                await UpdateTaskAsync();
                break;
            case "9":
                await DeleteTaskAsync();
                break;
            case "10":
                await SendAsync(() => httpClient.GetAsync("/shifts/"));
                break;
            case "11":
                await GetShiftAsync();
                break;
            case "12":
                await CreateShiftAsync();
                break;
            case "13":
                await UpdateShiftAsync();
                break;
            case "14":
                await DeleteShiftAsync();
                break;
            case "15":
                await SendAsync(() => httpClient.GetAsync("/staff/"));
                break;
            case "16":
                await GetStaffAsync();
                break;
            case "17":
                await CreateStaffAsync();
                break;
            case "18":
                await UpdateStaffAsync();
                break;
            case "19":
                await DeleteStaffAsync();
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

async Task GetTaskAsync()
{
    int id = ReadInt("Id задачи: ");
    await SendAsync(() => httpClient.GetAsync($"/tasks/{id}"));
}

async Task CreateTaskAsync()
{
    string title = ReadText("Title: ");
    string description = ReadText("Description: ");
    int heliId = ReadInt("Id вертолёта: ");
    int shiftId = ReadInt("Id смены: ");
    List<StaffMemberRequest> executor = ReadExecutors();

    AddTaskRequest request = new(title, description, executor, heliId, shiftId);
    await SendAsync(() => httpClient.PostAsJsonAsync("/tasks/", request, jsonOptions));
}

async Task UpdateTaskAsync()
{
    int id = ReadInt("Id задачи: ");
    Console.WriteLine("Оставьте поле пустым, чтобы не менять его.");

    string? title = ReadOptionalText("Title: ");
    string? description = ReadOptionalText("Description: ");
    bool? isDone = ReadOptionalBool("Is done (true/false): ");

    if (title == null && description == null && isDone == null)
    {
        Console.WriteLine("Необходимо указать хотя бы одно поле для обновления.");
        return;
    }

    UpdateTaskRequest request = new(title, description, isDone);
    await SendAsync(() => httpClient.PutAsJsonAsync($"/tasks/{id}", request, jsonOptions));
}

async Task DeleteTaskAsync()
{
    int id = ReadInt("Id задачи: ");
    await SendAsync(() => httpClient.DeleteAsync($"/tasks/{id}"));
}

async Task GetShiftAsync()
{
    int id = ReadInt("Id смены: ");
    await SendAsync(() => httpClient.GetAsync($"/shifts/{id}"));
}

async Task CreateShiftAsync()
{
    DateOnly date = ReadDate("Date (yyyy-MM-dd): ");
    int managerId = ReadInt("Id менеджера: ");

    AddShiftRequest request = new(date, managerId);
    await SendAsync(() => httpClient.PostAsJsonAsync("/shifts/", request, jsonOptions));
}

async Task UpdateShiftAsync()
{
    int id = ReadInt("Id смены: ");
    Console.WriteLine("Оставьте поле пустым, чтобы не менять его.");

    DateOnly? date = ReadOptionalDate("Date (yyyy-MM-dd): ");
    int? managerId = ReadOptionalInt("Id менеджера: ");

    if (date == null && managerId == null)
    {
        Console.WriteLine("Необходимо указать хотя бы одно поле для обновления.");
        return;
    }

    UpdateShiftRequest request = new(date, managerId);
    await SendAsync(() => httpClient.PutAsJsonAsync($"/shifts/{id}", request, jsonOptions));
}

async Task DeleteShiftAsync()
{
    int id = ReadInt("Id смены: ");
    await SendAsync(() => httpClient.DeleteAsync($"/shifts/{id}"));
}

async Task GetStaffAsync()
{
    int id = ReadInt("Id сотрудника: ");
    await SendAsync(() => httpClient.GetAsync($"/staff/{id}"));
}

async Task CreateStaffAsync()
{
    string name = ReadText("Name: ");
    int armyNumber = ReadInt("Army number: ");
    bool isManager = ReadBool("Is manager (true/false): ");

    AddStaffRequest request = new(name, armyNumber, isManager);
    await SendAsync(() => httpClient.PostAsJsonAsync("/staff/", request, jsonOptions));
}

async Task UpdateStaffAsync()
{
    int id = ReadInt("Id сотрудника: ");
    Console.WriteLine("Оставьте поле пустым, чтобы не менять его.");

    string? name = ReadOptionalText("Name: ");
    int? armyNumber = ReadOptionalInt("Army number: ");
    bool? isManager = ReadOptionalBool("Is manager (true/false): ");

    if (name == null && armyNumber == null && isManager == null)
    {
        Console.WriteLine("Необходимо указать хотя бы одно поле для обновления.");
        return;
    }

    UpdateStaffRequest request = new(name, armyNumber, isManager);
    await SendAsync(() => httpClient.PutAsJsonAsync($"/staff/{id}", request, jsonOptions));
}

async Task DeleteStaffAsync()
{
    int id = ReadInt("Id сотрудника: ");
    await SendAsync(() => httpClient.DeleteAsync($"/staff/{id}"));
}

List<StaffMemberRequest> ReadExecutors()
{
    int count = ReadNonNegativeInt("Количество исполнителей: ");
    List<StaffMemberRequest> executors = new();

    for (int index = 1; index <= count; index++)
    {
        Console.WriteLine($"Исполнитель {index}:");
        int id = ReadInt("  Id: ");
        string name = ReadText("  Name: ");
        int armyNumber = ReadInt("  Army number: ");
        bool isManager = ReadBool("  Is manager (true/false): ");
        executors.Add(new StaffMemberRequest(id, name, armyNumber, isManager));
    }

    return executors;
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

int ReadNonNegativeInt(string prompt)
{
    while (true)
    {
        int value = ReadInt(prompt);
        if (value >= 0)
        {
            return value;
        }

        Console.WriteLine("Введите неотрицательное число.");
    }
}

int? ReadOptionalInt(string prompt)
{
    while (true)
    {
        string? input = ReadOptionalText(prompt);
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        if (int.TryParse(input, out int value))
        {
            return value;
        }

        Console.WriteLine("Введите целое число или оставьте поле пустым.");
    }
}

DateOnly ReadDate(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (DateOnly.TryParse(Console.ReadLine(), out DateOnly value))
        {
            return value;
        }

        Console.WriteLine("Введите дату в формате yyyy-MM-dd.");
    }
}

DateOnly? ReadOptionalDate(string prompt)
{
    while (true)
    {
        string? input = ReadOptionalText(prompt);
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        if (DateOnly.TryParse(input, out DateOnly value))
        {
            return value;
        }

        Console.WriteLine("Введите дату в формате yyyy-MM-dd или оставьте поле пустым.");
    }
}

bool ReadBool(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (bool.TryParse(Console.ReadLine(), out bool value))
        {
            return value;
        }

        Console.WriteLine("Введите true или false.");
    }
}

bool? ReadOptionalBool(string prompt)
{
    while (true)
    {
        string? input = ReadOptionalText(prompt);
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }
        if (bool.TryParse(input, out bool value))
        {
            return value;
        }

        Console.WriteLine("Введите true, false или оставьте поле пустым.");
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
record AddTaskRequest(string Title, string Description, List<StaffMemberRequest> Executor, int HeliId, int ShiftId);
record UpdateTaskRequest(string? Title = null, string? Description = null, bool? IsDone = null);
record StaffMemberRequest(int Id, string Name, int ArmyNumber, bool IsManager);
record AddShiftRequest(DateOnly Date, int ManagerId);
record UpdateShiftRequest(DateOnly? Date = null, int? ManagerId = null);
record AddStaffRequest(string Name, int ArmyNumber, bool IsManager);
record UpdateStaffRequest(string? Name = null, int? ArmyNumber = null, bool? IsManager = null);
