using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class DataClass
    {
        private int id;
        private string firstName;
        private string lastName;
        private string position;
        private bool isAlive;

        public DataClass() { }
        public DataClass(int id, string firstName, string lastName, string position, bool isAlive)
        {
            this.id=id;
            this.firstName=firstName;
            this.lastName=lastName;
            this.position=position;
            this.isAlive = isAlive;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        public string Position
        {
            get { return position; }
            set { position = value; }
        }
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        
    }
}
