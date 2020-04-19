
namespace ImprezGarage.Infrastructure.Model
{
    using System;

    public class Account : IDbEntity
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public bool IsReadonly { get; set; }
    }
}
