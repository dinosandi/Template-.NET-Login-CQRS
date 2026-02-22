using System.Text;
using System.Text.Json;
using ComponentManagement.Application.Interfaces;
using ComponentManagement.Domain.Entities;
using ComponentManagement.Domain.Enums;
using System.Linq;

public class FonnteWhatsappNotificationService :
    IWhatsappNotificationService,
    ILifetimeNotificationService
{
    private readonly HttpClient _httpClient;
    private const string FONNTE_URL = "https://api.fonnte.com/send";
    private const string TARGET = "120363424090327583@g.us";
    private const string AUTH_TOKEN = "uwP1gxXMLGxQUQMEBa4z";

    public FonnteWhatsappNotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    // ==========================
    // STATUS CHANGE
    // ==========================
    public async Task SendComponentStatusNotificationAsync(PartComponent component)
    {
        var message = BuildMessageByStatus(component);
        if (string.IsNullOrEmpty(message))
            return;

        await SendAsync(message);
    }

    // ==========================
    // NEED APL (FINAL & AGGREGATE)
    // ==========================
   public async Task SendComponentNeedAplNotificationAsync(PartComponent component)
{
    var tanggal = DateTime.UtcNow
            .ToLocalTime()
            .ToString("dddd, dd MMMM yyyy");

    var aplListBuilder = new StringBuilder();
    int index = 1;

    var apls = component.PartComponentAPLs
        .Where(x => x.APL != null)
        .Select(x => x.APL)
        .DistinctBy(x => x.Id)
        .OrderBy(x => x.NameBrand)
        .ToList();

    foreach (var apl in apls)
    {
        aplListBuilder.AppendLine($"*{apl.NameBrand}*");

        if (apl.Parts == null || !apl.Parts.Any())
        {
            aplListBuilder.AppendLine("  - (Belum ada Part APL)");
            aplListBuilder.AppendLine();
            continue;
        }

        foreach (var part in apl.Parts.OrderBy(p => p.PartNumber))
        {
            aplListBuilder.AppendLine(
                $"{index}. {part.PartNumber} - {part.Description} (Qty: {part.Quantity})"
            );
            index++;
        }

        aplListBuilder.AppendLine();
    }

    var message = $"""
🔔 *NOTIFIKASI KOMPONEN*

Detail Komponen:
• Komponen    : {component.NamaBrand}
• EGI         : {component.NamaKomponen}
• Part Number : {component.PartNumber}

telah masuk ke daftar barang *Minimex* dengan status *WORK IN PROGRESS (Need APL)* pada tanggal *{tanggal}*.
Komponen akan disiapkan As Soon As Possible agar dapat dilakukan pemasangan kembali sesuai plan.
Mohon untuk lengkapi *APL* berikut untuk melakukan proses *Rebuilding*:

{aplListBuilder}
https://prime-kppmining.com/Component/Detail/{component.Id}
""";

    await SendAsync(message);
}


   

    public async Task SendFabricationRequestQrAsync(PartComponent component)
    {
        var tanggal = DateTime.UtcNow
            .ToLocalTime()
            .ToString("dd MMMM yyyy");

        var qrLink = $"https://prime-kppmining.com/Component/{component.QrToken}";

        var message = $"""
🔧 *FABRICATION REQUEST*

Detail Komponen:
• Komponen    : {component.NamaBrand}
• EGI         : {component.NamaKomponen}
• Part Number : {component.PartNumber}
• No Lambung  : {component.NomerLaMbung}

Komponen *{component.NamaBrand}* Egi *{component.NamaKomponen}* Part Number *{component.PartNumber}* telah masuk ke daftar barang *Minimex* dengan status *Work In Progress (Need FR)* pada tanggal *{tanggal}* Komponen memerlukan FR ( Fabrication Request) agar dapat dilakukan pemasangan Kembali sesuai plan.
.

Silakan gunakan QR berikut untuk proses Fabrication:

🔗 {qrLink}
""";

        await SendAsync(message);
    }


    // =====================================================
    // LIFETIME MONITORING (BACKGROUND SERVICE)
    // =====================================================
   public async Task SendLifetimeWarningAsync(
    ComponentLifetime lifetime,
    int threshold,
    CancellationToken cancellationToken)
{
    var component = lifetime.PartComponent;
    if (component == null) return;

    var remaining = lifetime.CalculateRemainingHm(DateTime.UtcNow);
    var remainingRounded = (int)Math.Floor(remaining);

  var unitName = component.Unit?.NameUnit ?? "-";

    var message = $"""
⚠️ *LIFETIME WARNING*

Detail Komponen:
• Komponen : {component.NamaKomponen}
• Brand    : {component.NamaBrand}
• Part No  : {component.PartNumber}
• Unit     : {unitName}

Remaining Lifetime : *{remainingRounded} HM*
Threshold          : *{threshold} HM*

Mohon untuk melakukan planning dan persiapan ketersediaan komponen untuk dilakukan penggantian.
Terima kasih.😊

*PLANT BDMA PRECISSION MAINTENANCE* 🔥
""";

    await SendAsync(message, cancellationToken);
}

    // =====================================================
    // PRIVATE HTTP SENDER (SATU-SATUNYA)
    // =====================================================
  private async Task SendAsync(
    string message,
    CancellationToken cancellationToken = default)
{
    try
    {
        var payload = new
        {
            target = TARGET,
            message = message
        };

        var request = new HttpRequestMessage(HttpMethod.Post, FONNTE_URL);
        request.Headers.Add("Authorization", AUTH_TOKEN);
        request.Content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        Console.WriteLine("===== FONNTE SEND =====");
        Console.WriteLine($"TARGET  : {TARGET}");
        Console.WriteLine("MESSAGE :");
        Console.WriteLine(message);
        Console.WriteLine("======================");

        var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        Console.WriteLine("===== FONNTE RESPONSE =====");
        Console.WriteLine($"STATUS : {(int)response.StatusCode}");
        Console.WriteLine($"BODY   : {body}");
        Console.WriteLine("==========================");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Fonnte Error: {body}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("===== FONNTE ERROR =====");
        Console.WriteLine(ex);
        Console.WriteLine("=======================");
        throw;
    }
}


    public async Task SendInstallationRequestNotificationAsync(
    PartComponent component,
    Unit unit)
    {
        var tanggal = DateTime.UtcNow
            .ToLocalTime()
            .ToString("dddd, dd MMMM yyyy");

        var approvalLink =
            $"https://prime-kppmining.com/LifetimeInstalledComponent/";

        var message = $"""
🔔 *INSTALLATION REQUEST*

Terdapat permintaan instalasi komponen pada:
📅 *{tanggal}*

Detail Komponen:
• Komponen    : {component.NamaBrand}
• EGI         : {component.NamaKomponen}
• Part Number : {component.PartNumber}
• Unit        : {unit.NameUnit}

Alasan:
{component.Note ?? "- Tidak ada keterangan -"}

Silakan login untuk melakukan approval:
🔗 {approvalLink}
""";

        await SendAsync(message);
    }




    public async Task SendUpdateStatusComponentAsync(PartComponent component)
    {
        var tanggal = DateTime.UtcNow
            .ToLocalTime()
            .ToString("dddd, dd MMMM yyyy");

        string message = component.Status switch
        {
            ComponentStatus.RFU => $"""
✅ *STATUS UPDATE READY FOR USE*

Komponen:
• Component Group : {component.NamaBrand}
• EGI             : {component.NamaKomponen}
• Part Number     : {component.PartNumber}

telah masuk ke daftar barang *Minimex* dengan status *Ready For Use* pada hari, tanggal *{tanggal}*.

Silahkan lanjutkan untuk proses *plan pemasangan*.

https://prime-kppmining.com/Component/Detail/{component.Id}

PLANT BDMA 🔥
""",

            ComponentStatus.WIP => $"""
🛠️ *STATUS UPDATE WORK IN PROGRESS (REBUILDING)*

Komponen:
• Component Group : {component.NamaBrand}
• EGI             : {component.NamaKomponen}
• Part Number     : {component.PartNumber}

telah masuk ke daftar barang *Minimex* dengan status *Work In Progress (Rebuilding)* pada hari, tanggal *{tanggal}*.
Komponen akan disiapkan *As Soon As Possible* agar dapat dilakukan pemasangan kembali sesuai plan.

https://prime-kppmining.com/Component/Detail/{component.Id}

PLANT BDMA 🔥
""",

            _ => null
        };

        if (!string.IsNullOrEmpty(message))
        {
            await SendAsync(message);
        }
    }


    private string BuildMessageByStatus(PartComponent component)
    {
        var tanggal = component.CreatedAt
            .ToLocalTime()
            .ToString("dd MMMM yyyy");

        return component.Status switch
        {
            ComponentStatus.RFU => $"""
🔔 *NOTIFIKASI KOMPONEN*

Detail Komponen:
• Komponen    : {component.NamaBrand}
• EGI         : {component.NamaKomponen}
• Part Number : {component.PartNumber}

telah masuk ke daftar barang *Minimex* dengan status **READY FOR USE** pada tanggal *{tanggal}*.

https://prime-kppmining.com/Component/Detail/{component.Id}
""",

            ComponentStatus.WIP => $"""
🔔 *NOTIFIKASI KOMPONEN*

Detail Komponen:
• Komponen    : {component.NamaBrand}
• EGI         : {component.NamaKomponen}
• Part Number : {component.PartNumber}

telah masuk ke daftar barang *Minimex* dengan status **WORK IN PROGRESS (Rebuilding)** pada tanggal *{tanggal}*.

https://prime-kppmining.com/Component/Detail/{component.Id}
""",

            _ => string.Empty
        };
    }
}
