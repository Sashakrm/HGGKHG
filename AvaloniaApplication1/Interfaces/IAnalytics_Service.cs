using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace AvaloniaApplication1.Interfaces
{

    public interface IAnalytics_Service
    {
        // Данные для дашборда
        Task<DashboardStats> GetDashboardStatsAsync();

        // Топ услуг
        Task<List<Service_Stats>> GetTopServicesAsync(int count = 5);

        // Загруженность врачей
        Task<List<Doctor_Work_load>> GetDoctorWorkloadAsync(DateTime date);

        // Доход по периодам
        Task<decimal> GetRevenueAsync(DateTime start, DateTime end);
    }

// DTO для передачи данных в UI
    public record DashboardStats(
        int Total_Patients,
        int Today_Appointments,
        decimal Monthly_Revenue,
        string Most_Popular_Service,
        Doctor_Work_load Busiest_Doctor
    );

    public record Service_Stats(string Service_Name, int Count, decimal Revenue);

    public record Doctor_Work_load(string Doctor_Name, int Appointments_Count);
}