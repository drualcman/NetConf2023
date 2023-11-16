using Database.Entities.Attributes;

namespace NetConf2023.Shared;

[Database(Name = "vib")]
public class Customers
{
    [Database(Name = "vibno")]
    public int Id { get; set; }
    [Database(Name = "company")]
    public string CompanyName { get; set; }
}
