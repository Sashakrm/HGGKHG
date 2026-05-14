using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvaloniaApplication1.Models
{
    public class Appointment : INotifyPropertyChanged
    {
        private int _id;
        private int _patientId;
        private int _doctorId;
        private int _serviceId;
        private DateTime _date;
        private TimeSpan _time;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public int PatientId
        {
            get => _patientId;
            set { _patientId = value; OnPropertyChanged(); }
        }

        public int DoctorId
        {
            get => _doctorId;
            set { _doctorId = value; OnPropertyChanged(); }
        }

        public int ServiceId
        {
            get => _serviceId;
            set { _serviceId = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(); }
        }

        public TimeSpan Time
        {
            get => _time;
            set { _time = value; OnPropertyChanged(); }
        }

        // Конструктор по умолчанию
        public Appointment() { }

        // Конструктор для создания объекта из данных
        public Appointment(int id, int patientId, int doctorId, int serviceId, DateTime date, TimeSpan time)
        {
            Id = id;
            PatientId = patientId;
            DoctorId = doctorId;
            ServiceId = serviceId;
            Date = date;
            Time = time;
        }

        // Метод для создания Appointment из зашифрованной строки БД
        // ЗАМЕНИТЕ этот метод на вашу реальную логику дешифровки
        public static Appointment FromEncryptedString(string encryptedData)
        {
           /* // Пример: раскомментируйте и адаптируйте под ваш формат
             var decrypted = Decrypt(encryptedData);
             var parts = decrypted.Split(';');
             return new Appointment(
                 int.Parse(parts[0]),
                 int.Parse(parts[1]),
                 int.Parse(parts[2]),
                 int.Parse(parts[3]),
                 DateTime.Parse(parts[4]),
               TimeSpan.Parse(parts[5])
             );*/
            
            // Заглушка - удалите когда добавите реальную дешифровку
            throw new NotImplementedException("Реализуйте дешифровку строки из БД");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}