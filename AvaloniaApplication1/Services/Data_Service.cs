using System;
using AvaloniaApplication1.Interfaces;
using AvaloniaApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using AvaloniaApplication1.Models;
/*
namespace AvaloniaApplication1.Services;

public class DataService : IDataService
{
    private readonly ClinicDatabase _context;

    public DataService(ClinicDatabase context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetAllPatientsAsync()
    {
        return await _context.Patient.ToListAsync();
    }

    public async Task<Patient?> GetPatientByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<bool> AddPatientAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdatePatientAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return false;
        
        _context.Patients.Remove(patient);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Doctor>> GetAllDoctorsAsync()
    {
        return await _context.Doctors.ToListAsync();
    }

    public async Task<bool> AddDoctorAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateDoctorAsync(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Appointment>> GetAppointmentsAsync(DateTime date)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.Service)
            .Where(a => a.Date.Date == date.Date)
            .ToListAsync();
    }

    public async Task<bool> AddAppointmentAsync(Appointment appointment)
    {
        await _context.Appointments.AddAsync(appointment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<List<Appointment>> GetPatientQueueAsync(int doctorId, DateTime date)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Service)
            .Where(a => a.DoctorId == doctorId && a.Date.Date == date.Date)
            .OrderBy(a => a.Time)
            .ToListAsync();
    }

    public async Task<List<Service>> GetAllServicesAsync()
    {
        return await _context.Services.ToListAsync();
    }
}*/