using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using AvaloniaApplication1.Models;
namespace AvaloniaApplication1.Interfaces;
using AvaloniaApplication1.Services;

public interface IDataService
{
    // Пациенты
    Task<List<Patient>> GetAllPatientsAsync();
    Task<Patient?> GetPatientByIdAsync(int id);
    Task<bool> AddPatientAsync(Patient patient);
    Task<bool> UpdatePatientAsync(Patient patient);
    Task<bool> DeletePatientAsync(int id);
    
    // Врачи
    Task<List<Doctor>> GetAllDoctorsAsync();
    Task<bool> AddDoctorAsync(Doctor doctor);
    Task<bool> UpdateDoctorAsync(Doctor doctor);
    
    // Записи
    Task<List<Appointment>> GetAppointmentsAsync(DateTime date);
    Task<bool> AddAppointmentAsync(Appointment appointment);
    Task<List<Appointment>> GetPatientQueueAsync(int doctorId, DateTime date);
    
    // Услуги
    //Task<List<Services>> GetAllServicesAsync();
}