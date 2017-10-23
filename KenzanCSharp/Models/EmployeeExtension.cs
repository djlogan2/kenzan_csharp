using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace KenzanCSharp
{

    public class JSONEmployee
    {
//        [Required]
        public int? _id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string firstName { get; set; }
        public string middleInitial { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public System.DateTime? dateOfBirth { get; set; }
        public System.DateTime? dateOfEmployment { get; set; }
        [Required]
        public Status? bStatus { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalProperties { get; set; } = new Dictionary<string, JToken>();

        public bool ContainsExtra { get { return AdditionalProperties.Count != 0; } }

        public JSONEmployee(Employee e)
        {
            if (e == null) return;
            this._id = e.id;
            this.username = e.username;
            this.firstName = e.firstName;
            this.middleInitial = e.middleInitial;
            this.lastName = e.lastName;
            this.dateOfBirth = e.dateOfBirth;
            this.dateOfEmployment = e.dateOfEmployment;
            this.bStatus = e.bStatus;
        }

    }

    public partial class Employee
    {
        public Employee(JSONEmployee e)
        {
            if(e._id != null)
                this.id = e._id.Value;
            this.username = e.username;
            this.firstName = e.firstName;
            this.middleInitial = e.middleInitial;
            this.lastName = e.lastName;
            this.dateOfBirth = e.dateOfBirth.Value;
            this.dateOfEmployment = e.dateOfEmployment;
            this.bStatus = e.bStatus.Value;
        }
    }

    [MetadataType(typeof(EmployeeMetaData))]
    public partial class Employee
    {
    }


    public class EmployeeMetaData
    {
        [JsonIgnore]
        public string password { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Status bStatus { get; set; }

        [JsonIgnore]
        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}