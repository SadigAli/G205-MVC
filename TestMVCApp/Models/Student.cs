namespace TestMVCApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Student(int id,string firtname,string lastname)
        {
            Id = id;
            FirstName = firtname;
            LastName = lastname;
        }

        public override string ToString()  
        {
            return $"{FirstName} {LastName}";
        }
    }
}
