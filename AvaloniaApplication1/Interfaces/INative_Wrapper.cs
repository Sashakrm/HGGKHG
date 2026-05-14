using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaApplication1.Models;
using AvaloniaApplication1.Services;

namespace AvaloniaApplication1.Interfaces;

public interface INativeWrapper
{
    Task<AnalyticsResult> CalculateAnalyticsAsync(RawData data);
    Task<string> EncryptDataAsync(string data);
}

public record RawData(
    List<Appointment> Appointments,
    List<Payment> Payments
    //List<Services> Services
);

public record AnalyticsResult(
    string MostPopularService,
    decimal TotalRevenue,
    string BusiestDoctor,
    double AverageCheck
);