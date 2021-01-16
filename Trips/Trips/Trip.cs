using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trips
{
    class Trip
    {
        // Custom InvalidDataException with string message as parameter
        public class InvalidDataException : Exception
        {
            public InvalidDataException(string msg) : base(msg) { }
        }

        // Constructors
        public Trip(string destination, string name, string passport, string departureDate, string returnDate)
        {
            Destination = destination;
            Name = name;
            Passport = passport;
            DepartureDate = departureDate;
            ReturnDate = returnDate;
        }
        public Trip(string dataLine)
        {
            string[] data = dataLine.Split(';');
            if (data.Length != 5)
            {
                throw new InvalidDataException("Line has invalid number for fields:\n" + dataLine);
            }
            Destination = data[0];
            Name = data[1];
            Passport = data[2];
            DepartureDate = data[3];
            ReturnDate = data[4];
        }

        // Getters and Setters
        private string _destination;
        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                if (value.Contains(";"))
                {
                    throw new InvalidDataException("It contains a invalid character ';'. Please remove it and try it again.");
                }
                _destination = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value.Contains(";"))
                {
                    throw new InvalidDataException("It contains a invalid character ';'. Please remove it and try it again.");
                }
                _name = value;
            }
        }

        private string _passport;
        public string Passport 
        {
            get
            {
                return _passport;
            }
            set
            {
                // Validating with Regex for passport
                string pattern = @"^[A-Za-z 0-9,\-.]{1,20}$";
                Regex rg = new Regex(pattern);
                if (!rg.IsMatch(value))
                {
                    throw new InvalidDataException("Passport must have between 1 to 20 characters and no special characters.");
                }
                _passport = value;
            }
        }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }

        public virtual string ToDataString()
        {
            return string.Format("{0};{1};{2};{3};{4}", Destination, Name, Passport, DepartureDate, ReturnDate);
        }
    }
}
