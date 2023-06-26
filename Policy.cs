using System;
using System.ComponentModel.DataAnnotations;

namespace PolizaAuto.Models
{
    public class Policy
    {
        [Key]
        public string Id { get; set; }

        public string PolicyNumber { get; set; }

        public string CustomerName { get; set; }

        public string CustomerIdentificationNumber { get; set; }

        public DateTime CustomerDateOfBirth { get; set; }

        public DateTime PolicyStartDate { get; set; }

        public DateTime PolicyEndDate { get; set; }

        public string CoveredCobertures { get; set; }

        public decimal MaximumCoverageValue { get; set; }

        public string PolicyPlanName { get; set; }

        public string CustomerCity { get; set; }

        public string CustomerAddress { get; set; }

        public string VehicleLicensePlate { get; set; }

        public string VehicleModel { get; set; }

        public bool VehicleInspection { get; set; }
    }
}

