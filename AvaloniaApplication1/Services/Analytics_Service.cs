using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaApplication1.Interfaces;
using AvaloniaApplication1.Models;
using Microsoft.Extensions.Logging;
using Data;

namespace AvaloniaApplication1.Services;

public class Analytics_Services : IAnalytics_Service
{
    private readonly ClinicDatabase _db;
    private readonly INativeWrapper _nativeWrapper;
    private readonly ILogger<IAnalytics_Service> _logger;

    public Analytics_Services(
        INativeWrapper nativeWrapper,
        ILogger<IAnalytics_Service> logger)
    {
        _db = ClinicDatabase.GetDatabase();
        _nativeWrapper = nativeWrapper;
        _logger = logger;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        try
        {
            // У тебя нет нормальных методов → используем строки
            var appointmentsRaw = _db.GetAppointments();

            var today = DateTime.Today;

            var todayAppointments = appointmentsRaw
                .Where(a => a.Contains(today.ToString("yyyy")))
                .ToList();

            // Заглушки (потому что у тебя нет методов получения пациентов/платежей)
            int totalPatients = 0;
            decimal revenue = 0;

            var busiestDoctor = new Doctor_Work_load("Не реализовано", 0);

            return new DashboardStats(
                Total_Patients: totalPatients,
                Today_Appointments: todayAppointments.Count,
                Monthly_Revenue: revenue,
                Most_Popular_Service: "Не реализовано",
                Busiest_Doctor: busiestDoctor
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении статистики дашборда");
            throw;
        }
    }

    public Task<List<Service_Stats>> GetTopServicesAsync(int count = 5)
    {
        // У тебя нет метода получения услуг → пока заглушка
        return Task.FromResult(new List<Service_Stats>());
    }

    public Task<List<Doctor_Work_load>> GetDoctorWorkloadAsync(DateTime date)
    {
        var appointments = _db.GetAppointments();

        var result = appointments
            .Where(a => a.Contains(date.ToString("yyyy")))
            .GroupBy(a => a) // костыль, потому что у тебя строки
            .Select(g => new Doctor_Work_load(g.Key, g.Count()))
            .ToList();

        return Task.FromResult(result);
    }

    public Task<decimal> GetRevenueAsync(DateTime start, DateTime end)
    {
        // Нет доступа к платежам → заглушка
        return Task.FromResult(0m);
    }
}