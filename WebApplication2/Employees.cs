using System;

namespace WebApplication2.Models
{
    public class Employee
    {
        public int id { get; set; }           // Çalışan ID'si (primary key)
        public string name { get; set; }       // Çalışanın adı
        public string role { get; set; }       // Çalışanın rolü
        public string email { get; set; }      // Çalışanın e-posta adresi
        public string phone { get; set; }      // Çalışanın telefon numarası
        public int hotel_id { get; set; }       // İlişkili otelin ID'si
        public int user_id { get; set; }        // Çalışanın kullanıcı ID'si
        public bool deleted { get; set; }      // Silinmiş mi (soft delete)
        public DateTime created_at { get; set; } // Kayıt oluşturulma tarihi
        public DateTime updated_at { get; set; } // Kayıt güncellenme tarihi
    }
}
