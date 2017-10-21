namespace Core.Domain
{
    public class Customer
    {
        public Customer()
        {

        }
        public Customer(int id, string firstName, string lastName, int version)
        {
            Id = id;
            Firstname = firstName;
            Lastname = lastName;
            Version = version;
        }

        public  int Id { get;  set; }
        public  string Firstname { get;  set; }

        public  string Lastname { get;  set; }

        public  int Version { get;  set; }
    }
}
