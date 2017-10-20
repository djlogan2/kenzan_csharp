using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KenzanCSharp
{
    /*
    public class JSONEmployee
    {
        public int? id { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        public string lastName { get; set; }
        public System.DateTime? dateOfBirth { get; set; }
        public System.DateTime? dateOfEmployment { get; set; }
        public Status? bStatus { get; set; }

        public JSONEmployee(Employee e)
        {
            this.id = e.id;
            this.username = e.username;
            this.firstName = e.firstName;
            this.middleInitial = e.middleInitial;
            this.lastName = e.lastName;
            this.dateOfBirth = e.dateOfBirth;
            this.dateOfEmployment = e.dateOfEmployment;
            this.bStatus = e.bStatus;
        }
    }
    */
    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {
    }


    public class EmployeeMetaData
    {
        [JsonProperty(Required = Required.Always)]
        public int id { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string username { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string firstName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public string lastName { get; set; }

        [JsonProperty(Required = Required.Always)]
        public System.DateTime dateOfBirth { get; set; }

        [JsonIgnore]
        public string password { get; set; }

        [JsonProperty(Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Status bStatus { get; set; }

        [JsonIgnore]
        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}