
namespace ImprezGarage.Infrastructure.Model
{
    using System;

    public interface IDbEntity
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
    }
}
