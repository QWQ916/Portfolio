using System.Text.Json;

// Мок боевого API для локального тестирования приложения мониторинга.
// Auth по логину/паролю с выдачей сессионной куки, дальше эндпоинты требуют эту куку.
// Данные генерируются один раз при старте, статистика в метриках увязана со списками.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var jsonOpts = new JsonSerializerOptions { PropertyNamingPolicy = null };
var gen = new Random(42);              // детерминированная генерация списков
var sessions = new HashSet<string>();

const string Login = "admin";
const string Password = "admin";

// ===================== Генерация данных =====================

var clusterNames = new[] { "CL-PROD-01", "CL-PROD-02", "CL-TEST-01", "CL-DMZ-01", "CL-BACKUP-01" };

// --- Кластеры (5) ---
var clusterItems = clusterNames.Select((c, i) => (object)new
{
    name = c,
    vm_count = gen.Next(20, 140),
    node_count = gen.Next(3, 14),
    storage_name = "vstor-" + c.ToLower(),
    created = new DateTime(2025, 3, 1).AddDays(i * 20)
}).ToList();

// --- Шаблоны ОС (20) ---
var templateDefs = new (string name, string desc, string ver)[]
{
    ("Windows Server 2022 Std",     "Базовый образ AD/DNS",       "10.0.20348"),
    ("Windows Server 2019 Std",     "Файловый сервер",            "10.0.17763"),
    ("Windows Server 2016",         "Legacy-приложения",          "10.0.14393"),
    ("Windows 11 Pro",              "Рабочее место (VDI)",        "10.0.22631"),
    ("Windows 10 Enterprise LTSC",  "VDI LTSC",                   "10.0.19044"),
    ("Ubuntu Server 24.04 LTS",     "nginx + docker",             "24.04"),
    ("Ubuntu Server 22.04 LTS",     "Kubernetes node",            "22.04"),
    ("Ubuntu Server 20.04 LTS",     "CI/CD агент",                "20.04"),
    ("Debian 12",                   "Образ под PostgreSQL",       "12.5"),
    ("Debian 11",                   "Легаси-сервисы",             "11.9"),
    ("AstraLinux SE 1.7",           "Сертифицированный образ",    "1.7.5"),
    ("AstraLinux SE 1.8",           "Защищённый контур",          "1.8.1"),
    ("RED OS 7.3",                  "Импортозамещение",           "7.3.4"),
    ("ALT Linux 10",                "Рабочая станция",            "10.2"),
    ("CentOS Stream 9",             "Веб-сервер Apache",          "9.0"),
    ("Rocky Linux 9",               "Замена CentOS",              "9.3"),
    ("RHEL 9",                      "Корпоративный Linux",        "9.4"),
    ("openSUSE Leap 15.5",          "Сервер приложений",          "15.5"),
    ("FreeBSD 14",                  "Сетевой шлюз",               "14.0"),
    ("Alpine Linux 3.19",           "Контейнерная база",          "3.19"),
};
var templateItems = templateDefs.Select((t, i) => (object)new
{
    name = t.name,
    description = t.desc,
    guest_os_version = t.ver,
    creation_date = new DateTime(2025, 1, 1).AddDays(i * 9)
}).ToList();

// --- Хосты: 59 онлайн + 4 оффлайн ---
var hostItems = new List<object>();
int onlineNodes = 0;

for (int i = 1; i <= 59; i++)
{
    var cl = clusterNames[i % clusterNames.Length];
    hostItems.Add(new
    {
        ip_address = $"10.10.{(i / 60) + 1}.{(i % 60) + 10}",
        os = "Astra Linux",
        hostname = $"node-{i:D2}",
        arch = "x86_64",
        status = "ONLINE",
        sdk_status = "AVAILABLE",
        cluster_name = cl,
        vstorage_cluster_name = "vstor-" + cl.ToLower()
    });
    onlineNodes++;
}
for (int i = 60; i <= 63; i++)   // оффлайн-хосты
{
    var cl = clusterNames[i % clusterNames.Length];
    hostItems.Add(new
    {
        ip_address = $"10.10.9.{i}",
        os = "Astra Linux",
        hostname = $"node-{i:D2}",
        arch = "x86_64",
        status = "OFFLINE",
        sdk_status = "UNAVAILABLE",
        cluster_name = cl,
        vstorage_cluster_name = "vstor-" + cl.ToLower()
    });
}

// --- Виртуальные машины (статистика увязана со списком) ---
var vmGuest = new (string os, string ver)[]
{
    ("Windows Server 2022", "10.0.20348"),
    ("Debian 12", "12.5"),
    ("Ubuntu 24.04", "24.04"),
    ("AstraLinux SE 1.7", "1.7.5"),
};
var cores = new[] { 2, 4, 8, 16 };
var rams = new[] { 4096, 8192, 16384, 32768, 65536 };

var vmItems = new List<object>();
int running = 0, stopped = 0, suspended = 0, paused = 0, error = 0;

for (int i = 1; i <= 120; i++)
{
    int r = gen.Next(100);
    string st;
    if (r < 45) { st = "RUNNING"; running++; }
    else if (r < 85) { st = "STOPPED"; stopped++; }
    else if (r < 90) { st = "SUSPENDED"; suspended++; }
    else if (r < 95) { st = "PAUSED"; paused++; }
    else { st = "ERROR"; error++; }

    var g = vmGuest[gen.Next(vmGuest.Length)];
    int nodeId = gen.Next(1, 60);

    vmItems.Add(new
    {
        hostname = $"vm-{i:D3}",
        status = st,
        ip_address = $"10.20.{(i / 250) + 1}.{(i % 250) + 2}",
        cores_count = cores[gen.Next(cores.Length)],
        guest_os_version = g.ver,
        guest_os = g.os,
        location = gen.Next(2) == 0 ? "MSK-DC1" : "MSK-DC2",
        ram_size = rams[gen.Next(rams.Length)],
        node = new { node_id = nodeId, name = $"node-{nodeId:D2}" }
    });
}

// ===================== Эндпоинты =====================

bool IsAuthed(HttpContext ctx) =>
    ctx.Request.Cookies.TryGetValue("session", out var t) && sessions.Contains(t);

app.MapPost("/api/0/auth", async (HttpContext ctx) =>
{
    Creds creds;
    try { creds = await ctx.Request.ReadFromJsonAsync<Creds>(); }
    catch { return Results.BadRequest(); }

    if (creds?.login == Login && creds?.password == Password)
    {
        var token = Guid.NewGuid().ToString("N");
        sessions.Add(token);
        ctx.Response.Cookies.Append("session", token, new CookieOptions { HttpOnly = true, Path = "/" });
        Console.WriteLine($"[AUTH] OK   session={token}");
        return Results.Ok(new { status = "ok" });
    }

    Console.WriteLine("[AUTH] FAIL 401 (неверный логин/пароль)");
    return Results.Unauthorized();
});

app.MapGet("/api/0/template", (HttpContext ctx) =>
    IsAuthed(ctx)
        ? Results.Json(new { template_list = new { total = templateItems.Count, items = templateItems } }, jsonOpts)
        : Results.Unauthorized());

app.MapGet("/api/0/cluster", (HttpContext ctx) =>
    IsAuthed(ctx)
        ? Results.Json(new { cluster_list = new { total = clusterItems.Count, items = clusterItems } }, jsonOpts)
        : Results.Unauthorized());

app.MapGet("/api/0/node", (HttpContext ctx) =>
    IsAuthed(ctx)
        ? Results.Json(new { node_list = new { total = hostItems.Count, items = hostItems } }, jsonOpts)
        : Results.Unauthorized());

app.MapGet("/api/0/vm", (HttpContext ctx) =>
    IsAuthed(ctx)
        ? Results.Json(new { vm_list = new { total = vmItems.Count, items = vmItems } }, jsonOpts)
        : Results.Unauthorized());

// Метрики: CPU/память случайные, диск фиксированные 34%, счётчики увязаны со списками
app.MapGet("/api/0/monitoring/infrastructure", (HttpContext ctx) =>
{
    if (!IsAuthed(ctx)) return Results.Unauthorized();

    const int part = 34;                      // диск = 34%
    const int mem = 85;                        // оперативка = 85%
    const int cpu = 67;                        // CPU = 67%
    double memTotal = 2_097_152.0;

    var payload = new
    {
        metrics = new
        {
            clusters_count = clusterItems.Count,
            metrics = new
            {
                combined_partition_usage_percent = part,
                combined_partition_usage_mb = Math.Round(147_111_626.39 * part / 100.0, 2),
                combined_partition_total_mb = 147_111_626.39,
                combined_memory_total_mb = memTotal,
                combined_memory_free_mb = Math.Round(memTotal * (100 - mem) / 100.0, 2),
                combined_memory_used_mb = Math.Round(memTotal * mem / 100.0, 2),
                combined_memory_used_percent = mem,
                cpu_usage_percent = cpu,
                combined_swap_total_mb = 65_536.0,
                combined_swap_free_mb = 60_000.0,
                combined_swap_used_mb = 5_536.0,
                combined_swap_used_percent = 8.4
            },
            node_statistics = new { ONLINE = onlineNodes },                       // = число онлайн-хостов
            vm_statistics = new { RUNNING = running, STOPPED = stopped, SUSPENDED = suspended, PAUSED = paused, ERROR = error }
        }
    };
    return Results.Json(payload, jsonOpts);
});

Console.WriteLine("=== Mock API: http://localhost:5005  (login: admin / password: admin) ===");
Console.WriteLine($"Шаблоны: {templateItems.Count} | Кластеры: {clusterItems.Count} | Хосты: {hostItems.Count} (онлайн {onlineNodes}) | ВМ: {vmItems.Count}");
app.Run("http://localhost:5005");


// Тело запроса авторизации
class Creds
{
    public string login { get; set; }
    public string password { get; set; }
}
