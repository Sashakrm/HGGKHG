using AvaloniaApplication1.Interfaces;
using AvaloniaApplication1.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace AvaloniaApplication1.Services;

public class Analytics_Service : IAnalytics_Service
{
    private readonly ClinicDatabase _db;
    private readonly INativeWrapper _native_wrapper;
    private readonly ILogger<Analytics_Service> _logger;

    public Analytics_Service(
        INativeWrapper native_wrapper,
        ILogger<Analytics_Service> logger)
    {
        _db = ClinicDatabase.GetDatabase();
        _native_wrapper = native_wrapper;
        _logger = logger;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        try
        {
            var today = DateTime.Today;
            
            var appointments = _db.GetAppointments();
            var today_appointments = appointments
                .Where(a => a.DateTime.Date == today)
                .ToList();

            var patients_count = 0;
            var payments = new List<object>();
            var services = new List<object>();

            var raw_data = new RawData(today_appointments, payments, services);
            var analytics = await _native_wrapper.CalculateAnalyticsAsync(raw_data);

            var busiest_doctor = new Doctor_Workload("Нет данных", 0);

            return new DashboardStats(
                Total_Patients: patients_count,
                Today_Appointments: today_appointments.Count,
                Monthly_Revenue: analytics.TotalRevenue,
                Most_Popular_Service: analytics.MostPopularService,
                Busiest_Doctor: busiest_doctor
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
        var result = new List<Service_Stats>();
        return Task.FromResult(result);
    }

    public Task<List<Doctor_Workload>> GetDoctorWorkloadAsync(DateTime date)
    {
        var appointments = _db.GetAppointments();

        var result = appointments
            .Where(a => a.DateTime.Date == date)
            .GroupBy(a => a.DoctorId)
            .Select(g => new Doctor_Workload(g.Key.ToString(), g.Count()))
            .ToList();

        return Task.FromResult(result);
    }

    public Task<decimal> GetRevenueAsync(DateTime start, DateTime end)
    {
        return Task.FromResult(0m);
    }
}