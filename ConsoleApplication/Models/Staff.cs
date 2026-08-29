using System;

namespace InventoryManagementSystem.Models
{
    public class Staff : Person
    {
        public string Department { get; set; }

        public Staff(int id, string name, string email, string department)
            : base(id, name, email)
        {
            Department = department;
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine($"Role: Staff, Department: {Department}");
        }
    }
}