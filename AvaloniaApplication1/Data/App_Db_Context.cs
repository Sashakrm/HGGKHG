using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Data
{

    /*
    ==========================================================
    МОДЕЛИ ДАННЫХ
    ==========================================================

    Это простые структуры данных (records).
    Они используются только для передачи данных из базы.
    */

    public record Doctor(
        int Id,
        string FullName,
        string Specialization,
        string Schedule
    );

    public record Patient(
        int Id,
        string FullName,
        DateTime BirthDate,
        string Contacts
    );

    public record Service(
        int Id,
        string Name,
        decimal Price,
        int DurationMinutes
    );

    public record Appointment(
        int Id,
        int PatientId,
        int DoctorId,
        int ServiceId,
        DateTime DateTime
    );

    public record Invoice(
        int Id,
        int AppointmentId,
        decimal Amount,
        DateTime CreatedAt,
        DateTime PayUntil,
        bool Paid,
        DateTime? PaidAt
    );



    /*
    ==========================================================
    ГЛАВНЫЙ КЛАСС БАЗЫ ДАННЫХ
    ==========================================================

    sealed — запрещает наследование
    private constructor — запрещает создание извне
    Singleton — в программе будет только ОДИН объект базы
    */

    public sealed class ClinicDatabase
    {

        private static ClinicDatabase? _instance;

        private readonly string _connectionString;



        /*
        ------------------------------------------------------
        КОНСТРУКТОР
        ------------------------------------------------------

        private — нельзя вызвать извне
        */

        private ClinicDatabase(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            InitializeDatabase();
        }



        /*
        ------------------------------------------------------
        ПОЛУЧЕНИЕ ЭКЗЕМПЛЯРА БАЗЫ
        ------------------------------------------------------

        Это единственный способ получить доступ к базе
        */

        public static ClinicDatabase GetDatabase(string path = "clinic.db")
        {
            if (_instance == null)
                _instance = new ClinicDatabase(path);

            return _instance;
        }



        /*
        ======================================================
        ИНИЦИАЛИЗАЦИЯ БАЗЫ
        ======================================================

        Создает таблицы если они еще не существуют
        */

        private void InitializeDatabase()
        {
            using var conn = new SqliteConnection(_connectionString);

            conn.Open();

            var cmd = conn.CreateCommand();

            cmd.CommandText =
            @"
            PRAGMA foreign_keys = ON;

            CREATE TABLE IF NOT EXISTS doctors(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                fullname TEXT NOT NULL,
                specialization TEXT,
                schedule TEXT
            );

            CREATE TABLE IF NOT EXISTS patients(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                fullname TEXT NOT NULL,
                birthdate TEXT,
                contacts TEXT
            );

            CREATE TABLE IF NOT EXISTS services(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                price REAL NOT NULL,
                duration INTEGER
            );

            CREATE TABLE IF NOT EXISTS appointments(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                patient_id INTEGER,
                doctor_id INTEGER,
                service_id INTEGER,
                datetime TEXT,

                FOREIGN KEY(patient_id) REFERENCES patients(id),
                FOREIGN KEY(doctor_id) REFERENCES doctors(id),
                FOREIGN KEY(service_id) REFERENCES services(id),

                UNIQUE(doctor_id, datetime)
            );

            CREATE TABLE IF NOT EXISTS invoices(
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                appointment_id INTEGER,
                amount REAL,
                created_at TEXT,
                pay_until TEXT,
                paid INTEGER DEFAULT 0,
                paid_at TEXT,

                FOREIGN KEY(appointment_id) REFERENCES appointments(id)
            );
            ";

            cmd.ExecuteNonQuery();
        }



        /*
        ======================================================
        ДОБАВЛЕНИЕ ВРАЧА
        ======================================================
        */

        public string AddDoctor(string name, string specialization, string schedule)
        {
            try
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();

                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"INSERT INTO doctors(fullname,specialization,schedule)
                  VALUES($name,$spec,$sch)";

                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$spec", specialization);
                cmd.Parameters.AddWithValue("$sch", schedule);

                cmd.ExecuteNonQuery();

                return "OK: Doctor added";
            }
            catch (Exception e)
            {
                return $"ERROR: {e.Message}";
            }
        }



        /*
        ======================================================
        ДОБАВЛЕНИЕ ПАЦИЕНТА
        ======================================================
        */

        public string AddPatient(string name, DateTime birth, string contacts)
        {
            try
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();

                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"INSERT INTO patients(fullname,birthdate,contacts)
                  VALUES($name,$birth,$contacts)";

                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$birth", birth);
                cmd.Parameters.AddWithValue("$contacts", contacts);

                cmd.ExecuteNonQuery();

                return "OK: Patient added";
            }
            catch (Exception e)
            {
                return $"ERROR: {e.Message}";
            }
        }



        /*
        ======================================================
        ДОБАВЛЕНИЕ УСЛУГИ
        ======================================================
        */

        public string AddService(string name, decimal price, int duration)
        {
            try
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();

                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"INSERT INTO services(name,price,duration)
                  VALUES($name,$price,$duration)";

                cmd.Parameters.AddWithValue("$name", name);
                cmd.Parameters.AddWithValue("$price", price);
                cmd.Parameters.AddWithValue("$duration", duration);

                cmd.ExecuteNonQuery();

                return "OK: Service added";
            }
            catch (Exception e)
            {
                return $"ERROR: {e.Message}";
            }
        }



        /*
        ======================================================
        СОЗДАНИЕ ЗАПИСИ К ВРАЧУ
        ======================================================

        Логика:
        1 создается запись
        2 создается чек
        3 возвращается номер чека

        Всё делается внутри транзакции
        */

        public string CreateAppointment(
            int patientId,
            int doctorId,
            int serviceId,
            DateTime time)
        {
            using var conn = new SqliteConnection(_connectionString);

            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {

                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"INSERT INTO appointments(patient_id,doctor_id,service_id,datetime)
                  VALUES($p,$d,$s,$t)";

                cmd.Parameters.AddWithValue("$p", patientId);
                cmd.Parameters.AddWithValue("$d", doctorId);
                cmd.Parameters.AddWithValue("$s", serviceId);
                cmd.Parameters.AddWithValue("$t", time);

                cmd.ExecuteNonQuery();

                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT last_insert_rowid()";

                long appointmentId = (long)cmd.ExecuteScalar();



                cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT price FROM services WHERE id=$id";

                cmd.Parameters.AddWithValue("$id", serviceId);

                decimal price = Convert.ToDecimal(cmd.ExecuteScalar());



                cmd = conn.CreateCommand();

                cmd.CommandText =
                @"INSERT INTO invoices(appointment_id,amount,created_at,pay_until)
                  VALUES($a,$price,$now,$deadline)";

                cmd.Parameters.AddWithValue("$a", appointmentId);
                cmd.Parameters.AddWithValue("$price", price);
                cmd.Parameters.AddWithValue("$now", DateTime.Now);
                cmd.Parameters.AddWithValue("$deadline", DateTime.Now.AddHours(2));

                cmd.ExecuteNonQuery();

                cmd.ExecuteNonQuery();

                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT last_insert_rowid()";

                long invoiceId = (long)cmd.ExecuteScalar();

                transaction.Commit();

                return $"OK: Invoice created #{invoiceId}";
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
            {
                transaction.Rollback();
                return "ERROR: Doctor already booked at this time";
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return $"ERROR: {e.Message}";
            }
        }



        /*
        ======================================================
        ОПЛАТА ЧЕКА
        ======================================================
        */

        public string PayInvoice(int invoiceId)
        {
            try
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();

                var cmd = conn.CreateCommand();

                cmd.CommandText =
                @"UPDATE invoices
                  SET paid = 1,
                      paid_at = $time
                  WHERE id = $id";

                cmd.Parameters.AddWithValue("$time", DateTime.Now);
                cmd.Parameters.AddWithValue("$id", invoiceId);

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return "ERROR: Invoice not found";

                return "OK: Payment successful";
            }
            catch (Exception e)
            {
                return $"ERROR: {e.Message}";
            }
        }



        /*
        ======================================================
        ПОЛУЧЕНИЕ СПИСКА ЗАПИСЕЙ
        ======================================================
        */

        public List<Appointment> GetAppointments()
        {
            var result = new List<Appointment>();

            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT id, patient_id, doctor_id, service_id, datetime
                        FROM appointments";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new Appointment(
                    reader.GetInt32(0),  // id
                    reader.GetInt32(1),  // patient_id
                    reader.GetInt32(2),  // doctor_id
                    reader.GetInt32(3),  // service_id
                    DateTime.Parse(reader.GetString(4))  // datetime
                ));
            }

            return result;
        }
    }
}